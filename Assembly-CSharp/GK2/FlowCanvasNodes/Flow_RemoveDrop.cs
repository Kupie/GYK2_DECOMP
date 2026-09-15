using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BEC RID: 3052
	[Name("Remove Drop", 0)]
	[Category("Game/Item")]
	[Description("Finds and removes an item dropped in the world by its reference.")]
	[Color("FFFFFF")]
	public class Flow_RemoveDrop : GKCustomFlowNode
	{
		// Token: 0x06004ED1 RID: 20177 RVA: 0x001739D4 File Offset: 0x00171BD4
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Remove), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.item = base.AddValueInput<Item>("item", "");
			this.worldId = base.AddValueInput<string>("worldId", "");
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x00173A50 File Offset: 0x00171C50
		private void Remove(Flow flow)
		{
			Item value = this.item.value;
			if (value != null && !value.IsEmpty)
			{
				string value2 = this.worldId.value;
				DropData dropData = null;
				foreach (GameSceneData gameSceneData in MainGame.Instance.GameSave.worldData.gameSceneDataList)
				{
					if (string.IsNullOrEmpty(value2) || !(gameSceneData.id != value2))
					{
						foreach (DropData dropData2 in gameSceneData.droppedItems)
						{
							if (dropData2.UniqueId == value.UniqueId)
							{
								dropData = dropData2;
								break;
							}
						}
						if (dropData != null)
						{
							break;
						}
					}
				}
				if (dropData != null)
				{
					MainGame.Instance.dropSystem.RemoveDrop(dropData, dropData.WorldId);
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x04004012 RID: 16402
		private FlowInput @in;

		// Token: 0x04004013 RID: 16403
		private FlowOutput @out;

		// Token: 0x04004014 RID: 16404
		private ValueInput<Item> item;

		// Token: 0x04004015 RID: 16405
		private ValueInput<string> worldId;
	}
}
