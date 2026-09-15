using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A24 RID: 2596
public class UIResourceBasedCraftWindow : LazyWindow<UIResourceBasedCraftWindowData>
{
	// Token: 0x060045BB RID: 17851 RVA: 0x00149F10 File Offset: 0x00148110
	public override void Init()
	{
		base.Init();
		this.craftBtn.onClick.RemoveAllListeners();
		this.craftBtn.onClick.AddListener(new UnityAction(this.OnBtnPressed));
		this.craftBtn.SetCallbacksIntoGamepadNavigationItem();
	}

	// Token: 0x060045BC RID: 17852 RVA: 0x00149F50 File Offset: 0x00148150
	public override void Redraw()
	{
		base.Redraw();
		UIInfoWidgetData uiinfoWidgetData = new UIInfoWidgetData(this.data.WgoData, null, true);
		uiinfoWidgetData.ExcludePlayerFromMultiinventoryWhenCountItemsForFuel = false;
		this.infoWidget.Draw(uiinfoWidgetData);
		this.btnLabel.text = this.data.BtnText;
		this.descLabel.text = this.data.LabelText;
		if (this.data.SelectedItem == null || this.data.SelectedItem.IsEmpty)
		{
			this.mainIngredient.DrawEmptyInteractable(false, false);
			this.plusMainIngredientObj.SetActive(true);
		}
		else
		{
			this.mainIngredient.Draw(new Item(this.data.SelectedItem.id, this.data.MainIngredientCount), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			this.plusMainIngredientObj.SetActive(false);
		}
		this.mainIngredient.OnItemCellPress = new Action<UIItemCell>(this.OnMainIngredientPressed);
		this.craftBtn.interactable = this.data.CanStartCraft();
		if (this.data.Ingredients.Count > 0)
		{
			this.ingredientsParent.SetActive(true);
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (i < this.data.Ingredients.Count)
				{
					this.ingredients[i].gameObject.SetActive(true);
					this.ingredients[i].Draw(new Item(this.data.Ingredients[i].id, (i < this.data.CraftNeedItemsCount) ? this.data.Ingredients[i].GetCount(this.data.WgoData) : this.data.Ingredients[i].GetCount(null)), true, this.data.IngredientsHasCount[i], false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
				}
				else
				{
					this.ingredients[i].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			this.ingredientsParent.SetActive(false);
			foreach (UIItemCell uiitemCell in this.ingredients)
			{
				uiitemCell.gameObject.SetActive(false);
			}
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(false, null, null);
			if (this.data.SelectedItem == null || this.data.SelectedItem.IsEmpty)
			{
				base.GamepadNavigationController.SetFocusedItem(this.mainIngredient.GamepadNavigationItem);
			}
			else
			{
				base.GamepadNavigationController.SetFocusedItem(this.craftBtn.GetComponent<GamepadNavigationItem>());
			}
			this.startTip.text = new LazyGameKeyTip(GameKey.AlchemyStart, this.data.BtnText, this.craftBtn.interactable, true, false).ToString();
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060045BD RID: 17853 RVA: 0x0014A270 File Offset: 0x00148470
	private void OnBtnPressed()
	{
		Action onBtnPressedAction = this.data.OnBtnPressedAction;
		if (onBtnPressedAction == null)
		{
			return;
		}
		onBtnPressedAction();
	}

	// Token: 0x060045BE RID: 17854 RVA: 0x0014A287 File Offset: 0x00148487
	private bool OnStartSurveyPressed()
	{
		if (this.craftBtn.interactable)
		{
			this.OnBtnPressed();
			return true;
		}
		return false;
	}

	// Token: 0x060045BF RID: 17855 RVA: 0x0014A29F File Offset: 0x0014849F
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.SurveyStart, new Func<bool>(this.OnStartSurveyPressed));
		return gameKeyDelegates;
	}

	// Token: 0x060045C0 RID: 17856 RVA: 0x0014A2BE File Offset: 0x001484BE
	private void OnMainIngredientPressed(UIItemCell itemCell)
	{
		Action<UIItemCell> onMainIngredientPressedAction = this.data.OnMainIngredientPressedAction;
		if (onMainIngredientPressedAction == null)
		{
			return;
		}
		onMainIngredientPressedAction(itemCell);
	}

	// Token: 0x060045C1 RID: 17857 RVA: 0x0014A2D8 File Offset: 0x001484D8
	[LazyUITest]
	protected override void TestDraw()
	{
		Wgo wgo = Wgo.Spawn(new WgoData("survey_wgo", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId), MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		SurveyInteractionHandler surveyInteractionHandler = new SurveyInteractionHandler();
		surveyInteractionHandler.Init(wgo);
		surveyInteractionHandler.HasInteraction(MainGame.PlayerController);
		surveyInteractionHandler.Interact(MainGame.PlayerController);
		MainGame.PlayerData.Inventory.AddItemToInventory(new Item("clean_paper", 15), null, false);
		MainGame.PlayerData.Inventory.AddItemToInventory(new Item("faith", 15), null, false);
		MainGame.PlayerData.Inventory.AddItemToInventory(new Item("wheat_seed", 1), null, false);
		MainGame.PlayerData.Inventory.AddItemToInventory(new Item("cabbage:1", 1), null, false);
		MainGame.PlayerData.Inventory.AddItemToInventory(new Item("mushroom_brown", 1), null, false);
		MainGame.Instance.GameSave.talentSystemData.GetTalentBranch("talent_yellow").curTalentValue += 5;
	}

	// Token: 0x04003691 RID: 13969
	[SerializeField]
	private UIInfoWidget infoWidget;

	// Token: 0x04003692 RID: 13970
	[SerializeField]
	private TextMeshProUGUI btnLabel;

	// Token: 0x04003693 RID: 13971
	[SerializeField]
	private TextMeshProUGUI descLabel;

	// Token: 0x04003694 RID: 13972
	[SerializeField]
	private UIItemCell mainIngredient;

	// Token: 0x04003695 RID: 13973
	[SerializeField]
	private List<UIItemCell> ingredients;

	// Token: 0x04003696 RID: 13974
	[SerializeField]
	private GameObject plusMainIngredientObj;

	// Token: 0x04003697 RID: 13975
	[SerializeField]
	private GameObject ingredientsParent;

	// Token: 0x04003698 RID: 13976
	[SerializeField]
	private LazyButton craftBtn;

	// Token: 0x04003699 RID: 13977
	[SerializeField]
	private TextMeshProUGUI startTip;
}
