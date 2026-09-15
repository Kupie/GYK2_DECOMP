using System;
using System.Collections.Generic;
using System.Linq;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004C1 RID: 1217
[Name("Disable Workshop Conveyors", 0)]
[Category("Game/Environment")]
[Color("313c8f")]
public class Flow_DisableWorkshopConveyors : GKCustomFlowNode
{
	// Token: 0x0600205C RID: 8284 RVA: 0x00099444 File Offset: 0x00097644
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DisableWorkshopConveyorCells), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x00099494 File Offset: 0x00097694
	private void DisableWorkshopConveyorCells(Flow flow)
	{
		List<WgoData> wgoDataList = MainGame.WorldData.GetWgoDataList("conveyor_cell");
		if (wgoDataList != null && wgoDataList.Count > 0)
		{
			foreach (WgoData wgoData in wgoDataList.Where(delegate(WgoData cellData)
			{
				WorldZoneData worldZoneData = cellData.WorldZoneData;
				return ((worldZoneData != null) ? worldZoneData.id : null) != "conveyor";
			}))
			{
				wgoData.IsHidden = true;
			}
		}
		WgoData wgoDataByCustomTag = MainGame.WorldData.GetWgoDataByCustomTag("conveyor_chest_workshop_input");
		WgoData wgoDataByCustomTag2 = MainGame.WorldData.GetWgoDataByCustomTag("conveyor_chest_workshop_output");
		WgoData wgoDataByCustomTag3 = MainGame.WorldData.GetWgoDataByCustomTag("conveyor_workbench_workshop");
		if (wgoDataByCustomTag != null)
		{
			wgoDataByCustomTag.IsHidden = true;
		}
		if (wgoDataByCustomTag2 != null)
		{
			wgoDataByCustomTag2.IsHidden = true;
		}
		if (wgoDataByCustomTag3 != null)
		{
			wgoDataByCustomTag3.IsHidden = true;
		}
		this.@out.Call(flow);
	}

	// Token: 0x04001CFB RID: 7419
	private const string CONVEYOR_CELL_ID = "conveyor_cell";

	// Token: 0x04001CFC RID: 7420
	private const string WORKSHOP_CHEST_INPUT_CUSTOM_TAG = "conveyor_chest_workshop_input";

	// Token: 0x04001CFD RID: 7421
	private const string WORKSHOP_CHEST_OUTPUT_CUSTOM_TAG = "conveyor_chest_workshop_output";

	// Token: 0x04001CFE RID: 7422
	private const string WORKSHOP_WORKBENCH_CUSTOM_TAG = "conveyor_workbench_workshop";

	// Token: 0x04001CFF RID: 7423
	private FlowInput @in;

	// Token: 0x04001D00 RID: 7424
	private FlowOutput @out;
}
