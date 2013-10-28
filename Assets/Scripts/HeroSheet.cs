using UnityEngine;
using System.Collections;

class HeroSheet extends JPanel {//JComponent { //Canvas {
                Hero hero;
                Image offscreen;
                Graphics2D offg,curseg;
                //BufferedImage offscreen;
                //Graphics2D offg;
                MirrorClick mirrorclick;
                boolean mirror = false;
                boolean stats = false;
                boolean skipchestscroll = false;
                boolean mirrorflag = false;
                boolean showresreinc = true;

                public HeroSheet() {
                        setBackground(Color.black);
                        /*setDoubleBuffered(true);*/
                }
                public HeroSheet(Hero h) {
                        hero = h;
                        setSize(448,326);
                        //setPreferredSize(new Dimension(448,326));
                        setBackground(Color.black);
                        /*setDoubleBuffered(true);*/
                }
                
                public void setHero(Hero h) {
                        hero = h;
                        repaint();
                        hero.repaint();
                }
                public void setHero(Hero h,boolean f) {
                        removeMouseListener(sheetclick);
                        hero = h;
                        mirror = true;
                        repaint();
                        hero.repaint();
                        mirrorflag = f;
                        if (mirrorclick==null) mirrorclick = new MirrorClick();
                        addMouseListener(mirrorclick);
                }

                public void paintComponent(Graphics g) {
                        if (sleeping) { sleepsheet(); g.drawImage(offscreen,0,0,this.size().width,this.size().height,null); return; }
                        else if (stats) { showStats(); g.drawImage(offscreen,0,0,this.size().width,this.size().height,null); return; }
                        else if (viewing) { showItem(); g.drawImage(offscreen,0,0,this.size().width,this.size().height,null); return; }
                        
                        //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                        offg.setFont(dungfont14);
                        offg.drawImage(hsheet,0,0,null);

                        offg.setColor(new Color(30,30,30));
                        offg.drawString(hero.name+"   "+hero.lastname,8,18);
                        offg.drawString("Health",18,288);
                        offg.drawString((hero.health+hero.berserkhealth)+" / "+hero.maxhealth,128,288);//was 290
                        offg.drawString("Stamina",18,306);
                        offg.drawString(hero.stamina+" / "+hero.maxstamina,128,306);//was 308
                        offg.drawString("Mana",18,324);
                        offg.drawString(hero.mana+" / "+hero.maxmana,128,324);

                        if (leader==hero.heronumber) offg.setColor(Color.yellow);
                        else offg.setColor(Color.white);
                        offg.drawString(hero.name+"   "+hero.lastname,5,15);
                        if (leader==hero.heronumber) offg.setColor(Color.white);
                        offg.drawString("Health",15,285);//was 252
                        offg.drawString("Stamina",15,303);//was 270
                        offg.drawString("Mana",15,321);//was 288

                        if ((hero.health+hero.berserkhealth)<(hero.maxhealth/3)) offg.setColor(Color.red);
                        else if (hero.berserkhealth>0) offg.setColor(Color.green);
                        else offg.setColor(Color.white);
                        offg.drawString((hero.health+hero.berserkhealth)+" / "+hero.maxhealth,125,285);

                        if (hero.stamina<(hero.maxstamina/3)) offg.setColor(Color.red);
                        else offg.setColor(Color.white);

                        offg.drawString(hero.stamina+" / "+hero.maxstamina,125,303);

                        if (hero.mana<(hero.maxmana/3)) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawString(hero.mana+" / "+hero.maxmana,125,321);
                        
                        offg.setColor(new Color(30,30,30));
                        offg.drawString("Load       "+((float)((int)(hero.load*10.0f+.5f)))/10.0f+" / "+((float)((int)(hero.maxload*10.0f+.5f)))/10.0f,263,324);
                        offg.drawString("Kg",423,324);
                        if (hero.load>hero.maxload) offg.setColor(Color.red);
                        else if (hero.load>hero.maxload*3/4) offg.setColor(Color.yellow);
                        else offg.setColor(Color.white);
                        offg.drawString("Load       "+((float)((int)(hero.load*10.0f+.5f)))/10.0f+" / "+((float)((int)(hero.maxload*10.0f+.5f)))/10.0f,260,321);
                        offg.drawString("Kg",420,321);
                        
                        if (hero.ispoisoned) {
                           offg.setColor(new Color(0,150,0));
                           offg.setStroke(new BasicStroke(2.0f));
                           //offg.drawRect(110,24,35,35);
                           offg.drawRect(106,22,40,37);
                           //draw poisoned pic here
                           offg.drawImage(poisonedpic,224,209,null);
                        }
                        if (hero.silenced) {
                           //offg.drawImage(silencedpic,224,209,null);
                           //offg.setFont(new Font("TimesRoman",Font.BOLD,24));
                           offg.setFont(dungfont.deriveFont(24.0f));
                           offg.setColor(new Color(30,30,30));
                           offg.drawString("SILENCED",264,284);
                           offg.setColor(Color.red);
                           offg.drawString("SILENCED",260,280);
                           //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                           offg.setFont(dungfont14);
                        }
                        boolean drewred = false;
                        if (hero.strengthboost<0 || hero.dexterityboost<0 || hero.vitalityboost<0 || hero.intelligenceboost<0 || hero.wisdomboost<0 || hero.defenseboost<0 || hero.magicresistboost<0) {
                           offg.setColor(Color.red);
                           offg.setStroke(new BasicStroke(2.0f));
                           //offg.drawRect(22,24,35,35);
                           offg.drawRect(18,22,40,37);
                           drewred = true;
                        }
                        if (hero.detectcurse>0) {
                           //outline eye in blue if detecting curses (purple if also have stat problem)
                           if (drewred) offg.setColor(new Color(150,0,150));
                           else offg.setColor(Color.blue);
                           offg.setStroke(new BasicStroke(2.0f));
                           offg.drawRect(18,22,40,37);
                        }
                        
                        if (AUTOMAP) offg.drawImage(automappic,309,5,null);//offg.drawImage(automappic,181,6,null);
                                        
                        if (hero.weapon==null || skipchestscroll || (hero.weapon.number!=5 && hero.weapon.number!=4)) {
                                int len = (int)(((float)hero.food/1000.0f)*170.0f);
                                offg.setColor(new Color(30,30,30));
                                offg.fillRect(245,150,len,10);
                                if (hero.food<75) offg.setColor(Color.red);
                                else if (hero.food<100) offg.setColor(Color.yellow);
                                else offg.setColor(new Color(200,130,10));
                                offg.fillRect(240,145,len,10);
        
                                len = (int)(((float)hero.water/1000.0f)*170.0f);
                                offg.setColor(new Color(30,30,30));
                                offg.fillRect(245,197,len,10);
                                if (hero.water<75) offg.setColor(Color.red);
                                else if (hero.water<100) offg.setColor(Color.yellow);
                                else offg.setColor(Color.blue);
                                offg.fillRect(240,192,len,10);
                        }
                        else if (!mirror && hero.weapon!=null && hero.weapon.number==5) {
                                //draw chest contents
                                //offg.drawImage(openchest,173,104,null);
                                offg.drawImage(openchest,156,104,null);
                                for (int i=0;i<12;i++) {
                                        if ( ((Chest)hero.weapon).contents[i]!=null) {
                                                if (i<6) offg.drawImage(((Chest)hero.weapon).contents[i].pic,227+i*34,207,this);
                                                else offg.drawImage(((Chest)hero.weapon).contents[i].pic,227+(i-6)*34,239,this);
                                        }
                                }
                        }
                        else if (!mirror && hero.weapon!=null && hero.weapon.number==4) {
                                //draw scroll
                                //offg.drawImage(scrollpic,200,104,null);
                                offg.drawImage(scrollpic,156,104,null);
                                //offg.setFont(new Font("Courier",Font.BOLD,16));
                                offg.setFont(scrollfont);
                                int messlength = hero.weapon.scroll.length;
                                while (hero.weapon.scroll[messlength-1].equals("")) { messlength--; }
                                int startdraw = 188-messlength*10;
                                offg.setColor(new Color(30,30,30));
                                for (int i=0;i<messlength;i++) {
                                        //offg.drawString(hero.weapon.scroll[i],325-hero.weapon.scroll[i].length()*5,startdraw+i*20);
                                        //offg.drawString(hero.weapon.scroll[i],320-hero.weapon.scroll[i].length()*5,startdraw+i*20);
                                        offg.drawString(hero.weapon.scroll[i],320-offg.getFontMetrics().stringWidth(hero.weapon.scroll[i])/2,startdraw+i*20);
                                }
                        }
                        if (mirror && herosheet.showresreinc) {
                                //draw res/reinc buttons
                                offg.drawImage(resreincpic,206,104,this);
                        }

                        offg.setColor(new Color(63,63,63));
                        offg.drawImage(hero.weapon.pic,124,106,this);
                        if (hero.weapon.cursed>0 && hero.weapon.cursefound) curseg.fillRect(124,106,32,32);
                        if (hero.hurtweapon) {
                                if (hero.weapon==fistfoot) offg.drawImage(hurtweapon,124,106,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(123,105,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.head!=null) {
                                offg.fillRect(68,52,32,32);
                                offg.drawImage(hero.head.pic,68,52,this);
                                if (hero.head.cursed>0 && hero.head.cursefound) curseg.fillRect(68,52,32,32);
                        }
                        if (hero.hurthead) {
                                if (hero.head==null) offg.drawImage(hurthead,68,52,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(67,51,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.neck!=null) {
                                offg.fillRect(12,66,32,32);
                                offg.drawImage(hero.neck.pic,12,66,this);
                                if (hero.neck.cursed>0 && hero.neck.cursefound) curseg.fillRect(12,66,32,32);
                        }
                        if (hero.torso!=null) {
                                offg.fillRect(68,92,32,32);
                                offg.drawImage(hero.torso.pic,68,92,this);
                                if (hero.torso.cursed>0 && hero.torso.cursefound) curseg.fillRect(68,92,32,32);
                        }
                        if (hero.hurttorso) {
                                if (hero.torso==null) offg.drawImage(hurttorso,68,92,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(67,91,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.hand!=null) {
                                offg.fillRect(12,106,32,32);
                                offg.drawImage(hero.hand.pic,12,106,this);
                                if (hero.hand.cursed>0 && hero.hand.cursefound) curseg.fillRect(12,106,32,32);
                        }
                        if (hero.hurthand) {
                                if (hero.hand==null) offg.drawImage(hurthand,12,106,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(11,105,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.legs!=null) {
                                offg.fillRect(68,132,32,32);
                                offg.drawImage(hero.legs.pic,68,132,this);
                                if (hero.legs.cursed>0 && hero.legs.cursefound) curseg.fillRect(68,132,32,32);
                        }
                        if (hero.hurtlegs) {
                                if (hero.legs==null) offg.drawImage(hurtlegs,68,132,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(67,131,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.feet!=null) {
                                offg.fillRect(68,172,32,32);
                                offg.drawImage(hero.feet.pic,68,172,this);
                                if (hero.feet.cursed>0 && hero.feet.cursefound) curseg.fillRect(68,172,32,32);
                        }
                        if (hero.hurtfeet) {
                                if (hero.feet==null) offg.drawImage(hurtfeet,68,172,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(67,171,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.pouch1!=null) {
                                //offg.fillRect(12,167,32,32);//was 146
                                offg.drawImage(hero.pouch1.pic,12,167,this);
                                if (hero.pouch1.cursed>0 && hero.pouch1.cursefound) curseg.fillRect(12,167,32,32);
                        }
                        if (hero.pouch2!=null) {
                                offg.drawImage(hero.pouch2.pic,12,201,this);//was 180
                                if (hero.pouch2.cursed>0 && hero.pouch2.cursefound) curseg.fillRect(12,201,32,32);
                        }
                        for (int i=0;i<8;i++) {
                                if (hero.pack[i]!=null) {
                                        offg.drawImage(hero.pack[i].pic,166+(34*i),32,this);
                                        if (hero.pack[i].cursed>0 && hero.pack[i].cursefound) curseg.fillRect(166+(34*i),32,32,32);
                                }
                        }
                        for (int i=0;i<8;i++) {
                                if (hero.pack[i+8]!=null) {
                                        offg.drawImage(hero.pack[i+8].pic,166+(34*i),66,this);
                                        if (hero.pack[i+8].cursed>0 && hero.pack[i+8].cursefound) curseg.fillRect(166+(34*i),66,32,32);
                                }
                        }
                        for (int i=0;i<2;i++) {
                                if (hero.quiver[i]!=null) {
                                        //offg.fillRect(124+(34*i),167,32,32);
                                        offg.drawImage(hero.quiver[i].pic,124+(34*i),167,this);
                                        if (hero.quiver[i].cursed>0 && hero.quiver[i].cursefound) curseg.fillRect(124+(34*i),167,32,32);
                                }
                        }
                        for (int i=0;i<2;i++) {
                                if (hero.quiver[i+2]!=null) {
                                        offg.drawImage(hero.quiver[i+2].pic,124+(34*i),201,this);
                                        if (hero.quiver[i+2].cursed>0 && hero.quiver[i+2].cursefound) curseg.fillRect(124+(34*i),201,32,32);
                                }
                        }
                        for (int i=0;i<2;i++) {
                                if (hero.quiver[i+4]!=null) {
                                        offg.drawImage(hero.quiver[i+4].pic,124+(34*i),235,this);
                                        if (hero.quiver[i+4].cursed>0 && hero.quiver[i+4].cursefound) curseg.fillRect(124+(34*i),235,32,32);
                                }
                        }
                        g.drawImage(offscreen,0,0,this.size().width,this.size().height,null);
                } 
                
                public void showStats() {
                        
                        offg.setColor(new Color(60,60,60));
                        offg.fillRect(206,104,232,190);
                        offg.setColor(new Color(95,95,95));
                        offg.setStroke(new BasicStroke(2.0f));
                        offg.drawRect(207,105,230,190);
                        //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                        offg.setFont(dungfont14);
                        int i=120;
                        if (herosheet.hero.flevel>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawString(LEVELNAMES[herosheet.hero.flevel-1]+" Fighter",218,i+3);
                                //offg.setColor(Color.white);
                                if (herosheet.hero.flevelboost>0) offg.setColor(Color.green);
                                else if (herosheet.hero.flevelboost<0) offg.setColor(Color.red);
                                else offg.setColor(Color.white);
                                offg.drawString(LEVELNAMES[herosheet.hero.flevel-1]+" Fighter",215,i);
                                i+=15;
                        }
                        if (herosheet.hero.nlevel>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawString(LEVELNAMES[herosheet.hero.nlevel-1]+" Ninja",218,i+3);
                                //offg.setColor(Color.white);
                                if (herosheet.hero.nlevelboost>0) offg.setColor(Color.green);
                                else if (herosheet.hero.nlevelboost<0) offg.setColor(Color.red);
                                else offg.setColor(Color.white);
                                offg.drawString(LEVELNAMES[herosheet.hero.nlevel-1]+" Ninja",215,i);
                                i+=15;
                        }
                        if (herosheet.hero.wlevel>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawString(LEVELNAMES[herosheet.hero.wlevel-1]+" Wizard",218,i+3);
                                //offg.setColor(Color.white);
                                if (herosheet.hero.wlevelboost>0) offg.setColor(Color.green);
                                else if (herosheet.hero.wlevelboost<0) offg.setColor(Color.red);
                                else offg.setColor(Color.white);
                                offg.drawString(LEVELNAMES[herosheet.hero.wlevel-1]+" Wizard",215,i);
                                i+=15;
                        }
                        if (herosheet.hero.plevel>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawString(LEVELNAMES[herosheet.hero.plevel-1]+" Priest",218,i+3);
                                //offg.setColor(Color.white);
                                if (herosheet.hero.plevelboost>0) offg.setColor(Color.green);
                                else if (herosheet.hero.plevelboost<0) offg.setColor(Color.red);
                                else offg.setColor(Color.white);
                                offg.drawString(LEVELNAMES[herosheet.hero.plevel-1]+" Priest",215,i);
                        }
                        offg.setColor(new Color(30,30,30));
                        offg.drawString("Strength",218,200);
                        offg.drawString("Dexterity",218,214);
                        offg.drawString("Vitality",218,228);
                        offg.drawString("Intelligence",218,242);
                        offg.drawString("Wisdom",218,256);
                        offg.drawString("Defense",218,275);
                        offg.drawString("Resist Magic",218,290);
                        int xpos = 218+offg.getFontMetrics().stringWidth("Resist Magic")+13;
                        offg.drawString(""+herosheet.hero.strength,xpos,200);
                        offg.drawString(""+herosheet.hero.dexterity,xpos,214);
                        offg.drawString(""+herosheet.hero.vitality,xpos,228);
                        offg.drawString(""+herosheet.hero.intelligence,xpos,242);
                        offg.drawString(""+herosheet.hero.wisdom,xpos,256);
                        offg.drawString(""+herosheet.hero.defense,xpos,275);
                        offg.drawString(""+herosheet.hero.magicresist,xpos,290);
                        offg.setColor(Color.white);
                        offg.drawString("Strength",215,197);
                        offg.drawString("Dexterity",215,211);
                        offg.drawString("Vitality",215,225);
                        offg.drawString("Intelligence",215,239);
                        offg.drawString("Wisdom",215,253);
                        offg.drawString("Defense",215,272);
                        offg.drawString("Resist Magic",215,287);
                        xpos-=3;
                        if (herosheet.hero.strengthboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.strengthboost<0) offg.setColor(Color.red);
                        offg.drawString(""+herosheet.hero.strength,xpos,197);
                        if (herosheet.hero.dexterityboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.dexterityboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawString(""+herosheet.hero.dexterity,xpos,211);
                        if (herosheet.hero.vitalityboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.vitalityboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawString(""+herosheet.hero.vitality,xpos,225);
                        if (herosheet.hero.intelligenceboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.intelligenceboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawString(""+herosheet.hero.intelligence,xpos,239);
                        if (herosheet.hero.wisdomboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.wisdomboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawString(""+herosheet.hero.wisdom,xpos,253);
                        if (herosheet.hero.defenseboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.defenseboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawString(""+herosheet.hero.defense,xpos,272);
                        if (herosheet.hero.magicresistboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.magicresistboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawString(""+herosheet.hero.magicresist,xpos,287);
                }

                public void showItem() {
                        if (inhand.number==4) {
                                //draw scroll
                                //offg.drawImage(scrollpic,200,104,null);
                                offg.drawImage(scrollpic,156,104,null);
                                //offg.setFont(new Font("Courier",Font.BOLD,16));
                                offg.setFont(scrollfont);
                                int messlength = inhand.scroll.length;
                                while (inhand.scroll[messlength-1].equals("")) { messlength--; }
                                int startdraw = 188-messlength*10;
                                offg.setColor(new Color(30,30,30));
                                for (int i=0;i<messlength;i++) {
                                        //offg.drawString(inhand.scroll[i],325-inhand.scroll[i].length()*5,startdraw+i*20);
                                        //offg.drawString(inhand.scroll[i],320-inhand.scroll[i].length()*5,startdraw+i*20);
                                        offg.drawString(inhand.scroll[i],320-offg.getFontMetrics().stringWidth(inhand.scroll[i])/2,startdraw+i*20);
                                }
                        }
                        else if (inhand.number==5) {
                                //draw chest contents
                                //offg.drawImage(openchest,173,104,null);
                                offg.drawImage(openchest,156,104,null);
                                for (int i=0;i<12;i++) {
                                        if ( ((Chest)inhand).contents[i]!=null) {
                                                if (i<6) offg.drawImage(((Chest)inhand).contents[i].pic,227+i*34,207,this);
                                                else offg.drawImage(((Chest)inhand).contents[i].pic,227+(i-6)*34,239,this);
                                        }
                                }
                                //for (int i=0;i<6;i++) {
                                //        if ( ((Chest)inhand).contents[i]!=null) {
                                //                offg.drawImage(((Chest)inhand).contents[i].pic,227+i*34,207,this);
                                //        }
                                //}
                        }
                        else {
                        offg.setColor(new Color(60,60,60));
                        offg.fillRect(208,106,228,142);
                        offg.setColor(new Color(95,95,95));
                        //offg.setStroke(new BasicStroke(2.0f));
                        //offg.drawRect(207,105,230,144);
                        offg.setRenderingHint(RenderingHints.KEY_ANTIALIASING,RenderingHints.VALUE_ANTIALIAS_ON);
                        offg.drawOval(231,111,50,50);
                        offg.setRenderingHint(RenderingHints.KEY_ANTIALIASING,RenderingHints.VALUE_ANTIALIAS_OFF);
                        
                        offg.drawImage(inhand.pic,240,120,null);
                        
                        //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                        offg.setFont(dungfont14);
                        offg.setColor(new Color(30,30,30));
                        offg.drawString(inhand.name,302,142);
                        offg.drawString(((float)((int)(inhand.weight*10.0f+.5f)))/10.0f+" kg",302,158);
                        offg.setColor(Color.white);
                        offg.drawString(inhand.name,300,140);
                        offg.drawString(((float)((int)(inhand.weight*10.0f+.5f)))/10.0f+" kg",300,156);
                        
                        if (inhand.number==9) {
                                //torch
                                if (((Torch)inhand).lightboost>36) return;
                                String burnout;
                                if (((Torch)inhand).lightboost==0) burnout = "(Burnt Out)";
                                else burnout = "(Almost Out)";
                                //offg.setFont(new Font("TimesRoman",Font.BOLD,12));
                                offg.setFont(dungfont);
                                offg.setColor(new Color(30,30,30));
                                offg.drawString(burnout,282,202);
                                offg.setColor(Color.white);
                                offg.drawString(burnout,280,200);
                                return;
                        }
                        else if (inhand.number==215) {
                                //stormbringer
                                offg.setColor(new Color(30,30,30));
                                offg.drawString("Stealer of Souls",281,206);
                                offg.setColor(new Color(120,120,120));
                                offg.drawString("Stealer of Souls",280,205);
                                offg.setColor(new Color(30,30,30));
                                offg.drawString("Slayer of Friends",281,224);
                                offg.setColor(new Color(120,120,120));
                                offg.drawString("Slayer of Friends",280,223);
                                offg.setColor(new Color(30,30,30));
                                offg.drawString("Destroyer of Balance",281,247);
                                offg.setColor(new Color(120,120,120));
                                offg.drawString("Destroyer of Balance",280,246);
                                return;
                        }
                        
                        if (inhand.defense>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawString("Defense: "+inhand.defense,301,173);
                                offg.setColor(new Color(250,100,100));
                                offg.drawString("Defense: "+inhand.defense,300,172);
                        }
                        if (inhand.magicresist>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawString("Resist Magic: "+inhand.magicresist,301,186);
                                offg.setColor(new Color(250,100,100));
                                offg.drawString("Resist Magic: "+inhand.magicresist,300,185);
                        }
                        int yalign = 0;
                        for (int i=0;i<inhand.functions;i++) {
                                if (inhand.charges[i]>0) {
                                        if ( (herosheet.hero.wlevel>4 && inhand.function[i][1].equals("w")) || (herosheet.hero.plevel>4 && inhand.function[i][1].equals("p")) ) {
                                                offg.setColor(new Color(30,30,30));
                                                String fs = inhand.function[i][0];
                                                if (fs.endsWith(" Party")) fs = fs.substring(0,fs.indexOf(" Party"));
                                                offg.drawString(fs+" charges: "+inhand.charges[i],251,206+yalign*14);
                                                offg.setColor(new Color(120,120,255));
                                                offg.drawString(fs+" charges: "+inhand.charges[i],250,205+yalign*14);
                                                yalign++;
                                        }
                                }
                        }
                        if ( (herosheet.hero.wlevel>3 && inhand.isbomb) || (herosheet.hero.plevel>3 && inhand.ispotion && !inhand.isbomb) ) {
                                offg.setColor(Color.white);
                                offg.drawString("(",242,178);
                                offg.drawString(")",267,178);
                                offg.drawImage(spellsheet.spellsymbol[inhand.potioncastpow-1],251,168,herosheet);
                        }
                        else if (inhand.number==73) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawString("Drinks left: "+((Waterskin)inhand).drinks,271,247);
                                offg.setColor(new Color(100,100,255));
                                offg.drawString("Drinks left: "+((Waterskin)inhand).drinks,270,246);
                        }
                        else if (inhand.cursed>0 && (inhand.cursefound || herosheet.hero.detectcurse>0 || herosheet.hero.plevel*8>inhand.cursed)) {
                                offg.setColor(Color.white);
                                offg.drawString("(Cursed)",227,178);
                                inhand.cursefound = true;
                        }
                        else if (inhand.type==Item.FOOD) {
                                String foodstring = "(Edible";
                                if (herosheet.hero.plevel>7) {
                                        if (inhand.poisonous>0) foodstring+=" - Poisoned!)";
                                        else if (inhand.foodvalue<0) foodstring+=" - Bad!)";
                                        else foodstring+=")";
                                }
                                else foodstring+=")";
                                offg.setColor(new Color(30,30,30));
                                offg.drawString(foodstring,271,245);
                                offg.setColor(new Color(100,100,255));
                                offg.drawString(foodstring,270,244);
                        }
                        }
                }
                
                public void sleepsheet() {
                        offg.setColor(Color.black);
                        //offg.fillRect(0,0,herosheet.getSize().width,herosheet.getSize().height);
                        offg.fillRect(0,0,herosheet.getWidth(),herosheet.getHeight());
                        //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                        offg.setFont(dungfont14);
                        offg.setColor(new Color(70,70,70));
                        offg.drawString("Sleeping. Click mouse to wake up.",143,143);
                        offg.setColor(Color.white);
                        offg.drawString("Sleeping. Click mouse to wake up.",140,140);
                }
        }