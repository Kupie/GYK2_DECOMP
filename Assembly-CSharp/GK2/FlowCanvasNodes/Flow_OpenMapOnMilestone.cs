using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BDD RID: 3037
	[Name("Open Map On Milestone", 0)]
	[Category("Game/UI")]
	public class Flow_OpenMapOnMilestone : GKCustomFlowNode
	{
		// Token: 0x06004E9F RID: 20127 RVA: 0x001727F4 File Offset: 0x001709F4
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x00172843 File Offset: 0x00170A43
		private void Show(Flow flow)
		{
			LazyUI.GetWindow<UIMapWindow>().Open(new MapPageWidgetData(MainGame.Instance.GameSave, true, base.SelfWgoData.id));
			this.@out.Call(flow);
		}

		// Token: 0x04003FC9 RID: 16329
		private FlowInput @in;

		// Token: 0x04003FCA RID: 16330
		private FlowOutput @out;
	}
}
