using UnityEngine;
using System.Collections;

public class DMCSharp
{
    static bool CREATEFLAG;
    static Object CREATELOCK = new Object();
    static int SLEEPTIME = 45;
    static int DIFFICULTY = 0;
    static int BRIGHTADJUST = 8;
    static bool NOTRANS;
    static bool NODARK;
    static bool TEXTANTIALIAS = true;
    static bool AUTOMAP,SHOWPARTYMAP;
    static bool PLAYFOOTSTEPS = true;
    static double DungeonViewScale = 1.0;
    static string version = "1.04";
    static int bonesflag = -1;
    static int counter,endcounter,darkcounter,moncycle=1;
    static bool didquit = false;
    static bool gameover = false;
    static bool alldead = false;
    static bool waitcredits = true;
    static string endanim = "fuseend.gif";//anim for multiple endings
    static string endsound = "fuseend.wav";//sound for multiple endings
    static int[] leveldarkfactor; //base darkness of each level - darkfactor can't go below, 255 means full brightness, 15 probably lowest should ever go
    static string[] leveldir;
    static ArrayList leveldirloaded;
    static int leveldark,numillumlets=0;
    static int darkfactor = 255; //how dark is it - uses alpha so a bit slow, deactivated with nodark
    static int magictorch = 0; //how much brightness caused by magic torch spell
    static int numheroes = 0; //incremented when a hero ress/reinc
    static int herolookx,herolooky;//,eventdoneflag;
    static int magicvision = 0;
    static int floatcounter = 0;
    static int dispell = 0;
    static int slowcount = 0;
    static int freezelife = 0;
    static bool herolook,allowswap;//,eventflag;
    static bool nomovement; //mons,projs,& heroes don't change (during game load/save & mirrorsheet)
    static bool mapchanging;
    static bool cloudchanging;
    static bool fluxchanging;
    static Collection mapstochange = new ArrayList(5);
    static Collection cloudstochange = new ArrayList(3);
    static HashMap fluxcages = new HashMap(10);
    static Random randGen;
    static int sleeper;
    static bool climbing = false;
    static bool doorkeyflag = false;
    bool sleeping = false;
    bool viewing = false;
    bool gamefrozen = false;
    static JLabel freezelabel,loadinglabel,endiconlabel,animlabel,creditslabel;
    static JPanel maincenterpan,centerpanel,toppanel,freezelabelpan,loadinglabelpan,endiconlabelpan,animlabelpan,creditslabelpan,buttonpanel,eventpanel;
    static JScrollPane mappane;
    static MyMapPanel vspacebox;
    static Box hspacebox;
    static Font dungfont,dungfont14,scrollfont;
    static bool needredraw = false;
    static bool needdrawdungeon = true;
    int spellready = 0;
    int weaponready = 0;
    string spellclass = null;
    static int leader = 0;
    static int walkdcount = 2,walkdelay = 2;//was 3
    static ArrayList walkqueue = new ArrayList(4);
    static ArrayList weaponqueue = new ArrayList(2);
    static ArrayList actionqueue = new ArrayList(1);
    static ArrayList clickqueue = new ArrayList(1);
    static ArrayList loopsounds = new ArrayList(2);
    static int footstepcount = 0;

    static Image back;
    static Image doortrack;
    static Image altaranim;
    static Image mondeathpic;
    static Image[] cloudpic = new Image[3];
    static Image cloudpicin;
    static Image hsheet;
    static Image openchest;
    static Image poisonedpic;
    static Image scrollpic;
    static Image automappic;
    static Image deadheropic;
    static Image resreincpic;
    static Image hurtweapon,hurthand,hurthead,hurttorso,hurtlegs,hurtfeet;
    static Image[][] cagepic;
    static HashMap mondarkpic = new HashMap();
    static int[][] cagex = { {0,0,384},{0,64,328},{0,120,298},{0,148,278} };
    static int[] cagey = { 0,22,50,62 };

    static MediaTracker ImageTracker;
    static Toolkit tk = Toolkit.getDefaultToolkit();
    Cursor dmcursor;
    Cursor handcursor = new Cursor(Cursor.HAND_CURSOR);
    File gamefile; //current saved game
    static JFileChooser chooser;
    static File workingdir;
    static DMMap dmmap = null;
    OptionsDialog optionsdialog;

    static int trywidth, tryheight;//, trybitdepth, tryrefresh;

    static DungView dview;
    DungMove dmove;
    DungClick dclick;
    WeaponSheet weaponsheet;
    SpellSheet spellsheet;
    ArrowSheet arrowsheet;
    static Message message;
    Formation formation;
    Holding holding;
    static int compassface;
    static ImageTilePanel imagePane = new ImageTilePanel("Interface"+File.separator+"bluerock.gif");
    JPanel hpanel = new JPanel(false);
    JPanel eastpanel = new JPanel(false);
    JPanel ecpanel = new JPanel(false);
    static JPanel realcenterpanel = new JPanel(false);
    static Hero[] hero = new Hero[4];
    static Hero mirrorhero;
    HeroClick hclick;
    SheetClick sheetclick;
    static HeroSheet herosheet;
    static bool sheet;

    static int mapwidth;
    static int mapheight;
    static int numlevels;
    static MapObject[][][] DungeonMap;

    MapObject[][] visibleg = new MapObject[4][5];
    static Wall outWall;
    static InvisibleWall invisWall;
 
    static char forwardkey = 'w', backkey='s', leftkey='a', rightkey='d', turnleftkey='q', turnrightkey='e';

    static int partyx;
    static int partyy;
    static int level;
    static int facing = 0;
    static int[] heroatsub = { -1,-1,-1,-1 };
    static bool mirrorback,nomirroradjust;
    static bool iteminhand = false;
    static Item inhand = null;
    static Item fistfoot = Item.fistfoot;

    static ArrayList dmprojs = new ArrayList(4);
    static Hashtable dmmons = new Hashtable(23);

    static string[] SYMBOLNAMES = {
            "LO","UM","ON","EE","PAL","MON",
            "YA","VI","OH","FUL","DES","ZO",
            "VEN","EW","KATH","IR","BRO","GOR",
            "KU","ROS","DAIN","NETA","RA","SAR" };
    static string[] LEVELNAMES = {
            "Neophyte","Novice","Apprentice","Journeyman","Craftsman","Artisan","Adept","Expert",
            "LO Master","UM Master","ON Master","EE Master","PAL Master","MON Master","ArchMaster" };
    static int NORTH=0;
    static int WEST=1;
    static int SOUTH=2;
    static int EAST=3;
    static Integer IFORWARD = new Integer(0);
    static Integer IBACK = new Integer(1);
    static Integer ILEFT = new Integer(2);
    static Integer IRIGHT = new Integer(3);
    static Integer ITURNLEFT = new Integer(4);
    static Integer ITURNRIGHT = new Integer(5);
    static int FORWARD = 0;
    static int BACK = 1;
    static int LEFT = 2;
    static int ITEM=0;
    static int SPELL=1;
    static int WEAPONHIT = 0;
    static int SPELLHIT = 1;
    static int POISONHIT = 2;
    static int DOORHIT = 3; //door bashing head
    static int STORMHIT = 4; //stormbringer feeds
    static int PROJWEAPONHIT = 5;
    static int FUSEHIT = 6; //fusing chaos
    static int DRAINHIT = 7; //drain life
    static int[][] SYMBOLCOST = {
            {2,3,4, 5, 6, 7},{3,4, 6, 7, 9,10},{4,6, 8,10,12,14},{5, 7,10,12,15,17},{6, 9,12,15,18,21},{7,10,14,17,21,24},
            {4,6,8,10,12,14},{5,7,10,12,15,17},{6,9,12,15,18,21},{7,10,14,17,21,24},{7,10,14,17,21,24},{9,13,18,22,27,31},
            {2,3,4, 5, 6, 7},{2,3, 4, 5, 6, 7},{3,4, 6, 7, 9,10},{4, 6, 8,10,12,14},{6, 9,12,15,18,21},{7,10,14,17,21,24}};

    // Ask AWT which menu modifier we should be using.
    static int MENU_MASK = Toolkit.getDefaultToolkit().getMenuShortcutKeyMask();

	/// <summary>
	/// Initializes a new instance of the <see cref="DMCSharp"/> class.
	/// </summary>
    private DMCSharp()
    {

        pack();
        imagePane.setLayout(new BoxLayout(imagePane,BoxLayout.Y_AXIS));
        imagePane.setBackground(Color.black);
        setContentPane(imagePane);
        sleeper = 0;
        ImageTracker = new MediaTracker(this);
        MapObject.tracker = ImageTracker;
        randGen = new Random();
        outWall = new Wall();
        invisWall = new InvisibleWall();
        outWall.canPassImmaterial = false;
        invisWall.canPassImmaterial = false;
        back = tk.getImage("Maps"+File.separator+"back.gif");
        doortrack = tk.getImage("Maps"+File.separator+"doortrack.gif");
        altaranim = tk.createImage("Maps"+File.separator+"altaranim.gif");//was getImage 7/2/01, not sure why
        mondeathpic = tk.createImage("Monsters"+File.separator+"mondeath.png");
        cloudpic[0] = tk.createImage("Spells"+File.separator+"poisoncloud1.gif");
        cloudpic[1] = tk.createImage("Spells"+File.separator+"poisoncloud2.gif");
        cloudpic[2] = tk.createImage("Spells"+File.separator+"poisoncloud3.gif");
        cloudpicin = tk.createImage("Spells"+File.separator+"poisoncloud0.gif");
        hsheet = tk.createImage("Interface"+File.separator+"hsheet.gif");
        openchest = tk.createImage("Interface"+File.separator+"openchest.gif");
        poisonedpic = tk.createImage("Interface"+File.separator+"poisoned.gif");
        scrollpic = tk.createImage("Interface"+File.separator+"scroll.gif");
        resreincpic = tk.createImage("Interface"+File.separator+"resreinc.gif");
        automappic = tk.createImage("Icons"+File.separator+"automappic.png");
        deadheropic = tk.createImage("Icons"+File.separator+"dead.gif");
        hurtweapon = tk.createImage("Icons"+File.separator+"hurt_weapon.gif");
        hurthand= tk.createImage("Icons"+File.separator+"hurt_hand.gif");
        hurthead = tk.createImage("Icons"+File.separator+"hurt_head.gif");
        hurttorso = tk.createImage("Icons"+File.separator+"hurt_torso.gif");
        hurtlegs = tk.createImage("Icons"+File.separator+"hurt_legs.gif");
        hurtfeet = tk.createImage("Icons"+File.separator+"hurt_feet.gif");
        ImageTracker.addImage(back,0);
        ImageTracker.addImage(doortrack,1);
        ImageTracker.addImage(altaranim,8);
        ImageTracker.addImage(mondeathpic,1);
        ImageTracker.addImage(cloudpic[0],1);
        ImageTracker.addImage(cloudpic[1],1);
        ImageTracker.addImage(cloudpic[2],1);
        ImageTracker.addImage(cloudpicin,1);
        ImageTracker.addImage(hsheet,1);
        ImageTracker.addImage(openchest,1);
        ImageTracker.addImage(poisonedpic,1);
        ImageTracker.addImage(scrollpic,1);
        ImageTracker.addImage(automappic,1);
        ImageTracker.addImage(resreincpic,1);
        ImageTracker.addImage(hurtweapon,1);
        ImageTracker.addImage(hurthand,1);
        ImageTracker.addImage(hurthead,1);
        ImageTracker.addImage(hurttorso,1);
        ImageTracker.addImage(hurtlegs,1);
        ImageTracker.addImage(hurtfeet,1);
        cagepic = new Image[4][3];

        for (int j=0;j<4;j++)
        {
            for (int i=0;i<3;i++)
            {
                cagepic[j][i] = tk.getImage("Maps"+File.separator+"fluxcage"+j+""+(i+1)+".gif");
                ImageTracker.addImage(cagepic[j][i],1);
            }
        }
        
        dmove = new DungMove();
        setBackground(Color.black);
        dmcursor = handcursor;
        hclick = new HeroClick();
        hpanel.setOpaque(false);
        hpanel.setMinimumSize(new Dimension(420,132));
        hpanel.setMaximumSize(new Dimension(420,132));
        hpanel.setPreferredSize(new Dimension(420,132));
     
        message = new Message();
        formation = new Formation();
        holding = new Holding();
        arrowsheet = new ArrowSheet();
        toppanel = new JPanel();
        toppanel.setOpaque(false);
        toppanel.add(hpanel);
        toppanel.setPreferredSize(new Dimension(662,136));
        toppanel.setMaximumSize(new Dimension(662,136));
        toppanel.setBackground(Color.black);
        toppanel.add(Box.createHorizontalStrut(20));
        toppanel.add(formation);

        eastpanel.setOpaque(false);
        eastpanel.setBackground(Color.black);
        eastpanel.setLayout(new BoxLayout(eastpanel,BoxLayout.Y_AXIS));
        eastpanel.setPreferredSize(new Dimension(183,374));
        eastpanel.setMaximumSize(new Dimension(183,374));
        eastpanel.setMinimumSize(new Dimension(183,374));

        ecpanel.setOpaque(false);
        ecpanel.setBackground(Color.black);
        ecpanel.setPreferredSize(new Dimension(160,40));
        ecpanel.setMaximumSize(new Dimension(160,40));
        ecpanel.add(holding);
        eastpanel.add(ecpanel);
        eastpanel.add(Box.createVerticalStrut(10));
        eastpanel.setBorder(BorderFactory.createEmptyBorder(0,0,0,10));
        eastpanel.add(Box.createVerticalGlue());
        eastpanel.add(arrowsheet);
        eastpanel.add(Box.createVerticalStrut(20));
        eastpanel.setBackground(Color.black);
        
        Dimension centerdim = new Dimension((int)(448 * DungeonViewScale), (int)(326 * DungeonViewScale));
        Dimension centerdim2 = new Dimension((int)(452 * DungeonViewScale), (int)(330 * DungeonViewScale));

        dview = new DungView();
        dview.setSize(centerdim);
        dview.setPreferredSize(centerdim);
        dview.setMinimumSize(centerdim);
        dview.setMaximumSize(centerdim);
        dclick = new DungClick();
        dview.addMouseListener(dclick);
        maincenterpan = new JPanel(false);
        maincenterpan.setBorder(BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED,new Color(60,60,80),new Color(20,20,40)));
        maincenterpan.setBackground(Color.black);
        maincenterpan.setLayout(new BorderLayout());
        maincenterpan.setPreferredSize(centerdim2);
        maincenterpan.setMinimumSize(centerdim2);
        maincenterpan.setMaximumSize(centerdim2);
        sheetclick = new SheetClick();
        herosheet = new HeroSheet();
        herosheet.setSize(centerdim);
        herosheet.setPreferredSize(centerdim);
        herosheet.setMinimumSize(centerdim);
        herosheet.setVisible(false);
        BufferedImage freezepic = new BufferedImage(200,100,BufferedImage.TYPE_3BYTE_BGR);
        Graphics freezeg = freezepic.getGraphics();
        freezeg.setFont(dungfont14);
        freezeg.setColor(new Color(70,70,70));
        freezeg.drawstring("Game Frozen",63,63);
        freezeg.setColor(Color.white);
        freezeg.drawstring("Game Frozen",60,60);
        freezelabel = new JLabel(new ImageIcon(freezepic));
        freezelabel.setBackground(Color.black);
        freezelabelpan = new JPanel();
        freezelabelpan.setSize(centerdim.width,centerdim.height);
        freezelabelpan.setPreferredSize(centerdim);
        freezelabelpan.setMinimumSize(centerdim);
        freezelabelpan.setBackground(Color.black);
        freezelabelpan.add(freezelabel);
        freezelabelpan.setVisible(false);
        BufferedImage loadingpic = new BufferedImage(200,100,BufferedImage.TYPE_3BYTE_BGR);
        Graphics loadingg = loadingpic.getGraphics();
        loadingg.setFont(dungfont14);
        loadingg.setColor(new Color(70,70,70));
        loadingg.drawstring("Loading Game...",63,63);
        loadingg.setColor(Color.white);
        loadingg.drawstring("Loading Game...",60,60);
        JLabel loadinglabel = new JLabel(new ImageIcon(loadingpic));
        loadinglabel.setBackground(Color.black);
        loadinglabelpan = new JPanel();
        loadinglabelpan.setSize(centerdim);
        loadinglabelpan.setPreferredSize(centerdim);
        loadinglabelpan.setMinimumSize(centerdim);
        loadinglabelpan.setBackground(Color.black);
        loadinglabelpan.add(loadinglabel);
        loadinglabelpan.setVisible(false);
        endiconlabel = new JLabel();
        endiconlabel.setBackground(Color.black);
        endiconlabel.setSize(centerdim);
        endiconlabel.setPreferredSize(centerdim);
        endiconlabel.setMinimumSize(centerdim);
        endiconlabel.setMaximumSize(centerdim);
        endiconlabel.setHorizontalAlignment(JLabel.CENTER);
        endiconlabelpan = new JPanel();
        endiconlabelpan.setLayout(null);
        endiconlabelpan.setSize(centerdim);
        endiconlabelpan.setPreferredSize(centerdim);
        endiconlabelpan.setMinimumSize(centerdim);
        endiconlabelpan.setMaximumSize(centerdim);
        endiconlabelpan.setBackground(Color.black);
        endiconlabelpan.add(endiconlabel);
        endiconlabelpan.setVisible(false);
        eventpanel = new JPanel();
        eventpanel.setLayout(null);
        eventpanel.setBackground(Color.black);
        eventpanel.setSize(centerdim);
        eventpanel.setPreferredSize(centerdim);
        eventpanel.setMinimumSize(centerdim);
        eventpanel.setMaximumSize(centerdim);
        eventpanel.setVisible(false);
        animlabel = new JLabel();
        animlabel.setBackground(Color.black);
        animlabel.setSize(centerdim);
        animlabel.setPreferredSize(centerdim);
        animlabel.setMinimumSize(centerdim);
        animlabel.setMaximumSize(centerdim);
        animlabel.setHorizontalAlignment(JLabel.CENTER);
        animlabelpan = new JPanel();
        animlabelpan.setLayout(null);
        animlabelpan.setSize(centerdim);
        animlabelpan.setPreferredSize(centerdim);
        animlabelpan.setMinimumSize(centerdim);
        animlabelpan.setMaximumSize(centerdim);
        animlabelpan.setBackground(Color.black);
        animlabelpan.add(animlabel);
        animlabelpan.setVisible(false);
        maincenterpan.add(dview);
        maincenterpan.add(herosheet);
        maincenterpan.add(freezelabelpan);
        maincenterpan.add(loadinglabelpan);
        maincenterpan.add(endiconlabelpan);
        maincenterpan.add(eventpanel);
        maincenterpan.add(animlabelpan);
        JButton loadbutton = new JButton("Load Game");
        JButton newbutton = new JButton("New Game");
        JButton custombutton = new JButton("New Custom");
        JButton exitbutton = new JButton("Quit");
        loadbutton.addActionListener(this);
        newbutton.addActionListener(this);
        custombutton.addActionListener(this);
        exitbutton.addActionListener(this);
        loadbutton.setOpaque(false);
        newbutton.setOpaque(false);
        custombutton.setOpaque(false);
        exitbutton.setOpaque(false);
        buttonpanel = new JPanel();
        buttonpanel.setOpaque(false);
        buttonpanel.setBackground(new Color(0,0,64));
        buttonpanel.setSize(450,70);
        buttonpanel.setPreferredSize(new Dimension(450,70));
        buttonpanel.setMaximumSize(new Dimension(450,70));
        buttonpanel.add(loadbutton);
        buttonpanel.add(newbutton);
        buttonpanel.add(custombutton);
        buttonpanel.add(exitbutton);
        buttonpanel.setVisible(false);
        realcenterpanel.setLayout(new BoxLayout(realcenterpanel,BoxLayout.Y_AXIS));
        realcenterpanel.setOpaque(false);
        realcenterpanel.add(maincenterpan);
        realcenterpanel.add(message);
        realcenterpanel.add(buttonpanel);

        centerpanel = new JPanel();
        centerpanel.setOpaque(false);
        centerpanel.add(realcenterpanel);
        centerpanel.add(eastpanel);

        creditslabel = new JLabel();
        creditslabelpan = new JPanel();
        creditslabelpan.setOpaque(false);
        creditslabelpan.add(creditslabel);
        creditslabelpan.addMouseListener(this);
        creditslabelpan.setVisible(false);

        hspacebox = Box.createHorizontalBox();
        hspacebox.setOpaque(false);
        hspacebox.setBackground(Color.black);
        vspacebox = new MyMapPanel();
        vspacebox.setBackground(Color.black);
        vspacebox.add(hspacebox);
        vspacebox.add(Box.createVerticalGlue());
        mappane = new JScrollPane(vspacebox);
        mappane.setBackground(Color.black);
        mappane.setVisible(false);
        
        imagePane.setBackground(Color.black);
        imagePane.add(Box.createVerticalGlue());
        imagePane.add(toppanel);
        imagePane.add(creditslabelpan);
        imagePane.add(mappane);
        imagePane.add(centerpanel);
        imagePane.add(Box.createVerticalGlue());
        
        dview.offscreen = new BufferedImage(448,326,BufferedImage.TYPE_3BYTE_BGR);
        dview.offg = dview.offscreen.createGraphics();
        dview.offg.setRenderingHint(RenderingHints.KEY_ALPHA_INTERPOLATION,RenderingHints.VALUE_ALPHA_INTERPOLATION_SPEED);
        dview.offg.setRenderingHint(RenderingHints.KEY_RENDERING,RenderingHints.VALUE_RENDER_SPEED);

        dview.offg2 = dview.offscreen.createGraphics();
        dview.offg2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER,.38f));
        dview.offg2.setRenderingHint(RenderingHints.KEY_ALPHA_INTERPOLATION,RenderingHints.VALUE_ALPHA_INTERPOLATION_SPEED);
        dview.offg2.setRenderingHint(RenderingHints.KEY_RENDERING,RenderingHints.VALUE_RENDER_SPEED);
        
        herosheet.offscreen = createImage(448,326);
        herosheet.offg = (Graphics2D)herosheet.offscreen.getGraphics();
        herosheet.curseg = (Graphics2D)herosheet.offscreen.getGraphics();
        herosheet.curseg.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER,.48f));
        herosheet.curseg.setRenderingHint(RenderingHints.KEY_ALPHA_INTERPOLATION,RenderingHints.VALUE_ALPHA_INTERPOLATION_SPEED);
        herosheet.curseg.setRenderingHint(RenderingHints.KEY_RENDERING,RenderingHints.VALUE_RENDER_SPEED);
        herosheet.curseg.setColor(new Color(200,0,0));

		if (TEXTANTIALIAS)
		{
			herosheet.offg.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING,RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
		}

		requestFocusInWindow();
        chooser = new JFileChooser();
        workingdir = new File(System.getProperty("user.dir"));

        Title t = new Title(this);
        JPanel tpan = new JPanel();
        tpan.setLayout(new BoxLayout(tpan,BoxLayout.Y_AXIS));
        tpan.setBackground(new Color(0,0,64));
        tpan.add(Box.createVerticalGlue());
        tpan.add(t);
        tpan.add(Box.createVerticalGlue());
        setContentPane(tpan);
        setIconImage(tk.getImage("Icons"+File.separator+"dmjicon.gif"));
        ecpanel.setSize(90,40);
        this.setCursor(dmcursor);

        if (trywidth != 0)
		{
			setSize(trywidth,tryheight);
		}
		else
		{
			setSize(700,600);//was 700,584
		}

		setLocation(tk.getScreenSize().width/2-getSize().width/2,tk.getScreenSize().height/2-getSize().height/2-10);
        show();
        message.repaint();
        optionsdialog = new OptionsDialog(this);


        Spell.doPics();
	}

	/// <summary>
	/// Loads the map.
	/// </summary>
	/// <returns>
	/// The map.
	/// </returns>
	/// <param name='mapfile'>
	/// If set to <c>true</c> mapfile.
	/// </param>
    public bool loadMap(File mapfile)
	{
        //load the map for a new game
        try
		{

			message.clear();


			FileInputStream inStream = new FileInputStream(mapfile);

			ObjectInputStream si = new ObjectInputStream(inStream );


			//System.out.println("Opened File");

			//skip over map start info
	        si.readUTF();//version
	        bool create = si.readbool();
	        si.readbool();

	        if (create)
			{
                si.readInt();
                si.readInt();
                si.readInt();
                si.readInt();
                int num=si.readInt();

                if (num>0)
				{
                	for (int i=0;i<num;i++)
					{
                    	si.readObject();
                   	}

					si.readInt();
                }

                num=si.readInt();

                if (num>0)
				{
                    si.readInt();

                    for (int i=0;i<num;i++)
					{
                            new SpecialAbility(si);
                   	}

					si.readInt();
               	}
            }

            //global stuff
            counter = si.readInt();
            leveldarkfactor = (int[])si.readObject();
            darkcounter = si.readInt();
            darkfactor = si.readInt();
            magictorch = si.readInt();
            magicvision = si.readInt();
            floatcounter = si.readInt();
            dispell = si.readInt();
            slowcount = si.readInt();
            freezelife = si.readInt();
            mapchanging = si.readbool();
            cloudchanging = si.readbool();
            fluxchanging = si.readbool();
            level = si.readInt();
            partyx = si.readInt();
            partyy = si.readInt();
            facing = si.readInt();
            leader = si.readInt();
            si.readObject();//heroatsub
            iteminhand = si.readbool();
            if (iteminhand) inhand = (Item)si.readObject();
            spellready = si.readInt();
            weaponready = si.readInt();
            mirrorback = si.readbool();
            compassface = facing;
            
            //monsters
            int nummons = si.readInt();
            int monnum;
            bool isdying;
            Monster tempmon;

            for (int i=0;i<nummons;i++)
			{
                isdying = si.readbool();
                monnum = si.readInt();

                if (monnum<28)
				{
					tempmon = new Monster(monnum,si.readInt(),si.readInt(),si.readInt());
				}
				else
				{
					tempmon = new Monster(monnum,si.readInt(),si.readInt(),si.readInt(),si.readUTF(),si.readUTF(),si.readUTF(),si.readUTF(),si.readbool(),si.readbool(),si.readbool(),si.readbool());
				}

				tempmon.subsquare = si.readInt();
                tempmon.health = si.readInt();
                tempmon.maxhealth = si.readInt();
                tempmon.mana = si.readInt();
                tempmon.maxmana = si.readInt();
                tempmon.facing = si.readInt();
                tempmon.currentai = si.readInt();
                tempmon.defaultai = si.readInt();
                tempmon.HITANDRUN = si.readbool();
                tempmon.isImmaterial = si.readbool();
                tempmon.wasfrightened = si.readbool();
                tempmon.hurt = si.readbool();
                tempmon.wasstuck = si.readbool();
                tempmon.ispoisoned = si.readbool();

                if (tempmon.ispoisoned)
				{
                	tempmon.poisonpow = si.readInt();
                	tempmon.poisoncounter = si.readInt();
                }

                tempmon.timecounter = si.readInt();
                tempmon.movecounter = si.readInt();
                tempmon.randomcounter = si.readInt();
                tempmon.runcounter = si.readInt();
                tempmon.carrying = (ArrayList)si.readObject();

                if (si.readbool())
				{
					tempmon.equipped = (ArrayList)si.readObject();
				}

				tempmon.gamewin = si.readbool();

				if (tempmon.gamewin)
				{
					tempmon.endanim = si.readUTF();
					tempmon.endsound = si.readUTF();
				}

                tempmon.hurtitem = si.readInt();
                tempmon.needitem = si.readInt();
                tempmon.needhandneck = si.readInt();

                tempmon.power = si.readInt();
                tempmon.defense = si.readInt();
                tempmon.magicresist = si.readInt();
                tempmon.speed = si.readInt();
                tempmon.movespeed = si.readInt();
                tempmon.attackspeed = si.readInt();
                tempmon.poison = si.readInt();
                tempmon.fearresist = si.readInt();
                tempmon.hasmagic = si.readbool();

                if (tempmon.hasmagic)
				{
                    tempmon.castpower = si.readInt();
                    tempmon.manapower = si.readInt();
                    tempmon.numspells = si.readInt();

                    if (tempmon.numspells>0)
					{
						tempmon.knownspells = (string[])si.readObject();
					}
					else
					{
						tempmon.hasmagic = false;
					}

					tempmon.minproj = si.readInt();
                    tempmon.hasheal = si.readbool();
                    tempmon.hasdrain = si.readbool();
                }

                tempmon.useammo = si.readbool();

                if (tempmon.useammo)
				{
					tempmon.ammo = si.readInt();
				}

				tempmon.pickup = si.readInt();
                tempmon.steal = si.readInt();
                tempmon.poisonimmune = si.readbool();
                tempmon.powerboost = si.readInt();
                tempmon.defenseboost = si.readInt();
                tempmon.magicresistboost = si.readInt();
                tempmon.speedboost = si.readInt();
                tempmon.manapowerboost = si.readInt();
                tempmon.movespeedboost = si.readInt();
                tempmon.attackspeedboost = si.readInt();
                tempmon.silenced = si.readbool();

                if (tempmon.silenced)
				{
					tempmon.silencecount = si.readInt();
				}

				tempmon.isdying = isdying;

                dmmons.put(tempmon.level+","+tempmon.x+","+tempmon.y+","+tempmon.subsquare,tempmon);
        	}

            //projectiles
            int numprojs = si.readInt();
            bool type,isending;
            Projectile tempproj;

            for (int i=0;i<numprojs;i++)
			{
                isending = si.readbool();
                type = si.readbool();

                if (type)
				{
					tempproj = new Projectile((Item)si.readObject(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readbool());
				}
				else
				{
					tempproj = new Projectile((Spell)si.readObject(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readbool());
				}

				tempproj.isending = isending;
                dmprojs.add(tempproj);
            }
            //System.out.println("projs loaded\n");

            //heroes
            si.readInt();

            //mapObjects
            int oldlevels = numlevels, oldmapwidth = mapwidth, oldmapheight = mapheight;
            //System.out.println(oldlevels+","+oldmapwidth+","+oldmapheight);
            numlevels = si.readInt();
            mapwidth = si.readInt();
            mapheight = si.readInt();
            MapObject[][][] oldmapobject = DungeonMap;
            MapObject oldmap;
            DungeonMap = new MapObject[numlevels][mapwidth][mapheight];
            //System.out.println("loading map: levels = "+numlevels+",width = "+mapwidth+",height = "+mapheight);

            for (int l=0;l<numlevels;l++)
			{
                for (int x=0;x<mapwidth;x++)
				{
                    for (int y=0;y<mapheight;y++)
					{
                        if (l<oldlevels && x<oldmapwidth && y<oldmapheight) oldmap = oldmapobject[l][x][y];
                        else oldmap = null;
                        DungeonMap[l][x][y] = loadMapObject(si,oldmap);
                    }
                }
                //System.out.println("level "+(l+1)+" loaded");
            }

            oldmapobject = null;
            //System.out.println("loading changing");
            if (mapchanging)
			{
               	int numchanging = si.readInt();

               	for (int i=0;i<numchanging;i++)
				{
                    mapstochange.add(si.readObject());
               	}
            }

            //fake load ambient sounds here
            si.readInt();

            //fake load dmmap here
            si.readbool();//dungeon files will have no automap to load

            if (dmmap!=null)
			{
                dmmap.setMap(numlevels,mapwidth,mapheight,null);
                dmmap.invalidate();
                vspacebox.invalidate();
                mappane.validate();

                if (mappane.isVisible())
				{
                    validate();
                    repaint();
                }
            }
            optionsdialog.resetOptions();
            
            //load map picture directory modifier
            leveldir = new string[numlevels];
            for (int l=0;l<numlevels;l++)
			{
                leveldir[l] = si.readUTF();

                if (leveldir[l].equals(""))
				{
					leveldir[l]=null;
            	}
			}

            //System.out.println("Map loaded, waiting on pics");
            inStream.close();
            ImageTracker.waitForID(0,10000);//map pics
            ImageTracker.waitForID(1,10000);//some interface pics
            Item.doFlaskBones();
            Item.ImageTracker.waitForID(0,8000);
            Item.ImageTracker = null;
            Item.ImageTracker = new MediaTracker(this);
            System.gc();
            System.runFinalization();
            //System.out.println("Done waiting, pictures loaded.");
            updateDark();
        }
        //catch (InterruptedException e) { System.out.println("Interrupted!"); return true; }
        catch (Exception e)
		{
        	message.setMessage("Unable to load map!",4);

            //pop up a dialog too
            e.printStackTrace();

            if (!this.isShowing())
			{
				JOptionPane.showMessageDialog(this, "Unable to load map!", "Error!", JOptionPane.ERROR_MESSAGE);
			}
			else
			{
				JOptionPane.showMessageDialog(this, "Unable to load map!\nApplication may be corrupted.\nLoad another or exit and restart.", "Error!", JOptionPane.ERROR_MESSAGE);
			}

			return false;
        }
        return true;
    }

	/// <summary>
	/// Starts the new.
	/// </summary>
	public void startNew()
	{
        //set up stuff for a new game
        numheroes=1;
        heroatsub[0]=0;
        hero[0].isleader=true;
        hero[0].removeMouseListener(hclick);
        hero[0].addMouseListener(hclick);
        hpanel.add(hero[0]);
        spellsheet = new SpellSheet();
        weaponsheet = new WeaponSheet();
        eastpanel.removeAll();
        eastpanel.add(ecpanel);
        eastpanel.add(Box.createVerticalStrut(10));
        eastpanel.add(spellsheet);
        eastpanel.add(Box.createVerticalStrut(20));
        eastpanel.add(weaponsheet);
        eastpanel.add(Box.createVerticalStrut(10));
        eastpanel.add(arrowsheet);
        formation.addNewHero();
        hero[0].repaint();
        spellsheet.repaint();
        weaponsheet.repaint();
        herosheet.removeMouseListener(sheetclick);
        herosheet.addMouseListener(sheetclick);
        updateDark();
 	}

	/// <summary>
	/// Main the specified args.
	/// </summary>
	/// <param name='args'>
	/// Arguments.
	/// </param>
	public static void main(string[] args)
	{
        for (int j=args.length;j>0;j--)
		{
                if (args[j-1].equals("-notrans")) dmnew.NOTRANS=true;
                else if (args[j-1].equals("-nodark")) dmnew.NODARK=true;
                else if (args[j-1].equals("-noaa")) dmnew.TEXTANTIALIAS=false;
        }

        try
		{
            FileInputStream inStream = new FileInputStream("Fonts"+File.separator+"gamefont.ttf");
            dungfont = Font.createFont(Font.TRUETYPE_FONT,inStream);
            inStream.close();
            dungfont = dungfont.deriveFont(Font.BOLD,12);
        }
        catch (Exception e)
		{
                e.printStackTrace();
                dungfont = new Font("SansSerif",Font.BOLD,12);
        }

		try
		{
            FileInputStream inStream = new FileInputStream("Fonts"+File.separator+"scrollfont.ttf");
            scrollfont = Font.createFont(Font.TRUETYPE_FONT,inStream);
            inStream.close();
            scrollfont = scrollfont.deriveFont(Font.BOLD,16);
        }
        catch (Exception ex)
		{
            ex.printStackTrace();
            scrollfont = new Font("Serif",Font.BOLD,16);
        }

     	System.setProperty("apple.laf.useScreenMenuBar", "true");
        dungfont14 = dungfont.deriveFont(14.0f);

        //read config file
        File cfg = new File("dmnew.cfg");

        if (cfg.exists())
		{
            try
			{
                BufferedReader r = new BufferedReader(new FileReader(cfg));
                string temps=r.readLine();
                while (temps!=null) {
                    temps = temps.toLowerCase();
                if (temps.startsWith(";"))
				{
				}
                 else if (temps.startsWith("forward")) {
                     forwardkey = temps.charAt(temps.indexOf(' ')+1);
                 }
                 else if (temps.startsWith("back")) {
                     backkey = temps.charAt(temps.indexOf(' ')+1);
                 }
                 else if (temps.startsWith("left")) {
                     leftkey = temps.charAt(temps.indexOf(' ')+1);
                 }
                 else if (temps.startsWith("right")) {
                     rightkey = temps.charAt(temps.indexOf(' ')+1);
                 }
                 else if (temps.startsWith("turnleft")) {
                     turnleftkey = temps.charAt(temps.indexOf(' ')+1);
                 }
                 else if (temps.startsWith("turnright")) {
                     turnrightkey = temps.charAt(temps.indexOf(' ')+1);
                 }
                 else if (temps.startsWith("width")) {
                     trywidth = Integer.parseInt(temps.substring(temps.indexOf(' ')+1));
                 }
                 else if (temps.startsWith("height")) {
                     tryheight = Integer.parseInt(temps.substring(temps.indexOf(' ')+1));
                 }
                    else if (temps.startsWith("speed")) {
                            int i = Integer.parseInt(temps.substring(6));
                            if (i<10) i=10;
                            else if (i>100) i=100;
                            SLEEPTIME = i;
                    }
                    else if (temps.startsWith("difficulty")) {
                            int i = Integer.parseInt(temps.substring(11));
                            if (i<-2) i=-2;
                            else if (i>2) i=2;
                            DIFFICULTY = i;
                    }
                    else if (temps.startsWith("darkness")) NODARK = (Integer.parseInt(temps.substring(9))==0);
                    else if (temps.startsWith("transparency")) NOTRANS = (Integer.parseInt(temps.substring(13))==0);
                    else if (temps.startsWith("antialias")) TEXTANTIALIAS = (Integer.parseInt(temps.substring(10))!=0);
                    else if (temps.startsWith("footsteps")) PLAYFOOTSTEPS = (Integer.parseInt(temps.substring(10))!=0);
                    else if (temps.startsWith("showparty")) SHOWPARTYMAP = (Integer.parseInt(temps.substring(10))!=0);
                    else if (temps.startsWith("brightness")) {
                            int i = Integer.parseInt(temps.substring(11));
                            if (i<0) i=0;
                            else if (i>255) i=255;
                            BRIGHTADJUST = i;
                    }
                 else if (temps.startsWith("dungeon scale")) DungeonViewScale = Double.parseDouble(temps.substring(14));
                    temps = r.readLine();
                }
                r.close();
            }
			catch(Exception e)
			{
				e.printStackTrace();
			}
        }

        frame = new dmnew();
	}


        public void start() {
                if (runner == null) {
                        runner=new Thread(this);
                }
                runner.start();
        }

        public void run() {
                counter = 0; endcounter = 0; darkcounter=0;
                while (!didquit) {
                  while (endcounter<20) {
                        if (nomovement || (needdrawdungeon && !sheet && !mappane.isVisible())) {} //do nothing
                        else {
                        if (!clickqueue.isEmpty()) {
                                string where = (string)clickqueue.remove(0);
                                int x = Integer.parseInt(where.substring(0,where.indexOf(",")));
                                int y = Integer.parseInt(where.substring(where.indexOf(",")+1));
                                dclick.processClick(x,y);
                        }
                        else if (!actionqueue.isEmpty()) {
                                string what = (string)actionqueue.remove(0);
                                if (what.equals("s")) {
                                        //save game
                                        if (gamefile==null && !setGameFile(false)) {}
                                        else saveGame();
                                }
                                else if (what.equals("l")) {
                                        //load game
                                        if (gamefile==null && !setGameFile(true)) {}
                                        else {
                                                loadGame(false,(numheroes==0));
                                                hpanel.repaint();
                                                changeCursor();
                                        }
                                }
                                else if (what.equals("o")) {
                                        //show options
                                        optionsdialog.setAndShow(numheroes>0);
                                        int selectedValue = optionsdialog.getValue();
                                        if (selectedValue==OptionsDialog.LOAD) {
                                                if (setGameFile(true)) {
                                                        if (loadGame(false,(numheroes==0))) {
                                                                hpanel.repaint();
                                                        }
                                                        else {
                                                                nomovement = false;
                                                                actionqueue.add("o");
                                                        }
                                                }
                                        }
                                        else if (selectedValue==OptionsDialog.SAVE) { 
                                                if (setGameFile(false)) {
                                                        saveGame();
                                                }
                                        }
                                        else if (selectedValue==OptionsDialog.NEWGAME || selectedValue==OptionsDialog.NEWCUST) { 
                                                File mapfile = null;
                                                if (selectedValue==OptionsDialog.NEWCUST) {
                                                        chooser.setCurrentDirectory(new File(workingdir,"Dungeons"));
                                                        chooser.setDialogTitle("Load a Custom Dungeon Map");
                                                        int returnVal = chooser.showOpenDialog(frame);
                                                        if (returnVal==JFileChooser.APPROVE_OPTION) mapfile = chooser.getSelectedFile();
                                                }
                                                else mapfile = new File("Dungeons"+File.separator+"dungeon.dat"); 
                                                if (mapfile!=null) {
                                                        bool create,nochar;
                                                        int levelpoints=-1,hsmpoints=-1,statpoints=-1,defensepoints=-1,itempoints=0,abilitypoints=0,abilityauto=0;
                                                        Item[] itemchoose = null; SpecialAbility[] abilitychoose = null;
                                                        try {
                                                        FileInputStream in = new FileInputStream(mapfile);
                                                        ObjectInputStream si = new ObjectInputStream(in);
                                                        
                                                     si.readUTF();//version
                                                        create = si.readbool();
                                                        nochar = si.readbool();
                                                        if (create) {
                                                                levelpoints = si.readInt();
                                                                hsmpoints = si.readInt();
                                                                statpoints = si.readInt();
                                                                defensepoints = si.readInt();
                                                                int num=si.readInt();
                                                                if (num>0) {
                                                                        itemchoose = new Item[num];
                                                                        for (int i=0;i<num;i++) {
                                                                                itemchoose[i] = (Item)si.readObject();
                                                                        }
                                                                        itempoints = si.readInt();
                                                                }
                                                                else { itemchoose = null; itempoints = 0; }
                                                                num=si.readInt();
                                                                if (num>0) {
                                                                        abilityauto = si.readInt();
                                                                        abilitychoose = new SpecialAbility[num];
                                                                        for (int i=0;i<num;i++) {
                                                                                abilitychoose[i] = new SpecialAbility(si);
                                                                        }
                                                                        abilitypoints = si.readInt();
                                                                }
                                                                else { abilitychoose = null; abilitypoints = 0; }
                                                        }
                                                        
                                                        in.close();
                                                        }
                                                        catch (Exception ex) {
                                                                System.out.println("Unable to load from map.");
                                                                ex.printStackTrace(System.out);
                                                                //pop up a dialog too
                                                                JOptionPane.showMessageDialog(frame, "Unable to load map!", "Error!", JOptionPane.ERROR_MESSAGE);
                                                                nomovement = false;
                                                                actionqueue.add("o");
                                                                break;
                                                        }
                                                        
                                                        dmmons.clear();
                                                        dmprojs.clear();
                                                        mapchanging = false;
                                                        cloudchanging = false;
                                                        fluxchanging = false;
                                                        mapstochange.clear();
                                                        cloudstochange.clear();
                                                        fluxcages.clear();
                                                        Compass.clearList();
                                                        hpanel.removeAll();
                                                        message.clear();
                                                        if (leveldirloaded!=null) leveldirloaded.clear();
                                                        stopSounds(true);
                                                        //compass.clear();
                                                        if (numheroes>0) {
                                                                spellready = 0;
                                                                hero[0]=null;
                                                                hero[1]=null;
                                                                hero[2]=null;
                                                                hero[3]=null;
                                                                heroatsub[0]=-1;
                                                                heroatsub[1]=-1;
                                                                heroatsub[2]=-1;
                                                                heroatsub[3]=-1;
                                                                numheroes = 0;
                                                        }
                                                        if (create) {
                                                         //System.out.println("show create option");
                                                                removeKeyListener(dmove);
                                                                CreateCharacter createit = new CreateCharacter(frame,mapfile,create,nochar,levelpoints,hsmpoints,statpoints,defensepoints,itemchoose,itempoints,abilitychoose,abilityauto,abilitypoints,true);
                                                                CREATEFLAG = false;
                                                                setContentPane(createit);
                                                                validate();
                                                                repaint();
                                                                synchronized(CREATELOCK) {
                                                                        while (!CREATEFLAG) {
                                                                                try { CREATELOCK.wait(); } catch(InterruptedException ex) {}
                                                                        }
                                                                }
                                                        }
                                                        else {
                                                                JPanel p = new JPanel(new BorderLayout());
                                                                p.setBackground(new Color(0,0,64));

                                                                p.add("Center",Title.loading);
                                                                setContentPane(p);
                                                                validate();
                                                                paint(getGraphics());
                                                                loadMap(mapfile);
                                                                setContentPane(imagePane);
                                                                showCenter(dview);
                                                                sheet=false;
                                                        }
                                                        message.repaint();
                                                        if (sheet) {
                                                                showCenter(dview);
                                                                sheet=false;
                                                        }
                                                        if (hero[0]!=null) {
                                                                if (spellsheet==null) {
                                                                        startNew();
                                                                }
                                                                else {
                                                                        numheroes=1;
                                                                        heroatsub[0]=0;
                                                                        hero[0].isleader=true;
                                                                        hero[0].removeMouseListener(hclick);
                                                                        hero[0].addMouseListener(hclick);
                                                                        hpanel.add(hero[0]);
                                                                        formation.addNewHero();
                                                                        hero[0].repaint();
                                                                        updateDark();
                                                                        spellsheet.repaint();
                                                                        if (weaponsheet.showingspecials) weaponsheet.toggleSpecials(0);
                                                                        weaponsheet.repaint();
                                                                        if (!spellsheet.isShowing()) {
                                                                                eastpanel.removeAll();
                                                                                eastpanel.add(ecpanel);
                                                                                eastpanel.add(Box.createVerticalStrut(10));
                                                                                eastpanel.add(spellsheet);
                                                                                eastpanel.add(Box.createVerticalStrut(20));
                                                                                eastpanel.add(weaponsheet);
                                                                                eastpanel.add(Box.createVerticalStrut(10));
                                                                                eastpanel.add(arrowsheet);
                                                                        }
                                                                }
                                                        }
                                                        else {
                                                                numheroes=0;
                                                                eastpanel.removeAll();
                                                                eastpanel.add(ecpanel);
                                                                eastpanel.add(Box.createVerticalStrut(10));
                                                                eastpanel.add(Box.createVerticalGlue());
                                                                eastpanel.add(arrowsheet);
                                                                eastpanel.add(Box.createVerticalStrut(20));
                                                                formation.addNewHero();
                                                                updateDark();
                                                        }
                                                        hpanel.repaint();
                                                        imagePane.validate();
                                                        frame.show();
                                                }
                                        }
                                        else {
                                                needredraw = true;
                                                if (sheet) herosheet.repaint();
                                        }
                                }
                        }
                        counter++;
                        if (bonesflag>-1) {
                                resurrection(bonesflag);
                                bonesflag=-1;
                                counter--;
                        }
                        else if (counter==1 || counter==5) {//was 7, then was just 1
                                if (mapchanging) mapChange();
                                if (counter==5) {
                                        if (cloudchanging) cloudChange();
                                        if (fluxchanging) fluxChange();
                                        //if (musicloop>1 || !loopsounds.isEmpty()) soundChange();
                                }
                        }
                        else if (counter==7) {//was 1
                                for (int i=0;i<numheroes;i++) hero[i].timePass();
                                int numtimes = 1;
                                if (sleeping) numtimes=2;
                                for (int i=0;i<numtimes;i++) {
                                        if (magicvision>0) { magicvision--; if (magicvision==0) needredraw=true; }
                                        if (dispell>0) { dispell--; if (dispell==0) needredraw=true; }
                                        if (floatcounter>0) { 
                                                floatcounter--;
                                                if (floatcounter==0) {
                                                        climbing=false;
                                                        message.setMessage("Slowfall wears off.",4);
                                                }
                                        }
                                        if (slowcount>0) {
                                                slowcount--;
                                                if (slowcount==0) message.setMessage("Slow wears off.",4);
                                        }
                                        if (freezelife>0) freezelife--;
                                        if (numheroes>0) weaponsheet.updateSpecials();
                                }
                                message.timePass();
                                if (spellsheet!=null) spellsheet.timePass();
                        }
                        else if (counter==3) {
                                monstertime();
                                footstepcount = 0;
                        }
                        else if (counter==2 && darkfactor>leveldark) {
                                darkcounter++;
                                if (sleeping) darkcounter+=4;
                                if (darkcounter>400) {
                                        int newdark = leveldark;
                                        //magic torch wears off a bit
                                        magictorch-=20; if (magictorch<0) magictorch=0;
                                        newdark+=magictorch;
                                        //torches burn down
                                        for (int i=0;i<numheroes;i++) {
                                                if (hero[i].weapon.number==9) {
                                                        ((Torch)hero[i].weapon).burnDown();
                                                        newdark+=((Torch)hero[i].weapon).lightboost;
                                                }
                                                if (hero[i].hand!=null && hero[i].hand.number==9) {
                                                        ((Torch)hero[i].hand).burnDown();
                                                        newdark+=((Torch)hero[i].hand).lightboost;
                                                }
                                        }
                                        if (newdark>255) newdark=255;
                                        darkfactor = newdark;
                                        darkcounter=0;
                                        hupdate();
                                        if (sheet) herosheet.repaint();
                                        needredraw=true;
                                }
                        }
                        while (!weaponqueue.isEmpty()) {
                                string wepstr = (string)weaponqueue.remove(0);
                                int hnum = Integer.parseInt(wepstr.substring(0,1));
                                int fnum = Integer.parseInt(wepstr.substring(1,2));
                                int wepnum = Integer.parseInt(wepstr.substring(2));
                                //System.out.println("hnum = "+hnum+", fnum = "+fnum+", wepnum = "+wepnum);
                                if (!hero[hnum].isdead && hero[hnum].weapon.number==wepnum) hero[hnum].useweapon(fnum);
                                weaponsheet.repaint();
                        }
                        if (counter%2==0) {
                                if (!dmprojs.isEmpty()) projectiles();
                                if (arrowsheet.presscount>0) { 
                                        arrowsheet.presscount--;
                                        if (arrowsheet.presscount==0) {
                                                arrowsheet.pressed = false;
                                                arrowsheet.repaint();
                                        }
                                }
                                if (!walkqueue.isEmpty()) {
                                        if (walkdcount==walkdelay) {
                                          int i=0; walkdelay = 2;//was 3,10,6
                                          if (slowcount>0) {
                                                bool speedboots = true;
                                                while (i<numheroes && speedboots) {
                                                        if (!hero[i].isdead && (hero[i].feet==null || hero[i].feet.number!=187)) speedboots=false;
                                                        else i++;
                                                }
                                                if (!speedboots) walkdelay=8;
                                          }
                                          else while (i<numheroes && walkdelay!=8) {
                                                if (!hero[i].isdead && (hero[i].feet==null || hero[i].feet.number!=187)) {
                                                   if (hero[i].hurtfeet || hero[i].hurtlegs || hero[i].load>hero[i].maxload) walkdelay=8;
                                                   else if (hero[i].hurttorso || hero[i].load>hero[i].maxload*3/4) walkdelay=4;
                                                }
                                                i++;
                                          }
                                          walkdcount=walkdelay;
                                        }
                                        walkdcount--;
                                        if (walkdcount==0) {
                                                int direc = ((Integer)walkqueue.remove(0)).intValue();
                                                
                                                if (direc<4) {
                                                        dmove.partyMove(direc);
                                                }
                                                else if (direc==4) dmove.partyTurn(true); //dmove.turnLeft();
                                                else dmove.partyTurn(false); //dmove.turnRight();
                                                walkdcount = walkdelay;
                                        }
                                }
                                if (needredraw) {
                                        needredraw = false;
                                        needdrawdungeon = true;
                                        dview.repaint();
                                        //dview.paint(dview.getGraphics());
                                        if (facing!=compassface && numheroes>0) {
                                                Compass.updateCompass(facing);
                                                compassface = facing;
                                                if (iteminhand && inhand.number==8) changeCursor();
                                                for (int i=0;i<numheroes;i++) hero[i].repaint();
                                                weaponsheet.repaint();
                                                if (sheet) herosheet.repaint();
                                        }
                                        //if (mappane.isVisible()) needdrawdungeon = false;
                                }
                        }
                        else if (herolook) {
                                herolook = false;
                                nomovement = true;
                                dview.removeMouseListener(dclick);
                                bool foundsub = false;
                                for (int i=0;i<numheroes;i++) {
                                        hero[i].removeMouseListener(hclick);
                                        if (numheroes<4 && !foundsub && heroatsub[i]==-1) {
                                                mirrorhero.subsquare=i;
                                                foundsub = true;
                                        }
                                }
                                if (!foundsub && numheroes<4) {
                                        mirrorhero.subsquare=numheroes;
                                        if (mirrorhero.subsquare==2 && heroatsub[3]==-1) mirrorhero.subsquare=3;
                                        foundsub = true;
                                }
                                //System.out.println("allowswap = "+allowswap+", foundsub = "+foundsub+", sub = "+mirrorhero.subsquare);
                                if (foundsub) {
                                        mirrorhero.heronumber=numheroes;
                                        hpanel.add(mirrorhero);
                                        hpanel.validate();
                                        herosheet.setHero(mirrorhero,true);
                                }
                                else if (allowswap) {
                                        mirrorhero.heronumber=leader;
                                        mirrorhero.subsquare=hero[leader].subsquare;
                                        hpanel.add(mirrorhero);
                                        hpanel.validate();
                                        herosheet.setHero(mirrorhero,false);
                                }
                                else herosheet.setHero(mirrorhero,false);
                                if (numheroes>0) { weaponsheet.setVisible(false); spellsheet.setVisible(false); }
                                arrowsheet.setVisible(false);
                                sheet=true;
                                showCenter(herosheet);
                        }
                        }
                        try { runner.sleep(SLEEPTIME); } catch (InterruptedException e) {}
                        if (counter>7) counter=0;
                        else if (gameover) { 
                                counter=-2; endcounter++;
                                if (endcounter==1) {
                                        removeKeyListener(dmove);
                                        dview.removeMouseListener(dclick);
                                        if (!alldead) {
                                                for (int i=0;i<numheroes;i++) hero[i].removeMouseListener(hclick);
                                                endcounter+=18;
                                        }
                                }
                        }
                  }
                  if (gameover) {
                     if (alldead) gameOver();
                     else gameWin();
                     gameover = false;
                     alldead = false;
                  }
                  try { runner.sleep(SLEEPTIME); } catch (InterruptedException e) {}
                }
                shutDown();
                //if (music!=null) { musicloop = 0; music.stop(); music.deallocate(); music.close(); }
                //System.exit(0);
        }

        public void stop() {
                if (runner !=null && runner.isAlive()) {
                        runner.interrupt();
                        runner = null;
                }
        }
        
        public static void shutDown() {
             ((dmnew)frame).stop();
             try {
                //write out config file
                PrintWriter w = new PrintWriter(new FileWriter("dmnew.cfg"));
//                w.println("; Display Settings. Default windowed, 800x600, 0(depth), 0(refresh)");
//               w.println("; 0 for depth and refresh means use best, but this doesn't work for some.");
//               w.println("; Other possibles are 8,16,24,or 32 and 60,72,85,etc.");
//                if (currentDevice.getFullScreenWindow()!=null) {
//                        w.println("fullscreen 1");
//                        w.println("width "+currentMode.getWidth());
//                        w.println("height "+currentMode.getHeight());
//                        w.println("depth "+trybitdepth);
//                        w.println("refresh "+tryrefresh);
//                }
//                else {
//                        w.println("fullscreen 0");
//                        w.println("width "+frame.getSize().width);
//                        w.println("height "+frame.getSize().height);
//                        w.println("depth "+trybitdepth);
//                        w.println("refresh "+tryrefresh);
//                }
             w.println("; Movement keys.  Default forward 'w', back 's', left 'a', right 'd', turnleft 'q', turnright 'e'");
             w.println("; (Number pad is always default as well.)");
             w.println("forward "+forwardkey);
             w.println("back "+backkey);
             w.println("left "+leftkey);
             w.println("right "+rightkey);
             w.println("turnleft "+turnleftkey);
             w.println("turnright "+turnrightkey);
             w.println("; Window size.  Default 700x600");
             w.println("width "+frame.getSize().width);
             w.println("height "+frame.getSize().height);
                w.println("; Game speed.  Values range from 10 (fastest) to 100 (slowest).  Default 45.");
                w.println("speed "+SLEEPTIME);
                w.println("; Game difficulty.  values range from -2 (easiest) to 2 (hardest).  Default 0.");
                w.println("difficulty "+DIFFICULTY);
                w.println("; Darkness, 0 to turn off.");
                w.println("darkness "+(NODARK?0:1));
                w.println("; Transparency (for ghosts).  0 to turn off.");
                w.println("transparency "+(NOTRANS?0:1));
                w.println("; Text antialiasing.  0 to turn off");
                w.println("antialias "+(TEXTANTIALIAS?1:0));
                w.println("; Monster footstep sounds.  0 to turn off.");
                w.println("footsteps "+(PLAYFOOTSTEPS?1:0));
                w.println("; Show the party on the automap.  0 to turn off.");
                w.println("showparty "+(SHOWPARTYMAP?1:0));
                w.println("; Boost the dungeon display brightness.  Values range from 0 (none) to 32 (brightest).  Default 8.");
                w.println("brightness "+BRIGHTADJUST);
                w.println("; Dungeon view scale multiplier.  Default 1.0 (no scaling).");
             w.println("dungeon scale "+DungeonViewScale);
                w.flush();
                w.close();
             } catch(Exception ex) { ex.printStackTrace(System.out); }
             frame.dispose();
             //if (music!=null) { musicloop = 0; music.stop(); music.deallocate(); music.close(); }
//             if (displayModeChanged && !originalMode.equals(currentDevice.getDisplayMode())) {
//                currentDevice.setDisplayMode(originalMode);
//             }
             System.exit(0);
        }

        public void changeCursor() {
                if (iteminhand) dmcursor = tk.createCustomCursor(inhand.pic,new Point(10,10),"dmc");
                else dmcursor = handcursor;//new Cursor(Cursor.HAND_CURSOR);
                this.setCursor(dmcursor);
                holding.repaint();
        }
        
        public void showCenter(JPanel c) {//JComponent c) {
                dview.setVisible(false);
                herosheet.setVisible(false);
                freezelabelpan.setVisible(false);
                loadinglabelpan.setVisible(false);
                endiconlabelpan.setVisible(false);
                eventpanel.setVisible(false);
                animlabelpan.setVisible(false);
                c.setVisible(true);
                validate();
                requestFocusInWindow();
        }
     
     public static void setDungeonViewScale(double scale) {
             DungeonViewScale = scale;

                Dimension maincenterdim = new Dimension((int)(452 * DungeonViewScale), (int)(330 * DungeonViewScale));
                maincenterpan.setPreferredSize(maincenterdim);
                maincenterpan.setMinimumSize(maincenterdim);
                maincenterpan.setMaximumSize(maincenterdim);

                Dimension centerdim = new Dimension((int)(448 * DungeonViewScale), (int)(326 * DungeonViewScale));
                dview.setSize(centerdim);
                dview.setPreferredSize(centerdim);
                dview.setMinimumSize(centerdim);
                dview.setMaximumSize(centerdim);
                herosheet.setSize(centerdim);
                herosheet.setPreferredSize(centerdim);
                herosheet.setMinimumSize(centerdim);
                freezelabelpan.setSize(centerdim);
                freezelabelpan.setPreferredSize(centerdim);
                freezelabelpan.setMinimumSize(centerdim);
                loadinglabelpan.setSize(centerdim);
                loadinglabelpan.setPreferredSize(centerdim);
                loadinglabelpan.setMinimumSize(centerdim);
                endiconlabel.setSize(centerdim);
                endiconlabel.setPreferredSize(centerdim);
                endiconlabel.setMinimumSize(centerdim);
                endiconlabel.setMaximumSize(centerdim);
                endiconlabelpan.setSize(centerdim);
                endiconlabelpan.setPreferredSize(centerdim);
                endiconlabelpan.setMinimumSize(centerdim);
                endiconlabelpan.setMaximumSize(centerdim);
                eventpanel.setSize(centerdim);
                eventpanel.setPreferredSize(centerdim);
                eventpanel.setMinimumSize(centerdim);
                eventpanel.setMaximumSize(centerdim);
                animlabel.setSize(centerdim);
                animlabel.setPreferredSize(centerdim);
                animlabel.setMinimumSize(centerdim);
                animlabel.setMaximumSize(centerdim);
                animlabelpan.setSize(centerdim);
                animlabelpan.setPreferredSize(centerdim);
                animlabelpan.setMinimumSize(centerdim);
                animlabelpan.setMaximumSize(centerdim);

             maincenterpan.validate();
             realcenterpanel.validate();
             centerpanel.validate();
             imagePane.validate();
     }
        
        public static void updateDark() {
                leveldark = leveldarkfactor[level]+numillumlets*70; if (leveldark>255) leveldark=255;
                int newdark = leveldark+magictorch;
                //torches
                for (int i=0;i<numheroes;i++) {
                        if (hero[i].weapon.number==9) {
                                newdark+=((Torch)hero[i].weapon).lightboost;
                        }
                        if (hero[i].hand!=null && hero[i].hand.number==9) {
                                newdark+=((Torch)hero[i].hand).lightboost;
                        }
                }
                if (newdark>255) newdark=255;
                darkfactor = newdark;
                if ((leveldir[level]!=null && !leveldir[level].equals(Wall.currentdir)) || (leveldir[level]==null && !Wall.currentdir.equals("Maps")) ) {
                        if (leveldir[level]==null) Wall.currentdir = "Maps";
                        else {
                                Wall.currentdir = leveldir[level];
                                if (dview.isShowing() && (leveldirloaded==null || !leveldirloaded.contains(Wall.currentdir))) {
                                        //since may take some time to load, clear dview and draw "Loading..."
                                        Graphics tempg = dview.getGraphics();
                                        tempg.setColor(Color.black);
                                        tempg.fillRect(0,0,448,326);
                                        tempg.setFont(dungfont14);
                                        tempg.setColor(new Color(70,70,70));
                                        tempg.drawstring("Loading Level...",183,153);
                                        tempg.setColor(Color.white);
                                        tempg.drawstring("Loading Level...",180,150);
                                        tempg.dispose();
                                        if (leveldirloaded==null) leveldirloaded = new ArrayList();
                                        leveldirloaded.add(Wall.currentdir);
                                }
                        }
                        Wall.redoPics();
                        Alcove.redoPics();
                        FloorSwitch.redoPics();
                        Door.redoPics();
                        Teleport.redoPics();
                        Pit.redoPics();//for ceiling view only
                        bool backexists = false, trackexists = false;
                        File testfile = new File(Wall.currentdir+File.separator+"back.gif");
                        if (testfile.exists()) {
                                back = tk.getImage(Wall.currentdir+File.separator+"back.gif");
                                ImageTracker.addImage(back,5);
                                backexists = true;
                        }
                        testfile = new File(Wall.currentdir+File.separator+"doortrack.gif");
                        if (testfile.exists()) {
                                doortrack = tk.getImage(Wall.currentdir+File.separator+"doortrack.gif");
                                ImageTracker.addImage(doortrack,5);
                                trackexists = true;
                        }
                        if (backexists || trackexists) {
                                try { ImageTracker.waitForID(5,2000); } catch(InterruptedException ex) {}
                                if (backexists) ImageTracker.removeImage(back,5);
                                if (trackexists) ImageTracker.removeImage(doortrack,5);
                        }
                        Writing2.ADDEDPICS = false;
                        //update stairs, alcoves, fountains, pits,
                        for (int x=0;x<mapwidth;x++) {
                                for (int y=0;y<mapheight;y++) {
                                        if (DungeonMap[level][x][y].mapchar=='>') ((Stairs)DungeonMap[level][x][y]).redoStairsPics();
                                        else if (DungeonMap[level][x][y].mapchar==']') ((OneAlcove)DungeonMap[level][x][y]).redoAlcovePics();
                                        else if (DungeonMap[level][x][y].mapchar=='w') ((Writing2)DungeonMap[level][x][y]).redoWritPics();
                                        else if (DungeonMap[level][x][y].mapchar=='f') ((Fountain)DungeonMap[level][x][y]).redoFountainPics();
                                        else if (DungeonMap[level][x][y].mapchar=='p') ((Pit)DungeonMap[level][x][y]).redoPitPics();
                                }
                        }
                }
                needredraw = true;
        }

        public void hupdate() {
                for (int i=0;i<numheroes;i++) hero[i].repaint();
        }
        
        public void projectiles() {
                Projectile tempp;
                int index = 0;
                while (index<dmprojs.size()) {
                        tempp = (Projectile)dmprojs.get(index);
                        if (tempp.isending) {
                                dmprojs.remove(index);
                                DungeonMap[tempp.level][tempp.x][tempp.y].numProjs--;
                                if (tempp.level == level) {
                                  int xdist = tempp.x-partyx; if (xdist<0) xdist*=-1;
                                  int ydist = tempp.y-partyy; if (ydist<0) ydist*=-1;
                                  if (xdist<5 && ydist<5) needredraw = true;
                                }
                        }
                        else if (!tempp.needsfirstdraw) {
                                tempp.update();
                                if (tempp.isending && tempp.it!=null) dmprojs.remove(index);
                                else index++;
                        }
                        else {
                                tempp.needsfirstdraw=false;
                                if (tempp.level == level) {
                                  int xdist = tempp.x-partyx; if (xdist<0) xdist*=-1;
                                  int ydist = tempp.y-partyy; if (ydist<0) ydist*=-1;
                                  if (xdist<5 && ydist<5) needredraw = true;
                                }
                                index++;
                        }
                }
        }

        public void monstertime() {
              moncycle = moncycle%2+1;
              for (Enumeration e=dmmons.elements();e.hasMoreElements();) {
                  ((Monster)e.nextElement()).timePass();
              }
        }

        public void mapChange() {
              Teleport.currentcycle = (Teleport.lastcycle+1)%2;
              MapPoint mapxy;
              int index=0;
              while (index<mapstochange.size()) {
                    mapxy = (MapPoint)((ArrayList)mapstochange).get(index);
                    if (!DungeonMap[mapxy.level][mapxy.x][mapxy.y].changeState()) ((ArrayList)mapstochange).remove(index);
                    else index++;
              }
              if (mapstochange.isEmpty()) mapchanging=false;
              Teleport.lastcycle = Teleport.currentcycle;
              Teleport.currentcycle = 2;
        }
        public void cloudChange() {
              PoisonCloud cloud;
              for (Iterator i=cloudstochange.iterator();i.hasNext();) {
                  cloud = (PoisonCloud)i.next();
                  if (!cloud.update()) i.remove();
                  else if (sleeping && !cloud.update()) i.remove(); //update twice if sleeping
              }
              if (cloudstochange.isEmpty()) cloudchanging=false;
        }
        public void fluxChange() {
              FluxCage flux;
              for (Iterator i=fluxcages.values().iterator();i.hasNext();) {
                  flux = (FluxCage)i.next();
                  if (!flux.update()) i.remove();
                  else if (sleeping && !flux.update()) i.remove(); //update twice if sleeping
              }
              if (fluxcages.isEmpty()) { fluxchanging=false; needredraw=true; }
        }
        public void soundChange() {
              LoopSound sound;
              for (int i=0;i<loopsounds.size();i++) {
                  sound = (LoopSound)loopsounds.get(i);
                  //System.out.println(sound.clipfile+", loop = "+sound.loop+", isRunning = "+sound.clip.isRunning()+", count = "+sound.count);
                  if (sound.loop>0 && !sound.clip.isRunning()) {
                          sound.count--;
                          if (sound.count<=0) {
                                sound.clip.setFramePosition(0);
                                playSound(sound.clip,sound.clipfile,sound.x,sound.y,0);
                                //sound.count = randGen.nextInt(200)+300;
                                sound.count = randGen.nextInt(100)+50;
                          }
                  }
              }
              /*
              if (musicloop>1) {
                musicloop--;
                if (musicloop==1) music.start();
              }
              */
        }
        public static void stopSounds(bool abrupt) {
              LoopSound sound;
              while (!loopsounds.isEmpty()) {
                  sound = (LoopSound)loopsounds.remove(0);
                  if (!abrupt && sound.loop<0) sound.clip.loop(0);
                  else {
                        sound.clip.stop();
                        sound.clip.close();
                  }
              }
              //musicloop = 0;
              //if (abrupt && music!=null) music.stop();
        }

        public void resurrection(int i) {
                nomovement = true;
                removeKeyListener(dmove);
                dview.removeMouseListener(dclick);
                playSound("altar.wav",-1,-1);
                dview.repaint();
                try { runner.sleep(2800); } catch (InterruptedException e) {}
                altaranim.flush();
                
                bool foundsub = false;
                for (int j=0;j<numheroes;j++) {
                        if (!foundsub && heroatsub[j]==-1) {
                                hero[i].subsquare=j;
                                heroatsub[j]=i;
                                foundsub = true;
                        }
                }
                if (!foundsub) { hero[i].subsquare=numheroes; heroatsub[numheroes]=i; }
                formation.addNewHero();
                hero[i].defense-=hero[i].defenseboost; hero[i].defenseboost=0;
                hero[i].magicresist-=hero[i].magicresistboost; hero[i].magicresistboost=0;
                hero[i].hurtcounter = 0;
                hero[i].removeMouseListener(hclick);
                hero[i].addMouseListener(hclick);
                hero[i].isdead=false;
                spellsheet.repaint();
                weaponsheet.repaint();
                hero[i].health=1; //end up with 1 health
                int drain = randGen.nextInt(hero[i].maxstamina/50+1); if (drain>10) drain=10;
                hero[i].maxstamina-=drain; if (hero[i].maxstamina<10) hero[i].maxstamina=10;
                hero[i].stamina=hero[i].maxstamina/2+1;
                drain = randGen.nextInt(3);
                hero[i].vitality-=drain; if (hero[i].vitality<1) hero[i].vitality=1;
                hero[i].setMaxLoad();
                hupdate();
                needredraw = true;
                
                setFocusTraversalKeys(KeyboardFocusManager.FORWARD_TRAVERSAL_KEYS,java.util.Collections.EMPTY_SET);
                setFocusTraversalKeys(KeyboardFocusManager.BACKWARD_TRAVERSAL_KEYS,java.util.Collections.EMPTY_SET);
                addKeyListener(dmove);
                dview.addMouseListener(dclick);
                nomovement = false;
        }
        
        /*
        public void controllerUpdate(ControllerEvent e) {
                //System.out.println(e);
                if (musicloop!=0 && e instanceof EndOfMediaEvent) {
                        if (musicloop<0) musicloop = 2;
                        else musicloop = randGen.nextInt(100)+50;
                }
        }
        */
        
        public static void playFootStep(string sound, int x, int y) {
                if (!PLAYFOOTSTEPS || footstepcount>2) return;
                playSound(sound,x,y,0);
                footstepcount++;
        }
        
        public static void playSound(string sound, int x, int y) {
                if (sound.toLowerCase().endsWith(".wav")) playSound(getClip(sound),sound,x,y,0);
                else playSound(sound,x,y,0);
        }
        public static void playSound(string sound, int x, int y, int looping) {
                if (sound.toLowerCase().endsWith(".wav")) playSound(getClip(sound),sound,x,y,looping);
                /*
                else {
                        URL u;
                        try { u = (new File("Music"+File.separator+sound)).toURL(); }
                        catch (MalformedURLException ex1) { return; }

                        //the null test can go away if i have title music...
                        if (music == null) {
                                try { music = Manager.createPlayer(u); }
                                catch (Exception ex2) { return; }
                                music.addControllerListener((dmnew)frame);
                        }
                        else {
                                music.removeControllerListener((dmnew)frame);
                                music.close();
                                try {
                                        music = Manager.createPlayer(u);
                                }
                                catch (Exception ex4) { return; };
                                music.addControllerListener((dmnew)frame);
                        }
                        musicloop = looping;
                        music.start();
                }
                */
        }
        //looping -> -1 is infinite loop w/o delays, 0 is no loop, 1 is infinite loop w/ delays between plays
        public static void playSound(Clip clip, string sound, int x, int y, int looping) {
                if (looping!=0) {
                        bool found = false;
                        int i = 0;
                        LoopSound ls;
                        while (!found && i<loopsounds.size()) {
                                ls = (LoopSound)loopsounds.get(i);
                                if (ls.clipfile.equals(sound)) {
                                        if (ls.loop>looping) ls.loop=looping;
                                        //ls.count = 0;
                                        found = true;
                                }
                                else i++;
                        }
                        if (found) return;
                }
                try {
                        //clip.open(stream);
                        double gain = 1.5;
                        if (x!=-1 || y!=-1) {
                            //get xy distance from party
                            int xdist = x-partyx; //if (xdist<0) xdist*=-1;
                            int ydist = y-partyy; //if (ydist<0) ydist*=-1;
                            //determine gain/pan values
                            if (xdist!=0 || ydist!=0) {
                              //double gain = 0.9;//was 0.8, this should be set by a volume slider in option dialog...
                              double pan = 0.0;
                              gain-=((int)(Math.sqrt(ydist*ydist+xdist*xdist)+.5))*.2;
                              //double tester = (gain<=0.0?0.0001:gain);
                              //System.out.println("gain = "+gain+", tester = "+tester+", log() = "+(Math.log(tester)/Math.log(10.0)*20.0));
                              if (gain<0.0) { clip.close(); return; }
                              //else if (gain<0.1) gain=0.1;
                              switch (facing) {
                                case NORTH:
                                        if (xdist>0) {
                                                pan+=.75;
                                                if (ydist>2 || ydist<-2) pan-=.25;
                                        }
                                        else if (xdist<0) {
                                                pan-=.75;
                                                if (ydist>2 || ydist<-2) pan+=.25;
                                        }
                                        break;
                                case SOUTH:
                                        if (xdist>0) {
                                                pan-=.75;
                                                if (ydist>2 || ydist<-2) pan+=.25;
                                        }
                                        else if (xdist<0) {
                                                pan+=.75;
                                                if (ydist>2 || ydist<-2) pan-=.25;
                                        }
                                        break;
                                case EAST:
                                        if (ydist>0) {
                                                pan+=.75;
                                                if (xdist>2 || xdist<-2) pan-=.25;
                                        }
                                        else if (ydist<0) {
                                                pan-=.75;
                                                if (xdist>2 || xdist<-2) pan+=.25;
                                        }
                                        break;
                                case WEST:
                                        if (ydist>0) {
                                                pan-=.75;
                                                if (xdist>2 || xdist<-2) pan+=.25;
                                        }
                                        else if (ydist<0) {
                                                pan+=.75;
                                                if (xdist>2 || xdist<-2) pan-=.25;
                                        }
                                        break;
                              }
                              //set pan
                              FloatControl panControl = (FloatControl) clip.getControl(FloatControl.Type.PAN);
                              panControl.setValue((float)pan);
                            }
                        }
                        //set gain
                        FloatControl gainControl = (FloatControl) clip.getControl(FloatControl.Type.MASTER_GAIN);
                        float dB = (float) (Math.log(gain)/Math.log(10.0)*20.0);
                        gainControl.setValue(dB);
                        if (looping>=0) clip.start();
                        else clip.loop(Clip.LOOP_CONTINUOUSLY);
                        //if (looping!=0) loopsounds.add(new LoopSound(clip,sound,x,y,looping,randGen.nextInt(200)+300));
                        if (looping!=0) loopsounds.add(new LoopSound(clip,sound,x,y,looping,randGen.nextInt(100)+50));
                }
                //catch (Exception ex) { ex.printStackTrace(); }
                catch (Exception ex) {}
        }
        private static Clip getClip(string sound) {
                try {
                        AudioInputStream stream = AudioSystem.getAudioInputStream(new File("Sounds"+File.separator+sound));
                        AudioFormat format = stream.getFormat();
                        DataLine.Info info = new DataLine.Info(
                                                  Clip.class, 
                                                  stream.getFormat(), 
                                                  ((int) stream.getFrameLength() *
                                                      format.getFrameSize()));
                
                        Clip clip = (Clip) AudioSystem.getLine(info);
                        clip.open(stream);
                        return clip;
                }
                catch (Exception ex) { return null; }
        }
        
        public void saveGame() {
              //nomovement = true;
              try {
                nomovement = true;
                if (gamefile.exists()) {
                        string gamename = gamefile.getPath();
                        File oldbak = new File(gamename+".bak");
                        if (oldbak.exists()) oldbak.delete();
                        //gamefile.renameTo(new File(gamename+".bak"));
                        gamefile.renameTo(oldbak);
                        gamefile = new File(gamename);
                }
                FileOutputStream out = new FileOutputStream(gamefile);
                ObjectOutputStream so = new ObjectOutputStream(out);
                
                //version string
                so.writeUTF(version);
                //write junk bools for map start info
                so.writebool(false);
                so.writebool(false);
                
                //global stuff
                so.writeInt(counter);
                so.writeObject(leveldarkfactor);
                so.writeInt(darkcounter);
                so.writeInt(darkfactor);
                so.writeInt(magictorch);
                so.writeInt(magicvision);

                so.writeInt(floatcounter);
                so.writeInt(dispell);
                so.writeInt(slowcount);
                so.writeInt(freezelife);
                so.writebool(mapchanging);
                so.writebool(cloudchanging);
                so.writebool(fluxchanging);
                so.writeInt(level);
                so.writeInt(partyx);
                so.writeInt(partyy);
                so.writeInt(facing);
                so.writeInt(leader);
                so.writeObject(heroatsub);
                so.writebool(iteminhand);
                if (iteminhand) so.writeObject(inhand);
                so.writeInt(spellready);
                so.writeInt(weaponready);
                so.writebool(mirrorback);
                //so.writebool(compass.isVisible());

                //monsters
                so.writeInt(dmmons.size());
                Monster tempmon;
                for (Enumeration e=dmmons.elements();e.hasMoreElements();) {
                //for (Iterator i=dmmons.iterator();i.hasNext();) {
                        //tempmon = (Monster)i.next();
                        tempmon = (Monster)e.nextElement();
                        so.writebool(tempmon.isdying);
                        so.writeInt(tempmon.number);
                        so.writeInt(tempmon.x);
                        so.writeInt(tempmon.y);
                        so.writeInt(tempmon.level);
                        if (tempmon.number>27) {
                                so.writeUTF(tempmon.name);
                                so.writeUTF(tempmon.picstring);
                                so.writeUTF(tempmon.soundstring);
                                so.writeUTF(tempmon.footstep);
                                so.writebool(tempmon.canusestairs);
                                so.writebool(tempmon.isflying);
                                so.writebool(tempmon.ignoremons);
                                so.writebool(tempmon.canteleport);
                        }
                        so.writeInt(tempmon.subsquare);
                        so.writeInt(tempmon.health);
                        so.writeInt(tempmon.maxhealth);
                        so.writeInt(tempmon.mana);
                        so.writeInt(tempmon.maxmana);
                        so.writeInt(tempmon.facing);
                        so.writeInt(tempmon.currentai);
                        so.writeInt(tempmon.defaultai);
                        so.writebool(tempmon.HITANDRUN);
                        so.writebool(tempmon.isImmaterial);
                        so.writebool(tempmon.wasfrightened);
                        so.writebool(tempmon.hurt);
                        so.writebool(tempmon.wasstuck);
                        so.writebool(tempmon.ispoisoned);
                        if (tempmon.ispoisoned) {
                                so.writeInt(tempmon.poisonpow);
                                so.writeInt(tempmon.poisoncounter);
                        }
                        so.writeInt(tempmon.timecounter);
                        so.writeInt(tempmon.movecounter);
                        so.writeInt(tempmon.randomcounter);
                        so.writeInt(tempmon.runcounter);
                        so.writeObject(tempmon.carrying);
                        if (tempmon.equipped!=null) {
                                so.writebool(true);
                                so.writeObject(tempmon.equipped);
                        }
                        else so.writebool(false);
                        so.writebool(tempmon.gamewin);
                        //if (tempmon.gamewin) { so.writeUTF(tempmon.endanim); so.writeUTF(tempmon.endmusic); so.writeUTF(tempmon.endsound); }
                        if (tempmon.gamewin) { so.writeUTF(tempmon.endanim); so.writeUTF(tempmon.endsound); }
                        so.writeInt(tempmon.hurtitem);
                        so.writeInt(tempmon.needitem);
                        so.writeInt(tempmon.needhandneck);
                        
                        so.writeInt(tempmon.power);
                        so.writeInt(tempmon.defense);
                        so.writeInt(tempmon.magicresist);
                        so.writeInt(tempmon.speed);
                        so.writeInt(tempmon.movespeed);
                        so.writeInt(tempmon.attackspeed);
                        so.writeInt(tempmon.poison);
                        so.writeInt(tempmon.fearresist);
                        so.writebool(tempmon.hasmagic);
                        if (tempmon.hasmagic) {
                                so.writeInt(tempmon.castpower);
                                so.writeInt(tempmon.manapower);
                                so.writeInt(tempmon.numspells);
                                if (tempmon.numspells>0) so.writeObject(tempmon.knownspells);
                                so.writeInt(tempmon.minproj);
                                so.writebool(tempmon.hasheal);
                                so.writebool(tempmon.hasdrain);
                        }
                        //so.writeInt(tempmon.ammonumber);
                        so.writebool(tempmon.useammo);
                        if (tempmon.useammo) {
                                so.writeInt(tempmon.ammo);
                        }
                        so.writeInt(tempmon.pickup);
                        so.writeInt(tempmon.steal);
                        so.writebool(tempmon.poisonimmune);
                        so.writeInt(tempmon.powerboost);
                        so.writeInt(tempmon.defenseboost);
                        so.writeInt(tempmon.magicresistboost);
                        so.writeInt(tempmon.speedboost);
                        so.writeInt(tempmon.manapowerboost);
                        so.writeInt(tempmon.movespeedboost);
                        so.writeInt(tempmon.attackspeedboost);
                        so.writebool(tempmon.silenced);
                        if (tempmon.silenced) so.writeInt(tempmon.silencecount);
                }
                tempmon = null;
                //System.out.println("mons saved");

                //projectiles
                so.writeInt(dmprojs.size());
                Projectile tempproj;
                for (Iterator i=dmprojs.iterator();i.hasNext();) {
                        tempproj = (Projectile)i.next();
                        so.writebool(tempproj.isending);
                        //write true if proj is made of an item, else false
                        if (tempproj.it!=null) {
                                so.writebool(true);
                                so.writeObject(tempproj.it);
                        }
                        else {
                                so.writebool(false);
                                so.writeObject(tempproj.sp);
                        }
                        so.writeInt(tempproj.level);
                        so.writeInt(tempproj.x);
                        so.writeInt(tempproj.y);
                        so.writeInt(tempproj.dist);
                        so.writeInt(tempproj.direction);
                        so.writeInt(tempproj.subsquare);
                        if (tempproj.sp!=null) {
                                so.writeInt(tempproj.powdrain);
                                so.writeInt(tempproj.powcount);
                        }
                        so.writebool(tempproj.justthrown);
                        so.writebool(tempproj.notelnext);
                }
                tempproj = null;
                //System.out.println("projs saved");

                //heroes
                so.writeInt(numheroes);
                for (int i=0;i<numheroes;i++) {
                        /*
                        so.writeUTF(hero[i].picname);
                        so.writeInt(hero[i].subsquare);
                        so.writeInt(hero[i].number);
                        so.writeUTF(hero[i].name);
                        so.writeUTF(hero[i].lastname);
                        so.writeInt(hero[i].maxhealth);
                        so.writeInt(hero[i].health);
                        so.writeInt(hero[i].maxstamina);
                        so.writeInt(hero[i].stamina);
                        so.writeInt(hero[i].maxmana);
                        so.writeInt(hero[i].mana);
                        //so.writeFloat(hero[i].maxload);
                        so.writeFloat(hero[i].load);
                        so.writeInt(hero[i].food);
                        so.writeInt(hero[i].water);
                        so.writeInt(hero[i].strength);
                        so.writeInt(hero[i].vitality);
                        so.writeInt(hero[i].dexterity);
                        so.writeInt(hero[i].intelligence);
                        so.writeInt(hero[i].wisdom);
                        so.writeInt(hero[i].defense);
                        so.writeInt(hero[i].magicresist);
                        so.writeInt(hero[i].strengthboost);
                        so.writeInt(hero[i].vitalityboost);
                        so.writeInt(hero[i].dexterityboost);
                        so.writeInt(hero[i].intelligenceboost);
                        so.writeInt(hero[i].wisdomboost);
                        so.writeInt(hero[i].defenseboost);
                        so.writeInt(hero[i].magicresistboost);
                        so.writeInt(hero[i].flevel);
                        so.writeInt(hero[i].nlevel);
                        so.writeInt(hero[i].plevel);
                        so.writeInt(hero[i].wlevel);
                        so.writeInt(hero[i].flevelboost);
                        so.writeInt(hero[i].nlevelboost);
                        so.writeInt(hero[i].plevelboost);
                        so.writeInt(hero[i].wlevelboost);
                        so.writeInt(hero[i].fxp);
                        so.writeInt(hero[i].nxp);
                        so.writeInt(hero[i].pxp);
                        so.writeInt(hero[i].wxp);
                        so.writebool(hero[i].isdead);
                        so.writebool(hero[i].wepready);
                        so.writebool(hero[i].ispoisoned);
                        if (hero[i].ispoisoned) {
                                so.writeInt(hero[i].poison);
                                so.writeInt(hero[i].poisoncounter);
                        }
                        so.writebool(hero[i].silenced);
                        if (hero[i].silenced) so.writeInt(hero[i].silencecount);
                        so.writebool(hero[i].hurtweapon);
                        so.writebool(hero[i].hurthand);
                        so.writebool(hero[i].hurthead);
                        so.writebool(hero[i].hurttorso);
                        so.writebool(hero[i].hurtlegs);
                        so.writebool(hero[i].hurtfeet);
                        so.writeInt(hero[i].timecounter);
                        so.writeInt(hero[i].walkcounter);
                        so.writeInt(hero[i].spellcount);
                        so.writeInt(hero[i].weaponcount);
                        so.writeInt(hero[i].kuswordcount);
                        so.writeInt(hero[i].rosbowcount);
                        so.writeUTF(hero[i].currentspell);
                        //write abilities here
                        if (hero[i].abilities!=null) {
                                so.writeInt(hero[i].abilities.length);
                                for (int j=0;j<hero[i].abilities.length;j++) {
                                        hero[i].abilities[j].save(so);
                                }
                        }
                        else so.writeInt(0);
                        if (hero[i].weapon==fistfoot) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].weapon);
                        }
                        if (hero[i].hand==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].hand);
                        }
                        if (hero[i].head==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].head);
                        }
                        if (hero[i].torso==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].torso);
                        }
                        if (hero[i].legs==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].legs);
                        }
                        if (hero[i].feet==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].feet);
                        }
                        if (hero[i].neck==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].neck);
                        }
                        if (hero[i].pouch1==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].pouch1);
                        }
                        if (hero[i].pouch2==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hero[i].pouch2);
                        }
                        so.writeObject(hero[i].quiver);
                        so.writeObject(hero[i].pack);
                        */
                        hero[i].save(so);
                }
                //System.out.println("heroes saved\n");
                
                //mapObjects
                //System.out.print("saving map");
                so.writeInt(numlevels);
                so.writeInt(mapwidth);
                so.writeInt(mapheight);
                for (int l=0;l<numlevels;l++) {
                    for (int x=0;x<mapwidth;x++) {
                        for (int y=0;y<mapheight;y++) {
                            DungeonMap[l][x][y].save(so);
                        }
                    }
                }
                if (mapchanging) {
                   so.writeInt(mapstochange.size());
                   for (Iterator i=mapstochange.iterator();i.hasNext();) {
                        so.writeObject(i.next());
                   }
                }
                if (cloudchanging) {
                   PoisonCloud tempcloud;
                   so.writeInt(cloudstochange.size());
                   for (Iterator i=cloudstochange.iterator();i.hasNext();) {
                        tempcloud = (PoisonCloud)i.next();
                        so.writeInt(tempcloud.level);
                        so.writeInt(tempcloud.x);
                        so.writeInt(tempcloud.y);
                        so.writeInt(tempcloud.stage);
                        so.writeInt(tempcloud.stagecounter);
                   }
                }
                if (fluxchanging) {
                   FluxCage tempcage;
                   so.writeInt(fluxcages.size());
                   for (Iterator i=fluxcages.values().iterator();i.hasNext();) {
                        tempcage = (FluxCage)i.next();
                        so.writeInt(tempcage.level);
                        so.writeInt(tempcage.x);
                        so.writeInt(tempcage.y);
                        so.writeInt(tempcage.counter);
                   }
                }
                //System.out.print("map saved\n");

                //save ambient sound data
                so.writeInt(loopsounds.size());
                for (int i=0;i<loopsounds.size();i++) {
                        LoopSound sound = (LoopSound)loopsounds.get(i);
                        so.writeUTF(sound.clipfile);
                        so.writeInt(sound.x);
                        so.writeInt(sound.y);
                        so.writeInt(sound.loop);
                        so.writeInt(sound.count);
                }

                //automap
                so.writebool(AUTOMAP);
                if (AUTOMAP) so.writeObject(dmmap.map);
                
                //save map picture directory modifier
                for (int l=0;l<numlevels;l++) {
                        if (leveldir[l]!=null) so.writeUTF(leveldir[l]);
                        else so.writeUTF("");
                }
                
                so.flush();
                out.close();
                //System.out.println("\ngame saved");
                message.setMessage("Game Saved.",4);
                //System.gc();
              }
              catch (Exception e) {
                      message.setMessage("Unable to save game!",4);
                      System.out.println("Unable to save game!");
                      //pop up a dialog too
                      JOptionPane.showMessageDialog(this, "Unable to save game!", "Error!", JOptionPane.ERROR_MESSAGE);
                      e.printStackTrace();
                      nomovement = false;
              }
              nomovement = false;
        }
        

        public bool setGameFile(bool loading) {
                /*
                string returnVal;
                chooser.setDirectory("Saves");
                if (loading) {
                        chooser.setMode(FileDialog.LOAD);
                        chooser.setTitle("Load a Saved Game");
                        chooser.show();
                        returnVal = chooser.getFile();
                }
                else {
                        chooser.setMode(FileDialog.SAVE);
                        chooser.setTitle("Save This Game");
                        chooser.show();
                        returnVal = chooser.getFile();
                }
                if (returnVal!=null) {
                        gamefile = new File(chooser.getDirectory()+returnVal);
                        return true;
                }
                */
             ///*
                int returnVal;
                chooser.setCurrentDirectory(new File(workingdir,"Saves"));
                if (loading) {
                        chooser.setDialogTitle("Load a Saved Game");
                        returnVal = chooser.showOpenDialog(frame);
                }
                else {
                        chooser.setDialogTitle("Save This Game");
                        returnVal = chooser.showSaveDialog(frame);
                }
                if (returnVal==JFileChooser.APPROVE_OPTION) {
                        gamefile = chooser.getSelectedFile();
                        return true;
                }
             //*/
                return false;
        }

        public bool loadGame(bool fromtitle) {
                return loadGame(fromtitle,false);
        }
        public bool loadGame(bool fromtitle,bool nocharload) {
                bool worked = loadGame(fromtitle,nocharload,false);
                while (!worked) {
                        if (setGameFile(true)) worked = loadGame(fromtitle,nocharload,false);
                        else {
                                shutDown();
                                //if (music!=null) { musicloop = 0; music.stop(); music.deallocate(); music.close(); }
                                //System.exit(0);
                        }
                }
                return true;
        }
        public bool loadGame(bool fromtitle,bool nocharload,bool junk) {
                try {
                if (!fromtitle) {
                        if (!nocharload) { spellsheet.setVisible(false); weaponsheet.setVisible(false); formation.setVisible(false); formation.ischanging=false; }
                        arrowsheet.setVisible(false);
                        showCenter(loadinglabelpan);
                }
                nomovement = true;
                message.clear();
                
                FileInputStream in = new FileInputStream(gamefile);
                ObjectInputStream si = new ObjectInputStream(in);
                
                //System.out.println("Opened File");
                
                if (!fromtitle) {
                  //clear some stuff
                  dmmons.clear();
                  dmprojs.clear();
                  mapchanging = false;
                  cloudchanging = false;
                  fluxchanging = false;
                  mapstochange.clear();
                  cloudstochange.clear();
                  fluxcages.clear();
                  walkqueue.clear();
                  weaponqueue.clear();
                  actionqueue.clear();
                  Compass.clearList();
                  if (leveldirloaded!=null) leveldirloaded.clear();
                  stopSounds(true);
                  viewing = false;
                  //animimgs.clear();
                  if (!nocharload) {
                          hpanel.removeAll();
                          for (int i=0;i<numheroes;i++) {
                                hero[i]=null;
                          }
                          //System.out.println("Cleared some stuff");
                  }
                }
                //map version string
                string ver = si.readUTF();
                if (!ver.equals(version)) {
                        in.close();
                        if (!fromtitle) {
                                if (!sheet) showCenter(dview);
                                else showCenter(herosheet);
                        }
                        message.setMessage("Incorrect Map Version: Found "+ver+", need "+version,4);
                        System.out.println("Incorrect Map Version: Found "+ver+", need "+version);
                        if (!this.isShowing()) JOptionPane.showMessageDialog(this, "Incorrect Map Version: Found "+ver+", need "+version, "Error!", JOptionPane.ERROR_MESSAGE);
                        else JOptionPane.showMessageDialog(this, "Incorrect Map Version: Found "+ver+", need "+version+"\nLoad another game or exit and restart.", "Error!", JOptionPane.ERROR_MESSAGE);
                        return false;
                }
                //skip over map start info
                si.readbool();
                si.readbool();

                //global stuff
                counter = si.readInt();
                leveldarkfactor = (int[])si.readObject();
                darkcounter = si.readInt();
                darkfactor = si.readInt();
                magictorch = si.readInt();
                magicvision = si.readInt();
                floatcounter = si.readInt();
                dispell = si.readInt();
                slowcount = si.readInt();
                freezelife = si.readInt();
                mapchanging = si.readbool();
                cloudchanging = si.readbool();
                fluxchanging = si.readbool();
                level = si.readInt();
                partyx = si.readInt();
                partyy = si.readInt();
                facing = si.readInt();
                leader = si.readInt();
                heroatsub = (int[])si.readObject();
                iteminhand = si.readbool();
                if (iteminhand) {
                        inhand = (Item)si.readObject();
                        if (inhand.number==8) Compass.addCompass(inhand);
                }
                spellready = si.readInt();
                weaponready = si.readInt();
                mirrorback = si.readbool();
                
                //System.out.println("globals loaded");
                
                //monsters
                int nummons = si.readInt();
                int monnum;
                bool isdying;
                Monster tempmon;
                for (int i=0;i<nummons;i++) {
                        isdying = si.readbool();
                        monnum = si.readInt();
                        //if (monnum>maxmonnum) maxmonnum = monnum;
                        if (monnum<28) tempmon = new Monster(monnum,si.readInt(),si.readInt(),si.readInt());
                        else tempmon = new Monster(monnum,si.readInt(),si.readInt(),si.readInt(),si.readUTF(),si.readUTF(),si.readUTF(),si.readUTF(),si.readbool(),si.readbool(),si.readbool(),si.readbool());
                        tempmon.subsquare = si.readInt();
                        tempmon.health = si.readInt();
                        tempmon.maxhealth = si.readInt();
                        tempmon.mana = si.readInt();
                        tempmon.maxmana = si.readInt();
                        tempmon.facing = si.readInt();
                        tempmon.currentai = si.readInt();
                        tempmon.defaultai = si.readInt();
                        tempmon.HITANDRUN = si.readbool();
                        tempmon.isImmaterial = si.readbool();
                        tempmon.wasfrightened = si.readbool();
                        tempmon.hurt = si.readbool();
                        tempmon.wasstuck = si.readbool();
                        tempmon.ispoisoned = si.readbool();
                        if (tempmon.ispoisoned) {
                                tempmon.poisonpow = si.readInt();
                                tempmon.poisoncounter = si.readInt();
                        }
                        tempmon.timecounter = si.readInt();
                        tempmon.movecounter = si.readInt();
                        tempmon.randomcounter = si.readInt();
                        tempmon.runcounter = si.readInt();
                        tempmon.carrying = (ArrayList)si.readObject();
                        if (si.readbool()) tempmon.equipped = (ArrayList)si.readObject();
                        tempmon.gamewin = si.readbool();
                        //if (tempmon.gamewin) { tempmon.endanim = si.readUTF(); tempmon.endmusic = si.readUTF(); tempmon.endsound = si.readUTF(); }
                        if (tempmon.gamewin) { tempmon.endanim = si.readUTF(); tempmon.endsound = si.readUTF(); }
                        tempmon.hurtitem = si.readInt();
                        tempmon.needitem = si.readInt();
                        tempmon.needhandneck = si.readInt();
                        
                        tempmon.power = si.readInt();
                        tempmon.defense = si.readInt();
                        tempmon.magicresist = si.readInt();
                        tempmon.speed = si.readInt();
                        tempmon.movespeed = si.readInt();
                        tempmon.attackspeed = si.readInt();
                        tempmon.poison = si.readInt();
                        tempmon.fearresist = si.readInt();
                        tempmon.hasmagic = si.readbool();
                        if (tempmon.hasmagic) {
                                tempmon.castpower = si.readInt();
                                tempmon.manapower = si.readInt();
                                tempmon.numspells = si.readInt();
                                if (tempmon.numspells>0) tempmon.knownspells = (string[])si.readObject();
                                else tempmon.hasmagic = false;
                                tempmon.minproj = si.readInt();
                                tempmon.hasheal = si.readbool();
                                tempmon.hasdrain = si.readbool();
                        }
                        //tempmon.ammonumber = si.readInt();
                        tempmon.useammo = si.readbool();
                        if (tempmon.useammo) tempmon.ammo = si.readInt();
                        tempmon.pickup = si.readInt();
                        tempmon.steal = si.readInt();
                        tempmon.poisonimmune = si.readbool();
                        tempmon.powerboost = si.readInt();
                        tempmon.defenseboost = si.readInt();
                        tempmon.magicresistboost = si.readInt();
                        tempmon.speedboost = si.readInt();
                        tempmon.manapowerboost = si.readInt();
                        tempmon.movespeedboost = si.readInt();
                        tempmon.attackspeedboost = si.readInt();
                        tempmon.silenced = si.readbool();
                        if (tempmon.silenced) tempmon.silencecount = si.readInt();
                        tempmon.isdying = isdying;
                        
                        dmmons.put(tempmon.level+","+tempmon.x+","+tempmon.y+","+tempmon.subsquare,tempmon);
                        //System.out.println(tempmon.level+","+tempmon.x+","+tempmon.y);                        
                }
                //System.out.println("mons loaded, "+dmmons.size()+" total");

                //projectiles
                int numprojs = si.readInt();
                bool type,isending;
                Projectile tempproj;
                for (int i=0;i<numprojs;i++) {
                        isending = si.readbool();
                        type = si.readbool();
                        if (type) tempproj = new Projectile((Item)si.readObject(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readbool());
                        else tempproj = new Projectile((Spell)si.readObject(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readbool());
                        tempproj.isending = isending;
                        dmprojs.add(tempproj);
                }
                //System.out.println("projs loaded\n");

                //heroes
                numheroes = si.readInt();
                //System.out.println(numheroes+" heroes to make");
                for (int i=0;i<numheroes;i++) {
                        hero[i] = new Hero(si.readUTF());
                        hero[i].heronumber = i;
                        hero[i].load(si);
                        /*
                        hero[i].subsquare = si.readInt();
                        hero[i].number = si.readInt();
                        hero[i].name = si.readUTF();
                        hero[i].lastname = si.readUTF();
                        hero[i].maxhealth = si.readInt();
                        hero[i].health = si.readInt();
                        hero[i].maxstamina = si.readInt();
                        hero[i].stamina = si.readInt();
                        hero[i].maxmana = si.readInt();
                        hero[i].mana = si.readInt();
                        //hero[i].maxload = si.readFloat();
                        hero[i].load = si.readFloat();
                        hero[i].food = si.readInt();
                        hero[i].water = si.readInt();
                        hero[i].strength = si.readInt();
                        hero[i].vitality = si.readInt();
                        hero[i].dexterity = si.readInt();
                        hero[i].intelligence = si.readInt();
                        hero[i].wisdom = si.readInt();
                        hero[i].defense = si.readInt();
                        hero[i].magicresist = si.readInt();
                        hero[i].strengthboost = si.readInt();
                        hero[i].vitalityboost = si.readInt();
                        hero[i].dexterityboost = si.readInt();
                        hero[i].intelligenceboost = si.readInt();
                        hero[i].wisdomboost = si.readInt();
                        hero[i].defenseboost = si.readInt();
                        hero[i].magicresistboost = si.readInt();
                        hero[i].flevel = si.readInt();
                        hero[i].nlevel = si.readInt();
                        hero[i].plevel = si.readInt();
                        hero[i].wlevel = si.readInt();
                        hero[i].flevelboost = si.readInt();
                        hero[i].nlevelboost = si.readInt();
                        hero[i].plevelboost = si.readInt();
                        hero[i].wlevelboost = si.readInt();
                        hero[i].fxp = si.readInt();
                        hero[i].nxp = si.readInt();
                        hero[i].pxp = si.readInt();
                        hero[i].wxp = si.readInt();
                        hero[i].isdead = si.readbool();
                        //hero[i].splready = si.readbool();
                        hero[i].wepready = si.readbool();
                        hero[i].ispoisoned = si.readbool();
                        if (hero[i].ispoisoned) {
                                hero[i].poison = si.readInt();
                                hero[i].poisoncounter = si.readInt();
                        }
                        hero[i].silenced = si.readbool();
                        if (hero[i].silenced) hero[i].silencecount = si.readInt();
                        hero[i].hurtweapon = si.readbool();
                        hero[i].hurthand = si.readbool();
                        hero[i].hurthead = si.readbool();
                        hero[i].hurttorso = si.readbool();
                        hero[i].hurtlegs = si.readbool();
                        hero[i].hurtfeet = si.readbool();
                        hero[i].timecounter = si.readInt();
                        //hero[i].hurtcounter = si.readInt();
                        hero[i].walkcounter = si.readInt();
                        hero[i].spellcount = si.readInt();
                        hero[i].weaponcount = si.readInt();
                        hero[i].kuswordcount = si.readInt();
                        hero[i].rosbowcount = si.readInt();
                        hero[i].currentspell = si.readUTF();
                        //read abilities here
                        int numabils = si.readInt();
                        if (numabils>0) {
                                hero[i].abilities = new SpecialAbility[numabils];
                                for (int j=0;j<numabils;j++) {
                                        hero[i].abilities[j] = new SpecialAbility(si);
                                }
                        }
                        if (si.readbool()) {
                                hero[i].weapon = (Item)si.readObject();
                                if (hero[i].weapon.number==9) ((Torch)hero[i].weapon).setPic();
                        }
                        else hero[i].weapon=fistfoot;
                        if (si.readbool()) {
                                hero[i].hand = (Item)si.readObject();
                                if (hero[i].hand.number==9) ((Torch)hero[i].hand).setPic();
                        }
                        if (si.readbool()) hero[i].head = (Item)si.readObject();
                        if (si.readbool()) hero[i].torso = (Item)si.readObject();
                        if (si.readbool()) hero[i].legs = (Item)si.readObject();
                        if (si.readbool()) hero[i].feet = (Item)si.readObject();
                        if (si.readbool()) {
                                hero[i].neck = (Item)si.readObject();
                                if (hero[i].neck.number==89) numillumlets++;
                        }
                        if (si.readbool()) hero[i].pouch1 = (Item)si.readObject();
                        if (si.readbool()) hero[i].pouch2 = (Item)si.readObject();
                        hero[i].quiver = (Item[])si.readObject();
                        hero[i].pack = (Item[])si.readObject();
                        hero[i].setMaxLoad();
                        */
                        if (i==leader) hero[i].isleader=true;
                        hero[i].setVisible(true);
                        hpanel.add(hero[i]);
                        if (!hero[i].isdead) {
                                hero[i].removeMouseListener(hclick);
                                hero[i].addMouseListener(hclick);
                        }
                        if (hero[i].neck!=null && hero[i].neck.number==89) numillumlets++;
                }
                if (fromtitle || nocharload) {
                        if (weaponsheet==null) {
                                spellsheet = new SpellSheet();
                                weaponsheet = new WeaponSheet();
                        }
                        if (!weaponsheet.isShowing()) {
                                eastpanel.removeAll();
                                eastpanel.add(ecpanel);
                                eastpanel.add(Box.createVerticalStrut(10));
                                eastpanel.add(spellsheet);
                                eastpanel.add(Box.createVerticalStrut(20));
                                eastpanel.add(weaponsheet);
                                eastpanel.add(Box.createVerticalStrut(10));
                                eastpanel.add(arrowsheet);
                        }
                        herosheet.removeMouseListener(sheetclick);
                        herosheet.addMouseListener(sheetclick);
                }
                //System.out.println("got here 1");
                for (int i=0;i<numheroes;i++) hero[i].doCompass();
                Compass.updateCompass(facing);
                compassface = facing;
                //System.out.println("heroes loaded");
                hpanel.validate();
                formation.addNewHero();
                
                //mapObjects
                int oldlevels = numlevels, oldmapwidth = mapwidth, oldmapheight = mapheight;
                numlevels = si.readInt();
                mapwidth = si.readInt();
                mapheight = si.readInt();
                MapObject[][][] oldmapobject = DungeonMap;
                MapObject oldmap;
                DungeonMap = new MapObject[numlevels][mapwidth][mapheight];
                for (int l=0;l<numlevels;l++) {
                    for (int x=0;x<mapwidth;x++) {
                        for (int y=0;y<mapheight;y++) {
                            if (l<oldlevels && x<oldmapwidth && y<oldmapheight) oldmap = oldmapobject[l][x][y];
                            else oldmap = null;
                            DungeonMap[l][x][y] = loadMapObject(si,oldmap);
                        }
                    }
                }
                oldmapobject = null;
                /*
                //set any necessary switch changeto pointers
                while (switchloading.size()>0) {
                        oldmap = (MapObject)switchloading.remove(0);
                        if (oldmap.mapchar=='s') ((FloorSwitch)oldmap).setChangeTo();
                        else ((WallSwitch)oldmap).setChangeTo();
                        //if (oldmap.mapchar=='s') ((FloorSwitch)oldmap).changeto = DungeonMap[((FloorSwitch)oldmap).targetlevel][((FloorSwitch)oldmap).targetx][((FloorSwitch)oldmap).targety];
                        //else ((WallSwitch)oldmap).changeto = DungeonMap[((WallSwitch)oldmap).targetlevel][((WallSwitch)oldmap).targetx][((WallSwitch)oldmap).targety];
                }
                */
                if (mapchanging) {
                   int numchanging = si.readInt();
                   for (int i=0;i<numchanging;i++) {
                        mapstochange.add(si.readObject());
                   }
                }
                if (cloudchanging) {
                   PoisonCloud tempcloud;
                   int numclouds = si.readInt();
                   for (int i=0;i<numclouds;i++) {
                        tempcloud = new PoisonCloud(si.readInt(),si.readInt(),si.readInt(),si.readInt());
                        tempcloud.stagecounter = si.readInt();
                        //cloudstochange.add(tempcloud);
                   }
                }
                if (fluxchanging) {
                   FluxCage tempcage;
                   int numcages = si.readInt();
                   for (int i=0;i<numcages;i++) {
                        tempcage = new FluxCage(si.readInt(),si.readInt(),si.readInt());
                        tempcage.counter = si.readInt();
                        //fluxcages.put(tempcage.level+","+tempcage.x+","+tempcage.y,tempcage);
                   }
                }
                //load ambient sound data
                int numsounds = si.readInt();
                for (int i=0;i<numsounds;i++) {
                        string clipfile = si.readUTF();
                        loopsounds.add(new LoopSound(getClip(clipfile),clipfile,si.readInt(),si.readInt(),si.readInt(),si.readInt()));
                }

                //load dmmap here
                if (si.readbool()) {
                        AUTOMAP = true;
                        char[][][] map = (char[][][])si.readObject();
                        if (dmmap==null) {
                                dmmap = new DMMap(this,numlevels,mapwidth,mapheight,map);
                                hspacebox.add(dmmap); hspacebox.add(Box.createHorizontalGlue()); 
                        }
                        else dmmap.setMap(numlevels,mapwidth,mapheight,map);
                        dmmap.invalidate();
                        vspacebox.invalidate();
                        mappane.validate();
                        if (mappane.isVisible()) {
                                validate();
                                repaint();
                        }
                }
                else if (dmmap!=null) {
                        dmmap.setMap(numlevels,mapwidth,mapheight,null);
                        dmmap.invalidate();
                        vspacebox.invalidate();
                        mappane.validate();
                        if (mappane.isVisible()) {
                                validate();
                                repaint();
                        }
                }
                /*
                else if (AUTOMAP) {
                        dmmap = new DMMap(this,numlevels,mapwidth,mapheight,null);
                        //mappane.setViewportView(dmmap);
                        hspacebox.add(dmmap); hspacebox.add(Box.createHorizontalGlue()); 
                        dmmap.invalidate();
                        vspacebox.invalidate();
                        //hspacebox.validate();
                        mappane.validate();
                }
                */
                optionsdialog.resetOptions();
                
                //load map picture directory modifier
                leveldir = new string[numlevels];
                for (int l=0;l<numlevels;l++) {
                        leveldir[l] = si.readUTF();
                        if (leveldir[l].equals("")) leveldir[l]=null;
                }
                
                //System.out.println("Map loaded, waiting on pics");
                in.close();

                sheet = false;
                //System.out.println("about to wait for images");
                //ImageTracker.waitForID(0,10000);//map pics
                //ImageTracker.waitForID(1,10000);//other map pics
                ImageTracker.waitForID(0,10000);//map pics
                ImageTracker.waitForID(1,10000);//some interface pics
                //ImageTracker.waitForID(1);//other map pics
                //ImageTracker.checkID(2,true);//spell pics
                Item.doFlaskBones();
                Item.ImageTracker.waitForID(0,8000);
                //ImageTracker = null;
                //MapObject.tracker = null;
                //ImageTracker = new MediaTracker(this);
                //MapObject.tracker = ImageTracker;
                Item.ImageTracker = null;
                Item.ImageTracker = new MediaTracker(this);
                System.gc();
                System.runFinalization();
                //Runtime.getRuntime().gc();
                ///spellsheet.setVisible(true);
                ///weaponsheet.setVisible(true);
                ///formation.setVisible(true);
                ///arrowsheet.setVisible(true); 
                hupdate();
                spellsheet.repaint();
                weaponsheet.repaint();
                //System.out.println("got here 1");
                //System.out.println("got here 2");
                //System.out.println("got here 3");
                //weaponsheet.weaponButton[weaponready].setSelected(true);
                //if (weaponsheet.showingspecials) weaponsheet.toggleSpecials(weaponready);
                changeCursor();
                updateDark();
                imagePane.validate();
                nomovement = false;
                //System.gc();
                //dmmap = new DMMap(this);
                showCenter(dview);
                message.setMessage("Game Loaded.",4);
                spellsheet.setVisible(true);
                weaponsheet.setVisible(true);
                formation.setVisible(true);
                arrowsheet.setVisible(true); 
                }
                //catch (InterruptedException e) { System.out.println("Interrupted!"); if (!fromtitle) { if (!sheet) centerlay.show(centerpanel,"dview"); else centerlay.show(centerpanel,"hsheet"); } return true; }
                catch (Exception e) {
                        if (!fromtitle) {
                                if (!sheet) {
                                        showCenter(dview);
                                }
                                else {
                                        showCenter(herosheet);
                                }
                        }
                        message.setMessage("Unable to load game!",4);
                        System.out.println("Unable to load game.");
                        //pop up a dialog too
                        if (!this.isShowing()) JOptionPane.showMessageDialog(this, "Unable to load game!", "Error!", JOptionPane.ERROR_MESSAGE);
                        else JOptionPane.showMessageDialog(this, "Unable to load game!\nLoad another game or exit and restart.", "Error!", JOptionPane.ERROR_MESSAGE);
                        e.printStackTrace();
                        return false;
                }
                return true;
        }
        
        static public MapObject loadMapObject(ObjectInputStream si) throws IOException,ClassNotFoundException { return loadMapObject(si,null); }
        static public MapObject loadMapObject(ObjectInputStream si, MapObject oldmap) throws IOException,ClassNotFoundException {
            char mapchar;
            MapObject m = null;
            //bool canHoldItems,isPassable,canPassProjs,canPassMons,canPassImmaterial,drawItems,drawFurtherItems,hasParty,hasMons,hasImmaterialMons,hasItems;
            bool canHoldItems,isPassable,canPassProjs,canPassMons,canPassImmaterial,drawItems,drawFurtherItems,hasParty,hasMons,hasItems;
            int numProjs;
            ArrayList mapItems = null;
            
            mapchar = si.readChar();
            canHoldItems = si.readbool();
            isPassable = si.readbool();
            canPassProjs = si.readbool();
            canPassMons = si.readbool();
            canPassImmaterial = si.readbool();
            drawItems = si.readbool();
            drawFurtherItems = si.readbool();
            numProjs = si.readInt();
            hasParty = si.readbool();
            hasMons = si.readbool();
            //hasImmaterialMons = si.readbool();
            hasItems = si.readbool();
            if (hasItems) mapItems = (ArrayList)si.readObject();
            //else if ((canHoldItems && mapchar!='0') || mapchar=='f' || mapchar=='a' || mapchar==']') mapItems = new ArrayList(4);
            switch (mapchar) {
                case '1': //wall
                        if (!canPassImmaterial) m = outWall;
                        else if (oldmap!=null && oldmap.mapchar=='1') m = oldmap;
                        else m = new Wall();
                        break;
                case '0': //floor
                        if (oldmap!=null && oldmap.mapchar=='0') m = oldmap;
                        else m = new Floor();
                        break;
                case 'd': //door
                        m = new Door((MapPoint)si.readObject(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readbool(),si.readbool(),si.readInt(),si.readbool(),si.readInt(),si.readInt(),si.readbool(),si.readInt());
                        //((Door)m).changecount = si.readInt();
                        //((Door)m).isclosing = si.readbool();
                        //if ( ((Door)m).isBreakable && !((Door)m).isBroken ) ((Door)m).breakpoints = si.readInt();
                        m.load(si);
                        break;
                case 's': //floorswitch
                        m = new FloorSwitch();
                        m.load(si);//for everything
                        break;
                case '/': //wallswitch
                        m = new WallSwitch(si.readInt());
                        m.load(si);//for everything but side
                        break;
                case 't': //teleport
                        //m = new Teleport(si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readInt(),si.readInt(),si.readbool(),si.readbool(),si.readbool(),si.readbool(),si.readbool(),si.readInt(),si.readInt(),si.readbool(),si.readbool(),si.readInt(),si.readbool(),si.readInt(),si.readbool());
                        //m = new Teleport(si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readbool(),si.readInt(),si.readInt(),si.readbool(),si.readbool(),si.readbool(),si.readbool(),si.readbool(),si.readbool(),si.readInt(),si.readInt(),si.readbool(),si.readbool(),si.readInt(),si.readbool(),si.readInt(),si.readbool());
                        m = new Teleport();
                        m.load(si);
                        break;
                case ']': //onealcove
                        m = new OneAlcove(si.readInt());
                        m.load(si);//for floorswitch stuff
                        break;
                case '[': //alcove
                        m = new Alcove();
                        m.load(si);//for vectors and floorswitch stuff
                        break;
                case 'a': //altar
                        m = new Altar(si.readInt());
                        m.load(si);//for floorswitch stuff
                        break;
                case '2': //fakewall
                        if (oldmap!=null && oldmap.mapchar=='2') m = oldmap;
                        else m = new FakeWall();
                        break;
                case 'f': //fountain
                        m = new Fountain(si.readInt());
                        m.load(si);
                        break;
                case 'p': //pit
                        m = new Pit(si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readbool(),si.readbool(),si.readbool(),si.readbool(),si.readbool(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readInt(),si.readInt(),si.readbool());
                        ((Pit)m).blinkcounter = si.readInt();
                        ((Pit)m).delaying = si.readbool();
                        ((Pit)m).delaycounter = si.readInt();
                        ((Pit)m).resetting = si.readbool();
                        ((Pit)m).resetcounter = si.readInt();
                        break;
                case '>': //stairs
                        int side = si.readInt();
                        bool goesUp = si.readbool();
                        if (oldmap!=null && oldmap.mapchar=='>' && ((Stairs)oldmap).goesUp==goesUp) {
                                m = oldmap;
                                ((Stairs)m).side = side;
                        }
                        else m = new Stairs(side,goesUp);
                        //m = new Stairs(si.readInt(),si.readbool());
                        break;
                case 'l': //launcher
                        side = si.readInt();
                        //m = new Launcher(si.readInt(),si.readInt(),si.readInt(),(dmnew)frame,side,si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool());
                        m = new Launcher(si.readInt(),si.readInt(),si.readInt(),(dmnew)frame,side,si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool());
                        m.load(si);
                        break;
                case 'm': //mirror
                        m = new Mirror(si.readInt());
                        /*
                        if (!si.readbool()) {
                              ((Mirror)m).wasUsed = false;
                              ((Mirror)m).hero = ((dmnew)frame).new Hero(si.readUTF());
                        }
                        else ((Mirror)m).wasUsed = true;
                        */
                        m.load(si);//for hero and wasused
                        break;
                case 'g': //generator
                        //m = new Generator(si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),(dmnew)frame,si.readInt(),si.readbool(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),new MonsterData(si.readInt()));
                        //((Generator)m).monster.load(si);
                        m = new Generator(si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),(dmnew)frame,si.readInt(),si.readbool(),si.readInt(),si.readInt(),si.readInt(),si.readInt(),si.readbool(),new MonsterData(si));
                        ((Generator)m).delaying = si.readbool();
                        //if (((Generator)m).monster.number>maxmonnum) maxmonnum = ((Generator)m).monster.number;
                        break;
                case 'w': //writing
                        side = si.readInt();
                        string[] message = (string[])si.readObject();
                        if (oldmap!=null && oldmap.mapchar=='w') {
                                m = oldmap;
                                ((Writing2)m).side = side;
                                ((Writing2)m).setMessage(message);
                        }
                        else m = new Writing2(side,message);
                        //m = new Writing2(si.readInt(),(string[])si.readObject());
                        break;
                case 'W': //gamewinsquare (note: has same mapchar as writing2 -> but one of writings will go away)
                        m = new GameWinSquare(si.readUTF(),si.readUTF());
                        break;
                case 'S': //multfloorswitch
                        m = new MultFloorSwitch2();
                        m.load(si);//for everything
                        break;
                case '\\': //multwallswitch
                        m = new MultWallSwitch2(si.readInt());
                        m.load(si);//for everything except side
                        break;
                case '}': //sconce
                        m = new Sconce(si.readInt());
                        m.load(si);//for torch and switch stuff
                        break;
                case '!': //stormbringer
                        m = new Stormbringer(si.readbool());
                        break;
                case 'G': //power gem
                        m = new PowerGem(si.readbool());
                        break;
                case 'D': //decoration
                        side = si.readInt();
                        int number = si.readInt();
                        if (oldmap!=null && oldmap.mapchar=='D') {
                                m = oldmap;
                                ((Decoration)m).side = side;
                                ((Decoration)m).setNumber(number);
                        }
                        else m = new Decoration(side,number);
                        //m = new Decoration(si.readInt(),si.readInt());
                        break;
                case 'F': //floor decoration
                        number = si.readInt();
                        if (oldmap!=null && oldmap.mapchar=='F') {
                                m = oldmap;
                                ((FDecoration)m).setNumber(number);
                        }
                        else m = new FDecoration(number);
                        if (number==3) {
                                ((FDecoration)m).level = si.readInt();
                                ((FDecoration)m).xcoord = si.readInt();
                                ((FDecoration)m).ycoord = si.readInt();
                        }
                        //m = new FDecoration(si.readInt());
                        break;
                case 'P': //pillar
                        number = si.readInt();
                        bool mirror = si.readbool();
                        if (oldmap!=null && oldmap.mapchar=='P') {
                                m = oldmap;
                                ((Pillar)m).setPillar(number,mirror);
                        }
                        else m = new Pillar(number,mirror);
                        if (number==2) m.load(si); //for custom pics
                        //m = new Pillar(si.readInt(),si.readbool());
                        break;
                case 'i': //invisible wall
                        if (!canPassImmaterial) m = invisWall;
                        else if (oldmap!=null && oldmap.mapchar=='i') m = oldmap;
                        else m = new InvisibleWall();
                        break;
                case 'E': //event square
                        m = new EventSquare((dmnew)frame);
                        m.load(si);
                        break;
                case 'y': //fulya pit
                        m = new FulYaPit((MapPoint)si.readObject(),si.readInt(),(MapPoint)si.readObject(),(MapPoint)si.readObject());
                        break;
                //case 'c': //customsided
                //        m = new CustomSided(si.readInt(),si.readUTF(),(int[])si.readObject(),(int[])si.readObject());
                //        break;
            }
            //if (oldmap!=null && m==oldmap) { m.mapItems = null; if (m.canHoldItems) m.mapItems = new ArrayList(); }
            m.canHoldItems = canHoldItems;
            m.isPassable = isPassable;
            m.canPassProjs = canPassProjs;
            m.canPassMons = canPassMons;
            m.canPassImmaterial = canPassImmaterial;
            m.drawItems = drawItems;
            m.drawFurtherItems = drawFurtherItems;
            m.numProjs = numProjs;
            m.hasParty = hasParty;
            m.hasMons = hasMons;
            //m.hasImmaterialMons = hasImmaterialMons;
            m.hasItems = hasItems;
            m.mapItems = mapItems;
            m.hasCloud = false;
            return m;
        }

        public void gameOver() {
                //play death music
                //music.setSpecial("Music"+File.separator+"confessions.mid");
                changeCursor();
                stopSounds(true);
                ImageIcon endicon = new ImageIcon(tk.getImage("Endings"+File.separator+"theend.gif"));
                ecpanel.setVisible(false);
                spellsheet.setVisible(false);
                weaponsheet.setVisible(false);
                arrowsheet.setVisible(false);
                //eastpanel.setVisible(false);
                message.setVisible(false);
                buttonpanel.setVisible(true);
                endiconlabel.setIcon(endicon);
                showCenter(endiconlabelpan);
                imagePane.validate();
                imagePane.repaint();
        }

        public void actionPerformed(ActionEvent e) {
                if (e.getActionCommand().startsWith("Load")) {
                        nomovement = true;
                        //bool shouldplay = false;
                        //if (music.isplaying) { shouldplay=true; music.stop(); }
                        if (!setGameFile(true)) {
                                nomovement = false;
                                return;
                        }
                        creditslabelpan.setVisible(false);
                        buttonpanel.setVisible(false);
                        hpanel.setVisible(true);
                        formation.setVisible(true);
                        toppanel.setVisible(true);
                        maincenterpan.setVisible(true);
                        ecpanel.setVisible(true);
                        eastpanel.setVisible(true);
                        spellsheet.setVisible(false);
                        weaponsheet.setVisible(false);
                        arrowsheet.setVisible(false);
                        message.clear();
                        message.setVisible(true);
                        addKeyListener(dmove);
                        dview.addMouseListener(dclick);
                        endcounter = 0;
                        showCenter(loadinglabelpan);
                        imagePane.doLayout();
                        paint(getGraphics());
                        //System.gc();
                        //if (shouldplay) music.start();
                        //if (music.isplaying) music.nextSong();
                        //music.stop();
                        loadGame(false);
                }
             /* //was for menus
             else if (e.getActionCommand().startsWith("Save")) {
                     bool chosefile = false;
                     if (gamefile==null || e.getActionCommand().endsWith("As")) {
                         chosefile = setGameFile(false);
                     }
                     else chosefile = true;
                     if (chosefile) saveGame();
             }
             else if (e.getActionCommand().equals("Help")) {
                     try { Runtime.getRuntime().exec("open Docs/readme.html"); }
                     catch(Exception ex){}
             }
             */
                else if (e.getActionCommand().equals("New Game") || e.getActionCommand().equals("New Custom")) { 
                        nomovement = true;
                        File mapfile;
                        if (e.getActionCommand().equals("New Custom")) {
                                /*
                                chooser.setDirectory("Dungeons");
                                chooser.setMode(FileDialog.LOAD);
                                chooser.setTitle("Load a Custom Dungeon Map");
                                chooser.show();
                                string returnVal = chooser.getFile();
                                if(returnVal!=null) {
                                        mapfile = new File(chooser.getDirectory()+returnVal);
                                }
                                else return;
                                */
                             ///*
                                chooser.setCurrentDirectory(new File(workingdir,"Dungeons"));
                                chooser.setDialogTitle("Load a Custom Dungeon Map");
                                int returnVal = chooser.showOpenDialog(frame);
                                if (returnVal==JFileChooser.APPROVE_OPTION) mapfile = chooser.getSelectedFile();
                                else return;
                             //*/
                        }
                        else mapfile = new File("Dungeons"+File.separator+"dungeon.dat"); 
                        //System.gc();
                        bool create,nochar;
                        int levelpoints=-1,hsmpoints=-1,statpoints=-1,defensepoints=-1,itempoints=0,abilitypoints=0,abilityauto=0;
                        Item[] itemchoose = null; SpecialAbility[] abilitychoose = null;
                        try {
                        
                        FileInputStream in = new FileInputStream(mapfile);
                        ObjectInputStream si = new ObjectInputStream(in);
                        
                     si.readUTF();//version
                        create = si.readbool();
                        nochar = si.readbool();
                        if (create) {
                                levelpoints = si.readInt();
                                hsmpoints = si.readInt();
                                statpoints = si.readInt();
                                defensepoints = si.readInt();
                                int num=si.readInt();
                                if (num>0) {
                                        itemchoose = new Item[num];
                                        for (int i=0;i<num;i++) {
                                                itemchoose[i] = (Item)si.readObject();
                                        }
                                        itempoints = si.readInt();
                                }
                                else { itemchoose = null; itempoints = 0; }
                                num=si.readInt();
                                if (num>0) {
                                        abilityauto = si.readInt();
                                        abilitychoose = new SpecialAbility[num];
                                        for (int i=0;i<num;i++) {
                                                abilitychoose[i] = new SpecialAbility(si);
                                        }
                                        abilitypoints = si.readInt();
                                }
                                else { abilitychoose = null; abilitypoints = 0; }
                        }
                        
                        in.close();
                        }
                        catch (Exception ex) {
                                System.out.println("Unable to load from map.");
                                ex.printStackTrace(System.out);
                                //pop up a dialog too
                                JOptionPane.showMessageDialog(this, "Unable to load map!", "Error!", JOptionPane.ERROR_MESSAGE);
                                return;
                        }
                        
                        dmmons.clear();
                        dmprojs.clear();
                        mapchanging = false;
                        cloudchanging = false;
                        fluxchanging = false;
                        mapstochange.clear();
                        cloudstochange.clear();
                        fluxcages.clear();
                        Compass.clearList();

                        hpanel.removeAll();
                        if (leveldirloaded!=null) leveldirloaded.clear();
                        hero[0]=null;
                        hero[1]=null;
                        hero[2]=null;
                        hero[3]=null;
                        heroatsub[0]=-1;
                        heroatsub[1]=-1;
                        heroatsub[2]=-1;
                        heroatsub[3]=-1;
                        numheroes = 0;
                        sheet=false;
                        
                        creditslabelpan.setVisible(false);
                        buttonpanel.setVisible(false);
                        hpanel.setVisible(true);
                        formation.setVisible(true);
                        toppanel.setVisible(true);
                        maincenterpan.setVisible(true);
                        ecpanel.setVisible(true);
                        spellsheet.setVisible(true);
                        weaponsheet.setVisible(true);
                        arrowsheet.setVisible(true);
                        eastpanel.setVisible(true);
                        message.setVisible(true);
                        dview.addMouseListener(dclick);
                        //if (music.isplaying) music.nextSong();
                        //music.stop();
                        nomovement = false;
                        if (create) {
                                removeKeyListener(dmove);
                                CreateCharacter createit = new CreateCharacter(frame,mapfile,create,nochar,levelpoints,hsmpoints,statpoints,defensepoints,itemchoose,itempoints,abilitychoose,abilityauto,abilitypoints,true,true);
                                CREATEFLAG = false;
                                setContentPane(createit);
                                validate();
                                repaint();
                        }
                        else {
                                JPanel p = new JPanel(new BorderLayout());
                                p.setBackground(new Color(0,0,64));
                                p.add("Center",Title.loading);
                                setContentPane(p);
                                validate();
                                paint(getGraphics());
                                loadMap(mapfile);
                                //message.repaint();
                                setContentPane(imagePane);
                                showCenter(dview);
                                addKeyListener(dmove);
                                finishNewGame();
                        }
                }
                else {
                        //System.gc();
                        didquit = true;
                }

        }
        public void finishNewGame() {        
                if (hero[0]!=null) {
                        numheroes=1;
                        heroatsub[0]=0;
                        hero[0].isleader=true;
                        hero[0].removeMouseListener(hclick);
                        hero[0].addMouseListener(hclick);
                        hpanel.add(hero[0]);
                        formation.addNewHero();
                        hero[0].repaint();
                        updateDark();
                        spellready=0;
                        spellsheet.repaint();
                        if (weaponsheet.showingspecials) weaponsheet.toggleSpecials(0);
                        weaponsheet.repaint();
                        if (!spellsheet.isShowing()) {
                                eastpanel.removeAll();
                                eastpanel.add(ecpanel);
                                eastpanel.add(Box.createVerticalStrut(10));
                                eastpanel.add(spellsheet);
                                eastpanel.add(Box.createVerticalStrut(20));
                                eastpanel.add(weaponsheet);
                                eastpanel.add(Box.createVerticalStrut(10));
                                eastpanel.add(arrowsheet);
                        }
                }
                else {
                        numheroes=0;
                        eastpanel.removeAll();
                        eastpanel.add(ecpanel);
                        eastpanel.add(Box.createVerticalStrut(10));
                        eastpanel.add(Box.createVerticalGlue());
                        eastpanel.add(arrowsheet);
                        eastpanel.add(Box.createVerticalStrut(20));
                        formation.addNewHero();
                        updateDark();
                        spellready=0;
                }
                hupdate();
                endcounter = 0;
        }

        public void gameWin() {
                
                //play win music
                //music.setSpecial("Endings"+File.separator+endmusic);
                /*
                ecpanel.setVisible(false);
                spellsheet.setVisible(false);
                weaponsheet.setVisible(false);
                arrowsheet.setVisible(false);
                message.setVisible(false);
                hpanel.setVisible(false);
                formation.setVisible(false);
                */
                //toppanel.setVisible(false);
                formation.setVisible(false);
                eastpanel.setVisible(false);
                message.setVisible(false);
                message.clear();
                stopSounds(true);
                
                //simply an animated gif for an ending
                ImageIcon animicon = new ImageIcon(tk.getImage("Endings"+File.separator+endanim));
                animicon.setImageObserver(this);
                animlabel.setIcon(animicon);
                
                showCenter(animlabelpan);
                playSound(endsound,-1,-1);
                
                while(waitcredits) {
                        try { Thread.currentThread().sleep(500); }
                        catch(InterruptedException ex) {}
                }
                if (!endanim.equals("stormend.gif")) {
                        try { Thread.currentThread().sleep(3000); }
                        catch(InterruptedException ex) {}
                }
                animicon.setImageObserver(null);
                maincenterpan.setVisible(false);
                //eastpanel.setVisible(false);
                hpanel.setVisible(false);
                creditslabelpan.setVisible(true);
                animicon.getImage().flush();
                validate();
                /*
         Player player = null;
         File mf = new File("Endings"+File.separator+endanim);
         string mediaFile = null;
         MediaLocator mrl = null;
         URL url = null;
        
         try {
             url = mf.toURL();
             mediaFile = url.toExternalForm();
         } catch (MalformedURLException mue) {}
         
         try {
             // Create a media locator from the file name
             if ((mrl = new MediaLocator(mediaFile)) == null)
             System.out.println("Can't build URL for " + mediaFile);
        
             // Create an instance of a player for this media
             try {
             player = Manager.createPlayer(mrl);
             } catch (NoPlayerException e) {
             System.out.println(e);
             System.out.println("Could not create player for " + mrl);
             }
        
             // Add ourselves as a listener for a player's events
             //player.addControllerListener(this);
             player.start();
        
         } catch (MalformedURLException e) {
             System.out.println("Invalid media file URL!");
         } catch (IOException e) {
             System.out.println("IO exception creating player for " + mrl);
         }
                
                message.setMessage("You Win",4);
         while (player.getState()<Controller.Realized) {}
         animlabel.add(player.getVisualComponent());
         animlabel.validate();
                */
        }

        public bool imageUpdate(Image img, int infoflags, int x, int y, int width, int height) {
                waitcredits = super.imageUpdate(img, infoflags, x, y, width, height);
                return waitcredits;
        }
        public void mousePressed(MouseEvent e) {
                //System.out.println("show buttons");
                buttonpanel.setVisible(true);
                validate();
        }
        
        public void componentResized(ComponentEvent e) {
                if (dmmap!=null) {
                        dmmap.updateSize();
                        dmmap.invalidate();
                        vspacebox.invalidate();
                        mappane.validate();
                        if (mappane.isVisible()) {
                                validate();
                                repaint();
                        }
                }
        }
//        public void componentMoved(ComponentEvent ev) {
//                if (currentDevice.getFullScreenWindow()!=null) {
//                        Component comp = (Component) ev.getSource();    
//                        comp.setLocation(frameLocation.x, frameLocation.y);
//                }
//        }
        
        public void mouseEntered(MouseEvent e) {}
        public void mouseExited(MouseEvent e) {}
        public void mouseClicked(MouseEvent e) {}
        public void mouseReleased(MouseEvent e) {}
        public void componentHidden(ComponentEvent e) {}
        public void componentShown(ComponentEvent e) {}
        




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

        class HeroClick extends MouseAdapter {

                public void mousePressed(MouseEvent e) {
                        int x=e.getX();
                        int y=e.getY();
                        Hero clickedhero = (Hero)e.getSource();
                        if (sleeping) {
                                needredraw=true;
                                sleeping = false;
                                sleeper = 0;
                                spellsheet.setVisible(true);
                                weaponsheet.setVisible(true);
                                arrowsheet.setVisible(true);
                                if (herosheet.hero.isdead) herosheet.setHero(hero[leader]);
                                else herosheet.repaint();
                        }
                        else if ((y>17 && y<83) || (e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) {//SwingUtilities.isRightMouseButton(e)) {
                                if (!sheet) {
                                        herosheet.setHero((Hero)e.getSource());
                                        sheet=true;
                                        showCenter(herosheet);
                                }
                                else {
                                        sheet=false;
                                        showCenter(dview);
                                        hupdate();
                                        if (!herosheet.hero.equals((Hero)e.getSource())) {
                                                ((Hero)e.getSource()).dispatchEvent(e);
                                        }
                                }
                        }
                        else if (y<18) {
                                //recalc loads?
                              //if (!clickedhero.isdead) {
                                if (iteminhand) { clickedhero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                hero[leader].isleader=false;
                                clickedhero.isleader=true;
                                if (hero[0].isleader) leader=0;
                                else if (hero[1].isleader) leader=1;
                                else if (hero[2].isleader) leader=2;
                                else leader=3;
                                hupdate();
                                if (sheet) herosheet.repaint();
                              //}
                        }
                        //weapon hand
                        else if (x>55 && x<89) {
                                if (clickedhero.weapon.number==215) {
                                        message.setMessage("Stormbringer resists...",4);
                                        return;
                                }
                                else if (clickedhero.weapon.number==219 || clickedhero.weapon.number==261) {
                                        return;
                                }
                                else if (clickedhero.weapon.cursed>0 && (clickedhero.weapon.type==Item.WEAPON || clickedhero.weapon.type==Item.SHIELD)) {
                                        message.setMessage(clickedhero.weapon.name+" is cursed and cannot be removed!",4);
                                        if (!clickedhero.weapon.cursefound) {
                                                clickedhero.weapon.cursefound = true;
                                                clickedhero.repaint();
                                                if (sheet) herosheet.repaint();
                                        }
                                        return;
                                }
                                if (iteminhand) { clickedhero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (clickedhero.weapon!=null) { clickedhero.load-=clickedhero.weapon.weight; hero[leader].load+=clickedhero.weapon.weight; }
                                if (!iteminhand && !clickedhero.weapon.name.equals("Fist/Foot")) {
                                        iteminhand=true;
                                        inhand=clickedhero.weapon;
                                        clickedhero.weapon=fistfoot;
                                        if (inhand.number==9) {
                                                ((Torch)inhand).putOut();
                                                updateDark();
                                        }
                                        if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) inhand.unEquipEffect(clickedhero);
                                        clickedhero.repaint();
                                        if (sheet) herosheet.repaint();
                                        weaponsheet.repaint();
                                        changeCursor();
                                }
                                else if (iteminhand) {
                                        if (!clickedhero.weapon.name.equals("Fist/Foot")) {
                                                Item tempitem = clickedhero.weapon;
                                                clickedhero.weapon=inhand;
                                                inhand=tempitem;
                                                if (inhand.number==9) {
                                                        ((Torch)inhand).putOut();
                                                        updateDark();
                                                }
                                                if (clickedhero.weapon.number==9) {
                                                        ((Torch)clickedhero.weapon).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) inhand.unEquipEffect(clickedhero);
                                                if (clickedhero.weapon.type==Item.WEAPON || clickedhero.weapon.type==Item.SHIELD || clickedhero.weapon.number==4) clickedhero.weapon.equipEffect(clickedhero);
                                        }
                                        else {
                                                clickedhero.weapon=inhand;
                                                iteminhand=false;
                                                if (clickedhero.weapon.number==9) {
                                                        ((Torch)clickedhero.weapon).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) clickedhero.weapon.equipEffect(clickedhero);
                                        }
                                        clickedhero.repaint();
                                        if (sheet) herosheet.repaint();
                                        weaponsheet.repaint();
                                        changeCursor();
                                }
                        }
                        //action hand
                        else if (x<45 && x>11) {
                                if (clickedhero.hand!=null && clickedhero.hand.cursed>0 && clickedhero.hand.type==Item.SHIELD) {
                                        message.setMessage(clickedhero.hand.name+" is cursed and cannot be removed!",4);
                                        if (!clickedhero.hand.cursefound) {
                                                clickedhero.hand.cursefound = true;
                                                clickedhero.repaint();
                                                if (sheet) herosheet.repaint();
                                        }
                                        return;
                                }
                                if (iteminhand) { clickedhero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (clickedhero.hand!=null) { clickedhero.load-=clickedhero.hand.weight; hero[leader].load+=clickedhero.hand.weight; }
                                if (!iteminhand && clickedhero.hand!=null) {
                                        iteminhand=true;
                                        inhand=clickedhero.hand;
                                        clickedhero.hand=null;
                                        if (inhand.number==9) {
                                                ((Torch)inhand).putOut();
                                                updateDark();
                                        }
                                        if (inhand.type==Item.SHIELD) inhand.unEquipEffect(clickedhero);
                                        clickedhero.repaint();
                                        if (sheet) herosheet.repaint();
                                        //holdingupdate();
                                        changeCursor();
                                }
                                else if (iteminhand) {
                                        if (clickedhero.hand!=null) {
                                                Item tempitem = clickedhero.hand;
                                                clickedhero.hand=inhand;
                                                inhand=tempitem;
                                                if (inhand.number==9) {
                                                        ((Torch)inhand).putOut();
                                                        updateDark();
                                                }
                                                if (clickedhero.hand.number==9) {
                                                        ((Torch)clickedhero.hand).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.SHIELD) inhand.unEquipEffect(clickedhero);
                                                if (clickedhero.hand.type==Item.SHIELD) clickedhero.hand.equipEffect(clickedhero);
                                        }
                                        else {
                                                clickedhero.hand=inhand;
                                                iteminhand=false;
                                                if (clickedhero.hand.number==9) {
                                                        ((Torch)clickedhero.hand).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.SHIELD) clickedhero.hand.equipEffect(clickedhero);
                                        }
                                        clickedhero.repaint();
                                        if (sheet) herosheet.repaint();
                                        //holdingupdate();
                                        changeCursor();
                                }
                        }
                }
        }

        class HeroSheet extends JPanel {//JComponent { //Canvas {
                Hero hero;
                Image offscreen;
                Graphics2D offg,curseg;
                //BufferedImage offscreen;
                //Graphics2D offg;
                MirrorClick mirrorclick;
                bool mirror = false;
                bool stats = false;
                bool skipchestscroll = false;
                bool mirrorflag = false;
                bool showresreinc = true;

                public HeroSheet() {
                        setBackground(Color.black);
                        /*setDoubleBuffered(true);*/
                }
                public HeroSheet(Hero h) {
                        hero = h;
                        setSize(448,326);
                        //setPreferredSize(new Dimension(448,326));
                        setBackground(Color.black);
                        /*setDoubleBuffered(true);*/
                }
                
                public void setHero(Hero h) {
                        hero = h;
                        repaint();
                        hero.repaint();
                }
                public void setHero(Hero h,bool f) {
                        removeMouseListener(sheetclick);
                        hero = h;
                        mirror = true;
                        repaint();
                        hero.repaint();
                        mirrorflag = f;
                        if (mirrorclick==null) mirrorclick = new MirrorClick();
                        addMouseListener(mirrorclick);
                }

                public void paintComponent(Graphics g) {
                        if (sleeping) { sleepsheet(); g.drawImage(offscreen,0,0,this.size().width,this.size().height,null); return; }
                        else if (stats) { showStats(); g.drawImage(offscreen,0,0,this.size().width,this.size().height,null); return; }
                        else if (viewing) { showItem(); g.drawImage(offscreen,0,0,this.size().width,this.size().height,null); return; }
                        
                        //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                        offg.setFont(dungfont14);
                        offg.drawImage(hsheet,0,0,null);

                        offg.setColor(new Color(30,30,30));
                        offg.drawstring(hero.name+"   "+hero.lastname,8,18);
                        offg.drawstring("Health",18,288);
                        offg.drawstring((hero.health+hero.berserkhealth)+" / "+hero.maxhealth,128,288);//was 290
                        offg.drawstring("Stamina",18,306);
                        offg.drawstring(hero.stamina+" / "+hero.maxstamina,128,306);//was 308
                        offg.drawstring("Mana",18,324);
                        offg.drawstring(hero.mana+" / "+hero.maxmana,128,324);

                        if (leader==hero.heronumber) offg.setColor(Color.yellow);
                        else offg.setColor(Color.white);
                        offg.drawstring(hero.name+"   "+hero.lastname,5,15);
                        if (leader==hero.heronumber) offg.setColor(Color.white);
                        offg.drawstring("Health",15,285);//was 252
                        offg.drawstring("Stamina",15,303);//was 270
                        offg.drawstring("Mana",15,321);//was 288

                        if ((hero.health+hero.berserkhealth)<(hero.maxhealth/3)) offg.setColor(Color.red);
                        else if (hero.berserkhealth>0) offg.setColor(Color.green);
                        else offg.setColor(Color.white);
                        offg.drawstring((hero.health+hero.berserkhealth)+" / "+hero.maxhealth,125,285);

                        if (hero.stamina<(hero.maxstamina/3)) offg.setColor(Color.red);
                        else offg.setColor(Color.white);

                        offg.drawstring(hero.stamina+" / "+hero.maxstamina,125,303);

                        if (hero.mana<(hero.maxmana/3)) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawstring(hero.mana+" / "+hero.maxmana,125,321);
                        
                        offg.setColor(new Color(30,30,30));
                        offg.drawstring("Load       "+((float)((int)(hero.load*10.0f+.5f)))/10.0f+" / "+((float)((int)(hero.maxload*10.0f+.5f)))/10.0f,263,324);
                        offg.drawstring("Kg",423,324);
                        if (hero.load>hero.maxload) offg.setColor(Color.red);
                        else if (hero.load>hero.maxload*3/4) offg.setColor(Color.yellow);
                        else offg.setColor(Color.white);
                        offg.drawstring("Load       "+((float)((int)(hero.load*10.0f+.5f)))/10.0f+" / "+((float)((int)(hero.maxload*10.0f+.5f)))/10.0f,260,321);
                        offg.drawstring("Kg",420,321);
                        
                        if (hero.ispoisoned) {
                           offg.setColor(new Color(0,150,0));
                           offg.setStroke(new BasicStroke(2.0f));
                           //offg.drawRect(110,24,35,35);
                           offg.drawRect(106,22,40,37);
                           //draw poisoned pic here
                           offg.drawImage(poisonedpic,224,209,null);
                        }
                        if (hero.silenced) {
                           //offg.drawImage(silencedpic,224,209,null);
                           //offg.setFont(new Font("TimesRoman",Font.BOLD,24));
                           offg.setFont(dungfont.deriveFont(24.0f));
                           offg.setColor(new Color(30,30,30));
                           offg.drawstring("SILENCED",264,284);
                           offg.setColor(Color.red);
                           offg.drawstring("SILENCED",260,280);
                           //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                           offg.setFont(dungfont14);
                        }
                        bool drewred = false;
                        if (hero.strengthboost<0 || hero.dexterityboost<0 || hero.vitalityboost<0 || hero.intelligenceboost<0 || hero.wisdomboost<0 || hero.defenseboost<0 || hero.magicresistboost<0) {
                           offg.setColor(Color.red);
                           offg.setStroke(new BasicStroke(2.0f));
                           //offg.drawRect(22,24,35,35);
                           offg.drawRect(18,22,40,37);
                           drewred = true;
                        }
                        if (hero.detectcurse>0) {
                           //outline eye in blue if detecting curses (purple if also have stat problem)
                           if (drewred) offg.setColor(new Color(150,0,150));
                           else offg.setColor(Color.blue);
                           offg.setStroke(new BasicStroke(2.0f));
                           offg.drawRect(18,22,40,37);
                        }
                        
                        if (AUTOMAP) offg.drawImage(automappic,309,5,null);//offg.drawImage(automappic,181,6,null);
                                        
                        if (hero.weapon==null || skipchestscroll || (hero.weapon.number!=5 && hero.weapon.number!=4)) {
                                int len = (int)(((float)hero.food/1000.0f)*170.0f);
                                offg.setColor(new Color(30,30,30));
                                offg.fillRect(245,150,len,10);
                                if (hero.food<75) offg.setColor(Color.red);
                                else if (hero.food<100) offg.setColor(Color.yellow);
                                else offg.setColor(new Color(200,130,10));
                                offg.fillRect(240,145,len,10);
        
                                len = (int)(((float)hero.water/1000.0f)*170.0f);
                                offg.setColor(new Color(30,30,30));
                                offg.fillRect(245,197,len,10);
                                if (hero.water<75) offg.setColor(Color.red);
                                else if (hero.water<100) offg.setColor(Color.yellow);
                                else offg.setColor(Color.blue);
                                offg.fillRect(240,192,len,10);
                        }
                        else if (!mirror && hero.weapon!=null && hero.weapon.number==5) {
                                //draw chest contents
                                //offg.drawImage(openchest,173,104,null);
                                offg.drawImage(openchest,156,104,null);
                                for (int i=0;i<12;i++) {
                                        if ( ((Chest)hero.weapon).contents[i]!=null) {
                                                if (i<6) offg.drawImage(((Chest)hero.weapon).contents[i].pic,227+i*34,207,this);
                                                else offg.drawImage(((Chest)hero.weapon).contents[i].pic,227+(i-6)*34,239,this);
                                        }
                                }
                        }
                        else if (!mirror && hero.weapon!=null && hero.weapon.number==4) {
                                //draw scroll
                                //offg.drawImage(scrollpic,200,104,null);
                                offg.drawImage(scrollpic,156,104,null);
                                //offg.setFont(new Font("Courier",Font.BOLD,16));
                                offg.setFont(scrollfont);
                                int messlength = hero.weapon.scroll.length;
                                while (hero.weapon.scroll[messlength-1].equals("")) { messlength--; }
                                int startdraw = 188-messlength*10;
                                offg.setColor(new Color(30,30,30));
                                for (int i=0;i<messlength;i++) {
                                        //offg.drawstring(hero.weapon.scroll[i],325-hero.weapon.scroll[i].length()*5,startdraw+i*20);
                                        //offg.drawstring(hero.weapon.scroll[i],320-hero.weapon.scroll[i].length()*5,startdraw+i*20);
                                        offg.drawstring(hero.weapon.scroll[i],320-offg.getFontMetrics().stringWidth(hero.weapon.scroll[i])/2,startdraw+i*20);
                                }
                        }
                        if (mirror && herosheet.showresreinc) {
                                //draw res/reinc buttons
                                offg.drawImage(resreincpic,206,104,this);
                        }

                        offg.setColor(new Color(63,63,63));
                        offg.drawImage(hero.weapon.pic,124,106,this);
                        if (hero.weapon.cursed>0 && hero.weapon.cursefound) curseg.fillRect(124,106,32,32);
                        if (hero.hurtweapon) {
                                if (hero.weapon==fistfoot) offg.drawImage(hurtweapon,124,106,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(123,105,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.head!=null) {
                                offg.fillRect(68,52,32,32);
                                offg.drawImage(hero.head.pic,68,52,this);
                                if (hero.head.cursed>0 && hero.head.cursefound) curseg.fillRect(68,52,32,32);
                        }
                        if (hero.hurthead) {
                                if (hero.head==null) offg.drawImage(hurthead,68,52,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(67,51,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.neck!=null) {
                                offg.fillRect(12,66,32,32);
                                offg.drawImage(hero.neck.pic,12,66,this);
                                if (hero.neck.cursed>0 && hero.neck.cursefound) curseg.fillRect(12,66,32,32);
                        }
                        if (hero.torso!=null) {
                                offg.fillRect(68,92,32,32);
                                offg.drawImage(hero.torso.pic,68,92,this);
                                if (hero.torso.cursed>0 && hero.torso.cursefound) curseg.fillRect(68,92,32,32);
                        }
                        if (hero.hurttorso) {
                                if (hero.torso==null) offg.drawImage(hurttorso,68,92,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(67,91,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.hand!=null) {
                                offg.fillRect(12,106,32,32);
                                offg.drawImage(hero.hand.pic,12,106,this);
                                if (hero.hand.cursed>0 && hero.hand.cursefound) curseg.fillRect(12,106,32,32);
                        }
                        if (hero.hurthand) {
                                if (hero.hand==null) offg.drawImage(hurthand,12,106,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(11,105,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.legs!=null) {
                                offg.fillRect(68,132,32,32);
                                offg.drawImage(hero.legs.pic,68,132,this);
                                if (hero.legs.cursed>0 && hero.legs.cursefound) curseg.fillRect(68,132,32,32);
                        }
                        if (hero.hurtlegs) {
                                if (hero.legs==null) offg.drawImage(hurtlegs,68,132,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(67,131,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.feet!=null) {
                                offg.fillRect(68,172,32,32);
                                offg.drawImage(hero.feet.pic,68,172,this);
                                if (hero.feet.cursed>0 && hero.feet.cursefound) curseg.fillRect(68,172,32,32);
                        }
                        if (hero.hurtfeet) {
                                if (hero.feet==null) offg.drawImage(hurtfeet,68,172,this);
                                offg.setColor(Color.red);
                                offg.setStroke(new BasicStroke(2.0f));
                                offg.drawRect(67,171,34,34);
                                offg.setColor(new Color(63,63,63));
                        }
                        if (hero.pouch1!=null) {
                                //offg.fillRect(12,167,32,32);//was 146
                                offg.drawImage(hero.pouch1.pic,12,167,this);
                                if (hero.pouch1.cursed>0 && hero.pouch1.cursefound) curseg.fillRect(12,167,32,32);
                        }
                        if (hero.pouch2!=null) {
                                offg.drawImage(hero.pouch2.pic,12,201,this);//was 180
                                if (hero.pouch2.cursed>0 && hero.pouch2.cursefound) curseg.fillRect(12,201,32,32);
                        }
                        for (int i=0;i<8;i++) {
                                if (hero.pack[i]!=null) {
                                        offg.drawImage(hero.pack[i].pic,166+(34*i),32,this);
                                        if (hero.pack[i].cursed>0 && hero.pack[i].cursefound) curseg.fillRect(166+(34*i),32,32,32);
                                }
                        }
                        for (int i=0;i<8;i++) {
                                if (hero.pack[i+8]!=null) {
                                        offg.drawImage(hero.pack[i+8].pic,166+(34*i),66,this);
                                        if (hero.pack[i+8].cursed>0 && hero.pack[i+8].cursefound) curseg.fillRect(166+(34*i),66,32,32);
                                }
                        }
                        for (int i=0;i<2;i++) {
                                if (hero.quiver[i]!=null) {
                                        //offg.fillRect(124+(34*i),167,32,32);
                                        offg.drawImage(hero.quiver[i].pic,124+(34*i),167,this);
                                        if (hero.quiver[i].cursed>0 && hero.quiver[i].cursefound) curseg.fillRect(124+(34*i),167,32,32);
                                }
                        }
                        for (int i=0;i<2;i++) {
                                if (hero.quiver[i+2]!=null) {
                                        offg.drawImage(hero.quiver[i+2].pic,124+(34*i),201,this);
                                        if (hero.quiver[i+2].cursed>0 && hero.quiver[i+2].cursefound) curseg.fillRect(124+(34*i),201,32,32);
                                }
                        }
                        for (int i=0;i<2;i++) {
                                if (hero.quiver[i+4]!=null) {
                                        offg.drawImage(hero.quiver[i+4].pic,124+(34*i),235,this);
                                        if (hero.quiver[i+4].cursed>0 && hero.quiver[i+4].cursefound) curseg.fillRect(124+(34*i),235,32,32);
                                }
                        }
                        g.drawImage(offscreen,0,0,this.size().width,this.size().height,null);
                } 
                
                public void showStats() {
                        
                        offg.setColor(new Color(60,60,60));
                        offg.fillRect(206,104,232,190);
                        offg.setColor(new Color(95,95,95));
                        offg.setStroke(new BasicStroke(2.0f));
                        offg.drawRect(207,105,230,190);
                        //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                        offg.setFont(dungfont14);
                        int i=120;
                        if (herosheet.hero.flevel>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring(LEVELNAMES[herosheet.hero.flevel-1]+" Fighter",218,i+3);
                                //offg.setColor(Color.white);
                                if (herosheet.hero.flevelboost>0) offg.setColor(Color.green);
                                else if (herosheet.hero.flevelboost<0) offg.setColor(Color.red);
                                else offg.setColor(Color.white);
                                offg.drawstring(LEVELNAMES[herosheet.hero.flevel-1]+" Fighter",215,i);
                                i+=15;
                        }
                        if (herosheet.hero.nlevel>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring(LEVELNAMES[herosheet.hero.nlevel-1]+" Ninja",218,i+3);
                                //offg.setColor(Color.white);
                                if (herosheet.hero.nlevelboost>0) offg.setColor(Color.green);
                                else if (herosheet.hero.nlevelboost<0) offg.setColor(Color.red);
                                else offg.setColor(Color.white);
                                offg.drawstring(LEVELNAMES[herosheet.hero.nlevel-1]+" Ninja",215,i);
                                i+=15;
                        }
                        if (herosheet.hero.wlevel>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring(LEVELNAMES[herosheet.hero.wlevel-1]+" Wizard",218,i+3);
                                //offg.setColor(Color.white);
                                if (herosheet.hero.wlevelboost>0) offg.setColor(Color.green);
                                else if (herosheet.hero.wlevelboost<0) offg.setColor(Color.red);
                                else offg.setColor(Color.white);
                                offg.drawstring(LEVELNAMES[herosheet.hero.wlevel-1]+" Wizard",215,i);
                                i+=15;
                        }
                        if (herosheet.hero.plevel>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring(LEVELNAMES[herosheet.hero.plevel-1]+" Priest",218,i+3);
                                //offg.setColor(Color.white);
                                if (herosheet.hero.plevelboost>0) offg.setColor(Color.green);
                                else if (herosheet.hero.plevelboost<0) offg.setColor(Color.red);
                                else offg.setColor(Color.white);
                                offg.drawstring(LEVELNAMES[herosheet.hero.plevel-1]+" Priest",215,i);
                        }
                        offg.setColor(new Color(30,30,30));
                        offg.drawstring("Strength",218,200);
                        offg.drawstring("Dexterity",218,214);
                        offg.drawstring("Vitality",218,228);
                        offg.drawstring("Intelligence",218,242);
                        offg.drawstring("Wisdom",218,256);
                        offg.drawstring("Defense",218,275);
                        offg.drawstring("Resist Magic",218,290);
                        int xpos = 218+offg.getFontMetrics().stringWidth("Resist Magic")+13;
                        offg.drawstring(""+herosheet.hero.strength,xpos,200);
                        offg.drawstring(""+herosheet.hero.dexterity,xpos,214);
                        offg.drawstring(""+herosheet.hero.vitality,xpos,228);
                        offg.drawstring(""+herosheet.hero.intelligence,xpos,242);
                        offg.drawstring(""+herosheet.hero.wisdom,xpos,256);
                        offg.drawstring(""+herosheet.hero.defense,xpos,275);
                        offg.drawstring(""+herosheet.hero.magicresist,xpos,290);
                        offg.setColor(Color.white);
                        offg.drawstring("Strength",215,197);
                        offg.drawstring("Dexterity",215,211);
                        offg.drawstring("Vitality",215,225);
                        offg.drawstring("Intelligence",215,239);
                        offg.drawstring("Wisdom",215,253);
                        offg.drawstring("Defense",215,272);
                        offg.drawstring("Resist Magic",215,287);
                        xpos-=3;
                        if (herosheet.hero.strengthboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.strengthboost<0) offg.setColor(Color.red);
                        offg.drawstring(""+herosheet.hero.strength,xpos,197);
                        if (herosheet.hero.dexterityboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.dexterityboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawstring(""+herosheet.hero.dexterity,xpos,211);
                        if (herosheet.hero.vitalityboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.vitalityboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawstring(""+herosheet.hero.vitality,xpos,225);
                        if (herosheet.hero.intelligenceboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.intelligenceboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawstring(""+herosheet.hero.intelligence,xpos,239);
                        if (herosheet.hero.wisdomboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.wisdomboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawstring(""+herosheet.hero.wisdom,xpos,253);
                        if (herosheet.hero.defenseboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.defenseboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawstring(""+herosheet.hero.defense,xpos,272);
                        if (herosheet.hero.magicresistboost>0) offg.setColor(Color.green);
                        else if (herosheet.hero.magicresistboost<0) offg.setColor(Color.red);
                        else offg.setColor(Color.white);
                        offg.drawstring(""+herosheet.hero.magicresist,xpos,287);
                }

                public void showItem() {
                        if (inhand.number==4) {
                                //draw scroll
                                //offg.drawImage(scrollpic,200,104,null);
                                offg.drawImage(scrollpic,156,104,null);
                                //offg.setFont(new Font("Courier",Font.BOLD,16));
                                offg.setFont(scrollfont);
                                int messlength = inhand.scroll.length;
                                while (inhand.scroll[messlength-1].equals("")) { messlength--; }
                                int startdraw = 188-messlength*10;
                                offg.setColor(new Color(30,30,30));
                                for (int i=0;i<messlength;i++) {
                                        //offg.drawstring(inhand.scroll[i],325-inhand.scroll[i].length()*5,startdraw+i*20);
                                        //offg.drawstring(inhand.scroll[i],320-inhand.scroll[i].length()*5,startdraw+i*20);
                                        offg.drawstring(inhand.scroll[i],320-offg.getFontMetrics().stringWidth(inhand.scroll[i])/2,startdraw+i*20);
                                }
                        }
                        else if (inhand.number==5) {
                                //draw chest contents
                                //offg.drawImage(openchest,173,104,null);
                                offg.drawImage(openchest,156,104,null);
                                for (int i=0;i<12;i++) {
                                        if ( ((Chest)inhand).contents[i]!=null) {
                                                if (i<6) offg.drawImage(((Chest)inhand).contents[i].pic,227+i*34,207,this);
                                                else offg.drawImage(((Chest)inhand).contents[i].pic,227+(i-6)*34,239,this);
                                        }
                                }
                                //for (int i=0;i<6;i++) {
                                //        if ( ((Chest)inhand).contents[i]!=null) {
                                //                offg.drawImage(((Chest)inhand).contents[i].pic,227+i*34,207,this);
                                //        }
                                //}
                        }
                        else {
                        offg.setColor(new Color(60,60,60));
                        offg.fillRect(208,106,228,142);
                        offg.setColor(new Color(95,95,95));
                        //offg.setStroke(new BasicStroke(2.0f));
                        //offg.drawRect(207,105,230,144);
                        offg.setRenderingHint(RenderingHints.KEY_ANTIALIASING,RenderingHints.VALUE_ANTIALIAS_ON);
                        offg.drawOval(231,111,50,50);
                        offg.setRenderingHint(RenderingHints.KEY_ANTIALIASING,RenderingHints.VALUE_ANTIALIAS_OFF);
                        
                        offg.drawImage(inhand.pic,240,120,null);
                        
                        //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                        offg.setFont(dungfont14);
                        offg.setColor(new Color(30,30,30));
                        offg.drawstring(inhand.name,302,142);
                        offg.drawstring(((float)((int)(inhand.weight*10.0f+.5f)))/10.0f+" kg",302,158);
                        offg.setColor(Color.white);
                        offg.drawstring(inhand.name,300,140);
                        offg.drawstring(((float)((int)(inhand.weight*10.0f+.5f)))/10.0f+" kg",300,156);
                        
                        if (inhand.number==9) {
                                //torch
                                if (((Torch)inhand).lightboost>36) return;
                                string burnout;
                                if (((Torch)inhand).lightboost==0) burnout = "(Burnt Out)";
                                else burnout = "(Almost Out)";
                                //offg.setFont(new Font("TimesRoman",Font.BOLD,12));
                                offg.setFont(dungfont);
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring(burnout,282,202);
                                offg.setColor(Color.white);
                                offg.drawstring(burnout,280,200);
                                return;
                        }
                        else if (inhand.number==215) {
                                //stormbringer
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring("Stealer of Souls",281,206);
                                offg.setColor(new Color(120,120,120));
                                offg.drawstring("Stealer of Souls",280,205);
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring("Slayer of Friends",281,224);
                                offg.setColor(new Color(120,120,120));
                                offg.drawstring("Slayer of Friends",280,223);
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring("Destroyer of Balance",281,247);
                                offg.setColor(new Color(120,120,120));
                                offg.drawstring("Destroyer of Balance",280,246);
                                return;
                        }
                        
                        if (inhand.defense>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring("Defense: "+inhand.defense,301,173);
                                offg.setColor(new Color(250,100,100));
                                offg.drawstring("Defense: "+inhand.defense,300,172);
                        }
                        if (inhand.magicresist>0) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring("Resist Magic: "+inhand.magicresist,301,186);
                                offg.setColor(new Color(250,100,100));
                                offg.drawstring("Resist Magic: "+inhand.magicresist,300,185);
                        }
                        int yalign = 0;
                        for (int i=0;i<inhand.functions;i++) {
                                if (inhand.charges[i]>0) {
                                        if ( (herosheet.hero.wlevel>4 && inhand.function[i][1].equals("w")) || (herosheet.hero.plevel>4 && inhand.function[i][1].equals("p")) ) {
                                                offg.setColor(new Color(30,30,30));
                                                string fs = inhand.function[i][0];
                                                if (fs.endsWith(" Party")) fs = fs.substring(0,fs.indexOf(" Party"));
                                                offg.drawstring(fs+" charges: "+inhand.charges[i],251,206+yalign*14);
                                                offg.setColor(new Color(120,120,255));
                                                offg.drawstring(fs+" charges: "+inhand.charges[i],250,205+yalign*14);
                                                yalign++;
                                        }
                                }
                        }
                        if ( (herosheet.hero.wlevel>3 && inhand.isbomb) || (herosheet.hero.plevel>3 && inhand.ispotion && !inhand.isbomb) ) {
                                offg.setColor(Color.white);
                                offg.drawstring("(",242,178);
                                offg.drawstring(")",267,178);
                                offg.drawImage(spellsheet.spellsymbol[inhand.potioncastpow-1],251,168,herosheet);
                        }
                        else if (inhand.number==73) {
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring("Drinks left: "+((Waterskin)inhand).drinks,271,247);
                                offg.setColor(new Color(100,100,255));
                                offg.drawstring("Drinks left: "+((Waterskin)inhand).drinks,270,246);
                        }
                        else if (inhand.cursed>0 && (inhand.cursefound || herosheet.hero.detectcurse>0 || herosheet.hero.plevel*8>inhand.cursed)) {
                                offg.setColor(Color.white);
                                offg.drawstring("(Cursed)",227,178);
                                inhand.cursefound = true;
                        }
                        else if (inhand.type==Item.FOOD) {
                                string foodstring = "(Edible";
                                if (herosheet.hero.plevel>7) {
                                        if (inhand.poisonous>0) foodstring+=" - Poisoned!)";
                                        else if (inhand.foodvalue<0) foodstring+=" - Bad!)";
                                        else foodstring+=")";
                                }
                                else foodstring+=")";
                                offg.setColor(new Color(30,30,30));
                                offg.drawstring(foodstring,271,245);
                                offg.setColor(new Color(100,100,255));
                                offg.drawstring(foodstring,270,244);
                        }
                        }
                }
                
                public void sleepsheet() {
                        offg.setColor(Color.black);
                        //offg.fillRect(0,0,herosheet.getSize().width,herosheet.getSize().height);
                        offg.fillRect(0,0,herosheet.getWidth(),herosheet.getHeight());
                        //offg.setFont(new Font("TimesRoman",Font.BOLD,14));
                        offg.setFont(dungfont14);
                        offg.setColor(new Color(70,70,70));
                        offg.drawstring("Sleeping. Click mouse to wake up.",143,143);
                        offg.setColor(Color.white);
                        offg.drawstring("Sleeping. Click mouse to wake up.",140,140);
                }
        }

        class SheetClick extends MouseAdapter {
                int x,y;
                
                public void mousePressed(MouseEvent e) {
                        x = e.getX();
                        y = e.getY();
                     x /= DungeonViewScale;
                     y /= DungeonViewScale;
                        if (sleeping) {
                                needredraw=true;
                                sleeping = false;
                                sleeper = 0;
                                spellsheet.setVisible(true);
                                weaponsheet.setVisible(true);
                                arrowsheet.setVisible(true);

                                if (herosheet.hero.isdead) herosheet.setHero(hero[leader]);
                                else herosheet.repaint();
                        }
                        else if (x>420 && x<440 && y<25 && y>5 || (e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) {//SwingUtilities.isRightMouseButton(e)) {
                                sheet=false;
                                showCenter(dview);
                                hupdate();
                             return;
                        }
                        //add load test to each after xy tests
                        //weapon hand
                        else if (x>124 && x<156 && y<136 && y>104) {
                                if (herosheet.hero.weapon.number==215) {
                                        message.setMessage("Stormbringer resists...",4);
                                        return;
                                }
                                else if (herosheet.hero.weapon.number==219 || herosheet.hero.weapon.number==261) {
                                        return;
                                }
                                else if (herosheet.hero.weapon.cursed>0 && (herosheet.hero.weapon.type==Item.WEAPON || herosheet.hero.weapon.type==Item.SHIELD)) {
                                        message.setMessage(herosheet.hero.weapon.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.weapon.cursefound) {
                                                herosheet.hero.weapon.cursefound = true;
                                                herosheet.hero.repaint();
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (iteminhand) { herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (herosheet.hero.weapon!=null) { herosheet.hero.load-=herosheet.hero.weapon.weight; hero[leader].load+=herosheet.hero.weapon.weight; }
                                if (!iteminhand && !herosheet.hero.weapon.name.equals("Fist/Foot")) {
                                        iteminhand=true;
                                        inhand=herosheet.hero.weapon;
                                        herosheet.hero.weapon=fistfoot;
                                        if (inhand.number==9) {
                                                ((Torch)inhand).putOut();
                                                updateDark();
                                        }
                                        if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                        herosheet.hero.repaint();
                                        weaponsheet.repaint();
                                }
                                else if (iteminhand) {
                                        if (!herosheet.hero.weapon.name.equals("Fist/Foot")) {
                                                Item tempitem = herosheet.hero.weapon;
                                                herosheet.hero.weapon=inhand;
                                                inhand=tempitem;
                                                if (inhand.number==9) {
                                                        ((Torch)inhand).putOut();
                                                        updateDark();
                                                }
                                                if (herosheet.hero.weapon.number==9) {
                                                        ((Torch)herosheet.hero.weapon).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) inhand.unEquipEffect(herosheet.hero);
                                                if (herosheet.hero.weapon.type==Item.WEAPON || herosheet.hero.weapon.type==Item.SHIELD || herosheet.hero.weapon.number==4) herosheet.hero.weapon.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.weapon=inhand;
                                                iteminhand=false;
                                                if (herosheet.hero.weapon.number==9) {
                                                        ((Torch)herosheet.hero.weapon).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.WEAPON || inhand.type==Item.SHIELD || inhand.number==4) herosheet.hero.weapon.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                        herosheet.hero.repaint();
                                        weaponsheet.repaint();
                                }
                        }
                        //action hand
                        else if (x>12 && x<44 && y<136 && y>104) {
                                if (herosheet.hero.hand!=null && herosheet.hero.hand.cursed>0 && herosheet.hero.hand.type==Item.SHIELD) {
                                        message.setMessage(herosheet.hero.hand.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.hand.cursefound) {
                                                herosheet.hero.hand.cursefound = true;
                                                herosheet.hero.repaint();
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (iteminhand) { herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (herosheet.hero.hand!=null) { herosheet.hero.load-=herosheet.hero.hand.weight; hero[leader].load+=herosheet.hero.hand.weight; }
                                if (!iteminhand && herosheet.hero.hand!=null) {
                                        iteminhand=true;
                                        inhand=herosheet.hero.hand;
                                        herosheet.hero.hand=null;
                                        if (inhand.number==9) {
                                                ((Torch)inhand).putOut();
                                                updateDark();
                                        }
                                        if (inhand.type==Item.SHIELD) inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                        herosheet.hero.repaint();
                                }
                                else if (iteminhand) {
                                        if (herosheet.hero.hand!=null) {
                                                Item tempitem = herosheet.hero.hand;
                                                herosheet.hero.hand=inhand;
                                                inhand=tempitem;
                                                if (inhand.number==9) {
                                                        ((Torch)inhand).putOut();
                                                        updateDark();
                                                }
                                                if (herosheet.hero.hand.number==9) {
                                                        ((Torch)herosheet.hero.hand).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.SHIELD) inhand.unEquipEffect(herosheet.hero);
                                                if (herosheet.hero.hand.type==Item.SHIELD) herosheet.hero.hand.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.hand=inhand;
                                                iteminhand=false;
                                                if (herosheet.hero.hand.number==9) {
                                                        ((Torch)herosheet.hero.hand).setPic();
                                                        updateDark();
                                                }
                                                if (inhand.type==Item.SHIELD) herosheet.hero.hand.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                        herosheet.hero.repaint();
                                }
                        }
                        //head
                        else if (x>68 && x<100 && y<84 && y>52) {
                                if (herosheet.hero.head!=null && herosheet.hero.head.cursed>0 && herosheet.hero.head.type==Item.HEAD) {
                                        message.setMessage(herosheet.hero.head.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.head.cursefound) {
                                                herosheet.hero.head.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.head!=null) {
                                        herosheet.hero.load-=herosheet.hero.head.weight;
                                        hero[leader].load+=herosheet.hero.head.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.head;
                                        herosheet.hero.head=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.HEAD) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.head!=null) {
                                                herosheet.hero.load-=herosheet.hero.head.weight;
                                                hero[leader].load+=herosheet.hero.head.weight;
                                                Item tempitem = herosheet.hero.head;
                                                herosheet.hero.head=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.head.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.head=inhand;
                                                iteminhand=false;
                                                herosheet.hero.head.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //neck
                        else if (x>12 && x<44 && y<98 && y>66) {
                                if (herosheet.hero.neck!=null && herosheet.hero.neck.cursed>0 && herosheet.hero.neck.type==Item.NECK) {
                                        message.setMessage(herosheet.hero.neck.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.neck.cursefound) {
                                                herosheet.hero.neck.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.neck!=null) {
                                        herosheet.hero.load-=herosheet.hero.neck.weight;
                                        hero[leader].load+=herosheet.hero.neck.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.neck;
                                        herosheet.hero.neck=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        if (inhand.number==89) { numillumlets--; updateDark(); }
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.NECK) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.neck!=null) {
                                                herosheet.hero.load-=herosheet.hero.neck.weight;
                                                hero[leader].load+=herosheet.hero.neck.weight;
                                                Item tempitem = herosheet.hero.neck;
                                                herosheet.hero.neck=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.neck.equipEffect(herosheet.hero);
                                                if (inhand.number==89) { numillumlets--; updateDark(); }
                                                else if (herosheet.hero.neck.number==89) {
                                                        numillumlets++;
                                                        updateDark();
                                                }
                                        }
                                        else {
                                                herosheet.hero.neck=inhand;
                                                iteminhand=false;
                                                herosheet.hero.neck.equipEffect(herosheet.hero);
                                                if (inhand.number==89) {
                                                        numillumlets++;
                                                        updateDark();
                                                }
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //torso
                        else if (x>68 && x<100 && y<124 && y>92) {
                                if (herosheet.hero.torso!=null && herosheet.hero.torso.cursed>0 && herosheet.hero.torso.type==Item.TORSO) {
                                        message.setMessage(herosheet.hero.torso.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.torso.cursefound) {
                                                herosheet.hero.torso.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.torso!=null) {
                                        herosheet.hero.load-=herosheet.hero.torso.weight;
                                        hero[leader].load+=herosheet.hero.torso.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.torso;
                                        herosheet.hero.torso=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.TORSO) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.torso!=null) {
                                                herosheet.hero.load-=herosheet.hero.torso.weight;
                                                hero[leader].load+=herosheet.hero.torso.weight;
                                                Item tempitem = herosheet.hero.torso;
                                                herosheet.hero.torso=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.torso.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.torso=inhand;
                                                iteminhand=false;
                                                herosheet.hero.torso.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //legs
                        else if (x>68 && x<100 && y<164 && y>132) {
                                if (herosheet.hero.legs!=null && herosheet.hero.legs.cursed>0 && herosheet.hero.legs.type==Item.LEGS) {
                                        message.setMessage(herosheet.hero.legs.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.legs.cursefound) {
                                                herosheet.hero.legs.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.legs!=null) {
                                        herosheet.hero.load-=herosheet.hero.legs.weight;
                                        hero[leader].load+=herosheet.hero.legs.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.legs;
                                        herosheet.hero.legs=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.LEGS) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.legs!=null) {
                                                herosheet.hero.load-=herosheet.hero.legs.weight;
                                                hero[leader].load+=herosheet.hero.legs.weight;
                                                Item tempitem = herosheet.hero.legs;
                                                herosheet.hero.legs=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.legs.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.legs=inhand;
                                                iteminhand=false;
                                                herosheet.hero.legs.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //feet
                        else if (x>68 && x<100 && y<204 && y>172) {
                                if (herosheet.hero.feet!=null && herosheet.hero.feet.cursed>0 && herosheet.hero.feet.type==Item.FEET) {
                                        message.setMessage(herosheet.hero.feet.name+" is cursed and cannot be removed!",4);
                                        if (!herosheet.hero.feet.cursefound) {
                                                herosheet.hero.feet.cursefound = true;
                                                herosheet.repaint();
                                        }
                                        return;
                                }
                                if (!iteminhand && herosheet.hero.feet!=null) {
                                        herosheet.hero.load-=herosheet.hero.feet.weight;
                                        hero[leader].load+=herosheet.hero.feet.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.feet;
                                        herosheet.hero.feet=null;
                                        inhand.unEquipEffect(herosheet.hero);
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.type==Item.FEET) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.feet!=null) {
                                                herosheet.hero.load-=herosheet.hero.feet.weight;
                                                hero[leader].load+=herosheet.hero.feet.weight;
                                                Item tempitem = herosheet.hero.feet;
                                                herosheet.hero.feet=inhand;
                                                inhand=tempitem;
                                                inhand.unEquipEffect(herosheet.hero);
                                                herosheet.hero.feet.equipEffect(herosheet.hero);
                                        }
                                        else {
                                                herosheet.hero.feet=inhand;
                                                iteminhand=false;
                                                herosheet.hero.feet.equipEffect(herosheet.hero);
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //pouch1
                        else if (x>12 && x<44 && y<199 && y>167) {
                                if (!iteminhand && herosheet.hero.pouch1!=null) {
                                        herosheet.hero.load-=herosheet.hero.pouch1.weight;
                                        hero[leader].load+=herosheet.hero.pouch1.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.pouch1;
                                        herosheet.hero.pouch1=null;
                                        herosheet.repaint();
                                }
                                //else if (iteminhand && (inhand.size<1 || (inhand.number>87 && inhand.number<101))) {
                                else if (iteminhand && inhand.size<2) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.pouch1!=null) {
                                                herosheet.hero.load-=herosheet.hero.pouch1.weight;
                                                hero[leader].load+=herosheet.hero.pouch1.weight;
                                                Item tempitem = herosheet.hero.pouch1;
                                                herosheet.hero.pouch1=inhand;
                                                inhand=tempitem;
                                        }
                                        else {
                                                herosheet.hero.pouch1=inhand;
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //pouch2
                        else if (x>12 && x<44 && y<233 && y>201) {
                                if (!iteminhand && herosheet.hero.pouch2!=null) {                                                                                    
                                        herosheet.hero.load-=herosheet.hero.pouch2.weight;
                                        hero[leader].load+=herosheet.hero.pouch2.weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.pouch2;
                                        herosheet.hero.pouch2=null;
                                        herosheet.repaint();
                                }
                                //else if (iteminhand && (inhand.size<1 || (inhand.number>87 && inhand.number<101))) {
                                else if (iteminhand && inhand.size<2) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.pouch2!=null) {
                                                herosheet.hero.load-=herosheet.hero.pouch2.weight;
                                                hero[leader].load+=herosheet.hero.pouch2.weight;
                                                Item tempitem = herosheet.hero.pouch2;
                                                herosheet.hero.pouch2=inhand;
                                                inhand=tempitem;
                                        }
                                        else {
                                                herosheet.hero.pouch2=inhand;
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //pack
                        else if (x>166 && x<436 && y<98 && y>32) {
                                int i=0;
                                if (y>65) i=8;
                                if (iteminhand) { herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                if (herosheet.hero.pack[(x-166)/34+i]!=null) { herosheet.hero.load-=herosheet.hero.pack[(x-166)/34+i].weight; hero[leader].load+=herosheet.hero.pack[(x-166)/34+i].weight; }
                                if (!iteminhand && herosheet.hero.pack[(x-166)/34+i]!=null) {
                                        iteminhand=true;
                                        inhand=herosheet.hero.pack[(x-166)/34+i];
                                        herosheet.hero.pack[(x-166)/34+i]=null;
                                        herosheet.repaint();
                                }
                                else if (iteminhand) {
                                        if (herosheet.hero.pack[(x-166)/34+i]!=null) {
                                                Item tempitem = herosheet.hero.pack[(x-166)/34+i];
                                                herosheet.hero.pack[(x-166)/34+i]=inhand;
                                                inhand=tempitem;
                                        }
                                        else {
                                                herosheet.hero.pack[(x-166)/34+i]=inhand;
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }

                        }
                        //quiver
                        else if (x>124 && x<190 && y<267 && y>167) {
                                int i=0;
                                if (y>234) i=4;
                                else if (y>200) i=2;
                                if (!iteminhand && herosheet.hero.quiver[(x-124)/34+i]!=null) {
                                        herosheet.hero.load-=herosheet.hero.quiver[(x-124)/34+i].weight;
                                        hero[leader].load+=herosheet.hero.quiver[(x-124)/34+i].weight;
                                        iteminhand=true;
                                        inhand=herosheet.hero.quiver[(x-124)/34+i];
                                        herosheet.hero.quiver[(x-124)/34+i]=null;
                                        herosheet.repaint();
                                }
                                //else if (iteminhand && inhand.type==Item.WEAPON && inhand.number!=9) {
                                //else if (iteminhand && inhand.type==Item.WEAPON && (inhand.size<2 || (inhand.number>225 && inhand.number<230) || (inhand.number>235 && inhand.number<256) || inhand.number==9) && (inhand.size<1 || (x-124)/34+i==0)) {
                                else if (iteminhand && inhand.type==Item.WEAPON && ((inhand.projtype>0 && inhand.size<2) || (inhand.size<4 && (x-124)/34+i==0))) {
                                        herosheet.hero.load+=inhand.weight;
                                        hero[leader].load-=inhand.weight;
                                        if (herosheet.hero.quiver[(x-124)/34+i]!=null) {
                                                herosheet.hero.load-=herosheet.hero.quiver[(x-124)/34+i].weight;
                                                hero[leader].load+=herosheet.hero.quiver[(x-124)/34+i].weight;
                                                Item tempitem = herosheet.hero.quiver[(x-124)/34+i];
                                                herosheet.hero.quiver[(x-124)/34+i]=inhand;
                                                inhand=tempitem;
                                        }
                                        else {
                                                herosheet.hero.quiver[(x-124)/34+i]=inhand;
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //chest
                        //else if (x>227 && x<431 && y>206 && y<239 && herosheet.hero.weapon!=null && herosheet.hero.weapon.number==5) {
                        else if (x>227 && x<431 && y>206 && y<271 && herosheet.hero.weapon!=null && herosheet.hero.weapon.number==5) {
                                //if (iteminhand) { herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight; }
                                //if (((Chest)herosheet.hero.weapon).contents[(x-227)/34]!=null) { herosheet.hero.load-=((Chest)herosheet.hero.weapon).contents[(x-227)/34].weight; hero[leader].load+=((Chest)herosheet.hero.weapon).contents[(x-227)/34].weight; }
                                int ymod = 0;if (y>238) ymod = 6;
                                if (!iteminhand && ((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod]!=null) {
                                        herosheet.hero.load-=((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod].weight; hero[leader].load+=((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod].weight;
                                        iteminhand=true;
                                        inhand=((Chest)herosheet.hero.weapon).getItem((x-227)/34+ymod);
                                        //((Chest)herosheet.hero.weapon).contents[(x-227)/34]=null;
                                        herosheet.repaint();
                                }
                                else if (iteminhand && inhand.number!=5) { //magically holds big things too, except other chests
                                //else if (iteminhand) { //magically holds big things too
                                //else if (iteminhand && inhand.size<3) {
                                //else if (iteminhand && inhand.size<3 && (inhand.number<200 || inhand.number>235)) {
                                        herosheet.hero.load+=inhand.weight; hero[leader].load-=inhand.weight;
                                        if (((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod]!=null) {
                                                herosheet.hero.load-=((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod].weight; hero[leader].load+=((Chest)herosheet.hero.weapon).contents[(x-227)/34+ymod].weight;
                                                Item tempitem = ((Chest)herosheet.hero.weapon).getItem((x-227)/34+ymod);
                                                ((Chest)herosheet.hero.weapon).putItem((x-227)/34+ymod,inhand);
                                                inhand=tempitem;
                                        }
                                        else {
                                                ((Chest)herosheet.hero.weapon).putItem((x-227)/34+ymod,inhand);
                                                iteminhand=false;
                                        }
                                        herosheet.repaint();
                                }
                        }
                        //eye (was below in sep method)
                        else if (x>20 && x<57 && y<58 && y>24) {
                                herosheet.skipchestscroll = true;
                                /*herosheet.paint(herosheet.offg);*/
                                viewing=true;
                                if (!iteminhand) herosheet.stats=true;
                                else if (inhand.number==4) {
                                        inhand.temppic = inhand.pic;
                                        inhand.pic = inhand.epic;
                                        changeCursor();
                                }
                                herosheet.repaint();
                             return;
                        }
                        //mouth
                        else if (x>109 && x<146 && y<58 && y>24) {
                                if (iteminhand && (inhand.type==Item.FOOD || inhand.ispotion || inhand.number==72 || inhand.number==73)) {
                                        herosheet.hero.eatdrink();
                                        herosheet.hero.repaint();
                                        herosheet.repaint();
                                }
                        }
                        //map
                        //else if (AUTOMAP && x>180 && x<208 && y<24 && y>5) {
                        else if (AUTOMAP && x>308 && x<336 && y<24 && y>5) {
                                toppanel.setVisible(false);
                                centerpanel.setVisible(false);
                                mappane.setVisible(true);
                                validate();
                             return;
                        }
                        //sleep
                        else if (x>260 && x<295 && y<24 && y>5) {
                                sleeping = true;
                                sleeper = 50;
                                spellsheet.setVisible(false);
                                weaponsheet.setVisible(false);
                                arrowsheet.setVisible(false);
                                herosheet.repaint();
                             return;
                        }
                        //options
                        else if (x>347 && x<366 && y<24 && y>5) {
                                if (actionqueue.isEmpty()) actionqueue.add("o");
                             return;
                        }
                        changeCursor();
                }

                public void mouseReleased(MouseEvent e) {
                        if (viewing) {
                                herosheet.skipchestscroll = false;
                                herosheet.stats=false;
                                viewing = false;
                                if (iteminhand && inhand.number==4) {
                                        inhand.pic = inhand.temppic;
                                        changeCursor();
                                }
                                /*herosheet.paint(herosheet.getGraphics());*/
                             herosheet.repaint();
                        }
                }
                
        }

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
        
        /*
        class SpellSymbolClick implements ActionListener {

                public void actionPerformed(ActionEvent e) {
                        if (hero[spellready].currentspell.length()==4) return;
                        int i = Integer.parseInt(e.getActionCommand());
                        //check if enough mana, then reduce it
                        int mananeed = i+1;
                        if (hero[spellready].currentspell.length()>0) mananeed=SYMBOLCOST[i+6*(hero[spellready].currentspell.length()-1)][Integer.parseInt(hero[spellready].currentspell.substring(0,1))-1];
                        if (hero[spellready].mana>=mananeed) {
                                hero[spellready].mana-=mananeed;
                                hero[spellready].currentspell+=(""+(i+1));
                                hero[spellready].repaint();
                                if (sheet && herosheet.hero.equals(hero[spellready])) herosheet.repaint();
                        }
                }
        }
        
        class SpellCasterClick implements ActionListener {

                public void actionPerformed(ActionEvent e) {
                        //shrink down old selected button
                        spellsheet.casterButton[spellready].setText("");
                        spellsheet.casterButton[spellready].setPreferredSize(spellsheet.unselecteddim);
                        spellsheet.casterButton[spellready].setMaximumSize(spellsheet.unselecteddim);

                        //change selected index
                        spellready = Integer.parseInt(e.getActionCommand());

                        //expand new selected button
                        spellsheet.casterButton[spellready].setPreferredSize(spellsheet.selecteddim);
                        spellsheet.casterButton[spellready].setMaximumSize(spellsheet.selecteddim);
                        spellsheet.casterButton[spellready].setText(hero[spellready].name);

                        spellsheet.update();
                }
        }

        class SpellClick implements ActionListener {

                public void actionPerformed(ActionEvent e) {
                        if (e.getActionCommand().equals("undo")) {
                                //undo last symbol
                                if (hero[spellready].currentspell.length()>0) {
                                        hero[spellready].currentspell = hero[spellready].currentspell.substring(0,hero[spellready].currentspell.length()-1);
                                        //if (hero[spellready].currentspell.length()>0) {
                                           //spellsheet.castsymbs.flush();
                                           //spellsheet.castsymbs = new BufferedImage(70,12,BufferedImage.TYPE_INT_ARGB);
                                           //spellsheet.castg = spellsheet.castsymbs.createGraphics();
                                        //}
                                        spellsheet.update();
                                }
                        }
                        else {
                                //cast spell
                                int tester = 0;
                                if (hero[spellready].currentspell.length()>1) tester = hero[spellready].castSpell();
                                else return;
                                if (tester==0) {
                                        //nonsense
                                        message.setMessage(hero[spellready].name+" mumbles nonsense.",4);
                                        hero[spellready].currentspell="";
                                }
                                else if (tester==1) {
                                        //success
                                        message.setMessage(hero[spellready].name+" casts a spell.",spellready);
                                        hero[spellready].currentspell="";
                                        if (sheet && herosheet.hero.equals(hero[spellready])) herosheet.repaint();
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
                                spellsheet.update();
                                weaponsheet.update();
                        }
                }
        }
        */

        /*
        class WeaponClick extends MouseAdapter implements ActionListener {
                public void actionPerformed(ActionEvent e) {
                        //ready weapon
                        weaponready = Integer.parseInt(e.getActionCommand());
                        weaponsheet.update();
                }
                
                public void mousePressed(MouseEvent e) {
                        if (SwingUtilities.isRightMouseButton(e)) {
                                if (e.getSource()==weaponsheet.weaponButton[0]) weaponsheet.toggleSpecials(0);
                                else if (e.getSource()==weaponsheet.weaponButton[1] && numheroes>1) weaponsheet.toggleSpecials(1);
                                else if (e.getSource()==weaponsheet.weaponButton[2] && numheroes>2) weaponsheet.toggleSpecials(2);
                                else if (e.getSource()==weaponsheet.weaponButton[3] && numheroes>3) weaponsheet.toggleSpecials(3);
                                else weaponsheet.toggleSpecials(weaponready);
                        }
                }
        }

        class WeaponFunctClick implements ActionListener {
                public void actionPerformed(ActionEvent e) {
                        //use weapon
                        if (!hero[weaponready].isdead && hero[weaponready].weapon.type==Item.WEAPON && hero[weaponready].weapon.functions>Integer.parseInt(e.getActionCommand()))
                                weaponqueue.add(weaponready+e.getActionCommand()+hero[weaponready].weapon.number);
                        hero[weaponready].wepready = false;
                        hero[weaponready].weaponcount = 5;
                        weaponsheet.update();
                }
        }
        */

        class DungMove extends KeyAdapter {

                public void keyPressed(KeyEvent e) {
                    //freeze game
                    if (!viewing && !eventpanel.isVisible() && e.getKeyCode()==KeyEvent.VK_ESCAPE) {
                        gamefrozen = !gamefrozen;
                        if (gamefrozen) {
                                nomovement = true;
                                showCenter(freezelabelpan);
                                if (numheroes>0) {
                                        for (int i=0;i<numheroes;i++) hero[i].removeMouseListener(hclick);
                                        spellsheet.setVisible(false);
                                        weaponsheet.setVisible(false);
                                }
                                arrowsheet.setVisible(false);
                                //System.gc();
                        }
                        else {
                                nomovement = false;
                                if (sheet) {
                                        showCenter(herosheet);
                                }
                                else {
                                        showCenter(dview);
                                }
                                if (numheroes>0) {
                                        if (!sleeping) { spellsheet.setVisible(true); weaponsheet.setVisible(true); }
                                        for (int i=0;i<numheroes;i++) if (!hero[i].isdead) hero[i].addMouseListener(hclick);
                                }
                                if (!sleeping) arrowsheet.setVisible(true);
                        }
                        return;
                    }
                    if (nomovement) return;
                    //herosheets for dif heroes, F1 thru F4 keys
                    if (!viewing) {
                        if (numheroes>0 && e.getKeyCode()==KeyEvent.VK_F1) {
                                MouseEvent me = new MouseEvent(hero[0],MouseEvent.MOUSE_PRESSED,0,0,20,20,1,false);
                                hero[0].dispatchEvent(me);
                        }
                        else if (numheroes>1 && e.getKeyCode()==KeyEvent.VK_F2) {
                                MouseEvent me = new MouseEvent(hero[1],MouseEvent.MOUSE_PRESSED,0,0,20,20,1,false);
                                hero[1].dispatchEvent(me);
                        }
                        else if (numheroes>2 && e.getKeyCode()==KeyEvent.VK_F3) {
                                MouseEvent me = new MouseEvent(hero[2],MouseEvent.MOUSE_PRESSED,0,0,20,20,1,false);
                                hero[2].dispatchEvent(me);
                        }
                        else if (numheroes>3 && e.getKeyCode()==KeyEvent.VK_F4) {
                                MouseEvent me = new MouseEvent(hero[3],MouseEvent.MOUSE_PRESSED,0,0,20,20,1,false);
                                hero[3].dispatchEvent(me);
                        }
                        else if (numheroes>0 && e.getKeyCode()==KeyEvent.VK_F5) {
                                //save game
                                if (actionqueue.isEmpty()) actionqueue.add("s");
                        }
                        else if (e.getKeyCode()==KeyEvent.VK_F7) {
                                //load game
                                if (actionqueue.isEmpty()) actionqueue.add("l");
                        }
                        else if (e.getKeyCode()==KeyEvent.VK_F9) {
                                //options
                                if (actionqueue.isEmpty()) actionqueue.add("o");
                        }
                        //map
                        else if (AUTOMAP && Character.toLowerCase(e.getKeyChar())=='m') {
                                if (!mappane.isVisible()) {
                                        toppanel.setVisible(false);
                                        centerpanel.setVisible(false);
                                        mappane.setVisible(true);
                                }
                                else {
                                        mappane.setVisible(false);
                                        toppanel.setVisible(true);
                                        centerpanel.setVisible(true);
                                }
                                validate();
                        }
                        //restore characters
                        /*
                        else if (e.getKeyChar()=='*') {
                                for (int i=0;i<numheroes;i++) {
                                        if (!hero[i].isdead) {
                                                if (hero[i].strengthboost<0) {
                                                        hero[i].strength-=hero[i].strengthboost;
                                                        hero[i].strengthboost = 0;
                                                }
                                                hero[i].mana = hero[i].maxmana;
                                                hero[i].heal(hero[i].maxhealth);
                                                hero[i].vitalize(hero[i].maxstamina);
                                                hero[i].weaponcount=0;
                                                hero[i].hitcounter=0;
                                                hero[i].wepready=true;
                                                hero[i].hurthead = false; hero[i].hurttorso = false; hero[i].hurtlegs = false; hero[i].hurtfeet = false; hero[i].hurthand = false; hero[i].hurtweapon = false;
                                        }
                                }
                                hupdate();
                                herosheet.repaint();
                                weaponsheet.repaint();
                        }
                        */
                        else if (Character.toLowerCase(e.getKeyChar())=='f') System.out.println(""+Runtime.getRuntime().freeMemory());
                    }
                    if (!sheet) {
                        //turn left
                        if(e.getKeyChar()=='7' || Character.toLowerCase(e.getKeyChar())==turnleftkey) {
                                //if (walkqueue.size()<4) walkqueue.add(ITURNLEFT);
                                if (walkqueue.size()<4) arrowsheet.doClick(ITURNLEFT);
                        }
                        //turn right
                        else if (e.getKeyChar()=='9' || Character.toLowerCase(e.getKeyChar())==turnrightkey) {
                                //if (walkqueue.size()<4) walkqueue.add(ITURNRIGHT);
                                if (walkqueue.size()<4) arrowsheet.doClick(ITURNRIGHT);
                        }
                        //forward
                        else if (e.getKeyChar()=='8' || Character.toLowerCase(e.getKeyChar())==forwardkey) {
                                //if (walkqueue.size()<4) walkqueue.add(IFORWARD);
                                if (walkqueue.size()<4) arrowsheet.doClick(IFORWARD);
                        }
                        //back
                        else if (e.getKeyChar()=='5' || e.getKeyChar()=='2' || Character.toLowerCase(e.getKeyChar())==backkey) {
                                //if (walkqueue.size()<4) walkqueue.add(IBACK);
                                if (walkqueue.size()<4) arrowsheet.doClick(IBACK);
                        }
                        //left
                        else if (e.getKeyChar()=='4' || Character.toLowerCase(e.getKeyChar())==leftkey) {
                                //if (walkqueue.size()<4) walkqueue.add(ILEFT);
                                if (walkqueue.size()<4) arrowsheet.doClick(ILEFT);
                        }
                        //right
                        else if (e.getKeyChar()=='6' || Character.toLowerCase(e.getKeyChar())==rightkey) {
                                //if (walkqueue.size()<4) walkqueue.add(IRIGHT);
                                if (walkqueue.size()<4) arrowsheet.doClick(IRIGHT);
                        }
                        //else if (e.getKeyChar()=='/') { backindex=(backindex+1)%backimage.length; imagePane.setImage(backimage[backindex]); }
                     /*
                        //magicvision - take this out eventually
                        else if (e.getKeyChar()=='-') {
                                magicvision+=20;
                                if (magicvision==20) needredraw=true;
                        }
                        //torch spell - take this out eventually
                        else if (e.getKeyChar()=='+') {
                                magictorch+=20;  if (magictorch>285) magictorch=285;
                                if ((darkfactor+20)>255) darkfactor=255;
                                else darkfactor+=20;
                                needredraw=true;
                        }
                        else if (e.getKeyChar()=='n') {
                                for (int i=0;i<mapheight;i++) {
                                        for (int j=0;j<mapwidth;j++) {
                                                System.out.print(DungeonMap[level][j][i].tostring());
                                        }
                                        System.out.println("");
                                }
                        }
                        else if (e.getKeyChar()=='v') {
                                for (int i=0;i<4;i++) {
                                        for (int j=0;j<5;j++) {
                                                System.out.print(visibleg[i][j].tostring()+" ");
                                        }
                                        System.out.println("");
                                }
                        }
                        //hammer down a wall - eventually add rocks,boulders to new floor square
                        else if (e.getKeyChar()=='[') {
                                int xadjust=partyx, yadjust=partyy;
                                if (facing==NORTH) yadjust--;
                                else if (facing==WEST) xadjust--;
                                else if (facing==SOUTH) yadjust++;
                                else xadjust++;
                                if (xadjust>=0 && yadjust>=0 && xadjust<mapwidth && yadjust<mapheight) {
                                        if (!DungeonMap[level][xadjust][yadjust].hasMons) {
                                           DungeonMap[level][xadjust][yadjust]=new Floor();
                                           needredraw = true;
                                        }
                                }
                        }
                        //make a wall
                        else if (e.getKeyChar()==']') {
                                int xadjust=partyx, yadjust=partyy;
                                if (facing==NORTH) yadjust--;
                                else if (facing==WEST) xadjust--;
                                else if (facing==SOUTH) yadjust++;
                                else xadjust++;
                                if (xadjust>=0 && yadjust>=0 && xadjust<mapwidth && yadjust<mapheight) {
                                        if (!DungeonMap[level][xadjust][yadjust].hasMons) {
                                           DungeonMap[level][xadjust][yadjust]=new Wall();
                                           needredraw = true;
                                        }
                                }
                        }
                     */
                    }
                }

                public void partyTurn(bool left) {
                        nomirroradjust = false;
                        if (DungeonMap[level][partyx][partyy] instanceof Stairs) {
                                DungeonMap[level][partyx][partyy].tryTeleport();
                                mirrorback = !mirrorback;
                                walkqueue.clear();
                                return;
                        }
                        mirrorback = !mirrorback;
                        bool oldmirror = mirrorback;
                        int oldfacing = facing; int oldlevel = level; int oldx = partyx; int oldy = partyy;
                        if (left) facing++;
                        else facing--;
                        if (facing<0) facing=3;
                        else if (facing>3) facing=0;
                        int newfacing = facing;
                        Projectile tempp;
                        for (Iterator i=dmprojs.iterator();i.hasNext();) {
                          tempp = (Projectile)i.next();
                          if (tempp.it!=null && tempp.it.hasthrowpic) {
                              int s = 2; //left
                              if (facing==tempp.direction) { s=0; } //away
                              else if ((facing-tempp.direction)%2==0) { s=1; }//towards - was Math.abs(facing-tem...)%2
                              else if (facing==(tempp.direction+1)%4) { s=3; } //right = dpic
                              tempp.pic = tempp.it.throwpic[s];
                          }
                        }
                        Pillar.swapmirror = !Pillar.swapmirror;
                        needredraw=true;
                        if (AUTOMAP) dmmap.doMap();
                        if (DungeonMap[level][partyx][partyy] instanceof EventSquare && ((EventSquare)DungeonMap[level][partyx][partyy]).eventface-1==facing) {
                                if (facing!=compassface && numheroes>0) {
                                        Compass.updateCompass(facing);
                                        compassface = facing;
                                        if (iteminhand && inhand.number==8) changeCursor();
                                        for (int i=0;i<numheroes;i++) hero[i].repaint();
                                        weaponsheet.repaint();
                                        if (sheet) herosheet.repaint();
                                }
                                //mirrorback = !mirrorback;
                                DungeonMap[level][partyx][partyy].tryTeleport();
                        }
                        else if (DungeonMap[level][partyx][partyy] instanceof FloorSwitch && ((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface>0) {
                                if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==facing) {
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGON);
                                }
                                else if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==oldfacing) {
                                        int tf = facing;
                                        facing = oldfacing;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGOFF);
                                        facing = tf;
                                }
                        }
                        else if (DungeonMap[level][partyx][partyy] instanceof MultFloorSwitch2) ((MultFloorSwitch2)DungeonMap[level][partyx][partyy]).turnTest(oldfacing);
                        if (mirrorback!=oldmirror && !nomirroradjust && (oldx!=partyx || oldy!=partyy || oldlevel!=level || newfacing!=facing)) mirrorback = !mirrorback;
                        nomirroradjust = false;
                }


                /*
                public void turnLeft() {
                        if (DungeonMap[level][partyx][partyy] instanceof Stairs) {
                                DungeonMap[level][partyx][partyy].tryTeleport();
                                mirrorback = !mirrorback;
                                walkqueue.clear();
                                return;
                        }
                        mirrorback = !mirrorback;
                        int oldfacing = facing; int oldlevel = level; int oldx = partyx; int oldy = partyy;
                        facing++;
                        if (facing>3) facing=0;
                        int newfacing = facing;
                        Projectile tempp;
                        for (Iterator i=dmprojs.iterator();i.hasNext();) {
                          tempp = (Projectile)i.next();
                          if (tempp.it!=null && tempp.it.hasthrowpic) {
                              int s = 2; //left
                              if (facing==tempp.direction) { s=0; } //away
                              else if ((facing-tempp.direction)%2==0) { s=1; }//towards - was Math.abs(facing-tem...)%2
                              else if (facing==(tempp.direction+1)%4) { s=3; } //right = dpic
                              tempp.pic = tempp.it.throwpic[s];
                              //if (s<3) tempp.pic = tempp.it.throwpic[s];
                              //else tempp.pic = tempp.it.dpic;
                          }
                        }
                        Pillar.swapmirror = !Pillar.swapmirror;
                        needredraw=true;
                        if (AUTOMAP) dmmap.doMap();
                        if (DungeonMap[level][partyx][partyy] instanceof EventSquare && ((EventSquare)DungeonMap[level][partyx][partyy]).eventface-1==facing) turnTest();
                        else if (DungeonMap[level][partyx][partyy] instanceof FloorSwitch && ((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface>0) {
                                if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==facing) {
                                        DungeonMap[level][partyx][partyy].hasParty=false;
                                        ((FloorSwitch)DungeonMap[level][partyx][partyy]).partyflag=true;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGON);
                                        //DungeonMap[level][partyx][partyy].hasParty=true;
                                }
                                else if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==oldfacing) {
                                        int tf = facing;
                                        DungeonMap[level][partyx][partyy].hasParty=false;
                                        facing = oldfacing;
                                        ((FloorSwitch)DungeonMap[level][partyx][partyy]).partyflag=true;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGOFF);
                                        facing = tf;
                                        //DungeonMap[level][partyx][partyy].hasParty=true;
                                }
                        }
                        else if (DungeonMap[level][partyx][partyy] instanceof MultFloorSwitch2) ((MultFloorSwitch2)DungeonMap[level][partyx][partyy]).turnTest(oldfacing);
                        if (oldx!=partyx || oldy!=partyy || oldlevel!=level || newfacing!=facing) mirrorback = !mirrorback;
                }

                public void turnRight() {
                        if (DungeonMap[level][partyx][partyy] instanceof Stairs) {
                                DungeonMap[level][partyx][partyy].tryTeleport();
                                mirrorback = !mirrorback;
                                walkqueue.clear();
                                return;
                        }
                        mirrorback = !mirrorback;
                        int oldfacing = facing; int oldlevel = level; int oldx = partyx; int oldy = partyy;
                        facing--;
                        if (facing<0) facing=3;
                        int newfacing = facing;
                        Projectile tempp;
                        for (Iterator i=dmprojs.iterator();i.hasNext();) {
                          tempp = (Projectile)i.next();
                          if (tempp.it!=null && tempp.it.hasthrowpic) {
                              int s = 2; //left
                              if (facing==tempp.direction) { s=0; } //away
                              else if ((facing-tempp.direction)%2==0) { s=1; }//towards - was Math.abs(facing-tem...)%2
                              else if (facing==(tempp.direction+1)%4) { s=3; } //right = dpic
                              tempp.pic = tempp.it.throwpic[s];
                              //if (s<3) tempp.pic = tempp.it.throwpic[s];
                              //else tempp.pic = tempp.it.dpic;
                          }
                        }
                        Pillar.swapmirror = !Pillar.swapmirror;
                        needredraw = true;
                        if (AUTOMAP) dmmap.doMap();
                        if (DungeonMap[level][partyx][partyy] instanceof EventSquare && ((EventSquare)DungeonMap[level][partyx][partyy]).eventface-1==facing) turnTest();
                        else if (DungeonMap[level][partyx][partyy] instanceof FloorSwitch && ((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface>0) {
                                if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==facing) {
                                        DungeonMap[level][partyx][partyy].hasParty=false;
                                        ((FloorSwitch)DungeonMap[level][partyx][partyy]).partyflag=true;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGON);
                                        //DungeonMap[level][partyx][partyy].hasParty=true;
                                }
                                else if (((FloorSwitch)DungeonMap[level][partyx][partyy]).switchface-1==oldfacing) {
                                        int tf = facing;
                                        DungeonMap[level][partyx][partyy].hasParty=false;
                                        facing = oldfacing;
                                        ((FloorSwitch)DungeonMap[level][partyx][partyy]).partyflag=true;
                                        DungeonMap[level][partyx][partyy].tryFloorSwitch(MapObject.PARTYSTEPPINGOFF);
                                        facing = tf;
                                        //DungeonMap[level][partyx][partyy].hasParty=true;
                                }
                        }
                        else if (DungeonMap[level][partyx][partyy] instanceof MultFloorSwitch2) ((MultFloorSwitch2)DungeonMap[level][partyx][partyy]).turnTest(oldfacing);
                        if (oldx!=partyx || oldy!=partyy || oldlevel!=level || newfacing!=facing) mirrorback = !mirrorback;
                }
                */
                
                /*
                private void turnTest() {
                        needredraw = false;
                        needdrawdungeon = true;
                        dview.repaint();
                        if (facing!=compassface && numheroes>0) {
                                Compass.updateCompass(facing);
                                compassface = facing;
                                if (iteminhand && inhand.number==8) changeCursor();
                                for (int i=0;i<numheroes;i++) hero[i].repaint();
                                weaponsheet.repaint();
                                if (sheet) herosheet.repaint();
                        }
                        DungeonMap[level][partyx][partyy].tryTeleport();
                }
                */
                
                public void partyMove(int dir) {
                        nomirroradjust = false;
                        //int c = 0;
                        int xadjust=0, yadjust=0;
                        int newx,newy;
                        if (facing==NORTH) yadjust=1;
                        else if (facing==WEST) xadjust=1;
                        else if (facing==SOUTH) yadjust=-1;
                        else xadjust=-1;

                        switch (dir) {
                                case FORWARD:
                                        newx = partyx-xadjust;
                                        newy = partyy-yadjust;
                                        break;
                                case BACK:
                                        newx = partyx+xadjust;
                                        newy = partyy+yadjust;
                                        break;
                                case LEFT:
                                        newx = partyx-yadjust;
                                        newy = partyy+xadjust;
                                        break;
                                default: //right
                                        newx = partyx+yadjust;
                                        newy = partyy-xadjust;
                        }
                        if (newx<0 || newy<0 || newx>=mapwidth || newy>=mapheight || (dir==BACK && DungeonMap[level][partyx][partyy] instanceof Stairs)) {
                                if (DungeonMap[level][partyx][partyy] instanceof Stairs) {
                                        //playSound("step.wav",-1,-1);
                                        DungeonMap[level][partyx][partyy].tryTeleport();
                                        if (AUTOMAP) dmmap.doMap();

                                }
                                else if (numheroes!=0) playSound("bump.wav",newx,newy);
                                //walkqueue.clear();
                                return;
                        }
                        if (DungeonMap[level][newx][newy].isPassable && !DungeonMap[level][newx][newy].hasMons) {
                                //check if old square had projs, end them if shared a subsquare with a hero or were walked through
                                if (numheroes>0 && DungeonMap[level][partyx][partyy].numProjs>0) {
                                        int nump = DungeonMap[level][partyx][partyy].numProjs;
                                        Projectile tempproj;
                                        int j=0,index=0;
                                        int shouldend = -1;
                                        while (j<nump) {
                                            do {
                                                  tempproj = (Projectile)dmprojs.get(index);
                                                  index++;
                                            }
                                            while (tempproj.level!=level || tempproj.x!=partyx || tempproj.y!=partyy);
                                            if (!tempproj.justthrown && !tempproj.isending) {
                                                int projsub = (tempproj.subsquare+facing)%4;
                                                if (heroatsub[projsub]!=-1) shouldend = heroatsub[projsub];
                                                else {
                                                     switch (dir) {
                                                        case FORWARD:
                                                                if (projsub==0) shouldend = heroatsub[3];
                                                                else if (projsub==1) shouldend = heroatsub[2];
                                                                break;
                                                        case BACK:
                                                                if (projsub==2) shouldend = heroatsub[1];
                                                                else if (projsub==3) shouldend = heroatsub[0];
                                                                break;
                                                        case LEFT:
                                                                if (projsub==0) shouldend = heroatsub[1];
                                                                else if (projsub==3) shouldend = heroatsub[2];
                                                                break;
                                                        default: //right
                                                                if (projsub==1) shouldend = heroatsub[0];
                                                                else if (projsub==2) shouldend = heroatsub[3];
                                                     }
                                                }
                                                if (shouldend!=-1) {
                                                        //proj hits hero[shouldend]
                                                        //System.out.println(hero[shouldend].name+" is hit by a proj.");
                                                        int oldhas = heroatsub[projsub];
                                                        heroatsub[projsub] = shouldend;
                                                        tempproj.projend();
                                                        if (oldhas==-1 || !hero[oldhas].isdead) heroatsub[projsub] = oldhas;
                                                        if (hero[shouldend].isdead) heroatsub[hero[shouldend].subsquare]=-1;
                                                        formation.addNewHero();
                                                        if (tempproj.it!=null) { index--; dmprojs.remove(index); }
                                                        else {
                                                                //move end pic onto new square
                                                                tempproj.x = newx; tempproj.y = newy;
                                                                DungeonMap[level][partyx][partyy].numProjs--;
                                                                DungeonMap[level][newx][newy].numProjs++;
                                                        }
                                                }
                                            }
                                            j++;
                                        }
                                }
                                
                                //playSound("step.wav",-1,-1);
                                
                                int oldy = partyy, oldx = partyx, oldlevel = level, oldface = facing;
                                MapObject newsquare = DungeonMap[level][newx][newy];
                                //remove party from old square
                                DungeonMap[level][partyx][partyy].hasParty=false;
                                //add party to new square
                                partyy=newy; partyx=newx;
                                DungeonMap[level][newx][newy].hasParty=true;
                                bool oldmirror = mirrorback;
                                mirrorback = !mirrorback;
                                //if have champions, test switch stepping off
                                if (numheroes!=0) {
                                        DungeonMap[oldlevel][oldx][oldy].tryFloorSwitch(MapObject.PARTYSTEPPINGOFF);
                                }
                                //continue if game is not over and didn't get teleported
                                if (!gameover && DungeonMap[level][newx][newy].hasParty) {
                                        //if have champions and newsquare hasn't changed, test switch stepping on
                                        if (numheroes!=0 && newsquare==DungeonMap[level][newx][newy]) DungeonMap[level][newx][newy].tryFloorSwitch(MapObject.PARTYSTEPPINGON);
                                        
                                        //try teleport if game is not over and didn't already get teleported and newsquare didn't change
                                        if (!gameover && DungeonMap[level][newx][newy].hasParty && newsquare==DungeonMap[level][newx][newy]) {
                                                //if (DungeonMap[level][newx][newy].mapchar=='p' && ((Pit)DungeonMap[level][newx][newy]).isOpen) shouldadjust = false;
                                                DungeonMap[level][newx][newy].tryTeleport();
                                        }
                                        
                                        //test to make sure have actually moved (some teleports act as barriers)
                                        if (partyx!=oldx || partyy!=oldy || level!=oldlevel) {
                                           //adjust mirroring if necessary (if teleport messed it up)
                                           if (mirrorback==oldmirror && !nomirroradjust) mirrorback = !mirrorback;
                                           //check if new square has projs that were passed through, end them now if so (only if new square was stepped into, not teleported into)
                                           if (!gameover && numheroes>0 && DungeonMap[level][partyx][partyy].numProjs>0 && partyx==newx && partyy==newy && level==oldlevel) {
                                                int nump = DungeonMap[level][partyx][partyy].numProjs;
                                                Projectile tempproj;
                                                int j=0,index=0;
                                                int shouldend = -1;
                                                while (j<nump) {
                                                    do {
                                                          tempproj = (Projectile)dmprojs.get(index);
                                                          index++;
                                                    }
                                                    while (tempproj.level!=level || tempproj.x!=partyx || tempproj.y!=partyy);
                                                    if (!tempproj.isending) {
                                                        int projsub = (tempproj.subsquare+facing)%4;
                                                        switch (dir) {
                                                                case FORWARD:
                                                                        if (projsub==2) shouldend = heroatsub[1];
                                                                        else if (projsub==3) shouldend = heroatsub[0];
                                                                        break;
                                                                case BACK:
                                                                        if (projsub==0) shouldend = heroatsub[3];
                                                                        else if (projsub==1) shouldend = heroatsub[2];
                                                                        break;
                                                                case LEFT:
                                                                        if (projsub==1) shouldend = heroatsub[0];
                                                                        else if (projsub==2) shouldend = heroatsub[3];
                                                                        break;
                                                                default: //right
                                                                        if (projsub==0) shouldend = heroatsub[1];
                                                                        else if (projsub==3) shouldend = heroatsub[2];
                                                        }
                                                        if (shouldend!=-1) {
                                                                //proj hits hero[shouldend]
                                                                //System.out.println(hero[shouldend].name+" is hit by a proj.");
                                                                int oldhas = heroatsub[projsub];
                                                                heroatsub[projsub] = shouldend;
                                                                tempproj.projend();
                                                                //heroatsub[projsub] = oldhas;
                                                                if (oldhas==-1 || !hero[oldhas].isdead) heroatsub[projsub] = oldhas;
                                                                if (hero[shouldend].isdead) heroatsub[hero[shouldend].subsquare]=-1;
                                                                formation.addNewHero();
                                                                if (tempproj.it!=null) { index--; dmprojs.remove(index); }
                                                        }
                                                    }
                                                    j++;
                                                }
                                           }
                                        }
                                        //didn't move, so if didn't change facing adjust mirroring if necessary
                                        else if (oldface==facing && mirrorback!=oldmirror) mirrorback = !mirrorback;
                                        needredraw=true;
                                        if (!gameover) {
                                                for (int i=0;i<numheroes;i++) {
                                                        hero[i].walkcounter++;
                                                        if (hero[i].load>hero[i].maxload*3/4) hero[i].walkcounter++;
                                                        if (hero[i].load>hero[i].maxload) hero[i].walkcounter++; //if overloaded, stamina drains faster
                                                        //if (hero[i].walkcounter>hero[i].vitality/4) {
                                                        if (hero[i].walkcounter>hero[i].vitality/3) {
                                                                //hero[i].vitalize(-(int)hero[i].load/10-1);
                                                                hero[i].vitalize(-(int)hero[i].load/15-1);
                                                                hero[i].repaint();
                                                                hero[i].walkcounter=0;
                                                        }
                                                }
                                                if (AUTOMAP) dmmap.doMap();
                                        }
                                }
                                else if (!nomirroradjust && mirrorback==oldmirror) mirrorback = !mirrorback;
                                nomirroradjust = false;
                        }
                        else if (!DungeonMap[level][newx][newy].isPassable) {
                                if (numheroes!=0) playSound("bump.wav",newx,newy);
                                if (AUTOMAP && DungeonMap[level][newx][newy].mapchar=='i') dmmap.update(level,newx,newy,'i');
                        }
                }
        }
        
        
        /*
        class WeaponSheet extends ImageTilePanel implements ActionListener{

                JButton[] weaponFunction = new JButton[3];
                JToggleButton[] weaponButton = new JToggleButton[4];
                //ButtonGroup userGroup = new ButtonGroup();
                Box uppan;
                JPanel dwnpan,hitpan;
                JLabel hitlabel;
                ImageIcon hiticon,hiticon2,hiticon3,missicon;
                WeaponClick weaponclick = new WeaponClick();
                WeaponFunctClick weaponfunctclick = new WeaponFunctClick();

                bool showingspecials = false;
                JPanel buttonpanel;
                JScrollPane buttonpane;
                Dimension buttondim = new Dimension(125,22);
                Insets buttoninsets = new Insets(0,5,0,5);
                ArrayList buttonlist;
                Hero h;

                public WeaponSheet() {
                        //super(false);//no double buffering
                        //super(true);//double buffering
                        super("Icons"+File.separator+"weaponsheet.gif");
                        setTiled(false);
                        setDoubleBuffered(true);
                        setBorder(BorderFactory.createEmptyBorder(4,4,4,4));

                        setPreferredSize(new Dimension(155,140));//was 155,116
                        setMaximumSize(new Dimension(155,140));
                        setMinimumSize(new Dimension(155,140));
                        setLayout(new BoxLayout(this,BoxLayout.Y_AXIS));
                        //setOpaque(false);

                        uppan = new Box(BoxLayout.X_AXIS);
                        dwnpan = new JPanel();
                        //dwnpan.setPreferredSize(new Dimension(120,62)); //was 120,45
                        //dwnpan.setMaximumSize(new Dimension(120,62));
                        //dwnpan.setMinimumSize(new Dimension(120,62));
                        dwnpan.setPreferredSize(new Dimension(125,62)); //was 120,45
                        dwnpan.setMaximumSize(new Dimension(125,62));
                        dwnpan.setMinimumSize(new Dimension(125,62));
                        dwnpan.setLayout(new GridLayout(3,1));
                        dwnpan.setOpaque(false);

                        for (int i=0;i<4;i++) {
                                weaponButton[i] = new JToggleButton();
                                weaponButton[i].setPreferredSize(new Dimension(36,36));
                                weaponButton[i].setMaximumSize(new Dimension(36,36));
                                weaponButton[i].setMinimumSize(new Dimension(36,36));
                                weaponButton[i].setActionCommand(""+i);
                                weaponButton[i].addActionListener(weaponclick);
                                weaponButton[i].addMouseListener(weaponclick);
                                //userGroup.add(weaponButton[i]);
                                uppan.add(weaponButton[i]);
                        }
                        weaponButton[weaponready].setSelected(true);

                        for (int j=0;j<3;j++) {
                                weaponFunction[j] = new JButton();
                                //weaponFunction[j].setPreferredSize(new Dimension(120,22));
                                //weaponFunction[j].setMaximumSize(new Dimension(120,22));
                                //weaponFunction[j].setMinimumSize(new Dimension(120,22));
                                weaponFunction[j].setPreferredSize(buttondim);
                                weaponFunction[j].setMaximumSize(buttondim);
                                weaponFunction[j].setMinimumSize(buttondim);
                                weaponFunction[j].setMargin(buttoninsets);
                                weaponFunction[j].setActionCommand(""+j);
                                weaponFunction[j].addActionListener(weaponfunctclick);
                                weaponFunction[j].addMouseListener(weaponclick);
                                dwnpan.add(weaponFunction[j]);
                        }
                        hiticon = new ImageIcon("Icons"+File.separator+"wephit.gif");
                        hiticon2 = new ImageIcon("Icons"+File.separator+"wephit2.gif");
                        hiticon3 = new ImageIcon("Icons"+File.separator+"wephit3.gif");
                        missicon = new ImageIcon("Icons"+File.separator+"wepmiss.gif");
                        hitlabel = new JLabel(hiticon3);
                        hitlabel.setHorizontalTextPosition(JLabel.CENTER);
                        hitlabel.setForeground(new Color(240,170,115));
                        hitpan = new JPanel();
                        hitpan.setOpaque(false);
                        hitpan.add(hitlabel);
                        hitpan.setVisible(false);

                        buttonpanel = new JPanel();
                        buttonpanel.setLayout(new BoxLayout(buttonpanel,BoxLayout.Y_AXIS));
                        buttonpane = new JScrollPane(buttonpanel);
                        buttonpane.setVisible(false);
                        buttonlist = new ArrayList();
                        
                        add(uppan);
                        add(Box.createVerticalStrut(5));
                        add(dwnpan);
                        add(hitpan);
                        add(buttonpane);
                        setCursor(handcursor);
                        addMouseListener(weaponclick);
                }
                
                public void update() {
                        for (int i=1;i<4;i++) {
                                //System.out.println("weaponsheet update, numheroes = "+numheroes+", i = "+i);
                                if (numheroes<=i) {
                                   weaponButton[i].setIcon(null);
                                   //System.out.println("set null icon");
                                   weaponButton[i].setDisabledIcon(null);
                                   //System.out.println("set null disabled icon");
                                   weaponButton[i].setEnabled(false);
                                   //System.out.println("set enabled false");
                                   weaponButton[i].setSelected(false);
                                }
                        }
                        //System.out.println("weaponsheet update, got here");
                        for (int i=0;i<numheroes;i++) {
                                ImageIcon junkicon = (ImageIcon)weaponButton[i].getIcon();
                                if (junkicon!=null) junkicon.setImageObserver(null);
                                //System.out.println("nulled imageobserver");
                                //could also get the icons image, and flush it
                                weaponButton[i].setIcon(new ImageIcon(hero[i].weapon.pic));
                                if (hero[i].weapon.type==Item.WEAPON && !hero[i].isdead) {
                                        weaponButton[i].setEnabled(true);
                                        if (weaponready==i) {
                                                weaponButton[i].setSelected(true);
                                                int index=0;
                                                bool shouldshow;
                                                while(index<hero[i].weapon.functions) {
                                                        shouldshow = true;
                                                        switch (hero[i].weapon.function[index][1].charAt(0)) {
                                                                case 'f':
                                                                        if (hero[i].flevel<hero[i].weapon.level[index]) shouldshow = false;
                                                                        break;
                                                                case 'n':
                                                                        if (hero[i].nlevel<hero[i].weapon.level[index]) shouldshow = false;
                                                                        break;
                                                                case 'w':
                                                                        if (hero[i].wlevel<hero[i].weapon.level[index]) shouldshow = false;
                                                                        break;
                                                                case 'p':
                                                                        if (hero[i].plevel<hero[i].weapon.level[index]) shouldshow = false;
                                                                        break;
                                                        }
                                                        if (shouldshow) {
                                                                weaponFunction[index].setText(hero[i].weapon.function[index][0]);
                                                                weaponFunction[index].setVisible(true);
                                                        }
                                                        else {
                                                                weaponFunction[index].setText("");
                                                                weaponFunction[index].setVisible(false);
                                                        }
                                                        index++;
                                                }
                                                for (int j=index;j<3;j++) {
                                                        weaponFunction[j].setText("");
                                                        weaponFunction[j].setVisible(false);
                                                }
                                                //System.out.println("enable or disable functions");
                                                if (hero[weaponready].wepready) functionsEnable(true);
                                                else functionsEnable(false);
                                        }
                                        else weaponButton[i].setSelected(false);
                                }
                                else {
                                        //System.out.println("hero "+i+" is dead, so disable and unselect");
                                        weaponButton[i].setDisabledIcon(null);//resets disabled icon
                                        //weaponButton[i].setDisabledIcon(new ImageIcon(GrayFilter.createDisabledImage(hero[i].weapon.pic)));
                                        //System.out.println("dead, got here 1");
                                        weaponButton[i].setSelected(false);
                                        //System.out.println("dead, got here 2");
                                        weaponButton[i].setEnabled(false);
                                        //System.out.println("dead, got here 3");
                                        if (weaponready==i) {
                                                weaponFunction[0].setVisible(false);
                                                weaponFunction[1].setVisible(false);
                                                weaponFunction[2].setVisible(false);
                                        }
                                        //System.out.println("dead, got here 4");
                                }
                        }
                }

                public void functionsEnable(bool enab) {
                        if (hitpan.isVisible() && hero[weaponready].hitcounter<=0) {
                                hitpan.setVisible(false);
                                dwnpan.setVisible(true);
                        }
                        else if (!hitpan.isVisible() && !showingspecials && hero[weaponready].hitcounter>0) {
                                dwnpan.setVisible(false);
                                hitpan.setVisible(true);
                        }
                        if (dwnpan.isVisible()) for (int i=0;i<hero[weaponready].weapon.functions;i++) {
                                weaponFunction[i].setEnabled(enab);
                        }
                        ((BoxLayout)uppan.getLayout()).invalidateLayout(uppan);
                        //uppan.doLayout();
                        uppan.validate();
                        this.repaint();
                }
                
                public void toggleSpecials(int i) {
                        if (showingspecials) {
                                showingspecials = false;
                                buttonpane.setVisible(false);
                                uppan.setVisible(true);
                                if (hero[weaponready].hitcounter<=0) dwnpan.setVisible(true);
                                else hitpan.setVisible(true);
                                update();
                        }
                        else {
                                showingspecials = true;
                                uppan.setVisible(false);
                                dwnpan.setVisible(false);
                                hitpan.setVisible(false);
                                if (i<numheroes && !hero[i].isdead) {
                                        weaponready = i;
                                        weaponButton[weaponready].doClick();
                                }
                                showForHero(hero[weaponready]);
                                buttonpane.setVisible(true);
                        }
                }

                public synchronized void showForHero(Hero h) {
                        this.h = h;
                        buttonpanel.removeAll();
                        buttonlist.clear();
                        if (h.abilities!=null) {
                                JButton button;
                                for (int i=0;i<h.abilities.length;i++) {
                                        button = new JButton(h.abilities[i].name);
                                        button.setActionCommand(""+i);
                                        button.addActionListener(this);
                                        button.addMouseListener(weaponclick);
                                        if (h.abilities[i].count<=0 && h.abilities[i].mana<=h.mana) {}
                                        else button.setEnabled(false);
                                        if (h.abilities[i].flevelneed<=h.flevel && h.abilities[i].nlevelneed<=h.nlevel && h.abilities[i].wlevelneed<=h.wlevel && h.abilities[i].plevelneed<=h.plevel) {}
                                        else button.setVisible(false);
                                        button.setPreferredSize(buttondim);
                                        button.setMinimumSize(buttondim);
                                        button.setMaximumSize(buttondim);
                                        button.setMargin(buttoninsets);
                                        buttonpanel.add(button);
                                        buttonlist.add(button);
                                }
                        }
                }
                
                public synchronized void updateSpecials() {
                        for (int j=0;j<numheroes;j++) {

                                if (hero[j].abilities!=null) for (int i=0;i<hero[j].abilities.length;i++) {
                                        //decrement speed counter
                                        if (hero[j].abilities[i].count>0) hero[j].abilities[i].count--;
                                        //if current hero, update button if necessary
                                        if (showingspecials && hero[j]==h) {
                                                JButton button = (JButton)buttonlist.get(i);
                                                if (!button.isEnabled() && h.abilities[i].count==0 && h.abilities[i].mana<=h.mana) button.setEnabled(true);
                                                if (h.abilities[i].flevelneed<=h.flevel && h.abilities[i].nlevelneed<=h.nlevel && h.abilities[i].wlevelneed<=h.wlevel && h.abilities[i].plevelneed<=h.plevel) button.setVisible(true);
                                                else button.setVisible(false);
                                        }
                                }
                        }
                }
                
                public void actionPerformed(ActionEvent e) {
                        int i = Integer.parseInt(e.getActionCommand());
                        //System.out.println(h.name+"'s ability "+i);
                        if (!h.doAbility(h.abilities[i].getActionName(),h.abilities[i].power,h.abilities[i].classgain,h.abilities[i].data)) return; //do nothing if action name not found
                        //play sound if not null
                        if (h.abilities[i].sound!=null && !h.abilities[i].sound.equals("")) playSound(h.abilities[i].sound,-1,-1,0);
                        //decrease mana if supposed to
                        if (h.abilities[i].mana>0) {
                                h.mana-=h.abilities[i].mana;
                                if (h.mana<h.abilities[i].mana) ((JButton)buttonlist.get(i)).setEnabled(false);
                        }
                        //set count to speed (delay before can use again)
                        if (h.abilities[i].speed>0) {
                                h.abilities[i].count = h.abilities[i].speed;
                                ((JButton)buttonlist.get(i)).setEnabled(false);
                        }
                        hupdate();
                        if (sheet) herosheet.repaint();
                        needredraw = true;
                }
        }
        */
        
        /*
        class SpellSheet extends JPanel {
                ImageIcon[] spellsymbol = new ImageIcon[24];
                ImageIcon casticon;
                JButton[] spellButton = new JButton[6];
                JButton backButton = new JButton();
                JButton toCastButton = new JButton();
                JToggleButton[] casterButton = new JToggleButton[4];
                SpellCasterClick spcastclick;
                ButtonGroup casterGroup = new ButtonGroup();
                Box uppan,midpan,dwnpan;
                Dimension unselecteddim,selecteddim;
                BufferedImage castsymbs,bufsrc;
                Graphics2D castg;
                
                public SpellSheet() {
                        Image src = tk.createImage("spell.gif");
                        ImageTracker.addImage(src,5);
                        try { ImageTracker.waitForID(5); } catch (InterruptedException e) {}
                        
                        //could try:
                        bufsrc = new BufferedImage(src.getWidth(null),src.getHeight(null),BufferedImage.TYPE_INT_ARGB);//was PRE
                        Graphics2D tempg = bufsrc.createGraphics();
                        tempg.drawImage(src,0,0,null);
                        tempg.dispose();
                        
                        int index = 0;
                        for (int j=0;j<4;j++) {
                                for (int i=0;i<6;i++) {
                                        //could try:
                                        spellsymbol[index]=new ImageIcon(bufsrc.getSubimage(i*12,j*12,12,12));
                                        
                                        //spellsymbol[index]=new ImageIcon(createImage(new FilteredImageSource(src.getSource(),new CropImageFilter(i*12,j*12,12,12))));
                                        index++;
                                }
                        }
                        //src.flush(); src=null;
                        
                        SpellClick spellclick = new SpellClick();
                        SpellSymbolClick spsymclick = new SpellSymbolClick();
                        spcastclick = new SpellCasterClick();
                        
                        Border compound1 = BorderFactory.createCompoundBorder(
                                BorderFactory.createRaisedBevelBorder(),BorderFactory.createLoweredBevelBorder());

                        setBorder(compound1);
                        //setBorder(BorderFactory.createCompoundBorder(
                        //        BorderFactory.createLineBorder(Color.blue),compound1));

                        setLayout(new GridLayout(3,1,0,0));
                        setPreferredSize(new Dimension(160,80));
                        setMaximumSize(new Dimension(160,80));
                        //setOpaque(false);

                        uppan = new Box(BoxLayout.X_AXIS);
                        midpan = new Box(BoxLayout.X_AXIS);
                        dwnpan = new Box(BoxLayout.X_AXIS);
                        unselecteddim = new Dimension(16,20);
                        selecteddim = new Dimension(95,20);
                        
                        uppan.add(Box.createHorizontalGlue());
                        midpan.add(Box.createHorizontalGlue());
                        dwnpan.add(Box.createHorizontalGlue());
                        for (int i=0;i<4;i++) {
                                casterButton[i]=new JToggleButton();
                                casterButton[i].setPreferredSize(unselecteddim);
                                casterButton[i].setMaximumSize(unselecteddim);
                                casterButton[i].setActionCommand(""+i);
                                casterButton[i].addActionListener(spcastclick);
                                casterGroup.add(casterButton[i]);
                                uppan.add(casterButton[i]);
                        }
                        casterButton[spellready].setSelected(true);
                        casterButton[spellready].setPreferredSize(selecteddim);
                        casterButton[spellready].setMaximumSize(selecteddim);
                        casterButton[spellready].setText(hero[spellready].name);
                        backButton.setIcon(new ImageIcon(createImage(new FilteredImageSource(src.getSource(),new CropImageFilter(0,48,17,14)))));
                        backButton.setPreferredSize(new Dimension(20,20));
                        backButton.setMaximumSize(new Dimension(20,20));
                        backButton.setActionCommand("undo");
                        backButton.addActionListener(spellclick);

                        toCastButton.setPreferredSize(new Dimension(100,20));
                        toCastButton.setMaximumSize(new Dimension(100,20));
                        toCastButton.addActionListener(spellclick);
                        for (int i=0;i<6;i++) {
                                spellButton[i]=new JButton();
                                spellButton[i].setPreferredSize(new Dimension(20,20));
                                spellButton[i].setMaximumSize(new Dimension(20,20));
                                spellButton[i].setActionCommand(""+i);
                                spellButton[i].addActionListener(spsymclick);
                                midpan.add(spellButton[i]);
                        }
                        castsymbs = new BufferedImage(70,12,BufferedImage.TYPE_INT_ARGB);
                        castg = castsymbs.createGraphics();
                        castg.setColor(new Color(80,80,140));
                        casticon = new ImageIcon(castsymbs);
                        toCastButton.setIcon(casticon);
                        
                        dwnpan.add(toCastButton);
                        dwnpan.add(backButton);
                        uppan.add(Box.createHorizontalGlue());
                        midpan.add(Box.createHorizontalGlue());
                        dwnpan.add(Box.createHorizontalGlue());
                        add(uppan);
                        add(midpan);
                        add(dwnpan);
                        setCursor(handcursor);
                }
        
                public void update() {
                        for (int i=1;i<4;i++) {
                                if (numheroes<=i) casterButton[i].setEnabled(false);
                        }
                        int spelllength = hero[spellready].currentspell.length();
                        for (int i=0;i<6;i++) {
                                if (spelllength<4) {
                                        spellButton[i].setIcon(spellsymbol[i+6*spelllength]);
                                        spellButton[i].setToolTipText(SYMBOLNAMES[i+6*spelllength]);
                                }
                                else spellButton[i].setIcon(spellsymbol[i]);
                        }
                        castg.fillRect(0,0,70,12);//clear it first
                        if (spelllength>0) {
                                for (int i=0;i<spelllength;i++) {
                                        castg.drawImage(spellsymbol[Integer.parseInt(hero[spellready].currentspell.substring(i,i+1))+i*6-1].getImage(),i*15+6,0,null);
                                }
                                casticon.setImage(castsymbs);
                        }
                        //else {
                        //        castsymbs = new BufferedImage(70,12,BufferedImage.TYPE_INT_ARGB);
                        //        castg = castsymbs.createGraphics();
                        //        castg.setColor(new Color(80,80,140));
                        //        casticon.setImage(castsymbs);
                        //}
                        ((BoxLayout)uppan.getLayout()).invalidateLayout(uppan);
                        //uppan.doLayout();
                        uppan.validate();
                        this.repaint();
                }
        }
        */
        
        class Formation extends JPanel implements MouseListener {
                Cursor cursor;
                ImageIcon[] heroicons = new ImageIcon[4];
                JLabel[] herolabels = new JLabel[4];
                bool ischanging = false;
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
                        int x = 10, width; bool raised;
                        for (int i=0;i<numheroes;i++) {
                                if (!hero[i].isdead) {
                                        if (spellready==i) { width=95; raised = false; }
                                        else { width=15; raised = true; }
                                        g.setColor(presscolor);
                                        if (!raised) {
                                                g.fill3DRect(x,6,width,20,false);
                                                g.setColor(Color.black);
                                                g.setFont(dungfont);
                                                g.drawstring(hero[i].name,x+width/2-g.getFontMetrics().stringWidth(hero[i].name)/2,20);
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
                                g.drawstring(SYMBOLNAMES[i],over*20-3,30);
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

        class WeaponSheet extends JPanel implements MouseListener {
                Image backpic,hit1,hit2,hit3,miss,hitpic,uparrow,downarrow;
                Color presscolor = new Color(115,115,200);
                Color disabcolor = new Color(125,125,145);
                Color darkcolor = new Color(55,55,75);
                Color greycolor = new Color(80,80,80);
                Color hitcolor = new Color(240,170,115);
                int pressed = 0, abilitymod = 0;
                bool[] showing = new bool[3];
                string hittext;
                bool showingspecials = false;
                SpecialAbility[] abilities = new SpecialAbility[4];
                
                public WeaponSheet() {
                        setSize(154,125);
                        setPreferredSize(new Dimension(154,125));
                        setMaximumSize(new Dimension(154,125));
                        setCursor(handcursor);
                        addMouseListener(this);
                        backpic = tk.createImage("Interface"+File.separator+"weaponsheet.gif");
                        hit1 = tk.createImage("Interface"+File.separator+"wephit.gif");
                        hit2 = tk.createImage("Interface"+File.separator+"wephit2.gif");
                        hit3 = tk.createImage("Interface"+File.separator+"wephit3.gif");
                        miss = tk.createImage("Interface"+File.separator+"wepmiss.gif");
                        uparrow = tk.createImage("Icons"+File.separator+"uparrow.gif");
                        downarrow = tk.createImage("Icons"+File.separator+"downarrow.gif");
                        ImageTracker.addImage(backpic,3);
                        ImageTracker.addImage(hit1,3);
                        ImageTracker.addImage(hit2,3);
                        ImageTracker.addImage(hit3,3);
                        ImageTracker.addImage(miss,3);
                        ImageTracker.addImage(uparrow,3);
                        ImageTracker.addImage(downarrow,3);
                        try { ImageTracker.waitForID(3,1000); }
                        catch(InterruptedException ex) {}
                }

                public void paintComponent(Graphics g) {
                        if (TEXTANTIALIAS) ((Graphics2D)g).setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING,RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
                        g.drawImage(backpic,0,0,null);
                        g.setFont(dungfont14);
                     if (hero==null || hero[weaponready]==null) return;
                        if (!showingspecials) {
                                //weapon buttons for each hero
                                int x=6;
                                for (int i=0;i<numheroes;i++) {
                                        //draw button outline (fill if selected)
                                        //g.setColor(presscolor);
                                        if (weaponready==i) {
                                                g.setColor(darkcolor);
                                                g.drawRect(x-1,4,34,34);
                                                g.setColor(presscolor);
                                                g.fill3DRect(x,5,35,35,false);
                                        }
                                        else if (!hero[i].isdead) {
                                                g.setColor(disabcolor);
                                                g.drawRect(x,5,34,34);
                                                g.setColor(darkcolor);
                                                g.drawRect(x-1,4,34,34);
                                                //g.draw3DRect(x,5,33,33,true);
                                                g.setColor(greycolor);
                                                g.fillRect(x+1,6,32,32);
                                        }
                                        //draw weapon pic
                                        if (hero[i].weapon.type==Item.WEAPON && !hero[i].isdead) g.drawImage(hero[i].weapon.pic,x+1,6,this);
                                        else {
                                                Image disab = GrayFilter.createDisabledImage(hero[i].weapon.pic);
                                                ImageTracker.addImage(disab,8);
                                                try { ImageTracker.waitForID(8,1000); } catch (InterruptedException e) {}
                                                ImageTracker.removeImage(disab,8);
                                                g.drawImage(disab,x+1,6,null);
                                        }
                                        x+=36;
                                }
                                //current weapon functions (if any)
                                if (hero[weaponready].hitcounter<=0 && hero[weaponready].weapon.type==Item.WEAPON) {
                                        int y = 0;
                                        showing[0] = true; showing[1] = true; showing[2] = true;
                                        for (int i=0;i<hero[weaponready].weapon.functions;i++) {
                                                switch (hero[weaponready].weapon.function[i][1].charAt(0)) {
                                                        case 'f':
                                                                if (hero[weaponready].flevel<hero[weaponready].weapon.level[i]) showing[i] = false;
                                                                break;
                                                        case 'n':
                                                                if (hero[weaponready].nlevel<hero[weaponready].weapon.level[i]) showing[i] = false;
                                                                break;
                                                        case 'w':
                                                                if (hero[weaponready].wlevel<hero[weaponready].weapon.level[i]) showing[i] = false;
                                                                break;
                                                        case 'p':
                                                                if (hero[weaponready].plevel<hero[weaponready].weapon.level[i]) showing[i] = false;
                                                                break;
                                                }
                                                if (showing[i]) {
                                                        //draw button (fill if selected)
                                                        if (hero[weaponready].wepready) {
                                                                g.setColor(presscolor);
                                                                if (pressed-1==i) g.fill3DRect(14,y+44,126,20,false);
                                                                else g.draw3DRect(14,y+44,126,20,true);
                                                                g.setColor(Color.black);
                                                        }
                                                        else {
                                                                g.setColor(disabcolor);
                                                                g.drawRect(14,y+44,126,20);
                                                        }
                                                        //draw function name
                                                        g.drawstring(hero[weaponready].weapon.function[i][0],77-g.getFontMetrics().stringWidth(hero[weaponready].weapon.function[i][0])/2,y+59);
                                                        y+=20;
                                                }
                                        }
                                }
                                else if (hero[weaponready].hitcounter>0) {
                                        g.drawImage(hitpic,4,40,null);
                                        g.setColor(hitcolor);
                                        g.drawstring(hittext,77-g.getFontMetrics().stringWidth(hittext)/2,85);
                                }
                        }
                        else {
                                g.setColor(Color.black);
                                g.drawstring("Abilities",77-g.getFontMetrics().stringWidth("Abilities")/2,17);
                                if (hero[weaponready].abilities==null) return;
                                g.setColor(presscolor);
                                if (abilitymod!=0) {
                                        if (pressed==8) g.fill3DRect(6,6,20,20,false);
                                        else g.draw3DRect(6,6,20,20,true);
                                        g.drawImage(uparrow,10,10,null);
                                }
                                if (abilities[3]!=null) {
                                        if (pressed==9) g.fill3DRect(128,6,20,20,false);
                                        else g.draw3DRect(128,6,20,20,true);
                                        g.drawImage(downarrow,132,10,null);
                                }
                                //draw currently visible abilities
                                for (int i=0;i<4;i++) {
                                        if (abilities[i]!=null) {
                                                if (abilities[i].count<=0 && abilities[i].mana<=hero[weaponready].mana) {
                                                        g.setColor(presscolor);
                                                        if (pressed-4==i) g.fill3DRect(14,i*20+34,126,20,false);
                                                        else g.draw3DRect(14,i*20+34,126,20,true);
                                                        g.setColor(Color.black);
                                                }
                                                else {
                                                        g.setColor(disabcolor);
                                                        g.drawRect(14,i*20+34,126,20);
                                                }
                                                //draw ability name
                                                g.drawstring(abilities[i].name,77-g.getFontMetrics().stringWidth(abilities[i].name)/2,i*20+49);
                                        }
                                }
                        }
                }
                
                public void updateSpecials() {
                        for (int j=0;j<numheroes;j++) {
                                if (hero[j].abilities!=null) for (int i=0;i<hero[j].abilities.length;i++) {
                                        //decrement speed counter
                                        if (hero[j].abilities[i].count>0) hero[j].abilities[i].count--;
                                        //if current hero, update button if necessary
                                        if (showingspecials && weaponready==j) { setAbilities(); repaint(); }
                                }
                        }
                }
                
                public void toggleSpecials(int h) {
                        weaponready = h;
                        if (showingspecials) {
                                //set back to weapons and functions
                                showingspecials = false;
                                repaint();
                                return;
                        }
                        abilitymod = 0;
                        if (hero[h].abilities!=null) setAbilities();
                        showingspecials = true;
                        repaint();
                }
                
                private void setAbilities() {
                        //show specials, so figure out what abilities to show (store pointers in array)
                        Hero h = hero[weaponready];
                        int i = abilitymod, count = 0;
                        bool found;
                        while (count<4) {
                                found = false;
                                while (!found && i<h.abilities.length) {
                                        if (h.abilities[i].flevelneed<=h.flevel && h.abilities[i].nlevelneed<=h.nlevel && h.abilities[i].wlevelneed<=h.wlevel && h.abilities[i].plevelneed<=h.plevel) {
                                                abilities[count] = h.abilities[i];
                                                found = true;
                                        }
                                        i++;
                                }
                                if (!found) abilities[count] = null;
                                count++;
                        }
                        if (abilities[2]==null && abilitymod>0) { abilitymod--; setAbilities(); }
                }
                
                public void mousePressed(MouseEvent e) {
                        //get x,y to determine button
                        int x = e.getX(), y = e.getY();
                        if (!showingspecials) {
                                //weapon buttons
                                if (y<39 && y>4 && x>5 && x<148) {
                                        int who=-1;
                                        if (x<40) who=0;
                                        else if (x<76 && x>41) who=1;
                                        else if (x<112 && x>77) who=2;
                                        else if (x>113) who=3;
                                        if (who>=0 && who<numheroes && !hero[who].isdead) weaponready = who;
                                        //switch from weapon stuff to special abilities
                                        //if (SwingUtilities.isRightMouseButton(e)) toggleSpecials(weaponready);
                                     if ((e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) toggleSpecials(weaponready);
                                }
                                //switch from weapon stuff to special abilities
                                //else if (SwingUtilities.isRightMouseButton(e)) toggleSpecials(weaponready);
                             else if ((e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) toggleSpecials(weaponready);
                                //weapon functions
                                else if (hero[weaponready].wepready && y>43 && y<105 && x>13 && x<151) {
                                        int f = (y-44)/20;
                                        while (f<3 && !showing[f]) { f++; }
                                        if (f<hero[weaponready].weapon.functions && showing[f]) pressed = (y-44)/20+1;
                                        //System.out.println("-----------------------");
                                        //System.out.println("f = "+f+", pressed = "+pressed);
                                }
                                else return;
                        }
                        //switch from special abilities to weapon stuff
                        //else if (SwingUtilities.isRightMouseButton(e)) toggleSpecials(weaponready);
                     else if ((e.isPopupTrigger() || e.getButton() != MouseEvent.BUTTON1)) toggleSpecials(weaponready);
                        //special abilities
                        else if (hero[weaponready].abilities!=null) {
                                //up/down buttons
                                if (y>5 && y<27) {
                                        if (x<27 && x>5) pressed = 8; //up
                                        else if (x>127 && x<149) pressed = 9; //down
                                }
                                //do ability buttons
                                else if (y>33 && x>13 && x<151) {
                                        int f = (y-34)/20;
                                        if (abilities[f]!=null && abilities[f].count<=0 && abilities[f].mana<=hero[weaponready].mana) pressed = f+4;
                                }
                        }
                        repaint();
                }
                
                public void mouseReleased(MouseEvent e) {
                        if (pressed==0) return;
                        else if (pressed<4) {
                                //use weapon function
                                //System.out.println("clicked button "+(pressed-1));
                                int f = pressed-1;
                                while (f<3 && !showing[f]) { f++; }
                                //System.out.println("clicked function "+f);
                                weaponqueue.add(""+weaponready+f+hero[weaponready].weapon.number);
                                hero[weaponready].wepready = false;
                                hero[weaponready].weaponcount = 5;
                        }
                        else if (pressed<8) {
                                //special ability
                                int i = pressed-4;
                                //System.out.println(hero[weaponready].name+"'s ability "+abilities[i].name);

                                if (!hero[weaponready].doAbility(abilities[i].getActionName(),abilities[i].power,abilities[i].classgain,abilities[i].data)) {} //do nothing if action name not found
                                else {
                                        //play sound if not null
                                        if (abilities[i].sound!=null && !abilities[i].sound.equals("")) playSound(abilities[i].sound,-1,-1,0);
                                        //decrease mana if supposed to
                                        if (abilities[i].mana>0) {
                                                hero[weaponready].mana-=abilities[i].mana;
                                        }
                                        //set count to speed (delay before can use again)
                                        if (abilities[i].speed>0) {
                                                abilities[i].count = abilities[i].speed;
                                        }
                                        hupdate();
                                        if (sheet) herosheet.repaint();
                                        needredraw = true;
                                }
                        }
                        else if (pressed==8) {
                                if (abilitymod>0) {
                                        abilitymod--;
                                        setAbilities();
                                }
                        }
                        else if (pressed==9 && abilitymod<hero[weaponready].abilities.length-1) {
                                abilitymod++;
                                setAbilities();
                        }
                        pressed = 0;
                        repaint();
                }

                public void mouseExited(MouseEvent e) {
                        if (pressed>0) {
                                pressed = 0;
                                repaint();
                        }
                }
                
                public void mouseClicked(MouseEvent e) {}
                public void mouseEntered(MouseEvent e) {}
        }

        class ArrowSheet extends JPanel implements MouseListener {
                Image unpressed,moveforward,moveback,moveleft,moveright,turnleft,turnright;
                int butpressed, presscount = 0;
                bool pressed = false;
                
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

        class Message extends JPanel {//JComponent {//Canvas {
                Color[] messagecolor = new Color[4];
                string currentmessage[] = { " "," "," ","Welcome" };
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

                public void setMessage(string m,int c) {
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
                        g.drawstring(currentmessage[0],2,18);
                        g.drawstring(currentmessage[1],2,34);
                        g.drawstring(currentmessage[2],2,50);
                        g.drawstring(currentmessage[3],2,66);
                        g.setColor(messagecolor[0]);
                        g.drawstring(currentmessage[0],0,16);
                        g.setColor(messagecolor[1]);
                        g.drawstring(currentmessage[1],0,32);
                        g.setColor(messagecolor[2]);
                        g.drawstring(currentmessage[2],0,48);
                        g.setColor(messagecolor[3]);
                        g.drawstring(currentmessage[3],0,64);
                }
        }

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
                                g.drawstring(inhand.name,39,28);
                                g.setColor(Color.yellow);
                                g.drawstring(inhand.name,37,25);
                        }
                }
        }
        
        class Projectile {
                Item it = null;
                Spell sp = null;
                Image pic;
                int pow,dist,direction,level,x,y,subsquare,powcount,powdrain;
                bool justthrown = true;
                bool needsfirstdraw = true;
                bool isending = false;
                bool notelnext = false;
                bool hitsImmaterial = false;
                bool passgrate = false; //determines if proj passes thru a grate

                //for loading:
                public Projectile(Item i,int lvl,int x,int y,int d,int dr,int subs,bool jt,bool ntn) {
                        it = i;
                        this.level = lvl;
                        this.x = x;
                        this.y = y;
                        dist = d;
                        direction = dr;
                        subsquare = subs;
                        justthrown = jt;
                        notelnext = ntn;
                        pow = it.throwpow + it.shotpow;
                        if (it.hasthrowpic) {
                                //get relative direction and facing
                                int s = 2; //left
                                if (facing==direction) { s=0; } //away
                                else if ((facing-direction)%2==0) { s=1; }//towards
                                else if (facing==(direction+1)%4) { s=3; } //right = dpic
                                pic = it.throwpic[s];
                                //if (s<3) pic = it.throwpic[s];
                                //else pic = it.dpic;
                        }
                        else pic = it.dpic;
                        if (it.hitsImmaterial) hitsImmaterial = true;
                        //if (it.size<2) passgrate = true;
                        if (it.size==0 || it.size==2) passgrate = true;
                }
                public Projectile(Spell s,int lvl,int x,int y,int d,int dr,int subs,int pd,int pc,bool jt,bool ntn) {
                        sp = s;
                        this.level = lvl;
                        this.x = x;
                        this.y = y;
                        dist = d;
                        direction = dr;
                        subsquare = subs;
                        powdrain = pd;
                        powcount = pc;
                        justthrown = jt;
                        notelnext = ntn;
                        pow = sp.power;
                        pic = sp.pic;
                        if (sp.hitsImmaterial) hitsImmaterial = true;
                        if (sp.number==51 || sp.number==52 || sp.number==335 || sp.number==31) passgrate=true;
                }
                
                public Projectile(Item i,int d,int dr,int subs) {
                        this(i,dmnew.level,partyx,partyy,d,dr,subs);
                }
                public Projectile(Item i,int x,int y,int d,int dr,int subs) {
                        this(i,dmnew.level,x,y,d,dr,subs);
                }
                public Projectile(Item i,int lvl,int x,int y,int d,int dr,int subs) {
                        it = i;
                        pow = it.throwpow + it.shotpow;
                        dist = d;
                        direction = dr;
                        subsquare = (subs-facing+4)%4; //if (subsquare<0) subsquare*=-1;
                        //System.out.println("sub+fac = "+subs+", sub = "+subsquare);
                        if (it.hasthrowpic) {
                                //get relative direction and facing
                                int s = 2; //left
                                if (facing==direction) { s=0; } //away
                                else if ((facing-direction)%2==0) { s=1; }//towards
                                else if (facing==(direction+1)%4) { s=3; } //right = dpic
                                pic = it.throwpic[s];
                                //if (s<3) pic = it.throwpic[s];
                                //else pic = it.dpic;
                        }
                        else pic = it.dpic;
                        //pic = it.dpic;
                        this.level = lvl;
                        this.x = x;
                        this.y = y;
                        dmprojs.add(this);
                        DungeonMap[level][x][y].numProjs++;
                        if (it.hitsImmaterial) hitsImmaterial = true;
                        //if (it.size<2) passgrate = true;
                        if (it.size==0 || it.size==2) passgrate = true;
                }
                public Projectile(Spell s,int d,int dr,int subs) {
                        this(s,dmnew.level,partyx,partyy,d,dr,subs);
                }
                public Projectile(Spell s,int x,int y,int d,int dr,int subs) {
                        this(s,dmnew.level,x,y,d,dr,subs);
                }
                public Projectile(Spell s,int lvl,int x,int y,int d,int dr,int subs) {
                        sp = s;
                        pic = sp.pic;
                        pow = sp.power;
                        dist = d;
                        direction = dr;
                        this.level = lvl;
                        this.x = x;
                        this.y = y;
                        powcount = 0;
                        powdrain = dist/sp.gain+1;//tells what value powcount must reach before a power loss
                        subsquare = (subs-facing+4)%4; //if (subsquare<0) subsquare*=-1;
                        //System.out.println("sub+fac = "+subs+", sub = "+subsquare);
                        dmprojs.add(this);
                        DungeonMap[level][x][y].numProjs++;
                        if (sp.hitsImmaterial) hitsImmaterial = true;
                        if (sp.number==51 || sp.number==52 || sp.number==335 || sp.number==31) passgrate=true;
                }

                public void update() {
                        if (dist==99) dist=100; //for projs that don't stop/fade
                        else if (sp!=null && sp.type!=0) powcount++;
                        bool moveproj = true;//, hitparty = false, hitmons = false;
                        //int newsub, xadjust = x, yadjust = y;
                        int newsub,xadjust,yadjust;
                        if (sp!=null && sp.number==642) {
                                int dir = direction;
                                ArrayList dirchange = new ArrayList(4);
                                for (int i=0;i<4;i++) if (i!=direction) dirchange.add(""+i);
                                do {
                                        xadjust=x; yadjust=y; direction = dir;
                                        if (direction==NORTH || direction==SOUTH) {
                                                if (subsquare==0) newsub = 3;
                                                else if (subsquare==3) newsub = 0;
                                                else if (subsquare==1) newsub = 2;
                                                else newsub = 1;
                                                if (direction==NORTH && (newsub==3 || newsub==2)) yadjust--;
                                                else if (direction==SOUTH && (newsub==0 || newsub==1)) yadjust++;
                                                if (randGen.nextbool()) {
                                                        if (dirchange.contains("1") && x>0 && !(DungeonMap[level][x-1][y] instanceof Wall) && DungeonMap[level][x-1][y].mapchar!='2') { dir = 1; dirchange.remove("1"); }
                                                        else if (dirchange.contains("3")) { dir = 3; dirchange.remove("3"); }
                                                        else { dir = 2-direction; dirchange.remove(0); }
                                                }
                                                else {
                                                        if (dirchange.contains("3") && x<mapwidth-1 && !(DungeonMap[level][x+1][y] instanceof Wall) && DungeonMap[level][x+1][y].mapchar!='2') { dir = 3; dirchange.remove("3"); }
                                                        else if (dirchange.contains("1")) { dir = 1; dirchange.remove("1"); }
                                                        else { dir = 2-direction; dirchange.remove(0); }
                                                }
                                        }
                                        else {
                                                if (subsquare==0) newsub = 1;
                                                else if (subsquare==1) newsub = 0;
                                                else if (subsquare==2) newsub = 3;
                                                else newsub = 2;
                                                if (direction==EAST && (newsub==0 || newsub==3)) xadjust++;
                                                else if (direction==WEST && (newsub==1 || newsub==2)) xadjust--;
                                                if (randGen.nextbool()) {
                                                        if (dirchange.contains("0") && y>0 && !(DungeonMap[level][x][y-1] instanceof Wall) && DungeonMap[level][x][y-1].mapchar!='2') { dir = 0; dirchange.remove("0"); }
                                                        else if (dirchange.contains("2")) { dir = 2; dirchange.remove("2"); }
                                                        else { dir = (direction+2)%4; dirchange.remove(0); }
                                                }
                                                else {
                                                        if (dirchange.contains("2") && y<mapheight-1 && !(DungeonMap[level][x][y+1] instanceof Wall) && DungeonMap[level][x][y+1].mapchar!='2') { dir = 2; dirchange.remove("2"); }
                                                        else if (dirchange.contains("0")) { dir = 0; dirchange.remove("0"); }
                                                        else { dir = (direction+2)%4; dirchange.remove(0); }
                                                }
                                        }
                                }
                                while ((xadjust<0 && direction==1) || (yadjust<0 && direction==0) || (xadjust==mapwidth && direction==3) || (yadjust==mapheight && direction==2) || ((DungeonMap[level][xadjust][yadjust] instanceof Wall) && DungeonMap[level][xadjust][yadjust].mapchar!='2' && !dirchange.isEmpty()));
                        }
                        else {
                                xadjust=x; yadjust=y;
                                if (direction==NORTH || direction==SOUTH) {
                                        if (subsquare==0) newsub = 3;
                                        else if (subsquare==3) newsub = 0;
                                        else if (subsquare==1) newsub = 2;
                                        else newsub = 1;
                                        if (direction==NORTH && (newsub==3 || newsub==2)) yadjust--;
                                        else if (direction==SOUTH && (newsub==0 || newsub==1)) yadjust++;
                                }
                                else {
                                        if (subsquare==0) newsub = 1;
                                        else if (subsquare==1) newsub = 0;
                                        else if (subsquare==2) newsub = 3;
                                        else newsub = 2;
                                        if (direction==EAST && (newsub==0 || newsub==3)) xadjust++;
                                        else if (direction==WEST && (newsub==1 || newsub==2)) xadjust--;
                                }
                        }
                        
                        if (dist==0 || xadjust<0 || yadjust<0 || xadjust==mapwidth || yadjust==mapheight) {
                           moveproj=false;
                        }
                        else if (!DungeonMap[level][xadjust][yadjust].canPassProjs && DungeonMap[level][xadjust][yadjust].mapchar!='d' && (DungeonMap[level][xadjust][yadjust].mapchar!='>' || ((Stairs)DungeonMap[level][xadjust][yadjust]).side!=direction)) {
                           moveproj=false;
                        }
                        else if (!justthrown && DungeonMap[level][x][y].mapchar=='>') {//DungeonMap[level][x][y] instanceof Stairs) {
                           moveproj = false;
                        }
                        else if (!justthrown && level==dmnew.level && x==partyx && y==partyy && !alldead && heroatsub[(subsquare+facing)%4]!=-1) { 
                           moveproj=false;
                        }
                        else if (!justthrown && DungeonMap[level][x][y].mapchar=='d' && ((sp!=null && sp.number==6) || ((Door)DungeonMap[level][x][y]).changecount>1 && (!passgrate || ((Door)DungeonMap[level][x][y]).pictype!=Door.GRATE))) {
                           moveproj = false;
                        }
                        else if (!justthrown && DungeonMap[level][x][y].hasMons) {
                                Monster tempmon = (Monster)dmmons.get(level+","+x+","+y+","+subsquare);
                                if (tempmon==null) tempmon = (Monster)dmmons.get(level+","+x+","+y+","+5);
                                if (tempmon!=null && !tempmon.isdying && (hitsImmaterial || !tempmon.isImmaterial)) { 
                                        moveproj=false;
                                }
                        }
                        
                        if (moveproj) {
                           DungeonMap[level][x][y].numProjs--;
                           dist--;
                           if (powcount>powdrain) {
                                powcount=0;
                                sp.gain--;
                                sp.power = sp.powers[sp.gain-1];
                           }
                           x = xadjust; y = yadjust; subsquare = newsub;
                           DungeonMap[level][x][y].numProjs++;
                           int xdist,ydist;
                           if (level == dmnew.level) {
                             xdist=partyx-x; if (xdist<0) xdist*=-1;
                             ydist=partyy-y; if (ydist<0) ydist*=-1;
                             if (xdist<5 && ydist<5) needredraw = true;
                           }
                           if (!notelnext) {
                                DungeonMap[level][x][y].tryTeleport(this);
                                if (level == dmnew.level) {
                                     xdist=partyx-x; if (xdist<0) xdist*=-1;
                                     ydist=partyy-y; if (ydist<0) ydist*=-1;
                                     if (xdist<5 && ydist<5) needredraw = true;
                                }
                           }
                           else notelnext = false;
                        }
                        else projend(); 
                        justthrown = false;
                }//end update
                
                public void projend() {
                        if (it!=null) { //item proj, not spell
                           isending = true;
                           bool shoulddrop = true;
                           //hit party? 
                           if (!justthrown && DungeonMap[level][x][y].hasParty) {
                                int hit = heroatsub[(subsquare+facing)%4];
                                if (hit!=-1) {
                                        playSound("oof.wav",-1,-1);
                                        hero[hit].damage(pow,PROJWEAPONHIT);
                                        if (it.poisonous>0 && randGen.nextbool()) { hero[hit].poison+=it.poisonous; hero[hit].ispoisoned=true; }
                                }
                           }
                           //hit mons?
                           else if (!justthrown && DungeonMap[level][x][y].hasMons) {
                                 Monster tempmon = (Monster)dmmons.get(level+","+x+","+y+","+subsquare);
                                 if (tempmon==null) tempmon = (Monster)dmmons.get(level+","+x+","+y+","+5);
                                 if (tempmon!=null) {
                                        if ((hitsImmaterial || !tempmon.isImmaterial) && (tempmon.hurtitem==0 || it.number==215 || it.number==tempmon.hurtitem)) {
                                                tempmon.damage(pow,PROJWEAPONHIT);
                                                if (!tempmon.isImmaterial && it.poisonous>0 && randGen.nextbool()) { tempmon.poisonpow+=it.poisonous; tempmon.ispoisoned=true;}
                                                //chance of sticking in mon (not heavily armored ones like stone golems, though)
                                                if (!tempmon.isImmaterial && !tempmon.isdying && tempmon.defense<80 && it.projtype>0 && it.number!=266 && randGen.nextInt(10)==0) { it.shotpow = 0; tempmon.carrying.add(it); shoulddrop = false; }
                                        }
                                 }
                           }
                           
                           if (shoulddrop) {
                                   //item drops to ground
                                   if (level==dmnew.level) {
                                        if (it.weight>4.0) playSound("thunk.wav",x,y);
                                        else if (it.type==Item.WEAPON) playSound("dink.wav",x,y);
                                   }
                                   it.shotpow = 0;
                                   it.subsquare=subsquare;
        
                                   if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                        DungeonMap[level][x][y].addItem(it);
                                        DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                                   }
                           }
                           DungeonMap[level][x][y].numProjs--;
                           if (level == dmnew.level) {
                             int xdist=partyx-x; if (xdist<0) xdist*=-1;
                             int ydist=partyy-y; if (ydist<0) ydist*=-1;
                             if (xdist<5 && ydist<5) needredraw = true;
                           }
                        }
                        else { //spell
                           //System.out.println("proj ending at "+level+","+x+","+y);
                           isending=true;
                           if (level==dmnew.level) {
                                if (sp.number==44 || sp.number==46 || sp.number==335 || sp.number==642 || sp.number==365) playSound("fball.wav",x,y);
                                else playSound("zap.wav",x,y);
                           }
                           
                           //hit party?
                           if (!hitsImmaterial && DungeonMap[level][x][y].hasParty) {
                                if (sp.ismultiple) {
                                   if (sp.number==362) { slowcount+=sp.power*3; message.setMessage("Party Slowed.",4); }
                                   for (int i=0;i<numheroes;i++) {
                                       if (sp.number==362) {
                                          //slow
                                          if (hero[i].dexterityboost>-sp.power) {
                                                  hero[i].dexterity-=hero[i].dexterityboost;
                                                  hero[i].dexterityboost-=sp.power;
                                                  if (hero[i].dexterityboost<-sp.power) hero[i].dexterityboost=-sp.power;
                                                  if (hero[i].dexterity+hero[i].dexterityboost<=0) hero[i].dexterityboost=1-hero[i].dexterity;
                                                  hero[i].dexterity+=hero[i].dexterityboost;
                                          }
                                          hero[i].damage(1,SPELLHIT);
                                       }
                                       else if (sp.number==523) {
                                          //silence
                                          if (hero[i].silencecount<sp.power) {
                                                  int tester = (hero[i].intelligence+hero[i].wisdom)/2;
                                                  if (randGen.nextInt(sp.gain*20)+30>tester || (tester>=100 && randGen.nextInt(5)==0)) {
                                                        if (!hero[i].silenced) message.setMessage(hero[i].name+" is silenced!",i);
                                                        hero[i].silenced = true;
                                                        hero[i].silencecount+=sp.power;
                                                        if (hero[i].silencecount>sp.power) hero[i].silencecount=sp.power;
                                                  }
                                          }
                                          hero[i].damage(1,SPELLHIT);
                                       }
                                       else if (sp.number==44 && hero[i].weapon.number==244 && hero[i].weapon.functions==1) {
                                          //dragon spit -> holder takes no pain from fireball and spit recharges
                                          hero[i].weapon.charges[1]=1;
                                          hero[i].weapon.power[1]=sp.gain;
                                          //if (hero[i].weapon.functions==1) {
                                                hero[i].weapon.functions=2;
                                                weaponsheet.repaint();
                                                hero[i].repaint();
                                          //}
                                          //hero[i].damage(pow/2,SPELLHIT);
                                       }
                                       else if (i==heroatsub[(subsquare+facing)%4]) hero[i].damage(pow,SPELLHIT);
                                       else hero[i].damage(pow*2/3,SPELLHIT);
                                   }
                                }
                                else if (!justthrown && sp.number!=6) {
                                        int hit = heroatsub[(subsquare+facing)%4];
                                        if (hit!=-1) {
                                                if (sp.number==51) { 
                                                        hero[hit].poison+=(sp.gain*2+1);
                                                        hero[hit].ispoisoned=true;
                                                        hero[hit].damage(pow,SPELLHIT);
                                                }
                                                else if (sp.number==461) {
                                                        //weakness
                                                        if (hero[hit].strengthboost>-sp.power) {
                                                                hero[hit].strength-=hero[hit].strengthboost;
                                                                hero[hit].strengthboost-=sp.power;
                                                                if (hero[hit].strengthboost<-sp.power) hero[hit].strengthboost=-sp.power;
                                                                if (hero[hit].strength+hero[hit].strengthboost<=0) hero[hit].strengthboost=1-hero[hit].strength;
                                                                hero[hit].strength+=hero[hit].strengthboost;
                                                                hero[hit].setMaxLoad();
                                                        }
                                                        if (hero[hit].vitalityboost>-sp.power) {
                                                                hero[hit].vitality-=hero[hit].vitalityboost;
                                                                hero[hit].vitalityboost-=sp.power;
                                                                if (hero[hit].vitalityboost<-sp.power) hero[hit].vitalityboost=-sp.power;
                                                                if (hero[hit].vitality+hero[hit].vitalityboost<=0) hero[hit].vitalityboost=1-hero[hit].vitality;
                                                                hero[hit].vitality+=hero[hit].vitalityboost;
                                                        }
                                                        hero[hit].damage(1,SPELLHIT);
                                                }
                                                else if (sp.number==363) {
                                                        //feeble mind
                                                        if (hero[hit].intelligenceboost>-sp.power) {
                                                                hero[hit].intelligence-=hero[hit].intelligenceboost;
                                                                hero[hit].intelligenceboost-=sp.power;
                                                                if (hero[hit].intelligenceboost<-sp.power) hero[hit].intelligenceboost=-sp.power;
                                                                if (hero[hit].intelligence+hero[hit].intelligenceboost<=0) hero[hit].intelligenceboost=1-hero[hit].intelligence;
                                                                hero[hit].intelligence+=hero[hit].intelligenceboost;
                                                        }
                                                        if (hero[hit].wisdomboost>-sp.power) {
                                                                hero[hit].wisdom-=hero[hit].wisdomboost;
                                                                hero[hit].wisdomboost-=sp.power;
                                                                if (hero[hit].wisdomboost<-sp.power) hero[hit].wisdomboost=-sp.power;
                                                                if (hero[hit].wisdom+hero[hit].wisdomboost<=0) hero[hit].wisdomboost=1-hero[hit].wisdom;
                                                                hero[hit].wisdom+=hero[hit].wisdomboost;
                                                        }
                                                        hero[hit].damage(1,SPELLHIT);
                                                }
                                                else if (sp.number==664) {
                                                        //strip defenses
                                                        if (hero[hit].defenseboost>-sp.power) {
                                                                hero[hit].defense-=hero[hit].defenseboost;
                                                                hero[hit].defenseboost-=sp.power;
                                                                if (hero[hit].defenseboost<-sp.power) hero[hit].defenseboost=-sp.power;
                                                                hero[hit].defense+=hero[hit].defenseboost;
                                                        }
                                                        if (hero[hit].magicresistboost>-sp.power) {
                                                                hero[hit].magicresist-=hero[hit].magicresistboost;
                                                                hero[hit].magicresistboost-=sp.power;
                                                                if (hero[hit].magicresistboost<-sp.power) hero[hit].magicresistboost=-sp.power;
                                                                hero[hit].magicresist+=hero[hit].magicresistboost;
                                                        }
                                                        hero[hit].damage(1,SPELLHIT);
                                                }
                                                else hero[hit].damage(pow,SPELLHIT);
                                        }
                                }
                           }
                           //hit mon?
                           else if (DungeonMap[level][x][y].hasMons) { 
                                if (sp.ismultiple) {
                                   Monster tempmon;
                                   for (int i=0;i<6;) {
                                        tempmon = (Monster)dmmons.get(level+","+x+","+y+","+i);
                                        if (tempmon!=null && (hitsImmaterial==tempmon.isImmaterial)) {
                                                if (sp.number==362) {
                                                   //slow
                                                   if (tempmon.speedboost>-sp.power) {
                                                           tempmon.speed-=tempmon.speedboost;
                                                           tempmon.speedboost-=sp.power;
                                                           if (tempmon.speedboost<-sp.power) tempmon.speedboost=-sp.power;
                                                           if (tempmon.speed+tempmon.speedboost<=0) tempmon.speedboost=1-tempmon.speed;
                                                           tempmon.speed+=tempmon.speedboost;
                                                   }
                                                   
                                                   tempmon.movespeed-=tempmon.movespeedboost;
                                                   tempmon.movespeedboost+=sp.gain*3;
                                                   if (tempmon.movespeedboost>sp.gain*3) tempmon.movespeedboost=sp.gain*3;
                                                   tempmon.movespeed+=tempmon.movespeedboost;
                                                   
                                                   /*
                                                   tempmon.attackspeed-=tempmon.attackspeedboost;
                                                   tempmon.attackspeedboost-=sp.gain*2;
                                                   if (tempmon.attackspeedboost<-sp.gain*2) tempmon.attackspeedboost=-sp.gain*2;
                                                   tempmon.attackspeed+=tempmon.attackspeedboost;
                                                   */
                                                }
                                                else if (sp.number==523) {
                                                   //silence
                                                   if (tempmon.number!=26 && tempmon.hasmagic && tempmon.silencecount<sp.power && (randGen.nextInt(sp.gain*20)+30>tempmon.manapower || (tempmon.manapower>=100 && randGen.nextInt(5)==0))) {
                                                         tempmon.silenced = true;
                                                         tempmon.silencecount+=sp.power;
                                                         if (tempmon.silencecount>sp.power) tempmon.silencecount=sp.power;
                                                   }
                                                }
                                                else if (i==subsquare || i==5) tempmon.damage(pow,SPELLHIT);
                                                else tempmon.damage(pow*2/3,SPELLHIT);
                                        }
                                        if (i==3) i=5;
                                        else i++;
                                   }
                                }
                                else if (!justthrown && sp.number!=6) {
                                   Monster tempmon = (Monster)dmmons.get(level+","+x+","+y+","+subsquare);
                                   if (tempmon==null) tempmon = (Monster)dmmons.get(level+","+x+","+y+","+5);
                                   if (tempmon!=null && (hitsImmaterial==tempmon.isImmaterial || (sp.number==0 && tempmon.number==26))) {
                                        //test to see if poison spell
                                        if (sp.number==51 && !tempmon.isImmaterial && tempmon.number!=26) { tempmon.damage(pow,SPELLHIT); tempmon.poisonpow+=(sp.gain*2+1); tempmon.ispoisoned=true; }
                                        else if (sp.number==0 && tempmon.number==26) {
                                                //check for fluxcages, kill if surrounded
                                                //should i be testing party xy? or can chaos escape by teleporting thru party?
                                                bool surrounded = true;
                                                //put seal test here
                                                //if (DungeonMap[level][tempmon.x][tempmon.y-1].mapchar!='M') surrounded=false;
                                                if (DungeonMap[level][tempmon.x][tempmon.y].mapchar!='F' || ((FDecoration)DungeonMap[level][tempmon.x][tempmon.y]).number!=2) surrounded=false;
                                                else if (fluxcages.get(level+","+tempmon.x+","+tempmon.y)==null) surrounded=false;
                                                /*party does count*/
                                                else if ((!(DungeonMap[level][tempmon.x][tempmon.y-1] instanceof Wall) || DungeonMap[level][tempmon.x][tempmon.y-1].mapchar=='2') && (partyx!=tempmon.x || partyy!=tempmon.y-1) && fluxcages.get(level+","+tempmon.x+","+(tempmon.y-1))==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x][tempmon.y+1] instanceof Wall) || DungeonMap[level][tempmon.x][tempmon.y+1].mapchar=='2') && (partyx!=tempmon.x || partyy!=tempmon.y+1) && fluxcages.get(level+","+tempmon.x+","+(tempmon.y+1))==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x-1][tempmon.y] instanceof Wall) || DungeonMap[level][tempmon.x-1][tempmon.y].mapchar=='2') && (partyx!=tempmon.x-1 || partyy!=tempmon.y) && fluxcages.get(level+","+(tempmon.x-1)+","+tempmon.y)==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x+1][tempmon.y] instanceof Wall) || DungeonMap[level][tempmon.x+1][tempmon.y].mapchar=='2') && (partyx!=tempmon.x+1 || partyy!=tempmon.y) && fluxcages.get(level+","+(tempmon.x+1)+","+tempmon.y)==null) surrounded=false;
                                                /*party doesn't count
                                                else if ((!(DungeonMap[level][tempmon.x][tempmon.y-1] instanceof Wall) || DungeonMap[level][tempmon.x][tempmon.y-1].mapchar=='2') && fluxcages.get(level+","+tempmon.x+","+(tempmon.y-1))==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x][tempmon.y+1] instanceof Wall) || DungeonMap[level][tempmon.x][tempmon.y+1].mapchar=='2') && fluxcages.get(level+","+tempmon.x+","+(tempmon.y+1))==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x-1][tempmon.y] instanceof Wall) || DungeonMap[level][tempmon.x-1][tempmon.y].mapchar=='2') && fluxcages.get(level+","+(tempmon.x-1)+","+tempmon.y)==null) surrounded=false;
                                                else if ((!(DungeonMap[level][tempmon.x+1][tempmon.y] instanceof Wall) || DungeonMap[level][tempmon.x+1][tempmon.y].mapchar=='2') && fluxcages.get(level+","+(tempmon.x+1)+","+tempmon.y)==null) surrounded=false;
                                                */
                                                if (surrounded) tempmon.damage(tempmon.maxhealth,FUSEHIT);
                                                else tempmon.teleport();
                                        }
                                        else if (sp.number==461) {
                                                //weakness
                                                if (tempmon.powerboost>-sp.power) {
                                                        tempmon.power-=tempmon.powerboost;
                                                        tempmon.powerboost-=sp.power;
                                                        if (tempmon.powerboost<-sp.power) tempmon.powerboost=-sp.power;
                                                        if (tempmon.power+tempmon.powerboost<=0) tempmon.powerboost=1-tempmon.power;
                                                        tempmon.power+=tempmon.powerboost;
                                                }
                                        }
                                        else if (sp.number==363) {
                                                //feeble mind
                                                if (tempmon.manapowerboost>-sp.power) {
                                                        tempmon.manapower-=tempmon.manapowerboost;
                                                        tempmon.manapowerboost-=sp.power;
                                                        if (tempmon.manapowerboost<-sp.power) tempmon.manapowerboost=-sp.power;
                                                        if (tempmon.manapower+tempmon.manapowerboost<=0) tempmon.manapowerboost=1-tempmon.manapower;
                                                        tempmon.manapower+=tempmon.manapowerboost;
                                                }
                                        }
                                        else if (sp.number==664) {
                                                //strip defenses
                                                if (tempmon.defenseboost>-sp.power) {
                                                        tempmon.defense-=tempmon.defenseboost;
                                                        tempmon.defenseboost-=sp.power;
                                                        if (tempmon.defenseboost<-sp.power) tempmon.defenseboost=-sp.power;
                                                        tempmon.defense+=tempmon.defenseboost;
                                                }
                                                if (tempmon.magicresistboost>-sp.power) {
                                                        tempmon.magicresist-=tempmon.magicresistboost;
                                                        tempmon.magicresistboost-=sp.power;
                                                        if (tempmon.magicresistboost<-sp.power) tempmon.magicresistboost=-sp.power;
                                                        tempmon.magicresist+=tempmon.magicresistboost;
                                                }
                                        }
                                        else tempmon.damage(pow,SPELLHIT);
                                   }
                                }
                           }
                           //open door spell?
                           else if ((sp.number==6) && (DungeonMap[level][x][y] instanceof Door)) {
                                Door tempdoor = (Door)DungeonMap[level][x][y];
                                if (tempdoor.opentype==Door.BUTTON) {
                                        if (tempdoor.isclosing) tempdoor.activate();
                                        else tempdoor.deactivate();
                                }
                           }
                           //fireball/ful bomb hitting door?
                           else if ((sp.number==44 || sp.number==46) && (DungeonMap[level][x][y] instanceof Door)) {
                                ((Door)DungeonMap[level][x][y]).breakDoor(pow,false,false);
                           }
                           if (sp.number==31 || sp.number==61) { PoisonCloud cloud = new PoisonCloud(level,x,y,sp.gain); }
                           if (level == dmnew.level) {
                             int xdist=partyx-x; if (xdist<0) xdist*=-1;
                             int ydist=partyy-y; if (ydist<0) ydist*=-1;
                             if (xdist<5 && ydist<5) needredraw = true;
                           }
                        }
                }//end projend
                
        }
        
        class Monster {
                int number;
                string name;
                int power;
                int defense;     //% of weapon damage resisted
                int magicresist; //% of magic damage resisted
                int speed;       //affects chance of missing/dodging
                int manapower;   //intelligence & wisdom merged into 1 attribute
                int movespeed;   //rate of movement
                int attackspeed; //rate of attack (kind of...) must be < movespeed
                int mana;
                int maxmana = 0;
                int health;
                int maxhealth;
                int fearresist = 5; //int affecting success rate of horn of fear (0-never works to 10-always works)
                bool wasfrightened = false; //switches mon's ai to run
                int facing = NORTH;
                int subsquare = 0;
                int currentai = 0;
                int defaultai = 1;
                int newai;
                int powerboost,defenseboost,magicresistboost,speedboost,manapowerboost,movespeedboost,attackspeedboost;
                int parry = -1;
                Image towardspic;
                Image awaypic;
                Image rightpic;
                Image leftpic;
                Image attackpic;
                Image castpic;
                bool hasmagic = false;
                bool hasheal = false; //does mon have heal magic?
                bool hasdrain = false; //does mon have drain life magic?
                bool silenced = false;
                int silencecount = 0;
                int castpower; //max power level of cast spells (can be 1-6), or shotpower for item projs
                int minproj = 0; //min mana needed to cast offensive magic
                bool ignoremons = true; //will this mon shoot other mons if they are in the way?
                int numspells;
                string[] knownspells;
                int poison = 0; //strength of poison
                bool isImmaterial = false; //not ghost
                bool isflying = false; //if true, can fly over pits
                bool canusestairs = true; //true if this mon can go up/down stairs (dragons too big, screamers cant climb, etc.)
                bool canteleport = false; //true if mon can teleport like a sorcerer/vexirk
                int timecounter = 0;
                int movecounter = 0;
                int deathcounter = 0;
                int randomcounter = 0; //for trying to get around obstacle
                int runcounter = 0; //for hit&run ai, including gigglers
                int x,y,xdist=0,ydist=0,level;
                bool isattacking = false; //used in drawMonster
                bool iscasting = false;   //"
                bool isdying = false;     //"
                bool mirrored = false;
                bool hurt = false; //used in ai routine
                bool wasstuck; //used in ai routine for stayback - gotowards swaps
                int moveattack = 0; //used in ai routine to prevent double ai update when moving & attacking at once
                int hurttest = 0; //used in ai routine to delay a successive run when injured test
                bool waitattack = false; //used in ai for anti-circling
                bool ispoisoned; //if poison is afflicting mon
                //int poisonresist; //0 = 0% resist (normal), 1 = 25% resist, 2 = 50% resist, 3 = 75% resist, 4 = 100% resist (immune)
                bool poisonimmune; //if true, poison has no effect
                int poisonpow; //power of poison afflicting mon
                int poisoncounter = 0; //determines when poison causes damage
                int olddir = -1; //used in ai routine for running
                int castsub = 0; //used in ai routine for sub5 smart mons casting
                bool breakdoor = false; //used in ai routine to prevent breaking and moving in same turn
                ArrayList carrying = new ArrayList();
                ArrayList equipped;
                int ammo = 0; //arrows, knives, stars, whatever (1 type per monster)
                //int ammonumber = -1; //item number of proj thrown/shot
                bool useammo = false;
                static int RANDOM = 0, GOTOWARDS = 1, STAYBACK = 2, RUN = 3, GUARD = 4, FRIENDLYMOVE = 5, FRIENDLYNOMOVE = 6; //AI states
                bool HITANDRUN = false; //true for gigglers, assassins, others?
                bool gamewin = false;
                //string endanim,endmusic,endsound,picstring,soundstring;
                string endanim,endsound,picstring,soundstring,footstep;
                int hurtitem,needitem,needhandneck,pickup,steal;
                
                public Monster(int num,int xc,int yc,int lev,string name, string picstring, string soundstring, string footstep, bool canusestairs, bool isflying, bool ignoremons, bool canteleport) {
                        number = num;
                        x = xc;
                        y = yc;
                        level = lev;
                        this.name = name;
                        this.picstring = picstring;
                        this.soundstring = soundstring;
                        this.footstep = footstep;
                        this.canusestairs = canusestairs;
                        this.isflying = isflying;
                        this.ignoremons = ignoremons;
                        this.canteleport = canteleport;
                        
                        File picfile = new File("Monsters"+File.separator+picstring+"-toward.gif");
                        if (picfile.exists()) towardspic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-toward.png");
                                if (picfile.exists()) towardspic = tk.getImage(picfile.getPath());
                                else towardspic = tk.getImage("Monsters"+File.separator+"screamer-toward.gif");
                        }
                        
                        picfile = new File("Monsters"+File.separator+picstring+"-away.gif");
                        if (picfile.exists()) awaypic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-away.png");
                                if (picfile.exists()) awaypic = tk.getImage(picfile.getPath());
                                else awaypic = towardspic;
                        }

                        picfile = new File("Monsters"+File.separator+picstring+"-right.gif");
                        if (picfile.exists()) rightpic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-right.png");
                                if (picfile.exists()) rightpic = tk.getImage(picfile.getPath());
                                else rightpic = towardspic;
                        }
                        
                        picfile = new File("Monsters"+File.separator+picstring+"-left.gif");
                        if (picfile.exists()) leftpic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-left.png");
                                if (picfile.exists()) leftpic = tk.getImage(picfile.getPath());
                                else leftpic = towardspic;
                        }
                        
                        picfile = new File("Monsters"+File.separator+picstring+"-attack.gif");
                        if (picfile.exists()) attackpic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-attack.png");
                                if (picfile.exists()) attackpic = tk.getImage(picfile.getPath());
                                else attackpic = towardspic;
                        }
                        
                        picfile = new File("Monsters"+File.separator+picstring+"-cast.gif");
                        if (picfile.exists()) castpic = tk.getImage(picfile.getPath());
                        else {
                                picfile = new File("Monsters"+File.separator+picstring+"-cast.png");
                                if (picfile.exists()) castpic = tk.getImage(picfile.getPath());
                                else castpic = attackpic;
                        }
                        
                        ImageTracker.addImage(towardspic,2);
                        ImageTracker.addImage(awaypic,2);
                        ImageTracker.addImage(rightpic,2);
                        ImageTracker.addImage(leftpic,2);
                        ImageTracker.addImage(attackpic,2);
                        ImageTracker.addImage(castpic,2);
                        try { ImageTracker.waitForID(2,3000); }
                        catch (InterruptedException e) {}
                        ImageTracker.removeImage(towardspic,2);
                        ImageTracker.removeImage(awaypic,2);
                        ImageTracker.removeImage(rightpic,2);
                        ImageTracker.removeImage(leftpic,2);
                        ImageTracker.removeImage(attackpic,2);
                        ImageTracker.removeImage(castpic,2);
                }

                public Monster(int num,int xc,int yc,int lev) {
                        number = num;
                        x = xc;
                        y = yc;
                        level = lev;
                        footstep = "";
                        switch (number) {     //when i know all these, put in monsterwizard/monsterdata
                                case 0:
                                        name = "Mummy";
                                        towardspic = tk.getImage("Monsters"+File.separator+"mummy-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"mummy-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"mummy-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"mummy-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"mummy-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "mummy.wav";
                                        footstep = "step2.wav";
                                        picstring = "mummy";
                                        break;
                                case 1:
                                        name = "Screamer";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"screamer.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"screamer-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "spider.wav";
                                        footstep = "step1.wav";
                                        picstring = "screamer";
                                        break;
                                case 2:
                                        name = "Giggler";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"giggler-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"giggler-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"giggler-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"giggler-left.gif");
                                        //attackpic = tk.getImage("Monsters"+File.separator+"giggler-attack.gif");
                                        attackpic = towardspic;
                                        castpic = attackpic;
                                        soundstring = "giggler.wav";
                                        //footstep = "step2.wav";
                                        picstring = "giggler";
                                        break;
                                case 3:
                                        name = "Rock Pile";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"rockpile.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"rockpile-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "rockmon.wav";
                                        footstep = "step1.wav";
                                        picstring = "rockpile";
                                        break;
                                case 4:
                                        name = "Slime";
                                        towardspic = tk.getImage("Monsters"+File.separator+"slime.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"slime-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "slime.wav";
                                        picstring = "slime";
                                        break;
                                case 5:
                                        name = "Wing Eye";
                                        //ignoremons = false;
                                        isflying = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"wingeye.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"wingeye-attack.gif");
                                        castpic = attackpic;
                                        footstep = "flap.wav";
                                        picstring = "wingeye";
                                        break;
                                case 6:
                                        name = "Ghost";
                                        isflying = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"ghost.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"ghost-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "mummy.wav";
                                        picstring = "ghost";
                                        break;
                                case 7:
                                        name = "Muncher";
                                        isflying = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"muncher-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"muncher-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"muncher-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"muncher-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"muncher-attack.gif");
                                        castpic = attackpic;
                                        //footstep = "flap.wav";
                                        picstring = "muncher";
                                        break;
                                case 8:
                                        name = "Skeleton";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"skeleton-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"skeleton-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"skeleton-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"skeleton-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"skeleton-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "swing.wav";
                                        footstep = "step3.wav";
                                        picstring = "skeleton";
                                        break;
                                case 9:
                                        name = "Worm";
                                        towardspic = tk.getImage("Monsters"+File.separator+"worm-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"worm-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"worm-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"worm-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"worm-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "worm.wav";
                                        footstep = "step1.wav";
                                        picstring = "worm";

                                        break;
                                case 10:
                                        name = "Fire Elemental";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"fireel.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"fireel-attack.gif");
                                        castpic = attackpic;
                                        picstring = "fireel";
                                        break;
                                case 11:
                                        name = "Water Elemental";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"waterel.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"waterel-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "waterel.wav";
                                        picstring = "waterel";
                                        break;
                                case 12:
                                        name = "Goblin";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"goblin-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"goblin-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"goblin-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"goblin-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"goblin-attack.gif");
                                        castpic = attackpic;
                                        footstep = "step2.wav";
                                        picstring = "goblin";
                                        break;
                                case 13:
                                        name = "Giant Rat";
                                        towardspic = tk.getImage("Monsters"+File.separator+"rat-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"rat-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"rat-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"rat-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"rat-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "roar.wav";
                                        footstep = "step1.wav";
                                        picstring = "rat";
                                        break;
                                case 14:
                                        name = "Ant Man";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"antman-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"antman-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"antman-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"antman-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"antman-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "swing.wav";
                                        footstep = "step2.wav";
                                        picstring = "antman";
                                        break;
                                case 15:
                                        name = "Beholder";
                                        isflying = true;
                                        //ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"beholder.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"beholder-attack.gif");
                                        castpic = attackpic;
                                        picstring = "beholder";
                                        break;
                                case 16:
                                        name = "Couatyl";
                                        isflying = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"couatyl-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"couatyl-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"couatyl-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"couatyl-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"couatyl-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "couatyl.wav";
                                        footstep = "flap.wav";
                                        picstring = "couatyl";
                                        break;
                                case 17:
                                        name = "Fader";
                                        isflying = true;
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"fader.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        //attackpic = tk.getImage("Monsters"+File.separator+"fader-attack.gif");
                                        attackpic = towardspic;
                                        castpic = attackpic;
                                        soundstring = "rockmon.wav";
                                        picstring = "fader";
                                        break;
                                case 18:
                                        name = "Tentacle Beast";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"tentaclemon.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"tentaclemon-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "slime.wav";
                                        footstep = "step1.wav";
                                        picstring = "tentaclemon";
                                        break;
                                case 19:
                                        name = "Scorpion";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"scorpion-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"scorpion-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"scorpion-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"scorpion-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"scorpion-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "scorpion.wav";
                                        footstep = "step1.wav";
                                        picstring = "scorpion";
                                        break;
                                case 20:
                                        name = "Demon";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"demon-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"demon-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"demon-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"demon-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"demon-attack.gif");
                                        castpic = attackpic;
                                        footstep = "step2.wav";
                                        picstring = "demon";
                                        break;
                                case 21:
                                        name = "Deth Knight";
                                        //ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"dethknight-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"dethknight-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"dethknight-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"dethknight-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"dethknight-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "swing.wav";
                                        footstep = "step4.wav";
                                        picstring = "dethknight";
                                        break;
                                case 22:
                                        name = "Spider";
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"spider-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"spider-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"spider-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"spider-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"spider-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "spider.wav";
                                        footstep = "step1.wav";
                                        picstring = "spider";
                                        break;
                                case 23:
                                        name = "Stone Golem";
                                        towardspic = tk.getImage("Monsters"+File.separator+"golem-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"golem-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"golem-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"golem-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"golem-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "swing.wav";
                                        footstep = "step2.wav";
                                        picstring = "golem";
                                        break;
                                case 24:
                                        name = "Sorcerer";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"sorcerer-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"sorcerer-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"sorcerer-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"sorcerer-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"sorcerer-attack.gif");
                                        castpic = attackpic;
                                        picstring = "sorcerer";
                                        canteleport = true;
                                        break;
                                case 25:  //need number in spellcasting
                                        name = "Dragon";
                                        //ignoremons = false;
                                        canusestairs = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"dragon-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"dragon-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"dragon-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"dragon-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"dragon-attack.gif");
                                        castpic = attackpic;
                                        soundstring = "roar.wav";
                                        footstep = "step5.wav";
                                        picstring = "dragon";
                                        break;
                                case 26:  //number used many places
                                        name = "Lord Chaos";
                                        isflying = true;
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"chaos-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"chaos-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"chaos-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"chaos-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"chaos-attack.gif");
                                        castpic = attackpic;
                                        picstring = "chaos";
                                        //canteleport = true;
                                        break;

                                case 27:  //number used some places
                                        name = "Demon Lord";
                                        ignoremons = false;
                                        towardspic = tk.getImage("Monsters"+File.separator+"demonlord.png");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"demonlord-attack.png");
                                        castpic = attackpic;
                                        soundstring = "demonlord.wav";
                                        footstep = "step5.wav";
                                        picstring = "demonlord";
                                        break;
                                /////////////////////////
                                /*
                                case :
                                        name = "Vexirk";
                                        power = 25;
                                        defense = 20;
                                        magicresist = 50;
                                        speed = 60;
                                        movespeed = 5;
                                        attackspeed = 1;
                                        maxhealth = 240+randGen.nextInt(30);
                                        maxmana = 2000;
                                        hasmagic = true;
                                        hasheal = true;
                                        ignoremons = false;
                                        castpower = 5;
                                        manapower = 80;
                                        numspells = 4;
                                        knownspells = new string[4];
                                        knownspells[0] = "44";
                                        knownspells[1] = "335";
                                        knownspells[2] = "51";//14 min for poison missle
                                        knownspells[3] = "31";
                                        minproj = 10; //min for poison cloud
                                        fearresist = 0;
                                        defaultai = STAYBACK;
                                        towardspic = tk.getImage("Monsters"+File.separator+"vexirk-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"vexirk-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"vexirk-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"vexirk-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"vexirk-attack.gif");
                                        castpic = attackpic;
                                        carrying.addElement(137); //robe top
                                        carrying.addElement(157); //robe bottom
                                        break;
                                case :
                                        name = "Hell Hound";
                                        power = 100;
                                        defense = 20;
                                        magicresist = 30;
                                        speed = 45;
                                        movespeed = 6;
                                        attackspeed = 1;
                                        maxhealth = 320+randGen.nextInt(30);
                                        fearresist = 4;
                                        defaultai = GOTOWARDS;
                                        towardspic = tk.getImage("Monsters"+File.separator+"hound-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"hound-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"hound-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"hound-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"hound-attack.gif");
                                        castpic = attackpic;
                                        carrying.addElement(new Item(68));//shank, maybe 2
                                        if (randGen.nextbool()) carrying.addElement(new Item(68));
                                        break;
                                case :
                                        name = "Giant Wasp";
                                        power = 15;
                                        defense = 10;
                                        magicresist = 20;
                                        speed = 75;
                                        movespeed = 4;
                                        attackspeed = 1;
                                        maxhealth = 180+randGen.nextInt(20);
                                        poison = 4;
                                        fearresist = 2;
                                        isflying = true;
                                        defaultai = GOTOWARDS;
                                        HITANDRUN = true;
                                        towardspic = tk.getImage("Monsters"+File.separator+"wasp.gif");
                                        awaypic = towardspic;
                                        rightpic = towardspic;
                                        leftpic = towardspic;
                                        attackpic = tk.getImage("Monsters"+File.separator+"wasp-attack.gif");
                                        castpic = attackpic;
                                        break;
                                case :
                                        name = "Blue Ogre";
                                        power = 28;
                                        defense = 10;
                                        magicresist = 10;
                                        speed = 40;
                                        movespeed = 5;
                                        attackspeed = 1;
                                        maxhealth = 70+randGen.nextInt(10);
                                        fearresist = 6;
                                        defaultai = GOTOWARDS;
                                        towardspic = tk.getImage("Monsters"+File.separator+"blueogre-toward.gif");
                                        awaypic = tk.getImage("Monsters"+File.separator+"blueogre-away.gif");
                                        rightpic = tk.getImage("Monsters"+File.separator+"blueogre-right.gif");
                                        leftpic = tk.getImage("Monsters"+File.separator+"blueogre-left.gif");
                                        attackpic = tk.getImage("Monsters"+File.separator+"blueogre-attack.gif");
                                        castpic = attackpic;
                                        carrying.addElement(new Item(226));//club
                                        break;
                                */
                        }
                        ImageTracker.addImage(towardspic,2);
                        ImageTracker.addImage(awaypic,2);
                        ImageTracker.addImage(rightpic,2);
                        ImageTracker.addImage(leftpic,2);
                        ImageTracker.addImage(attackpic,2);
                        ImageTracker.addImage(castpic,2);
                        try { ImageTracker.waitForID(2,3000); }
                        catch (InterruptedException e) {}
                        ImageTracker.removeImage(towardspic,2);
                        ImageTracker.removeImage(awaypic,2);
                        ImageTracker.removeImage(rightpic,2);
                        ImageTracker.removeImage(leftpic,2);
                        ImageTracker.removeImage(attackpic,2);
                        ImageTracker.removeImage(castpic,2);
                }

                public void timePass() {
                        if (moveattack==moncycle) { 
                                moveattack=0;
                                return;
                        }
                        else if (moveattack!=0) moveattack=0;
                        if (movespeedboost>0) { movespeedboost--; movespeed--; }
                        //if (attackspeedboost<0) { attackspeedboost++; attackspeed++; }
                        if (isdying) {
                                deathcounter++;
                                if (deathcounter>1) {
                                        dmmons.remove(level+","+x+","+y+","+subsquare);
                                        int i=0;
                                        bool montest = false;
                                        while (i<6 && !montest) {
                                                if (dmmons.get(level+","+x+","+y+","+i)!=null) montest=true;
                                                if (i==3) i=5;
                                                else i++;
                                        }        
                                        DungeonMap[level][x][y].hasMons=montest;
                                        if (level==dmnew.level) needredraw = true;
                                }
                                return;
                        }
                        else if (isattacking) {
                                if (!breakdoor) hitHero();
                                isattacking = false;
                                //attackpic.flush();
                                if (level==dmnew.level) needredraw = true;
                                return;
                        }
                        else if (iscasting) {
                                iscasting = false;
                                if (level==dmnew.level) needredraw = true;
                                return;
                        }
                        breakdoor = false;
                        if (freezelife>0 && number!=26 && number!=27) return; //if all life frozen, doesn't affect chaos or demon lord
                        if (timecounter<0) { timecounter++; timecounter+=sleeper; return; } //if frozen
                        movecounter++;
                        movecounter+=sleeper;
                        if (movecounter>movespeed) {
                                //if (wasfrightened) { wasfrightened=false; currentai=RUN; }
                                if (wasfrightened) currentai=RUN;
                                //chaos doesn't like flux cages:
                                if (number==26 && fluxchanging && fluxcages.get(level+","+x+","+y)!=null) { runcounter=1; currentai=RUN; }
                                doAI();
                                //test for ai state change from gotowards back to stayback
                                if (defaultai==STAYBACK && currentai==GOTOWARDS) {
                                        if (!wasstuck && (ammo>0 || (hasmagic && !silenced && mana >= minproj))) currentai=STAYBACK;
                                        wasstuck=false;
                                }
                        }
                        else if (level==dmnew.level && xdist<5 && ydist<5 && randGen.nextInt(5)==0) {
                                mirrored=!mirrored;
                                needredraw=true;
                        }
                        timecounter++;
                        timecounter+=sleeper;
                        if (timecounter>210) {
                                timecounter=0;
                                heal(maxhealth/20+2);
                                energize(maxmana/20+2);
                                if (ispoisoned) {
                                        if (poisonpow>0) {
                                                //if (poisonpow>15) poisonpow=15;
                                                //else if (randGen.nextbool()) poisonpow--;
                                                if (randGen.nextbool()) poisonpow--;
                                                //else if ((maxhealth/4)/6-poison > 0 || randGen.nextInt(100) < maxhealth/4) poisonpow--;
                                        }
                                        else ispoisoned=false;
                                }
                                //silence
                                if (silencecount>0) {
                                        silencecount--;
                                        if (silencecount==0) silenced = false;
                                }
                                //if have stat boosting spells for mons, put decrement stuff here
                                if (powerboost<0) { powerboost++; power++; }
                                if (defenseboost<0) { defenseboost++; defense++; }
                                if (magicresistboost<0) { magicresistboost++; magicresist++; }
                                if (speedboost<0) { speedboost++; speed++; }
                                if (manapowerboost<0) { manapowerboost++; manapower++; }
                        }
                        if (ispoisoned) {
                                if (isImmaterial || poisonimmune) { ispoisoned=false; poisonpow=0; }
                                else {
                                        poisoncounter++;
                                        poisoncounter+=sleeper;
                                        if (poisoncounter>30) {
                                           poisoncounter=0;
                                           if (poisonpow>15) poisonpow=15;
                                           damage(poisonpow,POISONHIT);
                                           //System.out.println(name+" takes "+damage(poisonpow,POISONHIT)+" poison damage.");
                                        }
                                }
                        }
                }


                public void hitHero() {
                        //give hero[who] xp for dodging?
                        
                        //bool backhit = false;
                        int who,whoalt,whotest1,whotest2,whotest3,whotest4;
                        if (facing==NORTH) { whotest1=2; whotest2=3; whotest3=0; whotest4=1; }
                        else if (facing==SOUTH) { whotest1=0; whotest2=1; whotest3=2; whotest4=3; }
                        else if (facing==EAST) { whotest1=0; whotest2=3; whotest3=1; whotest4=2; }
                        else { whotest1=1; whotest2=2; whotest3=0; whotest4=3; }
                        if (randGen.nextbool()) {
                                who = (whotest1+dmnew.facing)%4;
                                whoalt = (whotest2+dmnew.facing)%4;
                        }
                        else {
                                who = (whotest2+dmnew.facing)%4;
                                whoalt = (whotest1+dmnew.facing)%4;
                        }
                        if (heroatsub[who]==-1) {
                                who = whoalt;
                                if (heroatsub[who]==-1) {
                                        if (randGen.nextbool()) {
                                                who = (whotest3+dmnew.facing)%4;
                                                whoalt = (whotest4+dmnew.facing)%4;
                                        }
                                        else {
                                                who = (whotest4+dmnew.facing)%4;
                                                whoalt = (whotest3+dmnew.facing)%4;
                                        }
                                        if (heroatsub[who]==-1) who=whoalt;
                                        //backhit = true; //harder to hit back row
                                }
                        }
                        who = heroatsub[who];
                        //put this in to prevent errors when attacked with no character:
                        if (who==-1) return;
                        //if (name.equals("Giggler")) { //steal an item
                        if (steal!=0 && (steal==4 || (steal==2 && randGen.nextbool()) || (steal==1 && randGen.nextInt(4)==0) || (steal==3 && randGen.nextInt(4)!=0))) {
                                bool found = false;
                                if (!hero[who].weapon.name.equals("Fist/Foot") && hero[who].weapon.number!=219 && hero[who].weapon.number!=261) { 
                                        carrying.add(hero[who].weapon);
                                        hero[who].load-=hero[who].weapon.weight;
                                        if (hero[who].weapon.type==Item.WEAPON || hero[who].weapon.type==Item.SHIELD) hero[who].weapon.unEquipEffect(hero[who]);
                                        if (hero[who].weapon.number==9) ((Torch)hero[who].weapon).putOut();
                                        hero[who].weapon=fistfoot;
                                        found=true;
                                        hero[who].repaint();
                                        weaponsheet.repaint();
                                        updateDark();
                                }
                                else if (hero[who].hand!=null) { 
                                        carrying.add(hero[who].hand);
                                        hero[who].load-=hero[who].hand.weight;
                                        if (hero[who].hand.type==Item.SHIELD) hero[who].hand.unEquipEffect(hero[who]);
                                        else if (hero[who].hand.number==9) ((Torch)hero[who].hand).putOut();
                                        hero[who].hand=null;
                                        found=true;
                                        hero[who].repaint();
                                        updateDark();
                                }
                                else if (!found) {
                                        ArrayList tryval = new ArrayList(5);
                                        tryval.add(new Integer(0)); tryval.add(new Integer(1)); tryval.add(new Integer(2)); tryval.add(new Integer(3)); tryval.add(new Integer(4)); tryval.add(new Integer(5)); tryval.add(new Integer(6)); tryval.add(new Integer(7)); tryval.add(new Integer(8));
                                        while (!found && !tryval.isEmpty()) {
                                                int val = ((Integer)tryval.remove(0)).intValue();
                                                switch (val) {
                                                        case 0:
                                                                //head
                                                                if (hero[who].head!=null) {
                                                                        carrying.add(hero[who].head);
                                                                        hero[who].load-=hero[who].head.weight;
                                                                        hero[who].head.unEquipEffect(hero[who]);
                                                                        hero[who].head=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 1:
                                                                //neck
                                                                if (hero[who].neck!=null) {
                                                                        if (hero[who].neck.number==89) numillumlets--;
                                                                        carrying.add(hero[who].neck);
                                                                        hero[who].load-=hero[who].neck.weight;
                                                                        hero[who].neck.unEquipEffect(hero[who]);
                                                                        hero[who].neck=null;
                                                                        found=true;
                                                                        updateDark();
                                                                }
                                                                break;
                                                        case 2:
                                                                //torso
                                                                if (hero[who].torso!=null) {
                                                                        carrying.add(hero[who].torso);
                                                                        hero[who].load-=hero[who].torso.weight;
                                                                        hero[who].torso.unEquipEffect(hero[who]);
                                                                        hero[who].torso=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 3:
                                                                //legs
                                                                if (hero[who].legs!=null) {
                                                                        carrying.add(hero[who].legs);
                                                                        hero[who].load-=hero[who].legs.weight;
                                                                        hero[who].legs.unEquipEffect(hero[who]);
                                                                        hero[who].legs=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 4:
                                                                //feet
                                                                if (hero[who].feet!=null) {
                                                                        carrying.add(hero[who].feet);
                                                                        hero[who].load-=hero[who].feet.weight;
                                                                        hero[who].feet.unEquipEffect(hero[who]);
                                                                        hero[who].feet=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 5:
                                                                //pouch1
                                                                if (hero[who].pouch1!=null) {
                                                                        carrying.add(hero[who].pouch1);
                                                                        hero[who].load-=hero[who].pouch1.weight;
                                                                        hero[who].pouch1=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 6:
                                                                //pouch2
                                                                if (hero[who].pouch2!=null) {
                                                                        carrying.add(hero[who].pouch2);
                                                                        hero[who].load-=hero[who].pouch2.weight;
                                                                        hero[who].pouch2=null;
                                                                        found=true;
                                                                }
                                                                break;
                                                        case 7:
                                                                //quiver
                                                                int i=randGen.nextInt(6);
                                                                int count = 0;
                                                                while (!found && count<6) {
                                                                        if (hero[who].quiver[i]!=null) {
                                                                                carrying.add(hero[who].quiver[i]);
                                                                                hero[who].load-=hero[who].quiver[i].weight;
                                                                                hero[who].quiver[i]=null;
                                                                                found=true;
                                                                        }
                                                                        else { i=(i+1)%6; count++; }
                                                                }
                                                                break;
                                                        case 8:
                                                                //pack
                                                                i=randGen.nextInt(16);
                                                                count = 0;
                                                                while (!found && count<16) {
                                                                        if (hero[who].pack[i]!=null) {
                                                                                carrying.add(hero[who].pack[i]);
                                                                                hero[who].load-=hero[who].pack[i].weight;
                                                                                hero[who].pack[i]=null;
                                                                                found=true;
                                                                        }
                                                                        else { i=(i+1)%16; count++; }
                                                                }
                                                                break;
                                                
                                                }
                                        }
                                }
                                if (sheet && found && herosheet.hero.equals(hero[who])) herosheet.repaint();
                                if (HITANDRUN && (!found || randGen.nextInt(32)!=0)) { runcounter=40+randGen.nextInt(10); currentai=RUN; }
                                return;
                        }
                        if (!sleeping && randGen.nextInt(20)==0) { movecounter-=4; message.setMessage(name+": Critical Miss",5); return; }
                        
                        int speeddif = speed - hero[who].dexterity;
                        if (hero[who].stamina<hero[who].maxstamina/4) speeddif+=5;
                        if (hero[who].load>hero[who].maxload) speeddif+=5;
                        if (hero[who].hurttorso || hero[who].hurtlegs || hero[who].hurtfeet) speeddif+=10;
                        //if (backhit) speeddif-=10;
                        if (parry==who) speeddif-=15;
                        parry=-1;
                        
                        bool didhit;
                        if (sleeping) didhit = true;
                        else if (speeddif>40) didhit = (randGen.nextInt(8)!=0);
                        else if (speeddif>30) didhit = (randGen.nextInt(7)!=0);
                        else if (speeddif>20) didhit = (randGen.nextInt(6)!=0);
                        else if (speeddif>10) didhit = (randGen.nextInt(5)!=0);
                        else if (speeddif>0) didhit = (randGen.nextInt(4)!=0);
                        else if (speeddif>-10) didhit = (randGen.nextInt(3)!=0);
                        else if (speeddif>-20) didhit = (randGen.nextInt(2)!=0);
                        else if (speeddif>-30) didhit = (randGen.nextInt(3)==0);
                        else didhit = (randGen.nextInt(4)==0);
                        //else if (speeddif>-40) didhit = (randGen.nextInt(4)==0);
                        //else didhit = (randGen.nextInt(5)==0);
                        
                        //false image ability active -> image destroyed if didhit (and 50% chance if didmiss so doesn't get too powerful)
                        if (hero[who].falseimage>0 && (didhit || randGen.nextbool())) {
                                didhit = false;
                                hero[who].falseimage--;
                                if (hero[who].falseimage>0) message.setMessage("A false image around "+hero[who].name+" is destroyed.",hero[who].heronumber);
                                else message.setMessage("The last false image around "+hero[who].name+" is destroyed.",hero[who].heronumber);
                        }
                                
                        if (didhit) {
                                playSound("oof.wav",-1,-1);
                                int pow = power+randGen.nextInt(power/4+10);
                                //if (backhit) pow = pow*2/3;
                                if (speed>50 && (randGen.nextInt(20)==0 || (sleeping && randGen.nextInt(5)==0))) { pow=3*pow/2; message.setMessage(name+": Critical Hit",5); }
                                if (poison>0 && (randGen.nextInt(10)>4 || (DIFFICULTY>0 && randGen.nextInt(10+DIFFICULTY*5)>4))) {
                                   hero[who].poison+=poison; hero[who].ispoisoned=true;
                                   if (DIFFICULTY==-1 && hero[who].poison>poison*2) hero[who].poison=poison*2;
                                   else if (DIFFICULTY==-2) hero[who].poison=poison;
                                }
                                hero[who].damage(pow,WEAPONHIT);
                                hupdate();
                                if (HITANDRUN) { runcounter=12+(Math.abs(randGen.nextInt())%8); currentai=RUN; }
                        }
                }
                
                public void doAI() {
                        
                        //need improved way to deal with corners like:
                        //1111111111111
                        //11....m....11    .  floor
                        //11..11111..11    1  wall
                        //11..11111..11    x  party
                        //11....x....11    m  monster
                        //1111111111111
                        //need make move out of poison clouds/closing doors before really hurting
                        
                        //need deal with party fleeing up/down stairs...
                        //--maybe have a wasnear flag, go towards partyx partyy, could get lucky and find stairs
                        //--of course, currently party can simply stand in stairs and not be touchable since mon can't move into stairs if party is in them
                        //----maybe pull party up/down to mons level if mon tries to enter stairs, just like it does to mons if party enters them

                        if (currentai==FRIENDLYNOMOVE) {
                                movecounter = 0; //don't move at all
                                return;
                        }
                        
                        xdist = x-partyx; if (xdist<0) xdist*=-1;
                        ydist = y-partyy; if (ydist<0) ydist*=-1;
                        //if (level==dmnew.level && xdist<5 && ydist<5 && (number==16 || number==5)) playSound("flap.wav",x,y); //couatyl and wing-eye's flap
                        bool canattack = false;
                        if (level==dmnew.level && ((xdist==0 && ydist==1) || (ydist==0 && xdist==1))) {
                            if (subsquare!=5) { 
                                if (x>partyx) {
                                        if (subsquare==0 || subsquare==3) canattack = true;
                                        //else if (currentai!=RUN || wasstuck) {
                                        else if (currentai==GOTOWARDS) {
                                          if (subsquare==1 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+0)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 0;
                                                dmmons.put(level+","+x+","+y+","+0,this);
                                          }
                                          else if (subsquare==2 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+3)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 3;
                                                dmmons.put(level+","+x+","+y+","+3,this);
                                          }
                                          //if (leftpic.equals(towardspic)) facing=WEST;
                                          facing = WEST;
                                        }
                                }
                                else if (x<partyx) {
                                        if (subsquare==1 || subsquare==2) canattack = true;
                                        //else if (currentai!=RUN || wasstuck) {
                                        else if (currentai==GOTOWARDS) {
                                          if (subsquare==0 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+1)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 1;
                                                dmmons.put(level+","+x+","+y+","+1,this);
                                          }
                                          else if (subsquare==3 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+2)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 2;
                                                dmmons.put(level+","+x+","+y+","+2,this);
                                          }
                                          //if (leftpic.equals(towardspic)) facing=EAST;
                                          facing = EAST;
                                        }
                                }
                                else if (y>partyy) {
                                        if (subsquare==0 || subsquare==1) canattack = true;
                                        //else if (currentai!=RUN || wasstuck) {
                                        else if (currentai==GOTOWARDS) {
                                          if (subsquare==2 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+1)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 1;
                                                dmmons.put(level+","+x+","+y+","+1,this);
                                          }
                                          else if (subsquare==3 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+0)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 0;
                                                dmmons.put(level+","+x+","+y+","+0,this);
                                          }
                                          //if (leftpic.equals(towardspic)) facing=NORTH;
                                          facing = NORTH;
                                        }
                                }
                                else if (y<partyy) {
                                        if (subsquare==2 || subsquare==3) canattack = true;
                                        //else if (currentai!=RUN || wasstuck) {
                                        else if (currentai==GOTOWARDS) {
                                          if (subsquare==0 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+3)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 3;
                                                dmmons.put(level+","+x+","+y+","+3,this);
                                          }
                                          else if (subsquare==1 && DungeonMap[level][x][y].numProjs==0 && dmmons.get(level+","+x+","+y+","+2)==null) {
                                                if (!footstep.equals("")) playFootStep(footstep,x,y);
                                                canattack = true;
                                                moveattack = moncycle;
                                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                                subsquare = 2;
                                                dmmons.put(level+","+x+","+y+","+2,this);
                                          }
                                          //if (leftpic.equals(towardspic)) facing=SOUTH;
                                          facing = SOUTH;
                                        }
                                }
                            }
                            else canattack = true;
                        }
                        
                        int count,dir;
                        newai = currentai;
                        if (level!=dmnew.level) {
                                if (currentai!=GUARD && currentai!=FRIENDLYMOVE && currentai!=RANDOM) {
                                        //put a search for stairs here
                                        //if any within 3 away, go to
                                        currentai=RANDOM;
                                }
                        }
                        //else wasnear = true;
                        
                        while (movecounter>movespeed) { //go thru until move or attack
                           
                           switch (currentai) {
                                
                                case RANDOM:
                                  
                                  
                                  if (canattack) { //if in a square next to party
                                        randomcounter = 0;
                                        newai=defaultai; //could be GOTOWARDS or STAYBACK
                                        doAttack();
                                        break; //don't do a random move after all
                                  }
                                  else if (randomcounter>0) { //if temporarily in random (obstacle in way)
                                        randomcounter--;
                                        if (randomcounter==0) newai=defaultai;
                                  }
                                  else if (level==dmnew.level && xdist<5 && ydist<5) newai=defaultai; //if near party (within 5 squares)
                                  
                                  //chaos and sorcerer may teleport
                                  if ((number==26 || (canteleport && (!silenced || randGen.nextbool()))) && randGen.nextInt(20)==0 && teleport()) { moveattack=moncycle; }
                                  else {
                                          //move random direction if possible
                                          count = 0;
                                          dir=randGen.nextInt(4);
                                          while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                          if (count<4) monMove(dir);
                                          else movecounter = 0; //can't move at all
                                  }
                                  
                                  break;
                                
                                case GOTOWARDS:
                        
                                  //if hurting (health at or below 20% of full)
                                  if (health < maxhealth/5 && hasheal && !silenced && mana>59) useHealMagic(); //power depends on how powerful a mage mon is
                                  else if (fearresist>0 && hurttest==0 && health < maxhealth/5) {
                                        if (randGen.nextInt(10)<fearresist || (health<40 && maxhealth<400 && randGen.nextbool())) {
                                                hurt=true; newai=RUN; break;
                                        }
                                        else hurttest=12-fearresist;
                                  }
                                  else if (hurttest>0) hurttest--;
                                  if (canattack) doAttack(); //do attack or use close-range magic - have chance of setting hurt (even though not hurt) and entering run - dodge
                                  else { //cast proj, or go towards party
                                    //possibility of proj attack
                                    if ((ammo>0 || (hasmagic && !silenced && mana>=minproj)) && randGen.nextbool() && ((x==partyx && ydist>1) || (y==partyy && xdist>1)) ) { 
                                        if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                        else if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                        else if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH); 
                                        else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                        if (movecounter<movespeed) break; //if cast, don't move
                                    }
                                    //small chance of not moving at all
                                    if (randGen.nextInt()%10>5) {
                                        if (attackspeed>0) movecounter=attackspeed;
                                        else movecounter = 0;
                                    }
                                    //chance of waiting for party to step close (if party near) -> makes circling strategy harder
                                    else if (xdist==1 && ydist==1 && !HITANDRUN && randGen.nextbool()) {
                                        movecounter=movespeed;
                                        waitattack=true;
                                    }
                                    //get random bool to determine whether to start checking x or y values
                                    else if (randGen.nextbool()) { //check x values first, y's if can't move in x
                                        if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else { randomcounter=3; newai=RANDOM; } //couldn't move towards - obstacle - try get around by moving a few random steps - this doesn't work that great...
                                    }
                                    else { //check y values first
                                        if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else { randomcounter=3; newai=RANDOM; } //couldn't move towards - obstacle - try get around by moving a few random steps - this doesn't work that great...
                                    }
                                  }
                                  break;
                                
                                case RUN: //move away from party (can be fixed number of steps - giggler, horn of fear?)
                                
                                  //put possibility of proj attack here
                                  //similar to above go towards party
                                  if (((canteleport && (!silenced || randGen.nextbool())) || (number==26 && wasstuck)) && teleport()) { wasstuck=false; moveattack=moncycle; }
                                  else if (wasstuck && canattack) { wasstuck=false; doAttack(); }
                                  else if (canattack && !wasfrightened && randGen.nextbool() && (!HITANDRUN || randGen.nextInt(6)==0)) { waitattack = true; doAttack(); }//chance of turning and attacking in one move
                                  else if (randGen.nextbool()) { //check x values first, y's if can't move in x
                                      if (x==partyx && randGen.nextbool()) {
                                        if (randGen.nextbool() && canMove(EAST)) monMove(EAST);
                                        else if (canMove(WEST)) monMove(WEST);
                                        else if (canMove(EAST)) monMove(EAST);
                                        else if (y>partyy) { if ((!wasstuck || olddir==-1 || olddir!=NORTH) && canMove(SOUTH)) monMove(SOUTH); }
                                        else if (y<partyy) { if ((!wasstuck || olddir==-1 || olddir!=SOUTH) && canMove(NORTH)) monMove(NORTH); }
                                        else { wasstuck=true; randomcounter=3; newai=RANDOM; }
                                      }
                                      else if (x>partyx) { if ((!wasstuck || olddir==-1 || olddir!=WEST) && canMove(EAST)) monMove(EAST); }
                                      else if (x<partyx) { if ((!wasstuck || olddir==-1 || olddir!=EAST) && canMove(WEST)) monMove(WEST); }
                                      else if (y>partyy) { if ((!wasstuck || olddir==-1 || olddir!=NORTH) && canMove(SOUTH)) monMove(SOUTH); }
                                      else if (y<partyy) { if ((!wasstuck || olddir==-1 || olddir!=SOUTH) && canMove(NORTH)) monMove(NORTH); }
                                      //if (movecounter>0 && (attackspeed<=0 || movecounter<attackspeed)) { //couldn't move away, try random but not toward
                                      if (movecounter>movespeed) {
                                        //System.out.println("Couldn't move away");
                                        wasstuck = true;
                                        count = 0;
                                        dir=randGen.nextInt(4);
                                        while (count<4 && ((olddir!=-1 && dir==(olddir+2)%4) || (dir==NORTH && y>partyy) || (dir==SOUTH && y<partyy) || (dir==EAST && x<partyx) || (dir==WEST && x>partyx) || !canMove(dir))) { 
                                              count++; dir=(dir+1)%4;
                                              //if (dir==NORTH && y>partyy) { count++; dir=(dir+1)%4; }
                                              //else if (dir==SOUTH && y<partyy) { count++; dir=(dir+1)%4; }
                                              //else if (dir==EAST && x<partyx) { count++; dir=(dir+1)%4; }
                                              //else if (dir==WEST && x>partyx) { count++; dir=(dir+1)%4; }
                                        }
                                        if (count<4 && randGen.nextInt(15)>0) { olddir=dir; monMove(dir); }
                                        //else if (number==26) { wasstuck=true; }
                                        else if (canattack) doAttack();
                                        //else if (randGen.nextbool()) {
                                        //cast proj if can, else don't move at all
                                        else if ((ammo>0 || (hasmagic && !silenced && mana>=minproj)) && ((x==partyx && ydist>1) || (y==partyy && xdist>1)) ) { 
                                                 if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                                 else if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                                 else if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH); 
                                                 else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                                 else movecounter = 0;
                                        }
                                        //else movecounter = 0;
                                        //}
                                        else {
                                              if (randGen.nextbool()) { olddir=-1; newai = GOTOWARDS; }
                                              else if (x>partyx && canMove(WEST)) { wasstuck=true; olddir=WEST; monMove(WEST); }
                                              else if (x<partyx && canMove(EAST)) { wasstuck=true; olddir=EAST; monMove(EAST); }
                                              else if (y>partyy && canMove(NORTH)) { wasstuck=true; olddir=NORTH; monMove(NORTH); }
                                              else if (y<partyy && canMove(SOUTH)) { wasstuck=true; olddir=SOUTH; monMove(SOUTH); }
                                              else movecounter=0;
                                              //count = 0;
                                              //dir=randGen.nextInt(4);
                                              //while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                              //if (count<4) { wasstuck=true; monMove(dir); }
                                              //else movecounter = 0;
                                        }
                                      }
                                  }
                                  else { //check y values first
                                      if (y==partyy && randGen.nextbool()) {
                                        if (randGen.nextbool() && canMove(NORTH)) monMove(NORTH);
                                        else if (canMove(SOUTH)) monMove(SOUTH);
                                        else if (canMove(NORTH)) monMove(NORTH);
                                        else if (x>partyx) { if ((!wasstuck || olddir==-1 || olddir!=WEST) && canMove(EAST)) monMove(EAST); }
                                        else if (x<partyx) { if ((!wasstuck || olddir==-1 || olddir!=EAST) && canMove(WEST)) monMove(WEST); }
                                        else { wasstuck=true; randomcounter=3; newai=RANDOM; }
                                      }
                                      else if (y>partyy) { if ((!wasstuck || olddir!=-1 || olddir!=NORTH) && canMove(SOUTH)) monMove(SOUTH); }
                                      else if (y<partyy) { if ((!wasstuck || olddir!=-1 || olddir!=SOUTH) && canMove(NORTH)) monMove(NORTH); }
                                      else if (x>partyx) { if ((!wasstuck || olddir!=-1 || olddir!=WEST) && canMove(EAST)) monMove(EAST); }
                                      else if (x<partyx) { if ((!wasstuck || olddir!=-1 || olddir!=EAST) && canMove(WEST)) monMove(WEST); }
                                      if (movecounter>movespeed) { //couldn't move away
                                        //System.out.println("Couldn't move away");
                                        wasstuck = true;
                                        count = 0;
                                        dir=randGen.nextInt(4);
                                        while (count<4 && ((olddir!=-1 && dir==(olddir+2)%4) || (dir==NORTH && y>partyy) || (dir==SOUTH && y<partyy) || (dir==EAST && x<partyx) || (dir==WEST && x>partyx) || !canMove(dir))) { 
                                        //while (!canMove(dir) && count<4) { 
                                              count++; dir=(dir+1)%4;
                                              //if (dir==NORTH && y>partyy) { count++; dir=(dir+1)%4; }
                                              //else if (dir==SOUTH && y<partyy) { count++; dir=(dir+1)%4; }
                                              //else if (dir==EAST && x<partyx) { count++; dir=(dir+1)%4; }
                                              //else if (dir==WEST && x>partyx) { count++; dir=(dir+1)%4; }
                                        }
                                        if (count<4 && randGen.nextInt(15)>0) { olddir=dir; monMove(dir); }
                                        //else if (number==26) { wasstuck=true; }
                                        else if (canattack) doAttack();
                                        //else if (randGen.nextbool()) {  
                                        //cast proj if can, else don't move at all
                                        else if ((ammo>0 || (hasmagic && !silenced && mana>=minproj)) && ((x==partyx && ydist>1) || (y==partyy && xdist>1)) ) { 
                                                 if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                                 else if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                                 else if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH); 
                                                 else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                                 else movecounter = 0;
                                        }
                                        //else movecounter = 0;
                                        //}
                                        else {
                                              if (randGen.nextbool()) { olddir=-1; newai = GOTOWARDS; }
                                              else if (x>partyx && canMove(WEST)) { wasstuck=true; olddir=WEST; monMove(WEST); }
                                              else if (x<partyx && canMove(EAST)) { wasstuck=true; olddir=EAST; monMove(EAST); }
                                              else if (y>partyy && canMove(NORTH)) { wasstuck=true; olddir=NORTH; monMove(NORTH); }
                                              else if (y<partyy && canMove(SOUTH)) { wasstuck=true; olddir=SOUTH; monMove(SOUTH); }
                                              else movecounter=0;
                                              //count = 0;
                                              //dir=randGen.nextInt()%4; if (dir<0) dir*=-1;
                                              //while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                              //if (count<4) { wasstuck=true; monMove(dir); }

                                              //else movecounter = 0;
                                        }
                                      }
                                  }
                                  if (hurt && hasheal && !silenced && mana > 59 && randGen.nextbool()) { useHealMagic(); }
                                  else if (hurt && health>.2*maxhealth) { hurt=false; if (runcounter==0) { olddir=-1; newai=defaultai; } }//no longer hurting, perhaps no longer running
                                  if (!hurt && runcounter>0) { runcounter--; if (runcounter==0) { olddir=-1; newai=defaultai; if (newai==GUARD) {newai=GOTOWARDS;defaultai=GOTOWARDS;} } }
                                  if (wasfrightened) wasfrightened = false;
                                  break;

                                case STAYBACK: //wizards, archers?
                                  
                                  if (health < maxhealth/5 && hasheal && !silenced && mana>59) useHealMagic(); //power depends on how powerful a mage mon is
                                  else if (fearresist>0 && hurttest==0 && health < maxhealth/5) {
                                        if (randGen.nextInt(10)<fearresist || (health<40 && maxhealth<400 && randGen.nextbool())) {
                                                hurt=true; newai=RUN; break;
                                        }
                                        else hurttest=12-fearresist;
                                  }
                                  else if (hurttest>0) hurttest--;
                                  if (ammo==0 && (!hasmagic || mana < minproj || silenced)) { 
                                        if (randGen.nextbool()) newai=GOTOWARDS; else { runcounter = 5; newai = RUN; }
                                  }
                                  else if (x==partyx) { //if lined up along x
                                        //if far enough away, attack with proj if possible - move back if too close
                                        //if (ydist>1 && randGen.nextInt()%10<6) {
                                        if ((ydist>1 || (!canattack && randGen.nextInt(3)!=0)) && randGen.nextInt()%10<6) {
                                            if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH);
                                            else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                            //else if ((subsquare==0 || subsquare==3) && canMove(EAST)) monMove(EAST);
                                            //else if ((subsquare==1 || subsquare==2) && canMove(WEST)) monMove(WEST);
                                            else if ((subsquare==0 || subsquare==3) && canMove(EAST)) { monMove(EAST); movecounter=attackspeed; }
                                            else if ((subsquare==1 || subsquare==2) && canMove(WEST)) { monMove(WEST); movecounter=attackspeed; }
                                            else { randomcounter=3; newai=RANDOM; }
                                        }
                                        else if (y>partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y<partyy && canMove(NORTH)) monMove(NORTH);
                                        else if (randGen.nextInt()%10>2) {  //will this cause fluttering back and forth?
                                          if (randGen.nextbool() && canMove(EAST)) monMove(EAST);
                                          else if (canMove(WEST)) monMove(WEST);
                                          else if (canMove(EAST)) monMove(EAST);
                                          else { newai = GOTOWARDS; wasstuck=true; }
                                        }
                                        else { newai = GOTOWARDS; wasstuck=true; }
                                  }
                                  else if (y==partyy) { //if lined up along y
                                        //if far enough away, attack with proj if possible - move back if too close
                                        //if (xdist>1 && randGen.nextInt()%10<6) {
                                        if ((xdist>1 || (!canattack && randGen.nextInt(3)!=0)) && randGen.nextInt()%10<6) {
                                            if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                            else if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                            //else if ((subsquare==0 || subsquare==1) && canMove(SOUTH)) monMove(SOUTH);
                                            //else if ((subsquare==2 || subsquare==3) && canMove(NORTH)) monMove(NORTH);
                                            else if ((subsquare==0 || subsquare==1) && canMove(SOUTH)) { monMove(SOUTH); movecounter=attackspeed; }
                                            else if ((subsquare==2 || subsquare==3) && canMove(NORTH)) { monMove(NORTH); movecounter=attackspeed; }
                                            else { randomcounter=3; newai=RANDOM; }
                                        }
                                        else if (x>partyx && canMove(EAST)) monMove(EAST);
                                        else if (x<partyx && canMove(WEST)) monMove(WEST);
                                        else if (randGen.nextInt()%10>2) {  //will this cause fluttering back and forth?
                                          if (randGen.nextbool() && canMove(NORTH)) monMove(NORTH);
                                          else if (canMove(SOUTH)) monMove(SOUTH);
                                          else if (canMove(NORTH)) monMove(NORTH);
                                          else { newai = GOTOWARDS; wasstuck=true; }
                                        }
                                        else { newai = GOTOWARDS; wasstuck=true; }
                                  }
                                  else { //not lined up yet, so move towards party
                                    //check x or y first depending on which is already closer (xdist or ydist smaller)
                                    if (xdist==ydist) {
                                        count = 0;
                                        dir=randGen.nextInt(4);
                                        while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                        if (count<4) monMove(dir);
                                        else movecounter=0; //can't move at all, can't attack, so don't try
                                    }
                                    else if (xdist<ydist) {
                                        if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else { randomcounter=3; newai=RANDOM; }
                                    }
                                    else {
                                        if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else { randomcounter=3; newai=RANDOM; }
                                    }
                                  }
                                  break;
                                  
                                case GUARD: //don't move at all, but attack and may use projs or heal spells
                                default:
                                  
                                  //make face towards party - if xdist greater than ydist, face east or west towards party
                                  //otherwise, face north or south towards party
                                  //could instead turn in place:
                                  //facing=(facing+1)%4; //turns in place
                                  
                                  if (hasheal && !silenced && (health < .2*maxhealth) && mana>59) useHealMagic();//heal self if hurting (if can)
                                  else if (level!=dmnew.level) movecounter=0;
                                  else if (canattack) doAttack(); //do attack or use close-range magic
                                  else if ((x==partyx && ydist>1) && (ammo>0 || (hasmagic && !silenced && mana>=minproj))) {
                                        if (y>partyy && canDoProj(NORTH)) doProjAttack(NORTH);
                                        else if (y<partyy && canDoProj(SOUTH)) doProjAttack(SOUTH);
                                        else if ((subsquare==0 || subsquare==3) && canMove(EAST)) monMove(EAST);
                                        else if ((subsquare==1 || subsquare==2) && canMove(WEST)) monMove(WEST);
                                        else movecounter = 0;
                                  }
                                  else if ((y==partyy && xdist>1) && (ammo>0 || (hasmagic && !silenced && mana>=minproj))) {
                                        if (x<partyx && canDoProj(EAST)) doProjAttack(EAST);
                                        else if (x>partyx && canDoProj(WEST)) doProjAttack(WEST);
                                        else if ((subsquare==0 || subsquare==1) && canMove(SOUTH)) monMove(SOUTH);
                                        else if ((subsquare==2 || subsquare==3) && canMove(NORTH)) monMove(NORTH);
                                        else movecounter = 0;
                                  }
                                  else if (xdist==0 && ydist==1) {
                                        if (y>partyy && canMove(NORTH)) monMove(NORTH);
                                        else if (y<partyy && canMove(SOUTH)) monMove(SOUTH);
                                        else movecounter = 0;
                                  }
                                  else if (ydist==0 && xdist==1) {
                                        if (x>partyx && canMove(WEST)) monMove(WEST);
                                        else if (x<partyx && canMove(EAST)) monMove(EAST);
                                        else movecounter = 0;
                                  }
                                  else movecounter = 0;
                                  if (level==dmnew.level && xdist<5 && ydist<5) {
                                        //if close, face party
                                        if (xdist!=0 && xdist>ydist) {
                                                if (partyx>x) facing=EAST;
                                                else facing=WEST;
                                        }
                                        else {
                                                if (partyy>y) facing=SOUTH;
                                                else facing=NORTH;
                                        }
                                        if (useammo && ammo==0) { defaultai=GOTOWARDS; newai=GOTOWARDS; } //run out of ammo turns guard into gotoward
                                  }
                                  break;
                                
                                case FRIENDLYMOVE:
                                  
                                  //move random direction if possible
                                  count = 0;
                                  dir=randGen.nextInt(4);
                                  while (!canMove(dir) && count<4) { count++; dir=(dir+1)%4; }
                                  if (count<4) monMove(dir);
                                  else {
                                        movecounter = 0; //can't move at all
                                        //face party if close
                                        if (level==dmnew.level && xdist<5 && ydist<5) {
                                                if (xdist==0) {
                                                        if (y>partyy) facing=NORTH;
                                                        else facing=SOUTH;
                                                }
                                                else if (ydist==0) {
                                                        if (x>partyx) facing=WEST;
                                                        else facing=EAST;
                                                }
                                        } 
                                  }

                                  break;

                           } //end switch
                           currentai = newai;
                           //if no longer near party, reset to random ai
                           if (currentai!=GUARD && currentai!=FRIENDLYMOVE && xdist>4 && ydist>4 && (currentai!=RANDOM || randomcounter>0)) { randomcounter=0; currentai=RANDOM; }
                        } //end while
                        //if (wasstuck && currentai==GOTOWARDS && defaultai==STAYBACK) { wasstuck=false; currentai=STAYBACK; } //reset to stayback from gotowards (will be set again next time if still can't move)
                }
                
                public bool canMove(int d) {
                        int xadjust=x,yadjust=y;
                        int newsub = 5;
                        if (d==NORTH || d==SOUTH) {
                                if (subsquare==0) newsub = 3;
                                else if (subsquare==3) newsub = 0;
                                else if (subsquare==1) newsub = 2;
                                else if (subsquare==2) newsub = 1;
                                if (d==NORTH && (newsub==3 || newsub==2 || newsub==5)) yadjust--;
                                else if (d==SOUTH && (newsub==0 || newsub==1 || newsub==5)) yadjust++;
                        }
                        else {
                                if (subsquare==0) newsub = 1;
                                else if (subsquare==1) newsub = 0;
                                else if (subsquare==2) newsub = 3;
                                else if (subsquare==3) newsub = 2;
                                if (d==EAST && (newsub==0 || newsub==3 || newsub==5)) xadjust++;
                                else if (d==WEST && (newsub==1 || newsub==2 || newsub==5)) xadjust--;
                        }
                        if (xadjust<0 || yadjust<0 || xadjust>=mapwidth || yadjust>=mapheight) return false;
                        //if (d==NORTH) yadjust--;
                        //else if (d==WEST) xadjust--;
                        //else if (d==SOUTH) yadjust++;
                        //else xadjust++;

                        bool movetest=true;
                        if ((!DungeonMap[level][xadjust][yadjust].isPassable && !isImmaterial) || DungeonMap[level][xadjust][yadjust].hasParty || DungeonMap[level][xadjust][yadjust].numProjs>0 || DungeonMap[level][x][y].numProjs>0) movetest=false;
                        else if (DungeonMap[level][xadjust][yadjust].hasMons && (newsub==5 || dmmons.get(level+","+xadjust+","+yadjust+","+newsub)!=null || dmmons.get(level+","+xadjust+","+yadjust+","+5)!=null)) movetest = false;
                        //else if (xadjust<1 || xadjust>(mapwidth-2) || yadjust<1 || yadjust>(mapheight-2)) movetest = false;
                        //else if (xadjust<0 || yadjust<0 || xadjust==mapwidth || yadjust==mapheight) movetest = false;
                        else if (!DungeonMap[level][xadjust][yadjust].canPassMons) movetest=false;
                        else if (isImmaterial && !DungeonMap[level][xadjust][yadjust].canPassImmaterial) movetest=false;
                        //newly added:
                        //else if (!isImmaterial && !ignoremons && currentai==RUN && DungeonMap[level][xadjust][yadjust].hasCloud && randGen.nextbool()) movetest=false;
                        else if (!isImmaterial && !ignoremons && DungeonMap[level][xadjust][yadjust].hasCloud && randGen.nextbool()) movetest=false;
                        //else if (!isflying && DungeonMap[level][xadjust][yadjust] instanceof Pit && (((Pit)DungeonMap[level][xadjust][yadjust]).isOpen || (!ignoremons && ((Pit)DungeonMap[level][xadjust][yadjust]).isActive))) movetest=false;//don't step into open pit (unless can fly)
                        else if (!isflying && (DungeonMap[level][xadjust][yadjust] instanceof FulYaPit || (DungeonMap[level][xadjust][yadjust] instanceof Pit && (((Pit)DungeonMap[level][xadjust][yadjust]).isOpen || (!ignoremons && ((Pit)DungeonMap[level][xadjust][yadjust]).isActive))))) movetest=false;//don't step into open pit (unless can fly)
                        else if (!canusestairs && DungeonMap[level][xadjust][yadjust] instanceof Stairs) movetest=false;
                        //chaos and fluxcages:
                        else if (number==26 && fluxchanging && fluxcages.get(level+","+xadjust+","+yadjust)!=null) movetest=false;
                        //if smart enough, open a door that is blocking the way (only button type)
                        if (!movetest && DungeonMap[level][xadjust][yadjust] instanceof Door && !((Door)DungeonMap[level][xadjust][yadjust]).isOpen) {
                                if (!ignoremons && ((Door)DungeonMap[level][xadjust][yadjust]).opentype==Door.BUTTON) { 
                                        ((Door)DungeonMap[level][xadjust][yadjust]).activate();
                                        if (!DungeonMap[level][xadjust][yadjust].hasMons && !DungeonMap[level][xadjust][yadjust].hasParty && DungeonMap[level][xadjust][yadjust].numProjs==0 && DungeonMap[level][x][y].numProjs==0) movetest = true;
                                }
                                //or, try to break it down (only some mons) if isBreakable and is wooden (pictype==0)
                                else if (!breakdoor && (number==8 || number==13 || number==20 || number==21 || number==23 || number==25) && ((Door)DungeonMap[level][xadjust][yadjust]).isBreakable && ((Door)DungeonMap[level][xadjust][yadjust]).pictype==0) { 
                                        breakdoor = true;
                                        if (level==dmnew.level) { facing=d; isattacking = true; needredraw=true; }
                                        ((Door)DungeonMap[level][xadjust][yadjust]).breakDoor(power+randGen.nextInt(power/4+10),true,false);
                                        if (((Door)DungeonMap[level][xadjust][yadjust]).isBroken && !DungeonMap[level][xadjust][yadjust].hasMons && !DungeonMap[level][xadjust][yadjust].hasParty && DungeonMap[level][xadjust][yadjust].numProjs==0 && DungeonMap[level][x][y].numProjs==0) movetest = true;
                                }
                        }
                        return movetest;
                }

                public void monMove(int d) {
                        
                        parry=-1;
                        int xadjust=x,yadjust=y;
                        int newsub = 5;
                        if (d==NORTH || d==SOUTH) {
                                if (subsquare==0) newsub = 3;
                                else if (subsquare==3) newsub = 0;
                                else if (subsquare==1) newsub = 2;
                                else if (subsquare==2) newsub = 1;
                                if (d==NORTH && (newsub==3 || newsub==2 || newsub==5)) yadjust--;
                                else if (d==SOUTH && (newsub==0 || newsub==1 || newsub==5)) yadjust++;
                        }
                        else {
                                if (subsquare==0) newsub = 1;
                                else if (subsquare==1) newsub = 0;
                                else if (subsquare==2) newsub = 3;
                                else if (subsquare==3) newsub = 2;
                                if (d==EAST && (newsub==0 || newsub==3 || newsub==5)) xadjust++;
                                else if (d==WEST && (newsub==1 || newsub==2 || newsub==5)) xadjust--;
                        }

                        facing = d;

                        movecounter=0;

                        xdist = x-partyx; if (xdist<0) xdist*=-1;
                        ydist = y-partyy; if (ydist<0) ydist*=-1;
                        if (level==dmnew.level && xdist<6 && ydist<6) needredraw = true;
                        
                        //if just broke door, don't move into it
                        if (breakdoor) return;
                        //if just opened door, don't move into it
                        if (!isImmaterial && DungeonMap[level][xadjust][yadjust].mapchar=='d' && !DungeonMap[level][xadjust][yadjust].isPassable) return;
                        
                        int oldlevel = level, oldx = x; int oldy = y, oldsub = subsquare, oldface = facing;
                        MapObject newsquare = DungeonMap[level][xadjust][yadjust];
                        bool trytel = false;//, imflytest = false;
                        //remove mon from current square (and subsquare)
                        dmmons.remove(level+","+x+","+y+","+subsquare);
                        //put mon on new square (could be same square but different subsquare)
                        x = xadjust; y = yadjust; subsquare=newsub;
                        dmmons.put(level+","+xadjust+","+yadjust+","+subsquare,this);
                        DungeonMap[level][xadjust][yadjust].hasMons=true;
                        //if moved to different square (not just to different subsquare), test switch step off
                        if (x!=oldx || y!=oldy) {
                                trytel = true;
                                Monster tempmon;
                                bool montest = false;
                                int i=0;
                                while (i<6 && !montest) {
                                        tempmon = (Monster)dmmons.get(level+","+oldx+","+oldy+","+i);
                                        if (tempmon!=null) montest=true;
                                        if (i==3) i=5;
                                        else i++;
                                }
                                DungeonMap[level][oldx][oldy].hasMons=montest;
                                
                                //try switch stepping off unless this mon is flying
                                if (!isflying) {
                                        DungeonMap[level][oldx][oldy].tryFloorSwitch(MapObject.MONSTEPPINGOFF);
                                }
                        }
                        else if (DungeonMap[level][x][y].mapchar=='>') trytel = true;
                        //if still alive and haven't moved (stepping off switch could have caused death or teleport swap to happen)
                        if (!isdying && x==xadjust && y==yadjust && level==oldlevel) {
                                
                                //if moved to different square (not just to different subsquare) and new square has not changed, test switch step on
                                if (!isflying && newsquare==DungeonMap[level][xadjust][yadjust] && (x!=oldx || y!=oldy)) {
                                        //System.out.println("step on test");
                                        DungeonMap[level][x][y].tryFloorSwitch(MapObject.MONSTEPPINGON);
                                }
                
                                //test teleport unless was moved/killed by previous switches creating a teleport/pit or newsquare changed
                                if (trytel && !isdying && x==xadjust && y==yadjust && level==oldlevel && facing==oldface && newsquare==DungeonMap[level][xadjust][yadjust]) {
                                        DungeonMap[level][x][y].tryTeleport(this);
                                }
                                
                                //maybe pick up item on the ground if not dying (don't care here if we got teleported)
                                if (!isdying && pickup!=0 && DungeonMap[level][x][y].hasItems && (pickup==4 || (pickup==2 && randGen.nextbool()) || (pickup==1 && randGen.nextInt(4)==0) || (pickup==3 && randGen.nextInt(4)!=0))) {
                                        Item tempitem;
                                        if (subsquare<4) tempitem = DungeonMap[level][x][y].pickUpItem(subsquare);
                                        else {
                                                tempitem = (Item)DungeonMap[level][x][y].mapItems.remove(DungeonMap[level][x][y].mapItems.size()-1);
                                                if (DungeonMap[level][x][y].mapItems.isEmpty()) DungeonMap[level][x][y].hasItems=false;
                                        }
                                        if (tempitem!=null) {
                                                carrying.add(tempitem);
                                                if (useammo && ((tempitem.number>220 && (tempitem.hasthrowpic || tempitem.number==266)) || (tempitem.projtype>0 && tempitem.weight<=1.0) )) ammo++;
                                                DungeonMap[level][x][y].tryFloorSwitch(MapObject.TOOKITEM); //might activate a switch
                                        }
                                }
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<6 && ydist<6) {
                                        needredraw = true;
                                        if (attackspeed>0 && ((xdist==0 && ydist==1) || (ydist==0 && xdist==1))) movecounter = attackspeed;
                                        if (!sleeping && !footstep.equals("")) playFootStep(footstep,x,y);//footstep sound (not played when sleeping -> sounds weird because so fast)
                                }
                                if (movespeed-movecounter<2) moveattack = moncycle;//prevents double updates (since removed from dmmons and added again while iterating through it)
                        }
                }

                public void doAttack() {
                        //put close range magic here? (that is, if ever is such a thing)
                        
                        //make sure mon is facing party
                        //later change to turn only 90 deg at a time - inc or dec facing like in key listener
                        // -- may need to reset movecounter a little if turns too fast...
                        bool noattack = false;
                        if (partyx>x && facing!=EAST) { facing=EAST; noattack=true; }
                        else if (partyx<x && facing!=WEST) { facing=WEST; noattack=true; }
                        else if (partyy>y && facing!=SOUTH) { facing=SOUTH; noattack=true; }
                        else if (partyy<y && facing!=NORTH) { facing=NORTH; noattack=true; }
                        else if (alldead) noattack=true;
                        if (waitattack && noattack && !alldead) { noattack=false; waitattack=false; }

                        if (!noattack) {
                                
                                //maybe do magic
                                if (hasmagic && !silenced && mana>=minproj && randGen.nextInt()%10>0) { 
                                        if (x<partyx && canDoProj(EAST)) { doProjAttack(EAST); return; }
                                        else if (x>partyx && canDoProj(WEST)) { doProjAttack(WEST); return; }
                                        else if (y>partyy && canDoProj(NORTH)) { doProjAttack(NORTH); return; } 
                                        else if (y<partyy && canDoProj(SOUTH)) { doProjAttack(SOUTH); return; }
                                }
                                else if (hasdrain && !silenced && mana>=minproj && health<maxhealth) {
                                        //drain life from a hero
                                        int who,whoalt,whotest1,whotest2,whotest3,whotest4;
                                        if (facing==NORTH) { whotest1=2; whotest2=3; whotest3=0; whotest4=1; }
                                        else if (facing==SOUTH) { whotest1=0; whotest2=1; whotest3=2; whotest4=3; }
                                        else if (facing==EAST) { whotest1=0; whotest2=3; whotest3=1; whotest4=2; }
                                        else { whotest1=1; whotest2=2; whotest3=0; whotest4=3; }
                                        if (randGen.nextbool()) {
                                                who = (whotest1+dmnew.facing)%4;
                                                whoalt = (whotest2+dmnew.facing)%4;
                                        }
                                        else {
                                                who = (whotest2+dmnew.facing)%4;
                                                whoalt = (whotest1+dmnew.facing)%4;
                                        }
                                        if (heroatsub[who]==-1) {
                                                who = whoalt;
                                                if (heroatsub[who]==-1) {
                                                        if (randGen.nextbool()) {
                                                                who = (whotest3+dmnew.facing)%4;
                                                                whoalt = (whotest4+dmnew.facing)%4;
                                                        }
                                                        else {
                                                                who = (whotest4+dmnew.facing)%4;
                                                                whoalt = (whotest3+dmnew.facing)%4;
                                                        }
                                                        if (heroatsub[who]==-1) who=whoalt;
                                                }
                                        }
                                        who = heroatsub[who];
                                        int spellcost;
                                        int spellpower = castpower+1;
                                        do {
                                                spellpower--;
                                                spellcost = spellpower;
                                                for (int i=0;i<3;i++) {
                                                    spellcost+=spellpower*6*i;
                                                }
                                        }
                                        while (spellcost>mana && spellpower>=0);
                                        try { 
                                                Spell s = new Spell(spellpower+"666");
                                                s.powers[s.gain-1]+= randGen.nextInt()%10+(s.gain-1)*manapower/8;
                                                s.power = s.powers[s.gain-1];
                                                if (s.power<1) s.power=randGen.nextInt(4)+1;
                                                int hit = hero[who].damage(s.power,DRAINHIT);
                                                //System.out.println("drain life: old health = "+health+", new = "+(health+hit));
                                                if (hit>0) { heal(hit); message.setMessage(name+" drains "+hit+" life from "+hero[who].name,5); }
                                                else if (hit!=0) damage(hit,SPELLHIT);
                                                //put drain sound here instead - if no sound, put message instead
                                                //if (soundstring!=null) playSound(soundstring,x,y);
                                                playSound("drain.wav",x,y);
                                                iscasting=true;
                                                movecounter = attackspeed;
                                                needredraw = true;
                                                playSound("oof.wav",-1,-1);
                                                return;
                                        }
                                        catch(Exception e) {}
                                }
                                if (soundstring!=null) playSound(soundstring,x,y);
                                isattacking = true;
                                movecounter = attackspeed; //put this here, or after if {}
                        }
                        else { movecounter = movespeed-2; if (movecounter<0) movecounter=0; }
                        needredraw = true;
                }
                

                public bool canDoProj(int d) {
                        //check map between mon and party to see if can pass projs (that way know if can fire proj at party)
                        //also check if party in range if throwing items
                        int testval,tempsub1=0,tempsub2=1;
                        bool cancast = true, subtried=false, hashero1 = true, hashero2 = true;
                        switch (d) {
                            
                            case NORTH:
                                
                                if (!hasmagic || mana<minproj) {
                                        if (ydist>4 && ydist*2>((power+5)/8+1)) return false;
                                }
                                else if (ydist>4 && ydist*2>(castpower*5+3)) return false;
                                testval = y;
                                //if (!ignoremons) {
                                     castsub = 0;
                                     tempsub1 = 0;
                                     tempsub2 = 3;
                                     if (subsquare>0 && subsquare<3) { tempsub1 = 1; tempsub2 = 2; }
                                     if (heroatsub[(tempsub1+dmnew.facing)%4]==-1 && heroatsub[(tempsub2+dmnew.facing)%4]==-1) { cancast = false; hashero1=false; }
                                     if (subsquare==5) { 
                                        hashero2 = (heroatsub[(1+dmnew.facing)%4]!=-1 || heroatsub[(2+dmnew.facing)%4]!=-1);
                                        if (hashero2 && (!cancast || randGen.nextbool())) { tempsub1 = 1; tempsub2 = 2; cancast = true; castsub = 1; }
                                     }
                                     if (!cancast) return false;
                                //}
                                if (subsquare==0 || subsquare==1 || subsquare==5 || ignoremons) testval--;
                                Monster tempmon;
                                while(cancast && testval>partyy) {
                                     if (!DungeonMap[level][x][testval].canPassProjs) cancast=false;
                                     else if (!ignoremons) {
                                        tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+5);
                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || 5!=subsquare)) cancast = false;
                                        else {
                                                tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+tempsub1);
                                                if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || tempsub1!=subsquare)) cancast = false;
                                                else {
                                                        tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+tempsub2);
                                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || tempsub2!=subsquare)) cancast = false;
                                                }
                                        }
                                        //if (dmmons.get(level+","+x+","+testval+","+tempsub1)!=null && (testval!=y || tempsub1!=subsquare)) cancast=false;
                                        //else if (dmmons.get(level+","+x+","+testval+","+tempsub2)!=null && (testval!=y || tempsub2!=subsquare)) cancast=false;
                                        if (!cancast && subsquare==5 && !subtried) {
                                                if (castsub==0 && hashero2) { cancast = true; castsub=1; tempsub1 = 1; tempsub2 = 2; }
                                                else if (hashero1) { cancast = true; castsub=0; tempsub1 = 0; tempsub2 = 3; }
                                                subtried=true; testval = y;
                                        }
                                     }
                                     testval--;
                                }
                                break;
                            
                            case SOUTH:
                            
                                if (!hasmagic || mana<minproj) {
                                        if (ydist>4 && ydist*2>((power+5)/8+1)) return false;
                                }
                                else if (ydist>4 && ydist*2>(castpower*5+3)) return false;
                                testval = y;
                                //if (!ignoremons) {
                                     castsub = 3;
                                     tempsub1 = 0;
                                     tempsub2 = 3;
                                     if (subsquare>0 && subsquare<3) { tempsub1 = 1; tempsub2 = 2; }
                                     if (heroatsub[(tempsub1+dmnew.facing)%4]==-1 && heroatsub[(tempsub2+dmnew.facing)%4]==-1) { cancast = false; hashero1=false; }
                                     if (subsquare==5) { 
                                        hashero2 = (heroatsub[(1+dmnew.facing)%4]!=-1 || heroatsub[(2+dmnew.facing)%4]!=-1);
                                        if (hashero2 && (!cancast || randGen.nextbool())) { tempsub1 = 1; tempsub2 = 2; cancast = true; castsub = 2; }
                                     }
                                     if (!cancast) return false;
                                //}
                                if (subsquare==2 || subsquare==3 || subsquare==5 || ignoremons) testval++;
                                while(cancast && testval<partyy) {
                                     if (!DungeonMap[level][x][testval].canPassProjs) cancast=false;
                                     else if (!ignoremons) {
                                        tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+5);
                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || 5!=subsquare)) cancast = false;
                                        else {
                                                tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+tempsub1);
                                                if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || tempsub1!=subsquare)) cancast = false;
                                                else {
                                                        tempmon = (Monster)dmmons.get(level+","+x+","+testval+","+tempsub2);
                                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=y || tempsub2!=subsquare)) cancast = false;
                                                }
                                        }
                                        //if (dmmons.get(level+","+x+","+testval+","+tempsub1)!=null && (testval!=y || tempsub1!=subsquare)) cancast=false;
                                        //else if (dmmons.get(level+","+x+","+testval+","+tempsub2)!=null && (testval!=y || tempsub2!=subsquare)) cancast=false;
                                        if (!cancast && subsquare==5 && !subtried) {
                                                if (castsub==3 && hashero2) { cancast = true; castsub=2; tempsub1 = 1; tempsub2 = 2; }
                                                else if (hashero1) { cancast = true; castsub=3; tempsub1 = 0; tempsub2 = 3; }
                                                subtried=true; testval = y;
                                        }
                                     }
                                     testval++;
                                }
                                break;
                                
                            case EAST:
                                
                                if (!hasmagic || mana<minproj) {
                                        if (xdist>4 && xdist*2>((power+5)/8+1)) return false;
                                }
                                else if (xdist>4 && xdist*2>(castpower*5+3)) return false;
                                testval = x;
                                //if (!ignoremons) {
                                     castsub = 1;
                                     tempsub1 = 0;
                                     tempsub2 = 1;
                                     if (subsquare>1 && subsquare!=5) { tempsub1 = 2; tempsub2 = 3; }
                                     if (heroatsub[(tempsub1+dmnew.facing)%4]==-1 && heroatsub[(tempsub2+dmnew.facing)%4]==-1) { cancast = false; hashero1=false; }
                                     if (subsquare==5) { 
                                        hashero2 = (heroatsub[(2+dmnew.facing)%4]!=-1 || heroatsub[(3+dmnew.facing)%4]!=-1);
                                        if (hashero2 && (!cancast || randGen.nextbool())) { cancast = true; castsub = 2; tempsub1 = 2; tempsub2 = 3; }
                                     }
                                     if (!cancast) return false;
                                //}
                                if (subsquare==1 || subsquare==2 || subsquare==5 || ignoremons) testval++;
                                while(cancast && testval<partyx) {
                                     if (!DungeonMap[level][testval][y].canPassProjs) cancast=false;
                                     else if (!ignoremons) {
                                        tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+5);
                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || 5!=subsquare)) cancast = false;
                                        else {
                                                tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+tempsub1);
                                                if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || tempsub1!=subsquare)) cancast = false;
                                                else {
                                                        tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+tempsub2);
                                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || tempsub2!=subsquare)) cancast = false;
                                                }
                                        }
                                        //if (dmmons.get(level+","+testval+","+y+","+tempsub1)!=null && (testval!=x || tempsub1!=subsquare)) cancast=false;
                                        //else if (dmmons.get(level+","+testval+","+y+","+tempsub2)!=null && (testval!=x || tempsub2!=subsquare)) cancast=false;
                                        if (!cancast && subsquare==5 && !subtried) {
                                                if (castsub==1 && hashero2) { cancast = true; castsub=2; tempsub1 = 2; tempsub2 = 3; }
                                                else if (hashero1) { cancast = true; castsub=1; tempsub1 = 0; tempsub2 = 1; }
                                                subtried=true; testval = x;
                                        }
                                     }
                                     testval++;
                                }
                                break;
                                
                            case WEST:
                            
                                if (!hasmagic || mana<minproj) {
                                        if (xdist>4 && xdist*2>((power+5)/8+1)) return false;
                                }
                                else if (xdist>4 && xdist*2>(castpower*5+3)) return false;
                                testval = x;
                                //if (!ignoremons) {
                                     castsub = 0;
                                     tempsub1 = 0;
                                     tempsub2 = 1;
                                     if (subsquare>1 && subsquare!=5) { tempsub1 = 2; tempsub2 = 3; }
                                     if (heroatsub[(tempsub1+dmnew.facing)%4]==-1 && heroatsub[(tempsub2+dmnew.facing)%4]==-1) { cancast = false; hashero1=false; }
                                     if (subsquare==5) { 
                                        hashero2 = (heroatsub[(2+dmnew.facing)%4]!=-1 || heroatsub[(3+dmnew.facing)%4]!=-1);
                                        if (hashero2 && (!cancast || randGen.nextbool())) { cancast = true; castsub = 3; tempsub1 = 2; tempsub2 = 3; }
                                     }
                                     if (!cancast) return false;
                                //}
                                if (subsquare==0 || subsquare==3 || subsquare==5 || ignoremons) testval--;
                                while(cancast && testval>partyx) {
                                     if (!DungeonMap[level][testval][y].canPassProjs) cancast=false;
                                     else if (!ignoremons) {
                                        tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+5);
                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || 5!=subsquare)) cancast = false;
                                        else {
                                                tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+tempsub1);
                                                if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || tempsub1!=subsquare)) cancast = false;
                                                else {
                                                        tempmon = (Monster)dmmons.get(level+","+testval+","+y+","+tempsub2);
                                                        if (tempmon!=null && !tempmon.isdying && !tempmon.isImmaterial && (testval!=x || tempsub2!=subsquare)) cancast = false;
                                                }
                                        }
                                        //if (dmmons.get(level+","+testval+","+y+","+tempsub1)!=null && (testval!=x || tempsub1!=subsquare)) cancast=false;
                                        //else if (dmmons.get(level+","+testval+","+y+","+tempsub2)!=null && (testval!=x || tempsub2!=subsquare)) cancast=false;
                                        if (!cancast && subsquare==5 && !subtried) {
                                                if (castsub==0 && hashero2) { cancast = true; castsub=3; tempsub1 = 2; tempsub2 = 3; }
                                                else if (hashero1) { cancast = true; castsub=0; tempsub1 = 0; tempsub2 = 1; }
                                                subtried=true; testval = x;
                                        }
                                     }
                                     testval--;
                                }
                                break;
                                
                        }//end switch
                        //System.out.println("sub = "+subsquare+", cancast "+d+" = "+cancast+", castsub = "+castsub);
                        return cancast;
                }
                
                //magic and projectile weapons (bows, daggers, etc)
                public void doProjAttack(int dir) {
                        Projectile p;
                        int projsub = (subsquare+dmnew.facing)%4;
                        if (subsquare==5) {
                                projsub = (castsub+dmnew.facing)%4;
                        }
                        if (hasmagic && !silenced && mana>=minproj && (ammo==0 || randGen.nextbool())) {
                           bool foundspell = false;
                           int spellcost=0, spellpower=0;
                           int spellpick = randGen.nextInt(numspells);
                           while (!foundspell) { 
                                   spellpower=castpower;
                                   do {
                                        spellcost = spellpower;
                                        for (int i=0;i<knownspells[spellpick].length();i++) {
                                            //spellcost+=spellpower*Integer.parseInt(knownspells[spellpick].substring(i,i+1));
                                            //spellcost+=spellpower*Integer.parseInt(knownspells[spellpick].substring(i,i+1))*i;
                                            spellcost+=SYMBOLCOST[Integer.parseInt(knownspells[spellpick].substring(i,i+1))-1+i*6][spellpower-1];
                                        }
                                        //System.out.println("number = "+knownspells[spellpick]+", power = "+spellpower+", cost = "+spellcost);
                                        spellpower--;
                                        if (number==25) spellcost=0; //dragon breathes fire, doesn't pay to cast it
                                   }
                                   while (spellcost>mana && spellpower>0);
                                   if (spellcost<=mana) foundspell = true;
                                   else spellpick=(spellpick+1)%numspells;
                           }
                           //if (MONSTERSOUNDS[number]!=null) playSound(MONSTERSOUNDS[number],x,y);
                           if (soundstring!=null) playSound(soundstring,x,y);
                           Spell tempspell;
                           try { 
                               tempspell = new Spell((spellpower+1)+knownspells[spellpick]);
                               if (tempspell.number!=461 && tempspell.number!=363 && tempspell.number!=362 && tempspell.number!=664 && tempspell.number!=523) {
                                       for (int j=tempspell.gain-1;j>=0;j--) {
                                           tempspell.powers[j]+= randGen.nextInt()%10+j*manapower/8;
                                           if (tempspell.powers[j]<1) tempspell.powers[j]=randGen.nextInt(4)+1;
                                       }
                                       tempspell.power = tempspell.powers[tempspell.gain-1];
                               }
                               p = new Projectile(tempspell,x,y,tempspell.dist,dir,projsub);
                               mana-=spellcost;
                           }
                           catch(Exception e) { e.printStackTrace(); movecounter=attackspeed; }
                        }
                        else {
                           if (soundstring!=null) playSound(soundstring,x,y);
                           Item wep = null;
                           int index = 0;
                           bool found = false;
                           while (!found) {
                                wep = (Item)carrying.get(index);
                                if (wep.type==Item.WEAPON && ((wep.number>220 && (wep.hasthrowpic || wep.number==266)) || (wep.projtype>0 && wep.weight<=1.0))) {
                                        carrying.remove(index);
                                        found=true;
                                }
                                else index++;
                           }
                           ammo--;
                           wep.shotpow = castpower;
                           int dist = (power+5)/5-(int)(wep.weight/2.0f)+randGen.nextInt()%2;
                           if (dist<8) dist = 8;
                           if (wep.isbomb) {
                                try { 
                                    Spell tempspell = new Spell(wep.bombnum);
                                    tempspell.power = wep.potionpow + randGen.nextInt()%10;
                                    p = new Projectile(tempspell,x,y,dist,dir,projsub);
                                }
                                catch(Exception e) { p = new Projectile(wep,x,y,dist,dir,projsub); }
                           }
                           else p = new Projectile(wep,x,y,dist,dir,projsub);
                        }
                        
                        //could change this to turn 90 deg at a time, in doattack too
                        //bool noattack = false;
                        if (partyx>x && facing!=EAST) { facing=EAST; }//noattack=true; }
                        else if (partyx<x && facing!=WEST) { facing=WEST; }//noattack=true; }
                        else if (partyy>y && facing!=SOUTH) { facing=SOUTH; }//noattack=true; }
                        else if (partyy<y && facing!=NORTH) { facing=NORTH; }//noattack=true; }
                        
                        //if (!noattack) {
                           iscasting = true;
                           needredraw = true;
                           movecounter = attackspeed;
                           //System.out.println(name+" casts a proj, ai = "+currentai);
                        //}
                        //else movecounter = 0;
                }
                
                public void useHealMagic() {
                        //playSound("moncast.wav",x,y);
                        int spellpower = mana/60;
                        if (spellpower>castpower) spellpower=castpower;
                        heal(spellpower*(randGen.nextInt(manapower/2)+manapower/2));
                        energize(spellpower*-60);
                        iscasting = true;
                        if (level==dmnew.level) needredraw = true;
                        movecounter = attackspeed;
                        //System.out.println(name+" casts a heal spell -> health = "+health);
                }
                
                public bool teleport() {
                        //range 5 squares on all sides of mon
                        int newx,newy,xchng,ychng,count=121;
                        /*
                        //where go depends on facing
                        if (facing==0) { newx=x-5; newy=y-5; xchng=1; ychng=1; }
                        else if (facing==2) { newx=x+5; newy=y+5; xchng=-1; ychng=-1; }
                        else if (facing==1) { newx=x-5; newy=y+5; xchng=1; ychng=-1; }
                        else { newx=x+5; newy=y-5; xchng=-1; ychng=1; }
                        */
                        //where go random
                        int way = randGen.nextInt(4);
                        if (way==0) { newx=x-5; newy=y-5; xchng=1; ychng=1; }
                        else if (way==2) { newx=x+5; newy=y+5; xchng=-1; ychng=-1; }
                        else if (way==1) { newx=x-5; newy=y+5; xchng=1; ychng=-1; }
                        else { newx=x+5; newy=y-5; xchng=-1; ychng=1; }
                        //find a square that is safe to teleport to
                        do {
                                newx+=xchng;
                                if (newx>x+5) {
                                     newx=x-5;
                                     newy+=ychng;
                                }
                                else if (newx<x-5) {
                                     newx=x+5;
                                     newy+=ychng;
                                }
                                count--;
                        }
                        //while (count>0 && (newx<0 || newy<0 || newx>=mapwidth || newy>=mapheight || !DungeonMap[level][newx][newy].isPassable || DungeonMap[level][newx][newy].hasMons || DungeonMap[level][newx][newy].hasParty || !DungeonMap[level][newx][newy].canPassMons || DungeonMap[level][newx][newy].mapchar=='>' || (newx==x && newy==y) || (number==26 && fluxcages.get(level+","+newx+","+newy)!=null)));
                        while (count>0 && (newx<0 || newy<0 || newx>=mapwidth || newy>=mapheight || DungeonMap[level][newx][newy].hasMons || DungeonMap[level][newx][newy].hasParty || !DungeonMap[level][newx][newy].canPassMons || DungeonMap[level][newx][newy].mapchar=='>' || (DungeonMap[level][newx][newy].mapchar=='y' && !isflying) || (!isImmaterial && !DungeonMap[level][newx][newy].isPassable) || (isImmaterial && !DungeonMap[level][newx][newy].canPassImmaterial) || (!isflying && DungeonMap[level][newx][newy].mapchar=='p' && ((Pit)DungeonMap[level][newx][newy]).isOpen) || (newx==x && newy==y) || (number==26 && fluxcages.get(level+","+newx+","+newy)!=null)));
                        if (count!=0) {
                                //successfully teleported
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5) {
                                        needredraw = true;
                                }
                                int oldx = x, oldy = y, oldlevel = level, oldfacing = facing;
                                MapObject newsquare = DungeonMap[level][newx][newy];
                                //remove mon from old square
                                dmmons.remove(level+","+x+","+y+","+subsquare);
                                Monster tempmon;
                                bool montest = false;
                                int i=0;
                                while (i<6 && !montest) {
                                        tempmon = (Monster)dmmons.get(level+","+x+","+y+","+i);
                                        if (tempmon!=null) montest=true;
                                        if (i==3) i=5;
                                        else i++;
                                }
                                DungeonMap[level][x][y].hasMons=montest;
                                //put mon on new square
                                x = newx; y = newy;
                                dmmons.put(level+","+x+","+y+","+subsquare,this);
                                DungeonMap[level][x][y].hasMons=true;
                                //try switch stepping off unless this mon is flying
                                if (!isflying) {
                                        DungeonMap[level][oldx][oldy].tryFloorSwitch(MapObject.MONSTEPPINGOFF);
                                }
                                //if still alive and haven't moved (stepping off switch could have caused death or teleport swap to happen) and new square hasn't changed
                                if (!isdying && x==newx && y==newy && level==oldlevel && newsquare==DungeonMap[level][newx][newy]) {
                                        
                                        //try switch stepping on
                                        if (!isflying) {
                                                DungeonMap[level][x][y].tryFloorSwitch(MapObject.MONSTEPPINGON);
                                        }
                        
                                        //test teleport unless was moved/killed by previous switches creating a teleport/pit or newsquare changed
                                        if (!isdying && x==newx && y==newy && level==oldlevel && facing==oldfacing && newsquare==DungeonMap[level][newx][newy]) {
                                                DungeonMap[level][x][y].tryTeleport(this);
                                        }
                                }
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5) {
                                        needredraw = true;
                                }
                                movecounter = 0;
                                //System.out.println("Teleport to "+level+","+newx+","+newy+" which has mapchar "+DungeonMap[level][newx][newy].mapchar);
                                return true;
                                /*
                                if (!isflying && !imflytest) DungeonMap[level][x][y].tryFloorSwitch(MapObject.MONSTEPPINGOFF);
                                if (!isflying) DungeonMap[level][newx][newy].tryFloorSwitch(MapObject.MONSTEPPINGON);
                                x=newx;
                                y=newy;
                                dmmons.put(level+","+x+","+y+","+subsquare,this);
                                DungeonMap[level][x][y].hasMons=true;
                                
                                //test for redraw before possible teleport (by a teleport, that is) and after
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5) {
                                        needredraw = true;
                                }
                                DungeonMap[level][x][y].tryTeleport(this);
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5) {
                                        needredraw = true;
                                }
                                movecounter = 0;
                                //System.out.println("Teleport to "+level+","+newx+","+newy+" which has mapchar "+DungeonMap[level][newx][newy].mapchar);
                                return true;
                                */
                        }
                        //System.out.println("Can't Teleport");
                        return false;
                }

                public void heal(int h) {
                        health+=h;
                        if (health>maxhealth) health=maxhealth;
                }

                public void energize(int e) {
                        mana+=e;
                        if (mana>maxmana) mana=maxmana;
                        else if (mana<0) mana=0;
                }

                public int damage(int hit,int type) {
                        if (isdying || currentai>=FRIENDLYMOVE) return 0;
                        //check if need special item before can hurt
                        if (needitem!=0 && type!=STORMHIT && type!=FUSEHIT) {
                                bool found = false;
                                if (needhandneck==0) {
                                        //need in someone's hand
                                        for (int i=0;i<numheroes;i++) {
                                                if (hero[i].weapon.number==needitem || (hero[i].hand!=null && hero[i].hand.number==needitem)) found=true;
                                                //firestaff+ works for firestaff (but not other way around)
                                                else if (needitem==248 && hero[i].weapon.number==249 || (hero[i].hand!=null && hero[i].hand.number==249)) found=true;
                                                //stormbringer overrides
                                                else if (hero[i].weapon.number==215) found=true;
                                        }
                                }
                                else {
                                        //need around someone's neck
                                        for (int i=0;i<numheroes;i++) {
                                                if (hero[i].neck!=null && hero[i].neck.number==needitem) found=true;
                                        }
                                        //stormbringer overrides
                                        if (!found) for (int i=0;i<numheroes;i++) {
                                                if (hero[i].weapon.number==215) found=true;
                                        }
                                }
                                if (!found) { 
                                        if (type!=POISONHIT && type!=DOORHIT && level==dmnew.level) message.setMessage(name+" is unaffected.",4);
                                        return 0;
                                }
                        }
                        if (hurtitem!=0 && type!=WEAPONHIT && type!=STORMHIT && type!=FUSEHIT && type!=PROJWEAPONHIT) { 
                                if (type!=POISONHIT && type!=DOORHIT && level==dmnew.level) message.setMessage(name+" is unaffected.",4);
                                return 0;
                        }
                        if (type==WEAPONHIT || type==PROJWEAPONHIT || type==STORMHIT) hit-=(hit*defense/100);
                        else if (type==SPELLHIT || type==DRAINHIT) hit-=(hit*magicresist/100);
                        if (hit<1) hit=randGen.nextInt(4)+1;
                        else if (DIFFICULTY<0) hit = (int)(((double)hit)*(1.0-((double)DIFFICULTY)*.25));
                        if (type==DRAINHIT && (isImmaterial || number==8 || number==21)) {
                                //draining from immaterial mon, skeleton, or deth knight hurts you and heals them
                                health+=hit;
                                if (level==dmnew.level) message.setMessage(name+" gains some life.",5);
                                return 0-hit;
                        }
                        else {
                                health-=hit;
                                xdist = x-partyx; if (xdist<0) xdist*=-1;
                                ydist = y-partyy; if (ydist<0) ydist*=-1;
                                if (level==dmnew.level && xdist<5 && ydist<5 && type!=POISONHIT && type!=DOORHIT && type!=FUSEHIT) message.setMessage(name+" takes "+hit+" damage.",5);
                        }
                        //if (xdist<5 && ydist<5) {
                                //if (type!=POISONHIT) message.setMessage(name+" takes "+hit+" damage.",5);
                                //else message.setMessage(name+" takes "+hit+" poison damage.",5);
                        //}
                        if (health<1) {
                                isdying = true;
                                if (gamewin) {
                                        nomovement=true;
                                        isdying = false;
                                        if (type==FUSEHIT) {
                                                dmnew.endanim="fuseend.gif";
                                                //dmnew.endmusic="fuseend.mid";
                                                dmnew.endsound="fuseend.wav";
                                        }
                                        else {
                                                dmnew.endanim=endanim;
                                                //dmnew.endmusic=endmusic;
                                                dmnew.endsound=endsound;
                                        }
                                        gameover=true;
                                        return hit;
                                }
                                bool bigthunk = false;
                                Item it;
                                if (equipped!=null) {
                                        for (Iterator i=equipped.iterator();i.hasNext();) {
                                              it = (Item)i.next();
                                              if (it.type!=Item.WEAPON || it.weight>4.0f) bigthunk = true;
                                              if (subsquare!=5) it.subsquare = subsquare;
                                              else it.subsquare=randGen.nextInt(4);
                                              if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                                DungeonMap[level][x][y].addItem(it);
                                                DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                                              }
                                        }
                                }
                                for (Iterator i=carrying.iterator();i.hasNext();) {
                                      it = (Item)i.next();
                                      if (it.type!=Item.WEAPON || it.weight>4.0f) bigthunk = true;
                                      if (subsquare!=5) it.subsquare = subsquare;
                                      else it.subsquare=randGen.nextInt(4);
                                      if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                        DungeonMap[level][x][y].addItem(it);
                                        DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                                      }
                                }
                                if (level==dmnew.level && (!carrying.isEmpty() || (equipped!=null && !equipped.isEmpty()))) {
                                        if (bigthunk) playSound("thunk.wav",x,y);
                                        else playSound("dink.wav",x,y);
                                }
                                if (level==dmnew.level && xdist<5 && ydist<5) needredraw = true;
                                else {
                                  dmmons.remove(level+","+x+","+y+","+subsquare);
                                  bool montest = false;
                                  if (subsquare!=5) for (int sub=0;sub<3;sub++) if (dmmons.get(level+","+x+","+y+","+sub)!=null) montest=true;
                                  DungeonMap[level][x][y].hasMons=montest;
                                }
                                if (!isflying) DungeonMap[level][x][y].tryFloorSwitch(MapObject.MONSTEPPINGOFF);
                        }
                        else { 
                                if (type==DOORHIT && currentai!=RUN && !isImmaterial && randGen.nextbool() && (health<80 || randGen.nextInt(20)==0)) { 
                                        runcounter=2; currentai=RUN;
                                }
                                //if in poisoncloud, move unless already running or are immaterial (**currently move guard types**)
                                else if (fearresist>0 && currentai!=RUN && !isImmaterial && health<600 && DungeonMap[level][x][y].hasCloud && currentai!=FRIENDLYMOVE && currentai!=FRIENDLYNOMOVE && randGen.nextbool()) {
                                        bool willrun = false;
                                        if (randGen.nextInt(10)<fearresist || (fearresist>0 && health<40 && maxhealth<400 && randGen.nextbool())) {
                                                PoisonCloud tempcloud;
                                                Iterator i=cloudstochange.iterator();
                                                while (i.hasNext()) {
                                                        tempcloud=(PoisonCloud)i.next();
                                                        if (tempcloud.x==x && tempcloud.y==y) {
                                                                if (health<tempcloud.stage*100) willrun = true;
                                                                break;
                                                        }
                                                }
                                        }
                                        if (willrun) { runcounter=2; currentai=RUN; }
                                }
                        }
                        return hit;
                }
                
                public void pitDeath() {
                        isdying = true;
                        if (gamewin) {
                                nomovement=true;
                                dmnew.endanim=endanim;
                                //dmnew.endmusic=endmusic;
                                dmnew.endsound=endsound;
                                gameover=true;
                                return;
                        }
                        bool bigthunk = false;
                        Item it;
                        if (equipped!=null) {
                                for (Iterator i=equipped.iterator();i.hasNext();) {
                                      it = (Item)i.next();
                                      if (it.type!=Item.WEAPON || it.weight>4.0f) bigthunk = true;
                                      if (subsquare!=5) it.subsquare = subsquare;
                                      else it.subsquare=randGen.nextInt(4);
                                      if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                        DungeonMap[level][x][y].addItem(it);
                                        DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                                      }
                                }
                        }
                        for (Iterator i=carrying.iterator();i.hasNext();) {
                              it = (Item)i.next();
                              if (it.type!=Item.WEAPON || it.weight>4.0f) bigthunk = true;
                              if (subsquare!=5) it.subsquare = subsquare;
                              else it.subsquare=randGen.nextInt(4);
                              if (!DungeonMap[level][x][y].tryTeleport(it)) {
                                DungeonMap[level][x][y].addItem(it);
                                DungeonMap[level][x][y].tryFloorSwitch(MapObject.PUTITEM);
                              }
                        }
                        if (level==dmnew.level && (!carrying.isEmpty() || (equipped!=null && !equipped.isEmpty()))) {
                                if (bigthunk) playSound("thunk.wav",x,y);
                                else playSound("dink.wav",x,y);
                        }
                        if (level==dmnew.level && xdist<5 && ydist<5) needredraw = true;
                }

        }

        class Hero extends JPanel {//JComponent {//Canvas {
                string name;
                string lastname;
                string picname;
                Image pic;
                Item weapon = fistfoot;
                Item head = null;
                Item torso = null;
                Item legs = null;
                Item feet = null;
                Item hand = null;
                Item neck = null;
                Item pouch1 = null;
                Item pouch2 = null;
                Item[] pack = new Item[16];
                Item[] quiver = new Item[6];
                int heronumber;
                int subsquare;
                int number;
                int maxmana;
                int maxhealth;
                int maxstamina;
                int mana;
                int health;
                int stamina;
                int food;
                int water;
                int strength;
                int vitality;
                int dexterity;
                int intelligence;
                int wisdom;
                int defense;
                int magicresist;
                int flevel;
                int nlevel;
                int plevel;
                int wlevel;
                int fxp = 0;
                int nxp = 0;
                int pxp = 0;
                int wxp = 0;
                int strengthboost,intelligenceboost,wisdomboost,dexterityboost,vitalityboost,defenseboost,magicresistboost,flevelboost,nlevelboost,wlevelboost,plevelboost;
                float maxload,load;
                bool isleader = false;
                bool isdead = false;
                //bool splready = true;
                bool wepready = true;
                bool ispoisoned = false;
                bool silenced = false;
                bool hurthead=false,hurttorso=false,hurtlegs=false,hurtfeet=false,hurthand=false,hurtweapon=false;
                int silencecount = 0;
                int hit;
                int poison = 0;
                int spellcount = 0;
                int weaponcount = 0;
                int timecounter = 0;
                int hurtcounter = 0;
                int hitcounter = 0;
                int poisoncounter = 0;
                int walkcounter = 0; //for stamina drain
                int kuswordcount = 0;
                int rosbowcount = 0;
                int detectcurse = 0;
                int levelboostchange = 3;
                string currentspell = "";
                SpecialAbility[] abilities;
                Item abilitywep = null;
                int abilityactive=-1, abilitypoison, abilitywepnum, falseimage, berserk, berserkstr, berserkhealth;
                bool abilityimm, abilitystun, abilitydiamond, backstab;

                public Hero(string picname) {
                        //setSize(100,124);
                        setPreferredSize(new Dimension(100,128));//124));
                        setBackground(Color.black);
                        setBorder(BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED,new Color(60,60,80),new Color(20,20,40)));
                        if (picname.indexOf("Heroes")>=0) picname = picname.substring(picname.indexOf("Heroes")+7);
                        //System.out.println(picname);
                        this.picname = picname;
                        //pic = tk.getImage("Heroes"+File.separator+picname);
                        pic = tk.getImage("Heroes"+File.separator+picname);
                        //ImageTracker.addImage(pic,4);
                        //System.out.println(picname);
                }
                
                public Hero(string picn,string n, string ln, int fl, int nl, int wl, int pl, int h, int s, int m, int str, int dex, int vit, int intl, int wis, int def, int magr) {
                        //setSize(100,124);
                        setPreferredSize(new Dimension(100,128));//124));
                        setBackground(Color.black);
                        setBorder(BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED,new Color(60,60,80),new Color(20,20,40)));
                        picname = picn;
                        pic = tk.getImage("Heroes"+File.separator+picname);
                        //System.out.println("big constructor, "+picname);
                        name = n;
                        lastname = ln;
                        flevel = fl;
                        nlevel = nl;
                        wlevel = wl;
                        plevel = pl;
                        maxhealth = h;
                        health = h;
                        maxstamina = s;
                        stamina = s;
                        maxmana = m;
                        mana = m;
                        strength = str;
                        dexterity = dex;
                        vitality = vit;
                        intelligence = intl;
                        wisdom = wis;
                        defense = def;
                        magicresist = magr;
                        setMaxLoad();
                        load = 0.0f;
                        food = 1000;
                        water = 1000;
                }

                //conversion constructor
                public Hero(HeroData h) {
                        setPreferredSize(new Dimension(100,124));
                        setBackground(Color.black);
                        setBorder(BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED,new Color(60,60,80),new Color(20,20,40)));

                        name = new string(h.name);
                        lastname = new string(h.lastname);
                        picname = new string(h.picname);
                        pic = h.pic;
                        //setIcon(new ImageIcon(((ImageIcon)h.getIcon()).getImage()));
                        if (h.weapon!=Item.fistfoot) weapon = Item.createCopy(h.weapon);
                        else weapon = Item.fistfoot;
                        if (h.hand!=null) hand = Item.createCopy(h.hand);
                        if (h.head!=null) head = Item.createCopy(h.head);
                        if (h.neck!=null) neck = Item.createCopy(h.neck);
                        if (h.torso!=null) torso = Item.createCopy(h.torso);
                        if (h.legs!=null) legs = Item.createCopy(h.legs);
                        if (h.feet!=null) feet = Item.createCopy(h.feet);
                        if (h.pouch1!=null) pouch1 = Item.createCopy(h.pouch1);
                        if (h.pouch2!=null) pouch2 = Item.createCopy(h.pouch2);
                        for (int i=0;i<16;i++) {
                                if (h.pack[i]!=null) pack[i] = Item.createCopy(h.pack[i]);
                        }
                        for (int i=0;i<6;i++) {
                                if (h.quiver[i]!=null) quiver[i] = Item.createCopy(h.quiver[i]);
                        }
                        heronumber = h.heronumber;
                        subsquare = h.subsquare;
                        number = h.number;
                        maxmana = h.maxmana;
                        maxhealth = h.maxhealth;
                        maxstamina = h.maxstamina;
                        mana = h.mana;
                        health = h.health;
                        stamina = h.stamina;
                        food = h.food;
                        water = h.water;
                        strength = h.strength;
                        vitality = h.vitality;
                        dexterity = h.dexterity;
                        intelligence = h.intelligence;
                        wisdom = h.wisdom;
                        defense = h.defense;
                        magicresist = h.magicresist;
                        flevel = h.flevel;
                        nlevel = h.nlevel;
                        plevel = h.plevel;
                        wlevel = h.wlevel;
                        flevelboost = h.flevelboost;
                        nlevelboost = h.nlevelboost;
                        plevelboost = h.plevelboost;
                        wlevelboost = h.wlevelboost;
                        fxp = h.fxp;
                        nxp = h.nxp;
                        pxp = h.pxp;
                        wxp = h.wxp;
                        strengthboost = h.strengthboost;
                        intelligenceboost = h.intelligenceboost;
                        wisdomboost = h.wisdomboost;
                        dexterityboost = h.dexterityboost;
                        vitalityboost = h.vitalityboost;
                        defenseboost = h.defenseboost;
                        magicresistboost = h.magicresistboost;
                        maxload = h.maxload;
                        load = h.load;
                        isleader = h.isleader;
                        isdead = h.isdead;
                        wepready = h.wepready;
                        ispoisoned = h.ispoisoned;
                        silenced = h.silenced;
                        hurthead = h.hurthead;
                        hurttorso = h.hurttorso;
                        hurtlegs = h.hurtlegs;
                        hurtfeet = h.hurtfeet;
                        hurthand = h.hurthand;
                        hurtweapon = h.hurtweapon;
                        silencecount = h.silencecount;
                        poison = h.poison;
                        spellcount = h.spellcount;
                        weaponcount = h.weaponcount;
                        timecounter = h.timecounter;
                        poisoncounter = h.poisoncounter;
                        walkcounter = h.walkcounter;
                        kuswordcount = h.kuswordcount;
                        rosbowcount = h.rosbowcount;
                        currentspell = new string(h.currentspell);
                        if (h.abilities!=null) {
                                abilities = new SpecialAbility[h.abilities.length];
                                for (int j=0;j<h.abilities.length;j++) {
                                        abilities[j] = new SpecialAbility(h.abilities[j]);
                                }
                        }
                }

                public void setMaxLoad() {
                        maxload = strength*4/5;
                        if (stamina<maxstamina/5) maxload=maxload*2/3;
                        else if (stamina<maxstamina/3) maxload=maxload*4/5;
                }
                
                public void setLoad() {
                        if (head!=null) load+=head.weight;
                        if (neck!=null) load+=neck.weight;
                        if (torso!=null) load+=torso.weight;
                        if (legs!=null) load+=legs.weight;
                        if (feet!=null) load+=feet.weight;
                        if (hand!=null) load+=hand.weight;
                        if (weapon!=null) load+=weapon.weight;
                        if (pouch1!=null) load+=pouch1.weight;
                        if (pouch2!=null) load+=pouch2.weight;
                        for (int j=0;j<6;j++) if (quiver[j]!=null) load+=quiver[j].weight;
                        for (int i=0;i<16;i++) if (pack[i]!=null) load+=pack[i].weight;
                }
                
                public void setDefense() {
                        if (weapon!=null && (weapon.type==Item.WEAPON || weapon.type==Item.SHIELD)) {
                                //if (weapon.defense>0) defense+=weapon.defense;
                                //if (weapon.magicresist>0) magicresist+=weapon.magicresist;
                                weapon.equipEffect(this);
                        }
                        if (head!=null) {
                                //if (head.defense>0) defense+=head.defense;
                                //if (head.magicresist>0) magicresist+=head.magicresist;
                                head.equipEffect(this);
                        }
                        if (neck!=null) {
                                //if (neck.defense>0) defense+=neck.defense;
                                //if (neck.magicresist>0) magicresist+=neck.magicresist;
                                neck.equipEffect(this);
                        }
                        if (torso!=null) {
                                //if (torso.defense>0) defense+=torso.defense;
                                //if (torso.magicresist>0) magicresist+=torso.magicresist;
                                torso.equipEffect(this);
                        }
                        if (hand!=null) {
                                //if (hand.defense>0) defense+=hand.defense;
                                //if (hand.magicresist>0) magicresist+=hand.magicresist;
                                hand.equipEffect(this);
                        }
                        if (legs!=null) {
                                //if (legs.defense>0) defense+=legs.defense;
                                //if (legs.magicresist>0) magicresist+=legs.magicresist;
                                legs.equipEffect(this);
                        }
                        if (feet!=null) {
                                //if (feet.defense>0) defense+=feet.defense;
                                //if (feet.magicresist>0) magicresist+=feet.magicresist;
                                feet.equipEffect(this);
                        }
                }
                
                public void doCompass() {
                        if (weapon.number==8) Compass.addCompass(weapon);
                        if (hand!=null && hand.number==8) Compass.addCompass(hand);
                        for (int i=0;i<16;i++) {
                                if (pack[i]!=null) {
                                        if (pack[i].number==8) Compass.addCompass(pack[i]);
                                        else if (pack[i].number==5) {
                                                //search chest for compasses
                                                for (int j=0;j<6;j++) {
                                                        Item citem = ((Chest)pack[i]).itemAt(j);
                                                        if (citem!=null && citem.number==8) Compass.addCompass(citem);
                                                }
                                        }
                                }
                        }
                        //compassUpdate();
                }

                public void eatdrink() {
                        if (inhand.type==Item.FOOD) {
                                food+=inhand.foodvalue;
                                if (food>1000) food=1000;
                                else if (food<10) food=10;
                                //item effects here...
                                inhand.foodEffect(this);
                                iteminhand=false;
                                hero[leader].load-=inhand.weight;
                                inhand = null;
                        }
                        else if (inhand.number==72 || (inhand.number==73 && ((Waterskin)inhand).drinks>0)) {
                                water+=inhand.foodvalue;
                                if (water>1000) water=1000;
                                else if (water<10) water=10;
                                if (inhand.number==72) {
                                        inhand = new Item(7);//flask
                                        hero[leader].load-=0.2f;//empty flask is .2 lighter than full
                                }
                                else if (inhand.number==73) {
                                        ((Waterskin)inhand).drinks--;
                                        if (((Waterskin)inhand).drinks==0) ((Waterskin)inhand).swapPics();
                                        inhand.weight-=0.2f;
                                        hero[leader].load-=0.2f;//each drink reduces weight by .2
                                }
                                        
                        }
                        else if (inhand.ispotion) {
                                if (!usePotion(inhand)) return;
                                inhand = new Item(7); //flask
                                hero[leader].load-=0.2f;//empty flask is .2 lighter than full
                        }
                        else return;

                        playSound("gulp.wav",-1,-1);
                }

                public void timePass() {
                        if (!isdead) {
                                int numtimes=1; if (sleeping) numtimes=2;
                                for (int i=0;i<numtimes;i++) {
                                        if (hurtcounter>0) { 
                                                hurtcounter--;
                                                repaint();
                                        }
                                        if (!wepready && weaponcount>=-1) {
                                                weaponcount--;
                                                if (hitcounter>0) {
                                                        hitcounter--;
                                                        if (hitcounter==0) weaponsheet.repaint();
                                                }
                                                if (weaponcount<=0) {
                                                        wepready = true;
                                                        if (hitcounter>0) hitcounter=0;
                                                        weaponsheet.repaint();
                                                }
                                        }
                                        //silence
                                        if (silencecount>0) {
                                                silencecount--;
                                                if (weapon.number==215) silencecount--;
                                                if (silencecount<=0) { silencecount=0; silenced = false; }
                                        }
                                        //magic sword
                                        if (kuswordcount>0) {
                                                kuswordcount--;
                                                if (kuswordcount==0) { weapon = fistfoot; repaint(); weaponsheet.repaint(); }
                                        }
                                        //magic bow
                                        else if (rosbowcount>0) {
                                                rosbowcount--;
                                                if (rosbowcount==0) { weapon = fistfoot; repaint(); weaponsheet.repaint(); }
                                        }
                                        //detect curse
                                        if (detectcurse>0) detectcurse--;
                                }
                                if (ispoisoned) {
                                        poisoncounter++;
                                        poisoncounter+=sleeper;
                                        int poisonval = 30;
                                        if (DIFFICULTY<0) poisonval-=5*DIFFICULTY;
                                        if (poisoncounter>poisonval) {
                                           poisoncounter=0;
                                           if (poison>15) poison=15;
                                           damage(poison,POISONHIT);
                                           repaint();
                                           if (sheet && this.equals(herosheet.hero) && !sleeping && !viewing) herosheet.repaint();
                                        }
                                }
                                timecounter++;
                                timecounter+=sleeper;
                                if (timecounter>250) {
                                        timecounter=0;
                                        //stormbringer
                                        //if (weapon.number==215 || (hand!=null && hand.number==215)) {
                                        if (!sleeping && weapon.number==215) {
                                              //drains stamina more quickly
                                              //stamina-=(maxstamina/20); if (stamina<1) stamina = 1;
                                              vitalize(-maxstamina/20);
                                              //chance to kill companion:
                                              if (!sleeping && numheroes>1 && randGen.nextInt(60)==0) {
                                                   int numkilled = randGen.nextInt(numheroes);
                                                   while (hero[numkilled].equals(this)) numkilled = ++numkilled%numheroes;//numkilled = randGen.nextInt(numheroes);
                                                   //playSound("stormfeed.wav",-1,-1);
                                                   hero[numkilled].damage(hero[numkilled].maxhealth,STORMHIT);
                                                   message.setMessage("Stormbringer feeds...",5);
                                                   maxhealth+=hero[numkilled].maxhealth/10; maxstamina+=hero[numkilled].maxstamina/10; maxmana+=hero[numkilled].maxmana/10;
                                                   strength+=hero[numkilled].strength/10; vitality+=hero[numkilled].vitality/10; intelligence+=hero[numkilled].intelligence/10;
                                                   health = maxhealth; stamina = maxstamina; mana = maxmana; //restores completely when feeds (still sucks though)
                                                   setMaxLoad();
                                                   repaint();
                                              }
                                        }
                                        //test for injuries
                                        bool hashurt = (hurtweapon || hurthand || hurthead || hurttorso || hurtlegs || hurtfeet);
                                        int fooddec=8;
                                        int waterdec=6;
                                        if (DIFFICULTY<0) { fooddec+=DIFFICULTY*2; waterdec+=DIFFICULTY*2; }
                                        food-=fooddec;
                                        water-=waterdec;
                                        if (!hashurt) heal(vitality/8+1);
                                        else heal(vitality/12+1);
                                        stamcheck();
                                        if (!hashurt) vitalize(vitality/8+1);
                                        else vitalize(vitality/16+1);
                                        if (mana<maxmana) {
                                                if (!hurthead) energize(((intelligence+wisdom)/16)+1);
                                                else energize(((intelligence+wisdom)/24)+1);
                                                if (mana>maxmana) mana=maxmana;
                                        }
                                        else if (mana>maxmana) {
                                                int takeaway = randGen.nextInt(2);
                                                if (wlevel!=15 || plevel!=15) takeaway++;
                                                if (wlevel!=15 && plevel!=15) takeaway+=(mana-maxmana)/10;
                                                if (mana-takeaway<maxmana) takeaway=mana-maxmana;
                                                energize(-takeaway);
                                        }
                                        //if (randGen.nextInt(100)+20 < vitality || randGen.nextInt(10)==0) poison--;
                                        if (ispoisoned) {
                                                int poisontest = 100+(DIFFICULTY*20);
                                                if (randGen.nextInt(poisontest)+20 < vitality || randGen.nextInt(10)==0) poison--;
                                                if (poison<=0) { ispoisoned=false; poison=0; }
                                        }
                                        //stats
                                        if (strengthboost>0) { strengthboost--; strength--; }
                                        else if (strengthboost<0) { strengthboost++; strength++; }
                                        if (dexterityboost>0) { dexterityboost--; dexterity--; }
                                        else if (dexterityboost<0) { dexterityboost++; dexterity++; }
                                        if (vitalityboost>0) { vitalityboost--; vitality--; }
                                        else if (vitalityboost<0) { vitalityboost++; vitality++; }
                                        if (intelligenceboost>0) { intelligenceboost--; intelligence--; }
                                        else if (intelligenceboost<0) { intelligenceboost++; intelligence++; }
                                        if (wisdomboost>0) { wisdomboost--; wisdom--; }
                                        else if (wisdomboost<0) { wisdomboost++; wisdom++; }
                                        if (defenseboost>0) { defenseboost--; defense--; }
                                        else if (defenseboost<0) { defenseboost++; defense++; }
                                        if (magicresistboost>0) { magicresistboost--; magicresist--; }
                                        else if (magicresistboost<0) { magicresistboost++; magicresist++; }
                                        if (berserk>0) {
                                                berserk--;
                                                if (sleeping) berserk=0;
                                                if (berserk==0) {
                                                        if (strength-(berserkstr+1)>0) {
                                                                strength-=(berserkstr+1);
                                                                strengthboost-=(berserkstr+1);
                                                        }
                                                        else {
                                                                strength-=strengthboost;
                                                                strengthboost=1-strength;
                                                                strength=1;
                                                        }
                                                        berserkhealth = 0;
                                                        int stamadj = maxstamina/4;
                                                        if (stamina-stamadj<=0) stamina=1;
                                                        setMaxLoad();
                                                }
                                        }
                                        //levels
                                        if (levelboostchange<=0) {
                                                if (flevelboost>0) { flevelboost--; flevel--; }
                                                else if (flevelboost<0) { flevelboost++; flevel++; }
                                                if (nlevelboost>0) { nlevelboost--; nlevel--; }
                                                else if (nlevelboost<0) { nlevelboost++; nlevel++; }
                                                if (wlevelboost>0) { wlevelboost--; wlevel--; }
                                                else if (wlevelboost<0) { wlevelboost++; wlevel++; }
                                                if (plevelboost>0) { plevelboost--; plevel--; }
                                                else if (plevelboost<0) { plevelboost++; plevel++; }
                                                levelboostchange = 3;
                                                if (weaponready==heronumber) weaponsheet.repaint();
                                        }
                                        else levelboostchange--;
                                        setMaxLoad();
                                        repaint();
                                        if (sheet && this.equals(herosheet.hero) && !sleeping && !viewing) herosheet.repaint();
                                }
                                if (sleeper>0 || timecounter%25==0) stamcheck();
                        }
                }

                public void stamcheck() {
                        if (food<50) {
                                //stamina-=(maxstamina/25);
                                vitalize(-maxstamina/25-1);
                                if (food<10) food=10;
                        }
                        else if (food<100) vitalize(-maxstamina/50-1); //stamina-=(maxstamina/50);
                        if (water<75) {
                                vitalize(-maxstamina/25-1);//stamina-=(maxstamina/25);
                                if (water<10) water=10;
                        }
                        else if (water<100) vitalize(-maxstamina/50-1);//stamina-=(maxstamina/50);
                        if (load>maxload*2) vitalize(-maxstamina/50-1);
                        int p = (int)((float)stamina/(float)maxstamina*100.0f);
                        int stamtest = 10;
                        if (DIFFICULTY<0) stamtest+=DIFFICULTY*5;
                        if (p<stamtest) {
                                int hit = (int)((float)maxhealth/25.0f)+randGen.nextInt(4);
                                damage(hit,POISONHIT);
                                if (sheet && this.equals(herosheet.hero) && !sleeping && !viewing) herosheet.repaint();
                        }
                        repaint();
                }
                

                public void useweapon(int f) {
                        if (weapon.function[f][0].equals("Shoot")) {
                                if (hand==null || hand.projtype!=weapon.projtype || hand.weight>1.0) {
                                        hitcounter = 2;
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        //weaponsheet.hitlabel.setText("Need Ammo");
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Need Ammo";
                                        weaponcount=2;
                                        return;
                                }
                                if (weapon.name.toLowerCase().indexOf("bow")!=-1) playSound("bow.wav",-1,-1);
                                else playSound("swing.wav",-1,-1);
                                //hand.shotpow=weapon.power[0]+strength/10;
                                hand.shotpow = randGen.nextInt()%10+weapon.power[f]*strength/24+weapon.power[f]*nlevel/4;
                                if (hand.shotpow<=0) hand.shotpow=randGen.nextInt(4)+1;
                                int projsub = subsquare;
                                if (projsub==2) projsub=1;
                                else if (projsub==3) projsub=0;
                                wepThrow(hand,projsub);
                                hand = null;
                                repaint();
                                int i=1;
                                bool found=false;
                                while(i<6 && !found) {
                                        if (quiver[i]!=null && quiver[i].projtype==weapon.projtype) {
                                                hand = quiver[i];
                                                quiver[i]=null;
                                                found = true;
                                        }
                                        i++;
                                }
                                if (!found && quiver[0]!=null) { 
                                        hand=quiver[0]; quiver[0]=null;
                                        if (hand.number==9) {
                                                ((Torch)hand).setPic();
                                                updateDark();
                                        }
                                        if (hand.projtype!=weapon.projtype) {
                                                //swap hands
                                                Item tempitem = weapon;
                                                weapon = hand;
                                                hand = tempitem;
                                                hand.unEquipEffect(this);
                                                weapon.equipEffect(this);
                                        }
                                }
                                weaponcount = weapon.speed[f];
                                if (dexterity<40) {
                                        weaponcount++;
                                        if (dexterity<30) {
                                                weaponcount+=2;
                                                if (dexterity<20) {
                                                        weaponcount+=2;
                                                        if (dexterity<10) weaponcount+=4;
                                                }
                                        }
                                }
                                else if (dexterity>50) { 
                                   weaponcount--;
                                   if (dexterity>70) {
                                      weaponcount--;
                                      if (dexterity>90) weaponcount--;
                                   }
                                }
                                if (hurthand || hurtweapon) weaponcount+=4;
                                if (weaponcount<1) weaponcount=1;
                                if (stamina<maxstamina/5 || load>maxload) weaponcount+=4; //slower if low stamina or overloaded
                                gainxp(weapon.function[f][1].charAt(0),1);
                                //vitalize(-randGen.nextInt(((int)weapon.weight)+1));
                                vitalize(-randGen.nextInt((int)weapon.weight/2+weapon.power[f]/3+2));
                                //stamina-=randGen.nextInt(((int)weapon.weight)+1);
                                //if (stamina<1) stamina=1;
                                wepready = false;
                                backstab = false;
                                repaint();
                                return;
                        }
                        else if (weapon.function[f][0].equals("Magic Shot")) {
                                playSound("bow.wav",-1,-1);
                                Spell arrw = new Spell(weapon.power[0]);
                                arrw.power+=randGen.nextInt((nlevel+1)*5);
                                int projsub = subsquare;
                                if (projsub==2) projsub=1;
                                else if (projsub==3) projsub=0;
                                Projectile p = new Projectile(arrw,50,facing,projsub);
                                weaponcount = weapon.speed[f];
                                if (dexterity<40) {
                                        weaponcount++;
                                        if (dexterity<30) {
                                                weaponcount+=2;
                                                if (dexterity<20) {
                                                        weaponcount+=2;
                                                        if (dexterity<10) weaponcount+=4;
                                                }
                                        }
                                }
                                else if (dexterity>50) { 
                                   weaponcount--;
                                   if (dexterity>70) {
                                      weaponcount--;
                                      if (dexterity>90) weaponcount--;
                                   }
                                }
                                if (hurthand || hurtweapon) weaponcount+=4;
                                if (weaponcount<1) weaponcount=1;
                                if (stamina<maxstamina/5 || load>maxload) weaponcount+=4; //slower if low stamina or overloaded
                                gainxp(weapon.function[f][1].charAt(0),1);
                                //vitalize(-randGen.nextInt(((int)weapon.weight)+1));
                                vitalize(-randGen.nextInt((int)weapon.weight/2+weapon.power[f]/3+2));
                                //stamina-=randGen.nextInt(((int)weapon.weight)+1);
                                //if (stamina<1) stamina=1;
                                wepready = false;
                                backstab = false;
                                repaint();
                                return;
                        }
                        else if (weapon.function[f][0].equals("Drink")) {
                                if (!usePotion(weapon)) return;
                                playSound("gulp.wav",-1,-1);
                                weaponcount = weapon.speed[f];
                                wepready = false;
                                weapon = new Item(7); //flask
                                load-=0.2f;//empty flask is .2 lighter than full
                                repaint();
                                if (sheet && herosheet.hero.heronumber==heronumber) herosheet.repaint();
                                return;
                        }
                        weaponcount = weapon.speed[f];
                        if (dexterity<40) {
                                weaponcount++;
                                if (dexterity<30) {
                                        weaponcount+=2;
                                        if (dexterity<20) {
                                                weaponcount+=2;
                                                if (dexterity<10) weaponcount+=4;
                                        }
                                }
                        }
                        else if (dexterity>50) { 
                           weaponcount--;
                           if (dexterity>70) {
                              weaponcount--;
                              if (dexterity>90) weaponcount--;
                           }
                        }
                        if (hurtweapon) weaponcount+=4;
                        if (weaponcount<1) weaponcount=1;
                        if (stamina<maxstamina/5 || load>maxload) weaponcount+=4; //slower if low stamina or overloaded
                        //if (!weapon.function[f][0].equals("Climb Down") && !weapon.function[f][0].equals("Climb Up")) gainxp(weapon.function[f][1].charAt(0),1);
                        //stamina-=randGen.nextInt(((int)weapon.weight)+1);
                        //vitalize(-randGen.nextInt((int)weapon.weight+1));
                        vitalize(-randGen.nextInt((int)weapon.weight/2+weapon.power[f]/3+2));
                        //stamina = stamina - randGen.nextInt((int)weapon.weight+1)-1;
                        //if (stamina<1) stamina=1;
                        wepready = false;
                        if (weapon.function[f][0].equals("Throw")) {
                                playSound("swing.wav",-1,-1);
                                weapon.shotpow=strength/10+randGen.nextInt(4);
                                int projsub = subsquare;
                                if (projsub==2) projsub=1;
                                else if (projsub==3) projsub=0;
                                gainxp(weapon.function[f][1].charAt(0),1);
                                wepThrow(weapon,projsub);
                                weapon = fistfoot;
                                weapon.unEquipEffect(this);
                                repaint();
                                int i=1;
                                bool found=false;
                                while(i<6 && !found) {
                                        if (quiver[i]!=null) {
                                                weapon = quiver[i];
                                                quiver[i]=null;
                                                found = true;
                                        }
                                        i++;
                                }
                                if (!found && quiver[0]!=null) { 
                                        weapon=quiver[0]; quiver[0]=null;
                                        if (weapon.number==9) {
                                                ((Torch)weapon).setPic();
                                                updateDark();
                                        }
                                        weapon.equipEffect(this);
                                }
                        }
                        //check if mon in front if wep not using magic
                        //if so,damage mon, gain more xp
                        else if (!checkmagic(f)) { 
                                playSound("swing.wav",-1,-1);
                                if (!checkmon(f)) {
                                        checkdoor(f);
                                }
                                gainxp(weapon.function[f][1].charAt(0),1);
                        }
                        backstab = false;
                        repaint();
                }
                
                public bool doAbility(string ability, int power, char classgain) {
                        return doAbility(ability,power,classgain,null);
                }
                public bool doAbility(string ability, int power, char classgain, Object data) {
                        int xadjust=partyx,yadjust=partyy;
                        if (facing==NORTH) yadjust--;
                        else if (facing==WEST) xadjust--;
                        else if (facing==SOUTH) yadjust++;
                        else xadjust++;
                        int projsub = subsquare;
                        if (projsub==2) projsub=1;
                        else if (projsub==3) projsub=0;
                        //System.out.println(ability+", "+power+", "+classgain);
                        if (ability.charAt(0)<'G' || ability.equals("War Cry")) {
                        if (ability.equals("Anti-Ven")) {
                                poison-=power;
                                if (poison<=0) { ispoisoned = false; poison=0; }
                                repaint();
                        }
                        else if (ability.equals("Arc Bolt")) {
                                try { Spell casting = new Spell(power+"642");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }

                        else if (ability.equals("Armor")) {
                                if (defenseboost<power) {
                                        defense-=defenseboost;
                                        defenseboost=power;
                                        defense+=power;
                                }
                                if (magicresistboost<power) {
                                        magicresist-=magicresistboost;
                                        magicresistboost=power;
                                        magicresist+=power;
                                }
                                repaint();
                        }
                        else if (ability.equals("Armor Party")) {
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].defenseboost<power) {
                                        hero[i].defense-=hero[i].defenseboost;
                                        hero[i].defenseboost=power;
                                        hero[i].defense+=power;
                                }
                                if (hero[i].magicresistboost<power) {
                                        hero[i].magicresist-=hero[i].magicresistboost;
                                        hero[i].magicresistboost=power;
                                        hero[i].magicresist+=power;
                                }
                                hero[i].repaint();
                             }
                        }
                        else if (ability.equals("Backstab")) {
                                backstab = true;
                                weaponsheet.toggleSpecials(heronumber);//show weapon commands again
                        }
                        else if (ability.equals("Berserker")) {
                                if (berserk<power) {
                                        if (berserk==0) {
                                                //if ever make charm/confusion spells, put immunity stuff here
                                                strength-=strengthboost;
                                                berserkstr = strength/4+1;
                                                strength+=strengthboost;
                                                berserkhealth = vitality/3+1;
                                                strength+=berserkstr;
                                                strengthboost+=berserkstr;
                                                setMaxLoad();
                                        }
                                        berserk = power;
                                }
                                else return false;
                        }
                        else if (ability.equals("Bolt")) {
                                try { Spell casting = new Spell(power+"335");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Climb Down")) {
                                if (DungeonMap[level][xadjust][yadjust].mapchar=='p' && ((Pit)DungeonMap[level][xadjust][yadjust]).isOpen && !DungeonMap[level][xadjust][yadjust].hasMons && !DungeonMap[level+1][xadjust][yadjust].hasMons && (DungeonMap[level+1][xadjust][yadjust].mapchar!='p' || (DungeonMap[level+1][xadjust][yadjust].mapchar=='p' && !((Pit)DungeonMap[level+1][xadjust][yadjust]).isOpen))) {
                                        climbing = true;
                                        dmove.partyMove(FORWARD);
                                        gainxp(classgain,1);
                                }
                                else {
                                        message.setMessage("Can't Climb Down.",4);
                                        //weaponsheet.hitlabel.setText("Can't Climb");
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Can't Climb";
                                        hitcounter = 2;
                                }
                                return true;
                        }
                        else if (ability.equals("Climb Up")) {
                                if (level>0 && DungeonMap[level-1][partyx][partyy].mapchar=='p' && ((Pit)DungeonMap[level-1][partyx][partyy]).isOpen && DungeonMap[level-1][xadjust][yadjust].isPassable && !DungeonMap[level-1][xadjust][yadjust].hasMons) {
                                        level--;
                                        dmove.partyMove(FORWARD);
                                        for (int i=0;i<numheroes;i++) if (!hero[i].isdead) hero[i].energize((int)-hero[i].load);
                                        gainxp(classgain,1);
                                }
                                else {
                                        message.setMessage("Can't Climb Up.",4);
                                        //weaponsheet.hitlabel.setText("Can't Climb");
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Can't Climb";
                                        hitcounter = 2;
                                }
                                return true;
                        }
                        else if (ability.equals("Conjure")) {
                                Item it = (Item)data;
                                if (weapon==fistfoot || hand==null) {
                                        it = Item.createCopy(it);
                                        if (weapon==fistfoot) {
                                                weapon = it;
                                                if (it.type==Item.WEAPON || it.type==Item.SHIELD || it.number==4) it.equipEffect(this);
                                                weaponsheet.repaint();
                                        }
                                        else {
                                                hand = it;
                                                if (it.type==Item.SHIELD) it.equipEffect(this);
                                        }
                                        load+=it.weight;
                                        //if torch
                                        if (it.number==9) {
                                                ((Torch)it).setPic();
                                                updateDark();
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                }
                                else if (DungeonMap[level][partyx][partyy].canHoldItems) {
                                        it.subsquare = (-facing+4)%4;
                                        DungeonMap[level][partyx][partyy].addItem(Item.createCopy(it));
                                }
                                else {
                                        message.setMessage(name+" has nowhere to put the item.",4);
                                        return false;
                                }
                        }
                        else if (ability.equals("Detect Illusion")) {
                                if (randGen.nextInt(16-nlevel)<5) {
                                        if (DungeonMap[level][xadjust][yadjust].mapchar=='2' || DungeonMap[level][xadjust][yadjust].mapchar=='i' || (DungeonMap[level][xadjust][yadjust].mapchar=='p' && ((Pit)DungeonMap[level][xadjust][yadjust]).isIllusionary)) {
                                                message.setMessage(name+" senses an illusion.",heronumber);
                                                //dispell the illusion here -> change false wall to floor, remove illusion from pit
                                                if (DungeonMap[level][xadjust][yadjust].mapchar=='p') ((Pit)DungeonMap[level][xadjust][yadjust]).isIllusionary=false;
                                                else if (DungeonMap[level][xadjust][yadjust].mapchar=='2') {
                                                        //swap fake wall with floor and transfer any items
                                                        Floor newfloor = new Floor();
                                                        MapObject oldwall = DungeonMap[level][xadjust][yadjust];
                                                        newfloor.numProjs = oldwall.numProjs;
                                                        newfloor.hasMons = oldwall.hasMons;
                                                        newfloor.hasItems = oldwall.hasItems;
                                                        newfloor.hasCloud = oldwall.hasCloud;
                                                        if (newfloor.hasItems) {
                                                                //while (!oldwall.mapItems.isEmpty()) {
                                                                //        newfloor.mapItems.add(oldwall.mapItems.remove(0));
                                                                //}
                                                                newfloor.mapItems = oldwall.mapItems;
                                                        }
                                                        DungeonMap[level][xadjust][yadjust] = newfloor;
                                                }
                                                else {
                                                        //swap invisible wall with visible, reset hasmons
                                                        Wall newwall= new Wall();
                                                        MapObject oldwall = DungeonMap[level][xadjust][yadjust];
                                                        newwall.hasMons = oldwall.hasMons;
                                                        DungeonMap[level][xadjust][yadjust] = newwall;
                                                }
                                                needredraw = true;
                                        }
                                        else message.setMessage(name+" senses there is no illusion.",heronumber);
                                        gainxp(classgain,1);
                                }
                                else message.setMessage(name+" fails to sense anything.",4);
                                return true;
                        }
                        else if (ability.equals("Dispell")) {
                                try { Spell casting = new Spell(power+"52");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Drain Life")) {
                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                int i=3;
                                while (tempmon==null && i>=0) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+((i-facing+4)%4));
                                        i--;
                                }
                                if (tempmon!=null) {
                                        int hit = tempmon.damage(power,DRAINHIT);
                                        if (hit>=0) {
                                                heal(hit);
                                                /*
                                                weaponsheet.hitlabel.setText(""+hit);
                                                if (hit<50) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon3);
                                                else if (hit<100) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon2);
                                                else weaponsheet.hitlabel.setIcon(weaponsheet.hiticon);
                                                */
                                                if (hit<50) weaponsheet.hitpic=weaponsheet.hit3;
                                                else if (hit<100) weaponsheet.hitpic=weaponsheet.hit2;
                                                else weaponsheet.hitpic=weaponsheet.hit1;
                                                weaponsheet.hittext=""+hit;
                                                hitcounter = 2;
                                                message.setMessage(name+" drains "+hit+" life from "+tempmon.name,heronumber);
                                        }
                                        else damage(hit,SPELLHIT);
                                }
                                else return true;
                        }
                        else if (ability.equals("Drain Mana")) {
                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                int i=3;
                                while (tempmon==null && i>=0) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+((i-facing+4)%4));
                                        i--;
                                }
                                if (tempmon!=null && tempmon.mana>0) {
                                        int hit = power;
                                        hit-=(hit*tempmon.magicresist/100);
                                        if (hit<=0) hit = 1;
                                        if (hit>tempmon.mana) hit = tempmon.mana;
                                        energize(hit);
                                        message.setMessage(name+" drains "+hit+" mana from "+tempmon.name,heronumber);
                                }
                        }
                        else if (ability.equals("Enhance Weapon") || ability.equals("Enhance Fist")) {
                                //modify next attack -> hit ghosts, diamond edge style, stun, poison
                                if (ability.endsWith("Fist")) {
                                        if (weapon!=fistfoot) {
                                                message.setMessage(name+" must be barehanded to use that ability.",4);
                                                return false;
                                        }
                                }
                                else if (weapon==fistfoot) {
                                        message.setMessage(name+" must hold a weapon to use that ability.",4);
                                        return false;
                                }
                                abilitywep = weapon;
                                abilityactive = power; if (abilityactive<1) abilityactive = 1;
                                abilityimm = ((int[])data)[0]>0;
                                abilitydiamond = ((int[])data)[1]>0;
                                abilitystun = ((int[])data)[2]>0;
                                abilitypoison = ((int[])data)[3];
                                weaponsheet.toggleSpecials(heronumber);//show weapon commands again
                        }
                        else if (ability.equals("False Image")) {
                                if (falseimage<power) {
                                        falseimage = power;
                                        message.setMessage(name+" projects false images.",heronumber);
                                }
                                else {
                                        message.setMessage(name+" is already projecting false images.",4);
                                        return false;
                                }
                        }
                        else if (ability.equals("Feeble Mind")) {
                                try { Spell casting = new Spell(power+"363");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Fireball")) {
                                try { Spell casting = new Spell(power+"44");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Fluxcage")) {
                                if (!(DungeonMap[level][xadjust][yadjust] instanceof Wall)) {
                                        new FluxCage(level,xadjust,yadjust);
                                        needredraw = true;
                                }
                        }
                        else if (ability.equals("Freeze")) {
                                Monster tempmon;
                                for (int i=0;i<6;i++) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+i);
                                        if (tempmon!=null && tempmon.number!=26) { tempmon.timecounter = -power; }
                                        if (i==3) i++;
                                }
                        }
                        else if (ability.equals("Freeze Life")) {
                                freezelife+=power;
                        }
                        else if (ability.equals("Fuse")) {
                                Projectile p = new Projectile(new Spell(),1,facing,projsub);
                        }
                        else if (ability.equals("War Cry") || ability.equals("Blow Horn") || ability.equals("Frighten") || ability.equals("Calm")) {
                                int adjuster = 0;
                                if (ability.equals("War Cry")) {
                                        //war cry less effective, depends on hero's average level
                                        adjuster = 6;
                                        int avglevel = (flevel+nlevel+plevel+wlevel)/4;
                                        if (avglevel>4 && avglevel<8) adjuster--;
                                        else if (avglevel>7 && avglevel<11) adjuster-=2;
                                        else if (avglevel>10 && avglevel<13) adjuster-=3;
                                        else if (avglevel>12 && avglevel<15) { adjuster-=4; power++; }
                                        else if (avglevel>14) { adjuster=0; power+=2; }
                                        //System.out.println("war cry adjuster = "+adjuster);
                                }
                                Monster tempmon;
                                for (int i=0;i<6;i++) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+i);
                                        if (tempmon!=null && tempmon.currentai!=Monster.FRIENDLYMOVE && tempmon.currentai!=Monster.FRIENDLYNOMOVE && randGen.nextInt(11+adjuster)<tempmon.fearresist) { tempmon.runcounter+=power; tempmon.wasfrightened=true; }
                                        if (i==3) i++;
                                }
                        }
                        }

                        else if (ability.equals("Good Berries")) {
                                //create some magical berries -> food and healing
                                Item berry = new Item(70);
                                if (power>0) {
                                        berry.effect[0] = "health,"+(power+2*plevel);
                                        berry.effect[1] = "stamina,"+(power+2*plevel);
                                }
                                int nummade = 0;
                                int numtomake = randGen.nextInt(plevel/6+1)+1;
                                if (weapon==fistfoot) {
                                        weapon = berry;
                                        load+=berry.weight;
                                        nummade++;
                                }
                                if (nummade<numtomake && hand==null) {
                                        if (nummade>0) hand = Item.createCopy(berry);
                                        else hand = berry;
                                        load+=berry.weight;
                                        nummade++;
                                }
                                if (nummade<numtomake && DungeonMap[level][partyx][partyy].canHoldItems) {
                                        berry.subsquare = (-facing+4)%4;
                                        while (nummade<numtomake) {
                                                DungeonMap[level][partyx][partyy].addItem(Item.createCopy(berry));
                                                nummade++;
                                        }
                                }
                                if (nummade==0) {
                                        message.setMessage(name+" has nowhere to put the berries.",4);
                                        return false;
                                }
                        }
                        else if (ability.equals("Heal")) {
                                heal(maxhealth*power/100+5);
                                repaint();
                        }
                        else if (ability.equals("Invoke")) {
                                string[] spls = { "44","52","335","31","51","44" };
                                int gain = randGen.nextInt(wlevel/2)+wlevel/2+1; if (gain>6) gain=6; //(mana/50+1)%6+1;
                                try { Spell casting = new Spell(gain+spls[randGen.nextInt(6)]);
                                      casting.power+=randGen.nextInt(20)+20;
                                      for (int i=casting.gain-2;i>=0;i--) {
                                          casting.powers[i]+= randGen.nextInt(20)+20;
                                      }
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                      energize(-randGen.nextInt(10)-15);
                                }
                                catch(Exception e) {e.printStackTrace();} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Light")) {
                                darkcounter=0;
                                magictorch+=power; if (magictorch>285) magictorch=285; //can stock up some, but not infinite
                                if ((darkfactor+power)>255) darkfactor=255;
                                else darkfactor+=power;
                                needredraw=true;
                        }
                        else if (ability.equals("Purify")) {
                                Item it = null;
                                if (weapon.type==Item.FOOD && (weapon.poisonous>0 || weapon.foodvalue<0)) it = weapon;
                                else if (hand!=null && hand.type==Item.FOOD && (hand.poisonous>0 || hand.foodvalue<0)) it = hand;
                                if (it!=null) {
                                        it.poisonous = 0;
                                        if (it.foodvalue<0) it.foodvalue = 50;
                                        message.setMessage(name+" purifies the "+it.name+".",heronumber);
                                }
                                else {
                                        message.setMessage(name+" has nothing in hand to purify.",4);
                                        return false;
                                }
                        }
                        else if (ability.equals("Ruiner")) {
                                try { Spell s1 = new Spell(power+"461");//weakness
                                      Projectile p = new Projectile(s1,s1.dist,facing,projsub);
                                      s1 = new Spell(power+"363");//feeble mind
                                      p = new Projectile(s1,s1.dist,facing,projsub);
                                      s1 = new Spell(power+"362");//slow
                                      p = new Projectile(s1,s1.dist,facing,projsub);
                                      s1 = new Spell(power+"664");//strip defenses
                                      p = new Projectile(s1,s1.dist,facing,projsub);
                                      s1 = new Spell(power+"523");//silence
                                      p = new Projectile(s1,s1.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Shield")) {
                                if (defenseboost<power) {
                                        defense-=defenseboost;
                                        defenseboost=power;
                                        defense+=power;
                                        repaint();
                                }
                        }
                        else if (ability.equals("Shield Party")) {
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].defenseboost<power) {
                                        hero[i].defense-=hero[i].defenseboost;
                                        hero[i].defenseboost=power;
                                        hero[i].defense+=power;
                                        hero[i].repaint();
                                }
                             }
                        }
                        else if (ability.equals("Sight")) {
                                magicvision+=power;
                                needredraw = true;
                        }
                        else if (ability.equals("Silence")) {
                                try { Spell casting = new Spell(power+"523");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Slow")) {
                                try { Spell casting = new Spell(power+"362");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Slowfall")) {
                                floatcounter+=power;
                                if (!climbing) message.setMessage("Slowfall active.",4);
                                climbing=true;
                        }
                        else if (ability.equals("SpellShield")) {
                                if (magicresistboost<power) {
                                        magicresist-=magicresistboost;
                                        magicresistboost=power;
                                        magicresist+=power;
                                        repaint();
                                }
                        }
                        else if (ability.equals("SpellShield Party")) {
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].magicresistboost<power) {
                                        hero[i].magicresist-=hero[i].magicresistboost;
                                        hero[i].magicresistboost=power;
                                        hero[i].magicresist+=power;
                                        hero[i].repaint();
                                }
                             }
                        }
                        else if (ability.equals("Stat Boost")) {
                                int[] boost = (int[])data;
                                strength+=boost[0]; strengthboost+=boost[0];
                                dexterity+=boost[1]; dexterityboost+=boost[1];
                                vitality+=boost[2]; vitalityboost+=boost[2];
                                intelligence+=boost[3]; intelligenceboost+=boost[3];
                                wisdom+=boost[4]; wisdomboost+=boost[4];
                                if (boost[5]!=0) {
                                        if (flevel+boost[5]>15) { flevelboost+=15-flevel; flevel=15; }
                                        else if (flevel+boost[5]<0) { flevelboost+=0-flevel; flevel=0; }
                                        else { flevelboost+=boost[5]; flevel+=boost[5]; }
                                }
                                if (boost[6]!=0) {
                                        if (nlevel+boost[6]>15) { nlevelboost+=15-nlevel; nlevel=15; }
                                        else if (nlevel+boost[5]<0) { nlevelboost+=0-nlevel; nlevel=0; }
                                        else { nlevelboost+=boost[6]; nlevel+=boost[6]; }
                                }
                                if (boost[7]!=0) {
                                        if (wlevel+boost[7]>15) { wlevelboost+=15-wlevel; wlevel=15; }
                                        else if (wlevel+boost[5]<0) { wlevelboost+=0-wlevel; wlevel=0; }
                                        else { wlevelboost+=boost[7]; wlevel+=boost[7]; }
                                }
                                if (boost[8]!=0) {
                                        if (plevel+boost[8]>15) { plevelboost+=15-plevel; plevel=15; }
                                        else if (plevel+boost[5]<0) { plevelboost+=0-flevel; plevel=0; }
                                        else { plevelboost+=boost[8]; plevel+=boost[8]; }
                                }
                        }
                        else if (ability.equals("Steal")) {
                                //maybe should have a stealdifficulty integer for mons (like fearresist)
                                //steal an item from mons in front
                                if (weapon!=fistfoot && hand!=null) {
                                        //put check for glove-type weapons, if i make any
                                        message.setMessage(name+" needs a free hand to steal.",4);
                                        return false;
                                }
                                //don't allow steal from back row unless high level
                                if (nlevel<12 && ( (subsquare==2 && heroatsub[1]!=-1) || (subsquare==3 && heroatsub[0]!=-1) )) {
                                        message.setMessage(name+" needs more skill to steal from the back row.",4);
                                        return false;
                                }
                                //find a material monster in front of party to steal from, if any exist
                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                if (tempmon==null) {
                                        bool[] imtest = new bool[4];
                                        int subface = randGen.nextInt(2)+2;
                                        int who = (subface-facing+4)%4;
                                        tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                        imtest[who] = (tempmon!=null && tempmon.isImmaterial);
                                        if (tempmon==null || imtest[who]) {
                                                if (subface==2) subface=3;
                                                else subface=2;
                                                who = (subface-facing+4)%4;
                                                tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                imtest[who] = (tempmon!=null && tempmon.isImmaterial);
                                                if (tempmon==null || imtest[who]) {
                                                        subface=randGen.nextInt(2);
                                                        who = (subface-facing+4)%4;
                                                        int whocheck = ((3-subface)-facing+4)%4;
                                                        tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                        imtest[who] = (tempmon!=null && tempmon.isImmaterial);
                                                        if (tempmon==null || imtest[whocheck] || imtest[who]) {
                                                                if (subface==0) subface=1;
                                                                else subface=0;
                                                                who = (subface-facing+4)%4;
                                                                whocheck = ((3-subface)-facing+4)%4;
                                                                tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                                if (imtest[whocheck]) tempmon=null;
                                                                else if (tempmon!=null && tempmon.isImmaterial) tempmon=null;
                                                        }
                                                }
                                        }
                                }
                                else if (tempmon.isImmaterial || tempmon.number==26) tempmon=null;//can't steal from chaos (#26) - remove if/when have a stealdifficulty int
                                if (tempmon!=null) {
                                        //found a mon, try to steal if has anything, power is chance to succeed
                                        if (randGen.nextInt(100)<(power+3*nlevel-tempmon.speed/4)) {
                                                if (tempmon.carrying.size()==0) message.setMessage(tempmon.name+" has nothing to steal.",4);
                                                else {
                                                        Item it = (Item)tempmon.carrying.remove(randGen.nextInt(tempmon.carrying.size()));
                                                        if (weapon==fistfoot) {
                                                                weapon = it;
                                                                if (it.type==Item.WEAPON || it.type==Item.SHIELD || it.number==4) it.equipEffect(this);
                                                                weaponsheet.repaint();
                                                        }
                                                        else {
                                                                hand = it;
                                                                if (it.type==Item.SHIELD) it.equipEffect(this);
                                                        }
                                                        load+=it.weight;
                                                        //if torch
                                                        if (it.number==9) {
                                                                ((Torch)it).setPic();
                                                                updateDark();
                                                        }
                                                        repaint();
                                                        if (sheet) herosheet.repaint();
                                                }
                                        }
                                }
                        }
                        else if (ability.equals("Strip Defenses")) {
                                try { Spell casting = new Spell(power+"664");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("True Sight")) {
                                dispell+=power;
                                if (dispell==power) needredraw=true;
                        }
                        else if (ability.equals("Ven Cloud")) {
                                try { Spell casting = new Spell(power+"31");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Venom")) {
                                try { Spell casting = new Spell(power+"51");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("Weakness")) {
                                try { Spell casting = new Spell(power+"461");
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else if (ability.equals("ZO")) {
                                try { Spell casting = new Spell(power+"6");
                                      for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*wlevel;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(20);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                      }
                                      casting.power = casting.powers[casting.gain-1];
                                      Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                catch(Exception e) {} //spell# is valid, should be no error...
                        }
                        else return false;
                        gainxp(classgain,1);
                        return true;
                }
                
                public bool checkmagic(int f) {
                        if (weapon.charges[f]==0) return false; //0 charge means not a special magic ability
                        if (!doAbility(weapon.function[f][0],weapon.power[f],weapon.function[f][1].charAt(0))) return false; //do ability, based on function name -> if name not found, is error so just treat as normal attack
                        if (weapon.function[f][0].equals("Blow Horn")) playSound("horn.wav",-1,-1);
                        else if (weapon.function[f][0].equals("War Cry")) playSound("warcry.wav",-1,-1);
                        else if (weapon.function[f][0].startsWith("Drain")) playSound("drain.wav",-1,-1);
                        if (weapon.charges[f]==-1) return true; //-1 means infinite charges
                        weapon.charges[f]--;
                        if (weapon.charges[f]<1) {
                                weapon.functions--;
                                if (f==1 && weapon.functions==2) {
                                        weapon.function[1][0]=weapon.function[2][0];
                                        weapon.function[1][1]=weapon.function[2][1];
                                        weapon.power[1]=weapon.power[2];
                                        weapon.charges[1]=weapon.charges[2];
                                }
                                else if (f==0) {
                                        if (weapon.functions>0) {
                                            weapon.function[0][0]=weapon.function[1][0];
                                            weapon.function[0][1]=weapon.function[1][1];
                                            weapon.power[0]=weapon.power[1];
                                            weapon.charges[0]=weapon.charges[1];
                                        }
                                        else { load-=weapon.weight; weapon=fistfoot; repaint(); return true;} //used up, disappears
                                }
                                if (weapon.upic!=null) {
                                        bool hascharge = false;
                                        for (int i=0;i<weapon.functions;i++) {
                                                if (weapon.charges[i]!=0) hascharge=true;
                                        }
                                        if (!hascharge) {
                                                weapon.pic.flush();
                                                if (weapon.temppic!=null) weapon.temppic.flush();
                                                weapon.pic = weapon.upic;
                                                weapon.temppic = weapon.upic;
                                                weapon.picstring = weapon.usedupstring;//"Items"+File.separator+weapon.usedupstring;
                                                if (weapon.epic!=null) {
                                                        weapon.epic.flush();
                                                        weapon.epic = weapon.upic;
                                                        weapon.equipstring = weapon.usedupstring;
                                                }
                                                repaint();
                                        }
                                }
                        }
                        return true;
                }
                
                public bool checkmon(int f) {
                        
                        int xadjust=partyx,yadjust=partyy;
                        if (facing==NORTH) yadjust--;
                        else if (facing==WEST) xadjust--;
                        else if (facing==SOUTH) yadjust++;
                        else xadjust++;

                        if ( (subsquare==2 && heroatsub[1]!=-1) || (subsquare==3 && heroatsub[0]!=-1) ) {
                                if (!DungeonMap[level][xadjust][yadjust].hasMons) return false;//false so will check for a door and say can't reach if is one
                                message.setMessage(name+" can't reach.",4);
                                //weaponsheet.hitlabel.setText("Can't Reach");
                                //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                weaponsheet.hitpic=weaponsheet.miss;
                                weaponsheet.hittext="Can't Reach";
                                hitcounter = 2;
                                return true;
                        }
                        
                        //ability effects lost if run out of hits or changed weapon
                        if (abilityactive==0 || weapon!=abilitywep) {
                                abilityimm = false;
                                abilitydiamond = false;
                                abilitystun = false;
                                abilitypoison = 0;
                        }
                        bool oldimm = weapon.hitsImmaterial;
                        if (abilityimm) weapon.hitsImmaterial = true;
                        Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                        if (tempmon==null) {
                                Monster newtemp;
                                bool[] imtest = new bool[4];
                                int subface = randGen.nextInt(2)+2;
                                int who = (subface-facing+4)%4;
                                tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                imtest[who] = (tempmon!=null && ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt"))));
                                if (tempmon==null || imtest[who]) {
                                        if (subface==2) subface=3;
                                        else subface=2;
                                        who = (subface-facing+4)%4;
                                        tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                        imtest[who] = (tempmon!=null && ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt"))));
                                        if (tempmon==null || imtest[who]) {
                                                subface=randGen.nextInt(2);
                                                who = (subface-facing+4)%4;
                                                int whocheck = ((3-subface)-facing+4)%4;
                                                tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                imtest[who] = (tempmon!=null && ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt"))));
                                                if (tempmon==null || imtest[whocheck] || imtest[who]) {
                                                        if (subface==0) subface=1;
                                                        else subface=0;
                                                        who = (subface-facing+4)%4;
                                                        whocheck = ((3-subface)-facing+4)%4;
                                                        tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                        if (imtest[whocheck]) tempmon=null;
                                                        else if (tempmon!=null && ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt")))) tempmon=null;
                                                }
                                                //when backstabbing, if chosen mon not facing away try other (only affects 2 furthest subsquares)
                                                else if (backstab && tempmon.facing!=facing) {
                                                        if (subface==0) subface=1;
                                                        else subface=0;
                                                        who = (subface-facing+4)%4;
                                                        whocheck = ((3-subface)-facing+4)%4;
                                                        newtemp = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                                        if (imtest[whocheck]) newtemp=null;
                                                        else if (newtemp!=null && ((newtemp.isImmaterial && !weapon.hitsImmaterial) || (!newtemp.isImmaterial && weapon.function[f][0].equals("Disrupt")))) newtemp=null;
                                                        if (newtemp!=null && newtemp.facing==facing) tempmon = newtemp;
                                                }
                                        }
                                }
                                //when backstabbing, if chosen mon not facing away try other (only affects 2 closest subsquares)
                                else if (backstab && tempmon.facing!=facing) {
                                        if (subface==2) subface=3;
                                        else subface=2;
                                        who = (subface-facing+4)%4;
                                        newtemp = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+who);
                                        imtest[who] = (newtemp!=null && ((newtemp.isImmaterial && !weapon.hitsImmaterial) || (!newtemp.isImmaterial && weapon.function[f][0].equals("Disrupt"))));
                                        if (newtemp!=null && !imtest[who] && newtemp.facing==facing) tempmon = newtemp;
                                }
                        }
                        else if ((tempmon.isImmaterial && !weapon.hitsImmaterial) || (!tempmon.isImmaterial && weapon.function[f][0].equals("Disrupt"))) tempmon=null;
                        weapon.hitsImmaterial = oldimm;
                        
                        if (tempmon!=null) {
                                hitcounter=2;
                                if (randGen.nextInt(20+nlevel)==0 && nlevel<15) { 
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        //weaponsheet.hitlabel.setText("Critical Miss");
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Critical Miss";
                                        weaponcount+=4;
                                        message.setMessage(name+": Critical Miss",heronumber);
                                        return true;
                                }
                                
                                int speeddif = dexterity - tempmon.speed;
                                if (stamina<maxstamina/4) speeddif-=5;
                                if (load>maxload) speeddif-=5;
                                if (hurtweapon) speeddif-=10;
                                int backhit = 0;
                                //if (subsquare>1) { speeddif-=10; backhit = 1; }//harder to hit from back row
                                if (tempmon.subsquare!=5 && (tempmon.subsquare+facing)%4<2) { speeddif-=10; backhit++; }//harder to hit back row
                                
                                bool didhit;
                                if (speeddif>40) didhit = (randGen.nextInt(8)!=0);
                                else if (speeddif>30) didhit = (randGen.nextInt(7)!=0);
                                else if (speeddif>20) didhit = (randGen.nextInt(6)!=0);
                                else if (speeddif>10) didhit = (randGen.nextInt(5)!=0);
                                else if (speeddif>0) didhit = (randGen.nextInt(4)!=0);
                                else if (speeddif>-10) didhit = (randGen.nextInt(3)!=0);
                                else if (speeddif>-20) didhit = (randGen.nextInt(2)!=0);
                                else if (speeddif>-30) didhit = (randGen.nextInt(3)==0);
                                else didhit = (randGen.nextInt(4)==0);
                                //else if (speeddif>-40) didhit = (randGen.nextInt(4)==0);
                                //else didhit = (randGen.nextInt(5)==0);
                                
                                if (didhit) {
                                        int pow = randGen.nextInt()%10+weapon.power[f]*strength/12;
                                        if (backhit>0) {
                                                pow = pow*2/3;
                                                //if (backhit>1) pow = pow*2/3;
                                        }
                                        if (nlevel>1 && weapon.function[f][1].equals("n")) pow+=(randGen.nextInt(nlevel)+1)*3;//was *4
                                        if (hurtweapon) pow = pow/2;
                                        if (pow<1) pow=randGen.nextInt(4);
                                        
                                        //parry makes next mon attack against this hero more likely to miss
                                        if (weapon.function[f][0].equals("Parry")) {
                                                tempmon.parry=heronumber;
                                        }
                                        else if (dexterity>50 && randGen.nextInt(20-2*nlevel/3)==0) { pow=3*pow/2; message.setMessage(name+": Critical Hit",heronumber); } //critical hit
                                        
                                        //playSound("hit.wav",-1,-1);
                                        gainxp(weapon.function[f][1].charAt(0),1);
                                        
                                        //check if hurt by only certain item
                                        if (tempmon.hurtitem!=0 && weapon.number!=tempmon.hurtitem && weapon.number!=215 && (tempmon.hurtitem!=248 || weapon.number!=249)) {
                                                message.setMessage(name+"'s Weapon Has No Effect.",4);
                                                return true;
                                        }
                                        
                                        int diamondcut = 0;
                                        if ((weapon.number==206 || abilitydiamond) && (tempmon.number==3 || tempmon.number==21 || tempmon.number==23 || tempmon.defense>79)) {
                                                diamondcut = tempmon.defense;
                                                tempmon.defense/=2;
                                        }
                                        if (backstab && tempmon.facing==facing) {
                                                if (nlevel<9) pow=3*pow/2;
                                                else pow*=2;
                                                //else if (nlevel<15) pow*=2;
                                                //else pow=5*pow/2;
                                                message.setMessage(name+" performs a backstab attack!",heronumber);
                                        }
                                        if (weapon.number!=215) pow = tempmon.damage(pow,WEAPONHIT);
                                        else pow = tempmon.damage(pow,STORMHIT);
                                        /*
                                        if (pow<50) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon3);
                                        else if (pow<100) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon2);
                                        else weaponsheet.hitlabel.setIcon(weaponsheet.hiticon);
                                        weaponsheet.hitlabel.setText(""+pow);
                                        */
                                        if (pow<50) weaponsheet.hitpic=weaponsheet.hit3;
                                        else if (pow<100) weaponsheet.hitpic=weaponsheet.hit2;
                                        else weaponsheet.hitpic=weaponsheet.hit1;
                                        weaponsheet.hittext=""+pow;
                                        if (diamondcut>0) tempmon.defense=diamondcut;
                                        if (weapon.poisonous>tempmon.poisonpow && randGen.nextbool()) { tempmon.ispoisoned=true; tempmon.poisonpow=weapon.poisonous; }//50% chance of inflicting poison
                                        if (abilitypoison>tempmon.poisonpow && randGen.nextInt(5)>0) { tempmon.ispoisoned=true; tempmon.poisonpow=abilitypoison; }//4/5 chance of inflicting poison from an ability
                                        //stun freezes briefly (not chaos)
                                        if ((abilitystun || weapon.function[f][0].equals("Stun")) && tempmon.number!=26 && tempmon.health<2*tempmon.maxhealth/5 && randGen.nextbool()) {
                                                tempmon.timecounter = -weapon.power[f]; if (tempmon.timecounter>-10) tempmon.timecounter = -10;
                                                if (nlevel>8 && !tempmon.isdying) message.setMessage(name+"'s blow has stunned the "+tempmon.name+".",heronumber);
                                        }
                                        //stormbringer, small chance to kill a companion
                                        if (weapon.number==215 && pow>0) {
                                              heal(pow); vitalize(pow); energize(pow);
                                              //chance to kill companion:
                                              if (tempmon.isdying && numheroes>1 && randGen.nextInt(50)==0) {
                                                   int numkilled = randGen.nextInt(numheroes);
                                                   while (hero[numkilled].equals(this)) numkilled = ++numkilled%numheroes;//numkilled = randGen.nextInt(numheroes);
                                                   //playSound("stormfeed.wav",-1,-1);
                                                   hero[numkilled].damage(hero[numkilled].maxhealth,STORMHIT);
                                                   message.setMessage("Stormbringer feeds...",5);
                                                   maxhealth+=hero[numkilled].maxhealth/10; maxstamina+=hero[numkilled].maxstamina/10; maxmana+=hero[numkilled].maxmana/10;
                                                   strength+=hero[numkilled].strength/10; vitality+=hero[numkilled].vitality/10; intelligence+=hero[numkilled].intelligence/10;
                                                   health = maxhealth; stamina = maxstamina; mana = maxmana; //restores completely when feeds (still sucks though)
                                                   setMaxLoad();
                                                   repaint();
                                              }
                                        }
                                }
                                else {
                                        //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                        //weaponsheet.hitlabel.setText("Miss");
                                        weaponsheet.hitpic=weaponsheet.miss;
                                        weaponsheet.hittext="Miss";
                                }
                                //ability enhancement used, so decrement (even if missed)
                                if (abilityactive>=0) abilityactive--;
                                return true;
                        }
                        return false;
                }

                public void checkdoor(int f) {
                        if (weapon.number==9 || (weapon.number>235 && weapon.number<300) || !(weapon.function[f][0].equals("Swing") || weapon.function[f][0].equals("Chop") || weapon.function[f][0].equals("Melee") || weapon.function[f][0].equals("Bash") || weapon.function[f][0].equals("Crush") || weapon.function[f][0].equals("Berzerk"))) return;
                        if ( (subsquare==2 && heroatsub[1]!=-1) || (subsquare==3 && heroatsub[0]!=-1) ) {
                                message.setMessage(name+" can't reach.",4);
                                //weaponsheet.hitlabel.setText("Can't Reach");
                                //weaponsheet.hitlabel.setIcon(weaponsheet.missicon);
                                weaponsheet.hitpic=weaponsheet.miss;
                                weaponsheet.hittext="Can't Reach";
                                hitcounter = 2;
                                return;
                        }
                        int xadjust=0,yadjust=0;
                        if (facing==NORTH) yadjust=1;
                        else if (facing==WEST) xadjust=1;
                        else if (facing==SOUTH) yadjust=-1;
                        else xadjust=-1;
                        
                        //if (DungeonMap[level][partyx-xadjust][partyy-yadjust].mapchar=='d' && ((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).pictype==0 && !((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).isOpen && !((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).isBroken) { // && ((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).changecount==4) {
                        if (DungeonMap[level][partyx-xadjust][partyy-yadjust].mapchar=='d' && !((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).isOpen && !((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).isBroken) {
                                //Door d = (Door)DungeonMap[level][partyx-xadjust][partyy-yadjust];
                                //if (d.isOpen || d.isBroken || !d.isBreakable || d.changecount!=4) return;
                                //int pow = randGen.nextInt()%10+weapon.power[f]*strength/8;
                                
                                int pow = randGen.nextInt()%10+weapon.power[f]*strength/12;
                                ((Door)DungeonMap[level][partyx-xadjust][partyy-yadjust]).breakDoor(pow,true,true);
                                /*
                                weaponsheet.hitlabel.setText(""+pow);
                                if (pow<50) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon3);
                                else if (pow<100) weaponsheet.hitlabel.setIcon(weaponsheet.hiticon2);
                                else weaponsheet.hitlabel.setIcon(weaponsheet.hiticon);
                                */
                                if (pow<50) weaponsheet.hitpic=weaponsheet.hit3;
                                else if (pow<100) weaponsheet.hitpic=weaponsheet.hit2;
                                else weaponsheet.hitpic=weaponsheet.hit1;
                                weaponsheet.hittext=""+pow;
                                hitcounter = 2;
                        }
                }

                public void gainxp(char classgain, int x) {
                        if (classgain=='-') return;
                        int xptest;
                        switch (classgain) {
                                //were all level*30+50
                                case 'n':
                                        nlevel-=nlevelboost;
                                        if (nlevel<15) {
                                           nxp+=x;
                                           xptest = (int)(Math.pow(1.55,(double)nlevel)) + 35*nlevel + 50;
                                           if (nxp>xptest) levelgain('n');
                                           //if (nxp > nlevel*35+50) levelgain('n');
                                        }
                                        if (nlevel+nlevelboost>15) nlevelboost=15-nlevel;
                                        nlevel+=nlevelboost;
                                        break;
                                case 'w':
                                        wlevel-=wlevelboost;
                                        if (wlevel<15) {
                                           wxp+=x;
                                           xptest = (int)(Math.pow(1.55,(double)wlevel)) + 35*wlevel + 50;
                                           if (wxp>xptest) levelgain('w');
                                           //if (wxp > wlevel*30+50) levelgain('w');
                                        }
                                        if (wlevel+wlevelboost>15) wlevelboost=15-wlevel;
                                        wlevel+=wlevelboost;
                                        break;
                                case 'p':
                                        plevel-=plevelboost;
                                        if (plevel<15) {
                                           pxp+=x;
                                           if (neck!=null && neck.number==92) pxp++;//ekkhard cross effect
                                           xptest = (int)(Math.pow(1.55,(double)plevel)) + 35*plevel + 50;
                                           if (pxp>xptest) levelgain('p');
                                           //if (pxp > plevel*30+50) levelgain('p');
                                        }
                                        if (plevel+plevelboost>15) plevelboost=15-plevel;
                                        plevel+=plevelboost;
                                        break;
                                default:
                                        flevel-=flevelboost;
                                        if (flevel<15) {
                                           fxp+=x;
                                           xptest = (int)(Math.pow(1.55,(double)flevel)) + 35*flevel + 50;
                                           if (fxp>xptest) levelgain('f');
                                           //if (fxp > flevel*35+50) levelgain('f');
                                        }
                                        if (flevel+flevelboost>15) flevelboost=15-flevel;
                                        flevel+=flevelboost;
                                        break;
                        }
                }

                public void levelgain(char classgain) {
                        switch (classgain) {
                                case 'f':
                                        flevel++;
                                        fxp=0;
                                        int statinc=randGen.nextInt(3)+2;
                                        //strength+=statinc;
                                        strengthboost-=statinc;
                                        statinc=randGen.nextInt(2)+1;
                                        //dexterity+=statinc;
                                        dexterityboost-=statinc;
                                        statinc=randGen.nextInt(3)+1;
                                        //vitality+=statinc;
                                        vitalityboost-=statinc;
                                        statinc=randGen.nextInt()%5+vitality/4;
                                        if (statinc<1) statinc = 1;
                                        maxhealth+=statinc;
                                        statinc=randGen.nextInt()%5+vitality/4;
                                        if (statinc<1) statinc = 1;
                                        maxstamina+=statinc;
                                        stamina+=statinc;
                                        setMaxLoad();
                                        message.setMessage(name+" gains a fighter level!",heronumber);
                                        break;
                                case 'n':
                                        nlevel++;
                                        nxp=0;
                                        statinc=randGen.nextInt(3)+1;
                                        //strength+=statinc;
                                        strengthboost-=statinc;
                                        statinc=randGen.nextInt(3)+2;
                                        //dexterity+=statinc;
                                        dexterityboost-=statinc;
                                        statinc=randGen.nextInt(3)+1;
                                        //vitality+=statinc;
                                        vitalityboost-=statinc;
                                        statinc=randGen.nextInt()%5+vitality/5;
                                        if (statinc<1) statinc = 1;
                                        maxhealth+=statinc;
                                        statinc=randGen.nextInt()%5+vitality/5;
                                        if (statinc<1) statinc = 1;
                                        maxstamina+=statinc;
                                        stamina+=statinc;
                                        setMaxLoad();
                                        message.setMessage(name+" gains a ninja level!",heronumber);
                                        break;
                                case 'w':
                                        wlevel++;
                                        wxp=0;
                                        statinc=randGen.nextInt(3)+2;
                                        //intelligence+=statinc;
                                        intelligenceboost-=statinc;
                                        statinc=randGen.nextInt()%5+vitality/7;
                                        if (statinc<1) statinc = 1;
                                        maxhealth+=statinc;
                                        statinc=randGen.nextInt()%5+vitality/7;
                                        if (statinc<1) statinc = 1;
                                        maxstamina+=statinc;
                                        stamina+=statinc;
                                        //statinc=randGen.nextInt()%5+intelligence/4;
                                        statinc = 9-wlevel; if (statinc<4) statinc = 4;
                                        statinc = randGen.nextInt(5)+intelligence/statinc;
                                        if (statinc<1) statinc = 1;
                                        maxmana+=statinc;
                                        message.setMessage(name+" gains a wizard level!",heronumber);
                                        break;
                                case 'p':
                                        plevel++;
                                        pxp=0;
                                        statinc=randGen.nextInt(3)+2;
                                        //wisdom+=statinc;
                                        wisdomboost-=statinc;
                                        statinc=randGen.nextInt()%5+vitality/7;
                                        if (statinc<1) statinc = 1;
                                        maxhealth+=statinc;
                                        statinc=randGen.nextInt()%5+vitality/7;
                                        if (statinc<1) statinc = 1;
                                        maxstamina+=statinc;
                                        stamina+=statinc;
                                        //statinc=randGen.nextInt()%5+wisdom/4;
                                        statinc = 9-plevel; if (statinc<4) statinc = 4;
                                        statinc = randGen.nextInt(5)+wisdom/statinc;
                                        if (statinc<1) statinc = 1;
                                        maxmana+=statinc;
                                        message.setMessage(name+" gains a priest level!",heronumber);
                                        break;
                        }
                        if (sheet && herosheet.hero==this) herosheet.repaint();
                }

                //need pass it a subsquare as well
                public void wepThrow(Item wep, int subsq) {
                        int dist = (strength+5)/10-(int)(wep.weight/2.0f)+randGen.nextInt()%2+wep.shotpow/3+nlevel/2;
                        //if (wep.shotpow>10) dist+=wep.shotpow/10;
                        if (dist<2) dist = 2;
                        if (dist<nlevel) dist = nlevel;
                        if (nlevel>0 && (wep.projtype>0 || (wep.type==Item.WEAPON && wep.hasthrowpic))) wep.shotpow+=randGen.nextInt(nlevel)+nlevel;
                        Projectile p;
                        if (wep.isbomb) {
                                try { 
                                    Spell tempspell = new Spell(wep.bombnum);
                                    tempspell.power = wep.potionpow + randGen.nextInt()%10;
                                    p = new Projectile(tempspell,dist,facing,subsq);
                                }
                                catch(Exception e) { p = new Projectile(wep,dist,facing,subsq); }
                        }
                        else p = new Projectile(wep,dist,facing,subsq);
                        load-=wep.weight;
                }

                public int castSpell() {
                        //return 0 for nonsense, 1 for success, 2 for need flask, 3 for need practice, 4 for can't right now, 5 for silenced (fizzles)

                        Spell casting;
                        try { casting = new Spell(currentspell); }
                        catch(Exception e) { return 0; }
                        
                        if (casting.gain==6 && casting.number==3 && (hand==null || weapon==null || hand.number==weapon.number || (hand.number!=83 && hand.number!=285) || (weapon.number!=285 && weapon.number!=83)) ) return 0;

                        weaponcount=casting.gain*6+2;
                        if (hurthead) weaponcount+=4;
                        int i=0;
                        while (i<numheroes) {
                                if (this.equals(hero[i])) {
                                        wepready = false;
                                        if (weaponready==i) weaponsheet.repaint();
                                        break;
                                }
                                i++;
                        }
                        //fizzle if silenced
                        if (silenced) return 5;
                        //head injury causes 1 in 3 to fizzle, 1 in 5 for archmaster
                        if (hurthead) {
                                int injurytest;
                                if (casting.clsgain=='w') {
                                        if (wlevel<15) injurytest=randGen.nextInt(3);
                                        else injurytest=randGen.nextInt(5);
                                }
                                else if (plevel<15) injurytest=randGen.nextInt(3);
                                else injurytest=randGen.nextInt(5);
                                if (injurytest==0) return 5;
                        }
                        //determine if spell successfully cast:
                        int spelldif = casting.gain*currentspell.length();
                        int attempt;
                        if (casting.clsgain=='w') {
                                attempt = randGen.nextInt((wlevel+1)*2)+(wlevel+1);
                                if (wlevel>8) attempt = 24;
                                else if (wlevel>6) attempt+=4;
                                else if (wlevel>4) attempt++;
                        }
                        else {
                                attempt = randGen.nextInt((plevel+1)*2)+(plevel+1);
                                if (plevel>8) attempt = 24;
                                else if (plevel>6) attempt+=4;
                                else if (plevel>4) attempt++;
                        }
                        if (attempt >= spelldif) {
                                //if (casting.clsgain=='w' && casting.type!=2) {
                                if (casting.clsgain=='w' && casting.type!=2 && casting.number!=461 && casting.number!=363 && casting.number!=362 && casting.number!=664 && casting.number!=523) {
                                        //intelligence modifier
                                        for (int j=casting.gain-1;j>=0;j--) {
                                           casting.powers[j]+= randGen.nextInt()%10+j*intelligence/8;
                                           if (wlevel==15) casting.powers[j]+=randGen.nextInt(intelligence/8);
                                           if (casting.powers[j]<1) casting.powers[j]=randGen.nextInt(4)+1;
                                        }
                                        casting.power = casting.powers[casting.gain-1];
                                        //casting.power = randGen.nextInt()%10+casting.gain*intelligence/8+casting.power;
                                        //if (wlevel==15) casting.power+=randGen.nextInt(intelligence/4);
                                        //if (casting.power<1) casting.power=randGen.nextInt(4)+1;
                                }
                                else if (casting.type!=2) {
                                        //wisdom modifier - doesn't affect most potions
                                        if (casting.type!=0 || casting.number==1 || casting.number==2 || casting.number==655) {
                                                casting.power = randGen.nextInt()%10+casting.gain*wisdom/8+casting.power;
                                                if (plevel==15) casting.power+=randGen.nextInt(wisdom/8);
                                                if (casting.power<1) casting.power=randGen.nextInt(4)+1;
                                                if (casting.power<4 && (casting.number==1 || casting.number==2 || casting.number==655)) casting.power=randGen.nextInt(4)+4;
                                                if (casting.number==655) {
                                                        int cost = casting.gain+SYMBOLCOST[5][casting.gain-1]+SYMBOLCOST[10][casting.gain-1]+SYMBOLCOST[16][casting.gain-1];
                                                        //System.out.println("cost = "+cost);
                                                        if (plevel==15 && casting.power>cost*3/4) casting.power=cost*3/4;
                                                        else if (casting.power>cost*2/3) casting.power = cost*2/3;
                                                }
                                        }
                                }
                                //test for binding spells
                                if (casting.gain==6 && casting.number==4 && hand!=null && weapon!=null && ( (hand.number==83 && !hand.bound[0] && weapon.number==282) || (weapon.number==83 && !weapon.bound[0] && hand.number==282))) {
                                        //fire
                                        message.setMessage("Fire has been bound.",4);
                                        if (hand.number==282) {
                                                load-=hand.weight;
                                                hand = null;
                                                weapon.bound[0] = true;
                                                if (weapon.bound[1] && weapon.bound[2] && weapon.bound[3]) {
                                                        //forging complete
                                                        load-=weapon.weight;
                                                        weapon = new Item(248);
                                                        load+=weapon.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        else {
                                                load-=weapon.weight;
                                                weapon = fistfoot;
                                                hand.bound[0] = true;
                                                if (hand.bound[1] && hand.bound[2] && hand.bound[3]) {
                                                        //forging complete
                                                        load-=hand.weight;
                                                        hand = new Item(248);
                                                        load+=hand.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                        return 1;
                                }
                                else if (casting.gain==6 && casting.number==2 && hand!=null && weapon!=null && ( (hand.number==83 && !hand.bound[1] && weapon.number==283) || (weapon.number==83 && !weapon.bound[1] && hand.number==283))) {
                                        message.setMessage("Water has been bound.",4);
                                        if (hand.number==283) {
                                                load-=hand.weight;
                                                hand = null;
                                                weapon.bound[1] = true;
                                                if (weapon.bound[0] && weapon.bound[2] && weapon.bound[3]) {
                                                        //forging complete
                                                        load-=weapon.weight;
                                                        weapon = new Item(248);
                                                        load+=weapon.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        else {
                                                load-=weapon.weight;
                                                weapon = fistfoot;
                                                hand.bound[1] = true;
                                                if (hand.bound[0] && hand.bound[2] && hand.bound[3]) {
                                                        //forging complete
                                                        load-=hand.weight;
                                                        hand = new Item(248);
                                                        load+=hand.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                        return 1;
                                }
                                else if (casting.gain==6 && casting.number==1 && hand!=null && weapon!=null && ( (hand.number==83 && !hand.bound[2] && weapon.number==284) || (weapon.number==83 && !weapon.bound[2] && hand.number==284))) {
                                        //earth
                                        message.setMessage("Earth has been bound.",4);
                                        if (hand.number==284) {
                                                load-=hand.weight;
                                                hand = null;
                                                weapon.bound[2] = true;
                                                if (weapon.bound[0] && weapon.bound[1] && weapon.bound[3]) {
                                                        //forging complete
                                                        load-=weapon.weight;
                                                        weapon = new Item(248);
                                                        load+=weapon.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        else {
                                                load-=weapon.weight;
                                                weapon = fistfoot;
                                                hand.bound[2] = true;
                                                if (hand.bound[0] && hand.bound[1] && hand.bound[3]) {
                                                        //forging complete
                                                        load-=hand.weight;
                                                        hand = new Item(248);
                                                        load+=hand.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                        return 1;
                                }
                                else if (casting.gain==6 && casting.number==3 && hand!=null && weapon!=null && ( (hand.number==83 && !hand.bound[3] && weapon.number==285) || (weapon.number==83 && !weapon.bound[3] && hand.number==285))) {
                                        //wind
                                        message.setMessage("Wind has been bound.",4);
                                        if (hand.number==285) {
                                                load-=hand.weight;
                                                hand = null;
                                                weapon.bound[3] = true;
                                                if (weapon.bound[0] && weapon.bound[1] && weapon.bound[2]) {
                                                        //forging complete
                                                        load-=weapon.weight;
                                                        weapon = new Item(248);
                                                        load+=weapon.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        else {
                                                load-=weapon.weight;
                                                weapon = fistfoot;
                                                hand.bound[3] = true;
                                                if (hand.bound[0] && hand.bound[1] && hand.bound[2]) {
                                                        //forging complete
                                                        load-=hand.weight;
                                                        hand = new Item(248);
                                                        load+=hand.weight;
                                                        message.setMessage("The Firestaff is Complete.",4);
                                                }
                                        }
                                        repaint();
                                        if (sheet) herosheet.repaint();
                                        return 1;
                                }
                                else if (casting.number==3) return 0;
                                if (casting.type==0) {
                                        if (hand!=null && hand.number==7) {
                                                load-=hand.weight;
                                                hand=new Item(casting.potionnum, casting.power, casting.gain);
                                                load+=hand.weight;
                                        }
                                        else if (weapon.number==7) {
                                                load-=weapon.weight;
                                                weapon=new Item(casting.potionnum, casting.power, casting.gain);
                                                load+=weapon.weight;
                                        }
                                        else return 2;

                                        hupdate();
                                }
                                else if (casting.type==1) {
                                        int projsub = subsquare;
                                        if (projsub==2) projsub=1;
                                        else if (projsub==3) projsub=0;
                                        Projectile p = new Projectile(casting,casting.dist,facing,projsub);
                                }
                                else if (casting.type==2) {
                                        int castresult = specialSpell(casting);
                                        //if (castresult==1) gainxp(casting.clsgain,currentspell.length());
                                        if (castresult==1) gainxp(casting.clsgain,casting.gain-1+currentspell.length());
                                        return castresult;
                                        //could put test here for if cast special, 3 part spell
                                        //return nonsense so can't stumble across it by accident
                                        //1st 2 parts inc an int, 2nd cast before first, or 3rd before 2nd or 1st, int is reset
                                        //with 3rd casting, if int==2, effect happens (generates an item or something)
                                }
                                gainxp(casting.clsgain,casting.gain-1+currentspell.length());
                                return 1;
                        }
                        else {
                                gainxp(casting.clsgain,casting.gain);
                                if (casting.clsgain=='w') spellclass="wizard";
                                else spellclass="priest";
                                return 3;
                        }
                }

                public int specialSpell(Spell s) {
                        //magicvision
                        if (s.number==325) { magicvision+=s.power; if (magicvision==s.power) needredraw=true; }
                        //magic torch
                        else if (s.number==4) { 
                             //darkcounter=0;
                             magictorch+=s.power; if (magictorch>285) magictorch=285; //can stock up some, but not infinite
                             if ((darkfactor+s.power)>255) darkfactor=255;
                             else darkfactor+=s.power;
                             needredraw=true;
                        }
                        //perhaps make these not cumulative (ie defenseboot=s.power instead of +=)
                        //shield party
                        else if (s.number==14) {
                             if (plevel==15) s.power+=10;
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].defenseboost<s.power) {
                                        hero[i].defense-=hero[i].defenseboost;
                                        hero[i].defenseboost=s.power;
                                        hero[i].defense+=s.power;
                                        hero[i].repaint();
                                }
                             }
                        }
                        //shield party from magic
                        else if (s.number==64) {
                             if (plevel==15) s.power+=10;
                             for (int i=0;i<numheroes;i++) {   
                                if (hero[i].magicresistboost<s.power) {
                                        hero[i].magicresist-=hero[i].magicresistboost;
                                        hero[i].magicresistboost=s.power;
                                        hero[i].magicresist+=s.power;
                                        hero[i].repaint();
                                }
                             }
                        }
                        //float
                        else if (s.number==344) { 
                                floatcounter+=s.power; 
                                //if (!climbing) message.setMessage("Float active.",4);
                                if (!climbing) message.setMessage("Slowfall active.",4);
                                climbing=true;
                        }
                        /*
                        //invisibility
                        else if (s.number==326) { 
                                if (invisible<s.power) {
                                        //test if successful here -> should always succeed? fail if in sight of mons? maybe mons in sight aren't affected by it? maybe some mons never affected by it?
                                        //"in sight" would be hard to determine...
                                        //maybe just dodge more easily/often? -> change name to "partial invisibility"
                                        //maybe just keep mons from going after/attacking until duration expires or champion attacks or casts?
                                        invisible+=s.power; 
                                        if (invisible<=s.power) message.setMessage("Invisibility active.",4);
                                }
                                else return 4;
                        }
                        */
                        //magic sword
                        else if (s.number==121) { 
                                if (weapon.number==219 && kuswordcount<s.power) {
                                        kuswordcount=s.power;
                                        weapon.power[0]=s.gain*2+6;
                                        return 1;
                                }
                                else if (weapon!=fistfoot) return 4;
                                weapon = new Item(219);
                                weapon.power[0] = s.gain*2+6;
                                kuswordcount=s.power;
                                repaint();
                        }
                        //magic bow
                        else if (s.number==122) { 
                                if (weapon.number==261 && rosbowcount<s.power) {
                                        rosbowcount=s.power;
                                        weapon.power[0]=s.gain;
                                        return 1;
                                }
                                else if (weapon!=fistfoot) return 4;
                                weapon = new Item(261);
                                weapon.power[0] = s.gain;
                                rosbowcount=s.power;
                                repaint();
                        }
                        //detect curse
                        else if (s.number==364) {
                                if (detectcurse<s.power) {
                                        detectcurse=s.power;
                                }
                        }
                        //dispell illusion - only archmaster can cast, but also an item ability
                        else if (s.number==322) {
                                if (plevel<15) { spellclass="priest"; return 3; }//failure
                                dispell+=s.power;
                                if (dispell==s.power) needredraw=true;
                        }
                        //drain life
                        else if (s.number==666) { 
                                int xadjust=partyx,yadjust=partyy;

                                if (facing==NORTH) yadjust--;
                                else if (facing==WEST) xadjust--;
                                else if (facing==SOUTH) yadjust++;
                                else xadjust++;
                                Monster tempmon = (Monster)dmmons.get(level+","+xadjust+","+yadjust+","+5);
                                int i=3;
                                while (tempmon==null && i>=0) {
                                        tempmon=(Monster)dmmons.get(level+","+xadjust+","+yadjust+","+((i-facing+4)%4));
                                        i--;
                                }
                                if (tempmon!=null) {
                                        //make drain sound - pic would be hard to do
                                        playSound("drain.wav",-1,-1);
                                        int hit = tempmon.damage(s.power,DRAINHIT);
                                        if (hit>=0) { heal(hit); message.setMessage(name+" drains "+hit+" life from "+tempmon.name,heronumber); }
                                        else damage(hit,SPELLHIT);
                                }
                                else return 4;
                        }
                        //zo kath ra
                        else if (s.number==635) {
                                //create zo kath ra item (which is weightless and has no equip effects)
                                if (weapon.name.equals("Fist/Foot")) { weapon = new Item(80); repaint(); }
                                else if (hand==null) { hand = new Item(80); repaint(); }
                                else if (DungeonMap[level][partyx][partyy].canHoldItems) { DungeonMap[level][partyx][partyy].addItem(new Item(80)); needredraw = true; }
                                else return 4;//fail -> nowhere to put it
                        }
                        return 1;
                }

                public bool usePotion(Item inhand) {
                        switch (inhand.number) {
                                //stat boosters
                                //antidote
                                //shield(s)
                                case 10:
                                        //health
                                        heal(inhand.potionpow);
                                        int numhealed = randGen.nextInt(inhand.potioncastpow)+inhand.potioncastpow/3;
                                        if (numhealed==0 && inhand.potioncastpow>4) numhealed+=(inhand.potioncastpow-4);
                                        //if can heal, test for injuries
                                        if (numhealed>0 && (hurthead || hurttorso || hurtlegs || hurtfeet || hurthand || hurtweapon)) {
                                                ArrayList intlist = new ArrayList(6);
                                                intlist.add(new Integer(0)); intlist.add(new Integer(1)); intlist.add(new Integer(2));
                                                intlist.add(new Integer(3)); intlist.add(new Integer(4)); intlist.add(new Integer(5));
                                                int counter=0,index=randGen.nextInt(6);
                                                while (numhealed>0 && counter<6) {
                                                        switch ( ((Integer)intlist.remove(index)).intValue() ) {
                                                                case 0:
                                                                        //head
                                                                        if (hurthead) {
                                                                                hurthead=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 1:
                                                                        //torso
                                                                        if (hurttorso) {
                                                                                hurttorso=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 2:
                                                                        //legs
                                                                        if (hurtlegs) {
                                                                                hurtlegs=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 3:
                                                                        //feet
                                                                        if (hurtfeet) {
                                                                                hurtfeet=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 4:
                                                                        //hand
                                                                        if (hurthand) {
                                                                                hurthand=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                                case 5:
                                                                        //weapon
                                                                        if (hurtweapon) {
                                                                                hurtweapon=false;
                                                                                numhealed--;
                                                                        }
                                                                        break;
                                                        }
                                                        counter++;
                                                        if (counter<6) index = (index+1)%(6-counter);
                                                }
                                        }
                                        break;
                                case 11:
                                        //stamina
                                        vitalize(inhand.potionpow);
                                        //maxload=strength*4/5;
                                        //if (stamina<maxstamina/5) maxload=maxload*3/4;
                                        break;
                                case 12:
                                        //mana
                                        //energize(inhand.potionpow);
                                        mana+=inhand.potionpow;
                                        break;
                                case 13:
                                        //strength boost
                                        if (strength>150) return false;
                                        strengthboost+=inhand.potionpow;
                                        strength+=inhand.potionpow;
                                        setMaxLoad();
                                        break;
                                case 14:
                                        //dexterity boost
                                        if (dexterity>150) return false;
                                        dexterityboost+=inhand.potionpow;
                                        dexterity+=inhand.potionpow;
                                        break;
                                case 15:
                                        //vitality boost
                                        if (vitality>150) return false;
                                        vitalityboost+=inhand.potionpow;
                                        vitality+=inhand.potionpow;
                                        break;
                                case 16:
                                        //intelligence boost
                                        if (intelligence>150) return false;
                                        intelligenceboost+=inhand.potionpow;
                                        intelligence+=inhand.potionpow;
                                        break;
                                case 17:
                                        //wisdom boost
                                        if (wisdom>150) return false;
                                        wisdomboost+=inhand.potionpow;
                                        wisdom+=inhand.potionpow;
                                        break;
                                case 18:
                                        //defense boost - shield potion
                                        if (defenseboost<inhand.potionpow) {
                                                defense-=defenseboost;
                                                defenseboost=inhand.potionpow;
                                                defense+=inhand.potionpow;
                                        }
                                        break;
                                case 19:
                                        //magic defense boost
                                        if (magicresistboost<inhand.potionpow) {
                                                magicresist-=magicresistboost;
                                                magicresistboost+=inhand.potionpow;
                                                magicresist+=inhand.potionpow;
                                        }
                                        break;
                                case 20:
                                        //anti-ven
                                        poison-=inhand.potionpow;
                                        if (poison<1) {
                                                ispoisoned=false;
                                                poison=0;
                                        }
                                        break;
                                case 24:
                                        //anti-silence
                                        silencecount-=inhand.potionpow;
                                        if (silencecount<1) {
                                                silenced=false;
                                                silencecount=0;

                                        }
                                        break;
                                case 25:
                                        //remove curse
                                        //find a cursed item in inventory, decrease its cursed int
                                        Item toremove = null;
                                        if (weapon.cursed>0) toremove=weapon;
                                        else if (hand!=null && hand.cursed>0) toremove=hand;
                                        else if (head!=null && head.cursed>0) toremove=head;
                                        else if (neck!=null && neck.cursed>0) toremove=neck;
                                        else if (torso!=null && torso.cursed>0) toremove=torso;
                                        else if (legs!=null && legs.cursed>0) toremove=legs;
                                        else if (feet!=null && feet.cursed>0) toremove=feet;
                                        if (toremove!=null) {
                                                toremove.cursed-=inhand.potionpow;
                                                if (toremove.cursed<0) toremove.cursed=0;
                                                if (toremove.cursed==0) {
                                                        message.setMessage("The curse on "+toremove.name+" is broken.",4);
                                                        if (sheet) herosheet.repaint();
                                                        if (toremove==weapon || toremove==hand) repaint();
                                                }
                                        }
                                        break;
                                default:
                                        return false;
                        }
                        return true;
                }

                public int damage(int ht,int type) {
                     if (!isdead) {

                        if (sleeping && type!=POISONHIT) {
                                needredraw = true;
                                sleeping = false;
                                sleeper = 0;
                                sheet=false;
                                showCenter(dview);
                                spellsheet.setVisible(true);
                                weaponsheet.setVisible(true);
                                arrowsheet.setVisible(true);
                                weaponsheet.repaint();
                        }
                        else if (dmmap!=null && mappane.isVisible() && type!=POISONHIT) {
                                needredraw = true;
                                sheet=false;
                                showCenter(dview);
                                weaponsheet.repaint();
                                mappane.setVisible(false);
                                toppanel.setVisible(true);
                                centerpanel.setVisible(true);
                                /*
                                ecpanel.setVisible(true);
                                if (spellsheet!=null) {
                                        spellsheet.setVisible(true);
                                        weaponsheet.setVisible(true);
                                }
                                arrowsheet.setVisible(true);
                                message.setVisible(true);
                                hpanel.setVisible(true);
                                formation.setVisible(true);
                                toppanel.setVisible(true);
                                maincenterpan.setVisible(true);
                                eastpanel.setVisible(true);
                                */
                                validate();
                                //setContentPane(imagePane);
                                //validate();
                        }
                        bool hashurt = (hurtweapon || hurthand || hurthead || hurttorso || hurtlegs || hurtfeet);
                        if (type==WEAPONHIT || type==PROJWEAPONHIT) {
                                int olddefense = defense;
                                if (hashurt) {
                                        if (hurthand && hand!=null && hand.type==Item.SHIELD && hand.defense>0) defense-=hand.defense;
                                        if (hurtweapon && weapon!=null && weapon.type==Item.SHIELD && weapon.defense>0) defense-=weapon.defense;
                                        if (defense<0) defense=0;
                                        else defense = defense*2/3;
                                }
                                ht-=(ht*defense/100);
                                defense = olddefense;
                        }        
                        else if (type==SPELLHIT || type==DRAINHIT) {
                                int oldmagicresist = magicresist;
                                if (hashurt) {
                                        if (hurthand && hand!=null && hand.type==Item.SHIELD && hand.magicresist>0) magicresist-=hand.magicresist;
                                        if (hurtweapon && weapon!=null && weapon.type==Item.SHIELD && weapon.magicresist>0) magicresist-=weapon.magicresist;
                                        if (magicresist<0) magicresist=0;
                                        else magicresist = magicresist*2/3;
                                }
                                ht-=(ht*magicresist/100);
                                magicresist = oldmagicresist;
                        }
                        else if (type==DOORHIT && head!=null) ht-=(ht*head.defense/100);
                        if (ht<1) ht=randGen.nextInt(4)+1;
                        else if (DIFFICULTY<0) ht = (int)(((double)ht)*(1.0+((double)DIFFICULTY)*.25));
                        if (berserkhealth>0) {
                                berserkhealth-=ht;
                                if (berserkhealth<0) {
                                        health+=berserkhealth; //subtracts from health, since berserkhealth is negative
                                        berserkhealth = 0;
                                }
                        }
                        else health-=ht;
                        //if (hurtcounter>0) hit+=ht;
                        //else hit=ht;
                        hit=ht;
                        if (health<1) {
                                isdead=true;
                                removeMouseListener(hclick);
                                repaint();
                                if (weapon.number==215) type=STORMHIT;
                                dropAllItems(type); load=0.0f;
                                health = 0;
                                currentspell="";
                                ispoisoned = false; poison=0;
                                silencecount = 0; silenced = false;
                                detectcurse = 0; falseimage = 0; berserk = 0; berserkhealth = 0; berserkstr = 0; backstab = false;
                                abilityactive = -1; abilityimm = false; abilitydiamond = false; abilitystun = false; abilitypoison = 0;
                                //close herosheet
                                if (!sleeping && sheet && this.equals(herosheet.hero)) {
                                        herosheet.skipchestscroll = false;
                                        if (viewing) {
                                                viewing=false;
                                                if (iteminhand && inhand.number==4) {
                                                        inhand.pic = inhand.temppic;
                                                        if (!isleader) changeCursor();
                                                }
                                        }
                                        sheet=false;

                                        showCenter(dview);
                                }
                                //if you die with stormbringer in hand, your soul is consumed
                                updateDark();
                                formation.mouseExited(null);
                                heroatsub[subsquare]=-1;
                                formation.addNewHero();//removes as well
                                int i=0;
                                if (isleader) {
                                        isleader=false;
                                        int j = 0;
                                        while (j<numheroes) {
                                                if (j!=heronumber && !hero[j].isdead) {
                                                        hero[j].isleader=true;
                                                        leader=j;
                                                        if (sheet && herosheet.hero.heronumber==j) herosheet.repaint();
                                                        break;
                                                }
                                                else j++;
                                        }
                                }
                                if (leader==heronumber) {
                                        //System.out.println("all died");
                                        playSound("scream.wav",-1,-1);
                                        iteminhand = false;
                                        changeCursor();
                                        alldead=true;
                                        gameover = true;
                                        if (sleeping) {
                                                //System.out.println("all died while sleeping");
                                                sleeping = false;
                                                sleeper = 0;
                                                sheet=false;
                                                needredraw = true;
                                                showCenter(dview);
                                                spellsheet.setVisible(true);
                                                weaponsheet.setVisible(true);
                                                arrowsheet.setVisible(true);
                                                weaponsheet.repaint();
                                        }
                                        else if (dmmap!=null && dmmap.isShowing()) {
                                                needredraw = true;
                                                sheet=false;
                                                showCenter(dview);
                                                weaponsheet.repaint();
                                                setContentPane(imagePane);
                                                validate();
                                        }
                                }
                                else {
                                        if (spellready==heronumber) {
                                                spellready=leader;
                                        }
                                        if (weaponready==heronumber) {
                                                weaponready=leader;
                                                if (weaponsheet.showingspecials) weaponsheet.toggleSpecials(leader);
                                        }
                                        weaponsheet.repaint();
                                        spellsheet.repaint();
                                        strength-=strengthboost; strengthboost=0;
                                        vitality-=vitalityboost; vitalityboost=0;
                                        dexterity-=dexterityboost; dexterityboost=0;
                                        intelligence-=intelligenceboost; intelligenceboost=0;
                                        wisdom-=wisdomboost; wisdomboost=0;
                                        defense-=defenseboost; defenseboost=0;
                                        magicresist-=magicresistboost; magicresistboost=0;
                                        hurtweapon = false; hurthand = false; hurthead = false; hurttorso = false; hurtlegs = false; hurtfeet = false;
                                }
                        }
                        else { 
                           //hurtcounter = 3; paint(getGraphics()); if (sheet && !viewing && !sleeping) herosheet.repaint();
                           hurtcounter = 3; repaint(); if (sheet && !viewing && !sleeping) herosheet.repaint();
                           if (type==WEAPONHIT) gainxp('f',1);
                           //cause injuries if hit is hard enough and right type
                           //if a hit does half or more of total hp, 90% chance of injury
                           //if ((type==WEAPONHIT || type==SPELLHIT || type==PROJWEAPONHIT) && (10*ht)/maxhealth>=5 && (!hurtweapon || !hurthand || !hurthead || !hurttorso || !hurtlegs || !hurtfeet) && randGen.nextInt(10)>0) {
                           if ((type==WEAPONHIT || type==SPELLHIT || type==PROJWEAPONHIT) && (ht>100 || (ht>maxhealth/4 && ht>40)) && (!hurtweapon || !hurthand || !hurthead || !hurttorso || !hurtlegs || !hurtfeet) && randGen.nextInt(10)>0) {
                                ArrayList intlist = new ArrayList(6);
                                intlist.add(new Integer(0)); intlist.add(new Integer(1)); intlist.add(new Integer(2));
                                intlist.add(new Integer(3)); intlist.add(new Integer(4)); intlist.add(new Integer(5));
                                int i=6,index;
                                bool found=false;
                                while (!found && i>0) {
                                   index=randGen.nextInt(i);
                                   switch ( ((Integer)intlist.remove(index)).intValue() ) {
                                        case 0://head
                                                if (!hurthead) {
                                                        hurthead=true;
                                                        found=true;
                                                }
                                                break;
                                        case 1://torso
                                                if (!hurttorso) {
                                                        hurttorso=true;
                                                        found=true;
                                                }
                                                break;
                                        case 2://legs
                                                if (!hurtlegs) {
                                                        hurtlegs=true;
                                                        found=true;
                                                }
                                                break;
                                        case 3://feet
                                                if (!hurtfeet) {
                                                        hurtfeet=true;
                                                        found=true;
                                                }
                                                break;
                                        case 4://hand
                                                if (!hurthand) {
                                                        hurthand=true;
                                                        found=true;
                                                }
                                                break;
                                        case 5://weapon
                                                if (!hurtweapon) {
                                                        hurtweapon=true;
                                                        found=true;
                                                }
                                                break;
                                   }
                                   i--;
                                }
                                /*
                                int hurtnum=-1,hurtval=101;
                                ArrayList intlist = new ArrayList(6);
                                intlist.add(new Integer(0)); intlist.add(new Integer(1)); intlist.add(new Integer(2));
                                intlist.add(new Integer(3)); intlist.add(new Integer(4)); intlist.add(new Integer(5));
                                int i=0,index=randGen.nextInt(6);
                                bool found=false;
                                while (!found && i<6) {
                                        if (!injured[i]) switch (i) {
                                                case 0://head
                                                        if (head==null) {
                                                                hurtnum=0;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && head.magicresist<hurtval) {
                                                                hurtnum=0;
                                                                hurtval=head.magicresist;
                                                        }
                                                        else if (head.defense<hurtval) {
                                                                hurtnum=0;
                                                                hurtval=head.defense;
                                                        }
                                                        break;
                                                case 1://torso
                                                        if (torso==null) {
                                                                hurtnum=1;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && torso.magicresist<hurtval) {
                                                                hurtnum=1;
                                                                hurtval=torso.magicresist;
                                                        }
                                                        else if (torso.defense<hurtval) {
                                                                hurtnum=1;
                                                                hurtval=torso.defense;
                                                        }
                                                        break;
                                                case 2://legs
                                                        if (legs==null) {
                                                                hurtnum=2;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && legs.magicresist<hurtval) {
                                                                hurtnum=2;
                                                                hurtval=legs.magicresist;
                                                        }
                                                        else if (legs.defense<hurtval) {
                                                                hurtnum=2;
                                                                hurtval=legs.defense;
                                                        }
                                                        break;
                                                case 3://feet
                                                        if (feet==null) {
                                                                hurtnum=3;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && feet.magicresist<hurtval) {
                                                                hurtnum=3;
                                                                hurtval=feet.magicresist;
                                                        }
                                                        else if (feet.defense<hurtval) {
                                                                hurtnum=3;
                                                                hurtval=feet.defense;
                                                        }
                                                        break;
                                                case 4://ready hand
                                                        if (hand==null) {
                                                                hurtnum=4;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && hand.magicresist<hurtval) {
                                                                hurtnum=4;
                                                                hurtval=hand.magicresist;
                                                        }
                                                        else if (hand.defense<hurtval) {
                                                                hurtnum=4;
                                                                hurtval=hand.defense;
                                                        }
                                                        break;
                                                case 5://action hand
                                                        if (weapon==null) {
                                                                hurtnum=5;
                                                                hurtval=0;
                                                                found=true;
                                                        }
                                                        else if (type==SPELLHIT && weapon.magicresist<hurtval) {
                                                                hurtnum=5;
                                                                hurtval=weapon.magicresist;
                                                        }
                                                        else if (weapon.defense<hurtval) {
                                                                hurtnum=5;
                                                                hurtval=weapon.defense;
                                                        }
                                                        break;
                                        }
                                }
                                */
                           }
                        }
                        return ht;
                     }
                     else return 0;
                }

                private void dropAllItems(int type) {
                        MapObject p = DungeonMap[level][partyx][partyy];
                        for (int i=0;i<16;i++) {
                                if (pack[i]!=null) {
                                        pack[i].subsquare=(subsquare-facing+4)%4;
                                        if (!p.tryTeleport(pack[i])) {
                                                p.addItem(pack[i]);
                                                p.tryFloorSwitch(MapObject.PUTITEM);
                                        }
                                        pack[i] = null;
                                }
                        }
                        if (pouch2!=null) {
                                pouch2.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(pouch2)) {
                                        p.addItem(pouch2);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                pouch2 = null;
                        }
                        if (pouch1!=null) {
                                pouch1.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(pouch1)) {
                                        p.addItem(pouch1);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                pouch1 = null;
                        }
                        for (int i=0;i<6;i++) {
                                if (quiver[i]!=null) {
                                        quiver[i].subsquare=(subsquare-facing+4)%4;
                                        if (!p.tryTeleport(quiver[i])) {
                                                p.addItem(quiver[i]);
                                                p.tryFloorSwitch(MapObject.PUTITEM);
                                        }
                                        quiver[i] = null;
                                }
                        }
                        if (feet!=null) {
                                feet.unEquipEffect(this);
                                feet.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(feet)) {
                                        p.addItem(feet);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                feet = null;
                        }
                        if (legs!=null) {
                                legs.unEquipEffect(this);
                                legs.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(legs)) {
                                        p.addItem(legs);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                legs = null;
                        }
                        if (torso!=null) {
                                torso.unEquipEffect(this);
                                torso.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(torso)) {
                                        p.addItem(torso);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                torso = null;
                        }
                        if (neck!=null) {
                                neck.unEquipEffect(this);
                                if (neck.number==89) numillumlets--;
                                neck.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(neck)) {
                                        p.addItem(neck);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                neck = null;
                        }
                        if (head!=null) {
                                head.unEquipEffect(this);
                                head.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(head)) {
                                        p.addItem(head);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                head = null;
                        }
                        if (hand!=null) {
                                if (hand.number==9) {
                                        ((Torch)hand).putOut();
                                        //updateDark();
                                }
                                if (hand.type==Item.SHIELD) hand.unEquipEffect(this);
                                hand.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(hand)) {
                                        p.addItem(hand);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                hand = null;
                        }
                        if (!weapon.name.equals("Fist/Foot")) {
                                if (weapon.number==9) {
                                        ((Torch)weapon).putOut();
                                        //updateDark();
                                }
                                if (weapon.type==Item.WEAPON || weapon.type==Item.SHIELD) weapon.unEquipEffect(this);
                                if (weapon.number!=219 && weapon.number!=261) {
                                        weapon.subsquare=(subsquare-facing+4)%4;
                                        if (!p.tryTeleport(weapon)) {
                                                p.addItem(weapon);
                                                p.tryFloorSwitch(MapObject.PUTITEM);
                                        }
                                }
                                weapon=fistfoot;
                        }
                        if (isleader && iteminhand) {
                                iteminhand=false;
                                inhand.subsquare=(subsquare-facing+4)%4;
                                if (!p.tryTeleport(inhand)) {
                                        p.addItem(inhand);
                                        p.tryFloorSwitch(MapObject.PUTITEM);
                                }
                                changeCursor();
                        }
                        Item bones;
                        if (type!=STORMHIT) bones = new Item(name,heronumber);
                        else { bones = new Item(75); bones.name = name+" Bones"; }
                        bones.subsquare=(subsquare-facing+4)%4;
                        if (!p.tryTeleport(bones)) {
                                p.addItem(bones);
                                p.tryFloorSwitch(MapObject.PUTITEM);
                        }
                        needredraw = true;
                }

                public void heal(int h) {
                        health+=h;
                        if (health>maxhealth) health=maxhealth;
                        else if (health<=0) health=1;
                }

                public void vitalize(int s) {
                        stamina+=s;
                        if (stamina>maxstamina) stamina=maxstamina;
                        else if (stamina<=0) stamina=1;
                        setMaxLoad();
                        if (sheet && herosheet.hero.heronumber==heronumber) herosheet.repaint();
                }

                public void energize(int m) {
                        mana+=m;
                        if (mana<0) mana=0;
                }

                public void paintComponent(Graphics gg) {
                      if (!isdead) {
                        gg.setColor(Color.black);
                        gg.fillRect(0,0,getWidth(),getHeight());
                        Graphics2D g = (Graphics2D)gg;
                        g.setFont(dungfont14);
                        if (TEXTANTIALIAS) g.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING,RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
//                        g.clearRect(0,0,getWidth(),getHeight());
                        g.setColor(new Color(100,100,100));
                        g.fillRect(0,0,getWidth(),84);
                        g.setColor(new Color(150,150,150));
                        g.fillRect(0,0,getWidth(),18);
                        if (hurtcounter>0) {
                                g.setColor(Color.red);
                                g.fillRect(5,20,60,60);
                                //g.setFont(new Font("TimesRoman",Font.BOLD,14));
                                g.setColor(new Color(30,30,30));
                                g.drawstring("< "+hit+" >",17,50);
                                g.setColor(Color.yellow);
                                g.drawstring("< "+hit+" >",14,47);
                        }
                        else g.drawImage(pic,5,20,this);

                        //Double tempd=new Double(((float)health/(float)maxhealth)*60.0);
                        //int len=tempd.intValue();
                        int len = (int)((float)health/(float)maxhealth*60.0f);
                        if (len>60) len=60;
                        g.setColor(new Color(20,20,20));
                        g.fillRect(72,82-len,5,len);
                        g.setColor(new Color(0,150,0));
                        g.fillRect(70,80-len,5,len);

                        //tempd=new Double(((float)stamina/(float)maxstamina)*60.0);
                        //len=tempd.intValue();
                        len = (int)((float)stamina/(float)maxstamina*60.0f);
                        if (len>60) len=60;
                        g.setColor(new Color(20,20,20));
                        g.fillRect(82,82-len,5,len);
                        g.setColor(new Color(250,200,0));
                        g.fillRect(80,80-len,5,len);

                        //tempd=new Double(((float)mana/(float)maxmana)*60.0);
                        //len=tempd.intValue();
                        len = (int)((float)mana/(float)maxmana*60.0f);
                        if (len>60) len=60;
                        g.setColor(new Color(20,20,20));
                        g.fillRect(92,82-len,5,len);
                        g.setColor(new Color(100,0,240));
                        g.fillRect(90,80-len,5,len);

                        g.setColor(new Color(30,30,30));
                        g.drawstring(name,7,16);//14);
                        if (isleader) g.setColor(new Color(240,220,0));
                        else g.setColor(Color.white);
                        g.drawstring(name,5,14);//12);

                        if (heronumber==0) g.setColor(new Color(0,216,0));
                        else if (heronumber==1) g.setColor(new Color(236,217,0));
                        else if (heronumber==2) g.setColor(Color.red);
                        else g.setColor(Color.blue);
                        g.fillRect(getWidth()-10,0,10,10);

                        //draw hands
                        //g.setColor(new Color(50,50,50));
                        //g.fillRect(0,84,this.getSize().width,2);
                        g.setColor(new Color(60,60,60));//was 80,80,80
                        g.fillRect(12,89,32,32);
                        g.fillRect(56,89,32,32);
                        g.setColor(new Color(150,150,150));
                        g.drawRect(10,87,35,35);
                        g.drawRect(11,88,33,33);
                        g.drawRect(54,87,35,35);
                        g.drawRect(55,88,33,33);

                        if (hand!=null) {
                                g.drawImage(hand.pic,12,89,this);
                                if (hand.cursed>0 && hand.cursefound) {
                                        Graphics2D g2 = (Graphics2D)g;
                                        Composite ac = g2.getComposite();
                                        g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER,.48f));
                                        g2.setRenderingHint(RenderingHints.KEY_ALPHA_INTERPOLATION,RenderingHints.VALUE_ALPHA_INTERPOLATION_SPEED);
                                        g2.setRenderingHint(RenderingHints.KEY_RENDERING,RenderingHints.VALUE_RENDER_SPEED);
                                        g2.setColor(new Color(200,0,0));
                                        g2.fillRect(12,89,32,32);
                                        g2.setComposite(ac);
                                }
                        }
                        //else if (!hurthand) 
                        //else g.drawImage(fistfoot.pic,44,89,12,121,0,0,32,32,this);
                     else {
                         AffineTransform tran = AffineTransform.getTranslateInstance(12+32,89);
                         tran.scale(-1.0,1.0);
                         g.drawImage(fistfoot.pic,tran,this);
                     }
                        if (hurthand) {
                                if (hand==null) g.drawImage(dmnew.hurthand,12,89,this);
                                Graphics2D g2 = (Graphics2D)g;
                                g2.setColor(Color.red);
                                g2.setStroke(new BasicStroke(2.0f));
                                g2.drawRect(11,88,34,34);
                        }
                        //if (weapon!=fistfoot || !hurtweapon) 
                        g.drawImage(weapon.pic,56,89,this);
                        if (weapon.cursed>0 && weapon.cursefound) {
                                Graphics2D g2 = (Graphics2D)g;
                                Composite ac = g2.getComposite();
                                g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER,.48f));
                                g2.setRenderingHint(RenderingHints.KEY_ALPHA_INTERPOLATION,RenderingHints.VALUE_ALPHA_INTERPOLATION_SPEED);
                                g2.setRenderingHint(RenderingHints.KEY_RENDERING,RenderingHints.VALUE_RENDER_SPEED);
                                g2.setColor(new Color(200,0,0));
                                g2.fillRect(56,89,32,32);
                                g2.setComposite(ac);
                        }
                        if (hurtweapon) {
                                if (weapon==fistfoot) g.drawImage(dmnew.hurtweapon,56,89,this);
                                Graphics2D g2 = (Graphics2D)g;
                                g2.setColor(Color.red);
                                g2.setStroke(new BasicStroke(2.0f));
                                g2.drawRect(55,88,34,34);
                        }

                        
                        if (sheet && herosheet.hero.equals(this)) {
                                g.setColor(Color.yellow);
                                //g.drawRect(0,0,getWidth()-1,getHeight()-1);
                             g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }
                        else if (ispoisoned) {
                                g.setColor(new Color(0,150,0));
                                g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }
                        else if (magicresistboost>0 && defenseboost>0) {
                                g.setColor(new Color(150,0,150));
                                g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }
                        else if (magicresistboost>0) {
                                g.setColor(Color.red);
                                g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }
                        else if (defenseboost>0) {
                                g.setColor(Color.blue);
                                g.drawRect(2,2,getWidth()-5,getHeight()-5);
                        }

                      }
                      else {
                        gg.setColor(Color.black);
                        gg.fillRect(0,0,getWidth(),getHeight());
                        gg.drawImage(deadheropic,15,20,this);
                      }
                }
                
                public void save(ObjectOutputStream so) throws IOException {
                        so.writeUTF(picname);
                        so.writeInt(subsquare);
                        so.writeInt(number);
                        so.writeUTF(name);
                        so.writeUTF(lastname);
                        so.writeInt(maxhealth);
                        so.writeInt(health);
                        so.writeInt(maxstamina);
                        so.writeInt(stamina);
                        so.writeInt(maxmana);
                        so.writeInt(mana);
                        //so.writeFloat(maxload);
                        so.writeFloat(load);
                        so.writeInt(food);
                        so.writeInt(water);
                        so.writeInt(strength);
                        so.writeInt(vitality);
                        so.writeInt(dexterity);
                        so.writeInt(intelligence);
                        so.writeInt(wisdom);
                        so.writeInt(defense);
                        so.writeInt(magicresist);
                        so.writeInt(strengthboost);
                        so.writeInt(vitalityboost);
                        so.writeInt(dexterityboost);
                        so.writeInt(intelligenceboost);
                        so.writeInt(wisdomboost);
                        so.writeInt(defenseboost);
                        so.writeInt(magicresistboost);
                        so.writeInt(flevel);
                        so.writeInt(nlevel);
                        so.writeInt(plevel);
                        so.writeInt(wlevel);
                        so.writeInt(flevelboost);
                        so.writeInt(nlevelboost);
                        so.writeInt(plevelboost);
                        so.writeInt(wlevelboost);
                        so.writeInt(fxp);
                        so.writeInt(nxp);
                        so.writeInt(pxp);
                        so.writeInt(wxp);
                        so.writebool(isdead);
                        so.writebool(wepready);
                        so.writebool(ispoisoned);
                        if (ispoisoned) {
                                so.writeInt(poison);
                                so.writeInt(poisoncounter);
                        }
                        so.writebool(silenced);
                        if (silenced) so.writeInt(silencecount);
                        so.writebool(hurtweapon);
                        so.writebool(hurthand);
                        so.writebool(hurthead);
                        so.writebool(hurttorso);
                        so.writebool(hurtlegs);
                        so.writebool(hurtfeet);
                        so.writeInt(timecounter);
                        so.writeInt(walkcounter);
                        so.writeInt(spellcount);
                        so.writeInt(weaponcount);
                        so.writeInt(kuswordcount);
                        so.writeInt(rosbowcount);
                        so.writeUTF(currentspell);
                        //write abilities here
                        if (abilities!=null) {
                                so.writeInt(abilities.length);
                                for (int j=0;j<abilities.length;j++) {
                                        abilities[j].save(so);
                                }
                        }
                        else so.writeInt(0);
                        if (weapon==fistfoot) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(weapon);
                        }
                        if (hand==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(hand);
                        }
                        if (head==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(head);
                        }
                        if (torso==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(torso);
                        }
                        if (legs==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(legs);
                        }
                        if (feet==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(feet);
                        }
                        if (neck==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(neck);
                        }
                        if (pouch1==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(pouch1);
                        }
                        if (pouch2==null) so.writebool(false);
                        else {
                                so.writebool(true);
                                so.writeObject(pouch2);
                        }
                        so.writeObject(quiver);
                        so.writeObject(pack);
                }
                public void load(ObjectInputStream si) throws IOException,ClassNotFoundException {
                        subsquare = si.readInt();
                        number = si.readInt();
                        name = si.readUTF();
                        lastname = si.readUTF();
                        maxhealth = si.readInt();
                        health = si.readInt();
                        maxstamina = si.readInt();
                        stamina = si.readInt();
                        maxmana = si.readInt();
                        mana = si.readInt();
                        //maxload = si.readFloat();
                        load = si.readFloat();
                        food = si.readInt();
                        water = si.readInt();
                        strength = si.readInt();
                        vitality = si.readInt();
                        dexterity = si.readInt();
                        intelligence = si.readInt();
                        wisdom = si.readInt();
                        defense = si.readInt();
                        magicresist = si.readInt();
                        strengthboost = si.readInt();
                        vitalityboost = si.readInt();
                        dexterityboost = si.readInt();
                        intelligenceboost = si.readInt();
                        wisdomboost = si.readInt();
                        defenseboost = si.readInt();
                        magicresistboost = si.readInt();
                        flevel = si.readInt();
                        nlevel = si.readInt();
                        plevel = si.readInt();
                        wlevel = si.readInt();
                        flevelboost = si.readInt();
                        nlevelboost = si.readInt();
                        plevelboost = si.readInt();
                        wlevelboost = si.readInt();
                        fxp = si.readInt();
                        nxp = si.readInt();
                        pxp = si.readInt();
                        wxp = si.readInt();
                        isdead = si.readbool();
                        wepready = si.readbool();
                        ispoisoned = si.readbool();
                        if (ispoisoned) {
                                poison = si.readInt();
                                poisoncounter = si.readInt();
                        }
                        silenced = si.readbool();
                        if (silenced) silencecount = si.readInt();
                        hurtweapon = si.readbool();
                        hurthand = si.readbool();
                        hurthead = si.readbool();
                        hurttorso = si.readbool();
                        hurtlegs = si.readbool();
                        hurtfeet = si.readbool();
                        timecounter = si.readInt();
                        walkcounter = si.readInt();
                        spellcount = si.readInt();
                        weaponcount = si.readInt();
                        kuswordcount = si.readInt();
                        rosbowcount = si.readInt();
                        currentspell = si.readUTF();
                        //read abilities here
                        int numabils = si.readInt();
                        if (numabils>0) {
                                abilities = new SpecialAbility[numabils];
                                for (int j=0;j<numabils;j++) {
                                        abilities[j] = new SpecialAbility(si);
                                }
                        }
                        if (si.readbool()) {
                                weapon = (Item)si.readObject();
                                if (weapon.number==9) ((Torch)weapon).setPic();
                        }
                        else weapon=fistfoot;
                        if (si.readbool()) {
                                hand = (Item)si.readObject();
                                if (hand.number==9) ((Torch)hand).setPic();
                        }
                        if (si.readbool()) head = (Item)si.readObject();
                        if (si.readbool()) torso = (Item)si.readObject();
                        if (si.readbool()) legs = (Item)si.readObject();
                        if (si.readbool()) feet = (Item)si.readObject();
                        if (si.readbool()) {
                                neck = (Item)si.readObject();
                                //if (neck.number==89) numillumlets++;
                        }
                        if (si.readbool()) pouch1 = (Item)si.readObject();
                        if (si.readbool()) pouch2 = (Item)si.readObject();
                        quiver = (Item[])si.readObject();
                        pack = (Item[])si.readObject();
                        setMaxLoad();
                }
        }
}
public class NewBehaviourScript : MonoBehaviour {

 // Use this for initialization
 void Start () {
 
 }
 
 // Update is called once per frame
 void Update () {
 
 }
}
