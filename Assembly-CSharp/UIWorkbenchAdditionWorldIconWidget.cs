using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007F6 RID: 2038
public class UIWorkbenchAdditionWorldIconWidget : LazyWidget<UIWorkbenchAdditionWorldIconWidgetData>, IUIObjectBubbleWidgetWithoutRebuildingLayout
{
	// Token: 0x06003454 RID: 13396 RVA: 0x000FBD8C File Offset: 0x000F9F8C
	public override void Redraw()
	{
		if (this.icon != null)
		{
			this.icon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.IconId, "i_b_blueprint_placeholder");
			this.icon.enabled = this.icon.sprite != null;
		}
		this.RedrawBackground();
		this.RedrawStrikethrough();
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x000FBDF4 File Offset: 0x000F9FF4
	private void RedrawBackground()
	{
		if (this.background == null)
		{
			Transform transform = base.transform.Find("Background");
			this.background = ((transform != null) ? transform.gameObject : null);
		}
		if (this.background != null)
		{
			this.background.SetActive(this.data.ShowBackground);
		}
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x000FBE58 File Offset: 0x000FA058
	private void RedrawStrikethrough()
	{
		if (this.strikethrough == null)
		{
			return;
		}
		this.CacheStrikethroughDefaults();
		if (this.strikethroughImage != null)
		{
			bool flag = !string.IsNullOrEmpty(this.data.CrossIconId);
			this.strikethroughImage.sprite = (flag ? (LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.CrossIconId, null) ?? this.defaultStrikethroughSprite) : this.defaultStrikethroughSprite);
			this.strikethroughImage.rectTransform.sizeDelta = ((flag && this.icon != null) ? this.icon.rectTransform.sizeDelta : this.defaultStrikethroughSize);
		}
		this.strikethrough.SetActive(!this.data.IsInRange);
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x000FBF28 File Offset: 0x000FA128
	private void CacheStrikethroughDefaults()
	{
		if (this.strikethroughDefaultsCached)
		{
			return;
		}
		this.strikethroughDefaultsCached = true;
		this.strikethroughImage = this.strikethrough.GetComponent<Image>();
		if (this.strikethroughImage != null)
		{
			this.defaultStrikethroughSprite = this.strikethroughImage.sprite;
			this.defaultStrikethroughSize = this.strikethroughImage.rectTransform.sizeDelta;
		}
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040029C6 RID: 10694
	private const string FallbackIconId = "i_b_blueprint_placeholder";

	// Token: 0x040029C7 RID: 10695
	private const string BackgroundChildName = "Background";

	// Token: 0x040029C8 RID: 10696
	[SerializeField]
	private Image icon;

	// Token: 0x040029C9 RID: 10697
	[SerializeField]
	private GameObject strikethrough;

	// Token: 0x040029CA RID: 10698
	private GameObject background;

	// Token: 0x040029CB RID: 10699
	private Image strikethroughImage;

	// Token: 0x040029CC RID: 10700
	private Sprite defaultStrikethroughSprite;

	// Token: 0x040029CD RID: 10701
	private Vector2 defaultStrikethroughSize;

	// Token: 0x040029CE RID: 10702
	private bool strikethroughDefaultsCached;
}
