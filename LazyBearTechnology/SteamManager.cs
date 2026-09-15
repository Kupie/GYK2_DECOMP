using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x02000027 RID: 39
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060000AB RID: 171 RVA: 0x00004B3B File Offset: 0x00002D3B
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000AC RID: 172 RVA: 0x00004B5F File Offset: 0x00002D5F
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00004B6B File Offset: 0x00002D6B
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00004B73 File Offset: 0x00002D73
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00004B84 File Offset: 0x00002D84
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				Debug.Log("[Steamworks.NET] Shutting down because RestartAppIfNecessary returned true. Steam will restart the application.");
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string text = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(text + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00004C6C File Offset: 0x00002E6C
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00004CBA File Offset: 0x00002EBA
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00004CDE File Offset: 0x00002EDE
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04000064 RID: 100
	protected static bool s_EverInitialized;

	// Token: 0x04000065 RID: 101
	protected static SteamManager s_instance;

	// Token: 0x04000066 RID: 102
	protected bool m_bInitialized;

	// Token: 0x04000067 RID: 103
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
