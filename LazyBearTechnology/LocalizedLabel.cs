using System;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200010B RID: 267
	public class LocalizedLabel : MonoBehaviour
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x0001C8FC File Offset: 0x0001AAFC
		public bool IgnoreLocalize { get; set; }

		// Token: 0x06000558 RID: 1368 RVA: 0x0001C905 File Offset: 0x0001AB05
		private void Start()
		{
			this.Localize();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001C910 File Offset: 0x0001AB10
		public void Localize()
		{
			if (this.IgnoreLocalize)
			{
				return;
			}
			this.label = base.GetComponent<TextMeshProUGUI>();
			this.isInitialized = true;
			if (string.IsNullOrEmpty(this.langToken))
			{
				Debug.LogError("LocalizedLabel token is empty", base.gameObject);
				return;
			}
			this.label.text = ((this.highlightColorType != null) ? this.highlightColorType.TranslateAndColorizeTags(this.langToken) : LLBase.L(this.langToken));
		}

		// Token: 0x04000278 RID: 632
		[SerializeField]
		public string langToken = string.Empty;

		// Token: 0x04000279 RID: 633
		[SerializeField]
		private TextStyle highlightColorType;

		// Token: 0x0400027A RID: 634
		private TextMeshProUGUI label;

		// Token: 0x0400027B RID: 635
		private bool isInitialized;
	}
}
