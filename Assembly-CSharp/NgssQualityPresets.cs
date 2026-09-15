using System;

// Token: 0x02000784 RID: 1924
public static class NgssQualityPresets
{
	// Token: 0x060031BD RID: 12733 RVA: 0x000EE87F File Offset: 0x000ECA7F
	public static NgssQualitySettings Get(NgssQualityPreset preset)
	{
		if (preset == NgssQualityPreset.Medium)
		{
			return new NgssQualitySettings(16, 32, true, true, 16, 24);
		}
		if (preset != NgssQualityPreset.Low)
		{
			return new NgssQualitySettings(20, 48, true, true, 15, 32);
		}
		return new NgssQualitySettings(8, 16, false, false, 8, 12);
	}
}
