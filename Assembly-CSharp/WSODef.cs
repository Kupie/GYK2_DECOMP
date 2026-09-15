using System;
using LazyBearTechnology;

// Token: 0x02000225 RID: 549
[Serializable]
public class WSODef : BalanceBaseObject
{
	// Token: 0x06000CF1 RID: 3313 RVA: 0x0003CF87 File Offset: 0x0003B187
	public WSODef()
	{
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x00041062 File Offset: 0x0003F262
	public WSODef(string id)
	{
		this.id = id;
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x00041071 File Offset: 0x0003F271
	public ConstructorPartReplacementConfig ReplacementConfig
	{
		get
		{
			if (!this.cachedReplacementConfig && !string.IsNullOrEmpty(this.replacementConfigId))
			{
				this.cachedReplacementConfig = ConstructorPartReplacementService.GetReplacementConfig(this.replacementConfigId);
			}
			return this.cachedReplacementConfig;
		}
	}

	// Token: 0x04000FFB RID: 4091
	[AutoParse("replacement_config_id")]
	public string replacementConfigId;

	// Token: 0x04000FFC RID: 4092
	[AutoParse("town_quality")]
	public int townQuality;

	// Token: 0x04000FFD RID: 4093
	[NonSerialized]
	private ConstructorPartReplacementConfig cachedReplacementConfig;
}
