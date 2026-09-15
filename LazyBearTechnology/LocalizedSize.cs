using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000197 RID: 407
	[RequireComponent(typeof(RectTransform))]
	public class LocalizedSize : MonoBehaviour
	{
		// Token: 0x0600092C RID: 2348 RVA: 0x0002C567 File Offset: 0x0002A767
		private void Awake()
		{
			this.Apply();
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002C56F File Offset: 0x0002A76F
		private void OnEnable()
		{
			this.Apply();
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002C578 File Offset: 0x0002A778
		public void Apply()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = (RectTransform)base.transform;
			}
			if (!this.originalStored)
			{
				this.originalSize = this.rectTransform.sizeDelta;
				this.originalStored = true;
			}
			this.rectTransform.sizeDelta = (this.ShouldApply() ? (this.originalSize + new Vector2(this.deltaX, this.deltaY)) : this.originalSize);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002C5FC File Offset: 0x0002A7FC
		private bool ShouldApply()
		{
			string currentLang = LLBase.CurrentLang;
			if (LanguageModHooks.RequiresResize != null && LanguageModHooks.RequiresResize(currentLang))
			{
				return true;
			}
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

		// Token: 0x0400057D RID: 1405
		[SerializeField]
		private bool japanese = true;

		// Token: 0x0400057E RID: 1406
		[SerializeField]
		private bool korean = true;

		// Token: 0x0400057F RID: 1407
		[SerializeField]
		private bool chineseSimplified = true;

		// Token: 0x04000580 RID: 1408
		[SerializeField]
		private float deltaX;

		// Token: 0x04000581 RID: 1409
		[SerializeField]
		private float deltaY;

		// Token: 0x04000582 RID: 1410
		private RectTransform rectTransform;

		// Token: 0x04000583 RID: 1411
		private Vector2 originalSize;

		// Token: 0x04000584 RID: 1412
		private bool originalStored;
	}
}
