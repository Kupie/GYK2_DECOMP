using System;
using FlowCanvas;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020004BC RID: 1212
public abstract class CustomFlowScript : MonoBehaviour
{
	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x06002046 RID: 8262 RVA: 0x00098FB8 File Offset: 0x000971B8
	// (set) Token: 0x06002047 RID: 8263 RVA: 0x00098FC0 File Offset: 0x000971C0
	public string ScriptName { get; private set; }

	// Token: 0x06002048 RID: 8264 RVA: 0x00098FC9 File Offset: 0x000971C9
	private void Awake()
	{
		if (!CustomFlowScript.wasCustomExceptionHandlerRegistered)
		{
			ResourceManager.ExceptionHandler = new Action<AsyncOperationHandle, Exception>(this.CustomExceptionHandler);
			CustomFlowScript.wasCustomExceptionHandlerRegistered = true;
		}
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x00098FEC File Offset: 0x000971EC
	protected void Run(GameObject gameObject, FlowGraph graph, string scriptName, GlobalFlowScript.TerminateCallBack onFinished = null, FlowScriptLoadMode loadMode = FlowScriptLoadMode.DeserializeOnInit)
	{
		this.ScriptName = scriptName;
		this.OnFinished = onFinished;
		this.flowScriptController = gameObject.AddComponent<FlowScriptController>();
		this.blackboard = gameObject.AddComponent<Blackboard>();
		this.flowScriptController.graph = graph;
		this.flowScriptController.blackboard = this.blackboard;
		if (loadMode == FlowScriptLoadMode.DeserializeOnInit)
		{
			this.flowScriptController.Initialize();
		}
		if (gameObject.activeInHierarchy)
		{
			this.flowScriptController.StartBehaviour();
		}
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x00099060 File Offset: 0x00097260
	public void Reattach(FlowGraph graph, string scriptName)
	{
		this.ScriptName = scriptName;
		this.flowScriptController.SwitchBehaviour(graph as FlowScript);
		global::UnityEngine.Object.Destroy(this.blackboard);
		this.blackboard = base.gameObject.AddComponent<Blackboard>();
		this.flowScriptController.blackboard = this.blackboard;
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x000990B4 File Offset: 0x000972B4
	public virtual void Terminate(bool invokeOnFinished = true)
	{
		if (this.flowGraph != null)
		{
			FlowScriptAssetLoadPolicy.ReleaseGraph(this.flowGraph);
			this.flowGraph = null;
		}
		this.flowScriptController.StopBehaviour(true);
		GlobalFlowScript.TerminateCallBack onFinished = this.OnFinished;
		if (onFinished != null)
		{
			onFinished(invokeOnFinished);
		}
		global::UnityEngine.Object.Destroy(this.flowScriptController);
		global::UnityEngine.Object.Destroy(this.blackboard);
		global::UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x0009911C File Offset: 0x0009731C
	public void FireEvent(string eventName)
	{
		D.LogColor(string.Concat(new string[] { "FireEvent [", eventName, "] at [", base.name, "]" }), "yellow", this);
		this.flowScriptController.SendEvent(eventName);
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x00099170 File Offset: 0x00097370
	public void PauseBehaviour()
	{
		this.flowScriptController.PauseBehaviour();
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x0009917D File Offset: 0x0009737D
	public void StartBehaviour()
	{
		this.flowScriptController.StartBehaviour();
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x0009918A File Offset: 0x0009738A
	protected FlowGraph GetGraph(string graphPath)
	{
		if (this.flowGraph != null)
		{
			FlowScriptAssetLoadPolicy.ReleaseGraph(this.flowGraph);
			this.flowGraph = null;
		}
		this.flowGraph = FlowScriptAssetLoadPolicy.Load(graphPath);
		return this.flowGraph;
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000991C0 File Offset: 0x000973C0
	private void CustomExceptionHandler(AsyncOperationHandle handle, Exception exception)
	{
		string text = string.Empty;
		InvalidKeyException ex = exception as InvalidKeyException;
		if (ex != null)
		{
			string text2 = ex.Key as string;
			if (text2 != null && text2.StartsWith("VoiceOvers/"))
			{
				text = text2.Replace("VoiceOvers/", "");
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			Addressables.LogException(handle, exception);
			return;
		}
		Debug.LogWarning("Failed to load VoiceOver: " + text);
	}

	// Token: 0x04001CDF RID: 7391
	protected GlobalFlowScript.TerminateCallBack OnFinished;

	// Token: 0x04001CE1 RID: 7393
	protected FlowScriptController flowScriptController;

	// Token: 0x04001CE2 RID: 7394
	protected Blackboard blackboard;

	// Token: 0x04001CE3 RID: 7395
	protected FlowGraph flowGraph;

	// Token: 0x04001CE4 RID: 7396
	private static bool wasCustomExceptionHandlerRegistered;
}
