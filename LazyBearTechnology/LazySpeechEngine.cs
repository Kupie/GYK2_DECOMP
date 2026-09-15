using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace LazyBearTechnology
{
	// Token: 0x02000172 RID: 370
	[ExecuteInEditMode]
	public class LazySpeechEngine : MonoBehaviour
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00028838 File Offset: 0x00026A38
		public List<LazySpeechEngine.VoiceData> Develop_Voices
		{
			get
			{
				return this.voices;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00028840 File Offset: 0x00026A40
		public static LazySpeechEngine Instance
		{
			get
			{
				if (LazySpeechEngine.instance == null)
				{
					LazySpeechEngine.instance = global::UnityEngine.Object.FindObjectOfType<LazySpeechEngine>();
				}
				return LazySpeechEngine.instance;
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00028860 File Offset: 0x00026A60
		public void Play(VoiceID voiceId, float remainingPlayingTime = -1f)
		{
			if (voiceId == VoiceID.None)
			{
				return;
			}
			LazySpeechEngine.VoiceData voiceData = this.GetVoiceData(voiceId);
			bool flag = false;
			bool flag2 = true;
			float speechRemainingTime = this.GetSpeechRemainingTime(voiceId);
			float speechAveragePlayingTime = this.GetSpeechAveragePlayingTime(voiceId);
			if (voiceData.hasShortVoice && remainingPlayingTime > -1f)
			{
				flag = !this.IsSpeechPlaying(voiceId) && (this.IsSpeechPlaying(voiceData.shortVoiceId) || speechAveragePlayingTime > remainingPlayingTime - speechRemainingTime);
				if (this.IsSpeechPlaying(voiceData.shortVoiceId))
				{
					flag2 = this.GetSpeechAveragePlayingTime(voiceData.shortVoiceId) + this.GetSpeechRemainingTime(voiceData.shortVoiceId) <= remainingPlayingTime;
				}
				else
				{
					flag2 = !flag && speechAveragePlayingTime + speechRemainingTime <= remainingPlayingTime;
				}
			}
			if (flag)
			{
				voiceId = voiceData.shortVoiceId;
				voiceData = this.GetVoiceData(voiceId);
			}
			bool flag3 = false;
			for (int i = 0; i < this.activeSpeechSamples.Count; i++)
			{
				LazySpeechEngine.SpeechSample speechSample = this.activeSpeechSamples[i];
				if (speechSample.voiceData.voiceId == voiceId)
				{
					if (flag2)
					{
						speechSample.runAgain = true;
					}
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				LazySpeechEngine.SpeechSample speechSample2 = new LazySpeechEngine.SpeechSample
				{
					voiceData = voiceData
				};
				this.PlaySpeechSample(speechSample2);
				this.activeSpeechSamples.Add(speechSample2);
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00028998 File Offset: 0x00026B98
		public void Stop(VoiceID voiceId)
		{
			for (int i = 0; i < this.activeSpeechSamples.Count; i++)
			{
				if (this.activeSpeechSamples[i].voiceData.voiceId == voiceId)
				{
					this.activeSpeechSamples.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000289E8 File Offset: 0x00026BE8
		public bool IsSpeechPlaying(VoiceID voiceId)
		{
			for (int i = 0; i < this.activeSpeechSamples.Count; i++)
			{
				if (this.activeSpeechSamples[i].voiceData.voiceId == voiceId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00028A2C File Offset: 0x00026C2C
		public float GetSpeechRemainingTime(VoiceID voiceId)
		{
			float num = 0f;
			for (int i = 0; i < this.activeSpeechSamples.Count; i++)
			{
				LazySpeechEngine.SpeechSample speechSample = this.activeSpeechSamples[i];
				if (speechSample.voiceData.voiceId == voiceId)
				{
					num = speechSample.TriggerInterval - (Time.realtimeSinceStartup - speechSample.lastSampleStartTime);
					break;
				}
			}
			return num;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00028A8C File Offset: 0x00026C8C
		public float GetSpeechAveragePlayingTime(VoiceID voiceId)
		{
			for (int i = 0; i < this.voices.Count; i++)
			{
				if (this.voices[i].voiceId == voiceId && !string.IsNullOrEmpty(this.voices[i].soundId))
				{
					return LazySingletonSO<AudioConfig>.Instance.Get(this.voices[i].soundId).AveragePlayingTime;
				}
			}
			return 0f;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00028B08 File Offset: 0x00026D08
		public LazySpeechEngine.VoiceData GetVoiceData(VoiceID voiceId)
		{
			return this.voices.Find((LazySpeechEngine.VoiceData x) => x.voiceId == voiceId);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00028B3C File Offset: 0x00026D3C
		private void PlaySpeechSample(LazySpeechEngine.SpeechSample speechSample)
		{
			SoundHandler soundHandler = LazyAudio.Play(speechSample.voiceData.soundId);
			if (soundHandler == null)
			{
				return;
			}
			soundHandler.SetVolume(speechSample.voiceData.volume);
			soundHandler.SetPitch(global::UnityEngine.Random.Range(speechSample.voiceData.pitch - speechSample.voiceData.pitchVariation, speechSample.voiceData.pitch + speechSample.voiceData.pitchVariation));
			soundHandler.SetFadeInFadeOut(speechSample.voiceData.fadeInTime, speechSample.voiceData.fadeOutTime);
			AudioMixerGroup audioMixerGroup = ((LazySingletonSO<AudioConfig>.Instance != null) ? LazySingletonSO<AudioConfig>.Instance.mumbleGroup : null);
			if (audioMixerGroup != null)
			{
				soundHandler.SetMixerGroup(audioMixerGroup);
			}
			speechSample.RememberPlayedDuration(soundHandler);
			speechSample.lastSampleStartTime = Time.realtimeSinceStartup;
			speechSample.AddNewSoundHandler(soundHandler);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00028C0C File Offset: 0x00026E0C
		private void Update()
		{
			for (int i = 0; i < this.activeSpeechSamples.Count; i++)
			{
				LazySpeechEngine.SpeechSample speechSample = this.activeSpeechSamples[i];
				if (Time.realtimeSinceStartup - speechSample.lastSampleStartTime > speechSample.TriggerInterval || !speechSample.IsActive)
				{
					if (speechSample.runAgain)
					{
						speechSample.runAgain = false;
						this.PlaySpeechSample(speechSample);
					}
					else if (!speechSample.IsActive)
					{
						this.activeSpeechSamples.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x040004FA RID: 1274
		[SerializeField]
		private List<LazySpeechEngine.VoiceData> voices;

		// Token: 0x040004FB RID: 1275
		private List<LazySpeechEngine.SpeechSample> activeSpeechSamples = new List<LazySpeechEngine.SpeechSample>();

		// Token: 0x040004FC RID: 1276
		private static LazySpeechEngine instance;

		// Token: 0x040004FD RID: 1277
		private bool editorOnly_endlessPlay;

		// Token: 0x02000204 RID: 516
		private class SpeechSample
		{
			// Token: 0x1700015C RID: 348
			// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0002EEFF File Offset: 0x0002D0FF
			public float MinTime
			{
				get
				{
					return this.voiceData.sampleTime / this.voiceData.pitch;
				}
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0002EF18 File Offset: 0x0002D118
			public float TriggerInterval
			{
				get
				{
					bool flag = this.voiceData.playbackMode == LazySpeechEngine.VoicePlaybackMode.SampleLength && this.lastPlayedDuration > 0f;
					float num = (flag ? this.lastPlayedDuration : this.MinTime);
					float num2 = (flag ? (num + this.voiceData.sampleDelay) : (num - this.voiceData.sampleDelay));
					return Mathf.Max(0.02f, num2);
				}
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0002EF80 File Offset: 0x0002D180
			public bool IsActive
			{
				get
				{
					for (int i = 0; i < this.soundHandlers.Count; i++)
					{
						if (this.soundHandlers[i].IsActive)
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x06000A89 RID: 2697 RVA: 0x0002EFBC File Offset: 0x0002D1BC
			public void RememberPlayedDuration(SoundHandler soundHandler)
			{
				this.lastPlayedDuration = 0f;
				if (soundHandler == null)
				{
					return;
				}
				float clipLength = soundHandler.GetClipLength();
				if (clipLength <= 0f)
				{
					return;
				}
				float pitchValue = soundHandler.GetPitchValue();
				this.lastPlayedDuration = ((pitchValue > 0.01f) ? (clipLength / pitchValue) : clipLength);
			}

			// Token: 0x06000A8A RID: 2698 RVA: 0x0002F003 File Offset: 0x0002D203
			public void AddNewSoundHandler(SoundHandler newSoundHandler)
			{
				this.soundHandlers.Add(newSoundHandler);
			}

			// Token: 0x040006D6 RID: 1750
			public bool runAgain;

			// Token: 0x040006D7 RID: 1751
			public float lastSampleStartTime;

			// Token: 0x040006D8 RID: 1752
			public float lastPlayedDuration;

			// Token: 0x040006D9 RID: 1753
			public LazySpeechEngine.VoiceData voiceData;

			// Token: 0x040006DA RID: 1754
			private readonly List<SoundHandler> soundHandlers = new List<SoundHandler>();

			// Token: 0x040006DB RID: 1755
			private const float MIN_TRIGGER_INTERVAL = 0.02f;
		}

		// Token: 0x02000205 RID: 517
		public enum VoicePlaybackMode
		{
			// Token: 0x040006DD RID: 1757
			EqualLength,
			// Token: 0x040006DE RID: 1758
			SampleLength
		}

		// Token: 0x02000206 RID: 518
		[Serializable]
		public class VoiceData
		{
			// Token: 0x06000A8C RID: 2700 RVA: 0x0002F024 File Offset: 0x0002D224
			public void PlayOnce()
			{
				LazySpeechEngine.Instance.Play(this.voiceId, -1f);
			}

			// Token: 0x06000A8D RID: 2701 RVA: 0x0002F03B File Offset: 0x0002D23B
			public void PlaySeries()
			{
				LazySpeechEngine.Instance.Play(this.voiceId, -1f);
				LazySpeechEngine.Instance.editorOnly_endlessPlay = true;
			}

			// Token: 0x06000A8E RID: 2702 RVA: 0x0002F05D File Offset: 0x0002D25D
			public void Stop()
			{
				LazySpeechEngine.Instance.Stop(this.voiceId);
				LazySpeechEngine.Instance.editorOnly_endlessPlay = false;
			}

			// Token: 0x06000A8F RID: 2703 RVA: 0x0002F07C File Offset: 0x0002D27C
			public void LoadParametersFromJSON(string filename)
			{
				if (!File.Exists(filename))
				{
					return;
				}
				LazySpeechEngine.VoiceData voiceData = JsonUtility.FromJson<LazySpeechEngine.VoiceData>(File.ReadAllText(filename));
				this.pitch = voiceData.pitch;
				this.fadeInTime = voiceData.fadeInTime;
				this.fadeOutTime = voiceData.fadeOutTime;
				this.sampleTime = voiceData.sampleTime;
				this.pitchVariation = voiceData.pitchVariation;
			}

			// Token: 0x040006DF RID: 1759
			public VoiceID voiceId;

			// Token: 0x040006E0 RID: 1760
			public string soundId;

			// Token: 0x040006E1 RID: 1761
			public LazySpeechEngine.VoicePlaybackMode playbackMode;

			// Token: 0x040006E2 RID: 1762
			[Range(0.05f, 1f)]
			public float sampleTime = 0.05f;

			// Token: 0x040006E3 RID: 1763
			[Range(-1f, 1f)]
			public float sampleDelay;

			// Token: 0x040006E4 RID: 1764
			[Range(0.5f, 1.5f)]
			public float pitch = 1f;

			// Token: 0x040006E5 RID: 1765
			[Range(0f, 0.5f)]
			public float pitchVariation;

			// Token: 0x040006E6 RID: 1766
			[Range(0f, 1f)]
			public float volume = 1f;

			// Token: 0x040006E7 RID: 1767
			[Range(0f, 1f)]
			public float fadeInTime;

			// Token: 0x040006E8 RID: 1768
			[Range(0f, 1f)]
			public float fadeOutTime;

			// Token: 0x040006E9 RID: 1769
			[Space]
			public bool hasShortVoice;

			// Token: 0x040006EA RID: 1770
			public VoiceID shortVoiceId;

			// Token: 0x040006EB RID: 1771
			[Space]
			public bool useOneSampleOncePerCue;
		}
	}
}
