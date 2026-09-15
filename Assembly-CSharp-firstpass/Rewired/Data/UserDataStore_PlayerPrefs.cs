using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rewired.Utils.Attributes;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	// Token: 0x02000040 RID: 64
	public class UserDataStore_PlayerPrefs : UserDataStore
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00007868 File Offset: 0x00005A68
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x00007870 File Offset: 0x00005A70
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				this.isEnabled = value;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00007879 File Offset: 0x00005A79
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00007881 File Offset: 0x00005A81
		public bool LoadDataOnStart
		{
			get
			{
				return this.loadDataOnStart;
			}
			set
			{
				this.loadDataOnStart = value;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000788A File Offset: 0x00005A8A
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00007892 File Offset: 0x00005A92
		public bool LoadJoystickAssignments
		{
			get
			{
				return this.loadJoystickAssignments;
			}
			set
			{
				this.loadJoystickAssignments = value;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000789B File Offset: 0x00005A9B
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x000078A3 File Offset: 0x00005AA3
		public bool LoadKeyboardAssignments
		{
			get
			{
				return this.loadKeyboardAssignments;
			}
			set
			{
				this.loadKeyboardAssignments = value;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000078AC File Offset: 0x00005AAC
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x000078B4 File Offset: 0x00005AB4
		public bool LoadMouseAssignments
		{
			get
			{
				return this.loadMouseAssignments;
			}
			set
			{
				this.loadMouseAssignments = value;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x000078BD File Offset: 0x00005ABD
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x000078C5 File Offset: 0x00005AC5
		public UserDataStore_PlayerPrefs.ActionMappingSaveMode actionMappingSaveMode
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

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x000078CE File Offset: 0x00005ACE
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x000078D6 File Offset: 0x00005AD6
		public string PlayerPrefsKeyPrefix
		{
			get
			{
				return this.playerPrefsKeyPrefix;
			}
			set
			{
				this.playerPrefsKeyPrefix = value;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x000078DF File Offset: 0x00005ADF
		private string playerPrefsKey_controllerAssignments
		{
			get
			{
				return string.Format("{0}_{1}", this.playerPrefsKeyPrefix, "ControllerAssignments");
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x000078F6 File Offset: 0x00005AF6
		private bool loadControllerAssignments
		{
			get
			{
				return this.loadKeyboardAssignments || this.loadMouseAssignments || this.loadJoystickAssignments;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00007910 File Offset: 0x00005B10
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

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00007968 File Offset: 0x00005B68
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

		// Token: 0x06000417 RID: 1047 RVA: 0x000079D7 File Offset: 0x00005BD7
		public override void Save()
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveAll();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000079F3 File Offset: 0x00005BF3
		public override void SaveControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00007A12 File Offset: 0x00005C12
		public override void SaveControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00007A30 File Offset: 0x00005C30
		public override void SavePlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SavePlayerDataNow(playerId);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00007A4D File Offset: 0x00005C4D
		public override void SaveInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00007A6B File Offset: 0x00005C6B
		public override void Load()
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadAll();
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00007A88 File Offset: 0x00005C88
		public override void LoadControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00007AA8 File Offset: 0x00005CA8
		public override void LoadControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00007AC7 File Offset: 0x00005CC7
		public override void LoadPlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadPlayerDataNow(playerId);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00007AE5 File Offset: 0x00005CE5
		public override void LoadInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00007B04 File Offset: 0x00005D04
		protected override void OnInitialize()
		{
			if (this.loadDataOnStart)
			{
				this.Load();
				if (this.loadControllerAssignments && ReInput.controllers.joystickCount > 0)
				{
					this.wasJoystickEverDetected = true;
					this.SaveControllerAssignments();
				}
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00007B38 File Offset: 0x00005D38
		protected override void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				this.LoadJoystickData(args.controllerId);
				if (this.loadDataOnStart && this.loadJoystickAssignments && !this.wasJoystickEverDetected)
				{
					base.StartCoroutine(this.LoadJoystickAssignmentsDeferred());
				}
				if (this.loadJoystickAssignments && !this.deferredJoystickAssignmentLoadPending)
				{
					this.SaveControllerAssignments();
				}
				this.wasJoystickEverDetected = true;
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00007BA7 File Offset: 0x00005DA7
		protected override void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickData(args.controllerId);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00007BC7 File Offset: 0x00005DC7
		protected override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00007BE4 File Offset: 0x00005DE4
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
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00007C10 File Offset: 0x00005E10
		public override ControllerMap LoadControllerMap(int playerId, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return null;
			}
			return this.LoadControllerMap(player, controllerIdentifier, categoryId, layoutId);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00007C3C File Offset: 0x00005E3C
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

		// Token: 0x06000428 RID: 1064 RVA: 0x00007C95 File Offset: 0x00005E95
		private int LoadPlayerDataNow(int playerId)
		{
			return this.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId));
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00007CA8 File Offset: 0x00005EA8
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

		// Token: 0x0600042A RID: 1066 RVA: 0x00007D50 File Offset: 0x00005F50
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

		// Token: 0x0600042B RID: 1067 RVA: 0x00007D8C File Offset: 0x00005F8C
		private int LoadJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return 0;
			}
			if (!joystick.ImportCalibrationMapFromXmlString(this.GetJoystickCalibrationMapXml(joystick)))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00007DA5 File Offset: 0x00005FA5
		private int LoadJoystickCalibrationData(int joystickId)
		{
			return this.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00007DB8 File Offset: 0x00005FB8
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

		// Token: 0x0600042E RID: 1070 RVA: 0x00007E22 File Offset: 0x00006022
		private int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0 + this.LoadControllerMaps(playerId, controllerType, controllerId);
			this.RefreshLayoutManager(playerId);
			return num + this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00007E40 File Offset: 0x00006040
		private int LoadControllerDataNow(ControllerType controllerType, int controllerId)
		{
			int num = 0;
			if (controllerType == ControllerType.Joystick)
			{
				num += this.LoadJoystickCalibrationData(controllerId);
			}
			return num;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00007E60 File Offset: 0x00006060
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
						UserDataStore_PlayerPrefs.ActionMappingSaveMode actionMappingSaveMode = this._actionMappingSaveMode;
						if (actionMappingSaveMode != UserDataStore_PlayerPrefs.ActionMappingSaveMode.ByController)
						{
							if (actionMappingSaveMode != UserDataStore_PlayerPrefs.ActionMappingSaveMode.ByControllerElementRole)
							{
								throw new NotImplementedException();
							}
							Dictionary<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap> dictionary = ((this._tempElementByRoleMaps != null) ? this._tempElementByRoleMaps : (this._tempElementByRoleMaps = new Dictionary<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap>()));
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
								foreach (KeyValuePair<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap> keyValuePair in dictionary)
								{
									UserDataStore_PlayerPrefs.ControllerElementByRoleMap value = keyValuePair.Value;
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

		// Token: 0x06000431 RID: 1073 RVA: 0x0000824C File Offset: 0x0000644C
		private ControllerMap LoadControllerMap(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			if (player == null)
			{
				return null;
			}
			string controllerMapXml = this.GetControllerMapXml(player, controllerIdentifier, categoryId, layoutId);
			if (string.IsNullOrEmpty(controllerMapXml))
			{
				return null;
			}
			ControllerMap controllerMap = ControllerMap.CreateFromXml(controllerIdentifier.controllerType, controllerMapXml);
			if (controllerMap == null)
			{
				return null;
			}
			List<int> controllerMapKnownActionIds = this.GetControllerMapKnownActionIds(player, controllerIdentifier, categoryId, layoutId);
			this.AddDefaultMappingsForNewActions(controllerIdentifier, controllerMap, controllerMapKnownActionIds);
			return controllerMap;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000082A0 File Offset: 0x000064A0
		private bool LoadControllerElementMapByRole(Player player, Controller controller, string role, int mapCategoryId, int layoutId, Dictionary<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap> elementByRoleMaps)
		{
			if (string.IsNullOrEmpty(role))
			{
				return false;
			}
			string controllerElementByRoleMapPlayerPrefsKey = this.GetControllerElementByRoleMapPlayerPrefsKey(player, role, mapCategoryId, layoutId, 0);
			bool flag;
			try
			{
				string @string;
				if (!PlayerPrefs.HasKey(controllerElementByRoleMapPlayerPrefsKey) || string.IsNullOrEmpty(@string = PlayerPrefs.GetString(controllerElementByRoleMapPlayerPrefsKey)))
				{
					flag = false;
				}
				else if (string.IsNullOrEmpty(@string))
				{
					flag = false;
				}
				else
				{
					UserDataStore_PlayerPrefs.ControllerElementByRoleMap controllerElementByRoleMap = UserDataStore_PlayerPrefs.ControllerElementByRoleMap.FromJson(role, @string);
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

		// Token: 0x06000433 RID: 1075 RVA: 0x00008320 File Offset: 0x00006520
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

		// Token: 0x06000434 RID: 1076 RVA: 0x00008374 File Offset: 0x00006574
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

		// Token: 0x06000435 RID: 1077 RVA: 0x000083AC File Offset: 0x000065AC
		private int LoadInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return 0;
			}
			string inputBehaviorXml = this.GetInputBehaviorXml(player, inputBehavior.id);
			if (inputBehaviorXml == null || inputBehaviorXml == string.Empty)
			{
				return 0;
			}
			if (!inputBehavior.ImportXmlString(inputBehaviorXml))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000083F0 File Offset: 0x000065F0
		private bool LoadControllerAssignmentsNow()
		{
			try
			{
				UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = this.LoadControllerAssignmentData();
				if (controllerAssignmentSaveInfo == null)
				{
					return false;
				}
				if (this.loadKeyboardAssignments || this.loadMouseAssignments)
				{
					this.LoadKeyboardAndMouseAssignmentsNow(controllerAssignmentSaveInfo);
				}
				if (this.loadJoystickAssignments)
				{
					this.LoadJoystickAssignmentsNow(controllerAssignmentSaveInfo);
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000844C File Offset: 0x0000664C
		private bool LoadKeyboardAndMouseAssignmentsNow(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
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
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player.id)];
						if (this.loadKeyboardAssignments)
						{
							player.controllers.hasKeyboard = playerInfo.hasKeyboard;
						}
						if (this.loadMouseAssignments)
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

		// Token: 0x06000438 RID: 1080 RVA: 0x00008514 File Offset: 0x00006714
		private bool LoadJoystickAssignmentsNow(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
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
				List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo> list = (this.loadJoystickAssignments ? new List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>() : null);
				foreach (Player player2 in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player2.id))
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player2.id)];
						for (int i = 0; i < playerInfo.joystickCount; i++)
						{
							UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo2 = playerInfo.joysticks[i];
							if (joystickInfo2 != null)
							{
								Joystick joystick = this.FindJoystickPrecise(joystickInfo2);
								if (joystick != null)
								{
									if (list.Find((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.joystick == joystick) == null)
									{
										list.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick, joystickInfo2.id));
									}
									player2.controllers.AddController(joystick, false);
								}
							}
						}
					}
				}
				if (this.allowImpreciseJoystickAssignmentMatching)
				{
					foreach (Player player3 in ReInput.players.AllPlayers)
					{
						if (data.ContainsPlayer(player3.id))
						{
							UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo2 = data.players[data.IndexOfPlayer(player3.id)];
							for (int j = 0; j < playerInfo2.joystickCount; j++)
							{
								UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = playerInfo2.joysticks[j];
								if (joystickInfo != null)
								{
									Joystick joystick2 = null;
									int num = list.FindIndex((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.oldJoystickId == joystickInfo.id);
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
												if (list.Find((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.joystick == match) == null)
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
										list.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick2, joystickInfo.id));
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

		// Token: 0x06000439 RID: 1081 RVA: 0x00008880 File Offset: 0x00006A80
		private UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo LoadControllerAssignmentData()
		{
			UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo;
			try
			{
				if (!PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments))
				{
					controllerAssignmentSaveInfo = null;
				}
				else
				{
					string @string = PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments);
					if (string.IsNullOrEmpty(@string))
					{
						controllerAssignmentSaveInfo = null;
					}
					else
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo2 = JsonParser.FromJson<UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo>(@string);
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
			}
			catch
			{
				controllerAssignmentSaveInfo = null;
			}
			return controllerAssignmentSaveInfo;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000088E8 File Offset: 0x00006AE8
		private IEnumerator LoadJoystickAssignmentsDeferred()
		{
			this.deferredJoystickAssignmentLoadPending = true;
			yield return new WaitForEndOfFrame();
			if (!ReInput.isReady)
			{
				yield break;
			}
			this.LoadJoystickAssignmentsNow(null);
			this.SaveControllerAssignments();
			this.deferredJoystickAssignmentLoadPending = false;
			yield break;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000088F8 File Offset: 0x00006AF8
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
			PlayerPrefs.Save();
			for (int j = 0; j < allPlayers.Count; j++)
			{
				this.OnControllerMapsSaved(allPlayers[j]);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00008968 File Offset: 0x00006B68
		private void SavePlayerDataNow(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			this.SavePlayerDataNow(player);
			PlayerPrefs.Save();
			this.OnControllerMapsSaved(player);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00008994 File Offset: 0x00006B94
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

		// Token: 0x0600043E RID: 1086 RVA: 0x000089C0 File Offset: 0x00006BC0
		private void SaveAllJoystickCalibrationData()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				this.SaveJoystickCalibrationData(joysticks[i]);
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000089F6 File Offset: 0x00006BF6
		private void SaveJoystickCalibrationData(int joystickId)
		{
			this.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00008A0C File Offset: 0x00006C0C
		private void SaveJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
			PlayerPrefs.SetString(this.GetJoystickCalibrationMapPlayerPrefsKey(joystick), calibrationMapSaveData.map.ToXmlString());
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00008A3C File Offset: 0x00006C3C
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

		// Token: 0x06000442 RID: 1090 RVA: 0x00008A91 File Offset: 0x00006C91
		private void SaveControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			this.SaveControllerMaps(playerId, controllerType, controllerId);
			this.SaveControllerDataNow(controllerType, controllerId);
			PlayerPrefs.Save();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00008AA9 File Offset: 0x00006CA9
		private void SaveControllerDataNow(ControllerType controllerType, int controllerId)
		{
			if (controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickCalibrationData(controllerId);
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00008ABC File Offset: 0x00006CBC
		private void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
		{
			List<ControllerMapSaveData> list = new List<ControllerMapSaveData>(playerSaveData.AllControllerMapSaveData);
			if (this._actionMappingSaveMode == UserDataStore_PlayerPrefs.ActionMappingSaveMode.ByControllerElementRole)
			{
				list.Sort(new Comparison<ControllerMapSaveData>(UserDataStore_PlayerPrefs.SortOldestToNewest));
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.SaveControllerMap(player, list[i].map);
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00008B18 File Offset: 0x00006D18
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
			if (this._actionMappingSaveMode == UserDataStore_PlayerPrefs.ActionMappingSaveMode.ByControllerElementRole)
			{
				List<ControllerMapSaveData> list = new List<ControllerMapSaveData>(mapSaveData);
				list.Sort(new Comparison<ControllerMapSaveData>(UserDataStore_PlayerPrefs.SortOldestToNewest));
				list.CopyTo(mapSaveData);
			}
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				this.SaveControllerMap(player, mapSaveData[i].map);
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00008BA4 File Offset: 0x00006DA4
		private void SaveControllerMap(Player player, ControllerMap controllerMap)
		{
			UserDataStore_PlayerPrefs.ActionMappingSaveMode actionMappingSaveMode = this._actionMappingSaveMode;
			if (actionMappingSaveMode == UserDataStore_PlayerPrefs.ActionMappingSaveMode.ByController)
			{
				this.SaveControllerMapByController(player, controllerMap);
				return;
			}
			if (actionMappingSaveMode != UserDataStore_PlayerPrefs.ActionMappingSaveMode.ByControllerElementRole)
			{
				throw new NotImplementedException();
			}
			this.SaveControllerMapByControllerElementRole(player, controllerMap.controller, controllerMap);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00008BE0 File Offset: 0x00006DE0
		private void SaveControllerMapByController(Player player, ControllerMap controllerMap)
		{
			PlayerPrefs.SetString(this.GetControllerMapPlayerPrefsKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, 2), controllerMap.ToXmlString());
			PlayerPrefs.SetString(this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, 2), this.allActionIdsString);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00008C44 File Offset: 0x00006E44
		private void SaveControllerMapByControllerElementRole(Player player, Controller controller, ControllerMap controllerMap)
		{
			if (controller == null)
			{
				return;
			}
			this.SaveControllerMapByController(player, controllerMap);
			IList<ActionElementMap> elementMaps = controllerMap.ElementMaps;
			Dictionary<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap> dictionary = null;
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
							dictionary = new Dictionary<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap>();
						}
						dictionary.Add(role, new UserDataStore_PlayerPrefs.ControllerElementByRoleMap
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
			foreach (KeyValuePair<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap> keyValuePair in dictionary)
			{
				PlayerPrefs.SetString(this.GetControllerElementByRoleMapPlayerPrefsKey(player, keyValuePair.Value.role, controllerMap.categoryId, controllerMap.layoutId, 0), keyValuePair.Value.ToJson());
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00008D94 File Offset: 0x00006F94
		private bool AddControllerElementByRoleMapEntry(Player player, Controller controller, ActionElementMap elementMap, ref Dictionary<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap> maps)
		{
			ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(elementMap.elementIdentifierId);
			if (elementIdentifierById == null || string.IsNullOrEmpty(elementIdentifierById.role))
			{
				return false;
			}
			if (maps == null)
			{
				maps = new Dictionary<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap>();
			}
			UserDataStore_PlayerPrefs.ControllerElementByRoleMap controllerElementByRoleMap;
			if (!maps.TryGetValue(elementIdentifierById.role, out controllerElementByRoleMap))
			{
				controllerElementByRoleMap = new UserDataStore_PlayerPrefs.ControllerElementByRoleMap();
				controllerElementByRoleMap.role = elementIdentifierById.role;
				maps.Add(elementIdentifierById.role, controllerElementByRoleMap);
			}
			controllerElementByRoleMap.Add(elementMap);
			return true;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00008E08 File Offset: 0x00007008
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

		// Token: 0x0600044B RID: 1099 RVA: 0x00008E3C File Offset: 0x0000703C
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
			PlayerPrefs.Save();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00008E77 File Offset: 0x00007077
		private void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return;
			}
			PlayerPrefs.SetString(this.GetInputBehaviorPlayerPrefsKey(player, inputBehavior.id), inputBehavior.ToXmlString());
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00008E98 File Offset: 0x00007098
		private bool SaveControllerAssignments()
		{
			try
			{
				UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount);
				for (int i = 0; i < ReInput.players.allPlayerCount; i++)
				{
					Player player = ReInput.players.AllPlayers[i];
					UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
					controllerAssignmentSaveInfo.players[i] = playerInfo;
					playerInfo.id = player.id;
					playerInfo.hasKeyboard = player.controllers.hasKeyboard;
					playerInfo.hasMouse = player.controllers.hasMouse;
					UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] array = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[player.controllers.joystickCount];
					playerInfo.joysticks = array;
					for (int j = 0; j < player.controllers.joystickCount; j++)
					{
						Joystick joystick = player.controllers.Joysticks[j];
						array[j] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo
						{
							instanceGuid = joystick.deviceInstanceGuid,
							id = joystick.id,
							hardwareIdentifier = joystick.hardwareIdentifier
						};
					}
				}
				PlayerPrefs.SetString(this.playerPrefsKey_controllerAssignments, JsonWriter.ToJson(controllerAssignmentSaveInfo));
				PlayerPrefs.Save();
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00008FD8 File Offset: 0x000071D8
		private bool ControllerAssignmentSaveDataExists()
		{
			return PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments) && !string.IsNullOrEmpty(PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments));
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00009000 File Offset: 0x00007200
		private string GetControllerMapPlayerPrefsKey(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			this._sb.Length = 0;
			UserDataStore_PlayerPrefs.AppendBaseKey(this._sb, this.playerPrefsKeyPrefix);
			UserDataStore_PlayerPrefs.AppendPlayerKey(this._sb, player);
			UserDataStore_PlayerPrefs.AppendControllerMapKey(this._sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
			return this._sb.ToString();
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00009054 File Offset: 0x00007254
		private string GetControllerElementByRoleMapPlayerPrefsKey(Player player, string elementRole, int categoryId, int layoutId, int ppKeyVersion)
		{
			this._sb.Length = 0;
			UserDataStore_PlayerPrefs.AppendBaseKey(this._sb, this.playerPrefsKeyPrefix);
			UserDataStore_PlayerPrefs.AppendPlayerKey(this._sb, player);
			UserDataStore_PlayerPrefs.AppendControllerElementByRoleMapKey(this._sb, elementRole, categoryId, layoutId, ppKeyVersion);
			return this._sb.ToString();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000090A6 File Offset: 0x000072A6
		private string GetJoystickCalibrationMapPlayerPrefsKey(Joystick joystick)
		{
			this._sb.Length = 0;
			UserDataStore_PlayerPrefs.AppendBaseKey(this._sb, this.playerPrefsKeyPrefix);
			UserDataStore_PlayerPrefs.AppendJoystickCalibrationMapKey(this._sb, joystick);
			return this._sb.ToString();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000090DC File Offset: 0x000072DC
		private string GetControllerMapKnownActionIdsPlayerPrefsKey(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			this._sb.Length = 0;
			UserDataStore_PlayerPrefs.AppendBaseKey(this._sb, this.playerPrefsKeyPrefix);
			UserDataStore_PlayerPrefs.AppendPlayerKey(this._sb, player);
			UserDataStore_PlayerPrefs.AppendControllerMapKnownActionIdsKey(this._sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
			return this._sb.ToString();
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00009130 File Offset: 0x00007330
		private string GetInputBehaviorPlayerPrefsKey(Player player, int inputBehaviorId)
		{
			this._sb.Length = 0;
			UserDataStore_PlayerPrefs.AppendBaseKey(this._sb, this.playerPrefsKeyPrefix);
			UserDataStore_PlayerPrefs.AppendPlayerKey(this._sb, player);
			UserDataStore_PlayerPrefs.AppendInputBehaviorKey(this._sb, inputBehaviorId);
			return this._sb.ToString();
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000917D File Offset: 0x0000737D
		private static void AppendBaseKey(StringBuilder sb, string playerPrefsKeyPrefix)
		{
			sb.Append(playerPrefsKeyPrefix);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00009187 File Offset: 0x00007387
		private static void AppendPlayerKey(StringBuilder sb, Player player)
		{
			sb.Append("|playerName=");
			sb.Append(player.name);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000091A2 File Offset: 0x000073A2
		private static void AppendControllerMapKey(StringBuilder sb, Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			sb.Append("|dataType=ControllerMap");
			UserDataStore_PlayerPrefs.AppendControllerMapKeyCommonSuffix(sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000091BD File Offset: 0x000073BD
		private static void AppendControllerMapKnownActionIdsKey(StringBuilder sb, Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			sb.Append("|dataType=ControllerMap_KnownActionIds");
			UserDataStore_PlayerPrefs.AppendControllerMapKeyCommonSuffix(sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000091D8 File Offset: 0x000073D8
		private static void AppendControllerMapKeyCommonSuffix(StringBuilder sb, Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			if (ppKeyVersion >= 2)
			{
				sb.Append("|kv=");
				sb.Append(ppKeyVersion);
			}
			sb.Append("|controllerMapType=");
			sb.Append(UserDataStore_PlayerPrefs.GetControllerMapType(controllerIdentifier.controllerType).Name);
			sb.Append("|categoryId=");
			sb.Append(categoryId);
			sb.Append("|layoutId=");
			sb.Append(layoutId);
			if (ppKeyVersion >= 2)
			{
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
					sb.Append(UserDataStore_PlayerPrefs.GetDuplicateIndex(player, controllerIdentifier));
					return;
				}
			}
			else
			{
				sb.Append("|hardwareIdentifier=");
				sb.Append(controllerIdentifier.hardwareIdentifier);
				if (controllerIdentifier.controllerType == ControllerType.Joystick)
				{
					sb.Append("|hardwareGuid=");
					sb.Append(controllerIdentifier.hardwareTypeGuid);
					if (ppKeyVersion >= 1)
					{
						sb.Append("|duplicate=");
						sb.Append(UserDataStore_PlayerPrefs.GetDuplicateIndex(player, controllerIdentifier));
					}
				}
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00009324 File Offset: 0x00007524
		private static void AppendControllerElementByRoleMapKey(StringBuilder sb, string elementRole, int categoryId, int layoutId, int ppKeyVersion)
		{
			sb.Append("|dataType=ElementRoleMap");
			sb.Append("|kv=");
			sb.Append(ppKeyVersion);
			sb.Append("|categoryId=");
			sb.Append(categoryId);
			sb.Append("|layoutId=");
			sb.Append(layoutId);
			sb.Append("|role=");
			sb.Append(elementRole);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00009390 File Offset: 0x00007590
		private static void AppendJoystickCalibrationMapKey(StringBuilder sb, Joystick joystick)
		{
			sb.Append("|dataType=CalibrationMap");
			sb.Append("|controllerType=");
			sb.Append(joystick.type.ToString());
			sb.Append("|hardwareIdentifier=");
			sb.Append(joystick.hardwareIdentifier);
			sb.Append("|hardwareGuid=");
			sb.Append(joystick.hardwareTypeGuid.ToString());
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00009410 File Offset: 0x00007610
		private static void AppendInputBehaviorKey(StringBuilder sb, int inputBehaviorId)
		{
			sb.Append("|dataType=InputBehavior");
			sb.Append("|id=");
			sb.Append(inputBehaviorId);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00009434 File Offset: 0x00007634
		private string GetControllerMapXml(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			for (int i = 2; i >= 0; i--)
			{
				string controllerMapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controllerIdentifier, categoryId, layoutId, i);
				if (PlayerPrefs.HasKey(controllerMapPlayerPrefsKey))
				{
					return PlayerPrefs.GetString(controllerMapPlayerPrefsKey);
				}
			}
			return null;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000946C File Offset: 0x0000766C
		private List<int> GetControllerMapKnownActionIds(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			List<int> list = new List<int>();
			string text = null;
			bool flag = false;
			for (int i = 2; i >= 0; i--)
			{
				text = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controllerIdentifier, categoryId, layoutId, i);
				if (PlayerPrefs.HasKey(text))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return list;
			}
			string @string = PlayerPrefs.GetString(text);
			if (string.IsNullOrEmpty(@string))
			{
				return list;
			}
			string[] array = @string.Split(',', StringSplitOptions.None);
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

		// Token: 0x0600045E RID: 1118 RVA: 0x00009504 File Offset: 0x00007704
		private string GetJoystickCalibrationMapXml(Joystick joystick)
		{
			string joystickCalibrationMapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(joystick);
			if (!PlayerPrefs.HasKey(joystickCalibrationMapPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(joystickCalibrationMapPlayerPrefsKey);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00009530 File Offset: 0x00007730
		private string GetInputBehaviorXml(Player player, int id)
		{
			string inputBehaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, id);
			if (!PlayerPrefs.HasKey(inputBehaviorPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(inputBehaviorPlayerPrefsKey);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000955C File Offset: 0x0000775C
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
					flag = true;
				}
			}
			if (flag)
			{
				controllerMap.isModified = false;
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000096AC File Offset: 0x000078AC
		private Joystick FindJoystickPrecise(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo)
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

		// Token: 0x06000462 RID: 1122 RVA: 0x00009710 File Offset: 0x00007910
		private bool TryFindJoysticksImprecise(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo, out List<Joystick> matches)
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

		// Token: 0x06000463 RID: 1123 RVA: 0x00009788 File Offset: 0x00007988
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

		// Token: 0x06000464 RID: 1124 RVA: 0x00009858 File Offset: 0x00007A58
		private void RefreshLayoutManager(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			player.controllers.maps.layoutManager.Apply();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000988C File Offset: 0x00007A8C
		private void OnControllerMapsSaved(Player player)
		{
			if (this._actionMappingSaveMode == UserDataStore_PlayerPrefs.ActionMappingSaveMode.ByControllerElementRole)
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

		// Token: 0x06000466 RID: 1126 RVA: 0x000098F0 File Offset: 0x00007AF0
		private static Type GetControllerMapType(ControllerType controllerType)
		{
			switch (controllerType)
			{
			case ControllerType.Keyboard:
				return typeof(KeyboardMap);
			case ControllerType.Mouse:
				return typeof(MouseMap);
			case ControllerType.Joystick:
				return typeof(JoystickMap);
			default:
				if (controllerType == ControllerType.Custom)
				{
					return typeof(CustomControllerMap);
				}
				Debug.LogWarning("Rewired: Unknown ControllerType " + controllerType.ToString());
				return null;
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00009960 File Offset: 0x00007B60
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

		// Token: 0x0400026F RID: 623
		private const string thisScriptName = "UserDataStore_PlayerPrefs";

		// Token: 0x04000270 RID: 624
		private const string logPrefix = "Rewired: ";

		// Token: 0x04000271 RID: 625
		private const string playerPrefsKeySuffix_controllerAssignments = "ControllerAssignments";

		// Token: 0x04000272 RID: 626
		private const int controllerMapPPKeyVersion_original = 0;

		// Token: 0x04000273 RID: 627
		private const int controllerMapPPKeyVersion_includeDuplicateJoystickIndex = 1;

		// Token: 0x04000274 RID: 628
		private const int controllerMapPPKeyVersion_supportDisconnectedControllers = 2;

		// Token: 0x04000275 RID: 629
		private const int controllerMapPPKeyVersion_includeFormatVersion = 2;

		// Token: 0x04000276 RID: 630
		private const int controllerMapPPKeyVersion = 2;

		// Token: 0x04000277 RID: 631
		private const int controllerElementByRoleMapPPKeyVersion = 0;

		// Token: 0x04000278 RID: 632
		[Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
		[SerializeField]
		private bool isEnabled = true;

		// Token: 0x04000279 RID: 633
		[Tooltip("Should saved data be loaded on start?")]
		[SerializeField]
		private bool loadDataOnStart = true;

		// Token: 0x0400027A RID: 634
		[Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")]
		[SerializeField]
		private bool loadJoystickAssignments = true;

		// Token: 0x0400027B RID: 635
		[Tooltip("Should Player Keyboard assignments be saved and loaded?")]
		[SerializeField]
		private bool loadKeyboardAssignments = true;

		// Token: 0x0400027C RID: 636
		[Tooltip("Should Player Mouse assignments be saved and loaded?")]
		[SerializeField]
		private bool loadMouseAssignments = true;

		// Token: 0x0400027D RID: 637
		[Tooltip("How should Action mapping data be saved?\n\nBy Controller: Data is stored per-controller. Action mappings apply only to the specific controller for which it was saved.\n\nBy Controller Element Role: Data is stored per-element on the controller if the controller element has a known role. Action mappings are mirrored on controller elements with the same role on all other controllers for the Player. Example: When saving Action mappings for a gamepad, element on all gamepads that have the same roles will inherit the mappings. This allows you to remap once for all compatible gamepads simultaneously, for example. This can extend beyond just gamepads, however. For example: On a console platform, a racing wheel with A, B, X, Y, D-Pad etc. elements will also reflect the same Action mappings if the gamepad is remapped. Action mappings for any controller elements that do not have known roles will be saved per-controller. Warning: Do not use this mode if you need to allow a Player to save different mappings for multiple controllers of the same type such as gamepads. (This option currently works best for gamepads and only miminally for other controller types.)")]
		[SerializeField]
		private UserDataStore_PlayerPrefs.ActionMappingSaveMode _actionMappingSaveMode;

		// Token: 0x0400027E RID: 638
		[Tooltip("The PlayerPrefs key prefix. Change this to change how keys are stored in PlayerPrefs. Changing this will make saved data already stored with the old key no longer accessible.")]
		[SerializeField]
		private string playerPrefsKeyPrefix = "RewiredSaveData";

		// Token: 0x0400027F RID: 639
		[NonSerialized]
		private bool allowImpreciseJoystickAssignmentMatching = true;

		// Token: 0x04000280 RID: 640
		[NonSerialized]
		private bool deferredJoystickAssignmentLoadPending;

		// Token: 0x04000281 RID: 641
		[NonSerialized]
		private bool wasJoystickEverDetected;

		// Token: 0x04000282 RID: 642
		[NonSerialized]
		private List<int> __allActionIds;

		// Token: 0x04000283 RID: 643
		[NonSerialized]
		private string __allActionIdsString;

		// Token: 0x04000284 RID: 644
		[NonSerialized]
		private readonly StringBuilder _sb = new StringBuilder();

		// Token: 0x04000285 RID: 645
		[NonSerialized]
		private Dictionary<string, UserDataStore_PlayerPrefs.ControllerElementByRoleMap> _tempElementByRoleMaps;

		// Token: 0x04000286 RID: 646
		[NonSerialized]
		private Dictionary<string, bool> _tempElementByRoleMapsEnabled;

		// Token: 0x02000041 RID: 65
		private class ControllerAssignmentSaveInfo
		{
			// Token: 0x1700025D RID: 605
			// (get) Token: 0x06000469 RID: 1129 RVA: 0x000099FF File Offset: 0x00007BFF
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

			// Token: 0x0600046A RID: 1130 RVA: 0x000021D7 File Offset: 0x000003D7
			public ControllerAssignmentSaveInfo()
			{
			}

			// Token: 0x0600046B RID: 1131 RVA: 0x00009A14 File Offset: 0x00007C14
			public ControllerAssignmentSaveInfo(int playerCount)
			{
				this.players = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[playerCount];
				for (int i = 0; i < playerCount; i++)
				{
					this.players[i] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
				}
			}

			// Token: 0x0600046C RID: 1132 RVA: 0x00009A4C File Offset: 0x00007C4C
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

			// Token: 0x0600046D RID: 1133 RVA: 0x00009A87 File Offset: 0x00007C87
			public bool ContainsPlayer(int playerId)
			{
				return this.IndexOfPlayer(playerId) >= 0;
			}

			// Token: 0x04000287 RID: 647
			public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[] players;

			// Token: 0x02000042 RID: 66
			public class PlayerInfo
			{
				// Token: 0x1700025E RID: 606
				// (get) Token: 0x0600046E RID: 1134 RVA: 0x00009A96 File Offset: 0x00007C96
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

				// Token: 0x0600046F RID: 1135 RVA: 0x00009AAC File Offset: 0x00007CAC
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

				// Token: 0x06000470 RID: 1136 RVA: 0x00009AE7 File Offset: 0x00007CE7
				public bool ContainsJoystick(int joystickId)
				{
					return this.IndexOfJoystick(joystickId) >= 0;
				}

				// Token: 0x04000288 RID: 648
				public int id;

				// Token: 0x04000289 RID: 649
				public bool hasKeyboard;

				// Token: 0x0400028A RID: 650
				public bool hasMouse;

				// Token: 0x0400028B RID: 651
				public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] joysticks;
			}

			// Token: 0x02000043 RID: 67
			public class JoystickInfo
			{
				// Token: 0x0400028C RID: 652
				public Guid instanceGuid;

				// Token: 0x0400028D RID: 653
				public string hardwareIdentifier;

				// Token: 0x0400028E RID: 654
				public int id;
			}
		}

		// Token: 0x02000044 RID: 68
		private class JoystickAssignmentHistoryInfo
		{
			// Token: 0x06000473 RID: 1139 RVA: 0x00009AF6 File Offset: 0x00007CF6
			public JoystickAssignmentHistoryInfo(Joystick joystick, int oldJoystickId)
			{
				if (joystick == null)
				{
					throw new ArgumentNullException("joystick");
				}
				this.joystick = joystick;
				this.oldJoystickId = oldJoystickId;
			}

			// Token: 0x0400028F RID: 655
			public readonly Joystick joystick;

			// Token: 0x04000290 RID: 656
			public readonly int oldJoystickId;
		}

		// Token: 0x02000045 RID: 69
		[Serializable]
		private class ControllerElementByRoleMap
		{
			// Token: 0x06000474 RID: 1140 RVA: 0x00009B1A File Offset: 0x00007D1A
			[Preserve]
			public ControllerElementByRoleMap()
			{
				this.data = new List<UserDataStore_PlayerPrefs.ControllerElementByRoleMap.Entry>();
			}

			// Token: 0x06000475 RID: 1141 RVA: 0x00009B30 File Offset: 0x00007D30
			public void Add(ActionElementMap elementMap)
			{
				this.data.Add(new UserDataStore_PlayerPrefs.ControllerElementByRoleMap.Entry
				{
					actionId = elementMap.actionId,
					elementType = elementMap.elementType,
					axisRange = elementMap.axisRange,
					invert = elementMap.invert,
					axisContribution = elementMap.axisContribution
				});
			}

			// Token: 0x06000476 RID: 1142 RVA: 0x00009B94 File Offset: 0x00007D94
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

			// Token: 0x06000477 RID: 1143 RVA: 0x000075D5 File Offset: 0x000057D5
			public string ToJson()
			{
				return JsonWriter.ToJson(this);
			}

			// Token: 0x06000478 RID: 1144 RVA: 0x00009C54 File Offset: 0x00007E54
			public static UserDataStore_PlayerPrefs.ControllerElementByRoleMap FromJson(string role, string json)
			{
				UserDataStore_PlayerPrefs.ControllerElementByRoleMap controllerElementByRoleMap = JsonParser.FromJson<UserDataStore_PlayerPrefs.ControllerElementByRoleMap>(json);
				if (controllerElementByRoleMap != null)
				{
					controllerElementByRoleMap.role = role;
				}
				return controllerElementByRoleMap;
			}

			// Token: 0x04000291 RID: 657
			[DoNotSerialize]
			public string role;

			// Token: 0x04000292 RID: 658
			public List<UserDataStore_PlayerPrefs.ControllerElementByRoleMap.Entry> data;

			// Token: 0x02000046 RID: 70
			[Serializable]
			public struct Entry
			{
				// Token: 0x06000479 RID: 1145 RVA: 0x00009C74 File Offset: 0x00007E74
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

				// Token: 0x0600047A RID: 1146 RVA: 0x00009D78 File Offset: 0x00007F78
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

				// Token: 0x04000293 RID: 659
				public int actionId;

				// Token: 0x04000294 RID: 660
				public ControllerElementType elementType;

				// Token: 0x04000295 RID: 661
				public AxisRange axisRange;

				// Token: 0x04000296 RID: 662
				public bool invert;

				// Token: 0x04000297 RID: 663
				public Pole axisContribution;
			}
		}

		// Token: 0x02000047 RID: 71
		public enum ActionMappingSaveMode
		{
			// Token: 0x04000299 RID: 665
			ByController,
			// Token: 0x0400029A RID: 666
			ByControllerElementRole
		}
	}
}
