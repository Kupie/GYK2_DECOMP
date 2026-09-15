using System;
using Rewired.ControllerExtensions;
using Rewired.Interfaces;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x0200011E RID: 286
	public sealed class MyPlatformControllerExtension : CustomControllerExtension, IControllerVibrator
	{
		// Token: 0x06000D42 RID: 3394 RVA: 0x00027427 File Offset: 0x00025627
		public MyPlatformControllerExtension(MyPlatformInputSource.Joystick sourceJoystick)
			: base(new MyPlatformControllerExtension.Source(sourceJoystick))
		{
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00027435 File Offset: 0x00025635
		private MyPlatformControllerExtension(MyPlatformControllerExtension other)
			: base(other)
		{
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0002743E File Offset: 0x0002563E
		public override Controller.Extension ShallowCopy()
		{
			return new MyPlatformControllerExtension(this);
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00027446 File Offset: 0x00025646
		public int vibrationMotorCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00027449 File Offset: 0x00025649
		public void SetVibration(int motorIndex, float motorLevel)
		{
			((MyPlatformControllerExtension.Source)base.GetSource()).sourceJoystick.SetVibration(motorIndex, motorLevel);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00027462 File Offset: 0x00025662
		public void SetVibration(int motorIndex, float motorLevel, float duration)
		{
			((MyPlatformControllerExtension.Source)base.GetSource()).sourceJoystick.SetVibration(motorIndex, motorLevel, duration);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0002747C File Offset: 0x0002567C
		public void SetVibration(int motorIndex, float motorLevel, bool stopOtherMotors)
		{
			((MyPlatformControllerExtension.Source)base.GetSource()).sourceJoystick.SetVibration(motorIndex, motorLevel, stopOtherMotors);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00027496 File Offset: 0x00025696
		public void SetVibration(int motorIndex, float motorLevel, float duration, bool stopOtherMotors)
		{
			((MyPlatformControllerExtension.Source)base.GetSource()).sourceJoystick.SetVibration(motorIndex, motorLevel, duration, stopOtherMotors);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000274B2 File Offset: 0x000256B2
		public float GetVibration(int motorIndex)
		{
			return ((MyPlatformControllerExtension.Source)base.GetSource()).sourceJoystick.GetVibration(motorIndex);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x000274CA File Offset: 0x000256CA
		public void StopVibration()
		{
			((MyPlatformControllerExtension.Source)base.GetSource()).sourceJoystick.StopVibration();
		}

		// Token: 0x0200011F RID: 287
		private class Source : IControllerExtensionSource
		{
			// Token: 0x06000D4C RID: 3404 RVA: 0x000274E1 File Offset: 0x000256E1
			public Source(MyPlatformInputSource.Joystick sourceJoystick)
			{
				this.sourceJoystick = sourceJoystick;
			}

			// Token: 0x04000714 RID: 1812
			public readonly MyPlatformInputSource.Joystick sourceJoystick;
		}
	}
}
