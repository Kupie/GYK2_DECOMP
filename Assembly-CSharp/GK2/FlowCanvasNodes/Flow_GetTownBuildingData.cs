using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C35 RID: 3125
	[Name("Get Town Building Data For Character", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	public class Flow_GetTownBuildingData : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06004FBF RID: 20415 RVA: 0x00177C0D File Offset: 0x00175E0D
		private WgoData LinkedWgoData
		{
			get
			{
				return MainGame.WorldData.GetWgoData(base.GetWgoData().LinkedToTownBuildingUniqueId);
			}
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x00177C24 File Offset: 0x00175E24
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.tentWgoData = base.AddValueOutput<WgoData>("tentWgoData".CapitalizeFirst(), () => this.LinkedWgoData, "");
			this.gdPointTent = base.AddValueOutput<GDPointData>("gdPointTent".CapitalizeFirst(), () => MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.LinkedWgoData.TownBuildingWgoComponent.SceneConfiguration.gdPointTent), "");
			this.gdPointHomeOutside = base.AddValueOutput<GDPointData>("gdPointHomeOutside".CapitalizeFirst(), () => MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.LinkedWgoData.TownBuildingWgoComponent.SceneConfiguration.gdPointHomeOutside), "");
			this.gdPointHomeInside = base.AddValueOutput<GDPointData>("gdPointHomeInside".CapitalizeFirst(), () => MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.LinkedWgoData.TownBuildingWgoComponent.SceneConfiguration.gdPointHomeInside), "");
			this.gdPointPlayer = base.AddValueOutput<GDPointData>("gdPointPlayer".CapitalizeFirst(), () => MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(Flow_TownBuildingConsts.GetPlayerTpDuringFadeGdPointId(this.LinkedWgoData.TownBuildingWgoComponent.SceneConfiguration.gdPointTent)), "");
		}

		// Token: 0x04004134 RID: 16692
		private ValueOutput<WgoData> tentWgoData;

		// Token: 0x04004135 RID: 16693
		private ValueOutput<GDPointData> gdPointTent;

		// Token: 0x04004136 RID: 16694
		private ValueOutput<GDPointData> gdPointHomeOutside;

		// Token: 0x04004137 RID: 16695
		private ValueOutput<GDPointData> gdPointHomeInside;

		// Token: 0x04004138 RID: 16696
		private ValueOutput<GDPointData> gdPointPlayer;
	}
}
