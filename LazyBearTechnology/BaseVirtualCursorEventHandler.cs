using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000162 RID: 354
	[RequireComponent(typeof(Collider2D))]
	public abstract class BaseVirtualCursorEventHandler : MonoBehaviour
	{
		// Token: 0x0600079D RID: 1949 RVA: 0x0002703C File Offset: 0x0002523C
		private void Awake()
		{
			this.collider2D = base.GetComponent<Collider2D>();
			this.collider2D.isTrigger = true;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00027058 File Offset: 0x00025258
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("MapCursor"))
			{
				BaseVirtualCursor component = collision.GetComponent<BaseVirtualCursor>();
				if (component != null)
				{
					this.OnSelect(component);
				}
			}
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002708C File Offset: 0x0002528C
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.CompareTag("MapCursor"))
			{
				BaseVirtualCursor component = collision.GetComponent<BaseVirtualCursor>();
				if (component != null)
				{
					this.OnDeselect(component);
				}
			}
		}

		// Token: 0x060007A0 RID: 1952
		protected abstract void OnSelect(BaseVirtualCursor cursor);

		// Token: 0x060007A1 RID: 1953
		protected abstract void OnDeselect(BaseVirtualCursor cursor);

		// Token: 0x060007A2 RID: 1954 RVA: 0x000270BD File Offset: 0x000252BD
		public virtual void ForceSelect()
		{
			this.OnSelect(null);
		}

		// Token: 0x040004BC RID: 1212
		protected Collider2D collider2D;
	}
}
