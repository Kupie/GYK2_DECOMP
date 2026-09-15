using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000814 RID: 2068
[RequireComponent(typeof(VerticalLayoutGroup))]
public class FlexibleHorizontalSizeGridLayoutGroup : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x06003511 RID: 13585 RVA: 0x000FFA34 File Offset: 0x000FDC34
	public void Init()
	{
		this.layoutLinePrefab.gameObject.SetActive(false);
		this.linePool = LazyPooler.CreatePoolById(string.Format("{0}_{1}", "FlexibleHorizontalSizeGridLayoutGroup", this.GetHashCode()), this.layoutLinePrefab, 0, Pool.PoolType.ImmediateActivation, false, false, null);
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x000FFA84 File Offset: 0x000FDC84
	public void Draw(List<RectTransform> items)
	{
		float num = 0f;
		foreach (FlexibleHorizontalSizeGridLayoutLine flexibleHorizontalSizeGridLayoutLine in this.drawnLines)
		{
			this.linePool.ReleaseObject<FlexibleHorizontalSizeGridLayoutLine>(flexibleHorizontalSizeGridLayoutLine);
		}
		this.drawnLines.Clear();
		FlexibleHorizontalSizeGridLayoutLine flexibleHorizontalSizeGridLayoutLine2 = this.linePool.GetOrCreateObject<FlexibleHorizontalSizeGridLayoutLine>();
		this.drawnLines.Add(flexibleHorizontalSizeGridLayoutLine2);
		flexibleHorizontalSizeGridLayoutLine2.transform.SetAsLastSibling();
		for (int i = 0; i < items.Count; i++)
		{
			RectTransform rectTransform = items[i];
			num += rectTransform.sizeDelta.x;
			if (rectTransform.sizeDelta.x > this.width)
			{
				Debug.LogError("Too big size of element when draw FlexibleHorizontalSizeGridLayoutGroup");
			}
			if (num > this.width)
			{
				flexibleHorizontalSizeGridLayoutLine2 = this.linePool.GetOrCreateObject<FlexibleHorizontalSizeGridLayoutLine>();
				this.drawnLines.Add(flexibleHorizontalSizeGridLayoutLine2);
				flexibleHorizontalSizeGridLayoutLine2.transform.SetAsLastSibling();
				num = rectTransform.sizeDelta.x;
			}
			rectTransform.transform.SetParent(flexibleHorizontalSizeGridLayoutLine2.RectTransform);
		}
	}

	// Token: 0x04002A8B RID: 10891
	[SerializeField]
	private float width;

	// Token: 0x04002A8C RID: 10892
	[SerializeField]
	private FlexibleHorizontalSizeGridLayoutLine layoutLinePrefab;

	// Token: 0x04002A8D RID: 10893
	private Pool linePool;

	// Token: 0x04002A8E RID: 10894
	private List<FlexibleHorizontalSizeGridLayoutLine> drawnLines = new List<FlexibleHorizontalSizeGridLayoutLine>();
}
