using System;
using FlowCanvas;
using UnityEngine;

// Token: 0x020004EC RID: 1260
public class GlobalFlowScript : CustomFlowScript
{
	// Token: 0x060020F0 RID: 8432 RVA: 0x0009BCF3 File Offset: 0x00099EF3
	public bool Run(GameObject gameObject, string scriptName, GlobalFlowScript.TerminateCallBack onFinished = null)
	{
		return this.Run(gameObject, scriptName, onFinished, FlowScriptLoadMode.DeserializeOnInit);
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x0009BCFF File Offset: 0x00099EFF
	public bool Run(GameObject gameObject, string scriptName, GlobalFlowScript.TerminateCallBack onFinished, FlowScriptLoadMode loadMode)
	{
		this.flowGraph = base.GetGraph("Assets/AddressableAssets/VisualScripts/GlobalScripts/" + scriptName);
		if (this.flowGraph == null)
		{
			return false;
		}
		base.Run(gameObject, this.flowGraph, scriptName, onFinished, loadMode);
		return true;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x0009BD3A File Offset: 0x00099F3A
	public bool Run(GameObject gameObject, FlowGraph graph, GlobalFlowScript.TerminateCallBack onFinished = null)
	{
		return this.Run(gameObject, graph, onFinished, FlowScriptLoadMode.DeserializeOnInit);
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x0009BD46 File Offset: 0x00099F46
	public bool Run(GameObject gameObject, FlowGraph graph, GlobalFlowScript.TerminateCallBack onFinished, FlowScriptLoadMode loadMode)
	{
		base.Run(gameObject, graph, graph.name, onFinished, loadMode);
		return true;
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x0009BD5A File Offset: 0x00099F5A
	public override void Terminate(bool invokeOnFinished = true)
	{
		base.Terminate(invokeOnFinished);
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001D9D RID: 7581
	public const string GLOBAL_SCRIPT_RELATIVE_PATH = "Assets/AddressableAssets/VisualScripts/GlobalScripts/";

	// Token: 0x020004ED RID: 1261
	// (Invoke) Token: 0x060020F7 RID: 8439
	public delegate void TerminateCallBack(bool callOnFinished);
}
