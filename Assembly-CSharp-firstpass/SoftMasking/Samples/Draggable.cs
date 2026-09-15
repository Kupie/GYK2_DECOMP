using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples
{
	// Token: 0x02000005 RID: 5
	[RequireComponent(typeof(RectTransform))]
	public class Draggable : UIBehaviour, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000021DF File Offset: 0x000003DF
		protected override void Awake()
		{
			base.Awake();
			this._rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021F3 File Offset: 0x000003F3
		public void OnDrag(PointerEventData eventData)
		{
			this._rectTransform.anchoredPosition += eventData.delta;
		}

		// Token: 0x0400000A RID: 10
		private RectTransform _rectTransform;
	}
}
