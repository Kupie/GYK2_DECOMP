using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace LazyBearTechnology.Preloader
{
	// Token: 0x0200019C RID: 412
	public class LazyPreloader : MonoBehaviour
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x0002D0C2 File Offset: 0x0002B2C2
		private void Awake()
		{
			LazyPreloader.instance = this;
			this.logoList.ForEach(delegate(LazyLogoData x)
			{
				x.Initialize(this.videoPlayer);
			});
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002D0E4 File Offset: 0x0002B2E4
		public void Run(IEnumerator onFinishedCallback, IEnumerator preloaderCoroutine = null, bool waitUntilPreloaderCoroutineEnded = false)
		{
			this.onFinishedCallback = delegate
			{
				this.StartCoroutine(onFinishedCallback);
			};
			this.preloaderCoroutine = preloaderCoroutine;
			this.waitUntilPreloaderCoroutineEnded = waitUntilPreloaderCoroutineEnded;
			base.StartCoroutine(this.RunLogoCoroutine());
			base.StartCoroutine(this.RunPreloaderCoroutine());
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002D13F File Offset: 0x0002B33F
		private IEnumerator RunPreloaderCoroutine()
		{
			if (this.preloaderCoroutine == null)
			{
				yield break;
			}
			for (;;)
			{
				if (this.isAnimationRunning)
				{
					yield return null;
				}
				else
				{
					if (!this.preloaderCoroutine.MoveNext())
					{
						break;
					}
					yield return null;
				}
			}
			this.preloaderCoroutine = null;
			yield break;
			yield break;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002D14E File Offset: 0x0002B34E
		private IEnumerator RunLogoCoroutine()
		{
			int num;
			for (int i = 0; i < this.logoList.Count; i = num + 1)
			{
				LazyLogoData currentLogo = this.logoList[i];
				currentLogo.Show();
				if (currentLogo.fadeInTime > 0f)
				{
					yield return this.DoFadeCoroutine(currentLogo.fadeInTime, true);
				}
				else
				{
					this.SetFadingSpriteAlpha(0f);
				}
				yield return currentLogo.DoProcess();
				if (currentLogo.fadeOutTime > 0f)
				{
					yield return this.DoFadeCoroutine(currentLogo.fadeOutTime, false);
				}
				else
				{
					this.SetFadingSpriteAlpha(1f);
				}
				currentLogo.Hide();
				currentLogo = null;
				num = i;
			}
			yield return new WaitUntil(() => !this.waitUntilPreloaderCoroutineEnded || this.preloaderCoroutine == null);
			Action action = this.onFinishedCallback;
			if (action != null)
			{
				action();
			}
			yield break;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002D15D File Offset: 0x0002B35D
		private IEnumerator DoFadeCoroutine(float fadeOutTime, bool fadeIn)
		{
			float time = 0f;
			float startAlpha = (float)(fadeIn ? 1 : 0);
			float endAlpha = 1f - startAlpha;
			while (time < fadeOutTime)
			{
				float num = Mathf.Lerp(startAlpha, endAlpha, time / fadeOutTime);
				this.SetFadingSpriteAlpha(num);
				time += Time.deltaTime;
				yield return null;
			}
			this.SetFadingSpriteAlpha(endAlpha);
			yield break;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002D17C File Offset: 0x0002B37C
		private void SetFadingSpriteAlpha(float alpha)
		{
			if (this.fadingSprite == null)
			{
				return;
			}
			Color color = this.fadingSprite.color;
			color.a = alpha;
			this.fadingSprite.color = color;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002D1B8 File Offset: 0x0002B3B8
		public static void OnAnimationStarted()
		{
			LazyPreloader.instance.isAnimationRunning = true;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002D1C5 File Offset: 0x0002B3C5
		public static void OnAnimationStopped()
		{
			LazyPreloader.instance.isAnimationRunning = false;
		}

		// Token: 0x040005A6 RID: 1446
		[SerializeField]
		private SpriteRenderer fadingSprite;

		// Token: 0x040005A7 RID: 1447
		[SerializeField]
		private VideoPlayer videoPlayer;

		// Token: 0x040005A8 RID: 1448
		[Space]
		[SerializeField]
		private List<LazyLogoData> logoList = new List<LazyLogoData>();

		// Token: 0x040005A9 RID: 1449
		private Action onFinishedCallback;

		// Token: 0x040005AA RID: 1450
		private IEnumerator preloaderCoroutine;

		// Token: 0x040005AB RID: 1451
		private bool waitUntilPreloaderCoroutineEnded;

		// Token: 0x040005AC RID: 1452
		private bool isAnimationRunning;

		// Token: 0x040005AD RID: 1453
		private static LazyPreloader instance;
	}
}
