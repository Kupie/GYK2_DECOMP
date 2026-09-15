using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B92 RID: 2962
	[Name("Drop Item And Split", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_DropItemAndSplitBetweenWgos : GKCustomFlowNode
	{
		// Token: 0x06004DAE RID: 19886 RVA: 0x0016E854 File Offset: 0x0016CA54
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Drop), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.item = base.AddValueInput<Item>("item", "");
			this.wgos = base.AddValueInput<List<WgoData>>("wgos", "");
			if (this.shouldFly)
			{
				this.gdPointDataFlyTarget = base.AddValueInput<GDPointData>("gdPointDataFlyTarget", "");
			}
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x0016E8F0 File Offset: 0x0016CAF0
		private void Drop(Flow flow)
		{
			if (this.wgos.value == null || this.wgos.value.Count == 0 || this.item.value == null || this.item.value.Count == 0)
			{
				this.@out.Call(flow);
				return;
			}
			int count = this.wgos.value.Count;
			int count2 = this.item.value.Count;
			int num = Math.Min(count, count2);
			int num2 = count2;
			int num3 = 0;
			while (num3 < num && num2 > 0)
			{
				int num4;
				if (count2 > count)
				{
					num4 = count2 / count;
				}
				else
				{
					num4 = count / count2;
				}
				num4 = Math.Clamp(num4, 0, num2);
				num2 -= num4;
				Item item = new Item(this.item.value.id, num4);
				WgoData wgoData = this.wgos.value[Math.Clamp(num3, 0, this.wgos.value.Count - 1)];
				List<Item> list = new List<Item>();
				MainGame.Instance.dropSystem.DropItem(item, wgoData.WorldId, wgoData.Position, list);
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
				num3++;
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003EA8 RID: 16040
		[FlowNode.GatherPortsCallbackAttribute]
		public bool shouldFly;

		// Token: 0x04003EA9 RID: 16041
		private FlowInput @in;

		// Token: 0x04003EAA RID: 16042
		private FlowOutput @out;

		// Token: 0x04003EAB RID: 16043
		private ValueInput<Item> item;

		// Token: 0x04003EAC RID: 16044
		private ValueInput<List<WgoData>> wgos;

		// Token: 0x04003EAD RID: 16045
		private ValueInput<GDPointData> gdPointDataFlyTarget;
	}
}
