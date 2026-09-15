using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020000F7 RID: 247
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class CustomControllerDemo_Player : MonoBehaviour
	{
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00023292 File Offset: 0x00021492
		private Player player
		{
			get
			{
				if (this._player == null)
				{
					this._player = ReInput.players.GetPlayer(this.playerId);
				}
				return this._player;
			}
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000232B8 File Offset: 0x000214B8
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000232C8 File Offset: 0x000214C8
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			Vector2 vector = new Vector2(this.player.GetAxis("Move Horizontal"), this.player.GetAxis("Move Vertical"));
			this.cc.Move(vector * this.speed * Time.deltaTime);
			if (this.player.GetButtonDown("Fire"))
			{
				Vector3 vector2 = Vector3.Scale(new Vector3(1f, 0f, 0f), base.transform.right);
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, base.transform.position + vector2, Quaternion.identity);
				Vector3 vector3 = new Vector3(this.bulletSpeed * base.transform.right.x, 0f, 0f);
				gameObject.GetComponent<Rigidbody>().linearVelocity = vector3;
			}
			if (this.player.GetButtonDown("Change Color"))
			{
				Renderer component = base.GetComponent<Renderer>();
				Material material = component.material;
				material.color = new Color(global::UnityEngine.Random.Range(0f, 1f), global::UnityEngine.Random.Range(0f, 1f), global::UnityEngine.Random.Range(0f, 1f), 1f);
				component.material = material;
			}
		}

		// Token: 0x0400063F RID: 1599
		public int playerId;

		// Token: 0x04000640 RID: 1600
		public float speed = 1f;

		// Token: 0x04000641 RID: 1601
		public float bulletSpeed = 20f;

		// Token: 0x04000642 RID: 1602
		public GameObject bulletPrefab;

		// Token: 0x04000643 RID: 1603
		private Player _player;

		// Token: 0x04000644 RID: 1604
		private CharacterController cc;
	}
}
