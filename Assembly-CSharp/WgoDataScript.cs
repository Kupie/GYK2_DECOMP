using System;
using FlowCanvas;
using UnityEngine;

// Token: 0x020004F2 RID: 1266
public class WgoDataScript : CustomFlowScript
{
	// Token: 0x06002107 RID: 8455 RVA: 0x0009C0E8 File Offset: 0x0009A2E8
	public bool Run(WgoData wgoData, GameObject gameObject, string scriptName)
	{
		this.wgoData = wgoData;
		FlowGraph graph = base.GetGraph("Assets/AddressableAssets/VisualScripts/WGODataScripts/" + scriptName);
		if (graph == null)
		{
			return false;
		}
		base.Run(gameObject, graph, scriptName, null, FlowScriptLoadMode.DeserializeOnInit);
		return true;
	}

	// Token: 0x04001DA8 RID: 7592
	public const string WGO_DATA_SCRIPTS_RELATIVE_PATH = "Assets/AddressableAssets/VisualScripts/WGODataScripts/";

	// Token: 0x04001DA9 RID: 7593
	public WgoData wgoData;
}
