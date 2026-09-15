using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009C5 RID: 2501
public class UIFightingLine : MonoBehaviour
{
	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x06004292 RID: 17042 RVA: 0x0013C02F File Offset: 0x0013A22F
	public IReadOnlyList<UIFightingCaptureIcon> CaptureIcons
	{
		get
		{
			return this.captureIcons;
		}
	}

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x06004293 RID: 17043 RVA: 0x0013C037 File Offset: 0x0013A237
	public RectTransform CapturePointsParent
	{
		get
		{
			return this.capturePointsParent;
		}
	}

	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x06004294 RID: 17044 RVA: 0x0013C03F File Offset: 0x0013A23F
	public float CapturePointsWidth
	{
		get
		{
			if (!(this.capturePointsParent != null))
			{
				return 0f;
			}
			return this.capturePointsParent.sizeDelta.x;
		}
	}

	// Token: 0x06004295 RID: 17045 RVA: 0x0013C065 File Offset: 0x0013A265
	public void SetDirectionIcon(Sprite sprite)
	{
		if (this.directionIcon == null)
		{
			return;
		}
		this.directionIcon.sprite = sprite;
		this.directionIcon.gameObject.SetActive(sprite != null);
		this.directionIcon.SetNativeSize();
	}

	// Token: 0x06004296 RID: 17046 RVA: 0x0013C0A4 File Offset: 0x0013A2A4
	public void Draw(FightingLevelPreset.FightingLineData data, float totalTime, int lineIndex, FightingLine line)
	{
		UIFightingLine.<>c__DisplayClass22_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		this.fightingLine = line;
		this.lineIndex = lineIndex;
		this.secondsAtEndOfLastSpawnPhase = data.SecondsAtEndOfLastSpawnEnemyPhase();
		CS$<>8__locals1.mergedSpawnDuration = 0;
		foreach (FightingPhaseData fightingPhaseData in data.phases)
		{
			if (fightingPhaseData is FightingPhaseSpawnEnemiesData)
			{
				CS$<>8__locals1.mergedSpawnDuration += fightingPhaseData.duration;
			}
			else
			{
				this.<Draw>g__FlushMergedSpawnSegment|22_0(ref CS$<>8__locals1);
				LayoutElement layoutElement = null;
				if (fightingPhaseData is FightingPhasePauseData)
				{
					layoutElement = this.pausePhase.Copy(this.pausePhase.transform.parent, true, "Pause Phase");
				}
				if (layoutElement != null)
				{
					layoutElement.preferredWidth = (float)fightingPhaseData.duration * UIFightingTimelineRendererWidget.TIME_TO_SIZE;
					layoutElement.transform.SetParent(base.transform);
					layoutElement.transform.SetAsLastSibling();
					this.createdPhases.Add(layoutElement);
				}
			}
		}
		this.<Draw>g__FlushMergedSpawnSegment|22_0(ref CS$<>8__locals1);
		float totalTime2 = data.TotalTime;
		if ((totalTime - totalTime2).EqualsOrMore(0.001f, 1E-05f))
		{
			LayoutElement layoutElement2 = this.pausePhase.Copy(this.pausePhase.transform.parent, true, "Pause Phase");
			layoutElement2.preferredWidth = (totalTime - totalTime2) * UIFightingTimelineRendererWidget.TIME_TO_SIZE;
			layoutElement2.transform.SetParent(base.transform);
			layoutElement2.transform.SetAsLastSibling();
		}
		if (lineIndex == 0)
		{
			this.slider1.gameObject.SetActive(true);
			this.slider2.gameObject.SetActive(false);
		}
		else
		{
			this.slider1.gameObject.SetActive(false);
			this.slider2.gameObject.SetActive(true);
		}
		foreach (FightingSector fightingSector in line.sectors)
		{
			UIFightingCaptureIcon elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIFightingCaptureIcon>(this.capturePointsParent);
			this.captureIcons.Add(elementFromPool);
			elementFromPool.Draw(fightingSector.point);
		}
		this.viewportRect.SetAsLastSibling();
	}

	// Token: 0x06004297 RID: 17047 RVA: 0x0013C2EC File Offset: 0x0013A4EC
	public void Clear()
	{
		foreach (LayoutElement layoutElement in this.createdPhases)
		{
			global::UnityEngine.Object.Destroy(layoutElement.gameObject);
		}
		foreach (UIFightingCaptureIcon uifightingCaptureIcon in this.captureIcons)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIFightingCaptureIcon>(uifightingCaptureIcon);
		}
		this.captureIcons.Clear();
		this.createdPhases.Clear();
	}

	// Token: 0x06004298 RID: 17048 RVA: 0x0013C3A0 File Offset: 0x0013A5A0
	private void Awake()
	{
		this.spawnPhase.gameObject.SetActive(false);
		this.pausePhase.gameObject.SetActive(false);
		this.ResetOverlayFullRectStretch();
		if (this.overlayFullObj != null)
		{
			this.overlayFullObj.SetActive(false);
		}
	}

	// Token: 0x06004299 RID: 17049 RVA: 0x0013C3EF File Offset: 0x0013A5EF
	private void OnTransformParentChanged()
	{
		this.ResetOverlayFullRectStretch();
	}

	// Token: 0x0600429A RID: 17050 RVA: 0x0013C3F8 File Offset: 0x0013A5F8
	private void ResetOverlayFullRectStretch()
	{
		if (this.overlayFullObj == null)
		{
			return;
		}
		RectTransform rectTransform = this.overlayFullObj.transform as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
	}

	// Token: 0x0600429C RID: 17052 RVA: 0x0013C484 File Offset: 0x0013A684
	[CompilerGenerated]
	private void <Draw>g__FlushMergedSpawnSegment|22_0(ref UIFightingLine.<>c__DisplayClass22_0 A_1)
	{
		if (A_1.mergedSpawnDuration <= 0)
		{
			return;
		}
		LayoutElement layoutElement = this.spawnPhase.Copy(this.spawnPhase.transform.parent, true, "Spawn Phase");
		layoutElement.preferredWidth = (float)A_1.mergedSpawnDuration * UIFightingTimelineRendererWidget.TIME_TO_SIZE;
		layoutElement.transform.SetParent(base.transform);
		layoutElement.transform.SetAsLastSibling();
		this.createdPhases.Add(layoutElement);
		A_1.mergedSpawnDuration = 0;
	}

	// Token: 0x040033F0 RID: 13296
	public LayoutElement spawnPhase;

	// Token: 0x040033F1 RID: 13297
	public LayoutElement pausePhase;

	// Token: 0x040033F2 RID: 13298
	public RectTransform viewportRect;

	// Token: 0x040033F3 RID: 13299
	public RectTransform capturePointsParent;

	// Token: 0x040033F4 RID: 13300
	public GameObject overlayProgressObj;

	// Token: 0x040033F5 RID: 13301
	public GameObject overlayFullObj;

	// Token: 0x040033F6 RID: 13302
	public RectTransform slider;

	// Token: 0x040033F7 RID: 13303
	public GameObject slider1;

	// Token: 0x040033F8 RID: 13304
	public GameObject slider2;

	// Token: 0x040033F9 RID: 13305
	public Image directionIcon;

	// Token: 0x040033FA RID: 13306
	[HideInInspector]
	[NonSerialized]
	public FightingLine fightingLine;

	// Token: 0x040033FB RID: 13307
	[HideInInspector]
	[NonSerialized]
	public int lineIndex;

	// Token: 0x040033FC RID: 13308
	[HideInInspector]
	[NonSerialized]
	public float secondsAtEndOfLastSpawnPhase = -1f;

	// Token: 0x040033FD RID: 13309
	private List<LayoutElement> createdPhases = new List<LayoutElement>();

	// Token: 0x040033FE RID: 13310
	private List<UIFightingCaptureIcon> captureIcons = new List<UIFightingCaptureIcon>();
}
