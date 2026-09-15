using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace LazyBearTechnology
{
	// Token: 0x020000C7 RID: 199
	public class VoiceOverPlayer
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00011616 File Offset: 0x0000F816
		public bool HasVoiceOver
		{
			get
			{
				return this.audioClip != null;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00011624 File Offset: 0x0000F824
		public float ClipLength
		{
			get
			{
				if (!(this.audioClip != null))
				{
					return 0f;
				}
				return VoiceOverPlayer.GetPlayableClipLength(this.audioClip, this.voiceClipData) + VoiceOverSettings.StartPause + VoiceOverSettings.AdditionalClipLength;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00011657 File Offset: 0x0000F857
		public bool IsPlaying
		{
			get
			{
				return VoiceOverPlayer.voiceOverSoundController != null && VoiceOverPlayer.voiceOverSoundController.audioSource != null && VoiceOverPlayer.voiceOverSoundController.audioSource.isPlaying;
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001168C File Offset: 0x0000F88C
		public bool IsPlayingByLoud(VoiceID voiceId, string localKey)
		{
			if (!this.IsCurrentPlayback(voiceId, localKey))
			{
				return false;
			}
			if (!this.IsPlaying)
			{
				return false;
			}
			LoudInterval[] array = ((this.voiceClipData != null) ? this.voiceClipData.loudIntervals : null);
			if (array == null || array.Length == 0)
			{
				return true;
			}
			float time = VoiceOverPlayer.voiceOverSoundController.audioSource.time;
			for (int i = 0; i < array.Length; i++)
			{
				if (time >= array[i].start && time <= array[i].end)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00011714 File Offset: 0x0000F914
		public static bool IsMuted(string id)
		{
			return VoiceOverPlayer.MuteCheck != null && VoiceOverPlayer.MuteCheck(id);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0001172C File Offset: 0x0000F92C
		public virtual void Play(string id, VoiceID voiceId = null)
		{
			if (!VoiceOverSettings.IsEnabled)
			{
				return;
			}
			if (VoiceOverPlayer.IsMuted(id))
			{
				return;
			}
			this.Stop();
			VoiceOverPlayer.EnsureLocationsCache();
			try
			{
				IResourceLocation resourceLocation;
				if (!VoiceOverPlayer.TryGetLocation(VoiceOverPlayer.clipLocations, id, out resourceLocation))
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"Failed to find VoiceOver by id '",
						id,
						"' in Addressables label '",
						VoiceOverSettings.VoiceOversLabel,
						"'."
					}));
					return;
				}
				this.handler = Addressables.LoadAssetAsync<AudioClip>(resourceLocation);
				this.audioClip = this.handler.WaitForCompletion();
			}
			catch (Exception)
			{
				Debug.LogWarning("Failed to load VoiceOver: " + id);
			}
			this.voiceClipData = null;
			try
			{
				IResourceLocation resourceLocation2;
				if (VoiceOverPlayer.TryGetLocation(VoiceOverPlayer.dataLocations, id, out resourceLocation2))
				{
					this.voiceClipDataHandler = Addressables.LoadAssetAsync<VoiceClipData>(resourceLocation2);
					this.voiceClipData = this.voiceClipDataHandler.WaitForCompletion();
				}
			}
			catch (Exception)
			{
			}
			if (this.audioClip != null)
			{
				this.currentLocalKey = id;
				this.currentVoiceId = voiceId;
			}
			this.PlayLoadedClip();
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00011848 File Offset: 0x0000FA48
		public virtual void Stop()
		{
			if (VoiceOverPlayer.voiceOverTailStopper != null)
			{
				VoiceOverPlayer.voiceOverTailStopper.Cancel();
			}
			if (VoiceOverPlayer.voiceOverSoundController != null)
			{
				VoiceOverPlayer.voiceOverSoundController.audioSource.clip = null;
				if (this.handler.IsValid())
				{
					Addressables.Release<AudioClip>(this.handler);
				}
				if (this.voiceClipDataHandler.IsValid())
				{
					Addressables.Release<VoiceClipData>(this.voiceClipDataHandler);
				}
				VoiceOverPlayer.voiceOverSoundController.Stop();
			}
			this.currentLocalKey = null;
			this.currentVoiceId = null;
			this.audioClip = null;
			this.voiceClipData = null;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000118DF File Offset: 0x0000FADF
		private bool IsCurrentPlayback(VoiceID voiceId, string localKey)
		{
			return !string.IsNullOrEmpty(this.currentLocalKey) && !string.IsNullOrEmpty(localKey) && !(this.currentLocalKey != localKey) && this.currentVoiceId == voiceId;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00011914 File Offset: 0x0000FB14
		protected void PlayLoadedClip()
		{
			if (this.audioClip == null)
			{
				return;
			}
			if (VoiceOverPlayer.voiceOverSoundController == null)
			{
				VoiceOverPlayer.voiceOverSoundController = new GameObject().AddComponent<SoundController>();
				AudioSource audioSource = VoiceOverPlayer.voiceOverSoundController.gameObject.AddComponent<AudioSource>();
				audioSource.loop = false;
				audioSource.outputAudioMixerGroup = ((LazySingletonSO<AudioConfig>.Instance != null) ? LazySingletonSO<AudioConfig>.Instance.voiceOverGroup : null);
				VoiceOverPlayer.voiceOverSoundController.audioSource = audioSource;
				VoiceOverPlayer.voiceOverSoundController.spatial = SpatialType.sound1D;
			}
			if (VoiceOverPlayer.voiceOverTailStopper == null)
			{
				VoiceOverPlayer.voiceOverTailStopper = VoiceOverPlayer.voiceOverSoundController.GetComponent<VoiceOverPlayer.VoiceOverTailStopper>();
				if (VoiceOverPlayer.voiceOverTailStopper == null)
				{
					VoiceOverPlayer.voiceOverTailStopper = VoiceOverPlayer.voiceOverSoundController.gameObject.AddComponent<VoiceOverPlayer.VoiceOverTailStopper>();
				}
			}
			VoiceOverPlayer.voiceOverSoundController.audioSource.clip = this.audioClip;
			float startSilence = VoiceOverPlayer.GetStartSilence(this.audioClip, this.voiceClipData);
			float playableClipLength = VoiceOverPlayer.GetPlayableClipLength(this.audioClip, this.voiceClipData);
			float startPause = VoiceOverSettings.StartPause;
			VoiceOverPlayer.voiceOverSoundController.audioSource.time = startSilence;
			VoiceOverPlayer.voiceOverTailStopper.Cancel();
			if (startPause > 0f)
			{
				VoiceOverPlayer.voiceOverTailStopper.SchedulePlayAndStop(VoiceOverPlayer.voiceOverSoundController, VoiceOverPlayer.voiceOverSoundController.audioSource, startPause, playableClipLength);
				return;
			}
			VoiceOverPlayer.voiceOverSoundController.Play();
			if (playableClipLength > 0f)
			{
				VoiceOverPlayer.voiceOverTailStopper.ScheduleStop(VoiceOverPlayer.voiceOverSoundController.audioSource, playableClipLength);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00011A7A File Offset: 0x0000FC7A
		private static float GetStartSilence(AudioClip clip, VoiceClipData data)
		{
			if (clip == null)
			{
				return 0f;
			}
			if (!(data != null))
			{
				return 0f;
			}
			return Mathf.Clamp(data.startSilence, 0f, clip.length);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00011AB0 File Offset: 0x0000FCB0
		private static float GetPlayableClipLength(AudioClip clip, VoiceClipData data)
		{
			if (clip == null)
			{
				return 0f;
			}
			float startSilence = VoiceOverPlayer.GetStartSilence(clip, data);
			float num = ((data != null) ? Mathf.Clamp(data.endSilence, 0f, clip.length) : 0f);
			return Mathf.Max(0.01f, clip.length - startSilence - num);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00011B0F File Offset: 0x0000FD0F
		private static void EnsureLocationsCache()
		{
			if (VoiceOverPlayer.areLocationsBuilt)
			{
				return;
			}
			VoiceOverPlayer.clipLocations.Clear();
			VoiceOverPlayer.dataLocations.Clear();
			VoiceOverPlayer.BuildLocationsForType<AudioClip>(VoiceOverPlayer.clipLocations);
			VoiceOverPlayer.BuildLocationsForType<VoiceClipData>(VoiceOverPlayer.dataLocations);
			VoiceOverPlayer.areLocationsBuilt = true;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00011B48 File Offset: 0x0000FD48
		private static void BuildLocationsForType<T>(Dictionary<string, IResourceLocation> targetMap)
		{
			AsyncOperationHandle<IList<IResourceLocation>> asyncOperationHandle = Addressables.LoadResourceLocationsAsync(VoiceOverSettings.VoiceOversLabel, typeof(T));
			IList<IResourceLocation> list = asyncOperationHandle.WaitForCompletion();
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					IResourceLocation resourceLocation = list[i];
					if (resourceLocation != null && !string.IsNullOrEmpty(resourceLocation.PrimaryKey))
					{
						string text = VoiceOverPlayer.NormalizePathForKey(Path.ChangeExtension(resourceLocation.PrimaryKey, null));
						if (!string.IsNullOrEmpty(text))
						{
							string text2 = VoiceOverPlayer.NormalizePathForKey(Path.GetFileNameWithoutExtension(resourceLocation.PrimaryKey));
							if (!targetMap.ContainsKey(text))
							{
								targetMap.Add(text, resourceLocation);
							}
							if (!string.IsNullOrEmpty(text2) && !targetMap.ContainsKey(text2))
							{
								targetMap.Add(text2, resourceLocation);
							}
						}
					}
				}
			}
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<IList<IResourceLocation>>(asyncOperationHandle);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00011C10 File Offset: 0x0000FE10
		private static bool TryGetLocation(Dictionary<string, IResourceLocation> map, string id, out IResourceLocation location)
		{
			string text = VoiceOverPlayer.NormalizePathForKey(Path.ChangeExtension(id, null));
			if (!string.IsNullOrEmpty(text) && map.TryGetValue(text, out location))
			{
				return true;
			}
			string text2 = VoiceOverPlayer.NormalizePathForKey(Path.GetFileNameWithoutExtension(id));
			if (!string.IsNullOrEmpty(text2) && map.TryGetValue(text2, out location))
			{
				return true;
			}
			location = null;
			return false;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00011C62 File Offset: 0x0000FE62
		private static string NormalizePathForKey(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return string.Empty;
			}
			return path.Replace('\\', '/').TrimStart('/');
		}

		// Token: 0x0400010C RID: 268
		private static SoundController voiceOverSoundController;

		// Token: 0x0400010D RID: 269
		private static VoiceOverPlayer.VoiceOverTailStopper voiceOverTailStopper;

		// Token: 0x0400010E RID: 270
		private static readonly Dictionary<string, IResourceLocation> clipLocations = new Dictionary<string, IResourceLocation>();

		// Token: 0x0400010F RID: 271
		private static readonly Dictionary<string, IResourceLocation> dataLocations = new Dictionary<string, IResourceLocation>();

		// Token: 0x04000110 RID: 272
		private static bool areLocationsBuilt;

		// Token: 0x04000111 RID: 273
		protected AudioClip audioClip;

		// Token: 0x04000112 RID: 274
		protected VoiceClipData voiceClipData;

		// Token: 0x04000113 RID: 275
		protected AsyncOperationHandle<AudioClip> handler;

		// Token: 0x04000114 RID: 276
		protected AsyncOperationHandle<VoiceClipData> voiceClipDataHandler;

		// Token: 0x04000115 RID: 277
		protected VoiceID currentVoiceId;

		// Token: 0x04000116 RID: 278
		protected string currentLocalKey;

		// Token: 0x04000117 RID: 279
		public static Func<string, bool> MuteCheck;

		// Token: 0x020001C2 RID: 450
		private class VoiceOverTailStopper : MonoBehaviour
		{
			// Token: 0x060009EB RID: 2539 RVA: 0x0002DE8C File Offset: 0x0002C08C
			public void SchedulePlayAndStop(SoundController controller, AudioSource source, float playDelay, float playDuration)
			{
				this.Cancel();
				this.stopRoutine = base.StartCoroutine(this.PlayAndStopAfterDelay(controller, source, playDelay, playDuration));
			}

			// Token: 0x060009EC RID: 2540 RVA: 0x0002DEAB File Offset: 0x0002C0AB
			public void ScheduleStop(AudioSource source, float delay)
			{
				this.Cancel();
				this.stopRoutine = base.StartCoroutine(this.StopAfterDelay(source, delay));
			}

			// Token: 0x060009ED RID: 2541 RVA: 0x0002DEC7 File Offset: 0x0002C0C7
			public void Cancel()
			{
				if (this.stopRoutine == null)
				{
					return;
				}
				base.StopCoroutine(this.stopRoutine);
				this.stopRoutine = null;
			}

			// Token: 0x060009EE RID: 2542 RVA: 0x0002DEE5 File Offset: 0x0002C0E5
			private IEnumerator PlayAndStopAfterDelay(SoundController controller, AudioSource source, float playDelay, float playDuration)
			{
				yield return new WaitForSeconds(playDelay);
				if (controller != null && source != null && source.clip != null)
				{
					controller.Play();
				}
				if (playDuration > 0f)
				{
					yield return new WaitForSeconds(playDuration);
					if (source != null && source.isPlaying)
					{
						source.Stop();
					}
				}
				this.stopRoutine = null;
				yield break;
			}

			// Token: 0x060009EF RID: 2543 RVA: 0x0002DF11 File Offset: 0x0002C111
			private IEnumerator StopAfterDelay(AudioSource source, float delay)
			{
				yield return new WaitForSeconds(delay);
				if (source != null && source.isPlaying)
				{
					source.Stop();
				}
				this.stopRoutine = null;
				yield break;
			}

			// Token: 0x0400060D RID: 1549
			private Coroutine stopRoutine;
		}
	}
}
