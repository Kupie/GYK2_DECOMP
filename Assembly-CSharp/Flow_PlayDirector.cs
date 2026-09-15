using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020004D1 RID: 1233
[Name("Play Director", 0)]
[Category("Game/Animation")]
public class Flow_PlayDirector : GKCustomFlowNodeWithWgoData
{
	// Token: 0x0600208D RID: 8333 RVA: 0x0009A274 File Offset: 0x00098474
	protected override void RegisterPorts()
	{
		base.RegisterPorts();
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			this.wgo = GameScene.GetWgoViewGlobal((wgoData != null) ? wgoData.UniqueId : null);
			if (!this.wgo)
			{
				this.@out.Call(flow);
				this.onFinished.Call(flow);
				return;
			}
			this.wgo.UpdateFlag(ChunkingIgnoreType.Animation, true);
			PlayableDirector componentInChildren = this.wgo.GetComponentInChildren<PlayableDirector>();
			if (!componentInChildren)
			{
				Debug.LogError("Flow_PlayDirector: PlayableDirector not found on wgo");
				this.@out.Call(flow);
				this.onFinished.Call(flow);
				this.wgo.UpdateFlag(ChunkingIgnoreType.Animation, false);
				return;
			}
			this.flow = flow;
			componentInChildren.Play();
			componentInChildren.stopped += this.OnFinished;
			this.@out.Call(flow);
		}, "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x0009A2E4 File Offset: 0x000984E4
	private void OnFinished(PlayableDirector pb)
	{
		if (this.wgo)
		{
			this.wgo.UpdateFlag(ChunkingIgnoreType.Animation, false);
		}
		pb.stopped -= this.OnFinished;
		this.onFinished.Call(this.flow);
	}

	// Token: 0x04001D2F RID: 7471
	private FlowInput @in;

	// Token: 0x04001D30 RID: 7472
	private FlowOutput @out;

	// Token: 0x04001D31 RID: 7473
	private FlowOutput onFinished;

	// Token: 0x04001D32 RID: 7474
	private Flow flow;

	// Token: 0x04001D33 RID: 7475
	private Wgo wgo;
}
