using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    class HUD
    {
        public enum InventoryCategory
        {
            DESCRIBED,
            CHARACTER,
            ENEMY,
            WAYPOINT,
            LIGHT
        }

        public class Slot
        {
            public const float Size = 0.04f;
            public static float Width;
            public SimpleQuad BgQuad;
            public SimpleQuad BlueQuad;
            public TextLabel ItemLabel;
            public bool isSelected;

            public Slot(float left, float top)
            {
                BgQuad = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", left, top, Width, Size, new ColourValue(1, 1, 1), 1);
                BlueQuad = Engine.Singleton.Labeler.NewSimpleQuad("HighlightBlueMaterial", left, top, Width, Size, new ColourValue(1, 1, 1), 3);
                ItemLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.02f, ColourValue.White, ColourValue.Black, 2);
                ItemLabel.SetPosition(left, top + 0.015f);

                BlueQuad.IsVisible = false;
                isSelected = false;
            }

            public bool IsVisible
            {
                set
                {
                    BgQuad.IsVisible = value;
                    ItemLabel.IsVisible = value;
                    BlueQuad.IsVisible = value;
                }
            }

            public void SetItem(DescribedProfile item)
            {
                if (item != null)
                {
                    BlueQuad.IsVisible = isSelected;
                    ItemLabel.Caption = "  " + item.DisplayName;
                }
                else
                {
                    BlueQuad.IsVisible = false;
                    ItemLabel.Caption = "";
                }
            }

            public void SetCharacter(CharacterProfile character)
            {
                if (character != null)
                {
                    BlueQuad.IsVisible = isSelected;
                    ItemLabel.Caption = "  " + character.DisplayName;
                }
                else
                {
                    BlueQuad.IsVisible = false;
                    ItemLabel.Caption = "";
                }
            }
        }

		public List<DescribedProfile> I = Items.I.Values.ToList<DescribedProfile>();
        public List<CharacterProfile> C = CharacterProfileManager.C.Values.ToList<CharacterProfile>();
        public List<CharacterProfile> E = CharacterProfileManager.E.Values.ToList<CharacterProfile>();

        public const int SlotsCount = 19;
        const float SlotsSpacing = 0.01f;

        public Slot[] Slots;

        public int KtoraStrona;
        public int SelectedOne;

        public SimpleQuad DescriptionBg;
        public SimpleQuad SelectedPicture;
        TextLabel DescriptionLabel;

        public SimpleQuad InventoryBg;
        public SimpleQuad ArrowDown;
        public SimpleQuad ArrowUp;
		public SimpleQuad ArrowLeft;
		public SimpleQuad ArrowRight;

        public SimpleQuad MouseCursor;

        public SimpleQuad ChosenItemBg;
        public SimpleQuad ChosenItemPicture;
        public TextLabel ChosenItemLabel;

        public InventoryCategory Category = InventoryCategory.DESCRIBED;

		public SimpleQuad GravityBg;
		public TextLabel GravityLabel;

		public SimpleQuad CategoryBg;
		public TextLabel CategoryLabel;

        bool _isVisible;

        public HUD()
        {

			_isVisible = false;

            Slot.Width = Slot.Size * 6 / Engine.Singleton.Camera.AspectRatio;
            Slots = new Slot[SlotsCount];
            for (int i = 0; i < SlotsCount; i++)
                Slots[i] = new Slot(SlotsSpacing, SlotsSpacing + i * (Slot.Size + SlotsSpacing));

            DescriptionBg = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.3f, 0.5f, 0.6f, 0.45f, ColourValue.White, 1);
            SelectedPicture = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial",
                0.31f,
                0.58f,
                0.3f / Engine.Singleton.Camera.AspectRatio,
                0.3f, ColourValue.White, 2);
            DescriptionLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.03f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 2);
            DescriptionLabel.SetPosition(0.55f, 0.51f);

            ChosenItemBg = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.0f, 0.0f, 0.2f, 0.05f, ColourValue.White, 1);
            ChosenItemLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.02f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 2);
            ChosenItemLabel.SetPosition(0.05f, 0.0f);
            ChosenItemPicture = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.0f, 0.0f, 0.05f, 0.05f, ColourValue.White, 2);

			GravityBg = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.5f, 0.0f, 0.3f, 0.07f, ColourValue.White, 1);
			GravityLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.05f, ColourValue.Green, ColourValue.Green, 2);
			GravityLabel.SetPosition(0.55f, 0.01f);
			GravityLabel.Caption = "Gravity: ON";


            InventoryBg = Engine.Singleton.Labeler.NewSimpleQuad("InventoryBgMaterial", 0.01f, 0.01f, 0.98f, 0.98f, new ColourValue(1, 1, 1), 0);
            ArrowDown = Engine.Singleton.Labeler.NewSimpleQuad("DownArrow", 0.2f, 0.7f, Engine.Singleton.GetFloatFromPxWidth(64), Engine.Singleton.GetFloatFromPxHeight(128), ColourValue.White, 2);
            ArrowUp = Engine.Singleton.Labeler.NewSimpleQuad("UpArrow", 0.2f, 0.1f, Engine.Singleton.GetFloatFromPxWidth(64), Engine.Singleton.GetFloatFromPxHeight(128), ColourValue.Black, 2);
            MouseCursor = Engine.Singleton.Labeler.NewSimpleQuad("Kursor", 0.0f, 0.0f, Engine.Singleton.GetFloatFromPxWidth(32), Engine.Singleton.GetFloatFromPxHeight(32), new ColourValue(1, 1, 1), 4);

			CategoryBg = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.35f, 0.02f, 0.45f, 0.1f, new ColourValue(1, 1, 1), 1);
			CategoryLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.05f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 2);
			CategoryLabel.SetPosition(0.5f, 0.04f);

            KtoraStrona = 0;
            SelectedOne = -1;

            IsVisible = false;
        }

        public Player User
        {
            get
            {
                return Engine.Singleton.User;
            }
        }

        public void UpdateChosenItem()
        {
            switch (Category)
            {
                case InventoryCategory.DESCRIBED:
                    if (User.InventoryItem != null)
                    {
                        ChosenItemLabel.Caption = User.InventoryItem.DisplayName;

                        if (User.InventoryItem.InventoryPictureMaterial != null && I[SelectedOne].InventoryPictureMaterial != "-")
                            ChosenItemPicture.Panel.MaterialName = User.InventoryItem.InventoryPictureMaterial;
                        else
                            ChosenItemPicture.Panel.MaterialName = "QuadMaterial";
                    }

                    else
                    {
                        ChosenItemLabel.Caption = "";
                        ChosenItemPicture.Panel.MaterialName = "QuadMaterial";
                    }
                    break;

                case InventoryCategory.CHARACTER:
                    if (User.InventoryCharacter != null)
                    {
                        ChosenItemLabel.Caption = User.InventoryCharacter.DisplayName;
                        ChosenItemPicture.Panel.MaterialName = "QuadMaterial";
                    }

                    else
                    {
                        ChosenItemLabel.Caption = "";
                        ChosenItemPicture.Panel.MaterialName = "QuadMaterial";
                    }
                    break;

                case InventoryCategory.ENEMY:
                    if (User.InventoryCharacter != null)
                    {
                        ChosenItemLabel.Caption = User.InventoryCharacter.DisplayName;
                        ChosenItemPicture.Panel.MaterialName = "QuadMaterial";
                    }

                    else
                    {
                        ChosenItemLabel.Caption = "";
                        ChosenItemPicture.Panel.MaterialName = "QuadMaterial";
                    }
                    break;
            }
        }

        public void UnselectAll()
        {
            foreach (Slot S in Slots)
                if (S.isSelected)
                    S.isSelected = false;
        }

		public void UpdateGravityLabel(string what, ColourValue color)
		{
			GravityLabel.SetColor(color, color);
			GravityLabel.Caption = what;
		}

        public void UpdateView()
        {
            for (int i = KtoraStrona * SlotsCount; i < KtoraStrona * SlotsCount + SlotsCount; i++)
                switch (Category)
                {
                    case InventoryCategory.DESCRIBED:
                        if (i < I.Count)
                            Slots[i - KtoraStrona * SlotsCount].SetItem(I.ElementAt(i));
                        else
                            Slots[i - KtoraStrona * SlotsCount].SetItem(null);
						CategoryLabel.Caption = "Items";
                        break;

                    case InventoryCategory.CHARACTER:
                        if (i < C.Count)
                            Slots[i - KtoraStrona * SlotsCount].SetCharacter(C.ElementAt(i));
                        else
                            Slots[i - KtoraStrona * SlotsCount].SetCharacter(null);

						CategoryLabel.Caption = "Characters";
                        break;

                    case InventoryCategory.ENEMY:
                        if (i < E.Count)
                            Slots[i - KtoraStrona * SlotsCount].SetCharacter(E.ElementAt(i));
                        else
                            Slots[i - KtoraStrona * SlotsCount].SetCharacter(null);
                        CategoryLabel.Caption = "Enemies";
                        break;
                }

            MouseCursor.SetDimensions(Engine.Singleton.GetFloatFromPxWidth(User.Mysz.X.abs), Engine.Singleton.GetFloatFromPxHeight(User.Mysz.Y.abs), Engine.Singleton.GetFloatFromPxWidth(32), Engine.Singleton.GetFloatFromPxHeight(32));
        }

        public void UpdateDescription()
        {
            if (Category == InventoryCategory.CHARACTER && Category == InventoryCategory.ENEMY)
            {
                //DescriptionLabel.Caption = C[SelectedOne].DisplayName;

                SelectedPicture.Panel.MaterialName = "QuadMaterial";
            }

            else if (SelectedOne != -1 && SelectedOne < I.Count && Category != InventoryCategory.CHARACTER && Category != InventoryCategory.ENEMY)
            {
                DescriptionLabel.Caption =
                    I[SelectedOne].DisplayName
                    + "\n\n"
                    + I[SelectedOne].Description;

                if (I[SelectedOne] is ItemSword)
                    DescriptionLabel.Caption += "\nObrazenia: "
                        + (I[SelectedOne] as ItemSword).Damage.ToString();

				DescriptionLabel.Caption += "\nMasa: " + I[SelectedOne].Mass;

                if (I[SelectedOne].InventoryPictureMaterial != null && I[SelectedOne].InventoryPictureMaterial != "-")
                    SelectedPicture.Panel.MaterialName = I[SelectedOne].InventoryPictureMaterial;
                else
                    SelectedPicture.Panel.MaterialName = "QuadMaterial";
            }
            else
            {
                DescriptionLabel.Caption = "";
                SelectedPicture.Panel.MaterialName = "QuadMaterial";
            }
        }

        public bool IsVisible
        {
            set
            {
                foreach (var slot in Slots) slot.IsVisible = value;
                DescriptionBg.IsVisible = value;
                DescriptionLabel.IsVisible = value;
                SelectedPicture.IsVisible = value;
                InventoryBg.IsVisible = value;
                ArrowDown.IsVisible = value;
                ArrowUp.IsVisible = value;
                MouseCursor.IsVisible = value;

                ChosenItemBg.IsVisible = !value;
                ChosenItemLabel.IsVisible = !value;
                ChosenItemPicture.IsVisible = !value;

				GravityBg.IsVisible = !value;
				GravityLabel.IsVisible = !value;

				CategoryBg.IsVisible = value;
				CategoryLabel.IsVisible = value;

                if (value)
                {
                    UpdateView();
                }
            }
        }


        public void ToggleVisibility()
        {
            if (_isVisible == true)
                IsVisible = false;
            else
                IsVisible = true;
        }

        public bool IsOver(SimpleQuad quad)
        {
            if (Engine.Singleton.GetFloatFromPxWidth(User.Mysz.X.abs) >= quad.Panel.Left && Engine.Singleton.GetFloatFromPxWidth(User.Mysz.X.abs) <= quad.Panel.Left + quad.Panel.Width && Engine.Singleton.GetFloatFromPxHeight(User.Mysz.Y.abs) >= quad.Panel.Top && Engine.Singleton.GetFloatFromPxHeight(User.Mysz.Y.abs) <= quad.Panel.Top + quad.Panel.Height)
                return true;
            return false;
        }

    }
}
