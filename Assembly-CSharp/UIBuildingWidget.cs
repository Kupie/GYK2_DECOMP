using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000901 RID: 2305
public class UIBuildingWidget : LazyWidget<UIBuildingWidgetData>
{
	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x06003C52 RID: 15442 RVA: 0x00120566 File Offset: 0x0011E766
	public List<UICraftItemCell> DisplayedIngredients
	{
		get
		{
			return this.displayedIngredients;
		}
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x00120570 File Offset: 0x0011E770
	public override void Init()
	{
		base.Init();
		base.TryGetComponent<LazyButton>(out this.widgetButton);
		this.widgetButton.onEnter.AddListener(new UnityAction(this.OnOver));
		this.widgetButton.onExit.AddListener(new UnityAction(this.OnOut));
		this.widgetButton.onClick.AddListener(new UnityAction(this.OnPress));
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x001205E4 File Offset: 0x0011E7E4
	public override void DeInit()
	{
		base.DeInit();
		this.widgetButton.onEnter.RemoveAllListeners();
		this.widgetButton.onExit.RemoveAllListeners();
		this.widgetButton.onClick.RemoveAllListeners();
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x0012061C File Offset: 0x0011E81C
	public override void Redraw()
	{
		base.Redraw();
		this.onPress = this.data.OnPress;
		this.onOver = this.data.OnOver;
		this.onOut = this.data.OnOut;
		this.resultIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.BuildData.IconId, null);
		BuildingDef definition = this.data.BuildData.Definition;
		bool flag = definition != null && definition.HasLimits;
		foreach (UICraftItemCellData uicraftItemCellData in this.data.CraftItemCellsData)
		{
			UICraftItemCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UICraftItemCell>(this.ingredientsContainer.transform);
			elementFromPool.Draw(uicraftItemCellData, null, false);
			elementFromPool.GamepadNavigationItem.group = 1;
			elementFromPool.ItemCell.OnItemCellPress = delegate(UIItemCell _)
			{
				this.OnPress();
			};
			this.displayedIngredients.Add(elementFromPool);
		}
		this.nameLabel.text = LLBase.L(this.data.Name);
		if (flag)
		{
			TextMeshProUGUI textMeshProUGUI = this.nameLabel;
			textMeshProUGUI.text = textMeshProUGUI.text + " (" + this.data.BuildData.Definition.GetLimitsString() + ")";
		}
		TextStyle textStyle = (this.data.CanBuild(this.data.GetCurrentNeedItems()) ? this.canBuildStyle : this.canNotBuildStyle);
		textStyle.ApplyStyle(this.nameLabel, false, null, null, null);
		if (!string.IsNullOrEmpty(this.data.DescriptionModules))
		{
			this.descriptionLabel.text = this.data.DescriptionModules;
			this.descriptionModulesStyle.ApplyStyle(this.descriptionLabel, false, null, null, null);
		}
		else if (!string.IsNullOrEmpty(this.data.Description))
		{
			this.descriptionLabel.text = this.data.Description;
			this.descriptionDefaultStyle.ApplyStyle(this.descriptionLabel, false, null, null, null);
		}
		else if (this.IsWorkbenchExtension())
		{
			this.descriptionLabel.text = LLBase.L("ui_workbench_extention");
			textStyle.ApplyStyle(this.descriptionLabel, false, null, null, null);
		}
		else
		{
			this.descriptionLabel.text = string.Empty;
		}
		this.descriptionLabel.gameObject.SetActive(!string.IsNullOrEmpty(this.descriptionLabel.text));
		if (LL.IsCurrentLangAsian)
		{
			this.verticalLayoutGroup.spacing = this.asianSpaceBetweenNameAndDescription;
		}
		else
		{
			this.verticalLayoutGroup.spacing = this.defaultSpaceBetweenNameAndDescription;
		}
		this.Fold();
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x0012094C File Offset: 0x0011EB4C
	public void Fold()
	{
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.GamepadNavigationItem.Active = false;
		}
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x001209A4 File Offset: 0x0011EBA4
	public void Unfold()
	{
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.GamepadNavigationItem.Active = true;
		}
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x001209FC File Offset: 0x0011EBFC
	public override void Hide()
	{
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.Flush();
			uicraftItemCell.GamepadNavigationItem.group = 1;
			UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftItemCell>(uicraftItemCell);
		}
		this.displayedIngredients.Clear();
		this.selectionFrame.gameObject.SetActive(false);
		base.Hide();
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x00120A88 File Offset: 0x0011EC88
	private bool IsWorkbenchExtension()
	{
		UIBuildingWidgetData data = this.data;
		if (((data != null) ? data.BuildData : null) == null || GameBalance.Me == null)
		{
			return false;
		}
		string wgoId = this.data.BuildData.WgoId;
		if (GameBalance.Me.IsWorkbenchExtensionId(wgoId))
		{
			return true;
		}
		WGODef workbenchExtensionLogicDef = GameBalance.Me.GetWorkbenchExtensionLogicDef(wgoId);
		return workbenchExtensionLogicDef != null && GameBalance.Me.IsWorkbenchExtensionId(workbenchExtensionLogicDef.id);
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00120AF9 File Offset: 0x0011ECF9
	private void OnOver()
	{
		this.selectionFrame.gameObject.SetActive(true);
		Action action = this.onOver;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00120B1C File Offset: 0x0011ED1C
	private void OnOut()
	{
		this.selectionFrame.gameObject.SetActive(false);
		Action action = this.onOut;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x00120B40 File Offset: 0x0011ED40
	private void OnPress()
	{
		List<NeedItemData> currentNeedItems = this.data.GetCurrentNeedItems();
		if (!this.data.CanBuild(currentNeedItems))
		{
			return;
		}
		Action<List<NeedItemData>> action = this.onPress;
		if (action == null)
		{
			return;
		}
		action(this.data.GetCurrentNeedItems());
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002F79 RID: 12153
	private Action<List<NeedItemData>> onPress;

	// Token: 0x04002F7A RID: 12154
	private Action onOver;

	// Token: 0x04002F7B RID: 12155
	private Action onOut;

	// Token: 0x04002F7C RID: 12156
	[SerializeField]
	private Image resultIcon;

	// Token: 0x04002F7D RID: 12157
	[SerializeField]
	private Transform ingredientsContainer;

	// Token: 0x04002F7E RID: 12158
	[SerializeField]
	private Image selectionFrame;

	// Token: 0x04002F7F RID: 12159
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x04002F80 RID: 12160
	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	// Token: 0x04002F81 RID: 12161
	[SerializeField]
	private TextStyle canBuildStyle;

	// Token: 0x04002F82 RID: 12162
	[SerializeField]
	private TextStyle canNotBuildStyle;

	// Token: 0x04002F83 RID: 12163
	[SerializeField]
	private TextStyle descriptionDefaultStyle;

	// Token: 0x04002F84 RID: 12164
	[SerializeField]
	private TextStyle descriptionModulesStyle;

	// Token: 0x04002F85 RID: 12165
	[SerializeField]
	private VerticalLayoutGroup verticalLayoutGroup;

	// Token: 0x04002F86 RID: 12166
	[SerializeField]
	private float defaultSpaceBetweenNameAndDescription = -4f;

	// Token: 0x04002F87 RID: 12167
	[SerializeField]
	private float asianSpaceBetweenNameAndDescription = -2f;

	// Token: 0x04002F88 RID: 12168
	private LazyButton widgetButton;

	// Token: 0x04002F89 RID: 12169
	private List<UICraftItemCell> displayedIngredients = new List<UICraftItemCell>();
}
