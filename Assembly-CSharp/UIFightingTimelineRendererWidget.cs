using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009CA RID: 2506
public class UIFightingTimelineRendererWidget : LazyWidget<UIFightingTimelineRendererData>
{
	// Token: 0x060042B3 RID: 17075 RVA: 0x0013C94C File Offset: 0x0013AB4C
	public override void Init()
	{
		base.Init();
		if (this.fightingLinePrefab != null)
		{
			this.fightingLinePrefab.gameObject.SetActive(false);
		}
	}

	// Token: 0x060042B4 RID: 17076 RVA: 0x0013C974 File Offset: 0x0013AB74
	public override void Hide()
	{
		this.UnsubscribeFromCapturePoints();
		if (this.data != null)
		{
			this.data.UnsubscribeFromProcessor();
			this.data.OnProgressChanged -= this.HandleProgressNormalized;
		}
		foreach (UIFightingLine uifightingLine in this.drawnFightingLines)
		{
			if (!(uifightingLine == null))
			{
				uifightingLine.Clear();
				global::UnityEngine.Object.Destroy(uifightingLine.gameObject);
			}
		}
		this.drawnFightingLines.Clear();
		base.Hide();
	}

	// Token: 0x060042B5 RID: 17077 RVA: 0x0013CA1C File Offset: 0x0013AC1C
	protected override void SetData(UIFightingTimelineRendererData data)
	{
		base.SetData(data);
		if (((data != null) ? data.Preset : null) == null || data.CurrentLevel == null || this.fightingLinePrefab == null || this.linesContainer == null)
		{
			return;
		}
		for (int i = 0; i < data.Preset.lines.Count; i++)
		{
			if (i < data.CurrentLevel.FightingLines.Count)
			{
				FightingLevelPreset.FightingLineData fightingLineData = data.Preset.lines[i];
				UIFightingLine uifightingLine = this.fightingLinePrefab.Copy(null, true, "");
				uifightingLine.Draw(fightingLineData, data.TotalTime, i, data.CurrentLevel.FightingLines[i]);
				this.drawnFightingLines.Add(uifightingLine);
				uifightingLine.transform.SetParent(this.linesContainer);
				uifightingLine.transform.SetAsLastSibling();
				uifightingLine.gameObject.SetActive(true);
			}
		}
		this.linesContainer.anchoredPosition = new Vector2(0f, -8f);
		data.OnProgressChanged += this.HandleProgressNormalized;
		foreach (UIFightingLine uifightingLine2 in this.drawnFightingLines)
		{
			uifightingLine2.slider.anchoredPosition = Vector2.zero;
		}
		this.UpdateDirectionIcons();
		this.SubscribeToCapturePoints();
	}

	// Token: 0x060042B6 RID: 17078 RVA: 0x0013CBA0 File Offset: 0x0013ADA0
	public override void Redraw()
	{
		base.Redraw();
		this.RefreshLinesContainer();
		this.UpdateRightGroup();
	}

	// Token: 0x060042B7 RID: 17079 RVA: 0x0013CBB4 File Offset: 0x0013ADB4
	private void UpdateRightGroup()
	{
		this.UpdateTimeLabel();
		this.UpdateShieldsLabel();
		this.RefreshLinesContainer();
		this.UpdateRightGroupPosition();
	}

	// Token: 0x060042B8 RID: 17080 RVA: 0x0013CBD0 File Offset: 0x0013ADD0
	private void HandleProgressNormalized(float curProgress)
	{
		this.UpdateTimeLabel();
		foreach (UIFightingLine uifightingLine in this.drawnFightingLines)
		{
			float x = uifightingLine.viewportRect.sizeDelta.x;
			bool flag = uifightingLine.fightingLine != null && uifightingLine.fightingLine.AreAllSpawnZonesDisabled;
			float num = curProgress * this.data.TotalTime;
			bool flag2 = uifightingLine.secondsAtEndOfLastSpawnPhase >= 0f && num >= uifightingLine.secondsAtEndOfLastSpawnPhase;
			uifightingLine.slider.anchoredPosition = new Vector2(Mathf.Lerp(0f, x, curProgress), 0f);
			if (flag || flag2)
			{
				uifightingLine.overlayProgressObj.SetActive(false);
				uifightingLine.overlayFullObj.SetActive(true);
			}
			else
			{
				uifightingLine.overlayProgressObj.SetActive(true);
				uifightingLine.overlayFullObj.SetActive(false);
			}
		}
	}

	// Token: 0x060042B9 RID: 17081 RVA: 0x0013CCDC File Offset: 0x0013AEDC
	private Sprite GetDirectionIcon(int lineIdx)
	{
		if (this.directionIcons == null || lineIdx < 0 || lineIdx >= this.directionIcons.Length)
		{
			return null;
		}
		return this.directionIcons[lineIdx];
	}

	// Token: 0x060042BA RID: 17082 RVA: 0x0013CD00 File Offset: 0x0013AF00
	private void UpdateDirectionIcons()
	{
		for (int i = 0; i < this.drawnFightingLines.Count; i++)
		{
			UIFightingTimelineRendererData data = this.data;
			if (((data != null) ? data.CurrentLevel : null) == null || i >= this.data.CurrentLevel.FightingLines.Count)
			{
				this.drawnFightingLines[i].SetDirectionIcon(null);
			}
			else
			{
				this.drawnFightingLines[i].SetDirectionIcon(this.GetDirectionIcon(i));
			}
		}
	}

	// Token: 0x060042BB RID: 17083 RVA: 0x0013CD84 File Offset: 0x0013AF84
	private void UpdateTimeLabel()
	{
		if (!(this.timeLabel == null))
		{
			UIFightingTimelineRendererData data = this.data;
			if (!(((data != null) ? data.Processor : null) == null))
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds((double)Mathf.Max(0f, this.data.TotalTime - this.data.Processor.CurrentProgress));
				this.timeLabel.text = string.Format("{0}{1:00}:{2:00}", "icon_time".FontIcon(), (int)timeSpan.TotalMinutes, timeSpan.Seconds);
				return;
			}
		}
	}

	// Token: 0x060042BC RID: 17084 RVA: 0x0013CE20 File Offset: 0x0013B020
	private void UpdateShieldsLabel()
	{
		if (this.shieldsLabel == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		foreach (UIFightingLine uifightingLine in this.drawnFightingLines)
		{
			foreach (UIFightingCaptureIcon uifightingCaptureIcon in uifightingLine.CaptureIcons)
			{
				if (!(uifightingCaptureIcon.CapturePoint == null))
				{
					num++;
					if (uifightingCaptureIcon.CapturePoint.OwnedByTeam == LazyConsts.Fighting.TeamType.Player)
					{
						num2++;
					}
				}
			}
		}
		if (num <= 0)
		{
			this.shieldsLabel.gameObject.SetActive(false);
			this.spaceBetweenTimeAndShields.SetActive(false);
			return;
		}
		this.shieldsLabel.gameObject.SetActive(true);
		this.spaceBetweenTimeAndShields.SetActive(true);
		TextStyle textStyle = ((num2 < num) ? this.shieldsTextStyleRed : this.shieldsTextStyleGreen);
		string text = ((textStyle != null) ? textStyle.ApplyStyleToString(num2.ToString(), false, true) : num2.ToString());
		this.shieldsLabel.text = string.Format("{0}{1}/{2}", "icon-flag_secondary-blue-timer".FontIcon(), text, num);
	}

	// Token: 0x060042BD RID: 17085 RVA: 0x0013CF78 File Offset: 0x0013B178
	private void SubscribeToCapturePoints()
	{
		this.UnsubscribeFromCapturePoints();
		foreach (UIFightingLine uifightingLine in this.drawnFightingLines)
		{
			foreach (UIFightingCaptureIcon uifightingCaptureIcon in uifightingLine.CaptureIcons)
			{
				if (!(uifightingCaptureIcon.CapturePoint == null) && !this.subscribedCapturePoints.Contains(uifightingCaptureIcon.CapturePoint))
				{
					uifightingCaptureIcon.CapturePoint.OnCapturedByTeam += this.HandleCapturePointCaptured;
					this.subscribedCapturePoints.Add(uifightingCaptureIcon.CapturePoint);
				}
			}
		}
	}

	// Token: 0x060042BE RID: 17086 RVA: 0x0013D04C File Offset: 0x0013B24C
	private void UnsubscribeFromCapturePoints()
	{
		foreach (FightingCapturePoint fightingCapturePoint in this.subscribedCapturePoints)
		{
			if (fightingCapturePoint != null)
			{
				fightingCapturePoint.OnCapturedByTeam -= this.HandleCapturePointCaptured;
			}
		}
		this.subscribedCapturePoints.Clear();
	}

	// Token: 0x060042BF RID: 17087 RVA: 0x0013D0C0 File Offset: 0x0013B2C0
	private void HandleCapturePointCaptured(FightingCapturePoint capturePoint)
	{
		this.UpdateShieldsLabel();
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x0013D0C8 File Offset: 0x0013B2C8
	private void UpdateRightGroupPosition()
	{
		if (this.rightGroup == null || this.drawnFightingLines.Count == 0)
		{
			return;
		}
		float num = 0f;
		foreach (UIFightingLine uifightingLine in this.drawnFightingLines)
		{
			if (!(uifightingLine.CapturePointsParent == null))
			{
				num = Mathf.Max(num, uifightingLine.CapturePointsWidth);
			}
		}
		this.rightGroup.anchoredPosition = new Vector2(num + this.rightGroupOffsetFromIcons, 0f);
	}

	// Token: 0x060042C1 RID: 17089 RVA: 0x0013D170 File Offset: 0x0013B370
	private void RefreshLinesContainer()
	{
		if (this.linesContainer != null)
		{
			this.linesContainer.RefreshContentFitter();
		}
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400340B RID: 13323
	public static readonly float TIME_TO_SIZE = 10f;

	// Token: 0x0400340C RID: 13324
	[Header("Viewport Setup")]
	[SerializeField]
	private RectTransform linesContainer;

	// Token: 0x0400340D RID: 13325
	[SerializeField]
	private RectTransform rightGroup;

	// Token: 0x0400340E RID: 13326
	[SerializeField]
	private float rightGroupOffsetFromIcons;

	// Token: 0x0400340F RID: 13327
	[SerializeField]
	private UIFightingLine fightingLinePrefab;

	// Token: 0x04003410 RID: 13328
	[SerializeField]
	private Sprite[] directionIcons;

	// Token: 0x04003411 RID: 13329
	public TextMeshProUGUI timeLabel;

	// Token: 0x04003412 RID: 13330
	public GameObject spaceBetweenTimeAndShields;

	// Token: 0x04003413 RID: 13331
	public TextMeshProUGUI shieldsLabel;

	// Token: 0x04003414 RID: 13332
	public TextStyle shieldsTextStyleRed;

	// Token: 0x04003415 RID: 13333
	public TextStyle shieldsTextStyleGreen;

	// Token: 0x04003416 RID: 13334
	private List<UIFightingLine> drawnFightingLines = new List<UIFightingLine>();

	// Token: 0x04003417 RID: 13335
	private List<FightingCapturePoint> subscribedCapturePoints = new List<FightingCapturePoint>();
}
