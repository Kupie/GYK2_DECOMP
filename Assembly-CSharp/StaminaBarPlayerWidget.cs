using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007EC RID: 2028
public class StaminaBarPlayerWidget : LazyWidget<StaminaBarPlayerWidgetData>, IUIObjectBubbleWidgetWithoutRebuildingLayout
{
	// Token: 0x06003423 RID: 13347 RVA: 0x000FB6FD File Offset: 0x000F98FD
	protected override void SetData(StaminaBarPlayerWidgetData data)
	{
		base.SetData(data);
		data.onNotEnoughStamina = new Action(this.SetAnimatingState);
		data.SubscribeToDataChanges();
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x000FB720 File Offset: 0x000F9920
	public override void Redraw()
	{
		base.Redraw();
		this.slider.maxValue = this.data.StaminaResSystem.Max;
		int num = BarWigdetUtils.ClampSliderValueToViewableState(Mathf.CeilToInt(this.barImageParentRectTransform.rect.width), Mathf.RoundToInt(this.data.StaminaResSystem.Get()), Mathf.RoundToInt(this.data.StaminaResSystem.Max));
		this.slider.value = (float)num;
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x000FB7A4 File Offset: 0x000F99A4
	public override void CustomUpdate()
	{
		if (this.isAnimating)
		{
			this.PlayNotEnoughEffect();
		}
		int num = BarWigdetUtils.ClampSliderValueToViewableState(Mathf.CeilToInt(this.barImageParentRectTransform.rect.width), Mathf.RoundToInt(this.data.StaminaResSystem.Get()), Mathf.RoundToInt(this.data.StaminaResSystem.Max));
		this.slider.value = (float)num;
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x000FB814 File Offset: 0x000F9A14
	private void PlayNotEnoughEffect()
	{
		this.animatingTimer += Time.deltaTime;
		this.animatableImage.color = this.gradient.Evaluate(this.animatingTimer / this.animationTime);
		if (this.animatingTimer >= this.animationTime)
		{
			this.isAnimating = false;
			this.animatingTimer = 0f;
		}
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x000FB876 File Offset: 0x000F9A76
	private void SetAnimatingState()
	{
		this.isAnimating = true;
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x000FB87F File Offset: 0x000F9A7F
	public override void Hide()
	{
		base.Hide();
		this.data.UnsubscribeFromDataChanges();
	}

	// Token: 0x06003429 RID: 13353 RVA: 0x000FB892 File Offset: 0x000F9A92
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new StaminaBarPlayerWidgetData());
	}

	// Token: 0x040029A5 RID: 10661
	[SerializeField]
	private Slider slider;

	// Token: 0x040029A6 RID: 10662
	[SerializeField]
	private RectTransform barImageParentRectTransform;

	// Token: 0x040029A7 RID: 10663
	[SerializeField]
	private Gradient gradient;

	// Token: 0x040029A8 RID: 10664
	[SerializeField]
	private float animationTime;

	// Token: 0x040029A9 RID: 10665
	[SerializeField]
	private Image animatableImage;

	// Token: 0x040029AA RID: 10666
	private bool isAnimating;

	// Token: 0x040029AB RID: 10667
	private float animatingTimer;
}
