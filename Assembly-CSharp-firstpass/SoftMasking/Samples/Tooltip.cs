using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples
{
	// Token: 0x02000012 RID: 18
	public class Tooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00002D74 File Offset: 0x00000F74
		public void LateUpdate()
		{
			Vector2 vector;
			if (this.tooltip.gameObject.activeInHierarchy && RectTransformUtility.ScreenPointToLocalPointInRectangle(this.tooltip.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out vector))
			{
				this.tooltip.anchoredPosition = vector + new Vector2(10f, -20f);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002DD7 File Offset: 0x00000FD7
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			this.tooltip.gameObject.SetActive(true);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002DEA File Offset: 0x00000FEA
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			this.tooltip.gameObject.SetActive(false);
		}

		// Token: 0x04000044 RID: 68
		public RectTransform tooltip;
	}
}
