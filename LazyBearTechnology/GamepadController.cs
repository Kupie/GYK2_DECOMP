using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000127 RID: 295
	public class GamepadController : BaseInputController
	{
		// Token: 0x060005DA RID: 1498 RVA: 0x0001D8F8 File Offset: 0x0001BAF8
		public GamepadController(GameBindings gameBindings)
		{
			this.gameBindings = gameBindings;
			this.gamepadBindings = gameBindings.gamepadBindings;
			this.rewiredPlayer = ReInput.players.GetPlayer(0);
			this.stick = new Stick(0, 1);
			this.rightStick = new Stick(16, 17);
			this.verticalNavigation = new NavigationStick(true, this);
			this.horizontalNavigation = new NavigationStick(false, this);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001DA5C File Offset: 0x0001BC5C
		public override void Update()
		{
			base.Update();
			if (!ReInput.isReady)
			{
				return;
			}
			this.rewiredPlayer = ReInput.players.GetPlayer(0);
			this.stick.Update(this.rewiredPlayer);
			this.rightStick.Update(this.rewiredPlayer);
			this.direction = (this.stick.HasDirection ? this.stick.direction : Vector2.zero);
			this.direction2 = (this.rightStick.HasDirection ? this.rightStick.direction : Vector2.zero);
			bool flag = this.ShouldReplaceBackWithSonyTouchpad();
			bool flag2 = flag && this.IsSonyTouchpadButtonDown();
			bool flag3 = flag && this.IsSonyTouchpadButtonHeld();
			for (int i = 0; i < this.gamepadBindings.Count; i++)
			{
				GameKey gameKey = this.gamepadBindings[i].gameKey;
				GamepadButton gamepadButton = this.gamepadBindings[i].gamepadButton;
				int num = this.rewiredBindings[gamepadButton];
				bool flag4;
				bool flag5;
				if (gamepadButton == GamepadButton.Back && flag)
				{
					flag4 = flag2;
					flag5 = flag3;
				}
				else
				{
					flag4 = this.rewiredPlayer.GetButtonDown(num);
					flag5 = this.rewiredPlayer.GetButton(num);
				}
				if (flag4)
				{
					this.HandlePressing(gameKey);
				}
				if (flag5)
				{
					this.HandleHolding(gameKey);
				}
			}
			this.UpdateStickNavigation(this.direction);
			this.UpdateHolded(Time.deltaTime);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001DBCC File Offset: 0x0001BDCC
		private void UpdateHolded(float deltaTime)
		{
			for (int i = 0; i < this.holdedForRepeatPress.Count; i++)
			{
				GameKey gameKey = this.holdedForRepeatPress[i];
				if (!this.holdedKeys.Contains(gameKey))
				{
					this.holdedForRepeatPress.RemoveAt(i);
					this.holdedForRepeatPressDelays.RemoveAt(i);
					i--;
					HoldedGroup group = this.GetGroup(gameKey);
					if (group != null)
					{
						if (this.lastReleasesInGroup.ContainsKey(group.groupType))
						{
							this.lastReleasesInGroup[group.groupType] = Time.time;
						}
						else
						{
							this.lastReleasesInGroup.Add(group.groupType, Time.time);
						}
					}
				}
				else
				{
					List<float> list = this.holdedForRepeatPressDelays;
					int num = i;
					list[num] -= deltaTime;
					if (this.holdedForRepeatPressDelays[i] <= 0f)
					{
						this.HandlePressing(gameKey);
						HoldedGroup group2 = this.GetGroup(gameKey);
						this.holdedForRepeatPressDelays[i] = group2.timeRepeatPeriod;
					}
				}
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001DCD0 File Offset: 0x0001BED0
		private void UpdateStickNavigation(Vector2 guiNavigation)
		{
			if (guiNavigation.magnitude > 0f)
			{
				GamepadController.GuiNavigationAxis guiNavigationAxis;
				if (Mathf.Abs(guiNavigation.x) > Mathf.Abs(guiNavigation.y))
				{
					guiNavigation.y = 0f;
					guiNavigationAxis = GamepadController.GuiNavigationAxis.Horizontal;
				}
				else
				{
					guiNavigation.x = 0f;
					guiNavigationAxis = GamepadController.GuiNavigationAxis.Vertical;
				}
				if (this.lastGuiNavigationAxis != GamepadController.GuiNavigationAxis.None && this.lastGuiNavigationAxis != guiNavigationAxis)
				{
					this.guiNavigationDelay = 0.11f;
				}
				this.lastGuiNavigationAxis = guiNavigationAxis;
				if (this.guiNavigationDelay > 0f)
				{
					guiNavigation = Vector2.zero;
				}
				this.guiNavigationDelay -= LazyTime.GetUnscaledDeltaTime;
			}
			else
			{
				this.lastGuiNavigationAxis = GamepadController.GuiNavigationAxis.None;
			}
			this.verticalNavigation.Update(guiNavigation);
			this.horizontalNavigation.Update(guiNavigation);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001DD90 File Offset: 0x0001BF90
		public void HandlePressing(GameKey key)
		{
			this.pressedKeys.Add(key);
			List<BindingAlias> bindingAliases = this.gameBindings.bindingAliases;
			for (int i = 0; i < bindingAliases.Count; i++)
			{
				BindingAlias bindingAlias = bindingAliases[i];
				if (key == bindingAlias.gameKey1)
				{
					this.pressedKeys.Add(bindingAlias.gameKey2);
				}
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001DDF0 File Offset: 0x0001BFF0
		public bool AnyKeyDownExceptSticks()
		{
			bool flag = this.ShouldReplaceBackWithSonyTouchpad();
			foreach (KeyValuePair<GamepadButton, int> keyValuePair in this.rewiredBindings)
			{
				if ((!flag || !(keyValuePair.Key == GamepadButton.Back)) && this.rewiredPlayer.GetButtonDown(keyValuePair.Value))
				{
					return true;
				}
			}
			return flag && this.IsSonyTouchpadButtonDown();
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001DE84 File Offset: 0x0001C084
		private bool IsSonyTouchpadButtonDown()
		{
			return this.IsSonyTouchpadButtonState(true);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001DE8D File Offset: 0x0001C08D
		private bool IsSonyTouchpadButtonHeld()
		{
			return this.IsSonyTouchpadButtonState(false);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001DE98 File Offset: 0x0001C098
		private bool IsSonyTouchpadButtonState(bool justPressed)
		{
			if (this.rewiredPlayer == null || !GamepadController.IsSonyTouchpadOverrideEnabled())
			{
				return false;
			}
			foreach (Joystick joystick in this.rewiredPlayer.controllers.Joysticks)
			{
				if (GamepadController.IsSonyDualShockOrDualSense(joystick))
				{
					IList<ControllerElementIdentifier> buttonElementIdentifiers = joystick.ButtonElementIdentifiers;
					for (int i = 0; i < buttonElementIdentifiers.Count; i++)
					{
						ControllerElementIdentifier controllerElementIdentifier = buttonElementIdentifiers[i];
						if (GamepadController.IsSonyTouchpadClickElement(controllerElementIdentifier))
						{
							return justPressed ? joystick.GetButtonDownById(controllerElementIdentifier.id) : joystick.GetButtonById(controllerElementIdentifier.id);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001DF54 File Offset: 0x0001C154
		private bool ShouldReplaceBackWithSonyTouchpad()
		{
			if (!GamepadController.IsSonyTouchpadOverrideEnabled() || this.rewiredPlayer == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			using (IEnumerator<Joystick> enumerator = this.rewiredPlayer.controllers.Joysticks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (GamepadController.IsSonyDualShockOrDualSense(enumerator.Current))
					{
						flag = true;
					}
					else
					{
						flag2 = true;
					}
				}
			}
			return flag && (!flag2 || GamepadController.IsSonyDualShockOrDualSense(this.rewiredPlayer.controllers.GetLastActiveController() as Joystick));
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001DFEC File Offset: 0x0001C1EC
		private static bool IsSonyTouchpadOverrideEnabled()
		{
			return true;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001DFF0 File Offset: 0x0001C1F0
		private static bool IsSonyDualShockOrDualSense(Joystick joystick)
		{
			if (joystick == null)
			{
				return false;
			}
			GamepadTypeData instance = LazySingletonSO<GamepadTypeData>.Instance;
			if (instance == null)
			{
				return false;
			}
			GamepadType typeByGuid = instance.GetTypeByGuid(joystick.hardwareTypeGuid);
			return typeByGuid == GamepadType.Sony_DualShock || typeByGuid == GamepadType.Sony_DualSense;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001E03C File Offset: 0x0001C23C
		private static bool IsSonyTouchpadClickElement(ControllerElementIdentifier identifier)
		{
			if (identifier == null)
			{
				return false;
			}
			if (identifier.role == "gamepad/touchpad/press")
			{
				return true;
			}
			string key = identifier.key;
			return key == "touchpad_button" || key == "touchpad/button";
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001E084 File Offset: 0x0001C284
		public void HandleHolding(GameKey key)
		{
			if (!this.holdedKeys.Contains(key))
			{
				this.holdedKeys.Add(key);
			}
			List<BindingAlias> bindingAliases = this.gameBindings.bindingAliases;
			for (int i = 0; i < bindingAliases.Count; i++)
			{
				BindingAlias bindingAlias = bindingAliases[i];
				if (key == bindingAlias.gameKey1)
				{
					this.holdedKeys.Add(bindingAlias.gameKey2);
				}
			}
			if (!this.CanBeHoldedForRepeatPress(key) || this.holdedForRepeatPress.Contains(key))
			{
				return;
			}
			this.holdedForRepeatPress.Add(key);
			HoldedGroup group = this.GetGroup(key);
			float num = 0f;
			foreach (float num2 in this.lastReleasesInGroup.Values)
			{
				if (num == 0f)
				{
					num = num2;
				}
				else if (num2 > num)
				{
					num = num2;
				}
			}
			this.holdedForRepeatPressDelays.Add((Time.time - num < group.timeBeforeRepeat) ? group.timeRepeatPeriod : group.timeBeforeRepeat);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001E1A8 File Offset: 0x0001C3A8
		private bool CanBeHoldedForRepeatPress(GameKey key)
		{
			List<HoldableElement> canBeHoldedForRepeatPress = this.gameBindings.canBeHoldedForRepeatPress;
			for (int i = 0; i < canBeHoldedForRepeatPress.Count; i++)
			{
				if (canBeHoldedForRepeatPress[i].gameKey == key)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001E1EC File Offset: 0x0001C3EC
		private HoldedGroup GetGroup(GameKey key)
		{
			List<HoldableElement> canBeHoldedForRepeatPress = this.gameBindings.canBeHoldedForRepeatPress;
			for (int i = 0; i < canBeHoldedForRepeatPress.Count; i++)
			{
				if (canBeHoldedForRepeatPress[i].gameKey == key)
				{
					HoldedGroupType groupType = canBeHoldedForRepeatPress[i].groupType;
					List<HoldedGroup> holdedGroups = this.gameBindings.holdedGroups;
					for (int j = 0; j < holdedGroups.Count; j++)
					{
						if (holdedGroups[j].groupType == groupType)
						{
							return holdedGroups[j];
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001E274 File Offset: 0x0001C474
		public void Vibrate(float value, float duration)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.rewiredPlayer = ReInput.players.GetPlayer(0);
			if (this.rewiredPlayer == null)
			{
				return;
			}
			foreach (Joystick joystick in this.rewiredPlayer.controllers.Joysticks)
			{
				if (joystick.supportsVibration)
				{
					joystick.SetVibration(value, value, duration, duration);
				}
			}
		}

		// Token: 0x040002E3 RID: 739
		private Dictionary<GamepadButton, int> rewiredBindings = new Dictionary<GamepadButton, int>
		{
			{
				GamepadButton.X,
				2
			},
			{
				GamepadButton.Y,
				3
			},
			{
				GamepadButton.A,
				4
			},
			{
				GamepadButton.B,
				5
			},
			{
				GamepadButton.LB,
				6
			},
			{
				GamepadButton.RB,
				7
			},
			{
				GamepadButton.LT,
				8
			},
			{
				GamepadButton.RT,
				9
			},
			{
				GamepadButton.Back,
				10
			},
			{
				GamepadButton.Start,
				11
			},
			{
				GamepadButton.DUp,
				12
			},
			{
				GamepadButton.DDown,
				13
			},
			{
				GamepadButton.DLeft,
				14
			},
			{
				GamepadButton.DRight,
				15
			},
			{
				GamepadButton.RStick,
				19
			},
			{
				GamepadButton.LStick,
				20
			}
		};

		// Token: 0x040002E4 RID: 740
		private List<GamepadBinding> gamepadBindings;

		// Token: 0x040002E5 RID: 741
		private GameBindings gameBindings;

		// Token: 0x040002E6 RID: 742
		private Stick stick;

		// Token: 0x040002E7 RID: 743
		private Stick rightStick;

		// Token: 0x040002E8 RID: 744
		private Player rewiredPlayer;

		// Token: 0x040002E9 RID: 745
		private NavigationStick verticalNavigation;

		// Token: 0x040002EA RID: 746
		private NavigationStick horizontalNavigation;

		// Token: 0x040002EB RID: 747
		private float guiNavigationDelay;

		// Token: 0x040002EC RID: 748
		private GamepadController.GuiNavigationAxis lastGuiNavigationAxis;

		// Token: 0x040002ED RID: 749
		private const string SonyTouchpadRole = "gamepad/touchpad/press";

		// Token: 0x040002EE RID: 750
		private const string SonyTouchpadKeyDualShock = "touchpad_button";

		// Token: 0x040002EF RID: 751
		private const string SonyTouchpadKeyDualSense = "touchpad/button";

		// Token: 0x040002F0 RID: 752
		private List<GameKey> holdedForRepeatPress = new List<GameKey>();

		// Token: 0x040002F1 RID: 753
		private List<float> holdedForRepeatPressDelays = new List<float>();

		// Token: 0x040002F2 RID: 754
		private Dictionary<HoldedGroupType, float> lastReleasesInGroup = new Dictionary<HoldedGroupType, float>();

		// Token: 0x020001EA RID: 490
		private enum GuiNavigationAxis
		{
			// Token: 0x04000667 RID: 1639
			None,
			// Token: 0x04000668 RID: 1640
			Vertical,
			// Token: 0x04000669 RID: 1641
			Horizontal
		}
	}
}
