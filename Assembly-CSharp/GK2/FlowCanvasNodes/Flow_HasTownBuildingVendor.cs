using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C37 RID: 3127
	[Name("Has Town Building Vendor", 0)]
	public class Flow_HasTownBuildingVendor : GKCustomFlowNode
	{
		// Token: 0x06004FCB RID: 20427 RVA: 0x00177F48 File Offset: 0x00176148
		protected override void RegisterPorts()
		{
			this.hasLevelUp = base.AddValueOutput<bool>("hasLevelUp".CapitalizeFirst(), () => MainGame.WorldData.GetWgoData(base.SelfWgoData.LinkedToTownBuildingUniqueId).TownBuildingWgoComponent.HasVendor, "");
		}

		// Token: 0x0400413B RID: 16699
		private ValueOutput<bool> hasLevelUp;
	}
}
