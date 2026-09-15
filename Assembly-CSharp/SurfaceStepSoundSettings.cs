using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000135 RID: 309
[CreateAssetMenu(fileName = "SurfaceStepSoundSettings", menuName = "GK2/SurfaceStepSoundSettings", order = 0)]
public class SurfaceStepSoundSettings : LazySingletonSO<SurfaceStepSoundSettings>
{
	// Token: 0x06000760 RID: 1888 RVA: 0x00023088 File Offset: 0x00021288
	public string GetStepSoundIdForSurfaceType(SurfaceType surfaceType)
	{
		SurfaceStepSoundSettings.StepSoundPair stepSoundPair = this.soundPairs.Find((SurfaceStepSoundSettings.StepSoundPair p) => p.surfaceType == surfaceType);
		if (stepSoundPair == null)
		{
			Debug.LogError(string.Format("Can't find sound for surface type:[{0}]", surfaceType));
			return string.Empty;
		}
		return stepSoundPair.soundId;
	}

	// Token: 0x04000948 RID: 2376
	[Range(0f, 100f)]
	public int delayChance = 50;

	// Token: 0x04000949 RID: 2377
	public float maxDelayTime = 0.04f;

	// Token: 0x0400094A RID: 2378
	public float minDelayTime = 0.01f;

	// Token: 0x0400094B RID: 2379
	[SerializeField]
	[Space]
	private List<SurfaceStepSoundSettings.StepSoundPair> soundPairs;

	// Token: 0x02000136 RID: 310
	[Serializable]
	private class StepSoundPair
	{
		// Token: 0x0400094C RID: 2380
		public SurfaceType surfaceType;

		// Token: 0x0400094D RID: 2381
		public string soundId;
	}
}
