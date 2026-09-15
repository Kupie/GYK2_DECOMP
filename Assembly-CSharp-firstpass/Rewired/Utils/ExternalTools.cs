using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rewired.Internal;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Platforms.Windows;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace Rewired.Utils
{
	// Token: 0x0200002A RID: 42
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ExternalTools : IExternalTools
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00004C2F File Offset: 0x00002E2F
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00004C36 File Offset: 0x00002E36
		public static Func<object> getPlatformInitializerDelegate
		{
			get
			{
				return ExternalTools._getPlatformInitializerDelegate;
			}
			set
			{
				ExternalTools._getPlatformInitializerDelegate = value;
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00003466 File Offset: 0x00001666
		public void Destroy()
		{
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00004C3E File Offset: 0x00002E3E
		public bool isEditorPaused
		{
			get
			{
				return this._isEditorPaused;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000345 RID: 837 RVA: 0x00004C46 File Offset: 0x00002E46
		// (remove) Token: 0x06000346 RID: 838 RVA: 0x00004C5F File Offset: 0x00002E5F
		public event Action<bool> EditorPausedStateChangedEvent
		{
			add
			{
				this._EditorPausedStateChangedEvent = (Action<bool>)Delegate.Combine(this._EditorPausedStateChangedEvent, value);
			}
			remove
			{
				this._EditorPausedStateChangedEvent = (Action<bool>)Delegate.Remove(this._EditorPausedStateChangedEvent, value);
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00004C78 File Offset: 0x00002E78
		public object GetPlatformInitializer()
		{
			return Main.GetPlatformInitializer();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00004C7F File Offset: 0x00002E7F
		public string GetFocusedEditorWindowTitle()
		{
			return string.Empty;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00004C86 File Offset: 0x00002E86
		public bool IsEditorSceneViewFocused()
		{
			return false;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00004C86 File Offset: 0x00002E86
		public bool LinuxInput_IsJoystickPreconfigured(string name)
		{
			return false;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600034B RID: 843 RVA: 0x00004C8C File Offset: 0x00002E8C
		// (remove) Token: 0x0600034C RID: 844 RVA: 0x00004CC4 File Offset: 0x00002EC4
		public event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

		// Token: 0x0600034D RID: 845 RVA: 0x00004C86 File Offset: 0x00002E86
		public int XboxOneInput_GetUserIdForGamepad(uint id)
		{
			return 0;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00004CF9 File Offset: 0x00002EF9
		public ulong XboxOneInput_GetControllerId(uint unityJoystickId)
		{
			return 0UL;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00004C86 File Offset: 0x00002E86
		public bool XboxOneInput_IsGamepadActive(uint unityJoystickId)
		{
			return false;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00004C7F File Offset: 0x00002E7F
		public string XboxOneInput_GetControllerType(ulong xboxControllerId)
		{
			return string.Empty;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00004C86 File Offset: 0x00002E86
		public uint XboxOneInput_GetJoystickId(ulong xboxControllerId)
		{
			return 0U;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00003466 File Offset: 0x00001666
		public void XboxOne_Gamepad_UpdatePlugin()
		{
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00004C86 File Offset: 0x00002E86
		public bool XboxOne_Gamepad_SetGamepadVibration(ulong xboxOneJoystickId, float leftMotor, float rightMotor, float leftTriggerLevel, float rightTriggerLevel)
		{
			return false;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00003466 File Offset: 0x00001666
		public void XboxOne_Gamepad_PulseVibrateMotor(ulong xboxOneJoystickId, int motorInt, float startLevel, float endLevel, ulong durationMS)
		{
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00004CFD File Offset: 0x00002EFD
		public void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids)
		{
			vids = new List<int>();
			pids = new List<int>();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00004D0D File Offset: 0x00002F0D
		public int GetAndroidAPILevel()
		{
			return -1;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00004D10 File Offset: 0x00002F10
		public void WindowsStandalone_ForwardRawInput(IntPtr rawInputHeaderIndices, IntPtr rawInputDataIndices, uint indicesCount, IntPtr rawInputData, uint rawInputDataSize)
		{
			global::UnityEngine.Windows.Input.ForwardRawInput(rawInputHeaderIndices, rawInputDataIndices, indicesCount, rawInputData, rawInputDataSize);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00004D1E File Offset: 0x00002F1E
		public bool UnityUI_Graphic_GetRaycastTarget(object graphic)
		{
			return !(graphic as Graphic == null) && (graphic as Graphic).raycastTarget;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00004D3B File Offset: 0x00002F3B
		public void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value)
		{
			if (graphic as Graphic == null)
			{
				return;
			}
			(graphic as Graphic).raycastTarget = value;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00004D58 File Offset: 0x00002F58
		public bool UnityInput_IsTouchPressureSupported
		{
			get
			{
				return global::UnityEngine.Input.touchPressureSupported;
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00004D5F File Offset: 0x00002F5F
		public float UnityInput_GetTouchPressure(ref Touch touch)
		{
			return touch.pressure;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00004D67 File Offset: 0x00002F67
		public float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch)
		{
			return touch.maximumPossiblePressure;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00004D6F File Offset: 0x00002F6F
		public IControllerTemplate CreateControllerTemplate(Guid typeGuid, object payload)
		{
			return ControllerTemplateFactory.Create(typeGuid, payload);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00004D78 File Offset: 0x00002F78
		public Type[] GetControllerTemplateTypes()
		{
			return ControllerTemplateFactory.templateTypes;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00004D7F File Offset: 0x00002F7F
		public Type[] GetControllerTemplateInterfaceTypes()
		{
			return ControllerTemplateFactory.templateInterfaceTypes;
		}

		// Token: 0x0400022A RID: 554
		private static Func<object> _getPlatformInitializerDelegate;

		// Token: 0x0400022B RID: 555
		private bool _isEditorPaused;

		// Token: 0x0400022C RID: 556
		private Action<bool> _EditorPausedStateChangedEvent;
	}
}
