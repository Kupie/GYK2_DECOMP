using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009EF RID: 2543
public class UIItemCountWindow : LazyWindow<UIItemCountWindowData>
{
	// Token: 0x0600447A RID: 17530 RVA: 0x0014527F File Offset: 0x0014347F
	public override void Init()
	{
		this.slider.Init();
		base.Init();
	}

	// Token: 0x0600447B RID: 17531 RVA: 0x00145294 File Offset: 0x00143494
	public override void Open(UIItemCountWindowData data)
	{
		base.Open(data);
		this.priceCalculateDelegate = data.PriceCalculateDel;
		this.itemCell.Draw(data.Item, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.itemCell.ClearCallbacks();
		this.headerLabel.text = data.Item.Definition.GetHeader();
		this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, data.IsForVendor ? this.priceWindowSize : this.noPriceWindowSize);
		this.slider.Open(data.IsForVendor ? 1 : data.Max, data.Min, data.Max, delegate(int _)
		{
			this.RedrawPrice();
		}, true, true, 1, data.SnapStep);
		this.onConfirm = data.OnConfirm;
		data.OkBtnData.onPressed = new Action(this.OnConfirm);
		this.okBtn.Draw(data.OkBtnData);
		if (data.BackBtnData != null)
		{
			data.BackBtnData.onPressed = new Action(this.Close);
			this.backBtn.Draw(data.BackBtnData);
		}
		else
		{
			this.backBtn.gameObject.SetActive(false);
		}
		this.RedrawPrice();
		this.UpdateGamepadDependentStuff();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x0600447C RID: 17532 RVA: 0x00145400 File Offset: 0x00143600
	private void RedrawPrice()
	{
		this.price.text = ((!this.data.IsForVendor) ? "" : (LLBase.L("price_dialog_total") + "\n " + this.moneyStyle.ApplyStyleToString(Trading.FormatMoney(this.priceCalculateDelegate(this.slider.Value), false, " ", GameResIconType.MoneyBig), false, true)));
	}

	// Token: 0x0600447D RID: 17533 RVA: 0x00145473 File Offset: 0x00143673
	private void OnConfirm()
	{
		Action<int> action = this.onConfirm;
		if (action != null)
		{
			action(this.slider.Value);
		}
		LazyUI.GetWindow<UIItemCountWindow>().Close();
	}

	// Token: 0x0600447E RID: 17534 RVA: 0x0014549B File Offset: 0x0014369B
	private bool OnConfirmKeyPressed()
	{
		if (this.okBtn.LazyButton.interactable)
		{
			LazyButton lazyButton = this.okBtn.LazyButton;
			if (lazyButton != null)
			{
				lazyButton.ForceOnClick();
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600447F RID: 17535 RVA: 0x001454C8 File Offset: 0x001436C8
	protected override bool OnPressedBack()
	{
		if (this.data.BackBtnData == null)
		{
			this.Close();
			return true;
		}
		this.backBtn.LazyButton.ForceOnClick();
		return true;
	}

	// Token: 0x06004480 RID: 17536 RVA: 0x001454F0 File Offset: 0x001436F0
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		gameKeyDelegates.Add(GameKey.Left, () => false);
		gameKeyDelegates.Add(GameKey.Right, () => false);
		gameKeyDelegates.Add(GameKey.DpadLeft, () => false);
		gameKeyDelegates.Add(GameKey.DpadRight, () => false);
		gameKeyDelegates.Add(GameKey.IncSlider, () => false);
		gameKeyDelegates.Add(GameKey.DecSlider, () => false);
		gameKeyDelegates.Add(GameKey.ItemCountWindow_Increase, () => false);
		gameKeyDelegates.Add(GameKey.ItemCountWindow_Decrease, () => false);
		gameKeyDelegates.Add(GameKey.ItemCountWindow_Apply, new Func<bool>(this.OnConfirmKeyPressed));
		gameKeyDelegates.Add(GameKey.ItemCountWindow_Cancel, new Func<bool>(this.OnPressedBack));
		return gameKeyDelegates;
	}

	// Token: 0x06004481 RID: 17537 RVA: 0x001456AC File Offset: 0x001438AC
	protected override void PrintTips()
	{
		if (this.data == null)
		{
			return;
		}
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(new LazyGameKeyTip(GameKey.ItemCountWindow_Increase, "+", true, true, false));
		list.Add(new LazyGameKeyTip(GameKey.ItemCountWindow_Decrease, "-", true, true, false));
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06004482 RID: 17538 RVA: 0x0014570C File Offset: 0x0014390C
	[LazyUITest]
	protected override void TestDraw()
	{
		UIItemCountWindowData uiitemCountWindowData = new UIItemCountWindowData();
		uiitemCountWindowData.Item = new Item("apple", 25);
		uiitemCountWindowData.Min = 1;
		uiitemCountWindowData.Max = uiitemCountWindowData.Item.Count;
		uiitemCountWindowData.OnConfirm = delegate(int _)
		{
		};
		uiitemCountWindowData.PriceCalculateDel = (int _) => 11111;
		uiitemCountWindowData.IsForVendor = true;
		uiitemCountWindowData.OkBtnData = new UIDialogWindowData.ButtonData(null, LLBase.L("btn_ok"), null, true, GameKey.Select, "");
		uiitemCountWindowData.BackBtnData = new UIDialogWindowData.ButtonData(null, LLBase.L("btn_cancel"), null, true, GameKey.Back, "");
		this.Open(uiitemCountWindowData);
	}

	// Token: 0x04003567 RID: 13671
	[SerializeField]
	private TextMeshProUGUI headerLabel;

	// Token: 0x04003568 RID: 13672
	[SerializeField]
	private UIDialogWindowButton okBtn;

	// Token: 0x04003569 RID: 13673
	[SerializeField]
	private UIDialogWindowButton backBtn;

	// Token: 0x0400356A RID: 13674
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x0400356B RID: 13675
	[SerializeField]
	private SmartSlider slider;

	// Token: 0x0400356C RID: 13676
	[SerializeField]
	private TextStyle moneyStyle;

	// Token: 0x0400356D RID: 13677
	[SerializeField]
	private float priceWindowSize;

	// Token: 0x0400356E RID: 13678
	[SerializeField]
	private float noPriceWindowSize;

	// Token: 0x0400356F RID: 13679
	private Action<int> onConfirm;

	// Token: 0x04003570 RID: 13680
	public TextMeshProUGUI price;

	// Token: 0x04003571 RID: 13681
	public RectTransform rectTransform;

	// Token: 0x04003572 RID: 13682
	private UIItemCountWindowData.PriceCalculateDelegate priceCalculateDelegate;
}
