using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000157 RID: 343
	[RequireComponent(typeof(TMP_Text))]
	public class LocalizedTextMargins : MonoBehaviour
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00025C17 File Offset: 0x00023E17
		private TMP_Text Label
		{
			get
			{
				if (this.label == null)
				{
					this.label = base.GetComponent<TMP_Text>();
				}
				return this.label;
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00025C39 File Offset: 0x00023E39
		private void OnEnable()
		{
			this.ApplyMargins();
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00025C41 File Offset: 0x00023E41
		public void ApplyMargins()
		{
			this.Label.margin = this.GetMarginsForCurrentLanguage();
			this.Label.ForceMeshUpdate(false, false);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.Label.rectTransform);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00025C74 File Offset: 0x00023E74
		private Vector4 GetMarginsForCurrentLanguage()
		{
			string currentLang = LLBase.CurrentLang;
			foreach (LocalizedTextMargins.LocalizedMarginsData localizedMarginsData in this.localizedMargins)
			{
				if (localizedMarginsData != null && localizedMarginsData.Margins != null && !(localizedMarginsData.LanguageId != currentLang))
				{
					return localizedMarginsData.Margins.Value;
				}
			}
			return this.defaultMargins.Value;
		}

		// Token: 0x0400047D RID: 1149
		[SerializeField]
		private LocalizedTextMargins.MarginsData defaultMargins = new LocalizedTextMargins.MarginsData();

		// Token: 0x0400047E RID: 1150
		[SerializeField]
		private List<LocalizedTextMargins.LocalizedMarginsData> localizedMargins = new List<LocalizedTextMargins.LocalizedMarginsData>();

		// Token: 0x0400047F RID: 1151
		private TMP_Text label;

		// Token: 0x020001FB RID: 507
		[Serializable]
		private class MarginsData
		{
			// Token: 0x17000159 RID: 345
			// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0002ED88 File Offset: 0x0002CF88
			public Vector4 Value
			{
				get
				{
					return new Vector4(this.left, this.top, this.right, this.bottom);
				}
			}

			// Token: 0x040006B8 RID: 1720
			[SerializeField]
			private float left;

			// Token: 0x040006B9 RID: 1721
			[SerializeField]
			private float top;

			// Token: 0x040006BA RID: 1722
			[SerializeField]
			private float right;

			// Token: 0x040006BB RID: 1723
			[SerializeField]
			private float bottom;
		}

		// Token: 0x020001FC RID: 508
		[Serializable]
		private class LocalizedMarginsData
		{
			// Token: 0x1700015A RID: 346
			// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0002EDAF File Offset: 0x0002CFAF
			public string LanguageId
			{
				get
				{
					return this.languageId;
				}
			}

			// Token: 0x1700015B RID: 347
			// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0002EDB7 File Offset: 0x0002CFB7
			public LocalizedTextMargins.MarginsData Margins
			{
				get
				{
					return this.margins;
				}
			}

			// Token: 0x040006BC RID: 1724
			[SerializeField]
			private string languageId;

			// Token: 0x040006BD RID: 1725
			[SerializeField]
			private LocalizedTextMargins.MarginsData margins;
		}
	}
}
