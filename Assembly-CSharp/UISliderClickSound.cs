using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000840 RID: 2112
public static class UISliderClickSound
{
	// Token: 0x060035ED RID: 13805 RVA: 0x0010303C File Offset: 0x0010123C
	public static void Play()
	{
		float unscaledTime = Time.unscaledTime;
		if (unscaledTime - UISliderClickSound.lastPlayTime < 0.045f)
		{
			return;
		}
		UISliderClickSound.lastPlayTime = unscaledTime;
		LazyAudio.PlayAndForget("gui_hover");
	}

	// Token: 0x04002B34 RID: 11060
	private const float MinInterval = 0.045f;

	// Token: 0x04002B35 RID: 11061
	private static float lastPlayTime = -1f;
}
