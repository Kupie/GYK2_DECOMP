using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000142 RID: 322
	[CreateAssetMenu(fileName = "SpeechBubbleSettings", menuName = "Lazy/SpeechBubbleSettings", order = 1)]
	public class SpeechBubbleSettings : ScriptableObject
	{
		// Token: 0x06000679 RID: 1657 RVA: 0x00021530 File Offset: 0x0001F730
		public float CalculateBubbleShowingTime(string displayingText)
		{
			float num = this.showTime;
			float num2 = (float)displayingText.Length * this.oneSymbolTime;
			if (LLBase.IsEastern())
			{
				num2 *= this.easternSymbolTimeCoef;
			}
			return num + num2;
		}

		// Token: 0x040003C0 RID: 960
		public float preferredWidth = 150f;

		// Token: 0x040003C1 RID: 961
		public float showTime = 2.5f;

		// Token: 0x040003C2 RID: 962
		public float oneSymbolTime = 0.08449999f;

		// Token: 0x040003C3 RID: 963
		public float easternSymbolTimeCoef = 4f;

		// Token: 0x040003C4 RID: 964
		public float letterAnimAppearTime = 0.02f;

		// Token: 0x040003C5 RID: 965
		public float fadeTime = 0.35f;

		// Token: 0x040003C6 RID: 966
		public float voiceAdditionalAverageTime = 0.6f;
	}
}
