using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A5F RID: 2655
public class UIFightingOverlay : LazyWidget<UIFightingOverlayData>
{
	// Token: 0x060047B8 RID: 18360 RVA: 0x00154474 File Offset: 0x00152674
	protected override void SetData(UIFightingOverlayData data)
	{
		if (this.data != null)
		{
			this.data.OnDataUpdated -= this.HandleDataUpdated;
			this.data.OnCapturePointTracked -= this.HandleCapturePointTracked;
			this.data.OnCapturePointUntracked -= this.HandleCapturePointUntracked;
		}
		base.SetData(data);
		data.OnDataUpdated += this.HandleDataUpdated;
		data.OnCapturePointTracked += this.HandleCapturePointTracked;
		data.OnCapturePointUntracked += this.HandleCapturePointUntracked;
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x0010A599 File Offset: 0x00108799
	private void HandleDataUpdated()
	{
		this.Redraw();
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x0015450C File Offset: 0x0015270C
	private void HandleCapturePointTracked(UICaptureBannerWidgetData capturePoint)
	{
		UICaptureBannerWidget orCreateObject = this.captureBannerWidgetPool.GetOrCreateObject<UICaptureBannerWidget>();
		orCreateObject.Draw(capturePoint);
		orCreateObject.transform.SetParent(base.transform);
		orCreateObject.transform.SetAsLastSibling();
		orCreateObject.gameObject.SetActive(true);
		this.drawingCapturePoints.Add(capturePoint.InstanceID, orCreateObject);
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x00154566 File Offset: 0x00152766
	private void HandleCapturePointUntracked(UICaptureBannerWidgetData capturePoint)
	{
		this.captureBannerWidgetPool.ReleaseObject<UICaptureBannerWidget>(this.drawingCapturePoints[capturePoint.InstanceID]);
		this.drawingCapturePoints.Remove(capturePoint.InstanceID);
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x00154598 File Offset: 0x00152798
	public override void Hide()
	{
		base.Hide();
		this.data.OnDataUpdated -= this.HandleDataUpdated;
		this.data.OnCapturePointTracked -= this.HandleCapturePointTracked;
		this.data.OnCapturePointUntracked -= this.HandleCapturePointUntracked;
	}

	// Token: 0x060047BD RID: 18365 RVA: 0x001545F0 File Offset: 0x001527F0
	public override void Redraw()
	{
		Bounds screenBounds = LazyUI.GetScreenBounds();
		Vector2 vector = new Vector2(screenBounds.center.x, screenBounds.center.y);
		Rect rect = new Rect(screenBounds.min.x + this.clampWidgetOffsetFromScreenBorder, screenBounds.min.y + this.clampWidgetOffsetFromScreenBorder, screenBounds.size.x - this.clampWidgetOffsetFromScreenBorder * 2f, screenBounds.size.y - this.clampWidgetOffsetFromScreenBorder * 2f);
		foreach (KeyValuePair<int, UICaptureBannerWidget> keyValuePair in this.drawingCapturePoints)
		{
			UICaptureBannerWidgetData uicaptureBannerWidgetData = this.data.DrawingCapturePoints[keyValuePair.Key];
			Vector2 screenPosition = uicaptureBannerWidgetData.ScreenPosition;
			uicaptureBannerWidgetData.DirectionToCapturePoint = (screenPosition - vector).normalized;
			bool flag = !rect.Contains(screenPosition);
			if (flag)
			{
				uicaptureBannerWidgetData.ScreenPosition = this.GetRectEdgeIntersection(vector, screenPosition, rect);
			}
			uicaptureBannerWidgetData.IsOutOfScreen = flag;
			keyValuePair.Value.Draw(uicaptureBannerWidgetData);
		}
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x00154738 File Offset: 0x00152938
	private Vector2 GetRectEdgeIntersection(Vector2 from, Vector2 to, Rect rect)
	{
		Vector2 vector = to - from;
		float num = 0f;
		float num2 = 1f;
		if (Mathf.Abs(vector.x) > 0.0001f)
		{
			float num3 = (rect.xMin - from.x) / vector.x;
			float num4 = (rect.xMax - from.x) / vector.x;
			if (vector.x < 0f)
			{
				float num5 = num4;
				float num6 = num3;
				num3 = num5;
				num4 = num6;
			}
			num = Mathf.Max(num, num3);
			num2 = Mathf.Min(num2, num4);
		}
		if (Mathf.Abs(vector.y) > 0.0001f)
		{
			float num7 = (rect.yMin - from.y) / vector.y;
			float num8 = (rect.yMax - from.y) / vector.y;
			if (vector.y < 0f)
			{
				float num9 = num8;
				float num6 = num7;
				num7 = num9;
				num8 = num6;
			}
			num = Mathf.Max(num, num7);
			num2 = Mathf.Min(num2, num8);
		}
		float num10 = Mathf.Clamp(num2, 0f, 1f);
		return from + vector * num10;
	}

	// Token: 0x060047BF RID: 18367 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x060047C0 RID: 18368 RVA: 0x0015484D File Offset: 0x00152A4D
	private void Awake()
	{
		this.captureBannerWidgetPool = new Pool(this.captureBannerWidgetPrefab, base.transform, 10, Pool.PoolType.ImmediateActivation, false, null);
		this.captureBannerWidgetPrefab.gameObject.SetActive(false);
	}

	// Token: 0x040037F9 RID: 14329
	public UICaptureBannerWidget captureBannerWidgetPrefab;

	// Token: 0x040037FA RID: 14330
	public float clampWidgetOffsetFromScreenBorder = 20f;

	// Token: 0x040037FB RID: 14331
	private Dictionary<int, UICaptureBannerWidget> drawingCapturePoints = new Dictionary<int, UICaptureBannerWidget>();

	// Token: 0x040037FC RID: 14332
	private Pool captureBannerWidgetPool;
}
