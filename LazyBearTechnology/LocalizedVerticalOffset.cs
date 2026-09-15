using System;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000198 RID: 408
	[RequireComponent(typeof(RectTransform))]
	public class LocalizedVerticalOffset : MonoBehaviour
	{
		// Token: 0x06000931 RID: 2353 RVA: 0x0002C689 File Offset: 0x0002A889
		private void Awake()
		{
			this.Apply();
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002C691 File Offset: 0x0002A891
		private void OnEnable()
		{
			this.Apply();
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002C69C File Offset: 0x0002A89C
		public void Apply()
		{
			if (!this.TryResolveTarget())
			{
				return;
			}
			if (this.useFontExtraSettings)
			{
				Vector4 vector = this.originalMargin.GetValueOrDefault();
				if (this.originalMargin == null)
				{
					vector = this.tmpText.margin;
					this.originalMargin = new Vector4?(vector);
				}
				Vector4 value = this.originalMargin.Value;
				if (this.ShouldApplyOffset())
				{
					value.y += (float)this.verticalOffset;
				}
				this.tmpText.margin = value;
				return;
			}
			if (!this.originalStored)
			{
				this.originalValue = this.GetCurrentValue();
				this.originalStored = true;
			}
			this.SetCurrentValue(this.ShouldApplyOffset() ? (this.originalValue + (float)this.verticalOffset) : this.originalValue);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002C760 File Offset: 0x0002A960
		private bool ShouldApplyOffset()
		{
			string currentLang = LLBase.CurrentLang;
			bool flag;
			if (!(currentLang == "ja"))
			{
				if (!(currentLang == "ko"))
				{
					flag = currentLang == "zh_cn" && this.chineseSimplified;
				}
				else
				{
					flag = this.korean;
				}
			}
			else
			{
				flag = this.japanese;
			}
			return flag;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002C7BC File Offset: 0x0002A9BC
		private bool TryResolveTarget()
		{
			if (this.useFontExtraSettings)
			{
				if (this.tmpText == null)
				{
					this.tmpText = base.GetComponent<TMP_Text>();
				}
				return this.tmpText != null;
			}
			if (this.rectTransform == null)
			{
				this.rectTransform = (RectTransform)base.transform;
			}
			return true;
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x0002C818 File Offset: 0x0002AA18
		private bool IsVerticallyStretched
		{
			get
			{
				return this.rectTransform.anchorMin.y != this.rectTransform.anchorMax.y;
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002C83F File Offset: 0x0002AA3F
		private float GetCurrentValue()
		{
			if (!this.IsVerticallyStretched)
			{
				return this.rectTransform.anchoredPosition.y;
			}
			return this.rectTransform.offsetMax.y;
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002C86C File Offset: 0x0002AA6C
		private void SetCurrentValue(float value)
		{
			if (this.IsVerticallyStretched)
			{
				Vector2 offsetMax = this.rectTransform.offsetMax;
				offsetMax.y = value;
				this.rectTransform.offsetMax = offsetMax;
				return;
			}
			Vector2 anchoredPosition = this.rectTransform.anchoredPosition;
			anchoredPosition.y = value;
			this.rectTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x04000585 RID: 1413
		[SerializeField]
		private bool japanese = true;

		// Token: 0x04000586 RID: 1414
		[SerializeField]
		private bool korean = true;

		// Token: 0x04000587 RID: 1415
		[SerializeField]
		private bool chineseSimplified = true;

		// Token: 0x04000588 RID: 1416
		[SerializeField]
		private int verticalOffset;

		// Token: 0x04000589 RID: 1417
		[SerializeField]
		[Tooltip("If enabled, applies the offset to the TextMesh Pro Extra Settings top margin instead of moving the RectTransform.")]
		private bool useFontExtraSettings;

		// Token: 0x0400058A RID: 1418
		private RectTransform rectTransform;

		// Token: 0x0400058B RID: 1419
		private TMP_Text tmpText;

		// Token: 0x0400058C RID: 1420
		private float originalValue;

		// Token: 0x0400058D RID: 1421
		private bool originalStored;

		// Token: 0x0400058E RID: 1422
		private Vector4? originalMargin;
	}
}
