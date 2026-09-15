using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200026D RID: 621
[Serializable]
public class CraftElementSurvey : CraftElementT<SurveyDef>
{
	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06001030 RID: 4144 RVA: 0x00052087 File Offset: 0x00050287
	public string SelectedSurveyItemId
	{
		get
		{
			return this.selectedSurveyItemId;
		}
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0005208F File Offset: 0x0005028F
	public CraftElementSurvey(CraftDefBase definition)
		: base(definition)
	{
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x00052098 File Offset: 0x00050298
	public CraftElementSurvey(CraftDefBase definition, CraftParamsData craftParamsData)
		: base(definition, craftParamsData)
	{
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x000520A2 File Offset: 0x000502A2
	public CraftElementSurvey(CraftDefBase definition, List<NeedItemData> requirements, CraftParamsData craftParamsData, string selectedSurveyItemId)
		: base(definition.id, 1, requirements, craftParamsData)
	{
		this.selectedSurveyItemId = selectedSurveyItemId;
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x000520BB File Offset: 0x000502BB
	protected CraftElementSurvey(CraftElementSurvey other, int count = 1)
		: base(other, count)
	{
		this.selectedSurveyItemId = other.selectedSurveyItemId;
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x000520D1 File Offset: 0x000502D1
	public override CraftElementBase Clone(int count = 1)
	{
		return new CraftElementSurvey(this, count);
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x000520DC File Offset: 0x000502DC
	public override void RemoveCraftRequirements(ICraftable craftable)
	{
		if (base.Definition.isScienceFuelCraft)
		{
			this.craftInput = craftable.GetCraftableMultiInventory(false).RemoveItems(base.Requirements, craftable as WgoData);
			return;
		}
		List<NeedItemData> list = new List<NeedItemData>();
		for (int i = 1; i < base.Requirements.Count; i++)
		{
			list.Add(base.Requirements[i]);
		}
		this.craftInput = craftable.GetCraftableMultiInventory(false).RemoveItems(list, craftable as WgoData);
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0005215C File Offset: 0x0005035C
	public OutputPreview GetSelectedItemOutputPreview()
	{
		ItemDef dataOrNull = GameBalance.Me.GetDataOrNull<ItemDef>(this.selectedSurveyItemId);
		if (dataOrNull == null)
		{
			return base.Definition.GetOutputPreview(null);
		}
		return new OutputPreview(base.Definition.id, string.Empty, false, 1, -1, dataOrNull.iconId);
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x000521A8 File Offset: 0x000503A8
	protected override CraftDefBase GetCraftDef()
	{
		return GameBalance.GetSurveyDef(this.craftId);
	}

	// Token: 0x04001293 RID: 4755
	[SerializeField]
	private string selectedSurveyItemId;
}
