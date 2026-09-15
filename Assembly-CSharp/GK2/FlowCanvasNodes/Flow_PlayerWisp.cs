using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BE4 RID: 3044
	[Name("Player Wisp Target", 0)]
	[Category("Game/Environment")]
	public class Flow_PlayerWisp : GKCustomFlowNode
	{
		// Token: 0x06004EB5 RID: 20149 RVA: 0x00172CBC File Offset: 0x00170EBC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetTarget), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (this.targetType == PlayerWispControlType.GDPoint)
			{
				this.gdPoint = base.AddValueInput<GDPointData>("gdPoint", "");
			}
			if (this.targetType == PlayerWispControlType.Transform)
			{
				this.customTarget = base.AddValueInput<Transform>("customTarget", "");
			}
			if (this.targetType == PlayerWispControlType.Vector)
			{
				this.customVector = base.AddValueInput<Vector3>("customVector", "");
			}
			if (this.targetType == PlayerWispControlType.WgoData)
			{
				this.wgoDataInput = base.AddValueInput<WgoData>("wgoDataInput", "");
			}
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x00172D88 File Offset: 0x00170F88
		private void SetTarget(Flow flow)
		{
			switch (this.targetType)
			{
			case PlayerWispControlType.Player:
				MainGame.PlayerController.EnableWispControl();
				MainGame.PlayerController.WispController.ChangeTargetType(WispTargetType.Transform);
				MainGame.PlayerController.View.UpdateWispDirection();
				break;
			case PlayerWispControlType.Transform:
				MainGame.PlayerController.DisableWispControl();
				MainGame.PlayerController.WispController.ChangeTargetType(WispTargetType.Transform);
				MainGame.PlayerController.WispController.SetTargetTransform(this.customTarget.value);
				break;
			case PlayerWispControlType.GDPoint:
				MainGame.PlayerController.DisableWispControl();
				MainGame.PlayerController.WispController.ChangeTargetType(WispTargetType.GDPoint);
				MainGame.PlayerController.WispController.SetTargetGDPoint(this.gdPoint.value);
				break;
			case PlayerWispControlType.Vector:
				MainGame.PlayerController.DisableWispControl();
				MainGame.PlayerController.WispController.ChangeTargetType(WispTargetType.Vector);
				MainGame.PlayerController.WispController.SetTargetVector(this.customVector.value);
				break;
			case PlayerWispControlType.WgoData:
				MainGame.PlayerController.DisableWispControl();
				MainGame.PlayerController.WispController.ChangeTargetType(WispTargetType.WgoData);
				MainGame.PlayerController.WispController.SetTargetWgoData(this.wgoDataInput.value);
				break;
			}
			if (this.teleport)
			{
				MainGame.PlayerController.WispController.TeleportToTarget(true);
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003FDF RID: 16351
		[FlowNode.GatherPortsCallbackAttribute]
		public PlayerWispControlType targetType;

		// Token: 0x04003FE0 RID: 16352
		public bool teleport;

		// Token: 0x04003FE1 RID: 16353
		private FlowInput @in;

		// Token: 0x04003FE2 RID: 16354
		private FlowOutput @out;

		// Token: 0x04003FE3 RID: 16355
		private ValueInput<GDPointData> gdPoint;

		// Token: 0x04003FE4 RID: 16356
		private ValueInput<Transform> customTarget;

		// Token: 0x04003FE5 RID: 16357
		private ValueInput<Vector3> customVector;

		// Token: 0x04003FE6 RID: 16358
		private ValueInput<WgoData> wgoDataInput;
	}
}
