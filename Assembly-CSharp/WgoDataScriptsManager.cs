using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F3 RID: 1267
public class WgoDataScriptsManager : MonoBehaviour
{
	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06002109 RID: 8457 RVA: 0x0009C128 File Offset: 0x0009A328
	public static WgoDataScriptsManager Instance
	{
		get
		{
			if (WgoDataScriptsManager.cachedInstance == null)
			{
				WgoDataScriptsManager.cachedInstance = global::UnityEngine.Object.FindObjectOfType<WgoDataScriptsManager>();
				if (WgoDataScriptsManager.cachedInstance == null)
				{
					Debug.LogError(string.Format("Cannot find instance {0} On Scene.", typeof(WgoDataScriptsManager)));
				}
			}
			return WgoDataScriptsManager.cachedInstance;
		}
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x0009C178 File Offset: 0x0009A378
	public static void CreateScript(WgoData wgoData, string scriptName)
	{
		D.LogColor(string.Concat(new string[] { "Running script: [", scriptName, "] on WgoData: [", wgoData.id, "]." }), "yellow");
		if (WgoDataScriptsManager.Instance.runningScripts.ContainsKey(wgoData.UniqueId))
		{
			return;
		}
		GameObject gameObject = new GameObject(wgoData.id ?? "");
		gameObject.transform.SetParent(WgoDataScriptsManager.Instance.transform);
		WgoDataScript wgoDataScript = gameObject.AddComponent<WgoDataScript>();
		if (!wgoDataScript.Run(wgoData, gameObject, scriptName))
		{
			D.LogColor(string.Concat(new string[] { "Running script: [", scriptName, "] on WgoData: [", wgoData.id, "] was failed. Terminating." }), "red");
			global::UnityEngine.Object.Destroy(gameObject);
			return;
		}
		WgoDataScriptsManager.Instance.runningScripts.TryAdd(wgoData.UniqueId, wgoDataScript);
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x0009C26C File Offset: 0x0009A46C
	public static void FireEvent(WgoData wgoData, string eventName)
	{
		WgoDataScript wgoDataScript;
		if (!WgoDataScriptsManager.Instance.runningScripts.TryGetValue(wgoData.UniqueId, out wgoDataScript))
		{
			Debug.LogError("[WgoDataScriptsManager]: cannot find active script for wgo data [" + wgoData.id + "].");
			return;
		}
		wgoDataScript.FireEvent(eventName);
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x0009C2B4 File Offset: 0x0009A4B4
	public static void DestroyScript(WgoData wgoData)
	{
		WgoDataScript wgoDataScript;
		if (!WgoDataScriptsManager.Instance.runningScripts.TryGetValue(wgoData.UniqueId, out wgoDataScript))
		{
			Debug.LogError("[WgoDataScriptsManager]: script for wgo data [" + wgoData.id + "] has already been destroyed.");
			return;
		}
		WgoDataScriptsManager.Instance.runningScripts.Remove(wgoData.UniqueId);
		wgoDataScript.Terminate(false);
		global::UnityEngine.Object.Destroy(wgoDataScript.gameObject);
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x0009C320 File Offset: 0x0009A520
	public static void DestroyAll()
	{
		foreach (WgoDataScript wgoDataScript in WgoDataScriptsManager.Instance.runningScripts.Values)
		{
			wgoDataScript.Terminate(false);
			global::UnityEngine.Object.Destroy(wgoDataScript.gameObject);
		}
		WgoDataScriptsManager.Instance.runningScripts.Clear();
	}

	// Token: 0x04001DAA RID: 7594
	private static WgoDataScriptsManager cachedInstance;

	// Token: 0x04001DAB RID: 7595
	private readonly Dictionary<SGuid, WgoDataScript> runningScripts = new Dictionary<SGuid, WgoDataScript>();
}
