using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A6F RID: 2671
public class UIVendorOrdersListWidget : LazyWidget<UIVendorOrdersListWidgetData>
{
	// Token: 0x06004883 RID: 18563 RVA: 0x00157630 File Offset: 0x00155830
	private void Awake()
	{
		if (this.characterIconBtn != null)
		{
			this.characterIconBtn.onEnter.AddListener(new UnityAction(this.OnOver));
			this.characterIconBtn.onExit.AddListener(new UnityAction(this.OnOut));
		}
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x00157684 File Offset: 0x00155884
	public override void Redraw()
	{
		base.Redraw();
		this.ReleaseDrawnOrders();
		int i;
		Predicate<SGuid> <>9__0;
		int j;
		for (i = 0; i < this.data.Vendor.Orders.Count; i = j + 1)
		{
			UIVendorOrderWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIVendorOrderWidget>(this.elementsContent);
			List<SGuid> currentOrders = MainGame.Instance.GameSave.vendorSystem.currentOrders;
			Predicate<SGuid> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (SGuid o) => o.Guid == this.data.Vendor.Orders[i].Guid.Guid);
			}
			bool flag = currentOrders.Find(predicate) != null;
			bool flag2 = this.data.Vendor.Orders[i].State == VendorOrderState.Finished;
			elementFromPool.Draw(new UIVendorOrderWidgetData(this.data.Vendor, this.data.Vendor.Orders[i], false, new ValueTuple<bool, bool>(this.data.IsAllNotInteractable || flag || flag2, false), this.data.OnElementPressed, -1, false));
			elementFromPool.Init();
			this.drawnOrders.Add(elementFromPool);
			j = i;
		}
		this.vendorIcon.sprite = this.data.Vendor.Definition.Icon;
		this.vendorIcon.enabled = this.vendorIcon.sprite != null;
		this.vendorIcon.BlueColorReplace(this.toReplace);
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x00157811 File Offset: 0x00155A11
	public override void Hide()
	{
		this.ReleaseDrawnOrders();
		base.Hide();
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x00157820 File Offset: 0x00155A20
	private void ReleaseDrawnOrders()
	{
		foreach (UIVendorOrderWidget uivendorOrderWidget in this.drawnOrders)
		{
			uivendorOrderWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIVendorOrderWidget>(uivendorOrderWidget);
		}
		this.drawnOrders.Clear();
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x00157888 File Offset: 0x00155A88
	private void OnOver()
	{
		UITooltip.ShowSimpleInfo(this.characterIconBtn.transform, LLBase.L(this.data.Vendor.id), default(Vector2), null);
	}

	// Token: 0x06004888 RID: 18568 RVA: 0x001080F0 File Offset: 0x001062F0
	private void OnOut()
	{
		UITooltip.Hide();
	}

	// Token: 0x06004889 RID: 18569 RVA: 0x001578C4 File Offset: 0x00155AC4
	private void OnDisable()
	{
		if (this.characterIconBtn != null && UITooltip.IsTooltipShowingAtTarget(this.characterIconBtn.transform as RectTransform))
		{
			UITooltip.HideImmediately();
		}
	}

	// Token: 0x0600488A RID: 18570 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400388E RID: 14478
	[SerializeField]
	private Image vendorIcon;

	// Token: 0x0400388F RID: 14479
	[SerializeField]
	private RectTransform elementsContent;

	// Token: 0x04003890 RID: 14480
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04003891 RID: 14481
	[SerializeField]
	private LazyButton characterIconBtn;

	// Token: 0x04003892 RID: 14482
	private List<UIVendorOrderWidget> drawnOrders = new List<UIVendorOrderWidget>();
}
