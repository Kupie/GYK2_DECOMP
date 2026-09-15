using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200098C RID: 2444
public class UICraftPreviewItemCell : LazyWidget<UICraftPreviewItemCellData>
{
	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x060040D7 RID: 16599 RVA: 0x0013602C File Offset: 0x0013422C
	public GamepadNavigationItem ItemCellGamepadNavigationItem
	{
		get
		{
			return this.gamepadNavigationItem;
		}
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x00136034 File Offset: 0x00134234
	private void Awake()
	{
		this.tabButton.onEnter.AddListener(new UnityAction(this.OnOver));
		this.tabButton.onExit.AddListener(new UnityAction(this.OnOut));
		this.gamepadNavigationItemExtension.SetCallbacks(new UnityAction(this.tabButton.ForceOnEnter), new UnityAction(this.tabButton.ForceOnExit), null);
	}

	// Token: 0x060040D9 RID: 16601 RVA: 0x001360A8 File Offset: 0x001342A8
	public override void Redraw()
	{
		base.Redraw();
		this.tabButton.gameObject.SetActive(false);
		if (this.data.IsTab)
		{
			this.customImage.sprite = (this.data.UseTabAsIcon ? LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.TabId, "i_b_null") : LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("i_b_" + this.data.TabId, "i_b_null"));
			this.customImageBack.sprite = this.data.CustomImageBackSprite;
			this.customImageBack.SetNativeSize();
			this.customImageBack.gameObject.SetActive(true);
			this.uiItemCell.gameObject.SetActive(false);
			this.uknownCraftBlocker.gameObject.SetActive(false);
			this.canNotStartCraftBlocker.gameObject.SetActive(false);
			this.customImageNonInteractableBlocker.gameObject.SetActive(this.data.IsExtension && !this.data.IsExtensionAvailable);
			if (this.data.IsExtension)
			{
				this.tabButton.gameObject.SetActive(true);
				base.name = string.Concat(new string[]
				{
					"Extension ",
					this.data.ExtensionId,
					" tabId:[",
					this.data.TabId,
					"]"
				});
			}
			return;
		}
		this.uiItemCell.gameObject.SetActive(true);
		this.customImageBack.gameObject.SetActive(false);
		if (!this.data.IsUnknown)
		{
			this.DrawCraftOutput(this.data.CraftDef.GetOutputPreview(this.data.WgoData));
			UIItemCell uiitemCell = this.uiItemCell;
			uiitemCell.OnItemCellPress = (Action<UIItemCell>)Delegate.Combine(uiitemCell.OnItemCellPress, new Action<UIItemCell>(delegate(UIItemCell cell)
			{
				this.OpenCraftSetupWindow();
			}));
			this.uiItemCell.CustomTooltipShowAction = delegate(UIItemCell cell)
			{
				if (!this.data.IsUnknown)
				{
					UITooltip.ShowCraftInfo(cell, this.data.WgoData, this.data.CraftDef, null, null, null);
				}
			};
			return;
		}
		this.uiItemCell.CustomTooltipShowAction = null;
		this.uiItemCell.DrawEmpty(true, true, false);
		this.UpdateLocks();
	}

	// Token: 0x060040DA RID: 16602 RVA: 0x001362DA File Offset: 0x001344DA
	public override void Hide()
	{
		base.Hide();
		this.uiItemCell.ClearCallbacks();
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x001362ED File Offset: 0x001344ED
	private void DrawCraftOutput(OutputPreview outputPreview)
	{
		this.uiItemCell.DrawCraftOutput(outputPreview, -1, CraftStatus.OK, ItemType.None, 1, ItemRelatedWidgetState.NotSet, false);
		this.UpdateLocks();
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x00136308 File Offset: 0x00134508
	public void UpdateLocks()
	{
		if (this.data.IsTab)
		{
			return;
		}
		this.uknownCraftBlocker.SetActive(this.data.IsUnknown);
		this.canNotStartCraftBlocker.SetActive(!this.data.CanStart && !this.data.IsUnknown);
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x00136362 File Offset: 0x00134562
	private void OnOver()
	{
		if (!this.data.IsExtension)
		{
			return;
		}
		this.selectionFrameTab.SetActive(true);
		LazyAudio.PlayAndForget("gui_hover_light");
		UITooltip.ShowExtensionInfo(this, this.data.ExtensionId);
	}

	// Token: 0x060040DE RID: 16606 RVA: 0x00136399 File Offset: 0x00134599
	private void OnOut()
	{
		this.selectionFrameTab.SetActive(false);
		if (!this.data.IsExtension)
		{
			return;
		}
		UITooltip.Hide();
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x001363BA File Offset: 0x001345BA
	private void OnDisable()
	{
		this.selectionFrameTab.SetActive(false);
		if (!this.data.IsExtension)
		{
			return;
		}
		if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
		{
			UITooltip.HideImmediately();
		}
	}

	// Token: 0x060040E0 RID: 16608 RVA: 0x001363F0 File Offset: 0x001345F0
	private void OpenCraftSetupWindow()
	{
		if (this.data.CraftDef.isFuelCraft)
		{
			UISingleCraftWindowData uisingleCraftWindowData = new UISingleCraftWindowData(this.data.WgoData, this.data.CraftDef, this.data.OnQueueAdded, this.data.OnCraftStarted);
			LazyUI.GetWindow<UIFuelCraftWindow>().Open(uisingleCraftWindowData);
			return;
		}
		UICraftSelectionWindowData uicraftSelectionWindowData = new UICraftSelectionWindowData(this.data.WgoData, this.data.CraftDef, this.data.OnQueueAdded, this.data.OnCraftStarted);
		uicraftSelectionWindowData.IsGravePartRemove = this.data.IsGravePartRemove;
		LazyUI.GetWindow<UICraftSelectionWindow>().Open(uicraftSelectionWindowData);
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040032D5 RID: 13013
	public UIItemCell uiItemCell;

	// Token: 0x040032D6 RID: 13014
	public Image customImage;

	// Token: 0x040032D7 RID: 13015
	public Image customImageBack;

	// Token: 0x040032D8 RID: 13016
	[SerializeField]
	private GameObject uknownCraftBlocker;

	// Token: 0x040032D9 RID: 13017
	[SerializeField]
	private GameObject canNotStartCraftBlocker;

	// Token: 0x040032DA RID: 13018
	[SerializeField]
	private GameObject customImageNonInteractableBlocker;

	// Token: 0x040032DB RID: 13019
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItem;

	// Token: 0x040032DC RID: 13020
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItemExtension;

	// Token: 0x040032DD RID: 13021
	[SerializeField]
	private LazyButton tabButton;

	// Token: 0x040032DE RID: 13022
	[SerializeField]
	private GameObject selectionFrameTab;
}
