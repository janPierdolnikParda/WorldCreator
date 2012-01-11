using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    public class NPCManager
    {
        static public Character npc;
        //static public Enemy enemy0;

        static public Dictionary<String, Character> NPCs;
        //static public Dictionary<String, Enemy> Enemies;

        public NPCManager()
        {
            NPCs = new Dictionary<String, Character>();

            //npc = new Character(CharacterProfileManager.character);
            //npc.Position = new Vector3(-3.9599f, -0.1262f, -5.3875f);
            //npc.Position = new Vector3(7, 1, -18);
            //npc.TalkRoot = Conversations.ConversationRoot;
            //npc.TurnTo(Vector3.ZERO);
            //npc.Quests.Add(Quests.Quest1);
            //npc.IsContainer = true;
            //npc.Inventory.Add(Items.vaseProfile);
            //Engine.Singleton.ObjectManager.Add(npc);

            //NPCs.Add("npc", npc);

            //enemy0 = new Enemy(CharacterProfileManager.character, false, 10, 3);
            //enemy0.Position = new Vector3(7, 1, -20);
            //enemy0.DisplayNameOffset = new Vector3(0, 1, 0);
            //enemy0.Profile.DisplayName = "Enemy";
            //enemy0.TurnTo(Vector3.ZERO);
            //enemy0.Inventory.Add(Items.vaseProfile);
            //enemy0.Statistics = new Statistics(20, 0);
            //Engine.Singleton.ObjectManager.Add(enemy0);  
        }
    }
}
