using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A08 RID: 2568
public class UIPorterStationItemCell : MonoBehaviour
{
	// Token: 0x17000A8B RID: 2699
	// (get) Token: 0x0600451F RID: 17695 RVA: 0x00146F2A File Offset: 0x0014512A
	public RectTransform RectTransform
	{
		get
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.transform as RectTransform;
			}
			return this.rectTransform;
		}
	}

	// Token: 0x06004520 RID: 17696 RVA: 0x00146F54 File Offset: 0x00145154
	public void Draw(Item item, bool isListed, Action<UIItemCell> onItemCellPress)
	{
		this.itemCell.Draw(item, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.item = item;
		this.isListed = isListed;
		this.onItemCellPress = onItemCellPress;
		this.itemCell.OnItemCellPress = new Action<UIItemCell>(this.OnPressed);
		this.itemCell.SetNativeSizeForIcon();
		this.isBig = item.Definition.itemSize == ItemSize.Big;
		this.UpdateSize(this.isBig);
		if (isListed)
		{
			this.checkmark.SetActive(true);
			this.shade.SetActive(false);
			return;
		}
		this.checkmark.SetActive(false);
		this.shade.SetActive(true);
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x00147004 File Offset: 0x00145204
	private void OnPressed(UIItemCell cell)
	{
		LazyAudio.PlayAndForget("gui_click");
		Action<UIItemCell> action = this.onItemCellPress;
		if (action != null)
		{
			action(cell);
		}
		this.isListed = !this.isListed;
		this.Draw(this.item, this.isListed, this.onItemCellPress);
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x00147054 File Offset: 0x00145254
	private void UpdateSize(bool big)
	{
		if (big)
		{
			this.RectTransform.sizeDelta = this.bigLayoutSize;
			return;
		}
		this.RectTransform.sizeDelta = this.defaultLayoutSize;
	}

	// Token: 0x040035F3 RID: 13811
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x040035F4 RID: 13812
	[SerializeField]
	private Vector2 defaultLayoutSize;

	// Token: 0x040035F5 RID: 13813
	[SerializeField]
	private Vector2 bigLayoutSize;

	// Token: 0x040035F6 RID: 13814
	[SerializeField]
	private GameObject shade;

	// Token: 0x040035F7 RID: 13815
	[SerializeField]
	private GameObject checkmark;

	// Token: 0x040035F8 RID: 13816
	private RectTransform rectTransform;

	// Token: 0x040035F9 RID: 13817
	private bool isBig;

	// Token: 0x040035FA RID: 13818
	private Action<UIItemCell> onItemCellPress;

	// Token: 0x040035FB RID: 13819
	private bool isListed;

	// Token: 0x040035FC RID: 13820
	private Item item;
}
