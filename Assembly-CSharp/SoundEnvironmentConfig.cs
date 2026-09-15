using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AC0 RID: 2752
[CreateAssetMenu(fileName = "SoundEnvironmentConfig", menuName = "GK2/Sound/SoundEnvironmentConfig")]
public class SoundEnvironmentConfig : ScriptableObject
{
	// Token: 0x06004A80 RID: 19072 RVA: 0x0015FC42 File Offset: 0x0015DE42
	public float GetCrossfadeDuration01(float secondsPerCycle)
	{
		if (secondsPerCycle <= 0f)
		{
			return 0f;
		}
		return this.crossfadeSeconds / secondsPerCycle;
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x0015FC5C File Offset: 0x0015DE5C
	public static void FindPair(List<SoundEnvironmentConfig.SoundAndTime> sounds, float timeOfDay, float crossfadeDuration, out string id1, out string id2, out float lerp)
	{
		id1 = null;
		id2 = null;
		lerp = 0f;
		if (sounds == null || sounds.Count == 0)
		{
			return;
		}
		for (int i = 0; i < sounds.Count; i++)
		{
			SoundEnvironmentConfig.SoundAndTime soundAndTime = sounds[i];
			if (soundAndTime.time == timeOfDay)
			{
				string soundId;
				id2 = (soundId = soundAndTime.soundId);
				id1 = soundId;
				lerp = 0f;
				return;
			}
			if (timeOfDay > soundAndTime.time)
			{
				id1 = soundAndTime.soundId;
				if (i + 1 >= sounds.Count)
				{
					Debug.LogError("Error picking a sound environment entry. Probably, last sound time is less than 1.0");
					id2 = id1;
					lerp = 0f;
					return;
				}
				SoundEnvironmentConfig.SoundAndTime soundAndTime2 = sounds[i + 1];
				if (timeOfDay <= soundAndTime2.time)
				{
					id2 = soundAndTime2.soundId;
					float num = soundAndTime2.time - soundAndTime.time;
					if (num <= 0f)
					{
						lerp = 0f;
						return;
					}
					float num2 = Mathf.Min(crossfadeDuration, num);
					if (num2 <= 0f)
					{
						lerp = 0f;
						return;
					}
					float num3 = soundAndTime2.time - num2;
					lerp = Mathf.Clamp01((timeOfDay - num3) / num2);
					return;
				}
			}
		}
	}

	// Token: 0x06004A82 RID: 19074 RVA: 0x0015FD70 File Offset: 0x0015DF70
	public static string FindActiveSegment(List<SoundEnvironmentConfig.SoundAndTime> sounds, float timeOfDay)
	{
		if (sounds == null || sounds.Count == 0)
		{
			return null;
		}
		string text = null;
		int num = 0;
		while (num < sounds.Count && sounds[num].time <= timeOfDay)
		{
			text = sounds[num].soundId;
			num++;
		}
		return text;
	}

	// Token: 0x04003A5A RID: 14938
	public List<SoundEnvironmentConfig.SoundAndTime> sounds = new List<SoundEnvironmentConfig.SoundAndTime>();

	// Token: 0x04003A5B RID: 14939
	[Tooltip("Duration (in real-time seconds) of the volume crossfade at the end of each segment, right before switching to the next sound. Outside this window only the current sound is audible. 0 = no crossfade window; the previous sound is released with the standard fade-out at the boundary.")]
	[Min(0f)]
	public float crossfadeSeconds = 5f;

	// Token: 0x02000AC1 RID: 2753
	[Serializable]
	public struct SoundAndTime
	{
		// Token: 0x04003A5C RID: 14940
		public string soundId;

		// Token: 0x04003A5D RID: 14941
		[Range(0f, 1f)]
		public float time;
	}
}
