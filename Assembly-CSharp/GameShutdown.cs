using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
public static class GameShutdown
{
	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06002DFE RID: 11774 RVA: 0x000DBE5D File Offset: 0x000DA05D
	// (set) Token: 0x06002DFF RID: 11775 RVA: 0x000DBE64 File Offset: 0x000DA064
	public static bool IsRequested { get; private set; }

	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06002E00 RID: 11776 RVA: 0x000DBE6C File Offset: 0x000DA06C
	// (set) Token: 0x06002E01 RID: 11777 RVA: 0x000DBE73 File Offset: 0x000DA073
	public static bool IsQuitting { get; private set; }

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x06002E02 RID: 11778 RVA: 0x000DBE7C File Offset: 0x000DA07C
	// (remove) Token: 0x06002E03 RID: 11779 RVA: 0x000DBEB0 File Offset: 0x000DA0B0
	public static event Action Resumed;

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x06002E04 RID: 11780 RVA: 0x000DBEE3 File Offset: 0x000DA0E3
	public static CancellationToken Token
	{
		get
		{
			if (GameShutdown.IsRequested)
			{
				return new CancellationToken(true);
			}
			if (GameShutdown.cts == null)
			{
				GameShutdown.cts = new CancellationTokenSource();
			}
			return GameShutdown.cts.Token;
		}
	}

	// Token: 0x06002E05 RID: 11781 RVA: 0x000DBF0E File Offset: 0x000DA10E
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatics()
	{
		GameShutdown.UninstallHooks();
		GameShutdown.RestoreToken();
		GameShutdown.Resumed = null;
	}

	// Token: 0x06002E06 RID: 11782 RVA: 0x000DBF20 File Offset: 0x000DA120
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InstallHooks()
	{
		if (GameShutdown.hooksRegistered)
		{
			return;
		}
		GameShutdown.hooksRegistered = true;
		Application.wantsToQuit += GameShutdown.OnWantsToQuit;
		Application.quitting += GameShutdown.RequestQuit;
	}

	// Token: 0x06002E07 RID: 11783 RVA: 0x000DBF52 File Offset: 0x000DA152
	private static void UninstallHooks()
	{
		if (!GameShutdown.hooksRegistered)
		{
			return;
		}
		Application.wantsToQuit -= GameShutdown.OnWantsToQuit;
		Application.quitting -= GameShutdown.RequestQuit;
		GameShutdown.hooksRegistered = false;
	}

	// Token: 0x06002E08 RID: 11784 RVA: 0x000DBF84 File Offset: 0x000DA184
	public static void Request()
	{
		if (GameShutdown.IsRequested)
		{
			return;
		}
		GameShutdown.IsRequested = true;
		Debug.Log(string.Format("#shutdown# GameShutdown.Request: cancelling token (isQuitting:[{0}])", GameShutdown.IsQuitting));
		try
		{
			CancellationTokenSource cancellationTokenSource = GameShutdown.cts;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
		}
		catch (ObjectDisposedException)
		{
		}
		Debug.Log("#shutdown# GameShutdown.Request: token cancelled, unwind returned");
	}

	// Token: 0x06002E09 RID: 11785 RVA: 0x000DBFE8 File Offset: 0x000DA1E8
	public static void RequestQuit()
	{
		Debug.Log(string.Format("#shutdown# GameShutdown.RequestQuit (wasRequested:[{0}])", GameShutdown.IsRequested));
		GameShutdown.IsQuitting = true;
		GameShutdown.Request();
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x000DC010 File Offset: 0x000DA210
	public static void ThrowIfRequested()
	{
		GameShutdown.Token.ThrowIfCancellationRequested();
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x000DC02A File Offset: 0x000DA22A
	public static void SetQuitBlocker(Func<bool> blocker)
	{
		GameShutdown.quitBlocker = blocker;
	}

	// Token: 0x06002E0C RID: 11788 RVA: 0x000DC034 File Offset: 0x000DA234
	public static async UniTask<bool> WaitForResumeAsync()
	{
		GameShutdown.<>c__DisplayClass26_0 CS$<>8__locals1 = new GameShutdown.<>c__DisplayClass26_0();
		CS$<>8__locals1.completion = new UniTaskCompletionSource<bool>();
		GameShutdown.Resumed += CS$<>8__locals1.<WaitForResumeAsync>g__OnResumed|0;
		bool flag;
		try
		{
			if (GameShutdown.IsQuitting)
			{
				flag = false;
			}
			else if (!GameShutdown.IsRequested)
			{
				flag = true;
			}
			else
			{
				flag = await CS$<>8__locals1.completion.Task;
			}
		}
		finally
		{
			GameShutdown.Resumed -= CS$<>8__locals1.<WaitForResumeAsync>g__OnResumed|0;
		}
		return flag;
	}

	// Token: 0x06002E0D RID: 11789 RVA: 0x000DC070 File Offset: 0x000DA270
	private static bool OnWantsToQuit()
	{
		Debug.Log("#shutdown# Application.wantsToQuit received");
		GameShutdown.RequestQuit();
		if (!GameShutdown.quitForced && GameShutdown.IsQuitBlocked())
		{
			if (!GameShutdown.quitDeferred)
			{
				GameShutdown.quitDeferred = true;
				GameShutdown.QuitWhenUnblocked().Forget();
			}
			Debug.Log("#shutdown# Application.wantsToQuit: deferring quit until in-flight engine work lands");
			return false;
		}
		Debug.Log("#shutdown# Application.wantsToQuit: allowing quit");
		return true;
	}

	// Token: 0x06002E0E RID: 11790 RVA: 0x000DC0CC File Offset: 0x000DA2CC
	private static bool IsQuitBlocked()
	{
		if (GameShutdown.quitBlocker == null)
		{
			return false;
		}
		bool flag;
		try
		{
			flag = GameShutdown.quitBlocker();
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			flag = false;
		}
		return flag;
	}

	// Token: 0x06002E0F RID: 11791 RVA: 0x000DC10C File Offset: 0x000DA30C
	private static async UniTaskVoid QuitWhenUnblocked()
	{
		float deadline = Time.realtimeSinceStartup + 15f;
		while (GameShutdown.IsQuitBlocked() && Time.realtimeSinceStartup < deadline)
		{
			await UniTask.Yield();
		}
		if (GameShutdown.IsQuitBlocked())
		{
			GameShutdown.quitForced = true;
			Debug.LogWarning(string.Format("#shutdown# quit still blocked after {0}s, quitting anyway", 15f));
		}
		await UniTask.NextFrame();
		Debug.Log("#shutdown# re-issuing Application.Quit");
		Application.Quit();
	}

	// Token: 0x06002E10 RID: 11792 RVA: 0x000DC148 File Offset: 0x000DA348
	private static void RestoreToken()
	{
		Debug.Log(string.Format("#shutdown# GameShutdown.RestoreToken (wasRequested:[{0}] wasQuitting:[{1}])", GameShutdown.IsRequested, GameShutdown.IsQuitting));
		GameShutdown.IsRequested = false;
		GameShutdown.IsQuitting = false;
		GameShutdown.quitDeferred = false;
		GameShutdown.quitForced = false;
		try
		{
			CancellationTokenSource cancellationTokenSource = GameShutdown.cts;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Dispose();
			}
		}
		catch (ObjectDisposedException)
		{
		}
		GameShutdown.cts = new CancellationTokenSource();
	}

	// Token: 0x0400250A RID: 9482
	private const float QUIT_DEFER_TIMEOUT_SECONDS = 15f;

	// Token: 0x0400250B RID: 9483
	private static CancellationTokenSource cts = new CancellationTokenSource();

	// Token: 0x0400250C RID: 9484
	private static bool hooksRegistered;

	// Token: 0x0400250D RID: 9485
	private static Func<bool> quitBlocker;

	// Token: 0x0400250E RID: 9486
	private static bool quitDeferred;

	// Token: 0x0400250F RID: 9487
	private static bool quitForced;
}
