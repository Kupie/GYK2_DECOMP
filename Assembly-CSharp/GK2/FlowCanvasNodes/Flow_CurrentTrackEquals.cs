using System;
using System.Linq;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B8A RID: 2954
	[Name("Current Track Equals", 0)]
	[Category("Game/Sound")]
	[Color("f5da42")]
	public class Flow_CurrentTrackEquals : GKCustomFlowNode
	{
		// Token: 0x06004D93 RID: 19859 RVA: 0x0016DE64 File Offset: 0x0016C064
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				PlaylistController playlistController = LazyAudio.GetPlaylistControllers().FirstOrDefault((PlaylistController c) => c.Id == this.playlistId.value);
				Track track = ((playlistController != null) ? playlistController.LastTrack : null);
				if (track == null || string.IsNullOrEmpty(track.id) || track.id != this.trackId.value)
				{
					this.no.Call(flow);
					return;
				}
				this.yes.Call(flow);
			}, "");
			this.yes = base.AddFlowOutput("yes".CapitalizeFirst(), "");
			this.no = base.AddFlowOutput("no".CapitalizeFirst(), "");
			this.playlistId = base.AddValueInput<string>("playlistId".CapitalizeFirst(), "");
			this.trackId = base.AddValueInput<string>("trackId".CapitalizeFirst(), "");
		}

		// Token: 0x04003E82 RID: 16002
		private FlowInput @in;

		// Token: 0x04003E83 RID: 16003
		private FlowOutput yes;

		// Token: 0x04003E84 RID: 16004
		private FlowOutput no;

		// Token: 0x04003E85 RID: 16005
		private ValueInput<string> playlistId;

		// Token: 0x04003E86 RID: 16006
		private ValueInput<string> trackId;
	}
}
