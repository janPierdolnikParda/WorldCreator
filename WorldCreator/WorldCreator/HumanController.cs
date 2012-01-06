using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    class HumanController
    {
        public enum HumanControllerState
        {
            FREE,
            INVENTORY
        }

        public Player User;
        TextLabel3D TargetLabel;

        HumanControllerState State;

        HUD HUD;

        public HumanController()
        {
            TargetLabel = Engine.Singleton.Labeler.NewTextLabel3D("Primitive", 0.04f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 0);

            HUD = new HUD();
        }

        public void SwitchState(HumanControllerState newState)
        {
            if (State == HumanControllerState.FREE)
            {
                if (newState == HumanControllerState.INVENTORY)
                    HUD.IsVisible = true;
            }

            else if (State == HumanControllerState.INVENTORY)
                if (newState == HumanControllerState.FREE)
                    HUD.IsVisible = false;

            State = newState;
        }
		
        private void HandleInventory()                               // @@ funkcja odpowiedzialna za obsługę ekwipunku
        {
            /*if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_DOWN))      // następny przedmiot z listy
                HUDInventory.SelectIndex++;
            if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_UP))        // poprzedni przedmiot z listy
                HUDInventory.SelectIndex--;*/

            /*if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_LMENU))     // wyrzucenie wybranego przedmiotu
            {
                if (HUDInventory.SelectIndex != -1)
                {
                    if (Character.DropItem(HUDInventory.SelectIndex))
                    {
                        HUDInventory.SelectIndex = HUDInventory.SelectIndex;
                        HUDInventory.UpdateView();
                    }
                }
            }*/

            /*if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_RCONTROL))              // założenie / zdjęcie wybranego przedmiotu
            {
                if (HUDInventory.SelectIndex != -1)
                {
                    if (Character.Inventory[HUDInventory.SelectIndex] is ItemSword)
                    {
                        if (Character.Sword != Character.Inventory[HUDInventory.SelectIndex])
                            Character.Sword = Character.Inventory[HUDInventory.SelectIndex] as ItemSword;
                        else
                            Character.Sword = null;
                        HUDInventory.UpdateItem(HUDInventory.SelectIndex);
                    }
                }
            }*/

            if (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
            {
                int Obieg = 0;
                bool Flag = false;

                foreach (HUD.Slot S in HUD.Slots)
                {
                    if (HUD.IsOver(S.BgQuad))
                    {
                        HUD.SelectedOne = Obieg + HUD.SlotsCount * HUD.KtoraStrona;
                        S.isSelected = true;
                        Flag = true;
                    }
                    else
                        S.isSelected = false;

                    Obieg++;
                }

                if (Flag == false)
                    HUD.SelectedOne = -1;                
            }

            if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_Q))       // opuszczenie ekranu ekwipunku
                SwitchState(HumanControllerState.FREE);
        }
		
        public void Update()
        {
            if (User != null)
            {
                //HUD.IsVisible = false;

                if (State == HumanControllerState.FREE)
                {
                    HandleMovement();
                    HUD.IsVisible = false;
                }

                if (State == HumanControllerState.INVENTORY)
                {
                    HandleInventory();
                    HUD.IsVisible = true;
                }
            }
        }

        private void HandleMovement()                             // @@ funkcja odpowiedzialna za całokształt poruszania się
        {
            Quaternion rotation = new Quaternion();
            rotation.FromAngleAxis(new Degree(2), Vector3.UNIT_Y);

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_Z))         // tzw. skok
                User.Camera.Position = new Vector3(User.Camera.Position.x, User.Camera.Position.y + 1, User.Camera.Position.z);

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_X))          // wypisanie w konsoli aktualnej pozycji postaci
            {
                Console.Write("Pozycja: ");
                Console.Write(User.Camera.Position.x);
                Console.Write(", ");
                Console.Write(User.Camera.Position.y);
                Console.Write(", ");
                Console.WriteLine(User.Camera.Position.z);
            }

            if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_TAB))                    // @@ Otwarcie ekwipunku
                SwitchState(HumanControllerState.INVENTORY);

            User.Camera.TurnY = -Engine.Singleton.Mouse.MouseState.X.rel * 0.1f;
            User.Camera.TurnX = -Engine.Singleton.Mouse.MouseState.Y.rel * 0.1f;

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_A))          // obrót postaci
                User.Camera.Position += User.Camera.Orientation * Vector3.UNIT_X * -User.WalkSpeed;
            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_D))
                User.Camera.Position += User.Camera.Orientation * Vector3.UNIT_X * User.WalkSpeed;

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_W))            // chodzenie do przodu
            {
                User.Camera.Position += User.Camera.Orientation * Vector3.UNIT_Z * -User.WalkSpeed;
            }

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_S))          
            {
                User.Camera.Position += User.Camera.Orientation * Vector3.UNIT_Z * User.WalkSpeed;                                       
            }

            if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_TAB))             // wyświetlanie i chowanie HUD'a
            {
                HUD.ToggleVisibility();
            }

            if (User.FocusedObject != null)
            {                
                SelectableObject focus = User.FocusedObject as SelectableObject;

                TargetLabel.Caption = focus.DisplayName;
                TargetLabel.Position3D = focus.Position + focus.DisplayNameOffset;
                TargetLabel.IsVisible = true;
            }
            else
            {
                TargetLabel.IsVisible = false;
            }
        }

        public void ToggleHud()
        {
            HUD.ToggleVisibility();
        }
    }
}
