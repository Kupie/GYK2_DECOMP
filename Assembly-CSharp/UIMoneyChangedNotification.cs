using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x0200088A RID: 2186
public class UIMoneyChangedNotification : UIBaseNotification
{
	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x0600381D RID: 14365 RVA: 0x0010E5D5 File Offset: 0x0010C7D5
	// (set) Token: 0x0600381E RID: 14366 RVA: 0x0010E5DD File Offset: 0x0010C7DD
	public int DisplayingCount { get; set; }

	// Token: 0x0600381F RID: 14367 RVA: 0x0010E5E8 File Offset: 0x0010C7E8
	public override void Draw()
	{
		this.label.text = Trading.FormatMoney(this.DisplayingCount, false, " ", GameResIconType.MoneyBig);
		if (this.DisplayingCount >= 0)
		{
			this.plusStyle.ApplyStyle(this.label, false, null, null, null);
			return;
		}
		this.minusStyle.ApplyStyle(this.label, false, null, null, null);
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x0010E67A File Offset: 0x0010C87A
	public void AddCount(int value)
	{
		this.DisplayingCount += value;
		base.CurrentTime = 0f;
		this.Draw();
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x0010E69B File Offset: 0x0010C89B
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UIMoneyChangedNotification>(this);
	}

	// Token: 0x04002CB5 RID: 11445
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002CB6 RID: 11446
	[SerializeField]
	private TextStyle plusStyle;

	// Token: 0x04002CB7 RID: 11447
	[SerializeField]
	private TextStyle minusStyle;
}
