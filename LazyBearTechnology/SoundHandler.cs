using System;
using UnityEngine;
using UnityEngine.Audio;

namespace LazyBearTechnology
{
	// Token: 0x020000D9 RID: 217
	public class SoundHandler
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00014569 File Offset: 0x00012769
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00014571 File Offset: 0x00012771
		public float DefaultVolume
		{
			get
			{
				return this.defaultVolume;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00014579 File Offset: 0x00012779
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00014581 File Offset: 0x00012781
		public Action OnSoundPlayed
		{
			get
			{
				return this.onSoundPlayed;
			}
			set
			{
				this.onSoundPlayed = value;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001458C File Offset: 0x0001278C
		public SoundHandler(SoundController sourceHolder)
		{
			this.sourceHolder = sourceHolder;
			this.defaultVolume = sourceHolder.audioSource.volume;
			this.defaultPitch = sourceHolder.audioSource.pitch;
			this.defaultPanning = sourceHolder.audioSource.panStereo;
			sourceHolder.OnSoundPlayed += this.OnSoundPlayedHandler;
			this.isActive = true;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000145F2 File Offset: 0x000127F2
		public bool SetVolume(float volume)
		{
			if (!this.isActive)
			{
				return false;
			}
			this.sourceHolder.audioSource.volume = this.defaultVolume * volume;
			return true;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00014617 File Offset: 0x00012817
		public float GetVolume()
		{
			return this.sourceHolder.audioSource.volume;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00014629 File Offset: 0x00012829
		public bool SetPitch(float pitch)
		{
			if (!this.isActive)
			{
				return false;
			}
			this.sourceHolder.audioSource.pitch = this.defaultPitch * pitch;
			return true;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001464E File Offset: 0x0001284E
		public bool SetPanning(float panning)
		{
			if (!this.isActive)
			{
				return false;
			}
			this.sourceHolder.audioSource.panStereo = this.defaultPanning * panning;
			return true;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00014673 File Offset: 0x00012873
		public float GetClipLength()
		{
			return this.sourceHolder.audioSource.clip.length;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001468C File Offset: 0x0001288C
		public float GetTime()
		{
			if (!this.isActive || this.sourceHolder == null || this.sourceHolder.audioSource == null)
			{
				return 0f;
			}
			return this.sourceHolder.audioSource.time;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000146D8 File Offset: 0x000128D8
		public void SetTime(float time)
		{
			if (!this.isActive || this.sourceHolder == null || this.sourceHolder.audioSource == null || this.sourceHolder.audioSource.clip == null)
			{
				return;
			}
			this.sourceHolder.audioSource.time = Mathf.Clamp(time, 0f, this.sourceHolder.audioSource.clip.length);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00014757 File Offset: 0x00012957
		public float GetPitchValue()
		{
			return this.sourceHolder.audioSource.pitch;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00014769 File Offset: 0x00012969
		public bool Pause()
		{
			if (!this.isActive)
			{
				return false;
			}
			this.sourceHolder.Pause();
			return true;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00014781 File Offset: 0x00012981
		public bool UnPause()
		{
			if (!this.isActive)
			{
				return false;
			}
			this.sourceHolder.UnPause();
			return true;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00014799 File Offset: 0x00012999
		public void PauseIfActive()
		{
			if (!this.isActive)
			{
				return;
			}
			this.sourceHolder.Pause();
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000147AF File Offset: 0x000129AF
		public void UnPauseIfActive()
		{
			if (!this.isActive)
			{
				return;
			}
			this.sourceHolder.UnPause();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x000147C5 File Offset: 0x000129C5
		public bool Stop()
		{
			if (!this.isActive)
			{
				return false;
			}
			this.sourceHolder.Stop();
			return true;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x000147DD File Offset: 0x000129DD
		private void OnSoundPlayedHandler()
		{
			this.isActive = false;
			this.sourceHolder.OnSoundPlayed -= this.OnSoundPlayedHandler;
			Action action = this.OnSoundPlayed;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001480D File Offset: 0x00012A0D
		public void SetFadeInFadeOut(float fadeInSeconds, float fadeOutSeconds)
		{
			this.sourceHolder.SetFadeInFadeOut(fadeInSeconds, fadeOutSeconds);
			this.sourceHolder.UpdateVolumeWhilePlaying();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00014827 File Offset: 0x00012A27
		public bool SetMixerGroup(AudioMixerGroup group)
		{
			if (!this.isActive || this.sourceHolder == null || this.sourceHolder.audioSource == null)
			{
				return false;
			}
			this.sourceHolder.audioSource.outputAudioMixerGroup = group;
			return true;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00014866 File Offset: 0x00012A66
		public bool IsPaused
		{
			get
			{
				return this.sourceHolder != null && this.sourceHolder.IsPaused;
			}
		}

		// Token: 0x04000198 RID: 408
		private SoundController sourceHolder;

		// Token: 0x04000199 RID: 409
		private bool isActive;

		// Token: 0x0400019A RID: 410
		private float defaultVolume;

		// Token: 0x0400019B RID: 411
		private float defaultPitch;

		// Token: 0x0400019C RID: 412
		private float defaultPanning;

		// Token: 0x0400019D RID: 413
		private Action onSoundPlayed;
	}
}
