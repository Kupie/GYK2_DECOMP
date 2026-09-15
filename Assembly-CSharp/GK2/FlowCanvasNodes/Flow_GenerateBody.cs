using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BAE RID: 2990
	[Name("Generate Body", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_GenerateBody : GKCustomFlowNode
	{
		// Token: 0x06004E06 RID: 19974 RVA: 0x0017045C File Offset: 0x0016E65C
		protected override void RegisterPorts()
		{
			if (!this.fromCurrentPlayerBodiesTier)
			{
				this.itemId = base.AddValueInput<string>("itemId", "");
			}
			this.neverAutoDestroy = base.AddValueInput<bool>("neverAutoDestroy", "");
			this.body = base.AddValueOutput<Item>("body", delegate
			{
				Item item;
				if (this.fromCurrentPlayerBodiesTier)
				{
					List<BodyDef> list = new List<BodyDef>();
					int resInt = MainGame.PlayerData.GetResInt("bodies_tier");
					foreach (BodyDef bodyDef in GameBalance.Me.bodyDefs)
					{
						if (bodyDef.tier == resInt)
						{
							list.Add(bodyDef);
						}
					}
					item = list.GetRandom<BodyDef>().GenerateItem();
				}
				else
				{
					item = GameBalance.Me.GetData<BodyDef>(this.itemId.value).GenerateItem();
				}
				if (this.neverAutoDestroy.value)
				{
					item.AddProperty<NeverAutoDestroyDropSerializedItemProperty>(new NeverAutoDestroyDropSerializedItemProperty());
				}
				return item;
			}, "");
		}

		// Token: 0x04003F1E RID: 16158
		[FlowNode.GatherPortsCallbackAttribute]
		public bool fromCurrentPlayerBodiesTier;

		// Token: 0x04003F1F RID: 16159
		private ValueInput<string> itemId;

		// Token: 0x04003F20 RID: 16160
		private ValueInput<bool> neverAutoDestroy;

		// Token: 0x04003F21 RID: 16161
		private ValueOutput<Item> body;
	}
}
