﻿using System;
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
            //npc.Position = new Vector3(7, 1, -18);

            //npc = new Character(CharacterProfileManager.character);
            //npc.Position = new Vector3(-3.9599f, -0.1262f, -5.3875f);
            //npc.Position = new Vector3(7, 1, -18);

            //npc.TalkRoot = Conversations.ConversationRoot;
            //npc.TurnTo(Vector3.ZERO);
            //npc.Quests.Add(Quests.Quest1);
            //npc.IsContainer = true;
            //npc.Inventory.Add(Items.vaseProfile);
            //Engine.Singleton.ObjectManager.Add(npc);


           // NPCs.Add("npc", npc);

            //NPCs.Add("npc", npc);


 
        }
    }
}
