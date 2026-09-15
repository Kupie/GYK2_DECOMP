using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A60 RID: 2656
public class UIFightingOverlayData : LazyWidgetDataBase
{
	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x060047C2 RID: 18370 RVA: 0x0015489C File Offset: 0x00152A9C
	// (remove) Token: 0x060047C3 RID: 18371 RVA: 0x001548D4 File Offset: 0x00152AD4
	public event Action OnDataUpdated;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x060047C4 RID: 18372 RVA: 0x0015490C File Offset: 0x00152B0C
	// (remove) Token: 0x060047C5 RID: 18373 RVA: 0x00154944 File Offset: 0x00152B44
	public event Action<UICaptureBannerWidgetData> OnCapturePointTracked;

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x060047C6 RID: 18374 RVA: 0x0015497C File Offset: 0x00152B7C
	// (remove) Token: 0x060047C7 RID: 18375 RVA: 0x001549B4 File Offset: 0x00152BB4
	public event Action<UICaptureBannerWidgetData> OnCapturePointUntracked;

	// Token: 0x17000AD9 RID: 2777
	// (get) Token: 0x060047C8 RID: 18376 RVA: 0x001549E9 File Offset: 0x00152BE9
	// (set) Token: 0x060047C9 RID: 18377 RVA: 0x001549F1 File Offset: 0x00152BF1
	public FightingGameController FightingGameController { get; private set; }

	// Token: 0x17000ADA RID: 2778
	// (get) Token: 0x060047CA RID: 18378 RVA: 0x001549FA File Offset: 0x00152BFA
	// (set) Token: 0x060047CB RID: 18379 RVA: 0x00154A02 File Offset: 0x00152C02
	public Dictionary<int, FightingCapturePoint> CapturePoint { get; private set; } = new Dictionary<int, FightingCapturePoint>();

	// Token: 0x17000ADB RID: 2779
	// (get) Token: 0x060047CC RID: 18380 RVA: 0x00154A0B File Offset: 0x00152C0B
	// (set) Token: 0x060047CD RID: 18381 RVA: 0x00154A13 File Offset: 0x00152C13
	public Dictionary<int, UICaptureBannerWidgetData> DrawingCapturePoints { get; private set; } = new Dictionary<int, UICaptureBannerWidgetData>();

	// Token: 0x060047CE RID: 18382 RVA: 0x00154A1C File Offset: 0x00152C1C
	public UIFightingOverlayData(FightingGameController fightingGameController)
	{
		this.FightingGameController = fightingGameController;
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x00154A44 File Offset: 0x00152C44
	public void UpdateData(Vector3 playerPosition)
	{
		if (this.FightingGameController == null)
		{
			return;
		}
		foreach (KeyValuePair<int, FightingCapturePoint> keyValuePair in this.CapturePoint)
		{
			Vector2 vector = (keyValuePair.Value.transform.position - playerPosition).XZ().normalized;
			UICaptureBannerWidgetData uicaptureBannerWidgetData;
			if (this.DrawingCapturePoints.TryGetValue(keyValuePair.Key, out uicaptureBannerWidgetData))
			{
				uicaptureBannerWidgetData.CaptureProgress = keyValuePair.Value.CurrentProgress;
				uicaptureBannerWidgetData.DirectionToCapturePoint = vector;
				uicaptureBannerWidgetData.ScreenPosition = CameraSystem.WorldToScreenPoint(keyValuePair.Value.transform.position);
			}
		}
		Action onDataUpdated = this.OnDataUpdated;
		if (onDataUpdated == null)
		{
			return;
		}
		onDataUpdated();
	}

	// Token: 0x060047D0 RID: 18384 RVA: 0x00154B30 File Offset: 0x00152D30
	public void TrackCapturePoint(FightingCapturePoint capturePoint)
	{
		if (capturePoint == null)
		{
			return;
		}
		int instanceID = capturePoint.GetInstanceID();
		this.CapturePoint.Add(instanceID, capturePoint);
		this.DrawingCapturePoints.Add(instanceID, new UICaptureBannerWidgetData(instanceID, 0f, Vector2.zero, Vector2.zero));
		Action<UICaptureBannerWidgetData> onCapturePointTracked = this.OnCapturePointTracked;
		if (onCapturePointTracked == null)
		{
			return;
		}
		onCapturePointTracked(this.DrawingCapturePoints[instanceID]);
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x00154B98 File Offset: 0x00152D98
	public void UntrackCapturePoint(FightingCapturePoint capturePoint)
	{
		if (capturePoint == null)
		{
			return;
		}
		int instanceID = capturePoint.GetInstanceID();
		UICaptureBannerWidgetData uicaptureBannerWidgetData;
		if (this.DrawingCapturePoints.TryGetValue(instanceID, out uicaptureBannerWidgetData))
		{
			Action<UICaptureBannerWidgetData> onCapturePointUntracked = this.OnCapturePointUntracked;
			if (onCapturePointUntracked != null)
			{
				onCapturePointUntracked(uicaptureBannerWidgetData);
			}
			this.DrawingCapturePoints.Remove(instanceID);
		}
		this.CapturePoint.Remove(instanceID);
	}
}
