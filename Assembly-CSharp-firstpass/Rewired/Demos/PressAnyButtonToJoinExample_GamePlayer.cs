using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000103 RID: 259
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class PressAnyButtonToJoinExample_GamePlayer : MonoBehaviour
	{
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00024D35 File Offset: 0x00022F35
		private Player player
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00024D50 File Offset: 0x00022F50
		private void OnEnable()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00024D5E File Offset: 0x00022F5E
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

		// Token: 0x06000CBC RID: 3260 RVA: 0x00024D80 File Offset: 0x00022F80
		private void GetInput()
		{
			this.moveVector.x = this.player.GetAxis("Move Horizontal");
			this.moveVector.y = this.player.GetAxis("Move Vertical");
			this.fire = this.player.GetButtonDown("Fire");
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00024DDC File Offset: 0x00022FDC
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

		// Token: 0x04000680 RID: 1664
		public int playerId;

		// Token: 0x04000681 RID: 1665
		public float moveSpeed = 3f;

		// Token: 0x04000682 RID: 1666
		public float bulletSpeed = 15f;

		// Token: 0x04000683 RID: 1667
		public GameObject bulletPrefab;

		// Token: 0x04000684 RID: 1668
		private CharacterController cc;

		// Token: 0x04000685 RID: 1669
		private Vector3 moveVector;

		// Token: 0x04000686 RID: 1670
		private bool fire;
	}
}
