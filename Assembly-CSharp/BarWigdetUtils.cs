using System;
using UnityEngine;

// Token: 0x020007DB RID: 2011
public static class BarWigdetUtils
{
	// Token: 0x060033E5 RID: 13285 RVA: 0x000FA9AE File Offset: 0x000F8BAE
	public static int ClampSliderValueToViewableState(int fillRectMaxWidth, int currentSliderValue, int maxSliderValue)
	{
		if (currentSliderValue == 0)
		{
			return 0;
		}
		if ((float)currentSliderValue / (float)maxSliderValue * (float)fillRectMaxWidth >= 1f)
		{
			return currentSliderValue;
		}
		return Mathf.RoundToInt((float)maxSliderValue / (float)fillRectMaxWidth);
	}
}
