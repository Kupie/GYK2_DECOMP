using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000C5 RID: 197
	public class PlaylistController : MonoBehaviour
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000316 RID: 790 RVA: 0x00010030 File Offset: 0x0000E230
		public Track LastTrack
		{
			get
			{
				return this.lastTrack;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00010038 File Offset: 0x0000E238
		public Playlist Playlist
		{
			get
			{
				return this.playlist;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00010040 File Offset: 0x0000E240
		public bool IsActive
		{
			get
			{
				return this.isPlaying || this.isPaused;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00010052 File Offset: 0x0000E252
		public bool IsPaused
		{
			get
			{
				return this.isPaused;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0001005A File Offset: 0x0000E25A
		public string Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00010064 File Offset: 0x0000E264
		public void Initialize(Playlist playlist)
		{
			if (this.isInitialized)
			{
				return;
			}
			this.id = playlist.id;
			this.playlist = playlist;
			base.name = "Playlist: " + this.id;
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(base.transform);
			this.audioSource1 = gameObject.AddComponent<AudioSource>();
			gameObject = new GameObject();
			this.audioSource1.outputAudioMixerGroup = playlist.group;
			gameObject.transform.SetParent(base.transform);
			this.audioSource2 = gameObject.AddComponent<AudioSource>();
			this.audioSource2.outputAudioMixerGroup = playlist.group;
			this.PrepareTracks();
			this.isInitialized = true;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00010118 File Offset: 0x0000E318
		public void Play()
		{
			if (this.isPaused)
			{
				float num = this.pendingFadeDuration;
				this.pendingFadeDuration = -1f;
				if (num >= 0f)
				{
					this.UnPause(num);
					return;
				}
				this.UnPause();
				return;
			}
			else
			{
				this.CancelVolumeFades();
				if (this.isPlaying)
				{
					this.pendingFadeDuration = -1f;
					return;
				}
				this.isPlaying = true;
				this.NextTrack();
				return;
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0001017D File Offset: 0x0000E37D
		public void Play(float fadeDuration)
		{
			this.pendingFadeDuration = fadeDuration;
			this.Play();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0001018C File Offset: 0x0000E38C
		public void UnPause()
		{
			this.CancelVolumeFades();
			if (!this.isPlaying || !this.isPaused)
			{
				return;
			}
			this.isPaused = false;
			if (this.pausedWithFade)
			{
				this.activeAudioSource.volume = this.pausedVolume;
				this.pausedWithFade = false;
			}
			this.activeAudioSource.UnPause();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000101E2 File Offset: 0x0000E3E2
		public void UnPause(float duration)
		{
			if (!this.isPlaying || !this.isPaused)
			{
				return;
			}
			if (duration <= 0f)
			{
				this.UnPause();
				return;
			}
			this.CancelVolumeFades();
			this.BeginUnpauseFade(duration);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00010211 File Offset: 0x0000E411
		public void UnPause(float duration, Ease ease)
		{
			if (!this.isPlaying || !this.isPaused)
			{
				return;
			}
			if (duration <= 0f)
			{
				this.UnPause();
				return;
			}
			this.CancelVolumeFades();
			this.BeginUnpauseFade(duration, ease);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00010241 File Offset: 0x0000E441
		public void Pause()
		{
			this.CancelVolumeFades();
			if (!this.isPlaying || this.isPaused)
			{
				return;
			}
			this.activeAudioSource.Pause();
			this.isPaused = true;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0001026C File Offset: 0x0000E46C
		public void Pause(float duration)
		{
			if (!this.isPlaying || this.isPaused)
			{
				return;
			}
			if (duration <= 0f)
			{
				this.Pause();
				return;
			}
			this.CancelVolumeFades();
			this.BeginPauseFade(duration);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0001029B File Offset: 0x0000E49B
		public void Pause(float duration, Ease ease)
		{
			if (!this.isPlaying || this.isPaused)
			{
				return;
			}
			if (duration <= 0f)
			{
				this.Pause();
				return;
			}
			this.CancelVolumeFades();
			this.BeginPauseFade(duration, ease);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000102CC File Offset: 0x0000E4CC
		public void Stop()
		{
			if (!this.isPlaying)
			{
				return;
			}
			this.CancelVolumeFades();
			this.isPlaying = false;
			this.isPaused = false;
			this.pausedWithFade = false;
			this.FadeOutActiveAudioSource(null);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0001030C File Offset: 0x0000E50C
		public void Stop(float fadeDuration)
		{
			if (!this.isPlaying)
			{
				return;
			}
			if (fadeDuration <= 0f)
			{
				this.StopImmediately();
				return;
			}
			this.CancelVolumeFades();
			this.isPlaying = false;
			this.isPaused = false;
			this.pausedWithFade = false;
			this.FadeOutActiveAudioSource(new float?(fadeDuration));
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00010358 File Offset: 0x0000E558
		public void StopImmediately()
		{
			this.CancelVolumeFades();
			AudioSource audioSource = this.audioSource1;
			if (audioSource != null)
			{
				audioSource.Stop();
			}
			AudioSource audioSource2 = this.audioSource2;
			if (audioSource2 != null)
			{
				audioSource2.Stop();
			}
			if (!this.isPlaying)
			{
				return;
			}
			this.isPlaying = false;
			this.isPaused = false;
			this.pausedWithFade = false;
			AudioSource audioSource3 = this.activeAudioSource;
			if (audioSource3 == null)
			{
				return;
			}
			audioSource3.Stop();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000103BB File Offset: 0x0000E5BB
		public float GetActiveSourcePlaybackPosition()
		{
			if (!(this.activeAudioSource == null))
			{
				return this.activeAudioSource.time;
			}
			return 0f;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000103DC File Offset: 0x0000E5DC
		public void NextTrack()
		{
			if (!this.isPlaying)
			{
				return;
			}
			this.isPaused = false;
			this.PlayTrack(this.GetNextTrack());
		}

		// Token: 0x06000329 RID: 809 RVA: 0x000103FA File Offset: 0x0000E5FA
		public void PlayTrack(string trackId)
		{
			this.PlayTrackInternal(trackId);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00010403 File Offset: 0x0000E603
		public void PlayTrack(string trackId, float fadeDuration)
		{
			this.pendingFadeDuration = fadeDuration;
			this.PlayTrackInternal(trackId);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00010414 File Offset: 0x0000E614
		private void PlayTrackInternal(string trackId)
		{
			Track track = this.GetTrackById(trackId);
			if (track == null)
			{
				this.pendingFadeDuration = -1f;
				Debug.LogError("Audio System error: cannot find track " + trackId + " in playlist " + this.playlist.id);
				return;
			}
			if (this.playlist.weightRandomized)
			{
				if (!this.isPlaying || track != this.lastTrack)
				{
					this.PlayTrack(track);
					this.lastTrack = track;
					return;
				}
				if (this.isPaused)
				{
					this.Play();
					return;
				}
				this.pendingFadeDuration = -1f;
				return;
			}
			else
			{
				if (!this.isPlaying)
				{
					if (track == this.lastTrack)
					{
						this.notPlayedTracks.Insert(0, this.lastTrack);
						this.lastTrack = null;
					}
					else
					{
						int num = this.playedTracks.FindIndex((Track x) => x == track);
						if (num != -1)
						{
							this.playedTracks.RemoveAt(num);
							this.notPlayedTracks.Insert(0, track);
						}
						else
						{
							num = this.notPlayedTracks.FindIndex((Track x) => x == track);
							this.notPlayedTracks.RemoveAt(num);
							this.notPlayedTracks.Insert(0, track);
						}
					}
					this.isPlaying = true;
					this.NextTrack();
					return;
				}
				if (track != this.lastTrack)
				{
					int num2 = this.playedTracks.FindIndex((Track x) => x == track);
					if (num2 != -1)
					{
						this.playedTracks.RemoveAt(num2);
						this.notPlayedTracks.Insert(0, track);
					}
					else
					{
						num2 = this.notPlayedTracks.FindIndex((Track x) => x == track);
						this.notPlayedTracks.RemoveAt(num2);
						this.notPlayedTracks.Insert(0, track);
					}
					this.isPlaying = true;
					this.NextTrack();
					return;
				}
				if (this.isPaused)
				{
					this.Play();
					return;
				}
				this.pendingFadeDuration = -1f;
				return;
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0001061C File Offset: 0x0000E81C
		public void SetWeightTrack(string trackId, float weight)
		{
			Track trackById = this.GetTrackById(trackId);
			if (trackById != null)
			{
				trackById.weight = weight;
				if (trackById.weight < 0f)
				{
					trackById.weight = 0f;
					return;
				}
			}
			else
			{
				Debug.LogError("Cannot change weight for trackId [" + trackId + "]. Track not found.");
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0001066C File Offset: 0x0000E86C
		public void AddWeightTrack(string trackId, float weight)
		{
			Track trackById = this.GetTrackById(trackId);
			if (trackById != null)
			{
				this.SetWeightTrack(trackId, trackById.weight + weight);
				return;
			}
			Debug.LogError("Cannot change weight for trackId [" + trackId + "]. Track not found.");
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000106AC File Offset: 0x0000E8AC
		private void PrepareTracks()
		{
			this.notPlayedTracks.Clear();
			this.playedTracks.Clear();
			this.lastTrack = null;
			if (this.playlist.tracks.Count < 1)
			{
				Debug.LogError("Audio System Playlist error: not enough tracks in playlist " + this.playlist.id);
				return;
			}
			if (this.playlist.tracks.Count == 1)
			{
				Debug.LogWarning("Audio System Playlist playlist " + this.playlist.id + " contains only one track");
			}
			this.playedTracks.AddRange(this.playlist.tracks);
			if (this.playlist.shuffled)
			{
				while (this.playedTracks.Count != 0)
				{
					this.notPlayedTracks.Add(this.playedTracks.PopRandom<Track>());
				}
			}
			else
			{
				this.notPlayedTracks.AddRange(this.playedTracks);
			}
			this.playedTracks.Clear();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0001079C File Offset: 0x0000E99C
		private Track GetNextTrack()
		{
			Track track = null;
			if (this.playlist.weightRandomized)
			{
				float num = this.CalculateTotalWeight();
				float num2 = global::UnityEngine.Random.Range(0f, num);
				float num3 = 0f;
				for (int i = 0; i < this.playlist.tracks.Count; i++)
				{
					float weight = this.playlist.tracks[i].weight;
					if (((double)Mathf.Abs(num3 + weight - num2) <= 0.001 || num3 + weight > num2) && (double)Mathf.Abs(weight) >= 0.001)
					{
						track = this.playlist.tracks[i];
						break;
					}
					num3 += weight;
				}
			}
			else
			{
				if (this.notPlayedTracks.Count == 0)
				{
					if (this.playlist.tracks.Count == 1)
					{
						this.notPlayedTracks.Add(this.playlist.tracks[0]);
					}
					else if (this.playlist.shuffled)
					{
						while (this.playedTracks.Count != 0)
						{
							this.notPlayedTracks.Add(this.playedTracks.PopRandom<Track>());
						}
						if (this.lastTrack != null && this.notPlayedTracks.Count > 1)
						{
							int num4 = global::UnityEngine.Random.Range(1, this.notPlayedTracks.Count);
							this.notPlayedTracks.Insert(num4, this.lastTrack);
						}
					}
					else
					{
						this.lastTrack = null;
						this.notPlayedTracks.AddRange(this.playlist.tracks);
						this.playedTracks.Clear();
					}
				}
				else if (this.lastTrack != null)
				{
					this.playedTracks.Add(this.lastTrack);
				}
				track = this.notPlayedTracks[0];
				this.notPlayedTracks.RemoveAt(0);
			}
			this.lastTrack = track;
			return track;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0001097C File Offset: 0x0000EB7C
		private Track GetTrackById(string id)
		{
			return this.playlist.tracks.Find((Track x) => x.id == id);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000109B4 File Offset: 0x0000EBB4
		private void PlayTrack(Track track)
		{
			if (track == null)
			{
				Debug.LogError("Trying to play null track in " + this.playlist.id);
				this.StopImmediately();
				return;
			}
			AudioSource freeAudioSource = this.GetFreeAudioSource();
			freeAudioSource.clip = track.clip;
			freeAudioSource.volume = track.volume * this.playlist.volume;
			freeAudioSource.panStereo = track.panning;
			freeAudioSource.pitch = track.pitch;
			freeAudioSource.outputAudioMixerGroup = this.playlist.group;
			freeAudioSource.loop = track.loop;
			float num = ((this.pendingFadeDuration >= 0f) ? this.pendingFadeDuration : this.playlist.fadeDuration);
			this.pendingFadeDuration = -1f;
			if (num <= 0f)
			{
				if (this.isCrossfade)
				{
					AudioSource audioSource = this.fadeOutData.audioSource;
					if (audioSource != null)
					{
						audioSource.Stop();
					}
					AudioSource audioSource2 = this.fadeInData.audioSource;
					if (audioSource2 != null)
					{
						audioSource2.Stop();
					}
					this.isCrossfade = false;
				}
				else if (this.activeAudioSource != null && this.activeAudioSource != freeAudioSource)
				{
					this.activeAudioSource.Stop();
				}
				freeAudioSource.Play();
				this.activeAudioSource = freeAudioSource;
				this.currentFadeTime = 0f;
				return;
			}
			freeAudioSource.Play();
			this.activeCrossfadeDuration = num;
			if (this.isCrossfade)
			{
				if (this.fadeInData.audioSource != null)
				{
					this.fadeOutData.audioSource = this.fadeInData.audioSource;
					this.fadeOutData.maxVolume = this.fadeInData.audioSource.volume;
				}
				else
				{
					AudioSource audioSource3 = this.fadeOutData.audioSource;
					if (audioSource3 != null)
					{
						audioSource3.Stop();
					}
					this.fadeOutData.audioSource = null;
				}
				this.fadeInData.audioSource = freeAudioSource;
				this.fadeInData.maxVolume = freeAudioSource.volume;
				freeAudioSource.volume = 0f;
			}
			else
			{
				this.isCrossfade = true;
				this.fadeInData.audioSource = freeAudioSource;
				this.fadeInData.maxVolume = freeAudioSource.volume;
				freeAudioSource.volume = 0f;
				if (this.activeAudioSource != null)
				{
					this.fadeOutData.audioSource = this.activeAudioSource;
					this.fadeOutData.maxVolume = this.activeAudioSource.volume;
				}
				else
				{
					this.fadeOutData.audioSource = null;
				}
			}
			this.currentFadeTime = 0f;
			this.activeAudioSource = freeAudioSource;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00010C27 File Offset: 0x0000EE27
		private AudioSource GetFreeAudioSource()
		{
			if (!(this.activeAudioSource != null))
			{
				return this.audioSource1;
			}
			if (!(this.activeAudioSource == this.audioSource1))
			{
				return this.audioSource1;
			}
			return this.audioSource2;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00010C5E File Offset: 0x0000EE5E
		private void BeginPauseFade(float duration)
		{
			this.ResolveActiveCrossfade();
			this.isPauseFading = true;
			this.pauseFadeTime = 0f;
			this.pauseFadeDuration = duration;
			this.pauseFadeStartVolume = this.activeAudioSource.volume;
			this.pausedVolume = this.pauseFadeStartVolume;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00010C9C File Offset: 0x0000EE9C
		private void BeginUnpauseFade(float duration)
		{
			this.PrepareUnpauseFade();
			this.isUnpauseFading = true;
			this.unpauseFadeTime = 0f;
			this.unpauseFadeDuration = duration;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00010CC0 File Offset: 0x0000EEC0
		private void BeginPauseFade(float duration, Ease ease)
		{
			this.ResolveActiveCrossfade();
			this.pausedVolume = this.activeAudioSource.volume;
			this.pauseFadeTween = this.activeAudioSource.DOFade(0f, duration).SetEase(ease).OnComplete(new TweenCallback(this.CompletePauseFade));
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00010D12 File Offset: 0x0000EF12
		private void BeginUnpauseFade(float duration, Ease ease)
		{
			this.PrepareUnpauseFade();
			this.unpauseFadeTween = this.activeAudioSource.DOFade(this.unpauseFadeTargetVolume, duration).SetEase(ease).OnComplete(new TweenCallback(this.CompleteUnpauseFade));
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00010D4C File Offset: 0x0000EF4C
		private void PrepareUnpauseFade()
		{
			this.ResolveActiveCrossfade();
			this.unpauseFadeTargetVolume = (this.pausedWithFade ? this.pausedVolume : this.activeAudioSource.volume);
			this.isPaused = false;
			this.pausedWithFade = false;
			this.activeAudioSource.volume = 0f;
			this.activeAudioSource.UnPause();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00010DAC File Offset: 0x0000EFAC
		private void ResolveActiveCrossfade()
		{
			if (!this.isCrossfade)
			{
				return;
			}
			AudioSource audioSource = this.fadeOutData.audioSource;
			if (audioSource != null)
			{
				audioSource.Stop();
			}
			if (this.fadeInData.audioSource != null)
			{
				this.fadeInData.audioSource.volume = this.fadeInData.maxVolume;
			}
			this.isCrossfade = false;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00010E0D File Offset: 0x0000F00D
		private void CompletePauseFade()
		{
			this.isPauseFading = false;
			this.pauseFadeTween = null;
			this.pausedWithFade = true;
			this.activeAudioSource.Pause();
			this.isPaused = true;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00010E36 File Offset: 0x0000F036
		private void CompleteUnpauseFade()
		{
			this.isUnpauseFading = false;
			this.unpauseFadeTween = null;
			this.activeAudioSource.volume = this.unpauseFadeTargetVolume;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00010E58 File Offset: 0x0000F058
		private void CancelVolumeFades()
		{
			this.isPauseFading = false;
			this.isUnpauseFading = false;
			Tween tween = this.pauseFadeTween;
			if (tween != null)
			{
				tween.Kill(false);
			}
			this.pauseFadeTween = null;
			Tween tween2 = this.unpauseFadeTween;
			if (tween2 != null)
			{
				tween2.Kill(false);
			}
			this.unpauseFadeTween = null;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00010EA8 File Offset: 0x0000F0A8
		private void FadeOutActiveAudioSource(float? fadeDuration = null)
		{
			this.isCrossfade = true;
			this.fadeOutData.audioSource = this.activeAudioSource;
			this.fadeOutData.maxVolume = this.activeAudioSource.volume;
			this.fadeInData.audioSource = null;
			this.activeCrossfadeDuration = fadeDuration ?? this.playlist.fadeDuration;
			this.currentFadeTime = 0f;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00010F20 File Offset: 0x0000F120
		private float CalculateTotalWeight()
		{
			float num = 0f;
			for (int i = 0; i < this.playlist.tracks.Count; i++)
			{
				num += this.playlist.tracks[i].weight;
			}
			return num;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00010F68 File Offset: 0x0000F168
		private bool ShouldAdvanceToNextTrack()
		{
			return this.playlist.weightRandomized || this.playlist.tracks.Count == 1 || (this.notPlayedTracks.Count > 0 || this.playlist.loop || this.playlist.shuffled);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00010FC4 File Offset: 0x0000F1C4
		private void Update()
		{
			if (this.activeAudioSource != null)
			{
				if (this.isPlaying && !this.isPaused && !this.isCrossfade && !this.isPauseFading && !this.isUnpauseFading && !this.activeAudioSource.loop && this.activeAudioSource.clip != null && this.activeAudioSource.time >= this.activeAudioSource.clip.length - this.playlist.fadeDuration && this.ShouldAdvanceToNextTrack())
				{
					this.PlayTrack(this.GetNextTrack());
				}
				if (this.isPauseFading)
				{
					this.pauseFadeTime += Time.deltaTime;
					float num = Mathf.Clamp01(this.pauseFadeTime / this.pauseFadeDuration);
					this.activeAudioSource.volume = Mathf.Lerp(this.pauseFadeStartVolume, 0f, num);
					if (this.pauseFadeTime >= this.pauseFadeDuration)
					{
						this.CompletePauseFade();
					}
				}
				if (this.isUnpauseFading)
				{
					this.unpauseFadeTime += Time.deltaTime;
					float num2 = Mathf.Clamp01(this.unpauseFadeTime / this.unpauseFadeDuration);
					this.activeAudioSource.volume = Mathf.Lerp(0f, this.unpauseFadeTargetVolume, num2);
					if (this.unpauseFadeTime >= this.unpauseFadeDuration)
					{
						this.CompleteUnpauseFade();
					}
				}
				if (this.isCrossfade && !this.isPauseFading && !this.isUnpauseFading)
				{
					if (this.activeCrossfadeDuration <= 0f)
					{
						if (this.fadeInData.audioSource != null)
						{
							this.fadeInData.audioSource.volume = this.fadeInData.maxVolume;
						}
						this.isCrossfade = false;
						AudioSource audioSource = this.fadeOutData.audioSource;
						if (audioSource == null)
						{
							return;
						}
						audioSource.Stop();
						return;
					}
					else
					{
						this.currentFadeTime += Time.deltaTime;
						float num3 = this.currentFadeTime / this.activeCrossfadeDuration;
						if (this.fadeInData.audioSource != null)
						{
							this.fadeInData.audioSource.volume = Mathf.Lerp(0f, this.fadeInData.maxVolume, num3);
						}
						if (this.fadeOutData.audioSource != null)
						{
							this.fadeOutData.audioSource.volume = Mathf.Lerp(0f, this.fadeOutData.maxVolume, 1f - num3);
						}
						if (this.currentFadeTime >= this.activeCrossfadeDuration)
						{
							this.isCrossfade = false;
							AudioSource audioSource2 = this.fadeOutData.audioSource;
							if (audioSource2 == null)
							{
								return;
							}
							audioSource2.Stop();
						}
					}
				}
			}
		}

		// Token: 0x040000E4 RID: 228
		[SerializeField]
		private string id;

		// Token: 0x040000E5 RID: 229
		[Space(20f)]
		[SerializeField]
		private List<Track> playedTracks = new List<Track>();

		// Token: 0x040000E6 RID: 230
		[SerializeField]
		private List<Track> notPlayedTracks = new List<Track>();

		// Token: 0x040000E7 RID: 231
		[SerializeField]
		private Track lastTrack;

		// Token: 0x040000E8 RID: 232
		[Space(20f)]
		[SerializeField]
		private bool isPaused;

		// Token: 0x040000E9 RID: 233
		[SerializeField]
		private bool isPlaying;

		// Token: 0x040000EA RID: 234
		private Playlist playlist;

		// Token: 0x040000EB RID: 235
		private AudioSource audioSource1;

		// Token: 0x040000EC RID: 236
		private AudioSource audioSource2;

		// Token: 0x040000ED RID: 237
		private AudioSource activeAudioSource;

		// Token: 0x040000EE RID: 238
		private bool isCrossfade;

		// Token: 0x040000EF RID: 239
		private float currentFadeTime;

		// Token: 0x040000F0 RID: 240
		private float activeCrossfadeDuration;

		// Token: 0x040000F1 RID: 241
		private float pendingFadeDuration = -1f;

		// Token: 0x040000F2 RID: 242
		private bool isPauseFading;

		// Token: 0x040000F3 RID: 243
		private float pauseFadeTime;

		// Token: 0x040000F4 RID: 244
		private float pauseFadeDuration;

		// Token: 0x040000F5 RID: 245
		private float pauseFadeStartVolume;

		// Token: 0x040000F6 RID: 246
		private float pausedVolume;

		// Token: 0x040000F7 RID: 247
		private bool pausedWithFade;

		// Token: 0x040000F8 RID: 248
		private bool isUnpauseFading;

		// Token: 0x040000F9 RID: 249
		private float unpauseFadeTime;

		// Token: 0x040000FA RID: 250
		private float unpauseFadeDuration;

		// Token: 0x040000FB RID: 251
		private float unpauseFadeTargetVolume;

		// Token: 0x040000FC RID: 252
		private Tween pauseFadeTween;

		// Token: 0x040000FD RID: 253
		private Tween unpauseFadeTween;

		// Token: 0x040000FE RID: 254
		private PlaylistController.FadeData fadeOutData = new PlaylistController.FadeData();

		// Token: 0x040000FF RID: 255
		private PlaylistController.FadeData fadeInData = new PlaylistController.FadeData();

		// Token: 0x04000100 RID: 256
		private bool isInitialized;

		// Token: 0x020001BF RID: 447
		private class FadeData
		{
			// Token: 0x04000609 RID: 1545
			public AudioSource audioSource;

			// Token: 0x0400060A RID: 1546
			public float maxVolume;
		}
	}
}
