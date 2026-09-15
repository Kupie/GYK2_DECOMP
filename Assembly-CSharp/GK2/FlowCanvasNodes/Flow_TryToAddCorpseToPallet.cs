using System;
using System.Collections.Generic;
using System.Linq;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C28 RID: 3112
	[Name("Try To Add Corpse To Pallet", 0)]
	[Category("Game/Script")]
	[Color("70f1ff")]
	[Icon("FS", false, "")]
	public class Flow_TryToAddCorpseToPallet : GKCustomFlowNode
	{
		// Token: 0x06004F95 RID: 20373 RVA: 0x00177364 File Offset: 0x00175564
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.TryToAddCorpseToPallet), "");
			this.item = base.AddValueInput<Item>("item", "");
			this.item.skipSelfInstanceAssignment = false;
			this.yes = base.AddFlowOutput("yes".CapitalizeFirst(), "");
			this.no = base.AddFlowOutput("no".CapitalizeFirst(), "");
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001773F0 File Offset: 0x001755F0
		private void TryToAddCorpseToPallet(Flow flow)
		{
			List<WgoData> wgoDataListByGroup = MainGame.Instance.GameSave.worldData.GetWgoDataListByGroup("morgue_pallets");
			if (wgoDataListByGroup != null && wgoDataListByGroup.Count > 0 && wgoDataListByGroup.Any((WgoData data) => data.Inventory.AddItemToInventory(this.item.value, null, false)))
			{
				this.yes.Call(flow);
				return;
			}
			this.no.Call(flow);
		}

		// Token: 0x0400410F RID: 16655
		private FlowInput @in;

		// Token: 0x04004110 RID: 16656
		private FlowOutput yes;

		// Token: 0x04004111 RID: 16657
		private FlowOutput no;

		// Token: 0x04004112 RID: 16658
		private ValueInput<Item> item;
	}
}
