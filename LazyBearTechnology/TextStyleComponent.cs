using System;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200015C RID: 348
	[ExecuteInEditMode]
	[RequireComponent(typeof(TMP_Text))]
	public class TextStyleComponent : MonoBehaviour
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00026827 File Offset: 0x00024A27
		public TextStyle CurrentTextStyle
		{
			get
			{
				return this.textStyle;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002682F File Offset: 0x00024A2F
		private void Awake()
		{
			if (this.label == null)
			{
				this.label = base.GetComponent<TMP_Text>();
			}
			this.TryCaptureOriginalLineSpacing();
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00026854 File Offset: 0x00024A54
		public static void RefreshAll()
		{
			TextStyleComponent[] array = global::UnityEngine.Object.FindObjectsByType<TextStyleComponent>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ApplyStyle();
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00026880 File Offset: 0x00024A80
		public void ApplyStyle()
		{
			if (this.textStyle == null)
			{
				if (Application.isPlaying)
				{
					Debug.LogError("Null style on go:[" + base.gameObject.name + "]", this);
				}
				return;
			}
			if (this.label == null)
			{
				this.label = base.GetComponent<TMP_Text>();
			}
			this.textStyle.ApplyStyle(this.label, this.staticFont, this.hasCustomColor ? new Color?(this.customColor) : null, this.hasCustomOutlineColor ? new Color?(this.customOutlineColor) : null, null);
			this.TryCaptureOriginalLineSpacing();
			if (this.ShouldIncreaseLineSpacingForCurrentLanguage())
			{
				this.label.lineSpacing = this.originalLineSpacing.Value + (float)this.asianFontsLineSpacingIncrease;
				return;
			}
			this.label.lineSpacing = this.originalLineSpacing.Value;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002697C File Offset: 0x00024B7C
		public void SetTextStyle(TextStyle newStyle)
		{
			if (newStyle == null)
			{
				Debug.LogWarning("Can't find TextStyle!");
				return;
			}
			if (newStyle != this.textStyle)
			{
				this.textStyle = newStyle;
				this.ApplyStyle();
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000269AD File Offset: 0x00024BAD
		private void OnEnable()
		{
			this.TryCaptureOriginalLineSpacing();
			if (this.textStyle != null)
			{
				this.ApplyStyle();
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000269CC File Offset: 0x00024BCC
		private void TryCaptureOriginalLineSpacing()
		{
			if (this.originalLineSpacing != null)
			{
				return;
			}
			if (this.label == null)
			{
				this.label = base.GetComponent<TMP_Text>();
			}
			if (this.label != null)
			{
				this.originalLineSpacing = new float?(this.label.lineSpacing);
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00026A28 File Offset: 0x00024C28
		private bool ShouldIncreaseLineSpacingForCurrentLanguage()
		{
			if (!this.increaseLineSpacingForAsianFonts)
			{
				return false;
			}
			string currentLang = LLBase.CurrentLang;
			bool flag;
			if (!(currentLang == "zh_cn") && !(currentLang == "zh_cht"))
			{
				if (!(currentLang == "ja"))
				{
					flag = currentLang == "ko" && this.icnIfKorean;
				}
				else
				{
					flag = this.icnIfJapanese;
				}
			}
			else
			{
				flag = this.icnIfChinese;
			}
			return flag;
		}

		// Token: 0x04000496 RID: 1174
		[SerializeField]
		[HideInInspector]
		private TextStyle textStyle;

		// Token: 0x04000497 RID: 1175
		[SerializeField]
		private bool staticFont;

		// Token: 0x04000498 RID: 1176
		[SerializeField]
		private bool hasCustomColor;

		// Token: 0x04000499 RID: 1177
		[SerializeField]
		private Color customColor = Color.white;

		// Token: 0x0400049A RID: 1178
		[SerializeField]
		private bool hasCustomOutlineColor;

		// Token: 0x0400049B RID: 1179
		[SerializeField]
		private Color customOutlineColor = Color.white;

		// Token: 0x0400049C RID: 1180
		[SerializeField]
		private bool increaseLineSpacingForAsianFonts;

		// Token: 0x0400049D RID: 1181
		[SerializeField]
		private int asianFontsLineSpacingIncrease = 12;

		// Token: 0x0400049E RID: 1182
		[SerializeField]
		private bool icnIfChinese = true;

		// Token: 0x0400049F RID: 1183
		[SerializeField]
		private bool icnIfJapanese;

		// Token: 0x040004A0 RID: 1184
		[SerializeField]
		private bool icnIfKorean;

		// Token: 0x040004A1 RID: 1185
		private float? originalLineSpacing;

		// Token: 0x040004A2 RID: 1186
		private TMP_Text label;
	}
}
