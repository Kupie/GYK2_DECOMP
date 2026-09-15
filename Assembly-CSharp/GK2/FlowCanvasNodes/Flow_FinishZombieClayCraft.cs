using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B55 RID: 2901
	[Name("Finish Zombie Clay Craft", 0)]
	[Category("Game/Zombie")]
	public class Flow_FinishZombieClayCraft : GKCustomFlowNode
	{
		// Token: 0x06004CE9 RID: 19689 RVA: 0x0016A45C File Offset: 0x0016865C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x0016A4AB File Offset: 0x001686AB
		private void DoAction(Flow flow)
		{
			Flow_FinishZombieClayCraft.FinishCraft(base.SelfWgoData, true);
			this.@out.Call(flow);
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x0016A4C8 File Offset: 0x001686C8
		public static void FinishCraft(WgoData wgoData, bool startAnother)
		{
			if (wgoData.Worker is ZombieWgoData)
			{
				if (wgoData.CraftComponent.CurrentCraftElement == null)
				{
					Debug.LogError("Null CurrentCraftElement in Flow_FinishZombieClayCraft");
					return;
				}
				MultiInventory multiInventory = new MultiInventory(wgoData.WorldZoneData, wgoData, false);
				List<Item> list = OutputItems.MakeOutput(wgoData.CraftComponent.CurrentCraftElement.Def.outputItems.MakePreOutput(wgoData, 0f));
				multiInventory.TryAddItemsPerOne(list);
				wgoData.CraftComponent.Clear();
				if (startAnother)
				{
					CraftParamsData craftParamsData = new CraftParamsData(wgoData.CraftComponent.AvailableCrafts[0].id, wgoData, CraftParamsData.CraftParamsType.Common, -1);
					craftParamsData.customRes.Set("wait_for_zombie_at_clay", 1f);
					craftParamsData.customRes.Set("ignore_handle_output", 1f);
					wgoData.CraftComponent.AddToQueue(new CraftElement(wgoData.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData), false, -1);
					return;
				}
			}
			else
			{
				Debug.LogError("No zombie linked to clay object");
			}
		}

		// Token: 0x04003DCE RID: 15822
		private FlowInput @in;

		// Token: 0x04003DCF RID: 15823
		private FlowOutput @out;
	}
}
