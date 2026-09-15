using System;
using Galaxy.Api;
using UnityEngine;

// Token: 0x02000002 RID: 2
[DisallowMultipleComponent]
public class GogGalaxyManager : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static GogGalaxyManager Instance
	{
		get
		{
			if (GogGalaxyManager.singleton == null)
			{
				return new GameObject("GogGalaxyManager").AddComponent<GogGalaxyManager>();
			}
			return GogGalaxyManager.singleton;
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002074 File Offset: 0x00000274
	public static bool IsInitialized()
	{
		return GogGalaxyManager.singleton != null && GogGalaxyManager.singleton.isInitialized;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002090 File Offset: 0x00000290
	private void Awake()
	{
		if (GogGalaxyManager.singleton != null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GogGalaxyManager.singleton = this;
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		try
		{
			GalaxyInstance.Init(new InitParams(this.clientID, this.clientSecret));
		}
		catch (GalaxyInstance.Error error)
		{
			Debug.LogError("Failed to initialize GOG Galaxy: Error = " + error.ToString(), this);
			return;
		}
		Debug.Log("Galaxy SDK was initialized", this);
		this.isInitialized = true;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000211C File Offset: 0x0000031C
	private void OnDestroy()
	{
		if (GogGalaxyManager.singleton != this)
		{
			return;
		}
		GogGalaxyManager.singleton = null;
		if (!this.isInitialized)
		{
			return;
		}
		if (Application.isEditor)
		{
			GalaxyInstance.ShutdownEx(new ShutdownParams(true));
			return;
		}
		GalaxyInstance.Shutdown(true);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002154 File Offset: 0x00000354
	private void Update()
	{
		if (!this.isInitialized)
		{
			return;
		}
		GalaxyInstance.ProcessData();
	}

	// Token: 0x04000001 RID: 1
	public string clientID;

	// Token: 0x04000002 RID: 2
	public string clientSecret;

	// Token: 0x04000003 RID: 3
	private static GogGalaxyManager singleton;

	// Token: 0x04000004 RID: 4
	private bool isInitialized;
}
