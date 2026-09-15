using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000991 RID: 2449
public class UICraftRequirementWidgetData : LazyWidgetDataBase
{
	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06004149 RID: 16713 RVA: 0x00137524 File Offset: 0x00135724
	// (set) Token: 0x0600414A RID: 16714 RVA: 0x0013752C File Offset: 0x0013572C
	public string Id { get; private set; }

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x0600414B RID: 16715 RVA: 0x00137535 File Offset: 0x00135735
	// (set) Token: 0x0600414C RID: 16716 RVA: 0x0013753D File Offset: 0x0013573D
	public string IconId { get; private set; }

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x0600414D RID: 16717 RVA: 0x00137546 File Offset: 0x00135746
	// (set) Token: 0x0600414E RID: 16718 RVA: 0x0013754E File Offset: 0x0013574E
	public string RequirementValue { get; private set; }

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x0600414F RID: 16719 RVA: 0x00137557 File Offset: 0x00135757
	// (set) Token: 0x06004150 RID: 16720 RVA: 0x0013755F File Offset: 0x0013575F
	public List<PerkData> LinkedActivePerks { get; private set; }

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06004151 RID: 16721 RVA: 0x00137568 File Offset: 0x00135768
	// (set) Token: 0x06004152 RID: 16722 RVA: 0x00137570 File Offset: 0x00135770
	public CraftDefBase CraftDef { get; private set; }

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x06004153 RID: 16723 RVA: 0x00137579 File Offset: 0x00135779
	// (set) Token: 0x06004154 RID: 16724 RVA: 0x00137581 File Offset: 0x00135781
	public Item ToolForWork { get; private set; }

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x06004155 RID: 16725 RVA: 0x0013758A File Offset: 0x0013578A
	// (set) Token: 0x06004156 RID: 16726 RVA: 0x00137592 File Offset: 0x00135792
	public bool IsRequirement { get; private set; }

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06004157 RID: 16727 RVA: 0x0013759B File Offset: 0x0013579B
	// (set) Token: 0x06004158 RID: 16728 RVA: 0x001375A3 File Offset: 0x001357A3
	public bool IsEnough { get; private set; }

	// Token: 0x06004159 RID: 16729 RVA: 0x001375AC File Offset: 0x001357AC
	public UICraftRequirementWidgetData(string id, string iconId, float value, List<PerkData> linkedActivePerks, CraftDefBase craftDef, Item toolForWork, bool isRequirement, bool isEnough = true)
	{
		this.Id = id;
		this.IconId = iconId;
		if (value != 0f)
		{
			this.RequirementValue = value.ToInvariantCultureString();
		}
		this.LinkedActivePerks = linkedActivePerks;
		this.CraftDef = craftDef;
		this.ToolForWork = toolForWork;
		this.IsRequirement = isRequirement;
		this.IsEnough = isEnough;
	}

	// Token: 0x0600415A RID: 16730 RVA: 0x00137609 File Offset: 0x00135809
	public UICraftRequirementWidgetData(string id, string value, bool isRequirement, bool isEnough = true)
	{
		this.Id = id;
		this.RequirementValue = value;
		this.IsEnough = isEnough;
		this.IsRequirement = isRequirement;
	}
}
