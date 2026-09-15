using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BF7 RID: 3063
	[Name("Set Direction", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_SetDirection : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06004EF9 RID: 20217 RVA: 0x00174562 File Offset: 0x00172762
		public override string name
		{
			get
			{
				return "Set Direction " + (this.isForPlayer ? "Player" : "WgoData");
			}
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x00174584 File Offset: 0x00172784
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetDirection), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (!this.isForPlayer)
			{
				base.RegisterPorts();
			}
			if (!this.setDirectionToPos)
			{
				this.direction = base.AddValueInput<Direction>("direction", "");
				return;
			}
			this.lookAtPos = base.AddValueInput<Vector3>("lookAtPos", "");
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x00174618 File Offset: 0x00172818
		private void SetDirection(Flow flow)
		{
			if (this.isForPlayer)
			{
				if (!this.setDirectionToPos)
				{
					MainGame.PlayerController.View.PlayerAnimation.SetDirection(this.direction.value);
				}
				else
				{
					MainGame.PlayerController.View.PlayerAnimation.SetDirection(new Vector2(this.lookAtPos.value.x, this.lookAtPos.value.z));
				}
			}
			else
			{
				WgoData wgoData = base.GetWgoData();
				if (wgoData != null)
				{
					wgoData.direction.Value = ((!this.setDirectionToPos) ? this.direction.value.ConvertToVector2XZ() : new Vector2(this.lookAtPos.value.x, this.lookAtPos.value.z));
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x0400403C RID: 16444
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isForPlayer;

		// Token: 0x0400403D RID: 16445
		[FlowNode.GatherPortsCallbackAttribute]
		public bool setDirectionToPos;

		// Token: 0x0400403E RID: 16446
		private FlowInput @in;

		// Token: 0x0400403F RID: 16447
		private FlowOutput @out;

		// Token: 0x04004040 RID: 16448
		private ValueInput<Direction> direction;

		// Token: 0x04004041 RID: 16449
		private ValueInput<Vector3> lookAtPos;
	}
}
