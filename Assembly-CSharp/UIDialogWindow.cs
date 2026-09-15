using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009E8 RID: 2536
public class UIDialogWindow : LazyWindow<UIDialogWindowData>
{
	// Token: 0x0600441E RID: 17438 RVA: 0x00143B61 File Offset: 0x00141D61
	public override void Init()
	{
		base.Init();
		UIDialogWindow.pool = LazyPooler.CreatePool<UIDialogWindowButton>(this.buttonPrefab, 0, Pool.PoolType.ImmediateActivation, false, false, null);
		this.buttonPrefab.gameObject.SetActive(false);
	}

	// Token: 0x0600441F RID: 17439 RVA: 0x00143B90 File Offset: 0x00141D90
	public override void Open(UIDialogWindowData data)
	{
		base.Open(data);
		bool flag = !string.IsNullOrEmpty(data.Header);
		this.information.text = data.Information;
		this.header.text = data.Header;
		if (flag)
		{
			this.genericWindowLayout.UpdateSize(false);
		}
		else
		{
			this.genericWindowLayout.UpdateSize(false);
		}
		foreach (UIDialogWindowButton uidialogWindowButton in this.activeButtons)
		{
			UIDialogWindow.pool.ReleaseObject<UIDialogWindowButton>(uidialogWindowButton);
		}
		this.activeButtons.Clear();
		for (int i = 0; i < data.ButtonsData.Count; i++)
		{
			UIDialogWindowButton orCreateObject = UIDialogWindow.pool.GetOrCreateObject<UIDialogWindowButton>();
			UIDialogWindowData.ButtonData buttonData = data.ButtonsData[i];
			orCreateObject.transform.SetParent(this.buttonsContent);
			orCreateObject.Draw(buttonData);
			this.activeButtons.Add(orCreateObject);
			orCreateObject.transform.SetSiblingIndex(i);
		}
		this.UpdateGamepadDependentStuff();
		if (data.Item != null)
		{
			this.itemCell.Draw(data.Item, false, -1, false, 1, false, 0, data.DrawItemCounter, false, false, ItemRelatedWidgetState.NotSet, false);
			this.itemCell.gameObject.SetActive(true);
			this.itemCell.transform.SetAsFirstSibling();
			if (LazyInput.IsGamepadActive)
			{
				base.GamepadNavigationController.ReinitItems(false, null, null);
				if (this.activeButtons.Count > 0)
				{
					base.GamepadNavigationController.SetFocusedItem(this.activeButtons[0].GetComponent<GamepadNavigationItem>());
				}
				else
				{
					base.GamepadNavigationController.FocusOnFirstActive(-1);
				}
			}
		}
		else
		{
			this.itemCell.gameObject.SetActive(false);
			if (LazyInput.IsGamepadActive)
			{
				base.GamepadNavigationController.ReinitItems(true, null, null);
			}
		}
		this.itemIconWithBackgroundParent.SetActive(data.ShowAltVersion);
		this.informationBotText.gameObject.SetActive(data.ShowAltVersion && !string.IsNullOrEmpty(data.InformationBot));
		if (data.ShowAltVersion)
		{
			this.itemIconWithBackgroundParent.SetActive(true);
			this.itemIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(data.ItemIconId, null);
			this.itemIcon.BlueColorReplace(this.toReplace);
			this.itemIconText.text = data.ItemName;
			this.itemCounterText.text = string.Format("{0}/{1}", data.HasItemCount, data.NeedItemCount);
			this.itemCounterStyleComponent.SetTextStyle((data.HasItemCount >= data.NeedItemCount) ? this.itemCounterEnough : this.itemCounterNotEnough);
			this.informationBotText.text = data.InformationBot;
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004420 RID: 17440 RVA: 0x00143E70 File Offset: 0x00142070
	protected override void InitCloseButton(LazyButton button)
	{
		button.onClick.AddListener(new UnityAction(this.OnCloseButtonClicked));
	}

	// Token: 0x06004421 RID: 17441 RVA: 0x00143E89 File Offset: 0x00142089
	private void OnCloseButtonClicked()
	{
		UIDialogWindowData data = this.data;
		if (((data != null) ? data.CloseButtonAction : null) != null)
		{
			this.data.CloseButtonAction();
			return;
		}
		this.Close();
	}

	// Token: 0x06004422 RID: 17442 RVA: 0x00143EB8 File Offset: 0x001420B8
	protected override bool OnPressedBack()
	{
		foreach (UIDialogWindowButton uidialogWindowButton in this.activeButtons)
		{
			if (uidialogWindowButton.ReplaceForGamepad && uidialogWindowButton.KeyToReplace.value == GameKey.Back.value)
			{
				return false;
			}
		}
		if (this.closeButton && this.data.ShowCloseButton)
		{
			this.OnCloseButtonClicked();
			return true;
		}
		return false;
	}

	// Token: 0x06004423 RID: 17443 RVA: 0x00143F4C File Offset: 0x0014214C
	protected override void PrintTips()
	{
		this.lazyButtonTips.Clear();
	}

	// Token: 0x06004424 RID: 17444 RVA: 0x00143F5C File Offset: 0x0014215C
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (LazyInput.IsGamepadActive)
		{
			this.closeButton.gameObject.SetActive(false);
			return;
		}
		if (this.data != null)
		{
			this.closeButton.gameObject.SetActive(this.data.ShowCloseButton);
		}
	}

	// Token: 0x06004425 RID: 17445 RVA: 0x00143FAB File Offset: 0x001421AB
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x06004426 RID: 17446 RVA: 0x00143FE0 File Offset: 0x001421E0
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Open(new UIDialogWindowData("Header", "Information window test information text", new UIDialogWindowData.ButtonData(null, LLBase.L("btn_yes"), null, true, GameKey.Select, ""), new UIDialogWindowData.ButtonData(null, LLBase.L("btn_no"), null, true, GameKey.Back, "")));
	}

	// Token: 0x06004427 RID: 17447 RVA: 0x0014403C File Offset: 0x0014223C
	[LazyUITest]
	protected void TestDrawItemCell()
	{
		UIDialogWindowData.ButtonData buttonData = new UIDialogWindowData.ButtonData(delegate
		{
		}, LLBase.L("btn_extract"), null, true, GameKey.ExtractBody, "");
		this.Open(new UIDialogWindowData(new Item("heart_1_1", 1), LLBase.L("extract_organ"), LLBase.L("chance_success") + " 50%", buttonData, false));
	}

	// Token: 0x0400351D RID: 13597
	[SerializeField]
	private TextMeshProUGUI header;

	// Token: 0x0400351E RID: 13598
	[SerializeField]
	private TextMeshProUGUI information;

	// Token: 0x0400351F RID: 13599
	[SerializeField]
	private UIDialogWindowButton buttonPrefab;

	// Token: 0x04003520 RID: 13600
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04003521 RID: 13601
	[SerializeField]
	private GenericWindowLayout genericWindowLayout;

	// Token: 0x04003522 RID: 13602
	[SerializeField]
	private GameObject itemIconWithBackgroundParent;

	// Token: 0x04003523 RID: 13603
	[SerializeField]
	private Image itemIcon;

	// Token: 0x04003524 RID: 13604
	[SerializeField]
	private TextMeshProUGUI itemIconText;

	// Token: 0x04003525 RID: 13605
	[SerializeField]
	private TextMeshProUGUI itemCounterText;

	// Token: 0x04003526 RID: 13606
	[SerializeField]
	private TextStyleComponent itemCounterStyleComponent;

	// Token: 0x04003527 RID: 13607
	[SerializeField]
	private TextStyle itemCounterEnough;

	// Token: 0x04003528 RID: 13608
	[SerializeField]
	private TextStyle itemCounterNotEnough;

	// Token: 0x04003529 RID: 13609
	[SerializeField]
	private TextMeshProUGUI informationBotText;

	// Token: 0x0400352A RID: 13610
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);

	// Token: 0x0400352B RID: 13611
	[SerializeField]
	private RectTransform buttonsContent;

	// Token: 0x0400352C RID: 13612
	private List<UIDialogWindowButton> activeButtons = new List<UIDialogWindowButton>();

	// Token: 0x0400352D RID: 13613
	public static Pool pool;
}
