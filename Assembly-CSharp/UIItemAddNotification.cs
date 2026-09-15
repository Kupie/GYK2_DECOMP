using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000888 RID: 2184
public class UIItemAddNotification : UIBaseNotification
{
	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x0600380D RID: 14349 RVA: 0x0010E3E8 File Offset: 0x0010C5E8
	// (set) Token: 0x0600380E RID: 14350 RVA: 0x0010E3F0 File Offset: 0x0010C5F0
	public int DisplayingCount { get; set; }

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x0600380F RID: 14351 RVA: 0x0010E3F9 File Offset: 0x0010C5F9
	// (set) Token: 0x06003810 RID: 14352 RVA: 0x0010E401 File Offset: 0x0010C601
	public string ItemId { get; set; }

	// Token: 0x06003811 RID: 14353 RVA: 0x0010E40C File Offset: 0x0010C60C
	public override void Draw()
	{
		ItemDef data = GameBalance.Me.GetData<ItemDef>(this.ItemId);
		if (data != null)
		{
			this.nameLabel.text = data.GetHeader();
		}
		else
		{
			this.nameLabel.text = LLBase.L(this.ItemId);
		}
		this.itemCell.Draw(new Item(this.ItemId, this.DisplayingCount), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.UpdateCountLabel();
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x0010E483 File Offset: 0x0010C683
	public void AddCountToItem(int value)
	{
		this.DisplayingCount += value;
		base.CurrentTime = 0f;
		this.UpdateCountLabel();
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x0010E4A4 File Offset: 0x0010C6A4
	private void UpdateCountLabel()
	{
		this.countLabel.gameObject.SetActive(this.DisplayingCount > 1);
		this.countLabel.text = "+" + this.DisplayingCount.ToString();
	}

	// Token: 0x06003814 RID: 14356 RVA: 0x0010E4ED File Offset: 0x0010C6ED
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UIItemAddNotification>(this);
	}

	// Token: 0x04002CAB RID: 11435
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04002CAC RID: 11436
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x04002CAD RID: 11437
	[SerializeField]
	private TextMeshProUGUI countLabel;
}
