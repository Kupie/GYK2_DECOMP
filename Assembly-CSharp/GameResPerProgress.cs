using System;
using LazyBearTechnology;

// Token: 0x020001E3 RID: 483
[Serializable]
public class GameResPerProgress
{
	// Token: 0x06000C2E RID: 3118 RVA: 0x0003DDDA File Offset: 0x0003BFDA
	public GameResPerProgress(int sucessfulProgressTick, GameRes gameRes)
	{
		this.sucessfulProgressTick = sucessfulProgressTick;
		this.gameRes = gameRes;
	}

	// Token: 0x04000D99 RID: 3481
	public int sucessfulProgressTick;

	// Token: 0x04000D9A RID: 3482
	public GameRes gameRes;
}
