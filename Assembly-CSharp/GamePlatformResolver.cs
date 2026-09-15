using System;

// Token: 0x0200077F RID: 1919
public static class GamePlatformResolver
{
	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x060031B6 RID: 12726 RVA: 0x000EE6B1 File Offset: 0x000EC8B1
	public static GamePlatform Current
	{
		get
		{
			if (GamePlatformResolver.resolvedPlatform == null)
			{
				GamePlatformResolver.resolvedPlatform = new GamePlatform?(GamePlatformResolver.ResolveBuildPlatform());
			}
			return GamePlatformResolver.resolvedPlatform.Value;
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x060031B7 RID: 12727 RVA: 0x00028294 File Offset: 0x00026494
	public static bool HasEditorOverride
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x00028294 File Offset: 0x00026494
	public static GamePlatform ResolveBuildPlatform()
	{
		return GamePlatform.PC;
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x000EE6D8 File Offset: 0x000EC8D8
	public static string ToTextureImporterPlatformName(this GamePlatform platform)
	{
		switch (platform)
		{
		case GamePlatform.Switch:
			return "Switch";
		case GamePlatform.Switch2:
			return "Switch2";
		case GamePlatform.PS4:
			return "PS4";
		case GamePlatform.PS5:
			return "PS5";
		case GamePlatform.XboxOne:
			return "GameCoreXboxOne";
		case GamePlatform.XboxSeries:
			return "GameCoreScarlett";
		default:
			return "Standalone";
		}
	}

	// Token: 0x040027C6 RID: 10182
	private const string EDITOR_OVERRIDE_KEY = "debug_game_platform_override";

	// Token: 0x040027C7 RID: 10183
	private const int NO_OVERRIDE = -1;

	// Token: 0x040027C8 RID: 10184
	private static GamePlatform? resolvedPlatform;
}
