using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008E2 RID: 2274
public class UIBoostElement : LazyWidget<UIBoostElementData>
{
	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x06003B59 RID: 15193 RVA: 0x0011C0FD File Offset: 0x0011A2FD
	public List<UICraftItemCell> DisplayedIngredients
	{
		get
		{
			return this.displayedIngredients;
		}
	}

	// Token: 0x06003B5A RID: 15194 RVA: 0x0011C108 File Offset: 0x0011A308
	public override void Init()
	{
		base.Init();
		base.TryGetComponent<LazyButton>(out this.widgetButton);
		this.widgetButton.onEnter.AddListener(new UnityAction(this.OnOver));
		this.widgetButton.onExit.AddListener(new UnityAction(this.OnOut));
		this.widgetButton.onClick.AddListener(new UnityAction(this.OnPress));
	}

	// Token: 0x06003B5B RID: 15195 RVA: 0x0011C17C File Offset: 0x0011A37C
	public override void DeInit()
	{
		base.DeInit();
		this.widgetButton.onEnter.RemoveAllListeners();
		this.widgetButton.onExit.RemoveAllListeners();
		this.widgetButton.onClick.RemoveAllListeners();
	}

	// Token: 0x06003B5C RID: 15196 RVA: 0x00002318 File Offset: 0x00000518
	protected void Update()
	{
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x0011C1B4 File Offset: 0x0011A3B4
	public override void Redraw()
	{
		base.Redraw();
		this.onPress = this.data.OnPress;
		this.onOver = this.data.OnOver;
		this.onOut = this.data.OnOut;
		this.resultIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.CraftElement.Definition.GetCraftResultIcon(null), null);
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
		if (this.data.CanCraft(this.data.GetCurrentNeedItems()))
		{
			this.canNotCraftOverlay.SetActive(false);
		}
		else
		{
			this.canNotCraftOverlay.SetActive(true);
		}
		this.Fold();
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x0011C34C File Offset: 0x0011A54C
	public void Fold()
	{
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.GamepadNavigationItem.Active = false;
		}
	}

	// Token: 0x06003B5F RID: 15199 RVA: 0x0011C3A4 File Offset: 0x0011A5A4
	public void Unfold()
	{
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.GamepadNavigationItem.Active = true;
		}
	}

	// Token: 0x06003B60 RID: 15200 RVA: 0x0011C3FC File Offset: 0x0011A5FC
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

	// Token: 0x06003B61 RID: 15201 RVA: 0x0011C488 File Offset: 0x0011A688
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

	// Token: 0x06003B62 RID: 15202 RVA: 0x0011C4AB File Offset: 0x0011A6AB
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

	// Token: 0x06003B63 RID: 15203 RVA: 0x0011C4D0 File Offset: 0x0011A6D0
	private void OnPress()
	{
		List<NeedItemData> currentNeedItems = this.data.GetCurrentNeedItems();
		if (!this.data.CanCraft(currentNeedItems))
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

	// Token: 0x06003B64 RID: 15204 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002EE2 RID: 12002
	private Action<List<NeedItemData>> onPress;

	// Token: 0x04002EE3 RID: 12003
	private Action onOver;

	// Token: 0x04002EE4 RID: 12004
	private Action onOut;

	// Token: 0x04002EE5 RID: 12005
	[SerializeField]
	private Image resultIcon;

	// Token: 0x04002EE6 RID: 12006
	[SerializeField]
	private Transform ingredientsContainer;

	// Token: 0x04002EE7 RID: 12007
	[SerializeField]
	private Image selectionFrame;

	// Token: 0x04002EE8 RID: 12008
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x04002EE9 RID: 12009
	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	// Token: 0x04002EEA RID: 12010
	[SerializeField]
	private GameObject canNotCraftOverlay;

	// Token: 0x04002EEB RID: 12011
	private LazyButton widgetButton;

	// Token: 0x04002EEC RID: 12012
	private List<UICraftItemCell> displayedIngredients = new List<UICraftItemCell>();
}
