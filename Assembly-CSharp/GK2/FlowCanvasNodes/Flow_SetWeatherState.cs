using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C03 RID: 3075
	[Name("Set Weather State", 0)]
	[Category("Game/Weather")]
	[Color("313c8f")]
	public class Flow_SetWeatherState : GKCustomFlowNode
	{
		// Token: 0x06004F21 RID: 20257 RVA: 0x0017521C File Offset: 0x0017341C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeGameRes), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (!this.reset)
			{
				this.stateName = base.AddValueInput<string>("stateName", "");
			}
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x0017528C File Offset: 0x0017348C
		private void ChangeGameRes(Flow flow)
		{
			if (!this.reset)
			{
				if (!string.IsNullOrEmpty(this.stateName.value))
				{
					WeatherSystem.Instance.SetWeatherState(this.stateName.value, true);
				}
			}
			else
			{
				WeatherSystem.Instance.ResetWeatherState();
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06004F23 RID: 20259 RVA: 0x001752E1 File Offset: 0x001734E1
		public override string name
		{
			get
			{
				return (this.reset ? "Reset" : "Set") + " Weather State";
			}
		}

		// Token: 0x04004075 RID: 16501
		[FlowNode.GatherPortsCallbackAttribute]
		public bool reset;

		// Token: 0x04004076 RID: 16502
		private FlowInput @in;

		// Token: 0x04004077 RID: 16503
		private FlowOutput @out;

		// Token: 0x04004078 RID: 16504
		private ValueInput<string> stateName;
	}
}
