using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C29 RID: 3113
	[Name("Tutorial Arrow", 0)]
	[Category("Game")]
	public class Flow_TutorialArrow : GKCustomFlowNode
	{
		// Token: 0x06004F99 RID: 20377 RVA: 0x0017746C File Offset: 0x0017566C
		protected override void RegisterPorts()
		{
			if (this.findByCustomTag)
			{
				this.wgoId = base.AddValueInput<string>("WgoData", "");
			}
			else
			{
				this.wgoDataInput = base.AddValueInput<WgoData>("WgoData", "");
			}
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Action), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x001774F1 File Offset: 0x001756F1
		private void Action(Flow flow)
		{
			if (this.attach)
			{
				LazySingleton<UITutorialArrow>.Instance.Attach(this.GetWgoData());
			}
			else
			{
				LazySingleton<UITutorialArrow>.Instance.UnAttach();
			}
			this.@out.Call(flow);
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x00177523 File Offset: 0x00175723
		private WgoData GetWgoData()
		{
			if (!this.findByCustomTag)
			{
				return base.WgoDataParamOrSelf(this.wgoDataInput);
			}
			return MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(this.wgoId.value);
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06004F9C RID: 20380 RVA: 0x00177559 File Offset: 0x00175759
		public override string name
		{
			get
			{
				if (!this.attach)
				{
					return "UnAttach Tutorial Arrow";
				}
				return "Attach Tutorial Arrow";
			}
		}

		// Token: 0x04004113 RID: 16659
		[FlowNode.GatherPortsCallbackAttribute]
		public bool attach;

		// Token: 0x04004114 RID: 16660
		[FlowNode.GatherPortsCallbackAttribute]
		[InspectorName("Use Custom Tag for Wgo Data link")]
		public bool findByCustomTag;

		// Token: 0x04004115 RID: 16661
		private FlowInput @in;

		// Token: 0x04004116 RID: 16662
		private FlowOutput @out;

		// Token: 0x04004117 RID: 16663
		private ValueInput<string> wgoId;

		// Token: 0x04004118 RID: 16664
		private ValueInput<WgoData> wgoDataInput;
	}
}
