using UnityEngine;
using System.Collections;

        class DungView extends JPanel {//JComponent { //Canvas {
          //Image offscreen;
          BufferedImage offscreen;
          Graphics2D offg;
          Graphics2D offg2;
          //ImageFilter darkfilter;

          public DungView() {
                /*setDoubleBuffered(false);*/
                /*setDoubleBuffered(true);*/
                setBackground(Color.black);
                //darkfilter = new DarknessFilter();
          }
          
          public void paintComponent(Graphics g) {
                if (needdrawdungeon || nomovement || offscreen==null ) {
                        //get visible
                        int xtest,ytest;
                        switch (facing) {
                                case NORTH:
                                        for (int i=0;i<4;i++) {
                                                ytest = partyy-3+i;
                                                for (int j=0;j<5;j++) {
                                                        xtest = partyx-2+j;
                                                        if (xtest<0 || ytest<0 || xtest>(mapwidth-1) || ytest>(mapheight-1)) visibleg[i][j]=outWall;
                                                        else visibleg[i][j]=DungeonMap[level][xtest][ytest];
                                                }
                                        }
                                        break;
                                case SOUTH:
                                        for (int i=0;i<4;i++) {
                                                ytest = partyy+3-i;
                                                for (int j=0;j<5;j++) {
                                                        xtest = partyx+2-j;
                                                        if (xtest<0 || ytest<0 || xtest>(mapwidth-1) || ytest>(mapheight-1)) visibleg[i][j]=outWall;
                                                        else visibleg[i][j]=DungeonMap[level][xtest][ytest];
                                                }
                                        }
                                        break;
                                case WEST:
                                        for (int i=0;i<4;i++) {
                                                xtest = partyx-3+i;
                                                for (int j=0;j<5;j++) {
                                                        ytest = partyy+2-j;
                                                        if (xtest<0 || ytest<0 || xtest>(mapwidth-1) || ytest>(mapheight-1)) visibleg[i][j]=outWall;
                                                        else visibleg[i][j]=DungeonMap[level][xtest][ytest];
                                                }
                                        }
                                        break;
                                case EAST:
                                        for (int i=0;i<4;i++) {
                                                xtest = partyx+3-i;
                                                for (int j=0;j<5;j++) {
                                                        ytest = partyy-2+j;
                                                        if (xtest<0 || ytest<0 || xtest>(mapwidth-1) || ytest>(mapheight-1)) visibleg[i][j]=outWall;
                                                        else visibleg[i][j]=DungeonMap[level][xtest][ytest];
                                                }
                                        }
                                        break;
                        }
                        //upper left x,y, width,height, row, col
        
                        // 0  1  2  3  4      | dist
                        //[ ][ ][ ][ ][ ]  3  |  0
                        //   [ ][ ][ ]     2  |  1
                        //   [ ][ ][ ]     1  |  2
                        //   [ ][x][ ]     0  |  3
        

                        if (!mirrorback) offg.drawImage(back,0,0,this); //background - floor&ceiling
                        else offg.drawImage(back,448,0,0,326,0,0,448,326,this); //mirrored background
        
                        //row 3 in front of party
                        visibleg[0][0].drawPic(3,0,0,62,offg,this);
                        visibleg[0][1].drawPic(3,1,0,62,offg,this);
                        visibleg[0][4].drawPic(3,4,416,62,offg,this);
                        visibleg[0][3].drawPic(3,3,448,62,offg,this);
                        visibleg[0][2].drawPic(3,2,148,62,offg,this);
                        
                        //row 2 in front of party
                        visibleg[1][1].drawPic(2,1,0,50,offg,this);
                        visibleg[1][3].drawPic(2,3,448,50,offg,this);
                        visibleg[1][2].drawPic(2,2,120,50,offg,this);
        
                        //row 1 in front of party
                        visibleg[2][1].drawPic(1,1,0,22,offg,this);
                        visibleg[2][3].drawPic(1,3,448,22,offg,this);
                        if (magicvision>0 && !(visibleg[2][2] instanceof Floor) && !(visibleg[2][2] instanceof Door) && !(visibleg[2][2] instanceof Stairs)) {
                               visibleg[2][2].drawPic(1,2,64,22,offg2,this);
                        }
                        else visibleg[2][2].drawPic(1,2,64,22,offg,this);
        
                        //row party is in
                        visibleg[3][1].drawPic(0,1,0,0,offg,this);
                        visibleg[3][3].drawPic(0,3,448,0,offg,this);
                        visibleg[3][2].drawPic(0,2,0,0,offg,this);

                        if (!NODARK && darkfactor<255) {
                           offg.setColor(new Color(0,0,0,255-darkfactor));
                           offg.fillRect(0,0,448,326);
                        }
                        needdrawdungeon = false;
                        
                        //brightness adjustment
                        offg.setColor(new Color(255,255,255,BRIGHTADJUST));
                        offg.fillRect(0,0,448,326);
                        
                }
//                g.drawImage(offscreen,0,0,this);
                g.drawImage(offscreen,0,0,this.size().width,this.size().height,this);
                //System.out.println("drawdungeon complete");
          }
          
          
          /*
          public boolean imageUpdate(Image img, int infoflags, int x, int y, int width, int height) {
                boolean tf = super.imageUpdate(img, infoflags, x, y, width, height);
                if (tf && !animimgs.contains(img)) animimgs.add(img);
                return tf;
          }
          */
          
        }
