using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B7A RID: 2938
	[Name("Game/Camera Fly", 0)]
	[Category("Game/Camera")]
	[Color("8a8a8a")]
	public class Flow_CameraFly : GKCustomFlowNode
	{
		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06004D5F RID: 19807 RVA: 0x0016D042 File Offset: 0x0016B242
		public override string name
		{
			get
			{
				return "Camera Fly" + (this.returnToPlayer ? " to Player" : (this.useWgoDataAsTarget ? " to WgoData" : (this.useGdPointAsTarget ? " to GDPoint" : "")));
			}
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x0016D080 File Offset: 0x0016B280
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.CameraFly), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onFlyComplete = base.AddFlowOutput("onFlyComplete".CapitalizeFirst(), "");
			if (!this.returnToPlayer)
			{
				if (this.useWgoDataAsTarget)
				{
					this.targetWgoData = base.AddValueInput<WgoData>("targetWgoData", "");
				}
				else if (this.useGdPointAsTarget)
				{
					this.targetGdPoint = base.AddValueInput<GDPointData>("targetGdPoint", "");
				}
				else if (this.setPosition)
				{
					this.targetPosition = base.AddValueInput<Vector3>("targetPosition", "");
				}
				else
				{
					this.targetTransform = base.AddValueInput<Transform>("targetTransform", "");
				}
			}
			this.duration = base.AddValueInput<float>("duration", "");
			this.duration.SetDefaultAndSerializedValue(1f);
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x0016D190 File Offset: 0x0016B390
		private void CameraFly(Flow flow)
		{
			CameraController cameraController = CameraSystem.Instance.GetCameraController(global::CameraType.Main);
			if (this.returnToPlayer || (!this.setPosition && !this.useGdPointAsTarget && !this.useWgoDataAsTarget))
			{
				cameraController.SetTarget((!this.returnToPlayer) ? this.targetTransform.value : MainGame.PlayerController.View.transform, this.duration.value, delegate
				{
					this.onFlyComplete.Call(flow);
				});
			}
			else if (this.useWgoDataAsTarget)
			{
				WgoData value = this.targetWgoData.value;
				Wgo wgo = ((value != null) ? GameScene.GetWgoViewGlobal(value.UniqueId) : null);
				if (wgo != null)
				{
					cameraController.SetTarget(wgo.transform, this.duration.value, delegate
					{
						this.onFlyComplete.Call(flow);
					});
				}
				else
				{
					Vector3 vector = ((value != null) ? value.Position : Vector3.zero);
					cameraController.SetPosition(vector, this.duration.value, delegate
					{
						this.onFlyComplete.Call(flow);
					});
				}
			}
			else
			{
				Vector3 vector2 = (this.useGdPointAsTarget ? ((this.targetGdPoint.value != null) ? this.targetGdPoint.value.Position : Vector3.zero) : this.targetPosition.value);
				cameraController.SetPosition(vector2, this.duration.value, delegate
				{
					this.onFlyComplete.Call(flow);
				});
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003E42 RID: 15938
		[FlowNode.GatherPortsCallbackAttribute]
		public bool returnToPlayer;

		// Token: 0x04003E43 RID: 15939
		[FlowNode.GatherPortsCallbackAttribute]
		public bool setPosition;

		// Token: 0x04003E44 RID: 15940
		[FlowNode.GatherPortsCallbackAttribute]
		public bool useGdPointAsTarget;

		// Token: 0x04003E45 RID: 15941
		[FlowNode.GatherPortsCallbackAttribute]
		public bool useWgoDataAsTarget;

		// Token: 0x04003E46 RID: 15942
		private FlowInput @in;

		// Token: 0x04003E47 RID: 15943
		private FlowOutput @out;

		// Token: 0x04003E48 RID: 15944
		private FlowOutput onFlyComplete;

		// Token: 0x04003E49 RID: 15945
		private ValueInput<Transform> targetTransform;

		// Token: 0x04003E4A RID: 15946
		private ValueInput<Vector3> targetPosition;

		// Token: 0x04003E4B RID: 15947
		private ValueInput<GDPointData> targetGdPoint;

		// Token: 0x04003E4C RID: 15948
		private ValueInput<WgoData> targetWgoData;

		// Token: 0x04003E4D RID: 15949
		private ValueInput<float> duration;
	}
}
