using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B65 RID: 2917
	[Name("Finish Zombie Sawmill Craft", 0)]
	[Category("Game/Zombie")]
	public class Flow_FinishZombieSawmillCraft : GKCustomFlowNode
	{
		// Token: 0x06004D1C RID: 19740 RVA: 0x0016B820 File Offset: 0x00169A20
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x0016B86F File Offset: 0x00169A6F
		private void DoAction(Flow flow)
		{
			Flow_FinishZombieSawmillCraft.FinishCraft(base.SelfWgoData, true);
			this.@out.Call(flow);
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x0016B88C File Offset: 0x00169A8C
		public static void FinishCraft(WgoData wgoData, bool startAnother)
		{
			if (wgoData.Worker is ZombieWgoData)
			{
				if (wgoData.CraftComponent.CurrentCraftElement == null)
				{
					Debug.LogError("Null CurrentCraftElement in Flow_FinishZombieSawmillCraft");
					return;
				}
				Item item = new Item(wgoData.CraftComponent.CurrentCraftElement.Def.outputItems.chanceOutputItems[0].id, 1);
				for (int i = 0; i < wgoData.WorldZoneData.MultiInventoryWgoDatas.Count; i++)
				{
					WgoData wgoData2 = wgoData.WorldZoneData.MultiInventoryWgoDatas[i];
					if (wgoData2.id == "sawmill_wood_container" && wgoData2.Inventory.Data.InventoryFillSize < wgoData2.Inventory.Data.InventorySize)
					{
						wgoData2.Inventory.AddItemToInventory(item, null, false);
						break;
					}
				}
				MultiInventory multiInventory = new MultiInventory(wgoData.WorldZoneData, wgoData, false);
				List<Item> list = OutputItems.MakeOutput(wgoData.CraftComponent.CurrentCraftElement.Def.outputItems.MakePreOutput(wgoData, 0f));
				for (int j = 1; j < list.Count; j++)
				{
					multiInventory.TryAddItem(list[j]);
				}
				wgoData.CraftComponent.Clear();
				if (startAnother)
				{
					CraftParamsData craftParamsData = new CraftParamsData(wgoData.CraftComponent.AvailableCrafts[0].id, wgoData, CraftParamsData.CraftParamsType.Common, -1);
					craftParamsData.customRes.Set("wait_for_zombie_at_sawmill", 1f);
					craftParamsData.customRes.Set("ignore_handle_output", 1f);
					wgoData.CraftComponent.AddToQueue(new CraftElement(wgoData.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData), false, -1);
					return;
				}
			}
			else
			{
				Debug.LogError("No zombie linked to sawmill object");
			}
		}

		// Token: 0x04003DEF RID: 15855
		private FlowInput @in;

		// Token: 0x04003DF0 RID: 15856
		private FlowOutput @out;
	}
}
