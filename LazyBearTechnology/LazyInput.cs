using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000136 RID: 310
	public class LazyInput : MonoBehaviour
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000606 RID: 1542 RVA: 0x0001EE28 File Offset: 0x0001D028
		// (remove) Token: 0x06000607 RID: 1543 RVA: 0x0001EE5C File Offset: 0x0001D05C
		public static event Action OnInputChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000608 RID: 1544 RVA: 0x0001EE90 File Offset: 0x0001D090
		// (remove) Token: 0x06000609 RID: 1545 RVA: 0x0001EEC4 File Offset: 0x0001D0C4
		public static event Action OnActiveGamepadChangedEvent;

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001EEF7 File Offset: 0x0001D0F7
		public static GameBindings GameBindings
		{
			get
			{
				return LazyInput.Instance.gameBindings;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001EF03 File Offset: 0x0001D103
		public static bool IsInitialized
		{
			get
			{
				return LazyInput.isInitialized;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0001EF0A File Offset: 0x0001D10A
		public static GamepadType CurrentGamepadType
		{
			get
			{
				if (LazyInput.debugGamepadTypeForced)
				{
					return LazyInput.forcedGamepadType;
				}
				return LazyInput.Instance.currentGamepadType;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0001EF23 File Offset: 0x0001D123
		public static ControllerIconLibrary ControllerIconLibrary
		{
			get
			{
				return LazyInput.instance.controllerIconLibrary;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0001EF2F File Offset: 0x0001D12F
		public static bool IsAnyKeyDown
		{
			get
			{
				return LazyInput.Instance.pressedKeys.Count != 0;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0001EF43 File Offset: 0x0001D143
		public static bool IsAnyKey
		{
			get
			{
				return LazyInput.Instance.holdedKeys.Count != 0;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001EF57 File Offset: 0x0001D157
		public static bool IsGamepadActive
		{
			get
			{
				if (LazyInput.isGamepadActivityForcedByPlatform)
				{
					return true;
				}
				if (!LazyInput.isStateForced)
				{
					return LazyInput.isGamepadActive;
				}
				return LazyInput.forcedState;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x0001EF74 File Offset: 0x0001D174
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x0001EF7B File Offset: 0x0001D17B
		public new static bool DontDestroyOnLoad
		{
			get
			{
				return LazyInput.dontDestroyOnLoad;
			}
			set
			{
				LazyInput.dontDestroyOnLoad = value;
				if (LazyInput.dontDestroyOnLoad && LazyInput.instance != null)
				{
					global::UnityEngine.Object.DontDestroyOnLoad(LazyInput.instance);
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0001EFA1 File Offset: 0x0001D1A1
		private static LazyInput Instance
		{
			get
			{
				if (!Application.isPlaying)
				{
					return global::UnityEngine.Object.FindObjectOfType<LazyInput>();
				}
				LazyInput.TryInit();
				return LazyInput.instance;
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001EFBC File Offset: 0x0001D1BC
		public static void TryInit()
		{
			if (!LazyInput.isInitialized)
			{
				LazyInput lazyInput = global::UnityEngine.Object.FindObjectOfType<LazyInput>();
				if (lazyInput == null)
				{
					lazyInput = new GameObject("LazyInput").AddComponent<LazyInput>();
				}
				LazyInput.instance = lazyInput;
				if (LazyInput.instance.gameBindings == null)
				{
					LazyInput.instance.gameBindings = LazySingletonSO<GameBindings>.Instance;
				}
				if (LazyInput.instance.gamepadTypeConfiguration == null)
				{
					LazyInput.instance.gamepadTypeConfiguration = LazySingletonSO<GamepadTypeData>.Instance;
				}
				if (LazyInput.instance.controllerIconLibrary == null)
				{
					LazyInput.instance.controllerIconLibrary = LazySingletonSO<ControllerIconLibrary>.Instance;
				}
				LazyInput.gamepad = new GamepadController(LazyInput.instance.gameBindings);
				LazyInput.keyboard = new KeyboardController(LazyInput.instance.gameBindings);
				LazyInput.isGamepadActive = LazyInput.isGamepadActivityForcedByPlatform || (LazyInput.ShouldActivateGamepadAtStartByPlatform && ReInput.controllers.joystickCount > 0);
				LazyInput.instance.rewiredPlayer = ReInput.players.GetPlayer(0);
				LazyInput.TryCacheConnectedGamepadType(LazyInput.isGamepadActive);
				ReInput.ControllerConnectedEvent += LazyInput.OnGamepadConnected;
				ReInput.ControllerDisconnectedEvent += LazyInput.OnGamepadDisconnected;
				LazyInput.isInitialized = true;
				if (LazyInput.instance.controllerIconLibrary != null)
				{
					LazyInput.instance.controllerIconLibrary.Init();
				}
				if (LazyInput.dontDestroyOnLoad)
				{
					global::UnityEngine.Object.DontDestroyOnLoad(LazyInput.instance);
				}
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001F122 File Offset: 0x0001D322
		public void SetConfigurationReferences(GameBindings gameBindings, GamepadTypeData gamepadTypeData, ControllerIconLibrary controllerIconLibrary)
		{
			this.gameBindings = gameBindings;
			this.gamepadTypeConfiguration = gamepadTypeData;
			this.controllerIconLibrary = controllerIconLibrary;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001F13C File Offset: 0x0001D33C
		private void Update()
		{
			if (!LazyInput.isInitialized)
			{
				return;
			}
			LazyInput.keyboard.EraseMouseControl();
			if (!LazyInput.isInputActive)
			{
				return;
			}
			LazyInput.gamepad.Update();
			if (!LazyInput.isGamepadActivityForcedByPlatform)
			{
				LazyInput.keyboard.Update();
			}
			this.pressedKeys.Clear();
			this.holdedKeys.Clear();
			bool flag = false;
			if (LazyInput.isGamepadActivityForcedByPlatform)
			{
				flag = !LazyInput.isGamepadActive;
				LazyInput.isGamepadActive = true;
			}
			else if (LazyInput.gamepad.IsActive())
			{
				flag = !LazyInput.isGamepadActive;
				LazyInput.isGamepadActive = true;
			}
			else if (LazyInput.keyboard.IsActive())
			{
				flag = LazyInput.isGamepadActive;
				LazyInput.isGamepadActive = false;
			}
			if (LazyInput.IsGamepadActive && LazyInput.gamepad.IsActive())
			{
				this.CheckGamepadChange();
			}
			if (flag)
			{
				Action onInputChanged = LazyInput.OnInputChanged;
				if (onInputChanged != null)
				{
					onInputChanged();
				}
			}
			BaseInputController baseInputController = this.DefineActiveInput();
			foreach (GameKey gameKey in baseInputController.pressedKeys)
			{
				this.AddPressed(gameKey);
			}
			foreach (GameKey gameKey2 in baseInputController.holdedKeys)
			{
				this.AddHolded(gameKey2);
			}
			this.direction = baseInputController.Direction;
			this.direction2 = baseInputController.Direction2;
			this.CheckIgnoreUntilReleaseKeys();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
		private BaseInputController DefineActiveInput()
		{
			if (LazyInput.isGamepadActive)
			{
				return LazyInput.gamepad;
			}
			return LazyInput.keyboard;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001F2D4 File Offset: 0x0001D4D4
		private void AddPressed(GameKey key)
		{
			if (key == GameKey.None || this.pressedKeys.Contains(key) || this.ignoreUntilRelease.Contains(key))
			{
				return;
			}
			this.pressedKeys.Add(key);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001F30C File Offset: 0x0001D50C
		private void AddHolded(GameKey key)
		{
			if (key == GameKey.None || this.holdedKeys.Contains(key))
			{
				return;
			}
			this.holdedKeys.Add(key);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001F338 File Offset: 0x0001D538
		private void CheckIgnoreUntilReleaseKeys()
		{
			for (int i = 0; i < this.ignoreUntilRelease.Count; i++)
			{
				if (this.holdedKeys.Contains(this.ignoreUntilRelease[i]))
				{
					this.holdedKeys.Remove(this.ignoreUntilRelease[i]);
				}
				else
				{
					this.ignoreUntilRelease.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001F39E File Offset: 0x0001D59E
		private static bool ShouldActivateGamepadAtStartByPlatform
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001F3A4 File Offset: 0x0001D5A4
		private bool CheckGamepadChange()
		{
			if (!ReInput.isReady)
			{
				return false;
			}
			this.rewiredPlayer = ReInput.players.GetPlayer(0);
			if (this.rewiredPlayer == null)
			{
				return false;
			}
			Controller lastActiveController = this.rewiredPlayer.controllers.GetLastActiveController();
			if (lastActiveController != this.currentGamepad)
			{
				bool flag;
				LazyInput.OnActiveGamepadChanged(lastActiveController, out flag, true);
				return flag;
			}
			return false;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001F3FC File Offset: 0x0001D5FC
		private static void TryCacheConnectedGamepadType(bool notify)
		{
			if (!ReInput.isReady || LazyInput.instance.rewiredPlayer == null)
			{
				return;
			}
			Controller controller = null;
			if (notify)
			{
				controller = LazyInput.instance.rewiredPlayer.controllers.GetController(ControllerType.Joystick, 0);
			}
			else if (LazyInput.instance.rewiredPlayer.controllers.joystickCount > 0)
			{
				controller = LazyInput.instance.rewiredPlayer.controllers.Joysticks[0];
			}
			else if (ReInput.controllers.joystickCount > 0)
			{
				controller = ReInput.controllers.Joysticks[0];
			}
			bool flag;
			LazyInput.OnActiveGamepadChanged(controller, out flag, notify);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001F498 File Offset: 0x0001D698
		private static void OnGamepadConnected(ControllerStatusChangedEventArgs args)
		{
			bool flag = !LazyInput.isGamepadActive;
			LazyInput.isGamepadActive = true;
			bool flag2;
			LazyInput.OnActiveGamepadChanged(args.controller, out flag2, true);
			if (flag && !flag2)
			{
				Action onInputChanged = LazyInput.OnInputChanged;
				if (onInputChanged == null)
				{
					return;
				}
				onInputChanged();
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
		private static void OnGamepadDisconnected(ControllerStatusChangedEventArgs args)
		{
			bool flag = LazyInput.isGamepadActive;
			LazyInput.isGamepadActive = LazyInput.isGamepadActivityForcedByPlatform || ReInput.controllers.joystickCount > 0;
			if (flag != LazyInput.isGamepadActive)
			{
				Action onInputChanged = LazyInput.OnInputChanged;
				if (onInputChanged != null)
				{
					onInputChanged();
				}
			}
			if (LazyInput.isGamepadActive)
			{
				if (!ReInput.isReady)
				{
					return;
				}
				LazyInput.instance.rewiredPlayer = ReInput.players.GetPlayer(0);
				if (LazyInput.instance.rewiredPlayer == null)
				{
					return;
				}
				bool flag2;
				LazyInput.OnActiveGamepadChanged(LazyInput.instance.rewiredPlayer.controllers.GetLastActiveController(), out flag2, true);
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001F56C File Offset: 0x0001D76C
		private static void OnActiveGamepadChanged(Controller controller, out bool gamepadTypeChanged, bool notify = true)
		{
			if (controller == null)
			{
				gamepadTypeChanged = false;
				return;
			}
			GamepadType gamepadType = LazyInput.instance.currentGamepadType;
			LazyInput.instance.currentGamepad = controller;
			LazyInput.instance.currentGamepadType = LazyInput.instance.gamepadTypeConfiguration.GetTypeByGuid(controller.hardwareTypeGuid);
			gamepadTypeChanged = gamepadType != LazyInput.instance.currentGamepadType;
			if (gamepadTypeChanged && notify)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Active gamepad changed:[",
					controller.name,
					"] type:[",
					LazyInput.instance.currentGamepadType.value.ToString(),
					"]"
				}));
				Action onActiveGamepadChangedEvent = LazyInput.OnActiveGamepadChangedEvent;
				if (onActiveGamepadChangedEvent != null)
				{
					onActiveGamepadChangedEvent();
				}
				Action onInputChanged = LazyInput.OnInputChanged;
				if (onInputChanged == null)
				{
					return;
				}
				onInputChanged();
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001F634 File Offset: 0x0001D834
		public static bool GetKey(GameKey key)
		{
			return !LazyInput.Instance.ignoreUntilRelease.Contains(key) && LazyInput.Instance.holdedKeys.Contains(key);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001F65A File Offset: 0x0001D85A
		public static bool AnyKeyDown()
		{
			return LazyInput.Instance.pressedKeys.Count > 0;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001F66E File Offset: 0x0001D86E
		public static bool AnyKeyDownExceptSticks()
		{
			if (LazyInput.IsGamepadActive)
			{
				return LazyInput.gamepad.AnyKeyDownExceptSticks();
			}
			return LazyInput.AnyKeyDown();
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001F687 File Offset: 0x0001D887
		public static bool GetKeyDown(GameKey key)
		{
			return !LazyInput.Instance.ignoreUntilRelease.Contains(key) && LazyInput.Instance.pressedKeys.Contains(key);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001F6AD File Offset: 0x0001D8AD
		public static Vector2 GetDirection()
		{
			return LazyInput.Instance.direction;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001F6B9 File Offset: 0x0001D8B9
		public static Vector2 GetDirection2()
		{
			return LazyInput.Instance.direction2;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001F6C5 File Offset: 0x0001D8C5
		public static float MouseScrollDelta()
		{
			return LazyInput.keyboard.MouseScrollDelta;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001F6D1 File Offset: 0x0001D8D1
		public static void ClearKey(GameKey key)
		{
			LazyInput.Instance.holdedKeys.Remove(key);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001F6E4 File Offset: 0x0001D8E4
		public static void ClearKeyDown(GameKey key)
		{
			LazyInput.Instance.pressedKeys.Remove(key);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001F6F7 File Offset: 0x0001D8F7
		public static void ClearAllKeysDown()
		{
			LazyInput.Instance.pressedKeys.Clear();
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001F708 File Offset: 0x0001D908
		public static void WaitForRelease(GameKey key)
		{
			if (key == GameKey.None)
			{
				return;
			}
			if (!LazyInput.Instance.ignoreUntilRelease.Contains(key))
			{
				LazyInput.Instance.ignoreUntilRelease.Add(key);
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001F73A File Offset: 0x0001D93A
		public static void SetInputActivity(bool isInputActive)
		{
			LazyInput.isInputActive = isInputActive;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001F742 File Offset: 0x0001D942
		public static bool IsInputActive()
		{
			return LazyInput.isInputActive;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001F749 File Offset: 0x0001D949
		public static void SetKeyboardMovementType(KeyboardController.MovementType movementType)
		{
			LazyInput.keyboard.CurrentMovementType = movementType;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001F756 File Offset: 0x0001D956
		public static KeyboardController.MovementType GetKeyboardMovementType()
		{
			return LazyInput.keyboard.CurrentMovementType;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001F762 File Offset: 0x0001D962
		public static void Vibrate(float value, float duration)
		{
			LazyInput.gamepad.Vibrate(value, duration);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001F770 File Offset: 0x0001D970
		public static bool AnyClick()
		{
			return LazyInput.GetKeyDown(GameKey.LeftClick) || LazyInput.GetKeyDown(GameKey.RightClick);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001F78A File Offset: 0x0001D98A
		public static void ForceGamepadActivityState(bool isActive)
		{
			LazyInput.isStateForced = true;
			LazyInput.forcedState = isActive;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001F798 File Offset: 0x0001D998
		public static void SetGamepadActivityForcedByPlatform(bool isForced)
		{
			bool flag = LazyInput.IsGamepadActive;
			LazyInput.isGamepadActivityForcedByPlatform = isForced;
			if (LazyInput.isGamepadActivityForcedByPlatform)
			{
				LazyInput.isGamepadActive = true;
			}
			if (flag != LazyInput.IsGamepadActive)
			{
				Action onInputChanged = LazyInput.OnInputChanged;
				if (onInputChanged == null)
				{
					return;
				}
				onInputChanged();
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001F7C8 File Offset: 0x0001D9C8
		public static void ClearGamepadActivityState()
		{
			if (!LazyInput.isStateForced)
			{
				return;
			}
			bool flag = LazyInput.IsGamepadActive;
			LazyInput.isStateForced = false;
			if (flag != LazyInput.IsGamepadActive)
			{
				Action onInputChanged = LazyInput.OnInputChanged;
				if (onInputChanged == null)
				{
					return;
				}
				onInputChanged();
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001F7F3 File Offset: 0x0001D9F3
		public static void NotifyInputChanged()
		{
			Action onInputChanged = LazyInput.OnInputChanged;
			if (onInputChanged == null)
			{
				return;
			}
			onInputChanged();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001F804 File Offset: 0x0001DA04
		public static void ForceDebugGamepadType(GamepadType gamepadType)
		{
			LazyInput.debugGamepadTypeForced = true;
			LazyInput.forcedGamepadType = gamepadType;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001F812 File Offset: 0x0001DA12
		public static void ClearForcedGamepadType()
		{
			LazyInput.debugGamepadTypeForced = false;
		}

		// Token: 0x04000381 RID: 897
		private static bool isInitialized;

		// Token: 0x04000382 RID: 898
		private static bool isGamepadActive = false;

		// Token: 0x04000383 RID: 899
		private static bool isInputActive = true;

		// Token: 0x04000384 RID: 900
		private static bool isStateForced;

		// Token: 0x04000385 RID: 901
		private static bool forcedState;

		// Token: 0x04000386 RID: 902
		private static bool isGamepadActivityForcedByPlatform;

		// Token: 0x04000387 RID: 903
		private static bool dontDestroyOnLoad;

		// Token: 0x04000388 RID: 904
		private static LazyInput instance;

		// Token: 0x04000389 RID: 905
		private static GamepadController gamepad;

		// Token: 0x0400038A RID: 906
		private static KeyboardController keyboard;

		// Token: 0x0400038B RID: 907
		private List<GameKey> holdedKeys = new List<GameKey>();

		// Token: 0x0400038C RID: 908
		private List<GameKey> pressedKeys = new List<GameKey>();

		// Token: 0x0400038D RID: 909
		private List<GameKey> ignoreUntilRelease = new List<GameKey>();

		// Token: 0x0400038E RID: 910
		private Vector2 direction = Vector2.zero;

		// Token: 0x0400038F RID: 911
		private Vector2 direction2 = Vector2.zero;

		// Token: 0x04000390 RID: 912
		[SerializeField]
		private GameBindings gameBindings;

		// Token: 0x04000391 RID: 913
		[SerializeField]
		private GamepadTypeData gamepadTypeConfiguration;

		// Token: 0x04000392 RID: 914
		private static bool debugGamepadTypeForced;

		// Token: 0x04000393 RID: 915
		private static GamepadType forcedGamepadType;

		// Token: 0x04000394 RID: 916
		private GamepadType currentGamepadType;

		// Token: 0x04000395 RID: 917
		[SerializeField]
		private ControllerIconLibrary controllerIconLibrary;

		// Token: 0x04000396 RID: 918
		private Controller currentGamepad;

		// Token: 0x04000397 RID: 919
		private Player rewiredPlayer;
	}
}
