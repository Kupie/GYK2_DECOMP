using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD2 RID: 3026
	[Name("IsMovingByMovementComponent", 0)]
	[Category("Game/Environment")]
	public class Flow_IsMovingByMovementComponent : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004E7E RID: 20094 RVA: 0x00171F9C File Offset: 0x0017019C
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.IsMovingByMovementComponent), "");
			this.trueOut = base.AddFlowOutput("<color=green>✔</color>", "");
			this.falseOut = base.AddFlowOutput("<color=red>✘</color>", "");
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x00172004 File Offset: 0x00170204
		private void IsMovingByMovementComponent(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				this.isMovingByMovementComponent = wgoData.MovementComponent.IsMoving;
			}
			else
			{
				Debug.LogError("Flow_IsMovingByMovementComponent: WGOData is null");
			}
			if (this.isMovingByMovementComponent)
			{
				this.trueOut.Call(flow);
				return;
			}
			this.falseOut.Call(flow);
		}

		// Token: 0x04003FA6 RID: 16294
		private FlowInput @in;

		// Token: 0x04003FA7 RID: 16295
		private FlowOutput trueOut;

		// Token: 0x04003FA8 RID: 16296
		private FlowOutput falseOut;

		// Token: 0x04003FA9 RID: 16297
		private bool isMovingByMovementComponent;
	}
}
