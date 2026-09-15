using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B8F RID: 2959
	[Name("Drop Wgo Inventory", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_DropInventory : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004DA6 RID: 19878 RVA: 0x0016E2A4 File Offset: 0x0016C4A4
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Drop), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (this.shouldFly)
			{
				this.gdPointDataFlyTarget = base.AddValueInput<GDPointData>("gdPointDataFlyTarget", "");
			}
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x0016E318 File Offset: 0x0016C518
		private void Drop(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			Vector3 position = wgoData.Position;
			string worldId = wgoData.WorldId;
			foreach (Item item in wgoData.Inventory.Data.Inventory)
			{
				List<Item> list = new List<Item>();
				MainGame.Instance.dropSystem.DropItem(item, worldId, position, list);
				GDPointData gdpointData;
				if (this.shouldFly && base.TryGetParamValue<GDPointData>(this.gdPointDataFlyTarget, out gdpointData))
				{
					foreach (Item item2 in list)
					{
						DropView dropView = null;
						foreach (GameScene gameScene in LazySingleton<GameSceneManager>.Instance.LoadedGameScenes)
						{
							dropView = gameScene.GetDropView(item2);
							if (dropView != null)
							{
								break;
							}
						}
						if (dropView != null)
						{
							dropView.MoveToCustomPosition(gdpointData.Position);
						}
					}
				}
			}
			wgoData.Inventory.Data.RemoveAllItems();
			this.@out.Call(flow);
		}

		// Token: 0x04003E93 RID: 16019
		[FlowNode.GatherPortsCallbackAttribute]
		public bool shouldFly;

		// Token: 0x04003E94 RID: 16020
		private FlowInput @in;

		// Token: 0x04003E95 RID: 16021
		private FlowOutput @out;

		// Token: 0x04003E96 RID: 16022
		private ValueInput<GDPointData> gdPointDataFlyTarget;
	}
}
