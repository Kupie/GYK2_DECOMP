using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
[CreateAssetMenu(fileName = "SoundZonePreset", menuName = "GK2/SoundZonePreset", order = 1)]
public class SoundZonePreset : ScriptableObject
{
	// Token: 0x06000757 RID: 1879 RVA: 0x00023045 File Offset: 0x00021245
	public float Evaluate(float volumeCoef)
	{
		if (this.type == SoundZoneValuesType.Curve)
		{
			return this.curve.Evaluate(volumeCoef);
		}
		return volumeCoef;
	}

	// Token: 0x04000937 RID: 2359
	public SoundZoneValuesType type;

	// Token: 0x04000938 RID: 2360
	public float minDistance;

	// Token: 0x04000939 RID: 2361
	public float maxDistance;

	// Token: 0x0400093A RID: 2362
	public AnimationCurve curve;
}
