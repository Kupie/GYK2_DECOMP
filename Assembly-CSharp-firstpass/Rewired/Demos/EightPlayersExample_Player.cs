using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020000FD RID: 253
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class EightPlayersExample_Player : MonoBehaviour
	{
		// Token: 0x06000C89 RID: 3209 RVA: 0x00023DAE File Offset: 0x00021FAE
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00023DBC File Offset: 0x00021FBC
		private void Initialize()
		{
			this.player = ReInput.players.GetPlayer(this.playerId);
			this.initialized = true;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00023DDB File Offset: 0x00021FDB
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
			this.GetInput();
			this.ProcessInput();
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00023E00 File Offset: 0x00022000
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00023E5C File Offset: 0x0002205C
		private void ProcessInput()
		{
			if (this.moveVector.x != 0f || this.moveVector.y != 0f)
			{
				this.cc.Move(this.moveVector * this.moveSpeed * Time.deltaTime);
			}
			if (this.fire)
			{
				global::UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, base.transform.position + base.transform.right, base.transform.rotation).GetComponent<Rigidbody>().AddForce(base.transform.right * this.bulletSpeed, ForceMode.VelocityChange);
			}
		}

		// Token: 0x0400065C RID: 1628
		public int playerId;

		// Token: 0x0400065D RID: 1629
		public float moveSpeed = 3f;

		// Token: 0x0400065E RID: 1630
		public float bulletSpeed = 15f;

		// Token: 0x0400065F RID: 1631
		public GameObject bulletPrefab;

		// Token: 0x04000660 RID: 1632
		private Player player;

		// Token: 0x04000661 RID: 1633
		private CharacterController cc;

		// Token: 0x04000662 RID: 1634
		private Vector3 moveVector;

		// Token: 0x04000663 RID: 1635
		private bool fire;

		// Token: 0x04000664 RID: 1636
		[NonSerialized]
		private bool initialized;
	}
}
