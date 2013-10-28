using UnityEngine;
using System.Collections;

class FluxCage {
        int level,x,y,counter;
        bool mirrored;

        public FluxCage(int level,int x,int y) {
                FluxCage fc = (FluxCage)fluxcages.get(level+","+x+","+y);
                if (fc!=null) { fc.counter=0; fc.mirrored=!fc.mirrored; return; }
                this.level=level;
                this.x=x;
                this.y=y;
                counter=0;
                fluxcages.put(level+","+x+","+y,this);
                fluxchanging = true;
        }

        public bool update() {
                if (DungeonMap[level][x][y] instanceof Wall) { //make sure a switch didn't eliminate the square it was in
                        return false;
                }
                counter++;
                bool tempbool;
                if (counter>100) tempbool=false;
                else tempbool=true;
                if (level == dmnew.level) {
                  int xdist = x-partyx; if (xdist<0) xdist*=-1;
                  int ydist = y-partyy; if (ydist<0) ydist*=-1;
                  if (xdist<4 && ydist<4 && counter%3==0) {
                        mirrored=!mirrored;
                        needredraw=true;
                  }
                }
                return tempbool;
        }

}
