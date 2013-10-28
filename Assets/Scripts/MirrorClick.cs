using UnityEngine;
using System.Collections;

        class MirrorClick extends MouseAdapter {
                int x,y;

                public void mousePressed(MouseEvent e) {
                        x = e.getX();
                        y = e.getY();
						x /= DungeonViewScale;
						y /= DungeonViewScale;
                        if ((x>420 && x<440 && y<25 && y>5) || (x>205 && x<439 && y>223 && y<250) || (e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) {//SwingUtilities.isRightMouseButton(e)) {
                                sheet=false;
                                herosheet.mirror=false;
                                showCenter(dview);
                                if (numheroes>0) { spellsheet.setVisible(true); weaponsheet.setVisible(true); }
                                arrowsheet.setVisible(true);
                                dview.addMouseListener(dclick);
                                herosheet.removeMouseListener(this);
                                herosheet.addMouseListener(sheetclick);
                                hpanel.remove(mirrorhero);
                                hpanel.validate();
                                hpanel.repaint();
                                /*herosheet.offscreen.flush();*/
                                mirrorhero=null;
                                if (numheroes>0) {
                                        for (int i=0;i<numheroes;i++) if (!hero[i].isdead) hero[i].addMouseListener(hclick);
                                }
                                nomovement=false;
                        }
                        else if (x>22 && x<57 && y<58 && y>24) {
                                //eye
                                herosheet.skipchestscroll = true;
                                /*herosheet.paint(herosheet.offg);*/
                                herosheet.stats=true;
                                viewing=true;
                                herosheet.repaint();
                                //showStats(herosheet.getGraphics());
                        }
                        else if (x>109 && x<146 && y<58 && y>24) {
                                //mouth
                                herosheet.showresreinc = false;
                                herosheet.repaint();
                        }
                        else if (x>205 && x<320 && y>103 && y<220) {
                                //resurrect
                                if (herosheet.mirrorflag) {
                                        hero[numheroes]=mirrorhero;
                                        heroatsub[mirrorhero.subsquare]=numheroes;
                                        formation.addNewHero();
                                        message.setMessage(hero[numheroes].name+" resurrected.",numheroes);
                                        resetStuff();
                                }
                                else if (allowswap) {
                                        message.setMessage(mirrorhero.name+" resurrected.",leader);
                                        swapHeroes();
                                }
                                else message.setMessage("Can't Resurrect -- No room in party",4);
                        }
                        else if (x>323 && x<439 && y>103 && y<220) {
                                //reincarnate
                                if (herosheet.mirrorflag) {
                                        hero[numheroes]=mirrorhero;
                                        heroatsub[mirrorhero.subsquare]=numheroes;
                                        formation.addNewHero();
                                        hero[numheroes].flevel=0;
                                        hero[numheroes].nlevel=0;
                                        hero[numheroes].plevel=0;
                                        hero[numheroes].wlevel=0;
                                        herosheet.setVisible(false);
                                        //open a modal dialog to get first/last names
                                        EnterName en = new EnterName(frame,hero[numheroes]);
                                        herosheet.setVisible(true);
                                        message.setMessage(hero[numheroes].name+" reincarnated.",numheroes);
                                        resetStuff();
                                }
                                else message.setMessage("Can't Reincarnate -- No room in party",4);
                        }
                }
                private void resetStuff() {
                        ((Mirror)DungeonMap[level][herolookx][herolooky]).wasUsed=true;
                        //check for illumlet
                        if (hero[numheroes].neck!=null && hero[numheroes].neck.number==89) numillumlets++;
                        
                        //dview.repaint();
                        needredraw=true;
                        sheet=false;
                        showCenter(dview);
                        if (numheroes==0) {
                                if (spellsheet!=null) {
                                        if (weaponsheet.showingspecials) weaponsheet.toggleSpecials(0);
                                }
                                else {
                                        spellsheet = new SpellSheet();
                                        weaponsheet = new WeaponSheet();
                                }
                                eastpanel.removeAll();
                                eastpanel.add(ecpanel);
                                eastpanel.add(Box.createVerticalStrut(10));
                                eastpanel.add(spellsheet);
                                eastpanel.add(Box.createVerticalStrut(20));
                                eastpanel.add(weaponsheet);
                                eastpanel.add(Box.createVerticalStrut(10));
                                eastpanel.add(arrowsheet);
                                //eastpanel.add(compass);
                                hero[numheroes].isleader=true;
                                //open a door (instead of altering switches to check for numheroes)
                        }
                        numheroes++;
                        spellsheet.setVisible(true);
                        //spellsheet.setEnabled(true);
                        weaponsheet.setVisible(true);
                        //weaponsheet.setEnabled(true);
                        arrowsheet.setVisible(true);
                        spellsheet.repaint();
                        weaponsheet.repaint();
                        for (int i=0;i<numheroes;i++) {
                                if (!hero[i].isdead) hero[i].addMouseListener(hclick);
                        }
                        dview.addMouseListener(dclick);
                        herosheet.removeMouseListener(this);
                        herosheet.addMouseListener(sheetclick);
                        herosheet.mirror=false;
                        spellsheet.repaint();
                        weaponsheet.repaint();     
                        nomovement=false;
                        hupdate();
                        //activation target
                        MapPoint targ = ((Mirror)DungeonMap[level][herolookx][herolooky]).target;
                        if (targ!=null) DungeonMap[targ.level][targ.x][targ.y].activate();
                        updateDark();//in case has any torches in hand
                }
                private void swapHeroes() {
                        ((Mirror)DungeonMap[level][herolookx][herolooky]).hero=hero[leader];
                        message.setMessage(hero[leader].name+" trades places with "+mirrorhero.name+".",leader);
                        //check for illumlet before swap
                        if (hero[leader].neck!=null && hero[leader].neck.number==89) numillumlets--;
                        hero[leader] = mirrorhero;
                        hero[leader].isleader=true;
                        //check for illumlet after swap
                        if (hero[leader].neck!=null && hero[leader].neck.number==89) numillumlets++;
                        hpanel.removeAll();
                        for (int i=0;i<numheroes;i++) {
                                hpanel.add(hero[i]);
                                if (!hero[i].isdead) hero[i].addMouseListener(hclick);
                        }
                        needredraw=true;
                        sheet=false;
                        showCenter(dview);
                        spellsheet.setVisible(true);
                        weaponsheet.setVisible(true);
                        arrowsheet.setVisible(true);
                        weaponsheet.repaint();
                        dview.addMouseListener(dclick);
                        herosheet.removeMouseListener(this);
                        herosheet.addMouseListener(sheetclick);
                        herosheet.mirror=false;
                        spellsheet.repaint();
                        weaponsheet.repaint();     
                        nomovement=false;
                        hupdate();
                        updateDark();//in case has any torches in hand
                        hpanel.validate();
                }                
                
                public void mouseReleased(MouseEvent e) {
                        if (herosheet.stats) {
                                herosheet.skipchestscroll = false;
                                herosheet.stats=false;
                                viewing = false;
                                /*herosheet.paint(herosheet.getGraphics());*/
								herosheet.repaint();
                        }
                        else {
                                herosheet.showresreinc = true;
                                herosheet.repaint();
                        }
                }
                
        }
        