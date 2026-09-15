using System;
using System.Collections.Generic;
using FlowCanvas;
using UnityEngine;

// Token: 0x020004EE RID: 1262
public class GlobalScriptsManager : MonoBehaviour
{
	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x060020FA RID: 8442 RVA: 0x0009BD78 File Offset: 0x00099F78
	public static GlobalScriptsManager Instance
	{
		get
		{
			if (GlobalScriptsManager.cachedInstance == null)
			{
				GlobalScriptsManager.cachedInstance = global::UnityEngine.Object.FindObjectOfType<GlobalScriptsManager>();
				if (GlobalScriptsManager.cachedInstance == null)
				{
					Debug.LogError(string.Format("Cannot find instance {0} On Scene.", typeof(GlobalScriptsManager)));
				}
			}
			return GlobalScriptsManager.cachedInstance;
		}
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x0009BDC8 File Offset: 0x00099FC8
	public static GlobalFlowScript RunFlowScript(string scriptName, Action onFinished, FlowScriptLoadMode loadMode = FlowScriptLoadMode.DeserializeOnInit)
	{
		D.LogColor(string.Format("Running global script [{0}] ({1})", scriptName, loadMode), "yellow");
		GameObject scriptObject = new GameObject("[g_fs] " + scriptName);
		scriptObject.transform.SetParent(GlobalScriptsManager.Instance.transform);
		GlobalFlowScript globalScript = scriptObject.AddComponent<GlobalFlowScript>();
		GlobalScriptsManager.Instance.runningScripts.Add(globalScript);
		if (!globalScript.Run(scriptObject, scriptName, delegate(bool invokeOnFinished)
		{
			if (invokeOnFinished)
			{
				Action onFinished2 = onFinished;
				if (onFinished2 != null)
				{
					onFinished2();
				}
			}
			GlobalScriptsManager.Instance.runningScripts.Remove(globalScript);
			global::UnityEngine.Object.Destroy(scriptObject);
		}, loadMode))
		{
			D.LogColor("Running global script [" + scriptName + "] was failed. Terminating.", "red");
			GlobalScriptsManager.Instance.runningScripts.Remove(globalScript);
			global::UnityEngine.Object.Destroy(scriptObject);
			return null;
		}
		return globalScript;
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x0009BEB8 File Offset: 0x0009A0B8
	public static void RunFlowScript(FlowScript flowScript, Action onFinished, FlowScriptLoadMode loadMode = FlowScriptLoadMode.DeserializeOnInit)
	{
		D.LogColor(string.Format("Running flow script [{0}] ({1})", flowScript.name, loadMode), "yellow");
		GameObject scriptObject = new GameObject("[g_fs] " + flowScript.name);
		scriptObject.transform.SetParent(GlobalScriptsManager.Instance.transform);
		GlobalFlowScript globalScript = scriptObject.AddComponent<GlobalFlowScript>();
		GlobalScriptsManager.Instance.runningScripts.Add(globalScript);
		if (!globalScript.Run(scriptObject, flowScript, delegate(bool invokeOnFinished)
		{
			if (invokeOnFinished)
			{
				Action onFinished2 = onFinished;
				if (onFinished2 != null)
				{
					onFinished2();
				}
			}
			GlobalScriptsManager.Instance.runningScripts.Remove(globalScript);
			global::UnityEngine.Object.Destroy(scriptObject);
		}, loadMode))
		{
			D.LogColor("Running global script [" + flowScript.name + "] was failed. Terminating.", "red");
			GlobalScriptsManager.Instance.runningScripts.Remove(globalScript);
			global::UnityEngine.Object.Destroy(scriptObject);
		}
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x0009BFB0 File Offset: 0x0009A1B0
	public static void FireEvent(string scriptName, string eventName, Action onFinished = null)
	{
		GlobalFlowScript globalFlowScript = GlobalScriptsManager.RunFlowScript(scriptName, onFinished, FlowScriptLoadMode.DeserializeOnInit);
		if (globalFlowScript != null)
		{
			globalFlowScript.FireEvent(eventName);
		}
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x0009BFD8 File Offset: 0x0009A1D8
	public static bool HasFlowScript(string scriptName)
	{
		return GlobalScriptsManager.Instance.runningScripts.Find((GlobalFlowScript s) => s.ScriptName == scriptName) != null;
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x0009C014 File Offset: 0x0009A214
	public static void TerminateAllRunningScripts()
	{
		for (int i = GlobalScriptsManager.Instance.runningScripts.Count - 1; i >= 0; i--)
		{
			GlobalScriptsManager.Instance.runningScripts[0].Terminate(true);
		}
	}

	// Token: 0x04001D9E RID: 7582
	private const string GLOBAL_FLOW_SCRIPT_OBJECT_PREFIX = "[g_fs]";

	// Token: 0x04001D9F RID: 7583
	private static GlobalScriptsManager cachedInstance;

	// Token: 0x04001DA0 RID: 7584
	private readonly List<GlobalFlowScript> runningScripts = new List<GlobalFlowScript>();
}
