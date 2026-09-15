using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BE7 RID: 3047
	[Name("Play Sound", 0)]
	[Category("Game/Sounds")]
	[Color("f5da42")]
	public class Flow_PlaySound : GKCustomFlowNode
	{
		// Token: 0x06004EC2 RID: 20162 RVA: 0x00173590 File Offset: 0x00171790
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.PlaySound), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.soundId = base.AddValueInput<string>("soundId".CapitalizeFirst(), "");
			if (this.atPosition && !this.getSelfWgoForPosition)
			{
				this.position = base.AddValueInput<Transform>("position".CapitalizeFirst(), "");
			}
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x00173628 File Offset: 0x00171828
		private void PlaySound(Flow flow)
		{
			if (this.stop)
			{
				LazyAudio.Stop(this.soundId.value);
			}
			else
			{
				Transform transform = null;
				if (this.atPosition && this.getSelfWgoForPosition && base.SelfWgoData != null)
				{
					transform = GameScene.GetWgoViewGlobal(base.SelfWgoData.UniqueId).transform;
				}
				if (this.atPosition && this.position != null && transform == null)
				{
					transform = this.position.value;
				}
				if (this.atPosition && transform != null)
				{
					LazyAudio.PlayAtGameObject(this.soundId.value, transform, SpatialType.sound3D, true);
				}
				else
				{
					LazyAudio.Play(this.soundId.value);
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06004EC4 RID: 20164 RVA: 0x001736EB File Offset: 0x001718EB
		public override string name
		{
			get
			{
				return (this.stop ? "Stop" : "Play") + " Sound";
			}
		}

		// Token: 0x04003FFF RID: 16383
		[FlowNode.GatherPortsCallbackAttribute]
		public bool stop;

		// Token: 0x04004000 RID: 16384
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("stop", 0)]
		public bool atPosition;

		// Token: 0x04004001 RID: 16385
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("stop", 0)]
		public bool getSelfWgoForPosition;

		// Token: 0x04004002 RID: 16386
		private FlowInput @in;

		// Token: 0x04004003 RID: 16387
		private FlowOutput @out;

		// Token: 0x04004004 RID: 16388
		private ValueInput<string> soundId;

		// Token: 0x04004005 RID: 16389
		private new ValueInput<Transform> position;
	}
}
