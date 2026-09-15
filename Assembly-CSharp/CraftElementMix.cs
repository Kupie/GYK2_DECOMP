using System;

// Token: 0x0200026B RID: 619
[Serializable]
public class CraftElementMix : CraftElementT<AlchemyMixDef>
{
	// Token: 0x06001026 RID: 4134 RVA: 0x0005202B File Offset: 0x0005022B
	public CraftElementMix(CraftDefBase definition)
		: base(definition)
	{
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x00052034 File Offset: 0x00050234
	public CraftElementMix(CraftDefBase definition, CraftParamsData craftParamsData)
		: base(definition, craftParamsData)
	{
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0005203E File Offset: 0x0005023E
	protected CraftElementMix(CraftElementMix other, int count = 1)
		: base(other, count)
	{
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x00052048 File Offset: 0x00050248
	public override CraftElementBase Clone(int count = 1)
	{
		return new CraftElementMix(this, count);
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x00002318 File Offset: 0x00000518
	public override void RemoveCraftRequirements(ICraftable craftable)
	{
	}
}
