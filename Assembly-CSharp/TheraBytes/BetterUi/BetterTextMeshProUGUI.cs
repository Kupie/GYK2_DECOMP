using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheraBytes.BetterUi
{
	// Token: 0x02000B48 RID: 2888
	[ExecuteAlways]
	[HelpURL("https://documentation.therabytes.de/better-ui/BetterTextMeshProUGUI.html")]
	[AddComponentMenu("Better UI/TextMeshPro/Better TextMeshPro Text", 30)]
	public class BetterTextMeshProUGUI : TextMeshProUGUI, IResolutionDependency
	{
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004CAF RID: 19631 RVA: 0x00169BFD File Offset: 0x00167DFD
		// (set) Token: 0x06004CB0 RID: 19632 RVA: 0x00169C05 File Offset: 0x00167E05
		public BetterText.FittingMode Fitting
		{
			get
			{
				return this.fitting;
			}
			set
			{
				if (this.fitting == value)
				{
					return;
				}
				this.fitting = value;
				this.CalculateSize();
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06004CB1 RID: 19633 RVA: 0x00169C1E File Offset: 0x00167E1E
		public MarginSizeModifier MarginSizer
		{
			get
			{
				return this.customMarginSizers.GetCurrentItem(this.marginSizerFallback);
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x00169C31 File Offset: 0x00167E31
		public FloatSizeModifier FontSizer
		{
			get
			{
				return this.customFontSizers.GetCurrentItem(this.fontSizerFallback);
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x00169C44 File Offset: 0x00167E44
		public FloatSizeModifier MinFontSizer
		{
			get
			{
				return this.customMinFontSizers.GetCurrentItem(this.minFontSizerFallback);
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x00169C57 File Offset: 0x00167E57
		public FloatSizeModifier MaxFontSizer
		{
			get
			{
				return this.customMaxFontSizers.GetCurrentItem(this.maxFontSizerFallback);
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06004CB5 RID: 19637 RVA: 0x00169C6A File Offset: 0x00167E6A
		// (set) Token: 0x06004CB6 RID: 19638 RVA: 0x00169C72 File Offset: 0x00167E72
		public bool IgnoreFontSizerOptions { get; set; }

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x00169C7B File Offset: 0x00167E7B
		// (set) Token: 0x06004CB8 RID: 19640 RVA: 0x00169C83 File Offset: 0x00167E83
		public new float fontSize
		{
			get
			{
				return base.fontSize;
			}
			set
			{
				Config.Set<float>(value, delegate(float o)
				{
					base.fontSize = o;
				}, delegate(float o)
				{
					this.FontSizer.SetSize(this, o);
				});
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x00169CA3 File Offset: 0x00167EA3
		// (set) Token: 0x06004CBA RID: 19642 RVA: 0x00169CAB File Offset: 0x00167EAB
		public new float fontSizeMin
		{
			get
			{
				return base.fontSizeMin;
			}
			set
			{
				Config.Set<float>(value, delegate(float o)
				{
					base.fontSizeMin = o;
				}, delegate(float o)
				{
					this.MinFontSizer.SetSize(this, o);
				});
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x00169CCB File Offset: 0x00167ECB
		// (set) Token: 0x06004CBC RID: 19644 RVA: 0x00169CD3 File Offset: 0x00167ED3
		public new float fontSizeMax
		{
			get
			{
				return base.fontSizeMax;
			}
			set
			{
				Config.Set<float>(value, delegate(float o)
				{
					base.fontSizeMax = o;
				}, delegate(float o)
				{
					this.MaxFontSizer.SetSize(this, o);
				});
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06004CBD RID: 19645 RVA: 0x00169CF3 File Offset: 0x00167EF3
		// (set) Token: 0x06004CBE RID: 19646 RVA: 0x00169CFB File Offset: 0x00167EFB
		public new Vector4 margin
		{
			get
			{
				return base.margin;
			}
			set
			{
				Config.Set<Vector4>(value, delegate(Vector4 o)
				{
					base.margin = o;
				}, delegate(Vector4 o)
				{
					this.MarginSizer.SetSize(this, new Margin(o, Vector4Order.LeftTopRightBottom));
				});
			}
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x00169D1B File Offset: 0x00167F1B
		protected override void OnEnable()
		{
			this.CalculateSize();
			base.OnEnable();
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x00169D29 File Offset: 0x00167F29
		public void OnResolutionChanged()
		{
			this.CalculateSize();
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x00169D31 File Offset: 0x00167F31
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			this.CalculateSize();
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x00169D40 File Offset: 0x00167F40
		public void CalculateSize()
		{
			if (this.IgnoreFontSizerOptions)
			{
				base.enableAutoSizing = false;
			}
			else
			{
				switch (this.fitting)
				{
				case BetterText.FittingMode.SizerOnly:
					base.enableAutoSizing = false;
					base.fontSize = this.FontSizer.CalculateSize(this, "FontSizer");
					break;
				case BetterText.FittingMode.StayInBounds:
					base.enableAutoSizing = true;
					base.fontSizeMin = this.MinFontSizer.CalculateSize(this, "MinFontSizer");
					base.fontSizeMax = this.FontSizer.CalculateSize(this, "FontSizer");
					break;
				case BetterText.FittingMode.BestFit:
					base.enableAutoSizing = true;
					base.fontSizeMin = this.MinFontSizer.CalculateSize(this, "MinFontSizer");
					base.fontSizeMax = this.MaxFontSizer.CalculateSize(this, "MaxFontSizer");
					break;
				}
			}
			base.margin = this.MarginSizer.CalculateSize(this, "MarginSizer").ToVector4();
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x00169E27 File Offset: 0x00168027
		public void RegisterMaterials(Material[] materials)
		{
			base.GetMaterials(materials);
		}

		// Token: 0x04003DBD RID: 15805
		[SerializeField]
		private BetterText.FittingMode fitting;

		// Token: 0x04003DBE RID: 15806
		[FormerlySerializedAs("marginSizer")]
		[SerializeField]
		private MarginSizeModifier marginSizerFallback = new MarginSizeModifier(new Margin(), new Margin(), new Margin(1000, 1000, 1000, 1000));

		// Token: 0x04003DBF RID: 15807
		[SerializeField]
		private MarginSizeConfigCollection customMarginSizers = new MarginSizeConfigCollection();

		// Token: 0x04003DC0 RID: 15808
		[FormerlySerializedAs("fontSizer")]
		[SerializeField]
		private FloatSizeModifier fontSizerFallback = new FloatSizeModifier(36f, 10f, 500f);

		// Token: 0x04003DC1 RID: 15809
		[SerializeField]
		private FloatSizeConfigCollection customFontSizers = new FloatSizeConfigCollection();

		// Token: 0x04003DC2 RID: 15810
		[FormerlySerializedAs("minFontSizer")]
		[SerializeField]
		private FloatSizeModifier minFontSizerFallback = new FloatSizeModifier(10f, 10f, 500f);

		// Token: 0x04003DC3 RID: 15811
		[SerializeField]
		private FloatSizeConfigCollection customMinFontSizers = new FloatSizeConfigCollection();

		// Token: 0x04003DC4 RID: 15812
		[FormerlySerializedAs("maxFontSizer")]
		[SerializeField]
		private FloatSizeModifier maxFontSizerFallback = new FloatSizeModifier(500f, 500f, 500f);

		// Token: 0x04003DC5 RID: 15813
		[SerializeField]
		private FloatSizeConfigCollection customMaxFontSizers = new FloatSizeConfigCollection();
	}
}
