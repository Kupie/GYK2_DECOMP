using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200085E RID: 2142
[DefaultExecutionOrder(10000)]
public class UICraftHintWidgetForceUpdateCanvasesScheduler : LazySingleton<UICraftHintWidgetForceUpdateCanvasesScheduler>
{
	// Token: 0x060036D9 RID: 14041 RVA: 0x00109A84 File Offset: 0x00107C84
	public void RequestUpdate()
	{
		UICraftHintWidgetForceUpdateCanvasesScheduler.isUpdateRequested = true;
	}

	// Token: 0x060036DA RID: 14042 RVA: 0x00109A8C File Offset: 0x00107C8C
	private void LateUpdate()
	{
		if (!UICraftHintWidgetForceUpdateCanvasesScheduler.isUpdateRequested)
		{
			return;
		}
		UICraftHintWidgetForceUpdateCanvasesScheduler.isUpdateRequested = false;
		Canvas.ForceUpdateCanvases();
	}

	// Token: 0x04002BC8 RID: 11208
	private static bool isUpdateRequested;
}
