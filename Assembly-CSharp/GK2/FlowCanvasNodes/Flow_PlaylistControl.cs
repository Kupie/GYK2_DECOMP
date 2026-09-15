using System;
using DG.Tweening;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BE5 RID: 3045
	[Name("Playlist Control", 0)]
	[Category("Game/Sound")]
	[Color("f5da42")]
	public class Flow_PlaylistControl : GKCustomFlowNode
	{
		// Token: 0x06004EB8 RID: 20152 RVA: 0x00172EE8 File Offset: 0x001710E8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Control), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.playlistId = base.AddValueInput<string>("playlistId".CapitalizeFirst(), "");
			switch (this.controlType)
			{
			case Flow_PlaylistControl.ControlType.Play:
				this.duration = base.AddValueInput<float>("duration", "");
				return;
			case Flow_PlaylistControl.ControlType.Pause:
			case Flow_PlaylistControl.ControlType.UnPause:
				this.duration = base.AddValueInput<float>("duration", "");
				return;
			case Flow_PlaylistControl.ControlType.Stop:
				this.restorePreviousTrack = base.AddValueInput<bool>("restorePreviousTrack", "");
				this.duration = base.AddValueInput<float>("duration", "");
				return;
			case Flow_PlaylistControl.ControlType.NextTrack:
				break;
			case Flow_PlaylistControl.ControlType.PlayTrack:
				this.trackId = base.AddValueInput<string>("trackId", "");
				this.duration = base.AddValueInput<float>("duration", "");
				return;
			case Flow_PlaylistControl.ControlType.SetWeight:
			case Flow_PlaylistControl.ControlType.AddWeight:
				this.trackId = base.AddValueInput<string>("trackId", "");
				this.weight = base.AddValueInput<float>("weight", "");
				return;
			case Flow_PlaylistControl.ControlType.PauseWithEase:
			case Flow_PlaylistControl.ControlType.UnPauseWithEase:
				this.duration = base.AddValueInput<float>("duration", "");
				this.ease = base.AddValueInput<Ease>("ease", "");
				break;
			default:
				return;
			}
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x0017306C File Offset: 0x0017126C
		private void Control(Flow flow)
		{
			switch (this.controlType)
			{
			case Flow_PlaylistControl.ControlType.Play:
				if (((this.duration.value <= 0f) ? LazyAudio.PlayPlaylist(this.playlistId.value) : LazyAudio.PlayPlaylist(this.playlistId.value, this.duration.value)) != null)
				{
					Flow_PlaylistControl.PausePreviousActivePlaylist(this.playlistId.value, this.duration.value);
				}
				break;
			case Flow_PlaylistControl.ControlType.Pause:
				if (this.duration.value <= 0f)
				{
					LazyAudio.PausePlaylist(this.playlistId.value);
				}
				else
				{
					LazyAudio.PausePlaylist(this.playlistId.value, this.duration.value);
				}
				break;
			case Flow_PlaylistControl.ControlType.Stop:
				if (this.restorePreviousTrack.value)
				{
					if (Flow_PlaylistControl.TryRestorePreviousTrack(this.playlistId.value))
					{
						break;
					}
				}
				else
				{
					Flow_PlaylistControl.ClearPreviousTrack();
				}
				if (this.duration.value <= 0f)
				{
					LazyAudio.StopPlaylist(this.playlistId.value);
				}
				else
				{
					LazyAudio.StopPlaylist(this.playlistId.value, this.duration.value);
				}
				Flow_PlaylistControl.UnpausePreviousActivePlaylist(this.playlistId.value, this.duration.value);
				break;
			case Flow_PlaylistControl.ControlType.NextTrack:
				LazyAudio.PlayNextTrack(this.playlistId.value);
				break;
			case Flow_PlaylistControl.ControlType.PlayTrack:
				Flow_PlaylistControl.RememberCurrentTrack(this.playlistId.value, this.trackId.value);
				if (this.duration.value <= 0f)
				{
					LazyAudio.PlayTrackInPlaylist(this.trackId.value, this.playlistId.value);
				}
				else
				{
					LazyAudio.PlayTrackInPlaylist(this.trackId.value, this.playlistId.value, this.duration.value);
				}
				break;
			case Flow_PlaylistControl.ControlType.SetWeight:
				LazyAudio.SetWeightTrackInPlaylist(this.trackId.value, this.playlistId.value, this.weight.value);
				break;
			case Flow_PlaylistControl.ControlType.AddWeight:
				LazyAudio.AddWeightTrackInPlaylist(this.trackId.value, this.playlistId.value, this.weight.value);
				break;
			case Flow_PlaylistControl.ControlType.PauseWithEase:
				if (this.duration.value <= 0f)
				{
					LazyAudio.PausePlaylist(this.playlistId.value);
				}
				else
				{
					LazyAudio.PausePlaylist(this.playlistId.value, this.duration.value, this.ease.value);
				}
				break;
			case Flow_PlaylistControl.ControlType.UnPause:
				if (this.duration.value <= 0f)
				{
					LazyAudio.UnPausePlaylist(this.playlistId.value);
				}
				else
				{
					LazyAudio.UnPausePlaylist(this.playlistId.value, this.duration.value);
				}
				break;
			case Flow_PlaylistControl.ControlType.UnPauseWithEase:
				if (this.duration.value <= 0f)
				{
					LazyAudio.UnPausePlaylist(this.playlistId.value);
				}
				else
				{
					LazyAudio.UnPausePlaylist(this.playlistId.value, this.duration.value, this.ease.value);
				}
				break;
			}
			this.@out.Call(flow);
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x001733B8 File Offset: 0x001715B8
		private static void PausePreviousActivePlaylist(string newPlaylistId, float fadeDuration)
		{
			if (!string.IsNullOrEmpty(Flow_PlaylistControl.activePlaylistId) && Flow_PlaylistControl.activePlaylistId != newPlaylistId)
			{
				if (fadeDuration <= 0f)
				{
					LazyAudio.PausePlaylist(Flow_PlaylistControl.activePlaylistId);
				}
				else
				{
					LazyAudio.PausePlaylist(Flow_PlaylistControl.activePlaylistId, fadeDuration);
				}
				Flow_PlaylistControl.pausedPlaylistId = Flow_PlaylistControl.activePlaylistId;
			}
			Flow_PlaylistControl.activePlaylistId = newPlaylistId;
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x00173410 File Offset: 0x00171610
		private static void UnpausePreviousActivePlaylist(string stoppedPlaylistId, float fadeDuration)
		{
			if (Flow_PlaylistControl.activePlaylistId != stoppedPlaylistId)
			{
				return;
			}
			string text = Flow_PlaylistControl.pausedPlaylistId;
			Flow_PlaylistControl.pausedPlaylistId = null;
			Flow_PlaylistControl.activePlaylistId = text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (fadeDuration <= 0f)
			{
				LazyAudio.UnPausePlaylist(text);
				return;
			}
			LazyAudio.UnPausePlaylist(text, fadeDuration);
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x0017345C File Offset: 0x0017165C
		private static PlaylistController FindPlaylistController(string id)
		{
			foreach (PlaylistController playlistController in LazyAudio.GetPlaylistControllers())
			{
				if (playlistController.Id == id)
				{
					return playlistController;
				}
			}
			return null;
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x001734BC File Offset: 0x001716BC
		private static void RememberCurrentTrack(string playlistId, string newTrackId)
		{
			if (string.IsNullOrEmpty(playlistId))
			{
				return;
			}
			PlaylistController playlistController = Flow_PlaylistControl.FindPlaylistController(playlistId);
			Track track = ((playlistController != null) ? playlistController.LastTrack : null);
			if (track == null || string.IsNullOrEmpty(track.id))
			{
				return;
			}
			if (track.id == newTrackId)
			{
				return;
			}
			Flow_PlaylistControl.previousPlaylistId = playlistId;
			Flow_PlaylistControl.previousTrackId = track.id;
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x00173516 File Offset: 0x00171716
		private static void ClearPreviousTrack()
		{
			Flow_PlaylistControl.previousPlaylistId = null;
			Flow_PlaylistControl.previousTrackId = null;
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x00173524 File Offset: 0x00171724
		private static bool TryRestorePreviousTrack(string playlistId)
		{
			if (string.IsNullOrEmpty(Flow_PlaylistControl.previousTrackId) || Flow_PlaylistControl.previousPlaylistId != playlistId)
			{
				return false;
			}
			string text = Flow_PlaylistControl.previousTrackId;
			Flow_PlaylistControl.ClearPreviousTrack();
			LazyAudio.PlayTrackInPlaylist(text, playlistId);
			return true;
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004EC0 RID: 20160 RVA: 0x00173552 File Offset: 0x00171752
		public override string name
		{
			get
			{
				return string.Format("{0} \n<color=#58BF2B>{1}</color>", base.name, this.controlType) + (this.playlistId.isDefaultValue ? "<color=#F73B3B>\nPlaylistId is NULL</color>" : string.Empty);
			}
		}

		// Token: 0x04003FE7 RID: 16359
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_PlaylistControl.ControlType controlType;

		// Token: 0x04003FE8 RID: 16360
		private static string previousPlaylistId;

		// Token: 0x04003FE9 RID: 16361
		private static string previousTrackId;

		// Token: 0x04003FEA RID: 16362
		private static string activePlaylistId;

		// Token: 0x04003FEB RID: 16363
		private static string pausedPlaylistId;

		// Token: 0x04003FEC RID: 16364
		private FlowInput @in;

		// Token: 0x04003FED RID: 16365
		private FlowOutput @out;

		// Token: 0x04003FEE RID: 16366
		private ValueInput<string> playlistId;

		// Token: 0x04003FEF RID: 16367
		private ValueInput<string> trackId;

		// Token: 0x04003FF0 RID: 16368
		private ValueInput<float> weight;

		// Token: 0x04003FF1 RID: 16369
		private ValueInput<float> duration;

		// Token: 0x04003FF2 RID: 16370
		private ValueInput<Ease> ease;

		// Token: 0x04003FF3 RID: 16371
		private ValueInput<bool> restorePreviousTrack;

		// Token: 0x02000BE6 RID: 3046
		public enum ControlType
		{
			// Token: 0x04003FF5 RID: 16373
			Play,
			// Token: 0x04003FF6 RID: 16374
			Pause,
			// Token: 0x04003FF7 RID: 16375
			Stop,
			// Token: 0x04003FF8 RID: 16376
			NextTrack,
			// Token: 0x04003FF9 RID: 16377
			PlayTrack,
			// Token: 0x04003FFA RID: 16378
			SetWeight,
			// Token: 0x04003FFB RID: 16379
			AddWeight,
			// Token: 0x04003FFC RID: 16380
			PauseWithEase,
			// Token: 0x04003FFD RID: 16381
			UnPause,
			// Token: 0x04003FFE RID: 16382
			UnPauseWithEase
		}
	}
}
