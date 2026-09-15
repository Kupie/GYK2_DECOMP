using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BED RID: 3053
	[Name("Remove Items From WgoData", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_RemoveItemsFromWgoData : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004ED4 RID: 20180 RVA: 0x00173B70 File Offset: 0x00171D70
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.RemoveItems), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			base.RegisterPorts();
			this.itemId = base.AddValueInput<string>("itemId", "");
			this.removedItems = base.AddValueOutput<List<Item>>("removedItems", () => this.removedItemsList, "");
			this.removedCount = base.AddValueOutput<int>("removedCount", () => this.removedCountValue, "");
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x00173C20 File Offset: 0x00171E20
		private void RemoveItems(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			this.removedItemsList = ((wgoData != null) ? wgoData.Inventory.RemoveItemById(this.itemId.value, -1, null, null, false) : null);
			List<Item> list = this.removedItemsList;
			this.removedCountValue = ((list != null) ? list.Count : 0);
			if (this.splitItemsByOne && this.removedCountValue > 0)
			{
				List<Item> list2 = new List<Item>(this.removedItemsList);
				this.removedItemsList.Clear();
				this.removedCountValue = 0;
				foreach (Item item in list2)
				{
					int count = item.Count;
					for (int i = 0; i < count; i++)
					{
						this.removedItemsList.Add(item.Split(1, false));
						this.removedCountValue++;
					}
				}
			}
			string text = "Flow_RemoveItemsFromWgoData: {0}, count: {1}";
			object value = this.itemId.value;
			List<Item> list3 = this.removedItemsList;
			Debug.Log(string.Format(text, value, (list3 != null) ? new int?(list3.Count) : null));
			this.@out.Call(flow);
		}

		// Token: 0x04004016 RID: 16406
		[FlowNode.GatherPortsCallbackAttribute]
		public bool splitItemsByOne;

		// Token: 0x04004017 RID: 16407
		private FlowInput @in;

		// Token: 0x04004018 RID: 16408
		private FlowOutput @out;

		// Token: 0x04004019 RID: 16409
		private ValueInput<string> itemId;

		// Token: 0x0400401A RID: 16410
		private ValueOutput<List<Item>> removedItems;

		// Token: 0x0400401B RID: 16411
		private ValueOutput<int> removedCount;

		// Token: 0x0400401C RID: 16412
		private List<Item> removedItemsList;

		// Token: 0x0400401D RID: 16413
		private int removedCountValue;
	}
}
