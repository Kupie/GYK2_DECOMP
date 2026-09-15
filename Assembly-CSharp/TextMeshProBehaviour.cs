using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007C3 RID: 1987
[Serializable]
public class TextMeshProBehaviour : PlayableBehaviour
{
	// Token: 0x06003310 RID: 13072 RVA: 0x000F6000 File Offset: 0x000F4200
	public float GetFadeMultiplier(Playable playable)
	{
		if (this.fadeDuration <= 0f)
		{
			return 1f;
		}
		double time = playable.GetTime<Playable>();
		double duration = playable.GetDuration<Playable>();
		if (time < (double)this.fadeDuration)
		{
			return this.fadeInEase.Evaluate(Mathf.Clamp01((float)(time / (double)this.fadeDuration)));
		}
		if (duration > (double)this.fadeDuration && time > duration - (double)this.fadeDuration)
		{
			return this.fadeOutEase.Evaluate(Mathf.Clamp01((float)((time - duration + (double)this.fadeDuration) / (double)this.fadeDuration)));
		}
		return 1f;
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x000F6094 File Offset: 0x000F4294
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (info.effectiveWeight <= 0f)
		{
			return;
		}
		this.textMeshPro = playerData as TextMeshPro;
		if (!this.textMeshPro)
		{
			return;
		}
		Color color = this.color;
		color.a = this.color.a * this.GetFadeMultiplier(playable) * info.effectiveWeight;
		if (!Application.isPlaying)
		{
			this.ProcessFrameWorldPreview(color);
			return;
		}
		CinematicsCommonObject componentInParent = this.textMeshPro.GetComponentInParent<CinematicsCommonObject>(true);
		if (componentInParent != null && !componentInParent.IsTargetLabel(this.textMeshPro))
		{
			CinematicsCommonObject.HideWorldLabel(this.textMeshPro);
			return;
		}
		this.ProcessFrameUi(color);
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x000F6139 File Offset: 0x000F4339
	public override void OnGraphStop(Playable playable)
	{
		this.isInitialized = false;
		base.OnGraphStop(playable);
		this.CleanupUiWidget();
		this.HideBoundWorldText();
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x000F6155 File Offset: 0x000F4355
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (!this.isInitialized)
		{
			return;
		}
		this.isInitialized = false;
		this.CleanupUiWidget();
		this.HideBoundWorldText();
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x000F6174 File Offset: 0x000F4374
	private void ProcessFrameUi(Color fadedColor)
	{
		if (!this.EnsureUiWidget())
		{
			return;
		}
		this.HideBoundWorldText();
		if (!this.isInitialized)
		{
			string text = LLBase.L(this.localeId);
			Action<string> onTextChanged = this.uiWidgetData.OnTextChanged;
			if (onTextChanged != null)
			{
				onTextChanged(text);
			}
			Action<bool> onVisibleChanged = this.uiWidgetData.OnVisibleChanged;
			if (onVisibleChanged != null)
			{
				onVisibleChanged(true);
			}
			this.isInitialized = true;
		}
		this.SyncWorldLayoutToData();
		this.uiWidgetData.Color = fadedColor;
		Action<Color> onColorChanged = this.uiWidgetData.OnColorChanged;
		if (onColorChanged == null)
		{
			return;
		}
		onColorChanged(fadedColor);
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x000F6204 File Offset: 0x000F4404
	private void ProcessFrameWorldPreview(Color fadedColor)
	{
		if (!this.isInitialized)
		{
			this.textMeshPro.text = LLBase.L(this.localeId);
			this.textMeshPro.gameObject.SetActive(true);
			this.isInitialized = true;
		}
		this.textMeshPro.color = fadedColor;
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x000F6254 File Offset: 0x000F4454
	private bool EnsureUiWidget()
	{
		if (this.uiWidget != null)
		{
			return true;
		}
		if (LazySingleton<LazyWidgetPrefabContainer>.Instance == null || GUIElements.Instance == null)
		{
			return false;
		}
		LazyWidgetBase prefabFromDataType;
		try
		{
			prefabFromDataType = LazyWidgetPrefabContainer.GetPrefabFromDataType<CinematicsTextWidgetData>();
		}
		catch (Exception ex)
		{
			Debug.LogError("[TextMeshProBehaviour] CinematicsTextWidget prefab is missing: " + ex.Message);
			return false;
		}
		RectTransform rectTransform = this.textMeshPro.rectTransform;
		this.uiWidgetData = new CinematicsTextWidgetData(TextMeshProBehaviour.GetWorldPosition(rectTransform), TextMeshProBehaviour.GetWorldSize(rectTransform))
		{
			ShowBackground = TextMeshProBehaviour.IsOverImageLabel(this.textMeshPro),
			Pivot = rectTransform.pivot
		};
		this.uiWidget = prefabFromDataType.Copy(GUIElements.Instance.Root, true, "") as CinematicsTextWidget;
		if (this.uiWidget == null)
		{
			Debug.LogError("[TextMeshProBehaviour] Prefab is not CinematicsTextWidget");
			return false;
		}
		CanvasGroup canvasGroup;
		if (this.uiWidget.TryGetComponent<CanvasGroup>(out canvasGroup))
		{
			canvasGroup.ignoreParentGroups = true;
		}
		this.uiWidget.Init();
		this.uiWidget.Draw(this.uiWidgetData);
		return true;
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x000F6378 File Offset: 0x000F4578
	private void SyncWorldLayoutToData()
	{
		RectTransform rectTransform = this.textMeshPro.rectTransform;
		this.uiWidgetData.Position = TextMeshProBehaviour.GetWorldPosition(rectTransform);
		this.uiWidgetData.Size = TextMeshProBehaviour.GetWorldSize(rectTransform);
		this.uiWidgetData.Pivot = rectTransform.pivot;
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x000F63C4 File Offset: 0x000F45C4
	private static bool IsOverImageLabel(TextMeshPro boundText)
	{
		CinematicsCommonObject componentInParent = boundText.GetComponentInParent<CinematicsCommonObject>(true);
		return componentInParent != null && componentInParent.labelOverImage == boundText;
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x000F63F0 File Offset: 0x000F45F0
	private static Vector2 GetWorldPosition(RectTransform anchor)
	{
		Vector3 position = anchor.position;
		return new Vector2(position.x, position.y);
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x000F6418 File Offset: 0x000F4618
	private static Vector2 GetWorldSize(RectTransform anchor)
	{
		Vector2 size = anchor.rect.size;
		Vector3 lossyScale = anchor.lossyScale;
		return new Vector2(Mathf.Abs(size.x * lossyScale.x), Mathf.Abs(size.y * lossyScale.y));
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x000F6464 File Offset: 0x000F4664
	private void HideBoundWorldText()
	{
		CinematicsCommonObject.HideWorldLabel(this.textMeshPro);
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x000F6474 File Offset: 0x000F4674
	private void CleanupUiWidget()
	{
		if (this.uiWidget == null)
		{
			return;
		}
		CinematicsTextWidgetData cinematicsTextWidgetData = this.uiWidgetData;
		if (cinematicsTextWidgetData != null)
		{
			Action<bool> onVisibleChanged = cinematicsTextWidgetData.OnVisibleChanged;
			if (onVisibleChanged != null)
			{
				onVisibleChanged(false);
			}
		}
		this.uiWidget.Hide();
		if (Application.isPlaying)
		{
			global::UnityEngine.Object.Destroy(this.uiWidget.gameObject);
		}
		else
		{
			global::UnityEngine.Object.DestroyImmediate(this.uiWidget.gameObject);
		}
		this.uiWidget = null;
		this.uiWidgetData = null;
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x000F64EF File Offset: 0x000F46EF
	private void TryPlayVoiceOver()
	{
		if (string.IsNullOrEmpty(this.localeId) || !LazyAudio.IsInitialized)
		{
			return;
		}
		if (!VoiceOverSettings.IsEnabled)
		{
			return;
		}
		LazyAudio.VoiceOverPlayer.Play(this.localeId, null);
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x000F651F File Offset: 0x000F471F
	private void TryStopVoiceOver()
	{
		if (!Application.isPlaying || !LazyAudio.IsInitialized)
		{
			return;
		}
		LazyAudio.VoiceOverPlayer.Stop();
	}

	// Token: 0x040028E1 RID: 10465
	public string localeId;

	// Token: 0x040028E2 RID: 10466
	public Color color = Color.white;

	// Token: 0x040028E3 RID: 10467
	public float fadeDuration = 0.5f;

	// Token: 0x040028E4 RID: 10468
	public AnimationCurve fadeInEase = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x040028E5 RID: 10469
	public AnimationCurve fadeOutEase = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	// Token: 0x040028E6 RID: 10470
	private bool isInitialized;

	// Token: 0x040028E7 RID: 10471
	private TextMeshPro textMeshPro;

	// Token: 0x040028E8 RID: 10472
	private CinematicsTextWidget uiWidget;

	// Token: 0x040028E9 RID: 10473
	private CinematicsTextWidgetData uiWidgetData;
}
