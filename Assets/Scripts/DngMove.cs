using UnityEngine;
using System.Collections;

        class DungMove extends KeyAdapter {

                public void keyPressed(KeyEvent e) {
                    //freeze game
                    if (!viewing && !eventpanel.isVisible() && e.getKeyCode()==KeyEvent.VK_ESCAPE) {
                        gamefrozen = !gamefrozen;
                        if (gamefrozen) {
                                nomovement = true;
                                showCenter(freezelabelpan);
                                if (numheroes>0) {
                                        for (int i=0;i<numheroes;i++) hero[i].removeMouseListener(hclick);
                                        spellsheet.setVisible(false);
                                        weaponsheet.setVisible(false);
                                }
                                arrowsheet.setVisible(false);
                                //System.gc();
                        }
                        else {
                                nomovement = false;
                                if (sheet) {
                                        showCenter(herosheet);
                                }
                                else {
                                        showCenter(dview);
                                }
                                if (numheroes>0) {
                                        if (!sleeping) { spellsheet.setVisible(true); weaponsheet.setVisible(true); }
                                        for (int i=0;i<numheroes;i++) if (!hero[i].isdead) hero[i].addMouseListener(hclick);
                                }
                                if (!sleeping) arrowsheet.setVisible(true);
                        }
                        return;
                    }
                    if (nomovement) return;
                    //herosheets for dif heroes, F1 thru F4 keys
                    if (!viewing) {
                        if (numheroes>0 && e.getKeyCode()==KeyEvent.VK_F1) {
                                MouseEvent me = new MouseEvent(hero[0],MouseEvent.MOUSE_PRESSED,0,0,20,20,1,false);
                                hero[0].dispatchEvent(me);
                        }
                        else if (numheroes>1 && e.getKeyCode()==KeyEvent.VK_F2) {
                                MouseEvent me = new MouseEvent(hero[1],MouseEvent.MOUSE_PRESSED,0,0,20,20,1,false);
                                hero[1].dispatchEvent(me);
                        }
                        else if (numheroes>2 && e.getKeyCode()==KeyEvent.VK_F3) {
                                MouseEvent me = new MouseEvent(hero[2],MouseEvent.MOUSE_PRESSED,0,0,20,20,1,false);
                                hero[2].dispatchEvent(me);
                        }
                        else if (numheroes>3 && e.getKeyCode()==KeyEvent.VK_F4) {
                                MouseEvent me = new MouseEvent(hero[3],MouseEvent.MOUSE_PRESSED,0,0,20,20,1,false);
                                hero[3].dispatchEvent(me);
                        }
                        else if (numheroes>0 && e.getKeyCode()==KeyEvent.VK_F5) {
                                //save game
                                if (actionqueue.isEmpty()) actionqueue.add("s");
                        }
                        else if (e.getKeyCode()==KeyEvent.VK_F7) {
                                //load game
                                if (actionqueue.isEmpty()) actionqueue.add("l");
                        }
                        else if (e.getKeyCode()==KeyEvent.VK_F9) {
                                //options
                                if (actionqueue.isEmpty()) actionqueue.add("o");
                        }
                        //map
                        else if (AUTOMAP && Character.toLowerCase(e.getKeyChar())=='m') {
                                if (!mappane.isVisible()) {
                                        toppanel.setVisible(false);
                                        centerpanel.setVisible(false);
                                        mappane.setVisible(true);
                                }
                                else {
                                        mappane.setVisible(false);
                                        toppanel.setVisible(true);
                                        centerpanel.setVisible(true);
                                }
                                validate();
                        }
                        //restore characters
                        /*
                        else if (e.getKeyChar()=='*') {
                                for (int i=0;i<numheroes;i++) {
                                        if (!hero[i].isdead) {
                                                if (hero[i].strengthboost<0) {
                                                        hero[i].strength-=hero[i].strengthboost;
                                                        hero[i].strengthboost = 0;
                                                }
                                                hero[i].mana = hero[i].maxmana;
                                                hero[i].heal(hero[i].maxhealth);
                                                hero[i].vitalize(hero[i].maxstamina);
                                                hero[i].weaponcount=0;
                                                hero[i].hitcounter=0;
                                                hero[i].wepready=true;
                                                hero[i].hurthead = false; hero[i].hurttorso = false; hero[i].hurtlegs = false; hero[i].hurtfeet = false; hero[i].hurthand = false; hero[i].hurtweapon = false;
                                        }
                                }
                                hupdate();
                                herosheet.repaint();
                                weaponsheet.repaint();
                        }
                        */
                        else if (Character.toLowerCase(e.getKeyChar())=='f') System.out.println(""+Runtime.getRuntime().freeMemory());
                    }
                    if (!sheet) {
                        //turn left
                        if(e.getKeyChar()=='7' || Character.toLowerCase(e.getKeyChar())==turnleftkey) {
                                //if (walkqueue.size()<4) walkqueue.add(ITURNLEFT);
                                if (walkqueue.size()<4) arrowsheet.doClick(ITURNLEFT);
                        }
                        //turn right
                        else if (e.getKeyChar()=='9' || Character.toLowerCase(e.getKeyChar())==turnrightkey) {
                                //if (walkqueue.size()<4) walkqueue.add(ITURNRIGHT);
                                if (walkqueue.size()<4) arrowsheet.doClick(ITURNRIGHT);
                        }
                        //forward
                        else if (e.getKeyChar()=='8' || Character.toLowerCase(e.getKeyChar())==forwardkey) {
                                //if (walkqueue.size()<4) walkqueue.add(IFORWARD);
                                if (walkqueue.size()<4) arrowsheet.doClick(IFORWARD);
                        }
                        //back
                        else if (e.getKeyChar()=='5' || e.getKeyChar()=='2' || Character.toLowerCase(e.getKeyChar())==backkey) {
                                //if (walkqueue.size()<4) walkqueue.add(IBACK);
                                if (walkqueue.size()<4) arrowsheet.doClick(IBACK);
                        }
                        //left
                        else if (e.getKeyChar()=='4' || Character.toLowerCase(e.getKeyChar())==leftkey) {
                                //if (walkqueue.size()<4) walkqueue.add(ILEFT);
                                if (walkqueue.size()<4) arrowsheet.doClick(ILEFT);
                        }
                        //right
                        else if (e.getKeyChar()=='6' || Character.toLowerCase(e.getKeyChar())==rightkey) {
                                //if (walkqueue.size()<4) walkqueue.add(IRIGHT);
                                if (walkqueue.size()<4) arrowsheet.doClick(IRIGHT);
                        }
                        //else if (e.getKeyChar()=='/') { backindex=(backindex+1)%backimage.length; imagePane.setImage(backimage[backindex]); }
						/*
                        //magicvision - take this out eventually
                        else if (e.getKeyChar()=='-') {
                                magicvision+=20;
                                if (magicvision==20) needredraw=true;
                        }
                        //torch spell - take this out eventually
                        else if (e.getKeyChar()=='+') {
                                magictorch+=20;  if (magictorch>285) magictorch=285;
                                if ((darkfactor+20)>255) darkfactor=255;
                                else darkfactor+=20;
                                needredraw=true;
                        }
                        else if (e.getKeyChar()=='n') {
                                for (int i=0;i<mapheight;i++) {
                                        for (int j=0;j<mapwidth;j++) {
                                                System.out.print(DungeonMap[level][j][i].toString());
                                        }
                                        System.out.println("");
                                }
                        }
                        else if (e.getKeyChar()=='v') {
                                for (int i=0;i<4;i++) {
                                        for (int j=0;j<5;j++) {
                                                System.out.print(visibleg[i][j].toString()+" ");
                                        }
                                        System.out.println("");
                                }
                        }
                        //hammer down a wall - eventually add rocks,boulders to new floor square
                        else if (e.getKeyChar()=='[') {
                                int xadjust=partyx, yadjust=partyy;
                                if (facing==NORTH) yadjust--;
                                else if (facing==WEST) xadjust--;
                                else if (facing==SOUTH) yadjust++;
                                else xadjust++;
                                if (xadjust>=0 && yadjust>=0 && xadjust<mapwidth && yadjust<mapheight) {
                                        if (!DungeonMap[level][xadjust][yadjust].hasMons) {
                                           DungeonMap[level][xadjust][yadjust]=new Floor();
                                           needredraw = true;
                                        }
                                }
                        }
                        //make a wall
                        else if (e.getKeyChar()==']') {
                                int xadjust=partyx, yadjust=partyy;
                                if (facing==NORTH) yadjust--;
                                else if (facing==WEST) xadjust--;
                                else if (facing==SOUTH) yadjust++;
                                else xadjust++;
                                if (xadjust>=0 && yadjust>=0 && xadjust<mapwidth && yadjust<mapheight) {
                                        if (!DungeonMap[level][xadjust][yadjust].hasMons) {
                                           DungeonMap[level][xadjust][yadjust]=new Wall();
                                           needredraw = true;
                                        }
                                }
                        }
						*/
                    }
                }

                public void partyTurn(boolean left) {
                        nomirroradjust = false;
                        if (DungeonMap[level][partyx][partyy] instanceof Stairs) {
                                DungeonMap[level][partyx][partyy].tryTeleport();
                                mirrorback = !mirrorback;
                                walkqueue.clear();
                                return;
                        }
                        mirrorback = !mirrorback;
                        boolean oldmirror = mirrorback;
                        int oldfacing = facing; int oldlevel = level; int oldx = partyx; int oldy = partyy;
                        if (left) facing++;
                        else facing--;
                        if (facing<0) facing=3;
                        else if (facing>3) facing=0;
                        int newfacing = facing;
                        Projectile tempp;
                        for (Iterator i=dmprojs.iterator();i.hasNext();) {
                          tempp = (Projectile)i.next();
                          if (tempp.it!=null && tempp.it.hasthrowpic) {
                              int s = 2; //left
                              if (facing==tempp.direction) { s=0; } //away
                              else if ((facing-tempp.direction)%2==0) { s=1; }//towards - was Math.abs(facing-tem...)%2
                              else if (facing==(tempp.direction+1)%4) { s=3; } //right = dpic
                              tempp.pic = tempp.it.throwpic[s];
                          }
                        }
                        Pillar.swapmirror = !Pillar.swapmirror;
                        needredraw=true;
                        if (AUTOMAP) dmmap.doMap();
                        if (DungeonMap[level][partyx][partyy] instanceof EventSquare && ((EventSquare)DungeonMap[level][partyx][partyy]).eventface-1==facing) {
                                if (facing!=compassface && numheroes>0) {
                                        Compass.updateCompass(facing);
                                        compassface = facing;
                                        if (iteminhand && inhand.number==8) changeCursor();
                                        for (int i=0;i<numheroes;i++) hero[i].repaint();
                                        weaponsheet.repaint();
                                        if (sheet) herosheet.repaint();
                                }
                                //mirrorback = !mirrorback;
                                DungeonMap[level][partyx][partyy].tryTeleport();
                        }
                        else if (DungeonMap[level][partyx][partyy] instanceof FloorSwitch && ((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface>0) {
                                if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==facing) {
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGON);
                                }
                                else if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==oldfacing) {
                                        int tf = facing;
                                        facing = oldfacing;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGOFF);
                                        facing = tf;
                                }
                        }
                        else if (DungeonMap[level][partyx][partyy] instanceof MultFloorSwitch2) ((MultFloorSwitch2)DungeonMap[level][partyx][partyy]).turnTest(oldfacing);
                        if (mirrorback!=oldmirror && !nomirroradjust && (oldx!=partyx || oldy!=partyy || oldlevel!=level || newfacing!=facing)) mirrorback = !mirrorback;
                        nomirroradjust = false;
                }


                /*
                public void turnLeft() {
                        if (DungeonMap[level][partyx][partyy] instanceof Stairs) {
                                DungeonMap[level][partyx][partyy].tryTeleport();
                                mirrorback = !mirrorback;
                                walkqueue.clear();
                                return;
                        }
                        mirrorback = !mirrorback;
                        int oldfacing = facing; int oldlevel = level; int oldx = partyx; int oldy = partyy;
                        facing++;
                        if (facing>3) facing=0;
                        int newfacing = facing;
                        Projectile tempp;
                        for (Iterator i=dmprojs.iterator();i.hasNext();) {
                          tempp = (Projectile)i.next();
                          if (tempp.it!=null && tempp.it.hasthrowpic) {
                              int s = 2; //left
                              if (facing==tempp.direction) { s=0; } //away
                              else if ((facing-tempp.direction)%2==0) { s=1; }//towards - was Math.abs(facing-tem...)%2
                              else if (facing==(tempp.direction+1)%4) { s=3; } //right = dpic
                              tempp.pic = tempp.it.throwpic[s];
                              //if (s<3) tempp.pic = tempp.it.throwpic[s];
                              //else tempp.pic = tempp.it.dpic;
                          }
                        }
                        Pillar.swapmirror = !Pillar.swapmirror;
                        needredraw=true;
                        if (AUTOMAP) dmmap.doMap();
                        if (DungeonMap[level][partyx][partyy] instanceof EventSquare && ((EventSquare)DungeonMap[level][partyx][partyy]).eventface-1==facing) turnTest();
                        else if (DungeonMap[level][partyx][partyy] instanceof FloorSwitch && ((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface>0) {
                                if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==facing) {
                                        DungeonMap[level][partyx][partyy].hasParty=false;
                                        ((FloorSwitch)DungeonMap[level][partyx][partyy]).partyflag=true;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGON);
                                        //DungeonMap[level][partyx][partyy].hasParty=true;
                                }
                                else if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==oldfacing) {
                                        int tf = facing;
                                        DungeonMap[level][partyx][partyy].hasParty=false;
                                        facing = oldfacing;
                                        ((FloorSwitch)DungeonMap[level][partyx][partyy]).partyflag=true;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGOFF);
                                        facing = tf;
                                        //DungeonMap[level][partyx][partyy].hasParty=true;
                                }
                        }
                        else if (DungeonMap[level][partyx][partyy] instanceof MultFloorSwitch2) ((MultFloorSwitch2)DungeonMap[level][partyx][partyy]).turnTest(oldfacing);
                        if (oldx!=partyx || oldy!=partyy || oldlevel!=level || newfacing!=facing) mirrorback = !mirrorback;
                }

                public void turnRight() {
                        if (DungeonMap[level][partyx][partyy] instanceof Stairs) {
                                DungeonMap[level][partyx][partyy].tryTeleport();
                                mirrorback = !mirrorback;
                                walkqueue.clear();
                                return;
                        }
                        mirrorback = !mirrorback;
                        int oldfacing = facing; int oldlevel = level; int oldx = partyx; int oldy = partyy;
                        facing--;
                        if (facing<0) facing=3;
                        int newfacing = facing;
                        Projectile tempp;
                        for (Iterator i=dmprojs.iterator();i.hasNext();) {
                          tempp = (Projectile)i.next();
                          if (tempp.it!=null && tempp.it.hasthrowpic) {
                              int s = 2; //left
                              if (facing==tempp.direction) { s=0; } //away
                              else if ((facing-tempp.direction)%2==0) { s=1; }//towards - was Math.abs(facing-tem...)%2
                              else if (facing==(tempp.direction+1)%4) { s=3; } //right = dpic
                              tempp.pic = tempp.it.throwpic[s];
                              //if (s<3) tempp.pic = tempp.it.throwpic[s];
                              //else tempp.pic = tempp.it.dpic;
                          }
                        }
                        Pillar.swapmirror = !Pillar.swapmirror;
                        needredraw = true;
                        if (AUTOMAP) dmmap.doMap();
                        if (DungeonMap[level][partyx][partyy] instanceof EventSquare && ((EventSquare)DungeonMap[level][partyx][partyy]).eventface-1==facing) turnTest();
                        else if (DungeonMap[level][partyx][partyy] instanceof FloorSwitch && ((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface>0) {
                                if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==facing) {
                                        DungeonMap[level][partyx][partyy].hasParty=false;
                                        ((FloorSwitch)DungeonMap[level][partyx][partyy]).partyflag=true;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGON);
                                        //DungeonMap[level][partyx][partyy].hasParty=true;
                                }
                                else if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==oldfacing) {
                                        int tf = facing;
                                        DungeonMap[level][partyx][partyy].hasParty=false;
                                        facing = oldfacing;
                                        ((FloorSwitch)DungeonMap[level][partyx][partyy]).partyflag=true;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGOFF);
                                        facing = tf;
                                        //DungeonMap[level][partyx][partyy].hasParty=true;
                                }
                        }
                        else if (DungeonMap[level][partyx][partyy] instanceof MultFloorSwitch2) ((MultFloorSwitch2)DungeonMap[level][partyx][partyy]).turnTest(oldfacing);
                        if (oldx!=partyx || oldy!=partyy || oldlevel!=level || newfacing!=facing) mirrorback = !mirrorback;
                }
                */
                
                /*
                private void turnTest() {
                        needredraw = false;
                        needdrawdungeon = true;
                        dview.repaint();
                        if (facing!=compassface && numheroes>0) {
                                Compass.updateCompass(facing);
                                compassface = facing;
                                if (iteminhand && inhand.number==8) changeCursor();
                                for (int i=0;i<numheroes;i++) hero[i].repaint();
                                weaponsheet.repaint();
                                if (sheet) herosheet.repaint();
                        }
                        DungeonMap[level][partyx][partyy].tryTeleport();
                }
                */
                
                public void partyMove(int dir) {
                        nomirroradjust = false;
                        //int c = 0;
                        int xadjust=0, yadjust=0;
                        int newx,newy;
                        if (facing==NORTH) yadjust=1;
                        else if (facing==WEST) xadjust=1;
                        else if (facing==SOUTH) yadjust=-1;
                        else xadjust=-1;

                        switch (dir) {
                                case FORWARD:
                                        newx = partyx-xadjust;
                                        newy = partyy-yadjust;
                                        break;
                                case BACK:
                                        newx = partyx+xadjust;
                                        newy = partyy+yadjust;
                                        break;
                                case LEFT:
                                        newx = partyx-yadjust;
                                        newy = partyy+xadjust;
                                        break;
                                default: //right
                                        newx = partyx+yadjust;
                                        newy = partyy-xadjust;
                        }
                        if (newx<0 || newy<0 || newx>=mapwidth || newy>=mapheight || (dir==BACK && DungeonMap[level][partyx][partyy] instanceof Stairs)) {
                                if (DungeonMap[level][partyx][partyy] instanceof Stairs) {
                                        //playSound("step.wav",-1,-1);
                                        DungeonMap[level][partyx][partyy].tryTeleport();
                                        if (AUTOMAP) dmmap.doMap();

                                }
                                else if (numheroes!=0) playSound("bump.wav",newx,newy);
                                //walkqueue.clear();
                                return;
                        }
                        if (DungeonMap[level][newx][newy].isPassable && !DungeonMap[level][newx][newy].hasMons) {
                                //check if old square had projs, end them if shared a subsquare with a hero or were walked through
                                if (numheroes>0 && DungeonMap[level][partyx][partyy].numProjs>0) {
                                        int nump = DungeonMap[level][partyx][partyy].numProjs;
                                        Projectile tempproj;
                                        int j=0,index=0;
                                        int shouldend = -1;
                                        while (j<nump) {
                                            do {
                                                  tempproj = (Projectile)dmprojs.get(index);
                                                  index++;
                                            }
                                            while (tempproj.level!=level || tempproj.x!=partyx || tempproj.y!=partyy);
                                            if (!tempproj.justthrown && !tempproj.isending) {
                                                int projsub = (tempproj.subsquare+facing)%4;
                                                if (heroatsub[projsub]!=-1) shouldend = heroatsub[projsub];
                                                else {
                                                     switch (dir) {
                                                        case FORWARD:
                                                                if (projsub==0) shouldend = heroatsub[3];
                                                                else if (projsub==1) shouldend = heroatsub[2];
                                                                break;
                                                        case BACK:
                                                                if (projsub==2) shouldend = heroatsub[1];
                                                                else if (projsub==3) shouldend = heroatsub[0];
                                                                break;
                                                        case LEFT:
                                                                if (projsub==0) shouldend = heroatsub[1];
                                                                else if (projsub==3) shouldend = heroatsub[2];
                                                                break;
                                                        default: //right
                                                                if (projsub==1) shouldend = heroatsub[0];
                                                                else if (projsub==2) shouldend = heroatsub[3];
                                                     }
                                                }
                                                if (shouldend!=-1) {
                                                        //proj hits hero[shouldend]
                                                        //System.out.println(hero[shouldend].name+" is hit by a proj.");
                                                        int oldhas = heroatsub[projsub];
                                                        heroatsub[projsub] = shouldend;
                                                        tempproj.projend();
                                                        if (oldhas==-1 || !hero[oldhas].isdead) heroatsub[projsub] = oldhas;
                                                        if (hero[shouldend].isdead) heroatsub[hero[shouldend].subsquare]=-1;
                                                        formation.addNewHero();
                                                        if (tempproj.it!=null) { index--; dmprojs.remove(index); }
                                                        else {
                                                                //move end pic onto new square
                                                                tempproj.x = newx; tempproj.y = newy;
                                                                DungeonMap[level][partyx][partyy].numProjs--;
                                                                DungeonMap[level][newx][newy].numProjs++;
                                                        }
                                                }
                                            }
                                            j++;
                                        }
                                }
                                
                                //playSound("step.wav",-1,-1);
                                
                                int oldy = partyy, oldx = partyx, oldlevel = level, oldface = facing;
                                MapObject newsquare = DungeonMap[level][newx][newy];
                                //remove party from old square
                                DungeonMap[level][partyx][partyy].hasParty=false;
                                //add party to new square
                                partyy=newy; partyx=newx;
                                DungeonMap[level][newx][newy].hasParty=true;
                                boolean oldmirror = mirrorback;
                                mirrorback = !mirrorback;
                                //if have champions, test switch stepping off
                                if (numheroes!=0) {
                                        DungeonMap[oldlevel][oldx][oldy].tryFloorSwitch(MapObject.PARTYSTEPPINGOFF);
                                }
                                //continue if game is not over and didn't get teleported
                                if (!gameover && DungeonMap[level][newx][newy].hasParty) {
                                        //if have champions and newsquare hasn't changed, test switch stepping on
                                        if (numheroes!=0 && newsquare==DungeonMap[level][newx][newy]) DungeonMap[level][newx][newy].tryFloorSwitch(MapObject.PARTYSTEPPINGON);
                                        
                                        //try teleport if game is not over and didn't already get teleported and newsquare didn't change
                                        if (!gameover && DungeonMap[level][newx][newy].hasParty && newsquare==DungeonMap[level][newx][newy]) {
                                                //if (DungeonMap[level][newx][newy].mapchar=='p' && ((Pit)DungeonMap[level][newx][newy]).isOpen) shouldadjust = false;
                                                DungeonMap[level][newx][newy].tryTeleport();
                                        }
                                        
                                        //test to make sure have actually moved (some teleports act as barriers)
                                        if (partyx!=oldx || partyy!=oldy || level!=oldlevel) {
                                           //adjust mirroring if necessary (if teleport messed it up)
                                           if (mirrorback==oldmirror && !nomirroradjust) mirrorback = !mirrorback;
                                           //check if new square has projs that were passed through, end them now if so (only if new square was stepped into, not teleported into)
                                           if (!gameover && numheroes>0 && DungeonMap[level][partyx][partyy].numProjs>0 && partyx==newx && partyy==newy && level==oldlevel) {
                                                int nump = DungeonMap[level][partyx][partyy].numProjs;
                                                Projectile tempproj;
                                                int j=0,index=0;
                                                int shouldend = -1;
                                                while (j<nump) {
                                                    do {
                                                          tempproj = (Projectile)dmprojs.get(index);
                                                          index++;
                                                    }
                                                    while (tempproj.level!=level || tempproj.x!=partyx || tempproj.y!=partyy);
                                                    if (!tempproj.isending) {
                                                        int projsub = (tempproj.subsquare+facing)%4;
                                                        switch (dir) {
                                                                case FORWARD:
                                                                        if (projsub==2) shouldend = heroatsub[1];
                                                                        else if (projsub==3) shouldend = heroatsub[0];
                                                                        break;
                                                                case BACK:
                                                                        if (projsub==0) shouldend = heroatsub[3];
                                                                        else if (projsub==1) shouldend = heroatsub[2];
                                                                        break;
                                                                case LEFT:
                                                                        if (projsub==1) shouldend = heroatsub[0];
                                                                        else if (projsub==2) shouldend = heroatsub[3];
                                                                        break;
                                                                default: //right
                                                                        if (projsub==0) shouldend = heroatsub[1];
                                                                        else if (projsub==3) shouldend = heroatsub[2];
                                                        }
                                                        if (shouldend!=-1) {
                                                                //proj hits hero[shouldend]
                                                                //System.out.println(hero[shouldend].name+" is hit by a proj.");
                                                                int oldhas = heroatsub[projsub];
                                                                heroatsub[projsub] = shouldend;
                                                                tempproj.projend();
                                                                //heroatsub[projsub] = oldhas;
                                                                if (oldhas==-1 || !hero[oldhas].isdead) heroatsub[projsub] = oldhas;
                                                                if (hero[shouldend].isdead) heroatsub[hero[shouldend].subsquare]=-1;
                                                                formation.addNewHero();
                                                                if (tempproj.it!=null) { index--; dmprojs.remove(index); }
                                                        }
                                                    }
                                                    j++;
                                                }
                                           }
                                        }
                                        //didn't move, so if didn't change facing adjust mirroring if necessary
                                        else if (oldface==facing && mirrorback!=oldmirror) mirrorback = !mirrorback;
                                        needredraw=true;
                                        if (!gameover) {
                                                for (int i=0;i<numheroes;i++) {
                                                        hero[i].walkcounter++;
                                                        if (hero[i].load>hero[i].maxload*3/4) hero[i].walkcounter++;
                                                        if (hero[i].load>hero[i].maxload) hero[i].walkcounter++; //if overloaded, stamina drains faster
                                                        //if (hero[i].walkcounter>hero[i].vitality/4) {
                                                        if (hero[i].walkcounter>hero[i].vitality/3) {
                                                                //hero[i].vitalize(-(int)hero[i].load/10-1);
                                                                hero[i].vitalize(-(int)hero[i].load/15-1);
                                                                hero[i].repaint();
                                                                hero[i].walkcounter=0;
                                                        }
                                                }
                                                if (AUTOMAP) dmmap.doMap();
                                        }
                                }
                                else if (!nomirroradjust && mirrorback==oldmirror) mirrorback = !mirrorback;
                                nomirroradjust = false;
                        }
                        else if (!DungeonMap[level][newx][newy].isPassable) {
                                if (numheroes!=0) playSound("bump.wav",newx,newy);
                                if (AUTOMAP && DungeonMap[level][newx][newy].mapchar=='i') dmmap.update(level,newx,newy,'i');
                        }
                }
        }