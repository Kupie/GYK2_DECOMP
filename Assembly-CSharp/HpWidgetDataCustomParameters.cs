using System;
using UnityEngine;

// Token: 0x020007E3 RID: 2019
public class HpWidgetDataCustomParameters : MonoBehaviour
{
	// Token: 0x06003401 RID: 13313 RVA: 0x000FAE03 File Offset: 0x000F9003
	public bool GetCustomWidthIfHasSet(out float width)
	{
		width = (this.hasCustomWidth ? this.customWidth : 0f);
		return this.hasCustomWidth;
	}

	// Token: 0x06003402 RID: 13314 RVA: 0x000FAE22 File Offset: 0x000F9022
	public bool GetCustomHeightIfHasSet(out float height)
	{
		height = (this.hasCustomHeight ? this.customHeight : 0f);
		return this.hasCustomHeight;
	}

	// Token: 0x04002975 RID: 10613
	public bool hasCustomWidth;

	// Token: 0x04002976 RID: 10614
	public float customWidth;

	// Token: 0x04002977 RID: 10615
	public bool hasCustomHeight;

	// Token: 0x04002978 RID: 10616
	public float customHeight;
}
