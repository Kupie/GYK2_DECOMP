using System;
using FlowCanvas;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public static class GameScriptUtility
{
	// Token: 0x060020DB RID: 8411 RVA: 0x0009BA02 File Offset: 0x00099C02
	public static void RunGlobalScript(string globalScriptName, Action callback = null)
	{
		GlobalScriptsManager.RunFlowScript(globalScriptName, callback, FlowScriptLoadMode.DeserializeOnInit);
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x0009BA0D File Offset: 0x00099C0D
	public static void RunGlobalScript(FlowScript flowScript, Action callback = null)
	{
		GlobalScriptsManager.RunFlowScript(flowScript, callback, FlowScriptLoadMode.DeserializeOnInit);
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x0009BA17 File Offset: 0x00099C17
	public static void FireEvent(GameObject gameObject, string eventName)
	{
		GameScriptUtility.Internal_FireEvent(gameObject, eventName);
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x0009BA20 File Offset: 0x00099C20
	private static void Internal_FireEvent(GameObject gameObject, string eventName)
	{
		CustomFlowScript component = gameObject.GetComponent<CustomFlowScript>();
		if (component != null)
		{
			component.FireEvent(eventName);
			return;
		}
		Debug.LogError(string.Format("Not found flow script component in game object: {0}, event name: {1}", gameObject, eventName));
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x0009BA58 File Offset: 0x00099C58
	public static void TerminateScript(GameObject gameObject)
	{
		if (gameObject != null)
		{
			CustomFlowScript component = gameObject.GetComponent<CustomFlowScript>();
			if (component != null)
			{
				component.Terminate(true);
			}
		}
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x0009BA85 File Offset: 0x00099C85
	public static void PauseScript(GameObject gameObject)
	{
		if (gameObject != null)
		{
			gameObject.GetComponent<CustomFlowScript>().PauseBehaviour();
		}
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x0009BA9B File Offset: 0x00099C9B
	public static void StartScriptOnObject(GameObject gameObject)
	{
		if (gameObject != null)
		{
			gameObject.GetComponent<CustomFlowScript>().StartBehaviour();
		}
	}
}
