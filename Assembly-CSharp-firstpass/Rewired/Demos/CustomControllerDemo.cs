using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020000F6 RID: 246
	[AddComponentMenu("")]
	public class CustomControllerDemo : MonoBehaviour
	{
		// Token: 0x06000C57 RID: 3159 RVA: 0x00022FC0 File Offset: 0x000211C0
		private void Awake()
		{
			ScreenOrientation screenOrientation = ScreenOrientation.LandscapeLeft;
			if (SystemInfo.deviceType == DeviceType.Handheld && Screen.orientation != screenOrientation)
			{
				Screen.orientation = screenOrientation;
			}
			this.Initialize();
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00022FEC File Offset: 0x000211EC
		private void Initialize()
		{
			ReInput.InputSourceUpdateEvent += this.OnInputSourceUpdate;
			this.joysticks = base.GetComponentsInChildren<TouchJoystickExample>();
			this.buttons = base.GetComponentsInChildren<TouchButtonExample>();
			this.axisCount = this.joysticks.Length * 2;
			this.buttonCount = this.buttons.Length;
			this.axisValues = new float[this.axisCount];
			this.buttonValues = new bool[this.buttonCount];
			Player player = ReInput.players.GetPlayer(this.playerId);
			this.controller = player.controllers.GetControllerWithTag<CustomController>(this.controllerTag);
			if (this.controller == null)
			{
				Debug.LogError("A matching controller was not found for tag \"" + this.controllerTag + "\"");
			}
			if (this.controller.buttonCount != this.buttonValues.Length || this.controller.axisCount != this.axisValues.Length)
			{
				Debug.LogError("Controller has wrong number of elements!");
			}
			if (this.useUpdateCallbacks && this.controller != null)
			{
				this.controller.SetAxisUpdateCallback(new Func<int, float>(this.GetAxisValueCallback));
				this.controller.SetButtonUpdateCallback(new Func<int, bool>(this.GetButtonValueCallback));
			}
			this.initialized = true;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00023125 File Offset: 0x00021325
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0002313D File Offset: 0x0002133D
		private void OnInputSourceUpdate()
		{
			this.GetSourceAxisValues();
			this.GetSourceButtonValues();
			if (!this.useUpdateCallbacks)
			{
				this.SetControllerAxisValues();
				this.SetControllerButtonValues();
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00023160 File Offset: 0x00021360
		private void GetSourceAxisValues()
		{
			for (int i = 0; i < this.axisValues.Length; i++)
			{
				if (i % 2 != 0)
				{
					this.axisValues[i] = this.joysticks[i / 2].position.y;
				}
				else
				{
					this.axisValues[i] = this.joysticks[i / 2].position.x;
				}
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000231C0 File Offset: 0x000213C0
		private void GetSourceButtonValues()
		{
			for (int i = 0; i < this.buttonValues.Length; i++)
			{
				this.buttonValues[i] = this.buttons[i].isPressed;
			}
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x000231F8 File Offset: 0x000213F8
		private void SetControllerAxisValues()
		{
			for (int i = 0; i < this.axisValues.Length; i++)
			{
				this.controller.SetAxisValue(i, this.axisValues[i]);
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0002322C File Offset: 0x0002142C
		private void SetControllerButtonValues()
		{
			for (int i = 0; i < this.buttonValues.Length; i++)
			{
				this.controller.SetButtonValue(i, this.buttonValues[i]);
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00023260 File Offset: 0x00021460
		private float GetAxisValueCallback(int index)
		{
			if (index >= this.axisValues.Length)
			{
				return 0f;
			}
			return this.axisValues[index];
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0002327B File Offset: 0x0002147B
		private bool GetButtonValueCallback(int index)
		{
			return index < this.buttonValues.Length && this.buttonValues[index];
		}

		// Token: 0x04000634 RID: 1588
		public int playerId;

		// Token: 0x04000635 RID: 1589
		public string controllerTag;

		// Token: 0x04000636 RID: 1590
		public bool useUpdateCallbacks;

		// Token: 0x04000637 RID: 1591
		private int buttonCount;

		// Token: 0x04000638 RID: 1592
		private int axisCount;

		// Token: 0x04000639 RID: 1593
		private float[] axisValues;

		// Token: 0x0400063A RID: 1594
		private bool[] buttonValues;

		// Token: 0x0400063B RID: 1595
		private TouchJoystickExample[] joysticks;

		// Token: 0x0400063C RID: 1596
		private TouchButtonExample[] buttons;

		// Token: 0x0400063D RID: 1597
		private CustomController controller;

		// Token: 0x0400063E RID: 1598
		[NonSerialized]
		private bool initialized;
	}
}
