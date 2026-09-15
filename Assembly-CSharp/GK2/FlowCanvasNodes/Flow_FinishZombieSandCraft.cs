using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B60 RID: 2912
	[Name("Finish Zombie Sand Craft", 0)]
	[Category("Game/Zombie")]
	public class Flow_FinishZombieSandCraft : GKCustomFlowNode
	{
		// Token: 0x06004D0C RID: 19724 RVA: 0x0016B1B4 File Offset: 0x001693B4
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x0016B203 File Offset: 0x00169403
		private void DoAction(Flow flow)
		{
			Flow_FinishZombieSandCraft.FinishCraft(base.SelfWgoData, true);
			this.@out.Call(flow);
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x0016B220 File Offset: 0x00169420
		public static void FinishCraft(WgoData wgoData, bool startAnother)
		{
			if (wgoData.Worker is ZombieWgoData)
			{
				if (wgoData.CraftComponent.CurrentCraftElement == null)
				{
					Debug.LogError("Null CurrentCraftElement in Flow_FinishZombieSandCraft");
					return;
				}
				MultiInventory multiInventory = new MultiInventory(wgoData.WorldZoneData, wgoData, false);
				List<Item> list = OutputItems.MakeOutput(wgoData.CraftComponent.CurrentCraftElement.Def.outputItems.MakePreOutput(wgoData, 0f));
				multiInventory.TryAddItemsPerOne(list);
				wgoData.CraftComponent.Clear();
				if (startAnother)
				{
					CraftParamsData craftParamsData = new CraftParamsData(wgoData.CraftComponent.AvailableCrafts[0].id, wgoData, CraftParamsData.CraftParamsType.Common, -1);
					craftParamsData.customRes.Set("wait_for_zombie_at_sand", 1f);
					craftParamsData.customRes.Set("ignore_handle_output", 1f);
					wgoData.CraftComponent.AddToQueue(new CraftElement(wgoData.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData), false, -1);
					return;
				}
			}
			else
			{
				Debug.LogError("No zombie linked to sand object");
			}
		}

		// Token: 0x04003DE5 RID: 15845
		private FlowInput @in;

		// Token: 0x04003DE6 RID: 15846
		private FlowOutput @out;
	}
}
