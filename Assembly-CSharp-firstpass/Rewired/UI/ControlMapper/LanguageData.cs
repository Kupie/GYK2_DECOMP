using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000C5 RID: 197
	[Serializable]
	public class LanguageData : LanguageDataBase
	{
		// Token: 0x06000A66 RID: 2662 RVA: 0x0001D63D File Offset: 0x0001B83D
		public override void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			this.customDict = LanguageData.CustomEntry.ToDictionary(this._customEntries);
			this._initialized = true;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0001D660 File Offset: 0x0001B860
		public override string GetCustomEntry(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return string.Empty;
			}
			string text;
			if (!this.customDict.TryGetValue(key, out text))
			{
				return string.Empty;
			}
			return text;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0001D692 File Offset: 0x0001B892
		public override bool ContainsCustomEntryKey(string key)
		{
			return !string.IsNullOrEmpty(key) && this.customDict.ContainsKey(key);
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0001D6AA File Offset: 0x0001B8AA
		public override string yes
		{
			get
			{
				return this._yes;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0001D6B2 File Offset: 0x0001B8B2
		public override string no
		{
			get
			{
				return this._no;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0001D6BA File Offset: 0x0001B8BA
		public override string add
		{
			get
			{
				return this._add;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0001D6C2 File Offset: 0x0001B8C2
		public override string replace
		{
			get
			{
				return this._replace;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0001D6CA File Offset: 0x0001B8CA
		public override string remove
		{
			get
			{
				return this._remove;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0001D6D2 File Offset: 0x0001B8D2
		public override string swap
		{
			get
			{
				return this._swap;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0001D6DA File Offset: 0x0001B8DA
		public override string cancel
		{
			get
			{
				return this._cancel;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0001D6E2 File Offset: 0x0001B8E2
		public override string none
		{
			get
			{
				return this._none;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0001D6EA File Offset: 0x0001B8EA
		public override string okay
		{
			get
			{
				return this._okay;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0001D6F2 File Offset: 0x0001B8F2
		public override string done
		{
			get
			{
				return this._done;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0001D6FA File Offset: 0x0001B8FA
		public override string default_
		{
			get
			{
				return this._default;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0001D702 File Offset: 0x0001B902
		public override string assignControllerWindowTitle
		{
			get
			{
				return this._assignControllerWindowTitle;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0001D70A File Offset: 0x0001B90A
		public override string assignControllerWindowMessage
		{
			get
			{
				return this._assignControllerWindowMessage;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0001D712 File Offset: 0x0001B912
		public override string controllerAssignmentConflictWindowTitle
		{
			get
			{
				return this._controllerAssignmentConflictWindowTitle;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0001D71A File Offset: 0x0001B91A
		public override string elementAssignmentPrePollingWindowMessage
		{
			get
			{
				return this._elementAssignmentPrePollingWindowMessage;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0001D722 File Offset: 0x0001B922
		public override string elementAssignmentConflictWindowMessage
		{
			get
			{
				return this._elementAssignmentConflictWindowMessage;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0001D72A File Offset: 0x0001B92A
		public override string mouseAssignmentConflictWindowTitle
		{
			get
			{
				return this._mouseAssignmentConflictWindowTitle;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0001D732 File Offset: 0x0001B932
		public override string calibrateControllerWindowTitle
		{
			get
			{
				return this._calibrateControllerWindowTitle;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0001D73A File Offset: 0x0001B93A
		public override string calibrateAxisStep1WindowTitle
		{
			get
			{
				return this._calibrateAxisStep1WindowTitle;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0001D742 File Offset: 0x0001B942
		public override string calibrateAxisStep2WindowTitle
		{
			get
			{
				return this._calibrateAxisStep2WindowTitle;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0001D74A File Offset: 0x0001B94A
		public override string inputBehaviorSettingsWindowTitle
		{
			get
			{
				return this._inputBehaviorSettingsWindowTitle;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0001D752 File Offset: 0x0001B952
		public override string restoreDefaultsWindowTitle
		{
			get
			{
				return this._restoreDefaultsWindowTitle;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0001D75A File Offset: 0x0001B95A
		public override string actionColumnLabel
		{
			get
			{
				return this._actionColumnLabel;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0001D762 File Offset: 0x0001B962
		public override string keyboardColumnLabel
		{
			get
			{
				return this._keyboardColumnLabel;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0001D76A File Offset: 0x0001B96A
		public override string mouseColumnLabel
		{
			get
			{
				return this._mouseColumnLabel;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x0001D772 File Offset: 0x0001B972
		public override string controllerColumnLabel
		{
			get
			{
				return this._controllerColumnLabel;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x0001D77A File Offset: 0x0001B97A
		public override string removeControllerButtonLabel
		{
			get
			{
				return this._removeControllerButtonLabel;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0001D782 File Offset: 0x0001B982
		public override string calibrateControllerButtonLabel
		{
			get
			{
				return this._calibrateControllerButtonLabel;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0001D78A File Offset: 0x0001B98A
		public override string assignControllerButtonLabel
		{
			get
			{
				return this._assignControllerButtonLabel;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0001D792 File Offset: 0x0001B992
		public override string inputBehaviorSettingsButtonLabel
		{
			get
			{
				return this._inputBehaviorSettingsButtonLabel;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0001D79A File Offset: 0x0001B99A
		public override string doneButtonLabel
		{
			get
			{
				return this._doneButtonLabel;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0001D7A2 File Offset: 0x0001B9A2
		public override string restoreDefaultsButtonLabel
		{
			get
			{
				return this._restoreDefaultsButtonLabel;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0001D7AA File Offset: 0x0001B9AA
		public override string controllerSettingsGroupLabel
		{
			get
			{
				return this._controllerSettingsGroupLabel;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0001D7B2 File Offset: 0x0001B9B2
		public override string playersGroupLabel
		{
			get
			{
				return this._playersGroupLabel;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0001D7BA File Offset: 0x0001B9BA
		public override string assignedControllersGroupLabel
		{
			get
			{
				return this._assignedControllersGroupLabel;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0001D7C2 File Offset: 0x0001B9C2
		public override string settingsGroupLabel
		{
			get
			{
				return this._settingsGroupLabel;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0001D7CA File Offset: 0x0001B9CA
		public override string mapCategoriesGroupLabel
		{
			get
			{
				return this._mapCategoriesGroupLabel;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0001D7D2 File Offset: 0x0001B9D2
		public override string restoreDefaultsWindowMessage
		{
			get
			{
				if (ReInput.players.playerCount > 1)
				{
					return this._restoreDefaultsWindowMessage_multiPlayer;
				}
				return this._restoreDefaultsWindowMessage_onePlayer;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0001D7EE File Offset: 0x0001B9EE
		public override string calibrateWindow_deadZoneSliderLabel
		{
			get
			{
				return this._calibrateWindow_deadZoneSliderLabel;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0001D7F6 File Offset: 0x0001B9F6
		public override string calibrateWindow_zeroSliderLabel
		{
			get
			{
				return this._calibrateWindow_zeroSliderLabel;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0001D7FE File Offset: 0x0001B9FE
		public override string calibrateWindow_sensitivitySliderLabel
		{
			get
			{
				return this._calibrateWindow_sensitivitySliderLabel;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0001D806 File Offset: 0x0001BA06
		public override string calibrateWindow_invertToggleLabel
		{
			get
			{
				return this._calibrateWindow_invertToggleLabel;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x0001D80E File Offset: 0x0001BA0E
		public override string calibrateWindow_calibrateButtonLabel
		{
			get
			{
				return this._calibrateWindow_calibrateButtonLabel;
			}
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0001D816 File Offset: 0x0001BA16
		public override string GetControllerAssignmentConflictWindowMessage(string joystickName, string otherPlayerName, string currentPlayerName)
		{
			return string.Format(this._controllerAssignmentConflictWindowMessage, joystickName, otherPlayerName, currentPlayerName);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0001D826 File Offset: 0x0001BA26
		public override string GetJoystickElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(this._joystickElementAssignmentPollingWindowMessage, actionName);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0001D834 File Offset: 0x0001BA34
		public override string GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
		{
			return string.Format(this._joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0001D842 File Offset: 0x0001BA42
		public override string GetKeyboardElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(this._keyboardElementAssignmentPollingWindowMessage, actionName);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0001D850 File Offset: 0x0001BA50
		public override string GetMouseElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(this._mouseElementAssignmentPollingWindowMessage, actionName);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0001D85E File Offset: 0x0001BA5E
		public override string GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
		{
			return string.Format(this._mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0001D86C File Offset: 0x0001BA6C
		public override string GetElementAlreadyInUseBlocked(string elementName)
		{
			return string.Format(this._elementAlreadyInUseBlocked, elementName);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0001D87A File Offset: 0x0001BA7A
		public override string GetElementAlreadyInUseCanReplace(string elementName, bool allowConflicts)
		{
			if (!allowConflicts)
			{
				return string.Format(this._elementAlreadyInUseCanReplace, elementName);
			}
			return string.Format(this._elementAlreadyInUseCanReplace_conflictAllowed, elementName);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0001D898 File Offset: 0x0001BA98
		public override string GetMouseAssignmentConflictWindowMessage(string otherPlayerName, string thisPlayerName)
		{
			return string.Format(this._mouseAssignmentConflictWindowMessage, otherPlayerName, thisPlayerName);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0001D8A7 File Offset: 0x0001BAA7
		public override string GetCalibrateAxisStep1WindowMessage(string axisName)
		{
			return string.Format(this._calibrateAxisStep1WindowMessage, axisName);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0001D8B5 File Offset: 0x0001BAB5
		public override string GetCalibrateAxisStep2WindowMessage(string axisName)
		{
			return string.Format(this._calibrateAxisStep2WindowMessage, axisName);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0001D8C3 File Offset: 0x0001BAC3
		public override string GetPlayerName(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				throw new ArgumentException("Invalid player id: " + playerId.ToString());
			}
			return player.descriptiveName;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001D8EF File Offset: 0x0001BAEF
		public override string GetControllerName(Controller controller)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			return controller.name;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0001D908 File Offset: 0x0001BB08
		public override string GetElementIdentifierName(ActionElementMap actionElementMap)
		{
			if (actionElementMap == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			if (base.isLocalizationSystemEnabled)
			{
				return actionElementMap.elementIdentifierName;
			}
			if (actionElementMap.controllerMap.controllerType == ControllerType.Keyboard)
			{
				return this.GetElementIdentifierName(actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
			}
			return this.GetElementIdentifierName(actionElementMap.controllerMap.controller, actionElementMap.elementIdentifierId, actionElementMap.axisRange);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0001D970 File Offset: 0x0001BB70
		public override string GetElementIdentifierName(Controller controller, int elementIdentifierId, AxisRange axisRange)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(elementIdentifierId);
			if (elementIdentifierById == null)
			{
				throw new ArgumentException("Invalid element identifier id: " + elementIdentifierId.ToString());
			}
			Controller.Element elementById = controller.GetElementById(elementIdentifierId);
			if (elementById == null)
			{
				return string.Empty;
			}
			return elementIdentifierById.GetDisplayName(elementById.type, axisRange);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001D9CB File Offset: 0x0001BBCB
		public override string GetElementIdentifierName(KeyCode keyCode, ModifierKeyFlags modifierKeyFlags)
		{
			if (base.isLocalizationSystemEnabled)
			{
				return Keyboard.GetKeyName(keyCode, modifierKeyFlags);
			}
			if (modifierKeyFlags != ModifierKeyFlags.None)
			{
				return string.Format("{0}{1}{2}", this.ModifierKeyFlagsToString(modifierKeyFlags), this._modifierKeys.separator, Keyboard.GetKeyName(keyCode));
			}
			return Keyboard.GetKeyName(keyCode);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0001DA09 File Offset: 0x0001BC09
		public override string GetActionName(int actionId)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				throw new ArgumentException("Invalid action id: " + actionId.ToString());
			}
			return action.descriptiveName;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001DA38 File Offset: 0x0001BC38
		public override string GetActionName(int actionId, AxisRange axisRange)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				throw new ArgumentException("Invalid action id: " + actionId.ToString());
			}
			if (base.isLocalizationSystemEnabled)
			{
				return action.GetDisplayName(axisRange);
			}
			switch (axisRange)
			{
			case AxisRange.Full:
				return action.descriptiveName;
			case AxisRange.Positive:
				if (string.IsNullOrEmpty(action.positiveDescriptiveName))
				{
					return action.descriptiveName + " +";
				}
				return action.positiveDescriptiveName;
			case AxisRange.Negative:
				if (string.IsNullOrEmpty(action.negativeDescriptiveName))
				{
					return action.descriptiveName + " -";
				}
				return action.negativeDescriptiveName;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001DAE5 File Offset: 0x0001BCE5
		public override string GetMapCategoryName(int id)
		{
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(id);
			if (mapCategory == null)
			{
				throw new ArgumentException("Invalid map category id: " + id.ToString());
			}
			return mapCategory.descriptiveName;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001DB11 File Offset: 0x0001BD11
		public override string GetActionCategoryName(int id)
		{
			InputCategory actionCategory = ReInput.mapping.GetActionCategory(id);
			if (actionCategory == null)
			{
				throw new ArgumentException("Invalid action category id: " + id.ToString());
			}
			return actionCategory.descriptiveName;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0001DB3D File Offset: 0x0001BD3D
		public override string GetLayoutName(ControllerType controllerType, int id)
		{
			InputLayout layout = ReInput.mapping.GetLayout(controllerType, id);
			if (layout == null)
			{
				throw new ArgumentException("Invalid " + controllerType.ToString() + " layout id: " + id.ToString());
			}
			return layout.descriptiveName;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0001DB7C File Offset: 0x0001BD7C
		public override string ModifierKeyFlagsToString(ModifierKeyFlags flags)
		{
			if (base.isLocalizationSystemEnabled)
			{
				return Keyboard.ModifierKeyFlagsToString(flags);
			}
			int num = 0;
			string text = string.Empty;
			if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Control))
			{
				text += this._modifierKeys.control;
				num++;
			}
			if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Command))
			{
				if (num > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
				{
					text += this._modifierKeys.separator;
				}
				text += this._modifierKeys.command;
				num++;
			}
			if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Alt))
			{
				if (num > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
				{
					text += this._modifierKeys.separator;
				}
				text += this._modifierKeys.alt;
				num++;
			}
			if (num >= 3)
			{
				return text;
			}
			if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Shift))
			{
				if (num > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
				{
					text += this._modifierKeys.separator;
				}
				text += this._modifierKeys.shift;
				num++;
			}
			return text;
		}

		// Token: 0x04000519 RID: 1305
		[SerializeField]
		private string _yes = "Yes";

		// Token: 0x0400051A RID: 1306
		[SerializeField]
		private string _no = "No";

		// Token: 0x0400051B RID: 1307
		[SerializeField]
		private string _add = "Add";

		// Token: 0x0400051C RID: 1308
		[SerializeField]
		private string _replace = "Replace";

		// Token: 0x0400051D RID: 1309
		[SerializeField]
		private string _remove = "Remove";

		// Token: 0x0400051E RID: 1310
		[SerializeField]
		private string _swap = "Swap";

		// Token: 0x0400051F RID: 1311
		[SerializeField]
		private string _cancel = "Cancel";

		// Token: 0x04000520 RID: 1312
		[SerializeField]
		private string _none = "None";

		// Token: 0x04000521 RID: 1313
		[SerializeField]
		private string _okay = "Okay";

		// Token: 0x04000522 RID: 1314
		[SerializeField]
		private string _done = "Done";

		// Token: 0x04000523 RID: 1315
		[SerializeField]
		private string _default = "Default";

		// Token: 0x04000524 RID: 1316
		[SerializeField]
		private string _assignControllerWindowTitle = "Choose Controller";

		// Token: 0x04000525 RID: 1317
		[SerializeField]
		private string _assignControllerWindowMessage = "Press any button or move an axis on the controller you would like to use.";

		// Token: 0x04000526 RID: 1318
		[SerializeField]
		private string _controllerAssignmentConflictWindowTitle = "Controller Assignment";

		// Token: 0x04000527 RID: 1319
		[SerializeField]
		[Tooltip("{0} = Joystick Name\n{1} = Other Player Name\n{2} = This Player Name")]
		private string _controllerAssignmentConflictWindowMessage = "{0} is already assigned to {1}. Do you want to assign this controller to {2} instead?";

		// Token: 0x04000528 RID: 1320
		[SerializeField]
		private string _elementAssignmentPrePollingWindowMessage = "First center or zero all sticks and axes and press any button or wait for the timer to finish.";

		// Token: 0x04000529 RID: 1321
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _joystickElementAssignmentPollingWindowMessage = "Now press a button or move an axis to assign it to {0}.";

		// Token: 0x0400052A RID: 1322
		[SerializeField]
		[Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
		private string _joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Now move an axis to assign it to {0}.";

		// Token: 0x0400052B RID: 1323
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _keyboardElementAssignmentPollingWindowMessage = "Press a key to assign it to {0}. Modifier keys may also be used. To assign a modifier key alone, hold it down for 1 second.";

		// Token: 0x0400052C RID: 1324
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _mouseElementAssignmentPollingWindowMessage = "Press a mouse button or move an axis to assign it to {0}.";

		// Token: 0x0400052D RID: 1325
		[SerializeField]
		[Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
		private string _mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Move an axis to assign it to {0}.";

		// Token: 0x0400052E RID: 1326
		[SerializeField]
		private string _elementAssignmentConflictWindowMessage = "Assignment Conflict";

		// Token: 0x0400052F RID: 1327
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseBlocked = "{0} is already in use cannot be replaced.";

		// Token: 0x04000530 RID: 1328
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseCanReplace = "{0} is already in use. Do you want to replace it?";

		// Token: 0x04000531 RID: 1329
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseCanReplace_conflictAllowed = "{0} is already in use. Do you want to replace it? You may also choose to add the assignment anyway.";

		// Token: 0x04000532 RID: 1330
		[SerializeField]
		private string _mouseAssignmentConflictWindowTitle = "Mouse Assignment";

		// Token: 0x04000533 RID: 1331
		[SerializeField]
		[Tooltip("{0} = Other Player Name\n{1} = This Player Name")]
		private string _mouseAssignmentConflictWindowMessage = "The mouse is already assigned to {0}. Do you want to assign the mouse to {1} instead?";

		// Token: 0x04000534 RID: 1332
		[SerializeField]
		private string _calibrateControllerWindowTitle = "Calibrate Controller";

		// Token: 0x04000535 RID: 1333
		[SerializeField]
		private string _calibrateAxisStep1WindowTitle = "Calibrate Zero";

		// Token: 0x04000536 RID: 1334
		[SerializeField]
		[Tooltip("{0} = Axis Name")]
		private string _calibrateAxisStep1WindowMessage = "Center or zero {0} and press any button or wait for the timer to finish.";

		// Token: 0x04000537 RID: 1335
		[SerializeField]
		private string _calibrateAxisStep2WindowTitle = "Calibrate Range";

		// Token: 0x04000538 RID: 1336
		[SerializeField]
		[Tooltip("{0} = Axis Name")]
		private string _calibrateAxisStep2WindowMessage = "Move {0} through its entire range then press any button or wait for the timer to finish.";

		// Token: 0x04000539 RID: 1337
		[SerializeField]
		private string _inputBehaviorSettingsWindowTitle = "Sensitivity Settings";

		// Token: 0x0400053A RID: 1338
		[SerializeField]
		private string _restoreDefaultsWindowTitle = "Restore Defaults";

		// Token: 0x0400053B RID: 1339
		[SerializeField]
		[Tooltip("Message for a single player game.")]
		private string _restoreDefaultsWindowMessage_onePlayer = "This will restore the default input configuration. Are you sure you want to do this?";

		// Token: 0x0400053C RID: 1340
		[SerializeField]
		[Tooltip("Message for a multi-player game.")]
		private string _restoreDefaultsWindowMessage_multiPlayer = "This will restore the default input configuration for all players. Are you sure you want to do this?";

		// Token: 0x0400053D RID: 1341
		[SerializeField]
		private string _actionColumnLabel = "Actions";

		// Token: 0x0400053E RID: 1342
		[SerializeField]
		private string _keyboardColumnLabel = "Keyboard";

		// Token: 0x0400053F RID: 1343
		[SerializeField]
		private string _mouseColumnLabel = "Mouse";

		// Token: 0x04000540 RID: 1344
		[SerializeField]
		private string _controllerColumnLabel = "Controller";

		// Token: 0x04000541 RID: 1345
		[SerializeField]
		private string _removeControllerButtonLabel = "Remove";

		// Token: 0x04000542 RID: 1346
		[SerializeField]
		private string _calibrateControllerButtonLabel = "Calibrate";

		// Token: 0x04000543 RID: 1347
		[SerializeField]
		private string _assignControllerButtonLabel = "Assign Controller";

		// Token: 0x04000544 RID: 1348
		[SerializeField]
		private string _inputBehaviorSettingsButtonLabel = "Sensitivity";

		// Token: 0x04000545 RID: 1349
		[SerializeField]
		private string _doneButtonLabel = "Done";

		// Token: 0x04000546 RID: 1350
		[SerializeField]
		private string _restoreDefaultsButtonLabel = "Restore Defaults";

		// Token: 0x04000547 RID: 1351
		[SerializeField]
		private string _playersGroupLabel = "Players:";

		// Token: 0x04000548 RID: 1352
		[SerializeField]
		private string _controllerSettingsGroupLabel = "Controller:";

		// Token: 0x04000549 RID: 1353
		[SerializeField]
		private string _assignedControllersGroupLabel = "Assigned Controllers:";

		// Token: 0x0400054A RID: 1354
		[SerializeField]
		private string _settingsGroupLabel = "Settings:";

		// Token: 0x0400054B RID: 1355
		[SerializeField]
		private string _mapCategoriesGroupLabel = "Categories:";

		// Token: 0x0400054C RID: 1356
		[SerializeField]
		private string _calibrateWindow_deadZoneSliderLabel = "Dead Zone:";

		// Token: 0x0400054D RID: 1357
		[SerializeField]
		private string _calibrateWindow_zeroSliderLabel = "Zero:";

		// Token: 0x0400054E RID: 1358
		[SerializeField]
		private string _calibrateWindow_sensitivitySliderLabel = "Sensitivity:";

		// Token: 0x0400054F RID: 1359
		[SerializeField]
		private string _calibrateWindow_invertToggleLabel = "Invert";

		// Token: 0x04000550 RID: 1360
		[SerializeField]
		private string _calibrateWindow_calibrateButtonLabel = "Calibrate";

		// Token: 0x04000551 RID: 1361
		[SerializeField]
		private LanguageData.ModifierKeys _modifierKeys;

		// Token: 0x04000552 RID: 1362
		[SerializeField]
		private LanguageData.CustomEntry[] _customEntries;

		// Token: 0x04000553 RID: 1363
		private bool _initialized;

		// Token: 0x04000554 RID: 1364
		private Dictionary<string, string> customDict;

		// Token: 0x020000C6 RID: 198
		[Serializable]
		protected class CustomEntry
		{
			// Token: 0x06000AAB RID: 2731 RVA: 0x000021D7 File Offset: 0x000003D7
			public CustomEntry()
			{
			}

			// Token: 0x06000AAC RID: 2732 RVA: 0x0001DF17 File Offset: 0x0001C117
			public CustomEntry(string key, string value)
			{
				this.key = key;
				this.value = value;
			}

			// Token: 0x06000AAD RID: 2733 RVA: 0x0001DF30 File Offset: 0x0001C130
			public static Dictionary<string, string> ToDictionary(LanguageData.CustomEntry[] array)
			{
				if (array == null)
				{
					return new Dictionary<string, string>();
				}
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && !string.IsNullOrEmpty(array[i].key) && !string.IsNullOrEmpty(array[i].value))
					{
						if (dictionary.ContainsKey(array[i].key))
						{
							Debug.LogError("Key \"" + array[i].key + "\" is already in dictionary!");
						}
						else
						{
							dictionary.Add(array[i].key, array[i].value);
						}
					}
				}
				return dictionary;
			}

			// Token: 0x04000555 RID: 1365
			public string key;

			// Token: 0x04000556 RID: 1366
			public string value;
		}

		// Token: 0x020000C7 RID: 199
		[Serializable]
		protected class ModifierKeys
		{
			// Token: 0x04000557 RID: 1367
			public string control = "Control";

			// Token: 0x04000558 RID: 1368
			public string alt = "Alt";

			// Token: 0x04000559 RID: 1369
			public string shift = "Shift";

			// Token: 0x0400055A RID: 1370
			public string command = "Command";

			// Token: 0x0400055B RID: 1371
			public string separator = " + ";
		}
	}
}
