using System;
using LazyBearTechnology;

// Token: 0x020009E4 RID: 2532
public class UIGraveWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x060043F6 RID: 17398 RVA: 0x00143470 File Offset: 0x00141670
	// (set) Token: 0x060043F7 RID: 17399 RVA: 0x00143478 File Offset: 0x00141678
	public UICorpseWidgetData CorpseWidgetData { get; private set; }

	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x060043F8 RID: 17400 RVA: 0x00143481 File Offset: 0x00141681
	// (set) Token: 0x060043F9 RID: 17401 RVA: 0x00143489 File Offset: 0x00141689
	public UIGraveElementWidgetData TombstoneWidgetData { get; private set; }

	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x060043FA RID: 17402 RVA: 0x00143492 File Offset: 0x00141692
	// (set) Token: 0x060043FB RID: 17403 RVA: 0x0014349A File Offset: 0x0014169A
	public UIGraveElementWidgetData FenceWidgetData { get; private set; }

	// Token: 0x17000A5B RID: 2651
	// (get) Token: 0x060043FC RID: 17404 RVA: 0x001434A3 File Offset: 0x001416A3
	// (set) Token: 0x060043FD RID: 17405 RVA: 0x001434AB File Offset: 0x001416AB
	public float Quality { get; private set; }

	// Token: 0x17000A5C RID: 2652
	// (get) Token: 0x060043FE RID: 17406 RVA: 0x001434B4 File Offset: 0x001416B4
	// (set) Token: 0x060043FF RID: 17407 RVA: 0x001434BC File Offset: 0x001416BC
	public WgoData WgoData { get; private set; }

	// Token: 0x06004400 RID: 17408 RVA: 0x001434C8 File Offset: 0x001416C8
	public UIGraveWindowData(Wgo wgo)
	{
		this.WgoData = wgo.Data;
		this.Quality = wgo.Data.Quality;
		Item item = null;
		foreach (Item item2 in wgo.Data.Inventory.Data.Inventory)
		{
			if (item2.Definition.itemGroupIds.Contains("body"))
			{
				item = item2;
				break;
			}
		}
		if (item != null)
		{
			this.CorpseWidgetData = new UICorpseWidgetData(item, wgo.Data, new Action(this.TryExhumeBody), this.CanExhume(), GameKey.ExtractBody, LLBase.L("ui_grave_corpse_widget_header"), LLBase.L("ui_grave_corpse_text"), LLBase.L("exhume"), new Action<LazyButton>(this.ShowExhumeButtonTooltip));
		}
		else
		{
			this.CorpseWidgetData = new UICorpseWidgetData(GameKey.ExtractBody);
		}
		this.TombstoneWidgetData = new UIGraveElementWidgetData(wgo, GraveElementType.Top);
		this.FenceWidgetData = new UIGraveElementWidgetData(wgo, GraveElementType.Bot);
	}

	// Token: 0x06004401 RID: 17409 RVA: 0x001435E8 File Offset: 0x001417E8
	private bool CanExhume()
	{
		foreach (WgoPartData wgoPartData in this.WgoData.AdditionalWgoPartsData)
		{
			ItemDef dataOrNull = GameBalance.Me.GetDataOrNull<ItemDef>(wgoPartData.id);
			if (dataOrNull != null && (dataOrNull.itemGroupIds.Contains("gravetop") || dataOrNull.itemGroupIds.Contains("gravebot")))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004402 RID: 17410 RVA: 0x00143678 File Offset: 0x00141878
	private void TryExhumeBody()
	{
		if (this.CorpseWidgetData.IsEmpty)
		{
			return;
		}
		if (this.CorpseWidgetData.Body == null)
		{
			return;
		}
		if (!this.CanExhume())
		{
			return;
		}
		LazyWindow<UIDialogWindowData> window = LazyUI.GetWindow<UIDialogWindow>();
		UIDialogWindowData uidialogWindowData;
		if (this.HasExhumeCertificate())
		{
			uidialogWindowData = new UIDialogWindowData(new Item("exhume_certificate", 1), LLBase.L("exhume"), LLBase.L("exhume_confirmation"), LLBase.L("exhume_confirmation_bot"), MainGame.PlayerData.inventory.Data.GetTotalCountInInventory("exhume_certificate", null, false), 1, new Action(this.ExhumeBody), delegate
			{
				LazyUI.GetWindow<UIDialogWindow>().Close();
			}, false);
		}
		else
		{
			uidialogWindowData = new UIDialogWindowData(new Item("exhume_certificate", 1), LLBase.L("exhume"), LLBase.L("exhume_confirmation"), "", 0, 1, delegate
			{
				LazyUI.GetWindow<UIDialogWindow>().Close();
			}, false);
		}
		uidialogWindowData.ShowCloseButton = false;
		window.Open(uidialogWindowData);
	}

	// Token: 0x06004403 RID: 17411 RVA: 0x0014378C File Offset: 0x0014198C
	private void ExhumeBody()
	{
		MainGame.PlayerData.inventory.RemoveItemById("exhume_certificate", 1, null, null, false);
		MainGame.Instance.GameSave.worldData.ChangeWgoData(this.WgoData, "grave_exhume");
		LazyUI.GetWindow<UIDialogWindow>().Close();
		LazyUI.GetWindow<UIGraveWindow>().Close();
	}

	// Token: 0x06004404 RID: 17412 RVA: 0x001437E5 File Offset: 0x001419E5
	private bool HasExhumeCertificate()
	{
		return MainGame.PlayerData.inventory.Data.GetTotalCountInInventory("exhume_certificate", null, false) > 0;
	}

	// Token: 0x06004405 RID: 17413 RVA: 0x00143805 File Offset: 0x00141A05
	private void ShowExhumeButtonTooltip(LazyButton exhumeButton)
	{
		UITooltip.ShowExhumeButtonWidget(exhumeButton);
	}
}
