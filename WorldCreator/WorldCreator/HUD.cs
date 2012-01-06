using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    class HUD
    {
        class Slot
        {
            public const float Size = 0.04f;
            public static float Width;
            SimpleQuad BgQuad;
            SimpleQuad BlueQuad;
            TextLabel ItemLabel;

            public Slot(float left, float top)
            {
                BgQuad = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", left, top, Width, Size, new ColourValue(1, 1, 1), 1);
                BlueQuad = Engine.Singleton.Labeler.NewSimpleQuad("HighlightBlueMaterial", left, top, Width, Size, new ColourValue(1, 1, 1), 3);
                ItemLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.02f, new ColourValue(0, 0, 0), new ColourValue(0, 0, 0), 2);
                ItemLabel.SetPosition(left, top + 0.015f);

                BlueQuad.IsVisible = false;
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
                    BlueQuad.IsVisible = item.IsEquipment;
                    ItemLabel.Caption = "  " + item.DisplayName;
                }
                else
                {
                    BlueQuad.IsVisible = false;
 
                }
            }
        }

		List<DescribedProfile> I = Items.I.Values.ToList<DescribedProfile>();  // UHUHUHUHAHAHAHA!!!!!!!!!! <<<+==========

        const int SlotsCount = 19;
        const float SlotsSpacing = 0.01f;

        Slot[] Slots;

        int _SelectIndex;
        int _ViewIndex;

        SimpleQuad DescriptionBg;
        SimpleQuad SelectedPicture;
        TextLabel DescriptionLabel;

        //SimpleQuad CompassBg;
        //TextLabel CompassLabel;

        SimpleQuad InventoryBg;

        SimpleQuad MouseCursor;        

        bool _isVisible;

        public HUD()
        {
            Slot.Width = Slot.Size * 6 / Engine.Singleton.Camera.AspectRatio;
            Slots = new Slot[SlotsCount];
            for (int i = 0; i < SlotsCount; i++)
                Slots[i] = new Slot(SlotsSpacing, SlotsSpacing + i * (Slot.Size + SlotsSpacing));

            DescriptionBg = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.2f, 0.5f, 0.6f, 0.45f, ColourValue.White, 1);
            SelectedPicture = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial",
                0.21f,
                0.58f,
                0.3f / Engine.Singleton.Camera.AspectRatio,
                0.3f, ColourValue.White, 2);
            DescriptionLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.03f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 2);
            DescriptionLabel.SetPosition(0.45f, 0.51f);

            //CompassBg = Engine.Singleton.Labeler.NewSimpleQuad("QuadMaterial", 0.1f, 0.1f, 0.2f, 0.1f, new ColourValue(1, 1, 1), 1);
            //CompassLabel = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.05f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 2);
            //CompassLabel.SetPosition(0.11f, 0.13f);

            InventoryBg = Engine.Singleton.Labeler.NewSimpleQuad("InventoryBgMaterial", 0.01f, 0.01f, 0.98f, 0.98f, new ColourValue(1, 1, 1), 0);
            MouseCursor = Engine.Singleton.Labeler.NewSimpleQuad("Kursor", 0, 0, 32, 32, new ColourValue(1, 1, 1), 4);
            //MouseCursor = Engine.Singleton.Labeler.NewTextLabel("Primitive", 0.03f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 0);
            //MouseCursor.SetPosition(0, 0);
            //MouseCursor.Caption = "X";

            IsVisible = false;
        }

        public Player User
        {
            get
            {
                return Engine.Singleton.User;
            }
        }

        public void UpdateView()
        {
            for (int i = ViewIndex; i < ViewIndex + SlotsCount; i++)
                if (i < Items.I.Count)
                    Slots[i - ViewIndex].SetItem(I.ElementAt(i));
                else
                    Slots[i - ViewIndex].SetItem(null);

            //MouseCursor.SetPosition(User.Mysz.X.abs, User.Mysz.Y.abs);
            MouseCursor = Engine.Singleton.Labeler.NewSimpleQuad("Kursor", User.Mysz.X.abs, User.Mysz.Y.abs, 32, 32, new ColourValue(1, 1, 1), 4);
            Console.WriteLine("{0}, {1}" ,User.Mysz.X.abs, User.Mysz.Y.abs);
        }

        public void UpdateDescription()
        {
            if (SelectIndex != -1)
            {
                DescriptionLabel.Caption =
                    I[SelectIndex].DisplayName
                    + "\n\n"
                    + I[SelectIndex].Description;

                if (I[SelectIndex] is ItemSword)
                    DescriptionLabel.Caption += "\nObrazenia: "
                        + (I[SelectIndex] as ItemSword).Damage.ToString();

                if (I[SelectIndex].InventoryPictureMaterial != null)
                    SelectedPicture.Panel.MaterialName = I[SelectIndex].InventoryPictureMaterial;
            }
            else
            {
                DescriptionLabel.Caption = "";
                SelectedPicture.Panel.MaterialName = "QuadMaterial";
            }
        }

        public int SelectIndex
        {
            get
            {
                return (_SelectIndex >= I.Count ? -1 : _SelectIndex);
            }
            set
            {
                if (value < 0)
                    value = -1;
                else if (value >= I.Count)
                    value = I.Count - 1;
                _SelectIndex = value;
                if (_SelectIndex >= ViewIndex + SlotsCount)
                    ViewIndex = _SelectIndex - SlotsCount + 1;
                else if (_SelectIndex < ViewIndex)
                    ViewIndex = _SelectIndex;

                UpdateDescription();
            }
        }

        public int ViewIndex
        {
            get
            {
                return _ViewIndex;
            }
            set
            {
                if (value != _ViewIndex)
                {
                    _ViewIndex = System.Math.Max(0, value);
                    UpdateView();
                }
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
                MouseCursor.IsVisible = value;

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

    }
}
