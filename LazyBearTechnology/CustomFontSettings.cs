using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000109 RID: 265
	public class CustomFontSettings : MonoBehaviour
	{
		// Token: 0x06000548 RID: 1352 RVA: 0x0001C520 File Offset: 0x0001A720
		private void CheckOriginalParamsCached()
		{
			if (this.originalParams != null)
			{
				return;
			}
			this.originalParams = new List<CustomFontSettings.CustomParameterSetting>();
			foreach (CustomFontSettings.CustomParameterSetting customParameterSetting in this.customParams)
			{
				this.originalParams.Add(this.ReadCustomParam(customParameterSetting.pType));
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001C598 File Offset: 0x0001A798
		public void Apply()
		{
			this.CheckOriginalParamsCached();
			foreach (CustomFontSettings.CustomParameterSetting customParameterSetting in this.customParams)
			{
				this.SetCustomParam(customParameterSetting.pType, customParameterSetting);
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001C5F8 File Offset: 0x0001A7F8
		public void Restore()
		{
			this.CheckOriginalParamsCached();
			foreach (CustomFontSettings.CustomParameterSetting customParameterSetting in this.originalParams)
			{
				this.SetCustomParam(customParameterSetting.pType, customParameterSetting);
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001C658 File Offset: 0x0001A858
		private void SetCustomParam(CustomFontSettings.CustomParamType par, CustomFontSettings.CustomParameterSetting data)
		{
			if (par == CustomFontSettings.CustomParamType.Color)
			{
				base.GetComponent<TextMeshProUGUI>().color = data.color;
				return;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001C674 File Offset: 0x0001A874
		private CustomFontSettings.CustomParameterSetting ReadCustomParam(CustomFontSettings.CustomParamType param)
		{
			CustomFontSettings.CustomParameterSetting customParameterSetting = new CustomFontSettings.CustomParameterSetting
			{
				pType = param
			};
			if (param == CustomFontSettings.CustomParamType.Color)
			{
				customParameterSetting.color = base.GetComponent<TextMeshProUGUI>().color;
				return customParameterSetting;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x04000271 RID: 625
		public List<CustomFontSettings.CustomParameterSetting> customParams = new List<CustomFontSettings.CustomParameterSetting>();

		// Token: 0x04000272 RID: 626
		private List<CustomFontSettings.CustomParameterSetting> originalParams;

		// Token: 0x020001E7 RID: 487
		[Serializable]
		public enum CustomParamType
		{
			// Token: 0x04000660 RID: 1632
			Color
		}

		// Token: 0x020001E8 RID: 488
		[Serializable]
		public class CustomParameterSetting
		{
			// Token: 0x04000661 RID: 1633
			public CustomFontSettings.CustomParamType pType;

			// Token: 0x04000662 RID: 1634
			public Color color;

			// Token: 0x04000663 RID: 1635
			public float value;
		}
	}
}
