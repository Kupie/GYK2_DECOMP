using System;
using System.Collections.Generic;
using Rewired.Interfaces;
using Rewired.Platforms.Custom;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x02000125 RID: 293
	public sealed class MyPlatformInputSource : CustomPlatformInputSource
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00027599 File Offset: 0x00025799
		public override bool isReady
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x000275A1 File Offset: 0x000257A1
		public MyPlatformInputSource(CustomPlatformConfigVars configVars)
			: base(configVars, new CustomPlatformInputSource.InitOptions
			{
				unifiedKeyboardSource = new MyPlatformUnifiedKeyboardSource(),
				unifiedMouseSource = new MyPlatformUnifiedMouseSource()
			})
		{
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000275D0 File Offset: 0x000257D0
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._initialized = true;
			this.MonitorDeviceChanges();
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000275E5 File Offset: 0x000257E5
		public override void Update()
		{
			this._joystickInputSource.Update();
			this.MonitorDeviceChanges();
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x000275F8 File Offset: 0x000257F8
		private void MonitorDeviceChanges()
		{
			IList<CustomInputSource.Joystick> joysticks = base.GetJoysticks();
			IList<UnityInputJoystickSource.Joystick> joysticks2 = this._joystickInputSource.GetJoysticks();
			for (int i = joysticks.Count - 1; i >= 0; i--)
			{
				MyPlatformInputSource.Joystick joystick = joysticks[i] as MyPlatformInputSource.Joystick;
				if (!MyPlatformInputSource.ContainsSystemJoystickBySystemId(joysticks2, joystick.sourceJoystick.systemId))
				{
					base.RemoveJoystick(joystick);
				}
			}
			for (int j = 0; j < joysticks2.Count; j++)
			{
				UnityInputJoystickSource.Joystick joystick2 = joysticks2[j];
				if (!MyPlatformInputSource.ContainsJoystickBySystemId(joysticks, joystick2.systemId))
				{
					MyPlatformInputSource.Joystick joystick3 = new MyPlatformInputSource.Joystick(joystick2);
					if (joystick2.vibrationMotorCount > 0)
					{
						joystick3.extension = new MyPlatformControllerExtension(joystick3);
					}
					base.AddJoystick(joystick3);
				}
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x000276A9 File Offset: 0x000258A9
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x000276C4 File Offset: 0x000258C4
		private static bool ContainsJoystickBySystemId(IList<CustomInputSource.Joystick> joysticks, long systemId)
		{
			for (int i = 0; i < joysticks.Count; i++)
			{
				long? systemId2 = joysticks[i].systemId;
				if ((systemId2.GetValueOrDefault() == systemId) & (systemId2 != null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00027708 File Offset: 0x00025908
		private static bool ContainsSystemJoystickBySystemId(IList<UnityInputJoystickSource.Joystick> systemJoysticks, long systemId)
		{
			for (int i = 0; i < systemJoysticks.Count; i++)
			{
				if (systemJoysticks[i].systemId == systemId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400071B RID: 1819
		private UnityInputJoystickSource _joystickInputSource = new UnityInputJoystickSource();

		// Token: 0x0400071C RID: 1820
		private bool _initialized;

		// Token: 0x0400071D RID: 1821
		private bool _disposed;

		// Token: 0x02000126 RID: 294
		public new sealed class Joystick : CustomPlatformInputSource.Joystick, IControllerVibrator
		{
			// Token: 0x170004C5 RID: 1221
			// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00027738 File Offset: 0x00025938
			public UnityInputJoystickSource.Joystick sourceJoystick
			{
				get
				{
					return this._sourceJoystick;
				}
			}

			// Token: 0x06000D61 RID: 3425 RVA: 0x00027740 File Offset: 0x00025940
			public Joystick(UnityInputJoystickSource.Joystick sourceJoystick)
				: base(sourceJoystick.deviceName, sourceJoystick.systemId, sourceJoystick.axisCount, sourceJoystick.buttonCount)
			{
				if (sourceJoystick == null)
				{
					throw new ArgumentNullException("sourceJoystick");
				}
				this._sourceJoystick = sourceJoystick;
				base.customIdentifier = this._sourceJoystick.identifier;
				base.deviceInstanceGuid = sourceJoystick.deviceInstanceGuid;
			}

			// Token: 0x06000D62 RID: 3426 RVA: 0x000277A4 File Offset: 0x000259A4
			public override void Update()
			{
				for (int i = 0; i < base.buttonCount; i++)
				{
					this.SetButtonValue(i, this.sourceJoystick.GetButtonValue(i));
				}
				for (int j = 0; j < base.axisCount; j++)
				{
					this.SetAxisValue(j, this.sourceJoystick.GetAxisValue(j));
				}
			}

			// Token: 0x170004C6 RID: 1222
			// (get) Token: 0x06000D63 RID: 3427 RVA: 0x000277F9 File Offset: 0x000259F9
			public int vibrationMotorCount
			{
				get
				{
					return this._sourceJoystick.vibrationMotorCount;
				}
			}

			// Token: 0x06000D64 RID: 3428 RVA: 0x00027806 File Offset: 0x00025A06
			public void SetVibration(int motorIndex, float motorLevel)
			{
				this._sourceJoystick.SetVibration(motorIndex, motorLevel);
			}

			// Token: 0x06000D65 RID: 3429 RVA: 0x00027815 File Offset: 0x00025A15
			public void SetVibration(int motorIndex, float motorLevel, float duration)
			{
				this._sourceJoystick.SetVibration(motorIndex, motorLevel, duration);
			}

			// Token: 0x06000D66 RID: 3430 RVA: 0x00027825 File Offset: 0x00025A25
			public void SetVibration(int motorIndex, float motorLevel, bool stopOtherMotors)
			{
				this._sourceJoystick.SetVibration(motorIndex, motorLevel, stopOtherMotors);
			}

			// Token: 0x06000D67 RID: 3431 RVA: 0x00027835 File Offset: 0x00025A35
			public void SetVibration(int motorIndex, float motorLevel, float duration, bool stopOtherMotors)
			{
				this._sourceJoystick.SetVibration(motorIndex, motorLevel, duration, stopOtherMotors);
			}

			// Token: 0x06000D68 RID: 3432 RVA: 0x00027847 File Offset: 0x00025A47
			public float GetVibration(int motorIndex)
			{
				return this._sourceJoystick.GetVibration(motorIndex);
			}

			// Token: 0x06000D69 RID: 3433 RVA: 0x00027855 File Offset: 0x00025A55
			public void StopVibration()
			{
				this._sourceJoystick.StopVibration();
			}

			// Token: 0x0400071E RID: 1822
			private UnityInputJoystickSource.Joystick _sourceJoystick;
		}
	}
}
