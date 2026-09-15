using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200089B RID: 2203
public class UIMagnifyingGlassWidget : LazyWidget<UIMagnifyingGlassWidgetData>
{
	// Token: 0x060038BF RID: 14527 RVA: 0x00110B00 File Offset: 0x0010ED00
	public override void Draw(UIMagnifyingGlassWidgetData data)
	{
		base.gameObject.SetActive(true);
		this.CacheOriginalIcon();
		bool isOutOfScreen = data.IsOutOfScreen;
		if (this.pointerImage != null)
		{
			this.pointerImage.gameObject.SetActive(isOutOfScreen);
		}
		if (this.iconImage != null)
		{
			this.iconImage.gameObject.SetActive(true);
			if (isOutOfScreen)
			{
				if (this.originalIconSprite != null)
				{
					this.iconImage.sprite = this.originalIconSprite;
					this.iconImage.rectTransform.sizeDelta = this.originalIconSize;
				}
			}
			else
			{
				Sprite sprite = ((LazySingletonSO<EasySpritesCollection>.Instance != null) ? LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon_view_bubble", null) : null);
				if (sprite != null)
				{
					this.iconImage.sprite = sprite;
					this.iconImage.SetNativeSize();
				}
			}
		}
		if (isOutOfScreen)
		{
			this.UpdatePointer(data.DirectionToTarget);
		}
		base.transform.position = data.ScreenPosition;
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x00110C08 File Offset: 0x0010EE08
	private void CacheOriginalIcon()
	{
		if (this.originalIconCached || this.iconImage == null)
		{
			return;
		}
		this.originalIconSprite = this.iconImage.sprite;
		this.originalIconSize = this.iconImage.rectTransform.sizeDelta;
		this.originalIconCached = true;
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x00110C5C File Offset: 0x0010EE5C
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
		int num2 = Mathf.RoundToInt((90f - num + 360f) % 360f / 30f) % 12;
		if (num2 < 0 || num2 >= this.pointerSprites.Count)
		{
			return;
		}
		this.pointerImage.sprite = this.pointerSprites[num2];
		this.pointerImage.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x000E80FD File Offset: 0x000E62FD
	protected override void TestDraw()
	{
		throw new NotImplementedException();
	}

	// Token: 0x04002D29 RID: 11561
	private const int SpriteCount = 12;

	// Token: 0x04002D2A RID: 11562
	private const float DegreesPerSprite = 30f;

	// Token: 0x04002D2B RID: 11563
	private const string OnScreenExitIconId = "icon_view_bubble";

	// Token: 0x04002D2C RID: 11564
	public List<Sprite> pointerSprites = new List<Sprite>();

	// Token: 0x04002D2D RID: 11565
	public Image pointerImage;

	// Token: 0x04002D2E RID: 11566
	public Image iconImage;

	// Token: 0x04002D2F RID: 11567
	private Sprite originalIconSprite;

	// Token: 0x04002D30 RID: 11568
	private Vector2 originalIconSize;

	// Token: 0x04002D31 RID: 11569
	private bool originalIconCached;
}
