using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A27 RID: 2599
public class UIResurrectionWindow : LazyWindow<UIResurrectionWindowData>
{
	// Token: 0x060045E2 RID: 17890 RVA: 0x0014AB30 File Offset: 0x00148D30
	public override void Init()
	{
		base.Init();
		this.prepareResurrectionButton.onClick.AddListener(delegate
		{
			this.StartResurrectionButtonPressed();
		});
		this.diceBtn.onClick.AddListener(delegate
		{
			this.OnDiceButtonPressed();
		});
		this.prepareResurrectionButton.onNotInteractableEnter.AddListener(new UnityAction(this.OnPrepareResurrectionNotInteractableOver));
		this.prepareResurrectionButton.onNotInteractableExit.AddListener(new UnityAction(this.OnOut));
		UIMouseTooltip.Attach(this.collarSlot.gameObject, "tt_resur_collar", null, false, false, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x060045E3 RID: 17891 RVA: 0x0014ABE0 File Offset: 0x00148DE0
	public override void Redraw()
	{
		base.Redraw();
		this.data.onCollarUpdated = new Action(this.RedrawLite);
		this.infoWidget.Draw(this.data.InfoWidgetData);
		this.prepareResurrectionLabel.text = LLBase.L("ui_prepare");
		this.onPrepareResurrectionButtonPressed = this.data.onPrepareResurrectionButtonPressed;
		this.redSkullsValue.text = string.Format("{0}{1}", "rskull".FontIcon(), this.data.RedSkulls);
		this.whiteSkullsValue.text = string.Format("{0}{1}", "skull".FontIcon(), this.data.WhiteSkulls);
		this.RedrawLite();
		this.corpseWidget.Draw(this.data.CorpseWidgetData);
		this.bodyOrgansInventoryWidget.Draw(this.data.BodyOrgansInventoryWidgetData);
		this.needItemsWidget.Draw(this.data.NeedItemsWidgetData);
		if (this.data.IsEmpty)
		{
			this.diceBtn.interactable = false;
			this.skullsInactiveStyle.ApplyStyle(this.whiteSkullsValue, false, null, null, null);
			this.skullsInactiveStyle.ApplyStyle(this.redSkullsValue, false, null, null, null);
			this.nameInactiveStyle.ApplyStyle(this.zombieNameLabel, false, null, null, null);
			this.emptyOverlay.SetActive(true);
			this.zombieNameLabel.text = LLBase.L("ui_no_body");
			this.workerIcon.gameObject.SetActive(false);
			this.noBodyPortrait.SetActive(true);
			this.collarCell.LazyButton.interactable = false;
		}
		else
		{
			this.diceBtn.interactable = true;
			this.skullsActiveStyle.ApplyStyle(this.whiteSkullsValue, false, null, null, null);
			this.skullsActiveStyle.ApplyStyle(this.redSkullsValue, false, null, null, null);
			this.nameActiveStyle.ApplyStyle(this.zombieNameLabel, false, null, null, null);
			this.emptyOverlay.SetActive(false);
			this.zombieNameLabel.text = LLBase.L(this.data.ZombieName);
			this.RedrawWorkerIcon();
			this.workerIcon.gameObject.SetActive(true);
			this.noBodyPortrait.SetActive(false);
			this.collarCell.LazyButton.interactable = true;
		}
		this.collarCell.OnItemCellPress = new Action<UIItemCell>(this.data.OnCollarPressed);
		if (LazyInput.IsGamepadActive)
		{
			if (this.data.IsEmpty)
			{
				base.GamepadNavigationController.Disable();
			}
			else
			{
				base.GamepadNavigationController.Enable();
				base.GamepadNavigationController.ReinitItems(true, null, null);
			}
			this.UpdateResurrectionTip();
		}
	}

	// Token: 0x060045E4 RID: 17892 RVA: 0x0014AF30 File Offset: 0x00149130
	private void RedrawLite()
	{
		this.RedrawCollar();
		this.prepareResurrectionButton.interactable = this.data.CanStartResurrection();
		this.collarCell.Draw(this.data.Collar, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.collarCell.OnItemCellPress = new Action<UIItemCell>(this.data.OnCollarPressed);
		if (LazyInput.IsGamepadActive)
		{
			this.resurrectionGameKeyTip.text = new LazyGameKeyTip(GameKey.StartResurrection, this.prepareResurrectionLabel.text, this.prepareResurrectionButton.interactable, false, true).ToString();
		}
	}

	// Token: 0x060045E5 RID: 17893 RVA: 0x0014AFD4 File Offset: 0x001491D4
	private void RedrawWorkerIcon()
	{
		this.workerIcon.ShowWithoutTalent(null, ZombieSkinHelper.GetPresetForCustomizationData("zombie_worker", this.data.RolledSkin.Item1, this.data.RolledSkin.Item2, string.Empty, this.data.RolledSkin.Item3));
	}

	// Token: 0x060045E6 RID: 17894 RVA: 0x0014B02C File Offset: 0x0014922C
	private void RedrawCollar()
	{
		if (this.data.Collar.IsEmpty)
		{
			this.collarSlot.gameObject.SetActive(true);
			this.collarCell.LazyButton.interactable = false;
			return;
		}
		this.collarSlot.gameObject.SetActive(false);
		this.collarCell.LazyButton.interactable = true;
	}

	// Token: 0x060045E7 RID: 17895 RVA: 0x0014B090 File Offset: 0x00149290
	public override void Hide()
	{
		base.Hide();
		LazyButton lazyButton = this.prepareResurrectionButton;
		if (lazyButton != null)
		{
			LazyUIEvent onExit = lazyButton.onExit;
			if (onExit != null)
			{
				onExit.Invoke();
			}
		}
		if (this.data != null)
		{
			this.corpseWidget.Hide();
			this.bodyOrgansInventoryWidget.Hide();
			this.needItemsWidget.Hide();
		}
	}

	// Token: 0x060045E8 RID: 17896 RVA: 0x0014B0E8 File Offset: 0x001492E8
	protected override void HideWindow()
	{
		if (this.data != null && !string.IsNullOrEmpty(this.data.ZombieName))
		{
			MainGame.Instance.GameSave.knowledgeSystem.ReturnZombieName(this.data.ZombieName, this.data.ZombieNameRandomed);
			this.data.ZombieName = string.Empty;
		}
		base.HideWindow();
	}

	// Token: 0x060045E9 RID: 17897 RVA: 0x0014B14F File Offset: 0x0014934F
	private bool StartResurrectionButtonPressed()
	{
		if (!this.prepareResurrectionButton.interactable)
		{
			return false;
		}
		Action action = this.onPrepareResurrectionButtonPressed;
		if (action != null)
		{
			action();
		}
		return true;
	}

	// Token: 0x060045EA RID: 17898 RVA: 0x0014B172 File Offset: 0x00149372
	private bool OnTakeBodyButtonPressed()
	{
		this.corpseWidget.OnButtonPressed();
		return true;
	}

	// Token: 0x060045EB RID: 17899 RVA: 0x0014B180 File Offset: 0x00149380
	private bool OnDiceButtonPressed()
	{
		KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
		string zombieName = this.data.ZombieName;
		bool zombieNameRandomed = this.data.ZombieNameRandomed;
		string text = (string.IsNullOrEmpty(zombieName) ? string.Empty : LLBase.L(zombieName));
		string text2 = this.zombieNameLabel.text;
		int count = knowledgeSystem.freeZombieNames.Count;
		bool flag = !string.IsNullOrEmpty(zombieName) && knowledgeSystem.freeZombieNames.Contains(zombieName);
		bool flag2;
		this.data.ZombieName = knowledgeSystem.GetZombieName(out flag2);
		this.data.ZombieNameRandomed = flag2;
		string text3 = (string.IsNullOrEmpty(this.data.ZombieName) ? string.Empty : LLBase.L(this.data.ZombieName));
		int count2 = knowledgeSystem.freeZombieNames.Count;
		knowledgeSystem.ReturnZombieName(zombieName, zombieNameRandomed);
		this.zombieNameLabel.text = text3;
		this.LogZombieNameRollResult(zombieName, this.data.ZombieName, text, text3, text2, count, count2, knowledgeSystem.freeZombieNames.Count, flag);
		this.RedrawWorkerIcon();
		return true;
	}

	// Token: 0x060045EC RID: 17900 RVA: 0x0014B29C File Offset: 0x0014949C
	private void LogZombieNameRollResult(string previousName, string rolledName, string previousLocalizedName, string rolledLocalizedName, string previousLabelText, int freeNamesCountBeforeRoll, int freeNamesCountAfterGet, int freeNamesCountAfterReturn, bool freeNamesContainedPreviousName)
	{
		string zombieNameNotChangedReason = this.GetZombieNameNotChangedReason(previousName, rolledName, previousLocalizedName, rolledLocalizedName, freeNamesCountBeforeRoll, freeNamesContainedPreviousName);
		Debug.LogWarning(string.Concat(new string[]
		{
			"[UIResurrectionWindow][ZombieNameRoll] Name did not change after roll. reason:[",
			zombieNameNotChangedReason,
			"] previousRaw:[",
			previousName,
			"] rolledRaw:[",
			rolledName,
			"] previousLocalized:[",
			previousLocalizedName,
			"] rolledLocalized:[",
			rolledLocalizedName,
			"] ",
			string.Format("previousLabel:[{0}] freeNamesBeforeRoll:[{1}] ", previousLabelText, freeNamesCountBeforeRoll),
			string.Format("freeNamesAfterGet:[{0}] freeNamesAfterReturn:[{1}] ", freeNamesCountAfterGet, freeNamesCountAfterReturn),
			string.Format("freeNamesContainedPreviousNameBeforeRoll:[{0}]", freeNamesContainedPreviousName)
		}));
	}

	// Token: 0x060045ED RID: 17901 RVA: 0x0014B359 File Offset: 0x00149559
	private string GetZombieNameNotChangedReason(string previousName, string rolledName, string previousLocalizedName, string rolledLocalizedName, int freeNamesCountBeforeRoll, bool freeNamesContainedPreviousName)
	{
		if (freeNamesCountBeforeRoll == 0)
		{
			return "freeZombieNames was empty, KnowledgeSystem.GetZombieName rolled a random name from the existing 40";
		}
		if (previousName == rolledName)
		{
			if (!freeNamesContainedPreviousName)
			{
				return "rolled raw name equals previous raw name";
			}
			return "freeZombieNames contained the current raw name before roll, random selected it again";
		}
		else
		{
			if (previousLocalizedName == rolledLocalizedName)
			{
				return "raw name changed, but localized display text is the same";
			}
			return "label text stayed the same for an unknown reason";
		}
	}

	// Token: 0x060045EE RID: 17902 RVA: 0x0014B393 File Offset: 0x00149593
	private void OnPrepareResurrectionNotInteractableOver()
	{
		if (this.data.IsEmpty || this.prepareResurrectionButton.interactable)
		{
			return;
		}
		Action<LazyButton> onPrepareResurrectionNonInteractableButtonOver = this.data.onPrepareResurrectionNonInteractableButtonOver;
		if (onPrepareResurrectionNonInteractableButtonOver == null)
		{
			return;
		}
		onPrepareResurrectionNonInteractableButtonOver(this.prepareResurrectionButton);
	}

	// Token: 0x060045EF RID: 17903 RVA: 0x001080F0 File Offset: 0x001062F0
	private void OnOut()
	{
		UITooltip.Hide();
	}

	// Token: 0x060045F0 RID: 17904 RVA: 0x0014B3CC File Offset: 0x001495CC
	protected override void PrintTips()
	{
		this.lazyButtonTips.Print(new LazyGameKeyTip[]
		{
			LazyGameKeyTip.Select(true, true, true),
			new LazyGameKeyTip(GameKey.ZombieRollName, "tip_roll_zombie_name", true, true, true),
			LazyGameKeyTip.Back(true, true, true)
		});
		this.UpdateResurrectionTip();
	}

	// Token: 0x060045F1 RID: 17905 RVA: 0x0014B41C File Offset: 0x0014961C
	private void UpdateResurrectionTip()
	{
		if (this.data != null && this.data.CanStartResurrection != null)
		{
			this.resurrectionGameKeyTip.text = new LazyGameKeyTip(GameKey.StartResurrection, this.prepareResurrectionLabel.text, this.data.CanStartResurrection(), false, true).ToString();
		}
	}

	// Token: 0x060045F2 RID: 17906 RVA: 0x0014B478 File Offset: 0x00149678
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.ExtractBody, new Func<bool>(this.OnTakeBodyButtonPressed));
		gameKeyDelegates.Add(GameKey.ZombieRollName, new Func<bool>(this.OnDiceButtonPressed));
		gameKeyDelegates.Add(GameKey.StartResurrection, new Func<bool>(this.StartResurrectionButtonPressed));
		return gameKeyDelegates;
	}

	// Token: 0x060045F3 RID: 17907 RVA: 0x0014B4D0 File Offset: 0x001496D0
	[LazyUITest]
	protected override void TestDraw()
	{
		Wgo wgo = Wgo.Spawn(new WgoData("resurrection_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId), MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		CustomInteractionHandler customInteractionHandler = new CustomInteractionHandler();
		customInteractionHandler.Init(wgo);
		customInteractionHandler.HasInteraction(MainGame.PlayerController);
		customInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x060045F4 RID: 17908 RVA: 0x0014B538 File Offset: 0x00149738
	[LazyUITest]
	protected void TestDrawBody()
	{
		WgoData wgoData = new WgoData("resurrection_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		CustomInteractionHandler customInteractionHandler = new CustomInteractionHandler();
		customInteractionHandler.Init(wgo);
		Item item = GameBalance.Me.GetData<BodyDef>("body_0_2").GenerateItem();
		wgoData.Inventory.AddItemToInventory(item, null, false);
		customInteractionHandler.HasInteraction(MainGame.PlayerController);
		customInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x040036AB RID: 13995
	[SerializeField]
	private UIInfoWidget infoWidget;

	// Token: 0x040036AC RID: 13996
	[SerializeField]
	private UIWorkerIcon workerIcon;

	// Token: 0x040036AD RID: 13997
	[SerializeField]
	private GameObject noBodyPortrait;

	// Token: 0x040036AE RID: 13998
	[SerializeField]
	private UICorpseWidget corpseWidget;

	// Token: 0x040036AF RID: 13999
	[SerializeField]
	private BodyOrgansInventoryWidget bodyOrgansInventoryWidget;

	// Token: 0x040036B0 RID: 14000
	[SerializeField]
	private NeedItemsWidget needItemsWidget;

	// Token: 0x040036B1 RID: 14001
	[SerializeField]
	private UIItemCell collarCell;

	// Token: 0x040036B2 RID: 14002
	[SerializeField]
	private Image collarSlot;

	// Token: 0x040036B3 RID: 14003
	[SerializeField]
	private LazyButton prepareResurrectionButton;

	// Token: 0x040036B4 RID: 14004
	[SerializeField]
	private LazyButton diceBtn;

	// Token: 0x040036B5 RID: 14005
	[SerializeField]
	private TextMeshProUGUI zombieNameLabel;

	// Token: 0x040036B6 RID: 14006
	[SerializeField]
	private TextMeshProUGUI prepareResurrectionLabel;

	// Token: 0x040036B7 RID: 14007
	[SerializeField]
	private TextMeshProUGUI resurrectionGameKeyTip;

	// Token: 0x040036B8 RID: 14008
	[SerializeField]
	private TextMeshProUGUI redSkullsValue;

	// Token: 0x040036B9 RID: 14009
	[SerializeField]
	private TextMeshProUGUI whiteSkullsValue;

	// Token: 0x040036BA RID: 14010
	[SerializeField]
	private TextStyle skullsActiveStyle;

	// Token: 0x040036BB RID: 14011
	[SerializeField]
	private TextStyle skullsInactiveStyle;

	// Token: 0x040036BC RID: 14012
	[SerializeField]
	private TextStyle nameActiveStyle;

	// Token: 0x040036BD RID: 14013
	[SerializeField]
	private TextStyle nameInactiveStyle;

	// Token: 0x040036BE RID: 14014
	[SerializeField]
	private GameObject emptyOverlay;

	// Token: 0x040036BF RID: 14015
	private Action onPrepareResurrectionButtonPressed;
}
