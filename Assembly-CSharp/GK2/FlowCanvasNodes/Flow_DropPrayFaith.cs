using System;
using System.Collections.Generic;
using System.Linq;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B93 RID: 2963
	[Name("Drop Pray Faith", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_DropPrayFaith : GKCustomFlowNode
	{
		// Token: 0x06004DB1 RID: 19889 RVA: 0x0016EAF8 File Offset: 0x0016CCF8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Drop), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.wgos = base.AddValueInput<List<WgoData>>("wgos", "");
			this.gdPointDataFlyTarget = base.AddValueInput<GDPointData>("gdPointDataFlyTarget", "");
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x0016EB74 File Offset: 0x0016CD74
		private void Drop(Flow flow)
		{
			if (this.wgos.value == null || this.wgos.value.Count == 0)
			{
				this.@out.Call(flow);
				return;
			}
			int count = this.wgos.value.Count;
			int num = MainGame.PlayerData.currentSermon.parishionerDatas.Sum((ParishionerData parishionerData) => parishionerData.faithToDrop);
			int num2 = num / count;
			int num3 = num % count;
			for (int i = 0; i < count; i++)
			{
				int num4 = 0;
				if (i < num3 || num2 > 0)
				{
					num4 = num2;
					if (i < num3)
					{
						num4++;
					}
				}
				if (num4 <= 0)
				{
					break;
				}
				Item item = new Item("faith", num4);
				WgoData wgoData = this.wgos.value[i];
				List<Item> list = new List<Item>();
				MainGame.Instance.dropSystem.DropItem(item, wgoData.WorldId, wgoData.Position, list);
				GDPointData gdpointData;
				if (base.TryGetParamValue<GDPointData>(this.gdPointDataFlyTarget, out gdpointData))
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
			this.@out.Call(flow);
		}

		// Token: 0x04003EAE RID: 16046
		private FlowInput @in;

		// Token: 0x04003EAF RID: 16047
		private FlowOutput @out;

		// Token: 0x04003EB0 RID: 16048
		private ValueInput<List<WgoData>> wgos;

		// Token: 0x04003EB1 RID: 16049
		private ValueInput<GDPointData> gdPointDataFlyTarget;
	}
}
