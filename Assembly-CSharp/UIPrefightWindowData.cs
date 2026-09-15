using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020009CE RID: 2510
public class UIPrefightWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x060042F4 RID: 17140 RVA: 0x0013E505 File Offset: 0x0013C705
	// (set) Token: 0x060042F5 RID: 17141 RVA: 0x0013E50D File Offset: 0x0013C70D
	public FightDef FightDefinition { get; private set; }

	// Token: 0x17000A27 RID: 2599
	// (get) Token: 0x060042F6 RID: 17142 RVA: 0x0013E516 File Offset: 0x0013C716
	// (set) Token: 0x060042F7 RID: 17143 RVA: 0x0013E51E File Offset: 0x0013C71E
	public int BuildingsQuality { get; private set; }

	// Token: 0x17000A28 RID: 2600
	// (get) Token: 0x060042F8 RID: 17144 RVA: 0x0013E527 File Offset: 0x0013C727
	// (set) Token: 0x060042F9 RID: 17145 RVA: 0x0013E52F File Offset: 0x0013C72F
	public List<WgoData> BuildingInZone { get; private set; }

	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x060042FA RID: 17146 RVA: 0x0013E538 File Offset: 0x0013C738
	// (set) Token: 0x060042FB RID: 17147 RVA: 0x0013E540 File Offset: 0x0013C740
	public List<UIPrefightSquadWidgetData> SquadWidgetDatas { get; private set; }

	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x060042FC RID: 17148 RVA: 0x0013E549 File Offset: 0x0013C749
	// (set) Token: 0x060042FD RID: 17149 RVA: 0x0013E551 File Offset: 0x0013C751
	public int CurrentSquadCount { get; set; }

	// Token: 0x17000A2B RID: 2603
	// (get) Token: 0x060042FE RID: 17150 RVA: 0x0013E55A File Offset: 0x0013C75A
	// (set) Token: 0x060042FF RID: 17151 RVA: 0x0013E562 File Offset: 0x0013C762
	public int CurrentDefencePower { get; set; }

	// Token: 0x17000A2C RID: 2604
	// (get) Token: 0x06004300 RID: 17152 RVA: 0x0013E56B File Offset: 0x0013C76B
	// (set) Token: 0x06004301 RID: 17153 RVA: 0x0013E573 File Offset: 0x0013C773
	public Action OnStartButtonPressed { get; set; }

	// Token: 0x17000A2D RID: 2605
	// (get) Token: 0x06004302 RID: 17154 RVA: 0x0013E57C File Offset: 0x0013C77C
	// (set) Token: 0x06004303 RID: 17155 RVA: 0x0013E584 File Offset: 0x0013C784
	public Action OnPressedSquad { get; set; }

	// Token: 0x17000A2E RID: 2606
	// (get) Token: 0x06004304 RID: 17156 RVA: 0x0013E58D File Offset: 0x0013C78D
	// (set) Token: 0x06004305 RID: 17157 RVA: 0x0013E595 File Offset: 0x0013C795
	public bool AllowClose { get; private set; }

	// Token: 0x17000A2F RID: 2607
	// (get) Token: 0x06004306 RID: 17158 RVA: 0x0013E59E File Offset: 0x0013C79E
	// (set) Token: 0x06004307 RID: 17159 RVA: 0x0013E5A6 File Offset: 0x0013C7A6
	public bool ShowDefencePowerRequirement { get; private set; }

	// Token: 0x17000A30 RID: 2608
	// (get) Token: 0x06004308 RID: 17160 RVA: 0x0013E5AF File Offset: 0x0013C7AF
	public bool HasEnoughDefencePower
	{
		get
		{
			return !this.ShowDefencePowerRequirement || this.CurrentDefencePower >= this.FightDefinition.defencePowerLock;
		}
	}

	// Token: 0x06004309 RID: 17161 RVA: 0x0013E5D4 File Offset: 0x0013C7D4
	public UIPrefightWindowData(string fightId, Action onStartButtonPressed, bool allowClose = true)
	{
		this.AllowClose = allowClose;
		MilitaryBaseData militaryBaseData = MainGame.Instance.GameSave.militaryBaseData;
		this.FightDefinition = GameBalance.Me.GetData<FightDef>(fightId);
		this.ShowDefencePowerRequirement = this.FightDefinition.defencePowerLock > 0 && !this.IsFollowUpFight(fightId);
		this.OnStartButtonPressed = onStartButtonPressed;
		this.BuildingInZone = new List<WgoData>();
		for (int i = 0; i < militaryBaseData.baseBuildings.Count; i++)
		{
			WgoData wgoData = MainGame.WorldData.GetWgoData(militaryBaseData.baseBuildings[i]);
			BuildingDef dataOrNull = GameBalance.Me.GetDataOrNull<BuildingDef>(wgoData.id + "_fb");
			if (dataOrNull != null)
			{
				this.BuildingInZone.Add(wgoData);
				if ((!this.FightDefinition.isBarricadesUnavailable || !GameBalance.Me.HasWgoIdByGroup("barricades", dataOrNull.wgoId)) && (!this.FightDefinition.isTowersUnavailable || !GameBalance.Me.HasWgoIdByGroup("towers", dataOrNull.wgoId)))
				{
					this.BuildingsQuality += (int)wgoData.Quality;
				}
			}
		}
		this.CurrentDefencePower = this.BuildingsQuality;
		this.SquadWidgetDatas = new List<UIPrefightSquadWidgetData>();
		if (militaryBaseData.IsMercenaryPayed)
		{
			WgoData wgoData2 = MainGame.WorldData.GetWgoData(militaryBaseData.FighterContainerMercenary);
			this.SquadWidgetDatas.Add(new UIPrefightSquadWidgetData(wgoData2, true, true, false, null, null));
			this.CurrentSquadCount = 1;
		}
		else
		{
			this.SquadWidgetDatas.Add(new UIPrefightSquadWidgetData());
		}
		bool flag = this.CurrentSquadCount < this.FightDefinition.squads;
		for (int j = 0; j < 5; j++)
		{
			if (j < militaryBaseData.fighterContainers.Count)
			{
				WgoData wgoData3 = MainGame.WorldData.GetWgoData(militaryBaseData.fighterContainers[j]);
				this.SquadWidgetDatas.Add(new UIPrefightSquadWidgetData(wgoData3, false, false, flag, new Action<UIPrefightSquadWidget>(this.OnSquadPressedDefault), new Action<UIPrefightSquadWidget>(this.OnSquadPressedTurnedOn)));
			}
			else
			{
				this.SquadWidgetDatas.Add(new UIPrefightSquadWidgetData());
			}
		}
		for (int k = 0; k < this.SquadWidgetDatas.Count; k++)
		{
			if (this.SquadWidgetDatas[k].IsTurnedOn)
			{
				this.CurrentDefencePower += this.SquadWidgetDatas[k].SquadPower;
			}
		}
	}

	// Token: 0x0600430A RID: 17162 RVA: 0x0013E838 File Offset: 0x0013CA38
	private bool IsFollowUpFight(string fightId)
	{
		using (List<FightDef>.Enumerator enumerator = GameBalance.Me.fightDefinitions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Current.onWinNextFightId, fightId, StringComparison.Ordinal))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600430B RID: 17163 RVA: 0x0013E89C File Offset: 0x0013CA9C
	private void OnSquadPressedDefault(UIPrefightSquadWidget squadWidget)
	{
		int currentSquadCount = this.CurrentSquadCount;
		this.CurrentSquadCount = currentSquadCount + 1;
		squadWidget.Data.IsTurnedOn = true;
		squadWidget.Data.CanBeTurnedOn = false;
		squadWidget.Redraw();
		if (this.CurrentSquadCount >= this.FightDefinition.squads)
		{
			for (int i = 1; i < this.SquadWidgetDatas.Count; i++)
			{
				UIPrefightSquadWidgetData uiprefightSquadWidgetData = this.SquadWidgetDatas[i];
				if (uiprefightSquadWidgetData != squadWidget.Data && !uiprefightSquadWidgetData.IsTurnedOn)
				{
					uiprefightSquadWidgetData.CanBeTurnedOn = false;
					Action onRedraw = uiprefightSquadWidgetData.OnRedraw;
					if (onRedraw != null)
					{
						onRedraw();
					}
				}
			}
		}
		this.CurrentDefencePower = this.BuildingsQuality;
		for (int j = 0; j < this.SquadWidgetDatas.Count; j++)
		{
			if (this.SquadWidgetDatas[j].IsTurnedOn)
			{
				this.CurrentDefencePower += this.SquadWidgetDatas[j].SquadPower;
			}
		}
		Action onPressedSquad = this.OnPressedSquad;
		if (onPressedSquad == null)
		{
			return;
		}
		onPressedSquad();
	}

	// Token: 0x0600430C RID: 17164 RVA: 0x0013E99C File Offset: 0x0013CB9C
	private void OnSquadPressedTurnedOn(UIPrefightSquadWidget squadWidget)
	{
		bool flag = this.CurrentSquadCount >= this.FightDefinition.squads;
		int currentSquadCount = this.CurrentSquadCount;
		this.CurrentSquadCount = currentSquadCount - 1;
		squadWidget.Data.IsTurnedOn = false;
		squadWidget.Data.CanBeTurnedOn = true;
		squadWidget.Redraw();
		if (flag)
		{
			for (int i = 1; i < this.SquadWidgetDatas.Count; i++)
			{
				UIPrefightSquadWidgetData uiprefightSquadWidgetData = this.SquadWidgetDatas[i];
				if (uiprefightSquadWidgetData != squadWidget.Data && !uiprefightSquadWidgetData.IsTurnedOn)
				{
					uiprefightSquadWidgetData.CanBeTurnedOn = true;
					Action onRedraw = uiprefightSquadWidgetData.OnRedraw;
					if (onRedraw != null)
					{
						onRedraw();
					}
				}
			}
		}
		this.CurrentDefencePower = this.BuildingsQuality;
		for (int j = 0; j < this.SquadWidgetDatas.Count; j++)
		{
			if (this.SquadWidgetDatas[j].IsTurnedOn)
			{
				this.CurrentDefencePower += this.SquadWidgetDatas[j].SquadPower;
			}
		}
		Action onPressedSquad = this.OnPressedSquad;
		if (onPressedSquad == null)
		{
			return;
		}
		onPressedSquad();
	}
}
