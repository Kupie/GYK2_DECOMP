using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B6E RID: 2926
	[Name("Add Item", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_AddItem : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004D38 RID: 19768 RVA: 0x0016C400 File Offset: 0x0016A600
		protected override void RegisterPorts()
		{
			if (!this.addToPlayer)
			{
				base.RegisterPorts();
			}
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeGameRes), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (!this.instanceAsInput)
			{
				this.itemId = base.AddValueInput<string>("itemId".CapitalizeFirst(), "");
				this.itemCount = base.AddValueInput<int>("itemCount".CapitalizeFirst(), "");
			}
			else
			{
				this.item = base.AddValueInput<Item>("item".CapitalizeFirst(), "");
			}
			this.itemNameOut = base.AddValueOutput<string>("itemId", () => this.itemId.value, "");
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x0016C4DC File Offset: 0x0016A6DC
		private void ChangeGameRes(Flow flow)
		{
			Item item = (this.instanceAsInput ? this.item.value : new Item(this.itemId.value, this.itemCount.value));
			if (this.addToPlayer)
			{
				Flow_AddItem.ItemGiveType itemGiveType = this.giveType;
				if (itemGiveType != Flow_AddItem.ItemGiveType.Add)
				{
					if (itemGiveType == Flow_AddItem.ItemGiveType.Remove)
					{
						MainGame.PlayerData.inventory.RemoveItemById(item.id, item.Count, null, null, false);
					}
				}
				else
				{
					MainGame.PlayerData.inventory.AddItemToInventory(item, null, false);
				}
			}
			else
			{
				WgoData wgoData = base.GetWgoData();
				if (wgoData != null)
				{
					Flow_AddItem.ItemGiveType itemGiveType = this.giveType;
					if (itemGiveType != Flow_AddItem.ItemGiveType.Add)
					{
						if (itemGiveType == Flow_AddItem.ItemGiveType.Remove)
						{
							wgoData.Inventory.RemoveItemById(item.id, item.Count, null, null, false);
						}
					}
					else
					{
						wgoData.Inventory.AddItemToInventory(item, null, false);
					}
				}
				else
				{
					Debug.LogError(string.Format("{0}: Tried to {1} Item to null WGO", "Flow_AddItem", this.giveType));
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004D3A RID: 19770 RVA: 0x0016C5E4 File Offset: 0x0016A7E4
		public override string name
		{
			get
			{
				return this.giveType.ToString() + " Item " + ((this.giveType == Flow_AddItem.ItemGiveType.Add) ? "to " : "from ") + (this.addToPlayer ? "Player" : "WgoData");
			}
		}

		// Token: 0x04003E0C RID: 15884
		[FlowNode.GatherPortsCallbackAttribute]
		public bool addToPlayer;

		// Token: 0x04003E0D RID: 15885
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_AddItem.ItemGiveType giveType;

		// Token: 0x04003E0E RID: 15886
		[FlowNode.GatherPortsCallbackAttribute]
		public bool instanceAsInput;

		// Token: 0x04003E0F RID: 15887
		private FlowInput @in;

		// Token: 0x04003E10 RID: 15888
		private FlowOutput @out;

		// Token: 0x04003E11 RID: 15889
		private ValueInput<WgoData> wgo;

		// Token: 0x04003E12 RID: 15890
		private ValueInput<string> itemId;

		// Token: 0x04003E13 RID: 15891
		private ValueInput<Item> item;

		// Token: 0x04003E14 RID: 15892
		private ValueInput<int> itemCount;

		// Token: 0x04003E15 RID: 15893
		private ValueOutput<string> itemNameOut;

		// Token: 0x02000B6F RID: 2927
		public enum ItemGiveType
		{
			// Token: 0x04003E17 RID: 15895
			Add,
			// Token: 0x04003E18 RID: 15896
			Remove
		}
	}
}
