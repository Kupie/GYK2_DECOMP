using System;
using System.Collections.Generic;
using LinqTools;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000161 RID: 353
	[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
	public abstract class BaseVirtualCursor : MonoBehaviour
	{
		// Token: 0x06000792 RID: 1938 RVA: 0x00026CF2 File Offset: 0x00024EF2
		protected virtual void Awake()
		{
			this.Init();
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00026CFC File Offset: 0x00024EFC
		private VirtualCursorVisualPair GetPairByState(VirtualCursorState cursorState)
		{
			return this.cursorVisualPairs.FirstOrDefault((VirtualCursorVisualPair c) => c.cursorState == cursorState);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00026D30 File Offset: 0x00024F30
		protected void AccelerateSpeed(bool isCursorActive)
		{
			if (isCursorActive)
			{
				this.currentSpeed += this.accelerateSpeed * Time.deltaTime;
			}
			else
			{
				this.currentSpeed -= this.accelerateSpeed * Time.deltaTime;
			}
			this.currentSpeed = Mathf.Clamp(this.currentSpeed, this.minSpeed, this.maxSpeed);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00026D94 File Offset: 0x00024F94
		protected virtual void Init()
		{
			base.gameObject.SetActive(false);
			this.collider2D = base.GetComponent<Collider2D>();
			this.rigidbody2D = base.GetComponent<Rigidbody2D>();
			this.collider2D.tag = "MapCursor";
			this.collider2D.isTrigger = false;
			this.rigidbody2D.isKinematic = true;
			this.currentSpeed = this.minSpeed;
			if (this.cursorVisualPairs.Count == 0)
			{
				Debug.LogError("Cursors not found, please setup pairs in inspector");
				return;
			}
			foreach (VirtualCursorVisualPair virtualCursorVisualPair in this.cursorVisualPairs)
			{
				virtualCursorVisualPair.gameObject.SetActive(false);
			}
			this.activeVisualPair = this.GetPairByState(VirtualCursorState.Default);
			this.activeVisualPair.gameObject.SetActive(true);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00026E7C File Offset: 0x0002507C
		protected virtual void OnEnable()
		{
			this.CalculateBounds();
			this.OverlapColliders(null);
			this.currentSpeed = this.minSpeed;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00026E98 File Offset: 0x00025098
		protected virtual void Update()
		{
			Vector2 direction = LazyInput.GetDirection();
			this.AccelerateSpeed(direction != Vector2.zero);
			Vector2 vector = base.transform.position + this.GetSpeed() * direction;
			vector = vector.Clamp(this.minCoords, this.maxCoords);
			base.transform.position = new Vector3(vector.x, vector.y, this.zPosition);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00026F13 File Offset: 0x00025113
		protected virtual float GetSpeed()
		{
			return this.currentSpeed * Time.deltaTime;
		}

		// Token: 0x06000799 RID: 1945
		public abstract void CalculateBounds();

		// Token: 0x0600079A RID: 1946 RVA: 0x00026F24 File Offset: 0x00025124
		public virtual void SetState(VirtualCursorState cursorState)
		{
			VirtualCursorVisualPair pairByState = this.GetPairByState(cursorState);
			if (pairByState == null)
			{
				Debug.LogError(string.Format("Invalid cursor state:[{0}]", cursorState));
				return;
			}
			this.activeVisualPair.gameObject.SetActive(false);
			pairByState.gameObject.SetActive(true);
			this.activeVisualPair = pairByState;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00026F74 File Offset: 0x00025174
		public virtual void OverlapColliders(List<BaseVirtualCursorEventHandler> exclude)
		{
			List<Collider2D> list = new List<Collider2D>();
			ContactFilter2D contactFilter2D = new ContactFilter2D
			{
				useTriggers = true
			};
			this.collider2D.Overlap(contactFilter2D, list);
			foreach (Collider2D collider2D in list)
			{
				BaseVirtualCursorEventHandler component = collider2D.GetComponent<BaseVirtualCursorEventHandler>();
				if (component != null && (exclude == null || !exclude.Contains(component)))
				{
					component.ForceSelect();
				}
			}
		}

		// Token: 0x040004B0 RID: 1200
		public const string TAG = "MapCursor";

		// Token: 0x040004B1 RID: 1201
		[SerializeField]
		protected float currentSpeed;

		// Token: 0x040004B2 RID: 1202
		[SerializeField]
		protected float minSpeed = 3f;

		// Token: 0x040004B3 RID: 1203
		[SerializeField]
		protected float maxSpeed = 6f;

		// Token: 0x040004B4 RID: 1204
		[SerializeField]
		protected float accelerateSpeed = 6f;

		// Token: 0x040004B5 RID: 1205
		[SerializeField]
		protected float zPosition = -500f;

		// Token: 0x040004B6 RID: 1206
		[SerializeField]
		[Space]
		protected List<VirtualCursorVisualPair> cursorVisualPairs;

		// Token: 0x040004B7 RID: 1207
		protected Collider2D collider2D;

		// Token: 0x040004B8 RID: 1208
		protected Rigidbody2D rigidbody2D;

		// Token: 0x040004B9 RID: 1209
		protected VirtualCursorVisualPair activeVisualPair;

		// Token: 0x040004BA RID: 1210
		protected Vector2 minCoords;

		// Token: 0x040004BB RID: 1211
		protected Vector2 maxCoords;
	}
}
