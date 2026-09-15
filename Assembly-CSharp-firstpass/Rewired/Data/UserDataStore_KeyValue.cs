using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rewired.Utils.Attributes;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	// Token: 0x02000033 RID: 51
	public abstract class UserDataStore_KeyValue : UserDataStore
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00005235 File Offset: 0x00003435
		// (set) Token: 0x06000386 RID: 902 RVA: 0x0000523D File Offset: 0x0000343D
		public bool isEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				this._isEnabled = value;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00005246 File Offset: 0x00003446
		// (set) Token: 0x06000388 RID: 904 RVA: 0x0000524E File Offset: 0x0000344E
		public bool loadDataOnStart
		{
			get
			{
				return this._loadDataOnStart;
			}
			set
			{
				this._loadDataOnStart = value;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00005257 File Offset: 0x00003457
		// (set) Token: 0x0600038A RID: 906 RVA: 0x0000525F File Offset: 0x0000345F
		public bool loadJoystickAssignments
		{
			get
			{
				return this._loadJoystickAssignments;
			}
			set
			{
				this._loadJoystickAssignments = value;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00005268 File Offset: 0x00003468
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00005270 File Offset: 0x00003470
		public bool loadKeyboardAssignments
		{
			get
			{
				return this._loadKeyboardAssignments;
			}
			set
			{
				this._loadKeyboardAssignments = value;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00005279 File Offset: 0x00003479
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00005281 File Offset: 0x00003481
		public bool loadMouseAssignments
		{
			get
			{
				return this._loadMouseAssignments;
			}
			set
			{
				this._loadMouseAssignments = value;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000528A File Offset: 0x0000348A
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00005292 File Offset: 0x00003492
		public UserDataStore_KeyValue.ActionMappingSaveMode actionMappingSaveMode
		{
			get
			{
				return this._actionMappingSaveMode;
			}
			set
			{
				this._actionMappingSaveMode = value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000391 RID: 913
		protected abstract UserDataStore_KeyValue.IDataStore dataStore { get; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000529B File Offset: 0x0000349B
		private bool loadControllerAssignments
		{
			get
			{
				return this._loadKeyboardAssignments || this._loadMouseAssignments || this._loadJoystickAssignments;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000393 RID: 915 RVA: 0x000052B8 File Offset: 0x000034B8
		private List<int> allActionIds
		{
			get
			{
				if (this.__allActionIds != null)
				{
					return this.__allActionIds;
				}
				List<int> list = new List<int>();
				IList<InputAction> actions = ReInput.mapping.Actions;
				for (int i = 0; i < actions.Count; i++)
				{
					list.Add(actions[i].id);
				}
				this.__allActionIds = list;
				return list;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00005310 File Offset: 0x00003510
		private string allActionIdsString
		{
			get
			{
				if (!string.IsNullOrEmpty(this.__allActionIdsString))
				{
					return this.__allActionIdsString;
				}
				StringBuilder stringBuilder = new StringBuilder();
				List<int> allActionIds = this.allActionIds;
				for (int i = 0; i < allActionIds.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(allActionIds[i]);
				}
				this.__allActionIdsString = stringBuilder.ToString();
				return this.__allActionIdsString;
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000537F File Offset: 0x0000357F
		public override void Save()
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not save any data.", this);
				return;
			}
			this.SaveAll();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000053AA File Offset: 0x000035AA
		public override void SaveControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000053D8 File Offset: 0x000035D8
		public override void SaveControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00005405 File Offset: 0x00003605
		public override void SavePlayerData(int playerId)
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not save any data.", this);
				return;
			}
			this.SavePlayerDataNow(playerId);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00005431 File Offset: 0x00003631
		public override void SaveInputBehavior(int playerId, int behaviorId)
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not save any data.", this);
				return;
			}
			this.SaveInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000545E File Offset: 0x0000365E
		public override void Load()
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not load any data.", this);
				return;
			}
			this.LoadAll();
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000548A File Offset: 0x0000368A
		public override void LoadControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not load any data.", this);
				return;
			}
			this.LoadControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000054B9 File Offset: 0x000036B9
		public override void LoadControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not load any data.", this);
				return;
			}
			this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000054E7 File Offset: 0x000036E7
		public override void LoadPlayerData(int playerId)
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not load any data.", this);
				return;
			}
			this.LoadPlayerDataNow(playerId);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00005514 File Offset: 0x00003714
		public override void LoadInputBehavior(int playerId, int behaviorId)
		{
			if (!this._isEnabled)
			{
				Debug.LogWarning("Rewired: " + UserDataStore_KeyValue.thisScriptName + " is disabled and will not load any data.", this);
				return;
			}
			this.LoadInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00005542 File Offset: 0x00003742
		protected override void OnInitialize()
		{
			if (this._loadDataOnStart)
			{
				this.Load();
				if (this.loadControllerAssignments && ReInput.controllers.joystickCount > 0)
				{
					this._wasJoystickEverDetected = true;
					this.SaveControllerAssignments();
				}
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00005578 File Offset: 0x00003778
		protected override void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this._isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				this.LoadJoystickData(args.controllerId);
				if (this._loadDataOnStart && this._loadJoystickAssignments && !this._wasJoystickEverDetected)
				{
					base.StartCoroutine(this.LoadJoystickAssignmentsDeferred());
				}
				if (this._loadJoystickAssignments && !this._deferredJoystickAssignmentLoadPending)
				{
					this.SaveControllerAssignments();
				}
				this._wasJoystickEverDetected = true;
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000055E7 File Offset: 0x000037E7
		protected override void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this._isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickData(args.controllerId);
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00005607 File Offset: 0x00003807
		protected override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this._isEnabled)
			{
				return;
			}
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00005624 File Offset: 0x00003824
		public override void SaveControllerMap(int playerId, ControllerMap controllerMap)
		{
			if (controllerMap == null)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			this.SaveControllerMap(player, controllerMap);
			this.dataStore.Save();
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000565C File Offset: 0x0000385C
		public override ControllerMap LoadControllerMap(int playerId, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return null;
			}
			return this.LoadControllerMap(player, controllerIdentifier, categoryId, layoutId);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00005685 File Offset: 0x00003885
		public virtual void ClearSaveData()
		{
			this.dataStore.Clear();
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00005694 File Offset: 0x00003894
		private int LoadAll()
		{
			int num = 0;
			if (this.loadControllerAssignments && this.LoadControllerAssignmentsNow())
			{
				num++;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				num += this.LoadPlayerDataNow(allPlayers[i]);
			}
			return num + this.LoadAllJoystickCalibrationData();
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000056ED File Offset: 0x000038ED
		private int LoadPlayerDataNow(int playerId)
		{
			return this.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId));
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00005700 File Offset: 0x00003900
		private int LoadPlayerDataNow(Player player)
		{
			if (player == null)
			{
				return 0;
			}
			int num = 0;
			num += this.LoadInputBehaviors(player.id);
			num += this.LoadControllerMaps(player.id, ControllerType.Keyboard, 0);
			num += this.LoadControllerMaps(player.id, ControllerType.Mouse, 0);
			foreach (Joystick joystick in player.controllers.Joysticks)
			{
				num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystick.id);
			}
			this.RefreshLayoutManager(player.id);
			return num;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x000057A8 File Offset: 0x000039A8
		private int LoadAllJoystickCalibrationData()
		{
			int num = 0;
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				num += this.LoadJoystickCalibrationData(joysticks[i]);
			}
			return num;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000057E4 File Offset: 0x000039E4
		private int LoadJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return 0;
			}
			if (!joystick.ImportCalibrationMapFromJsonString(this.GetJoystickCalibrationMapJson(joystick)))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000057FD File Offset: 0x000039FD
		private int LoadJoystickCalibrationData(int joystickId)
		{
			return this.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00005810 File Offset: 0x00003A10
		private int LoadJoystickData(int joystickId)
		{
			int num = 0;
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
				{
					num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystickId);
					this.RefreshLayoutManager(player.id);
				}
			}
			return num + this.LoadJoystickCalibrationData(joystickId);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000587A File Offset: 0x00003A7A
		private int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0 + this.LoadControllerMaps(playerId, controllerType, controllerId);
			this.RefreshLayoutManager(playerId);
			return num + this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00005898 File Offset: 0x00003A98
		private int LoadControllerDataNow(ControllerType controllerType, int controllerId)
		{
			int num = 0;
			if (controllerType == ControllerType.Joystick)
			{
				num += this.LoadJoystickCalibrationData(controllerId);
			}
			return num;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000058B8 File Offset: 0x00003AB8
		private int LoadControllerMaps(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0;
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return num;
			}
			Controller controller = ReInput.controllers.GetController(controllerType, controllerId);
			if (controller == null)
			{
				return num;
			}
			IList<InputMapCategory> mapCategories = ReInput.mapping.MapCategories;
			for (int i = 0; i < mapCategories.Count; i++)
			{
				InputMapCategory inputMapCategory = mapCategories[i];
				if (inputMapCategory.userAssignable)
				{
					IList<InputLayout> list = ReInput.mapping.MapLayouts(controller.type);
					for (int j = 0; j < list.Count; j++)
					{
						InputLayout inputLayout = list[j];
						UserDataStore_KeyValue.ActionMappingSaveMode actionMappingSaveMode = this._actionMappingSaveMode;
						if (actionMappingSaveMode != UserDataStore_KeyValue.ActionMappingSaveMode.ByController)
						{
							if (actionMappingSaveMode != UserDataStore_KeyValue.ActionMappingSaveMode.ByControllerElementRole)
							{
								throw new NotImplementedException();
							}
							Dictionary<string, UserDataStore_KeyValue.ControllerElementByRoleMap> dictionary = ((this._tempElementByRoleMaps != null) ? this._tempElementByRoleMaps : (this._tempElementByRoleMaps = new Dictionary<string, UserDataStore_KeyValue.ControllerElementByRoleMap>()));
							dictionary.Clear();
							bool flag = false;
							bool flag2 = false;
							for (int k = 0; k < controller.elementCount; k++)
							{
								string role = controller.Elements[k].elementIdentifier.role;
								if (!string.IsNullOrEmpty(role))
								{
									this.LoadControllerElementMapByRole(player, controller, role, inputMapCategory.id, inputLayout.id, dictionary);
								}
							}
							ControllerMap controllerMap = this.LoadControllerMap(player, controller.identifier, inputMapCategory.id, inputLayout.id);
							if (controllerMap == null)
							{
								controllerMap = player.controllers.maps.GetMap(controller.type, controller.id, inputMapCategory.id, inputLayout.id);
								if (controllerMap == null)
								{
									if (dictionary.Count == 0)
									{
										goto IL_03A5;
									}
									controllerMap = ControllerMap.Create(controller, inputMapCategory.id, inputLayout.id);
								}
							}
							else
							{
								flag = true;
							}
							if (dictionary.Count != 0)
							{
								if (this._tempElementByRoleMapsEnabled == null)
								{
									this._tempElementByRoleMapsEnabled = new Dictionary<string, bool>();
								}
								this._tempElementByRoleMapsEnabled.Clear();
								for (int l = controllerMap.elementMapCount - 1; l >= 0; l--)
								{
									ActionElementMap actionElementMap = controllerMap.ElementMaps[l];
									ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(actionElementMap.elementIdentifierId);
									if (elementIdentifierById != null && dictionary.ContainsKey(elementIdentifierById.role))
									{
										this._tempElementByRoleMapsEnabled[elementIdentifierById.role] = actionElementMap.enabled;
										controllerMap.DeleteElementMap(actionElementMap.id);
										flag2 = true;
									}
								}
								foreach (KeyValuePair<string, UserDataStore_KeyValue.ControllerElementByRoleMap> keyValuePair in dictionary)
								{
									UserDataStore_KeyValue.ControllerElementByRoleMap value = keyValuePair.Value;
									for (int m = 0; m < controller.Elements.Count; m++)
									{
										Controller.Element element = controller.Elements[m];
										if (!(element.elementIdentifier.role != keyValuePair.Value.role) && value.data != null && value.data.Count != 0)
										{
											for (int n = 0; n < value.data.Count; n++)
											{
												ElementAssignment elementAssignment;
												ActionElementMap actionElementMap2;
												if (value.data[n].TryGetElementAssignment(controllerType, element, out elementAssignment) && controllerMap.CreateElementMap(elementAssignment, out actionElementMap2))
												{
													bool flag3;
													if (this._tempElementByRoleMapsEnabled.TryGetValue(keyValuePair.Value.role, out flag3))
													{
														actionElementMap2.enabled = flag3;
													}
													flag = true;
													flag2 = true;
												}
											}
										}
									}
								}
							}
							if (flag2)
							{
								controllerMap.isModified = false;
							}
							if (flag)
							{
								player.controllers.maps.AddMap(controller, controllerMap);
								num++;
							}
						}
						else
						{
							ControllerMap controllerMap2 = this.LoadControllerMap(player, controller.identifier, inputMapCategory.id, inputLayout.id);
							if (controllerMap2 != null)
							{
								player.controllers.maps.AddMap(controller, controllerMap2);
								num++;
							}
						}
						IL_03A5:;
					}
				}
			}
			return num;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00005CA4 File Offset: 0x00003EA4
		private ControllerMap LoadControllerMap(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			if (player == null)
			{
				return null;
			}
			string controllerMapJson = this.GetControllerMapJson(player, controllerIdentifier, categoryId, layoutId);
			if (string.IsNullOrEmpty(controllerMapJson))
			{
				return null;
			}
			ControllerMap controllerMap = ControllerMap.CreateFromJson(controllerIdentifier.controllerType, controllerMapJson);
			if (controllerMap == null)
			{
				return null;
			}
			List<int> controllerMapKnownActionIds = this.GetControllerMapKnownActionIds(player, controllerIdentifier, categoryId, layoutId);
			this.AddDefaultMappingsForNewActions(controllerIdentifier, controllerMap, controllerMapKnownActionIds);
			return controllerMap;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00005CF8 File Offset: 0x00003EF8
		private bool LoadControllerElementMapByRole(Player player, Controller controller, string role, int mapCategoryId, int layoutId, Dictionary<string, UserDataStore_KeyValue.ControllerElementByRoleMap> elementByRoleMaps)
		{
			if (string.IsNullOrEmpty(role))
			{
				return false;
			}
			this._sb.Length = 0;
			UserDataStore_KeyValue.AppendPlayerKey(this._sb, player);
			UserDataStore_KeyValue.AppendControllerElementByRoleMapKey(this._sb, role, mapCategoryId, layoutId, 0);
			bool flag;
			try
			{
				string text;
				if (!UserDataStore_KeyValue.TryGetString(this.dataStore, this._sb.ToString(), out text))
				{
					flag = false;
				}
				else if (string.IsNullOrEmpty(text))
				{
					flag = false;
				}
				else
				{
					UserDataStore_KeyValue.ControllerElementByRoleMap controllerElementByRoleMap = UserDataStore_KeyValue.ControllerElementByRoleMap.FromJson(role, text);
					if (controllerElementByRoleMap == null)
					{
						flag = false;
					}
					else
					{
						elementByRoleMaps[role] = controllerElementByRoleMap;
						flag = true;
					}
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00005D94 File Offset: 0x00003F94
		private int LoadInputBehaviors(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return 0;
			}
			int num = 0;
			IList<InputBehavior> inputBehaviors = ReInput.mapping.GetInputBehaviors(player.id);
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				num += this.LoadInputBehaviorNow(player, inputBehaviors[i]);
			}
			return num;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00005DE8 File Offset: 0x00003FE8
		private int LoadInputBehaviorNow(int playerId, int behaviorId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return 0;
			}
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return 0;
			}
			return this.LoadInputBehaviorNow(player, inputBehavior);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00005E20 File Offset: 0x00004020
		private int LoadInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return 0;
			}
			string inputBehaviorJson = this.GetInputBehaviorJson(player, inputBehavior.id);
			if (inputBehaviorJson == null || inputBehaviorJson == string.Empty)
			{
				return 0;
			}
			if (!inputBehavior.ImportJsonString(inputBehaviorJson))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00005E64 File Offset: 0x00004064
		private bool LoadControllerAssignmentsNow()
		{
			try
			{
				UserDataStore_KeyValue.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = this.LoadControllerAssignmentData();
				if (controllerAssignmentSaveInfo == null)
				{
					return false;
				}
				if (this._loadKeyboardAssignments || this._loadMouseAssignments)
				{
					this.LoadKeyboardAndMouseAssignmentsNow(controllerAssignmentSaveInfo);
				}
				if (this._loadJoystickAssignments)
				{
					this.LoadJoystickAssignmentsNow(controllerAssignmentSaveInfo);
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00005EC0 File Offset: 0x000040C0
		private bool LoadKeyboardAndMouseAssignmentsNow(UserDataStore_KeyValue.ControllerAssignmentSaveInfo data)
		{
			try
			{
				if (data == null && (data = this.LoadControllerAssignmentData()) == null)
				{
					return false;
				}
				foreach (Player player in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player.id))
					{
						UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player.id)];
						if (this._loadKeyboardAssignments)
						{
							player.controllers.hasKeyboard = playerInfo.hasKeyboard;
						}
						if (this._loadMouseAssignments)
						{
							player.controllers.hasMouse = playerInfo.hasMouse;
						}
					}
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00005F88 File Offset: 0x00004188
		private bool LoadJoystickAssignmentsNow(UserDataStore_KeyValue.ControllerAssignmentSaveInfo data)
		{
			try
			{
				if (ReInput.controllers.joystickCount == 0)
				{
					return false;
				}
				if (data == null && (data = this.LoadControllerAssignmentData()) == null)
				{
					return false;
				}
				foreach (Player player in ReInput.players.AllPlayers)
				{
					player.controllers.ClearControllersOfType(ControllerType.Joystick);
				}
				List<UserDataStore_KeyValue.JoystickAssignmentHistoryInfo> list = (this._loadJoystickAssignments ? new List<UserDataStore_KeyValue.JoystickAssignmentHistoryInfo>() : null);
				foreach (Player player2 in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player2.id))
					{
						UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player2.id)];
						for (int i = 0; i < playerInfo.joystickCount; i++)
						{
							UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo2 = playerInfo.joysticks[i];
							if (joystickInfo2 != null)
							{
								Joystick joystick = this.FindJoystickPrecise(joystickInfo2);
								if (joystick != null)
								{
									if (list.Find((UserDataStore_KeyValue.JoystickAssignmentHistoryInfo x) => x.joystick == joystick) == null)
									{
										list.Add(new UserDataStore_KeyValue.JoystickAssignmentHistoryInfo(joystick, joystickInfo2.id));
									}
									player2.controllers.AddController(joystick, false);
								}
							}
						}
					}
				}
				if (this._allowImpreciseJoystickAssignmentMatching)
				{
					foreach (Player player3 in ReInput.players.AllPlayers)
					{
						if (data.ContainsPlayer(player3.id))
						{
							UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo playerInfo2 = data.players[data.IndexOfPlayer(player3.id)];
							for (int j = 0; j < playerInfo2.joystickCount; j++)
							{
								UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = playerInfo2.joysticks[j];
								if (joystickInfo != null)
								{
									Joystick joystick2 = null;
									int num = list.FindIndex((UserDataStore_KeyValue.JoystickAssignmentHistoryInfo x) => x.oldJoystickId == joystickInfo.id);
									if (num >= 0)
									{
										joystick2 = list[num].joystick;
									}
									else
									{
										List<Joystick> list2;
										if (!this.TryFindJoysticksImprecise(joystickInfo, out list2))
										{
											goto IL_0298;
										}
										using (List<Joystick>.Enumerator enumerator2 = list2.GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												Joystick match = enumerator2.Current;
												if (list.Find((UserDataStore_KeyValue.JoystickAssignmentHistoryInfo x) => x.joystick == match) == null)
												{
													joystick2 = match;
													break;
												}
											}
										}
										if (joystick2 == null)
										{
											goto IL_0298;
										}
										list.Add(new UserDataStore_KeyValue.JoystickAssignmentHistoryInfo(joystick2, joystickInfo.id));
									}
									player3.controllers.AddController(joystick2, false);
								}
								IL_0298:;
							}
						}
					}
				}
			}
			catch
			{
			}
			if (ReInput.configuration.autoAssignJoysticks)
			{
				ReInput.controllers.AutoAssignJoysticks();
			}
			return true;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x000062F4 File Offset: 0x000044F4
		private UserDataStore_KeyValue.ControllerAssignmentSaveInfo LoadControllerAssignmentData()
		{
			UserDataStore_KeyValue.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo;
			try
			{
				string text;
				if (!UserDataStore_KeyValue.TryGetString(this.dataStore, "ControllerAssignments", out text))
				{
					controllerAssignmentSaveInfo = null;
				}
				else if (string.IsNullOrEmpty(text))
				{
					controllerAssignmentSaveInfo = null;
				}
				else
				{
					UserDataStore_KeyValue.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo2 = JsonParser.FromJson<UserDataStore_KeyValue.ControllerAssignmentSaveInfo>(text);
					if (controllerAssignmentSaveInfo2 == null || controllerAssignmentSaveInfo2.playerCount == 0)
					{
						controllerAssignmentSaveInfo = null;
					}
					else
					{
						controllerAssignmentSaveInfo = controllerAssignmentSaveInfo2;
					}
				}
			}
			catch
			{
				controllerAssignmentSaveInfo = null;
			}
			return controllerAssignmentSaveInfo;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00006358 File Offset: 0x00004558
		private IEnumerator LoadJoystickAssignmentsDeferred()
		{
			this._deferredJoystickAssignmentLoadPending = true;
			yield return new WaitForEndOfFrame();
			if (!ReInput.isReady)
			{
				yield break;
			}
			this.LoadJoystickAssignmentsNow(null);
			this.SaveControllerAssignments();
			this._deferredJoystickAssignmentLoadPending = false;
			yield break;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00006368 File Offset: 0x00004568
		private void SaveAll()
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				this.SavePlayerDataNow(allPlayers[i]);
			}
			this.SaveAllJoystickCalibrationData();
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
			this.dataStore.Save();
			for (int j = 0; j < allPlayers.Count; j++)
			{
				this.OnControllerMapsSaved(allPlayers[j]);
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000063E0 File Offset: 0x000045E0
		private void SavePlayerDataNow(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			this.SavePlayerDataNow(player);
			this.dataStore.Save();
			this.OnControllerMapsSaved(player);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00006414 File Offset: 0x00004614
		private void SavePlayerDataNow(Player player)
		{
			if (player == null)
			{
				return;
			}
			PlayerSaveData saveData = player.GetSaveData(true);
			this.SaveInputBehaviors(player, saveData);
			this.SaveControllerMaps(player, saveData);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00006440 File Offset: 0x00004640
		private void SaveAllJoystickCalibrationData()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				this.SaveJoystickCalibrationData(joysticks[i]);
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00006476 File Offset: 0x00004676
		private void SaveJoystickCalibrationData(int joystickId)
		{
			this.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000648C File Offset: 0x0000468C
		private void SaveJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
			string joystickCalibrationMapKey = this.GetJoystickCalibrationMapKey(joystick);
			this.dataStore.SetValue(joystickCalibrationMapKey, calibrationMapSaveData.map.ToJsonString());
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000064C4 File Offset: 0x000046C4
		private void SaveJoystickData(int joystickId)
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
				{
					this.SaveControllerMaps(player.id, ControllerType.Joystick, joystickId);
				}
			}
			this.SaveJoystickCalibrationData(joystickId);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00006519 File Offset: 0x00004719
		private void SaveControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			this.SaveControllerMaps(playerId, controllerType, controllerId);
			this.SaveControllerData(controllerType, controllerId);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000652C File Offset: 0x0000472C
		private void SaveControllerDataNow(ControllerType controllerType, int controllerId)
		{
			if (controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickCalibrationData(controllerId);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000653C File Offset: 0x0000473C
		private void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
		{
			List<ControllerMapSaveData> list = new List<ControllerMapSaveData>(playerSaveData.AllControllerMapSaveData);
			if (this._actionMappingSaveMode == UserDataStore_KeyValue.ActionMappingSaveMode.ByControllerElementRole)
			{
				list.Sort(new Comparison<ControllerMapSaveData>(UserDataStore_KeyValue.SortOldestToNewest));
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.SaveControllerMap(player, list[i].map);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00006598 File Offset: 0x00004798
		private void SaveControllerMaps(int playerId, ControllerType controllerType, int controllerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(controllerType, controllerId))
			{
				return;
			}
			ControllerMapSaveData[] mapSaveData = player.controllers.maps.GetMapSaveData(controllerType, controllerId, true);
			if (mapSaveData == null)
			{
				return;
			}
			if (this._actionMappingSaveMode == UserDataStore_KeyValue.ActionMappingSaveMode.ByControllerElementRole)
			{
				List<ControllerMapSaveData> list = new List<ControllerMapSaveData>(mapSaveData);
				list.Sort(new Comparison<ControllerMapSaveData>(UserDataStore_KeyValue.SortOldestToNewest));
				list.CopyTo(mapSaveData);
			}
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				this.SaveControllerMap(player, mapSaveData[i].map);
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00006624 File Offset: 0x00004824
		private void SaveControllerMap(Player player, ControllerMap controllerMap)
		{
			UserDataStore_KeyValue.ActionMappingSaveMode actionMappingSaveMode = this._actionMappingSaveMode;
			if (actionMappingSaveMode == UserDataStore_KeyValue.ActionMappingSaveMode.ByController)
			{
				this.SaveControllerMapByController(player, controllerMap);
				return;
			}
			if (actionMappingSaveMode != UserDataStore_KeyValue.ActionMappingSaveMode.ByControllerElementRole)
			{
				throw new NotImplementedException();
			}
			this.SaveControllerMapByControllerElementRole(player, controllerMap.controller, controllerMap);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00006660 File Offset: 0x00004860
		private void SaveControllerMapByController(Player player, ControllerMap controllerMap)
		{
			string text = this.GetControllerMapKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, 0);
			this.dataStore.SetValue(text, controllerMap.ToJsonString());
			text = this.GetControllerMapKnownActionIdsKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, 0);
			this.dataStore.SetValue(text, this.allActionIdsString);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000066D4 File Offset: 0x000048D4
		private void SaveControllerMapByControllerElementRole(Player player, Controller controller, ControllerMap controllerMap)
		{
			if (controller == null)
			{
				return;
			}
			this.SaveControllerMapByController(player, controllerMap);
			IList<ActionElementMap> elementMaps = controllerMap.ElementMaps;
			Dictionary<string, UserDataStore_KeyValue.ControllerElementByRoleMap> dictionary = null;
			for (int i = 0; i < controller.elementCount; i++)
			{
				string role = controller.Elements[i].elementIdentifier.role;
				if (!string.IsNullOrEmpty(role))
				{
					bool flag = false;
					for (int j = 0; j < elementMaps.Count; j++)
					{
						Controller.Element elementById = controller.GetElementById(elementMaps[j].elementIdentifierId);
						if (elementById != null && !(elementById.elementIdentifier.role != role))
						{
							flag |= this.AddControllerElementByRoleMapEntry(player, controllerMap.controller, elementMaps[j], ref dictionary);
						}
					}
					if (!flag)
					{
						if (dictionary == null)
						{
							dictionary = new Dictionary<string, UserDataStore_KeyValue.ControllerElementByRoleMap>();
						}
						dictionary.Add(role, new UserDataStore_KeyValue.ControllerElementByRoleMap
						{
							role = role
						});
					}
				}
			}
			if (dictionary == null)
			{
				return;
			}
			foreach (KeyValuePair<string, UserDataStore_KeyValue.ControllerElementByRoleMap> keyValuePair in dictionary)
			{
				this._sb.Length = 0;
				UserDataStore_KeyValue.AppendPlayerKey(this._sb, player);
				UserDataStore_KeyValue.AppendControllerElementByRoleMapKey(this._sb, keyValuePair.Value.role, controllerMap.categoryId, controllerMap.layoutId, 0);
				this.dataStore.SetValue(this._sb.ToString(), keyValuePair.Value.ToJson());
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00006850 File Offset: 0x00004A50
		private bool AddControllerElementByRoleMapEntry(Player player, Controller controller, ActionElementMap elementMap, ref Dictionary<string, UserDataStore_KeyValue.ControllerElementByRoleMap> maps)
		{
			ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(elementMap.elementIdentifierId);
			if (elementIdentifierById == null || string.IsNullOrEmpty(elementIdentifierById.role))
			{
				return false;
			}
			if (maps == null)
			{
				maps = new Dictionary<string, UserDataStore_KeyValue.ControllerElementByRoleMap>();
			}
			UserDataStore_KeyValue.ControllerElementByRoleMap controllerElementByRoleMap;
			if (!maps.TryGetValue(elementIdentifierById.role, out controllerElementByRoleMap))
			{
				controllerElementByRoleMap = new UserDataStore_KeyValue.ControllerElementByRoleMap();
				controllerElementByRoleMap.role = elementIdentifierById.role;
				maps.Add(elementIdentifierById.role, controllerElementByRoleMap);
			}
			controllerElementByRoleMap.Add(elementMap);
			return true;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000068C4 File Offset: 0x00004AC4
		private void SaveInputBehaviors(Player player, PlayerSaveData playerSaveData)
		{
			if (player == null)
			{
				return;
			}
			InputBehavior[] inputBehaviors = playerSaveData.inputBehaviors;
			for (int i = 0; i < inputBehaviors.Length; i++)
			{
				this.SaveInputBehaviorNow(player, inputBehaviors[i]);
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000068F8 File Offset: 0x00004AF8
		private void SaveInputBehaviorNow(int playerId, int behaviorId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			this.SaveInputBehaviorNow(player, inputBehavior);
			this.dataStore.Save();
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000693C File Offset: 0x00004B3C
		private void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return;
			}
			string inputBehaviorKey = this.GetInputBehaviorKey(player, inputBehavior.id);
			this.dataStore.SetValue(inputBehaviorKey, inputBehavior.ToJsonString());
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00006974 File Offset: 0x00004B74
		private bool SaveControllerAssignments()
		{
			try
			{
				UserDataStore_KeyValue.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount);
				for (int i = 0; i < ReInput.players.allPlayerCount; i++)
				{
					Player player = ReInput.players.AllPlayers[i];
					UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo();
					controllerAssignmentSaveInfo.players[i] = playerInfo;
					playerInfo.id = player.id;
					playerInfo.hasKeyboard = player.controllers.hasKeyboard;
					playerInfo.hasMouse = player.controllers.hasMouse;
					UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo[] array = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo[player.controllers.joystickCount];
					playerInfo.joysticks = array;
					for (int j = 0; j < player.controllers.joystickCount; j++)
					{
						Joystick joystick = player.controllers.Joysticks[j];
						array[j] = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo
						{
							instanceGuid = joystick.deviceInstanceGuid,
							id = joystick.id,
							hardwareIdentifier = joystick.hardwareIdentifier
						};
					}
				}
				this.dataStore.SetValue("ControllerAssignments", JsonWriter.ToJson(controllerAssignmentSaveInfo));
				this.dataStore.Save();
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00006AC0 File Offset: 0x00004CC0
		private static void AppendPlayerKey(StringBuilder sb, Player player)
		{
			sb.Append("playerId=");
			sb.Append(player.id);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00006ADC File Offset: 0x00004CDC
		private string GetControllerMapKey(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			this._sb.Length = 0;
			UserDataStore_KeyValue.AppendPlayerKey(this._sb, player);
			this._sb.Append("|dataType=ControllerMap");
			UserDataStore_KeyValue.AppendControllerMapKeyCommonSuffix(this._sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
			return this._sb.ToString();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00006B30 File Offset: 0x00004D30
		private string GetControllerMapKnownActionIdsKey(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			this._sb.Length = 0;
			UserDataStore_KeyValue.AppendPlayerKey(this._sb, player);
			this._sb.Append("|dataType=ControllerMap_KnownActionIds");
			UserDataStore_KeyValue.AppendControllerMapKeyCommonSuffix(this._sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
			return this._sb.ToString();
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00006B84 File Offset: 0x00004D84
		private static void AppendControllerMapKeyCommonSuffix(StringBuilder sb, Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int keyVersion)
		{
			sb.Append("|kv=");
			sb.Append(keyVersion);
			sb.Append("|controllerMapType=");
			sb.Append((int)controllerIdentifier.controllerType);
			sb.Append("|categoryId=");
			sb.Append(categoryId);
			sb.Append("|");
			sb.Append("layoutId=");
			sb.Append(layoutId);
			sb.Append("|hardwareGuid=");
			sb.Append(controllerIdentifier.hardwareTypeGuid);
			if (controllerIdentifier.hardwareTypeGuid == Guid.Empty)
			{
				sb.Append("|hardwareIdentifier=");
				sb.Append(controllerIdentifier.hardwareIdentifier);
			}
			if (controllerIdentifier.controllerType == ControllerType.Joystick)
			{
				sb.Append("|duplicate=");
				sb.Append(UserDataStore_KeyValue.GetDuplicateIndex(player, controllerIdentifier).ToString());
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00006C70 File Offset: 0x00004E70
		private static void AppendControllerElementByRoleMapKey(StringBuilder sb, string elementRole, int categoryId, int layoutId, int keyVersion)
		{
			sb.Append("|dataType=ElementRoleMap");
			sb.Append("|kv=");
			sb.Append(keyVersion);
			sb.Append("|categoryId=");
			sb.Append(categoryId);
			sb.Append("|layoutId=");
			sb.Append(layoutId);
			sb.Append("|role=");
			sb.Append(elementRole);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00006CDC File Offset: 0x00004EDC
		private string GetJoystickCalibrationMapKey(Joystick joystick)
		{
			this._sb.Length = 0;
			this._sb.Append("dataType=CalibrationMap");
			this._sb.Append("|controllerType=");
			this._sb.Append((int)joystick.type);
			this._sb.Append("|hardwareIdentifier=");
			this._sb.Append(joystick.hardwareIdentifier);
			this._sb.Append("|hardwareGuid=");
			this._sb.Append(joystick.hardwareTypeGuid.ToString());
			return this._sb.ToString();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00006D88 File Offset: 0x00004F88
		private string GetInputBehaviorKey(Player player, int inputBehaviorId)
		{
			this._sb.Length = 0;
			UserDataStore_KeyValue.AppendPlayerKey(this._sb, player);
			this._sb.Append("|dataType=InputBehavior");
			this._sb.Append("|id=");
			this._sb.Append(inputBehaviorId);
			return this._sb.ToString();
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00006DE8 File Offset: 0x00004FE8
		private string GetControllerMapJson(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			for (int i = 0; i >= 0; i--)
			{
				string controllerMapKey = this.GetControllerMapKey(player, controllerIdentifier, categoryId, layoutId, i);
				string text;
				if (UserDataStore_KeyValue.TryGetString(this.dataStore, controllerMapKey, out text) && !string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00006E2C File Offset: 0x0000502C
		private List<int> GetControllerMapKnownActionIds(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			List<int> list = new List<int>();
			string text = null;
			bool flag = false;
			for (int i = 0; i >= 0; i--)
			{
				string controllerMapKnownActionIdsKey = this.GetControllerMapKnownActionIdsKey(player, controllerIdentifier, categoryId, layoutId, i);
				if (UserDataStore_KeyValue.TryGetString(this.dataStore, controllerMapKnownActionIdsKey, out text))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return list;
			}
			if (string.IsNullOrEmpty(text))
			{
				return list;
			}
			string[] array = text.Split(',', StringSplitOptions.None);
			for (int j = 0; j < array.Length; j++)
			{
				int num;
				if (!string.IsNullOrEmpty(array[j]) && int.TryParse(array[j], out num))
				{
					list.Add(num);
				}
			}
			return list;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00006EC8 File Offset: 0x000050C8
		private string GetJoystickCalibrationMapJson(Joystick joystick)
		{
			string joystickCalibrationMapKey = this.GetJoystickCalibrationMapKey(joystick);
			string text;
			UserDataStore_KeyValue.TryGetString(this.dataStore, joystickCalibrationMapKey, out text);
			return text;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00006EF0 File Offset: 0x000050F0
		private string GetInputBehaviorJson(Player player, int id)
		{
			string inputBehaviorKey = this.GetInputBehaviorKey(player, id);
			string text;
			UserDataStore_KeyValue.TryGetString(this.dataStore, inputBehaviorKey, out text);
			return text;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00006F18 File Offset: 0x00005118
		private void AddDefaultMappingsForNewActions(ControllerIdentifier controllerIdentifier, ControllerMap controllerMap, List<int> knownActionIds)
		{
			if (controllerMap == null || knownActionIds == null)
			{
				return;
			}
			if (knownActionIds == null || knownActionIds.Count == 0)
			{
				return;
			}
			ControllerMap controllerMapInstance = ReInput.mapping.GetControllerMapInstance(controllerIdentifier, controllerMap.categoryId, controllerMap.layoutId);
			if (controllerMapInstance == null)
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (int num in this.allActionIds)
			{
				if (!knownActionIds.Contains(num))
				{
					list.Add(num);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			bool flag = false;
			foreach (ActionElementMap actionElementMap in controllerMapInstance.AllMaps)
			{
				if (list.Contains(actionElementMap.actionId) && !controllerMap.DoesElementAssignmentConflict(actionElementMap))
				{
					ElementAssignment elementAssignment = new ElementAssignment(controllerMap.controllerType, actionElementMap.elementType, actionElementMap.elementIdentifierId, actionElementMap.axisRange, actionElementMap.keyCode, actionElementMap.modifierKeyFlags, actionElementMap.actionId, actionElementMap.axisContribution, actionElementMap.invert);
					controllerMap.CreateElementMap(elementAssignment);
				}
			}
			if (flag)
			{
				controllerMap.isModified = false;
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00007064 File Offset: 0x00005264
		private Joystick FindJoystickPrecise(UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo)
		{
			if (joystickInfo == null)
			{
				return null;
			}
			if (joystickInfo.instanceGuid == Guid.Empty)
			{
				return null;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (joysticks[i].deviceInstanceGuid == joystickInfo.instanceGuid)
				{
					return joysticks[i];
				}
			}
			return null;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000070C8 File Offset: 0x000052C8
		private bool TryFindJoysticksImprecise(UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo, out List<Joystick> matches)
		{
			matches = null;
			if (joystickInfo == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(joystickInfo.hardwareIdentifier))
			{
				return false;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (string.Equals(joysticks[i].hardwareIdentifier, joystickInfo.hardwareIdentifier, StringComparison.OrdinalIgnoreCase))
				{
					if (matches == null)
					{
						matches = new List<Joystick>();
					}
					matches.Add(joysticks[i]);
				}
			}
			return matches != null;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00007140 File Offset: 0x00005340
		private void RefreshLayoutManager(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			player.controllers.maps.layoutManager.Apply();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00007174 File Offset: 0x00005374
		private void OnControllerMapsSaved(Player player)
		{
			if (this._actionMappingSaveMode == UserDataStore_KeyValue.ActionMappingSaveMode.ByControllerElementRole)
			{
				int joystickCount = player.controllers.joystickCount;
				if (joystickCount > 1)
				{
					for (int i = 0; i < joystickCount; i++)
					{
						this.LoadControllerMaps(player.id, ControllerType.Joystick, player.controllers.Joysticks[i].id);
					}
					this.RefreshLayoutManager(player.id);
				}
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000071D8 File Offset: 0x000053D8
		private static int GetDuplicateIndex(Player player, ControllerIdentifier controllerIdentifier)
		{
			Controller controller = ReInput.controllers.GetController(controllerIdentifier);
			if (controller == null)
			{
				return 0;
			}
			int num = 0;
			foreach (Controller controller2 in player.controllers.Controllers)
			{
				if (controller2.type == controller.type)
				{
					bool flag = false;
					if (controller.type == ControllerType.Joystick)
					{
						if ((controller2 as Joystick).hardwareTypeGuid != controller.hardwareTypeGuid)
						{
							continue;
						}
						if (controller.hardwareTypeGuid != Guid.Empty)
						{
							flag = true;
						}
					}
					if (flag || !(controller2.hardwareIdentifier != controller.hardwareIdentifier))
					{
						if (controller2 == controller)
						{
							return num;
						}
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000072A8 File Offset: 0x000054A8
		private static bool TryGetString(UserDataStore_KeyValue.IDataStore store, string key, out string result)
		{
			if (store == null || string.IsNullOrEmpty(key))
			{
				result = null;
				return false;
			}
			object obj;
			if (!store.TryGetValue(key, out obj))
			{
				result = null;
				return false;
			}
			result = obj as string;
			return obj is string;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000072E8 File Offset: 0x000054E8
		private static int SortOldestToNewest(ControllerMapSaveData a, ControllerMapSaveData b)
		{
			if (a.map == null)
			{
				if (b.map == null)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (b.map == null)
				{
					return 1;
				}
				return a.map.modifiedTime.CompareTo(b.map.modifiedTime);
			}
		}

		// Token: 0x04000242 RID: 578
		private static readonly string thisScriptName = typeof(UserDataStore_KeyValue).Name;

		// Token: 0x04000243 RID: 579
		private const string logPrefix = "Rewired: ";

		// Token: 0x04000244 RID: 580
		private const string key_controllerAssignments = "ControllerAssignments";

		// Token: 0x04000245 RID: 581
		private const int controllerMapKeyVersion = 0;

		// Token: 0x04000246 RID: 582
		private const int controllerElementByRoleMapKeyVersion = 0;

		// Token: 0x04000247 RID: 583
		[Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
		[SerializeField]
		private bool _isEnabled = true;

		// Token: 0x04000248 RID: 584
		[Tooltip("Should saved data be loaded on start?")]
		[SerializeField]
		private bool _loadDataOnStart = true;

		// Token: 0x04000249 RID: 585
		[Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")]
		[SerializeField]
		private bool _loadJoystickAssignments = true;

		// Token: 0x0400024A RID: 586
		[Tooltip("Should Player Keyboard assignments be saved and loaded?")]
		[SerializeField]
		private bool _loadKeyboardAssignments = true;

		// Token: 0x0400024B RID: 587
		[Tooltip("Should Player Mouse assignments be saved and loaded?")]
		[SerializeField]
		private bool _loadMouseAssignments = true;

		// Token: 0x0400024C RID: 588
		[Tooltip("How should Action mapping data be saved?\n\nBy Controller: Data is stored per-controller. Action mappings apply only to the specific controller for which it was saved.\n\nBy Controller Element Role: Data is stored per-element on the controller if the controller element has a known role. Action mappings are mirrored on controller elements with the same role on all other controllers for the Player. Example: When saving Action mappings for a gamepad, element on all gamepads that have the same roles will inherit the mappings. This allows you to remap once for all compatible gamepads simultaneously, for example. This can extend beyond just gamepads, however. For example: On a console platform, a racing wheel with A, B, X, Y, D-Pad etc. elements will also reflect the same Action mappings if the gamepad is remapped. Action mappings for any controller elements that do not have known roles will be saved per-controller. Warning: Do not use this mode if you need to allow a Player to save different mappings for multiple controllers of the same type such as gamepads. (This option currently works best for gamepads and only miminally for other controller types.)")]
		[SerializeField]
		private UserDataStore_KeyValue.ActionMappingSaveMode _actionMappingSaveMode;

		// Token: 0x0400024D RID: 589
		[NonSerialized]
		private bool _allowImpreciseJoystickAssignmentMatching = true;

		// Token: 0x0400024E RID: 590
		[NonSerialized]
		private bool _deferredJoystickAssignmentLoadPending;

		// Token: 0x0400024F RID: 591
		[NonSerialized]
		private bool _wasJoystickEverDetected;

		// Token: 0x04000250 RID: 592
		[NonSerialized]
		private List<int> __allActionIds;

		// Token: 0x04000251 RID: 593
		[NonSerialized]
		private string __allActionIdsString;

		// Token: 0x04000252 RID: 594
		[NonSerialized]
		private readonly StringBuilder _sb = new StringBuilder();

		// Token: 0x04000253 RID: 595
		[NonSerialized]
		private Dictionary<string, UserDataStore_KeyValue.ControllerElementByRoleMap> _tempElementByRoleMaps;

		// Token: 0x04000254 RID: 596
		[NonSerialized]
		private Dictionary<string, bool> _tempElementByRoleMapsEnabled;

		// Token: 0x02000034 RID: 52
		private class ControllerAssignmentSaveInfo
		{
			// Token: 0x1700024E RID: 590
			// (get) Token: 0x060003E2 RID: 994 RVA: 0x00007384 File Offset: 0x00005584
			public int playerCount
			{
				get
				{
					if (this.players == null)
					{
						return 0;
					}
					return this.players.Length;
				}
			}

			// Token: 0x060003E3 RID: 995 RVA: 0x000021D7 File Offset: 0x000003D7
			public ControllerAssignmentSaveInfo()
			{
			}

			// Token: 0x060003E4 RID: 996 RVA: 0x00007398 File Offset: 0x00005598
			public ControllerAssignmentSaveInfo(int playerCount)
			{
				this.players = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo[playerCount];
				for (int i = 0; i < playerCount; i++)
				{
					this.players[i] = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo();
				}
			}

			// Token: 0x060003E5 RID: 997 RVA: 0x000073D0 File Offset: 0x000055D0
			public int IndexOfPlayer(int playerId)
			{
				for (int i = 0; i < this.playerCount; i++)
				{
					if (this.players[i] != null && this.players[i].id == playerId)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060003E6 RID: 998 RVA: 0x0000740B File Offset: 0x0000560B
			public bool ContainsPlayer(int playerId)
			{
				return this.IndexOfPlayer(playerId) >= 0;
			}

			// Token: 0x04000255 RID: 597
			public UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo[] players;

			// Token: 0x02000035 RID: 53
			public class PlayerInfo
			{
				// Token: 0x1700024F RID: 591
				// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000741A File Offset: 0x0000561A
				public int joystickCount
				{
					get
					{
						if (this.joysticks == null)
						{
							return 0;
						}
						return this.joysticks.Length;
					}
				}

				// Token: 0x060003E8 RID: 1000 RVA: 0x00007430 File Offset: 0x00005630
				public int IndexOfJoystick(int joystickId)
				{
					for (int i = 0; i < this.joystickCount; i++)
					{
						if (this.joysticks[i] != null && this.joysticks[i].id == joystickId)
						{
							return i;
						}
					}
					return -1;
				}

				// Token: 0x060003E9 RID: 1001 RVA: 0x0000746B File Offset: 0x0000566B
				public bool ContainsJoystick(int joystickId)
				{
					return this.IndexOfJoystick(joystickId) >= 0;
				}

				// Token: 0x04000256 RID: 598
				public int id;

				// Token: 0x04000257 RID: 599
				public bool hasKeyboard;

				// Token: 0x04000258 RID: 600
				public bool hasMouse;

				// Token: 0x04000259 RID: 601
				public UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo[] joysticks;
			}

			// Token: 0x02000036 RID: 54
			public class JoystickInfo
			{
				// Token: 0x0400025A RID: 602
				public Guid instanceGuid;

				// Token: 0x0400025B RID: 603
				public string hardwareIdentifier;

				// Token: 0x0400025C RID: 604
				public int id;
			}
		}

		// Token: 0x02000037 RID: 55
		private class JoystickAssignmentHistoryInfo
		{
			// Token: 0x060003EC RID: 1004 RVA: 0x0000747A File Offset: 0x0000567A
			public JoystickAssignmentHistoryInfo(Joystick joystick, int oldJoystickId)
			{
				if (joystick == null)
				{
					throw new ArgumentNullException("joystick");
				}
				this.joystick = joystick;
				this.oldJoystickId = oldJoystickId;
			}

			// Token: 0x0400025D RID: 605
			public readonly Joystick joystick;

			// Token: 0x0400025E RID: 606
			public readonly int oldJoystickId;
		}

		// Token: 0x02000038 RID: 56
		protected interface IDataStore
		{
			// Token: 0x060003ED RID: 1005
			bool Save();

			// Token: 0x060003EE RID: 1006
			bool Load();

			// Token: 0x060003EF RID: 1007
			bool Clear();

			// Token: 0x060003F0 RID: 1008
			bool TryGetValue(string key, out object result);

			// Token: 0x060003F1 RID: 1009
			bool SetValue(string key, object value);
		}

		// Token: 0x02000039 RID: 57
		[Serializable]
		protected class ControllerElementByRoleMap
		{
			// Token: 0x060003F2 RID: 1010 RVA: 0x0000749E File Offset: 0x0000569E
			[Preserve]
			public ControllerElementByRoleMap()
			{
				this.data = new List<UserDataStore_KeyValue.ControllerElementByRoleMap.Entry>();
			}

			// Token: 0x060003F3 RID: 1011 RVA: 0x000074B4 File Offset: 0x000056B4
			public void Add(ActionElementMap elementMap)
			{
				this.data.Add(new UserDataStore_KeyValue.ControllerElementByRoleMap.Entry
				{
					actionId = elementMap.actionId,
					elementType = elementMap.elementType,
					axisRange = elementMap.axisRange,
					invert = elementMap.invert,
					axisContribution = elementMap.axisContribution
				});
			}

			// Token: 0x060003F4 RID: 1012 RVA: 0x00007518 File Offset: 0x00005718
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("role: ");
				stringBuilder.Append(this.role);
				stringBuilder.Append("\nentries:");
				stringBuilder.Append((this.data != null) ? this.data.Count : 0);
				stringBuilder.Append("\n");
				if (this.data != null)
				{
					for (int i = 0; i < this.data.Count; i++)
					{
						stringBuilder.Append("Entry[");
						stringBuilder.Append(i);
						stringBuilder.Append("]:\n");
						stringBuilder.Append(this.data[i]);
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060003F5 RID: 1013 RVA: 0x000075D5 File Offset: 0x000057D5
			public string ToJson()
			{
				return JsonWriter.ToJson(this);
			}

			// Token: 0x060003F6 RID: 1014 RVA: 0x000075E0 File Offset: 0x000057E0
			public static UserDataStore_KeyValue.ControllerElementByRoleMap FromJson(string role, string json)
			{
				UserDataStore_KeyValue.ControllerElementByRoleMap controllerElementByRoleMap = JsonParser.FromJson<UserDataStore_KeyValue.ControllerElementByRoleMap>(json);
				if (controllerElementByRoleMap != null)
				{
					controllerElementByRoleMap.role = role;
				}
				return controllerElementByRoleMap;
			}

			// Token: 0x0400025F RID: 607
			[DoNotSerialize]
			public string role;

			// Token: 0x04000260 RID: 608
			public List<UserDataStore_KeyValue.ControllerElementByRoleMap.Entry> data;

			// Token: 0x0200003A RID: 58
			[Serializable]
			public struct Entry
			{
				// Token: 0x060003F7 RID: 1015 RVA: 0x00007600 File Offset: 0x00005800
				public bool TryGetElementAssignment(ControllerType controllerType, Controller.Element targetElement, out ElementAssignment assignment)
				{
					if (targetElement.type == this.elementType)
					{
						assignment = ElementAssignment.CompleteAssignment(controllerType, targetElement.type, targetElement.elementIdentifier.id, this.axisRange, KeyCode.None, ModifierKeyFlags.None, this.actionId, this.axisContribution, this.invert);
						return true;
					}
					ControllerElementType controllerElementType = this.elementType;
					if (controllerElementType != ControllerElementType.Axis)
					{
						if (controllerElementType != ControllerElementType.Button)
						{
							assignment = default(ElementAssignment);
							return false;
						}
						if (targetElement.type == ControllerElementType.Axis)
						{
							assignment = ElementAssignment.CompleteAssignment(controllerType, targetElement.type, targetElement.elementIdentifier.id, AxisRange.Positive, KeyCode.None, ModifierKeyFlags.None, this.actionId, this.axisContribution, false);
							return true;
						}
						assignment = default(ElementAssignment);
						return false;
					}
					else
					{
						if (targetElement.type == ControllerElementType.Button)
						{
							Pole pole = this.axisContribution;
							if (this.axisRange == AxisRange.Full && this.invert)
							{
								pole = Pole.Negative;
							}
							assignment = ElementAssignment.CompleteAssignment(controllerType, targetElement.type, targetElement.elementIdentifier.id, AxisRange.Full, KeyCode.None, ModifierKeyFlags.None, this.actionId, pole, false);
							return true;
						}
						assignment = default(ElementAssignment);
						return false;
					}
				}

				// Token: 0x060003F8 RID: 1016 RVA: 0x00007704 File Offset: 0x00005904
				public override string ToString()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("actionId: ");
					stringBuilder.Append(this.actionId);
					stringBuilder.Append("\nelementType: ");
					stringBuilder.Append(this.elementType);
					stringBuilder.Append("\naxisRange: ");
					stringBuilder.Append(this.axisRange);
					stringBuilder.Append("\ninvert: ");
					stringBuilder.Append(this.invert);
					stringBuilder.Append("\naxisContribution: ");
					stringBuilder.Append(this.axisContribution);
					return stringBuilder.ToString();
				}

				// Token: 0x04000261 RID: 609
				public int actionId;

				// Token: 0x04000262 RID: 610
				public ControllerElementType elementType;

				// Token: 0x04000263 RID: 611
				public AxisRange axisRange;

				// Token: 0x04000264 RID: 612
				public bool invert;

				// Token: 0x04000265 RID: 613
				public Pole axisContribution;
			}
		}

		// Token: 0x0200003B RID: 59
		public enum ActionMappingSaveMode
		{
			// Token: 0x04000267 RID: 615
			ByController,
			// Token: 0x04000268 RID: 616
			ByControllerElementRole
		}
	}
}
