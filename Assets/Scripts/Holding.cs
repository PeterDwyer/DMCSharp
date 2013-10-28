using UnityEngine;
using System.Collections;

        class Holding extends JPanel {//JComponent {//Canvas {
                //Image offscreen;
                //Graphics offg;

                public Holding() {
                        //setSize(160,36);
                        setPreferredSize(new Dimension(160,36));
                        //setBackground(Color.black);
                        setOpaque(false);
                }

                public void paintComponent(Graphics g) {
                        //g.clearRect(0,0,this.getSize().width,this.getSize().height);
                        if (iteminhand) {
                                //g.setColor(new Color(60,60,60));
                                //g.fillRect(0,0,32,32);
                                g.drawImage(inhand.pic,0,0,null);
                                if (inhand.cursed>0 && inhand.cursefound) {
                                        Graphics2D g2 = (Graphics2D)g;
                                        Composite ac = g2.getComposite();
                                        g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER,.48f));
                                        g2.setRenderingHint(RenderingHints.KEY_ALPHA_INTERPOLATION,RenderingHints.VALUE_ALPHA_INTERPOLATION_SPEED);
                                        g2.setRenderingHint(RenderingHints.KEY_RENDERING,RenderingHints.VALUE_RENDER_SPEED);
                                        g2.setColor(new Color(200,0,0));
                                        g2.fillRect(0,0,32,32);
                                        g2.setComposite(ac);
                                }
                                //g.setFont(new Font("TimesRoman",Font.BOLD,14));
                                g.setFont(dungfont14);
                                //g.setColor(new Color(100,100,70));
                                if (TEXTANTIALIAS) ((Graphics2D)g).setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING,RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
                                g.setColor(Color.black);
                                g.drawString(inhand.name,39,28);
                                g.setColor(Color.yellow);
                                g.drawString(inhand.name,37,25);
                        }
                }
        }
        