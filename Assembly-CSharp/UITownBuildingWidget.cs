using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A6A RID: 2666
public class UITownBuildingWidget : LazyWidget<UITownBuildingWidgetData>
{
	// Token: 0x17000ADD RID: 2781
	// (get) Token: 0x0600484A RID: 18506 RVA: 0x00156B65 File Offset: 0x00154D65
	public List<UICraftItemCell> DisplayedIngredients
	{
		get
		{
			return this.displayedIngredients;
		}
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x00156B70 File Offset: 0x00154D70
	public override void Init()
	{
		base.Init();
		base.TryGetComponent<LazyButton>(out this.widgetButton);
		this.widgetButton.onEnter.AddListener(new UnityAction(this.OnOver));
		this.widgetButton.onExit.AddListener(new UnityAction(this.OnOut));
		this.widgetButton.onClick.AddListener(new UnityAction(this.OnPress));
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x00156BE4 File Offset: 0x00154DE4
	public override void DeInit()
	{
		base.DeInit();
		this.widgetButton.onEnter.RemoveAllListeners();
		this.widgetButton.onExit.RemoveAllListeners();
		this.widgetButton.onClick.RemoveAllListeners();
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x00156C1C File Offset: 0x00154E1C
	public override void Redraw()
	{
		base.Redraw();
		this.onPress = this.data.OnPress;
		this.onOver = this.data.OnOver;
		this.onOut = this.data.OnOut;
		this.resultIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.TownBuildingDef.iconId, "i_b_goc_tent");
		this.resultIcon.SetNativeSize();
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
		this.descriptionLabel.text = this.data.Description;
		this.descriptionLabel.gameObject.SetActive(!string.IsNullOrEmpty(this.descriptionLabel.text));
		this.canBuildStyle.ApplyStyle(this.nameLabel, false, null, null, null);
		this.Fold();
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x00156DB0 File Offset: 0x00154FB0
	public void Fold()
	{
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.GamepadNavigationItem.Active = false;
		}
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x00156E08 File Offset: 0x00155008
	public void Unfold()
	{
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.GamepadNavigationItem.Active = true;
		}
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x00156E60 File Offset: 0x00155060
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

	// Token: 0x06004851 RID: 18513 RVA: 0x00156EEC File Offset: 0x001550EC
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

	// Token: 0x06004852 RID: 18514 RVA: 0x00156F0F File Offset: 0x0015510F
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

	// Token: 0x06004853 RID: 18515 RVA: 0x00156F32 File Offset: 0x00155132
	private void OnPress()
	{
		Action<List<NeedItemData>> action = this.onPress;
		if (action == null)
		{
			return;
		}
		action(this.data.GetCurrentNeedItems());
	}

	// Token: 0x06004854 RID: 18516 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003869 RID: 14441
	private Action<List<NeedItemData>> onPress;

	// Token: 0x0400386A RID: 14442
	private Action onOver;

	// Token: 0x0400386B RID: 14443
	private Action onOut;

	// Token: 0x0400386C RID: 14444
	[SerializeField]
	private Image resultIcon;

	// Token: 0x0400386D RID: 14445
	[SerializeField]
	private Transform ingredientsContainer;

	// Token: 0x0400386E RID: 14446
	[SerializeField]
	private Image selectionFrame;

	// Token: 0x0400386F RID: 14447
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x04003870 RID: 14448
	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	// Token: 0x04003871 RID: 14449
	[SerializeField]
	private TextStyle canBuildStyle;

	// Token: 0x04003872 RID: 14450
	[SerializeField]
	private TextStyle canNotBuildStyle;

	// Token: 0x04003873 RID: 14451
	private LazyButton widgetButton;

	// Token: 0x04003874 RID: 14452
	private List<UICraftItemCell> displayedIngredients = new List<UICraftItemCell>();
}
