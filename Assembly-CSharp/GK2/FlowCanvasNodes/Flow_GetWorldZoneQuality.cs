using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BC3 RID: 3011
	[Name("Get World Zone Quality", 0)]
	[Category("Game/World Zones")]
	public class Flow_GetWorldZoneQuality : GKCustomFlowNode
	{
		// Token: 0x06004E48 RID: 20040 RVA: 0x001711B5 File Offset: 0x0016F3B5
		protected override void RegisterPorts()
		{
			this.worldZoneId = base.AddValueInput<string>("worldZoneId".CapitalizeFirst(), "");
			this.quality = base.AddValueOutput<int>("quality", delegate
			{
				WorldZoneData worldZoneDataById = MainGame.WorldData.GetWorldZoneDataById(this.worldZoneId.value);
				if (worldZoneDataById == null)
				{
					Debug.LogError("Can't find world zone with id " + this.worldZoneId.value);
					return 0;
				}
				return (int)worldZoneDataById.GetTotalQuality();
			}, "");
		}

		// Token: 0x04003F6C RID: 16236
		private ValueInput<string> worldZoneId;

		// Token: 0x04003F6D RID: 16237
		private ValueOutput<int> quality;
	}
}
