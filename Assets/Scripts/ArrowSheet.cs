using UnityEngine;
using System.Collections;


        class ArrowSheet extends JPanel implements MouseListener {
                Image unpressed,moveforward,moveback,moveleft,moveright,turnleft,turnright;
                int butpressed, presscount = 0;
                boolean pressed = false;
                
                public ArrowSheet() {
                        setSize(173,95);
                        setPreferredSize(new Dimension(173,95));
                        setMaximumSize(new Dimension(173,95));
                        setMinimumSize(new Dimension(173,95));
                        setCursor(handcursor);
                        addMouseListener(this);
                        unpressed = tk.createImage("Interface"+File.separator+"movement.gif");
                        moveforward = tk.createImage("Interface"+File.separator+"forward.gif");
                        moveback = tk.createImage("Interface"+File.separator+"back.gif");
                        moveleft = tk.createImage("Interface"+File.separator+"left.gif");
                        moveright = tk.createImage("Interface"+File.separator+"right.gif");
                        turnleft = tk.createImage("Interface"+File.separator+"turnleft.gif");
                        turnright = tk.createImage("Interface"+File.separator+"turnright.gif");
                        ImageTracker.addImage(unpressed,3);
                        ImageTracker.addImage(moveforward,3);
                        ImageTracker.addImage(moveback,3);
                        ImageTracker.addImage(moveleft,3);
                        ImageTracker.addImage(moveright,3);
                        ImageTracker.addImage(turnleft,3);
                        ImageTracker.addImage(turnright,3);
                        ImageTracker.checkID(3,true);
                }
                
                public void doClick(Integer button) {
                        butpressed = button.intValue();
                        pressed = true;
                        presscount = 3;
                        repaint();
                        walkqueue.add(button);
                }
                
                public void mousePressed(MouseEvent e) {
                        if (actionqueue.size()==4) return; //can only have 4 moves in buffer
                        //get x,y to determine button
                        int x = e.getX(), y = e.getY();
                        Integer WALK;
                        if (x<58) {
                                if (y<47) { butpressed = 4; WALK = ITURNLEFT; }
                                else { butpressed = 2; WALK = ILEFT; }
                        }
                        else if (x<116) {
                                if (y<47) { butpressed = 0; WALK = IFORWARD; }
                                else { butpressed = 1; WALK = IBACK; }
                        }
                        else if (y<47) { butpressed = 5; WALK = ITURNRIGHT; }
                        else { butpressed = 3; WALK = IRIGHT; }
                        pressed = true;
                        //clicknum = walkqueue.size();
                        repaint();
                        walkqueue.add(WALK);
                }
                
                public void paintComponent(Graphics g) {
                        g.drawImage(unpressed,0,0,null);
                        //draw pressed version if should
                        if (pressed) {
                                if (butpressed==0) g.drawImage(moveforward,57,1,null);
                                else if (butpressed==1) g.drawImage(moveback,57,47,null);
                                else if (butpressed==2) g.drawImage(moveleft,0,47,null);
                                else if (butpressed==3) g.drawImage(moveright,114,47,null);
                                else if (butpressed==4) g.drawImage(turnleft,0,1,null);
                                else g.drawImage(turnright,114,1,null);
                        }
                }
                
                public void mouseReleased(MouseEvent e) {
                        pressed = false;
                        repaint();
                }
                public void mouseExited(MouseEvent e) {
                        if (pressed) {
                                pressed = false;
                                repaint();
                        }
                }
                
                public void mouseClicked(MouseEvent e) {}
                public void mouseEntered(MouseEvent e) {}
        }