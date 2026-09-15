using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x020008D1 RID: 2257
public class MoneyWidget : LazyWidget<MoneyWidgetData>
{
	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x001196BA File Offset: 0x001178BA
	public TextMeshProUGUI MoneyLabel
	{
		get
		{
			return this.moneyLabel;
		}
	}

	// Token: 0x06003AE6 RID: 15078 RVA: 0x001196C2 File Offset: 0x001178C2
	public override void Redraw()
	{
		base.Redraw();
		this.moneyLabel.text = Trading.FormatMoney(this.data.Money(), true, " ", GameResIconType.MoneyBig);
	}

	// Token: 0x06003AE7 RID: 15079 RVA: 0x001196F8 File Offset: 0x001178F8
	[LazyUITest]
	protected override void TestDraw()
	{
		MoneyWidgetData moneyWidgetData = new MoneyWidgetData();
		moneyWidgetData.Money = () => 11111;
		this.Draw(moneyWidgetData);
	}

	// Token: 0x04002E96 RID: 11926
	[SerializeField]
	private TextMeshProUGUI moneyLabel;
}
