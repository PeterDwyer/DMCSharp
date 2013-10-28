using UnityEngine;
using System.Collections;

class SheetClick extends MouseAdapter {
                int x,y;
                
                public void mousePressed(MouseEvent e) {
                        x = e.getX();
                        y = e.getY();
						x /= DungeonViewScale;
						y /= DungeonViewScale;
                        if (sleeping) {
                                needredraw=true;
                                sleeping = false;
                                sleeper = 0;
                                spellsheet.setVisible(true);
                                weaponsheet.setVisible(true);
                                arrowsheet.setVisible(true);

                                if (herosheet.hero.isdead) herosheet.setHero(hero[leader]);
                                else herosheet.repaint();
                        }
                        else if (x>420 && x<440 && y<25 && y>5 || (e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) {//SwingUtilities.isRightMouseButton(e)) {
                                sheet=false;
                                showCenter(dview);
                                hupdate();
								return;
                        }
                        //add load test to each after xy tests
                        //weapon hand
                        else if (x>124 && x<156 && y<136 && y>104) {
                                if (herosheet.hero.weapon.number==215) {
                                        message.setMessage("Stormbringer resists...",4);
                                        return;
                                }
                                else if (herosheet.hero.weapon.number==219 || herosheet.hero.weapon.number==261) {
                                        return;
                                }
                                else if (herosheet.hero.weapon.cursed>0 && (herosheet.hero.weapon.type==Item.WEAPON || herosheet.hero.weapon.type==Item.SHIELD)) {
                                        message.setMessage(herosheet.hero.weapon.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.weapon.cursefound) {
                                                herosheet.hero.weapon.cursefound = true;
                                                herosheet.hero.repaint();
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (iteminhand) { herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (herosheet.hero.weapon!=null) { herosheet.hero.load-=herosheet.hero.weapon.weight; hero[leader].load+=herosheet.hero.weapon.weight; }
                                if (!iteminhand && !herosheet.hero.weapon.name.equals("Fist/Foot")) {
                                        iteminhand=true;
                                        inhand=herosheet.hero.weapon;
                                        herosheet.hero.weapon=fistfoot;
                                        if (inhand.number==9) {
                                                ((Torch)inhand).putOut();
                                                updateDark();
                                        }
                                        if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                        herosheet.hero.repaint();
                                        weaponsheet.repaint();
                                }
                                else if (iteminhand) {
                                        if (!herosheet.hero.weapon.name.equals("Fist/Foot")) {
                                                Item tempitem = herosheet.hero.weapon;
                                                herosheet.hero.weapon=inhand;
                                                inhand=tempitem;
                                                if (inhand.number==9) {
                                                        ((Torch)inhand).putOut();
                                                        updateDark();
                                                }
                                                if (herosheet.hero.weapon.number==9) {
                                                        ((Torch)herosheet.hero.weapon).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) inhand.unEquipEffect(herosheet.hero);
                                                if (herosheet.hero.weapon.type==Item.WEAPON || herosheet.hero.weapon.type==Item.SHIELD || herosheet.hero.weapon.number==4) herosheet.hero.weapon.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.weapon=inhand;
                                                iteminhand=false;
                                                if (herosheet.hero.weapon.number==9) {
                                                        ((Torch)herosheet.hero.weapon).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) herosheet.hero.weapon.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                        herosheet.hero.repaint();
                                        weaponsheet.repaint();
                                }
                        }
                        //action hand
                        else if (x>12 && x<44 && y<136 && y>104) {
                                if (herosheet.hero.hand!=null && herosheet.hero.hand.cursed>0 && herosheet.hero.hand.type==Item.SHIELD) {
                                        message.setMessage(herosheet.hero.hand.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.hand.cursefound) {
                                                herosheet.hero.hand.cursefound = true;
                                                herosheet.hero.repaint();
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (iteminhand) { herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (herosheet.hero.hand!=null) { herosheet.hero.load-=herosheet.hero.hand.weight; hero[leader].load+=herosheet.hero.hand.weight; }
                                if (!iteminhand && herosheet.hero.hand!=null) {
                                        iteminhand=true;
                                        inhand=herosheet.hero.hand;
                                        herosheet.hero.hand=null;
                                        if (inhand.number==9) {
                                                ((Torch)inhand).putOut();
                                                updateDark();
                                        }
                                        if (inhand.type==Item.SHIELD) inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                        herosheet.hero.repaint();
                                }
                                else if (iteminhand) {
                                        if (herosheet.hero.hand!=null) {
                                                Item tempitem = herosheet.hero.hand;
                                                herosheet.hero.hand=inhand;
                                                inhand=tempitem;
                                                if (inhand.number==9) {
                                                        ((Torch)inhand).putOut();
                                                        updateDark();
                                                }
                                                if (herosheet.hero.hand.number==9) {
                                                        ((Torch)herosheet.hero.hand).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.SHIELD) inhand.unEquipEffect(herosheet.hero);
                                                if (herosheet.hero.hand.type==Item.SHIELD) herosheet.hero.hand.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.hand=inhand;
                                                iteminhand=false;
                                                if (herosheet.hero.hand.number==9) {
                                                        ((Torch)herosheet.hero.hand).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.SHIELD) herosheet.hero.hand.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                        herosheet.hero.repaint();
                                }
                        }
                        //head
                        else if (x>68 && x<100 && y<84 && y>52) {
                                if (herosheet.hero.head!=null && herosheet.hero.head.cursed>0 && herosheet.hero.head.type==Item.HEAD) {
                                        message.setMessage(herosheet.hero.head.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.head.cursefound) {
                                                herosheet.hero.head.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.head!=null) {
                                        herosheet.hero.load-=herosheet.hero.head.weight;
                                        hero[leader].load+=herosheet.hero.head.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.head;
                                        herosheet.hero.head=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.HEAD) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.head!=null) {
                                                herosheet.hero.load-=herosheet.hero.head.weight;
                                                hero[leader].load+=herosheet.hero.head.weight;
                                                Item tempitem = herosheet.hero.head;
                                                herosheet.hero.head=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.head.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.head=inhand;
                                                iteminhand=false;
                                                herosheet.hero.head.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //neck
                        else if (x>12 && x<44 && y<98 && y>66) {
                                if (herosheet.hero.neck!=null && herosheet.hero.neck.cursed>0 && herosheet.hero.neck.type==Item.NECK) {
                                        message.setMessage(herosheet.hero.neck.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.neck.cursefound) {
                                                herosheet.hero.neck.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.neck!=null) {
                                        herosheet.hero.load-=herosheet.hero.neck.weight;
                                        hero[leader].load+=herosheet.hero.neck.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.neck;
                                        herosheet.hero.neck=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        if (inhand.number==89) { numillumlets--; updateDark(); }
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.NECK) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.neck!=null) {
                                                herosheet.hero.load-=herosheet.hero.neck.weight;
                                                hero[leader].load+=herosheet.hero.neck.weight;
                                                Item tempitem = herosheet.hero.neck;
                                                herosheet.hero.neck=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.neck.equipEffect(herosheet.hero);
                                                if (inhand.number==89) { numillumlets--; updateDark(); }
                                                else if (herosheet.hero.neck.number==89) {
                                                        numillumlets++;
                                                        updateDark();
                                                }
                                        }
                                        else {
                                                herosheet.hero.neck=inhand;
                                                iteminhand=false;
                                                herosheet.hero.neck.equipEffect(herosheet.hero);
                                                if (inhand.number==89) {
                                                        numillumlets++;
                                                        updateDark();
                                                }
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //torso
                        else if (x>68 && x<100 && y<124 && y>92) {
                                if (herosheet.hero.torso!=null && herosheet.hero.torso.cursed>0 && herosheet.hero.torso.type==Item.TORSO) {
                                        message.setMessage(herosheet.hero.torso.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.torso.cursefound) {
                                                herosheet.hero.torso.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.torso!=null) {
                                        herosheet.hero.load-=herosheet.hero.torso.weight;
                                        hero[leader].load+=herosheet.hero.torso.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.torso;
                                        herosheet.hero.torso=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.TORSO) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.torso!=null) {
                                                herosheet.hero.load-=herosheet.hero.torso.weight;
                                                hero[leader].load+=herosheet.hero.torso.weight;
                                                Item tempitem = herosheet.hero.torso;
                                                herosheet.hero.torso=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.torso.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.torso=inhand;
                                                iteminhand=false;
                                                herosheet.hero.torso.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //legs
                        else if (x>68 && x<100 && y<164 && y>132) {
                                if (herosheet.hero.legs!=null && herosheet.hero.legs.cursed>0 && herosheet.hero.legs.type==Item.LEGS) {
                                        message.setMessage(herosheet.hero.legs.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.legs.cursefound) {
                                                herosheet.hero.legs.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.legs!=null) {
                                        herosheet.hero.load-=herosheet.hero.legs.weight;
                                        hero[leader].load+=herosheet.hero.legs.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.legs;
                                        herosheet.hero.legs=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.LEGS) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.legs!=null) {
                                                herosheet.hero.load-=herosheet.hero.legs.weight;
                                                hero[leader].load+=herosheet.hero.legs.weight;
                                                Item tempitem = herosheet.hero.legs;
                                                herosheet.hero.legs=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.legs.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.legs=inhand;
                                                iteminhand=false;
                                                herosheet.hero.legs.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //feet
                        else if (x>68 && x<100 && y<204 && y>172) {
                                if (herosheet.hero.feet!=null && herosheet.hero.feet.cursed>0 && herosheet.hero.feet.type==Item.FEET) {
                                        message.setMessage(herosheet.hero.feet.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.feet.cursefound) {
                                                herosheet.hero.feet.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.feet!=null) {
                                        herosheet.hero.load-=herosheet.hero.feet.weight;
                                        hero[leader].load+=herosheet.hero.feet.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.feet;
                                        herosheet.hero.feet=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.FEET) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.feet!=null) {
                                                herosheet.hero.load-=herosheet.hero.feet.weight;
                                                hero[leader].load+=herosheet.hero.feet.weight;
                                                Item tempitem = herosheet.hero.feet;
                                                herosheet.hero.feet=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.feet.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.feet=inhand;
                                                iteminhand=false;
                                                herosheet.hero.feet.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //pouch1
                        else if (x>12 && x<44 && y<199 && y>167) {
                                if (!iteminhand && herosheet.hero.pouch1!=null) {
                                        herosheet.hero.load-=herosheet.hero.pouch1.weight;
                                        hero[leader].load+=herosheet.hero.pouch1.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.pouch1;
                                        herosheet.hero.pouch1=null;
                                        herosheet.repaint();
                                }
                                //else if (iteminhand && (inhand.size<1 || (inhand.number>87 && inhand.number<101))) {
                                else if (iteminhand && inhand.size<2) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.pouch1!=null) {
                                                herosheet.hero.load-=herosheet.hero.pouch1.weight;
                                                hero[leader].load+=herosheet.hero.pouch1.weight;
                                                Item tempitem = herosheet.hero.pouch1;
                                                herosheet.hero.pouch1=inhand;
                                                inhand=tempitem;
                                        }
                                        else {
                                                herosheet.hero.pouch1=inhand;
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //pouch2
                        else if (x>12 && x<44 && y<233 && y>201) {
                                if (!iteminhand && herosheet.hero.pouch2!=null) {                                                                                    
                                        herosheet.hero.load-=herosheet.hero.pouch2.weight;
                                        hero[leader].load+=herosheet.hero.pouch2.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.pouch2;
                                        herosheet.hero.pouch2=null;
                                        herosheet.repaint();
                                }
                                //else if (iteminhand && (inhand.size<1 || (inhand.number>87 && inhand.number<101))) {
                                else if (iteminhand && inhand.size<2) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.pouch2!=null) {
                                                herosheet.hero.load-=herosheet.hero.pouch2.weight;
                                                hero[leader].load+=herosheet.hero.pouch2.weight;
                                                Item tempitem = herosheet.hero.pouch2;
                                                herosheet.hero.pouch2=inhand;
                                                inhand=tempitem;
                                        }
                                        else {
                                                herosheet.hero.pouch2=inhand;
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //pack
                        else if (x>166 && x<436 && y<98 && y>32) {
                                int i=0;
                                if (y>65) i=8;
                                if (iteminhand) { herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (herosheet.hero.pack[(x-166)/34+i]!=null) { herosheet.hero.load-=herosheet.hero.pack[(x-166)/34+i].weight; hero[leader].load+=herosheet.hero.pack[(x-166)/34+i].weight; }
                                if (!iteminhand && herosheet.hero.pack[(x-166)/34+i]!=null) {
                                        iteminhand=true;
                                        inhand=herosheet.hero.pack[(x-166)/34+i];
                                        herosheet.hero.pack[(x-166)/34+i]=null;
                                        herosheet.repaint();
                                }
                                else if (iteminhand) {
                                        if (herosheet.hero.pack[(x-166)/34+i]!=null) {
                                                Item tempitem = herosheet.hero.pack[(x-166)/34+i];
                                                herosheet.hero.pack[(x-166)/34+i]=inhand;
                                                inhand=tempitem;
                                        }
                                        else {
                                                herosheet.hero.pack[(x-166)/34+i]=inhand;
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }

                        }
                        //quiver
                        else if (x>124 && x<190 && y<267 && y>167) {
                                int i=0;
                                if (y>234) i=4;
                                else if (y>200) i=2;
                                if (!iteminhand && herosheet.hero.quiver[(x-124)/34+i]!=null) {
                                        herosheet.hero.load-=herosheet.hero.quiver[(x-124)/34+i].weight;
                                        hero[leader].load+=herosheet.hero.quiver[(x-124)/34+i].weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.quiver[(x-124)/34+i];
                                        herosheet.hero.quiver[(x-124)/34+i]=null;
                                        herosheet.repaint();
                                }
                                //else if (iteminhand && inhand.type==Item.WEAPON && inhand.number!=9) {
                                //else if (iteminhand && inhand.type==Item.WEAPON && (inhand.size<2 || (inhand.number>225 && inhand.number<230) || (inhand.number>235 && inhand.number<256) || inhand.number==9) && (inhand.size<1 || (x-124)/34+i==0)) {
                                else if (iteminhand && inhand.type==Item.WEAPON && ((inhand.projtype>0 && inhand.size<2) || (inhand.size<4 && (x-124)/34+i==0))) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.quiver[(x-124)/34+i]!=null) {
                                                herosheet.hero.load-=herosheet.hero.quiver[(x-124)/34+i].weight;
                                                hero[leader].load+=herosheet.hero.quiver[(x-124)/34+i].weight;
                                                Item tempitem = herosheet.hero.quiver[(x-124)/34+i];
                                                herosheet.hero.quiver[(x-124)/34+i]=inhand;
                                                inhand=tempitem;
                                        }
                                        else {
                                                herosheet.hero.quiver[(x-124)/34+i]=inhand;
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //chest
                        //else if (x>227 && x<431 && y>206 && y<239 && herosheet.hero.weapon!=null && herosheet.hero.weapon.number==5) {
                        else if (x>227 && x<431 && y>206 && y<271 && herosheet.hero.weapon!=null && herosheet.hero.weapon.number==5) {
                                //if (iteminhand) { herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                //if (((Chest)herosheet.hero.weapon).contents[(x-227)/34]!=null) { herosheet.hero.load-=((Chest)herosheet.hero.weapon).contents[(x-227)/34].weight; hero[leader].load+=((Chest)herosheet.hero.weapon).contents[(x-227)/34].weight; }
                                int ymod = 0;if (y>238) ymod = 6;
                                if (!iteminhand && ((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod]!=null) {
                                        herosheet.hero.load-=((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod].weight; hero[leader].load+=((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod].weight;
                                        iteminhand=true;
                                        inhand=((Chest)herosheet.hero.weapon).getItem((x-227)/34+ymod);
                                        //((Chest)herosheet.hero.weapon).contents[(x-227)/34]=null;
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.number!=5) { //magically holds big things too, except other chests
                                //else if (iteminhand) { //magically holds big things too
                                //else if (iteminhand && inhand.size<3) {
                                //else if (iteminhand && inhand.size<3 && (inhand.number<200 || inhand.number>235)) {
                                        herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight;
                                        if (((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod]!=null) {
                                                herosheet.hero.load-=((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod].weight; hero[leader].load+=((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod].weight;
                                                Item tempitem = ((Chest)herosheet.hero.weapon).getItem((x-227)/34+ymod);
                                                ((Chest)herosheet.hero.weapon).putItem((x-227)/34+ymod,inhand);
                                                inhand=tempitem;
                                        }
                                        else {
                                                ((Chest)herosheet.hero.weapon).putItem((x-227)/34+ymod,inhand);
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //eye (was below in sep method)
                        else if (x>20 && x<57 && y<58 && y>24) {
                                herosheet.skipchestscroll = true;
                                /*herosheet.paint(herosheet.offg);*/
                                viewing=true;
                                if (!iteminhand) herosheet.stats=true;
                                else if (inhand.number==4) {
                                        inhand.temppic = inhand.pic;
                                        inhand.pic = inhand.epic;
                                        changeCursor();
                                }
                                herosheet.repaint();
								return;
                        }
                        //mouth
                        else if (x>109 && x<146 && y<58 && y>24) {
                                if (iteminhand && (inhand.type==Item.FOOD || inhand.ispotion || inhand.number==72 || inhand.number==73)) {
                                        herosheet.hero.eatdrink();
                                        herosheet.hero.repaint();
                                        herosheet.repaint();
                                }
                        }
                        //map
                        //else if (AUTOMAP && x>180 && x<208 && y<24 && y>5) {
                        else if (AUTOMAP && x>308 && x<336 && y<24 && y>5) {
                                toppanel.setVisible(false);
                                centerpanel.setVisible(false);
                                mappane.setVisible(true);
                                validate();
								return;
                        }
                        //sleep
                        else if (x>260 && x<295 && y<24 && y>5) {
                                sleeping = true;
                                sleeper = 50;
                                spellsheet.setVisible(false);
                                weaponsheet.setVisible(false);
                                arrowsheet.setVisible(false);
                                herosheet.repaint();
								return;
                        }
                        //options
                        else if (x>347 && x<366 && y<24 && y>5) {
                                if (actionqueue.isEmpty()) actionqueue.add("o");
								return;
                        }
                        changeCursor();
                }

                public void mouseReleased(MouseEvent e) {
                        if (viewing) {
                                herosheet.skipchestscroll = false;
                                herosheet.stats=false;
                                viewing = false;
                                if (iteminhand && inhand.number==4) {
                                        inhand.pic = inhand.temppic;
                                        changeCursor();
                                }
                                /*herosheet.paint(herosheet.getGraphics());*/
								herosheet.repaint();
                        }
                }
                
        }
