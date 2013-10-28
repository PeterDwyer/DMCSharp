using UnityEngine;
using System.Collections;

        class Projectile {
                Item it = null;
                Spell sp = null;
                Image pic;
                int pow,dist,direction,level,x,y,subsquare,powcount,powdrain;
                boolean justthrown = true;
                boolean needsfirstdraw = true;
                boolean isending = false;
                boolean notelnext = false;
                boolean hitsImmaterial = false;
                boolean passgrate = false; //determines if proj passes thru a grate

                //for loading:
                public Projectile(Item i,int lvl,int x,int y,int d,int dr,int subs,boolean jt,boolean ntn) {
                        it = i;
                        this.level = lvl;
                        this.x = x;
                        this.y = y;
                        dist = d;
                        direction = dr;
                        subsquare = subs;
                        justthrown = jt;
                        notelnext = ntn;
                        pow = it.throwpow + it.shotpow;
                        if (it.hasthrowpic) {
                                //get relative direction and facing
                                int s = 2; //left
                                if (facing==direction) { s=0; } //away
                                else if ((facing-direction)%2==0) { s=1; }//towards
                                else if (facing==(direction+1)%4) { s=3; } //right = dpic
                                pic = it.throwpic[s];
                                //if (s<3) pic = it.throwpic[s];
                                //else pic = it.dpic;
                        }
                        else pic = it.dpic;
                        if (it.hitsImmaterial) hitsImmaterial = true;
                        //if (it.size<2) passgrate = true;
                        if (it.size==0 || it.size==2) passgrate = true;
                }
                public Projectile(Spell s,int lvl,int x,int y,int d,int dr,int subs,int pd,int pc,boolean jt,boolean ntn) {
                        sp = s;
                        this.level = lvl;
                        this.x = x;
                        this.y = y;
                        dist = d;
                        direction = dr;
                        subsquare = subs;
                        powdrain = pd;
                        powcount = pc;
                        justthrown = jt;
                        notelnext = ntn;
                        pow = sp.power;
                        pic = sp.pic;
                        if (sp.hitsImmaterial) hitsImmaterial = true;
                        if (sp.number==51 || sp.number==52 || sp.number==335 || sp.number==31) passgrate=true;
                }
                
                public Projectile(Item i,int d,int dr,int subs) {
                        this(i,dmnew.level,partyx,partyy,d,dr,subs);
                }
                public Projectile(Item i,int x,int y,int d,int dr,int subs) {
                        this(i,dmnew.level,x,y,d,dr,subs);
                }
                public Projectile(Item i,int lvl,int x,int y,int d,int dr,int subs) {
                        it = i;
                        pow = it.throwpow + it.shotpow;
                        dist = d;
                        direction = dr;
                        subsquare = (subs-facing+4)%4; //if (subsquare<0) subsquare*=-1;
                        //System.out.println("sub+fac = "+subs+", sub = "+subsquare);
                        if (it.hasthrowpic) {
                                //get relative direction and facing
                                int s = 2; //left
                                if (facing==direction) { s=0; } //away
                                else if ((facing-direction)%2==0) { s=1; }//towards
                                else if (facing==(direction+1)%4) { s=3; } //right = dpic
                                pic = it.throwpic[s];
                                //if (s<3) pic = it.throwpic[s];
                                //else pic = it.dpic;
                        }
                        else pic = it.dpic;
                        //pic = it.dpic;
                        this.level = lvl;
                        this.x = x;
                        this.y = y;
                        dmprojs.add(this);
                        DungeonMap[level][x][y].numProjs++;
                        if (it.hitsImmaterial) hitsImmaterial = true;
                        //if (it.size<2) passgrate = true;
                        if (it.size==0 || it.size==2) passgrate = true;
                }
                public Projectile(Spell s,int d,int dr,int subs) {
                        this(s,dmnew.level,partyx,partyy,d,dr,subs);
                }
                public Projectile(Spell s,int x,int y,int d,int dr,int subs) {
                        this(s,dmnew.level,x,y,d,dr,subs);
                }
                public Projectile(Spell s,int lvl,int x,int y,int d,int dr,int subs) {
                        sp = s;
                        pic = sp.pic;
                        pow = sp.power;
                        dist = d;
                        direction = dr;
                        this.level = lvl;
                        this.x = x;
                        this.y = y;
                        powcount = 0;
                        powdrain = dist/sp.gain+1;//tells what value powcount must reach before a power loss
                        subsquare = (subs-facing+4)%4; //if (subsquare<0) subsquare*=-1;
                        //System.out.println("sub+fac = "+subs+", sub = "+subsquare);
                        dmprojs.add(this);
                        DungeonMap[level][x][y].numProjs++;
                        if (sp.hitsImmaterial) hitsImmaterial = true;
                        if (sp.number==51 || sp.number==52 || sp.number==335 || sp.number==31) passgrate=true;
                }

                public void update() {
                        if (dist==99) dist=100; //for projs that don't stop/fade
                        else if (sp!=null && sp.type!=0) powcount++;
                        boolean moveproj = true;//, hitparty = false, hitmons = false;
                        //int newsub, xadjust = x, yadjust = y;
                        int newsub,xadjust,yadjust;
                        if (sp!=null && sp.number==642) {
                                int dir = direction;
                                ArrayList dirchange = new ArrayList(4);
                                for (int i=0;i<4;i++) if (i!=direction) dirchange.add(""+i);
                                do {
                                        xadjust=x; yadjust=y; direction = dir;
                                        if (direction==NORTH || direction==SOUTH) {
                                                if (subsquare==0) newsub = 3;
                                                else if (subsquare==3) newsub = 0;
                                                else if (subsquare==1) newsub = 2;
                                                else newsub = 1;
                                                if (direction==NORTH && (newsub==3 || newsub==2)) yadjust--;
                                                else if (direction==SOUTH && (newsub==0 || newsub==1)) yadjust++;
                                                if (randGen.nextBoolean()) {
                                                        if (dirchange.contains("1") && x>0 && !(DungeonMap[level][x-1][y] instanceof Wall) && DungeonMap[level][x-1][y].mapchar!='2') { dir = 1; dirchange.remove("1"); }
                                                        else if (dirchange.contains("3")) { dir = 3; dirchange.remove("3"); }
                                                        else { dir = 2-direction; dirchange.remove(0); }
                                                }
                                                else {
                                                        if (dirchange.contains("3") && x<mapwidth-1 && !(DungeonMap[level][x+1][y] instanceof Wall) && DungeonMap[level][x+1][y].mapchar!='2') { dir = 3; dirchange.remove("3"); }
                                                        else if (dirchange.contains("1")) { dir = 1; dirchange.remove("1"); }
                                                        else { dir = 2-direction; dirchange.remove(0); }
                                                }
                                        }
                                        else {
                                                if (subsquare==0) newsub = 1;
                                                else if (subsquare==1) newsub = 0;
                                                else if (subsquare==2) newsub = 3;
                                                else newsub = 2;
                                                if (direction==EAST && (newsub==0 || newsub==3)) xadjust++;
                                                else if (direction==WEST && (newsub==1 || newsub==2)) xadjust--;
                                                if (randGen.nextBoolean()) {
                                                        if (dirchange.contains("0") && y>0 && !(DungeonMap[level][x][y-1] instanceof Wall) && DungeonMap[level][x][y-1].mapchar!='2') { dir = 0; dirchange.remove("0"); }
                                                        else if (dirchange.contains("2")) { dir = 2; dirchange.remove("2"); }
                                                        else { dir = (direction+2)%4; dirchange.remove(0); }
                                                }
                                                else {
                                                        if (dirchange.contains("2") && y<mapheight-1 && !(DungeonMap[level][x][y+1] instanceof Wall) && DungeonMap[level][x][y+1].mapchar!='2') { dir = 2; dirchange.remove("2"); }
                                                        else if (dirchange.contains("0")) { dir = 0; dirchange.remove("0"); }
                                                        else { dir = (direction+2)%4; dirchange.remove(0); }
                                                }
                                        }
                                }
                                while ((xadjust<0 && direction==1) || (yadjust<0 && direction==0) || (xadjust==mapwidth && direction==3) || (yadjust==mapheight && direction==2) || ((DungeonMap[level][xadjust][yadjust] instanceof Wall) && DungeonMap[level][xadjust][yadjust].mapchar!='2' && !dirchange.isEmpty()));
                        }
                        else {
                                xadjust=x; yadjust=y;
                                if (direction==NORTH || direction==SOUTH) {
                                        if (subsquare==0) newsub = 3;
                                        else if (subsquare==3) newsub = 0;
                                        else if (subsquare==1) newsub = 2;
                                        else newsub = 1;
                                        if (direction==NORTH && (newsub==3 || newsub==2)) yadjust--;
                                        else if (direction==SOUTH && (newsub==0 || newsub==1)) yadjust++;
                                }
                                else {
                                        if (subsquare==0) newsub = 1;
                                        else if (subsquare==1) newsub = 0;
                                        else if (subsquare==2) newsub = 3;
                                        else newsub = 2;
                                        if (direction==EAST && (newsub==0 || newsub==3)) xadjust++;
                                        else if (direction==WEST && (newsub==1 || newsub==2)) xadjust--;
                                }
                        }
                        
                        if (dist==0 || xadjust<0 || yadjust<0 || xadjust==mapwidth || yadjust==mapheight) {
                           moveproj=false;
                        }
                        else if (!DungeonMap[level][xadjust][yadjust].canPassProjs && DungeonMap[level][xadjust][yadjust].mapchar!='d' && (DungeonMap[level][xadjust][yadjust].mapchar!='>' || ((Stairs)DungeonMap[level][xadjust][yadjust]).side!=direction)) {
                           moveproj=false;
                        }
                        else if (!justthrown && DungeonMap[level][x][y].mapchar=='>') {//DungeonMap[level][x][y] instanceof Stairs) {
                           moveproj = false;
                        }
                        else if (!justthrown && level==dmnew.level && x==partyx && y==partyy && !alldead && heroatsub[(subsquare+facing)%4]!=-1) { 
                           moveproj=false;
                        }
                        else if (!justthrown && DungeonMap[level][x][y].mapchar=='d' && ((sp!=null && sp.number==6) || ((Door)DungeonMap[level][x][y]).changecount>1 && (!passgrate || ((Door)DungeonMap[level][x][y]).pictype!=Door.GRATE))) {
                           moveproj = false;
                        }
                        else if (!justthrown && DungeonMap[level][x][y].hasMons) {
                                Monster tempmon = (Monster)dmmons.get(level+","+x+","+y+","+subsquare);
                                if (tempmon==null) tempmon = (Monster)dmmons.get(level+","+x+","+y+","+5);
                                if (tempmon!=null && !tempmon.isdying && (hitsImmaterial || !tempmon.isImmaterial)) { 
                                        moveproj=false;
                                }
                        }
                        
                        if (moveproj) {
                           DungeonMap[level][x][y].numProjs--;
                           dist--;
                           if (powcount>powdrain) {
                                powcount=0;
                                sp.gain--;
                                sp.power = sp.powers[sp.gain-1];
                           }
                           x = xadjust; y = yadjust; subsquare = newsub;
                           DungeonMap[level][x][y].numProjs++;
                           int xdist,ydist;
                           if (level == dmnew.level) {
                             xdist=partyx-x; if (xdist<0) xdist*=-1;
                             ydist=partyy-y; if (ydist<0) ydist*=-1;
                             if (xdist<5 && ydist<5) needredraw = true;
                           }
                           if (!notelnext) {
                                DungeonMap[level][x][y].tryTeleport(this);
                                if (level == dmnew.level) {
                                     xdist=partyx-x; if (xdist<0) xdist*=-1;
                                     ydist=partyy-y; if (ydist<0) ydist*=-1;
                                     if (xdist<5 && ydist<5) needredraw = true;
                                }
                           }
                           else notelnext = false;
                        }
                        else projend(); 
                        justthrown = false;
                }//end update
                
                public void projend() {
                        if (it!=null) { //item proj, not spell
                           isending = true;
                           boolean shoulddrop = true;
                           //hit party? 
                           if (!justthrown && DungeonMap[level][x][y].hasParty) {
                                int hit = heroatsub[(subsquare+facing)%4];
                                if (hit!=-1) {
                                        playSound("oof.wav",-1,-1);
                                        hero[hit].damage(pow,PROJWEAPONHIT);
                                        if (it.poisonous>0 && randGen.nextBoolean()) { hero[hit].poison+=it.poisonous; hero[hit].ispoisoned=true; }
                                }
                           }
                           //hit mons?
                           else if (!justthrown && DungeonMap[level][x][y].hasMons) {
                                 Monster tempmon = (Monster)dmmons.get(level+","+x+","+y+","+subsquare);
                                 if (tempmon==null) tempmon = (Monster)dmmons.get(level+","+x+","+y+","+5);
                                 if (tempmon!=null) {
                                        if ((hitsImmaterial || !tempmon.isImmaterial) && (tempmon.hurtitem==0 || it.number==215 || it.number==tempmon.hurtitem)) {
                                                tempmon.damage(pow,PROJWEAPONHIT);
                                                if (!tempmon.isImmaterial && it.poisonous>0 && randGen.nextBoolean()) { tempmon.poisonpow+=it.poisonous; tempmon.ispoisoned=true;}
                                                //chance of sticking in mon (not heavily armored ones like stone golems, though)
                                                if (!tempmon.isImmaterial && !tempmon.isdying && tempmon.defense<80 && it.projtype>0 && it.number!=266 && randGen.nextInt(10)==0) { it.shotpow = 0; tempmon.carrying.add(it); shoulddrop = false; }
                                        }
                                 }
                           }
                           
                           if (shoulddrop) {
                                   //item drops to ground
                                   if (level==dmnew.level) {
                                        if (it.weight>4.0) playSound("thunk.wav",x,y);
                                        else if (it.type==Item.WEAPON) playSound("dink.wav",x,y);
                                   }
                                   it.shotpow = 0;
                                   it.subsquare=subsquare;
        
                                   if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                        DungeonMap[level][x][y].addItem(it);
                                        DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                                   }
                           }
                           DungeonMap[level][x][y].numProjs--;
                           if (level == dmnew.level) {
                             int xdist=partyx-x; if (xdist<0) xdist*=-1;
                             int ydist=partyy-y; if (ydist<0) ydist*=-1;
                             if (xdist<5 && ydist<5) needredraw = true;
                           }
                        }
                        else { //spell
                           //System.out.println("proj ending at "+level+","+x+","+y);
                           isending=true;
                           if (level==dmnew.level) {
                                if (sp.number==44 || sp.number==46 || sp.number==335 || sp.number==642 || sp.number==365) playSound("fball.wav",x,y);
                                else playSound("zap.wav",x,y);
                           }
                           
                           //hit party?
                           if (!hitsImmaterial && DungeonMap[level][x][y].hasParty) {
                                if (sp.ismultiple) {
                                   if (sp.number==362) { slowcount+=sp.power*3; message.setMessage("Party Slowed.",4); }
                                   for (int i=0;i<numheroes;i++) {
                                       if (sp.number==362) {
                                          //slow
                                          if (hero[i].dexterityboost>-sp.power) {
                                                  hero[i].dexterity-=hero[i].dexterityboost;
                                                  hero[i].dexterityboost-=sp.power;
                                                  if (hero[i].dexterityboost<-sp.power) hero[i].dexterityboost=-sp.power;
                                                  if (hero[i].dexterity+hero[i].dexterityboost<=0) hero[i].dexterityboost=1-hero[i].dexterity;
                                                  hero[i].dexterity+=hero[i].dexterityboost;
                                          }
                                          hero[i].damage(1,SPELLHIT);
                                       }
                                       else if (sp.number==523) {
                                          //silence
                                          if (hero[i].silencecount<sp.power) {
                                                  int tester = (hero[i].intelligence+hero[i].wisdom)/2;
                                                  if (randGen.nextInt(sp.gain*20)+30>tester || (tester>=100 && randGen.nextInt(5)==0)) {
                                                        if (!hero[i].silenced) message.setMessage(hero[i].name+" is silenced!",i);
                                                        hero[i].silenced = true;
                                                        hero[i].silencecount+=sp.power;
                                                        if (hero[i].silencecount>sp.power) hero[i].silencecount=sp.power;
                                                  }
                                          }
                                          hero[i].damage(1,SPELLHIT);
                                       }
                                       else if (sp.number==44 && hero[i].weapon.number==244 && hero[i].weapon.functions==1) {
                                          //dragon spit -> holder takes no pain from fireball and spit recharges
                                          hero[i].weapon.charges[1]=1;
                                          hero[i].weapon.power[1]=sp.gain;
                                          //if (hero[i].weapon.functions==1) {
                                                hero[i].weapon.functions=2;
                                                weaponsheet.repaint();
                                                hero[i].repaint();
                                          //}
                                          //hero[i].damage(pow/2,SPELLHIT);
                                       }
                                       else if (i==heroatsub[(subsquare+facing)%4]) hero[i].damage(pow,SPELLHIT);
                                       else hero[i].damage(pow*2/3,SPELLHIT);
                                   }
                                }
                                else if (!justthrown && sp.number!=6) {
                                        int hit = heroatsub[(subsquare+facing)%4];
                                        if (hit!=-1) {
                                                if (sp.number==51) { 
                                                        hero[hit].poison+=(sp.gain*2+1);
                                                        hero[hit].ispoisoned=true;
                                                        hero[hit].damage(pow,SPELLHIT);
                                                }
                                                else if (sp.number==461) {
                                                        //weakness
                                                        if (hero[hit].strengthboost>-sp.power) {
                                                                hero[hit].strength-=hero[hit].strengthboost;
                                                                hero[hit].strengthboost-=sp.power;
                                                                if (hero[hit].strengthboost<-sp.power) hero[hit].strengthboost=-sp.power;
                                                                if (hero[hit].strength+hero[hit].strengthboost<=0) hero[hit].strengthboost=1-hero[hit].strength;
                                                                hero[hit].strength+=hero[hit].strengthboost;
                                                                hero[hit].setMaxLoad();
                                                        }
                                                        if (hero[hit].vitalityboost>-sp.power) {
                                                                hero[hit].vitality-=hero[hit].vitalityboost;
                                                                hero[hit].vitalityboost-=sp.power;
                                                                if (hero[hit].vitalityboost<-sp.power) hero[hit].vitalityboost=-sp.power;
                                                                if (hero[hit].vitality+hero[hit].vitalityboost<=0) hero[hit].vitalityboost=1-hero[hit].vitality;
                                                                hero[hit].vitality+=hero[hit].vitalityboost;
                                                        }
                                                        hero[hit].damage(1,SPELLHIT);
                                                }
                                                else if (sp.number==363) {
                                                        //feeble mind
                                                        if (hero[hit].intelligenceboost>-sp.power) {
                                                                hero[hit].intelligence-=hero[hit].intelligenceboost;
                                                                hero[hit].intelligenceboost-=sp.power;
                                                                if (hero[hit].intelligenceboost<-sp.power) hero[hit].intelligenceboost=-sp.power;
                                                                if (hero[hit].intelligence+hero[hit].intelligenceboost<=0) hero[hit].intelligenceboost=1-hero[hit].intelligence;
                                                                hero[hit].intelligence+=hero[hit].intelligenceboost;
                                                        }
                                                        if (hero[hit].wisdomboost>-sp.power) {
                                                                hero[hit].wisdom-=hero[hit].wisdomboost;
                                                                hero[hit].wisdomboost-=sp.power;
                                                                if (hero[hit].wisdomboost<-sp.power) hero[hit].wisdomboost=-sp.power;
                                                                if (hero[hit].wisdom+hero[hit].wisdomboost<=0) hero[hit].wisdomboost=1-hero[hit].wisdom;
                                                                hero[hit].wisdom+=hero[hit].wisdomboost;
                                                        }
                                                        hero[hit].damage(1,SPELLHIT);
                                                }
                                                else if (sp.number==664) {
                                                        //strip defenses
                                                        if (hero[hit].defenseboost>-sp.power) {
                                                                hero[hit].defense-=hero[hit].defenseboost;
                                                                hero[hit].defenseboost-=sp.power;
                                                                if (hero[hit].defenseboost<-sp.power) hero[hit].defenseboost=-sp.power;
                                                                hero[hit].defense+=hero[hit].defenseboost;
                                                        }
                                                        if (hero[hit].magicresistboost>-sp.power) {
                                                                hero[hit].magicresist-=hero[hit].magicresistboost;
                                                                hero[hit].magicresistboost-=sp.power;
                                                                if (hero[hit].magicresistboost<-sp.power) hero[hit].magicresistboost=-sp.power;
                                                                hero[hit].magicresist+=hero[hit].magicresistboost;
                                                        }
                                                        hero[hit].damage(1,SPELLHIT);
                                                }
                                                else hero[hit].damage(pow,SPELLHIT);
                                        }
                                }
                           }
                           //hit mon?
                           else if (DungeonMap[level][x][y].hasMons) { 
                                if (sp.ismultiple) {
                                   Monster tempmon;
                                   for (int i=0;i<6;) {
                                        tempmon = (Monster)dmmons.get(level+","+x+","+y+","+i);
                                        if (tempmon!=null && (hitsImmaterial==tempmon.isImmaterial)) {
                                                if (sp.number==362) {
                                                   //slow
                                                   if (tempmon.speedboost>-sp.power) {
                                                           tempmon.speed-=tempmon.speedboost;
                                                           tempmon.speedboost-=sp.power;
                                                           if (tempmon.speedboost<-sp.power) tempmon.speedboost=-sp.power;
                                                           if (tempmon.speed+tempmon.speedboost<=0) tempmon.speedboost=1-tempmon.speed;
                                                           tempmon.speed+=tempmon.speedboost;
                                                   }
                                                   
                                                   tempmon.movespeed-=tempmon.movespeedboost;
                                                   tempmon.movespeedboost+=sp.gain*3;
                                                   if (tempmon.movespeedboost>sp.gain*3) tempmon.movespeedboost=sp.gain*3;
                                                   tempmon.movespeed+=tempmon.movespeedboost;
                                                   
                                                   /*
                                                   tempmon.attackspeed-=tempmon.attackspeedboost;
                                                   tempmon.attackspeedboost-=sp.gain*2;
                                                   if (tempmon.attackspeedboost<-sp.gain*2) tempmon.attackspeedboost=-sp.gain*2;
                                                   tempmon.attackspeed+=tempmon.attackspeedboost;
                                                   */
                                                }
                                                else if (sp.number==523) {
                                                   //silence
                                                   if (tempmon.number!=26 && tempmon.hasmagic && tempmon.silencecount<sp.power && (randGen.nextInt(sp.gain*20)+30>tempmon.manapower || (tempmon.manapower>=100 && randGen.nextInt(5)==0))) {
                                                         tempmon.silenced = true;
                                                         tempmon.silencecount+=sp.power;
                                                         if (tempmon.silencecount>sp.power) tempmon.silencecount=sp.power;
                                                   }
                                                }
                                                else if (i==subsquare || i==5) tempmon.damage(pow,SPELLHIT);
                                                else tempmon.damage(pow*2/3,SPELLHIT);
                                        }
                                        if (i==3) i=5;
                                        else i++;
                                   }
                                }
                                else if (!justthrown && sp.number!=6) {
                                   Monster tempmon = (Monster)dmmons.get(level+","+x+","+y+","+subsquare);
                                   if (tempmon==null) tempmon = (Monster)dmmons.get(level+","+x+","+y+","+5);
                                   if (tempmon!=null && (hitsImmaterial==tempmon.isImmaterial || (sp.number==0 && tempmon.number==26))) {
                                        //test to see if poison spell
                                        if (sp.number==51 && !tempmon.isImmaterial && tempmon.number!=26) { tempmon.damage(pow,SPELLHIT); tempmon.poisonpow+=(sp.gain*2+1); tempmon.ispoisoned=true; }
                                        else if (sp.number==0 && tempmon.number==26) {
                                                //check for fluxcages, kill if surrounded
                                                //should i be testing party xy? or can chaos escape by teleporting thru party?
                                                boolean surrounded = true;
                                                //put seal test here
                                                //if (DungeonMap[level][tempmon.x][tempmon.y-1].mapchar!='M') surrounded=false;
                                                if (DungeonMap[level][tempmon.x][tempmon.y].mapchar!='F' || ((FDecoration)DungeonMap[level][tempmon.x][tempmon.y]).number!=2) surrounded=false;
                                                else if (fluxcages.get(level+","+tempmon.x+","+tempmon.y)==null) surrounded=false;
                                                /*party does count*/
                                                else if ((!(DungeonMap[level][tempmon.x][tempmon.y-1] instanceof Wall) || DungeonMap[level][tempmon.x][tempmon.y-1].mapchar=='2') && (partyx!=tempmon.x || partyy!=tempmon.y-1) && fluxcages.get(level+","+tempmon.x+","+(tempmon.y-1))==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x][tempmon.y+1] instanceof Wall) || DungeonMap[level][tempmon.x][tempmon.y+1].mapchar=='2') && (partyx!=tempmon.x || partyy!=tempmon.y+1) && fluxcages.get(level+","+tempmon.x+","+(tempmon.y+1))==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x-1][tempmon.y] instanceof Wall) || DungeonMap[level][tempmon.x-1][tempmon.y].mapchar=='2') && (partyx!=tempmon.x-1 || partyy!=tempmon.y) && fluxcages.get(level+","+(tempmon.x-1)+","+tempmon.y)==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x+1][tempmon.y] instanceof Wall) || DungeonMap[level][tempmon.x+1][tempmon.y].mapchar=='2') && (partyx!=tempmon.x+1 || partyy!=tempmon.y) && fluxcages.get(level+","+(tempmon.x+1)+","+tempmon.y)==null) surrounded=false;
                                                /*party doesn't count
                                                else if ((!(DungeonMap[level][tempmon.x][tempmon.y-1] instanceof Wall) || DungeonMap[level][tempmon.x][tempmon.y-1].mapchar=='2') && fluxcages.get(level+","+tempmon.x+","+(tempmon.y-1))==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x][tempmon.y+1] instanceof Wall) || DungeonMap[level][tempmon.x][tempmon.y+1].mapchar=='2') && fluxcages.get(level+","+tempmon.x+","+(tempmon.y+1))==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x-1][tempmon.y] instanceof Wall) || DungeonMap[level][tempmon.x-1][tempmon.y].mapchar=='2') && fluxcages.get(level+","+(tempmon.x-1)+","+tempmon.y)==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x+1][tempmon.y] instanceof Wall) || DungeonMap[level][tempmon.x+1][tempmon.y].mapchar=='2') && fluxcages.get(level+","+(tempmon.x+1)+","+tempmon.y)==null) surrounded=false;
                                                */
                                                if (surrounded) tempmon.damage(tempmon.maxhealth,FUSEHIT);
                                                else tempmon.teleport();
                                        }
                                        else if (sp.number==461) {
                                                //weakness
                                                if (tempmon.powerboost>-sp.power) {
                                                        tempmon.power-=tempmon.powerboost;
                                                        tempmon.powerboost-=sp.power;
                                                        if (tempmon.powerboost<-sp.power) tempmon.powerboost=-sp.power;
                                                        if (tempmon.power+tempmon.powerboost<=0) tempmon.powerboost=1-tempmon.power;
                                                        tempmon.power+=tempmon.powerboost;
                                                }
                                        }
                                        else if (sp.number==363) {
                                                //feeble mind
                                                if (tempmon.manapowerboost>-sp.power) {
                                                        tempmon.manapower-=tempmon.manapowerboost;
                                                        tempmon.manapowerboost-=sp.power;
                                                        if (tempmon.manapowerboost<-sp.power) tempmon.manapowerboost=-sp.power;
                                                        if (tempmon.manapower+tempmon.manapowerboost<=0) tempmon.manapowerboost=1-tempmon.manapower;
                                                        tempmon.manapower+=tempmon.manapowerboost;
                                                }
                                        }
                                        else if (sp.number==664) {
                                                //strip defenses
                                                if (tempmon.defenseboost>-sp.power) {
                                                        tempmon.defense-=tempmon.defenseboost;
                                                        tempmon.defenseboost-=sp.power;
                                                        if (tempmon.defenseboost<-sp.power) tempmon.defenseboost=-sp.power;
                                                        tempmon.defense+=tempmon.defenseboost;
                                                }
                                                if (tempmon.magicresistboost>-sp.power) {
                                                        tempmon.magicresist-=tempmon.magicresistboost;
                                                        tempmon.magicresistboost-=sp.power;
                                                        if (tempmon.magicresistboost<-sp.power) tempmon.magicresistboost=-sp.power;
                                                        tempmon.magicresist+=tempmon.magicresistboost;
                                                }
                                        }
                                        else tempmon.damage(pow,SPELLHIT);
                                   }
                                }
                           }
                           //open door spell?
                           else if ((sp.number==6) && (DungeonMap[level][x][y] instanceof Door)) {
                                Door tempdoor = (Door)DungeonMap[level][x][y];
                                if (tempdoor.opentype==Door.BUTTON) {
                                        if (tempdoor.isclosing) tempdoor.activate();
                                        else tempdoor.deactivate();
                                }
                           }
                           //fireball/ful bomb hitting door?
                           else if ((sp.number==44 || sp.number==46) && (DungeonMap[level][x][y] instanceof Door)) {
                                ((Door)DungeonMap[level][x][y]).breakDoor(pow,false,false);
                           }
                           if (sp.number==31 || sp.number==61) { PoisonCloud cloud = new PoisonCloud(level,x,y,sp.gain); }
                           if (level == dmnew.level) {
                             int xdist=partyx-x; if (xdist<0) xdist*=-1;
                             int ydist=partyy-y; if (ydist<0) ydist*=-1;
                             if (xdist<5 && ydist<5) needredraw = true;
                           }
                        }
                }//end projend
                
        }