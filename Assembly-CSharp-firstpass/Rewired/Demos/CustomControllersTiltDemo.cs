using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020000F5 RID: 245
	[AddComponentMenu("")]
	public class CustomControllersTiltDemo : MonoBehaviour
	{
		// Token: 0x06000C53 RID: 3155 RVA: 0x00022E84 File Offset: 0x00021084
		private void Awake()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			this.player = ReInput.players.GetPlayer(0);
			ReInput.InputSourceUpdateEvent += this.OnInputUpdate;
			this.controller = (CustomController)this.player.controllers.GetControllerWithTag(ControllerType.Custom, "TiltController");
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00022EDC File Offset: 0x000210DC
		private void Update()
		{
			if (this.target == null)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			vector.y = this.player.GetAxis("Tilt Vertical");
			vector.x = this.player.GetAxis("Tilt Horizontal");
			if (vector.sqrMagnitude > 1f)
			{
				vector.Normalize();
			}
			vector *= Time.deltaTime;
			this.target.Translate(vector * this.speed);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00022F64 File Offset: 0x00021164
		private void OnInputUpdate()
		{
			Vector3 acceleration = Input.acceleration;
			this.controller.SetAxisValue(0, acceleration.x);
			this.controller.SetAxisValue(1, acceleration.y);
			this.controller.SetAxisValue(2, acceleration.z);
		}

		// Token: 0x04000630 RID: 1584
		public Transform target;

		// Token: 0x04000631 RID: 1585
		public float speed = 10f;

		// Token: 0x04000632 RID: 1586
		private CustomController controller;

		// Token: 0x04000633 RID: 1587
		private Player player;
	}
}
