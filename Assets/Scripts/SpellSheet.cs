using UnityEngine;
using System.Collections;

public class SpellSheet : MonoBehaviour {
        class SpellSheet extends JPanel implements MouseListener, MouseMotionListener {
                Image backpic,backbutpic;
                Image[] spellsymbol = new Image[24];
                BufferedImage bufsrc;
                Color presscolor = new Color(115,115,200);
                Color tooltipcolor = new Color(165,165,250);
                int pressed = 0, over = 0, counter = 5;
                
                public SpellSheet() {
                        setSize(160,80);
                        setPreferredSize(new Dimension(160,80));
                        setMaximumSize(new Dimension(160,80));
                        setCursor(handcursor);
                        backpic = tk.createImage("Interface"+File.separator+"spellsheet.gif");
                        Image src = tk.createImage("Icons"+File.separator+"spell.gif");
                        ImageTracker.addImage(backpic,3);
                        ImageTracker.addImage(src,3);
                        try { ImageTracker.waitForID(3,5000); } catch (InterruptedException e) {}
                        
                        bufsrc = new BufferedImage(src.getWidth(null),src.getHeight(null),BufferedImage.TYPE_INT_ARGB);//was PRE
                        Graphics2D tempg = bufsrc.createGraphics();
                        tempg.drawImage(src,0,0,null);
                        tempg.dispose();
                        
                        int index = 0;
                        for (int j=0;j<4;j++) {
                                for (int i=0;i<6;i++) {
                                        spellsymbol[index]=bufsrc.getSubimage(i*12,j*12,12,12);
                                        index++;
                                }
                        }
                        backbutpic = bufsrc.getSubimage(0,48,17,14);
                        addMouseListener(this);
                        addMouseMotionListener(this);
                }
                
                public void paintComponent(Graphics g) {
                        if (TEXTANTIALIAS) ((Graphics2D)g).setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING,RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
                        g.drawImage(backpic,0,0,null);
						if (hero==null || hero[spellready]==null) return;
                        //draw caster buttons (big button for hero[spellready])
                        int x = 10, width; boolean raised;
                        for (int i=0;i<numheroes;i++) {
                                if (!hero[i].isdead) {
                                        if (spellready==i) { width=95; raised = false; }
                                        else { width=15; raised = true; }
                                        g.setColor(presscolor);
                                        if (!raised) {
                                                g.fill3DRect(x,6,width,20,false);
                                                g.setColor(Color.black);
                                                g.setFont(dungfont);
                                                g.drawString(hero[i].name,x+width/2-g.getFontMetrics().stringWidth(hero[i].name)/2,20);
                                        }
                                        else g.draw3DRect(x,6,width,19,true);
                                }
                                else width=15;
                                x+=(width+1);
                        }
                        //if a button pressed, draw it so
                        if (pressed>0) {
                                g.setColor(presscolor);
                                if (pressed<7) g.fill3DRect(pressed*20+1,30,19,19,false);
                                else if (pressed==7) g.fill3DRect(21,54,98,19,false);
                                else g.fill3DRect(120,54,20,19,false);
                        }
                        //draw spell symbols
                        int index = hero[spellready].currentspell.length(); if (index==4) index=0;
                        for (int i=0;i<6;i++) {
                                g.drawImage(spellsymbol[index*6+i],i*20+24,33,null);
                        }
                        //draw current spell symbols
                        int startx = 70-hero[spellready].currentspell.length()*8;
                        for (int i=0;i<hero[spellready].currentspell.length();i++) {
                                g.drawImage(spellsymbol[Integer.parseInt(hero[spellready].currentspell.substring(i,i+1))+i*6-1],startx+i*15,57,null);
                        }
                        //draw back button
                        g.drawImage(backbutpic,121,57,null);
                        //draw tool tips
                        if (over>0 && counter<=0) {
                                int i = over-1;
                                if (hero[spellready].currentspell.length()<4) i=i+6*hero[spellready].currentspell.length();
                                g.setFont(dungfont);
                                g.setColor(tooltipcolor);
                                g.fillRect(over*20-5,15,g.getFontMetrics().stringWidth(SYMBOLNAMES[i])+4,20);
                                g.setColor(Color.black);
                                g.drawString(SYMBOLNAMES[i],over*20-3,30);
                        }
                }
                
                public void mousePressed(MouseEvent e) {
                        //get x,y to determine button
                        int x = e.getX(), y = e.getY();
                        if (y<25 && y>5 && x>9 && x<150) {
                                //caster button (spellready is 95x20, others are 15x20)
                                x-=10;
                                int who,x1=16,x2=112,x3=128;
                                if (spellready==0) x1=95;
                                else if (spellready==2) x2=32;
                                else if (spellready==3) { x2=32; x3=48; }
                                if (x<x1) who=0;
                                else if (x<x2) who=1;
                                else if (x<x3) who=2;
                                else who=3;
                                if (who<numheroes && !hero[who].isdead) {
                                        //System.out.println("change caster to be "+hero[who].name);
                                        spellready = who;
                                }
                                else return;
                        }
                        else if (x>19 && x<140) {
                                if (y<49 && y>29) {
                                        //spell symbol (20x20)
                                        if (hero[spellready].currentspell.length()<4) pressed = x/20;
                                        else return;
                                        
                                }
                                else if (y<73) {
                                        if (x>118) {
                                                //back button (20x20) - undo last symbol
                                                if (hero[spellready].currentspell.length()>0) pressed = 8;
                                                else return;
                                        }
                                        else {
                                                //cast button (100x20)
                                                if (hero[spellready].currentspell.length()>1) pressed = 7;
                                                else return;
                                        }
                                }
                        }
                        else return;
                        repaint();
                }

                public void mouseReleased(MouseEvent e) {
                        if (pressed==0) return;
                        else if (pressed<7) {
                                //spell symbol - check if enough mana, then reduce it
                                int mananeed = pressed;
                                if (hero[spellready].currentspell.length()>0) mananeed=SYMBOLCOST[(pressed-1)+6*(hero[spellready].currentspell.length()-1)][Integer.parseInt(hero[spellready].currentspell.substring(0,1))-1];
                                if (hero[spellready].mana>=mananeed) {
                                        hero[spellready].mana-=mananeed;
                                        hero[spellready].currentspell+=(""+pressed);

                                        hero[spellready].repaint();
                                        if (sheet && herosheet.hero.equals(hero[spellready])) herosheet.repaint();
                                }
                        }
                        else if (pressed==8) {
                                //back button - undo last symbol
                                hero[spellready].currentspell = hero[spellready].currentspell.substring(0,hero[spellready].currentspell.length()-1);
                        }
                        else {
                                //cast spell button
                                int tester = hero[spellready].castSpell();
                                if (tester==0) {
                                        //nonsense
                                        message.setMessage(hero[spellready].name+" mumbles nonsense.",4);
                                        hero[spellready].currentspell="";
                                }
                                else if (tester==1) {
                                        //success
                                        message.setMessage(hero[spellready].name+" casts a spell.",spellready);
                                        hero[spellready].currentspell="";
                                }
                                else if (tester==2) {
                                        //need flask
                                        message.setMessage(hero[spellready].name+" needs an empty flask in hand.",4);
                                }
                                else if (tester==3) {
                                        //need more practice
                                        message.setMessage(hero[spellready].name+" needs more practice to cast that "+spellclass+" spell.",4);
                                        hero[spellready].currentspell="";
                                }
                                else if (tester==4) {
                                        //some condition not met
                                        message.setMessage(hero[spellready].name+" can't cast that now.",4);
                                }
                                else {
                                        //silenced
                                        message.setMessage(hero[spellready].name+"'s spell fizzles.",4);
                                        hero[spellready].currentspell="";
                                }
                                if (sheet && herosheet.hero.equals(hero[spellready])) herosheet.repaint();
                        }
                        pressed = 0; over = -1; mouseMoved(new MouseEvent(this,MouseEvent.MOUSE_MOVED,0,0,e.getX(),e.getY(),0,false));
                        //if (counter<5) repaint();
                }
                
                public void mouseExited(MouseEvent e) {
                        if (pressed>0) {
                                pressed = 0;
                                repaint();
                        }
                        over = 0;
                }
                
                public void mouseMoved(MouseEvent e) {
                        int x = e.getX(), y = e.getY();
                        int newover = 0;
                        if (x>19 && x<140 && y<49 && y>29) {
                                newover = x/20;
                        }
                        if (over!=newover) {
                                over = newover;
                                counter = 5;
                                repaint();
                        }
                }
                public void timePass() {
                        if (over>0 && counter>0) {
                                counter--;
                                if (counter<=0) repaint();
                                //System.out.println("over = "+over+", counter = "+counter);
                        }
                }
                
                public void mouseDragged(MouseEvent e) {}
                public void mouseClicked(MouseEvent e) {}
                public void mouseEntered(MouseEvent e) {}
        }