using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
[CreateAssetMenu(fileName = "VoiceClipProcessorSettings", menuName = "GK2/Audio/Voice Clip Processor Settings")]
public class VoiceClipProcessorSettings : ScriptableObject
{
	// Token: 0x04000001 RID: 1
	public const string ResourceName = "VoiceClipProcessorSettings";

	// Token: 0x04000002 RID: 2
	public const string DefaultAssetPath = "Assets/Resources/VoiceClipProcessorSettings.asset";

	// Token: 0x04000003 RID: 3
	public float frameDurationSeconds = 0.02f;

	// Token: 0x04000004 RID: 4
	[Range(0f, 1f)]
	public float speechLevelPercentile = 0.85f;

	// Token: 0x04000005 RID: 5
	[Range(0f, 1f)]
	public float silenceRelativeToSpeech = 0.12f;

	// Token: 0x04000006 RID: 6
	[Range(0f, 1f)]
	[Tooltip("Threshold for continuous trailing silence from the end of the file. Middle pauses are ignored.")]
	public float endSilenceRelativeToSpeech = 0.04f;

	// Token: 0x04000007 RID: 7
	[Tooltip("If detected end silence is shorter than this, it is ignored (set to 0).")]
	public float minEndSilenceSeconds = 0.1f;

	// Token: 0x04000008 RID: 8
	public int minConsecutiveSpeechFrames = 3;

	// Token: 0x04000009 RID: 9
	public int minConsecutiveSilenceFrames = 3;

	// Token: 0x0400000A RID: 10
	public float absoluteSilenceFloor = 0.001f;

	// Token: 0x0400000B RID: 11
	[Range(0f, 1f)]
	public float maxSilenceRatio = 0.9f;
}
