using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreNewt;
using System.Xml;
using System.IO;

namespace WorldCreator
{
    sealed class Engine
    {
        public Root Root;
        public RenderWindow RenderWindow;
        public SceneManager SceneManager;
        public Camera Camera;
        public Viewport Viewport;
        public MOIS.Keyboard Keyboard;
        public MOIS.Mouse Mouse;
        public MOIS.InputManager InputManager;
        public World NewtonWorld;
        public Debugger NewtonDebugger;

        public Level CurrentLevel;
        public ObjectManager ObjectManager;
        int BodyId;

        public const float FixedFPS = 60.0f;
        public const float FixedTimeStep = 1.0f / FixedFPS;
        float TimeAccumulator;
        public long LastTime;
        public float TimeStep;

        public MaterialManager MaterialManager;
        public TextLabeler Labeler;

        public HumanController HumanController;
        public TypedInput TypedInput;
        public CharacterProfileManager CharacterProfileManager;
        public NPCManager NPCManager;

        //CharacterProfileManager CharacterProfileManager;
        //public NPCManager NPCManager;
        public Items Items;
        //public PrizeManager PrizeManager;
        //public Quests Quests;

        //public SoundManager SoundManager;

        public Player User;

        public void Initialise()
        {
            Root = new Root();
            ConfigFile cf = new ConfigFile();
            cf.Load("Resources.cfg", "\t:=", true);

            ConfigFile.SectionIterator seci = cf.GetSectionIterator();

            while (seci.MoveNext())
            {
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                    ResourceGroupManager.Singleton.AddResourceLocation(pair.Value, pair.Key, seci.CurrentKey);
            }

            if (!Root.RestoreConfig())
                if (!Root.ShowConfigDialog())
                    return;

            RenderWindow = Root.Initialise(true, "WorldCreator v.0.0.1");  // @@@@@@@@@@@@@@@ Nazwa okna gry.
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
            
            SceneManager = Root.CreateSceneManager(SceneType.ST_GENERIC);
            Camera = SceneManager.CreateCamera("MainCamera");
            Viewport = RenderWindow.AddViewport(Camera);
            Camera.NearClipDistance = 0.1f;
            Camera.FarClipDistance = 1000.0f;
            Camera.AspectRatio = ((float)RenderWindow.Width / (float)RenderWindow.Height);

            MOIS.ParamList pl = new MOIS.ParamList();
            IntPtr windowHnd;
            RenderWindow.GetCustomAttribute("WINDOW", out windowHnd);
            pl.Insert("WINDOW", windowHnd.ToString());

            InputManager = MOIS.InputManager.CreateInputSystem(pl);

            Keyboard = (MOIS.Keyboard)InputManager.CreateInputObject(MOIS.Type.OISKeyboard, false);
            Mouse = (MOIS.Mouse)InputManager.CreateInputObject(MOIS.Type.OISMouse, false);

            NewtonWorld = new World();
            NewtonDebugger = new Debugger(NewtonWorld);
            NewtonDebugger.Init(SceneManager);

            ObjectManager = new ObjectManager();

            MaterialManager = new MaterialManager();
            MaterialManager.Initialise();

            //CharacterProfileManager = new CharacterProfileManager();
            Items = new Items();
            //PrizeManager = new PrizeManager();  //////////////////// @@ Brand nju staff. Nawet trochę działa :Δ
            //Quests = new Quests();
            //NPCManager = new NPCManager();
            
            Labeler = new TextLabeler(5);

            User = new Player();

            CharacterProfileManager = new CharacterProfileManager();

            NPCManager = new NPCManager();

            HumanController = new HumanController();

            TypedInput = new TypedInput();


            //SoundManager = new SoundManager();         
        }

        public void Update()
        {
            long currentTime = Root.Timer.Milliseconds;
            TimeStep = (currentTime - LastTime) / 1000.0f;
            LastTime = currentTime;
            TimeAccumulator += TimeStep;
            TimeAccumulator = System.Math.Min(TimeAccumulator, FixedTimeStep * (FixedFPS / 15));

            Keyboard.Capture();
            Mouse.Capture();
            Root.RenderOneFrame();
            Labeler.Update();
            User.Update();

            while (TimeAccumulator >= FixedTimeStep)
            {
                TypedInput.Update();
                
                NewtonWorld.Update(FixedTimeStep);
                HumanController.Update();
                ObjectManager.Update();
                TimeAccumulator -= FixedTimeStep;

                        //// mjuzik status i ogarnięcie żeby przełączało na następną piosenkę z plejlisty po zakończeniu poprzedniej


            }
            WindowEventUtilities.MessagePump();
        }

        public void Save()
        {
            XmlTextWriter w = new XmlTextWriter("Media\\Wololo.xml", (Encoding) null);

            w.WriteStartElement("items");

            foreach (GameObject GO in Engine.Singleton.ObjectManager.Objects)
            {
                if (GO.GetType().ToString() == "WorldCreator.Described" && (GO as Described).Profile.ProfileName[0] != 's')
                {
                    w.WriteStartElement("item");
                    w.WriteElementString("DescribedProfile", (GO as Described).Profile.ProfileName);
                    w.WriteElementString("Position_x", (GO as Described).Position.x.ToString());
                    w.WriteElementString("Position_y", (GO as Described).Position.y.ToString());
                    w.WriteElementString("Position_z", (GO as Described).Position.z.ToString());
                    w.WriteElementString("Orientation_w", (GO as Described).Orientation.w.ToString());
                    w.WriteElementString("Orientation_x", (GO as Described).Orientation.x.ToString());
                    w.WriteElementString("Orientation_y", (GO as Described).Orientation.y.ToString());
                    w.WriteElementString("Orientation_z", (GO as Described).Orientation.z.ToString());
                    w.WriteEndElement();
                }
            }

            w.WriteEndElement();
            w.Flush();
            w.Close();
        }

        public void Load()
        {
            while (Engine.Singleton.ObjectManager.Objects.Count > 0)
                Engine.Singleton.ObjectManager.Destroy(Engine.Singleton.ObjectManager.Objects[0]);

            if (System.IO.File.Exists("Media\\Wololo.xml"))
            {
                XmlDocument File = new XmlDocument();
                File.Load("Media\\Wololo.xml");

                XmlElement root = File.DocumentElement;
                XmlNodeList Items = root.SelectNodes("//items/item");

                foreach (XmlNode item in Items)
                {
                    Described newDescribed = new Described(WorldCreator.Items.I[item["DescribedProfile"].InnerText]);
                    Vector3 Position = new Vector3();

                    Quaternion Orientation = new Quaternion(float.Parse(item["Orientation_w"].InnerText), float.Parse(item["Orientation_x"].InnerText), float.Parse(item["Orientation_y"].InnerText), float.Parse(item["Orientation_z"].InnerText));
                    newDescribed.Orientation = Orientation;
                    
                    Position.x = float.Parse(item["Position_x"].InnerText);
                    Position.y = float.Parse(item["Position_y"].InnerText);
                    Position.z = float.Parse(item["Position_z"].InnerText);
                    newDescribed.Position = Position;

                    Engine.Singleton.ObjectManager.Add(newDescribed);
                }
            }
        }

        public bool IsKeyTyped(MOIS.KeyCode code)
        {
            return TypedInput.IsKeyTyped[(int)code];
        }

        public int GetUniqueBodyId()
        {
            return BodyId++;
        }

        static Engine instance;

        Engine()
        {
        }

        static Engine()
        {
            instance = new Engine();
        }

        public static Engine Singleton
        {
            get
            {
                return instance;
            }
        }

        public static double Distance(Vector3 v1, Vector3 v2)
        {
            return
            (
               System.Math.Sqrt
               (
                   (v1.x - v2.x) * (v1.x - v2.x) +
                   (v1.y - v2.y) * (v1.y - v2.y) +
                   (v1.z - v2.z) * (v1.z - v2.z)
               )
            );
        }

        public float GetFloatFromPxHeight(int px)
        {
            float ret;

            ret = ((float)px / (float)Root.AutoCreatedWindow.Height);

            return ret;
        }

        public float GetFloatFromPxWidth(int px)
        {
            float ret;

            ret = ((float)px / (float)Root.AutoCreatedWindow.Width);

            return ret;
        }
    }
}
