using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000C6 RID: 198
	public class SoundController : MonoBehaviour
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000341 RID: 833 RVA: 0x0001129C File Offset: 0x0000F49C
		// (remove) Token: 0x06000342 RID: 834 RVA: 0x000112D4 File Offset: 0x0000F4D4
		public event Action OnSoundPlayed;

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00011309 File Offset: 0x0000F509
		public bool IsPaused
		{
			get
			{
				return this.isPaused;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00011311 File Offset: 0x0000F511
		public bool IsAlive
		{
			get
			{
				return this.audioSource.isPlaying || this.isPaused;
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00011328 File Offset: 0x0000F528
		public void Play()
		{
			this.startTime = Time.time;
			this.isPaused = false;
			this.audioSource.Play();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00011347 File Offset: 0x0000F547
		public void Pause()
		{
			this.audioSource.Pause();
			this.isPaused = true;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0001135B File Offset: 0x0000F55B
		public void UnPause()
		{
			this.audioSource.UnPause();
			this.isPaused = false;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001136F File Offset: 0x0000F56F
		public void Stop()
		{
			if (this.audioSource != null)
			{
				this.audioSource.Stop();
			}
			this.isPaused = false;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00011391 File Offset: 0x0000F591
		public void Reset()
		{
			Action onSoundPlayed = this.OnSoundPlayed;
			if (onSoundPlayed != null)
			{
				onSoundPlayed();
			}
			this.target = null;
			this.soundHandler = null;
			this.id = string.Empty;
			base.transform.position = Vector3.zero;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000113D0 File Offset: 0x0000F5D0
		public void UpdatePosition(Transform microphone)
		{
			if (this.target == null)
			{
				return;
			}
			Vector3 vector;
			switch (this.spatial)
			{
			case SpatialType.sound3D:
				vector = this.target.position;
				break;
			case SpatialType.sound2D:
				vector.x = this.target.position.x;
				vector.y = this.target.position.y;
				vector.z = microphone.position.z;
				break;
			case SpatialType.sound1D:
				vector.x = this.target.position.x;
				vector.y = microphone.position.y;
				vector.z = microphone.position.z;
				break;
			default:
				vector = base.transform.position;
				break;
			}
			base.transform.position = vector;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000114B4 File Offset: 0x0000F6B4
		public void SetFadeInFadeOut(float fadeInSeconds, float fadeOutSeconds)
		{
			if (this.audioSource == null || this.audioSource.clip == null)
			{
				return;
			}
			float num = this.audioSource.clip.length / this.audioSource.pitch;
			if (fadeInSeconds * 2f > num)
			{
				fadeInSeconds = num / 2f;
			}
			if (fadeOutSeconds * 2f > num)
			{
				fadeOutSeconds = num / 2f;
			}
			this.fadeInTime = fadeInSeconds;
			this.fadeOutTime = fadeOutSeconds;
			this.targetVolume = this.audioSource.volume;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00011548 File Offset: 0x0000F748
		public void UpdateVolumeWhilePlaying()
		{
			float num = Time.time - this.startTime;
			if (num < this.fadeInTime)
			{
				this.audioSource.volume = ((this.fadeInTime > 0.01f) ? Mathf.Lerp(0f, this.targetVolume, num / this.fadeInTime) : this.targetVolume);
				return;
			}
			if (this.fadeOutTime > 0.01f)
			{
				float num2 = this.audioSource.clip.length / this.audioSource.pitch - num;
				if (num2 < this.fadeOutTime)
				{
					this.audioSource.volume = Mathf.Lerp(0f, this.targetVolume, num2 / this.fadeOutTime);
					return;
				}
				this.audioSource.volume = this.targetVolume;
			}
		}

		// Token: 0x04000102 RID: 258
		public string id;

		// Token: 0x04000103 RID: 259
		public float startTime;

		// Token: 0x04000104 RID: 260
		public AudioSource audioSource;

		// Token: 0x04000105 RID: 261
		public SoundHandler soundHandler;

		// Token: 0x04000106 RID: 262
		public Transform target;

		// Token: 0x04000107 RID: 263
		public SpatialType spatial;

		// Token: 0x04000108 RID: 264
		private bool isPaused;

		// Token: 0x04000109 RID: 265
		private float targetVolume;

		// Token: 0x0400010A RID: 266
		private float fadeInTime;

		// Token: 0x0400010B RID: 267
		private float fadeOutTime;
	}
}
