using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000106 RID: 262
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class PressStartToJoinExample_GamePlayer : MonoBehaviour
	{
		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00025067 File Offset: 0x00023267
		private Player player
		{
			get
			{
				return PressStartToJoinExample_Assigner.GetRewiredPlayer(this.gamePlayerId);
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00025074 File Offset: 0x00023274
		private void OnEnable()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00025082 File Offset: 0x00023282
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this.player == null)
			{
				return;
			}
			this.GetInput();
			this.ProcessInput();
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x000250A4 File Offset: 0x000232A4
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00025100 File Offset: 0x00023300
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

		// Token: 0x0400068D RID: 1677
		public int gamePlayerId;

		// Token: 0x0400068E RID: 1678
		public float moveSpeed = 3f;

		// Token: 0x0400068F RID: 1679
		public float bulletSpeed = 15f;

		// Token: 0x04000690 RID: 1680
		public GameObject bulletPrefab;

		// Token: 0x04000691 RID: 1681
		private CharacterController cc;

		// Token: 0x04000692 RID: 1682
		private Vector3 moveVector;

		// Token: 0x04000693 RID: 1683
		private bool fire;
	}
}
