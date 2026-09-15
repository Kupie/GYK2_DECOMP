using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace LazyBearTechnology.Preloader
{
	// Token: 0x0200019B RID: 411
	[Serializable]
	public class LazyLogoData
	{
		// Token: 0x06000950 RID: 2384 RVA: 0x0002D000 File Offset: 0x0002B200
		public void Initialize(VideoPlayer videoPlayer = null)
		{
			this.videoPlayer = videoPlayer;
			this.Hide();
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002D010 File Offset: 0x0002B210
		public void Show()
		{
			LazyLogoData.ObjectMode objectMode = this.objectMode;
			if (objectMode != LazyLogoData.ObjectMode.GameObject)
			{
				if (objectMode != LazyLogoData.ObjectMode.Video)
				{
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				this.logo.SetActive(true);
			}
			UnityEvent unityEvent = this.onBeginShowing;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002D054 File Offset: 0x0002B254
		public void Hide()
		{
			LazyLogoData.ObjectMode objectMode = this.objectMode;
			if (objectMode != LazyLogoData.ObjectMode.GameObject)
			{
				if (objectMode != LazyLogoData.ObjectMode.Video)
				{
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				this.logo.SetActive(false);
			}
			UnityEvent unityEvent = this.onEndShowing;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002D095 File Offset: 0x0002B295
		public IEnumerator DoProcess()
		{
			LazyLogoData.<>c__DisplayClass15_0 CS$<>8__locals1 = new LazyLogoData.<>c__DisplayClass15_0();
			LazyLogoData.ShowLengthMode showLengthMode = this.showLengthMode;
			if (showLengthMode != LazyLogoData.ShowLengthMode.TimeLimited)
			{
				if (showLengthMode != LazyLogoData.ShowLengthMode.SyncedToVideo)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (this.videoPlayer == null)
				{
					Debug.LogError("VideoPlayer is null during LazyPreloader. Please call Initialize(videoPlayer).");
				}
				else
				{
					this.videoPlayer.enabled = true;
					this.videoPlayer.clip = this.videoClip;
					this.videoPlayer.time = 0.0;
					this.videoPlayer.isLooping = false;
					this.videoPlayer.Play();
					yield return null;
					LazyPreloader.OnAnimationStarted();
					CS$<>8__locals1.videoIsPlaying = true;
					this.videoPlayer.loopPointReached += CS$<>8__locals1.<DoProcess>g__OnLoopPointReached|0;
					while (CS$<>8__locals1.videoIsPlaying)
					{
						yield return null;
					}
					this.videoPlayer.loopPointReached -= CS$<>8__locals1.<DoProcess>g__OnLoopPointReached|0;
					this.videoPlayer.enabled = false;
					this.videoPlayer.clip = null;
					LazyPreloader.OnAnimationStopped();
				}
			}
			else
			{
				yield return new WaitForSeconds(this.showingTime);
			}
			CS$<>8__locals1 = null;
			yield break;
		}

		// Token: 0x0400059C RID: 1436
		[Range(0f, 5f)]
		public float fadeOutTime = 1f;

		// Token: 0x0400059D RID: 1437
		[Range(0f, 5f)]
		public float fadeInTime = 1f;

		// Token: 0x0400059E RID: 1438
		public LazyLogoData.ShowLengthMode showLengthMode;

		// Token: 0x0400059F RID: 1439
		[Range(0f, 10f)]
		public float showingTime;

		// Token: 0x040005A0 RID: 1440
		public LazyLogoData.ObjectMode objectMode;

		// Token: 0x040005A1 RID: 1441
		public GameObject logo;

		// Token: 0x040005A2 RID: 1442
		public VideoClip videoClip;

		// Token: 0x040005A3 RID: 1443
		private VideoPlayer videoPlayer;

		// Token: 0x040005A4 RID: 1444
		public UnityEvent onBeginShowing;

		// Token: 0x040005A5 RID: 1445
		public UnityEvent onEndShowing;

		// Token: 0x02000214 RID: 532
		public enum ShowLengthMode
		{
			// Token: 0x0400070A RID: 1802
			TimeLimited,
			// Token: 0x0400070B RID: 1803
			SyncedToVideo
		}

		// Token: 0x02000215 RID: 533
		public enum ObjectMode
		{
			// Token: 0x0400070D RID: 1805
			GameObject,
			// Token: 0x0400070E RID: 1806
			Video
		}
	}
}
