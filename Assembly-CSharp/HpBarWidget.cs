using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020007E1 RID: 2017
public class HpBarWidget : LazyWidget<HpBarWidgetData>, IBubbleLayoutAlwaysActive
{
	// Token: 0x060033FA RID: 13306 RVA: 0x000FAD83 File Offset: 0x000F8F83
	public override void Redraw()
	{
		base.Redraw();
		this.ApplyProgress();
	}

	// Token: 0x060033FB RID: 13307 RVA: 0x000FAD91 File Offset: 0x000F8F91
	public override void CustomUpdate()
	{
		this.ApplyProgress();
	}

	// Token: 0x060033FC RID: 13308 RVA: 0x000FAD99 File Offset: 0x000F8F99
	public override void Hide()
	{
		base.Hide();
		this.progressBarWidget.Hide();
	}

	// Token: 0x060033FD RID: 13309 RVA: 0x000FADAC File Offset: 0x000F8FAC
	private void ApplyProgress()
	{
		int maxHpValue = this.data.hpComponent.MaxHpValue;
		int num = maxHpValue - this.data.hpComponent.Hp;
		this.progressBarWidget.Apply(maxHpValue, num, 0, null);
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002973 RID: 10611
	[SerializeField]
	private ProgressBarWiget_Simplified progressBarWidget;
}
