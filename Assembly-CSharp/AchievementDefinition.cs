using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001D4 RID: 468
[Serializable]
public class AchievementDefinition : BalanceBaseObject
{
	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0003C7E6 File Offset: 0x0003A9E6
	public AchievementType AchievementType
	{
		get
		{
			return this.achievementType;
		}
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x0003C7EE File Offset: 0x0003A9EE
	public string GetIdForPlatformUnlock()
	{
		return this.id;
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x0003C7F6 File Offset: 0x0003A9F6
	public string GetPlatformCounterTrigger()
	{
		return this.countTrigger;
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public bool IsAvailableOnCurrentPlatform()
	{
		return true;
	}

	// Token: 0x04000D0F RID: 3343
	[AutoParse("count_trigger")]
	public string countTrigger = string.Empty;

	// Token: 0x04000D10 RID: 3344
	[AutoParse("counter")]
	public int counter = 1;

	// Token: 0x04000D11 RID: 3345
	[AutoParse("xbox_id")]
	public int xboxId = -1;

	// Token: 0x04000D12 RID: 3346
	[AutoParse("playstation4_id")]
	public int playStation4Id = -1;

	// Token: 0x04000D13 RID: 3347
	[AutoParse("playstation5_id")]
	public int playStation5Id = -1;

	// Token: 0x04000D14 RID: 3348
	[SerializeField]
	private AchievementType achievementType;
}
