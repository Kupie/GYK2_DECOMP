using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C1A RID: 3098
	[Name("Stop GoTo", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	[Icon("Halt", false, "")]
	public class Flow_StopGoTo : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06004F69 RID: 20329 RVA: 0x00099840 File Offset: 0x00097A40
		public override int MinWidth
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06004F6A RID: 20330 RVA: 0x00176544 File Offset: 0x00174744
		public override string name
		{
			get
			{
				if (!this.stopPlayer)
				{
					return "Stop GoTo Wgo";
				}
				if (!this.goTo)
				{
					return "Stop Player";
				}
				return "Stop GoTo Player";
			}
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x00176568 File Offset: 0x00174768
		protected override void RegisterPorts()
		{
			if (!this.stopPlayer)
			{
				base.RegisterPorts();
			}
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Stop), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x001765C8 File Offset: 0x001747C8
		protected virtual void Stop(Flow flow)
		{
			WgoData wgoData = ((!this.stopPlayer) ? base.GetWgoData() : null);
			MovementComponent movementComponent = ((!this.stopPlayer) ? ((wgoData != null) ? wgoData.MovementComponent : null) : MainGame.PlayerController.MovementComponent);
			if (this.stopPlayer && !this.goTo)
			{
				MainGame.PlayerController.PhysicalBody.StopMoving();
			}
			else if (movementComponent != null && movementComponent.IsMoving)
			{
				movementComponent.ForceStop();
			}
			this.@out.Call(flow);
		}

		// Token: 0x040040CF RID: 16591
		[FlowNode.GatherPortsCallbackAttribute]
		public bool stopPlayer;

		// Token: 0x040040D0 RID: 16592
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("stopPlayer", 1)]
		public bool goTo = true;

		// Token: 0x040040D1 RID: 16593
		protected FlowInput @in;

		// Token: 0x040040D2 RID: 16594
		protected FlowOutput @out;
	}
}
