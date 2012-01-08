using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    class HUD
    {
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
                ItemLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.02f, new ColourValue(0, 0, 0), new ColourValue(0, 0, 0), 2);
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
        }

		public List<DescribedProfile> I = Items.I.Values.ToList<DescribedProfile>();  // UHUHUHUHAHAHAHA!!!!!!!!!! <<<+==========

        public const int SlotsCount = 19;
        const float SlotsSpacing = 0.01f;

        public Slot[] Slots;

        public int KtoraStrona;
        public int SelectedOne;

        public SimpleQuad DescriptionBg;
        public SimpleQuad SelectedPicture;
        TextLabel DescriptionLabel;

        //SimpleQuad CompassBg;
        //TextLabel CompassLabel;

        public SimpleQuad InventoryBg;
        public SimpleQuad ArrowDown;
        public SimpleQuad ArrowUp;

        public SimpleQuad MouseCursor;

        public SimpleQuad ChosenItemBg;
        public SimpleQuad ChosenItemPicture;
        public TextLabel ChosenItemLabel;

        bool _isVisible;

        public HUD()
        {
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

            //CompassBg = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.1f, 0.1f, 0.2f, 0.1f, new ColourValue(1, 1, 1), 1);
            //CompassLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.05f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 2);
            //CompassLabel.SetPosition(0.11f, 0.13f);

            ChosenItemBg = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.0f, 0.0f, 0.2f, 0.05f, ColourValue.White, 1);
            ChosenItemLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.02f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 2);
            ChosenItemLabel.SetPosition(0.05f, 0.0f);
            ChosenItemPicture = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.0f, 0.0f, 0.05f, 0.05f, ColourValue.White, 2);

            InventoryBg = Engine.Singleton.Labeler.NewSimpleQuad("InventoryBgMaterial", 0.01f, 0.01f, 0.98f, 0.98f, new ColourValue(1, 1, 1), 0);
            ArrowDown = Engine.Singleton.Labeler.NewSimpleQuad("DownArrow", 0.2f, 0.7f, Engine.Singleton.GetFloatFromPxWidth(64), Engine.Singleton.GetFloatFromPxHeight(128), ColourValue.White, 2);
            ArrowUp = Engine.Singleton.Labeler.NewSimpleQuad("UpArrow", 0.2f, 0.1f, Engine.Singleton.GetFloatFromPxWidth(64), Engine.Singleton.GetFloatFromPxHeight(128), ColourValue.Black, 2);
            MouseCursor = Engine.Singleton.Labeler.NewSimpleQuad("Kursor", 0.0f, 0.0f, Engine.Singleton.GetFloatFromPxWidth(32), Engine.Singleton.GetFloatFromPxHeight(32), new ColourValue(1, 1, 1), 4);

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
            if (User.InventoryItem != null)
            {
                ChosenItemLabel.Caption = User.InventoryItem.DisplayName;

                if (User.InventoryItem.InventoryPictureMaterial != null)
                    ChosenItemPicture.Panel.MaterialName = User.InventoryItem.InventoryPictureMaterial;
                else
                    ChosenItemPicture.Panel.MaterialName = "QuadMaterial";
            }

            else
            {
                ChosenItemLabel.Caption = "";
                ChosenItemPicture.Panel.MaterialName = "QuadMaterial";
            }

            ChosenItemLabel.Caption += "\n" + User.AimPosition.ToString();
        }

        public void UpdateView()
        {
            for (int i = KtoraStrona * SlotsCount; i < KtoraStrona * SlotsCount + SlotsCount; i++)
                if (i < Items.I.Count)
                    Slots[i - KtoraStrona * SlotsCount].SetItem(I.ElementAt(i));
                else
                    Slots[i - KtoraStrona * SlotsCount].SetItem(null);

            MouseCursor.SetDimensions(Engine.Singleton.GetFloatFromPxWidth(User.Mysz.X.abs), Engine.Singleton.GetFloatFromPxHeight(User.Mysz.Y.abs), Engine.Singleton.GetFloatFromPxWidth(32), Engine.Singleton.GetFloatFromPxHeight(32));
        }

        public void UpdateDescription()
        {
            if (SelectedOne != -1 && SelectedOne < I.Count)
            {
                DescriptionLabel.Caption =
                    I[SelectedOne].DisplayName
                    + "\n\n"
                    + I[SelectedOne].Description;

                if (I[SelectedOne] is ItemSword)
                    DescriptionLabel.Caption += "\nObrazenia: "
                        + (I[SelectedOne] as ItemSword).Damage.ToString();

                if (I[SelectedOne].InventoryPictureMaterial != null)
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
