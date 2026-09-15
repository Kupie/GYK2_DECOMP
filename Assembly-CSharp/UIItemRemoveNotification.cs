using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000889 RID: 2185
public class UIItemRemoveNotification : UIBaseNotification
{
	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x06003816 RID: 14358 RVA: 0x0010E4FD File Offset: 0x0010C6FD
	// (set) Token: 0x06003817 RID: 14359 RVA: 0x0010E505 File Offset: 0x0010C705
	public int DisplayingCount { get; set; }

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x06003818 RID: 14360 RVA: 0x0010E50E File Offset: 0x0010C70E
	// (set) Token: 0x06003819 RID: 14361 RVA: 0x0010E516 File Offset: 0x0010C716
	public string ItemId { get; set; }

	// Token: 0x0600381A RID: 14362 RVA: 0x0010E520 File Offset: 0x0010C720
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
		this.countLabel.gameObject.SetActive(this.DisplayingCount > 1);
		this.countLabel.text = "-" + this.DisplayingCount.ToString();
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x0010E5CD File Offset: 0x0010C7CD
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UIItemRemoveNotification>(this);
	}

	// Token: 0x04002CB0 RID: 11440
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04002CB1 RID: 11441
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x04002CB2 RID: 11442
	[SerializeField]
	private TextMeshProUGUI countLabel;
}
