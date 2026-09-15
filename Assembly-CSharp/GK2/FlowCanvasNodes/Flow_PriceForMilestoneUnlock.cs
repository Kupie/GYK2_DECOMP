using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BE8 RID: 3048
	[Name("Price For Milestone Unlock", 0)]
	[Category("Game/UI")]
	public class Flow_PriceForMilestoneUnlock : GKCustomFlowNode
	{
		// Token: 0x06004EC6 RID: 20166 RVA: 0x0017370B File Offset: 0x0017190B
		protected override void RegisterPorts()
		{
			this.output = base.AddValueOutput<SmartRes>("output".CapitalizeFirst(), delegate
			{
				SmartRes smartRes = new SmartRes();
				foreach (Item item in base.SelfWgoData.Inventory.Data.Inventory)
				{
					smartRes.items.Add(new ItemCount(item));
				}
				return smartRes;
			}, "");
		}

		// Token: 0x04004006 RID: 16390
		private ValueOutput<SmartRes> output;
	}
}
