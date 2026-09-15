using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AB7 RID: 2743
public static class DevUtils
{
	// Token: 0x06004A0E RID: 18958 RVA: 0x00002318 File Offset: 0x00000518
	public static void SetCameraRenderType(bool isNativeCameraInUse)
	{
	}

	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x06004A0F RID: 18959 RVA: 0x0015DB00 File Offset: 0x0015BD00
	public static HashSet<string> GameplayScenes
	{
		get
		{
			return new HashSet<string> { "RuinedTemple", "NorthRuinedTemple", "Prison", "IntroScoutBuilding", "IntroScoutSewer", "PalaceSewer" };
		}
	}

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x06004A10 RID: 18960 RVA: 0x0015DB5A File Offset: 0x0015BD5A
	public static int PixelSize
	{
		get
		{
			if (!DevUtils.IsNativeCameraInUse)
			{
				return 2;
			}
			return 1;
		}
	}

	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x06004A11 RID: 18961 RVA: 0x0015DB66 File Offset: 0x0015BD66
	// (set) Token: 0x06004A12 RID: 18962 RVA: 0x0015DB75 File Offset: 0x0015BD75
	public static bool IsNativeCameraInUse
	{
		get
		{
			return PlayerPrefs.GetInt("native_camera_disable") == 0;
		}
		private set
		{
			PlayerPrefs.SetInt("native_camera_disable", value ? 0 : 1);
		}
	}

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x06004A13 RID: 18963 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06004A14 RID: 18964 RVA: 0x0015DB88 File Offset: 0x0015BD88
	public static bool SkipMainScene
	{
		get
		{
			return false;
		}
		set
		{
			PlayerPrefs.SetInt("main_scene_skip", value.ToInt(0));
		}
	}

	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x06004A15 RID: 18965 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06004A16 RID: 18966 RVA: 0x0015DB9B File Offset: 0x0015BD9B
	public static bool AutoSelectWgo
	{
		get
		{
			return false;
		}
		set
		{
			PlayerPrefs.SetInt("auto_select_wgo", value.ToInt(0));
		}
	}

	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x06004A17 RID: 18967 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06004A18 RID: 18968 RVA: 0x0015DBAE File Offset: 0x0015BDAE
	public static bool AutoSelectConstructorPart
	{
		get
		{
			return false;
		}
		set
		{
			PlayerPrefs.SetInt("auto_select_constructor_part", value.ToInt(0));
		}
	}

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x06004A19 RID: 18969 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06004A1A RID: 18970 RVA: 0x0015DBC1 File Offset: 0x0015BDC1
	public static bool EnableAnalyzer
	{
		get
		{
			return false;
		}
		set
		{
			PlayerPrefs.SetInt("enable_analyzer", value.ToInt(0));
		}
	}

	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x06004A1B RID: 18971 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06004A1C RID: 18972 RVA: 0x0015DBD4 File Offset: 0x0015BDD4
	public static bool SkipQuestAutoStart
	{
		get
		{
			return false;
		}
		set
		{
			PlayerPrefs.SetInt("skip_quest_auto_start", value.ToInt(0));
		}
	}

	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x06004A1D RID: 18973 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06004A1E RID: 18974 RVA: 0x0015DBE7 File Offset: 0x0015BDE7
	public static bool SkipCameraAnimation
	{
		get
		{
			return false;
		}
		set
		{
			PlayerPrefs.SetInt("skip_camera_animation", value.ToInt(0));
		}
	}

	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x06004A1F RID: 18975 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06004A20 RID: 18976 RVA: 0x0015DBFA File Offset: 0x0015BDFA
	public static bool SimulateDemoBitsummit
	{
		get
		{
			return false;
		}
		set
		{
			PlayerPrefs.SetInt("simulate_demo_bitsummit", value.ToInt(0));
		}
	}

	// Token: 0x17000B38 RID: 2872
	// (get) Token: 0x06004A21 RID: 18977 RVA: 0x00028294 File Offset: 0x00026494
	public static bool IsDemoBitsummitActive
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000B39 RID: 2873
	// (get) Token: 0x06004A22 RID: 18978 RVA: 0x0015DC0D File Offset: 0x0015BE0D
	// (set) Token: 0x06004A23 RID: 18979 RVA: 0x0015DC15 File Offset: 0x0015BE15
	public static bool ZombieLog
	{
		get
		{
			return DevUtils.IsLogChannelEnabled(LogChannel.Zombie);
		}
		set
		{
			DevUtils.SetLogChannelEnabled(LogChannel.Zombie, value);
		}
	}

	// Token: 0x17000B3A RID: 2874
	// (get) Token: 0x06004A24 RID: 18980 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06004A25 RID: 18981 RVA: 0x0015DC1E File Offset: 0x0015BE1E
	public static LogChannel EnabledLogChannels
	{
		get
		{
			return LogChannel.None;
		}
		set
		{
			PlayerPrefs.SetInt("log_channels", (int)value);
		}
	}

	// Token: 0x06004A26 RID: 18982 RVA: 0x0015DC2B File Offset: 0x0015BE2B
	public static bool IsLogChannelEnabled(LogChannel channel)
	{
		return (DevUtils.EnabledLogChannels & channel) > LogChannel.None;
	}

	// Token: 0x06004A27 RID: 18983 RVA: 0x0015DC37 File Offset: 0x0015BE37
	public static void SetLogChannelEnabled(LogChannel channel, bool enabled)
	{
		DevUtils.EnabledLogChannels = (enabled ? (DevUtils.EnabledLogChannels | channel) : (DevUtils.EnabledLogChannels & ~channel));
	}

	// Token: 0x06004A28 RID: 18984 RVA: 0x0015DC52 File Offset: 0x0015BE52
	public static void ToggleLogChannel(LogChannel channel)
	{
		DevUtils.EnabledLogChannels ^= channel;
	}

	// Token: 0x06004A29 RID: 18985 RVA: 0x0015DC60 File Offset: 0x0015BE60
	private static LogChannel GetAllLogChannelsMask()
	{
		LogChannel logChannel = LogChannel.None;
		foreach (object obj in Enum.GetValues(typeof(LogChannel)))
		{
			LogChannel logChannel2 = (LogChannel)obj;
			if (logChannel2 != LogChannel.None)
			{
				logChannel |= logChannel2;
			}
		}
		return logChannel;
	}

	// Token: 0x06004A2A RID: 18986 RVA: 0x0015DCC8 File Offset: 0x0015BEC8
	public static string GetTextureGndPostfix(bool isConstructor)
	{
		if (!isConstructor)
		{
			return "_gnd";
		}
		return "-gnd";
	}

	// Token: 0x06004A2B RID: 18987 RVA: 0x0015DCD8 File Offset: 0x0015BED8
	public static string GetTextureShPostfix(bool isConstructor)
	{
		if (!isConstructor)
		{
			return "_sh";
		}
		return "-sh";
	}

	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x06004A2C RID: 18988 RVA: 0x0015DCE8 File Offset: 0x0015BEE8
	public static bool IsDebugFogActive
	{
		get
		{
			return DevUtils.verticalFogDisableZones != null;
		}
	}

	// Token: 0x06004A2D RID: 18989 RVA: 0x0015DCF4 File Offset: 0x0015BEF4
	public static void SetDebugFogState(bool isActive)
	{
		if (isActive)
		{
			DevUtils.verticalFogDisableZones = global::UnityEngine.Object.FindObjectsByType<VerticalFogDisableZone>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
			VerticalFogDisableZone[] array = DevUtils.verticalFogDisableZones;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}
		else
		{
			VerticalFogDisableZone[] array = DevUtils.verticalFogDisableZones;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
			DevUtils.verticalFogDisableZones = null;
		}
		WeatherSystem.Instance.SetWeatherComponent("DebugFogOpacity", isActive);
	}

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x06004A2E RID: 18990 RVA: 0x0015DD62 File Offset: 0x0015BF62
	// (set) Token: 0x06004A2F RID: 18991 RVA: 0x0015DD69 File Offset: 0x0015BF69
	public static bool IsDevWindValueSet { get; set; }

	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x06004A30 RID: 18992 RVA: 0x0015DD71 File Offset: 0x0015BF71
	// (set) Token: 0x06004A31 RID: 18993 RVA: 0x0015DD78 File Offset: 0x0015BF78
	public static float DevWindValue { get; set; }

	// Token: 0x040039D1 RID: 14801
	public static List<string> MeshUnlitMaterialPostfixes = new List<string> { "_ut", "-ut" };

	// Token: 0x040039D2 RID: 14802
	public const string MESH_BLACKOUT_MATERIAL_POSTFIX = "-blackout";

	// Token: 0x040039D3 RID: 14803
	public const string TREE_MESH_PREFIX = "tree_";

	// Token: 0x040039D4 RID: 14804
	public const string TREE_LEAVES_MESH_PART = "_fm";

	// Token: 0x040039D5 RID: 14805
	public const float GFX_SIZE_MULTIPLIER = 100f;

	// Token: 0x040039D6 RID: 14806
	public const string KEY_AUTOLOADER_SCENE = "scene_autoloader";

	// Token: 0x040039D7 RID: 14807
	public const string KEY_SKIP_MAIN_SCENE = "main_scene_skip";

	// Token: 0x040039D8 RID: 14808
	public const string KEY_AUTO_SELECT_WGO = "auto_select_wgo";

	// Token: 0x040039D9 RID: 14809
	public const string KEY_AUTO_SELECT_CONSTRUCTOR_PART = "auto_select_constructor_part";

	// Token: 0x040039DA RID: 14810
	public const string KEY_ENABLE_ANALYZER = "enable_analyzer";

	// Token: 0x040039DB RID: 14811
	public const string SKIP_QUEST_AUTO_START = "skip_quest_auto_start";

	// Token: 0x040039DC RID: 14812
	public const string KEY_SKIP_CAMERA_ANIMATION = "skip_camera_animation";

	// Token: 0x040039DD RID: 14813
	public const string KEY_NATIVE_CAMERA_DISABLED = "native_camera_disable";

	// Token: 0x040039DE RID: 14814
	public const string KEY_ZOMBIE_LOG = "zombie_log";

	// Token: 0x040039DF RID: 14815
	public const string KEY_LOG_CHANNELS = "log_channels";

	// Token: 0x040039E0 RID: 14816
	public const string KEY_SIMULATE_DEMO_BITSUMMIT = "simulate_demo_bitsummit";

	// Token: 0x040039E1 RID: 14817
	public const string CONSTRUCTOR_ROOT_PATH = "Assets/_WorldAssets/Constructor";

	// Token: 0x040039E2 RID: 14818
	public const string MODELS_ROOT_PATH = "Assets/_WorldAssets";

	// Token: 0x040039E3 RID: 14819
	public const string TEXTURE_DIR_NAME = "Textures";

	// Token: 0x040039E4 RID: 14820
	public const string MESHES_DIR_NAME = "Meshes";

	// Token: 0x040039E5 RID: 14821
	public const string MODEL_PREFAB_DIR_NAME = "Prefabs";

	// Token: 0x040039E6 RID: 14822
	public const string LUTS_DIR_NAME = "LUTs";

	// Token: 0x040039E7 RID: 14823
	public const string MESH_PIVOT_POSTFIX = "-pivot";

	// Token: 0x040039E8 RID: 14824
	public const string MESH_COLLISION_POSTFIX = "-collision";

	// Token: 0x040039E9 RID: 14825
	public const string PREFAB_EXTENSION = ".prefab";

	// Token: 0x040039EA RID: 14826
	public const string MESH_MERGE_TECH_POSTFIX = "-MERGE";

	// Token: 0x040039EB RID: 14827
	public const string MESH_SHADOW_POSTFIX_0 = "_sh";

	// Token: 0x040039EC RID: 14828
	public const string MESH_SHADOW_POSTFIX_1 = "-sh";

	// Token: 0x040039ED RID: 14829
	public const string MESH_SHADOW_POSTFIX_2 = "_sh2";

	// Token: 0x040039EE RID: 14830
	public const string MESH_SHADOW_POSTFIX_3 = "-sh2";

	// Token: 0x040039EF RID: 14831
	public const string MESH_WIND_CLOTH_POSTFIX = "-wind";

	// Token: 0x040039F0 RID: 14832
	public static bool isMainSceneSkipped = false;

	// Token: 0x040039F1 RID: 14833
	public const string GND_TEXTURE_SUFFIX = "gnd";

	// Token: 0x040039F2 RID: 14834
	public const string SH_TEXTURE_SUFFIX = "sh";

	// Token: 0x040039F3 RID: 14835
	private static VerticalFogDisableZone[] verticalFogDisableZones;
}
