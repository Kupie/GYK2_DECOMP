using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007DC RID: 2012
public class HpBarPlayerWidget : LazyWidget<HpBarPlayerWidgetData>, IUIObjectBubbleWidgetWithoutRebuildingLayout
{
	// Token: 0x060033E6 RID: 13286 RVA: 0x000FA9D0 File Offset: 0x000F8BD0
	public override void Redraw()
	{
		base.Redraw();
		Vector2 vector = new Vector2((this.data.CustomWidth > 0f) ? this.data.CustomWidth : this.layoutElement.preferredWidth, (this.data.CustomHeight > 0f) ? this.data.CustomHeight : this.layoutElement.preferredHeight);
		this.layoutElement.preferredWidth = vector.x;
		this.layoutElement.preferredHeight = vector.y;
		this.slider.maxValue = (float)this.data.hpComponent.MaxHpValue;
		int num = BarWigdetUtils.ClampSliderValueToViewableState(Mathf.CeilToInt(this.barImageParentRectTransform.rect.width), this.data.hpComponent.Hp, this.data.hpComponent.MaxHpValue);
		this.slider.value = (float)num;
	}

	// Token: 0x060033E7 RID: 13287 RVA: 0x000FAAC8 File Offset: 0x000F8CC8
	public override void CustomUpdate()
	{
		int num = BarWigdetUtils.ClampSliderValueToViewableState(Mathf.CeilToInt(this.barImageParentRectTransform.rect.width), this.data.hpComponent.Hp, this.data.hpComponent.MaxHpValue);
		this.slider.value = (float)num;
	}

	// Token: 0x060033E8 RID: 13288 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002960 RID: 10592
	[SerializeField]
	private LayoutElement layoutElement;

	// Token: 0x04002961 RID: 10593
	[SerializeField]
	private Slider slider;

	// Token: 0x04002962 RID: 10594
	[SerializeField]
	private RectTransform barImageParentRectTransform;
}
