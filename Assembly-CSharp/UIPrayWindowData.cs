using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A12 RID: 2578
public class UIPrayWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x06004558 RID: 17752 RVA: 0x0014844D File Offset: 0x0014664D
	// (set) Token: 0x06004559 RID: 17753 RVA: 0x00148455 File Offset: 0x00146655
	public Action OnPraySlotPress { get; private set; }

	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x0600455A RID: 17754 RVA: 0x0014845E File Offset: 0x0014665E
	// (set) Token: 0x0600455B RID: 17755 RVA: 0x00148466 File Offset: 0x00146666
	public PlayerData PlayerData { get; private set; }

	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x0600455C RID: 17756 RVA: 0x0014846F File Offset: 0x0014666F
	// (set) Token: 0x0600455D RID: 17757 RVA: 0x00148477 File Offset: 0x00146677
	public WgoData PrayingStand { get; private set; }

	// Token: 0x17000A99 RID: 2713
	// (get) Token: 0x0600455E RID: 17758 RVA: 0x00148480 File Offset: 0x00146680
	// (set) Token: 0x0600455F RID: 17759 RVA: 0x00148488 File Offset: 0x00146688
	public CraftElementSermon CraftQueueElement { get; private set; }

	// Token: 0x17000A9A RID: 2714
	// (get) Token: 0x06004560 RID: 17760 RVA: 0x00148491 File Offset: 0x00146691
	// (set) Token: 0x06004561 RID: 17761 RVA: 0x00148499 File Offset: 0x00146699
	public string RewardBoxId { get; private set; }

	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x06004562 RID: 17762 RVA: 0x001484A2 File Offset: 0x001466A2
	// (set) Token: 0x06004563 RID: 17763 RVA: 0x001484AA File Offset: 0x001466AA
	public SermonDef SermonDef { get; set; }

	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x06004564 RID: 17764 RVA: 0x001484B3 File Offset: 0x001466B3
	// (set) Token: 0x06004565 RID: 17765 RVA: 0x001484BB File Offset: 0x001466BB
	private MultiInventory PlayerMultiInventory { get; set; }

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x06004566 RID: 17766 RVA: 0x001484C4 File Offset: 0x001466C4
	// (set) Token: 0x06004567 RID: 17767 RVA: 0x001484CC File Offset: 0x001466CC
	public int ChurchQuality { get; private set; }

	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x06004568 RID: 17768 RVA: 0x001484D5 File Offset: 0x001466D5
	// (set) Token: 0x06004569 RID: 17769 RVA: 0x001484DD File Offset: 0x001466DD
	public int GraveyardQuality { get; private set; }

	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x0600456A RID: 17770 RVA: 0x001484E6 File Offset: 0x001466E6
	// (set) Token: 0x0600456B RID: 17771 RVA: 0x001484EE File Offset: 0x001466EE
	public int Happiness { get; private set; }

	// Token: 0x17000AA0 RID: 2720
	// (get) Token: 0x0600456C RID: 17772 RVA: 0x001484F7 File Offset: 0x001466F7
	// (set) Token: 0x0600456D RID: 17773 RVA: 0x001484FF File Offset: 0x001466FF
	public PerkWidgetData BuffWidgetData { get; private set; }

	// Token: 0x17000AA1 RID: 2721
	// (get) Token: 0x0600456E RID: 17774 RVA: 0x00148508 File Offset: 0x00146708
	// (set) Token: 0x0600456F RID: 17775 RVA: 0x00148510 File Offset: 0x00146710
	public int ResultVisitors { get; private set; }

	// Token: 0x06004570 RID: 17776 RVA: 0x0014851C File Offset: 0x0014671C
	public UIPrayWindowData(PlayerData playerData, WgoData prayingStand, int graveyardQuality, int churchQuality, int happiness, string rewardBoxId)
	{
		this.PlayerData = playerData;
		this.PrayingStand = prayingStand;
		this.RewardBoxId = rewardBoxId;
		this.OnPraySlotPress = new Action(this.HandlePraySlotPress);
		this.ChurchQuality = Mathf.FloorToInt((float)churchQuality);
		this.GraveyardQuality = Mathf.FloorToInt((float)graveyardQuality);
		this.PlayerMultiInventory = new MultiInventory(playerData, true);
		this.Happiness = happiness;
		this.ResultVisitors = Math.Min(this.Happiness, this.ChurchQuality);
	}

	// Token: 0x06004571 RID: 17777 RVA: 0x0014859E File Offset: 0x0014679E
	public bool CanStartCraft()
	{
		return this.EnoughParishioners() && this.CraftQueueElement != null && this.PrayingStand.CraftComponent.GetStartCraftStatus(this.CraftQueueElement, null) == CraftStatus.OK;
	}

	// Token: 0x06004572 RID: 17778 RVA: 0x001485CC File Offset: 0x001467CC
	public bool EnoughParishioners()
	{
		return this.SermonDef != null && this.ResultVisitors >= this.SermonDef.minParishioners;
	}

	// Token: 0x06004573 RID: 17779 RVA: 0x001485F0 File Offset: 0x001467F0
	public void StartCraft(int parishionersCount, int chance)
	{
		this.PlayerData.SubRes("happiness", (float)parishionersCount);
		MainGame.PlayerData.currentSermon = new SermonResultData(this.SermonDef.id, this.RewardBoxId, parishionersCount, global::UnityEngine.Random.Range(0f, 100f) <= (float)chance, this.ChurchQuality, this.GraveyardQuality);
		MainGame.WorldData.Cache.wgoDataByCustomTagsCache["church_tribune_real"][0].SetGameRes("cur_pray_ppl", parishionersCount);
		this.PrayingStand.CraftComponent.TryStartCurCraft();
		this.PrayingStand.CraftComponent.TryFinishCurCraft();
		MainGame.PlayerController.View.SetSermonIcon(this.SermonDef.PrayIcon);
		GlobalScriptsManager.FireEvent("System_Pray", "sermon_start", new Action(this.OnPrayFinished));
	}

	// Token: 0x06004574 RID: 17780 RVA: 0x001486D4 File Offset: 0x001468D4
	private void OnPrayFinished()
	{
		AchievementsSystem.Instance.TriggerCountable("sermon_done", 1);
		WgoData rewardBox = MainGame.Instance.GameSave.worldData.GetWgoData(this.RewardBoxId);
		rewardBox.StoreSermonResult(MainGame.PlayerData.currentSermon);
		foreach (LazyExpression lazyExpression in this.SermonDef.prayOnEndExpressions)
		{
			lazyExpression.Evaluate();
		}
		UIPrayReportWindowData uiprayReportWindowData = new UIPrayReportWindowData(MainGame.PlayerData.currentSermon, delegate
		{
			List<Item> list = OutputItems.MakeOutput(this.SermonDef.successRewardItem.MakePreOutput(rewardBox, 0f));
			for (int i = 0; i < list.Count; i++)
			{
				MainGame.Instance.dropSystem.DropItem(list[i], MainGame.PlayerData.currentGameSceneId, MainGame.PlayerData.position.Value, null);
			}
			if (!string.IsNullOrEmpty(this.SermonDef.successRewardBuff))
			{
				MainGame.Instance.GameSave.perkSystemData.AddPerk(this.SermonDef.successRewardBuff);
			}
		});
		LazyUI.GetWindow<UIPrayReportWindow>().Open(uiprayReportWindowData);
	}

	// Token: 0x06004575 RID: 17781 RVA: 0x001487A4 File Offset: 0x001469A4
	public void EraseNonStartedCraft()
	{
		this.PrayingStand.CraftComponent.RemoveCurNotStartedCraft();
	}

	// Token: 0x06004576 RID: 17782 RVA: 0x001487B8 File Offset: 0x001469B8
	private void HandlePraySlotPress()
	{
		LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(this.PlayerData, delegate(UIItemCell uiItemCell)
		{
			SermonDef sermonDef = GameBalance.GetSermonDef(uiItemCell.DisplayingItem.id);
			if (sermonDef != null)
			{
				this.CreateCraftElementForChosenSermon(sermonDef);
				LazyUI.GetWindow<UIPrayWindow>().Redraw();
				LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
			}
		}, new Func<Item, bool>(this.CanUsePray), true, null, null);
		window.Open(uimultiInventoryWindowData);
	}

	// Token: 0x06004577 RID: 17783 RVA: 0x001487F7 File Offset: 0x001469F7
	private bool CanUsePray(Item item)
	{
		return item != null && !item.IsEmpty && item.Definition.type == ItemType.Preach && GameBalance.GetSermonDef(item.id) != null;
	}

	// Token: 0x06004578 RID: 17784 RVA: 0x00148828 File Offset: 0x00146A28
	private void CreateCraftElementForChosenSermon(SermonDef sermonDef)
	{
		this.SermonDef = sermonDef;
		if (!string.IsNullOrEmpty(this.SermonDef.successRewardBuff))
		{
			this.BuffWidgetData = new PerkWidgetData(new PerkData(this.SermonDef.successRewardBuff), true, null, null, null);
		}
		this.CraftQueueElement = new CraftElementSermon(sermonDef.id, 1, new List<NeedItemData>(), new CraftParamsData(sermonDef.id, new GameRes()));
		MainGame.Instance.craftSystem.AddCraftObject(this.PrayingStand.CraftComponent);
		this.PrayingStand.CraftComponent.RemoveCurNotStartedCraft();
		this.PrayingStand.CraftComponent.AddCraftNoStart(this.CraftQueueElement);
	}
}
