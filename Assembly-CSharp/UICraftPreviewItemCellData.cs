using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200098D RID: 2445
public class UICraftPreviewItemCellData : LazyWidgetDataBase
{
	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x060040E5 RID: 16613 RVA: 0x001364D9 File Offset: 0x001346D9
	// (set) Token: 0x060040E6 RID: 16614 RVA: 0x001364E1 File Offset: 0x001346E1
	public CraftDef CraftDef { get; private set; }

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x060040E7 RID: 16615 RVA: 0x001364EA File Offset: 0x001346EA
	// (set) Token: 0x060040E8 RID: 16616 RVA: 0x001364F2 File Offset: 0x001346F2
	public WgoData WgoData { get; private set; }

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x060040E9 RID: 16617 RVA: 0x001364FB File Offset: 0x001346FB
	public bool IsUnknown
	{
		get
		{
			return this.CraftDef != null && this.CraftDef.isNeedsUnlock && !MainGame.Instance.GameSave.knowledgeSystem.unlockedCrafts.Contains(this.CraftDef.id);
		}
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x060040EA RID: 16618 RVA: 0x0013653B File Offset: 0x0013473B
	// (set) Token: 0x060040EB RID: 16619 RVA: 0x00136543 File Offset: 0x00134743
	public bool IsTab { get; private set; }

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x060040EC RID: 16620 RVA: 0x0013654C File Offset: 0x0013474C
	public bool CanStart
	{
		get
		{
			return this.CraftDef.CanActuallyStartCraft(this.WgoData);
		}
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x060040ED RID: 16621 RVA: 0x0013655F File Offset: 0x0013475F
	// (set) Token: 0x060040EE RID: 16622 RVA: 0x00136567 File Offset: 0x00134767
	public string TabId { get; private set; }

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x060040EF RID: 16623 RVA: 0x00136570 File Offset: 0x00134770
	// (set) Token: 0x060040F0 RID: 16624 RVA: 0x00136578 File Offset: 0x00134778
	public string ExtensionId { get; private set; }

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x060040F1 RID: 16625 RVA: 0x00136581 File Offset: 0x00134781
	// (set) Token: 0x060040F2 RID: 16626 RVA: 0x00136589 File Offset: 0x00134789
	public bool IsExtensionAvailable { get; private set; }

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x060040F3 RID: 16627 RVA: 0x00136592 File Offset: 0x00134792
	// (set) Token: 0x060040F4 RID: 16628 RVA: 0x0013659A File Offset: 0x0013479A
	public bool IsExtension { get; private set; }

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x060040F5 RID: 16629 RVA: 0x001365A3 File Offset: 0x001347A3
	// (set) Token: 0x060040F6 RID: 16630 RVA: 0x001365AB File Offset: 0x001347AB
	public bool UseTabAsIcon { get; private set; }

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x060040F7 RID: 16631 RVA: 0x001365B4 File Offset: 0x001347B4
	// (set) Token: 0x060040F8 RID: 16632 RVA: 0x001365BC File Offset: 0x001347BC
	public bool IsGravePartRemove { get; private set; }

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x060040F9 RID: 16633 RVA: 0x001365C5 File Offset: 0x001347C5
	// (set) Token: 0x060040FA RID: 16634 RVA: 0x001365CD File Offset: 0x001347CD
	public Sprite CustomImageBackSprite { get; private set; }

	// Token: 0x060040FB RID: 16635 RVA: 0x001365D8 File Offset: 0x001347D8
	public UICraftPreviewItemCellData(CraftDef craftDef, WgoData wgoData, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onQueueAdded, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onCraftStarted, string tabId = "", bool isGravePartRemove = false, Sprite customImageBackSprite = null)
	{
		this.CraftDef = craftDef;
		this.WgoData = wgoData;
		this.OnQueueAdded = onQueueAdded;
		this.OnCraftStarted = onCraftStarted;
		this.TabId = tabId;
		this.IsGravePartRemove = isGravePartRemove;
		this.IsTab = !string.IsNullOrEmpty(tabId);
		this.CustomImageBackSprite = customImageBackSprite;
	}

	// Token: 0x060040FC RID: 16636 RVA: 0x00136630 File Offset: 0x00134830
	public UICraftPreviewItemCellData(string tabId, string extensionId, bool isExtensionAvailable, bool useTabAsIcon, bool isGravePartRemove = false, Sprite customImageBackSprite = null)
	{
		this.CraftDef = null;
		this.WgoData = null;
		this.OnQueueAdded = null;
		this.OnCraftStarted = null;
		this.TabId = tabId;
		this.ExtensionId = extensionId;
		this.IsTab = true;
		this.IsExtensionAvailable = isExtensionAvailable;
		this.IsExtension = true;
		this.UseTabAsIcon = useTabAsIcon;
		this.IsGravePartRemove = isGravePartRemove;
		this.CustomImageBackSprite = customImageBackSprite;
	}

	// Token: 0x040032DF RID: 13023
	public Action<CraftDef, List<NeedItemData>, CraftParamsData, int> OnQueueAdded;

	// Token: 0x040032E0 RID: 13024
	public Action<CraftDef, List<NeedItemData>, CraftParamsData, int> OnCraftStarted;
}
