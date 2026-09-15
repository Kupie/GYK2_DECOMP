using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B5A RID: 2906
	[Name("Finish Zombie Mine Craft", 0)]
	[Category("Game/Zombie")]
	public class Flow_FinishZombieMineCraft : GKCustomFlowNode
	{
		// Token: 0x06004CF9 RID: 19705 RVA: 0x0016AAC8 File Offset: 0x00168CC8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x0016AB17 File Offset: 0x00168D17
		private void DoAction(Flow flow)
		{
			Flow_FinishZombieMineCraft.FinishCraft(base.SelfWgoData, true);
			this.@out.Call(flow);
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x0016AB34 File Offset: 0x00168D34
		public static void FinishCraft(WgoData wgoData, bool startAnother)
		{
			if (wgoData.Worker is ZombieWgoData)
			{
				if (wgoData.CraftComponent.CurrentCraftElement == null)
				{
					Debug.LogError("Null CurrentCraftElement in Flow_FinishZombieMineCraft");
					return;
				}
				MultiInventory multiInventory = new MultiInventory(wgoData.WorldZoneData, wgoData, false);
				List<Item> list = OutputItems.MakeOutput(wgoData.CraftComponent.CurrentCraftElement.Def.outputItems.MakePreOutput(wgoData, 0f));
				multiInventory.TryAddItemsPerOne(list);
				wgoData.CraftComponent.Clear();
				if (startAnother)
				{
					CraftParamsData craftParamsData = new CraftParamsData(wgoData.CraftComponent.AvailableCrafts[0].id, wgoData, CraftParamsData.CraftParamsType.Common, -1);
					craftParamsData.customRes.Set("wait_for_zombie_at_mine", 1f);
					craftParamsData.customRes.Set("ignore_handle_output", 1f);
					wgoData.CraftComponent.AddToQueue(new CraftElement(wgoData.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData), false, -1);
					return;
				}
			}
			else
			{
				Debug.LogError("No zombie linked to mine object");
			}
		}

		// Token: 0x04003DD8 RID: 15832
		private FlowInput @in;

		// Token: 0x04003DD9 RID: 15833
		private FlowOutput @out;
	}
}
