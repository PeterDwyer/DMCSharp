using UnityEngine;
using System.Collections;

        class Hero extends JPanel {//JComponent {//Canvas {
                String name;
                String lastname;
                String picname;
                Image pic;
                Item weapon = fistfoot;
                Item head = null;
                Item torso = null;
                Item legs = null;
                Item feet = null;
                Item hand = null;
                Item neck = null;
                Item pouch1 = null;
                Item pouch2 = null;
                Item[] pack = new Item[16];
                Item[] quiver = new Item[6];
                int heronumber;
                int subsquare;
                int number;
                int maxmana;
                int maxhealth;
                int maxstamina;
                int mana;
                int health;
                int stamina;
                int food;
                int water;
                int strength;
                int vitality;
                int dexterity;
                int intelligence;
                int wisdom;
                int defense;
                int magicresist;
                int flevel;
                int nlevel;
                int plevel;
                int wlevel;
                int fxp = 0;
                int nxp = 0;
                int pxp = 0;
                int wxp = 0;
                int strengthboost,intelligenceboost,wisdomboost,dexterityboost,vitalityboost,defenseboost,magicresistboost,flevelboost,nlevelboost,wlevelboost,plevelboost;
                float maxload,load;
                boolean isleader = false;
                boolean isdead = false;
                //boolean splready = true;
                boolean wepready = true;
                boolean ispoisoned = false;
                boolean silenced = false;
                boolean hurthead=false,hurttorso=false,hurtlegs=false,hurtfeet=false,hurthand=false,hurtweapon=false;
                int silencecount = 0;
                int hit;
                int poison = 0;
                int spellcount = 0;
                int weaponcount = 0;
                int timecounter = 0;
                int hurtcounter = 0;
                int hitcounter = 0;
                int poisoncounter = 0;
                int walkcounter = 0; //for stamina drain
                int kuswordcount = 0;
                int rosbowcount = 0;
                int detectcurse = 0;
                int levelboostchange = 3;
                String currentspell = "";
                SpecialAbility[] abilities;
                Item abilitywep = null;
                int abilityactive=-1, abilitypoison, abilitywepnum, falseimage, berserk, berserkstr, berserkhealth;
                boolean abilityimm, abilitystun, abilitydiamond, backstab;

                public Hero(String picname) {
                        //setSize(100,124);
                        setPreferredSize(new Dimension(100,128));//124));
                        setBackground(Color.black);
                        setBorder(BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED,new Color(60,60,80),new Color(20,20,40)));
                        if (picname.indexOf("Heroes")>=0) picname = picname.substring(picname.indexOf("Heroes")+7);
                        //System.out.println(picname);
                        this.picname = picname;
                        //pic = tk.getImage("Heroes"+File.separator+picname);
                        pic = tk.getImage("Heroes"+File.separator+picname);
                        //ImageTracker.addImage(pic,4);
                        //System.out.println(picname);
                }
                
                public Hero(String picn,String n, String ln, int fl, int nl, int wl, int pl, int h, int s, int m, int str, int dex, int vit, int intl, int wis, int def, int magr) {
                        //setSize(100,124);
                        setPreferredSize(new Dimension(100,128));//124));
                        setBackground(Color.black);
                        setBorder(BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED,new Color(60,60,80),new Color(20,20,40)));
                        picname = picn;
                        pic = tk.getImage("Heroes"+File.separator+picname);
                        //System.out.println("big constructor, "+picname);
                        name = n;
                        lastname = ln;
                        flevel = fl;
                        nlevel = nl;
                        wlevel = wl;
                        plevel = pl;
                        maxhealth = h;
                        health = h;
                        maxstamina = s;
                        stamina = s;
                        maxmana = m;
                        mana = m;
                        strength = str;
                        dexterity = dex;
                        vitality = vit;
                        intelligence = intl;
                        wisdom = wis;
                        defense = def;
                        magicresist = magr;
                        setMaxLoad();
                        load = 0.0f;
                        food = 1000;
                        water = 1000;
                }

                //conversion constructor
                public Hero(HeroData h) {
                        setPreferredSize(new Dimension(100,124));
                        setBackground(Color.black);
                        setBorder(BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED,new Color(60,60,80),new Color(20,20,40)));

                        name = new String(h.name);
                        lastname = new String(h.lastname);
                        picname = new String(h.picname);
                        pic = h.pic;
                        //setIcon(new ImageIcon(((ImageIcon)h.getIcon()).getImage()));
                        if (h.weapon!=Item.fistfoot) weapon = Item.createCopy(h.weapon);
                        else weapon = Item.fistfoot;
                        if (h.hand!=null) hand = Item.createCopy(h.hand);
                        if (h.head!=null) head = Item.createCopy(h.head);
                        if (h.neck!=null) neck = Item.createCopy(h.neck);
                        if (h.torso!=null) torso = Item.createCopy(h.torso);
                        if (h.legs!=null) legs = Item.createCopy(h.legs);
                        if (h.feet!=null) feet = Item.createCopy(h.feet);
                        if (h.pouch1!=null) pouch1 = Item.createCopy(h.pouch1);
                        if (h.pouch2!=null) pouch2 = Item.createCopy(h.pouch2);
                        for (int i=0;i<16;i++) {
                                if (h.pack[i]!=null) pack[i] = Item.createCopy(h.pack[i]);
                        }
                        for (int i=0;i<6;i++) {
                                if (h.quiver[i]!=null) quiver[i] = Item.createCopy(h.quiver[i]);
                        }
                        heronumber = h.heronumber;
                        subsquare = h.subsquare;
                        number = h.number;
                        maxmana = h.maxmana;
                        maxhealth = h.maxhealth;
                        maxstamina = h.maxstamina;
                        mana = h.mana;
                        health = h.health;
                        stamina = h.stamina;
                        food = h.food;
                        water = h.water;
                        strength = h.strength;
                        vitality = h.vitality;
                        dexterity = h.dexterity;
                        intelligence = h.intelligence;
                        wisdom = h.wisdom;
                        defense = h.defense;
                        magicresist = h.magicresist;
                        flevel = h.flevel;
                        nlevel = h.nlevel;
                        plevel = h.plevel;
                        wlevel = h.wlevel;
                        flevelboost = h.flevelboost;
                        nlevelboost = h.nlevelboost;
                        plevelboost = h.plevelboost;
                        wlevelboost = h.wlevelboost;
                        fxp = h.fxp;
                        nxp = h.nxp;
                        pxp = h.pxp;
                        wxp = h.wxp;
                        strengthboost = h.strengthboost;
                        intelligenceboost = h.intelligenceboost;
                        wisdomboost = h.wisdomboost;
                        dexterityboost = h.dexterityboost;
                        vitalityboost = h.vitalityboost;
                        defenseboost = h.defenseboost;
                        magicresistboost = h.magicresistboost;
                        maxload = h.maxload;
                        load = h.load;
                        isleader = h.isleader;
                        isdead = h.isdead;
                        wepready = h.wepready;
                        ispoisoned = h.ispoisoned;
                        silenced = h.silenced;
                        hurthead = h.hurthead;
                        hurttorso = h.hurttorso;
                        hurtlegs = h.hurtlegs;
                        hurtfeet = h.hurtfeet;
                        hurthand = h.hurthand;
                        hurtweapon = h.hurtweapon;
                        silencecount = h.silencecount;
                        poison = h.poison;
                        spellcount = h.spellcount;
                        weaponcount = h.weaponcount;
                        timecounter = h.timecounter;
                        poisoncounter = h.poisoncounter;
                        walkcounter = h.walkcounter;
                        kuswordcount = h.kuswordcount;
                        rosbowcount = h.rosbowcount;
                        currentspell = new String(h.currentspell);
                        if (h.abilities!=null) {
                                abilities = new SpecialAbility[h.abilities.length];
                                for (int j=0;j<h.abilities.length;j++) {
                                        abilities[j] = new SpecialAbility(h.abilities[j]);
                                }
                        }
                }

                public void setMaxLoad() {
                        maxload = strength*4/5;
                        if (stamina<maxstamina/5) maxload=maxload*2/3;
                        else if (stamina<maxstamina/3) maxload=maxload*4/5;
                }
                
                public void setLoad() {
                        if (head!=null) load+=head.weight;
                        if (neck!=null) load+=neck.weight;
                        if (torso!=null) load+=torso.weight;
                        if (legs!=null) load+=legs.weight;
                        if (feet!=null) load+=feet.weight;
                        if (hand!=null) load+=hand.weight;
                        if (weapon!=null) load+=weapon.weight;
                        if (pouch1!=null) load+=pouch1.weight;
                        if (pouch2!=null) load+=pouch2.weight;
                        for (int j=0;j<6;j++) if (quiver[j]!=null) load+=quiver[j].weight;
                        for (int i=0;i<16;i++) if (pack[i]!=null) load+=pack[i].weight;
                }
                
                public void setDefense() {
                        if (weapon!=null && (weapon.type==Item.WEAPON || weapon.type==Item.SHIELD)) {
                                //if (weapon.defense>0) defense+=weapon.defense;
                                //if (weapon.magicresist>0) magicresist+=weapon.magicresist;
                                weapon.equipEffect(this);
                        }
                        if (head!=null) {
                                //if (head.defense>0) defense+=head.defense;
                                //if (head.magicresist>0) magicresist+=head.magicresist;
                                head.equipEffect(this);
                        }
                        if (neck!=null) {
                                //if (neck.defense>0) defense+=neck.defense;
                                //if (neck.magicresist>0) magicresist+=neck.magicresist;
                                neck.equipEffect(this);
                        }
                        if (torso!=null) {
                                //if (torso.defense>0) defense+=torso.defense;
                                //if (torso.magicresist>0) magicresist+=torso.magicresist;
                                torso.equipEffect(this);
                        }
                        if (hand!=null) {
                                //if (hand.defense>0) defense+=hand.defense;
                                //if (hand.magicresist>0) magicresist+=hand.magicresist;
                                hand.equipEffect(this);
                        }
                        if (legs!=null) {
                                //if (legs.defense>0) defense+=legs.defense;
                                //if (legs.magicresist>0) magicresist+=legs.magicresist;
                                legs.equipEffect(this);
                        }
                        if (feet!=null) {
                                //if (feet.defense>0) defense+=feet.defense;
                                //if (feet.magicresist>0) magicresist+=feet.magicresist;
                                feet.equipEffect(this);
                        }
                }
                
                public void doCompass() {
                        if (weapon.number==8) Compass.addCompass(weapon);
                        if (hand!=null && hand.number==8) Compass.addCompass(hand);
                        for (int i=0;i<16;i++) {
                                if (pack[i]!=null) {
                                        if (pack[i].number==8) Compass.addCompass(pack[i]);
                                        else if (pack[i].number==5) {
                                                //search chest for compasses
                                                for (int j=0;j<6;j++) {
                                                        Item citem = ((Chest)pack[i]).itemAt(j);
                                                        if (citem!=null && citem.number==8) Compass.addCompass(citem);
                                                }
                                        }
                                }
                        }
                        //compassUpdate();
                }

                public void eatdrink() {
                        if (inhand.type==Item.FOOD) {
                                food+=inhand.foodvalue;
                                if (food>1000) food=1000;
                                else if (food<10) food=10;
                                //item effects here...
                                inhand.foodEffect(this);
                                iteminhand=false;
                                hero[leader].load-=inhand.weight;
                                inhand = null;
                        }
                        else if (inhand.number==72 || (inhand.number==73 && ((Waterskin)inhand).drinks>0)) {
                                water+=inhand.foodvalue;
                                if (water>1000) water=1000;
                                else if (water<10) water=10;
                                if (inhand.number==72) {
                                        inhand = new Item(7);//flask
                                        hero[leader].load-=0.2f;//empty flask is .2 lighter than full
                                }
                                else if (inhand.number==73) {
                                        ((Waterskin)inhand).drinks--;
                                        if (((Waterskin)inhand).drinks==0) ((Waterskin)inhand).swapPics();
                                        inhand.weight-=0.2f;
                                        hero[leader].load-=0.2f;//each drink reduces weight by .2
                                }
                                        
                        }
                        else if (inhand.ispotion) {
                                if (!usePotion(inhand)) return;
                                inhand = new Item(7); //flask
                                hero[leader].load-=0.2f;//empty flask is .2 lighter than full
                        }
                        else return;

                        playSound("gulp.wav",-1,-1);
                }

                public void timePass() {
                        if (!isdead) {
                                int numtimes=1; if (sleeping) numtimes=2;
                                for (int i=0;i<numtimes;i++) {
                                        if (hurtcounter>0) { 
                                                hurtcounter--;
                                                repaint();
                                        }
                                        if (!wepready && weaponcount>=-1) {
                                                weaponcount--;
                                                if (hitcounter>0) {
                                                        hitcounter--;
                                                        if (hitcounter==0) weaponsheet.repaint();
                                                }
                                                if (weaponcount<=0) {
                                                        wepready = true;
                                                        if (hitcounter>0) hitcounter=0;
                                                        weaponsheet.repaint();
                                                }
                                        }
                                        //silence
                                        if (silencecount>0) {
                                                silencecount--;
                                                if (weapon.number==215) silencecount--;
                                                if (silencecount<=0) { silencecount=0; silenced = false; }
                                        }
                                        //magic sword
                                        if (kuswordcount>0) {
                                                kuswordcount--;
                                                if (kuswordcount==0) { weapon = fistfoot; repaint(); weaponsheet.repaint(); }
                                        }
                                        //magic bow
                                        else if (rosbowcount>0) {
                                                rosbowcount--;
                                                if (rosbowcount==0) { weapon = fistfoot; repaint(); weaponsheet.repaint(); }
                                        }
                                        //detect curse
                                        if (detectcurse>0) detectcurse--;
                                }
                                if (ispoisoned) {
                                        poisoncounter++;
                                        poisoncounter+=sleeper;
                                        int poisonval = 30;
                                        if (DIFFICULTY<0) poisonval-=5*DIFFICULTY;
                                        if (poisoncounter>poisonval) {
                                           poisoncounter=0;
                                           if (poison>15) poison=15;
                                           damage(poison,POISONHIT);
                                           repaint();
                                           if (sheet && this.equals(herosheet.hero) && !sleeping && !viewing) herosheet.repaint();
                                        }
                                }
                                timecounter++;
                                timecounter+=sleeper;
                                if (timecounter>250) {
                                        timecounter=0;
                                        //stormbringer
                                        //if (weapon.number==215 || (hand!=null && hand.number==215)) {
                                        if (!sleeping && weapon.number==215) {
                                              //drains stamina more quickly
                                              //stamina-=(maxstamina/20); if (stamina<1) stamina = 1;
                                              vitalize(-maxstamina/20);
                                              //chance to kill companion:
                                              if (!sleeping && numheroes>1 && randGen.nextInt(60)==0) {
                                                   int numkilled = randGen.nextInt(numheroes);
                                                   while (hero[numkilled].equals(this)) numkilled = ++numkilled%numheroes;//numkilled = randGen.nextInt(numheroes);
                                                   //playSound("stormfeed.wav",-1,-1);
                                                   hero[numkilled].damage(hero[numkilled].maxhealth,STORMHIT);
                                                   message.setMessage("Stormbringer feeds...",5);
                                                   maxhealth+=hero[numkilled].maxhealth/10; maxstamina+=hero[numkilled].maxstamina/10; maxmana+=hero[numkilled].maxmana/10;
                                                   strength+=hero[numkilled].strength/10; vitality+=hero[numkilled].vitality/10; intelligence+=hero[numkilled].intelligence/10;
                                                   health = maxhealth; stamina = maxstamina; mana = maxmana; //restores completely when feeds (still sucks though)
                                                   setMaxLoad();
                                                   repaint();
                                              }
                                        }
                                        //test for injuries
                                        boolean hashurt = (hurtweapon || hurthand || hurthead || hurttorso || hurtlegs || hurtfeet);
                                        int fooddec=8;
                                        int waterdec=6;
                                        if (DIFFICULTY<0) { fooddec+=DIFFICULTY*2; waterdec+=DIFFICULTY*2; }
                                        food-=fooddec;
                                        water-=waterdec;
                                        if (!hashurt) heal(vitality/8+1);
                                        else heal(vitality/12+1);
                                        stamcheck();
                                        if (!hashurt) vitalize(vitality/8+1);
                                        else vitalize(vitality/16+1);
                                        if (mana<maxmana) {
                                                if (!hurthead) energize(((intelligence+wisdom)/16)+1);
                                                else energize(((intelligence+wisdom)/24)+1);
                                                if (mana>maxmana) mana=maxmana;
                                        }
                                        else if (mana>maxmana) {
                                                int takeaway = randGen.nextInt(2);
                                                if (wlevel!=15 || plevel!=15) takeaway++;
                                                if (wlevel!=15 && plevel!=15) takeaway+=(mana-maxmana)/10;
                                                if (mana-takeaway<maxmana) takeaway=mana-maxmana;
                                                energize(-takeaway);
                                        }
                                        //if (randGen.nextInt(100)+20 < vitality || randGen.nextInt(10)==0) poison--;
                                        if (ispoisoned) {
                                                int poisontest = 100+(DIFFICULTY*20);
                                                if (randGen.nextInt(poisontest)+20 < vitality || randGen.nextInt(10)==0) poison--;
                                                if (poison<=0) { ispoisoned=false; poison=0; }
                                        }
                                        //stats
                                        if (strengthboost>0) { strengthboost--; strength--; }
                                        else if (strengthboost<0) { strengthboost++; strength++; }
                                        if (dexterityboost>0) { dexterityboost--; dexterity--; }
                                        else if (dexterityboost<0) { dexterityboost++; dexterity++; }
                                        if (vitalityboost>0) { vitalityboost--; vitality--; }
                                        else if (vitalityboost<0) { vitalityboost++; vitality++; }
                                        if (intelligenceboost>0) { intelligenceboost--; intelligence--; }
                                        else if (intelligenceboost<0) { intelligenceboost++; intelligence++; }
                                        if (wisdomboost>0) { wisdomboost--; wisdom--; }
                                        else if (wisdomboost<0) { wisdomboost++; wisdom++; }
                                        if (defenseboost>0) { defenseboost--; defense--; }
                                        else if (defenseboost<0) { defenseboost++; defense++; }
                                        if (magicresistboost>0) { magicresistboost--; magicresist--; }
                                        else if (magicresistboost<0) { magicresistboost++; magicresist++; }
                                        if (berserk>0) {
                                                berserk--;
                                                if (sleeping) berserk=0;
                                                if (berserk==0) {
                                                        if (strength-(berserkstr+1)>0) {
                                                                strength-=(berserkstr+1);
                                                                strengthboost-=(berserkstr+1);
                                                        }
                                                        else {
                                                                strength-=strengthboost;
                                                                strengthboost=1-strength;
                                                                strength=1;
                                                        }
                                                        berserkhealth = 0;
                                                        int stamadj = maxstamina/4;
                                                        if (stamina-stamadj<=0) stamina=1;
                                                        setMaxLoad();
                                                }
                                        }
                                        //levels
                                        if (levelboostchange<=0) {
                                                if (flevelboost>0) { flevelboost--; flevel--; }
                                                else if (flevelboost<0) { flevelboost++; flevel++; }
                                                if (nlevelboost>0) { nlevelboost--; nlevel--; }
                                                else if (nlevelboost<0) { nlevelboost++; nlevel++; }
                                                if (wlevelboost>0) { wlevelboost--; wlevel--; }
                                                else if (wlevelboost<0) { wlevelboost++; wlevel++; }
                                                if (plevelboost>0) { plevelboost--; plevel--; }
                                                else if (plevelboost<0) { plevelboost++; plevel++; }
                                                levelboostchange = 3;
                                                if (weaponready==heronumber) weaponsheet.repaint();
                                        }
                                        else levelboostchange--;
                                        setMaxLoad();
                                        repaint();
                                        if (sheet && this.equals(herosheet.hero) && !sleeping && !viewing) herosheet.repaint();
                                }
                                if (sleeper>0 || timecounter%25==0) stamcheck();
                        }
                }

                public void stamcheck() {
                        if (food<50) {
                                //stamina-=(maxstamina/25);
                                vitalize(-maxstamina/25-1);
                                if (food<10) food=10;
                        }
                        else if (food<100) vitalize(-maxstamina/50-1); //stamina-=(maxstamina/50);
                        if (water<75) {
                                vitalize(-maxstamina/25-1);//stamina-=(maxstamina/25);
                                if (water<10) water=10;
                        }
                        else if (water<100) vitalize(-maxstamina/50-1);//stamina-=(maxstamina/50);
                        if (load>maxload*2) vitalize(-maxstamina/50-1);
                        int p = (int)((float)stamina/(float)maxstamina*100.0f);
                        int stamtest = 10;
                        if (DIFFICULTY<0) stamtest+=DIFFICULTY*5;
                        if (p<stamtest) {
                                int hit = (int)((float)maxhealth/25.0f)+randGen.nextInt(4);
                                damage(hit,POISONHIT);
                                if (sheet && this.equals(herosheet.hero) && !sleeping && !viewing) herosheet.repaint();
                        }
                        repaint();
                }
                

                public void useweapon(int f) {
                        if (weapon.function[f][0].equals("Shoot")) {
                                if (hand==null || hand.projtype!=weapon.projtype || hand.weight>1.0) {
                                        hitcounter = 2;
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        //weaponsheet.hitlabel.setText("Need Ammo");
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Need Ammo";
                                        weaponcount=2;
                                        return;
                                }
                                if (weapon.name.toLowerCase().indexOf("bow")!=-1) playSound("bow.wav",-1,-1);
                                else playSound("swing.wav",-1,-1);
                                //hand.shotpow=weapon.power[0]+strength/10;
                                hand.shotpow = randGen.nextInt()%10+weapon.power[f]*strength/24+weapon.power[f]*nlevel/4;
                                if (hand.shotpow<=0) hand.shotpow=randGen.nextInt(4)+1;
                                int projsub = subsquare;
                                if (projsub==2) projsub=1;
                                else if (projsub==3) projsub=0;
                                wepThrow(hand,projsub);
                                hand = null;
                                repaint();
                                int i=1;
                                boolean found=false;
                                while(i<6 && !found) {
                                        if (quiver[i]!=null && quiver[i].projtype==weapon.projtype) {
                                                hand = quiver[i];
                                                quiver[i]=null;
                                                found = true;
                                        }
                                        i++;
                                }
                                if (!found && quiver[0]!=null) { 
                                        hand=quiver[0]; quiver[0]=null;
                                        if (hand.number==9) {
                                                ((Torch)hand).setPic();
                                                updateDark();
                                        }
                                        if (hand.projtype!=weapon.projtype) {
                                                //swap hands
                                                Item tempitem = weapon;
                                                weapon = hand;
                                                hand = tempitem;
                                                hand.unEquipEffect(this);
                                                weapon.equipEffect(this);
                                        }
                                }
                                weaponcount = weapon.speed[f];
                                if (dexterity<40) {
                                        weaponcount++;
                                        if (dexterity<30) {
                                                weaponcount+=2;
                                                if (dexterity<20) {
                                                        weaponcount+=2;
                                                        if (dexterity<10) weaponcount+=4;
                                                }
                                        }
                                }
                                else if (dexterity>50) { 
                                   weaponcount--;
                                   if (dexterity>70) {
                                      weaponcount--;
                                      if (dexterity>90) weaponcount--;
                                   }
                                }
                                if (hurthand || hurtweapon) weaponcount+=4;
                                if (weaponcount<1) weaponcount=1;
                                if (stamina<maxstamina/5 || load>maxload) weaponcount+=4; //slower if low stamina or overloaded
                                gainxp(weapon.function[f][1].charAt(0),1);
                                //vitalize(-randGen.nextInt(((int)weapon.weight)+1));
                                vitalize(-randGen.nextInt((int)weapon.weight/2+weapon.power[f]/3+2));
                                //stamina-=randGen.nextInt(((int)weapon.weight)+1);
                                //if (stamina<1) stamina=1;
                                wepready = false;
                                backstab = false;
                                repaint();
                                return;
                        }
                        else if (weapon.function[f][0].equals("Magic Shot")) {
                                playSound("bow.wav",-1,-1);
                                Spell arrw = new Spell(weapon.power[0]);
                                arrw.power+=randGen.nextInt((nlevel+1)*5);
                                int projsub = subsquare;
                                if (projsub==2) projsub=1;
                                else if (projsub==3) projsub=0;
                                Projectile p = new Projectile(arrw,50,facing,projsub);
                                weaponcount = weapon.speed[f];
                                if (dexterity<40) {
                                        weaponcount++;
                                        if (dexterity<30) {
                                                weaponcount+=2;
                                                if (dexterity<20) {
                                                        weaponcount+=2;
                                                        if (dexterity<10) weaponcount+=4;
                                                }
                                        }
                                }
                                else if (dexterity>50) { 
                                   weaponcount--;
                                   if (dexterity>70) {
                                      weaponcount--;
                                      if (dexterity>90) weaponcount--;
                                   }
                                }
                                if (hurthand || hurtweapon) weaponcount+=4;
                                if (weaponcount<1) weaponcount=1;
                                if (stamina<maxstamina/5 || load>maxload) weaponcount+=4; //slower if low stamina or overloaded
                                gainxp(weapon.function[f][1].charAt(0),1);
                                //vitalize(-randGen.nextInt(((int)weapon.weight)+1));
                                vitalize(-randGen.nextInt((int)weapon.weight/2+weapon.power[f]/3+2));
                                //stamina-=randGen.nextInt(((int)weapon.weight)+1);
                                //if (stamina<1) stamina=1;
                                wepready = false;
                                backstab = false;
                                repaint();
                                return;
                        }
                        else if (weapon.function[f][0].equals("Drink")) {
                                if (!usePotion(weapon)) return;
                                playSound("gulp.wav",-1,-1);
                                weaponcount = weapon.speed[f];
                                wepready = false;
                                weapon = new Item(7); //flask
                                load-=0.2f;//empty flask is .2 lighter than full
                                repaint();
                                if (sheet && herosheet.hero.heronumber==heronumber) herosheet.repaint();
                                return;
                        }
                        weaponcount = weapon.speed[f];
                        if (dexterity<40) {
                                weaponcount++;
                                if (dexterity<30) {
                                        weaponcount+=2;
                                        if (dexterity<20) {
                                                weaponcount+=2;
                                                if (dexterity<10) weaponcount+=4;
                                        }
                                }
                        }
                        else if (dexterity>50) { 
                           weaponcount--;
                           if (dexterity>70) {
                              weaponcount--;
                              if (dexterity>90) weaponcount--;
                           }
                        }
                        if (hurtweapon) weaponcount+=4;
                        if (weaponcount<1) weaponcount=1;
                        if (stamina<maxstamina/5 || load>maxload) weaponcount+=4; //slower if low stamina or overloaded
                        //if (!weapon.function[f][0].equals("Climb Down") && !weapon.function[f][0].equals("Climb Up")) gainxp(weapon.function[f][1].charAt(0),1);
                        //stamina-=randGen.nextInt(((int)weapon.weight)+1);
                        //vitalize(-randGen.nextInt((int)weapon.weight+1));
                        vitalize(-randGen.nextInt((int)weapon.weight/2+weapon.power[f]/3+2));
                        //stamina = stamina - randGen.nextInt((int)weapon.weight+1)-1;
                        //if (stamina<1) stamina=1;
                        wepready = false;
                        if (weapon.function[f][0].equals("Throw")) {
                                playSound("swing.wav",-1,-1);
                                weapon.shotpow=strength/10+randGen.nextInt(4);
                                int projsub = subsquare;
                                if (projsub==2) projsub=1;
                                else if (projsub==3) projsub=0;
                                gainxp(weapon.function[f][1].charAt(0),1);
                                wepThrow(weapon,projsub);
                                weapon = fistfoot;
                                weapon.unEquipEffect(this);
                                repaint();
                                int i=1;
                                boolean found=false;
                                while(i<6 && !found) {
                                        if (quiver[i]!=null) {
                                                weapon = quiver[i];
                                                quiver[i]=null;
                                                found = true;
                                        }
                                        i++;
                                }
                                if (!found && quiver[0]!=null) { 
                                        weapon=quiver[0]; quiver[0]=null;
                                        if (weapon.number==9) {
                                                ((Torch)weapon).setPic();
                                                updateDark();
                                        }
                                        weapon.equipEffect(this);
                                }
                        }
                        //check if mon in front if wep not using magic
                        //if so,damage mon, gain more xp
                        else if (!checkmagic(f)) { 
                                playSound("swing.wav",-1,-1);
                                if (!checkmon(f)) {
                                        checkdoor(f);
                                }
                                gainxp(weapon.function[f][1].charAt(0),1);
                        }
                        backstab = false;
                        repaint();
                }
                
                public boolean doAbility(String ability, int power, char classgain) {
                        return doAbility(ability,power,classgain,null);
                }
                public boolean doAbility(String ability, int power, char classgain, Object data) {
                        int xadjust=partyx,yadjust=partyy;
                        if (facing==NORTH) yadjust--;
                        else if (facing==WEST) xadjust--;
                        else if (facing==SOUTH) yadjust++;
                        else xadjust++;
                        int projsub = subsquare;
                        if (projsub==2) projsub=1;
                        else if (projsub==3) projsub=0;
                        //System.out.println(ability+", "+power+", "+classgain);
                        if (ability.charAt(0)<'G' || ability.equals("War Cry")) {
                        if (ability.equals("Anti-Ven")) {
                                poison-=power;
                                if (poison<=0) { ispoisoned = false; poison=0; }
                                repaint();
                        }
                        else if (ability.equals("Arc Bolt")) {
                                try { Spell casting = new Spell(power+"642");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }

                        else if (ability.equals("Armor")) {
                                if (defenseboost<power) {
                                        defense-=defenseboost;
                                        defenseboost=power;
                                        defense+=power;
                                }
                                if (magicresistboost<power) {
                                        magicresist-=magicresistboost;
                                        magicresistboost=power;
                                        magicresist+=power;
                                }
                                repaint();
                        }
                        else if (ability.equals("Armor Party")) {
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].defenseboost<power) {
                                        hero[i].defense-=hero[i].defenseboost;
                                        hero[i].defenseboost=power;
                                        hero[i].defense+=power;
                                }
                                if (hero[i].magicresistboost<power) {
                                        hero[i].magicresist-=hero[i].magicresistboost;
                                        hero[i].magicresistboost=power;
                                        hero[i].magicresist+=power;
                                }
                                hero[i].repaint();
                             }
                        }
                        else if (ability.equals("Backstab")) {
                                backstab = true;
                                weaponsheet.toggleSpecials(heronumber);//show weapon commands again
                        }
                        else if (ability.equals("Berserker")) {
                                if (berserk<power) {
                                        if (berserk==0) {
                                                //if ever make charm/confusion spells, put immunity stuff here
                                                strength-=strengthboost;
                                                berserkstr = strength/4+1;
                                                strength+=strengthboost;
                                                berserkhealth = vitality/3+1;
                                                strength+=berserkstr;
                                                strengthboost+=berserkstr;
                                                setMaxLoad();
                                        }
                                        berserk = power;
                                }
                                else return false;
                        }
                        else if (ability.equals("Bolt")) {
                                try { Spell casting = new Spell(power+"335");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Climb Down")) {
                                if (DungeonMap[level][xadjust][yadjust].mapchar=='p' && ((Pit)DungeonMap[level][xadjust][yadjust]).isOpen && !DungeonMap[level][xadjust][yadjust].hasMons && !DungeonMap[level+1][xadjust][yadjust].hasMons && (DungeonMap[level+1][xadjust][yadjust].mapchar!='p' || (DungeonMap[level+1][xadjust][yadjust].mapchar=='p' && !((Pit)DungeonMap[level+1][xadjust][yadjust]).isOpen))) {
                                        climbing = true;
                                        dmove.partyMove(FORWARD);
                                        gainxp(classgain,1);
                                }
                                else {
                                        message.setMessage("Can't Climb Down.",4);
                                        //weaponsheet.hitlabel.setText("Can't Climb");
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Can't Climb";
                                        hitcounter = 2;
                                }
                                return true;
                        }
                        else if (ability.equals("Climb Up")) {
                                if (level>0 && DungeonMap[level-1][partyx][partyy].mapchar=='p' && ((Pit)DungeonMap[level-1][partyx][partyy]).isOpen && DungeonMap[level-1][xadjust][yadjust].isPassable && !DungeonMap[level-1][xadjust][yadjust].hasMons) {
                                        level--;
                                        dmove.partyMove(FORWARD);
                                        for (int i=0;i<numheroes;i++) if (!hero[i].isdead) hero[i].energize((int)-hero[i].load);
                                        gainxp(classgain,1);
                                }
                                else {
                                        message.setMessage("Can't Climb Up.",4);
                                        //weaponsheet.hitlabel.setText("Can't Climb");
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Can't Climb";
                                        hitcounter = 2;
                                }
                                return true;
                        }
                        else if (ability.equals("Conjure")) {
                                Item it = (Item)data;
                                if (weapon==fistfoot || hand==null) {
                                        it = Item.createCopy(it);
                                        if (weapon==fistfoot) {
                                                weapon = it;
                                                if (it.type==Item.WEAPON || it.type==Item.SHIELD || it.number==4) it.equipEffect(this);
                                                weaponsheet.repaint();
                                        }
                                        else {
                                                hand = it;
                                                if (it.type==Item.SHIELD) it.equipEffect(this);
                                        }
                                        load+=it.weight;
                                        //if torch
                                        if (it.number==9) {
                                                ((Torch)it).setPic();
                                                updateDark();
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                }
                                else if (DungeonMap[level][partyx][partyy].canHoldItems) {
                                        it.subsquare = (-facing+4)%4;
                                        DungeonMap[level][partyx][partyy].addItem(Item.createCopy(it));
                                }
                                else {
                                        message.setMessage(name+" has nowhere to put the item.",4);
                                        return false;
                                }
                        }
                        else if (ability.equals("Detect Illusion")) {
                                if (randGen.nextInt(16-nlevel)<5) {
                                        if (DungeonMap[level][xadjust][yadjust].mapchar=='2' || DungeonMap[level][xadjust][yadjust].mapchar=='i' || (DungeonMap[level][xadjust][yadjust].mapchar=='p' && ((Pit)DungeonMap[level][xadjust][yadjust]).isIllusionary)) {
                                                message.setMessage(name+" senses an illusion.",heronumber);
                                                //dispell the illusion here -> change false wall to floor, remove illusion from pit
                                                if (DungeonMap[level][xadjust][yadjust].mapchar=='p') ((Pit)DungeonMap[level][xadjust][yadjust]).isIllusionary=false;
                                                else if (DungeonMap[level][xadjust][yadjust].mapchar=='2') {
                                                        //swap fake wall with floor and transfer any items
                                                        Floor newfloor = new Floor();
                                                        MapObject oldwall = DungeonMap[level][xadjust][yadjust];
                                                        newfloor.numProjs = oldwall.numProjs;
                                                        newfloor.hasMons = oldwall.hasMons;
                                                        newfloor.hasItems = oldwall.hasItems;
                                                        newfloor.hasCloud = oldwall.hasCloud;
                                                        if (newfloor.hasItems) {
                                                                //while (!oldwall.mapItems.isEmpty()) {
                                                                //        newfloor.mapItems.add(oldwall.mapItems.remove(0));
                                                                //}
                                                                newfloor.mapItems = oldwall.mapItems;
                                                        }
                                                        DungeonMap[level][xadjust][yadjust] = newfloor;
                                                }
                                                else {
                                                        //swap invisible wall with visible, reset hasmons
                                                        Wall newwall= new Wall();
                                                        MapObject oldwall = DungeonMap[level][xadjust][yadjust];
                                                        newwall.hasMons = oldwall.hasMons;
                                                        DungeonMap[level][xadjust][yadjust] = newwall;
                                                }
                                                needredraw = true;
                                        }
                                        else message.setMessage(name+" senses there is no illusion.",heronumber);
                                        gainxp(classgain,1);
                                }
                                else message.setMessage(name+" fails to sense anything.",4);
                                return true;
                        }
                        else if (ability.equals("Dispell")) {
                                try { Spell casting = new Spell(power+"52");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Drain Life")) {
                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                int i=3;
                                while (tempmon==null && i>=0) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+((i-facing+4)%4));
                                        i--;
                                }
                                if (tempmon!=null) {
                                        int hit = tempmon.damage(power,DRAINHIT);
                                        if (hit>=0) {
                                                heal(hit);
                                                /*
                                                weaponsheet.hitlabel.setText(""+hit);
                                                if (hit<50) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon3);
                                                else if (hit<100) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon2);
                                                else weaponsheet.hitlabel.setIcon(weaponsheet.hiticon);
                                                */
                                                if (hit<50) weaponsheet.hitpic=weaponsheet.hit3;
                                                else if (hit<100) weaponsheet.hitpic=weaponsheet.hit2;
                                                else weaponsheet.hitpic=weaponsheet.hit1;
                                                weaponsheet.hittext=""+hit;
                                                hitcounter = 2;
                                                message.setMessage(name+" drains "+hit+" life from "+tempmon.name,heronumber);
                                        }
                                        else damage(hit,SPELLHIT);
                                }
                                else return true;
                        }
                        else if (ability.equals("Drain Mana")) {
                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                int i=3;
                                while (tempmon==null && i>=0) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+((i-facing+4)%4));
                                        i--;
                                }
                                if (tempmon!=null && tempmon.mana>0) {
                                        int hit = power;
                                        hit-=(hit*tempmon.magicresist/100);
                                        if (hit<=0) hit = 1;
                                        if (hit>tempmon.mana) hit = tempmon.mana;
                                        energize(hit);
                                        message.setMessage(name+" drains "+hit+" mana from "+tempmon.name,heronumber);
                                }
                        }
                        else if (ability.equals("Enhance Weapon") || ability.equals("Enhance Fist")) {
                                //modify next attack -> hit ghosts, diamond edge style, stun, poison
                                if (ability.endsWith("Fist")) {
                                        if (weapon!=fistfoot) {
                                                message.setMessage(name+" must be barehanded to use that ability.",4);
                                                return false;
                                        }
                                }
                                else if (weapon==fistfoot) {
                                        message.setMessage(name+" must hold a weapon to use that ability.",4);
                                        return false;
                                }
                                abilitywep = weapon;
                                abilityactive = power; if (abilityactive<1) abilityactive = 1;
                                abilityimm = ((int[])data)[0]>0;
                                abilitydiamond = ((int[])data)[1]>0;
                                abilitystun = ((int[])data)[2]>0;
                                abilitypoison = ((int[])data)[3];
                                weaponsheet.toggleSpecials(heronumber);//show weapon commands again
                        }
                        else if (ability.equals("False Image")) {
                                if (falseimage<power) {
                                        falseimage = power;
                                        message.setMessage(name+" projects false images.",heronumber);
                                }
                                else {
                                        message.setMessage(name+" is already projecting false images.",4);
                                        return false;
                                }
                        }
                        else if (ability.equals("Feeble Mind")) {
                                try { Spell casting = new Spell(power+"363");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Fireball")) {
                                try { Spell casting = new Spell(power+"44");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Fluxcage")) {
                                if (!(DungeonMap[level][xadjust][yadjust] instanceof Wall)) {
                                        new FluxCage(level,xadjust,yadjust);
                                        needredraw = true;
                                }
                        }
                        else if (ability.equals("Freeze")) {
                                Monster tempmon;
                                for (int i=0;i<6;i++) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+i);
                                        if (tempmon!=null && tempmon.number!=26) { tempmon.timecounter = -power; }
                                        if (i==3) i++;
                                }
                        }
                        else if (ability.equals("Freeze Life")) {
                                freezelife+=power;
                        }
                        else if (ability.equals("Fuse")) {
                                Projectile p = new Projectile(new Spell(),1,facing,projsub);
                        }
                        else if (ability.equals("War Cry") || ability.equals("Blow Horn") || ability.equals("Frighten") || ability.equals("Calm")) {
                                int adjuster = 0;
                                if (ability.equals("War Cry")) {
                                        //war cry less effective, depends on hero's average level
                                        adjuster = 6;
                                        int avglevel = (flevel+nlevel+plevel+wlevel)/4;
                                        if (avglevel>4 && avglevel<8) adjuster--;
                                        else if (avglevel>7 && avglevel<11) adjuster-=2;
                                        else if (avglevel>10 && avglevel<13) adjuster-=3;
                                        else if (avglevel>12 && avglevel<15) { adjuster-=4; power++; }
                                        else if (avglevel>14) { adjuster=0; power+=2; }
                                        //System.out.println("war cry adjuster = "+adjuster);
                                }
                                Monster tempmon;
                                for (int i=0;i<6;i++) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+i);
                                        if (tempmon!=null && tempmon.currentai!=Monster.FRIENDLYMOVE && tempmon.currentai!=Monster.FRIENDLYNOMOVE && randGen.nextInt(11+adjuster)<tempmon.fearresist) { tempmon.runcounter+=power; tempmon.wasfrightened=true; }
                                        if (i==3) i++;
                                }
                        }
                        }

                        else if (ability.equals("Good Berries")) {
                                //create some magical berries -> food and healing
                                Item berry = new Item(70);
                                if (power>0) {
                                        berry.effect[0] = "health,"+(power+2*plevel);
                                        berry.effect[1] = "stamina,"+(power+2*plevel);
                                }
                                int nummade = 0;
                                int numtomake = randGen.nextInt(plevel/6+1)+1;
                                if (weapon==fistfoot) {
                                        weapon = berry;
                                        load+=berry.weight;
                                        nummade++;
                                }
                                if (nummade<numtomake && hand==null) {
                                        if (nummade>0) hand = Item.createCopy(berry);
                                        else hand = berry;
                                        load+=berry.weight;
                                        nummade++;
                                }
                                if (nummade<numtomake && DungeonMap[level][partyx][partyy].canHoldItems) {
                                        berry.subsquare = (-facing+4)%4;
                                        while (nummade<numtomake) {
                                                DungeonMap[level][partyx][partyy].addItem(Item.createCopy(berry));
                                                nummade++;
                                        }
                                }
                                if (nummade==0) {
                                        message.setMessage(name+" has nowhere to put the berries.",4);
                                        return false;
                                }
                        }
                        else if (ability.equals("Heal")) {
                                heal(maxhealth*power/100+5);
                                repaint();
                        }
                        else if (ability.equals("Invoke")) {
                                String[] spls = { "44","52","335","31","51","44" };
                                int gain = randGen.nextInt(wlevel/2)+wlevel/2+1; if (gain>6) gain=6; //(mana/50+1)%6+1;
                                try { Spell casting = new Spell(gain+spls[randGen.nextInt(6)]);
                                      casting.power+=randGen.nextInt(20)+20;
                                      for (int i=casting.gain-2;i>=0;i--) {
                                          casting.powers[i]+= randGen.nextInt(20)+20;
                                      }
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                      energize(-randGen.nextInt(10)-15);
                                }
                                catch(Exception e) {e.printStackTrace();} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Light")) {
                                darkcounter=0;
                                magictorch+=power; if (magictorch>285) magictorch=285; //can stock up some, but not infinite
                                if ((darkfactor+power)>255) darkfactor=255;
                                else darkfactor+=power;
                                needredraw=true;
                        }
                        else if (ability.equals("Purify")) {
                                Item it = null;
                                if (weapon.type==Item.FOOD && (weapon.poisonous>0 || weapon.foodvalue<0)) it = weapon;
                                else if (hand!=null && hand.type==Item.FOOD && (hand.poisonous>0 || hand.foodvalue<0)) it = hand;
                                if (it!=null) {
                                        it.poisonous = 0;
                                        if (it.foodvalue<0) it.foodvalue = 50;
                                        message.setMessage(name+" purifies the "+it.name+".",heronumber);
                                }
                                else {
                                        message.setMessage(name+" has nothing in hand to purify.",4);
                                        return false;
                                }
                        }
                        else if (ability.equals("Ruiner")) {
                                try { Spell s1 = new Spell(power+"461");//weakness
                                      Projectile p = new Projectile(s1,s1.dist,facing,projsub);
                                      s1 = new Spell(power+"363");//feeble mind
                                      p = new Projectile(s1,s1.dist,facing,projsub);
                                      s1 = new Spell(power+"362");//slow
                                      p = new Projectile(s1,s1.dist,facing,projsub);
                                      s1 = new Spell(power+"664");//strip defenses
                                      p = new Projectile(s1,s1.dist,facing,projsub);
                                      s1 = new Spell(power+"523");//silence
                                      p = new Projectile(s1,s1.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Shield")) {
                                if (defenseboost<power) {
                                        defense-=defenseboost;
                                        defenseboost=power;
                                        defense+=power;
                                        repaint();
                                }
                        }
                        else if (ability.equals("Shield Party")) {
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].defenseboost<power) {
                                        hero[i].defense-=hero[i].defenseboost;
                                        hero[i].defenseboost=power;
                                        hero[i].defense+=power;
                                        hero[i].repaint();
                                }
                             }
                        }
                        else if (ability.equals("Sight")) {
                                magicvision+=power;
                                needredraw = true;
                        }
                        else if (ability.equals("Silence")) {
                                try { Spell casting = new Spell(power+"523");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Slow")) {
                                try { Spell casting = new Spell(power+"362");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Slowfall")) {
                                floatcounter+=power;
                                if (!climbing) message.setMessage("Slowfall active.",4);
                                climbing=true;
                        }
                        else if (ability.equals("SpellShield")) {
                                if (magicresistboost<power) {
                                        magicresist-=magicresistboost;
                                        magicresistboost=power;
                                        magicresist+=power;
                                        repaint();
                                }
                        }
                        else if (ability.equals("SpellShield Party")) {
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].magicresistboost<power) {
                                        hero[i].magicresist-=hero[i].magicresistboost;
                                        hero[i].magicresistboost=power;
                                        hero[i].magicresist+=power;
                                        hero[i].repaint();
                                }
                             }
                        }
                        else if (ability.equals("Stat Boost")) {
                                int[] boost = (int[])data;
                                strength+=boost[0]; strengthboost+=boost[0];
                                dexterity+=boost[1]; dexterityboost+=boost[1];
                                vitality+=boost[2]; vitalityboost+=boost[2];
                                intelligence+=boost[3]; intelligenceboost+=boost[3];
                                wisdom+=boost[4]; wisdomboost+=boost[4];
                                if (boost[5]!=0) {
                                        if (flevel+boost[5]>15) { flevelboost+=15-flevel; flevel=15; }
                                        else if (flevel+boost[5]<0) { flevelboost+=0-flevel; flevel=0; }
                                        else { flevelboost+=boost[5]; flevel+=boost[5]; }
                                }
                                if (boost[6]!=0) {
                                        if (nlevel+boost[6]>15) { nlevelboost+=15-nlevel; nlevel=15; }
                                        else if (nlevel+boost[5]<0) { nlevelboost+=0-nlevel; nlevel=0; }
                                        else { nlevelboost+=boost[6]; nlevel+=boost[6]; }
                                }
                                if (boost[7]!=0) {
                                        if (wlevel+boost[7]>15) { wlevelboost+=15-wlevel; wlevel=15; }
                                        else if (wlevel+boost[5]<0) { wlevelboost+=0-wlevel; wlevel=0; }
                                        else { wlevelboost+=boost[7]; wlevel+=boost[7]; }
                                }
                                if (boost[8]!=0) {
                                        if (plevel+boost[8]>15) { plevelboost+=15-plevel; plevel=15; }
                                        else if (plevel+boost[5]<0) { plevelboost+=0-flevel; plevel=0; }
                                        else { plevelboost+=boost[8]; plevel+=boost[8]; }
                                }
                        }
                        else if (ability.equals("Steal")) {
                                //maybe should have a stealdifficulty integer for mons (like fearresist)
                                //steal an item from mons in front
                                if (weapon!=fistfoot && hand!=null) {
                                        //put check for glove-type weapons, if i make any
                                        message.setMessage(name+" needs a free hand to steal.",4);
                                        return false;
                                }
                                //don't allow steal from back row unless high level
                                if (nlevel<12 && ( (subsquare==2 && heroatsub[1]!=-1) || (subsquare==3 && heroatsub[0]!=-1) )) {
                                        message.setMessage(name+" needs more skill to steal from the back row.",4);
                                        return false;
                                }
                                //find a material monster in front of party to steal from, if any exist
                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                if (tempmon==null) {
                                        boolean[] imtest = new boolean[4];
                                        int subface = randGen.nextInt(2)+2;
                                        int who = (subface-facing+4)%4;
                                        tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                        imtest[who] = (tempmon!=null && tempmon.isImmaterial);
                                        if (tempmon==null || imtest[who]) {
                                                if (subface==2) subface=3;
                                                else subface=2;
                                                who = (subface-facing+4)%4;
                                                tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                imtest[who] = (tempmon!=null && tempmon.isImmaterial);
                                                if (tempmon==null || imtest[who]) {
                                                        subface=randGen.nextInt(2);
                                                        who = (subface-facing+4)%4;
                                                        int whocheck = ((3-subface)-facing+4)%4;
                                                        tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                        imtest[who] = (tempmon!=null && tempmon.isImmaterial);
                                                        if (tempmon==null || imtest[whocheck] || imtest[who]) {
                                                                if (subface==0) subface=1;
                                                                else subface=0;
                                                                who = (subface-facing+4)%4;
                                                                whocheck = ((3-subface)-facing+4)%4;
                                                                tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                                if (imtest[whocheck]) tempmon=null;
                                                                else if (tempmon!=null && tempmon.isImmaterial) tempmon=null;
                                                        }
                                                }
                                        }
                                }
                                else if (tempmon.isImmaterial || tempmon.number==26) tempmon=null;//can't steal from chaos (#26) - remove if/when have a stealdifficulty int
                                if (tempmon!=null) {
                                        //found a mon, try to steal if has anything, power is chance to succeed
                                        if (randGen.nextInt(100)<(power+3*nlevel-tempmon.speed/4)) {
                                                if (tempmon.carrying.size()==0) message.setMessage(tempmon.name+" has nothing to steal.",4);
                                                else {
                                                        Item it = (Item)tempmon.carrying.remove(randGen.nextInt(tempmon.carrying.size()));
                                                        if (weapon==fistfoot) {
                                                                weapon = it;
                                                                if (it.type==Item.WEAPON || it.type==Item.SHIELD || it.number==4) it.equipEffect(this);
                                                                weaponsheet.repaint();
                                                        }
                                                        else {
                                                                hand = it;
                                                                if (it.type==Item.SHIELD) it.equipEffect(this);
                                                        }
                                                        load+=it.weight;
                                                        //if torch
                                                        if (it.number==9) {
                                                                ((Torch)it).setPic();
                                                                updateDark();
                                                        }
                                                        repaint();
                                                        if (sheet) herosheet.repaint();
                                                }
                                        }
                                }
                        }
                        else if (ability.equals("Strip Defenses")) {
                                try { Spell casting = new Spell(power+"664");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("True Sight")) {
                                dispell+=power;
                                if (dispell==power) needredraw=true;
                        }
                        else if (ability.equals("Ven Cloud")) {
                                try { Spell casting = new Spell(power+"31");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Venom")) {
                                try { Spell casting = new Spell(power+"51");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Weakness")) {
                                try { Spell casting = new Spell(power+"461");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("ZO")) {
                                try { Spell casting = new Spell(power+"6");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else return false;
                        gainxp(classgain,1);
                        return true;
                }
                
                public boolean checkmagic(int f) {
                        if (weapon.charges[f]==0) return false; //0 charge means not a special magic ability
                        if (!doAbility(weapon.function[f][0],weapon.power[f],weapon.function[f][1].charAt(0))) return false; //do ability, based on function name -> if name not found, is error so just treat as normal attack
                        if (weapon.function[f][0].equals("Blow Horn")) playSound("horn.wav",-1,-1);
                        else if (weapon.function[f][0].equals("War Cry")) playSound("warcry.wav",-1,-1);
                        else if (weapon.function[f][0].startsWith("Drain")) playSound("drain.wav",-1,-1);
                        if (weapon.charges[f]==-1) return true; //-1 means infinite charges
                        weapon.charges[f]--;
                        if (weapon.charges[f]<1) {
                                weapon.functions--;
                                if (f==1 && weapon.functions==2) {
                                        weapon.function[1][0]=weapon.function[2][0];
                                        weapon.function[1][1]=weapon.function[2][1];
                                        weapon.power[1]=weapon.power[2];
                                        weapon.charges[1]=weapon.charges[2];
                                }
                                else if (f==0) {
                                        if (weapon.functions>0) {
                                            weapon.function[0][0]=weapon.function[1][0];
                                            weapon.function[0][1]=weapon.function[1][1];
                                            weapon.power[0]=weapon.power[1];
                                            weapon.charges[0]=weapon.charges[1];
                                        }
                                        else { load-=weapon.weight; weapon=fistfoot; repaint(); return true;} //used up, disappears
                                }
                                if (weapon.upic!=null) {
                                        boolean hascharge = false;
                                        for (int i=0;i<weapon.functions;i++) {
                                                if (weapon.charges[i]!=0) hascharge=true;
                                        }
                                        if (!hascharge) {
                                                weapon.pic.flush();
                                                if (weapon.temppic!=null) weapon.temppic.flush();
                                                weapon.pic = weapon.upic;
                                                weapon.temppic = weapon.upic;
                                                weapon.picstring = weapon.usedupstring;//"Items"+File.separator+weapon.usedupstring;
                                                if (weapon.epic!=null) {
                                                        weapon.epic.flush();
                                                        weapon.epic = weapon.upic;
                                                        weapon.equipstring = weapon.usedupstring;
                                                }
                                                repaint();
                                        }
                                }
                        }
                        return true;
                }
                
                public boolean checkmon(int f) {
                        
                        int xadjust=partyx,yadjust=partyy;
                        if (facing==NORTH) yadjust--;
                        else if (facing==WEST) xadjust--;
                        else if (facing==SOUTH) yadjust++;
                        else xadjust++;

                        if ( (subsquare==2 && heroatsub[1]!=-1) || (subsquare==3 && heroatsub[0]!=-1) ) {
                                if (!DungeonMap[level][xadjust][yadjust].hasMons) return false;//false so will check for a door and say can't reach if is one
                                message.setMessage(name+" can't reach.",4);
                                //weaponsheet.hitlabel.setText("Can't Reach");
                                //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                weaponsheet.hitpic=weaponsheet.miss;
                                weaponsheet.hittext="Can't Reach";
                                hitcounter = 2;
                                return true;
                        }
                        
                        //ability effects lost if run out of hits or changed weapon
                        if (abilityactive==0 || weapon!=abilitywep) {
                                abilityimm = false;
                                abilitydiamond = false;
                                abilitystun = false;
                                abilitypoison = 0;
                        }
                        boolean oldimm = weapon.hitsImmaterial;
                        if (abilityimm) weapon.hitsImmaterial = true;
                        Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                        if (tempmon==null) {
                                Monster newtemp;
                                boolean[] imtest = new boolean[4];
                                int subface = randGen.nextInt(2)+2;
                                int who = (subface-facing+4)%4;
                                tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                imtest[who] = (tempmon!=null && ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt"))));
                                if (tempmon==null || imtest[who]) {
                                        if (subface==2) subface=3;
                                        else subface=2;
                                        who = (subface-facing+4)%4;
                                        tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                        imtest[who] = (tempmon!=null && ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt"))));
                                        if (tempmon==null || imtest[who]) {
                                                subface=randGen.nextInt(2);
                                                who = (subface-facing+4)%4;
                                                int whocheck = ((3-subface)-facing+4)%4;
                                                tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                imtest[who] = (tempmon!=null && ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt"))));
                                                if (tempmon==null || imtest[whocheck] || imtest[who]) {
                                                        if (subface==0) subface=1;
                                                        else subface=0;
                                                        who = (subface-facing+4)%4;
                                                        whocheck = ((3-subface)-facing+4)%4;
                                                        tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                        if (imtest[whocheck]) tempmon=null;
                                                        else if (tempmon!=null && ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt")))) tempmon=null;
                                                }
                                                //when backstabbing, if chosen mon not facing away try other (only affects 2 furthest subsquares)
                                                else if (backstab && tempmon.facing!=facing) {
                                                        if (subface==0) subface=1;
                                                        else subface=0;
                                                        who = (subface-facing+4)%4;
                                                        whocheck = ((3-subface)-facing+4)%4;
                                                        newtemp = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                        if (imtest[whocheck]) newtemp=null;
                                                        else if (newtemp!=null && ((newtemp.isImmaterial && !weapon.hitsImmaterial) || (!newtemp.isImmaterial && weapon.function[f][0].equals("Disrupt")))) newtemp=null;
                                                        if (newtemp!=null && newtemp.facing==facing) tempmon = newtemp;
                                                }
                                        }
                                }
                                //when backstabbing, if chosen mon not facing away try other (only affects 2 closest subsquares)
                                else if (backstab && tempmon.facing!=facing) {
                                        if (subface==2) subface=3;
                                        else subface=2;
                                        who = (subface-facing+4)%4;
                                        newtemp = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                        imtest[who] = (newtemp!=null && ((newtemp.isImmaterial && !weapon.hitsImmaterial) || (!newtemp.isImmaterial && weapon.function[f][0].equals("Disrupt"))));
                                        if (newtemp!=null && !imtest[who] && newtemp.facing==facing) tempmon = newtemp;
                                }
                        }
                        else if ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt"))) tempmon=null;
                        weapon.hitsImmaterial = oldimm;
                        
                        if (tempmon!=null) {
                                hitcounter=2;
                                if (randGen.nextInt(20+nlevel)==0 && nlevel<15) { 
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        //weaponsheet.hitlabel.setText("Critical Miss");
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Critical Miss";
                                        weaponcount+=4;
                                        message.setMessage(name+": Critical Miss",heronumber);
                                        return true;
                                }
                                
                                int speeddif = dexterity - tempmon.speed;
                                if (stamina<maxstamina/4) speeddif-=5;
                                if (load>maxload) speeddif-=5;
                                if (hurtweapon) speeddif-=10;
                                int backhit = 0;
                                //if (subsquare>1) { speeddif-=10; backhit = 1; }//harder to hit from back row
                                if (tempmon.subsquare!=5 && (tempmon.subsquare+facing)%4<2) { speeddif-=10; backhit++; }//harder to hit back row
                                
                                boolean didhit;
                                if (speeddif>40) didhit = (randGen.nextInt(8)!=0);
                                else if (speeddif>30) didhit = (randGen.nextInt(7)!=0);
                                else if (speeddif>20) didhit = (randGen.nextInt(6)!=0);
                                else if (speeddif>10) didhit = (randGen.nextInt(5)!=0);
                                else if (speeddif>0) didhit = (randGen.nextInt(4)!=0);
                                else if (speeddif>-10) didhit = (randGen.nextInt(3)!=0);
                                else if (speeddif>-20) didhit = (randGen.nextInt(2)!=0);
                                else if (speeddif>-30) didhit = (randGen.nextInt(3)==0);
                                else didhit = (randGen.nextInt(4)==0);
                                //else if (speeddif>-40) didhit = (randGen.nextInt(4)==0);
                                //else didhit = (randGen.nextInt(5)==0);
                                
                                if (didhit) {
                                        int pow = randGen.nextInt()%10+weapon.power[f]*strength/12;
                                        if (backhit>0) {
                                                pow = pow*2/3;
                                                //if (backhit>1) pow = pow*2/3;
                                        }
                                        if (nlevel>1 && weapon.function[f][1].equals("n")) pow+=(randGen.nextInt(nlevel)+1)*3;//was *4
                                        if (hurtweapon) pow = pow/2;
                                        if (pow<1) pow=randGen.nextInt(4);
                                        
                                        //parry makes next mon attack against this hero more likely to miss
                                        if (weapon.function[f][0].equals("Parry")) {
                                                tempmon.parry=heronumber;
                                        }
                                        else if (dexterity>50 && randGen.nextInt(20-2*nlevel/3)==0) { pow=3*pow/2; message.setMessage(name+": Critical Hit",heronumber); } //critical hit
                                        
                                        //playSound("hit.wav",-1,-1);
                                        gainxp(weapon.function[f][1].charAt(0),1);
                                        
                                        //check if hurt by only certain item
                                        if (tempmon.hurtitem!=0 && weapon.number!=tempmon.hurtitem && weapon.number!=215 && (tempmon.hurtitem!=248 || weapon.number!=249)) {
                                                message.setMessage(name+"'s Weapon Has No Effect.",4);
                                                return true;
                                        }
                                        
                                        int diamondcut = 0;
                                        if ((weapon.number==206 || abilitydiamond) && (tempmon.number==3 || tempmon.number==21 || tempmon.number==23 || tempmon.defense>79)) {
                                                diamondcut = tempmon.defense;
                                                tempmon.defense/=2;
                                        }
                                        if (backstab && tempmon.facing==facing) {
                                                if (nlevel<9) pow=3*pow/2;
                                                else pow*=2;
                                                //else if (nlevel<15) pow*=2;
                                                //else pow=5*pow/2;
                                                message.setMessage(name+" performs a backstab attack!",heronumber);
                                        }
                                        if (weapon.number!=215) pow = tempmon.damage(pow,WEAPONHIT);
                                        else pow = tempmon.damage(pow,STORMHIT);
                                        /*
                                        if (pow<50) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon3);
                                        else if (pow<100) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon2);
                                        else weaponsheet.hitlabel.setIcon(weaponsheet.hiticon);
                                        weaponsheet.hitlabel.setText(""+pow);
                                        */
                                        if (pow<50) weaponsheet.hitpic=weaponsheet.hit3;
                                        else if (pow<100) weaponsheet.hitpic=weaponsheet.hit2;
                                        else weaponsheet.hitpic=weaponsheet.hit1;
                                        weaponsheet.hittext=""+pow;
                                        if (diamondcut>0) tempmon.defense=diamondcut;
                                        if (weapon.poisonous>tempmon.poisonpow && randGen.nextBoolean()) { tempmon.ispoisoned=true; tempmon.poisonpow=weapon.poisonous; }//50% chance of inflicting poison
                                        if (abilitypoison>tempmon.poisonpow && randGen.nextInt(5)>0) { tempmon.ispoisoned=true; tempmon.poisonpow=abilitypoison; }//4/5 chance of inflicting poison from an ability
                                        //stun freezes briefly (not chaos)
                                        if ((abilitystun || weapon.function[f][0].equals("Stun")) && tempmon.number!=26 && tempmon.health<2*tempmon.maxhealth/5 && randGen.nextBoolean()) {
                                                tempmon.timecounter = -weapon.power[f]; if (tempmon.timecounter>-10) tempmon.timecounter = -10;
                                                if (nlevel>8 && !tempmon.isdying) message.setMessage(name+"'s blow has stunned the "+tempmon.name+".",heronumber);
                                        }
                                        //stormbringer, small chance to kill a companion
                                        if (weapon.number==215 && pow>0) {
                                              heal(pow); vitalize(pow); energize(pow);
                                              //chance to kill companion:
                                              if (tempmon.isdying && numheroes>1 && randGen.nextInt(50)==0) {
                                                   int numkilled = randGen.nextInt(numheroes);
                                                   while (hero[numkilled].equals(this)) numkilled = ++numkilled%numheroes;//numkilled = randGen.nextInt(numheroes);
                                                   //playSound("stormfeed.wav",-1,-1);
                                                   hero[numkilled].damage(hero[numkilled].maxhealth,STORMHIT);
                                                   message.setMessage("Stormbringer feeds...",5);
                                                   maxhealth+=hero[numkilled].maxhealth/10; maxstamina+=hero[numkilled].maxstamina/10; maxmana+=hero[numkilled].maxmana/10;
                                                   strength+=hero[numkilled].strength/10; vitality+=hero[numkilled].vitality/10; intelligence+=hero[numkilled].intelligence/10;
                                                   health = maxhealth; stamina = maxstamina; mana = maxmana; //restores completely when feeds (still sucks though)
                                                   setMaxLoad();
                                                   repaint();
                                              }
                                        }
                                }
                                else {
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        //weaponsheet.hitlabel.setText("Miss");
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Miss";
                                }
                                //ability enhancement used, so decrement (even if missed)
                                if (abilityactive>=0) abilityactive--;
                                return true;
                        }
                        return false;
                }

                public void checkdoor(int f) {
                        if (weapon.number==9 || (weapon.number>235 && weapon.number<300) || !(weapon.function[f][0].equals("Swing") || weapon.function[f][0].equals("Chop") || weapon.function[f][0].equals("Melee") || weapon.function[f][0].equals("Bash") || weapon.function[f][0].equals("Crush") || weapon.function[f][0].equals("Berzerk"))) return;
                        if ( (subsquare==2 && heroatsub[1]!=-1) || (subsquare==3 && heroatsub[0]!=-1) ) {
                                message.setMessage(name+" can't reach.",4);
                                //weaponsheet.hitlabel.setText("Can't Reach");
                                //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                weaponsheet.hitpic=weaponsheet.miss;
                                weaponsheet.hittext="Can't Reach";
                                hitcounter = 2;
                                return;
                        }
                        int xadjust=0,yadjust=0;
                        if (facing==NORTH) yadjust=1;
                        else if (facing==WEST) xadjust=1;
                        else if (facing==SOUTH) yadjust=-1;
                        else xadjust=-1;
                        
                        //if (DungeonMap[level][partyx-xadjust][partyy-yadjust].mapchar=='d' && ((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).pictype==0 && !((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).isOpen && !((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).isBroken) { // && ((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).changecount==4) {
                        if (DungeonMap[level][partyx-xadjust][partyy-yadjust].mapchar=='d' && !((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).isOpen && !((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).isBroken) {
                                //Door d = (Door)DungeonMap[level][partyx-xadjust][partyy-yadjust];
                                //if (d.isOpen || d.isBroken || !d.isBreakable || d.changecount!=4) return;
                                //int pow = randGen.nextInt()%10+weapon.power[f]*strength/8;
                                
                                int pow = randGen.nextInt()%10+weapon.power[f]*strength/12;
                                ((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).breakDoor(pow,true,true);
                                /*
                                weaponsheet.hitlabel.setText(""+pow);
                                if (pow<50) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon3);
                                else if (pow<100) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon2);
                                else weaponsheet.hitlabel.setIcon(weaponsheet.hiticon);
                                */
                                if (pow<50) weaponsheet.hitpic=weaponsheet.hit3;
                                else if (pow<100) weaponsheet.hitpic=weaponsheet.hit2;
                                else weaponsheet.hitpic=weaponsheet.hit1;
                                weaponsheet.hittext=""+pow;
                                hitcounter = 2;
                        }
                }

                public void gainxp(char classgain, int x) {
                        if (classgain=='-') return;
                        int xptest;
                        switch (classgain) {
                                //were all level*30+50
                                case 'n':
                                        nlevel-=nlevelboost;
                                        if (nlevel<15) {
                                           nxp+=x;
                                           xptest = (int)(Math.pow(1.55,(double)nlevel)) + 35*nlevel + 50;
                                           if (nxp>xptest) levelgain('n');
                                           //if (nxp > nlevel*35+50) levelgain('n');
                                        }
                                        if (nlevel+nlevelboost>15) nlevelboost=15-nlevel;
                                        nlevel+=nlevelboost;
                                        break;
                                case 'w':
                                        wlevel-=wlevelboost;
                                        if (wlevel<15) {
                                           wxp+=x;
                                           xptest = (int)(Math.pow(1.55,(double)wlevel)) + 35*wlevel + 50;
                                           if (wxp>xptest) levelgain('w');
                                           //if (wxp > wlevel*30+50) levelgain('w');
                                        }
                                        if (wlevel+wlevelboost>15) wlevelboost=15-wlevel;
                                        wlevel+=wlevelboost;
                                        break;
                                case 'p':
                                        plevel-=plevelboost;
                                        if (plevel<15) {
                                           pxp+=x;
                                           if (neck!=null && neck.number==92) pxp++;//ekkhard cross effect
                                           xptest = (int)(Math.pow(1.55,(double)plevel)) + 35*plevel + 50;
                                           if (pxp>xptest) levelgain('p');
                                           //if (pxp > plevel*30+50) levelgain('p');
                                        }
                                        if (plevel+plevelboost>15) plevelboost=15-plevel;
                                        plevel+=plevelboost;
                                        break;
                                default:
                                        flevel-=flevelboost;
                                        if (flevel<15) {
                                           fxp+=x;
                                           xptest = (int)(Math.pow(1.55,(double)flevel)) + 35*flevel + 50;
                                           if (fxp>xptest) levelgain('f');
                                           //if (fxp > flevel*35+50) levelgain('f');
                                        }
                                        if (flevel+flevelboost>15) flevelboost=15-flevel;
                                        flevel+=flevelboost;
                                        break;
                        }
                }

                public void levelgain(char classgain) {
                        switch (classgain) {
                                case 'f':
                                        flevel++;
                                        fxp=0;
                                        int statinc=randGen.nextInt(3)+2;
                                        //strength+=statinc;
                                        strengthboost-=statinc;
                                        statinc=randGen.nextInt(2)+1;
                                        //dexterity+=statinc;
                                        dexterityboost-=statinc;
                                        statinc=randGen.nextInt(3)+1;
                                        //vitality+=statinc;
                                        vitalityboost-=statinc;
                                        statinc=randGen.nextInt()%5+vitality/4;
                                        if (statinc<1) statinc = 1;
                                        maxhealth+=statinc;
                                        statinc=randGen.nextInt()%5+vitality/4;
                                        if (statinc<1) statinc = 1;
                                        maxstamina+=statinc;
                                        stamina+=statinc;
                                        setMaxLoad();
                                        message.setMessage(name+" gains a fighter level!",heronumber);
                                        break;
                                case 'n':
                                        nlevel++;
                                        nxp=0;
                                        statinc=randGen.nextInt(3)+1;
                                        //strength+=statinc;
                                        strengthboost-=statinc;
                                        statinc=randGen.nextInt(3)+2;
                                        //dexterity+=statinc;
                                        dexterityboost-=statinc;
                                        statinc=randGen.nextInt(3)+1;
                                        //vitality+=statinc;
                                        vitalityboost-=statinc;
                                        statinc=randGen.nextInt()%5+vitality/5;
                                        if (statinc<1) statinc = 1;
                                        maxhealth+=statinc;
                                        statinc=randGen.nextInt()%5+vitality/5;
                                        if (statinc<1) statinc = 1;
                                        maxstamina+=statinc;
                                        stamina+=statinc;
                                        setMaxLoad();
                                        message.setMessage(name+" gains a ninja level!",heronumber);
                                        break;
                                case 'w':
                                        wlevel++;
                                        wxp=0;
                                        statinc=randGen.nextInt(3)+2;
                                        //intelligence+=statinc;
                                        intelligenceboost-=statinc;
                                        statinc=randGen.nextInt()%5+vitality/7;
                                        if (statinc<1) statinc = 1;
                                        maxhealth+=statinc;
                                        statinc=randGen.nextInt()%5+vitality/7;
                                        if (statinc<1) statinc = 1;
                                        maxstamina+=statinc;
                                        stamina+=statinc;
                                        //statinc=randGen.nextInt()%5+intelligence/4;
                                        statinc = 9-wlevel; if (statinc<4) statinc = 4;
                                        statinc = randGen.nextInt(5)+intelligence/statinc;
                                        if (statinc<1) statinc = 1;
                                        maxmana+=statinc;
                                        message.setMessage(name+" gains a wizard level!",heronumber);
                                        break;
                                case 'p':
                                        plevel++;
                                        pxp=0;
                                        statinc=randGen.nextInt(3)+2;
                                        //wisdom+=statinc;
                                        wisdomboost-=statinc;
                                        statinc=randGen.nextInt()%5+vitality/7;
                                        if (statinc<1) statinc = 1;
                                        maxhealth+=statinc;
                                        statinc=randGen.nextInt()%5+vitality/7;
                                        if (statinc<1) statinc = 1;
                                        maxstamina+=statinc;
                                        stamina+=statinc;
                                        //statinc=randGen.nextInt()%5+wisdom/4;
                                        statinc = 9-plevel; if (statinc<4) statinc = 4;
                                        statinc = randGen.nextInt(5)+wisdom/statinc;
                                        if (statinc<1) statinc = 1;
                                        maxmana+=statinc;
                                        message.setMessage(name+" gains a priest level!",heronumber);
                                        break;
                        }
                        if (sheet && herosheet.hero==this) herosheet.repaint();
                }

                //need pass it a subsquare as well
                public void wepThrow(Item wep, int subsq) {
                        int dist = (strength+5)/10-(int)(wep.weight/2.0f)+randGen.nextInt()%2+wep.shotpow/3+nlevel/2;
                        //if (wep.shotpow>10) dist+=wep.shotpow/10;
                        if (dist<2) dist = 2;
                        if (dist<nlevel) dist = nlevel;
                        if (nlevel>0 && (wep.projtype>0 || (wep.type==Item.WEAPON && wep.hasthrowpic))) wep.shotpow+=randGen.nextInt(nlevel)+nlevel;
                        Projectile p;
                        if (wep.isbomb) {
                                try { 
                                    Spell tempspell = new Spell(wep.bombnum);
                                    tempspell.power = wep.potionpow + randGen.nextInt()%10;
                                    p = new Projectile(tempspell,dist,facing,subsq);
                                }
                                catch(Exception e) { p = new Projectile(wep,dist,facing,subsq); }
                        }
                        else p = new Projectile(wep,dist,facing,subsq);
                        load-=wep.weight;
                }

                public int castSpell() {
                        //return 0 for nonsense, 1 for success, 2 for need flask, 3 for need practice, 4 for can't right now, 5 for silenced (fizzles)

                        Spell casting;
                        try { casting = new Spell(currentspell); }
                        catch(Exception e) { return 0; }
                        
                        if (casting.gain==6 && casting.number==3 && (hand==null || weapon==null || hand.number==weapon.number || (hand.number!=83 && hand.number!=285) || (weapon.number!=285 && weapon.number!=83)) ) return 0;

                        weaponcount=casting.gain*6+2;
                        if (hurthead) weaponcount+=4;
                        int i=0;
                        while (i<numheroes) {
                                if (this.equals(hero[i])) {
                                        wepready = false;
                                        if (weaponready==i) weaponsheet.repaint();
                                        break;
                                }
                                i++;
                        }
                        //fizzle if silenced
                        if (silenced) return 5;
                        //head injury causes 1 in 3 to fizzle, 1 in 5 for archmaster
                        if (hurthead) {
                                int injurytest;
                                if (casting.clsgain=='w') {
                                        if (wlevel<15) injurytest=randGen.nextInt(3);
                                        else injurytest=randGen.nextInt(5);
                                }
                                else if (plevel<15) injurytest=randGen.nextInt(3);
                                else injurytest=randGen.nextInt(5);
                                if (injurytest==0) return 5;
                        }
                        //determine if spell successfully cast:
                        int spelldif = casting.gain*currentspell.length();
                        int attempt;
                        if (casting.clsgain=='w') {
                                attempt = randGen.nextInt((wlevel+1)*2)+(wlevel+1);
                                if (wlevel>8) attempt = 24;
                                else if (wlevel>6) attempt+=4;
                                else if (wlevel>4) attempt++;
                        }
                        else {
                                attempt = randGen.nextInt((plevel+1)*2)+(plevel+1);
                                if (plevel>8) attempt = 24;
                                else if (plevel>6) attempt+=4;
                                else if (plevel>4) attempt++;
                        }
                        if (attempt >= spelldif) {
                                //if (casting.clsgain=='w' && casting.type!=2) {
                                if (casting.clsgain=='w' && casting.type!=2 && casting.number!=461 && casting.number!=363 && casting.number!=362 && casting.number!=664 && casting.number!=523) {
                                        //intelligence modifier
                                        for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*intelligence/8;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(intelligence/8);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                        }
                                        casting.power = casting.powers[casting.gain-1];
                                        //casting.power = randGen.nextInt()%10+casting.gain*intelligence/8+casting.power;
                                        //if (wlevel==15) casting.power+=randGen.nextInt(intelligence/4);
                                        //if (casting.power<1) casting.power=randGen.nextInt(4)+1;
                                }
                                else if (casting.type!=2) {
                                        //wisdom modifier - doesn't affect most potions
                                        if (casting.type!=0 || casting.number==1 || casting.number==2 || casting.number==655) {
                                                casting.power = randGen.nextInt()%10+casting.gain*wisdom/8+casting.power;
                                                if (plevel==15) casting.power+=randGen.nextInt(wisdom/8);
                                                if (casting.power<1) casting.power=randGen.nextInt(4)+1;
                                                if (casting.power<4 && (casting.number==1 || casting.number==2 || casting.number==655)) casting.power=randGen.nextInt(4)+4;
                                                if (casting.number==655) {
                                                        int cost = casting.gain+SYMBOLCOST[5][casting.gain-1]+SYMBOLCOST[10][casting.gain-1]+SYMBOLCOST[16][casting.gain-1];
                                                        //System.out.println("cost = "+cost);
                                                        if (plevel==15 && casting.power>cost*3/4) casting.power=cost*3/4;
                                                        else if (casting.power>cost*2/3) casting.power = cost*2/3;
                                                }
                                        }
                                }
                                //test for binding spells
                                if (casting.gain==6 && casting.number==4 && hand!=null && weapon!=null && ( (hand.number==83 && !hand.bound[0] && weapon.number==282) || (weapon.number==83 && !weapon.bound[0] && hand.number==282))) {
                                        //fire
                                        message.setMessage("Fire has been bound.",4);
                                        if (hand.number==282) {
                                                load-=hand.weight;
                                                hand = null;
                                                weapon.bound[0] = true;
                                                if (weapon.bound[1] && weapon.bound[2] && weapon.bound[3]) {
                                                        //forging complete
                                                        load-=weapon.weight;
                                                        weapon = new Item(248);
                                                        load+=weapon.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        else {
                                                load-=weapon.weight;
                                                weapon = fistfoot;
                                                hand.bound[0] = true;
                                                if (hand.bound[1] && hand.bound[2] && hand.bound[3]) {
                                                        //forging complete
                                                        load-=hand.weight;
                                                        hand = new Item(248);
                                                        load+=hand.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                        return 1;
                                }
                                else if (casting.gain==6 && casting.number==2 && hand!=null && weapon!=null && ( (hand.number==83 && !hand.bound[1] && weapon.number==283) || (weapon.number==83 && !weapon.bound[1] && hand.number==283))) {
                                        message.setMessage("Water has been bound.",4);
                                        if (hand.number==283) {
                                                load-=hand.weight;
                                                hand = null;
                                                weapon.bound[1] = true;
                                                if (weapon.bound[0] && weapon.bound[2] && weapon.bound[3]) {
                                                        //forging complete
                                                        load-=weapon.weight;
                                                        weapon = new Item(248);
                                                        load+=weapon.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        else {
                                                load-=weapon.weight;
                                                weapon = fistfoot;
                                                hand.bound[1] = true;
                                                if (hand.bound[0] && hand.bound[2] && hand.bound[3]) {
                                                        //forging complete
                                                        load-=hand.weight;
                                                        hand = new Item(248);
                                                        load+=hand.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                        return 1;
                                }
                                else if (casting.gain==6 && casting.number==1 && hand!=null && weapon!=null && ( (hand.number==83 && !hand.bound[2] && weapon.number==284) || (weapon.number==83 && !weapon.bound[2] && hand.number==284))) {
                                        //earth
                                        message.setMessage("Earth has been bound.",4);
                                        if (hand.number==284) {
                                                load-=hand.weight;
                                                hand = null;
                                                weapon.bound[2] = true;
                                                if (weapon.bound[0] && weapon.bound[1] && weapon.bound[3]) {
                                                        //forging complete
                                                        load-=weapon.weight;
                                                        weapon = new Item(248);
                                                        load+=weapon.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        else {
                                                load-=weapon.weight;
                                                weapon = fistfoot;
                                                hand.bound[2] = true;
                                                if (hand.bound[0] && hand.bound[1] && hand.bound[3]) {
                                                        //forging complete
                                                        load-=hand.weight;
                                                        hand = new Item(248);
                                                        load+=hand.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                        return 1;
                                }
                                else if (casting.gain==6 && casting.number==3 && hand!=null && weapon!=null && ( (hand.number==83 && !hand.bound[3] && weapon.number==285) || (weapon.number==83 && !weapon.bound[3] && hand.number==285))) {
                                        //wind
                                        message.setMessage("Wind has been bound.",4);
                                        if (hand.number==285) {
                                                load-=hand.weight;
                                                hand = null;
                                                weapon.bound[3] = true;
                                                if (weapon.bound[0] && weapon.bound[1] && weapon.bound[2]) {
                                                        //forging complete
                                                        load-=weapon.weight;
                                                        weapon = new Item(248);
                                                        load+=weapon.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        else {
                                                load-=weapon.weight;
                                                weapon = fistfoot;
                                                hand.bound[3] = true;
                                                if (hand.bound[0] && hand.bound[1] && hand.bound[2]) {
                                                        //forging complete
                                                        load-=hand.weight;
                                                        hand = new Item(248);
                                                        load+=hand.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                        return 1;
                                }
                                else if (casting.number==3) return 0;
                                if (casting.type==0) {
                                        if (hand!=null && hand.number==7) {
                                                load-=hand.weight;
                                                hand=new Item(casting.potionnum, casting.power, casting.gain);
                                                load+=hand.weight;
                                        }
                                        else if (weapon.number==7) {
                                                load-=weapon.weight;
                                                weapon=new Item(casting.potionnum, casting.power, casting.gain);
                                                load+=weapon.weight;
                                        }
                                        else return 2;

                                        hupdate();
                                }
                                else if (casting.type==1) {
                                        int projsub = subsquare;
                                        if (projsub==2) projsub=1;
                                        else if (projsub==3) projsub=0;
                                        Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                else if (casting.type==2) {
                                        int castresult = specialSpell(casting);
                                        //if (castresult==1) gainxp(casting.clsgain,currentspell.length());
                                        if (castresult==1) gainxp(casting.clsgain,casting.gain-1+currentspell.length());
                                        return castresult;
                                        //could put test here for if cast special, 3 part spell
                                        //return nonsense so can't stumble across it by accident
                                        //1st 2 parts inc an int, 2nd cast before first, or 3rd before 2nd or 1st, int is reset
                                        //with 3rd casting, if int==2, effect happens (generates an item or something)
                                }
                                gainxp(casting.clsgain,casting.gain-1+currentspell.length());
                                return 1;
                        }
                        else {
                                gainxp(casting.clsgain,casting.gain);
                                if (casting.clsgain=='w') spellclass="wizard";
                                else spellclass="priest";
                                return 3;
                        }
                }

                public int specialSpell(Spell s) {
                        //magicvision
                        if (s.number==325) { magicvision+=s.power; if (magicvision==s.power) needredraw=true; }
                        //magic torch
                        else if (s.number==4) { 
                             //darkcounter=0;
                             magictorch+=s.power; if (magictorch>285) magictorch=285; //can stock up some, but not infinite
                             if ((darkfactor+s.power)>255) darkfactor=255;
                             else darkfactor+=s.power;
                             needredraw=true;
                        }
                        //perhaps make these not cumulative (ie defenseboot=s.power instead of +=)
                        //shield party
                        else if (s.number==14) {
                             if (plevel==15) s.power+=10;
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].defenseboost<s.power) {
                                        hero[i].defense-=hero[i].defenseboost;
                                        hero[i].defenseboost=s.power;
                                        hero[i].defense+=s.power;
                                        hero[i].repaint();
                                }
                             }
                        }
                        //shield party from magic
                        else if (s.number==64) {
                             if (plevel==15) s.power+=10;
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].magicresistboost<s.power) {
                                        hero[i].magicresist-=hero[i].magicresistboost;
                                        hero[i].magicresistboost=s.power;
                                        hero[i].magicresist+=s.power;
                                        hero[i].repaint();
                                }
                             }
                        }
                        //float
                        else if (s.number==344) { 
                                floatcounter+=s.power; 
                                //if (!climbing) message.setMessage("Float active.",4);
                                if (!climbing) message.setMessage("Slowfall active.",4);
                                climbing=true;
                        }
                        /*
                        //invisibility
                        else if (s.number==326) { 
                                if (invisible<s.power) {
                                        //test if successful here -> should always succeed? fail if in sight of mons? maybe mons in sight aren't affected by it? maybe some mons never affected by it?
                                        //"in sight" would be hard to determine...
                                        //maybe just dodge more easily/often? -> change name to "partial invisibility"
                                        //maybe just keep mons from going after/attacking until duration expires or champion attacks or casts?
                                        invisible+=s.power; 
                                        if (invisible<=s.power) message.setMessage("Invisibility active.",4);
                                }
                                else return 4;
                        }
                        */
                        //magic sword
                        else if (s.number==121) { 
                                if (weapon.number==219 && kuswordcount<s.power) {
                                        kuswordcount=s.power;
                                        weapon.power[0]=s.gain*2+6;
                                        return 1;
                                }
                                else if (weapon!=fistfoot) return 4;
                                weapon = new Item(219);
                                weapon.power[0] = s.gain*2+6;
                                kuswordcount=s.power;
                                repaint();
                        }
                        //magic bow
                        else if (s.number==122) { 
                                if (weapon.number==261 && rosbowcount<s.power) {
                                        rosbowcount=s.power;
                                        weapon.power[0]=s.gain;
                                        return 1;
                                }
                                else if (weapon!=fistfoot) return 4;
                                weapon = new Item(261);
                                weapon.power[0] = s.gain;
                                rosbowcount=s.power;
                                repaint();
                        }
                        //detect curse
                        else if (s.number==364) {
                                if (detectcurse<s.power) {
                                        detectcurse=s.power;
                                }
                        }
                        //dispell illusion - only archmaster can cast, but also an item ability
                        else if (s.number==322) {
                                if (plevel<15) { spellclass="priest"; return 3; }//failure
                                dispell+=s.power;
                                if (dispell==s.power) needredraw=true;
                        }
                        //drain life
                        else if (s.number==666) { 
                                int xadjust=partyx,yadjust=partyy;

                                if (facing==NORTH) yadjust--;
                                else if (facing==WEST) xadjust--;
                                else if (facing==SOUTH) yadjust++;
                                else xadjust++;
                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                int i=3;
                                while (tempmon==null && i>=0) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+((i-facing+4)%4));
                                        i--;
                                }
                                if (tempmon!=null) {
                                        //make drain sound - pic would be hard to do
                                        playSound("drain.wav",-1,-1);
                                        int hit = tempmon.damage(s.power,DRAINHIT);
                                        if (hit>=0) { heal(hit); message.setMessage(name+" drains "+hit+" life from "+tempmon.name,heronumber); }
                                        else damage(hit,SPELLHIT);
                                }
                                else return 4;
                        }
                        //zo kath ra
                        else if (s.number==635) {
                                //create zo kath ra item (which is weightless and has no equip effects)
                                if (weapon.name.equals("Fist/Foot")) { weapon = new Item(80); repaint(); }
                                else if (hand==null) { hand = new Item(80); repaint(); }
                                else if (DungeonMap[level][partyx][partyy].canHoldItems) { DungeonMap[level][partyx][partyy].addItem(new Item(80)); needredraw = true; }
                                else return 4;//fail -> nowhere to put it
                        }
                        return 1;
                }

                public boolean usePotion(Item inhand) {
                        switch (inhand.number) {
                                //stat boosters
                                //antidote
                                //shield(s)
                                case 10:
                                        //health
                                        heal(inhand.potionpow);
                                        int numhealed = randGen.nextInt(inhand.potioncastpow)+inhand.potioncastpow/3;
                                        if (numhealed==0 && inhand.potioncastpow>4) numhealed+=(inhand.potioncastpow-4);
                                        //if can heal, test for injuries
                                        if (numhealed>0 && (hurthead || hurttorso || hurtlegs || hurtfeet || hurthand || hurtweapon)) {
                                                ArrayList intlist = new ArrayList(6);
                                                intlist.add(new Integer(0)); intlist.add(new Integer(1)); intlist.add(new Integer(2));
                                                intlist.add(new Integer(3)); intlist.add(new Integer(4)); intlist.add(new Integer(5));
                                                int counter=0,index=randGen.nextInt(6);
                                                while (numhealed>0 && counter<6) {
                                                        switch ( ((Integer)intlist.remove(index)).intValue() ) {
                                                                case 0:
                                                                        //head
                                                                        if (hurthead) {
                                                                                hurthead=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 1:
                                                                        //torso
                                                                        if (hurttorso) {
                                                                                hurttorso=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 2:
                                                                        //legs
                                                                        if (hurtlegs) {
                                                                                hurtlegs=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 3:
                                                                        //feet
                                                                        if (hurtfeet) {
                                                                                hurtfeet=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 4:
                                                                        //hand
                                                                        if (hurthand) {
                                                                                hurthand=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 5:
                                                                        //weapon
                                                                        if (hurtweapon) {
                                                                                hurtweapon=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                        }
                                                        counter++;
                                                        if (counter<6) index = (index+1)%(6-counter);
                                                }
                                        }
                                        break;
                                case 11:
                                        //stamina
                                        vitalize(inhand.potionpow);
                                        //maxload=strength*4/5;
                                        //if (stamina<maxstamina/5) maxload=maxload*3/4;
                                        break;
                                case 12:
                                        //mana
                                        //energize(inhand.potionpow);
                                        mana+=inhand.potionpow;
                                        break;
                                case 13:
                                        //strength boost
                                        if (strength>150) return false;
                                        strengthboost+=inhand.potionpow;
                                        strength+=inhand.potionpow;
                                        setMaxLoad();
                                        break;
                                case 14:
                                        //dexterity boost
                                        if (dexterity>150) return false;
                                        dexterityboost+=inhand.potionpow;
                                        dexterity+=inhand.potionpow;
                                        break;
                                case 15:
                                        //vitality boost
                                        if (vitality>150) return false;
                                        vitalityboost+=inhand.potionpow;
                                        vitality+=inhand.potionpow;
                                        break;
                                case 16:
                                        //intelligence boost
                                        if (intelligence>150) return false;
                                        intelligenceboost+=inhand.potionpow;
                                        intelligence+=inhand.potionpow;
                                        break;
                                case 17:
                                        //wisdom boost
                                        if (wisdom>150) return false;
                                        wisdomboost+=inhand.potionpow;
                                        wisdom+=inhand.potionpow;
                                        break;
                                case 18:
                                        //defense boost - shield potion
                                        if (defenseboost<inhand.potionpow) {
                                                defense-=defenseboost;
                                                defenseboost=inhand.potionpow;
                                                defense+=inhand.potionpow;
                                        }
                                        break;
                                case 19:
                                        //magic defense boost
                                        if (magicresistboost<inhand.potionpow) {
                                                magicresist-=magicresistboost;
                                                magicresistboost+=inhand.potionpow;
                                                magicresist+=inhand.potionpow;
                                        }
                                        break;
                                case 20:
                                        //anti-ven
                                        poison-=inhand.potionpow;
                                        if (poison<1) {
                                                ispoisoned=false;
                                                poison=0;
                                        }
                                        break;
                                case 24:
                                        //anti-silence
                                        silencecount-=inhand.potionpow;
                                        if (silencecount<1) {
                                                silenced=false;
                                                silencecount=0;

                                        }
                                        break;
                                case 25:
                                        //remove curse
                                        //find a cursed item in inventory, decrease its cursed int
                                        Item toremove = null;
                                        if (weapon.cursed>0) toremove=weapon;
                                        else if (hand!=null && hand.cursed>0) toremove=hand;
                                        else if (head!=null && head.cursed>0) toremove=head;
                                        else if (neck!=null && neck.cursed>0) toremove=neck;
                                        else if (torso!=null && torso.cursed>0) toremove=torso;
                                        else if (legs!=null && legs.cursed>0) toremove=legs;
                                        else if (feet!=null && feet.cursed>0) toremove=feet;
                                        if (toremove!=null) {
                                                toremove.cursed-=inhand.potionpow;
                                                if (toremove.cursed<0) toremove.cursed=0;
                                                if (toremove.cursed==0) {
                                                        message.setMessage("The curse on "+toremove.name+" is broken.",4);
                                                        if (sheet) herosheet.repaint();
                                                        if (toremove==weapon || toremove==hand) repaint();
                                                }
                                        }
                                        break;
                                default:
                                        return false;
                        }
                        return true;
                }

                public int damage(int ht,int type) {
                     if (!isdead) {

                        if (sleeping && type!=POISONHIT) {
                                needredraw = true;
                                sleeping = false;
                                sleeper = 0;
                                sheet=false;
                                showCenter(dview);
                                spellsheet.setVisible(true);
                                weaponsheet.setVisible(true);
                                arrowsheet.setVisible(true);
                                weaponsheet.repaint();
                        }
                        else if (dmmap!=null && mappane.isVisible() && type!=POISONHIT) {
                                needredraw = true;
                                sheet=false;
                                showCenter(dview);
                                weaponsheet.repaint();
                                mappane.setVisible(false);
                                toppanel.setVisible(true);
                                centerpanel.setVisible(true);
                                /*
                                ecpanel.setVisible(true);
                                if (spellsheet!=null) {
                                        spellsheet.setVisible(true);
                                        weaponsheet.setVisible(true);
                                }
                                arrowsheet.setVisible(true);
                                message.setVisible(true);
                                hpanel.setVisible(true);
                                formation.setVisible(true);
                                toppanel.setVisible(true);
                                maincenterpan.setVisible(true);
                                eastpanel.setVisible(true);
                                */
                                validate();
                                //setContentPane(imagePane);
                                //validate();
                        }
                        boolean hashurt = (hurtweapon || hurthand || hurthead || hurttorso || hurtlegs || hurtfeet);
                        if (type==WEAPONHIT || type==PROJWEAPONHIT) {
                                int olddefense = defense;
                                if (hashurt) {
                                        if (hurthand && hand!=null && hand.type==Item.SHIELD && hand.defense>0) defense-=hand.defense;
                                        if (hurtweapon && weapon!=null && weapon.type==Item.SHIELD && weapon.defense>0) defense-=weapon.defense;
                                        if (defense<0) defense=0;
                                        else defense = defense*2/3;
                                }
                                ht-=(ht*defense/100);
                                defense = olddefense;
                        }        
                        else if (type==SPELLHIT || type==DRAINHIT) {
                                int oldmagicresist = magicresist;
                                if (hashurt) {
                                        if (hurthand && hand!=null && hand.type==Item.SHIELD && hand.magicresist>0) magicresist-=hand.magicresist;
                                        if (hurtweapon && weapon!=null && weapon.type==Item.SHIELD && weapon.magicresist>0) magicresist-=weapon.magicresist;
                                        if (magicresist<0) magicresist=0;
                                        else magicresist = magicresist*2/3;
                                }
                                ht-=(ht*magicresist/100);
                                magicresist = oldmagicresist;
                        }
                        else if (type==DOORHIT && head!=null) ht-=(ht*head.defense/100);
                        if (ht<1) ht=randGen.nextInt(4)+1;
                        else if (DIFFICULTY<0) ht = (int)(((double)ht)*(1.0+((double)DIFFICULTY)*.25));
                        if (berserkhealth>0) {
                                berserkhealth-=ht;
                                if (berserkhealth<0) {
                                        health+=berserkhealth; //subtracts from health, since berserkhealth is negative
                                        berserkhealth = 0;
                                }
                        }
                        else health-=ht;
                        //if (hurtcounter>0) hit+=ht;
                        //else hit=ht;
                        hit=ht;
                        if (health<1) {
                                isdead=true;
                                removeMouseListener(hclick);
                                repaint();
                                if (weapon.number==215) type=STORMHIT;
                                dropAllItems(type); load=0.0f;
                                health = 0;
                                currentspell="";
                                ispoisoned = false; poison=0;
                                silencecount = 0; silenced = false;
                                detectcurse = 0; falseimage = 0; berserk = 0; berserkhealth = 0; berserkstr = 0; backstab = false;
                                abilityactive = -1; abilityimm = false; abilitydiamond = false; abilitystun = false; abilitypoison = 0;
                                //close herosheet
                                if (!sleeping && sheet && this.equals(herosheet.hero)) {
                                        herosheet.skipchestscroll = false;
                                        if (viewing) {
                                                viewing=false;
                                                if (iteminhand && inhand.number==4) {
                                                        inhand.pic = inhand.temppic;
                                                        if (!isleader) changeCursor();
                                                }
                                        }
                                        sheet=false;

                                        showCenter(dview);
                                }
                                //if you die with stormbringer in hand, your soul is consumed
                                updateDark();
                                formation.mouseExited(null);
                                heroatsub[subsquare]=-1;
                                formation.addNewHero();//removes as well
                                int i=0;
                                if (isleader) {
                                        isleader=false;
                                        int j = 0;
                                        while (j<numheroes) {
                                                if (j!=heronumber && !hero[j].isdead) {
                                                        hero[j].isleader=true;
                                                        leader=j;
                                                        if (sheet && herosheet.hero.heronumber==j) herosheet.repaint();
                                                        break;
                                                }
                                                else j++;
                                        }
                                }
                                if (leader==heronumber) {
                                        //System.out.println("all died");
                                        playSound("scream.wav",-1,-1);
                                        iteminhand = false;
                                        changeCursor();
                                        alldead=true;
                                        gameover = true;
                                        if (sleeping) {
                                                //System.out.println("all died while sleeping");
                                                sleeping = false;
                                                sleeper = 0;
                                                sheet=false;
                                                needredraw = true;
                                                showCenter(dview);
                                                spellsheet.setVisible(true);
                                                weaponsheet.setVisible(true);
                                                arrowsheet.setVisible(true);
                                                weaponsheet.repaint();
                                        }
                                        else if (dmmap!=null && dmmap.isShowing()) {
                                                needredraw = true;
                                                sheet=false;
                                                showCenter(dview);
                                                weaponsheet.repaint();
                                                setContentPane(imagePane);
                                                validate();
                                        }
                                }
                                else {
                                        if (spellready==heronumber) {
                                                spellready=leader;
                                        }
                                        if (weaponready==heronumber) {
                                                weaponready=leader;
                                                if (weaponsheet.showingspecials) weaponsheet.toggleSpecials(leader);
                                        }
                                        weaponsheet.repaint();
                                        spellsheet.repaint();
                                        strength-=strengthboost; strengthboost=0;
                                        vitality-=vitalityboost; vitalityboost=0;
                                        dexterity-=dexterityboost; dexterityboost=0;
                                        intelligence-=intelligenceboost; intelligenceboost=0;
                                        wisdom-=wisdomboost; wisdomboost=0;
                                        defense-=defenseboost; defenseboost=0;
                                        magicresist-=magicresistboost; magicresistboost=0;
                                        hurtweapon = false; hurthand = false; hurthead = false; hurttorso = false; hurtlegs = false; hurtfeet = false;
                                }
                        }
                        else { 
                           //hurtcounter = 3; paint(getGraphics()); if (sheet && !viewing && !sleeping) herosheet.repaint();
                           hurtcounter = 3; repaint(); if (sheet && !viewing && !sleeping) herosheet.repaint();
                           if (type==WEAPONHIT) gainxp('f',1);
                           //cause injuries if hit is hard enough and right type
                           //if a hit does half or more of total hp, 90% chance of injury
                           //if ((type==WEAPONHIT || type==SPELLHIT || type==PROJWEAPONHIT) && (10*ht)/maxhealth>=5 && (!hurtweapon || !hurthand || !hurthead || !hurttorso || !hurtlegs || !hurtfeet) && randGen.nextInt(10)>0) {
                           if ((type==WEAPONHIT || type==SPELLHIT || type==PROJWEAPONHIT) && (ht>100 || (ht>maxhealth/4 && ht>40)) && (!hurtweapon || !hurthand || !hurthead || !hurttorso || !hurtlegs || !hurtfeet) && randGen.nextInt(10)>0) {
                                ArrayList intlist = new ArrayList(6);
                                intlist.add(new Integer(0)); intlist.add(new Integer(1)); intlist.add(new Integer(2));
                                intlist.add(new Integer(3)); intlist.add(new Integer(4)); intlist.add(new Integer(5));
                                int i=6,index;
                                boolean found=false;
                                while (!found && i>0) {
                                   index=randGen.nextInt(i);
                                   switch ( ((Integer)intlist.remove(index)).intValue() ) {
                                        case 0://head
                                                if (!hurthead) {
                                                        hurthead=true;
                                                        found=true;
                                                }
                                                break;
                                        case 1://torso
                                                if (!hurttorso) {
                                                        hurttorso=true;
                                                        found=true;
                                                }
                                                break;
                                        case 2://legs
                                                if (!hurtlegs) {
                                                        hurtlegs=true;
                                                        found=true;
                                                }
                                                break;
                                        case 3://feet
                                                if (!hurtfeet) {
                                                        hurtfeet=true;
                                                        found=true;
                                                }
                                                break;
                                        case 4://hand
                                                if (!hurthand) {
                                                        hurthand=true;
                                                        found=true;
                                                }
                                                break;
                                        case 5://weapon
                                                if (!hurtweapon) {
                                                        hurtweapon=true;
                                                        found=true;
                                                }
                                                break;
                                   }
                                   i--;
                                }
                                /*
                                int hurtnum=-1,hurtval=101;
                                ArrayList intlist = new ArrayList(6);
                                intlist.add(new Integer(0)); intlist.add(new Integer(1)); intlist.add(new Integer(2));
                                intlist.add(new Integer(3)); intlist.add(new Integer(4)); intlist.add(new Integer(5));
                                int i=0,index=randGen.nextInt(6);
                                boolean found=false;
                                while (!found && i<6) {
                                        if (!injured[i]) switch (i) {
                                                case 0://head
                                                        if (head==null) {
                                                                hurtnum=0;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && head.magicresist<hurtval) {
                                                                hurtnum=0;
                                                                hurtval=head.magicresist;
                                                        }
                                                        else if (head.defense<hurtval) {
                                                                hurtnum=0;
                                                                hurtval=head.defense;
                                                        }
                                                        break;
                                                case 1://torso
                                                        if (torso==null) {
                                                                hurtnum=1;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && torso.magicresist<hurtval) {
                                                                hurtnum=1;
                                                                hurtval=torso.magicresist;
                                                        }
                                                        else if (torso.defense<hurtval) {
                                                                hurtnum=1;
                                                                hurtval=torso.defense;
                                                        }
                                                        break;
                                                case 2://legs
                                                        if (legs==null) {
                                                                hurtnum=2;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && legs.magicresist<hurtval) {
                                                                hurtnum=2;
                                                                hurtval=legs.magicresist;
                                                        }
                                                        else if (legs.defense<hurtval) {
                                                                hurtnum=2;
                                                                hurtval=legs.defense;
                                                        }
                                                        break;
                                                case 3://feet
                                                        if (feet==null) {
                                                                hurtnum=3;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && feet.magicresist<hurtval) {
                                                                hurtnum=3;
                                                                hurtval=feet.magicresist;
                                                        }
                                                        else if (feet.defense<hurtval) {
                                                                hurtnum=3;
                                                                hurtval=feet.defense;
                                                        }
                                                        break;
                                                case 4://ready hand
                                                        if (hand==null) {
                                                                hurtnum=4;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && hand.magicresist<hurtval) {
                                                                hurtnum=4;
                                                                hurtval=hand.magicresist;
                                                        }
                                                        else if (hand.defense<hurtval) {
                                                                hurtnum=4;
                                                                hurtval=hand.defense;
                                                        }
                                                        break;
                                                case 5://action hand
                                                        if (weapon==null) {
                                                                hurtnum=5;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && weapon.magicresist<hurtval) {
                                                                hurtnum=5;
                                                                hurtval=weapon.magicresist;
                                                        }
                                                        else if (weapon.defense<hurtval) {
                                                                hurtnum=5;
                                                                hurtval=weapon.defense;
                                                        }
                                                        break;
                                        }
                                }
                                */
                           }
                        }
                        return ht;
                     }
                     else return 0;
                }

                private void dropAllItems(int type) {
                        MapObject p = DungeonMap[level][partyx][partyy];
                        for (int i=0;i<16;i++) {
                                if (pack[i]!=null) {
                                        pack[i].subsquare=(subsquare-facing+4)%4;
                                        if (!p.tryTeleport(pack[i])) {
                                                p.addItem(pack[i]);
                                                p.tryFloorSwitch(MapObject.PUTITEM);
                                        }
                                        pack[i] = null;
                                }
                        }
                        if (pouch2!=null) {
                                pouch2.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(pouch2)) {
                                        p.addItem(pouch2);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                pouch2 = null;
                        }
                        if (pouch1!=null) {
                                pouch1.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(pouch1)) {
                                        p.addItem(pouch1);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                pouch1 = null;
                        }
                        for (int i=0;i<6;i++) {
                                if (quiver[i]!=null) {
                                        quiver[i].subsquare=(subsquare-facing+4)%4;
                                        if (!p.tryTeleport(quiver[i])) {
                                                p.addItem(quiver[i]);
                                                p.tryFloorSwitch(MapObject.PUTITEM);
                                        }
                                        quiver[i] = null;
                                }
                        }
                        if (feet!=null) {
                                feet.unEquipEffect(this);
                                feet.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(feet)) {
                                        p.addItem(feet);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                feet = null;
                        }
                        if (legs!=null) {
                                legs.unEquipEffect(this);
                                legs.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(legs)) {
                                        p.addItem(legs);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                legs = null;
                        }
                        if (torso!=null) {
                                torso.unEquipEffect(this);
                                torso.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(torso)) {
                                        p.addItem(torso);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                torso = null;
                        }
                        if (neck!=null) {
                                neck.unEquipEffect(this);
                                if (neck.number==89) numillumlets--;
                                neck.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(neck)) {
                                        p.addItem(neck);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                neck = null;
                        }
                        if (head!=null) {
                                head.unEquipEffect(this);
                                head.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(head)) {
                                        p.addItem(head);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                head = null;
                        }
                        if (hand!=null) {
                                if (hand.number==9) {
                                        ((Torch)hand).putOut();
                                        //updateDark();
                                }
                                if (hand.type==Item.SHIELD) hand.unEquipEffect(this);
                                hand.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(hand)) {
                                        p.addItem(hand);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                hand = null;
                        }
                        if (!weapon.name.equals("Fist/Foot")) {
                                if (weapon.number==9) {
                                        ((Torch)weapon).putOut();
                                        //updateDark();
                                }
                                if (weapon.type==Item.WEAPON || weapon.type==Item.SHIELD) weapon.unEquipEffect(this);
                                if (weapon.number!=219 && weapon.number!=261) {
                                        weapon.subsquare=(subsquare-facing+4)%4;
                                        if (!p.tryTeleport(weapon)) {
                                                p.addItem(weapon);
                                                p.tryFloorSwitch(MapObject.PUTITEM);
                                        }
                                }
                                weapon=fistfoot;
                        }
                        if (isleader && iteminhand) {
                                iteminhand=false;
                                inhand.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(inhand)) {
                                        p.addItem(inhand);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                changeCursor();
                        }
                        Item bones;
                        if (type!=STORMHIT) bones = new Item(name,heronumber);
                        else { bones = new Item(75); bones.name = name+" Bones"; }
                        bones.subsquare=(subsquare-facing+4)%4;
                        if (!p.tryTeleport(bones)) {
                                p.addItem(bones);
                                p.tryFloorSwitch(MapObject.PUTITEM);
                        }
                        needredraw = true;
                }

                public void heal(int h) {
                        health+=h;
                        if (health>maxhealth) health=maxhealth;
                        else if (health<=0) health=1;
                }

                public void vitalize(int s) {
                        stamina+=s;
                        if (stamina>maxstamina) stamina=maxstamina;
                        else if (stamina<=0) stamina=1;
                        setMaxLoad();
                        if (sheet && herosheet.hero.heronumber==heronumber) herosheet.repaint();
                }

                public void energize(int m) {
                        mana+=m;
                        if (mana<0) mana=0;
                }

                public void paintComponent(Graphics gg) {
                      if (!isdead) {
                        gg.setColor(Color.black);
                        gg.fillRect(0,0,getWidth(),getHeight());
                        Graphics2D g = (Graphics2D)gg;
                        g.setFont(dungfont14);
                        if (TEXTANTIALIAS) g.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING,RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
//                        g.clearRect(0,0,getWidth(),getHeight());
                        g.setColor(new Color(100,100,100));
                        g.fillRect(0,0,getWidth(),84);
                        g.setColor(new Color(150,150,150));
                        g.fillRect(0,0,getWidth(),18);
                        if (hurtcounter>0) {
                                g.setColor(Color.red);
                                g.fillRect(5,20,60,60);
                                //g.setFont(new Font("TimesRoman",Font.BOLD,14));
                                g.setColor(new Color(30,30,30));
                                g.drawString("< "+hit+" >",17,50);
                                g.setColor(Color.yellow);
                                g.drawString("< "+hit+" >",14,47);
                        }
                        else g.drawImage(pic,5,20,this);

                        //Double tempd=new Double(((float)health/(float)maxhealth)*60.0);
                        //int len=tempd.intValue();
                        int len = (int)((float)health/(float)maxhealth*60.0f);
                        if (len>60) len=60;
                        g.setColor(new Color(20,20,20));
                        g.fillRect(72,82-len,5,len);
                        g.setColor(new Color(0,150,0));
                        g.fillRect(70,80-len,5,len);

                        //tempd=new Double(((float)stamina/(float)maxstamina)*60.0);
                        //len=tempd.intValue();
                        len = (int)((float)stamina/(float)maxstamina*60.0f);
                        if (len>60) len=60;
                        g.setColor(new Color(20,20,20));
                        g.fillRect(82,82-len,5,len);
                        g.setColor(new Color(250,200,0));
                        g.fillRect(80,80-len,5,len);

                        //tempd=new Double(((float)mana/(float)maxmana)*60.0);
                        //len=tempd.intValue();
                        len = (int)((float)mana/(float)maxmana*60.0f);
                        if (len>60) len=60;
                        g.setColor(new Color(20,20,20));
                        g.fillRect(92,82-len,5,len);
                        g.setColor(new Color(100,0,240));
                        g.fillRect(90,80-len,5,len);

                        g.setColor(new Color(30,30,30));
                        g.drawString(name,7,16);//14);
                        if (isleader) g.setColor(new Color(240,220,0));
                        else g.setColor(Color.white);
                        g.drawString(name,5,14);//12);

                        if (heronumber==0) g.setColor(new Color(0,216,0));
                        else if (heronumber==1) g.setColor(new Color(236,217,0));
                        else if (heronumber==2) g.setColor(Color.red);
                        else g.setColor(Color.blue);
                        g.fillRect(getWidth()-10,0,10,10);

                        //draw hands
                        //g.setColor(new Color(50,50,50));
                        //g.fillRect(0,84,this.getSize().width,2);
                        g.setColor(new Color(60,60,60));//was 80,80,80
                        g.fillRect(12,89,32,32);
                        g.fillRect(56,89,32,32);
                        g.setColor(new Color(150,150,150));
                        g.drawRect(10,87,35,35);
                        g.drawRect(11,88,33,33);
                        g.drawRect(54,87,35,35);
                        g.drawRect(55,88,33,33);

                        if (hand!=null) {
                                g.drawImage(hand.pic,12,89,this);
                                if (hand.cursed>0 && hand.cursefound) {
                                        Graphics2D g2 = (Graphics2D)g;
                                        Composite ac = g2.getComposite();
                                        g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER,.48f));
                                        g2.setRenderingHint(RenderingHints.KEY_ALPHA_INTERPOLATION,RenderingHints.VALUE_ALPHA_INTERPOLATION_SPEED);
                                        g2.setRenderingHint(RenderingHints.KEY_RENDERING,RenderingHints.VALUE_RENDER_SPEED);
                                        g2.setColor(new Color(200,0,0));
                                        g2.fillRect(12,89,32,32);
                                        g2.setComposite(ac);
                                }
                        }
                        //else if (!hurthand) 
                        //else g.drawImage(fistfoot.pic,44,89,12,121,0,0,32,32,this);
						else {
							AffineTransform tran = AffineTransform.getTranslateInstance(12+32,89);
							tran.scale(-1.0,1.0);
							g.drawImage(fistfoot.pic,tran,this);
						}
                        if (hurthand) {
                                if (hand==null) g.drawImage(dmnew.hurthand,12,89,this);
                                Graphics2D g2 = (Graphics2D)g;
                                g2.setColor(Color.red);
                                g2.setStroke(new BasicStroke(2.0f));
                                g2.drawRect(11,88,34,34);
                        }
                        //if (weapon!=fistfoot || !hurtweapon) 
                        g.drawImage(weapon.pic,56,89,this);
                        if (weapon.cursed>0 && weapon.cursefound) {
                                Graphics2D g2 = (Graphics2D)g;
                                Composite ac = g2.getComposite();
                                g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER,.48f));
                                g2.setRenderingHint(RenderingHints.KEY_ALPHA_INTERPOLATION,RenderingHints.VALUE_ALPHA_INTERPOLATION_SPEED);
                                g2.setRenderingHint(RenderingHints.KEY_RENDERING,RenderingHints.VALUE_RENDER_SPEED);
                                g2.setColor(new Color(200,0,0));
                                g2.fillRect(56,89,32,32);
                                g2.setComposite(ac);
                        }
                        if (hurtweapon) {
                                if (weapon==fistfoot) g.drawImage(dmnew.hurtweapon,56,89,this);
                                Graphics2D g2 = (Graphics2D)g;
                                g2.setColor(Color.red);
                                g2.setStroke(new BasicStroke(2.0f));
                                g2.drawRect(55,88,34,34);
                        }

                        
                        if (sheet && herosheet.hero.equals(this)) {
                                g.setColor(Color.yellow);
                                //g.drawRect(0,0,getWidth()-1,getHeight()-1);
								g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }
                        else if (ispoisoned) {
                                g.setColor(new Color(0,150,0));
                                g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }
                        else if (magicresistboost>0 && defenseboost>0) {
                                g.setColor(new Color(150,0,150));
                                g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }
                        else if (magicresistboost>0) {
                                g.setColor(Color.red);
                                g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }
                        else if (defenseboost>0) {
                                g.setColor(Color.blue);
                                g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }

                      }
                      else {
                        gg.setColor(Color.black);
                        gg.fillRect(0,0,getWidth(),getHeight());
                        gg.drawImage(deadheropic,15,20,this);
                      }
                }
                
                public void save(ObjectOutputStream so) throws IOException {
                        so.writeUTF(picname);
                        so.writeInt(subsquare);
                        so.writeInt(number);
                        so.writeUTF(name);
                        so.writeUTF(lastname);
                        so.writeInt(maxhealth);
                        so.writeInt(health);
                        so.writeInt(maxstamina);
                        so.writeInt(stamina);
                        so.writeInt(maxmana);
                        so.writeInt(mana);
                        //so.writeFloat(maxload);
                        so.writeFloat(load);
                        so.writeInt(food);
                        so.writeInt(water);
                        so.writeInt(strength);
                        so.writeInt(vitality);
                        so.writeInt(dexterity);
                        so.writeInt(intelligence);
                        so.writeInt(wisdom);
                        so.writeInt(defense);
                        so.writeInt(magicresist);
                        so.writeInt(strengthboost);
                        so.writeInt(vitalityboost);
                        so.writeInt(dexterityboost);
                        so.writeInt(intelligenceboost);
                        so.writeInt(wisdomboost);
                        so.writeInt(defenseboost);
                        so.writeInt(magicresistboost);
                        so.writeInt(flevel);
                        so.writeInt(nlevel);
                        so.writeInt(plevel);
                        so.writeInt(wlevel);
                        so.writeInt(flevelboost);
                        so.writeInt(nlevelboost);
                        so.writeInt(plevelboost);
                        so.writeInt(wlevelboost);
                        so.writeInt(fxp);
                        so.writeInt(nxp);
                        so.writeInt(pxp);
                        so.writeInt(wxp);
                        so.writeBoolean(isdead);
                        so.writeBoolean(wepready);
                        so.writeBoolean(ispoisoned);
                        if (ispoisoned) {
                                so.writeInt(poison);
                                so.writeInt(poisoncounter);
                        }
                        so.writeBoolean(silenced);
                        if (silenced) so.writeInt(silencecount);
                        so.writeBoolean(hurtweapon);
                        so.writeBoolean(hurthand);
                        so.writeBoolean(hurthead);
                        so.writeBoolean(hurttorso);
                        so.writeBoolean(hurtlegs);
                        so.writeBoolean(hurtfeet);
                        so.writeInt(timecounter);
                        so.writeInt(walkcounter);
                        so.writeInt(spellcount);
                        so.writeInt(weaponcount);
                        so.writeInt(kuswordcount);
                        so.writeInt(rosbowcount);
                        so.writeUTF(currentspell);
                        //write abilities here
                        if (abilities!=null) {
                                so.writeInt(abilities.length);
                                for (int j=0;j<abilities.length;j++) {
                                        abilities[j].save(so);
                                }
                        }
                        else so.writeInt(0);
                        if (weapon==fistfoot) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(weapon);
                        }
                        if (hand==null) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(hand);
                        }
                        if (head==null) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(head);
                        }
                        if (torso==null) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(torso);
                        }
                        if (legs==null) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(legs);
                        }
                        if (feet==null) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(feet);
                        }
                        if (neck==null) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(neck);
                        }
                        if (pouch1==null) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(pouch1);
                        }
                        if (pouch2==null) so.writeBoolean(false);
                        else {
                                so.writeBoolean(true);
                                so.writeObject(pouch2);
                        }
                        so.writeObject(quiver);
                        so.writeObject(pack);
                }
                public void load(ObjectInputStream si) throws IOException,ClassNotFoundException {
                        subsquare = si.readInt();
                        number = si.readInt();
                        name = si.readUTF();
                        lastname = si.readUTF();
                        maxhealth = si.readInt();
                        health = si.readInt();
                        maxstamina = si.readInt();
                        stamina = si.readInt();
                        maxmana = si.readInt();
                        mana = si.readInt();
                        //maxload = si.readFloat();
                        load = si.readFloat();
                        food = si.readInt();
                        water = si.readInt();
                        strength = si.readInt();
                        vitality = si.readInt();
                        dexterity = si.readInt();
                        intelligence = si.readInt();
                        wisdom = si.readInt();
                        defense = si.readInt();
                        magicresist = si.readInt();
                        strengthboost = si.readInt();
                        vitalityboost = si.readInt();
                        dexterityboost = si.readInt();
                        intelligenceboost = si.readInt();
                        wisdomboost = si.readInt();
                        defenseboost = si.readInt();
                        magicresistboost = si.readInt();
                        flevel = si.readInt();
                        nlevel = si.readInt();
                        plevel = si.readInt();
                        wlevel = si.readInt();
                        flevelboost = si.readInt();
                        nlevelboost = si.readInt();
                        plevelboost = si.readInt();
                        wlevelboost = si.readInt();
                        fxp = si.readInt();
                        nxp = si.readInt();
                        pxp = si.readInt();
                        wxp = si.readInt();
                        isdead = si.readBoolean();
                        wepready = si.readBoolean();
                        ispoisoned = si.readBoolean();
                        if (ispoisoned) {
                                poison = si.readInt();
                                poisoncounter = si.readInt();
                        }
                        silenced = si.readBoolean();
                        if (silenced) silencecount = si.readInt();
                        hurtweapon = si.readBoolean();
                        hurthand = si.readBoolean();
                        hurthead = si.readBoolean();
                        hurttorso = si.readBoolean();
                        hurtlegs = si.readBoolean();
                        hurtfeet = si.readBoolean();
                        timecounter = si.readInt();
                        walkcounter = si.readInt();
                        spellcount = si.readInt();
                        weaponcount = si.readInt();
                        kuswordcount = si.readInt();
                        rosbowcount = si.readInt();
                        currentspell = si.readUTF();
                        //read abilities here
                        int numabils = si.readInt();
                        if (numabils>0) {
                                abilities = new SpecialAbility[numabils];
                                for (int j=0;j<numabils;j++) {
                                        abilities[j] = new SpecialAbility(si);
                                }
                        }
                        if (si.readBoolean()) {
                                weapon = (Item)si.readObject();
                                if (weapon.number==9) ((Torch)weapon).setPic();
                        }
                        else weapon=fistfoot;
                        if (si.readBoolean()) {
                                hand = (Item)si.readObject();
                                if (hand.number==9) ((Torch)hand).setPic();
                        }
                        if (si.readBoolean()) head = (Item)si.readObject();
                        if (si.readBoolean()) torso = (Item)si.readObject();
                        if (si.readBoolean()) legs = (Item)si.readObject();
                        if (si.readBoolean()) feet = (Item)si.readObject();
                        if (si.readBoolean()) {
                                neck = (Item)si.readObject();
                                //if (neck.number==89) numillumlets++;
                        }
                        if (si.readBoolean()) pouch1 = (Item)si.readObject();
                        if (si.readBoolean()) pouch2 = (Item)si.readObject();
                        quiver = (Item[])si.readObject();
                        pack = (Item[])si.readObject();
                        setMaxLoad();
                }
        }
}
