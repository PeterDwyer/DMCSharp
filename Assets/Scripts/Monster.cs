using UnityEngine;
using System.Collections;

        class Monster {
                int number;
                String name;
                int power;
                int defense;     //% of weapon damage resisted
                int magicresist; //% of magic damage resisted
                int speed;       //affects chance of missing/dodging
                int manapower;   //intelligence & wisdom merged into 1 attribute
                int movespeed;   //rate of movement
                int attackspeed; //rate of attack (kind of...) must be < movespeed
                int mana;
                int maxmana = 0;
                int health;
                int maxhealth;
                int fearresist = 5; //int affecting success rate of horn of fear (0-never works to 10-always works)
                boolean wasfrightened = false; //switches mon's ai to run
                int facing = NORTH;
                int subsquare = 0;
                int currentai = 0;
                int defaultai = 1;
                int newai;
                int powerboost,defenseboost,magicresistboost,speedboost,manapowerboost,movespeedboost,attackspeedboost;
                int parry = -1;
                Image towardspic;
                Image awaypic;
                Image rightpic;
                Image leftpic;
                Image attackpic;
                Image castpic;
                boolean hasmagic = false;
                boolean hasheal = false; //does mon have heal magic?
                boolean hasdrain = false; //does mon have drain life magic?
                boolean silenced = false;
                int silencecount = 0;
                int castpower; //max power level of cast spells (can be 1-6), or shotpower for item projs
                int minproj = 0; //min mana needed to cast offensive magic
                boolean ignoremons = true; //will this mon shoot other mons if they are in the way?
                int numspells;
                String[] knownspells;
                int poison = 0; //strength of poison
                boolean isImmaterial = false; //not ghost
                boolean isflying = false; //if true, can fly over pits
                boolean canusestairs = true; //true if this mon can go up/down stairs (dragons too big, screamers cant climb, etc.)
                boolean canteleport = false; //true if mon can teleport like a sorcerer/vexirk
                int timecounter = 0;
                int movecounter = 0;
                int deathcounter = 0;
                int randomcounter = 0; //for trying to get around obstacle
                int runcounter = 0; //for hit&run ai, including gigglers
                int x,y,xdist=0,ydist=0,level;
                boolean isattacking = false; //used in drawMonster
                boolean iscasting = false;   //"
                boolean isdying = false;     //"
                boolean mirrored = false;
                boolean hurt = false; //used in ai routine
                boolean wasstuck; //used in ai routine for stayback - gotowards swaps
                int moveattack = 0; //used in ai routine to prevent double ai update when moving & attacking at once
                int hurttest = 0; //used in ai routine to delay a successive run when injured test
                boolean waitattack = false; //used in ai for anti-circling
                boolean ispoisoned; //if poison is afflicting mon
                //int poisonresist; //0 = 0% resist (normal), 1 = 25% resist, 2 = 50% resist, 3 = 75% resist, 4 = 100% resist (immune)
                boolean poisonimmune; //if true, poison has no effect
                int poisonpow; //power of poison afflicting mon
                int poisoncounter = 0; //determines when poison causes damage
                int olddir = -1; //used in ai routine for running
                int castsub = 0; //used in ai routine for sub5 smart mons casting
                boolean breakdoor = false; //used in ai routine to prevent breaking and moving in same turn
                ArrayList carrying = new ArrayList();
                ArrayList equipped;
                int ammo = 0; //arrows, knives, stars, whatever (1 type per monster)
                //int ammonumber = -1; //item number of proj thrown/shot
                boolean useammo = false;
                static final int RANDOM = 0, GOTOWARDS = 1, STAYBACK = 2, RUN = 3, GUARD = 4, FRIENDLYMOVE = 5, FRIENDLYNOMOVE = 6; //AI states
                boolean HITANDRUN = false; //true for gigglers, assassins, others?
                boolean gamewin = false;
                //String endanim,endmusic,endsound,picstring,soundstring;
                String endanim,endsound,picstring,soundstring,footstep;
                int hurtitem,needitem,needhandneck,pickup,steal;
                
                public Monster(int num,int xc,int yc,int lev,String name, String picstring, String soundstring, String footstep, boolean canusestairs, boolean isflying, boolean ignoremons, boolean canteleport) {
                        number = num;
                        x = xc;
                        y = yc;
                        level = lev;
                        this.name = name;
                        this.picstring = picstring;
                        this.soundstring = soundstring;
                        this.footstep = footstep;
                        this.canusestairs = canusestairs;
                        this.isflying = isflying;
                        this.ignoremons = ignoremons;
                        this.canteleport = canteleport;
                        
                        File picfile = new File("Monsters"+File.separator+picstring+"-toward.gif");
                        if (picfile.exists()) towardspic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-toward.png");
                                if (picfile.exists()) towardspic = tk.getImage(picfile.getPath());
                                else towardspic = tk.getImage("Monsters"+File.separator+"screamer-toward.gif");
                        }
                        
                        picfile = new File("Monsters"+File.separator+picstring+"-away.gif");
                        if (picfile.exists()) awaypic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-away.png");
                                if (picfile.exists()) awaypic = tk.getImage(picfile.getPath());
                                else awaypic = towardspic;
                        }

                        picfile = new File("Monsters"+File.separator+picstring+"-right.gif");
                        if (picfile.exists()) rightpic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-right.png");
                                if (picfile.exists()) rightpic = tk.getImage(picfile.getPath());
                                else rightpic = towardspic;
                        }
                        
                        picfile = new File("Monsters"+File.separator+picstring+"-left.gif");
                        if (picfile.exists()) leftpic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-left.png");
                                if (picfile.exists()) leftpic = tk.getImage(picfile.getPath());
                                else leftpic = towardspic;
                        }
                        
                        picfile = new File("Monsters"+File.separator+picstring+"-attack.gif");
                        if (picfile.exists()) attackpic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-attack.png");
                                if (picfile.exists()) attackpic = tk.getImage(picfile.getPath());
                                else attackpic = towardspic;
                        }
                        
                        picfile = new File("Monsters"+File.separator+picstring+"-cast.gif");
                        if (picfile.exists()) castpic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-cast.png");
                                if (picfile.exists()) castpic = tk.getImage(picfile.getPath());
                                else castpic = attackpic;
                        }
                        
                        ImageTracker.addImage(towardspic,2);
                        ImageTracker.addImage(awaypic,2);
                        ImageTracker.addImage(rightpic,2);
                        ImageTracker.addImage(leftpic,2);
                        ImageTracker.addImage(attackpic,2);
                        ImageTracker.addImage(castpic,2);
                        try { ImageTracker.waitForID(2,3000); }
                        catch (InterruptedException e) {}
                        ImageTracker.removeImage(towardspic,2);
                        ImageTracker.removeImage(awaypic,2);
                        ImageTracker.removeImage(rightpic,2);
                        ImageTracker.removeImage(leftpic,2);
                        ImageTracker.removeImage(attackpic,2);
                        ImageTracker.removeImage(castpic,2);
                }

                public Monster(int num,int xc,int yc,int lev) {
                        number = num;
                        x = xc;
                        y = yc;
                        level = lev;
                        footstep = "";
                        switch (number) {     //when i know all these, put in monsterwizard/monsterdata
                                case 0:
                                        name = "Mummy";
                                        towardspic = tk.getImage("Monsters"+File.separator+"mummy-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"mummy-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"mummy-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"mummy-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"mummy-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "mummy.wav";
                                        footstep = "step2.wav";
                                        picstring = "mummy";
                                        break;
                                case 1:
                                        name = "Screamer";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"screamer.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"screamer-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "spider.wav";
                                        footstep = "step1.wav";
                                        picstring = "screamer";
                                        break;
                                case 2:
                                        name = "Giggler";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"giggler-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"giggler-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"giggler-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"giggler-left.gif");
                                        //attackpic = tk.getImage("Monsters"+File.separator+"giggler-attack.gif");
                                        attackpic = towardspic;
                                        castpic = attackpic;
                                        soundstring = "giggler.wav";
                                        //footstep = "step2.wav";
                                        picstring = "giggler";
                                        break;
                                case 3:
                                        name = "Rock Pile";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"rockpile.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"rockpile-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "rockmon.wav";
                                        footstep = "step1.wav";
                                        picstring = "rockpile";
                                        break;
                                case 4:
                                        name = "Slime";
                                        towardspic = tk.getImage("Monsters"+File.separator+"slime.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"slime-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "slime.wav";
                                        picstring = "slime";
                                        break;
                                case 5:
                                        name = "Wing Eye";
                                        //ignoremons = false;
                                        isflying = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"wingeye.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"wingeye-attack.gif");
                                        castpic = attackpic;
                                        footstep = "flap.wav";
                                        picstring = "wingeye";
                                        break;
                                case 6:
                                        name = "Ghost";
                                        isflying = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"ghost.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"ghost-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "mummy.wav";
                                        picstring = "ghost";
                                        break;
                                case 7:
                                        name = "Muncher";
                                        isflying = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"muncher-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"muncher-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"muncher-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"muncher-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"muncher-attack.gif");
                                        castpic = attackpic;
                                        //footstep = "flap.wav";
                                        picstring = "muncher";
                                        break;
                                case 8:
                                        name = "Skeleton";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"skeleton-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"skeleton-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"skeleton-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"skeleton-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"skeleton-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "swing.wav";
                                        footstep = "step3.wav";
                                        picstring = "skeleton";
                                        break;
                                case 9:
                                        name = "Worm";
                                        towardspic = tk.getImage("Monsters"+File.separator+"worm-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"worm-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"worm-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"worm-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"worm-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "worm.wav";
                                        footstep = "step1.wav";
                                        picstring = "worm";

                                        break;
                                case 10:
                                        name = "Fire Elemental";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"fireel.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"fireel-attack.gif");
                                        castpic = attackpic;
                                        picstring = "fireel";
                                        break;
                                case 11:
                                        name = "Water Elemental";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"waterel.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"waterel-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "waterel.wav";
                                        picstring = "waterel";
                                        break;
                                case 12:
                                        name = "Goblin";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"goblin-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"goblin-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"goblin-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"goblin-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"goblin-attack.gif");
                                        castpic = attackpic;
                                        footstep = "step2.wav";
                                        picstring = "goblin";
                                        break;
                                case 13:
                                        name = "Giant Rat";
                                        towardspic = tk.getImage("Monsters"+File.separator+"rat-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"rat-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"rat-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"rat-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"rat-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "roar.wav";
                                        footstep = "step1.wav";
                                        picstring = "rat";
                                        break;
                                case 14:
                                        name = "Ant Man";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"antman-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"antman-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"antman-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"antman-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"antman-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "swing.wav";
                                        footstep = "step2.wav";
                                        picstring = "antman";
                                        break;
                                case 15:
                                        name = "Beholder";
                                        isflying = true;
                                        //ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"beholder.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"beholder-attack.gif");
                                        castpic = attackpic;
                                        picstring = "beholder";
                                        break;
                                case 16:
                                        name = "Couatyl";
                                        isflying = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"couatyl-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"couatyl-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"couatyl-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"couatyl-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"couatyl-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "couatyl.wav";
                                        footstep = "flap.wav";
                                        picstring = "couatyl";
                                        break;
                                case 17:
                                        name = "Fader";
                                        isflying = true;
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"fader.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        //attackpic = tk.getImage("Monsters"+File.separator+"fader-attack.gif");
                                        attackpic = towardspic;
                                        castpic = attackpic;
                                        soundstring = "rockmon.wav";
                                        picstring = "fader";
                                        break;
                                case 18:
                                        name = "Tentacle Beast";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"tentaclemon.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"tentaclemon-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "slime.wav";
                                        footstep = "step1.wav";
                                        picstring = "tentaclemon";
                                        break;
                                case 19:
                                        name = "Scorpion";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"scorpion-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"scorpion-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"scorpion-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"scorpion-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"scorpion-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "scorpion.wav";
                                        footstep = "step1.wav";
                                        picstring = "scorpion";
                                        break;
                                case 20:
                                        name = "Demon";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"demon-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"demon-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"demon-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"demon-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"demon-attack.gif");
                                        castpic = attackpic;
                                        footstep = "step2.wav";
                                        picstring = "demon";
                                        break;
                                case 21:
                                        name = "Deth Knight";
                                        //ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"dethknight-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"dethknight-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"dethknight-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"dethknight-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"dethknight-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "swing.wav";
                                        footstep = "step4.wav";
                                        picstring = "dethknight";
                                        break;
                                case 22:
                                        name = "Spider";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"spider-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"spider-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"spider-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"spider-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"spider-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "spider.wav";
                                        footstep = "step1.wav";
                                        picstring = "spider";
                                        break;
                                case 23:
                                        name = "Stone Golem";
                                        towardspic = tk.getImage("Monsters"+File.separator+"golem-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"golem-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"golem-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"golem-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"golem-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "swing.wav";
                                        footstep = "step2.wav";
                                        picstring = "golem";
                                        break;
                                case 24:
                                        name = "Sorcerer";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"sorcerer-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"sorcerer-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"sorcerer-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"sorcerer-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"sorcerer-attack.gif");
                                        castpic = attackpic;
                                        picstring = "sorcerer";
                                        canteleport = true;
                                        break;
                                case 25:  //need number in spellcasting
                                        name = "Dragon";
                                        //ignoremons = false;
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"dragon-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"dragon-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"dragon-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"dragon-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"dragon-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "roar.wav";
                                        footstep = "step5.wav";
                                        picstring = "dragon";
                                        break;
                                case 26:  //number used many places
                                        name = "Lord Chaos";
                                        isflying = true;
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"chaos-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"chaos-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"chaos-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"chaos-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"chaos-attack.gif");
                                        castpic = attackpic;
                                        picstring = "chaos";
                                        //canteleport = true;
                                        break;

                                case 27:  //number used some places
                                        name = "Demon Lord";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"demonlord.png");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"demonlord-attack.png");
                                        castpic = attackpic;
                                        soundstring = "demonlord.wav";
                                        footstep = "step5.wav";
                                        picstring = "demonlord";
                                        break;
                                /////////////////////////
                                /*
                                case :
                                        name = "Vexirk";
                                        power = 25;
                                        defense = 20;
                                        magicresist = 50;
                                        speed = 60;
                                        movespeed = 5;
                                        attackspeed = 1;
                                        maxhealth = 240+randGen.nextInt(30);
                                        maxmana = 2000;
                                        hasmagic = true;
                                        hasheal = true;
                                        ignoremons = false;
                                        castpower = 5;
                                        manapower = 80;
                                        numspells = 4;
                                        knownspells = new String[4];
                                        knownspells[0] = "44";
                                        knownspells[1] = "335";
                                        knownspells[2] = "51";//14 min for poison missle
                                        knownspells[3] = "31";
                                        minproj = 10; //min for poison cloud
                                        fearresist = 0;
                                        defaultai = STAYBACK;
                                        towardspic = tk.getImage("Monsters"+File.separator+"vexirk-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"vexirk-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"vexirk-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"vexirk-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"vexirk-attack.gif");
                                        castpic = attackpic;
                                        carrying.addElement(137); //robe top
                                        carrying.addElement(157); //robe bottom
                                        break;
                                case :
                                        name = "Hell Hound";
                                        power = 100;
                                        defense = 20;
                                        magicresist = 30;
                                        speed = 45;
                                        movespeed = 6;
                                        attackspeed = 1;
                                        maxhealth = 320+randGen.nextInt(30);
                                        fearresist = 4;
                                        defaultai = GOTOWARDS;
                                        towardspic = tk.getImage("Monsters"+File.separator+"hound-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"hound-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"hound-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"hound-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"hound-attack.gif");
                                        castpic = attackpic;
                                        carrying.addElement(new Item(68));//shank, maybe 2
                                        if (randGen.nextBoolean()) carrying.addElement(new Item(68));
                                        break;
                                case :
                                        name = "Giant Wasp";
                                        power = 15;
                                        defense = 10;
                                        magicresist = 20;
                                        speed = 75;
                                        movespeed = 4;
                                        attackspeed = 1;
                                        maxhealth = 180+randGen.nextInt(20);
                                        poison = 4;
                                        fearresist = 2;
                                        isflying = true;
                                        defaultai = GOTOWARDS;
                                        HITANDRUN = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"wasp.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"wasp-attack.gif");
                                        castpic = attackpic;
                                        break;
                                case :
                                        name = "Blue Ogre";
                                        power = 28;
                                        defense = 10;
                                        magicresist = 10;
                                        speed = 40;
                                        movespeed = 5;
                                        attackspeed = 1;
                                        maxhealth = 70+randGen.nextInt(10);
                                        fearresist = 6;
                                        defaultai = GOTOWARDS;
                                        towardspic = tk.getImage("Monsters"+File.separator+"blueogre-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"blueogre-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"blueogre-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"blueogre-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"blueogre-attack.gif");
                                        castpic = attackpic;
                                        carrying.addElement(new Item(226));//club
                                        break;
                                */
                        }
                        ImageTracker.addImage(towardspic,2);
                        ImageTracker.addImage(awaypic,2);
                        ImageTracker.addImage(rightpic,2);
                        ImageTracker.addImage(leftpic,2);
                        ImageTracker.addImage(attackpic,2);
                        ImageTracker.addImage(castpic,2);
                        try { ImageTracker.waitForID(2,3000); }
                        catch (InterruptedException e) {}
                        ImageTracker.removeImage(towardspic,2);
                        ImageTracker.removeImage(awaypic,2);
                        ImageTracker.removeImage(rightpic,2);
                        ImageTracker.removeImage(leftpic,2);
                        ImageTracker.removeImage(attackpic,2);
                        ImageTracker.removeImage(castpic,2);
                }

                public void timePass() {
                        if (moveattack==moncycle) { 
                                moveattack=0;
                                return;
                        }
                        else if (moveattack!=0) moveattack=0;
                        if (movespeedboost>0) { movespeedboost--; movespeed--; }
                        //if (attackspeedboost<0) { attackspeedboost++; attackspeed++; }
                        if (isdying) {
                                deathcounter++;
                                if (deathcounter>1) {
                                        dmmons.remove(level+","+x+","+y+","+subsquare);
                                        int i=0;
                                        boolean montest = false;
                                        while (i<6 && !montest) {
                                                if (dmmons.get(level+","+x+","+y+","+i)!=null) montest=true;
                                                if (i==3) i=5;
                                                else i++;
                                        }        
                                        DungeonMap[level][x][y].hasMons=montest;
                                        if (level==dmnew.level) needredraw = true;
                                }
                                return;
                        }
                        else if (isattacking) {
                                if (!breakdoor) hitHero();
                                isattacking = false;
                                //attackpic.flush();
                                if (level==dmnew.level) needredraw = true;
                                return;
                        }
                        else if (iscasting) {
                                iscasting = false;
                                if (level==dmnew.level) needredraw = true;
                                return;
                        }
                        breakdoor = false;
                        if (freezelife>0 && number!=26 && number!=27) return; //if all life frozen, doesn't affect chaos or demon lord
                        if (timecounter<0) { timecounter++; timecounter+=sleeper; return; } //if frozen
                        movecounter++;
                        movecounter+=sleeper;
                        if (movecounter>movespeed) {
                                //if (wasfrightened) { wasfrightened=false; currentai=RUN; }
                                if (wasfrightened) currentai=RUN;
                                //chaos doesn't like flux cages:
                                if (number==26 && fluxchanging && fluxcages.get(level+","+x+","+y)!=null) { runcounter=1; currentai=RUN; }
                                doAI();
                                //test for ai state change from gotowards back to stayback
                                if (defaultai==STAYBACK && currentai==GOTOWARDS) {
                                        if (!wasstuck && (ammo>0 || (hasmagic && !silenced && mana >= minproj))) currentai=STAYBACK;
                                        wasstuck=false;
                                }
                        }
                        else if (level==dmnew.level && xdist<5 && ydist<5 && randGen.nextInt(5)==0) {
                                mirrored=!mirrored;
                                needredraw=true;
                        }
                        timecounter++;
                        timecounter+=sleeper;
                        if (timecounter>210) {
                                timecounter=0;
                                heal(maxhealth/20+2);
                                energize(maxmana/20+2);
                                if (ispoisoned) {
                                        if (poisonpow>0) {
                                                //if (poisonpow>15) poisonpow=15;
                                                //else if (randGen.nextBoolean()) poisonpow--;
                                                if (randGen.nextBoolean()) poisonpow--;
                                                //else if ((maxhealth/4)/6-poison > 0 || randGen.nextInt(100) < maxhealth/4) poisonpow--;
                                        }
                                        else ispoisoned=false;
                                }
                                //silence
                                if (silencecount>0) {
                                        silencecount--;
                                        if (silencecount==0) silenced = false;
                                }
                                //if have stat boosting spells for mons, put decrement stuff here
                                if (powerboost<0) { powerboost++; power++; }
                                if (defenseboost<0) { defenseboost++; defense++; }
                                if (magicresistboost<0) { magicresistboost++; magicresist++; }
                                if (speedboost<0) { speedboost++; speed++; }
                                if (manapowerboost<0) { manapowerboost++; manapower++; }
                        }
                        if (ispoisoned) {
                                if (isImmaterial || poisonimmune) { ispoisoned=false; poisonpow=0; }
                                else {
                                        poisoncounter++;
                                        poisoncounter+=sleeper;
                                        if (poisoncounter>30) {
                                           poisoncounter=0;
                                           if (poisonpow>15) poisonpow=15;
                                           damage(poisonpow,POISONHIT);
                                           //System.out.println(name+" takes "+damage(poisonpow,POISONHIT)+" poison damage.");
                                        }
                                }
                        }
                }


                public void hitHero() {
                        //give hero[who] xp for dodging?
                        
                        //boolean backhit = false;
                        int who,whoalt,whotest1,whotest2,whotest3,whotest4;
                        if (facing==NORTH) { whotest1=2; whotest2=3; whotest3=0; whotest4=1; }
                        else if (facing==SOUTH) { whotest1=0; whotest2=1; whotest3=2; whotest4=3; }
                        else if (facing==EAST) { whotest1=0; whotest2=3; whotest3=1; whotest4=2; }
                        else { whotest1=1; whotest2=2; whotest3=0; whotest4=3; }
                        if (randGen.nextBoolean()) {
                                who = (whotest1+dmnew.facing)%4;
                                whoalt = (whotest2+dmnew.facing)%4;
                        }
                        else {
                                who = (whotest2+dmnew.facing)%4;
                                whoalt = (whotest1+dmnew.facing)%4;
                        }
                        if (heroatsub[who]==-1) {
                                who = whoalt;
                                if (heroatsub[who]==-1) {
                                        if (randGen.nextBoolean()) {
                                                who = (whotest3+dmnew.facing)%4;
                                                whoalt = (whotest4+dmnew.facing)%4;
                                        }
                                        else {
                                                who = (whotest4+dmnew.facing)%4;
                                                whoalt = (whotest3+dmnew.facing)%4;
                                        }
                                        if (heroatsub[who]==-1) who=whoalt;
                                        //backhit = true; //harder to hit back row
                                }
                        }
                        who = heroatsub[who];
                        //put this in to prevent errors when attacked with no character:
                        if (who==-1) return;
                        //if (name.equals("Giggler")) { //steal an item
                        if (steal!=0 && (steal==4 || (steal==2 && randGen.nextBoolean()) || (steal==1 && randGen.nextInt(4)==0) || (steal==3 && randGen.nextInt(4)!=0))) {
                                boolean found = false;
                                if (!hero[who].weapon.name.equals("Fist/Foot") && hero[who].weapon.number!=219 && hero[who].weapon.number!=261) { 
                                        carrying.add(hero[who].weapon);
                                        hero[who].load-=hero[who].weapon.weight;
                                        if (hero[who].weapon.type==Item.WEAPON || hero[who].weapon.type==Item.SHIELD) hero[who].weapon.unEquipEffect(hero[who]);
                                        if (hero[who].weapon.number==9) ((Torch)hero[who].weapon).putOut();
                                        hero[who].weapon=fistfoot;
                                        found=true;
                                        hero[who].repaint();
                                        weaponsheet.repaint();
                                        updateDark();
                                }
                                else if (hero[who].hand!=null) { 
                                        carrying.add(hero[who].hand);
                                        hero[who].load-=hero[who].hand.weight;
                                        if (hero[who].hand.type==Item.SHIELD) hero[who].hand.unEquipEffect(hero[who]);
                                        else if (hero[who].hand.number==9) ((Torch)hero[who].hand).putOut();
                                        hero[who].hand=null;
                                        found=true;
                                        hero[who].repaint();
                                        updateDark();
                                }
                                else if (!found) {
                                        ArrayList tryval = new ArrayList(5);
                                        tryval.add(new Integer(0)); tryval.add(new Integer(1)); tryval.add(new Integer(2)); tryval.add(new Integer(3)); tryval.add(new Integer(4)); tryval.add(new Integer(5)); tryval.add(new Integer(6)); tryval.add(new Integer(7)); tryval.add(new Integer(8));
                                        while (!found && !tryval.isEmpty()) {
                                                int val = ((Integer)tryval.remove(0)).intValue();
                                                switch (val) {
                                                        case 0:
                                                                //head
                                                                if (hero[who].head!=null) {
                                                                        carrying.add(hero[who].head);
                                                                        hero[who].load-=hero[who].head.weight;
                                                                        hero[who].head.unEquipEffect(hero[who]);
                                                                        hero[who].head=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 1:
                                                                //neck
                                                                if (hero[who].neck!=null) {
                                                                        if (hero[who].neck.number==89) numillumlets--;
                                                                        carrying.add(hero[who].neck);
                                                                        hero[who].load-=hero[who].neck.weight;
                                                                        hero[who].neck.unEquipEffect(hero[who]);
                                                                        hero[who].neck=null;
                                                                        found=true;
                                                                        updateDark();
                                                                }
                                                                break;
                                                        case 2:
                                                                //torso
                                                                if (hero[who].torso!=null) {
                                                                        carrying.add(hero[who].torso);
                                                                        hero[who].load-=hero[who].torso.weight;
                                                                        hero[who].torso.unEquipEffect(hero[who]);
                                                                        hero[who].torso=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 3:
                                                                //legs
                                                                if (hero[who].legs!=null) {
                                                                        carrying.add(hero[who].legs);
                                                                        hero[who].load-=hero[who].legs.weight;
                                                                        hero[who].legs.unEquipEffect(hero[who]);
                                                                        hero[who].legs=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 4:
                                                                //feet
                                                                if (hero[who].feet!=null) {
                                                                        carrying.add(hero[who].feet);
                                                                        hero[who].load-=hero[who].feet.weight;
                                                                        hero[who].feet.unEquipEffect(hero[who]);
                                                                        hero[who].feet=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 5:
                                                                //pouch1
                                                                if (hero[who].pouch1!=null) {
                                                                        carrying.add(hero[who].pouch1);
                                                                        hero[who].load-=hero[who].pouch1.weight;
                                                                        hero[who].pouch1=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 6:
                                                                //pouch2
                                                                if (hero[who].pouch2!=null) {
                                                                        carrying.add(hero[who].pouch2);
                                                                        hero[who].load-=hero[who].pouch2.weight;
                                                                        hero[who].pouch2=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 7:
                                                                //quiver
                                                                int i=randGen.nextInt(6);
                                                                int count = 0;
                                                                while (!found && count<6) {
                                                                        if (hero[who].quiver[i]!=null) {
                                                                                carrying.add(hero[who].quiver[i]);
                                                                                hero[who].load-=hero[who].quiver[i].weight;
                                                                                hero[who].quiver[i]=null;
                                                                                found=true;
                                                                        }
                                                                        else { i=(i+1)%6; count++; }
                                                                }
                                                                break;
                                                        case 8:
                                                                //pack
                                                                i=randGen.nextInt(16);
                                                                count = 0;
                                                                while (!found && count<16) {
                                                                        if (hero[who].pack[i]!=null) {
                                                                                carrying.add(hero[who].pack[i]);
                                                                                hero[who].load-=hero[who].pack[i].weight;
                                                                                hero[who].pack[i]=null;
                                                                                found=true;
                                                                        }
                                                                        else { i=(i+1)%16; count++; }
                                                                }
                                                                break;
                                                
                                                }
                                        }
                                }
                                if (sheet && found && herosheet.hero.equals(hero[who])) herosheet.repaint();
                                if (HITANDRUN && (!found || randGen.nextInt(32)!=0)) { runcounter=40+randGen.nextInt(10); currentai=RUN; }
                                return;
                        }
                        if (!sleeping && randGen.nextInt(20)==0) { movecounter-=4; message.setMessage(name+": Critical Miss",5); return; }
                        
                        int speeddif = speed - hero[who].dexterity;
                        if (hero[who].stamina<hero[who].maxstamina/4) speeddif+=5;
                        if (hero[who].load>hero[who].maxload) speeddif+=5;
                        if (hero[who].hurttorso || hero[who].hurtlegs || hero[who].hurtfeet) speeddif+=10;
                        //if (backhit) speeddif-=10;
                        if (parry==who) speeddif-=15;
                        parry=-1;
                        
                        boolean didhit;
                        if (sleeping) didhit = true;
                        else if (speeddif>40) didhit = (randGen.nextInt(8)!=0);
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
                        
                        //false image ability active -> image destroyed if didhit (and 50% chance if didmiss so doesn't get too powerful)
                        if (hero[who].falseimage>0 && (didhit || randGen.nextBoolean())) {
                                didhit = false;
                                hero[who].falseimage--;
                                if (hero[who].falseimage>0) message.setMessage("A false image around "+hero[who].name+" is destroyed.",hero[who].heronumber);
                                else message.setMessage("The last false image around "+hero[who].name+" is destroyed.",hero[who].heronumber);
                        }
                                
                        if (didhit) {
                                playSound("oof.wav",-1,-1);
                                int pow = power+randGen.nextInt(power/4+10);
                                //if (backhit) pow = pow*2/3;
                                if (speed>50 && (randGen.nextInt(20)==0 || (sleeping && randGen.nextInt(5)==0))) { pow=3*pow/2; message.setMessage(name+": Critical Hit",5); }
                                if (poison>0 && (randGen.nextInt(10)>4 || (DIFFICULTY>0 && randGen.nextInt(10+DIFFICULTY*5)>4))) {
                                   hero[who].poison+=poison; hero[who].ispoisoned=true;
                                   if (DIFFICULTY==-1 && hero[who].poison>poison*2) hero[who].poison=poison*2;
                                   else if (DIFFICULTY==-2) hero[who].poison=poison;
                                }
                                hero[who].damage(pow,WEAPONHIT);
                                hupdate();
                                if (HITANDRUN) { runcounter=12+(Math.abs(randGen.nextInt())%8); currentai=RUN; }
                        }
                }
                
                public void doAI() {
                        
                        //need improved way to deal with corners like:
                        //1111111111111
                        //11....m....11    .  floor
                        //11..11111..11    1  wall
                        //11..11111..11    x  party
                        //11....x....11    m  monster
                        //1111111111111
                        //need make move out of poison clouds/closing doors before really hurting
                        
                        //need deal with party fleeing up/down stairs...
                        //--maybe have a wasnear flag, go towards partyx partyy, could get lucky and find stairs
                        //--of course, currently party can simply stand in stairs and not be touchable since mon can't move into stairs if party is in them
                        //----maybe pull party up/down to mons level if mon tries to enter stairs, just like it does to mons if party enters them

                        if (currentai==FRIENDLYNOMOVE) {
                                movecounter = 0; //don't move at all
                                return;
                        }
                        
                        xdist = x-partyx; if (xdist<0) xdist*=-1;
                        ydist = y-partyy; if (ydist<0) ydist*=-1;
                        //if (level==dmnew.level && xdist<5 && ydist<5 && (number==16 || number==5)) playSound("flap.wav",x,y); //couatyl and wing-eye's flap
                        boolean canattack = false;
                        if (level==dmnew.level && ((xdist==0 && ydist==1) || (ydist==0 && xdist==1))) {
                            if (subsquare!=5) { 
                                if (x>partyx) {
                                        if (subsquare==0 || subsquare==3) canattack = true;
                                        //else if (currentai!=RUN || wasstuck) {
                                        else if (currentai==GOTOWARDS) {
                                          if (subsquare==1 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+0)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 0;
                                                dmmons.put(level+","+x+","+y+","+0,this);
                                          }
                                          else if (subsquare==2 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+3)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 3;
                                                dmmons.put(level+","+x+","+y+","+3,this);
                                          }
                                          //if (leftpic.equals(towardspic)) facing=WEST;
                                          facing = WEST;
                                        }
                                }
                                else if (x<partyx) {
                                        if (subsquare==1 || subsquare==2) canattack = true;
                                        //else if (currentai!=RUN || wasstuck) {
                                        else if (currentai==GOTOWARDS) {
                                          if (subsquare==0 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+1)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 1;
                                                dmmons.put(level+","+x+","+y+","+1,this);
                                          }
                                          else if (subsquare==3 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+2)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 2;
                                                dmmons.put(level+","+x+","+y+","+2,this);
                                          }
                                          //if (leftpic.equals(towardspic)) facing=EAST;
                                          facing = EAST;
                                        }
                                }
                                else if (y>partyy) {
                                        if (subsquare==0 || subsquare==1) canattack = true;
                                        //else if (currentai!=RUN || wasstuck) {
                                        else if (currentai==GOTOWARDS) {
                                          if (subsquare==2 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+1)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 1;
                                                dmmons.put(level+","+x+","+y+","+1,this);
                                          }
                                          else if (subsquare==3 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+0)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 0;
                                                dmmons.put(level+","+x+","+y+","+0,this);
                                          }
                                          //if (leftpic.equals(towardspic)) facing=NORTH;
                                          facing = NORTH;
                                        }
                                }
                                else if (y<partyy) {
                                        if (subsquare==2 || subsquare==3) canattack = true;
                                        //else if (currentai!=RUN || wasstuck) {
                                        else if (currentai==GOTOWARDS) {
                                          if (subsquare==0 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+3)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 3;
                                                dmmons.put(level+","+x+","+y+","+3,this);
                                          }
                                          else if (subsquare==1 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+2)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 2;
                                                dmmons.put(level+","+x+","+y+","+2,this);
                                          }
                                          //if (leftpic.equals(towardspic)) facing=SOUTH;
                                          facing = SOUTH;
                                        }
                                }
                            }
                            else canattack = true;
                        }
                        
                        int count,dir;
                        newai = currentai;
                        if (level!=dmnew.level) {
                                if (currentai!=GUARD && currentai!=FRIENDLYMOVE && currentai!=RANDOM) {
                                        //put a search for stairs here
                                        //if any within 3 away, go to
                                        currentai=RANDOM;
                                }
                        }
                        //else wasnear = true;
                        
                        while (movecounter>movespeed) { //go thru until move or attack
                           
                           switch (currentai) {
                                
                                case RANDOM:
                                  
                                  
                                  if (canattack) { //if in a square next to party
                                        randomcounter = 0;
                                        newai=defaultai; //could be GOTOWARDS or STAYBACK
                                        doAttack();
                                        break; //don't do a random move after all
                                  }
                                  else if (randomcounter>0) { //if temporarily in random (obstacle in way)
                                        randomcounter--;
                                        if (randomcounter==0) newai=defaultai;
                                  }
                                  else if (level==dmnew.level && xdist<5 && ydist<5) newai=defaultai; //if near party (within 5 squares)
                                  
                                  //chaos and sorcerer may teleport
                                  if ((number==26 || (canteleport && (!silenced || randGen.nextBoolean()))) && randGen.nextInt(20)==0 && teleport()) { moveattack=moncycle; }
                                  else {
                                          //move random direction if possible
                                          count = 0;
                                          dir=randGen.nextInt(4);
                                          while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                          if (count<4) monMove(dir);
                                          else movecounter = 0; //can't move at all
                                  }
                                  
                                  break;
                                
                                case GOTOWARDS:
                        
                                  //if hurting (health at or below 20% of full)
                                  if (health < maxhealth/5 && hasheal && !silenced && mana>59) useHealMagic(); //power depends on how powerful a mage mon is
                                  else if (fearresist>0 && hurttest==0 && health < maxhealth/5) {
                                        if (randGen.nextInt(10)<fearresist || (health<40 && maxhealth<400 && randGen.nextBoolean())) {
                                                hurt=true; newai=RUN; break;
                                        }
                                        else hurttest=12-fearresist;
                                  }
                                  else if (hurttest>0) hurttest--;
                                  if (canattack) doAttack(); //do attack or use close-range magic - have chance of setting hurt (even though not hurt) and entering run - dodge
                                  else { //cast proj, or go towards party
                                    //possibility of proj attack
                                    if ((ammo>0 || (hasmagic && !silenced && mana>=minproj)) && randGen.nextBoolean() && ((x==partyx && ydist>1) || (y==partyy && xdist>1)) ) { 
                                        if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                        else if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                        else if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH); 
                                        else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                        if (movecounter<movespeed) break; //if cast, don't move
                                    }
                                    //small chance of not moving at all
                                    if (randGen.nextInt()%10>5) {
                                        if (attackspeed>0) movecounter=attackspeed;
                                        else movecounter = 0;
                                    }
                                    //chance of waiting for party to step close (if party near) -> makes circling strategy harder
                                    else if (xdist==1 && ydist==1 && !HITANDRUN && randGen.nextBoolean()) {
                                        movecounter=movespeed;
                                        waitattack=true;
                                    }
                                    //get random boolean to determine whether to start checking x or y values
                                    else if (randGen.nextBoolean()) { //check x values first, y's if can't move in x
                                        if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else { randomcounter=3; newai=RANDOM; } //couldn't move towards - obstacle - try get around by moving a few random steps - this doesn't work that great...
                                    }
                                    else { //check y values first
                                        if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else { randomcounter=3; newai=RANDOM; } //couldn't move towards - obstacle - try get around by moving a few random steps - this doesn't work that great...
                                    }
                                  }
                                  break;
                                
                                case RUN: //move away from party (can be fixed number of steps - giggler, horn of fear?)
                                
                                  //put possibility of proj attack here
                                  //similar to above go towards party
                                  if (((canteleport && (!silenced || randGen.nextBoolean())) || (number==26 && wasstuck)) && teleport()) { wasstuck=false; moveattack=moncycle; }
                                  else if (wasstuck && canattack) { wasstuck=false; doAttack(); }
                                  else if (canattack && !wasfrightened && randGen.nextBoolean() && (!HITANDRUN || randGen.nextInt(6)==0)) { waitattack = true; doAttack(); }//chance of turning and attacking in one move
                                  else if (randGen.nextBoolean()) { //check x values first, y's if can't move in x
                                      if (x==partyx && randGen.nextBoolean()) {
                                        if (randGen.nextBoolean() && canMove(EAST)) monMove(EAST);
                                        else if (canMove(WEST)) monMove(WEST);
                                        else if (canMove(EAST)) monMove(EAST);
                                        else if (y>partyy) { if ((!wasstuck || olddir==-1 || olddir!=NORTH) && canMove(SOUTH)) monMove(SOUTH); }
                                        else if (y<partyy) { if ((!wasstuck || olddir==-1 || olddir!=SOUTH) && canMove(NORTH)) monMove(NORTH); }
                                        else { wasstuck=true; randomcounter=3; newai=RANDOM; }
                                      }
                                      else if (x>partyx) { if ((!wasstuck || olddir==-1 || olddir!=WEST) && canMove(EAST)) monMove(EAST); }
                                      else if (x<partyx) { if ((!wasstuck || olddir==-1 || olddir!=EAST) && canMove(WEST)) monMove(WEST); }
                                      else if (y>partyy) { if ((!wasstuck || olddir==-1 || olddir!=NORTH) && canMove(SOUTH)) monMove(SOUTH); }
                                      else if (y<partyy) { if ((!wasstuck || olddir==-1 || olddir!=SOUTH) && canMove(NORTH)) monMove(NORTH); }
                                      //if (movecounter>0 && (attackspeed<=0 || movecounter<attackspeed)) { //couldn't move away, try random but not toward
                                      if (movecounter>movespeed) {
                                        //System.out.println("Couldn't move away");
                                        wasstuck = true;
                                        count = 0;
                                        dir=randGen.nextInt(4);
                                        while (count<4 && ((olddir!=-1 && dir==(olddir+2)%4) || (dir==NORTH && y>partyy) || (dir==SOUTH && y<partyy) || (dir==EAST && x<partyx) || (dir==WEST && x>partyx) || !canMove(dir))) { 
                                              count++; dir=(dir+1)%4;
                                              //if (dir==NORTH && y>partyy) { count++; dir=(dir+1)%4; }
                                              //else if (dir==SOUTH && y<partyy) { count++; dir=(dir+1)%4; }
                                              //else if (dir==EAST && x<partyx) { count++; dir=(dir+1)%4; }
                                              //else if (dir==WEST && x>partyx) { count++; dir=(dir+1)%4; }
                                        }
                                        if (count<4 && randGen.nextInt(15)>0) { olddir=dir; monMove(dir); }
                                        //else if (number==26) { wasstuck=true; }
                                        else if (canattack) doAttack();
                                        //else if (randGen.nextBoolean()) {
                                        //cast proj if can, else don't move at all
                                        else if ((ammo>0 || (hasmagic && !silenced && mana>=minproj)) && ((x==partyx && ydist>1) || (y==partyy && xdist>1)) ) { 
                                                 if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                                 else if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                                 else if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH); 
                                                 else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                                 else movecounter = 0;
                                        }
                                        //else movecounter = 0;
                                        //}
                                        else {
                                              if (randGen.nextBoolean()) { olddir=-1; newai = GOTOWARDS; }
                                              else if (x>partyx && canMove(WEST)) { wasstuck=true; olddir=WEST; monMove(WEST); }
                                              else if (x<partyx && canMove(EAST)) { wasstuck=true; olddir=EAST; monMove(EAST); }
                                              else if (y>partyy && canMove(NORTH)) { wasstuck=true; olddir=NORTH; monMove(NORTH); }
                                              else if (y<partyy && canMove(SOUTH)) { wasstuck=true; olddir=SOUTH; monMove(SOUTH); }
                                              else movecounter=0;
                                              //count = 0;
                                              //dir=randGen.nextInt(4);
                                              //while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                              //if (count<4) { wasstuck=true; monMove(dir); }
                                              //else movecounter = 0;
                                        }
                                      }
                                  }
                                  else { //check y values first
                                      if (y==partyy && randGen.nextBoolean()) {
                                        if (randGen.nextBoolean() && canMove(NORTH)) monMove(NORTH);
                                        else if (canMove(SOUTH)) monMove(SOUTH);
                                        else if (canMove(NORTH)) monMove(NORTH);
                                        else if (x>partyx) { if ((!wasstuck || olddir==-1 || olddir!=WEST) && canMove(EAST)) monMove(EAST); }
                                        else if (x<partyx) { if ((!wasstuck || olddir==-1 || olddir!=EAST) && canMove(WEST)) monMove(WEST); }
                                        else { wasstuck=true; randomcounter=3; newai=RANDOM; }
                                      }
                                      else if (y>partyy) { if ((!wasstuck || olddir!=-1 || olddir!=NORTH) && canMove(SOUTH)) monMove(SOUTH); }
                                      else if (y<partyy) { if ((!wasstuck || olddir!=-1 || olddir!=SOUTH) && canMove(NORTH)) monMove(NORTH); }
                                      else if (x>partyx) { if ((!wasstuck || olddir!=-1 || olddir!=WEST) && canMove(EAST)) monMove(EAST); }
                                      else if (x<partyx) { if ((!wasstuck || olddir!=-1 || olddir!=EAST) && canMove(WEST)) monMove(WEST); }
                                      if (movecounter>movespeed) { //couldn't move away
                                        //System.out.println("Couldn't move away");
                                        wasstuck = true;
                                        count = 0;
                                        dir=randGen.nextInt(4);
                                        while (count<4 && ((olddir!=-1 && dir==(olddir+2)%4) || (dir==NORTH && y>partyy) || (dir==SOUTH && y<partyy) || (dir==EAST && x<partyx) || (dir==WEST && x>partyx) || !canMove(dir))) { 
                                        //while (!canMove(dir) && count<4) { 
                                              count++; dir=(dir+1)%4;
                                              //if (dir==NORTH && y>partyy) { count++; dir=(dir+1)%4; }
                                              //else if (dir==SOUTH && y<partyy) { count++; dir=(dir+1)%4; }
                                              //else if (dir==EAST && x<partyx) { count++; dir=(dir+1)%4; }
                                              //else if (dir==WEST && x>partyx) { count++; dir=(dir+1)%4; }
                                        }
                                        if (count<4 && randGen.nextInt(15)>0) { olddir=dir; monMove(dir); }
                                        //else if (number==26) { wasstuck=true; }
                                        else if (canattack) doAttack();
                                        //else if (randGen.nextBoolean()) {  
                                        //cast proj if can, else don't move at all
                                        else if ((ammo>0 || (hasmagic && !silenced && mana>=minproj)) && ((x==partyx && ydist>1) || (y==partyy && xdist>1)) ) { 
                                                 if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                                 else if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                                 else if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH); 
                                                 else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                                 else movecounter = 0;
                                        }
                                        //else movecounter = 0;
                                        //}
                                        else {
                                              if (randGen.nextBoolean()) { olddir=-1; newai = GOTOWARDS; }
                                              else if (x>partyx && canMove(WEST)) { wasstuck=true; olddir=WEST; monMove(WEST); }
                                              else if (x<partyx && canMove(EAST)) { wasstuck=true; olddir=EAST; monMove(EAST); }
                                              else if (y>partyy && canMove(NORTH)) { wasstuck=true; olddir=NORTH; monMove(NORTH); }
                                              else if (y<partyy && canMove(SOUTH)) { wasstuck=true; olddir=SOUTH; monMove(SOUTH); }
                                              else movecounter=0;
                                              //count = 0;
                                              //dir=randGen.nextInt()%4; if (dir<0) dir*=-1;
                                              //while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                              //if (count<4) { wasstuck=true; monMove(dir); }

                                              //else movecounter = 0;
                                        }
                                      }
                                  }
                                  if (hurt && hasheal && !silenced && mana > 59 && randGen.nextBoolean()) { useHealMagic(); }
                                  else if (hurt && health>.2*maxhealth) { hurt=false; if (runcounter==0) { olddir=-1; newai=defaultai; } }//no longer hurting, perhaps no longer running
                                  if (!hurt && runcounter>0) { runcounter--; if (runcounter==0) { olddir=-1; newai=defaultai; if (newai==GUARD) {newai=GOTOWARDS;defaultai=GOTOWARDS;} } }
                                  if (wasfrightened) wasfrightened = false;
                                  break;

                                case STAYBACK: //wizards, archers?
                                  
                                  if (health < maxhealth/5 && hasheal && !silenced && mana>59) useHealMagic(); //power depends on how powerful a mage mon is
                                  else if (fearresist>0 && hurttest==0 && health < maxhealth/5) {
                                        if (randGen.nextInt(10)<fearresist || (health<40 && maxhealth<400 && randGen.nextBoolean())) {
                                                hurt=true; newai=RUN; break;
                                        }
                                        else hurttest=12-fearresist;
                                  }
                                  else if (hurttest>0) hurttest--;
                                  if (ammo==0 && (!hasmagic || mana < minproj || silenced)) { 
                                        if (randGen.nextBoolean()) newai=GOTOWARDS; else { runcounter = 5; newai = RUN; }
                                  }
                                  else if (x==partyx) { //if lined up along x
                                        //if far enough away, attack with proj if possible - move back if too close
                                        //if (ydist>1 && randGen.nextInt()%10<6) {
                                        if ((ydist>1 || (!canattack && randGen.nextInt(3)!=0)) && randGen.nextInt()%10<6) {
                                            if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH);
                                            else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                            //else if ((subsquare==0 || subsquare==3) && canMove(EAST)) monMove(EAST);
                                            //else if ((subsquare==1 || subsquare==2) && canMove(WEST)) monMove(WEST);
                                            else if ((subsquare==0 || subsquare==3) && canMove(EAST)) { monMove(EAST); movecounter=attackspeed; }
                                            else if ((subsquare==1 || subsquare==2) && canMove(WEST)) { monMove(WEST); movecounter=attackspeed; }
                                            else { randomcounter=3; newai=RANDOM; }
                                        }
                                        else if (y>partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y<partyy && canMove(NORTH)) monMove(NORTH);
                                        else if (randGen.nextInt()%10>2) {  //will this cause fluttering back and forth?
                                          if (randGen.nextBoolean() && canMove(EAST)) monMove(EAST);
                                          else if (canMove(WEST)) monMove(WEST);
                                          else if (canMove(EAST)) monMove(EAST);
                                          else { newai = GOTOWARDS; wasstuck=true; }
                                        }
                                        else { newai = GOTOWARDS; wasstuck=true; }
                                  }
                                  else if (y==partyy) { //if lined up along y
                                        //if far enough away, attack with proj if possible - move back if too close
                                        //if (xdist>1 && randGen.nextInt()%10<6) {
                                        if ((xdist>1 || (!canattack && randGen.nextInt(3)!=0)) && randGen.nextInt()%10<6) {
                                            if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                            else if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                            //else if ((subsquare==0 || subsquare==1) && canMove(SOUTH)) monMove(SOUTH);
                                            //else if ((subsquare==2 || subsquare==3) && canMove(NORTH)) monMove(NORTH);
                                            else if ((subsquare==0 || subsquare==1) && canMove(SOUTH)) { monMove(SOUTH); movecounter=attackspeed; }
                                            else if ((subsquare==2 || subsquare==3) && canMove(NORTH)) { monMove(NORTH); movecounter=attackspeed; }
                                            else { randomcounter=3; newai=RANDOM; }
                                        }
                                        else if (x>partyx && canMove(EAST)) monMove(EAST);
                                        else if (x<partyx && canMove(WEST)) monMove(WEST);
                                        else if (randGen.nextInt()%10>2) {  //will this cause fluttering back and forth?
                                          if (randGen.nextBoolean() && canMove(NORTH)) monMove(NORTH);
                                          else if (canMove(SOUTH)) monMove(SOUTH);
                                          else if (canMove(NORTH)) monMove(NORTH);
                                          else { newai = GOTOWARDS; wasstuck=true; }
                                        }
                                        else { newai = GOTOWARDS; wasstuck=true; }
                                  }
                                  else { //not lined up yet, so move towards party
                                    //check x or y first depending on which is already closer (xdist or ydist smaller)
                                    if (xdist==ydist) {
                                        count = 0;
                                        dir=randGen.nextInt(4);
                                        while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                        if (count<4) monMove(dir);
                                        else movecounter=0; //can't move at all, can't attack, so don't try
                                    }
                                    else if (xdist<ydist) {
                                        if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else { randomcounter=3; newai=RANDOM; }
                                    }
                                    else {
                                        if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else { randomcounter=3; newai=RANDOM; }
                                    }
                                  }
                                  break;
                                  
                                case GUARD: //don't move at all, but attack and may use projs or heal spells
                                default:
                                  
                                  //make face towards party - if xdist greater than ydist, face east or west towards party
                                  //otherwise, face north or south towards party
                                  //could instead turn in place:
                                  //facing=(facing+1)%4; //turns in place
                                  
                                  if (hasheal && !silenced && (health < .2*maxhealth) && mana>59) useHealMagic();//heal self if hurting (if can)
                                  else if (level!=dmnew.level) movecounter=0;
                                  else if (canattack) doAttack(); //do attack or use close-range magic
                                  else if ((x==partyx && ydist>1) && (ammo>0 || (hasmagic && !silenced && mana>=minproj))) {
                                        if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH);
                                        else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                        else if ((subsquare==0 || subsquare==3) && canMove(EAST)) monMove(EAST);
                                        else if ((subsquare==1 || subsquare==2) && canMove(WEST)) monMove(WEST);
                                        else movecounter = 0;
                                  }
                                  else if ((y==partyy && xdist>1) && (ammo>0 || (hasmagic && !silenced && mana>=minproj))) {
                                        if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                        else if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                        else if ((subsquare==0 || subsquare==1) && canMove(SOUTH)) monMove(SOUTH);
                                        else if ((subsquare==2 || subsquare==3) && canMove(NORTH)) monMove(NORTH);
                                        else movecounter = 0;
                                  }
                                  else if (xdist==0 && ydist==1) {
                                        if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else movecounter = 0;
                                  }
                                  else if (ydist==0 && xdist==1) {
                                        if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else movecounter = 0;
                                  }
                                  else movecounter = 0;
                                  if (level==dmnew.level && xdist<5 && ydist<5) {
                                        //if close, face party
                                        if (xdist!=0 && xdist>ydist) {
                                                if (partyx>x) facing=EAST;
                                                else facing=WEST;
                                        }
                                        else {
                                                if (partyy>y) facing=SOUTH;
                                                else facing=NORTH;
                                        }
                                        if (useammo && ammo==0) { defaultai=GOTOWARDS; newai=GOTOWARDS; } //run out of ammo turns guard into gotoward
                                  }
                                  break;
                                
                                case FRIENDLYMOVE:
                                  
                                  //move random direction if possible
                                  count = 0;
                                  dir=randGen.nextInt(4);
                                  while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                  if (count<4) monMove(dir);
                                  else {
                                        movecounter = 0; //can't move at all
                                        //face party if close
                                        if (level==dmnew.level && xdist<5 && ydist<5) {
                                                if (xdist==0) {
                                                        if (y>partyy) facing=NORTH;
                                                        else facing=SOUTH;
                                                }
                                                else if (ydist==0) {
                                                        if (x>partyx) facing=WEST;
                                                        else facing=EAST;
                                                }
                                        } 
                                  }

                                  break;

                           } //end switch
                           currentai = newai;
                           //if no longer near party, reset to random ai
                           if (currentai!=GUARD && currentai!=FRIENDLYMOVE && xdist>4 && ydist>4 && (currentai!=RANDOM || randomcounter>0)) { randomcounter=0; currentai=RANDOM; }
                        } //end while
                        //if (wasstuck && currentai==GOTOWARDS && defaultai==STAYBACK) { wasstuck=false; currentai=STAYBACK; } //reset to stayback from gotowards (will be set again next time if still can't move)
                }
                
                public boolean canMove(int d) {
                        int xadjust=x,yadjust=y;
                        int newsub = 5;
                        if (d==NORTH || d==SOUTH) {
                                if (subsquare==0) newsub = 3;
                                else if (subsquare==3) newsub = 0;
                                else if (subsquare==1) newsub = 2;
                                else if (subsquare==2) newsub = 1;
                                if (d==NORTH && (newsub==3 || newsub==2 || newsub==5)) yadjust--;
                                else if (d==SOUTH && (newsub==0 || newsub==1 || newsub==5)) yadjust++;
                        }
                        else {
                                if (subsquare==0) newsub = 1;
                                else if (subsquare==1) newsub = 0;
                                else if (subsquare==2) newsub = 3;
                                else if (subsquare==3) newsub = 2;
                                if (d==EAST && (newsub==0 || newsub==3 || newsub==5)) xadjust++;
                                else if (d==WEST && (newsub==1 || newsub==2 || newsub==5)) xadjust--;
                        }
                        if (xadjust<0 || yadjust<0 || xadjust>=mapwidth || yadjust>=mapheight) return false;
                        //if (d==NORTH) yadjust--;
                        //else if (d==WEST) xadjust--;
                        //else if (d==SOUTH) yadjust++;
                        //else xadjust++;

                        boolean movetest=true;
                        if ((!DungeonMap[level][xadjust][yadjust].isPassable && !isImmaterial) || DungeonMap[level][xadjust][yadjust].hasParty || DungeonMap[level][xadjust][yadjust].numProjs>0 || DungeonMap[level][x][y].numProjs>0) movetest=false;
                        else if (DungeonMap[level][xadjust][yadjust].hasMons && (newsub==5 || dmmons.get(level+","+xadjust+","+yadjust+","+newsub)!=null || dmmons.get(level+","+xadjust+","+yadjust+","+5)!=null)) movetest = false;
                        //else if (xadjust<1 || xadjust>(mapwidth-2) || yadjust<1 || yadjust>(mapheight-2)) movetest = false;
                        //else if (xadjust<0 || yadjust<0 || xadjust==mapwidth || yadjust==mapheight) movetest = false;
                        else if (!DungeonMap[level][xadjust][yadjust].canPassMons) movetest=false;
                        else if (isImmaterial && !DungeonMap[level][xadjust][yadjust].canPassImmaterial) movetest=false;
                        //newly added:
                        //else if (!isImmaterial && !ignoremons && currentai==RUN && DungeonMap[level][xadjust][yadjust].hasCloud && randGen.nextBoolean()) movetest=false;
                        else if (!isImmaterial && !ignoremons && DungeonMap[level][xadjust][yadjust].hasCloud && randGen.nextBoolean()) movetest=false;
                        //else if (!isflying && DungeonMap[level][xadjust][yadjust] instanceof Pit && (((Pit)DungeonMap[level][xadjust][yadjust]).isOpen || (!ignoremons && ((Pit)DungeonMap[level][xadjust][yadjust]).isActive))) movetest=false;//don't step into open pit (unless can fly)
                        else if (!isflying && (DungeonMap[level][xadjust][yadjust] instanceof FulYaPit || (DungeonMap[level][xadjust][yadjust] instanceof Pit && (((Pit)DungeonMap[level][xadjust][yadjust]).isOpen || (!ignoremons && ((Pit)DungeonMap[level][xadjust][yadjust]).isActive))))) movetest=false;//don't step into open pit (unless can fly)
                        else if (!canusestairs && DungeonMap[level][xadjust][yadjust] instanceof Stairs) movetest=false;
                        //chaos and fluxcages:
                        else if (number==26 && fluxchanging && fluxcages.get(level+","+xadjust+","+yadjust)!=null) movetest=false;
                        //if smart enough, open a door that is blocking the way (only button type)
                        if (!movetest && DungeonMap[level][xadjust][yadjust] instanceof Door && !((Door)DungeonMap[level][xadjust][yadjust]).isOpen) {
                                if (!ignoremons && ((Door)DungeonMap[level][xadjust][yadjust]).opentype==Door.BUTTON) { 
                                        ((Door)DungeonMap[level][xadjust][yadjust]).activate();
                                        if (!DungeonMap[level][xadjust][yadjust].hasMons && !DungeonMap[level][xadjust][yadjust].hasParty && DungeonMap[level][xadjust][yadjust].numProjs==0 && DungeonMap[level][x][y].numProjs==0) movetest = true;
                                }
                                //or, try to break it down (only some mons) if isBreakable and is wooden (pictype==0)
                                else if (!breakdoor && (number==8 || number==13 || number==20 || number==21 || number==23 || number==25) && ((Door)DungeonMap[level][xadjust][yadjust]).isBreakable && ((Door)DungeonMap[level][xadjust][yadjust]).pictype==0) { 
                                        breakdoor = true;
                                        if (level==dmnew.level) { facing=d; isattacking = true; needredraw=true; }
                                        ((Door)DungeonMap[level][xadjust][yadjust]).breakDoor(power+randGen.nextInt(power/4+10),true,false);
                                        if (((Door)DungeonMap[level][xadjust][yadjust]).isBroken && !DungeonMap[level][xadjust][yadjust].hasMons && !DungeonMap[level][xadjust][yadjust].hasParty && DungeonMap[level][xadjust][yadjust].numProjs==0 && DungeonMap[level][x][y].numProjs==0) movetest = true;
                                }
                        }
                        return movetest;
                }

                public void monMove(int d) {
                        
                        parry=-1;
                        int xadjust=x,yadjust=y;
                        int newsub = 5;
                        if (d==NORTH || d==SOUTH) {
                                if (subsquare==0) newsub = 3;
                                else if (subsquare==3) newsub = 0;
                                else if (subsquare==1) newsub = 2;
                                else if (subsquare==2) newsub = 1;
                                if (d==NORTH && (newsub==3 || newsub==2 || newsub==5)) yadjust--;
                                else if (d==SOUTH && (newsub==0 || newsub==1 || newsub==5)) yadjust++;
                        }
                        else {
                                if (subsquare==0) newsub = 1;
                                else if (subsquare==1) newsub = 0;
                                else if (subsquare==2) newsub = 3;
                                else if (subsquare==3) newsub = 2;
                                if (d==EAST && (newsub==0 || newsub==3 || newsub==5)) xadjust++;
                                else if (d==WEST && (newsub==1 || newsub==2 || newsub==5)) xadjust--;
                        }

                        facing = d;

                        movecounter=0;

                        xdist = x-partyx; if (xdist<0) xdist*=-1;
                        ydist = y-partyy; if (ydist<0) ydist*=-1;
                        if (level==dmnew.level && xdist<6 && ydist<6) needredraw = true;
                        
                        //if just broke door, don't move into it
                        if (breakdoor) return;
                        //if just opened door, don't move into it
                        if (!isImmaterial && DungeonMap[level][xadjust][yadjust].mapchar=='d' && !DungeonMap[level][xadjust][yadjust].isPassable) return;
                        
                        int oldlevel = level, oldx = x; int oldy = y, oldsub = subsquare, oldface = facing;
                        MapObject newsquare = DungeonMap[level][xadjust][yadjust];
                        boolean trytel = false;//, imflytest = false;
                        //remove mon from current square (and subsquare)
                        dmmons.remove(level+","+x+","+y+","+subsquare);
                        //put mon on new square (could be same square but different subsquare)
                        x = xadjust; y = yadjust; subsquare=newsub;
                        dmmons.put(level+","+xadjust+","+yadjust+","+subsquare,this);
                        DungeonMap[level][xadjust][yadjust].hasMons=true;
                        //if moved to different square (not just to different subsquare), test switch step off
                        if (x!=oldx || y!=oldy) {
                                trytel = true;
                                Monster tempmon;
                                boolean montest = false;
                                int i=0;
                                while (i<6 && !montest) {
                                        tempmon = (Monster)dmmons.get(level+","+oldx+","+oldy+","+i);
                                        if (tempmon!=null) montest=true;
                                        if (i==3) i=5;
                                        else i++;
                                }
                                DungeonMap[level][oldx][oldy].hasMons=montest;
                                
                                //try switch stepping off unless this mon is flying
                                if (!isflying) {
                                        DungeonMap[level][oldx][oldy].tryFloorSwitch(MapObject.MONSTEPPINGOFF);
                                }
                        }
                        else if (DungeonMap[level][x][y].mapchar=='>') trytel = true;
                        //if still alive and haven't moved (stepping off switch could have caused death or teleport swap to happen)
                        if (!isdying && x==xadjust && y==yadjust && level==oldlevel) {
                                
                                //if moved to different square (not just to different subsquare) and new square has not changed, test switch step on
                                if (!isflying && newsquare==DungeonMap[level][xadjust][yadjust] && (x!=oldx || y!=oldy)) {
                                        //System.out.println("step on test");
                                        DungeonMap[level][x][y].tryFloorSwitch(MapObject.MONSTEPPINGON);
                                }
                
                                //test teleport unless was moved/killed by previous switches creating a teleport/pit or newsquare changed
                                if (trytel && !isdying && x==xadjust && y==yadjust && level==oldlevel && facing==oldface && newsquare==DungeonMap[level][xadjust][yadjust]) {
                                        DungeonMap[level][x][y].tryTeleport(this);
                                }
                                
                                //maybe pick up item on the ground if not dying (don't care here if we got teleported)
                                if (!isdying && pickup!=0 && DungeonMap[level][x][y].hasItems && (pickup==4 || (pickup==2 && randGen.nextBoolean()) || (pickup==1 && randGen.nextInt(4)==0) || (pickup==3 && randGen.nextInt(4)!=0))) {
                                        Item tempitem;
                                        if (subsquare<4) tempitem = DungeonMap[level][x][y].pickUpItem(subsquare);
                                        else {
                                                tempitem = (Item)DungeonMap[level][x][y].mapItems.remove(DungeonMap[level][x][y].mapItems.size()-1);
                                                if (DungeonMap[level][x][y].mapItems.isEmpty()) DungeonMap[level][x][y].hasItems=false;
                                        }
                                        if (tempitem!=null) {
                                                carrying.add(tempitem);
                                                if (useammo && ((tempitem.number>220 && (tempitem.hasthrowpic || tempitem.number==266)) || (tempitem.projtype>0 && tempitem.weight<=1.0) )) ammo++;
                                                DungeonMap[level][x][y].tryFloorSwitch(MapObject.TOOKITEM); //might activate a switch
                                        }
                                }
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<6 && ydist<6) {
                                        needredraw = true;
                                        if (attackspeed>0 && ((xdist==0 && ydist==1) || (ydist==0 && xdist==1))) movecounter = attackspeed;
                                        if (!sleeping && !footstep.equals("")) playFootStep(footstep,x,y);//footstep sound (not played when sleeping -> sounds weird because so fast)
                                }
                                if (movespeed-movecounter<2) moveattack = moncycle;//prevents double updates (since removed from dmmons and added again while iterating through it)
                        }
                }

                public void doAttack() {
                        //put close range magic here? (that is, if ever is such a thing)
                        
                        //make sure mon is facing party
                        //later change to turn only 90 deg at a time - inc or dec facing like in key listener
                        // -- may need to reset movecounter a little if turns too fast...
                        boolean noattack = false;
                        if (partyx>x && facing!=EAST) { facing=EAST; noattack=true; }
                        else if (partyx<x && facing!=WEST) { facing=WEST; noattack=true; }
                        else if (partyy>y && facing!=SOUTH) { facing=SOUTH; noattack=true; }
                        else if (partyy<y && facing!=NORTH) { facing=NORTH; noattack=true; }
                        else if (alldead) noattack=true;
                        if (waitattack && noattack && !alldead) { noattack=false; waitattack=false; }

                        if (!noattack) {
                                
                                //maybe do magic
                                if (hasmagic && !silenced && mana>=minproj && randGen.nextInt()%10>0) { 
                                        if (x<partyx && canDoProj(EAST)) { doProjAttack(EAST); return; }
                                        else if (x>partyx && canDoProj(WEST)) { doProjAttack(WEST); return; }
                                        else if (y>partyy && canDoProj(NORTH)) { doProjAttack(NORTH); return; } 
                                        else if (y<partyy && canDoProj(SOUTH)) { doProjAttack(SOUTH); return; }
                                }
                                else if (hasdrain && !silenced && mana>=minproj && health<maxhealth) {
                                        //drain life from a hero
                                        int who,whoalt,whotest1,whotest2,whotest3,whotest4;
                                        if (facing==NORTH) { whotest1=2; whotest2=3; whotest3=0; whotest4=1; }
                                        else if (facing==SOUTH) { whotest1=0; whotest2=1; whotest3=2; whotest4=3; }
                                        else if (facing==EAST) { whotest1=0; whotest2=3; whotest3=1; whotest4=2; }
                                        else { whotest1=1; whotest2=2; whotest3=0; whotest4=3; }
                                        if (randGen.nextBoolean()) {
                                                who = (whotest1+dmnew.facing)%4;
                                                whoalt = (whotest2+dmnew.facing)%4;
                                        }
                                        else {
                                                who = (whotest2+dmnew.facing)%4;
                                                whoalt = (whotest1+dmnew.facing)%4;
                                        }
                                        if (heroatsub[who]==-1) {
                                                who = whoalt;
                                                if (heroatsub[who]==-1) {
                                                        if (randGen.nextBoolean()) {
                                                                who = (whotest3+dmnew.facing)%4;
                                                                whoalt = (whotest4+dmnew.facing)%4;
                                                        }
                                                        else {
                                                                who = (whotest4+dmnew.facing)%4;
                                                                whoalt = (whotest3+dmnew.facing)%4;
                                                        }
                                                        if (heroatsub[who]==-1) who=whoalt;
                                                }
                                        }
                                        who = heroatsub[who];
                                        int spellcost;
                                        int spellpower = castpower+1;
                                        do {
                                                spellpower--;
                                                spellcost = spellpower;
                                                for (int i=0;i<3;i++) {
                                                    spellcost+=spellpower*6*i;
                                                }
                                        }
                                        while (spellcost>mana && spellpower>=0);
                                        try { 
                                                Spell s = new Spell(spellpower+"666");
                                                s.powers[s.gain-1]+= randGen.nextInt()%10+(s.gain-1)*manapower/8;
                                                s.power = s.powers[s.gain-1];
                                                if (s.power<1) s.power=randGen.nextInt(4)+1;
                                                int hit = hero[who].damage(s.power,DRAINHIT);
                                                //System.out.println("drain life: old health = "+health+", new = "+(health+hit));
                                                if (hit>0) { heal(hit); message.setMessage(name+" drains "+hit+" life from "+hero[who].name,5); }
                                                else if (hit!=0) damage(hit,SPELLHIT);
                                                //put drain sound here instead - if no sound, put message instead
                                                //if (soundstring!=null) playSound(soundstring,x,y);
                                                playSound("drain.wav",x,y);
                                                iscasting=true;
                                                movecounter = attackspeed;
                                                needredraw = true;
                                                playSound("oof.wav",-1,-1);
                                                return;
                                        }
                                        catch(Exception e) {}
                                }
                                if (soundstring!=null) playSound(soundstring,x,y);
                                isattacking = true;
                                movecounter = attackspeed; //put this here, or after if {}
                        }
                        else { movecounter = movespeed-2; if (movecounter<0) movecounter=0; }
                        needredraw = true;
                }
                

                public boolean canDoProj(int d) {
                        //check map between mon and party to see if can pass projs (that way know if can fire proj at party)
                        //also check if party in range if throwing items
                        int testval,tempsub1=0,tempsub2=1;
                        boolean cancast = true, subtried=false, hashero1 = true, hashero2 = true;
                        switch (d) {
                            
                            case NORTH:
                                
                                if (!hasmagic || mana<minproj) {
                                        if (ydist>4 && ydist*2>((power+5)/8+1)) return false;
                                }
                                else if (ydist>4 && ydist*2>(castpower*5+3)) return false;
                                testval = y;
                                //if (!ignoremons) {
                                     castsub = 0;
                                     tempsub1 = 0;
                                     tempsub2 = 3;
                                     if (subsquare>0 && subsquare<3) { tempsub1 = 1; tempsub2 = 2; }
                                     if (heroatsub[(tempsub1+dmnew.facing)%4]==-1 && heroatsub[(tempsub2+dmnew.facing)%4]==-1) { cancast = false; hashero1=false; }
                                     if (subsquare==5) { 
                                        hashero2 = (heroatsub[(1+dmnew.facing)%4]!=-1 || heroatsub[(2+dmnew.facing)%4]!=-1);
                                        if (hashero2 && (!cancast || randGen.nextBoolean())) { tempsub1 = 1; tempsub2 = 2; cancast = true; castsub = 1; }
                                     }
                                     if (!cancast) return false;
                                //}
                                if (subsquare==0 || subsquare==1 || subsquare==5 || ignoremons) testval--;
                                Monster tempmon;
                                while(cancast && testval>partyy) {
                                     if (!DungeonMap[level][x][testval].canPassProjs) cancast=false;
                                     else if (!ignoremons) {
                                        tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+5);
                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || 5!=subsquare)) cancast = false;
                                        else {
                                                tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+tempsub1);
                                                if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || tempsub1!=subsquare)) cancast = false;
                                                else {
                                                        tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+tempsub2);
                                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || tempsub2!=subsquare)) cancast = false;
                                                }
                                        }
                                        //if (dmmons.get(level+","+x+","+testval+","+tempsub1)!=null && (testval!=y || tempsub1!=subsquare)) cancast=false;
                                        //else if (dmmons.get(level+","+x+","+testval+","+tempsub2)!=null && (testval!=y || tempsub2!=subsquare)) cancast=false;
                                        if (!cancast && subsquare==5 && !subtried) {
                                                if (castsub==0 && hashero2) { cancast = true; castsub=1; tempsub1 = 1; tempsub2 = 2; }
                                                else if (hashero1) { cancast = true; castsub=0; tempsub1 = 0; tempsub2 = 3; }
                                                subtried=true; testval = y;
                                        }
                                     }
                                     testval--;
                                }
                                break;
                            
                            case SOUTH:
                            
                                if (!hasmagic || mana<minproj) {
                                        if (ydist>4 && ydist*2>((power+5)/8+1)) return false;
                                }
                                else if (ydist>4 && ydist*2>(castpower*5+3)) return false;
                                testval = y;
                                //if (!ignoremons) {
                                     castsub = 3;
                                     tempsub1 = 0;
                                     tempsub2 = 3;
                                     if (subsquare>0 && subsquare<3) { tempsub1 = 1; tempsub2 = 2; }
                                     if (heroatsub[(tempsub1+dmnew.facing)%4]==-1 && heroatsub[(tempsub2+dmnew.facing)%4]==-1) { cancast = false; hashero1=false; }
                                     if (subsquare==5) { 
                                        hashero2 = (heroatsub[(1+dmnew.facing)%4]!=-1 || heroatsub[(2+dmnew.facing)%4]!=-1);
                                        if (hashero2 && (!cancast || randGen.nextBoolean())) { tempsub1 = 1; tempsub2 = 2; cancast = true; castsub = 2; }
                                     }
                                     if (!cancast) return false;
                                //}
                                if (subsquare==2 || subsquare==3 || subsquare==5 || ignoremons) testval++;
                                while(cancast && testval<partyy) {
                                     if (!DungeonMap[level][x][testval].canPassProjs) cancast=false;
                                     else if (!ignoremons) {
                                        tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+5);
                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || 5!=subsquare)) cancast = false;
                                        else {
                                                tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+tempsub1);
                                                if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || tempsub1!=subsquare)) cancast = false;
                                                else {
                                                        tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+tempsub2);
                                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || tempsub2!=subsquare)) cancast = false;
                                                }
                                        }
                                        //if (dmmons.get(level+","+x+","+testval+","+tempsub1)!=null && (testval!=y || tempsub1!=subsquare)) cancast=false;
                                        //else if (dmmons.get(level+","+x+","+testval+","+tempsub2)!=null && (testval!=y || tempsub2!=subsquare)) cancast=false;
                                        if (!cancast && subsquare==5 && !subtried) {
                                                if (castsub==3 && hashero2) { cancast = true; castsub=2; tempsub1 = 1; tempsub2 = 2; }
                                                else if (hashero1) { cancast = true; castsub=3; tempsub1 = 0; tempsub2 = 3; }
                                                subtried=true; testval = y;
                                        }
                                     }
                                     testval++;
                                }
                                break;
                                
                            case EAST:
                                
                                if (!hasmagic || mana<minproj) {
                                        if (xdist>4 && xdist*2>((power+5)/8+1)) return false;
                                }
                                else if (xdist>4 && xdist*2>(castpower*5+3)) return false;
                                testval = x;
                                //if (!ignoremons) {
                                     castsub = 1;
                                     tempsub1 = 0;
                                     tempsub2 = 1;
                                     if (subsquare>1 && subsquare!=5) { tempsub1 = 2; tempsub2 = 3; }
                                     if (heroatsub[(tempsub1+dmnew.facing)%4]==-1 && heroatsub[(tempsub2+dmnew.facing)%4]==-1) { cancast = false; hashero1=false; }
                                     if (subsquare==5) { 
                                        hashero2 = (heroatsub[(2+dmnew.facing)%4]!=-1 || heroatsub[(3+dmnew.facing)%4]!=-1);
                                        if (hashero2 && (!cancast || randGen.nextBoolean())) { cancast = true; castsub = 2; tempsub1 = 2; tempsub2 = 3; }
                                     }
                                     if (!cancast) return false;
                                //}
                                if (subsquare==1 || subsquare==2 || subsquare==5 || ignoremons) testval++;
                                while(cancast && testval<partyx) {
                                     if (!DungeonMap[level][testval][y].canPassProjs) cancast=false;
                                     else if (!ignoremons) {
                                        tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+5);
                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || 5!=subsquare)) cancast = false;
                                        else {
                                                tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+tempsub1);
                                                if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || tempsub1!=subsquare)) cancast = false;
                                                else {
                                                        tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+tempsub2);
                                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || tempsub2!=subsquare)) cancast = false;
                                                }
                                        }
                                        //if (dmmons.get(level+","+testval+","+y+","+tempsub1)!=null && (testval!=x || tempsub1!=subsquare)) cancast=false;
                                        //else if (dmmons.get(level+","+testval+","+y+","+tempsub2)!=null && (testval!=x || tempsub2!=subsquare)) cancast=false;
                                        if (!cancast && subsquare==5 && !subtried) {
                                                if (castsub==1 && hashero2) { cancast = true; castsub=2; tempsub1 = 2; tempsub2 = 3; }
                                                else if (hashero1) { cancast = true; castsub=1; tempsub1 = 0; tempsub2 = 1; }
                                                subtried=true; testval = x;
                                        }
                                     }
                                     testval++;
                                }
                                break;
                                
                            case WEST:
                            
                                if (!hasmagic || mana<minproj) {
                                        if (xdist>4 && xdist*2>((power+5)/8+1)) return false;
                                }
                                else if (xdist>4 && xdist*2>(castpower*5+3)) return false;
                                testval = x;
                                //if (!ignoremons) {
                                     castsub = 0;
                                     tempsub1 = 0;
                                     tempsub2 = 1;
                                     if (subsquare>1 && subsquare!=5) { tempsub1 = 2; tempsub2 = 3; }
                                     if (heroatsub[(tempsub1+dmnew.facing)%4]==-1 && heroatsub[(tempsub2+dmnew.facing)%4]==-1) { cancast = false; hashero1=false; }
                                     if (subsquare==5) { 
                                        hashero2 = (heroatsub[(2+dmnew.facing)%4]!=-1 || heroatsub[(3+dmnew.facing)%4]!=-1);
                                        if (hashero2 && (!cancast || randGen.nextBoolean())) { cancast = true; castsub = 3; tempsub1 = 2; tempsub2 = 3; }
                                     }
                                     if (!cancast) return false;
                                //}
                                if (subsquare==0 || subsquare==3 || subsquare==5 || ignoremons) testval--;
                                while(cancast && testval>partyx) {
                                     if (!DungeonMap[level][testval][y].canPassProjs) cancast=false;
                                     else if (!ignoremons) {
                                        tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+5);
                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || 5!=subsquare)) cancast = false;
                                        else {
                                                tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+tempsub1);
                                                if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || tempsub1!=subsquare)) cancast = false;
                                                else {
                                                        tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+tempsub2);
                                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || tempsub2!=subsquare)) cancast = false;
                                                }
                                        }
                                        //if (dmmons.get(level+","+testval+","+y+","+tempsub1)!=null && (testval!=x || tempsub1!=subsquare)) cancast=false;
                                        //else if (dmmons.get(level+","+testval+","+y+","+tempsub2)!=null && (testval!=x || tempsub2!=subsquare)) cancast=false;
                                        if (!cancast && subsquare==5 && !subtried) {
                                                if (castsub==0 && hashero2) { cancast = true; castsub=3; tempsub1 = 2; tempsub2 = 3; }
                                                else if (hashero1) { cancast = true; castsub=0; tempsub1 = 0; tempsub2 = 1; }
                                                subtried=true; testval = x;
                                        }
                                     }
                                     testval--;
                                }
                                break;
                                
                        }//end switch
                        //System.out.println("sub = "+subsquare+", cancast "+d+" = "+cancast+", castsub = "+castsub);
                        return cancast;
                }
                
                //magic and projectile weapons (bows, daggers, etc)
                public void doProjAttack(int dir) {
                        Projectile p;
                        int projsub = (subsquare+dmnew.facing)%4;
                        if (subsquare==5) {
                                projsub = (castsub+dmnew.facing)%4;
                        }
                        if (hasmagic && !silenced && mana>=minproj && (ammo==0 || randGen.nextBoolean())) {
                           boolean foundspell = false;
                           int spellcost=0, spellpower=0;
                           int spellpick = randGen.nextInt(numspells);
                           while (!foundspell) { 
                                   spellpower=castpower;
                                   do {
                                        spellcost = spellpower;
                                        for (int i=0;i<knownspells[spellpick].length();i++) {
                                            //spellcost+=spellpower*Integer.parseInt(knownspells[spellpick].substring(i,i+1));
                                            //spellcost+=spellpower*Integer.parseInt(knownspells[spellpick].substring(i,i+1))*i;
                                            spellcost+=SYMBOLCOST[Integer.parseInt(knownspells[spellpick].substring(i,i+1))-1+i*6][spellpower-1];
                                        }
                                        //System.out.println("number = "+knownspells[spellpick]+", power = "+spellpower+", cost = "+spellcost);
                                        spellpower--;
                                        if (number==25) spellcost=0; //dragon breathes fire, doesn't pay to cast it
                                   }
                                   while (spellcost>mana && spellpower>0);
                                   if (spellcost<=mana) foundspell = true;
                                   else spellpick=(spellpick+1)%numspells;
                           }
                           //if (MONSTERSOUNDS[number]!=null) playSound(MONSTERSOUNDS[number],x,y);
                           if (soundstring!=null) playSound(soundstring,x,y);
                           Spell tempspell;
                           try { 
                               tempspell = new Spell((spellpower+1)+knownspells[spellpick]);
                               if (tempspell.number!=461 && tempspell.number!=363 && tempspell.number!=362 && tempspell.number!=664 && tempspell.number!=523) {
                                       for (int j=tempspell.gain-1;j>=0;j--) {
                                           tempspell.powers[j]+= randGen.nextInt()%10+j*manapower/8;
                                           if (tempspell.powers[j]<1) tempspell.powers[j]=randGen.nextInt(4)+1;
                                       }
                                       tempspell.power = tempspell.powers[tempspell.gain-1];
                               }
                               p = new Projectile(tempspell,x,y,tempspell.dist,dir,projsub);
                               mana-=spellcost;
                           }
                           catch(Exception e) { e.printStackTrace(); movecounter=attackspeed; }
                        }
                        else {
                           if (soundstring!=null) playSound(soundstring,x,y);
                           Item wep = null;
                           int index = 0;
                           boolean found = false;
                           while (!found) {
                                wep = (Item)carrying.get(index);
                                if (wep.type==Item.WEAPON && ((wep.number>220 && (wep.hasthrowpic || wep.number==266)) || (wep.projtype>0 && wep.weight<=1.0))) {
                                        carrying.remove(index);
                                        found=true;
                                }
                                else index++;
                           }
                           ammo--;
                           wep.shotpow = castpower;
                           int dist = (power+5)/5-(int)(wep.weight/2.0f)+randGen.nextInt()%2;
                           if (dist<8) dist = 8;
                           if (wep.isbomb) {
                                try { 
                                    Spell tempspell = new Spell(wep.bombnum);
                                    tempspell.power = wep.potionpow + randGen.nextInt()%10;
                                    p = new Projectile(tempspell,x,y,dist,dir,projsub);
                                }
                                catch(Exception e) { p = new Projectile(wep,x,y,dist,dir,projsub); }
                           }
                           else p = new Projectile(wep,x,y,dist,dir,projsub);
                        }
                        
                        //could change this to turn 90 deg at a time, in doattack too
                        //boolean noattack = false;
                        if (partyx>x && facing!=EAST) { facing=EAST; }//noattack=true; }
                        else if (partyx<x && facing!=WEST) { facing=WEST; }//noattack=true; }
                        else if (partyy>y && facing!=SOUTH) { facing=SOUTH; }//noattack=true; }
                        else if (partyy<y && facing!=NORTH) { facing=NORTH; }//noattack=true; }
                        
                        //if (!noattack) {
                           iscasting = true;
                           needredraw = true;
                           movecounter = attackspeed;
                           //System.out.println(name+" casts a proj, ai = "+currentai);
                        //}
                        //else movecounter = 0;
                }
                
                public void useHealMagic() {
                        //playSound("moncast.wav",x,y);
                        int spellpower = mana/60;
                        if (spellpower>castpower) spellpower=castpower;
                        heal(spellpower*(randGen.nextInt(manapower/2)+manapower/2));
                        energize(spellpower*-60);
                        iscasting = true;
                        if (level==dmnew.level) needredraw = true;
                        movecounter = attackspeed;
                        //System.out.println(name+" casts a heal spell -> health = "+health);
                }
                
                public boolean teleport() {
                        //range 5 squares on all sides of mon
                        int newx,newy,xchng,ychng,count=121;
                        /*
                        //where go depends on facing
                        if (facing==0) { newx=x-5; newy=y-5; xchng=1; ychng=1; }
                        else if (facing==2) { newx=x+5; newy=y+5; xchng=-1; ychng=-1; }
                        else if (facing==1) { newx=x-5; newy=y+5; xchng=1; ychng=-1; }
                        else { newx=x+5; newy=y-5; xchng=-1; ychng=1; }
                        */
                        //where go random
                        int way = randGen.nextInt(4);
                        if (way==0) { newx=x-5; newy=y-5; xchng=1; ychng=1; }
                        else if (way==2) { newx=x+5; newy=y+5; xchng=-1; ychng=-1; }
                        else if (way==1) { newx=x-5; newy=y+5; xchng=1; ychng=-1; }
                        else { newx=x+5; newy=y-5; xchng=-1; ychng=1; }
                        //find a square that is safe to teleport to
                        do {
                                newx+=xchng;
                                if (newx>x+5) {
                                     newx=x-5;
                                     newy+=ychng;
                                }
                                else if (newx<x-5) {
                                     newx=x+5;
                                     newy+=ychng;
                                }
                                count--;
                        }
                        //while (count>0 && (newx<0 || newy<0 || newx>=mapwidth || newy>=mapheight || !DungeonMap[level][newx][newy].isPassable || DungeonMap[level][newx][newy].hasMons || DungeonMap[level][newx][newy].hasParty || !DungeonMap[level][newx][newy].canPassMons || DungeonMap[level][newx][newy].mapchar=='>' || (newx==x && newy==y) || (number==26 && fluxcages.get(level+","+newx+","+newy)!=null)));
                        while (count>0 && (newx<0 || newy<0 || newx>=mapwidth || newy>=mapheight || DungeonMap[level][newx][newy].hasMons || DungeonMap[level][newx][newy].hasParty || !DungeonMap[level][newx][newy].canPassMons || DungeonMap[level][newx][newy].mapchar=='>' || (DungeonMap[level][newx][newy].mapchar=='y' && !isflying) || (!isImmaterial && !DungeonMap[level][newx][newy].isPassable) || (isImmaterial && !DungeonMap[level][newx][newy].canPassImmaterial) || (!isflying && DungeonMap[level][newx][newy].mapchar=='p' && ((Pit)DungeonMap[level][newx][newy]).isOpen) || (newx==x && newy==y) || (number==26 && fluxcages.get(level+","+newx+","+newy)!=null)));
                        if (count!=0) {
                                //successfully teleported
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5) {
                                        needredraw = true;
                                }
                                int oldx = x, oldy = y, oldlevel = level, oldfacing = facing;
                                MapObject newsquare = DungeonMap[level][newx][newy];
                                //remove mon from old square
                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                Monster tempmon;
                                boolean montest = false;
                                int i=0;
                                while (i<6 && !montest) {
                                        tempmon = (Monster)dmmons.get(level+","+x+","+y+","+i);
                                        if (tempmon!=null) montest=true;
                                        if (i==3) i=5;
                                        else i++;
                                }
                                DungeonMap[level][x][y].hasMons=montest;
                                //put mon on new square
                                x = newx; y = newy;
                                dmmons.put(level+","+x+","+y+","+subsquare,this);
                                DungeonMap[level][x][y].hasMons=true;
                                //try switch stepping off unless this mon is flying
                                if (!isflying) {
                                        DungeonMap[level][oldx][oldy].tryFloorSwitch(MapObject.MONSTEPPINGOFF);
                                }
                                //if still alive and haven't moved (stepping off switch could have caused death or teleport swap to happen) and new square hasn't changed
                                if (!isdying && x==newx && y==newy && level==oldlevel && newsquare==DungeonMap[level][newx][newy]) {
                                        
                                        //try switch stepping on
                                        if (!isflying) {
                                                DungeonMap[level][x][y].tryFloorSwitch(MapObject.MONSTEPPINGON);
                                        }
                        
                                        //test teleport unless was moved/killed by previous switches creating a teleport/pit or newsquare changed
                                        if (!isdying && x==newx && y==newy && level==oldlevel && facing==oldfacing && newsquare==DungeonMap[level][newx][newy]) {
                                                DungeonMap[level][x][y].tryTeleport(this);
                                        }
                                }
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5) {
                                        needredraw = true;
                                }
                                movecounter = 0;
                                //System.out.println("Teleport to "+level+","+newx+","+newy+" which has mapchar "+DungeonMap[level][newx][newy].mapchar);
                                return true;
                                /*
                                if (!isflying && !imflytest) DungeonMap[level][x][y].tryFloorSwitch(MapObject.MONSTEPPINGOFF);
                                if (!isflying) DungeonMap[level][newx][newy].tryFloorSwitch(MapObject.MONSTEPPINGON);
                                x=newx;
                                y=newy;
                                dmmons.put(level+","+x+","+y+","+subsquare,this);
                                DungeonMap[level][x][y].hasMons=true;
                                
                                //test for redraw before possible teleport (by a teleport, that is) and after
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5) {
                                        needredraw = true;
                                }
                                DungeonMap[level][x][y].tryTeleport(this);
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5) {
                                        needredraw = true;
                                }
                                movecounter = 0;
                                //System.out.println("Teleport to "+level+","+newx+","+newy+" which has mapchar "+DungeonMap[level][newx][newy].mapchar);
                                return true;
                                */
                        }
                        //System.out.println("Can't Teleport");
                        return false;
                }

                public void heal(int h) {
                        health+=h;
                        if (health>maxhealth) health=maxhealth;
                }

                public void energize(int e) {
                        mana+=e;
                        if (mana>maxmana) mana=maxmana;
                        else if (mana<0) mana=0;
                }

                public int damage(int hit,int type) {
                        if (isdying || currentai>=FRIENDLYMOVE) return 0;
                        //check if need special item before can hurt
                        if (needitem!=0 && type!=STORMHIT && type!=FUSEHIT) {
                                boolean found = false;
                                if (needhandneck==0) {
                                        //need in someone's hand
                                        for (int i=0;i<numheroes;i++) {
                                                if (hero[i].weapon.number==needitem || (hero[i].hand!=null && hero[i].hand.number==needitem)) found=true;
                                                //firestaff+ works for firestaff (but not other way around)
                                                else if (needitem==248 && hero[i].weapon.number==249 || (hero[i].hand!=null && hero[i].hand.number==249)) found=true;
                                                //stormbringer overrides
                                                else if (hero[i].weapon.number==215) found=true;
                                        }
                                }
                                else {
                                        //need around someone's neck
                                        for (int i=0;i<numheroes;i++) {
                                                if (hero[i].neck!=null && hero[i].neck.number==needitem) found=true;
                                        }
                                        //stormbringer overrides
                                        if (!found) for (int i=0;i<numheroes;i++) {
                                                if (hero[i].weapon.number==215) found=true;
                                        }
                                }
                                if (!found) { 
                                        if (type!=POISONHIT && type!=DOORHIT && level==dmnew.level) message.setMessage(name+" is unaffected.",4);
                                        return 0;
                                }
                        }
                        if (hurtitem!=0 && type!=WEAPONHIT && type!=STORMHIT && type!=FUSEHIT && type!=PROJWEAPONHIT) { 
                                if (type!=POISONHIT && type!=DOORHIT && level==dmnew.level) message.setMessage(name+" is unaffected.",4);
                                return 0;
                        }
                        if (type==WEAPONHIT || type==PROJWEAPONHIT || type==STORMHIT) hit-=(hit*defense/100);
                        else if (type==SPELLHIT || type==DRAINHIT) hit-=(hit*magicresist/100);
                        if (hit<1) hit=randGen.nextInt(4)+1;
                        else if (DIFFICULTY<0) hit = (int)(((double)hit)*(1.0-((double)DIFFICULTY)*.25));
                        if (type==DRAINHIT && (isImmaterial || number==8 || number==21)) {
                                //draining from immaterial mon, skeleton, or deth knight hurts you and heals them
                                health+=hit;
                                if (level==dmnew.level) message.setMessage(name+" gains some life.",5);
                                return 0-hit;
                        }
                        else {
                                health-=hit;
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5 && type!=POISONHIT && type!=DOORHIT && type!=FUSEHIT) message.setMessage(name+" takes "+hit+" damage.",5);
                        }
                        //if (xdist<5 && ydist<5) {
                                //if (type!=POISONHIT) message.setMessage(name+" takes "+hit+" damage.",5);
                                //else message.setMessage(name+" takes "+hit+" poison damage.",5);
                        //}
                        if (health<1) {
                                isdying = true;
                                if (gamewin) {
                                        nomovement=true;
                                        isdying = false;
                                        if (type==FUSEHIT) {
                                                dmnew.endanim="fuseend.gif";
                                                //dmnew.endmusic="fuseend.mid";
                                                dmnew.endsound="fuseend.wav";
                                        }
                                        else {
                                                dmnew.endanim=endanim;
                                                //dmnew.endmusic=endmusic;
                                                dmnew.endsound=endsound;
                                        }
                                        gameover=true;
                                        return hit;
                                }
                                boolean bigthunk = false;
                                Item it;
                                if (equipped!=null) {
                                        for (Iterator i=equipped.iterator();i.hasNext();) {
                                              it = (Item)i.next();
                                              if (it.type!=Item.WEAPON || it.weight>4.0f) bigthunk = true;
                                              if (subsquare!=5) it.subsquare = subsquare;
                                              else it.subsquare=randGen.nextInt(4);
                                              if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                                DungeonMap[level][x][y].addItem(it);
                                                DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                                              }
                                        }
                                }
                                for (Iterator i=carrying.iterator();i.hasNext();) {
                                      it = (Item)i.next();
                                      if (it.type!=Item.WEAPON || it.weight>4.0f) bigthunk = true;
                                      if (subsquare!=5) it.subsquare = subsquare;
                                      else it.subsquare=randGen.nextInt(4);
                                      if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                        DungeonMap[level][x][y].addItem(it);
                                        DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                                      }
                                }
                                if (level==dmnew.level && (!carrying.isEmpty() || (equipped!=null && !equipped.isEmpty()))) {
                                        if (bigthunk) playSound("thunk.wav",x,y);
                                        else playSound("dink.wav",x,y);
                                }
                                if (level==dmnew.level && xdist<5 && ydist<5) needredraw = true;
                                else {
                                  dmmons.remove(level+","+x+","+y+","+subsquare);
                                  boolean montest = false;
                                  if (subsquare!=5) for (int sub=0;sub<3;sub++) if (dmmons.get(level+","+x+","+y+","+sub)!=null) montest=true;
                                  DungeonMap[level][x][y].hasMons=montest;
                                }
                                if (!isflying) DungeonMap[level][x][y].tryFloorSwitch(MapObject.MONSTEPPINGOFF);
                        }
                        else { 
                                if (type==DOORHIT && currentai!=RUN && !isImmaterial && randGen.nextBoolean() && (health<80 || randGen.nextInt(20)==0)) { 
                                        runcounter=2; currentai=RUN;
                                }
                                //if in poisoncloud, move unless already running or are immaterial (**currently move guard types**)
                                else if (fearresist>0 && currentai!=RUN && !isImmaterial && health<600 && DungeonMap[level][x][y].hasCloud && currentai!=FRIENDLYMOVE && currentai!=FRIENDLYNOMOVE && randGen.nextBoolean()) {
                                        boolean willrun = false;
                                        if (randGen.nextInt(10)<fearresist || (fearresist>0 && health<40 && maxhealth<400 && randGen.nextBoolean())) {
                                                PoisonCloud tempcloud;
                                                Iterator i=cloudstochange.iterator();
                                                while (i.hasNext()) {
                                                        tempcloud=(PoisonCloud)i.next();
                                                        if (tempcloud.x==x && tempcloud.y==y) {
                                                                if (health<tempcloud.stage*100) willrun = true;
                                                                break;
                                                        }
                                                }
                                        }
                                        if (willrun) { runcounter=2; currentai=RUN; }
                                }
                        }
                        return hit;
                }
                
                public void pitDeath() {
                        isdying = true;
                        if (gamewin) {
                                nomovement=true;
                                dmnew.endanim=endanim;
                                //dmnew.endmusic=endmusic;
                                dmnew.endsound=endsound;
                                gameover=true;
                                return;
                        }
                        boolean bigthunk = false;
                        Item it;
                        if (equipped!=null) {
                                for (Iterator i=equipped.iterator();i.hasNext();) {
                                      it = (Item)i.next();
                                      if (it.type!=Item.WEAPON || it.weight>4.0f) bigthunk = true;
                                      if (subsquare!=5) it.subsquare = subsquare;
                                      else it.subsquare=randGen.nextInt(4);
                                      if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                        DungeonMap[level][x][y].addItem(it);
                                        DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                                      }
                                }
                        }
                        for (Iterator i=carrying.iterator();i.hasNext();) {
                              it = (Item)i.next();
                              if (it.type!=Item.WEAPON || it.weight>4.0f) bigthunk = true;
                              if (subsquare!=5) it.subsquare = subsquare;
                              else it.subsquare=randGen.nextInt(4);
                              if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                DungeonMap[level][x][y].addItem(it);
                                DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                              }
                        }
                        if (level==dmnew.level && (!carrying.isEmpty() || (equipped!=null && !equipped.isEmpty()))) {
                                if (bigthunk) playSound("thunk.wav",x,y);
                                else playSound("dink.wav",x,y);
                        }
                        if (level==dmnew.level && xdist<5 && ydist<5) needredraw = true;
                }

        }