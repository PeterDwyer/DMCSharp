using UnityEngine;
using System.Collections;

        class Message extends JPanel {//JComponent {//Canvas {
                Color[] messagecolor = new Color[4];
                String currentmessage[] = { " "," "," ","Welcome" };
                //transient Image offscreen;
                //transient Graphics offg;
                int index = 0;
                int timecounter = 0;
                Color colors[] = new Color[6];

                public Message() {
                        //setSize(662,70);
                        //setPreferredSize(new Dimension(662,70));
                        //setMaximumSize(new Dimension(662,70));
                        setPreferredSize(new Dimension(450,70));
                        setMaximumSize(new Dimension(450,70));
                        //setBackground(Color.black);
                        setOpaque(false);
                        colors[0] = Color.green;
                        colors[1] = Color.yellow;
                        colors[2] = Color.red;//new Color(200,0,200);
                        /*colors[3] = Color.blue;*/
                        colors[3] = new Color(70,70,255);
                        colors[4] = Color.white;
                        colors[5] = Color.red;
                        messagecolor[3] = Color.white;
                }

                public void setMessage(String m,int c) {
                        currentmessage[0] = currentmessage[1];
                        currentmessage[1] = currentmessage[2];
                        currentmessage[2] = currentmessage[3];
                        currentmessage[3] = m;
                        messagecolor[0] = messagecolor[1];
                        messagecolor[1] = messagecolor[2];
                        messagecolor[2] = messagecolor[3];
                        messagecolor[3] = colors[c];
                        repaint();
                }
                
                public void clear() {
                        currentmessage[0] = " ";
                        currentmessage[1] = " ";
                        currentmessage[2] = " ";
                        currentmessage[3] = " ";
                        repaint();
                }
                
                public void timePass() {
                        if (currentmessage[3].equals(" ")) return;
                        timecounter++;
                        if (timecounter>150) {
                                int i=0;
                                while(currentmessage[i].equals(" ")) { i++; }
                                currentmessage[i] = " ";
                                timecounter=0;
                        }
                        repaint();
                }

                public void paintComponent(Graphics g) {
                        //g.clearRect(0,0,this.getSize().width,this.getSize().height);
                        //g.setFont(new Font("TimesRoman",Font.BOLD,12));
                        g.setFont(dungfont);
                        //g.setColor(new Color(70,70,70));
                        g.setColor(Color.black);
                        if (TEXTANTIALIAS) ((Graphics2D)g).setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING,RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
                        g.drawString(currentmessage[0],2,18);
                        g.drawString(currentmessage[1],2,34);
                        g.drawString(currentmessage[2],2,50);
                        g.drawString(currentmessage[3],2,66);
                        g.setColor(messagecolor[0]);
                        g.drawString(currentmessage[0],0,16);
                        g.setColor(messagecolor[1]);
                        g.drawString(currentmessage[1],0,32);
                        g.setColor(messagecolor[2]);
                        g.drawString(currentmessage[2],0,48);
                        g.setColor(messagecolor[3]);
                        g.drawString(currentmessage[3],0,64);
                }
        }