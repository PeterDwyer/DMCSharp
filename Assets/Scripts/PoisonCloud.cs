using UnityEngine;
using System.Collections;

        class PoisonCloud {
                int level,x,y,stage,stagecounter;
                
                public PoisonCloud(int lvl,int x,int y,int stage) {
                        if (DungeonMap[lvl][x][y].hasCloud) {
                                PoisonCloud tempcloud;
                                Iterator i=cloudstochange.iterator();
                                while (i.hasNext()) {
                                        tempcloud=(PoisonCloud)i.next();
                                        if (tempcloud.x==x && tempcloud.y==y) {
                                                tempcloud.stagecounter=0;
                                                tempcloud.stage+=stage;
                                                if (tempcloud.stage>6) tempcloud.stage=6;
                                                break;
                                        }
                                }
                        }
                        else {
                                level = lvl;
                                this.x = x;
                                this.y = y;
                                this.stage = stage;
                                stagecounter=0;
                                cloudstochange.add(this);
                                DungeonMap[level][x][y].hasCloud=true;
                                cloudchanging = true;
                        }
                }



                public bool update() {
                        if (DungeonMap[level][x][y] instanceof Wall && DungeonMap[level][x][y].mapchar!='>' && DungeonMap[level][x][y].mapchar!='2') { //make sure a switch didn't eliminate the square it was in
                                DungeonMap[level][x][y].hasCloud=false;
                                return false;
                        }
                        stagecounter++;
                        bool tempbool = true;
                        if (stagecounter>6) { stage--; stagecounter=0; }
                        if (stage>0) {
                                if (DungeonMap[level][x][y].hasMons) {
                                        Monster tempmon;
                                        for (int sub=0;sub<4;sub++) {
                                                tempmon = (Monster)dmmons.get(level+","+x+","+y+","+sub);
                                                //if (tempmon!=null && !tempmon.isImmaterial) tempmon.damage(stage*2,POISONHIT);
                                                if (tempmon!=null && !tempmon.isImmaterial) {
                                                        if (!tempmon.poisonimmune) tempmon.damage(4*stage/3,POISONHIT);
                                                        else tempmon.damage(4*stage/6+1,POISONHIT);
                                                }
                                        }
                                        tempmon = (Monster)dmmons.get(level+","+x+","+y+","+5);
                                        //if (tempmon!=null && !tempmon.isImmaterial) tempmon.damage(stage*2,POISONHIT);
                                        if (tempmon!=null && !tempmon.isImmaterial) {
                                                if (!tempmon.poisonimmune) tempmon.damage(4*stage/3,POISONHIT);
                                                else tempmon.damage(4*stage/6+1,POISONHIT);
                                        }
                                }
                                if (DungeonMap[level][x][y].hasParty) {
                                        for (int i=0;i<numheroes;i++) {
                                                //hero[i].damage(stage*2,POISONHIT);
                                                hero[i].damage(4*stage/3,POISONHIT);
                                        }
                                }
                        }
                        else { 
                                DungeonMap[level][x][y].hasCloud=false;
                                tempbool = false;
                        }
                        if (!needredraw && level == dmnew.level) {
                          int xdist = x-partyx; if (xdist<0) xdist*=-1;
                          int ydist = y-partyy; if (ydist<0) ydist*=-1;
                          if (xdist<4 && ydist<4) needredraw=true;
                        }
                        return tempbool;
                }
        }
