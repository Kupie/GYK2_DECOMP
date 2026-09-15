using System;
using UnityEngine;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x0200012B RID: 299
	public class VibrationTest : MonoBehaviour
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00027FC2 File Offset: 0x000261C2
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00027FD4 File Offset: 0x000261D4
		private void Update()
		{
			for (int i = 0; i < VibrationTest.action_motors.Length; i++)
			{
				if (this.player.GetButtonDown(VibrationTest.action_motors[i]))
				{
					this.SetVibration(i, Mathf.Clamp01(this.motors[i] + this.vibrationIncrement));
				}
				if (this.player.GetNegativeButtonDown(VibrationTest.action_motors[i]))
				{
					this.SetVibration(i, Mathf.Clamp01(this.motors[i] - this.vibrationIncrement));
				}
			}
			if (this.player.GetButtonDown(VibrationTest.action_stop))
			{
				this.StopVibration();
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00028069 File Offset: 0x00026269
		private void StopVibration()
		{
			this.player.StopVibration();
			Array.Clear(this.motors, 0, this.motors.Length);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0002808A File Offset: 0x0002628A
		private void SetVibration(int motorIndex, float value)
		{
			this.motors[motorIndex] = value;
			this.player.SetVibration(motorIndex, this.motors[motorIndex]);
		}

		// Token: 0x04000733 RID: 1843
		public int playerId;

		// Token: 0x04000734 RID: 1844
		public float vibrationIncrement = 0.1f;

		// Token: 0x04000735 RID: 1845
		private float[] motors = new float[2];

		// Token: 0x04000736 RID: 1846
		private static readonly string[] action_motors = new string[] { "VibrationMotor0", "VibrationMotor1" };

		// Token: 0x04000737 RID: 1847
		private static readonly string action_stop = "StopVibration";
	}
}
