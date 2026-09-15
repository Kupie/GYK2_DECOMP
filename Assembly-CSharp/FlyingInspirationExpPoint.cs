using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000915 RID: 2325
public class FlyingInspirationExpPoint : MonoBehaviour, IPoolable
{
	// Token: 0x06003D3A RID: 15674 RVA: 0x00124CB8 File Offset: 0x00122EB8
	public void Fly(Vector3 from, Vector3 to, string icon, Pool pool, float duration, Action onReachedDestination)
	{
		this.pool = pool;
		this.onReachedDestination = onReachedDestination;
		this.isReleased = false;
		this.EnsureLabel();
		if (this.label == null)
		{
			return;
		}
		this.label.raycastTarget = false;
		this.label.tintAllSprites = false;
		this.label.text = FlyingInspirationExpPoint.EnsureSpriteUntinted(icon);
		base.transform.position = from;
		base.transform.localScale = Vector3.one;
		RectTransform rectTransform = base.transform as RectTransform;
		if (rectTransform != null)
		{
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.position = from;
		}
		base.gameObject.SetActive(true);
		this.label.color = Color.white;
		float num = global::UnityEngine.Random.Range(0f, 0.12f);
		this.EnsureTweenStopped();
		Sequence sequence = DOTween.Sequence().SetLink(base.gameObject, LinkBehaviour.KillOnDisable);
		sequence.Append(base.transform.DOMove(to, duration, false).SetEase(Ease.InOutCubic));
		sequence.Join(base.transform.DOScale(1.1f, duration * 0.4f).SetEase(Ease.OutCubic));
		sequence.Insert(duration * 0.4f, base.transform.DOScale(0.8f, duration * 0.6f).SetEase(Ease.InCubic));
		sequence.SetDelay(num);
		sequence.OnComplete(new TweenCallback(this.OnReachedDestination));
		this.moveTween = sequence;
	}

	// Token: 0x06003D3B RID: 15675 RVA: 0x00124E61 File Offset: 0x00123061
	public void StopAndRelease()
	{
		if (this.isReleased)
		{
			return;
		}
		this.onReachedDestination = null;
		this.EnsureTweenStopped();
		this.ReleaseToPool();
	}

	// Token: 0x06003D3C RID: 15676 RVA: 0x00124E7F File Offset: 0x0012307F
	public void OnPoolableObjReleased()
	{
		this.EnsureTweenStopped();
		this.onReachedDestination = null;
	}

	// Token: 0x06003D3D RID: 15677 RVA: 0x00124E8E File Offset: 0x0012308E
	private void OnReachedDestination()
	{
		this.EnsureTweenStopped();
		Action action = this.onReachedDestination;
		this.onReachedDestination = null;
		this.ReleaseToPool();
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003D3E RID: 15678 RVA: 0x00124EB3 File Offset: 0x001230B3
	private void OnDisable()
	{
		this.onReachedDestination = null;
		this.EnsureTweenStopped();
	}

	// Token: 0x06003D3F RID: 15679 RVA: 0x00124EC2 File Offset: 0x001230C2
	private void EnsureLabel()
	{
		if (this.label == null)
		{
			this.label = base.GetComponent<TextMeshProUGUI>();
		}
	}

	// Token: 0x06003D40 RID: 15680 RVA: 0x00124EDE File Offset: 0x001230DE
	private static string EnsureSpriteUntinted(string icon)
	{
		if (string.IsNullOrEmpty(icon) || icon.Contains("tint="))
		{
			return icon;
		}
		return icon.Replace("\">", "\" tint=0>");
	}

	// Token: 0x06003D41 RID: 15681 RVA: 0x00124F07 File Offset: 0x00123107
	private void ReleaseToPool()
	{
		if (this.isReleased)
		{
			return;
		}
		this.isReleased = true;
		Pool pool = this.pool;
		if (pool == null)
		{
			return;
		}
		pool.ReleaseObject<FlyingInspirationExpPoint>(this);
	}

	// Token: 0x06003D42 RID: 15682 RVA: 0x00124F2C File Offset: 0x0012312C
	private void EnsureTweenStopped()
	{
		Tween tween = this.moveTween;
		this.moveTween = null;
		if (tween != null && tween.IsActive())
		{
			tween.Kill(false);
		}
	}

	// Token: 0x04003014 RID: 12308
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04003015 RID: 12309
	private Pool pool;

	// Token: 0x04003016 RID: 12310
	private Tween moveTween;

	// Token: 0x04003017 RID: 12311
	private Action onReachedDestination;

	// Token: 0x04003018 RID: 12312
	private bool isReleased;
}
