using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BCE RID: 3022
	[Name("Has Item", 0)]
	[Category("Game/Script")]
	[Color("70f1ff")]
	[Icon("FS", false, "")]
	public class Flow_HasItem : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004E6F RID: 20079 RVA: 0x00171A98 File Offset: 0x0016FC98
		protected override void RegisterPorts()
		{
			if (!this.isForPlayer)
			{
				base.RegisterPorts();
			}
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.HasItem), "");
			this.yes = base.AddFlowOutput("yes".CapitalizeFirst(), "");
			this.no = base.AddFlowOutput("no".CapitalizeFirst(), "");
			if (!this.isInventoryEmpty)
			{
				this.itemId = base.AddValueInput<string>("itemId".CapitalizeFirst(), "");
				this.itemCount = base.AddValueInput<int>("itemCount".CapitalizeFirst(), "");
			}
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x00171B50 File Offset: 0x0016FD50
		private void HasItem(Flow flow)
		{
			if (this.isInventoryEmpty)
			{
				if (this.isForPlayer)
				{
					if (MainGame.PlayerData.Inventory.Data.IsInventoryEmpty() || MainGame.PlayerData.toolBeltInventory.Data.IsInventoryEmpty())
					{
						this.yes.Call(flow);
						return;
					}
					this.no.Call(flow);
					return;
				}
				else
				{
					WgoData wgoData = base.GetWgoData();
					if (wgoData != null && wgoData.Inventory.Data.IsInventoryEmpty())
					{
						this.yes.Call(flow);
						return;
					}
					this.no.Call(flow);
					return;
				}
			}
			else if (this.isForPlayer)
			{
				if (MainGame.PlayerData.Inventory.Data.HasItemQuantityInInventory(this.itemId.value, this.itemCount.value) || MainGame.PlayerData.toolBeltInventory.Data.HasItemQuantityInInventory(this.itemId.value, this.itemCount.value))
				{
					this.yes.Call(flow);
					return;
				}
				this.no.Call(flow);
				return;
			}
			else
			{
				WgoData wgoData2 = base.GetWgoData();
				if (wgoData2 != null && wgoData2.Inventory.Data.HasItemQuantityInInventory(this.itemId.value, this.itemCount.value))
				{
					this.yes.Call(flow);
					return;
				}
				this.no.Call(flow);
				return;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06004E71 RID: 20081 RVA: 0x00171CB4 File Offset: 0x0016FEB4
		public override string name
		{
			get
			{
				return string.Concat(new string[]
				{
					this.isInventoryEmpty ? "Is" : "Has",
					" ",
					this.isForPlayer ? "Player" : "WgoData",
					" ",
					this.isInventoryEmpty ? "Inventory Empty" : "Item In Inventory"
				});
			}
		}

		// Token: 0x04003F94 RID: 16276
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isForPlayer;

		// Token: 0x04003F95 RID: 16277
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isInventoryEmpty;

		// Token: 0x04003F96 RID: 16278
		private FlowInput @in;

		// Token: 0x04003F97 RID: 16279
		private FlowOutput yes;

		// Token: 0x04003F98 RID: 16280
		private FlowOutput no;

		// Token: 0x04003F99 RID: 16281
		private ValueInput<string> itemId;

		// Token: 0x04003F9A RID: 16282
		private ValueInput<int> itemCount;
	}
}
