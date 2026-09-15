using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A5E RID: 2654
public class UICaptureBannerWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000AD4 RID: 2772
	// (get) Token: 0x060047AD RID: 18349 RVA: 0x001543F8 File Offset: 0x001525F8
	// (set) Token: 0x060047AE RID: 18350 RVA: 0x00154400 File Offset: 0x00152600
	public int InstanceID { get; private set; }

	// Token: 0x17000AD5 RID: 2773
	// (get) Token: 0x060047AF RID: 18351 RVA: 0x00154409 File Offset: 0x00152609
	// (set) Token: 0x060047B0 RID: 18352 RVA: 0x00154411 File Offset: 0x00152611
	public float CaptureProgress { get; set; }

	// Token: 0x17000AD6 RID: 2774
	// (get) Token: 0x060047B1 RID: 18353 RVA: 0x0015441A File Offset: 0x0015261A
	// (set) Token: 0x060047B2 RID: 18354 RVA: 0x00154422 File Offset: 0x00152622
	public Vector2 DirectionToCapturePoint { get; set; }

	// Token: 0x17000AD7 RID: 2775
	// (get) Token: 0x060047B3 RID: 18355 RVA: 0x0015442B File Offset: 0x0015262B
	// (set) Token: 0x060047B4 RID: 18356 RVA: 0x00154433 File Offset: 0x00152633
	public Vector2 ScreenPosition { get; set; }

	// Token: 0x17000AD8 RID: 2776
	// (get) Token: 0x060047B5 RID: 18357 RVA: 0x0015443C File Offset: 0x0015263C
	// (set) Token: 0x060047B6 RID: 18358 RVA: 0x00154444 File Offset: 0x00152644
	public bool IsOutOfScreen { get; set; }

	// Token: 0x060047B7 RID: 18359 RVA: 0x0015444D File Offset: 0x0015264D
	public UICaptureBannerWidgetData(int instanceID, float captureProgress, Vector2 directionToCapturePoint, Vector2 screenPosition)
	{
		this.InstanceID = instanceID;
		this.CaptureProgress = captureProgress;
		this.DirectionToCapturePoint = directionToCapturePoint;
		this.ScreenPosition = screenPosition;
	}
}
