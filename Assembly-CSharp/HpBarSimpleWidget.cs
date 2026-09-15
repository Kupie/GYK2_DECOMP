using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007DE RID: 2014
public class HpBarSimpleWidget : LazyWidget<HpBarSimpleWidgetData>, IUIObjectBubbleWidgetWithoutRebuildingLayout
{
	// Token: 0x060033EF RID: 13295 RVA: 0x000FAB84 File Offset: 0x000F8D84
	public override void Redraw()
	{
		base.Redraw();
		Vector2 vector = new Vector2((this.data.CustomWidth > 0f) ? this.data.CustomWidth : this.layoutElement.preferredWidth, (this.data.CustomHeight > 0f) ? this.data.CustomHeight : this.layoutElement.preferredHeight);
		this.layoutElement.preferredWidth = vector.x;
		this.layoutElement.preferredHeight = vector.y;
		this.slider.maxValue = (float)this.data.hpComponent.MaxHpValue;
		int num = BarWigdetUtils.ClampSliderValueToViewableState(Mathf.CeilToInt(this.barImageParentRectTransform.rect.width), this.data.hpComponent.Hp, this.data.hpComponent.MaxHpValue);
		this.slider.value = (float)num;
		HpBarSimpleWidgetData.SpriteType sprite = this.data.Sprite;
		if (sprite == HpBarSimpleWidgetData.SpriteType.Ally)
		{
			this.barImage.sprite = this.allySprite;
			return;
		}
		if (sprite != HpBarSimpleWidgetData.SpriteType.Enemy)
		{
			return;
		}
		this.barImage.sprite = this.enemySprite;
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x000FACB4 File Offset: 0x000F8EB4
	public override void CustomUpdate()
	{
		int num = BarWigdetUtils.ClampSliderValueToViewableState(Mathf.CeilToInt(this.barImageParentRectTransform.rect.width), this.data.hpComponent.Hp, this.data.hpComponent.MaxHpValue);
		this.slider.value = (float)num;
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x000E80FD File Offset: 0x000E62FD
	protected override void TestDraw()
	{
		throw new NotImplementedException();
	}

	// Token: 0x04002965 RID: 10597
	[SerializeField]
	private LayoutElement layoutElement;

	// Token: 0x04002966 RID: 10598
	[SerializeField]
	private Slider slider;

	// Token: 0x04002967 RID: 10599
	[SerializeField]
	private Image barImage;

	// Token: 0x04002968 RID: 10600
	[SerializeField]
	private RectTransform barImageParentRectTransform;

	// Token: 0x04002969 RID: 10601
	[Space]
	[SerializeField]
	private Sprite allySprite;

	// Token: 0x0400296A RID: 10602
	[SerializeField]
	private Sprite enemySprite;
}
