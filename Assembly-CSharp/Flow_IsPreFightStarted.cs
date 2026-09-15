using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004C6 RID: 1222
[Name("Is Pre Fight Started", 0)]
[Category("Game/Fighting")]
[Color("313c8f")]
public class Flow_IsPreFightStarted : GKCustomFlowNode
{
	// Token: 0x0600206B RID: 8299 RVA: 0x000997B0 File Offset: 0x000979B0
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.IsPreFightingStarted), "");
		this.trueOut = base.AddFlowOutput("<color=green>✔</color>", "");
		this.falseOut = base.AddFlowOutput("<color=red>✘</color>", "");
	}

	// Token: 0x0600206C RID: 8300 RVA: 0x00099810 File Offset: 0x00097A10
	private void IsPreFightingStarted(Flow flow)
	{
		if (LazySingleton<FightingGameController>.Instance.CurrentFightState == FightState.InPreFight)
		{
			this.trueOut.Call(flow);
			return;
		}
		this.falseOut.Call(flow);
	}

	// Token: 0x04001D0C RID: 7436
	private FlowInput @in;

	// Token: 0x04001D0D RID: 7437
	private FlowOutput trueOut;

	// Token: 0x04001D0E RID: 7438
	private FlowOutput falseOut;

	// Token: 0x04001D0F RID: 7439
	private ValueInput<string> levelName;
}
