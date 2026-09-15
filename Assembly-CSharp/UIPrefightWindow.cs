using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009CD RID: 2509
public class UIPrefightWindow : LazyWindow<UIPrefightWindowData>
{
	// Token: 0x060042E5 RID: 17125 RVA: 0x0013DA04 File Offset: 0x0013BC04
	public override void Init()
	{
		base.Init();
		UIPrefightWindow.linkedPool = new Pool(this.linkedEntityWidgetPrefab, base.transform, 1, Pool.PoolType.ImmediateActivation, false, null);
		this.linkedEntityWidgetPrefab.gameObject.SetActive(false);
		this.startFightButton.onClick.AddListener(new UnityAction(this.OnStartButtonPressed));
		UIMouseTooltip.Attach(this.squadValueLabel.transform.parent.gameObject, "tt_prefight_1", null, true, true, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x060042E6 RID: 17126 RVA: 0x0013DA94 File Offset: 0x0013BC94
	public override void Redraw()
	{
		base.Redraw();
		this.UpdateCloseButtonState();
		this.data.OnPressedSquad = new Action(this.OnPressedSquad);
		foreach (LinkedEntityWidget linkedEntityWidget in this.shownLinked)
		{
			UIPrefightWindow.linkedPool.ReleaseObject<LinkedEntityWidget>(linkedEntityWidget);
		}
		this.shownLinked.Clear();
		this.headerLabel.text = LLBase.L(this.data.FightDefinition.id);
		this.startFightButton.interactable = this.data.HasEnoughDefencePower;
		this.RedrawSquadAndDefenceLabels();
		string text = this.buildingsPowerStyle.ApplyStyleToString(this.data.BuildingsQuality.ToString(), false, true);
		this.buildingsInZoneLabel.text = this.GetBuildingsInZoneText(text);
		UIItemCell[] array = this.rewardCells;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		if (this.data.FightDefinition.rewards.Count > 0)
		{
			for (int j = 0; j < this.data.FightDefinition.rewards.Count; j++)
			{
				this.rewardCells[j].gameObject.SetActive(true);
				this.rewardCells[j].Draw(new Item(this.data.FightDefinition.rewards[j].id, this.data.FightDefinition.rewards[j].GetCount(null)), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			}
			this.noRewardsObj.SetActive(false);
			this.rewardsObj.SetActive(true);
		}
		else
		{
			this.noRewardsObj.SetActive(true);
			this.rewardsObj.SetActive(false);
		}
		this.entitiesSizeFitter.enabled = true;
		this.entitiesScrollRect.enabled = false;
		if (this.data.BuildingInZone.Count > 0)
		{
			this.noBuildingsObj.SetActive(false);
			this.buildingsObj.SetActive(true);
			GameRes gameRes = new GameRes();
			for (int k = 0; k < this.data.BuildingInZone.Count; k++)
			{
				gameRes.Add(this.data.BuildingInZone[k].id, 1f);
			}
			for (int l = 0; l < gameRes.List.Count; l++)
			{
				LinkedEntityWidget orCreateObject = UIPrefightWindow.linkedPool.GetOrCreateObject<LinkedEntityWidget>();
				GameResAtom gameResAtom = gameRes.List[l];
				BuildingDef data = GameBalance.Me.GetData<BuildingDef>(gameResAtom.type + "_fb");
				bool flag = (this.data.FightDefinition.isBarricadesUnavailable && GameBalance.Me.HasWgoIdByGroup("barricades", data.wgoId)) || (this.data.FightDefinition.isTowersUnavailable && GameBalance.Me.HasWgoIdByGroup("towers", data.wgoId));
				LinkedEntityWidgetData linkedEntityWidgetData = new LinkedEntityWidgetData(GameBalance.Me.GetData<BuildingDef>(gameResAtom.type + "_fb"), null, (int)gameResAtom.value, flag);
				orCreateObject.Draw(linkedEntityWidgetData);
				orCreateObject.gameObject.SetActive(true);
				orCreateObject.transform.SetParent(this.entitiesContent);
				this.shownLinked.Add(orCreateObject);
			}
			if (this.shownLinked.Count > 9)
			{
				this.entitiesSizeFitter.enabled = false;
				this.entitiesBackground.sizeDelta = new Vector2(this.entitiesBackground.sizeDelta.x, this.maxEntitiesBackgroundWidth);
				this.entitiesScrollRect.enabled = true;
			}
		}
		else
		{
			this.noBuildingsObj.SetActive(true);
			this.buildingsObj.SetActive(false);
		}
		this.DisableLastSquadWidget();
		int num = ((this.squadWidgets == null) ? 0 : Math.Max(0, this.squadWidgets.Length - 1));
		int num2 = Math.Min(this.data.SquadWidgetDatas.Count, num);
		for (int m = 0; m < num2; m++)
		{
			if (this.squadWidgets != null)
			{
				this.squadWidgets[m].Draw(this.data.SquadWidgetDatas[m]);
			}
		}
		this.DisableLastSquadWidget();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060042E7 RID: 17127 RVA: 0x0013DF40 File Offset: 0x0013C140
	private void RedrawSquadAndDefenceLabels()
	{
		string text = this.slashStyleDefault.ApplyStyleToString("/", false, true);
		this.squadValueLabel.text = string.Format("{0}{1}{2}", this.data.CurrentSquadCount, text, this.data.FightDefinition.squads);
		bool showDefencePowerRequirement = this.data.ShowDefencePowerRequirement;
		UIMouseTooltip.Attach(this.defencePowerValueLabel.transform.parent.gameObject, showDefencePowerRequirement ? "tt_prefight_2" : "tt_prefight_2b", null, true, true, default(UIMouseTooltipEdges), default(Vector2), null);
		if (!showDefencePowerRequirement)
		{
			string text2 = this.enoughStyle.ApplyStyleToString(string.Format("{0}", this.data.CurrentDefencePower), false, true);
			this.defencePowerValueLabel.text = "barracks".FontIcon() + text2;
			return;
		}
		if (this.data.CurrentDefencePower >= this.data.FightDefinition.defencePowerLock)
		{
			string text3 = this.enoughStyle.ApplyStyleToString(string.Format("{0}", this.data.CurrentDefencePower), false, true);
			string text4 = this.enoughStyleSlash.ApplyStyleToString("/", false, true);
			this.defencePowerValueLabel.text = string.Format("{0}{1}{2}{3}", new object[]
			{
				"barracks".FontIcon(),
				text3,
				text4,
				this.data.FightDefinition.defencePowerLock
			});
			return;
		}
		string text5 = this.notEnoughStyle.ApplyStyleToString(string.Format("{0}", this.data.CurrentDefencePower), false, true);
		string text6 = this.notEnoughStyleSlash.ApplyStyleToString("/", false, true);
		this.defencePowerValueLabel.text = string.Format("{0}{1}{2}{3}", new object[]
		{
			"barracks".FontIcon(),
			text5,
			text6,
			this.data.FightDefinition.defencePowerLock
		});
	}

	// Token: 0x060042E8 RID: 17128 RVA: 0x0013E15F File Offset: 0x0013C35F
	private bool OnStartPressed()
	{
		if (this.startFightButton.interactable)
		{
			this.OnStartButtonPressed();
			return true;
		}
		return false;
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x0013E178 File Offset: 0x0013C378
	private void OnStartButtonPressed()
	{
		this.Close();
		MainGame.Instance.GameSave.militaryBaseData.fighterContainersSelectedForFight = new List<SGuid>();
		for (int i = 0; i < this.data.SquadWidgetDatas.Count; i++)
		{
			if (this.data.SquadWidgetDatas[i].IsTurnedOn)
			{
				MainGame.Instance.GameSave.militaryBaseData.fighterContainersSelectedForFight.Add(this.data.SquadWidgetDatas[i].WgoData.UniqueId);
			}
		}
		Action onStartButtonPressed = this.data.OnStartButtonPressed;
		if (onStartButtonPressed == null)
		{
			return;
		}
		onStartButtonPressed();
	}

	// Token: 0x060042EA RID: 17130 RVA: 0x0013E220 File Offset: 0x0013C420
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.StartFight, new Func<bool>(this.OnStartPressed));
		return gameKeyDelegates;
	}

	// Token: 0x060042EB RID: 17131 RVA: 0x0013E23F File Offset: 0x0013C43F
	protected override bool OnPressedBack()
	{
		return (this.data == null || this.data.AllowClose) && base.OnPressedBack();
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x0013E25E File Offset: 0x0013C45E
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		this.UpdateCloseButtonState();
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x0013E26C File Offset: 0x0013C46C
	protected override void PrintTips()
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>
		{
			new LazyGameKeyTip(GameKey.StartFight, "start_fight", this.startFightButton.interactable, true, true),
			LazyGameKeyTip.Select(true, true, true)
		};
		if (this.data == null || this.data.AllowClose)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x0013E2E4 File Offset: 0x0013C4E4
	private void UpdateCloseButtonState()
	{
		if (!this.closeButton)
		{
			return;
		}
		UIPrefightWindowData data = this.data;
		bool flag = data == null || data.AllowClose;
		this.closeButton.interactable = flag;
		this.closeButton.gameObject.SetActive(flag && !LazyInput.IsGamepadActive);
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x0013E33C File Offset: 0x0013C53C
	private void OnPressedSquad()
	{
		this.RedrawSquadAndDefenceLabels();
		this.startFightButton.interactable = this.data.HasEnoughDefencePower && (MainGame.PlayerController.Sword.id != "empty" || MainGame.PlayerController.Bow.id != "empty");
		if (LazyInput.IsGamepadActive)
		{
			this.PrintTips();
		}
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x0013E3AE File Offset: 0x0013C5AE
	private void DisableLastSquadWidget()
	{
		if (this.squadWidgets == null || this.squadWidgets.Length == 0)
		{
			return;
		}
		UIPrefightSquadWidget[] array = this.squadWidgets;
		array[array.Length - 1].gameObject.SetActive(false);
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x0013E3DC File Offset: 0x0013C5DC
	private string GetBuildingsInZoneText(string buildingPower)
	{
		if (this.data.FightDefinition.isBarricadesUnavailable && this.data.FightDefinition.isTowersUnavailable)
		{
			return LLBase.L("ui_fight_construction_unavailable") ?? "";
		}
		if (this.data.FightDefinition.isBarricadesUnavailable)
		{
			return string.Concat(new string[]
			{
				LLBase.L("ui_fight_barricades_unavailable"),
				" (",
				"barracks".FontIcon(),
				buildingPower,
				"):"
			});
		}
		if (this.data.FightDefinition.isTowersUnavailable)
		{
			return string.Concat(new string[]
			{
				LLBase.L("ui_fight_towers_unavailable"),
				" (",
				"barracks".FontIcon(),
				buildingPower,
				"):"
			});
		}
		return string.Concat(new string[]
		{
			LLBase.L("ui_buildings_quality_in_zone"),
			" (",
			"barracks".FontIcon(),
			buildingPower,
			"):"
		});
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400342D RID: 13357
	[SerializeField]
	private TextMeshProUGUI headerLabel;

	// Token: 0x0400342E RID: 13358
	[SerializeField]
	private TextMeshProUGUI squadValueLabel;

	// Token: 0x0400342F RID: 13359
	[SerializeField]
	private TextMeshProUGUI defencePowerValueLabel;

	// Token: 0x04003430 RID: 13360
	[SerializeField]
	private TextMeshProUGUI buildingsInZoneLabel;

	// Token: 0x04003431 RID: 13361
	[SerializeField]
	private GameObject noBuildingsObj;

	// Token: 0x04003432 RID: 13362
	[SerializeField]
	private GameObject buildingsObj;

	// Token: 0x04003433 RID: 13363
	[SerializeField]
	private GameObject noRewardsObj;

	// Token: 0x04003434 RID: 13364
	[SerializeField]
	private GameObject rewardsObj;

	// Token: 0x04003435 RID: 13365
	[SerializeField]
	private TextStyle slashStyleDefault;

	// Token: 0x04003436 RID: 13366
	[SerializeField]
	private TextStyle notEnoughStyle;

	// Token: 0x04003437 RID: 13367
	[SerializeField]
	private TextStyle notEnoughStyleSlash;

	// Token: 0x04003438 RID: 13368
	[SerializeField]
	private TextStyle enoughStyle;

	// Token: 0x04003439 RID: 13369
	[SerializeField]
	private TextStyle enoughStyleSlash;

	// Token: 0x0400343A RID: 13370
	[SerializeField]
	private TextStyle buildingsPowerStyle;

	// Token: 0x0400343B RID: 13371
	[SerializeField]
	private UIPrefightSquadWidget[] squadWidgets;

	// Token: 0x0400343C RID: 13372
	[SerializeField]
	private RectTransform entitiesContent;

	// Token: 0x0400343D RID: 13373
	[SerializeField]
	private RectTransform entitiesBackground;

	// Token: 0x0400343E RID: 13374
	[SerializeField]
	private ContentSizeFitter entitiesSizeFitter;

	// Token: 0x0400343F RID: 13375
	[SerializeField]
	private ScrollRect entitiesScrollRect;

	// Token: 0x04003440 RID: 13376
	[SerializeField]
	private float maxEntitiesBackgroundWidth;

	// Token: 0x04003441 RID: 13377
	[SerializeField]
	private LinkedEntityWidget linkedEntityWidgetPrefab;

	// Token: 0x04003442 RID: 13378
	[SerializeField]
	private UIItemCell[] rewardCells;

	// Token: 0x04003443 RID: 13379
	[SerializeField]
	private LazyButton startFightButton;

	// Token: 0x04003444 RID: 13380
	private List<LinkedEntityWidget> shownLinked = new List<LinkedEntityWidget>();

	// Token: 0x04003445 RID: 13381
	private static Pool linkedPool;
}
