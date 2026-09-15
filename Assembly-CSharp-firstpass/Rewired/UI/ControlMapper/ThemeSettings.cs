using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000CD RID: 205
	[Serializable]
	public class ThemeSettings : ScriptableObject
	{
		// Token: 0x06000B04 RID: 2820 RVA: 0x0001E20C File Offset: 0x0001C40C
		public void Apply(ThemedElement.ElementInfo[] elementInfo)
		{
			if (elementInfo == null)
			{
				return;
			}
			for (int i = 0; i < elementInfo.Length; i++)
			{
				if (elementInfo[i] != null)
				{
					this.Apply(elementInfo[i].themeClass, elementInfo[i].component);
				}
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0001E248 File Offset: 0x0001C448
		private void Apply(string themeClass, Component component)
		{
			if (component as Selectable != null)
			{
				this.Apply(themeClass, (Selectable)component);
				return;
			}
			if (component as Image != null)
			{
				this.Apply(themeClass, (Image)component);
				return;
			}
			if (component as TMP_Text != null)
			{
				this.Apply(themeClass, (TMP_Text)component);
				return;
			}
			if (component as UIImageHelper != null)
			{
				this.Apply(themeClass, (UIImageHelper)component);
				return;
			}
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0001E2C8 File Offset: 0x0001C4C8
		private void Apply(string themeClass, Selectable item)
		{
			if (item == null)
			{
				return;
			}
			ThemeSettings.SelectableSettings_Base selectableSettings_Base;
			if (item as Button != null)
			{
				if (themeClass == "inputGridField")
				{
					selectableSettings_Base = this._inputGridFieldSettings;
				}
				else
				{
					selectableSettings_Base = this._buttonSettings;
				}
			}
			else if (item as Scrollbar != null)
			{
				selectableSettings_Base = this._scrollbarSettings;
			}
			else if (item as Slider != null)
			{
				selectableSettings_Base = this._sliderSettings;
			}
			else if (item as Toggle != null)
			{
				if (themeClass == "button")
				{
					selectableSettings_Base = this._buttonSettings;
				}
				else
				{
					selectableSettings_Base = this._selectableSettings;
				}
			}
			else
			{
				selectableSettings_Base = this._selectableSettings;
			}
			selectableSettings_Base.Apply(item);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0001E378 File Offset: 0x0001C578
		private void Apply(string themeClass, Image item)
		{
			if (item == null)
			{
				return;
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(themeClass);
			if (num <= 2822822017U)
			{
				if (num <= 665291243U)
				{
					if (num != 106194061U)
					{
						if (num != 283896133U)
						{
							if (num != 665291243U)
							{
								return;
							}
							if (!(themeClass == "calibrationBackground"))
							{
								return;
							}
							if (this._calibrationBackground != null)
							{
								this._calibrationBackground.CopyTo(item);
								return;
							}
						}
						else
						{
							if (!(themeClass == "popupWindow"))
							{
								return;
							}
							if (this._popupWindowBackground != null)
							{
								this._popupWindowBackground.CopyTo(item);
								return;
							}
						}
					}
					else
					{
						if (!(themeClass == "invertToggleButtonBackground"))
						{
							return;
						}
						if (this._buttonSettings != null)
						{
							this._buttonSettings.imageSettings.CopyTo(item);
						}
					}
				}
				else if (num != 2579191547U)
				{
					if (num != 2601460036U)
					{
						if (num != 2822822017U)
						{
							return;
						}
						if (!(themeClass == "invertToggle"))
						{
							return;
						}
						if (this._invertToggle != null)
						{
							this._invertToggle.CopyTo(item);
							return;
						}
					}
					else
					{
						if (!(themeClass == "area"))
						{
							return;
						}
						if (this._areaBackground != null)
						{
							this._areaBackground.CopyTo(item);
							return;
						}
					}
				}
				else
				{
					if (!(themeClass == "calibrationDeadzone"))
					{
						return;
					}
					if (this._calibrationDeadzone != null)
					{
						this._calibrationDeadzone.CopyTo(item);
						return;
					}
				}
			}
			else if (num <= 3490313510U)
			{
				if (num != 2998767316U)
				{
					if (num != 3338297968U)
					{
						if (num != 3490313510U)
						{
							return;
						}
						if (!(themeClass == "calibrationRawValueMarker"))
						{
							return;
						}
						if (this._calibrationRawValueMarker != null)
						{
							this._calibrationRawValueMarker.CopyTo(item);
							return;
						}
					}
					else
					{
						if (!(themeClass == "calibrationCalibratedZeroMarker"))
						{
							return;
						}
						if (this._calibrationCalibratedZeroMarker != null)
						{
							this._calibrationCalibratedZeroMarker.CopyTo(item);
							return;
						}
					}
				}
				else
				{
					if (!(themeClass == "mainWindow"))
					{
						return;
					}
					if (this._mainWindowBackground != null)
					{
						this._mainWindowBackground.CopyTo(item);
						return;
					}
				}
			}
			else if (num != 3776179782U)
			{
				if (num != 3836396811U)
				{
					if (num != 3911450241U)
					{
						return;
					}
					if (!(themeClass == "invertToggleBackground"))
					{
						return;
					}
					if (this._inputGridFieldSettings != null)
					{
						this._inputGridFieldSettings.imageSettings.CopyTo(item);
						return;
					}
				}
				else
				{
					if (!(themeClass == "calibrationZeroMarker"))
					{
						return;
					}
					if (this._calibrationZeroMarker != null)
					{
						this._calibrationZeroMarker.CopyTo(item);
						return;
					}
				}
			}
			else
			{
				if (!(themeClass == "calibrationValueMarker"))
				{
					return;
				}
				if (this._calibrationValueMarker != null)
				{
					this._calibrationValueMarker.CopyTo(item);
					return;
				}
			}
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0001E608 File Offset: 0x0001C808
		private void Apply(string themeClass, TMP_Text item)
		{
			if (item == null)
			{
				return;
			}
			ThemeSettings.TextSettings textSettings;
			if (!(themeClass == "button"))
			{
				if (!(themeClass == "inputGridField"))
				{
					textSettings = this._textSettings;
				}
				else
				{
					textSettings = this._inputGridFieldTextSettings;
				}
			}
			else
			{
				textSettings = this._buttonTextSettings;
			}
			if (textSettings.font != null)
			{
				item.font = textSettings.font;
			}
			item.color = textSettings.color;
			item.lineSpacing = textSettings.lineSpacing;
			if (textSettings.sizeMultiplier != 1f)
			{
				item.fontSize = (float)((int)(item.fontSize * textSettings.sizeMultiplier));
				item.fontSizeMax = (float)((int)(item.fontSizeMax * textSettings.sizeMultiplier));
				item.fontSizeMin = (float)((int)(item.fontSizeMin * textSettings.sizeMultiplier));
			}
			item.characterSpacing = textSettings.chracterSpacing;
			item.wordSpacing = textSettings.wordSpacing;
			if (textSettings.style != ThemeSettings.FontStyleOverride.Default)
			{
				item.fontStyle = ThemeSettings.GetFontStyle(textSettings.style);
			}
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0001E703 File Offset: 0x0001C903
		private void Apply(string themeClass, UIImageHelper item)
		{
			if (item == null)
			{
				return;
			}
			item.SetEnabledStateColor(this._invertToggle.color);
			item.SetDisabledStateColor(this._invertToggleDisabledColor);
			item.Refresh();
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0001E732 File Offset: 0x0001C932
		private static FontStyles GetFontStyle(ThemeSettings.FontStyleOverride style)
		{
			switch (style)
			{
			case ThemeSettings.FontStyleOverride.Default:
			case ThemeSettings.FontStyleOverride.Normal:
				return FontStyles.Normal;
			case ThemeSettings.FontStyleOverride.Bold:
				return FontStyles.Bold;
			case ThemeSettings.FontStyleOverride.Italic:
				return FontStyles.Italic;
			case ThemeSettings.FontStyleOverride.BoldAndItalic:
				return FontStyles.Bold | FontStyles.Italic;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x04000564 RID: 1380
		[SerializeField]
		private ThemeSettings.ImageSettings _mainWindowBackground;

		// Token: 0x04000565 RID: 1381
		[SerializeField]
		private ThemeSettings.ImageSettings _popupWindowBackground;

		// Token: 0x04000566 RID: 1382
		[SerializeField]
		private ThemeSettings.ImageSettings _areaBackground;

		// Token: 0x04000567 RID: 1383
		[SerializeField]
		private ThemeSettings.SelectableSettings _selectableSettings;

		// Token: 0x04000568 RID: 1384
		[SerializeField]
		private ThemeSettings.SelectableSettings _buttonSettings;

		// Token: 0x04000569 RID: 1385
		[SerializeField]
		private ThemeSettings.SelectableSettings _inputGridFieldSettings;

		// Token: 0x0400056A RID: 1386
		[SerializeField]
		private ThemeSettings.ScrollbarSettings _scrollbarSettings;

		// Token: 0x0400056B RID: 1387
		[SerializeField]
		private ThemeSettings.SliderSettings _sliderSettings;

		// Token: 0x0400056C RID: 1388
		[SerializeField]
		private ThemeSettings.ImageSettings _invertToggle;

		// Token: 0x0400056D RID: 1389
		[SerializeField]
		private Color _invertToggleDisabledColor;

		// Token: 0x0400056E RID: 1390
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationBackground;

		// Token: 0x0400056F RID: 1391
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationValueMarker;

		// Token: 0x04000570 RID: 1392
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationRawValueMarker;

		// Token: 0x04000571 RID: 1393
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationZeroMarker;

		// Token: 0x04000572 RID: 1394
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationCalibratedZeroMarker;

		// Token: 0x04000573 RID: 1395
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationDeadzone;

		// Token: 0x04000574 RID: 1396
		[SerializeField]
		private ThemeSettings.TextSettings _textSettings;

		// Token: 0x04000575 RID: 1397
		[SerializeField]
		private ThemeSettings.TextSettings _buttonTextSettings;

		// Token: 0x04000576 RID: 1398
		[SerializeField]
		private ThemeSettings.TextSettings _inputGridFieldTextSettings;

		// Token: 0x020000CE RID: 206
		[Serializable]
		private abstract class SelectableSettings_Base
		{
			// Token: 0x17000451 RID: 1105
			// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0001E75D File Offset: 0x0001C95D
			public Selectable.Transition transition
			{
				get
				{
					return this._transition;
				}
			}

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0001E765 File Offset: 0x0001C965
			public ThemeSettings.CustomColorBlock selectableColors
			{
				get
				{
					return this._colors;
				}
			}

			// Token: 0x17000453 RID: 1107
			// (get) Token: 0x06000B0E RID: 2830 RVA: 0x0001E76D File Offset: 0x0001C96D
			public ThemeSettings.CustomSpriteState spriteState
			{
				get
				{
					return this._spriteState;
				}
			}

			// Token: 0x17000454 RID: 1108
			// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0001E775 File Offset: 0x0001C975
			public ThemeSettings.CustomAnimationTriggers animationTriggers
			{
				get
				{
					return this._animationTriggers;
				}
			}

			// Token: 0x06000B10 RID: 2832 RVA: 0x0001E780 File Offset: 0x0001C980
			public virtual void Apply(Selectable item)
			{
				Selectable.Transition transition = this._transition;
				bool flag = item.transition != transition;
				item.transition = transition;
				ICustomSelectable customSelectable = item as ICustomSelectable;
				if (transition == Selectable.Transition.ColorTint)
				{
					ThemeSettings.CustomColorBlock colors = this._colors;
					colors.fadeDuration = 0f;
					item.colors = colors;
					colors.fadeDuration = this._colors.fadeDuration;
					item.colors = colors;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedColor = colors.disabledHighlightedColor;
					}
				}
				else if (transition == Selectable.Transition.SpriteSwap)
				{
					item.spriteState = this._spriteState;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedSprite = this._spriteState.disabledHighlightedSprite;
					}
				}
				else if (transition == Selectable.Transition.Animation)
				{
					item.animationTriggers.disabledTrigger = this._animationTriggers.disabledTrigger;
					item.animationTriggers.highlightedTrigger = this._animationTriggers.highlightedTrigger;
					item.animationTriggers.normalTrigger = this._animationTriggers.normalTrigger;
					item.animationTriggers.pressedTrigger = this._animationTriggers.pressedTrigger;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedTrigger = this._animationTriggers.disabledHighlightedTrigger;
					}
				}
				if (flag)
				{
					item.targetGraphic.CrossFadeColor(item.targetGraphic.color, 0f, true, true);
				}
			}

			// Token: 0x04000577 RID: 1399
			[SerializeField]
			protected Selectable.Transition _transition;

			// Token: 0x04000578 RID: 1400
			[SerializeField]
			protected ThemeSettings.CustomColorBlock _colors;

			// Token: 0x04000579 RID: 1401
			[SerializeField]
			protected ThemeSettings.CustomSpriteState _spriteState;

			// Token: 0x0400057A RID: 1402
			[SerializeField]
			protected ThemeSettings.CustomAnimationTriggers _animationTriggers;
		}

		// Token: 0x020000CF RID: 207
		[Serializable]
		private class SelectableSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x17000455 RID: 1109
			// (get) Token: 0x06000B12 RID: 2834 RVA: 0x0001E8C4 File Offset: 0x0001CAC4
			public ThemeSettings.ImageSettings imageSettings
			{
				get
				{
					return this._imageSettings;
				}
			}

			// Token: 0x06000B13 RID: 2835 RVA: 0x0001E8CC File Offset: 0x0001CACC
			public override void Apply(Selectable item)
			{
				if (item == null)
				{
					return;
				}
				base.Apply(item);
				if (this._imageSettings != null)
				{
					this._imageSettings.CopyTo(item.targetGraphic as Image);
				}
			}

			// Token: 0x0400057B RID: 1403
			[SerializeField]
			private ThemeSettings.ImageSettings _imageSettings;
		}

		// Token: 0x020000D0 RID: 208
		[Serializable]
		private class SliderSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x17000456 RID: 1110
			// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0001E905 File Offset: 0x0001CB05
			public ThemeSettings.ImageSettings handleImageSettings
			{
				get
				{
					return this._handleImageSettings;
				}
			}

			// Token: 0x17000457 RID: 1111
			// (get) Token: 0x06000B16 RID: 2838 RVA: 0x0001E90D File Offset: 0x0001CB0D
			public ThemeSettings.ImageSettings fillImageSettings
			{
				get
				{
					return this._fillImageSettings;
				}
			}

			// Token: 0x17000458 RID: 1112
			// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0001E915 File Offset: 0x0001CB15
			public ThemeSettings.ImageSettings backgroundImageSettings
			{
				get
				{
					return this._backgroundImageSettings;
				}
			}

			// Token: 0x06000B18 RID: 2840 RVA: 0x0001E920 File Offset: 0x0001CB20
			private void Apply(Slider item)
			{
				if (item == null)
				{
					return;
				}
				if (this._handleImageSettings != null)
				{
					this._handleImageSettings.CopyTo(item.targetGraphic as Image);
				}
				if (this._fillImageSettings != null)
				{
					RectTransform fillRect = item.fillRect;
					if (fillRect != null)
					{
						this._fillImageSettings.CopyTo(fillRect.GetComponent<Image>());
					}
				}
				if (this._backgroundImageSettings != null)
				{
					Transform transform = item.transform.Find("Background");
					if (transform != null)
					{
						this._backgroundImageSettings.CopyTo(transform.GetComponent<Image>());
					}
				}
			}

			// Token: 0x06000B19 RID: 2841 RVA: 0x0001E9B1 File Offset: 0x0001CBB1
			public override void Apply(Selectable item)
			{
				base.Apply(item);
				this.Apply(item as Slider);
			}

			// Token: 0x0400057C RID: 1404
			[SerializeField]
			private ThemeSettings.ImageSettings _handleImageSettings;

			// Token: 0x0400057D RID: 1405
			[SerializeField]
			private ThemeSettings.ImageSettings _fillImageSettings;

			// Token: 0x0400057E RID: 1406
			[SerializeField]
			private ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		// Token: 0x020000D1 RID: 209
		[Serializable]
		private class ScrollbarSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x17000459 RID: 1113
			// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0001E9C6 File Offset: 0x0001CBC6
			public ThemeSettings.ImageSettings handle
			{
				get
				{
					return this._handleImageSettings;
				}
			}

			// Token: 0x1700045A RID: 1114
			// (get) Token: 0x06000B1C RID: 2844 RVA: 0x0001E9CE File Offset: 0x0001CBCE
			public ThemeSettings.ImageSettings background
			{
				get
				{
					return this._backgroundImageSettings;
				}
			}

			// Token: 0x06000B1D RID: 2845 RVA: 0x0001E9D8 File Offset: 0x0001CBD8
			private void Apply(Scrollbar item)
			{
				if (item == null)
				{
					return;
				}
				if (this._handleImageSettings != null)
				{
					this._handleImageSettings.CopyTo(item.targetGraphic as Image);
				}
				if (this._backgroundImageSettings != null)
				{
					this._backgroundImageSettings.CopyTo(item.GetComponent<Image>());
				}
			}

			// Token: 0x06000B1E RID: 2846 RVA: 0x0001EA26 File Offset: 0x0001CC26
			public override void Apply(Selectable item)
			{
				base.Apply(item);
				this.Apply(item as Scrollbar);
			}

			// Token: 0x0400057F RID: 1407
			[SerializeField]
			private ThemeSettings.ImageSettings _handleImageSettings;

			// Token: 0x04000580 RID: 1408
			[SerializeField]
			private ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		// Token: 0x020000D2 RID: 210
		[Serializable]
		private class ImageSettings
		{
			// Token: 0x1700045B RID: 1115
			// (get) Token: 0x06000B20 RID: 2848 RVA: 0x0001EA3B File Offset: 0x0001CC3B
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x1700045C RID: 1116
			// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0001EA43 File Offset: 0x0001CC43
			public Sprite sprite
			{
				get
				{
					return this._sprite;
				}
			}

			// Token: 0x1700045D RID: 1117
			// (get) Token: 0x06000B22 RID: 2850 RVA: 0x0001EA4B File Offset: 0x0001CC4B
			public Material materal
			{
				get
				{
					return this._materal;
				}
			}

			// Token: 0x1700045E RID: 1118
			// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0001EA53 File Offset: 0x0001CC53
			public Image.Type type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x1700045F RID: 1119
			// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0001EA5B File Offset: 0x0001CC5B
			public bool preserveAspect
			{
				get
				{
					return this._preserveAspect;
				}
			}

			// Token: 0x17000460 RID: 1120
			// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0001EA63 File Offset: 0x0001CC63
			public bool fillCenter
			{
				get
				{
					return this._fillCenter;
				}
			}

			// Token: 0x17000461 RID: 1121
			// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0001EA6B File Offset: 0x0001CC6B
			public Image.FillMethod fillMethod
			{
				get
				{
					return this._fillMethod;
				}
			}

			// Token: 0x17000462 RID: 1122
			// (get) Token: 0x06000B27 RID: 2855 RVA: 0x0001EA73 File Offset: 0x0001CC73
			public float fillAmout
			{
				get
				{
					return this._fillAmout;
				}
			}

			// Token: 0x17000463 RID: 1123
			// (get) Token: 0x06000B28 RID: 2856 RVA: 0x0001EA7B File Offset: 0x0001CC7B
			public bool fillClockwise
			{
				get
				{
					return this._fillClockwise;
				}
			}

			// Token: 0x17000464 RID: 1124
			// (get) Token: 0x06000B29 RID: 2857 RVA: 0x0001EA83 File Offset: 0x0001CC83
			public int fillOrigin
			{
				get
				{
					return this._fillOrigin;
				}
			}

			// Token: 0x06000B2A RID: 2858 RVA: 0x0001EA8C File Offset: 0x0001CC8C
			public virtual void CopyTo(Image image)
			{
				if (image == null)
				{
					return;
				}
				image.color = this._color;
				image.sprite = this._sprite;
				image.material = this._materal;
				image.type = this._type;
				image.preserveAspect = this._preserveAspect;
				image.fillCenter = this._fillCenter;
				image.fillMethod = this._fillMethod;
				image.fillAmount = this._fillAmout;
				image.fillClockwise = this._fillClockwise;
				image.fillOrigin = this._fillOrigin;
			}

			// Token: 0x04000581 RID: 1409
			[SerializeField]
			private Color _color = Color.white;

			// Token: 0x04000582 RID: 1410
			[SerializeField]
			private Sprite _sprite;

			// Token: 0x04000583 RID: 1411
			[SerializeField]
			private Material _materal;

			// Token: 0x04000584 RID: 1412
			[SerializeField]
			private Image.Type _type;

			// Token: 0x04000585 RID: 1413
			[SerializeField]
			private bool _preserveAspect;

			// Token: 0x04000586 RID: 1414
			[SerializeField]
			private bool _fillCenter;

			// Token: 0x04000587 RID: 1415
			[SerializeField]
			private Image.FillMethod _fillMethod;

			// Token: 0x04000588 RID: 1416
			[SerializeField]
			private float _fillAmout;

			// Token: 0x04000589 RID: 1417
			[SerializeField]
			private bool _fillClockwise;

			// Token: 0x0400058A RID: 1418
			[SerializeField]
			private int _fillOrigin;
		}

		// Token: 0x020000D3 RID: 211
		[Serializable]
		private struct CustomColorBlock
		{
			// Token: 0x17000465 RID: 1125
			// (get) Token: 0x06000B2C RID: 2860 RVA: 0x0001EB2E File Offset: 0x0001CD2E
			// (set) Token: 0x06000B2D RID: 2861 RVA: 0x0001EB36 File Offset: 0x0001CD36
			public float colorMultiplier
			{
				get
				{
					return this.m_ColorMultiplier;
				}
				set
				{
					this.m_ColorMultiplier = value;
				}
			}

			// Token: 0x17000466 RID: 1126
			// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0001EB3F File Offset: 0x0001CD3F
			// (set) Token: 0x06000B2F RID: 2863 RVA: 0x0001EB47 File Offset: 0x0001CD47
			public Color disabledColor
			{
				get
				{
					return this.m_DisabledColor;
				}
				set
				{
					this.m_DisabledColor = value;
				}
			}

			// Token: 0x17000467 RID: 1127
			// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0001EB50 File Offset: 0x0001CD50
			// (set) Token: 0x06000B31 RID: 2865 RVA: 0x0001EB58 File Offset: 0x0001CD58
			public float fadeDuration
			{
				get
				{
					return this.m_FadeDuration;
				}
				set
				{
					this.m_FadeDuration = value;
				}
			}

			// Token: 0x17000468 RID: 1128
			// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0001EB61 File Offset: 0x0001CD61
			// (set) Token: 0x06000B33 RID: 2867 RVA: 0x0001EB69 File Offset: 0x0001CD69
			public Color highlightedColor
			{
				get
				{
					return this.m_HighlightedColor;
				}
				set
				{
					this.m_HighlightedColor = value;
				}
			}

			// Token: 0x17000469 RID: 1129
			// (get) Token: 0x06000B34 RID: 2868 RVA: 0x0001EB72 File Offset: 0x0001CD72
			// (set) Token: 0x06000B35 RID: 2869 RVA: 0x0001EB7A File Offset: 0x0001CD7A
			public Color normalColor
			{
				get
				{
					return this.m_NormalColor;
				}
				set
				{
					this.m_NormalColor = value;
				}
			}

			// Token: 0x1700046A RID: 1130
			// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0001EB83 File Offset: 0x0001CD83
			// (set) Token: 0x06000B37 RID: 2871 RVA: 0x0001EB8B File Offset: 0x0001CD8B
			public Color pressedColor
			{
				get
				{
					return this.m_PressedColor;
				}
				set
				{
					this.m_PressedColor = value;
				}
			}

			// Token: 0x1700046B RID: 1131
			// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0001EB94 File Offset: 0x0001CD94
			// (set) Token: 0x06000B39 RID: 2873 RVA: 0x0001EB9C File Offset: 0x0001CD9C
			public Color selectedColor
			{
				get
				{
					return this.m_SelectedColor;
				}
				set
				{
					this.m_SelectedColor = value;
				}
			}

			// Token: 0x1700046C RID: 1132
			// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0001EBA5 File Offset: 0x0001CDA5
			// (set) Token: 0x06000B3B RID: 2875 RVA: 0x0001EBAD File Offset: 0x0001CDAD
			public Color disabledHighlightedColor
			{
				get
				{
					return this.m_DisabledHighlightedColor;
				}
				set
				{
					this.m_DisabledHighlightedColor = value;
				}
			}

			// Token: 0x06000B3C RID: 2876 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
			public static implicit operator ColorBlock(ThemeSettings.CustomColorBlock item)
			{
				return new ColorBlock
				{
					selectedColor = item.m_SelectedColor,
					colorMultiplier = item.m_ColorMultiplier,
					disabledColor = item.m_DisabledColor,
					fadeDuration = item.m_FadeDuration,
					highlightedColor = item.m_HighlightedColor,
					normalColor = item.m_NormalColor,
					pressedColor = item.m_PressedColor
				};
			}

			// Token: 0x0400058B RID: 1419
			[SerializeField]
			private float m_ColorMultiplier;

			// Token: 0x0400058C RID: 1420
			[SerializeField]
			private Color m_DisabledColor;

			// Token: 0x0400058D RID: 1421
			[SerializeField]
			private float m_FadeDuration;

			// Token: 0x0400058E RID: 1422
			[SerializeField]
			private Color m_HighlightedColor;

			// Token: 0x0400058F RID: 1423
			[SerializeField]
			private Color m_NormalColor;

			// Token: 0x04000590 RID: 1424
			[SerializeField]
			private Color m_PressedColor;

			// Token: 0x04000591 RID: 1425
			[SerializeField]
			private Color m_SelectedColor;

			// Token: 0x04000592 RID: 1426
			[SerializeField]
			private Color m_DisabledHighlightedColor;
		}

		// Token: 0x020000D4 RID: 212
		[Serializable]
		private struct CustomSpriteState
		{
			// Token: 0x1700046D RID: 1133
			// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0001EC29 File Offset: 0x0001CE29
			// (set) Token: 0x06000B3E RID: 2878 RVA: 0x0001EC31 File Offset: 0x0001CE31
			public Sprite disabledSprite
			{
				get
				{
					return this.m_DisabledSprite;
				}
				set
				{
					this.m_DisabledSprite = value;
				}
			}

			// Token: 0x1700046E RID: 1134
			// (get) Token: 0x06000B3F RID: 2879 RVA: 0x0001EC3A File Offset: 0x0001CE3A
			// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0001EC42 File Offset: 0x0001CE42
			public Sprite highlightedSprite
			{
				get
				{
					return this.m_HighlightedSprite;
				}
				set
				{
					this.m_HighlightedSprite = value;
				}
			}

			// Token: 0x1700046F RID: 1135
			// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0001EC4B File Offset: 0x0001CE4B
			// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0001EC53 File Offset: 0x0001CE53
			public Sprite pressedSprite
			{
				get
				{
					return this.m_PressedSprite;
				}
				set
				{
					this.m_PressedSprite = value;
				}
			}

			// Token: 0x17000470 RID: 1136
			// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0001EC5C File Offset: 0x0001CE5C
			// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0001EC64 File Offset: 0x0001CE64
			public Sprite selectedSprite
			{
				get
				{
					return this.m_SelectedSprite;
				}
				set
				{
					this.m_SelectedSprite = value;
				}
			}

			// Token: 0x17000471 RID: 1137
			// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0001EC6D File Offset: 0x0001CE6D
			// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0001EC75 File Offset: 0x0001CE75
			public Sprite disabledHighlightedSprite
			{
				get
				{
					return this.m_DisabledHighlightedSprite;
				}
				set
				{
					this.m_DisabledHighlightedSprite = value;
				}
			}

			// Token: 0x06000B47 RID: 2887 RVA: 0x0001EC80 File Offset: 0x0001CE80
			public static implicit operator SpriteState(ThemeSettings.CustomSpriteState item)
			{
				return new SpriteState
				{
					selectedSprite = item.m_SelectedSprite,
					disabledSprite = item.m_DisabledSprite,
					highlightedSprite = item.m_HighlightedSprite,
					pressedSprite = item.m_PressedSprite
				};
			}

			// Token: 0x04000593 RID: 1427
			[SerializeField]
			private Sprite m_DisabledSprite;

			// Token: 0x04000594 RID: 1428
			[SerializeField]
			private Sprite m_HighlightedSprite;

			// Token: 0x04000595 RID: 1429
			[SerializeField]
			private Sprite m_PressedSprite;

			// Token: 0x04000596 RID: 1430
			[SerializeField]
			private Sprite m_SelectedSprite;

			// Token: 0x04000597 RID: 1431
			[SerializeField]
			private Sprite m_DisabledHighlightedSprite;
		}

		// Token: 0x020000D5 RID: 213
		[Serializable]
		private class CustomAnimationTriggers
		{
			// Token: 0x06000B48 RID: 2888 RVA: 0x0001ECCC File Offset: 0x0001CECC
			public CustomAnimationTriggers()
			{
				this.m_DisabledTrigger = string.Empty;
				this.m_HighlightedTrigger = string.Empty;
				this.m_NormalTrigger = string.Empty;
				this.m_PressedTrigger = string.Empty;
				this.m_SelectedTrigger = string.Empty;
				this.m_DisabledHighlightedTrigger = string.Empty;
			}

			// Token: 0x17000472 RID: 1138
			// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0001ED21 File Offset: 0x0001CF21
			// (set) Token: 0x06000B4A RID: 2890 RVA: 0x0001ED29 File Offset: 0x0001CF29
			public string disabledTrigger
			{
				get
				{
					return this.m_DisabledTrigger;
				}
				set
				{
					this.m_DisabledTrigger = value;
				}
			}

			// Token: 0x17000473 RID: 1139
			// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0001ED32 File Offset: 0x0001CF32
			// (set) Token: 0x06000B4C RID: 2892 RVA: 0x0001ED3A File Offset: 0x0001CF3A
			public string highlightedTrigger
			{
				get
				{
					return this.m_HighlightedTrigger;
				}
				set
				{
					this.m_HighlightedTrigger = value;
				}
			}

			// Token: 0x17000474 RID: 1140
			// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0001ED43 File Offset: 0x0001CF43
			// (set) Token: 0x06000B4E RID: 2894 RVA: 0x0001ED4B File Offset: 0x0001CF4B
			public string normalTrigger
			{
				get
				{
					return this.m_NormalTrigger;
				}
				set
				{
					this.m_NormalTrigger = value;
				}
			}

			// Token: 0x17000475 RID: 1141
			// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0001ED54 File Offset: 0x0001CF54
			// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0001ED5C File Offset: 0x0001CF5C
			public string pressedTrigger
			{
				get
				{
					return this.m_PressedTrigger;
				}
				set
				{
					this.m_PressedTrigger = value;
				}
			}

			// Token: 0x17000476 RID: 1142
			// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0001ED65 File Offset: 0x0001CF65
			// (set) Token: 0x06000B52 RID: 2898 RVA: 0x0001ED6D File Offset: 0x0001CF6D
			public string selectedTrigger
			{
				get
				{
					return this.m_SelectedTrigger;
				}
				set
				{
					this.m_SelectedTrigger = value;
				}
			}

			// Token: 0x17000477 RID: 1143
			// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0001ED76 File Offset: 0x0001CF76
			// (set) Token: 0x06000B54 RID: 2900 RVA: 0x0001ED7E File Offset: 0x0001CF7E
			public string disabledHighlightedTrigger
			{
				get
				{
					return this.m_DisabledHighlightedTrigger;
				}
				set
				{
					this.m_DisabledHighlightedTrigger = value;
				}
			}

			// Token: 0x06000B55 RID: 2901 RVA: 0x0001ED88 File Offset: 0x0001CF88
			public static implicit operator AnimationTriggers(ThemeSettings.CustomAnimationTriggers item)
			{
				return new AnimationTriggers
				{
					selectedTrigger = item.m_SelectedTrigger,
					disabledTrigger = item.m_DisabledTrigger,
					highlightedTrigger = item.m_HighlightedTrigger,
					normalTrigger = item.m_NormalTrigger,
					pressedTrigger = item.m_PressedTrigger
				};
			}

			// Token: 0x04000598 RID: 1432
			[SerializeField]
			private string m_DisabledTrigger;

			// Token: 0x04000599 RID: 1433
			[SerializeField]
			private string m_HighlightedTrigger;

			// Token: 0x0400059A RID: 1434
			[SerializeField]
			private string m_NormalTrigger;

			// Token: 0x0400059B RID: 1435
			[SerializeField]
			private string m_PressedTrigger;

			// Token: 0x0400059C RID: 1436
			[SerializeField]
			private string m_SelectedTrigger;

			// Token: 0x0400059D RID: 1437
			[SerializeField]
			private string m_DisabledHighlightedTrigger;
		}

		// Token: 0x020000D6 RID: 214
		[Serializable]
		private class TextSettings
		{
			// Token: 0x17000478 RID: 1144
			// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0001EDD6 File Offset: 0x0001CFD6
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x17000479 RID: 1145
			// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0001EDDE File Offset: 0x0001CFDE
			public TMP_FontAsset font
			{
				get
				{
					return this._font;
				}
			}

			// Token: 0x1700047A RID: 1146
			// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0001EDE6 File Offset: 0x0001CFE6
			public ThemeSettings.FontStyleOverride style
			{
				get
				{
					return this._style;
				}
			}

			// Token: 0x1700047B RID: 1147
			// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0001EDEE File Offset: 0x0001CFEE
			public float sizeMultiplier
			{
				get
				{
					return this._sizeMultiplier;
				}
			}

			// Token: 0x1700047C RID: 1148
			// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0001EDF6 File Offset: 0x0001CFF6
			public float lineSpacing
			{
				get
				{
					return this._lineSpacing;
				}
			}

			// Token: 0x1700047D RID: 1149
			// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0001EDFE File Offset: 0x0001CFFE
			public float chracterSpacing
			{
				get
				{
					return this._characterSpacing;
				}
			}

			// Token: 0x1700047E RID: 1150
			// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0001EE06 File Offset: 0x0001D006
			public float wordSpacing
			{
				get
				{
					return this._wordSpacing;
				}
			}

			// Token: 0x0400059E RID: 1438
			[SerializeField]
			private Color _color = Color.white;

			// Token: 0x0400059F RID: 1439
			[SerializeField]
			private TMP_FontAsset _font;

			// Token: 0x040005A0 RID: 1440
			[SerializeField]
			private ThemeSettings.FontStyleOverride _style;

			// Token: 0x040005A1 RID: 1441
			[SerializeField]
			private float _sizeMultiplier = 1f;

			// Token: 0x040005A2 RID: 1442
			[SerializeField]
			private float _lineSpacing = 1f;

			// Token: 0x040005A3 RID: 1443
			[SerializeField]
			private float _characterSpacing = 1f;

			// Token: 0x040005A4 RID: 1444
			[SerializeField]
			private float _wordSpacing = 1f;
		}

		// Token: 0x020000D7 RID: 215
		private enum FontStyleOverride
		{
			// Token: 0x040005A6 RID: 1446
			Default,
			// Token: 0x040005A7 RID: 1447
			Normal,
			// Token: 0x040005A8 RID: 1448
			Bold,
			// Token: 0x040005A9 RID: 1449
			Italic,
			// Token: 0x040005AA RID: 1450
			BoldAndItalic
		}
	}
}
