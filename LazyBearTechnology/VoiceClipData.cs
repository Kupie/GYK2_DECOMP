using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000D6 RID: 214
	[CreateAssetMenu(fileName = "VoiceClipData", menuName = "Lazy/VoiceClipData", order = 1)]
	public class VoiceClipData : ScriptableObject
	{
		// Token: 0x04000160 RID: 352
		public float startSilence;

		// Token: 0x04000161 RID: 353
		public float endSilence;

		// Token: 0x04000162 RID: 354
		public LoudInterval[] loudIntervals = Array.Empty<LoudInterval>();
	}
}
