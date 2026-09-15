using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BCB RID: 3019
	[Name("GoTo Wisp", 0)]
	[Category("Game/Environment")]
	public class Flow_GoToWisp : Flow_GoTo
	{
		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06004E66 RID: 20070 RVA: 0x00171874 File Offset: 0x0016FA74
		public override string name
		{
			get
			{
				return "GoTo Wisp";
			}
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x0017187B File Offset: 0x0016FA7B
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.wispInput = base.AddValueInput<WispController>("wispInput".CapitalizeFirst(), "");
			this.wgoDataInput = null;
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x001718A8 File Offset: 0x0016FAA8
		protected override void GoTo(Flow flow)
		{
			WispController wispController = this.wispInput.value;
			if (wispController == null)
			{
				wispController = MainGame.PlayerController.WispController;
			}
			WgoData wispWgoData = wispController.GetWispWgoData();
			GDPointData gdpointData = ((!this.useGdDataInsteadId) ? MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.gdPointId.value) : this.gdPointData.value);
			if (wispWgoData.WorldId != gdpointData.GameSceneDataId)
			{
				MainGame.Instance.GameSave.worldData.MoveWgoDataToAnotherGameScene(wispWgoData, gdpointData.GameSceneDataId);
			}
			wispWgoData.MovementComponent.StartPath(gdpointData.Position, wispWgoData.WorldId, gdpointData.GameSceneDataId, this.navigation.value, this.speed.value, this.fireEventOnFinish.value, delegate
			{
				this.onFinish.Call(flow);
			}, null, MovementComponent.DestinationType.Position);
			wispController.ChangeTargetType(WispTargetType.WgoData);
			wispController.SetTargetWgoData(wispWgoData);
			this.@out.Call(flow);
		}

		// Token: 0x04003F8D RID: 16269
		private ValueInput<WispController> wispInput;
	}
}
