using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000C8 RID: 200
	[Serializable]
	public abstract class LanguageDataBase : ScriptableObject
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0001E001 File Offset: 0x0001C201
		public bool isLocalizationSystemEnabled
		{
			get
			{
				return ReInput.localization.localizedStringProvider != null;
			}
		}

		// Token: 0x06000AB0 RID: 2736
		public abstract void Initialize();

		// Token: 0x06000AB1 RID: 2737
		public abstract string GetCustomEntry(string key);

		// Token: 0x06000AB2 RID: 2738
		public abstract bool ContainsCustomEntryKey(string key);

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000AB3 RID: 2739
		public abstract string yes { get; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000AB4 RID: 2740
		public abstract string no { get; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000AB5 RID: 2741
		public abstract string add { get; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000AB6 RID: 2742
		public abstract string replace { get; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000AB7 RID: 2743
		public abstract string remove { get; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000AB8 RID: 2744
		public abstract string swap { get; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000AB9 RID: 2745
		public abstract string cancel { get; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000ABA RID: 2746
		public abstract string none { get; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000ABB RID: 2747
		public abstract string okay { get; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000ABC RID: 2748
		public abstract string done { get; }

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000ABD RID: 2749
		public abstract string default_ { get; }

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000ABE RID: 2750
		public abstract string assignControllerWindowTitle { get; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000ABF RID: 2751
		public abstract string assignControllerWindowMessage { get; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000AC0 RID: 2752
		public abstract string controllerAssignmentConflictWindowTitle { get; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000AC1 RID: 2753
		public abstract string elementAssignmentPrePollingWindowMessage { get; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000AC2 RID: 2754
		public abstract string elementAssignmentConflictWindowMessage { get; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000AC3 RID: 2755
		public abstract string mouseAssignmentConflictWindowTitle { get; }

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000AC4 RID: 2756
		public abstract string calibrateControllerWindowTitle { get; }

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000AC5 RID: 2757
		public abstract string calibrateAxisStep1WindowTitle { get; }

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000AC6 RID: 2758
		public abstract string calibrateAxisStep2WindowTitle { get; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000AC7 RID: 2759
		public abstract string inputBehaviorSettingsWindowTitle { get; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000AC8 RID: 2760
		public abstract string restoreDefaultsWindowTitle { get; }

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000AC9 RID: 2761
		public abstract string actionColumnLabel { get; }

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000ACA RID: 2762
		public abstract string keyboardColumnLabel { get; }

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000ACB RID: 2763
		public abstract string mouseColumnLabel { get; }

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000ACC RID: 2764
		public abstract string controllerColumnLabel { get; }

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000ACD RID: 2765
		public abstract string removeControllerButtonLabel { get; }

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000ACE RID: 2766
		public abstract string calibrateControllerButtonLabel { get; }

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000ACF RID: 2767
		public abstract string assignControllerButtonLabel { get; }

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000AD0 RID: 2768
		public abstract string inputBehaviorSettingsButtonLabel { get; }

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000AD1 RID: 2769
		public abstract string doneButtonLabel { get; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000AD2 RID: 2770
		public abstract string restoreDefaultsButtonLabel { get; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000AD3 RID: 2771
		public abstract string controllerSettingsGroupLabel { get; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000AD4 RID: 2772
		public abstract string playersGroupLabel { get; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000AD5 RID: 2773
		public abstract string assignedControllersGroupLabel { get; }

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000AD6 RID: 2774
		public abstract string settingsGroupLabel { get; }

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000AD7 RID: 2775
		public abstract string mapCategoriesGroupLabel { get; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000AD8 RID: 2776
		public abstract string restoreDefaultsWindowMessage { get; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000AD9 RID: 2777
		public abstract string calibrateWindow_deadZoneSliderLabel { get; }

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000ADA RID: 2778
		public abstract string calibrateWindow_zeroSliderLabel { get; }

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000ADB RID: 2779
		public abstract string calibrateWindow_sensitivitySliderLabel { get; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000ADC RID: 2780
		public abstract string calibrateWindow_invertToggleLabel { get; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000ADD RID: 2781
		public abstract string calibrateWindow_calibrateButtonLabel { get; }

		// Token: 0x06000ADE RID: 2782
		public abstract string GetControllerAssignmentConflictWindowMessage(string joystickName, string otherPlayerName, string currentPlayerName);

		// Token: 0x06000ADF RID: 2783
		public abstract string GetJoystickElementAssignmentPollingWindowMessage(string actionName);

		// Token: 0x06000AE0 RID: 2784
		public abstract string GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName);

		// Token: 0x06000AE1 RID: 2785
		public abstract string GetKeyboardElementAssignmentPollingWindowMessage(string actionName);

		// Token: 0x06000AE2 RID: 2786
		public abstract string GetMouseElementAssignmentPollingWindowMessage(string actionName);

		// Token: 0x06000AE3 RID: 2787
		public abstract string GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName);

		// Token: 0x06000AE4 RID: 2788
		public abstract string GetElementAlreadyInUseBlocked(string elementName);

		// Token: 0x06000AE5 RID: 2789
		public abstract string GetElementAlreadyInUseCanReplace(string elementName, bool allowConflicts);

		// Token: 0x06000AE6 RID: 2790
		public abstract string GetMouseAssignmentConflictWindowMessage(string otherPlayerName, string thisPlayerName);

		// Token: 0x06000AE7 RID: 2791
		public abstract string GetCalibrateAxisStep1WindowMessage(string axisName);

		// Token: 0x06000AE8 RID: 2792
		public abstract string GetCalibrateAxisStep2WindowMessage(string axisName);

		// Token: 0x06000AE9 RID: 2793
		public abstract string GetPlayerName(int playerId);

		// Token: 0x06000AEA RID: 2794
		public abstract string GetControllerName(Controller controller);

		// Token: 0x06000AEB RID: 2795
		public abstract string GetElementIdentifierName(ActionElementMap actionElementMap);

		// Token: 0x06000AEC RID: 2796
		public abstract string GetElementIdentifierName(Controller controller, int elementIdentifierId, AxisRange axisRange);

		// Token: 0x06000AED RID: 2797
		public abstract string GetElementIdentifierName(KeyCode keyCode, ModifierKeyFlags modifierKeyFlags);

		// Token: 0x06000AEE RID: 2798
		public abstract string GetActionName(int actionId);

		// Token: 0x06000AEF RID: 2799
		public abstract string GetActionName(int actionId, AxisRange axisRange);

		// Token: 0x06000AF0 RID: 2800
		public abstract string GetMapCategoryName(int id);

		// Token: 0x06000AF1 RID: 2801
		public abstract string GetActionCategoryName(int id);

		// Token: 0x06000AF2 RID: 2802
		public abstract string GetLayoutName(ControllerType controllerType, int id);

		// Token: 0x06000AF3 RID: 2803
		public abstract string ModifierKeyFlagsToString(ModifierKeyFlags flags);
	}
}
