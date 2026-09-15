using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace LazyBearTechnology
{
	// Token: 0x02000C4A RID: 3146
	[RequireComponent(typeof(TMP_Text))]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public sealed class ArabicTextPreprocessor : MonoBehaviour, ITextPreprocessor
	{
		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x0600502E RID: 20526 RVA: 0x0017AA9A File Offset: 0x00178C9A
		// (set) Token: 0x0600502F RID: 20527 RVA: 0x0017AAA2 File Offset: 0x00178CA2
		public bool Tashkeel
		{
			get
			{
				return this.tashkeel;
			}
			set
			{
				if (this.tashkeel == value)
				{
					return;
				}
				this.tashkeel = value;
				this.Reprocess();
			}
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x0017AABB File Offset: 0x00178CBB
		private void Reprocess()
		{
			if (this.label != null && base.isActiveAndEnabled)
			{
				this.label.ForceMeshUpdate(false, false);
			}
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x0017AAE0 File Offset: 0x00178CE0
		public string PreprocessText(string text)
		{
			if (this.previous != null)
			{
				text = this.previous.PreprocessText(text);
			}
			return ArabicShaper.ShapeForRender(text, this.tashkeel);
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x0017AB04 File Offset: 0x00178D04
		private void OnEnable()
		{
			this.label = base.GetComponent<TMP_Text>();
			if (this.label == null)
			{
				return;
			}
			if (this.label.textPreprocessor != this)
			{
				this.previous = this.label.textPreprocessor;
			}
			this.label.textPreprocessor = this;
			List<OTL_FeatureTag> fontFeatures = this.label.fontFeatures;
			if (!fontFeatures.Contains(OTL_FeatureTag.mark) || !fontFeatures.Contains(OTL_FeatureTag.mkmk))
			{
				if (!fontFeatures.Contains(OTL_FeatureTag.mark))
				{
					fontFeatures.Add(OTL_FeatureTag.mark);
				}
				if (!fontFeatures.Contains(OTL_FeatureTag.mkmk))
				{
					fontFeatures.Add(OTL_FeatureTag.mkmk);
				}
				this.label.fontFeatures = fontFeatures;
			}
			this.label.ForceMeshUpdate(false, false);
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x0017ABC8 File Offset: 0x00178DC8
		private void OnDisable()
		{
			if (this.label == null || this.label.textPreprocessor != this)
			{
				return;
			}
			this.label.textPreprocessor = this.previous;
			this.previous = null;
			this.label.ForceMeshUpdate(false, false);
		}

		// Token: 0x040041B0 RID: 16816
		[Tooltip("Render tashkeel/harakat (diacritic marks). When off they are stripped from the text.")]
		[SerializeField]
		private bool tashkeel;

		// Token: 0x040041B1 RID: 16817
		private TMP_Text label;

		// Token: 0x040041B2 RID: 16818
		private ITextPreprocessor previous;
	}
}
