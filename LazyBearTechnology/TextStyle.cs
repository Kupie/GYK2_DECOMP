using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000159 RID: 345
	[CreateAssetMenu(menuName = "LazyFont/Text Style", fileName = "TextStyle")]
	public class TextStyle : ScriptableObject
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00025D2D File Offset: 0x00023F2D
		public LazyFontData Font
		{
			get
			{
				return this.lazyFont;
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00025D38 File Offset: 0x00023F38
		public static void ClearDynamicMaterialCaches()
		{
			TextStyle[] array = Resources.FindObjectsOfTypeAll<TextStyle>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ClearDynamicMaterialCache();
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00025D61 File Offset: 0x00023F61
		private void ClearDynamicMaterialCache()
		{
			TextStyle.DestroyCachedMaterials(this.cachedMaterialsByDynamicLangId);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00025D70 File Offset: 0x00023F70
		private static void DestroyCachedMaterials(Dictionary<string, Material> dictionary)
		{
			foreach (Material material in dictionary.Values)
			{
				if (material != null)
				{
					global::UnityEngine.Object.DestroyImmediate(material, true);
				}
			}
			dictionary.Clear();
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00025DD4 File Offset: 0x00023FD4
		public void ApplyStyle(TMP_Text label, bool staticFont = false, Color? overrideColor = null, Color? overrideOutlineColor = null, Color? overrideSecondOutlineColor = null)
		{
			string currentLang = LLBase.CurrentLang;
			this.ApplyStyleAndLanguage(label, currentLang, staticFont, overrideColor, overrideOutlineColor, overrideSecondOutlineColor);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00025DF8 File Offset: 0x00023FF8
		public void ApplyStyleAndLanguage(TMP_Text label, string lang, bool staticFont = false, Color? overrideColor = null, Color? overrideOutlineColor = null, Color? overrideSecondOutlineColor = null)
		{
			if (label == null)
			{
				Debug.LogError("Error in ApplyStyle(): Label is null");
				return;
			}
			staticFont = staticFont || (this.data != null && this.data.isAlwaysStaticFont) || label.GetComponent<StaticFontLabel>() != null;
			label.font = this.Font.GetFontAssetFor(lang, staticFont, true);
			LanguageModHooks.ApplyDirectionHandler applyDirection = LanguageModHooks.ApplyDirection;
			if (applyDirection != null)
			{
				applyDirection(label, lang, staticFont);
			}
			Material material = ((this.Font.IsFontUsesOwnMaterial(lang) && !staticFont) ? this.GetMaterialFor(label.font.material, lang, overrideOutlineColor, overrideSecondOutlineColor) : this.GetMaterialFor(lang, staticFont, overrideOutlineColor, overrideSecondOutlineColor, true));
			this.SetColor(label, lang, overrideColor);
			if (this.data != null)
			{
				TextStyleData textStyleData;
				if (this.HasTextStyleData(lang, (TextStyleData style) => style.containsLineSpacing, out textStyleData))
				{
					label.lineSpacing = textStyleData.lineSpacing;
				}
			}
			label.fontSharedMaterial = material;
			label.ForceMeshUpdate(false, false);
			LayoutRebuilder.ForceRebuildLayoutImmediate(label.rectTransform);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00025F04 File Offset: 0x00024104
		public string ApplyStyleToString(string str, bool staticFont = false, bool includeFontTag = true)
		{
			string currentLang = LLBase.CurrentLang;
			string text = string.Empty;
			if (staticFont || !this.Font.IsFontUsesOwnMaterial(currentLang))
			{
				int num = 0;
				text = (staticFont ? (currentLang + ":" + base.name + "_static") : (currentLang + ":" + base.name + "_dynamic"));
				for (int i = 0; i < text.Length; i++)
				{
					num = ((num << 5) + num) ^ (int)TMP_TextParsingUtilities.ToUpperASCIIFast(text[i]);
				}
				Material materialFor;
				if (!MaterialReferenceManager.TryGetMaterial(num, out materialFor))
				{
					materialFor = this.GetMaterialFor(currentLang, staticFont, null, null, true);
					MaterialReferenceManager.AddFontMaterial(num, materialFor);
				}
			}
			string text2 = "color=#" + ColorUtility.ToHtmlStringRGB(this.GetColorForLabel(currentLang, null));
			string text3 = string.Empty;
			if (!string.IsNullOrEmpty(text))
			{
				text3 = " material=" + text;
			}
			string name = this.Font.GetFontAssetFor(currentLang, staticFont, true).name;
			string text4 = (includeFontTag ? string.Concat(new string[] { "<font=\"", name, "\"", text3, ">" }) : "");
			text4 = string.Concat(new string[] { text4, "<", text2, ">", str, "</color>" });
			if (includeFontTag)
			{
				text4 += "</font>";
			}
			return text4;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00026093 File Offset: 0x00024293
		public string TranslateAndColorizeTags(string localeKey)
		{
			return this.ColorizeTags(LLBase.L(localeKey));
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000260A4 File Offset: 0x000242A4
		public string ColorizeTags(string str)
		{
			int num = str.IndexOf('[');
			int num2 = str.IndexOf(']');
			while (num != -1 && num2 != -1 && num < num2)
			{
				string text = str.Substring(num + 1, num2 - num - 1);
				text = this.ApplyStyleToString(text, false, true);
				str = str.Replace(str.Substring(num, num2 - num + 1), text);
				num = str.IndexOf('[');
				num2 = str.IndexOf(']');
			}
			return str;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00026114 File Offset: 0x00024314
		private Material GetMaterialFor(string lang, bool staticFont, Color? overrideOutlineColor = null, Color? overrideSecondOutlineColor = null, bool applyLanguagePackFont = true)
		{
			Dictionary<string, Material> dictionary = (staticFont ? this.cachedMaterialsByStaticLangId : this.cachedMaterialsByDynamicLangId);
			string text = lang;
			if (!applyLanguagePackFont)
			{
				text += ":style";
			}
			if (overrideOutlineColor != null)
			{
				text += overrideOutlineColor.Value.ToString();
			}
			if (overrideSecondOutlineColor != null)
			{
				text += overrideSecondOutlineColor.Value.ToString();
			}
			Material material;
			if (!dictionary.TryGetValue(text, out material))
			{
				material = this.CreateMaterial(lang, staticFont, overrideOutlineColor, overrideSecondOutlineColor, applyLanguagePackFont);
				dictionary.Add(text, material);
			}
			return material;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000261B4 File Offset: 0x000243B4
		private Material GetMaterialFor(Material fontMaterial, string lang, Color? overrideOutlineColor = null, Color? overrideSecondOutlineColor = null)
		{
			Dictionary<string, Material> dictionary = this.cachedMaterialsByDynamicLangId;
			string text = lang;
			if (overrideOutlineColor != null)
			{
				text += overrideOutlineColor.Value.ToString();
			}
			if (overrideSecondOutlineColor != null)
			{
				text += overrideSecondOutlineColor.Value.ToString();
			}
			Material material;
			if (!dictionary.TryGetValue(text, out material))
			{
				material = this.CreateMaterial(fontMaterial, LLBase.CurrentLang, overrideOutlineColor, overrideSecondOutlineColor);
				dictionary.Add(text, material);
			}
			return material;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00026238 File Offset: 0x00024438
		private void SetColor(TMP_Text label, string lang, Color? overrideColor = null)
		{
			Color colorForLabel = this.GetColorForLabel(lang, overrideColor);
			if (colorForLabel != default(Color))
			{
				label.color = colorForLabel;
			}
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00026268 File Offset: 0x00024468
		private Color GetColorForLabel(string lang, Color? overrideColor = null)
		{
			OverrideTextStyleData overrideTextStyleData = this.overrideDataList.Find((OverrideTextStyleData x) => x.languages.Contains(lang) && x.data.containsFontColor);
			if (overrideTextStyleData != null)
			{
				return overrideTextStyleData.data.fontColor;
			}
			if (this.data.containsFontColor)
			{
				return this.data.fontColor;
			}
			if (overrideColor != null)
			{
				return overrideColor.Value;
			}
			return default(Color);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000262DC File Offset: 0x000244DC
		private Material CreateMaterial(string lang, bool staticFont, Color? overrideOutlineColor = null, Color? overrideSecondOutlineColor = null, bool applyLanguagePackFont = true)
		{
			TextStyleData textStyleData;
			bool flag = this.HasTextStyleData(lang, (TextStyleData style) => style.containsOutline, out textStyleData);
			TextStyleData textStyleData2;
			bool flag2 = this.HasTextStyleData(lang, (TextStyleData style) => style.containsSecondOutline, out textStyleData2);
			TextStyleData textStyleData3;
			bool flag3 = this.HasTextStyleData(lang, (TextStyleData style) => style.containsShadow, out textStyleData3);
			TextStyleData textStyleData4;
			bool flag4 = this.HasTextStyleData(lang, (TextStyleData style) => style.containsOverlayTexture, out textStyleData4);
			bool flag5 = flag && textStyleData.eightSide;
			Material material = new Material(TMPShaderSetup.FindShader(flag, flag2, flag3, flag4, flag5))
			{
				name = lang + ":[" + base.name + "]"
			};
			TMP_FontAsset fontAssetFor = this.Font.GetFontAssetFor(lang, staticFont, applyLanguagePackFont);
			material.SetTexture("_MainTex", fontAssetFor.atlasTexture);
			material.SetFloat("_MainTexSizeX", (float)fontAssetFor.atlasWidth);
			material.SetFloat("_MainTexSizeY", (float)fontAssetFor.atlasHeight);
			if (flag)
			{
				material.SetFloat("_Outline", 1f);
				material.SetColor("_OutlineColor", overrideOutlineColor ?? textStyleData.outlineColor);
			}
			if (flag2)
			{
				material.SetFloat("_Outline", 1f);
				material.SetColor("_OutlineColor2", overrideSecondOutlineColor ?? textStyleData2.secondOutlineColor);
			}
			if (flag3)
			{
				material.SetInt("_UseShadow", 1);
				material.SetColor("_ShadowColor", textStyleData3.shadowOutlineColor);
			}
			if (flag4)
			{
				material.SetTexture("_FaceTex", textStyleData4.overlayTexture);
			}
			return material;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000264C8 File Offset: 0x000246C8
		private Material CreateMaterial(Material fontMaterial, string lang, Color? overrideOutlineColor = null, Color? overrideSecondOutlineColor = null)
		{
			TextStyleData textStyleData;
			bool flag = this.HasTextStyleData(lang, (TextStyleData style) => style.containsOutline, out textStyleData);
			TextStyleData textStyleData2;
			this.HasTextStyleData(lang, (TextStyleData style) => style.containsSecondOutline, out textStyleData2);
			TextStyleData textStyleData3;
			this.HasTextStyleData(lang, (TextStyleData style) => style.containsShadow, out textStyleData3);
			TextStyleData textStyleData4;
			this.HasTextStyleData(lang, (TextStyleData style) => style.containsOverlayTexture, out textStyleData4);
			Material material = new Material(fontMaterial);
			if (flag)
			{
				material.EnableKeyword("UNDERLAY_ON");
				material.SetColor("_UnderlayColor", this.LinearizeColor(overrideOutlineColor ?? textStyleData.outlineColor));
				material.SetFloat("_UnderlayDilate", 0.7f);
			}
			else
			{
				material.DisableKeyword("UNDERLAY_ON");
			}
			return material;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000265DC File Offset: 0x000247DC
		private bool HasTextStyleData(string lang, Func<TextStyleData, bool> condition, out TextStyleData result)
		{
			OverrideTextStyleData overrideTextStyleData = this.overrideDataList.Find((OverrideTextStyleData x) => x.languages.Contains(lang) && condition(x.data));
			if (overrideTextStyleData != null)
			{
				result = overrideTextStyleData.data;
				return true;
			}
			if (condition(this.data))
			{
				result = this.data;
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00026643 File Offset: 0x00024843
		private float LinearizeColorComponent(float sRGBValue)
		{
			if ((double)sRGBValue > 0.04045)
			{
				return Mathf.Pow((sRGBValue + 0.055f) / 1.055f, 2.4f);
			}
			return sRGBValue / 12.92f;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00026671 File Offset: 0x00024871
		private Color LinearizeColor(Color sRGBColor)
		{
			return new Color(this.LinearizeColorComponent(sRGBColor.r), this.LinearizeColorComponent(sRGBColor.g), this.LinearizeColorComponent(sRGBColor.b), sRGBColor.a);
		}

		// Token: 0x04000482 RID: 1154
		public LazyFontData lazyFont;

		// Token: 0x04000483 RID: 1155
		public TextStyleData data;

		// Token: 0x04000484 RID: 1156
		public List<OverrideTextStyleData> overrideDataList = new List<OverrideTextStyleData>();

		// Token: 0x04000485 RID: 1157
		[NonSerialized]
		private Dictionary<string, Material> cachedMaterialsByDynamicLangId = new Dictionary<string, Material>();

		// Token: 0x04000486 RID: 1158
		[NonSerialized]
		private Dictionary<string, Material> cachedMaterialsByStaticLangId = new Dictionary<string, Material>();
	}
}
