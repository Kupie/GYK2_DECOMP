using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B74 RID: 2932
	[Name("Add WGO To Sim Group", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	public class Flow_AddWgoToSimGroup : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004D48 RID: 19784 RVA: 0x0016C91C File Offset: 0x0016AB1C
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetInteractableState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (this.findGroupById)
			{
				this.groupId = base.AddValueInput<string>("groupId?", "");
			}
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0016C990 File Offset: 0x0016AB90
		private void SetInteractableState(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				NPCLifeSimulator npcLifeSimulator = MainGame.Instance.npcLifeSimulator;
				if (this.remove)
				{
					npcLifeSimulator.RemoveWgoFromGroup(wgoData);
				}
				else if (this.findGroupById)
				{
					npcLifeSimulator.AddWgoToGroup(wgoData, this.groupId.value);
				}
				else
				{
					npcLifeSimulator.AddWgoToGroupFromBalance(wgoData);
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06004D4A RID: 19786 RVA: 0x0016C9F4 File Offset: 0x0016ABF4
		public override string name
		{
			get
			{
				return string.Concat(new string[]
				{
					this.remove ? "Remove" : "Add",
					" WGO",
					this.remove ? " From" : " To",
					" Sim Group",
					(!this.remove && !this.findGroupById) ? " From Balance" : ""
				});
			}
		}

		// Token: 0x04003E28 RID: 15912
		[FlowNode.GatherPortsCallbackAttribute]
		public bool remove;

		// Token: 0x04003E29 RID: 15913
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("remove", 0)]
		public bool findGroupById;

		// Token: 0x04003E2A RID: 15914
		private FlowInput @in;

		// Token: 0x04003E2B RID: 15915
		private FlowOutput @out;

		// Token: 0x04003E2C RID: 15916
		private ValueInput<string> groupId;
	}
}
