using System;
using UnityEngine;
using UnityEngine.Events;

namespace LazyBearTechnology
{
	// Token: 0x0200015D RID: 349
	[RequireComponent(typeof(RectTransform), typeof(Collider2D))]
	[ExecuteInEditMode]
	public class UIButtonCollider : MonoBehaviour
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x00026AC8 File Offset: 0x00024CC8
		private void Awake()
		{
			if (this.collisionCollider2D == null)
			{
				this.collisionCollider2D = base.GetComponent<Collider2D>();
			}
			if (this.boxCollider2D == null)
			{
				this.boxCollider2D = this.collisionCollider2D as BoxCollider2D;
			}
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00026B28 File Offset: 0x00024D28
		private void Update()
		{
			if (!this.interactable)
			{
				return;
			}
			if (!this.entered)
			{
				if (this.IsMouseOvered())
				{
					this.entered = true;
					this.OnEnter.Invoke();
					return;
				}
			}
			else
			{
				if (Input.GetMouseButtonDown(0))
				{
					this.OnClick.Invoke();
				}
				if (!this.IsMouseOvered())
				{
					this.entered = false;
					this.OnExit.Invoke();
				}
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00026B8E File Offset: 0x00024D8E
		public void SetInteractable(bool state)
		{
			this.interactable = state;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00026B97 File Offset: 0x00024D97
		private bool IsMouseOvered()
		{
			return this.collisionCollider2D.OverlapPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00026BC0 File Offset: 0x00024DC0
		private void OnRectTransformDimensionsChange()
		{
			if (!this.syncSizeWithUIRect || this.boxCollider2D == null)
			{
				return;
			}
			this.boxCollider2D.size = this.rectTransform.rect.size;
		}

		// Token: 0x040004A3 RID: 1187
		[SerializeField]
		private bool interactable;

		// Token: 0x040004A4 RID: 1188
		[SerializeField]
		private UnityEvent OnEnter;

		// Token: 0x040004A5 RID: 1189
		[SerializeField]
		private UnityEvent OnClick;

		// Token: 0x040004A6 RID: 1190
		[SerializeField]
		private UnityEvent OnExit;

		// Token: 0x040004A7 RID: 1191
		[SerializeField]
		private Collider2D collisionCollider2D;

		// Token: 0x040004A8 RID: 1192
		[SerializeField]
		private BoxCollider2D boxCollider2D;

		// Token: 0x040004A9 RID: 1193
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x040004AA RID: 1194
		private bool entered;

		// Token: 0x040004AB RID: 1195
		public bool syncSizeWithUIRect;
	}
}
