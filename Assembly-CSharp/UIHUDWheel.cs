using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000898 RID: 2200
public class UIHUDWheel : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x06003891 RID: 14481 RVA: 0x0010FA78 File Offset: 0x0010DC78
	public void Init()
	{
		EnvironmentEngine.OnNewDayStarted += this.OnNewDayStartedAnimated;
		EnvironmentEngine.OnTimeOfDayChangedEvent += this.OnTimeOfDayChanged;
		this.iconsByDayNumber = new Dictionary<int, Sprite>();
		this.dayNamesByDayNumber = new Dictionary<int, string>();
		this.customCenterIconDayByDayNumber = new Dictionary<int, Sprite>();
		this.customCenterIconNightByDayNumber = new Dictionary<int, Sprite>();
		this.newDaySoundsByDayNumber.Clear();
		for (int i = 0; i < LazyConsts.ConstDefs.AllDays.Length; i++)
		{
			this.<Init>g__AddDayVisuals|56_0(LazyConsts.ConstDefs.AllDays[i]);
			this.RegisterNewDaySound(LazyConsts.ConstDefs.AllDays[i], (LazyConsts.ConstDefs.AllDays[i] == "day_sloth") ? "zombie_bell" : "new_day_bell");
		}
		Vector2 anchoredPosition = this.center.anchoredPosition;
		float num = 4.712389f + 3.1415927f;
		this.starsNightRadiuses = new float[this.starsNight.Length];
		this.angleOffset = new float[this.starsNight.Length];
		for (int j = 0; j < this.starsNight.Length; j++)
		{
			Vector2 vector = this.starsNight[j].rectTransform.anchoredPosition - anchoredPosition;
			this.starsNightRadiuses[j] = vector.magnitude;
			float num2 = Mathf.Atan2(vector.y, vector.x);
			this.angleOffset[j] = num2 - num;
		}
		this.InitWheelLayout();
	}

	// Token: 0x06003892 RID: 14482 RVA: 0x0010FBCC File Offset: 0x0010DDCC
	public void OnTimeOfDayChanged(float time, bool isFake = false)
	{
		float num = (time * 360f - 90f) * 0.017453292f;
		float num2 = num + 3.1415927f;
		Vector2 anchoredPosition = this.center.anchoredPosition;
		this.sun.anchoredPosition = anchoredPosition + new Vector2(Mathf.Cos(num) * this.radius, Mathf.Sin(num) * this.radius);
		this.moon.anchoredPosition = anchoredPosition + new Vector2(Mathf.Cos(num2) * this.radius, Mathf.Sin(num2) * this.radius);
		for (int i = 0; i < this.starsNight.Length; i++)
		{
			float num3 = num2 + this.angleOffset[i];
			float num4 = this.starsNightRadiuses[i];
			this.starsNight[i].rectTransform.anchoredPosition = anchoredPosition + new Vector2(Mathf.Cos(num3) * num4, Mathf.Sin(num3) * num4);
		}
		this.sky.color = this.gradientSky.Evaluate(time);
		this.horizon.color = this.gradientHorizon.Evaluate(time);
		if (time >= 0.25f && time < 0.8f)
		{
			if (!this.isDayTime || this.forceTimeDependentChange)
			{
				this.isDayTime = true;
				this.housesNight.DOFade(0f, this.nightObjectsTweenFade);
				Image[] array = this.starsNight;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].DOFade(0f, this.nightObjectsTweenFade);
				}
				this.forceTimeDependentChange = false;
				return;
			}
		}
		else if (this.isDayTime || this.forceTimeDependentChange)
		{
			this.isDayTime = false;
			this.housesNight.DOFade(1f, this.nightObjectsTweenFade);
			Image[] array = this.starsNight;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].DOFade(1f, this.nightObjectsTweenFade);
			}
			this.forceTimeDependentChange = false;
		}
	}

	// Token: 0x06003893 RID: 14483 RVA: 0x0010FDC9 File Offset: 0x0010DFC9
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(this.center.position, this.radius);
	}

	// Token: 0x06003894 RID: 14484 RVA: 0x0010FDE1 File Offset: 0x0010DFE1
	private void Update()
	{
		this.UpdateDayWheelRotation();
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x0010FDE9 File Offset: 0x0010DFE9
	private void OnDestroy()
	{
		EnvironmentEngine.OnNewDayStarted -= this.OnNewDayStartedAnimated;
		EnvironmentEngine.OnTimeOfDayChangedEvent -= this.OnTimeOfDayChanged;
		this.StopDayWheelAnimation();
	}

	// Token: 0x06003896 RID: 14486 RVA: 0x0010FE13 File Offset: 0x0010E013
	private void OnNewDayStartedAnimated(int day)
	{
		this.OnNewDayStarted(day, true);
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x0010FE20 File Offset: 0x0010E020
	public void OnNewDayStarted(int day, bool animate = true)
	{
		if (!this.wheelLayoutInitialized)
		{
			this.InitWheelLayout();
		}
		if (!animate)
		{
			this.ApplyCenterWheelIcons(day);
			this.StopDayWheelAnimation();
			this.ResetWheelSlotsToBaseline();
			this.ApplyWheelVisualState(day);
			this.lastAppliedWheelDay = day;
			return;
		}
		int num = ((this.lastAppliedWheelDay < 0) ? 1 : (day - this.lastAppliedWheelDay));
		if (num <= 0)
		{
			this.ApplyCenterWheelIcons(day);
			this.lastAppliedWheelDay = day;
			return;
		}
		this.ApplyCenterWheelIcons(day);
		this.PlayNewDaySound(day);
		int num2 = UIHUDWheel.Mod(num, 6);
		if (!base.gameObject.activeInHierarchy || num2 == 0)
		{
			this.SnapWheelToDay(day, num2);
			return;
		}
		if (this.isDayWheelAnimating)
		{
			this.StopDayWheelAnimation();
		}
		this.StartDayWheelAnimation(day, num2);
	}

	// Token: 0x06003898 RID: 14488 RVA: 0x0010FED0 File Offset: 0x0010E0D0
	private void ApplyCenterWheelIcons(int day)
	{
		int dayNumberFromDay = EnvironmentEngine.Instance.Data.GetDayNumberFromDay(day);
		Sprite sprite = this.centerWheelIconDaySpriteDefault;
		Sprite sprite2 = this.centerWheelIconNightSpriteDefault;
		if (this.IsCustomCenterIconUnlocked(dayNumberFromDay))
		{
			Sprite sprite3;
			if (this.customCenterIconDayByDayNumber.TryGetValue(dayNumberFromDay, out sprite3) && sprite3)
			{
				sprite = sprite3;
			}
			Sprite sprite4;
			if (this.customCenterIconNightByDayNumber.TryGetValue(dayNumberFromDay, out sprite4) && sprite4)
			{
				sprite2 = sprite4;
			}
		}
		this.centerWheelIconDay.sprite = sprite;
		this.centerWheelIconNight.sprite = sprite2;
	}

	// Token: 0x06003899 RID: 14489 RVA: 0x0010FF54 File Offset: 0x0010E154
	private bool IsCustomCenterIconUnlocked(int dayNumber)
	{
		string text;
		return this.dayNamesByDayNumber.TryGetValue(dayNumber, out text) && MainGame.Instance.GameSave.knowledgeSystem.unlockedCustomHudDaySprites.Contains(text);
	}

	// Token: 0x0600389A RID: 14490 RVA: 0x0010FF8D File Offset: 0x0010E18D
	private void RegisterNewDaySound(string dayName, string soundId)
	{
		this.newDaySoundsByDayNumber[ConstDef.Get(dayName).IntValue] = soundId;
	}

	// Token: 0x0600389B RID: 14491 RVA: 0x0010FFA8 File Offset: 0x0010E1A8
	private void PlayNewDaySound(int day)
	{
		int dayNumberFromDay = EnvironmentEngine.Instance.Data.GetDayNumberFromDay(day);
		string text;
		if (this.newDaySoundsByDayNumber.TryGetValue(dayNumberFromDay, out text) && !string.IsNullOrEmpty(text))
		{
			LazyAudio.Play(text);
		}
	}

	// Token: 0x0600389C RID: 14492 RVA: 0x0010FFE8 File Offset: 0x0010E1E8
	private void SnapWheelToDay(int day, int slotDelta)
	{
		this.StopDayWheelAnimation();
		if (slotDelta > 0)
		{
			for (int i = 0; i < 6; i++)
			{
				this.elementSlotIndices[i] = UIHUDWheel.Mod(this.elementSlotIndices[i] - slotDelta, 6);
				this.SetElementAngle(i, this.slotAngles[this.elementSlotIndices[i]]);
			}
		}
		this.ApplyWheelVisualState(day);
		this.lastAppliedWheelDay = day;
	}

	// Token: 0x0600389D RID: 14493 RVA: 0x00110048 File Offset: 0x0010E248
	private void InitWheelLayout()
	{
		this.wheelCenterPos = Vector2.zero;
		this.slotAngles = new float[6];
		this.slotRadii = new float[6];
		this.elementSlotIndices = new int[6];
		this.elementAnimStartAngles = new float[6];
		this.elementAnimTargetAngles = new float[6];
		for (int i = 0; i < 6; i++)
		{
			Vector2 vector = this.elements[i].rectTransform.anchoredPosition - this.wheelCenterPos;
			this.slotAngles[i] = Mathf.Atan2(vector.y, vector.x);
			this.slotRadii[i] = vector.magnitude;
			this.elementSlotIndices[i] = i;
			this.SetElementAngle(i, this.slotAngles[i]);
		}
		this.InitArrows();
		this.dayWheelOneStepRad = UIHUDWheel.GetSignedAngleDelta(this.slotAngles[1], this.slotAngles[0]);
		this.wheelLayoutInitialized = true;
	}

	// Token: 0x0600389E RID: 14494 RVA: 0x00110130 File Offset: 0x0010E330
	private void InitArrows()
	{
		if (this.arrows == null)
		{
			return;
		}
		for (int i = 0; i < this.arrows.Length; i++)
		{
			if (this.arrows[i])
			{
				this.arrows[i].gameObject.SetActive(true);
				UIHUDWheel.SetGraphicAlpha(this.arrows[i], 0f);
			}
		}
	}

	// Token: 0x0600389F RID: 14495 RVA: 0x00110190 File Offset: 0x0010E390
	private void ResetWheelSlotsToBaseline()
	{
		for (int i = 0; i < 6; i++)
		{
			this.elementSlotIndices[i] = i;
			this.SetElementAngle(i, this.slotAngles[i]);
		}
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x001101C4 File Offset: 0x0010E3C4
	private void StartDayWheelAnimation(int day, int dayDelta)
	{
		this.pendingDay = day;
		this.pendingDayWheelSlotDelta = dayDelta;
		this.oldCurrentElementIndex = this.FindElementIndexWithSlot(0);
		this.newCurrentElementIndex = this.FindElementIndexWithSlot(dayDelta);
		int num = this.FindElementIndexWithTargetSlot(this.GetNextDayArrowIndex(), dayDelta);
		float num2 = Mathf.Abs(this.dayWheelOneStepRad * (float)dayDelta);
		this.dayWheelRotationDuration = num2 / (this.dayWheelAngularSpeedDeg * 0.017453292f);
		this.dayWheelRotationProgress = 0f;
		this.isDayWheelAnimating = true;
		for (int i = 0; i < 6; i++)
		{
			int num3 = UIHUDWheel.Mod(this.elementSlotIndices[i] - dayDelta, 6);
			this.elementAnimStartAngles[i] = this.slotAngles[this.elementSlotIndices[i]];
			this.elementAnimTargetAngles[i] = this.slotAngles[num3];
			int dayNumberFromDay = EnvironmentEngine.Instance.Data.GetDayNumberFromDay(day + num3);
			this.elements[i].icon.sprite = this.iconsByDayNumber[dayNumberFromDay];
		}
		this.KillDayWheelTweens();
		this.SetAllElementsUnderParentWithInactiveIcons();
		this.SetAllArrowsAlpha(0f);
		Image arrow = this.GetArrow(this.GetNextDayArrowIndex());
		if (arrow)
		{
			UIHUDWheel.PrepareElementForFade(arrow, 1f);
			this.dayWheelTweens.Add(arrow.DOFade(0f, this.fadeOutCurrentArrowDuration));
		}
		if (this.oldCurrentElementIndex != this.newCurrentElementIndex && this.oldCurrentElementIndex != num)
		{
			this.SetElementOverParent(this.elements[this.oldCurrentElementIndex]);
			UIHUDWheel.SetIconVisual(this.elements[this.oldCurrentElementIndex], true);
			this.TweenIconToActive(this.elements[this.oldCurrentElementIndex], false, this.fadeOutCurrentIconDuration);
		}
		this.SetElementOverParent(this.elements[this.newCurrentElementIndex]);
		this.TweenIconToActive(this.elements[this.newCurrentElementIndex], true, this.fadeInNewCurrentIconDuration);
		if (num >= 0 && num != this.newCurrentElementIndex)
		{
			this.SetElementOverParent(this.elements[num]);
			this.TweenIconToActive(this.elements[num], true, this.fadeInNewCurrentIconDuration);
		}
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x001103BD File Offset: 0x0010E5BD
	private int GetNextDayArrowIndex()
	{
		return EnvironmentEngine.Instance.Data.GetDiffInDaysBetweenCurrentAndNext();
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x001103D0 File Offset: 0x0010E5D0
	private void UpdateDayWheelRotation()
	{
		if (!this.isDayWheelAnimating)
		{
			return;
		}
		this.dayWheelRotationProgress += Time.deltaTime / this.dayWheelRotationDuration;
		float num = Mathf.Clamp01(this.dayWheelRotationProgress);
		for (int i = 0; i < 6; i++)
		{
			float num2 = this.elementAnimStartAngles[i] + this.dayWheelOneStepRad * (float)this.pendingDayWheelSlotDelta * num;
			this.SetElementAngleAnimated(i, num2, num);
		}
		if (num < 1f)
		{
			return;
		}
		this.CompleteDayWheelAnimation();
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x0011044C File Offset: 0x0010E64C
	private void CompleteDayWheelAnimation()
	{
		this.isDayWheelAnimating = false;
		for (int i = 0; i < 6; i++)
		{
			this.elementSlotIndices[i] = UIHUDWheel.Mod(this.elementSlotIndices[i] - this.pendingDayWheelSlotDelta, 6);
			this.SetElementAngle(i, this.slotAngles[this.elementSlotIndices[i]]);
		}
		this.lastAppliedWheelDay = this.pendingDay;
		this.ApplyWheelVisualState(this.pendingDay);
		Image arrow = this.GetArrow(this.GetNextDayArrowIndex());
		if (arrow)
		{
			UIHUDWheel.PrepareElementForFade(arrow, 0f);
			this.dayWheelTweens.Add(arrow.DOFade(1f, this.fadeInNewCurrentArrowDuration));
		}
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x001104F4 File Offset: 0x0010E6F4
	private void ApplyWheelVisualState(int day)
	{
		this.KillDayWheelTweens();
		for (int i = 0; i < 6; i++)
		{
			this.ApplyElementVisualState(i, day);
		}
		this.ApplyArrowVisualState();
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x00110524 File Offset: 0x0010E724
	private void SetAllElementsUnderParentWithInactiveIcons()
	{
		for (int i = 0; i < 6; i++)
		{
			UIHUDWheelElement uihudwheelElement = this.elements[i];
			uihudwheelElement.rectTransform.SetParent(this.underParent);
			UIHUDWheel.SetIconVisual(uihudwheelElement, false);
		}
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x0011055C File Offset: 0x0010E75C
	private void ApplyElementVisualState(int elementIndex, int day)
	{
		UIHUDWheelElement uihudwheelElement = this.elements[elementIndex];
		int num = this.elementSlotIndices[elementIndex];
		int dayNumberFromDay = EnvironmentEngine.Instance.Data.GetDayNumberFromDay(day + num);
		bool flag = dayNumberFromDay == EnvironmentEngine.Instance.Data.CurrentDayNumber;
		bool flag2 = dayNumberFromDay == EnvironmentEngine.Instance.Data.NextDayNumber;
		bool flag3 = flag || flag2;
		uihudwheelElement.icon.sprite = this.iconsByDayNumber[dayNumberFromDay];
		if (flag3)
		{
			this.SetElementOverParent(uihudwheelElement);
			UIHUDWheel.SetIconVisual(uihudwheelElement, true);
			return;
		}
		uihudwheelElement.rectTransform.SetParent(this.underParent);
		UIHUDWheel.SetIconVisual(uihudwheelElement, false);
	}

	// Token: 0x060038A7 RID: 14503 RVA: 0x001105F8 File Offset: 0x0010E7F8
	private void ApplyArrowVisualState()
	{
		this.SetAllArrowsAlpha(0f);
		Image arrow = this.GetArrow(this.GetNextDayArrowIndex());
		if (arrow)
		{
			UIHUDWheel.SetGraphicAlpha(arrow, 1f);
		}
	}

	// Token: 0x060038A8 RID: 14504 RVA: 0x00110630 File Offset: 0x0010E830
	private int FindElementIndexWithSlot(int slot)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.elementSlotIndices[i] == slot)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x060038A9 RID: 14505 RVA: 0x00110658 File Offset: 0x0010E858
	private int FindElementIndexWithTargetSlot(int targetSlot, int dayDelta)
	{
		if (targetSlot < 0 || targetSlot >= 6)
		{
			return -1;
		}
		int num = UIHUDWheel.Mod(targetSlot + dayDelta, 6);
		return this.FindElementIndexWithSlot(num);
	}

	// Token: 0x060038AA RID: 14506 RVA: 0x00110680 File Offset: 0x0010E880
	private Image GetArrow(int slotIndex)
	{
		if (this.arrows == null || slotIndex < 0 || slotIndex >= this.arrows.Length)
		{
			return null;
		}
		return this.arrows[slotIndex];
	}

	// Token: 0x060038AB RID: 14507 RVA: 0x001106A4 File Offset: 0x0010E8A4
	private void SetAllArrowsAlpha(float alpha)
	{
		if (this.arrows == null)
		{
			return;
		}
		for (int i = 0; i < this.arrows.Length; i++)
		{
			if (this.arrows[i])
			{
				UIHUDWheel.SetGraphicAlpha(this.arrows[i], alpha);
			}
		}
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x001106EA File Offset: 0x0010E8EA
	private static void SetIconVisual(UIHUDWheelElement element, bool active)
	{
		element.icon.color = (active ? element.iconColorDefault : element.iconColorInactive);
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x00110708 File Offset: 0x0010E908
	private void SetElementOverParent(UIHUDWheelElement element)
	{
		element.rectTransform.SetParent(this.overParent);
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x0011071C File Offset: 0x0010E91C
	private void TweenIconToActive(UIHUDWheelElement element, bool active, float duration)
	{
		float num = (active ? element.iconColorDefault.a : element.iconColorInactive.a);
		Color color = (active ? element.iconColorDefault : element.iconColorInactive);
		Color color2 = element.icon.color;
		element.icon.color = new Color(color.r, color.g, color.b, color2.a);
		this.dayWheelTweens.Add(element.icon.DOFade(num, duration));
	}

	// Token: 0x060038AF RID: 14511 RVA: 0x001107A4 File Offset: 0x0010E9A4
	private void SetElementAngle(int elementIndex, float angle)
	{
		int num = this.elementSlotIndices[elementIndex];
		this.SetElementPosition(elementIndex, angle, this.slotRadii[num]);
	}

	// Token: 0x060038B0 RID: 14512 RVA: 0x001107CC File Offset: 0x0010E9CC
	private void SetElementAngleAnimated(int elementIndex, float angle, float t)
	{
		int num = this.elementSlotIndices[elementIndex];
		float num2 = (float)this.pendingDayWheelSlotDelta * t;
		int num3 = Mathf.FloorToInt(num2);
		float num4 = num2 - (float)num3;
		int num5 = UIHUDWheel.Mod(num - num3, 6);
		int num6 = UIHUDWheel.Mod(num - num3 - 1, 6);
		float num7 = Mathf.Lerp(this.slotRadii[num5], this.slotRadii[num6], num4);
		this.SetElementPosition(elementIndex, angle, num7);
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x0011082C File Offset: 0x0010EA2C
	private void SetElementPosition(int elementIndex, float angle, float r)
	{
		this.elements[elementIndex].rectTransform.anchoredPosition = this.wheelCenterPos + new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x00110860 File Offset: 0x0010EA60
	private static int Mod(int value, int modulus)
	{
		return (value % modulus + modulus) % modulus;
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x00110869 File Offset: 0x0010EA69
	private static float GetSignedAngleDelta(float fromRad, float toRad)
	{
		return Mathf.DeltaAngle(fromRad * 57.29578f, toRad * 57.29578f) * 0.017453292f;
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x00110884 File Offset: 0x0010EA84
	private static void SetGraphicAlpha(Graphic graphic, float alpha)
	{
		Color color = graphic.color;
		color.a = alpha;
		graphic.color = color;
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x001108A7 File Offset: 0x0010EAA7
	private static void PrepareElementForFade(Graphic graphic, float alpha)
	{
		graphic.DOKill(false);
		UIHUDWheel.SetGraphicAlpha(graphic, alpha);
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x001108B8 File Offset: 0x0010EAB8
	private void StopDayWheelAnimation()
	{
		this.isDayWheelAnimating = false;
		this.KillDayWheelTweens();
	}

	// Token: 0x060038B7 RID: 14519 RVA: 0x001108C8 File Offset: 0x0010EAC8
	private void KillDayWheelTweens()
	{
		for (int i = 0; i < this.dayWheelTweens.Count; i++)
		{
			Tween tween = this.dayWheelTweens[i];
			if (tween != null)
			{
				tween.Kill(false);
			}
		}
		this.dayWheelTweens.Clear();
		for (int j = 0; j < 6; j++)
		{
			this.elements[j].icon.DOKill(false);
		}
		if (this.arrows == null)
		{
			return;
		}
		for (int k = 0; k < this.arrows.Length; k++)
		{
			Image image = this.arrows[k];
			if (image != null)
			{
				image.DOKill(false);
			}
		}
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x001109E8 File Offset: 0x0010EBE8
	[CompilerGenerated]
	private void <Init>g__AddDayVisuals|56_0(string dayName)
	{
		int intValue = ConstDef.Get(dayName).IntValue;
		this.dayNamesByDayNumber.Add(intValue, dayName);
		this.iconsByDayNumber.Add(intValue, this.dayIcons.Find((Sprite s) => s && s.name == dayName));
		this.customCenterIconDayByDayNumber.Add(intValue, this.dayCustomWheelCenterIconDay.Find((Sprite s) => s && s.name == dayName));
		this.customCenterIconNightByDayNumber.Add(intValue, this.dayCustomWheelCenterIconNight.Find((Sprite s) => s && s.name == dayName));
	}

	// Token: 0x04002CEC RID: 11500
	[SerializeField]
	private RectTransform underParent;

	// Token: 0x04002CED RID: 11501
	[SerializeField]
	private RectTransform overParent;

	// Token: 0x04002CEE RID: 11502
	[SerializeField]
	private UIHUDWheelElement[] elements;

	// Token: 0x04002CEF RID: 11503
	[SerializeField]
	private List<Sprite> dayIcons = new List<Sprite>();

	// Token: 0x04002CF0 RID: 11504
	[SerializeField]
	private List<Sprite> dayCustomWheelCenterIconDay = new List<Sprite>();

	// Token: 0x04002CF1 RID: 11505
	[SerializeField]
	private List<Sprite> dayCustomWheelCenterIconNight = new List<Sprite>();

	// Token: 0x04002CF2 RID: 11506
	[SerializeField]
	private Image centerWheelIconDay;

	// Token: 0x04002CF3 RID: 11507
	[SerializeField]
	private Image centerWheelIconNight;

	// Token: 0x04002CF4 RID: 11508
	[SerializeField]
	private Sprite centerWheelIconDaySpriteDefault;

	// Token: 0x04002CF5 RID: 11509
	[SerializeField]
	private Sprite centerWheelIconNightSpriteDefault;

	// Token: 0x04002CF6 RID: 11510
	[SerializeField]
	private RectTransform sun;

	// Token: 0x04002CF7 RID: 11511
	[SerializeField]
	private RectTransform moon;

	// Token: 0x04002CF8 RID: 11512
	[SerializeField]
	private float radius;

	// Token: 0x04002CF9 RID: 11513
	[SerializeField]
	private RectTransform center;

	// Token: 0x04002CFA RID: 11514
	[SerializeField]
	private Gradient gradientHorizon;

	// Token: 0x04002CFB RID: 11515
	[SerializeField]
	private Gradient gradientSky;

	// Token: 0x04002CFC RID: 11516
	[SerializeField]
	private Image sky;

	// Token: 0x04002CFD RID: 11517
	[SerializeField]
	private Image horizon;

	// Token: 0x04002CFE RID: 11518
	[SerializeField]
	private Image housesNight;

	// Token: 0x04002CFF RID: 11519
	[SerializeField]
	private Image[] starsNight;

	// Token: 0x04002D00 RID: 11520
	[SerializeField]
	private float nightObjectsTweenFade;

	// Token: 0x04002D01 RID: 11521
	[SerializeField]
	[Header("Day wheel rotation")]
	private float dayWheelAngularSpeedDeg = 120f;

	// Token: 0x04002D02 RID: 11522
	[SerializeField]
	private float fadeOutCurrentArrowDuration = 0.5f;

	// Token: 0x04002D03 RID: 11523
	[SerializeField]
	private float fadeOutCurrentIconDuration = 0.25f;

	// Token: 0x04002D04 RID: 11524
	[SerializeField]
	private float fadeInNewCurrentIconDuration = 0.25f;

	// Token: 0x04002D05 RID: 11525
	[SerializeField]
	private float fadeInNewCurrentArrowDuration = 0.5f;

	// Token: 0x04002D06 RID: 11526
	[SerializeField]
	private Image[] arrows;

	// Token: 0x04002D07 RID: 11527
	private Dictionary<int, Sprite> iconsByDayNumber;

	// Token: 0x04002D08 RID: 11528
	private Dictionary<int, string> dayNamesByDayNumber;

	// Token: 0x04002D09 RID: 11529
	private Dictionary<int, Sprite> customCenterIconDayByDayNumber;

	// Token: 0x04002D0A RID: 11530
	private Dictionary<int, Sprite> customCenterIconNightByDayNumber;

	// Token: 0x04002D0B RID: 11531
	private readonly Dictionary<int, string> newDaySoundsByDayNumber = new Dictionary<int, string>();

	// Token: 0x04002D0C RID: 11532
	private bool isDayTime;

	// Token: 0x04002D0D RID: 11533
	public bool forceTimeDependentChange;

	// Token: 0x04002D0E RID: 11534
	private float[] starsNightRadiuses;

	// Token: 0x04002D0F RID: 11535
	private float[] angleOffset;

	// Token: 0x04002D10 RID: 11536
	private Vector2 wheelCenterPos;

	// Token: 0x04002D11 RID: 11537
	private float[] slotAngles;

	// Token: 0x04002D12 RID: 11538
	private float[] slotRadii;

	// Token: 0x04002D13 RID: 11539
	private int[] elementSlotIndices;

	// Token: 0x04002D14 RID: 11540
	private float[] elementAnimStartAngles;

	// Token: 0x04002D15 RID: 11541
	private float[] elementAnimTargetAngles;

	// Token: 0x04002D16 RID: 11542
	private bool isDayWheelAnimating;

	// Token: 0x04002D17 RID: 11543
	private float dayWheelRotationProgress;

	// Token: 0x04002D18 RID: 11544
	private float dayWheelRotationDuration;

	// Token: 0x04002D19 RID: 11545
	private int pendingDay;

	// Token: 0x04002D1A RID: 11546
	private int pendingDayWheelSlotDelta;

	// Token: 0x04002D1B RID: 11547
	private int oldCurrentElementIndex;

	// Token: 0x04002D1C RID: 11548
	private int newCurrentElementIndex;

	// Token: 0x04002D1D RID: 11549
	private int lastAppliedWheelDay = -1;

	// Token: 0x04002D1E RID: 11550
	private float dayWheelOneStepRad;

	// Token: 0x04002D1F RID: 11551
	private bool wheelLayoutInitialized;

	// Token: 0x04002D20 RID: 11552
	private readonly List<Tween> dayWheelTweens = new List<Tween>();

	// Token: 0x04002D21 RID: 11553
	private const float STARS_LAYOUT_TIME = 1f;

	// Token: 0x04002D22 RID: 11554
	private const string ZOMBIE_BELL_SOUND_ID = "zombie_bell";

	// Token: 0x04002D23 RID: 11555
	private const string NEW_DAY_BELL_SOUND_ID = "new_day_bell";
}
