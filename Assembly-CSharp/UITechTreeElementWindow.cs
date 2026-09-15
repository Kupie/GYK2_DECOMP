using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000964 RID: 2404
public class UITechTreeElementWindow : LazyWindow<UITechTreeElementWindowData>
{
	// Token: 0x06003F52 RID: 16210 RVA: 0x0012FA2A File Offset: 0x0012DC2A
	public override void Init()
	{
		base.Init();
		this.buttonPrefab.gameObject.SetActive(false);
		base.GamepadNavigationController.loopVerticalNavigation = true;
	}

	// Token: 0x06003F53 RID: 16211 RVA: 0x0012FA50 File Offset: 0x0012DC50
	public override void Open(UITechTreeElementWindowData data)
	{
		base.Open(data);
		this.header.text = data.HeaderText;
		if (data.TechDef.TechState != TechState.Unlocked)
		{
			this.resourcesLabel.text = data.TechDef.GetPriceLabel(this.enoughStyle, this.notEnoughStyle, GameResIconType.Common);
			this.resourcesLabel.gameObject.SetActive(true);
		}
		else
		{
			this.resourcesLabel.gameObject.SetActive(false);
		}
		if (string.IsNullOrEmpty(data.BotText))
		{
			this.botLabel.gameObject.SetActive(false);
		}
		else
		{
			this.botLabel.gameObject.SetActive(true);
			this.botLabel.text = data.BotText;
		}
		if (string.IsNullOrEmpty(data.TopText))
		{
			this.topLabel.gameObject.SetActive(false);
		}
		else
		{
			this.topLabel.gameObject.SetActive(true);
			this.topLabel.text = data.TopText;
		}
		LinkedEntityWidgetWithText[] array = this.linkedEntityWidgets;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < data.TechDef.linkedEntityWidgetDatas.Count; j++)
		{
			this.linkedEntityWidgets[j].Draw(data.TechDef.linkedEntityWidgetDatas[j]);
			this.linkedEntityWidgets[j].gameObject.SetActive(true);
			this.linkedEntityWidgets[j].Button.GetComponent<GamepadNavigationItem>().OnSelect.RemoveAllListeners();
		}
		foreach (UIDialogWindowButton uidialogWindowButton in this.activeButtons)
		{
			UIDialogWindow.pool.ReleaseObject<UIDialogWindowButton>(uidialogWindowButton);
		}
		this.activeButtons.Clear();
		for (int k = 0; k < data.ButtonsData.Count; k++)
		{
			UIDialogWindowButton orCreateObject = UIDialogWindow.pool.GetOrCreateObject<UIDialogWindowButton>();
			UIDialogWindowData.ButtonData buttonData = data.ButtonsData[k];
			orCreateObject.transform.SetParent(this.buttonsContent);
			orCreateObject.Draw(buttonData);
			this.activeButtons.Add(orCreateObject);
			orCreateObject.transform.SetSiblingIndex(k);
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06003F54 RID: 16212 RVA: 0x0012FCC4 File Offset: 0x0012DEC4
	protected override void PrintTips()
	{
		this.lazyButtonTips.Clear();
	}

	// Token: 0x06003F55 RID: 16213 RVA: 0x0012FCD1 File Offset: 0x0012DED1
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x06003F56 RID: 16214 RVA: 0x0012FD04 File Offset: 0x0012DF04
	protected override bool OnPressedBack()
	{
		foreach (UIDialogWindowButton uidialogWindowButton in this.activeButtons)
		{
			if (uidialogWindowButton.ReplaceForGamepad && uidialogWindowButton.KeyToReplace.value == GameKey.Back.value)
			{
				return false;
			}
		}
		return base.OnPressedBack();
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x0012FD7C File Offset: 0x0012DF7C
	[LazyUITest]
	protected override void TestDraw()
	{
		UITechTreeElementWindow.<>c__DisplayClass15_0 CS$<>8__locals1 = new UITechTreeElementWindow.<>c__DisplayClass15_0();
		CS$<>8__locals1.techDef = GameBalance.Me.GetData<TechDef>("wood_basic");
		UIDialogWindowData.ButtonData buttonData = new UIDialogWindowData.ButtonData(new Action(CS$<>8__locals1.<TestDraw>g__Unlock|0), LLBase.L("btn_unlock"), () => CS$<>8__locals1.techDef.TechState == TechState.Available && CS$<>8__locals1.techDef.EnoughResources, true, GameKey.Select, "");
		UIDialogWindowData.ButtonData buttonData2 = new UIDialogWindowData.ButtonData(new Action(LazyUI.GetWindow<UITechTreeElementWindow>().Close), LLBase.L("btn_cancel"), null, true, GameKey.Back, "");
		UITechTreeElementWindowData uitechTreeElementWindowData = new UITechTreeElementWindowData(CS$<>8__locals1.techDef, new List<UIDialogWindowData.ButtonData> { buttonData, buttonData2 }, CS$<>8__locals1.techDef.ParentsUnlocked ? string.Empty : LLBase.L("tech_not_all_techs_unlocked"), null);
		LazyUI.GetWindow<UITechTreeElementWindow>().Open(uitechTreeElementWindowData);
	}

	// Token: 0x040031E8 RID: 12776
	[SerializeField]
	private TextMeshProUGUI header;

	// Token: 0x040031E9 RID: 12777
	[SerializeField]
	private TextMeshProUGUI topLabel;

	// Token: 0x040031EA RID: 12778
	[SerializeField]
	private TextMeshProUGUI botLabel;

	// Token: 0x040031EB RID: 12779
	[SerializeField]
	private TextMeshProUGUI resourcesLabel;

	// Token: 0x040031EC RID: 12780
	[SerializeField]
	private TextStyle enoughStyle;

	// Token: 0x040031ED RID: 12781
	[SerializeField]
	private TextStyle notEnoughStyle;

	// Token: 0x040031EE RID: 12782
	[SerializeField]
	private LinkedEntityWidgetWithText[] linkedEntityWidgets;

	// Token: 0x040031EF RID: 12783
	[SerializeField]
	private UIDialogWindowButton buttonPrefab;

	// Token: 0x040031F0 RID: 12784
	[SerializeField]
	private RectTransform buttonsContent;

	// Token: 0x040031F1 RID: 12785
	private List<UIDialogWindowButton> activeButtons = new List<UIDialogWindowButton>();
}
