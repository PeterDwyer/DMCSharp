using UnityEngine;
using System.Collections;

        class HeroClick extends MouseAdapter {

                public void mousePressed(MouseEvent e) {
                        int x=e.getX();
                        int y=e.getY();
                        Hero clickedhero = (Hero)e.getSource();
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
                        else if ((y>17 && y<83) || (e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) {//SwingUtilities.isRightMouseButton(e)) {
                                if (!sheet) {
                                        herosheet.setHero((Hero)e.getSource());
                                        sheet=true;
                                        showCenter(herosheet);
                                }
                                else {
                                        sheet=false;
                                        showCenter(dview);
                                        hupdate();
                                        if (!herosheet.hero.equals((Hero)e.getSource())) {
                                                ((Hero)e.getSource()).dispatchEvent(e);
                                        }
                                }
                        }
                        else if (y<18) {
                                //recalc loads?
                              //if (!clickedhero.isdead) {
                                if (iteminhand) { clickedhero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                hero[leader].isleader=false;
                                clickedhero.isleader=true;
                                if (hero[0].isleader) leader=0;
                                else if (hero[1].isleader) leader=1;
                                else if (hero[2].isleader) leader=2;
                                else leader=3;
                                hupdate();
                                if (sheet) herosheet.repaint();
                              //}
                        }
                        //weapon hand
                        else if (x>55 && x<89) {
                                if (clickedhero.weapon.number==215) {
                                        message.setMessage("Stormbringer resists...",4);
                                        return;
                                }
                                else if (clickedhero.weapon.number==219 || clickedhero.weapon.number==261) {
                                        return;
                                }
                                else if (clickedhero.weapon.cursed>0 && (clickedhero.weapon.type==Item.WEAPON || clickedhero.weapon.type==Item.SHIELD)) {
                                        message.setMessage(clickedhero.weapon.name+" is cursed and cannot be removed!",4);
                                        if (!clickedhero.weapon.cursefound) {
                                                clickedhero.weapon.cursefound = true;
                                                clickedhero.repaint();
                                                if (sheet) herosheet.repaint();
                                        }
                                        return;
                                }
                                if (iteminhand) { clickedhero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (clickedhero.weapon!=null) { clickedhero.load-=clickedhero.weapon.weight; hero[leader].load+=clickedhero.weapon.weight; }
                                if (!iteminhand && !clickedhero.weapon.name.equals("Fist/Foot")) {
                                        iteminhand=true;
                                        inhand=clickedhero.weapon;
                                        clickedhero.weapon=fistfoot;
                                        if (inhand.number==9) {
                                                ((Torch)inhand).putOut();
                                                updateDark();
                                        }
                                        if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) inhand.unEquipEffect(clickedhero);
                                        clickedhero.repaint();
                                        if (sheet) herosheet.repaint();
                                        weaponsheet.repaint();
                                        changeCursor();
                                }
                                else if (iteminhand) {
                                        if (!clickedhero.weapon.name.equals("Fist/Foot")) {
                                                Item tempitem = clickedhero.weapon;
                                                clickedhero.weapon=inhand;
                                                inhand=tempitem;
                                                if (inhand.number==9) {
                                                        ((Torch)inhand).putOut();
                                                        updateDark();
                                                }
                                                if (clickedhero.weapon.number==9) {
                                                        ((Torch)clickedhero.weapon).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) inhand.unEquipEffect(clickedhero);
                                                if (clickedhero.weapon.type==Item.WEAPON || clickedhero.weapon.type==Item.SHIELD || clickedhero.weapon.number==4) clickedhero.weapon.equipEffect(clickedhero);
                                        }
                                        else {
                                                clickedhero.weapon=inhand;
                                                iteminhand=false;
                                                if (clickedhero.weapon.number==9) {
                                                        ((Torch)clickedhero.weapon).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) clickedhero.weapon.equipEffect(clickedhero);
                                        }
                                        clickedhero.repaint();
                                        if (sheet) herosheet.repaint();
                                        weaponsheet.repaint();
                                        changeCursor();
                                }
                        }
                        //action hand
                        else if (x<45 && x>11) {
                                if (clickedhero.hand!=null && clickedhero.hand.cursed>0 && clickedhero.hand.type==Item.SHIELD) {
                                        message.setMessage(clickedhero.hand.name+" is cursed and cannot be removed!",4);
                                        if (!clickedhero.hand.cursefound) {
                                                clickedhero.hand.cursefound = true;
                                                clickedhero.repaint();
                                                if (sheet) herosheet.repaint();
                                        }
                                        return;
                                }
                                if (iteminhand) { clickedhero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (clickedhero.hand!=null) { clickedhero.load-=clickedhero.hand.weight; hero[leader].load+=clickedhero.hand.weight; }
                                if (!iteminhand && clickedhero.hand!=null) {
                                        iteminhand=true;
                                        inhand=clickedhero.hand;
                                        clickedhero.hand=null;
                                        if (inhand.number==9) {
                                                ((Torch)inhand).putOut();
                                                updateDark();
                                        }
                                        if (inhand.type==Item.SHIELD) inhand.unEquipEffect(clickedhero);
                                        clickedhero.repaint();
                                        if (sheet) herosheet.repaint();
                                        //holdingupdate();
                                        changeCursor();
                                }
                                else if (iteminhand) {
                                        if (clickedhero.hand!=null) {
                                                Item tempitem = clickedhero.hand;
                                                clickedhero.hand=inhand;
                                                inhand=tempitem;
                                                if (inhand.number==9) {
                                                        ((Torch)inhand).putOut();
                                                        updateDark();
                                                }
                                                if (clickedhero.hand.number==9) {
                                                        ((Torch)clickedhero.hand).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.SHIELD) inhand.unEquipEffect(clickedhero);
                                                if (clickedhero.hand.type==Item.SHIELD) clickedhero.hand.equipEffect(clickedhero);
                                        }
                                        else {
                                                clickedhero.hand=inhand;
                                                iteminhand=false;
                                                if (clickedhero.hand.number==9) {
                                                        ((Torch)clickedhero.hand).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.SHIELD) clickedhero.hand.equipEffect(clickedhero);
                                        }
                                        clickedhero.repaint();
                                        if (sheet) herosheet.repaint();
                                        //holdingupdate();
                                        changeCursor();
                                }
                        }
                }
        }