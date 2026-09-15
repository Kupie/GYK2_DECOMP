using System;
using System.Globalization;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000243 RID: 579
[CreateAssetMenu(fileName = "GameInfo", menuName = "GK2/Game Info")]
public class GameInfo : LazySingletonSO<GameInfo>
{
	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0004C204 File Offset: 0x0004A404
	public string Version
	{
		get
		{
			return this.version.ToString(CultureInfo.InvariantCulture);
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0004C216 File Offset: 0x0004A416
	public bool ShowVersionInGame
	{
		get
		{
			return this.showVersionInGame;
		}
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x0004C21E File Offset: 0x0004A41E
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void LogStartupVersion()
	{
		Debug.Log("Starting game, ver. " + LazySingletonSO<GameInfo>.Instance.Version + "." + LazySingletonSO<GameInfo>.Instance.gitCommitShortHash);
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x0004C248 File Offset: 0x0004A448
	public int GetVersion()
	{
		string text = this.version.Replace(".", "");
		int num;
		if (int.TryParse(text, out num))
		{
			return num;
		}
		throw new ArgumentException("Can't parse version: " + text);
	}

	// Token: 0x04001169 RID: 4457
	[SerializeField]
	private string version;

	// Token: 0x0400116A RID: 4458
	[SerializeField]
	public string gitCommitShortHash;

	// Token: 0x0400116B RID: 4459
	[SerializeField]
	[Tooltip("If enabled, the version label stays visible during gameplay, not only in the main menu.")]
	private bool showVersionInGame;
}
