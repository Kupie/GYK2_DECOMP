using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000D7 RID: 215
	[CreateAssetMenu(fileName = "VoiceOverSettings", menuName = "Lazy/VoiceOverSettings", order = 1)]
	public class VoiceOverSettings : LazySingletonSO<VoiceOverSettings>
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000374 RID: 884 RVA: 0x000124A0 File Offset: 0x000106A0
		// (set) Token: 0x06000375 RID: 885 RVA: 0x000124A7 File Offset: 0x000106A7
		public static bool IsEnabled { get; set; } = true;

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000376 RID: 886 RVA: 0x000124AF File Offset: 0x000106AF
		// (set) Token: 0x06000377 RID: 887 RVA: 0x000124B6 File Offset: 0x000106B6
		public static string LanguageId { get; set; } = "en";

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000378 RID: 888 RVA: 0x000124BE File Offset: 0x000106BE
		public static float AdditionalClipLength
		{
			get
			{
				if (!(LazySingletonSO<VoiceOverSettings>.Instance != null))
				{
					return 0.2f;
				}
				return Mathf.Max(0f, LazySingletonSO<VoiceOverSettings>.Instance.additionalClipLength);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000379 RID: 889 RVA: 0x000124E7 File Offset: 0x000106E7
		public static float StartPause
		{
			get
			{
				if (!(LazySingletonSO<VoiceOverSettings>.Instance != null))
				{
					return 0.2f;
				}
				return Mathf.Max(0f, LazySingletonSO<VoiceOverSettings>.Instance.startPause);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00012510 File Offset: 0x00010710
		public static string VoiceOversLabel
		{
			get
			{
				if (!(LazySingletonSO<VoiceOverSettings>.Instance != null) || string.IsNullOrWhiteSpace(LazySingletonSO<VoiceOverSettings>.Instance.voiceOversLabel))
				{
					return "Voiceovers";
				}
				return LazySingletonSO<VoiceOverSettings>.Instance.voiceOversLabel;
			}
		}

		// Token: 0x04000163 RID: 355
		private const float DEFAULT_TIME = 0.2f;

		// Token: 0x04000164 RID: 356
		private const string DEFAULT_VOICE_OVERS_LABEL = "Voiceovers";

		// Token: 0x04000165 RID: 357
		public float additionalClipLength = 0.2f;

		// Token: 0x04000166 RID: 358
		public float startPause = 0.2f;

		// Token: 0x04000167 RID: 359
		public string voiceOversLabel = "Voiceovers";
	}
}
