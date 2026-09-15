using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x020008C8 RID: 2248
[RequireComponent(typeof(Canvas))]
public class UIFadeWithText : UIBasicFade
{
	// Token: 0x06003AB8 RID: 15032 RVA: 0x001185F7 File Offset: 0x001167F7
	public override void Init()
	{
		base.Init();
		this.canvas = base.GetComponent<Canvas>();
		this.canvas.overrideSorting = true;
		this.canvas.sortingOrder = 800;
	}

	// Token: 0x06003AB9 RID: 15033 RVA: 0x00118628 File Offset: 0x00116828
	public void ScreenTextFade(string text, float screenFadeTime = 1f, float textFadeTime = 1f, float pauseTime = 1f, float textHoldTime = 2f, Action onTextShown = null, Action onScreenFadeOut = null, TextStyle overrideTextStyle = null)
	{
		if (overrideTextStyle != null)
		{
			overrideTextStyle.ApplyStyle(this.textMesh, false, null, null, null);
		}
		else
		{
			this.defaultTextStyle.ApplyStyle(this.textMesh, false, null, null, null);
		}
		Color textColor = this.textMesh.color;
		this.textMesh.text = LLBase.L(text);
		Color textColor2 = textColor;
		textColor2.a = 0f;
		this.textMesh.color = textColor2;
		this.textMesh.gameObject.SetActive(true);
		Action <>9__5;
		TweenCallback <>9__4;
		Action <>9__3;
		TweenCallback <>9__2;
		Action <>9__1;
		base.FadeIn(screenFadeTime, delegate
		{
			float pauseTime2 = pauseTime;
			Action action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate
				{
					TweenerCore<Color, Color, ColorOptions> tweenerCore = this.textMesh.DOColor(textColor, textFadeTime).SetEase(Ease.Linear);
					TweenCallback tweenCallback;
					if ((tweenCallback = <>9__2) == null)
					{
						tweenCallback = (<>9__2 = delegate
						{
							float textHoldTime2 = textHoldTime;
							Action action2;
							if ((action2 = <>9__3) == null)
							{
								action2 = (<>9__3 = delegate
								{
									TweenerCore<Color, Color, ColorOptions> tweenerCore2 = this.textMesh.DOColor(Color.clear, textFadeTime).SetEase(Ease.Linear);
									TweenCallback tweenCallback2;
									if ((tweenCallback2 = <>9__4) == null)
									{
										tweenCallback2 = (<>9__4 = delegate
										{
											Action onTextShown2 = onTextShown;
											if (onTextShown2 != null)
											{
												onTextShown2();
											}
											UIBasicFade <>4__this = this;
											float screenFadeTime2 = screenFadeTime;
											Action action3;
											if ((action3 = <>9__5) == null)
											{
												action3 = (<>9__5 = delegate
												{
													this.textMesh.gameObject.SetActive(false);
													Action onScreenFadeOut2 = onScreenFadeOut;
													if (onScreenFadeOut2 == null)
													{
														return;
													}
													onScreenFadeOut2();
												});
											}
											<>4__this.FadeOut(screenFadeTime2, action3, FadeFlag.Common);
										});
									}
									tweenerCore2.OnComplete(tweenCallback2);
								});
							}
							LazyTimer.AddTimer(textHoldTime2, action2, null);
						});
					}
					tweenerCore.OnComplete(tweenCallback);
				});
			}
			LazyTimer.AddTimer(pauseTime2, action, null);
		}, FadeFlag.Common, false);
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x00118740 File Offset: 0x00116940
	public void ScreenTextFade(List<string> ids, float screenFadeTime = 1f, float textFadeTime = 1f, float pauseTime = 1f, float textHoldTime = 2f, Action onTextShown = null, Action onScreenFadeOut = null, TextStyle overrideTextStyle = null)
	{
		if (overrideTextStyle != null)
		{
			overrideTextStyle.ApplyStyle(this.textMesh, false, null, null, null);
		}
		else
		{
			this.defaultTextStyle.ApplyStyle(this.textMesh, false, null, null, null);
		}
		Color textColor = this.textMesh.color;
		this.textMesh.text = LLBase.L(ids[0]);
		Color textColor2 = textColor;
		textColor2.a = 0f;
		this.textMesh.color = textColor2;
		this.textMesh.gameObject.SetActive(true);
		List<string> remainingPhrases = new List<string>();
		remainingPhrases.AddRange(ids);
		remainingPhrases.RemoveAt(0);
		base.FadeIn(screenFadeTime, delegate
		{
			LazyTimer.AddTimer(pauseTime, new Action(base.<ScreenTextFade>g__ShowText|0), null);
		}, FadeFlag.Common, false);
	}

	// Token: 0x04002E4C RID: 11852
	[SerializeField]
	private TextMeshProUGUI textMesh;

	// Token: 0x04002E4D RID: 11853
	[SerializeField]
	private TextStyle defaultTextStyle;

	// Token: 0x04002E4E RID: 11854
	private Canvas canvas;
}
