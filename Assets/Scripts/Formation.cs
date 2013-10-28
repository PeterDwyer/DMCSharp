using UnityEngine;
using System.Collections;

        class Formation extends JPanel implements MouseListener {
                Cursor cursor;
                ImageIcon[] heroicons = new ImageIcon[4];
                JLabel[] herolabels = new JLabel[4];
                boolean ischanging = false;
                int oldindex;
                
                public Formation() {
                        setLayout(new GridLayout(2,2));
                        for (int i=0;i<4;i++) {
                                heroicons[i] = new ImageIcon("Icons"+File.separator+"heroicon"+i+".gif");
                                herolabels[i] = new JLabel();
                                if (i<numheroes) herolabels[i].setIcon(heroicons[i]);
                                herolabels[i].setPreferredSize(new Dimension(32,24));
                                herolabels[i].setMinimumSize(new Dimension(32,24));
                                herolabels[i].setMaximumSize(new Dimension(32,24));
                                add(herolabels[i]);
                        }
                        setBackground(Color.black);
                        setBorder(BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED,new Color(60,60,80),new Color(20,20,40)));
                        cursor = handcursor;
                        setCursor(cursor);
                        addMouseListener(this);
                }
                
                public void addNewHero() {
                        //called when a hero is res/reinc or brought back with altar
                        //note: will never be called when ischanging
                        for (int i=0;i<2;i++) {
                                if (heroatsub[i]!=-1) herolabels[i].setIcon(heroicons[heroatsub[i]]);
                                else herolabels[i].setIcon(null);
                        }
                        if (heroatsub[2]!=-1) herolabels[3].setIcon(heroicons[heroatsub[2]]);
                        else herolabels[3].setIcon(null);
                        if (heroatsub[3]!=-1) herolabels[2].setIcon(heroicons[heroatsub[3]]);
                        else herolabels[2].setIcon(null);
                        repaint();
                }
                
                public void mousePressed(MouseEvent e) {
                        int index;
                        int x = e.getX();
                        int y = e.getY();
                        if (y<getHeight()/2) {//this.getSize().height/2) {
                                if (x<getWidth()/2) {//this.getSize().width/2) {
                                        //0,0
                                        index = 0;
                                }
                                else {
                                        //0,1
                                        index = 1;
                                }
                        }
                        else {
                                if (x<getWidth()/2) {//this.getSize().width/2) {
                                        //1,0
                                        index = 3;
                                }
                                else {
                                        //1,1
                                        index = 2;
                                }
                        }
                        if (ischanging) {
                                if (index==oldindex) {
                                        if (index<2) herolabels[index].setIcon(heroicons[heroatsub[index]]);
                                        else if (index==2) herolabels[3].setIcon(heroicons[heroatsub[index]]);
                                        else herolabels[2].setIcon(heroicons[heroatsub[index]]);
                                }
                                else if (heroatsub[index]!=-1) {
                                        if (index<2) herolabels[index].setIcon(heroicons[heroatsub[oldindex]]);
                                        else if (index==2) herolabels[3].setIcon(heroicons[heroatsub[oldindex]]);
                                        else herolabels[2].setIcon(heroicons[heroatsub[oldindex]]);
                                        if (oldindex<2) herolabels[oldindex].setIcon(heroicons[heroatsub[index]]);
                                        else if (oldindex==2) herolabels[3].setIcon(heroicons[heroatsub[index]]);
                                        else herolabels[2].setIcon(heroicons[heroatsub[index]]);
                                        int tempindex = heroatsub[index];
                                        heroatsub[index]=heroatsub[oldindex];
                                        heroatsub[oldindex]=tempindex;
                                        hero[heroatsub[index]].subsquare=index;
                                        hero[heroatsub[oldindex]].subsquare=oldindex;
                                }
                                else {
                                        if (index<2) herolabels[index].setIcon(heroicons[heroatsub[oldindex]]);
                                        else if (index==2) herolabels[3].setIcon(heroicons[heroatsub[oldindex]]);
                                        else herolabels[2].setIcon(heroicons[heroatsub[oldindex]]);
                                        heroatsub[index]=heroatsub[oldindex];
                                        heroatsub[oldindex]=-1;
                                        hero[heroatsub[index]].subsquare=index;
                                }
                                ischanging = false;
                                cursor = handcursor;
                                setCursor(cursor);
                                repaint();
                        }
                        else if (heroatsub[index]!=-1) {
                                oldindex = index;
                                if (index<2) herolabels[index].setIcon(null);
                                else if (index==2) herolabels[3].setIcon(null);
                                else herolabels[2].setIcon(null);
                                cursor = tk.createCustomCursor(heroicons[heroatsub[index]].getImage(),new Point(14,14),"formc");
                                setCursor(cursor);
                                ischanging = true;
                                repaint();
                        }
                }
                public void mouseExited(MouseEvent e) {
                        if (ischanging) {
                                ischanging = false;
                                if (oldindex<2) herolabels[oldindex].setIcon(heroicons[heroatsub[oldindex]]);
                                else if (oldindex==2) herolabels[3].setIcon(heroicons[heroatsub[2]]);
                                else herolabels[2].setIcon(heroicons[heroatsub[3]]);
                                cursor = handcursor;
                                setCursor(cursor);
                                repaint();
                        }
                }
                
                public void mouseEntered(MouseEvent e) {}
                public void mouseClicked(MouseEvent e) {}
                public void mouseReleased(MouseEvent e) {}

        }