using System;
using System.Threading;
using Cysharp.Threading.Tasks;

// Token: 0x020006C7 RID: 1735
public static class BackgroundLoading
{
	// Token: 0x06002DFB RID: 11771 RVA: 0x000DBE0D File Offset: 0x000DA00D
	public static bool ShouldYield(int index, int yieldEveryBase)
	{
		return BackgroundLoading.IsActive && index % (yieldEveryBase * BackgroundLoading.YieldEveryMultiplier) == 0;
	}

	// Token: 0x06002DFC RID: 11772 RVA: 0x000DBE24 File Offset: 0x000DA024
	public static UniTask YieldIfNeeded(int index, int yieldEveryBase)
	{
		CancellationToken token = GameShutdown.Token;
		token.ThrowIfCancellationRequested();
		if (!BackgroundLoading.ShouldYield(index, yieldEveryBase))
		{
			return UniTask.CompletedTask;
		}
		return UniTask.Yield(PlayerLoopTiming.Update, token, false);
	}

	// Token: 0x04002508 RID: 9480
	public static bool IsActive;

	// Token: 0x04002509 RID: 9481
	public static int YieldEveryMultiplier = 1;
}
