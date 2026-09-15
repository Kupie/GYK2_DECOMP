using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020007F1 RID: 2033
public class UIInteractionHintWidgetData : LazyWidgetDataBase
{
	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x0600343A RID: 13370 RVA: 0x000FBB88 File Offset: 0x000F9D88
	// (set) Token: 0x0600343B RID: 13371 RVA: 0x000FBB90 File Offset: 0x000F9D90
	public UIInteractionHintRowWidgetData RowData1 { get; private set; }

	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x0600343C RID: 13372 RVA: 0x000FBB99 File Offset: 0x000F9D99
	// (set) Token: 0x0600343D RID: 13373 RVA: 0x000FBBA1 File Offset: 0x000F9DA1
	public UIInteractionHintRowWidgetData RowData2 { get; private set; }

	// Token: 0x0600343E RID: 13374 RVA: 0x000FBBAA File Offset: 0x000F9DAA
	public UIInteractionHintWidgetData(UIInteractionHintRowWidgetData rowData)
	{
		this.RowData1 = rowData;
		this.RowData2 = null;
	}

	// Token: 0x0600343F RID: 13375 RVA: 0x000FBBC0 File Offset: 0x000F9DC0
	public UIInteractionHintWidgetData(List<UIInteractionHintRowWidgetData> rowWidgetsData)
	{
		this.RowData1 = ((rowWidgetsData.Count > 0) ? rowWidgetsData[0] : null);
		this.RowData2 = ((rowWidgetsData.Count > 1) ? rowWidgetsData[1] : null);
		if (rowWidgetsData.Count > 2)
		{
			Debug.LogWarning("UIInteractionHintWidgetData: Too many row widgets data. Allowed only 2.");
		}
	}
}
