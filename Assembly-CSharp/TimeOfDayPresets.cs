using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
[CreateAssetMenu(fileName = "TimeOfDayPresets", menuName = "GK2/Light/TimeOfDayPresets")]
public class TimeOfDayPresets : ScriptableObject
{
	// Token: 0x04003A6E RID: 14958
	public bool indoorPreset;

	// Token: 0x04003A6F RID: 14959
	public bool applySfxFromOutdoor;

	// Token: 0x04003A70 RID: 14960
	public List<TimeOfDayPresets.TimeAndPreset> presets = new List<TimeOfDayPresets.TimeAndPreset>();

	// Token: 0x04003A71 RID: 14961
	public SoundEnvironmentConfig soundEnvironmentConfig;

	// Token: 0x02000AC4 RID: 2756
	[Serializable]
	public struct TimeAndPreset
	{
		// Token: 0x04003A72 RID: 14962
		[Range(0f, 1f)]
		public float time;

		// Token: 0x04003A73 RID: 14963
		public LightEnvironmentPreset preset;
	}
}
