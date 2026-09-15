using System;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200015E RID: 350
	public static class LazyTextMeshProUtils
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x00026C0C File Offset: 0x00024E0C
		public static void FitToAspectOrOverflowAndGetScrollHeight(this TextMeshProUGUI label, float aspectRatio, float minHeight, float maxHeight, out float heightForScroll, float step = 5f)
		{
			label.textWrappingMode = TextWrappingModes.Normal;
			label.overflowMode = TextOverflowModes.Overflow;
			label.autoSizeTextContainer = false;
			float num = minHeight * aspectRatio;
			float num2 = minHeight;
			heightForScroll = maxHeight;
			float num3 = minHeight;
			float num4 = maxHeight;
			bool flag = false;
			while (num4 - num3 > step)
			{
				float num5 = (num3 + num4) / 2f;
				float num6 = num5 * aspectRatio;
				if (label.GetPreferredValues(label.text, num6, float.PositiveInfinity).y <= num5)
				{
					num4 = num5;
					num = num6;
					num2 = num5;
					heightForScroll = num5;
					flag = true;
				}
				else
				{
					num3 = num5;
				}
			}
			if (!flag)
			{
				num = maxHeight * aspectRatio;
				num2 = label.GetPreferredValues(label.text, num, float.PositiveInfinity).y;
				heightForScroll = maxHeight;
			}
			label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
		}
	}
}
