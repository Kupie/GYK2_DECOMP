using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B90 RID: 2960
	[Name("Drop Item", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_DropItem : GKCustomFlowNode
	{
		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06004DA9 RID: 19881 RVA: 0x0016E48C File Offset: 0x0016C68C
		public override string name
		{
			get
			{
				return string.Format("Drop {0} On {1}{2}", (!this.dropBody) ? "Item" : "Body", this.dropTargetType, this.shouldFly ? " And Fly" : "");
			}
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x0016E4CC File Offset: 0x0016C6CC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Drop), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.item = base.AddValueInput<Item>("item", "");
			this.item.skipSelfInstanceAssignment = false;
			if (this.customCountForItem)
			{
				this.count = base.AddValueInput<int>("count", "");
				this.count.serializedValue = 1;
			}
			switch (this.dropTargetType)
			{
			case Flow_DropItem.DropTargetType.Wgo:
				this.wgoDataDropOn = base.AddValueInput<WgoData>("wgoDataDropOn", "");
				break;
			case Flow_DropItem.DropTargetType.GDPoint:
				this.gdPointDataDropOn = base.AddValueInput<GDPointData>("gdPointDataDropOn", "");
				break;
			}
			if (this.shouldFly)
			{
				this.gdPointDataFlyTarget = base.AddValueInput<GDPointData>("gdPointDataFlyTarget", "");
			}
			this.droppedItem = base.AddValueOutput<Item>("droppedItem", () => this.lastDroppedItem, "");
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x0016E5F8 File Offset: 0x0016C7F8
		private void Drop(Flow flow)
		{
			Vector3 vector = Vector3.zero;
			string text = string.Empty;
			bool flag = false;
			switch (this.dropTargetType)
			{
			case Flow_DropItem.DropTargetType.Player:
				vector = MainGame.PlayerController.PlayerData.position.Value;
				text = MainGame.PlayerData.currentGameSceneId;
				flag = true;
				break;
			case Flow_DropItem.DropTargetType.Wgo:
			{
				WgoData wgoData;
				if (base.TryGetParamValue<WgoData>(this.wgoDataDropOn, out wgoData))
				{
					vector = wgoData.Position;
					text = wgoData.WorldId;
					flag = true;
				}
				break;
			}
			case Flow_DropItem.DropTargetType.GDPoint:
			{
				GDPointData gdpointData;
				if (base.TryGetParamValue<GDPointData>(this.gdPointDataDropOn, out gdpointData))
				{
					vector = gdpointData.Position;
					text = gdpointData.GameSceneDataId;
					flag = true;
				}
				break;
			}
			}
			if (flag)
			{
				Item value = this.item.value;
				bool flag2 = value.Definition != null && value.Definition.itemGroupIds.Contains("body");
				this.lastDroppedItem = ((this.dropBody || flag2) ? value : new Item(value.id, this.customCountForItem ? this.count.value : value.Count));
				List<Item> list = new List<Item>();
				if (this.lastDroppedItem.Definition == null)
				{
					Debug.LogError("DropItem Error: No ItemDef found for id \"" + this.lastDroppedItem.id + "\"");
					this.@out.Call(flow);
					return;
				}
				MainGame.Instance.dropSystem.DropItem(this.lastDroppedItem, text, vector, list);
				GDPointData gdpointData2;
				if (this.shouldFly && base.TryGetParamValue<GDPointData>(this.gdPointDataFlyTarget, out gdpointData2))
				{
					foreach (Item item in list)
					{
						DropView dropView = null;
						foreach (GameScene gameScene in LazySingleton<GameSceneManager>.Instance.LoadedGameScenes)
						{
							dropView = gameScene.GetDropView(item);
							if (dropView != null)
							{
								break;
							}
						}
						if (dropView != null)
						{
							dropView.MoveToCustomPosition(gdpointData2.Position);
						}
					}
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003E97 RID: 16023
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_DropItem.DropTargetType dropTargetType = Flow_DropItem.DropTargetType.Wgo;

		// Token: 0x04003E98 RID: 16024
		[FlowNode.GatherPortsCallbackAttribute]
		public bool shouldFly;

		// Token: 0x04003E99 RID: 16025
		[FlowNode.GatherPortsCallbackAttribute]
		public bool dropBody;

		// Token: 0x04003E9A RID: 16026
		[FlowNode.GatherPortsCallbackAttribute]
		public bool customCountForItem;

		// Token: 0x04003E9B RID: 16027
		private FlowInput @in;

		// Token: 0x04003E9C RID: 16028
		private FlowOutput @out;

		// Token: 0x04003E9D RID: 16029
		private ValueInput<Item> item;

		// Token: 0x04003E9E RID: 16030
		private ValueInput<int> count;

		// Token: 0x04003E9F RID: 16031
		private ValueInput<WgoData> wgoDataDropOn;

		// Token: 0x04003EA0 RID: 16032
		private ValueInput<GDPointData> gdPointDataDropOn;

		// Token: 0x04003EA1 RID: 16033
		private ValueInput<GDPointData> gdPointDataFlyTarget;

		// Token: 0x04003EA2 RID: 16034
		private Item lastDroppedItem;

		// Token: 0x04003EA3 RID: 16035
		private ValueOutput<Item> droppedItem;

		// Token: 0x02000B91 RID: 2961
		public enum DropTargetType
		{
			// Token: 0x04003EA5 RID: 16037
			Player,
			// Token: 0x04003EA6 RID: 16038
			Wgo,
			// Token: 0x04003EA7 RID: 16039
			GDPoint
		}
	}
}
