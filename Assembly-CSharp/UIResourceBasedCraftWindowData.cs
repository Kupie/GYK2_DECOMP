using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000A25 RID: 2597
public class UIResourceBasedCraftWindowData : LazyWidgetDataBase
{
	// Token: 0x17000AA5 RID: 2725
	// (get) Token: 0x060045C3 RID: 17859 RVA: 0x0014A401 File Offset: 0x00148601
	// (set) Token: 0x060045C4 RID: 17860 RVA: 0x0014A409 File Offset: 0x00148609
	public Item SelectedItem { get; private set; }

	// Token: 0x17000AA6 RID: 2726
	// (get) Token: 0x060045C5 RID: 17861 RVA: 0x0014A412 File Offset: 0x00148612
	// (set) Token: 0x060045C6 RID: 17862 RVA: 0x0014A41A File Offset: 0x0014861A
	public List<NeedItemData> Ingredients { get; private set; }

	// Token: 0x17000AA7 RID: 2727
	// (get) Token: 0x060045C7 RID: 17863 RVA: 0x0014A423 File Offset: 0x00148623
	// (set) Token: 0x060045C8 RID: 17864 RVA: 0x0014A42B File Offset: 0x0014862B
	public List<int> IngredientsHasCount { get; private set; }

	// Token: 0x17000AA8 RID: 2728
	// (get) Token: 0x060045C9 RID: 17865 RVA: 0x0014A434 File Offset: 0x00148634
	// (set) Token: 0x060045CA RID: 17866 RVA: 0x0014A43C File Offset: 0x0014863C
	public int CraftNeedItemsCount { get; private set; }

	// Token: 0x17000AA9 RID: 2729
	// (get) Token: 0x060045CB RID: 17867 RVA: 0x0014A445 File Offset: 0x00148645
	// (set) Token: 0x060045CC RID: 17868 RVA: 0x0014A44D File Offset: 0x0014864D
	public Item FuelItem { get; private set; }

	// Token: 0x17000AAA RID: 2730
	// (get) Token: 0x060045CD RID: 17869 RVA: 0x0014A456 File Offset: 0x00148656
	// (set) Token: 0x060045CE RID: 17870 RVA: 0x0014A45E File Offset: 0x0014865E
	public string BtnText { get; private set; }

	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x060045CF RID: 17871 RVA: 0x0014A467 File Offset: 0x00148667
	// (set) Token: 0x060045D0 RID: 17872 RVA: 0x0014A46F File Offset: 0x0014866F
	public string LabelText { get; private set; }

	// Token: 0x17000AAC RID: 2732
	// (get) Token: 0x060045D1 RID: 17873 RVA: 0x0014A478 File Offset: 0x00148678
	// (set) Token: 0x060045D2 RID: 17874 RVA: 0x0014A480 File Offset: 0x00148680
	public Action OnBtnPressedAction { get; private set; }

	// Token: 0x17000AAD RID: 2733
	// (get) Token: 0x060045D3 RID: 17875 RVA: 0x0014A489 File Offset: 0x00148689
	// (set) Token: 0x060045D4 RID: 17876 RVA: 0x0014A491 File Offset: 0x00148691
	public Action<UIItemCell> OnMainIngredientPressedAction { get; private set; }

	// Token: 0x17000AAE RID: 2734
	// (get) Token: 0x060045D5 RID: 17877 RVA: 0x0014A49A File Offset: 0x0014869A
	// (set) Token: 0x060045D6 RID: 17878 RVA: 0x0014A4A2 File Offset: 0x001486A2
	public WgoData WgoData { get; set; }

	// Token: 0x17000AAF RID: 2735
	// (get) Token: 0x060045D7 RID: 17879 RVA: 0x0014A4AB File Offset: 0x001486AB
	// (set) Token: 0x060045D8 RID: 17880 RVA: 0x0014A4B3 File Offset: 0x001486B3
	public int MainIngredientCount { get; private set; }

	// Token: 0x060045D9 RID: 17881 RVA: 0x0014A4BC File Offset: 0x001486BC
	public UIResourceBasedCraftWindowData(WgoData wgoData)
	{
		UIResourceBasedCraftWindowData.<>c__DisplayClass48_0 CS$<>8__locals1 = new UIResourceBasedCraftWindowData.<>c__DisplayClass48_0();
		CS$<>8__locals1.wgoData = wgoData;
		base..ctor();
		CS$<>8__locals1.<>4__this = this;
		this.WgoData = CS$<>8__locals1.wgoData;
		this.allowedItemIds = new List<string>();
		for (int i = 0; i < GameBalance.Me.surveyDefs.Count; i++)
		{
			SurveyDef surveyDef = GameBalance.Me.surveyDefs[i];
			if (!surveyDef.isOneTimeCraft || !MainGame.Instance.GameSave.knowledgeSystem.IsSurveyCompleted(surveyDef))
			{
				foreach (ItemDef itemDef in surveyDef.GetSurveyedItemDefs())
				{
					this.allowedItemIds.Add(itemDef.id);
				}
			}
		}
		this.OnBtnPressedAction = new Action(this.OnStartSurvey);
		this.onUpdateDataAction = new Action(CS$<>8__locals1.<.ctor>g__OnUpdateData|0);
		this.OnMainIngredientPressedAction = new Action<UIItemCell>(this.OnResourcePickerPressed);
		CS$<>8__locals1.<.ctor>g__OnUpdateData|0();
	}

	// Token: 0x060045DA RID: 17882 RVA: 0x0014A5D8 File Offset: 0x001487D8
	private void OnStartSurvey()
	{
		if (this.WgoData.CraftComponent.TryStartCraft(this.currentCraftElement))
		{
			this.currentCraftElement = this.WgoData.CraftComponent.CurrentCraftElement;
			if (!this.currentSurveyDef.isScienceFuelCraft)
			{
				LazyUI.GetWindow<UIResourceBasedCraftWindow>().Close();
				this.WgoData.ClearWorker();
				return;
			}
			this.currentCraftElement.UpdateActualOutputBeforeFinish();
			this.WgoData.CraftComponent.TryFinishCurCraft();
			Action action = this.onUpdateDataAction;
			if (action != null)
			{
				action();
			}
			LazyUI.GetWindow<UIResourceBasedCraftWindow>().Redraw();
		}
	}

	// Token: 0x060045DB RID: 17883 RVA: 0x0014A66C File Offset: 0x0014886C
	public bool CanStartCraft()
	{
		return this.SelectedItem != null && this.currentCraftElement != null && this.currentCraftElement.CanStartCraft(this.WgoData, null) == CraftStatus.OK;
	}

	// Token: 0x060045DC RID: 17884 RVA: 0x0014A698 File Offset: 0x00148898
	private void OnResourcePickerPressed(UIItemCell windowItemCell)
	{
		LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, delegate(UIItemCell uiItemCell)
		{
			this.SelectedItem = uiItemCell.DisplayingItem;
			this.currentSurveyDef = GameBalance.GetSurveyDefForItemOrNull(uiItemCell.DisplayingItem.id);
			this.currentCraftElement = this.CreateCraftElement(this.SelectedItem, this.currentSurveyDef, this.WgoData);
			Action action = this.onUpdateDataAction;
			if (action != null)
			{
				action();
			}
			LazyUI.GetWindow<UIResourceBasedCraftWindow>().Redraw();
			LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
		}, new Func<Item, bool>(this.IsItemAllowed), true, null, null);
		window.Open(uimultiInventoryWindowData);
	}

	// Token: 0x060045DD RID: 17885 RVA: 0x0014A6D6 File Offset: 0x001488D6
	private bool IsItemAllowed(Item item)
	{
		return item != null && this.allowedItemIds.Contains(item.id);
	}

	// Token: 0x060045DE RID: 17886 RVA: 0x0014A6F0 File Offset: 0x001488F0
	private CraftElementSurvey CreateCraftElement(Item selectedItem, SurveyDef surveyDef, WgoData wgoData)
	{
		List<NeedItemData> list = new List<NeedItemData>();
		list.Add(new NeedItemData(selectedItem.id, surveyDef.SurveyedItem.count));
		for (int i = 1; i < surveyDef.needItems.Count; i++)
		{
			list.Add(surveyDef.needItems[i]);
		}
		return new CraftElementSurvey(surveyDef, list, new CraftParamsData(surveyDef.id, wgoData, CraftParamsData.CraftParamsType.Common, -1), selectedItem.id);
	}

	// Token: 0x040036A5 RID: 13989
	private Action onUpdateDataAction;

	// Token: 0x040036A6 RID: 13990
	private List<string> allowedItemIds;

	// Token: 0x040036A7 RID: 13991
	private CraftElementBase currentCraftElement;

	// Token: 0x040036A8 RID: 13992
	private SurveyDef currentSurveyDef;
}
