using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200087F RID: 2175
public class UIResElement : LazyWidget<UIResElementData>
{
	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x06003798 RID: 14232 RVA: 0x0010C2C5 File Offset: 0x0010A4C5
	public UIResElementData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x0010C2CD File Offset: 0x0010A4CD
	public void Init(Action<UIResElement> onAnimationCompleted)
	{
		this.onAnimationCompleted = onAnimationCompleted;
	}

	// Token: 0x0600379A RID: 14234 RVA: 0x0010C2D6 File Offset: 0x0010A4D6
	public override void Redraw()
	{
		base.Redraw();
		this.onShowTimeComplete = this.data.OnShowTimeComplete;
		this.DoAppearAnimation();
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x0010C2F8 File Offset: 0x0010A4F8
	private void DoAppearAnimation()
	{
		this.FormLabelAndApplyStyle();
		if (this.data.DisplayingType == UIGameResDisplayingType.AppearOverTargetType)
		{
			this.label.alpha = 0f;
			this.label.transform.localPosition = Vector3.zero;
			this.sequence = DOTween.Sequence();
			this.sequence.Join(this.label.DOFade(1f, this.appearingTime));
			this.sequence.Join(this.label.transform.DOLocalMove(this.moveOffset, this.moveTime, false).SetEase(this.easeType));
			this.sequence.AppendInterval(this.idleTime);
			this.sequence.Append(this.label.DOFade(0f, this.hidingTime).OnComplete(new TweenCallback(this.OnAnimationEnded)));
			return;
		}
		this.originalPosition = CameraSystem.WorldToScreenPoint(this.data.StartPosition);
		this.currentFlyingTime = 0f;
		this.label.alpha = 1f;
		this.label.transform.localPosition = Vector3.zero;
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0010C42D File Offset: 0x0010A62D
	public void ForceHide()
	{
		Sequence sequence = this.sequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.OnAnimationEnded();
	}

	// Token: 0x0600379D RID: 14237 RVA: 0x0010C447 File Offset: 0x0010A647
	private void OnAnimationEnded()
	{
		Action<UIResElementData> action = this.onShowTimeComplete;
		if (action != null)
		{
			action(this.data);
		}
		Action<UIResElement> action2 = this.onAnimationCompleted;
		if (action2 == null)
		{
			return;
		}
		action2(this);
	}

	// Token: 0x0600379E RID: 14238 RVA: 0x0010C474 File Offset: 0x0010A674
	private void LateUpdate()
	{
		if (this.data.HasTarget && this.data.Target == null)
		{
			this.ForceHide();
			return;
		}
		if (this.data.DisplayingType != UIGameResDisplayingType.AppearOverTargetType)
		{
			Vector3 vector = (this.data.IsUITarget ? this.data.Target.position : CameraSystem.WorldToScreenPoint(this.data.Target.position));
			this.currentFlyingTime += Time.deltaTime;
			float num = this.currentFlyingTime / 0.5f;
			base.transform.position = Vector3.Lerp(this.originalPosition, vector, num);
			if (num >= 1f)
			{
				this.OnAnimationEnded();
			}
			return;
		}
		if (this.data.IsUITarget)
		{
			base.transform.position = this.data.StartPosition;
			return;
		}
		base.transform.position = CameraSystem.WorldToScreenPoint(this.data.HasTarget ? this.data.Target.position : this.data.StartPosition);
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x0010C590 File Offset: 0x0010A790
	private void FormLabelAndApplyStyle()
	{
		int showValue = this.data.GetShowValue();
		string text = ((showValue != 0) ? (showValue.ToString() + this.data.IconId.FontIcon()) : this.data.IconId.FontIcon());
		this.label.text = ((showValue > 0) ? ("+" + text) : text);
		string iconId = this.data.IconId;
		if (iconId == "energy")
		{
			this.labelTextStyle.SetTextStyle(this.energyTextStyle);
			return;
		}
		if (!(iconId == "insanity"))
		{
			this.labelTextStyle.SetTextStyle(this.commonTextStyle);
			return;
		}
		this.labelTextStyle.SetTextStyle(this.insanityTextStyle);
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x0010C655 File Offset: 0x0010A855
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIResElementData("hint_inspiration", UIGameResDisplayingType.AppearOverTargetType, "hint_inspiration", 2f, null, false, null));
	}

	// Token: 0x04002C42 RID: 11330
	private const float FOLLOW_TARGET_TIME = 0.5f;

	// Token: 0x04002C43 RID: 11331
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002C44 RID: 11332
	[SerializeField]
	private TextStyleComponent labelTextStyle;

	// Token: 0x04002C45 RID: 11333
	[SerializeField]
	private TextStyle commonTextStyle;

	// Token: 0x04002C46 RID: 11334
	[SerializeField]
	private TextStyle energyTextStyle;

	// Token: 0x04002C47 RID: 11335
	[SerializeField]
	private TextStyle insanityTextStyle;

	// Token: 0x04002C48 RID: 11336
	[SerializeField]
	private float appearingTime;

	// Token: 0x04002C49 RID: 11337
	[SerializeField]
	private float moveTime;

	// Token: 0x04002C4A RID: 11338
	[SerializeField]
	private float idleTime;

	// Token: 0x04002C4B RID: 11339
	[SerializeField]
	private float hidingTime;

	// Token: 0x04002C4C RID: 11340
	[SerializeField]
	private Vector3 moveOffset;

	// Token: 0x04002C4D RID: 11341
	[SerializeField]
	private Ease easeType;

	// Token: 0x04002C4E RID: 11342
	private Sequence sequence;

	// Token: 0x04002C4F RID: 11343
	private Action<UIResElement> onAnimationCompleted;

	// Token: 0x04002C50 RID: 11344
	private Action<UIResElementData> onShowTimeComplete;

	// Token: 0x04002C51 RID: 11345
	private Vector3 originalPosition;

	// Token: 0x04002C52 RID: 11346
	private float currentFlyingTime;
}
