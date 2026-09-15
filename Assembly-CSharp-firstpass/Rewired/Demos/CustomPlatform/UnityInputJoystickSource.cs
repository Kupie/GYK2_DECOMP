using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Interfaces;
using UnityEngine;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x02000129 RID: 297
	public class UnityInputJoystickSource
	{
		// Token: 0x06000D71 RID: 3441 RVA: 0x00027A4B File Offset: 0x00025C4B
		public UnityInputJoystickSource()
		{
			this._joysticks = new List<UnityInputJoystickSource.Joystick>();
			this._joysticks_readOnly = new ReadOnlyCollection<UnityInputJoystickSource.Joystick>(this._joysticks);
			this.RefreshJoysticks();
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00027A81 File Offset: 0x00025C81
		public void Update()
		{
			this.CheckForJoystickChanges();
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00027A89 File Offset: 0x00025C89
		public IList<UnityInputJoystickSource.Joystick> GetJoysticks()
		{
			return this._joysticks_readOnly;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00027A94 File Offset: 0x00025C94
		private void CheckForJoystickChanges()
		{
			double unscaledTime = ReInput.time.unscaledTime;
			if (unscaledTime >= this._nextJoystickCheckTime)
			{
				this._nextJoystickCheckTime = unscaledTime + 1.0;
				if (this.DidJoysticksChange())
				{
					this.RefreshJoysticks();
				}
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00027AD4 File Offset: 0x00025CD4
		private bool DidJoysticksChange()
		{
			string[] joystickNames = Input.GetJoystickNames();
			string[] unityJoysticks = this._unityJoysticks;
			this._unityJoysticks = joystickNames;
			if (unityJoysticks.Length != joystickNames.Length)
			{
				return true;
			}
			for (int i = 0; i < joystickNames.Length; i++)
			{
				if (!string.Equals(unityJoysticks[i], joystickNames[i], StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00027B20 File Offset: 0x00025D20
		private void RefreshJoysticks()
		{
			bool[] array = new bool[this._unityJoysticks.Length];
			for (int i = this._joysticks.Count - 1; i >= 0; i--)
			{
				int unityIndex = this._joysticks[i].unityIndex;
				if (unityIndex >= this._unityJoysticks.Length || !string.Equals(this._joysticks[i].deviceName, this._unityJoysticks[unityIndex]))
				{
					bool flag = false;
					for (int j = this._unityJoysticks.Length - 1; j >= 0; j--)
					{
						if (!array[j] && string.Equals(this._unityJoysticks[j], this._joysticks[i].deviceName))
						{
							this._joysticks[i].unityIndex = j;
							array[j] = true;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Debug.Log(this._joysticks[i].deviceName + " was disconnected.");
						this._joysticks.RemoveAt(i);
					}
				}
				else
				{
					array[unityIndex] = true;
				}
			}
			for (int k = 0; k < this._unityJoysticks.Length; k++)
			{
				if (!array[k] && !string.IsNullOrEmpty(this._unityJoysticks[k]))
				{
					UnityInputJoystickSource.Joystick joystick;
					if (this._unityJoysticks[k].ToLower().Contains("xbox one") || this._unityJoysticks[k].ToLower().Contains("xbox bluetooth"))
					{
						joystick = new UnityInputJoystickSource.Joystick((long)UnityInputJoystickSource.systemIdCounter++, this._unityJoysticks[k], 7, 16);
						joystick.identifier = new MyPlatformControllerIdentifier
						{
							vendorId = 1118,
							productId = 721
						};
						joystick.vibrationMotorCount = 2;
					}
					else
					{
						joystick = new UnityInputJoystickSource.Joystick((long)UnityInputJoystickSource.systemIdCounter++, this._unityJoysticks[k], 10, 20);
					}
					joystick.unityIndex = k;
					Debug.Log(this._unityJoysticks[k] + " was connected.");
					this._joysticks.Add(joystick);
				}
			}
		}

		// Token: 0x04000720 RID: 1824
		private const float joystickCheckInterval = 1f;

		// Token: 0x04000721 RID: 1825
		private static int systemIdCounter;

		// Token: 0x04000722 RID: 1826
		private string[] _unityJoysticks = new string[0];

		// Token: 0x04000723 RID: 1827
		private double _nextJoystickCheckTime;

		// Token: 0x04000724 RID: 1828
		private List<UnityInputJoystickSource.Joystick> _joysticks;

		// Token: 0x04000725 RID: 1829
		private ReadOnlyCollection<UnityInputJoystickSource.Joystick> _joysticks_readOnly;

		// Token: 0x0200012A RID: 298
		public class Joystick : IControllerVibrator
		{
			// Token: 0x06000D77 RID: 3447 RVA: 0x00027D3D File Offset: 0x00025F3D
			public Joystick(long systemId, string deviceName, int axisCount, int buttonCount)
			{
				this.systemId = systemId;
				this.deviceName = deviceName;
				this.axisCount = axisCount;
				this.buttonCount = buttonCount;
				this.axisValues = new float[axisCount];
				this.buttonValues = new bool[buttonCount];
			}

			// Token: 0x06000D78 RID: 3448 RVA: 0x00027D7B File Offset: 0x00025F7B
			public bool GetButtonValue(int index)
			{
				return index < 20 && this.systemId < 8L && Input.GetKey(KeyCode.Joystick1Button0 + this.unityIndex * 20 + index);
			}

			// Token: 0x06000D79 RID: 3449 RVA: 0x00027DA8 File Offset: 0x00025FA8
			public float GetAxisValue(int index)
			{
				if (index >= 10)
				{
					return 0f;
				}
				if (this.systemId >= 8L)
				{
					return 0f;
				}
				return Input.GetAxis("Joy" + (this.unityIndex + 1).ToString() + "Axis" + (index + 1).ToString());
			}

			// Token: 0x170004C8 RID: 1224
			// (get) Token: 0x06000D7A RID: 3450 RVA: 0x00027DFF File Offset: 0x00025FFF
			// (set) Token: 0x06000D7B RID: 3451 RVA: 0x00027E07 File Offset: 0x00026007
			public int vibrationMotorCount { get; set; }

			// Token: 0x06000D7C RID: 3452 RVA: 0x00027E10 File Offset: 0x00026010
			public void SetVibration(int motorIndex, float motorLevel)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Vibrate ",
					this.deviceName,
					": motorIndex: ",
					motorIndex.ToString(),
					", motorLevel: ",
					motorLevel.ToString()
				}));
			}

			// Token: 0x06000D7D RID: 3453 RVA: 0x00027E64 File Offset: 0x00026064
			public void SetVibration(int motorIndex, float motorLevel, float duration)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Vibrate ",
					this.deviceName,
					": motorIndex: ",
					motorIndex.ToString(),
					", motorLevel: ",
					motorLevel.ToString(),
					", duration: ",
					duration.ToString()
				}));
			}

			// Token: 0x06000D7E RID: 3454 RVA: 0x00027EC8 File Offset: 0x000260C8
			public void SetVibration(int motorIndex, float motorLevel, bool stopOtherMotors)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Vibrate ",
					this.deviceName,
					": motorIndex: ",
					motorIndex.ToString(),
					", motorLevel: ",
					motorLevel.ToString(),
					", stopOtherMotors: ",
					stopOtherMotors.ToString()
				}));
			}

			// Token: 0x06000D7F RID: 3455 RVA: 0x00027F2C File Offset: 0x0002612C
			public void SetVibration(int motorIndex, float motorLevel, float duration, bool stopOtherMotors)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Vibrate ",
					this.deviceName,
					": motorIndex: ",
					motorIndex.ToString(),
					", motorLevel: ",
					motorLevel.ToString(),
					", duration: ",
					duration.ToString(),
					", stopOtherMotors: ",
					stopOtherMotors.ToString()
				}));
			}

			// Token: 0x06000D80 RID: 3456 RVA: 0x00027FA4 File Offset: 0x000261A4
			public float GetVibration(int motorIndex)
			{
				return 0f;
			}

			// Token: 0x06000D81 RID: 3457 RVA: 0x00027FAB File Offset: 0x000261AB
			public void StopVibration()
			{
				Debug.Log("Stop vibration " + this.deviceName);
			}

			// Token: 0x04000726 RID: 1830
			private const int maxJoysticks = 8;

			// Token: 0x04000727 RID: 1831
			private const int maxAxes = 10;

			// Token: 0x04000728 RID: 1832
			private const int maxButtons = 20;

			// Token: 0x04000729 RID: 1833
			public readonly long systemId;

			// Token: 0x0400072A RID: 1834
			public readonly string deviceName;

			// Token: 0x0400072B RID: 1835
			public Guid deviceInstanceGuid;

			// Token: 0x0400072C RID: 1836
			public readonly int axisCount;

			// Token: 0x0400072D RID: 1837
			public readonly int buttonCount;

			// Token: 0x0400072E RID: 1838
			public MyPlatformControllerIdentifier identifier;

			// Token: 0x0400072F RID: 1839
			public readonly bool[] buttonValues;

			// Token: 0x04000730 RID: 1840
			public readonly float[] axisValues;

			// Token: 0x04000731 RID: 1841
			public int unityIndex;
		}
	}
}
