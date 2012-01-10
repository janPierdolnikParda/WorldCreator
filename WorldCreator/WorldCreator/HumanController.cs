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
            INVENTORY,
            GRAB,
            ROTATE
        }

        public enum GrabOrRotateAxis
        {
            X,
            Y,
            Z
        }

        public Player User;
        TextLabel3D TargetLabel;

        public HumanControllerState State;

        public Vector3 FocusedObjectPos;
		public Quaternion FocusedObjectRot;

        HUD HUD;

        GrabOrRotateAxis ActualAxis;

        Vector3 PositionBefore;
		Quaternion RotationBefore;

        public HumanController()
        {
            TargetLabel = Engine.Singleton.Labeler.NewTextLabel3D("Primitive", 0.04f, new ColourValue(0.7f, 0.4f, 0), new ColourValue(1, 1.0f, 0.6f), 0);

            HUD = new HUD();

            ActualAxis = GrabOrRotateAxis.X;
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
                {
                    HUD.IsVisible = false;


                    if (HUD.SelectedOne > -1)
                        HUD.Slots[HUD.SelectedOne - (HUD.KtoraStrona * HUD.SlotsCount)].isSelected = false;
                    HUD.KtoraStrona = 0;
                    HUD.SelectedOne = -1;
                    HUD.UpdateDescription();
                }

            State = newState;
        }
		
        private void HandleInventory()                               // @@ funkcja odpowiedzialna za obsługę ekwipunku
        {
            if (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
            {
                while (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left)) 
                {
                    Engine.Singleton.Mouse.Capture();               //petla, zeby nie klikal mi milion razy, tylko raz :)
                }

                bool Flag = false;
                

                int Obieg = 0;

                foreach (HUD.Slot S in HUD.Slots)
                {
                    if (HUD.IsOver(S.BgQuad))
                    {
                        if (S.isSelected)
                        {
                            User.InventoryItem = HUD.I[Obieg + (HUD.KtoraStrona * HUD.SlotsCount)];
                            HUD.UpdateChosenItem();
                            SwitchState(HumanControllerState.FREE);
                        }

                        else
                        {
                            HUD.SelectedOne = Obieg + HUD.SlotsCount * HUD.KtoraStrona;
                            S.isSelected = true;
                            Flag = true;
                        }
                    }
                    else
                        S.isSelected = false;

                    Obieg++;
                }

                if (HUD.IsOver(HUD.ArrowDown) && HUD.Slots[0].ItemLabel.Caption != "")
                {
                    HUD.KtoraStrona++;
                    Flag = false;
                }

                if (HUD.IsOver(HUD.ArrowUp) && HUD.KtoraStrona > 0)
                {
                    HUD.KtoraStrona--;
                    Flag = false;
                }

                if (Flag == false)
                    HUD.SelectedOne = -1;

                HUD.UpdateView();
                HUD.UpdateDescription();
            }

            if (Engine.Singleton.Mouse.MouseState.Z.rel > 0 && HUD.KtoraStrona > 0)     //scroll - gora!
            {
                HUD.KtoraStrona--;
                HUD.SelectedOne = -1;
                HUD.UpdateView();
                HUD.UpdateDescription();
            }

            else if (Engine.Singleton.Mouse.MouseState.Z.rel < 0 && HUD.Slots[0].ItemLabel.Caption != "")   //scroll - dol!
            {
                HUD.KtoraStrona++;
                HUD.SelectedOne = -1;
                HUD.UpdateView();
                HUD.UpdateDescription();
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

                if (State == HumanControllerState.GRAB)
                {
                    HUD.IsVisible = false;
                    HandleGrab();
                }

                if (State == HumanControllerState.ROTATE)
                {
                    HUD.IsVisible = false;
                    HandleRotate();
                }
            }
        }

        private void HandleGrab()												// @@ GRAB!!!!!!!__________________________________________
        {
            /*if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_G))
            {
                User.FocusedObject.Position = PositionBefore;
                SwitchState(HumanControllerState.FREE);
            }*/
			User.FocusedObject.Position = FocusedObjectPos;
			User.FocusedObject.Orientation = RotationBefore;
            if (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
            {
                while (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
                {
                    Engine.Singleton.Mouse.Capture();               //petla, zeby nie klikal mi milion razy, tylko raz :)
                }

				
				(User.FocusedObject as Described).Body.MaterialGroupID = Engine.Singleton.MaterialManager.DescribedMaterialID;
				User.FocusedObject.Position = PositionBefore;
               
				SwitchState(HumanControllerState.FREE);
            }

            if (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
            {
                while (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
                {
                    Engine.Singleton.Mouse.Capture();               //petla, zeby nie klikal mi milion razy, tylko raz :)
                }

                (User.FocusedObject as Described).Body.MaterialGroupID = Engine.Singleton.MaterialManager.DescribedMaterialID;

                SwitchState(HumanControllerState.FREE);
            }

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_X))
                ActualAxis = GrabOrRotateAxis.X;

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_Y))
                ActualAxis = GrabOrRotateAxis.Y;

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_Z))
                ActualAxis = GrabOrRotateAxis.Z;

            switch (ActualAxis)
            {
                case GrabOrRotateAxis.X:
                    FocusedObjectPos.x += -Engine.Singleton.Mouse.MouseState.X.rel * 0.01f;
                    break;

                case GrabOrRotateAxis.Y:
                    FocusedObjectPos.y += -Engine.Singleton.Mouse.MouseState.X.rel * 0.01f;
                    break;

                case GrabOrRotateAxis.Z:
                    FocusedObjectPos.z += -Engine.Singleton.Mouse.MouseState.X.rel * 0.01f;
                    break;
            }

           
        }

        private void HandleRotate()
        {
			User.FocusedObject.Orientation = FocusedObjectRot;
			User.FocusedObject.Position = PositionBefore;


			if (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
			{
				while (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
				{
					Engine.Singleton.Mouse.Capture();               //petla, zeby nie klikal mi milion razy, tylko raz :)
				}


				(User.FocusedObject as Described).Body.MaterialGroupID = Engine.Singleton.MaterialManager.DescribedMaterialID;
				

				User.FocusedObject.Orientation = RotationBefore;

				SwitchState(HumanControllerState.FREE);
			}

			if (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
			{
				while (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
				{
					Engine.Singleton.Mouse.Capture();               //petla, zeby nie klikal mi milion razy, tylko raz :)
				}

				(User.FocusedObject as Described).Body.MaterialGroupID = Engine.Singleton.MaterialManager.DescribedMaterialID;

				SwitchState(HumanControllerState.FREE);
			}

			if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_X))
				ActualAxis = GrabOrRotateAxis.X;

			if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_Y))
				ActualAxis = GrabOrRotateAxis.Y;

			if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_Z))
				ActualAxis = GrabOrRotateAxis.Z;

			switch (ActualAxis)
			{
				case GrabOrRotateAxis.X:
					//FocusedObjectPos.x += -Engine.Singleton.Mouse.MouseState.X.rel * 0.01f;
					FocusedObjectRot.x += -Engine.Singleton.Mouse.MouseState.X.rel * 0.01f;
					break;

				case GrabOrRotateAxis.Y:
					FocusedObjectRot.y += -Engine.Singleton.Mouse.MouseState.X.rel * 0.01f;
					break;

				case GrabOrRotateAxis.Z:
					FocusedObjectRot.z += -Engine.Singleton.Mouse.MouseState.X.rel * 0.01f;
					break;
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

			if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_Q))								// chodzenie w górę
				User.Camera.Position += User.Camera.Orientation * Vector3.UNIT_Y * User.WalkSpeed;
			else if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_E))								// chodzenie w dół
				User.Camera.Position += User.Camera.Orientation * Vector3.UNIT_Y * -User.WalkSpeed;


            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_W))            // chodzenie do przodu
            {
                User.Camera.Position += User.Camera.Orientation * Vector3.UNIT_Z * -User.WalkSpeed;
            }

            if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_S))          
            {
                User.Camera.Position += User.Camera.Orientation * Vector3.UNIT_Z * User.WalkSpeed;                                       
            }

            if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_F))      // usuwanie obiektu z Usera
            {
                User.InventoryItem = null;
                HUD.UpdateChosenItem();
            }

            if (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
            {
                while (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
                {
                    Engine.Singleton.Mouse.Capture();               //petla, zeby nie klikal mi milion razy, tylko raz :)
                }

                if (User.InventoryItem != null)
                    User.AddItem(true);
            }

            if (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
            {
                while (Engine.Singleton.Mouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
                {
                    Engine.Singleton.Mouse.Capture();               //petla, zeby nie klikal mi milion razy, tylko raz :)
                }

                if (User.InventoryItem != null)
                    User.AddItem(false);
            }

            if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_C))
            {
                while (Engine.Singleton.ObjectManager.Objects.Count > 0)
                    Engine.Singleton.ObjectManager.Destroy(Engine.Singleton.ObjectManager.Objects[0]);
                User.FocusedObject = null;
            }

            if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_G) && User.FocusedObject != null)
            {

				float x = User.FocusedObject.Position.x;
				float y = User.FocusedObject.Position.y;
				float z = User.FocusedObject.Position.z;

                PositionBefore = new Vector3(x, y, z);


				float xr = User.FocusedObject.Orientation.x;
				float yr = User.FocusedObject.Orientation.y;
				float zr = User.FocusedObject.Orientation.z;

				RotationBefore = User.FocusedObject.Orientation;
				
				(User.FocusedObject as Described).Body.MaterialGroupID = Engine.Singleton.MaterialManager.NoColID;

				
                FocusedObjectPos = User.FocusedObject.Position;
                SwitchState(HumanControllerState.GRAB);
            }

			if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_R) && User.FocusedObject != null)
			{
				float x = User.FocusedObject.Orientation.x;
				float y = User.FocusedObject.Orientation.y;
				float z = User.FocusedObject.Orientation.z;

				//RotationBefore = new Quaternion(new Vector3(x, 0, 0), new Vector3(0, y, 0), new Vector3(0, 0, z));
				RotationBefore = User.FocusedObject.Orientation;

				float xg = User.FocusedObject.Position.x;					//// pozycja musi być stała żeby nie spadało podczas rotejta
				float yg = User.FocusedObject.Position.y;
				float zg = User.FocusedObject.Position.z;

				PositionBefore = new Vector3(xg, yg, zg);

				(User.FocusedObject as Described).Body.MaterialGroupID = Engine.Singleton.MaterialManager.NoColID;

				SwitchState(HumanControllerState.ROTATE);
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
