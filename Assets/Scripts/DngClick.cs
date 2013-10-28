using UnityEngine;
using System.Collections;

        class DungClick extends MouseAdapter {

                public void mousePressed(MouseEvent e) {
						//if (numheroes==0) return;
                        //switch to hsheet of leader if right mouse but clicked
                        //if (SwingUtilities.isRightMouseButton(e) && numheroes>0) {
						if (numheroes>0 && (e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) {
                                herosheet.setHero(hero[leader]);
                                sheet=true;
                                showCenter(herosheet);
                                return;
                        }
                        else if (clickqueue.isEmpty()) {
							int x = (int)(e.getX() / DungeonViewScale);
							int y = (int)(e.getY() / DungeonViewScale);
							clickqueue.add(x+","+y);
						}
                }        
                public void processClick(int x, int y) {
                        int yadjust=partyy,xadjust=partyx,gridadjust=1;
                        if (facing==NORTH) yadjust--;
                        else if (facing==WEST) xadjust--;
                        else if (facing==SOUTH) yadjust++;
                        else xadjust++;

                        //int x=e.getX(), y=e.getY();
                        MapObject tocheck;
                        if (xadjust>=0 && yadjust>=0 && xadjust<mapwidth && yadjust<mapheight) tocheck = DungeonMap[level][xadjust][yadjust];
                        else tocheck=outWall;
                        MapObject standingon = DungeonMap[level][partyx][partyy];

                        //message.setMessage("Click at "+x+","+y,4);
                        
                        //if no heroes, can only use mirrors
                        if (numheroes==0) {
                                if (tocheck instanceof Mirror) tocheck.tryWallSwitch(x,y);
                                return;
                        }

                        if (iteminhand) { //drop,throw,use key/coin,push button
                                if (y<236) { //was 190 then 220
                                        iteminhand = tocheck.tryWallSwitch(x,y,inhand); //use switch, open door
                                        if (doorkeyflag) doorkeyflag=false;
                                        else if (!iteminhand) {
                                                //if (inhand.number!=71 || tocheck.mapchar!=']' || tocheck.mapchar!='[' || tocheck.mapchar!='f') hero[leader].load-=inhand.weight;
                                                if (inhand.number!=71 || tocheck.mapchar==']' || tocheck.mapchar=='[' || tocheck.mapchar=='f') hero[leader].load-=inhand.weight;
                                                else iteminhand = true;
                                        }
                                        //changed xy bounds - was x<300  x>148
                                        //if (x<284 && x>164 && (tocheck.canPassProjs || tocheck instanceof Door || tocheck instanceof Stairs)) {
                                        //else if (x<336 && x>114 && (tocheck.canPassProjs || tocheck instanceof Door || tocheck instanceof Stairs)) {//was <350, >100
                                        else if (y<190 && x<356 && x>94 && (tocheck.canPassProjs || tocheck instanceof Door || tocheck instanceof Stairs)) {//was <350, >100
                                                playSound("swing.wav",-1,-1);
                                                inhand.shotpow=hero[leader].strength/10+randGen.nextInt(4);
                                                int projsub = 0;
                                                if (x>224) projsub = 1;
                                                hero[leader].gainxp('n',1);
                                                hero[leader].vitalize(-randGen.nextInt((int)inhand.weight+2));
                                                //hero[leader].stamina = hero[leader].stamina - randGen.nextInt((int)inhand.weight+1)-1;
                                                //if (hero[leader].stamina<1) hero[leader].stamina=1;
                                                hero[leader].wepThrow(inhand,projsub);
                                                iteminhand=false;
                                                hero[leader].weaponcount = (int)inhand.weight+8;
                                                if (hero[leader].dexterity<40) {
                                                        hero[leader].weaponcount++;
                                                        if (hero[leader].dexterity<30) {
                                                                hero[leader].weaponcount+=2;
                                                                if (hero[leader].dexterity<20) {
                                                                        hero[leader].weaponcount+=2;
                                                                        if (hero[leader].dexterity<10) hero[leader].weaponcount+=4;
                                                                }
                                                        }
                                                }
                                                else if (hero[leader].dexterity>50) { 
                                                   hero[leader].weaponcount--;
                                                   if (hero[leader].dexterity>70) {
                                                      hero[leader].weaponcount--;
                                                      if (hero[leader].dexterity>90) hero[leader].weaponcount--;
                                                   }
                                                }
                                                if (hero[leader].weaponcount<1) hero[leader].weaponcount=1;
                                                if (hero[leader].stamina<hero[leader].maxstamina/5 || hero[leader].load>hero[leader].maxload) hero[leader].weaponcount+=4; //slower if low stamina or overloaded
                                                hero[leader].repaint();
                                                hero[leader].wepready = false;
                                                if (leader==weaponready) weaponsheet.repaint();
                                        }
                                        changeCursor();
                                }
                                else if (y>235 && (x>64 && x<384)) {
                                    //could elim x test above and put here - make so can pick up from squares in front but to the sides (subsquares 3<- and ->2 only)
                                    if (y>278) {
                                        tocheck = standingon;
                                        gridadjust = 0;
                                    }
                                    if (tocheck.canHoldItems) {
                                        if (x<224) inhand.subsquare = (3*gridadjust-facing+4)%4;
                                        else inhand.subsquare = (1+gridadjust-facing+4)%4;
                                        iteminhand = false;
                                        hero[leader].load-=inhand.weight;
                                        changeCursor();
                                        if (!tocheck.tryTeleport(inhand)) {
                                                tocheck.addItem(inhand);
                                                tocheck.tryFloorSwitch(MapObject.PUTITEM); //activate pressure plate or trap
                                        }
                                        needredraw=true;
                                    }
                                }
                        }
                        else if (y>235 && (x>64 && x<384)) { //pick up, later add -- or alcove in partyy-1 and xy in it
                                if (y>278) {
                                        tocheck = standingon;
                                        gridadjust = 0;
                                }
                                if (tocheck.canHoldItems && tocheck.hasItems) {
                                        int square;
                                        if (x<224) square = (3*gridadjust-facing+4)%4;
                                        else square = (1+gridadjust-facing+4)%4;
                                        //can't pick up if mon standing on:
                                        if (gridadjust!=0) {
                                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+square);
                                                if (tempmon==null) tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                                if (tempmon!=null && !tempmon.isflying && tempmon.currentai!=Monster.FRIENDLYNOMOVE && tempmon.currentai!=Monster.FRIENDLYMOVE) return;
                                        }
                                        inhand = tocheck.pickUpItem(square);
                                        if (inhand!=null) {
                                                needredraw = true;// needdrawdungeon = true; dview.repaint(); //this fix didn't work -> forced repaint is bad since map can be in process of change
                                                iteminhand=true;
                                                hero[leader].load+=inhand.weight;
                                                if (inhand.number==8) { Compass.addCompass(inhand); ((Compass)inhand).setPic(facing); }
                                                else if (inhand.number==5) {
                                                        //search chest for compasses
                                                        for (int j=0;j<6;j++) {
                                                                Item citem = ((Chest)inhand).itemAt(j);
                                                                if (citem!=null && citem.number==8) { Compass.addCompass(citem); ((Compass)citem).setPic(facing); }
                                                        }
                                                }
                                                changeCursor();
                                                tocheck.tryFloorSwitch(MapObject.TOOKITEM);
                                        }
                                }
                        }
                        else { //switch or door or alcove
                                tocheck.tryWallSwitch(x,y);
                                if (iteminhand) {
                                        if  (inhand.number==8) { Compass.addCompass(inhand); ((Compass)inhand).setPic(facing); }
                                        else if (inhand.number==5) {
                                                //search chest for compasses
                                                for (int j=0;j<6;j++) {
                                                        Item citem = ((Chest)inhand).itemAt(j);
                                                        if (citem!=null && citem.number==8) { Compass.addCompass(citem); ((Compass)citem).setPic(facing); }
                                                }
                                        }
                                }        
                                changeCursor();
                        }
                }
        }