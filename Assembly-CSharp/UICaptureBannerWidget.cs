using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A5D RID: 2653
public class UICaptureBannerWidget : LazyWidget<UICaptureBannerWidgetData>
{
	// Token: 0x060047A7 RID: 18343 RVA: 0x001541F4 File Offset: 0x001523F4
	public override void Draw(UICaptureBannerWidgetData data)
	{
		base.gameObject.SetActive(data.CaptureProgress.More(0f, 0.0001f) && data.CaptureProgress.Less(1f, 0.0001f) && data.IsOutOfScreen);
		float num = 1f - data.CaptureProgress;
		this.progressBarImage.fillAmount = num;
		this.glowImage.fillAmount = num;
		this.UpdatePointer(data.DirectionToCapturePoint);
		base.transform.position = data.ScreenPosition;
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x0015428C File Offset: 0x0015248C
	private void UpdatePointer(Vector2 direction)
	{
		if (direction == Vector2.zero || this.pointerSprites.Count == 0)
		{
			return;
		}
		float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
		if (num < 0f)
		{
			num += 360f;
		}
		float num2 = (450f - num) % 360f;
		int num3 = Mathf.FloorToInt(num2 / 90f) % 4;
		float num4 = num2 - (float)num3 * 90f;
		int num5 = this.FindClosestSpriteIndex(num4);
		float num6 = Mathf.Abs(num4 - UICaptureBannerWidget.SpriteAngles[num5]);
		if (90f - num4 < num6)
		{
			num3 = (num3 + 1) % 4;
			num5 = 0;
		}
		float num7 = (float)(-(float)num3) * 90f;
		this.pointerImage.transform.localRotation = Quaternion.Euler(0f, 0f, num7);
		this.pointerImage.sprite = this.pointerSprites[num5];
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x00154370 File Offset: 0x00152570
	private int FindClosestSpriteIndex(float localAngle)
	{
		int num = 0;
		float num2 = Mathf.Abs(localAngle - UICaptureBannerWidget.SpriteAngles[0]);
		int num3 = Mathf.Min(UICaptureBannerWidget.SpriteAngles.Length, this.pointerSprites.Count);
		for (int i = 1; i < num3; i++)
		{
			float num4 = Mathf.Abs(localAngle - UICaptureBannerWidget.SpriteAngles[i]);
			if (num4 < num2)
			{
				num2 = num4;
				num = i;
			}
		}
		return num;
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x000E80FD File Offset: 0x000E62FD
	protected override void TestDraw()
	{
		throw new NotImplementedException();
	}

	// Token: 0x040037EF RID: 14319
	public List<Sprite> pointerSprites = new List<Sprite>();

	// Token: 0x040037F0 RID: 14320
	public Image progressBarImage;

	// Token: 0x040037F1 RID: 14321
	public Image glowImage;

	// Token: 0x040037F2 RID: 14322
	public Image pointerImage;

	// Token: 0x040037F3 RID: 14323
	private static readonly float[] SpriteAngles = new float[] { 0f, 19f, 33f, 45f, 57f, 71f };
}
