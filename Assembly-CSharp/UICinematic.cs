using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000838 RID: 2104
[RequireComponent(typeof(Canvas))]
public class UICinematic : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x060035CA RID: 13770 RVA: 0x0010265C File Offset: 0x0010085C
	public void Init()
	{
		base.gameObject.SetActive(false);
		this.canvas = base.GetComponent<Canvas>();
		this.canvas.overrideSorting = true;
		this.canvas.sortingOrder = 300;
		GameSettings.OnResolutionChanged += this.OnResolutionChanged;
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x001026B0 File Offset: 0x001008B0
	public void EnableCinematic(Action onCompleted = null, bool instant = false)
	{
		if (!base.gameObject.activeSelf || this.isAnimatingDisabling)
		{
			this.CalculateHeight();
			if (!instant)
			{
				if (!this.isAnimatingEnabling)
				{
					if (this.isAnimatingDisabling)
					{
						Action action = this.onCompleted;
						if (action != null)
						{
							action();
						}
						this.onCompleted = null;
						this.isAnimatingDisabling = false;
						this.cinematicUp.DOKill(false);
						this.cinematicDown.DOKill(false);
					}
					this.onCompleted = onCompleted;
					LazyUI.Get<HUD>().SetDisableState(HudStateType.Cinematic, false, null);
					base.gameObject.SetActive(true);
					this.isAnimatingEnabling = true;
					float y = this.cinematicUp.sizeDelta.y;
					this.cinematicUp.DOAnchorPosY(-y, this.duration, false).SetEase(this.enableEaseType);
					this.cinematicDown.DOAnchorPosY(y, this.duration, false).SetEase(this.enableEaseType).OnComplete(delegate
					{
						this.isAnimatingEnabling = false;
						Action action6 = onCompleted;
						if (action6 == null)
						{
							return;
						}
						action6();
					});
					return;
				}
				Action action2 = onCompleted;
				if (action2 == null)
				{
					return;
				}
				action2();
				return;
			}
			else
			{
				if (this.isAnimatingDisabling)
				{
					Action action3 = this.onCompleted;
					if (action3 != null)
					{
						action3();
					}
					this.onCompleted = null;
					this.isAnimatingDisabling = false;
					this.cinematicUp.DOKill(false);
					this.cinematicDown.DOKill(false);
				}
				LazyUI.Get<HUD>().SetDisableState(HudStateType.Cinematic, false, null);
				base.gameObject.SetActive(true);
				float y2 = this.cinematicUp.sizeDelta.y;
				this.cinematicUp.anchoredPosition = new Vector2(0f, -y2);
				this.cinematicDown.anchoredPosition = new Vector2(0f, y2);
				Action action4 = onCompleted;
				if (action4 == null)
				{
					return;
				}
				action4();
				return;
			}
		}
		else
		{
			Debug.LogWarning("Trying to enable already active cinematics");
			Action action5 = onCompleted;
			if (action5 == null)
			{
				return;
			}
			action5();
			return;
		}
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x001028A4 File Offset: 0x00100AA4
	public void DisableCinematic(Action onCompleted = null, bool instant = false)
	{
		if (base.gameObject.activeSelf)
		{
			if (!instant)
			{
				if (!this.isAnimatingDisabling)
				{
					if (this.isAnimatingEnabling)
					{
						Action action = this.onCompleted;
						if (action != null)
						{
							action();
						}
						this.onCompleted = null;
						this.isAnimatingEnabling = false;
						this.cinematicUp.DOKill(false);
						this.cinematicDown.DOKill(false);
					}
					this.onCompleted = onCompleted;
					this.isAnimatingDisabling = true;
					this.cinematicUp.DOAnchorPosY(0f, this.duration, false).SetEase(this.disableEaseType);
					this.cinematicDown.DOAnchorPosY(0f, this.duration, false).SetEase(this.disableEaseType).OnComplete(delegate
					{
						this.isAnimatingDisabling = false;
						Action action6 = onCompleted;
						if (action6 != null)
						{
							action6();
						}
						LazyUI.Get<HUD>().SetDisableState(HudStateType.Cinematic, true, null);
						this.gameObject.SetActive(false);
					});
					return;
				}
				Action action2 = onCompleted;
				if (action2 == null)
				{
					return;
				}
				action2();
				return;
			}
			else
			{
				if (this.isAnimatingEnabling)
				{
					Action action3 = this.onCompleted;
					if (action3 != null)
					{
						action3();
					}
					this.onCompleted = null;
					this.isAnimatingEnabling = false;
					this.cinematicUp.DOKill(false);
					this.cinematicDown.DOKill(false);
				}
				this.isAnimatingDisabling = false;
				this.cinematicUp.anchoredPosition = Vector2.zero;
				this.cinematicDown.anchoredPosition = Vector2.zero;
				LazyUI.Get<HUD>().SetDisableState(HudStateType.Cinematic, true, null);
				base.gameObject.SetActive(false);
				Action action4 = onCompleted;
				if (action4 == null)
				{
					return;
				}
				action4();
				return;
			}
		}
		else
		{
			Debug.LogWarning("Trying to disable already non-active cinematics");
			Action action5 = onCompleted;
			if (action5 == null)
			{
				return;
			}
			action5();
			return;
		}
	}

	// Token: 0x060035CD RID: 13773 RVA: 0x00102A4F File Offset: 0x00100C4F
	private void OnResolutionChanged(IntVector2 _)
	{
		this.CalculateHeight();
	}

	// Token: 0x060035CE RID: 13774 RVA: 0x00102A58 File Offset: 0x00100C58
	private void CalculateHeight()
	{
		float num = GUIElements.Instance.Root.sizeDelta.y * this.heightRatio;
		if (GameSettings.Instance.GetResolutionIntVector2().y <= 720)
		{
			num *= 0.7f;
		}
		this.cinematicUp.sizeDelta = new Vector2(0f, num);
		this.cinematicDown.sizeDelta = new Vector2(0f, num);
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x00102ACB File Offset: 0x00100CCB
	private void OnDestroy()
	{
		GameSettings.OnResolutionChanged -= this.OnResolutionChanged;
	}

	// Token: 0x04002B13 RID: 11027
	[SerializeField]
	private RectTransform cinematicUp;

	// Token: 0x04002B14 RID: 11028
	[SerializeField]
	private RectTransform cinematicDown;

	// Token: 0x04002B15 RID: 11029
	private Canvas canvas;

	// Token: 0x04002B16 RID: 11030
	[Space]
	[SerializeField]
	[Range(0f, 10f)]
	public float duration = 1f;

	// Token: 0x04002B17 RID: 11031
	[SerializeField]
	private Ease enableEaseType = Ease.Linear;

	// Token: 0x04002B18 RID: 11032
	[SerializeField]
	private Ease disableEaseType = Ease.Linear;

	// Token: 0x04002B19 RID: 11033
	[SerializeField]
	[Range(0f, 0.5f)]
	private float heightRatio = 0.1f;

	// Token: 0x04002B1A RID: 11034
	private const float LowResolutionHeightMultiplier = 0.7f;

	// Token: 0x04002B1B RID: 11035
	private bool isAnimatingEnabling;

	// Token: 0x04002B1C RID: 11036
	private bool isAnimatingDisabling;

	// Token: 0x04002B1D RID: 11037
	private Action onCompleted;
}
