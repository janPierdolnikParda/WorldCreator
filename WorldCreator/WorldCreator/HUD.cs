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
            TextLabel ItemLabel;
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
                }
            }
        }

		List<DescribedProfile> I = Items.I.Values.ToList<DescribedProfile>();  // UHUHUHUHAHAHAHA!!!!!!!!!! <<<+==========

        public const int SlotsCount = 19;
        const float SlotsSpacing = 0.01f;

        public Slot[] Slots;

        int _SelectIndex;
        int _ViewIndex;

        public int KtoraStrona;
        public int SelectedOne;

        public SimpleQuad DescriptionBg;
        public SimpleQuad SelectedPicture;
        TextLabel DescriptionLabel;

        public SimpleQuad InventoryBg;

        public SimpleQuad MouseCursor;        

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

            InventoryBg = Engine.Singleton.Labeler.NewSimpleQuad("InventoryBgMaterial", 0.01f, 0.01f, 0.98f, 0.98f, new ColourValue(1, 1, 1), 0);
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
				else
					SelectedPicture.Panel.MaterialName = "NoImage";
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

        public bool IsOver(SimpleQuad quad)
        {
            if (Engine.Singleton.GetFloatFromPxWidth(User.Mysz.X.abs) >= quad.Panel.Left && Engine.Singleton.GetFloatFromPxWidth(User.Mysz.X.abs) <= quad.Panel.Left + quad.Panel.Width && Engine.Singleton.GetFloatFromPxHeight(User.Mysz.Y.abs) >= quad.Panel.Top && Engine.Singleton.GetFloatFromPxHeight(User.Mysz.Y.abs) <= quad.Panel.Top + quad.Panel.Height)
                return true;
            return false;
        }

    }
}
