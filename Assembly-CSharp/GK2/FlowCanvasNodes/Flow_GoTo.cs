using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BC6 RID: 3014
	[Name("GoTo", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	[global::ParadoxNotion.Design.Icon("Sequencer", false, "")]
	public class Flow_GoTo : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004E51 RID: 20049 RVA: 0x00099840 File Offset: 0x00097A40
		public override int MinWidth
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06004E52 RID: 20050 RVA: 0x00171306 File Offset: 0x0016F506
		public override string name
		{
			get
			{
				if (!this.movePlayer)
				{
					return "GoTo Wgo";
				}
				return "GoTo Player";
			}
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x0017131C File Offset: 0x0016F51C
		protected override void RegisterPorts()
		{
			if (!this.movePlayer)
			{
				base.RegisterPorts();
			}
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.GoTo), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onFinish = base.AddFlowOutput("onFinish".CapitalizeFirst(), "");
			this.gdPointDataOutput = base.AddValueOutput<GDPointData>("GDPointData", () => this.gdPointData.value, "");
			this.speed = base.AddValueInput<float>("speed".CapitalizeFirst(), "");
			if (!this.useGdDataInsteadId)
			{
				this.gdPointId = base.AddValueInput<string>("gdPointId", "");
			}
			else
			{
				this.gdPointData = base.AddValueInput<GDPointData>("gdPointData", "");
			}
			this.navigation = base.AddValueInput<MovementType>("navigation", "");
			this.navigation.serializedValue = MovementType.GDGraph;
			this.fireEventOnFinish = base.AddValueInput<string>("fireEventOnFinish", "");
			this.speed.SetDefaultAndSerializedValue(1.5f);
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x00171458 File Offset: 0x0016F658
		protected virtual void GoTo(Flow flow)
		{
			GDPointData gdpointData = ((!this.useGdDataInsteadId) ? MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.gdPointId.value) : this.gdPointData.value);
			WgoData wgoData = ((!this.movePlayer) ? base.GetWgoData() : null);
			MovementComponent movementComponent = ((!this.movePlayer) ? wgoData.MovementComponent : MainGame.PlayerController.MovementComponent);
			Seeker seeker = ((!this.movePlayer) ? null : MainGame.PlayerController.PlayerLocalAreaMovement.Seeker);
			string text = ((!this.movePlayer) ? wgoData.WorldId : MainGame.PlayerData.currentGameSceneId);
			this.BindPathLengthCallback(wgoData, movementComponent);
			MovementComponent.StartPathResult startPathResult;
			if (this.navigation.value == MovementType.WorldZone)
			{
				startPathResult = movementComponent.StartPath(gdpointData.Position, this.movePlayer ? MainGame.PlayerData.CurrentWorldZoneData.navigationGraph : wgoData.WorldZoneData.navigationGraph, text, this.speed.value, this.fireEventOnFinish.value ?? string.Empty, delegate
				{
					this.onFinish.Call(flow);
				}, MovementComponent.DestinationType.Position);
			}
			else
			{
				startPathResult = movementComponent.StartPath(gdpointData.Position, text, gdpointData.GameSceneDataId, this.navigation.value, this.speed.value, this.fireEventOnFinish.value ?? string.Empty, delegate
				{
					this.onFinish.Call(flow);
				}, seeker, MovementComponent.DestinationType.Position);
			}
			this.HandlePathLengthStartResult(wgoData, movementComponent, startPathResult);
			this.@out.Call(flow);
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x001715FC File Offset: 0x0016F7FC
		private void BindPathLengthCallback(WgoData wgoData, MovementComponent movementComponent)
		{
			if (!this.returnPathLength || this.movePlayer || wgoData == null)
			{
				return;
			}
			movementComponent.SetOnPathLengthReady(delegate(float length)
			{
				wgoData.SetGameRes("lastGoToPathLength", length);
			});
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x00171641 File Offset: 0x0016F841
		private void HandlePathLengthStartResult(WgoData wgoData, MovementComponent movementComponent, MovementComponent.StartPathResult result)
		{
			if (!this.returnPathLength || this.movePlayer || wgoData == null)
			{
				return;
			}
			if (result == MovementComponent.StartPathResult.AlreadyAtDestinationPoint)
			{
				wgoData.SetGameRes("lastGoToPathLength", 0f);
				movementComponent.SetOnPathLengthReady(null);
				return;
			}
			if (result != MovementComponent.StartPathResult.Started)
			{
				movementComponent.SetOnPathLengthReady(null);
			}
		}

		// Token: 0x04003F72 RID: 16242
		[FlowNode.GatherPortsCallbackAttribute]
		public bool movePlayer;

		// Token: 0x04003F73 RID: 16243
		[FlowNode.GatherPortsCallbackAttribute]
		public bool useGdDataInsteadId;

		// Token: 0x04003F74 RID: 16244
		[FlowNode.GatherPortsCallbackAttribute]
		[InspectorName("Return path length")]
		[ShowIf("movePlayer", 0)]
		public bool returnPathLength;

		// Token: 0x04003F75 RID: 16245
		protected FlowInput @in;

		// Token: 0x04003F76 RID: 16246
		protected FlowOutput @out;

		// Token: 0x04003F77 RID: 16247
		protected FlowOutput onFinish;

		// Token: 0x04003F78 RID: 16248
		protected ValueOutput<GDPointData> gdPointDataOutput;

		// Token: 0x04003F79 RID: 16249
		protected ValueInput<float> speed;

		// Token: 0x04003F7A RID: 16250
		protected ValueInput<string> gdPointId;

		// Token: 0x04003F7B RID: 16251
		protected ValueInput<GDPointData> gdPointData;

		// Token: 0x04003F7C RID: 16252
		protected ValueInput<MovementType> navigation;

		// Token: 0x04003F7D RID: 16253
		protected ValueInput<string> fireEventOnFinish;
	}
}
