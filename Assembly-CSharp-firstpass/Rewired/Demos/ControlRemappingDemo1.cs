using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020000E7 RID: 231
	[AddComponentMenu("")]
	public class ControlRemappingDemo1 : MonoBehaviour
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x00020176 File Offset: 0x0001E376
		private void Awake()
		{
			this.inputMapper.options.timeout = 5f;
			this.inputMapper.options.ignoreMouseXAxis = true;
			this.inputMapper.options.ignoreMouseYAxis = true;
			this.Initialize();
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000201B5 File Offset: 0x0001E3B5
		private void OnEnable()
		{
			this.Subscribe();
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000201BD File Offset: 0x0001E3BD
		private void OnDisable()
		{
			this.Unsubscribe();
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000201C8 File Offset: 0x0001E3C8
		private void Initialize()
		{
			this.dialog = new ControlRemappingDemo1.DialogHelper();
			this.actionQueue = new Queue<ControlRemappingDemo1.QueueEntry>();
			this.selectedController = new ControlRemappingDemo1.ControllerSelection();
			ReInput.ControllerConnectedEvent += this.JoystickConnected;
			ReInput.ControllerPreDisconnectEvent += this.JoystickPreDisconnect;
			ReInput.ControllerDisconnectedEvent += this.JoystickDisconnected;
			this.ResetAll();
			this.initialized = true;
			ReInput.userDataStore.Load();
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00020250 File Offset: 0x0001E450
		private void Setup()
		{
			if (this.setupFinished)
			{
				return;
			}
			this.style_wordWrap = new GUIStyle(GUI.skin.label);
			this.style_wordWrap.wordWrap = true;
			this.style_centeredBox = new GUIStyle(GUI.skin.box);
			this.style_centeredBox.alignment = TextAnchor.MiddleCenter;
			this.setupFinished = true;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000202AF File Offset: 0x0001E4AF
		private void Subscribe()
		{
			this.Unsubscribe();
			this.inputMapper.ConflictFoundEvent += this.OnConflictFound;
			this.inputMapper.StoppedEvent += this.OnStopped;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x000202E5 File Offset: 0x0001E4E5
		private void Unsubscribe()
		{
			this.inputMapper.RemoveAllEventListeners();
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000202F4 File Offset: 0x0001E4F4
		public void OnGUI()
		{
			if (!this.initialized)
			{
				return;
			}
			this.Setup();
			this.HandleMenuControl();
			if (!this.showMenu)
			{
				this.DrawInitialScreen();
				return;
			}
			this.SetGUIStateStart();
			this.ProcessQueue();
			this.DrawPage();
			this.ShowDialog();
			this.SetGUIStateEnd();
			this.busy = false;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0002034C File Offset: 0x0001E54C
		private void HandleMenuControl()
		{
			if (this.dialog.enabled)
			{
				return;
			}
			if (Event.current.type == EventType.Layout && ReInput.players.GetSystemPlayer().GetButtonDown("Menu"))
			{
				if (this.showMenu)
				{
					ReInput.userDataStore.Save();
					this.Close();
					return;
				}
				this.Open();
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000203A9 File Offset: 0x0001E5A9
		private void Close()
		{
			this.ClearWorkingVars();
			this.showMenu = false;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x000203B8 File Offset: 0x0001E5B8
		private void Open()
		{
			this.showMenu = true;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000203C4 File Offset: 0x0001E5C4
		private void DrawInitialScreen()
		{
			ActionElementMap firstElementMapWithAction = ReInput.players.GetSystemPlayer().controllers.maps.GetFirstElementMapWithAction("Menu", true);
			GUIContent guicontent;
			if (firstElementMapWithAction != null)
			{
				guicontent = new GUIContent("Press " + firstElementMapWithAction.elementIdentifierName + " to open the menu.");
			}
			else
			{
				guicontent = new GUIContent("There is no element assigned to open the menu!");
			}
			GUILayout.BeginArea(this.GetScreenCenteredRect(300f, 50f));
			GUILayout.Box(guicontent, this.style_centeredBox, new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true),
				GUILayout.ExpandWidth(true)
			});
			GUILayout.EndArea();
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0002045C File Offset: 0x0001E65C
		private void DrawPage()
		{
			if (GUI.enabled != this.pageGUIState)
			{
				GUI.enabled = this.pageGUIState;
			}
			GUILayout.BeginArea(new Rect(((float)Screen.width - (float)Screen.width * 0.9f) * 0.5f, ((float)Screen.height - (float)Screen.height * 0.9f) * 0.5f, (float)Screen.width * 0.9f, (float)Screen.height * 0.9f));
			this.DrawPlayerSelector();
			this.DrawJoystickSelector();
			this.DrawMouseAssignment();
			this.DrawControllerSelector();
			this.DrawCalibrateButton();
			this.DrawMapCategories();
			this.actionScrollPos = GUILayout.BeginScrollView(this.actionScrollPos, Array.Empty<GUILayoutOption>());
			this.DrawCategoryActions();
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00020520 File Offset: 0x0001E720
		private void DrawPlayerSelector()
		{
			if (ReInput.players.allPlayerCount == 0)
			{
				GUILayout.Label("There are no players.", Array.Empty<GUILayoutOption>());
				return;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Players:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			foreach (Player player in ReInput.players.GetPlayers(true))
			{
				if (this.selectedPlayer == null)
				{
					this.selectedPlayer = player;
				}
				bool flag = player == this.selectedPlayer;
				bool flag2 = GUILayout.Toggle(flag, (player.descriptiveName != string.Empty) ? player.descriptiveName : player.name, "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
				if (flag2 != flag && flag2)
				{
					this.selectedPlayer = player;
					this.selectedController.Clear();
					this.selectedMapCategoryId = -1;
				}
			}
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00020634 File Offset: 0x0001E834
		private void DrawMouseAssignment()
		{
			bool enabled = GUI.enabled;
			if (this.selectedPlayer == null)
			{
				GUI.enabled = false;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Assign Mouse:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag = this.selectedPlayer != null && this.selectedPlayer.controllers.hasMouse;
			bool flag2 = GUILayout.Toggle(flag, "Assign Mouse", "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
			if (flag2 != flag)
			{
				if (flag2)
				{
					this.selectedPlayer.controllers.hasMouse = true;
					using (IEnumerator<Player> enumerator = ReInput.players.Players.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Player player = enumerator.Current;
							if (player != this.selectedPlayer)
							{
								player.controllers.hasMouse = false;
							}
						}
						goto IL_00E9;
					}
				}
				this.selectedPlayer.controllers.hasMouse = false;
			}
			IL_00E9:
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00020750 File Offset: 0x0001E950
		private void DrawJoystickSelector()
		{
			bool enabled = GUI.enabled;
			if (this.selectedPlayer == null)
			{
				GUI.enabled = false;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Assign Joysticks:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag = this.selectedPlayer == null || this.selectedPlayer.controllers.joystickCount == 0;
			bool flag2 = GUILayout.Toggle(flag, "None", "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
			if (flag2 != flag)
			{
				this.selectedPlayer.controllers.ClearControllersOfType(ControllerType.Joystick);
				this.ControllerSelectionChanged();
			}
			if (this.selectedPlayer != null)
			{
				foreach (Joystick joystick in ReInput.controllers.Joysticks)
				{
					flag = this.selectedPlayer.controllers.ContainsController(joystick);
					flag2 = GUILayout.Toggle(flag, joystick.name, "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
					if (flag2 != flag)
					{
						this.EnqueueAction(new ControlRemappingDemo1.JoystickAssignmentChange(this.selectedPlayer.id, joystick.id, flag2));
					}
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x000208AC File Offset: 0x0001EAAC
		private void DrawControllerSelector()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(15f);
			GUILayout.Label("Controller to Map:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (!this.selectedController.hasSelection)
			{
				this.selectedController.Set(0, ControllerType.Keyboard);
				this.ControllerSelectionChanged();
			}
			bool flag = this.selectedController.type == ControllerType.Keyboard;
			if (GUILayout.Toggle(flag, ReInput.controllers.Keyboard.name, "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }) != flag)
			{
				this.selectedController.Set(0, ControllerType.Keyboard);
				this.ControllerSelectionChanged();
			}
			if (!this.selectedPlayer.controllers.hasMouse)
			{
				GUI.enabled = false;
			}
			flag = this.selectedController.type == ControllerType.Mouse;
			if (GUILayout.Toggle(flag, ReInput.controllers.Mouse.name, "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }) != flag)
			{
				this.selectedController.Set(0, ControllerType.Mouse);
				this.ControllerSelectionChanged();
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
			foreach (Joystick joystick in this.selectedPlayer.controllers.Joysticks)
			{
				flag = this.selectedController.type == ControllerType.Joystick && this.selectedController.id == joystick.id;
				if (GUILayout.Toggle(flag, joystick.name, "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }) != flag)
				{
					this.selectedController.Set(joystick.id, ControllerType.Joystick);
					this.ControllerSelectionChanged();
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00020A94 File Offset: 0x0001EC94
		private void DrawCalibrateButton()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(10f);
			Controller controller = (this.selectedController.hasSelection ? this.selectedPlayer.controllers.GetController(this.selectedController.type, this.selectedController.id) : null);
			if (controller == null || this.selectedController.type != ControllerType.Joystick)
			{
				GUI.enabled = false;
				GUILayout.Button("Select a controller to calibrate", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			else if (GUILayout.Button("Calibrate " + controller.name, new GUILayoutOption[] { GUILayout.ExpandWidth(false) }))
			{
				Joystick joystick = controller as Joystick;
				if (joystick != null)
				{
					CalibrationMap calibrationMap = joystick.calibrationMap;
					if (calibrationMap != null)
					{
						this.EnqueueAction(new ControlRemappingDemo1.Calibration(this.selectedPlayer, joystick, calibrationMap));
					}
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00020B90 File Offset: 0x0001ED90
		private void DrawMapCategories()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			if (!this.selectedController.hasSelection)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(15f);
			GUILayout.Label("Categories:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			foreach (InputMapCategory inputMapCategory in ReInput.mapping.UserAssignableMapCategories)
			{
				if (!this.selectedPlayer.controllers.maps.ContainsMapInCategory(this.selectedController.type, inputMapCategory.id))
				{
					GUI.enabled = false;
				}
				else if (this.selectedMapCategoryId < 0)
				{
					this.selectedMapCategoryId = inputMapCategory.id;
					this.selectedMap = this.selectedPlayer.controllers.maps.GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, inputMapCategory.id);
				}
				bool flag = inputMapCategory.id == this.selectedMapCategoryId;
				if (GUILayout.Toggle(flag, (inputMapCategory.descriptiveName != string.Empty) ? inputMapCategory.descriptiveName : inputMapCategory.name, "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }) != flag)
				{
					this.selectedMapCategoryId = inputMapCategory.id;
					this.selectedMap = this.selectedPlayer.controllers.maps.GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, inputMapCategory.id);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00020D64 File Offset: 0x0001EF64
		private void DrawCategoryActions()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			if (this.selectedMapCategoryId < 0)
			{
				return;
			}
			bool enabled = GUI.enabled;
			if (this.selectedMap == null)
			{
				return;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Actions:", Array.Empty<GUILayoutOption>());
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(this.selectedMapCategoryId);
			if (mapCategory == null)
			{
				return;
			}
			InputCategory actionCategory = ReInput.mapping.GetActionCategory(mapCategory.name);
			if (actionCategory == null)
			{
				return;
			}
			float num = 150f;
			foreach (InputAction inputAction in ReInput.mapping.ActionsInCategory(actionCategory.id))
			{
				string text = ((inputAction.descriptiveName != string.Empty) ? inputAction.descriptiveName : inputAction.name);
				if (inputAction.type == InputActionType.Button)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text, new GUILayoutOption[] { GUILayout.Width(num) });
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap in this.selectedMap.AllMaps)
					{
						if (actionElementMap.actionId == inputAction.id)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap, actionElementMap);
						}
					}
					GUILayout.EndHorizontal();
				}
				else if (inputAction.type == InputActionType.Axis)
				{
					if (this.selectedController.type != ControllerType.Keyboard)
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label(text, new GUILayoutOption[] { GUILayout.Width(num) });
						this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Full, this.selectedController, this.selectedMap);
						foreach (ActionElementMap actionElementMap2 in this.selectedMap.AllMaps)
						{
							if (actionElementMap2.actionId == inputAction.id && actionElementMap2.elementType != ControllerElementType.Button && actionElementMap2.axisType != AxisType.Split)
							{
								this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Full, this.selectedController, this.selectedMap, actionElementMap2);
								this.DrawInvertButton(this.selectedPlayer.id, inputAction, Pole.Positive, this.selectedController, this.selectedMap, actionElementMap2);
							}
						}
						GUILayout.EndHorizontal();
					}
					string text2 = ((inputAction.positiveDescriptiveName != string.Empty) ? inputAction.positiveDescriptiveName : (inputAction.descriptiveName + " +"));
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text2, new GUILayoutOption[] { GUILayout.Width(num) });
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap3 in this.selectedMap.AllMaps)
					{
						if (actionElementMap3.actionId == inputAction.id && actionElementMap3.axisContribution == Pole.Positive && actionElementMap3.axisType != AxisType.Normal)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap, actionElementMap3);
						}
					}
					GUILayout.EndHorizontal();
					string text3 = ((inputAction.negativeDescriptiveName != string.Empty) ? inputAction.negativeDescriptiveName : (inputAction.descriptiveName + " -"));
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text3, new GUILayoutOption[] { GUILayout.Width(num) });
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Negative, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap4 in this.selectedMap.AllMaps)
					{
						if (actionElementMap4.actionId == inputAction.id && actionElementMap4.axisContribution == Pole.Negative && actionElementMap4.axisType != AxisType.Normal)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Negative, this.selectedController, this.selectedMap, actionElementMap4);
						}
					}
					GUILayout.EndHorizontal();
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00021258 File Offset: 0x0001F458
		private void DrawActionAssignmentButton(int playerId, InputAction action, AxisRange actionRange, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap, ActionElementMap elementMap)
		{
			if (GUILayout.Button(elementMap.elementIdentifierName, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false),
				GUILayout.MinWidth(30f)
			}))
			{
				InputMapper.Context context = new InputMapper.Context
				{
					actionId = action.id,
					actionRange = actionRange,
					controllerMap = controllerMap,
					actionElementMapToReplace = elementMap
				};
				this.EnqueueAction(new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove, context));
				this.startListening = true;
			}
			GUILayout.Space(4f);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x000212D8 File Offset: 0x0001F4D8
		private void DrawInvertButton(int playerId, InputAction action, Pole actionAxisContribution, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap, ActionElementMap elementMap)
		{
			bool invert = elementMap.invert;
			bool flag = GUILayout.Toggle(invert, "Invert", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
			if (flag != invert)
			{
				elementMap.invert = flag;
			}
			GUILayout.Space(10f);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00021320 File Offset: 0x0001F520
		private void DrawAddActionMapButton(int playerId, InputAction action, AxisRange actionRange, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap)
		{
			if (GUILayout.Button("Add...", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }))
			{
				InputMapper.Context context = new InputMapper.Context
				{
					actionId = action.id,
					actionRange = actionRange,
					controllerMap = controllerMap
				};
				this.EnqueueAction(new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.Add, context));
				this.startListening = true;
			}
			GUILayout.Space(10f);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00021387 File Offset: 0x0001F587
		private void ShowDialog()
		{
			this.dialog.Update();
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00021394 File Offset: 0x0001F594
		private void DrawModalWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Okay");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00021400 File Offset: 0x0001F600
		private void DrawModalWindow_OkayOnly(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Okay");
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0002145C File Offset: 0x0001F65C
		private void DrawElementAssignmentWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = this.actionQueue.Peek() as ControlRemappingDemo1.ElementAssignmentChange;
			if (elementAssignmentChange == null)
			{
				this.dialog.Cancel();
				return;
			}
			float num;
			if (!this.dialog.busy)
			{
				if (this.startListening && this.inputMapper.status == InputMapper.Status.Idle)
				{
					this.inputMapper.Start(elementAssignmentChange.context);
					this.startListening = false;
				}
				if (this.conflictFoundEventData != null)
				{
					this.dialog.Confirm();
					return;
				}
				num = this.inputMapper.timeRemaining;
				if (num == 0f)
				{
					this.dialog.Cancel();
					return;
				}
			}
			else
			{
				num = this.inputMapper.options.timeout;
			}
			GUILayout.Label("Assignment will be canceled in " + ((int)Mathf.Ceil(num)).ToString() + "...", this.style_wordWrap, Array.Empty<GUILayoutOption>());
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00021568 File Offset: 0x0001F768
		private void DrawElementAssignmentProtectedConflictWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x000215F4 File Offset: 0x0001F7F4
		private void DrawElementAssignmentNormalConflictWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Confirm, "Replace");
			GUILayout.FlexibleSpace();
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00021694 File Offset: 0x0001F894
		private void DrawReassignOrRemoveElementAssignmentWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Reassign");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton("Remove");
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00021704 File Offset: 0x0001F904
		private void DrawFallbackJoystickIdentificationWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			ControlRemappingDemo1.FallbackJoystickIdentification fallbackJoystickIdentification = this.actionQueue.Peek() as ControlRemappingDemo1.FallbackJoystickIdentification;
			if (fallbackJoystickIdentification == null)
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Press any button or axis on \"" + fallbackJoystickIdentification.joystickName + "\" now.", this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Skip", Array.Empty<GUILayoutOption>()))
			{
				this.dialog.Cancel();
				return;
			}
			if (this.dialog.busy)
			{
				return;
			}
			if (!ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(fallbackJoystickIdentification.joystickId, 0.8f, false))
			{
				return;
			}
			this.dialog.Confirm();
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000217D4 File Offset: 0x0001F9D4
		private void DrawCalibrationWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			ControlRemappingDemo1.Calibration calibration = this.actionQueue.Peek() as ControlRemappingDemo1.Calibration;
			if (calibration == null)
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool enabled = GUI.enabled;
			GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(200f) });
			this.calibrateScrollPos = GUILayout.BeginScrollView(this.calibrateScrollPos, Array.Empty<GUILayoutOption>());
			if (calibration.recording)
			{
				GUI.enabled = false;
			}
			IList<ControllerElementIdentifier> axisElementIdentifiers = calibration.joystick.AxisElementIdentifiers;
			for (int i = 0; i < axisElementIdentifiers.Count; i++)
			{
				ControllerElementIdentifier controllerElementIdentifier = axisElementIdentifiers[i];
				bool flag = calibration.selectedElementIdentifierId == controllerElementIdentifier.id;
				bool flag2 = GUILayout.Toggle(flag, controllerElementIdentifier.name, "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
				if (flag != flag2)
				{
					calibration.selectedElementIdentifierId = controllerElementIdentifier.id;
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(200f) });
			if (calibration.selectedElementIdentifierId >= 0)
			{
				float axisRawById = calibration.joystick.GetAxisRawById(calibration.selectedElementIdentifierId);
				GUILayout.Label("Raw Value: " + axisRawById.ToString(), Array.Empty<GUILayoutOption>());
				int axisIndexById = calibration.joystick.GetAxisIndexById(calibration.selectedElementIdentifierId);
				AxisCalibration axis = calibration.calibrationMap.GetAxis(axisIndexById);
				GUILayout.Label("Calibrated Value: " + calibration.joystick.GetAxisById(calibration.selectedElementIdentifierId).ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Zero: " + axis.calibratedZero.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Min: " + axis.calibratedMin.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max: " + axis.calibratedMax.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Dead Zone: " + axis.deadZone.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Space(15f);
				bool flag3 = GUILayout.Toggle(axis.enabled, "Enabled", "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
				if (axis.enabled != flag3)
				{
					axis.enabled = flag3;
				}
				GUILayout.Space(10f);
				bool flag4 = GUILayout.Toggle(calibration.recording, "Record Min/Max", "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
				if (flag4 != calibration.recording)
				{
					if (flag4)
					{
						axis.calibratedMax = 0f;
						axis.calibratedMin = 0f;
					}
					calibration.recording = flag4;
				}
				if (calibration.recording)
				{
					axis.calibratedMin = Mathf.Min(new float[] { axis.calibratedMin, axisRawById, axis.calibratedMin });
					axis.calibratedMax = Mathf.Max(new float[] { axis.calibratedMax, axisRawById, axis.calibratedMax });
					GUI.enabled = false;
				}
				if (GUILayout.Button("Set Zero", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }))
				{
					axis.calibratedZero = axisRawById;
				}
				if (GUILayout.Button("Set Dead Zone", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }))
				{
					axis.deadZone = axisRawById;
				}
				bool flag5 = GUILayout.Toggle(axis.invert, "Invert", "Button", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
				if (axis.invert != flag5)
				{
					axis.invert = flag5;
				}
				GUILayout.Space(10f);
				if (GUILayout.Button("Reset", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }))
				{
					axis.Reset();
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			else
			{
				GUILayout.Label("Select an axis to begin.", Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			if (calibration.recording)
			{
				GUI.enabled = false;
			}
			if (GUILayout.Button("Close", Array.Empty<GUILayoutOption>()))
			{
				this.calibrateScrollPos = default(Vector2);
				this.dialog.Confirm();
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00021C74 File Offset: 0x0001FE74
		private void DialogResultCallback(int queueActionId, ControlRemappingDemo1.UserResponse response)
		{
			foreach (ControlRemappingDemo1.QueueEntry queueEntry in this.actionQueue)
			{
				if (queueEntry.id == queueActionId)
				{
					if (response != ControlRemappingDemo1.UserResponse.Cancel)
					{
						queueEntry.Confirm(response);
						break;
					}
					queueEntry.Cancel();
					break;
				}
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00021CE0 File Offset: 0x0001FEE0
		private Rect GetScreenCenteredRect(float width, float height)
		{
			return new Rect((float)Screen.width * 0.5f - width * 0.5f, (float)((double)Screen.height * 0.5 - (double)(height * 0.5f)), width, height);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00021D18 File Offset: 0x0001FF18
		private void EnqueueAction(ControlRemappingDemo1.QueueEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			this.busy = true;
			GUI.enabled = false;
			this.actionQueue.Enqueue(entry);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00021D38 File Offset: 0x0001FF38
		private void ProcessQueue()
		{
			if (this.dialog.enabled)
			{
				return;
			}
			if (this.busy || this.actionQueue.Count == 0)
			{
				return;
			}
			while (this.actionQueue.Count > 0)
			{
				ControlRemappingDemo1.QueueEntry queueEntry = this.actionQueue.Peek();
				bool flag = false;
				switch (queueEntry.queueActionType)
				{
				case ControlRemappingDemo1.QueueActionType.JoystickAssignment:
					flag = this.ProcessJoystickAssignmentChange((ControlRemappingDemo1.JoystickAssignmentChange)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.ElementAssignment:
					flag = this.ProcessElementAssignmentChange((ControlRemappingDemo1.ElementAssignmentChange)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification:
					flag = this.ProcessFallbackJoystickIdentification((ControlRemappingDemo1.FallbackJoystickIdentification)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.Calibrate:
					flag = this.ProcessCalibration((ControlRemappingDemo1.Calibration)queueEntry);
					break;
				}
				if (!flag)
				{
					break;
				}
				this.actionQueue.Dequeue();
			}
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00021DF4 File Offset: 0x0001FFF4
		private bool ProcessJoystickAssignmentChange(ControlRemappingDemo1.JoystickAssignmentChange entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			Player player = ReInput.players.GetPlayer(entry.playerId);
			if (player == null)
			{
				return true;
			}
			if (!entry.assign)
			{
				player.controllers.RemoveController(ControllerType.Joystick, entry.joystickId);
				this.ControllerSelectionChanged();
				return true;
			}
			if (player.controllers.ContainsController(ControllerType.Joystick, entry.joystickId))
			{
				return true;
			}
			if (!ReInput.controllers.IsJoystickAssigned(entry.joystickId) || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				player.controllers.AddController(ControllerType.Joystick, entry.joystickId, true);
				this.ControllerSelectionChanged();
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Joystick Reassignment",
				message = "This joystick is already assigned to another player. Do you want to reassign this joystick to " + player.descriptiveName + "?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00021F0C File Offset: 0x0002010C
		private bool ProcessElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			switch (entry.changeType)
			{
			case ControlRemappingDemo1.ElementAssignmentChangeType.Add:
			case ControlRemappingDemo1.ElementAssignmentChangeType.Replace:
				return this.ProcessAddOrReplaceElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.Remove:
				return this.ProcessRemoveElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove:
				return this.ProcessRemoveOrReassignElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck:
				return this.ProcessElementAssignmentConflictCheck(entry);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00021F64 File Offset: 0x00020164
		private bool ProcessRemoveOrReassignElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Remove;
				this.actionQueue.Enqueue(elementAssignmentChange);
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange2 = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange2.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Replace;
				this.actionQueue.Enqueue(elementAssignmentChange2);
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
			{
				title = "Reassign or Remove",
				message = "Do you want to reassign or remove this assignment?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawReassignOrRemoveElementAssignmentWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00022038 File Offset: 0x00020238
		private bool ProcessRemoveElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				entry.context.controllerMap.DeleteElementMap(entry.context.actionElementMapToReplace.id);
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.DeleteAssignmentConfirmation, new ControlRemappingDemo1.WindowProperties
			{
				title = "Remove Assignment",
				message = "Are you sure you want to remove this assignment?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x000220F8 File Offset: 0x000202F8
		private bool ProcessAddOrReplaceElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				this.inputMapper.Stop();
				return true;
			}
			if (entry.state != ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				string text;
				if (entry.context.controllerMap.controllerType == ControllerType.Keyboard)
				{
					if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
					{
						text = "Press any key to assign it to this action. You may also use the modifier keys Command, Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
					}
					else
					{
						text = "Press any key to assign it to this action. You may also use the modifier keys Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
					}
					if (Application.isEditor)
					{
						text += "\n\nNOTE: Some modifier key combinations will not work in the Unity Editor, but they will work in a game build.";
					}
				}
				else if (entry.context.controllerMap.controllerType == ControllerType.Mouse)
				{
					text = "Press any mouse button or axis to assign it to this action.\n\nTo assign mouse movement axes, move the mouse quickly in the direction you want mapped to the action. Slow movements will be ignored.";
				}
				else
				{
					text = "Press any button or axis to assign it to this action.";
				}
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assign",
					message = text,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
				return false;
			}
			if (Event.current.type != EventType.Layout)
			{
				return false;
			}
			if (this.conflictFoundEventData != null)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck;
				this.actionQueue.Enqueue(elementAssignmentChange);
			}
			return true;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00022228 File Offset: 0x00020428
		private bool ProcessElementAssignmentConflictCheck(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				this.inputMapper.Stop();
				return true;
			}
			if (this.conflictFoundEventData == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				if (entry.response == ControlRemappingDemo1.UserResponse.Confirm)
				{
					this.conflictFoundEventData.responseCallback(InputMapper.ConflictResponse.Replace);
				}
				else
				{
					if (entry.response != ControlRemappingDemo1.UserResponse.Custom1)
					{
						throw new NotImplementedException();
					}
					this.conflictFoundEventData.responseCallback(InputMapper.ConflictResponse.Add);
				}
				return true;
			}
			if (this.conflictFoundEventData.isProtected)
			{
				string text = this.conflictFoundEventData.assignment.elementDisplayName + " is already in use and is protected from reassignment. You cannot remove the protected assignment, but you can still assign the action to this element. If you do so, the element will trigger multiple actions when activated.";
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assignment Conflict",
					message = text,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentProtectedConflictWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			}
			else
			{
				string text2 = this.conflictFoundEventData.assignment.elementDisplayName + " is already in use. You may replace the other conflicting assignments, add this assignment anyway which will leave multiple actions assigned to this element, or cancel this assignment.";
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assignment Conflict",
					message = text2,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentNormalConflictWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			}
			return false;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000223C4 File Offset: 0x000205C4
		private bool ProcessFallbackJoystickIdentification(ControlRemappingDemo1.FallbackJoystickIdentification entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Joystick Identification Required",
				message = "A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawFallbackJoystickIdentificationWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback), 1f);
			return false;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00022458 File Offset: 0x00020658
		private bool ProcessCalibration(ControlRemappingDemo1.Calibration entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Calibrate Controller",
				message = "Select an axis to calibrate on the " + entry.joystick.name + ".",
				rect = this.GetScreenCenteredRect(450f, 480f),
				windowDrawDelegate = new Action<string, string>(this.DrawCalibrationWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000224FA File Offset: 0x000206FA
		private void PlayerSelectionChanged()
		{
			this.ClearControllerSelection();
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00022502 File Offset: 0x00020702
		private void ControllerSelectionChanged()
		{
			this.ClearMapSelection();
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0002250A File Offset: 0x0002070A
		private void ClearControllerSelection()
		{
			this.selectedController.Clear();
			this.ClearMapSelection();
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002251D File Offset: 0x0002071D
		private void ClearMapSelection()
		{
			this.selectedMapCategoryId = -1;
			this.selectedMap = null;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0002252D File Offset: 0x0002072D
		private void ResetAll()
		{
			this.ClearWorkingVars();
			this.initialized = false;
			this.showMenu = false;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00022544 File Offset: 0x00020744
		private void ClearWorkingVars()
		{
			this.selectedPlayer = null;
			this.ClearMapSelection();
			this.selectedController.Clear();
			this.actionScrollPos = default(Vector2);
			this.dialog.FullReset();
			this.actionQueue.Clear();
			this.busy = false;
			this.startListening = false;
			this.conflictFoundEventData = null;
			this.inputMapper.Stop();
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000225AC File Offset: 0x000207AC
		private void SetGUIStateStart()
		{
			this.guiState = true;
			if (this.busy)
			{
				this.guiState = false;
			}
			this.pageGUIState = this.guiState && !this.busy && !this.dialog.enabled && !this.dialog.busy;
			if (GUI.enabled != this.guiState)
			{
				GUI.enabled = this.guiState;
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002261B File Offset: 0x0002081B
		private void SetGUIStateEnd()
		{
			this.guiState = true;
			if (!GUI.enabled)
			{
				GUI.enabled = this.guiState;
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00022638 File Offset: 0x00020838
		private void JoystickConnected(ControllerStatusChangedEventArgs args)
		{
			if (ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId))
			{
				using (IEnumerator<Player> enumerator = ReInput.players.AllPlayers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Player player = enumerator.Current;
						if (player.controllers.ContainsController(args.controllerType, args.controllerId))
						{
							ReInput.userDataStore.LoadControllerData(player.id, args.controllerType, args.controllerId);
						}
					}
					goto IL_0090;
				}
			}
			ReInput.userDataStore.LoadControllerData(args.controllerType, args.controllerId);
			IL_0090:
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000226F4 File Offset: 0x000208F4
		private void JoystickPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (this.selectedController.hasSelection && args.controllerType == this.selectedController.type && args.controllerId == this.selectedController.id)
			{
				this.ClearControllerSelection();
			}
			if (this.showMenu)
			{
				if (ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId))
				{
					using (IEnumerator<Player> enumerator = ReInput.players.AllPlayers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Player player = enumerator.Current;
							if (player.controllers.ContainsController(args.controllerType, args.controllerId))
							{
								ReInput.userDataStore.SaveControllerData(player.id, args.controllerType, args.controllerId);
							}
						}
						return;
					}
				}
				ReInput.userDataStore.SaveControllerData(args.controllerType, args.controllerId);
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x000227E8 File Offset: 0x000209E8
		private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (this.showMenu)
			{
				this.ClearWorkingVars();
			}
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00022805 File Offset: 0x00020A05
		private void OnConflictFound(InputMapper.ConflictFoundEventData data)
		{
			this.conflictFoundEventData = data;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0002280E File Offset: 0x00020A0E
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.conflictFoundEventData = null;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00022818 File Offset: 0x00020A18
		public void IdentifyAllJoysticks()
		{
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			this.ClearWorkingVars();
			this.Open();
			foreach (Joystick joystick in ReInput.controllers.Joysticks)
			{
				this.actionQueue.Enqueue(new ControlRemappingDemo1.FallbackJoystickIdentification(joystick.id, joystick.name));
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00003466 File Offset: 0x00001666
		protected void CheckRecompile()
		{
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00003466 File Offset: 0x00001666
		private void RecompileWindow(int windowId)
		{
		}

		// Token: 0x040005D7 RID: 1495
		private const float defaultModalWidth = 250f;

		// Token: 0x040005D8 RID: 1496
		private const float defaultModalHeight = 200f;

		// Token: 0x040005D9 RID: 1497
		private const float assignmentTimeout = 5f;

		// Token: 0x040005DA RID: 1498
		private ControlRemappingDemo1.DialogHelper dialog;

		// Token: 0x040005DB RID: 1499
		private InputMapper inputMapper = new InputMapper();

		// Token: 0x040005DC RID: 1500
		private InputMapper.ConflictFoundEventData conflictFoundEventData;

		// Token: 0x040005DD RID: 1501
		private bool guiState;

		// Token: 0x040005DE RID: 1502
		private bool busy;

		// Token: 0x040005DF RID: 1503
		private bool pageGUIState;

		// Token: 0x040005E0 RID: 1504
		private Player selectedPlayer;

		// Token: 0x040005E1 RID: 1505
		private int selectedMapCategoryId;

		// Token: 0x040005E2 RID: 1506
		private ControlRemappingDemo1.ControllerSelection selectedController;

		// Token: 0x040005E3 RID: 1507
		private ControllerMap selectedMap;

		// Token: 0x040005E4 RID: 1508
		private bool showMenu;

		// Token: 0x040005E5 RID: 1509
		private bool startListening;

		// Token: 0x040005E6 RID: 1510
		private Vector2 actionScrollPos;

		// Token: 0x040005E7 RID: 1511
		private Vector2 calibrateScrollPos;

		// Token: 0x040005E8 RID: 1512
		private Queue<ControlRemappingDemo1.QueueEntry> actionQueue;

		// Token: 0x040005E9 RID: 1513
		private bool setupFinished;

		// Token: 0x040005EA RID: 1514
		[NonSerialized]
		private bool initialized;

		// Token: 0x040005EB RID: 1515
		private bool isCompiling;

		// Token: 0x040005EC RID: 1516
		private GUIStyle style_wordWrap;

		// Token: 0x040005ED RID: 1517
		private GUIStyle style_centeredBox;

		// Token: 0x020000E8 RID: 232
		private class ControllerSelection
		{
			// Token: 0x06000C06 RID: 3078 RVA: 0x000228AB File Offset: 0x00020AAB
			public ControllerSelection()
			{
				this.Clear();
			}

			// Token: 0x17000496 RID: 1174
			// (get) Token: 0x06000C07 RID: 3079 RVA: 0x000228B9 File Offset: 0x00020AB9
			// (set) Token: 0x06000C08 RID: 3080 RVA: 0x000228C1 File Offset: 0x00020AC1
			public int id
			{
				get
				{
					return this._id;
				}
				set
				{
					this._idPrev = this._id;
					this._id = value;
				}
			}

			// Token: 0x17000497 RID: 1175
			// (get) Token: 0x06000C09 RID: 3081 RVA: 0x000228D6 File Offset: 0x00020AD6
			// (set) Token: 0x06000C0A RID: 3082 RVA: 0x000228DE File Offset: 0x00020ADE
			public ControllerType type
			{
				get
				{
					return this._type;
				}
				set
				{
					this._typePrev = this._type;
					this._type = value;
				}
			}

			// Token: 0x17000498 RID: 1176
			// (get) Token: 0x06000C0B RID: 3083 RVA: 0x000228F3 File Offset: 0x00020AF3
			public int idPrev
			{
				get
				{
					return this._idPrev;
				}
			}

			// Token: 0x17000499 RID: 1177
			// (get) Token: 0x06000C0C RID: 3084 RVA: 0x000228FB File Offset: 0x00020AFB
			public ControllerType typePrev
			{
				get
				{
					return this._typePrev;
				}
			}

			// Token: 0x1700049A RID: 1178
			// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00022903 File Offset: 0x00020B03
			public bool hasSelection
			{
				get
				{
					return this._id >= 0;
				}
			}

			// Token: 0x06000C0E RID: 3086 RVA: 0x00022911 File Offset: 0x00020B11
			public void Set(int id, ControllerType type)
			{
				this.id = id;
				this.type = type;
			}

			// Token: 0x06000C0F RID: 3087 RVA: 0x00022921 File Offset: 0x00020B21
			public void Clear()
			{
				this._id = -1;
				this._idPrev = -1;
				this._type = ControllerType.Joystick;
				this._typePrev = ControllerType.Joystick;
			}

			// Token: 0x040005EE RID: 1518
			private int _id;

			// Token: 0x040005EF RID: 1519
			private int _idPrev;

			// Token: 0x040005F0 RID: 1520
			private ControllerType _type;

			// Token: 0x040005F1 RID: 1521
			private ControllerType _typePrev;
		}

		// Token: 0x020000E9 RID: 233
		private class DialogHelper
		{
			// Token: 0x1700049B RID: 1179
			// (get) Token: 0x06000C10 RID: 3088 RVA: 0x0002293F File Offset: 0x00020B3F
			private float busyTimer
			{
				get
				{
					if (!this._busyTimerRunning)
					{
						return 0f;
					}
					return this._busyTime - Time.realtimeSinceStartup;
				}
			}

			// Token: 0x1700049C RID: 1180
			// (get) Token: 0x06000C11 RID: 3089 RVA: 0x0002295B File Offset: 0x00020B5B
			// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00022963 File Offset: 0x00020B63
			public bool enabled
			{
				get
				{
					return this._enabled;
				}
				set
				{
					if (!value)
					{
						this._enabled = value;
						this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
						this.StateChanged(0.1f);
						return;
					}
					if (this._type == ControlRemappingDemo1.DialogHelper.DialogType.None)
					{
						return;
					}
					this.StateChanged(0.25f);
				}
			}

			// Token: 0x1700049D RID: 1181
			// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00022996 File Offset: 0x00020B96
			// (set) Token: 0x06000C14 RID: 3092 RVA: 0x000229A8 File Offset: 0x00020BA8
			public ControlRemappingDemo1.DialogHelper.DialogType type
			{
				get
				{
					if (!this._enabled)
					{
						return ControlRemappingDemo1.DialogHelper.DialogType.None;
					}
					return this._type;
				}
				set
				{
					if (value == ControlRemappingDemo1.DialogHelper.DialogType.None)
					{
						this._enabled = false;
						this.StateChanged(0.1f);
					}
					else
					{
						this._enabled = true;
						this.StateChanged(0.25f);
					}
					this._type = value;
				}
			}

			// Token: 0x1700049E RID: 1182
			// (get) Token: 0x06000C15 RID: 3093 RVA: 0x000229DA File Offset: 0x00020BDA
			public bool busy
			{
				get
				{
					return this._busyTimerRunning;
				}
			}

			// Token: 0x06000C16 RID: 3094 RVA: 0x000229E2 File Offset: 0x00020BE2
			public DialogHelper()
			{
				this.drawWindowDelegate = new Action<int>(this.DrawWindow);
				this.drawWindowFunction = new GUI.WindowFunction(this.drawWindowDelegate.Invoke);
			}

			// Token: 0x06000C17 RID: 3095 RVA: 0x00022A13 File Offset: 0x00020C13
			public void StartModal(int queueActionId, ControlRemappingDemo1.DialogHelper.DialogType type, ControlRemappingDemo1.WindowProperties windowProperties, Action<int, ControlRemappingDemo1.UserResponse> resultCallback)
			{
				this.StartModal(queueActionId, type, windowProperties, resultCallback, -1f);
			}

			// Token: 0x06000C18 RID: 3096 RVA: 0x00022A25 File Offset: 0x00020C25
			public void StartModal(int queueActionId, ControlRemappingDemo1.DialogHelper.DialogType type, ControlRemappingDemo1.WindowProperties windowProperties, Action<int, ControlRemappingDemo1.UserResponse> resultCallback, float openBusyDelay)
			{
				this.currentActionId = queueActionId;
				this.windowProperties = windowProperties;
				this.type = type;
				this.resultCallback = resultCallback;
				if (openBusyDelay >= 0f)
				{
					this.StateChanged(openBusyDelay);
				}
			}

			// Token: 0x06000C19 RID: 3097 RVA: 0x00022A55 File Offset: 0x00020C55
			public void Update()
			{
				this.Draw();
				this.UpdateTimers();
			}

			// Token: 0x06000C1A RID: 3098 RVA: 0x00022A64 File Offset: 0x00020C64
			public void Draw()
			{
				if (!this._enabled)
				{
					return;
				}
				bool enabled = GUI.enabled;
				GUI.enabled = true;
				GUILayout.Window(this.windowProperties.windowId, this.windowProperties.rect, this.drawWindowFunction, this.windowProperties.title, Array.Empty<GUILayoutOption>());
				GUI.FocusWindow(this.windowProperties.windowId);
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06000C1B RID: 3099 RVA: 0x00022AD6 File Offset: 0x00020CD6
			public void DrawConfirmButton()
			{
				this.DrawConfirmButton("Confirm");
			}

			// Token: 0x06000C1C RID: 3100 RVA: 0x00022AE4 File Offset: 0x00020CE4
			public void DrawConfirmButton(string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06000C1D RID: 3101 RVA: 0x00022B27 File Offset: 0x00020D27
			public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response)
			{
				this.DrawConfirmButton(response, "Confirm");
			}

			// Token: 0x06000C1E RID: 3102 RVA: 0x00022B38 File Offset: 0x00020D38
			public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response, string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Confirm(response);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06000C1F RID: 3103 RVA: 0x00022B7B File Offset: 0x00020D7B
			public void DrawCancelButton()
			{
				this.DrawCancelButton("Cancel");
			}

			// Token: 0x06000C20 RID: 3104 RVA: 0x00022B88 File Offset: 0x00020D88
			public void DrawCancelButton(string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Cancel();
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06000C21 RID: 3105 RVA: 0x00022BCA File Offset: 0x00020DCA
			public void Confirm()
			{
				this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
			}

			// Token: 0x06000C22 RID: 3106 RVA: 0x00022BD3 File Offset: 0x00020DD3
			public void Confirm(ControlRemappingDemo1.UserResponse response)
			{
				this.resultCallback(this.currentActionId, response);
				this.Close();
			}

			// Token: 0x06000C23 RID: 3107 RVA: 0x00022BED File Offset: 0x00020DED
			public void Cancel()
			{
				this.resultCallback(this.currentActionId, ControlRemappingDemo1.UserResponse.Cancel);
				this.Close();
			}

			// Token: 0x06000C24 RID: 3108 RVA: 0x00022C07 File Offset: 0x00020E07
			private void DrawWindow(int windowId)
			{
				this.windowProperties.windowDrawDelegate(this.windowProperties.title, this.windowProperties.message);
			}

			// Token: 0x06000C25 RID: 3109 RVA: 0x00022C2F File Offset: 0x00020E2F
			private void UpdateTimers()
			{
				if (this._busyTimerRunning && this.busyTimer <= 0f)
				{
					this._busyTimerRunning = false;
				}
			}

			// Token: 0x06000C26 RID: 3110 RVA: 0x00022C4D File Offset: 0x00020E4D
			private void StartBusyTimer(float time)
			{
				this._busyTime = time + Time.realtimeSinceStartup;
				this._busyTimerRunning = true;
			}

			// Token: 0x06000C27 RID: 3111 RVA: 0x00022C63 File Offset: 0x00020E63
			private void Close()
			{
				this.Reset();
				this.StateChanged(0.1f);
			}

			// Token: 0x06000C28 RID: 3112 RVA: 0x00022C76 File Offset: 0x00020E76
			private void StateChanged(float delay)
			{
				this.StartBusyTimer(delay);
			}

			// Token: 0x06000C29 RID: 3113 RVA: 0x00022C7F File Offset: 0x00020E7F
			private void Reset()
			{
				this._enabled = false;
				this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
				this.currentActionId = -1;
				this.resultCallback = null;
			}

			// Token: 0x06000C2A RID: 3114 RVA: 0x00022C9D File Offset: 0x00020E9D
			private void ResetTimers()
			{
				this._busyTimerRunning = false;
			}

			// Token: 0x06000C2B RID: 3115 RVA: 0x00022CA6 File Offset: 0x00020EA6
			public void FullReset()
			{
				this.Reset();
				this.ResetTimers();
			}

			// Token: 0x040005F2 RID: 1522
			private const float openBusyDelay = 0.25f;

			// Token: 0x040005F3 RID: 1523
			private const float closeBusyDelay = 0.1f;

			// Token: 0x040005F4 RID: 1524
			private ControlRemappingDemo1.DialogHelper.DialogType _type;

			// Token: 0x040005F5 RID: 1525
			private bool _enabled;

			// Token: 0x040005F6 RID: 1526
			private float _busyTime;

			// Token: 0x040005F7 RID: 1527
			private bool _busyTimerRunning;

			// Token: 0x040005F8 RID: 1528
			private Action<int> drawWindowDelegate;

			// Token: 0x040005F9 RID: 1529
			private GUI.WindowFunction drawWindowFunction;

			// Token: 0x040005FA RID: 1530
			private ControlRemappingDemo1.WindowProperties windowProperties;

			// Token: 0x040005FB RID: 1531
			private int currentActionId;

			// Token: 0x040005FC RID: 1532
			private Action<int, ControlRemappingDemo1.UserResponse> resultCallback;

			// Token: 0x020000EA RID: 234
			public enum DialogType
			{
				// Token: 0x040005FE RID: 1534
				None,
				// Token: 0x040005FF RID: 1535
				JoystickConflict,
				// Token: 0x04000600 RID: 1536
				ElementConflict,
				// Token: 0x04000601 RID: 1537
				KeyConflict,
				// Token: 0x04000602 RID: 1538
				DeleteAssignmentConfirmation = 10,
				// Token: 0x04000603 RID: 1539
				AssignElement
			}
		}

		// Token: 0x020000EB RID: 235
		private abstract class QueueEntry
		{
			// Token: 0x1700049F RID: 1183
			// (get) Token: 0x06000C2C RID: 3116 RVA: 0x00022CB4 File Offset: 0x00020EB4
			// (set) Token: 0x06000C2D RID: 3117 RVA: 0x00022CBC File Offset: 0x00020EBC
			public int id { get; protected set; }

			// Token: 0x170004A0 RID: 1184
			// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00022CC5 File Offset: 0x00020EC5
			// (set) Token: 0x06000C2F RID: 3119 RVA: 0x00022CCD File Offset: 0x00020ECD
			public ControlRemappingDemo1.QueueActionType queueActionType { get; protected set; }

			// Token: 0x170004A1 RID: 1185
			// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00022CD6 File Offset: 0x00020ED6
			// (set) Token: 0x06000C31 RID: 3121 RVA: 0x00022CDE File Offset: 0x00020EDE
			public ControlRemappingDemo1.QueueEntry.State state { get; protected set; }

			// Token: 0x170004A2 RID: 1186
			// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00022CE7 File Offset: 0x00020EE7
			// (set) Token: 0x06000C33 RID: 3123 RVA: 0x00022CEF File Offset: 0x00020EEF
			public ControlRemappingDemo1.UserResponse response { get; protected set; }

			// Token: 0x170004A3 RID: 1187
			// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00022CF8 File Offset: 0x00020EF8
			protected static int nextId
			{
				get
				{
					int num = ControlRemappingDemo1.QueueEntry.uidCounter;
					ControlRemappingDemo1.QueueEntry.uidCounter++;
					return num;
				}
			}

			// Token: 0x06000C35 RID: 3125 RVA: 0x00022D0B File Offset: 0x00020F0B
			public QueueEntry(ControlRemappingDemo1.QueueActionType queueActionType)
			{
				this.id = ControlRemappingDemo1.QueueEntry.nextId;
				this.queueActionType = queueActionType;
			}

			// Token: 0x06000C36 RID: 3126 RVA: 0x00022D25 File Offset: 0x00020F25
			public void Confirm(ControlRemappingDemo1.UserResponse response)
			{
				this.state = ControlRemappingDemo1.QueueEntry.State.Confirmed;
				this.response = response;
			}

			// Token: 0x06000C37 RID: 3127 RVA: 0x00022D35 File Offset: 0x00020F35
			public void Cancel()
			{
				this.state = ControlRemappingDemo1.QueueEntry.State.Canceled;
			}

			// Token: 0x04000608 RID: 1544
			private static int uidCounter;

			// Token: 0x020000EC RID: 236
			public enum State
			{
				// Token: 0x0400060A RID: 1546
				Waiting,
				// Token: 0x0400060B RID: 1547
				Confirmed,
				// Token: 0x0400060C RID: 1548
				Canceled
			}
		}

		// Token: 0x020000ED RID: 237
		private class JoystickAssignmentChange : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170004A4 RID: 1188
			// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00022D3E File Offset: 0x00020F3E
			// (set) Token: 0x06000C39 RID: 3129 RVA: 0x00022D46 File Offset: 0x00020F46
			public int playerId { get; private set; }

			// Token: 0x170004A5 RID: 1189
			// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00022D4F File Offset: 0x00020F4F
			// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00022D57 File Offset: 0x00020F57
			public int joystickId { get; private set; }

			// Token: 0x170004A6 RID: 1190
			// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00022D60 File Offset: 0x00020F60
			// (set) Token: 0x06000C3D RID: 3133 RVA: 0x00022D68 File Offset: 0x00020F68
			public bool assign { get; private set; }

			// Token: 0x06000C3E RID: 3134 RVA: 0x00022D71 File Offset: 0x00020F71
			public JoystickAssignmentChange(int newPlayerId, int joystickId, bool assign)
				: base(ControlRemappingDemo1.QueueActionType.JoystickAssignment)
			{
				this.playerId = newPlayerId;
				this.joystickId = joystickId;
				this.assign = assign;
			}
		}

		// Token: 0x020000EE RID: 238
		private class ElementAssignmentChange : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170004A7 RID: 1191
			// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00022D8F File Offset: 0x00020F8F
			// (set) Token: 0x06000C40 RID: 3136 RVA: 0x00022D97 File Offset: 0x00020F97
			public ControlRemappingDemo1.ElementAssignmentChangeType changeType { get; set; }

			// Token: 0x170004A8 RID: 1192
			// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00022DA0 File Offset: 0x00020FA0
			// (set) Token: 0x06000C42 RID: 3138 RVA: 0x00022DA8 File Offset: 0x00020FA8
			public InputMapper.Context context { get; private set; }

			// Token: 0x06000C43 RID: 3139 RVA: 0x00022DB1 File Offset: 0x00020FB1
			public ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType changeType, InputMapper.Context context)
				: base(ControlRemappingDemo1.QueueActionType.ElementAssignment)
			{
				this.changeType = changeType;
				this.context = context;
			}

			// Token: 0x06000C44 RID: 3140 RVA: 0x00022DC8 File Offset: 0x00020FC8
			public ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChange other)
				: this(other.changeType, other.context.Clone())
			{
			}
		}

		// Token: 0x020000EF RID: 239
		private class FallbackJoystickIdentification : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170004A9 RID: 1193
			// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00022DE1 File Offset: 0x00020FE1
			// (set) Token: 0x06000C46 RID: 3142 RVA: 0x00022DE9 File Offset: 0x00020FE9
			public int joystickId { get; private set; }

			// Token: 0x170004AA RID: 1194
			// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00022DF2 File Offset: 0x00020FF2
			// (set) Token: 0x06000C48 RID: 3144 RVA: 0x00022DFA File Offset: 0x00020FFA
			public string joystickName { get; private set; }

			// Token: 0x06000C49 RID: 3145 RVA: 0x00022E03 File Offset: 0x00021003
			public FallbackJoystickIdentification(int joystickId, string joystickName)
				: base(ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification)
			{
				this.joystickId = joystickId;
				this.joystickName = joystickName;
			}
		}

		// Token: 0x020000F0 RID: 240
		private class Calibration : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170004AB RID: 1195
			// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00022E1A File Offset: 0x0002101A
			// (set) Token: 0x06000C4B RID: 3147 RVA: 0x00022E22 File Offset: 0x00021022
			public Player player { get; private set; }

			// Token: 0x170004AC RID: 1196
			// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00022E2B File Offset: 0x0002102B
			// (set) Token: 0x06000C4D RID: 3149 RVA: 0x00022E33 File Offset: 0x00021033
			public ControllerType controllerType { get; private set; }

			// Token: 0x170004AD RID: 1197
			// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00022E3C File Offset: 0x0002103C
			// (set) Token: 0x06000C4F RID: 3151 RVA: 0x00022E44 File Offset: 0x00021044
			public Joystick joystick { get; private set; }

			// Token: 0x170004AE RID: 1198
			// (get) Token: 0x06000C50 RID: 3152 RVA: 0x00022E4D File Offset: 0x0002104D
			// (set) Token: 0x06000C51 RID: 3153 RVA: 0x00022E55 File Offset: 0x00021055
			public CalibrationMap calibrationMap { get; private set; }

			// Token: 0x06000C52 RID: 3154 RVA: 0x00022E5E File Offset: 0x0002105E
			public Calibration(Player player, Joystick joystick, CalibrationMap calibrationMap)
				: base(ControlRemappingDemo1.QueueActionType.Calibrate)
			{
				this.player = player;
				this.joystick = joystick;
				this.calibrationMap = calibrationMap;
				this.selectedElementIdentifierId = -1;
			}

			// Token: 0x04000618 RID: 1560
			public int selectedElementIdentifierId;

			// Token: 0x04000619 RID: 1561
			public bool recording;
		}

		// Token: 0x020000F1 RID: 241
		private struct WindowProperties
		{
			// Token: 0x0400061A RID: 1562
			public int windowId;

			// Token: 0x0400061B RID: 1563
			public Rect rect;

			// Token: 0x0400061C RID: 1564
			public Action<string, string> windowDrawDelegate;

			// Token: 0x0400061D RID: 1565
			public string title;

			// Token: 0x0400061E RID: 1566
			public string message;
		}

		// Token: 0x020000F2 RID: 242
		private enum QueueActionType
		{
			// Token: 0x04000620 RID: 1568
			None,
			// Token: 0x04000621 RID: 1569
			JoystickAssignment,
			// Token: 0x04000622 RID: 1570
			ElementAssignment,
			// Token: 0x04000623 RID: 1571
			FallbackJoystickIdentification,
			// Token: 0x04000624 RID: 1572
			Calibrate
		}

		// Token: 0x020000F3 RID: 243
		private enum ElementAssignmentChangeType
		{
			// Token: 0x04000626 RID: 1574
			Add,
			// Token: 0x04000627 RID: 1575
			Replace,
			// Token: 0x04000628 RID: 1576
			Remove,
			// Token: 0x04000629 RID: 1577
			ReassignOrRemove,
			// Token: 0x0400062A RID: 1578
			ConflictCheck
		}

		// Token: 0x020000F4 RID: 244
		public enum UserResponse
		{
			// Token: 0x0400062C RID: 1580
			Confirm,
			// Token: 0x0400062D RID: 1581
			Cancel,
			// Token: 0x0400062E RID: 1582
			Custom1,
			// Token: 0x0400062F RID: 1583
			Custom2
		}
	}
}
