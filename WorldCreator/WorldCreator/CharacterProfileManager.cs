﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Xml;
using System.IO;

namespace WorldCreator
{
    public class CharacterProfileManager
    {
        static public CharacterProfile character;

        static public Dictionary<String, CharacterProfile> C;
        static public Dictionary<String, CharacterProfile> E;

		public CharacterProfileManager()
		{
			C = new Dictionary<String, CharacterProfile>();
            E = new Dictionary<String, CharacterProfile>();
            if (System.IO.File.Exists("Media\\Profiles\\NPCs.xml"))
            {
                XmlDocument File = new XmlDocument();
                File.Load("Media\\Profiles\\NPCs.xml");

                XmlElement root = File.DocumentElement;
                XmlNodeList Items = root.SelectNodes("//npcs//npc");

                foreach (XmlNode item in Items)
                {

                    CharacterProfile Kriper = new CharacterProfile();
                    Kriper.DisplayName = item["DisplayName"].InnerText;
                    Kriper.MeshName = item["MeshName"].InnerText;
                    Kriper.BodyMass = int.Parse(item["BodyMass"].InnerText);
                    Kriper.WalkSpeed = float.Parse(item["WalkSpeed"].InnerText);
                    Kriper.DisplayNameOffset = Vector3.ZERO;
                    Kriper.DisplayNameOffset.x = float.Parse(item["DisplayNameOffset_x"].InnerText);
                    Kriper.DisplayNameOffset.y = float.Parse(item["DisplayNameOffset_y"].InnerText);
                    Kriper.DisplayNameOffset.z = float.Parse(item["DisplayNameOffset_z"].InnerText);
                    Kriper.HeadOffset = Vector3.ZERO;
                    Kriper.HeadOffset.x = float.Parse(item["HeadOffset_x"].InnerText);
                    Kriper.HeadOffset.y = float.Parse(item["HeadOffset_y"].InnerText);
                    Kriper.HeadOffset.z = float.Parse(item["HeadOffset_z"].InnerText);
                    Kriper.ProfileName = item["ProfileName"].InnerText;
                    //Kriper.FriendlyType = (Character.FriendType)int.Parse(item["FriendlyType"].InnerText);
                    //Kriper.Statistics = new Statistics(int.Parse(item["WalkaWrecz"].InnerText), int.Parse(item["Sila"].InnerText), int.Parse(item["Opanowanie"].InnerText), int.Parse(item["Wytrzymalosc"].InnerText), int.Parse(item["Zrecznosc"].InnerText), int.Parse(item["Charyzma"].InnerText), int.Parse(item["Zywotnosc"].InnerText));
                    //Kriper.DialogRoot = item["DialogRoot"].InnerText;

                    C.Add(Kriper.ProfileName, Kriper);
                }

            }

            if (System.IO.File.Exists("Media\\Profiles\\Enemies.xml"))
            {
                XmlDocument File = new XmlDocument();
                File.Load("Media\\Profiles\\Enemies.xml");

                XmlElement root = File.DocumentElement;
                XmlNodeList Items = root.SelectNodes("//enemies//enemy");

                foreach (XmlNode item in Items)
                {

                    CharacterProfile Kriper = new CharacterProfile();
                    Kriper.DisplayName = item["DisplayName"].InnerText;
                    Kriper.MeshName = item["MeshName"].InnerText;
                    Kriper.BodyMass = int.Parse(item["BodyMass"].InnerText);
                    Kriper.WalkSpeed = float.Parse(item["WalkSpeed"].InnerText);
                    Kriper.DisplayNameOffset = Vector3.ZERO;
                    Kriper.DisplayNameOffset.x = float.Parse(item["DisplayNameOffset_x"].InnerText);
                    Kriper.DisplayNameOffset.y = float.Parse(item["DisplayNameOffset_y"].InnerText);
                    Kriper.DisplayNameOffset.z = float.Parse(item["DisplayNameOffset_z"].InnerText);
                    Kriper.HeadOffset = Vector3.ZERO;
                    Kriper.HeadOffset.x = float.Parse(item["HeadOffset_x"].InnerText);
                    Kriper.HeadOffset.y = float.Parse(item["HeadOffset_y"].InnerText);
                    Kriper.HeadOffset.z = float.Parse(item["HeadOffset_z"].InnerText);
                    Kriper.ProfileName = item["ProfileName"].InnerText;
                    //Kriper.FriendlyType = (Character.FriendType)int.Parse(item["FriendlyType"].InnerText);
                    //Kriper.Statistics = new Statistics(int.Parse(item["WalkaWrecz"].InnerText), int.Parse(item["Sila"].InnerText), int.Parse(item["Opanowanie"].InnerText), int.Parse(item["Wytrzymalosc"].InnerText), int.Parse(item["Zrecznosc"].InnerText), int.Parse(item["Charyzma"].InnerText), int.Parse(item["Zywotnosc"].InnerText));
                    //Kriper.ZasiegOgolny = int.Parse(item["ZasiegOgolny"].InnerText);
                    //Kriper.ZasiegWzroku = int.Parse(item["ZasiegWzroku"].InnerText);

                    E.Add(item["ProfileName"].InnerText, Kriper);
                }

            }
		}
    }
}
