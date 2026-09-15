using System;
using FlowCanvas;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public abstract class GKCustomFlowNodeWithWgoData : GKCustomFlowNode
{
	// Token: 0x060020EC RID: 8428 RVA: 0x0009BC50 File Offset: 0x00099E50
	protected override void RegisterPorts()
	{
		if (this.findById)
		{
			this.wgoId = base.AddValueInput<string>("WgoData", "");
		}
		else
		{
			this.wgoDataInput = base.AddValueInput<WgoData>("WgoData", "");
		}
		this.wgoOutput = base.AddValueOutput<WgoData>("WgoData", () => this.GetWgoData(), "");
	}

	// Token: 0x060020ED RID: 8429 RVA: 0x0009BCB5 File Offset: 0x00099EB5
	protected WgoData GetWgoData()
	{
		if (!this.findById)
		{
			return base.WgoDataParamOrSelf(this.wgoDataInput);
		}
		return MainGame.Instance.GameSave.worldData.GetWgoData(this.wgoId.value);
	}

	// Token: 0x04001D99 RID: 7577
	[FlowNode.GatherPortsCallbackAttribute]
	[InspectorName("Use ID for WGO link")]
	public bool findById;

	// Token: 0x04001D9A RID: 7578
	protected ValueInput<string> wgoId;

	// Token: 0x04001D9B RID: 7579
	protected ValueInput<WgoData> wgoDataInput;

	// Token: 0x04001D9C RID: 7580
	private ValueOutput<WgoData> wgoOutput;
}
