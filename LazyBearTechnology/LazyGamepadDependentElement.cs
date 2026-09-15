using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000111 RID: 273
	public class LazyGamepadDependentElement : MonoBehaviour
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0001CBA0 File Offset: 0x0001ADA0
		public void UpdateState()
		{
			if (!base.enabled)
			{
				return;
			}
			switch (Platform.Type)
			{
			case PlatformType.PС:
				if (!LazyInput.IsGamepadActive)
				{
					base.gameObject.SetActive(false);
					return;
				}
				if (!(LazyInput.CurrentGamepadType == GamepadType.Sony_DualShock) && !(LazyInput.CurrentGamepadType == GamepadType.Sony_DualSense))
				{
					base.gameObject.SetActive(this.anyGamepad || this.gamepadXbox);
					return;
				}
				if (LazyInput.CurrentGamepadType == GamepadType.Sony_DualShock)
				{
					base.gameObject.SetActive(this.anyGamepad || (this.isDualShock && this.gamepadPs));
					return;
				}
				base.gameObject.SetActive(this.anyGamepad || (!this.isDualShock && this.gamepadPs));
				return;
			case PlatformType.XBox:
				if (LazyInput.IsGamepadActive)
				{
					base.gameObject.SetActive(this.anyGamepad || this.gamepadXbox);
					return;
				}
				base.gameObject.SetActive(false);
				return;
			case PlatformType.PlayStation:
				if (!this.gamepadPs)
				{
					base.gameObject.SetActive(this.anyGamepad);
					return;
				}
				if (this.anyGamepad)
				{
					base.gameObject.SetActive(true);
					return;
				}
				if (LazyInput.CurrentGamepadType == GamepadType.Sony_DualShock)
				{
					base.gameObject.SetActive(this.isDualShock && this.gamepadPs);
					return;
				}
				base.gameObject.SetActive(!this.isDualShock && this.gamepadPs);
				return;
			case PlatformType.Switch:
			case PlatformType.Switch2:
				base.gameObject.SetActive(this.anyGamepad || this.gamepadSwitch);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000289 RID: 649
		public bool anyGamepad;

		// Token: 0x0400028A RID: 650
		public bool gamepadXbox;

		// Token: 0x0400028B RID: 651
		public bool gamepadPs;

		// Token: 0x0400028C RID: 652
		public bool gamepadSwitch;

		// Token: 0x0400028D RID: 653
		public bool isDualShock;
	}
}
