using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000815 RID: 2069
[RequireComponent(typeof(HorizontalLayoutGroup))]
public class FlexibleHorizontalSizeGridLayoutLine : MonoBehaviour
{
	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06003514 RID: 13588 RVA: 0x000FFBC3 File Offset: 0x000FDDC3
	public RectTransform RectTransform
	{
		get
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.transform as RectTransform;
			}
			return this.rectTransform;
		}
	}

	// Token: 0x04002A8F RID: 10895
	private RectTransform rectTransform;
}
