using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200081E RID: 2078
public abstract class UIBasicFade : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x06003533 RID: 13619 RVA: 0x0010025C File Offset: 0x000FE45C
	public bool IsFadeShowing
	{
		get
		{
			return base.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x00100269 File Offset: 0x000FE469
	public virtual void Init()
	{
		this.fadeFlags.Init(null, true);
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x00100278 File Offset: 0x000FE478
	public void Fade(float fadeTime = 0.3f, Action onInCompleted = null, Action onOutCompleted = null)
	{
		this.FadeIn(fadeTime, delegate
		{
			Action onInCompleted2 = onInCompleted;
			if (onInCompleted2 != null)
			{
				onInCompleted2();
			}
			this.FadeOut(fadeTime, onOutCompleted, FadeFlag.Common);
		}, FadeFlag.Common, false);
	}

	// Token: 0x06003536 RID: 13622 RVA: 0x001002C1 File Offset: 0x000FE4C1
	public void FadeIn(Action onComplete, FadeFlag fadeFlag = FadeFlag.Common, bool blockInterceptions = false)
	{
		this.FadeIn(0.3f, onComplete, fadeFlag, blockInterceptions);
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x001002D4 File Offset: 0x000FE4D4
	public void FadeIn(float fadeTime = 0.3f, Action onComplete = null, FadeFlag fadeFlag = FadeFlag.Common, bool blockInterceptions = false)
	{
		if (this.locked)
		{
			Debug.LogWarning("Fade in is already in progress");
			return;
		}
		if (fadeFlag == FadeFlag.All)
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(FadeFlag)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					FadeFlag fadeFlag2 = (FadeFlag)obj;
					if (fadeFlag2 != FadeFlag.All)
					{
						this.fadeFlags.UpdateFlag(fadeFlag2, false);
					}
				}
				goto IL_0087;
			}
		}
		this.fadeFlags.UpdateFlag(fadeFlag, false);
		IL_0087:
		if (!base.gameObject.activeInHierarchy)
		{
			this.SetBlackState();
			this.EnsureTweenStopped();
			this.locked = blockInterceptions;
			this.currentTweener = this.blackoutCanvas.DOFade(1f, fadeTime).OnComplete(delegate
			{
				Action onComplete3 = onComplete;
				if (onComplete3 != null)
				{
					onComplete3();
				}
				this.locked = false;
			});
			return;
		}
		Action onComplete2 = onComplete;
		if (onComplete2 == null)
		{
			return;
		}
		onComplete2();
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x001003D4 File Offset: 0x000FE5D4
	public void FadeInInstant(FadeFlag fadeFlag = FadeFlag.Common)
	{
		if (fadeFlag == FadeFlag.All)
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(FadeFlag)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					FadeFlag fadeFlag2 = (FadeFlag)obj;
					if (fadeFlag2 != FadeFlag.All)
					{
						this.fadeFlags.UpdateFlag(fadeFlag2, false);
					}
				}
				goto IL_0060;
			}
		}
		this.fadeFlags.UpdateFlag(fadeFlag, false);
		IL_0060:
		this.SetBlackState();
		this.blackoutCanvas.alpha = 1f;
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x00100468 File Offset: 0x000FE668
	public void FadeOutInstant(FadeFlag fadeFlag = FadeFlag.Common)
	{
		if (fadeFlag == FadeFlag.All)
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(FadeFlag)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					FadeFlag fadeFlag2 = (FadeFlag)obj;
					if (fadeFlag2 != FadeFlag.All)
					{
						this.fadeFlags.UpdateFlag(fadeFlag2, true);
					}
				}
				goto IL_0060;
			}
		}
		this.fadeFlags.UpdateFlag(fadeFlag, true);
		IL_0060:
		if (!this.fadeFlags.ResultFlag)
		{
			return;
		}
		this.Disable();
		this.blackoutCanvas.alpha = 1f;
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x0010050C File Offset: 0x000FE70C
	public void FadeOut(Action onComplete, FadeFlag fadeFlag = FadeFlag.Common)
	{
		this.FadeOut(0.3f, onComplete, fadeFlag);
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x0010051C File Offset: 0x000FE71C
	public void FadeOut(float fadeTime = 0.3f, Action onComplete = null, FadeFlag fadeFlag = FadeFlag.Common)
	{
		if (this.locked)
		{
			Debug.LogWarning("Fade out is already in progress");
			return;
		}
		if (fadeFlag == FadeFlag.All)
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(FadeFlag)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					FadeFlag fadeFlag2 = (FadeFlag)obj;
					if (fadeFlag2 != FadeFlag.All)
					{
						this.fadeFlags.UpdateFlag(fadeFlag2, true);
					}
				}
				goto IL_0087;
			}
		}
		this.fadeFlags.UpdateFlag(fadeFlag, true);
		IL_0087:
		if (this.fadeFlags.ResultFlag)
		{
			this.EnsureTweenStopped();
			this.currentTweener = this.blackoutCanvas.DOFade(0f, fadeTime).OnComplete(delegate
			{
				this.Disable();
				Action onComplete3 = onComplete;
				if (onComplete3 == null)
				{
					return;
				}
				onComplete3();
			});
			return;
		}
		Action onComplete2 = onComplete;
		if (onComplete2 == null)
		{
			return;
		}
		onComplete2();
	}

	// Token: 0x0600353C RID: 13628 RVA: 0x00027874 File Offset: 0x00025A74
	private void Disable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600353D RID: 13629 RVA: 0x0010060C File Offset: 0x000FE80C
	public void SetBlackState()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.blackoutCanvas.alpha = 0f;
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x00100637 File Offset: 0x000FE837
	private void EnsureTweenStopped()
	{
		if (this.currentTweener != null && this.currentTweener.IsActive())
		{
			this.currentTweener.Kill(false);
		}
	}

	// Token: 0x04002AA9 RID: 10921
	protected const float FADE_DURATION = 0.3f;

	// Token: 0x04002AAA RID: 10922
	[SerializeField]
	protected CanvasGroup blackoutCanvas;

	// Token: 0x04002AAB RID: 10923
	private Tween currentTweener;

	// Token: 0x04002AAC RID: 10924
	private bool locked;

	// Token: 0x04002AAD RID: 10925
	private MultiFlagAND<FadeFlag> fadeFlags = new MultiFlagAND<FadeFlag>();
}
