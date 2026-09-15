using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace LazyBearTechnology
{
	// Token: 0x020000C4 RID: 196
	public class LazyAudio : MonoBehaviour
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000EFEB File Offset: 0x0000D1EB
		public static bool IsInitialized
		{
			get
			{
				return LazyAudio.isInitialized;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000EFF2 File Offset: 0x0000D1F2
		public static Transform Microphone
		{
			get
			{
				return LazyAudio.instance.microphone;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000EFFE File Offset: 0x0000D1FE
		public static AudioMixer AudioMixer
		{
			get
			{
				LazyAudio lazyAudio = LazyAudio.Instance;
				if (lazyAudio == null)
				{
					return null;
				}
				AudioConfig audioConfig = lazyAudio.audioConfig;
				if (audioConfig == null)
				{
					return null;
				}
				return audioConfig.audioMixer;
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F01B File Offset: 0x0000D21B
		public static bool TryGetAudioMixer(out AudioMixer audioMixer)
		{
			audioMixer = LazyAudio.AudioMixer;
			return audioMixer != null;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000F02C File Offset: 0x0000D22C
		private static LazyAudio Instance
		{
			get
			{
				if (LazyAudio.instance == null)
				{
					LazyAudio.instance = global::UnityEngine.Object.FindAnyObjectByType<LazyAudio>(FindObjectsInactive.Exclude);
					if (LazyAudio.instance == null)
					{
						Debug.LogError("LazyAudio.Instance error: couldn't find a LazyAudio object.");
						return null;
					}
					LazyAudio.instance.BindSharedAudioConfig();
					if (LazyAudio.instance.voiceOverPlayer == null)
					{
						Debug.Log("#aud# VoiceOverPlayer creating");
						LazyAudio.instance.voiceOverPlayer = ((LazyAudio.VoiceOverPlayerFactory != null) ? LazyAudio.VoiceOverPlayerFactory() : new VoiceOverPlayer());
					}
					LazyAudio.instance.InitializeAudio3dPresets();
					LazyAudio.isInitialized = true;
				}
				return LazyAudio.instance;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000F0C1 File Offset: 0x0000D2C1
		public static VoiceOverPlayer VoiceOverPlayer
		{
			get
			{
				return LazyAudio.instance.voiceOverPlayer;
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000F0CD File Offset: 0x0000D2CD
		public void SetConfigurationReference(AudioConfig config)
		{
			this.audioConfig = config;
			if (config != null)
			{
				LazySingletonSO<AudioConfig>.SetReference(config);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
		public static void BindAudioConfig(AudioConfig config)
		{
			if (config == null)
			{
				return;
			}
			LazySingletonSO<AudioConfig>.SetReference(config);
			if (LazyAudio.instance != null)
			{
				LazyAudio.instance.audioConfig = config;
				return;
			}
			LazyAudio lazyAudio = global::UnityEngine.Object.FindAnyObjectByType<LazyAudio>(FindObjectsInactive.Include);
			if (lazyAudio != null)
			{
				lazyAudio.audioConfig = config;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F138 File Offset: 0x0000D338
		private void BindSharedAudioConfig()
		{
			AudioConfig audioConfig = LazySingletonSO<AudioConfig>.Instance;
			if (audioConfig != null)
			{
				this.audioConfig = audioConfig;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000F15B File Offset: 0x0000D35B
		private void Awake()
		{
			if (Application.isPlaying && LazyAudio.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (Application.isPlaying)
			{
				this.Initialize();
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000F18A File Offset: 0x0000D38A
		private void Initialize()
		{
			LazyAudio lazyAudio = LazyAudio.Instance;
			this.InitializeAudio3dPresets();
			this.InitializeStartPool();
			this.InitializePlaylists();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000F1A4 File Offset: 0x0000D3A4
		private void InitializePlaylists()
		{
			this.BindSharedAudioConfig();
			if (this.audioConfig == null)
			{
				Debug.LogError("Audio System Error: AudioConfig for AudioSystem not found.");
				return;
			}
			for (int i = 0; i < this.audioConfig.playlists.Count; i++)
			{
				PlaylistController playlistController = new GameObject().AddComponent<PlaylistController>();
				playlistController.Initialize(this.audioConfig.playlists[i]);
				playlistController.transform.SetParent(base.gameObject.transform);
				this.playlistControllers.Add(playlistController);
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000F230 File Offset: 0x0000D430
		private void InitializeStartPool()
		{
			for (int i = 0; i < this.initialPoolSize; i++)
			{
				SoundController soundController = this.CreateSoundController();
				soundController.gameObject.SetActive(false);
				this.inactiveSources.Push(soundController);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000F270 File Offset: 0x0000D470
		private void InitializeAudio3dPresets()
		{
			this.audio3DSettingsPresets.Clear();
			AudioSettings3DPreset[] componentsInChildren = base.GetComponentsInChildren<AudioSettings3DPreset>(true);
			if (componentsInChildren.Length == 0)
			{
				Debug.LogError(string.Format("Audio System Error: Cannot find any audio settings 3d presets [{0}]", typeof(AudioSettings3DPreset)));
			}
			bool flag = false;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.audio3DSettingsPresets.Add(componentsInChildren[i].type, componentsInChildren[i].GetComponent<AudioSource>());
				if (!flag && componentsInChildren[i].type == AudioSettings3DType.Default)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				Debug.LogError(string.Format("Audio System Error: Cannot find default audio settings 3d preset [{0}]", typeof(AudioSettings3DPreset)));
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000F310 File Offset: 0x0000D510
		private SoundController CreateSoundController()
		{
			SoundController soundController = new GameObject().AddComponent<SoundController>();
			AudioSource audioSource = soundController.gameObject.AddComponent<AudioSource>();
			soundController.audioSource = audioSource;
			soundController.transform.SetParent(base.gameObject.transform);
			return soundController;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000F350 File Offset: 0x0000D550
		private void Update()
		{
			for (int i = 0; i < this.activeSources.Count; i++)
			{
				SoundController soundController = this.activeSources[i];
				if (soundController.IsAlive)
				{
					soundController.UpdateVolumeWhilePlaying();
					soundController.UpdatePosition(this.microphone);
				}
				else
				{
					soundController.gameObject.SetActive(false);
					soundController.Reset();
					this.activeSources.RemoveAt(i);
					i--;
					this.inactiveSources.Push(soundController);
				}
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000F3CC File Offset: 0x0000D5CC
		public static void PlayAndForget(string id)
		{
			SoundController soundController = LazyAudio.Instance.PlaySound(id, true, true);
			if (soundController == null)
			{
				return;
			}
			soundController.audioSource.spatialBlend = 0f;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000F401 File Offset: 0x0000D601
		public static SoundHandler Play(string id)
		{
			return LazyAudio.Play(id, true);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000F40C File Offset: 0x0000D60C
		public static SoundHandler Play(string id, bool checkDelay)
		{
			SoundController soundController = LazyAudio.Instance.PlaySound(id, false, checkDelay);
			if (soundController == null)
			{
				return null;
			}
			soundController.audioSource.spatialBlend = 0f;
			return soundController.soundHandler;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000F448 File Offset: 0x0000D648
		public static SoundHandler PlayAtPos(string id, Vector3 position)
		{
			SoundController soundController = LazyAudio.Instance.PlaySound(id, false, true);
			if (soundController == null)
			{
				return null;
			}
			soundController.transform.position = position;
			soundController.audioSource.spatialBlend = 1f;
			return soundController.soundHandler;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000F490 File Offset: 0x0000D690
		public static SoundHandler PlayAtGameObject(string id, Transform obj, SpatialType spatial = SpatialType.sound1D, bool checkDelay = true)
		{
			SoundController soundController = LazyAudio.Instance.PlaySound(id, false, checkDelay);
			if (soundController == null)
			{
				return null;
			}
			soundController.target = obj;
			soundController.spatial = spatial;
			soundController.UpdatePosition(LazyAudio.Instance.microphone);
			return soundController.soundHandler;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000F4DC File Offset: 0x0000D6DC
		public static void Stop(string id)
		{
			if (LazyAudio.Instance == null)
			{
				return;
			}
			foreach (SoundController soundController in LazyAudio.Instance.activeSources)
			{
				if (soundController.id == id)
				{
					soundController.Stop();
				}
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000F550 File Offset: 0x0000D750
		public static void Stop(SoundHandler handler)
		{
			handler.Stop();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000F559 File Offset: 0x0000D759
		public static void Pause(SoundHandler handler)
		{
			handler.Pause();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000F562 File Offset: 0x0000D762
		public static void UnPause(SoundHandler handler)
		{
			handler.UnPause();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000F56C File Offset: 0x0000D76C
		public static void StopAll()
		{
			foreach (SoundController soundController in LazyAudio.Instance.activeSources)
			{
				soundController.Stop();
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		public static PlaylistController PlayPlaylist(string playlistId)
		{
			List<PlaylistController> list = LazyAudio.Instance.playlistControllers;
			PlaylistController playlistController = null;
			foreach (PlaylistController playlistController2 in list)
			{
				if (playlistController2.Id == playlistId)
				{
					playlistController = playlistController2;
					break;
				}
			}
			if (playlistController == null)
			{
				Debug.LogError("Audio System Error: playlist with key " + playlistId + " not found.");
				return null;
			}
			playlistController.Play();
			return playlistController;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000F64C File Offset: 0x0000D84C
		public static PlaylistController PlayPlaylist(string playlistId, float fadeDuration)
		{
			List<PlaylistController> list = LazyAudio.Instance.playlistControllers;
			PlaylistController playlistController = null;
			foreach (PlaylistController playlistController2 in list)
			{
				if (playlistController2.Id == playlistId)
				{
					playlistController = playlistController2;
					break;
				}
			}
			if (playlistController == null)
			{
				Debug.LogError("Audio System Error: playlist with key " + playlistId + " not found.");
				return null;
			}
			playlistController.Play(fadeDuration);
			return playlistController;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000F6D8 File Offset: 0x0000D8D8
		public static void PausePlaylist(string playlistId)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.Pause();
				}
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000F73C File Offset: 0x0000D93C
		public static void PausePlaylist(string playlistId, float duration)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.Pause(duration);
				}
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000F7A4 File Offset: 0x0000D9A4
		public static void PausePlaylist(string playlistId, float duration, Ease ease)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.Pause(duration, ease);
				}
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000F80C File Offset: 0x0000DA0C
		public static void UnPausePlaylist(string playlistId)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.UnPause();
				}
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000F870 File Offset: 0x0000DA70
		public static void UnPausePlaylist(string playlistId, float duration)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.UnPause(duration);
				}
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000F8D8 File Offset: 0x0000DAD8
		public static void UnPausePlaylist(string playlistId, float duration, Ease ease)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.UnPause(duration, ease);
				}
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000F940 File Offset: 0x0000DB40
		public static void StopPlaylist(string playlistId)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.Stop();
				}
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		public static void StopPlaylist(string playlistId, float fadeDuration)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.Stop(fadeDuration);
				}
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000FA0C File Offset: 0x0000DC0C
		public static void PlayNextTrack(string playlistId)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.NextTrack();
				}
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000FA70 File Offset: 0x0000DC70
		public static void PlayTrackInPlaylist(string trackId, string playlistId)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.PlayTrack(trackId);
				}
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000FAD8 File Offset: 0x0000DCD8
		public static void PlayTrackInPlaylist(string trackId, string playlistId, float fadeDuration)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.PlayTrack(trackId, fadeDuration);
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000FB40 File Offset: 0x0000DD40
		public static void StopAllPlaylistsImmediately()
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				playlistController.StopImmediately();
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000FB94 File Offset: 0x0000DD94
		public static void SetWeightTrackInPlaylist(string trackId, string playlistId, float weight)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.SetWeightTrack(trackId, weight);
				}
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000FBFC File Offset: 0x0000DDFC
		public static void AddWeightTrackInPlaylist(string trackId, string playlistId, float weight)
		{
			foreach (PlaylistController playlistController in LazyAudio.Instance.playlistControllers)
			{
				if (playlistController.Id == playlistId)
				{
					playlistController.AddWeightTrack(trackId, weight);
				}
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000FC64 File Offset: 0x0000DE64
		public static List<PlaylistController> GetPlaylistControllers()
		{
			return new List<PlaylistController>(LazyAudio.Instance.playlistControllers);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000FC78 File Offset: 0x0000DE78
		public static void SetChannelVolume(string channel, float volume)
		{
			if (volume <= 0f)
			{
				LazyAudio.Instance.audioConfig.audioMixer.SetFloat(channel, -80f);
				return;
			}
			LazyAudio.Instance.audioConfig.audioMixer.SetFloat(channel, Mathf.Log10(volume) * 20f);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000FCCB File Offset: 0x0000DECB
		public static void UpdateMicrophone(Transform microphone)
		{
			LazyAudio.Instance.microphone = microphone;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		private SoundController PlaySound(string id, bool playAndForget, bool checkDelay = true)
		{
			if (checkDelay && !this.IsValidTimePlaybackSound(id))
			{
				return null;
			}
			SoundController freeSoundController = this.GetFreeSoundController();
			if (freeSoundController == null)
			{
				return null;
			}
			Sound sound = this.audioConfig.Get(id);
			if (sound == null)
			{
				if (!this.audioConfig.defaultClip)
				{
					Debug.LogError("Audio System Error: sound with key [" + id + "] not found.");
					return null;
				}
				sound = new Sound();
				Sample sample = new Sample();
				sample.clip = this.audioConfig.defaultClip;
				sample.volume = 0.3f;
				sound.samples.Add(sample);
				sound.audioSettings3DType = AudioSettings3DType.Default;
				Debug.LogWarning("Audio System Warning: sound with key [" + id + "] not found. Playing [defaultClip]");
			}
			AudioSource audioSource;
			if (!this.audio3DSettingsPresets.TryGetValue(sound.audioSettings3DType, out audioSource))
			{
				Debug.LogError("Audio System Error: cannot find audio 3d settings preset for Sound [" + sound.id + "]");
				return null;
			}
			this.ApplySettingFromSample(sound.RandomSample, freeSoundController.audioSource, sound.volume, sound.panning, sound.loop, sound.group, audioSource);
			this.activeSources.Add(freeSoundController);
			freeSoundController.id = id;
			freeSoundController.Play();
			if (!playAndForget)
			{
				freeSoundController.soundHandler = new SoundHandler(freeSoundController);
			}
			return freeSoundController;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000FE18 File Offset: 0x0000E018
		private void ApplySettingFromSample(Sample sample, AudioSource audioSource, float soundVolume, float soundPanning, bool soundLooped, AudioMixerGroup group, AudioSource settings3DPreset)
		{
			audioSource.volume = sample.volume * soundVolume;
			audioSource.panStereo = sample.panning * soundPanning;
			audioSource.pitch = sample.Pitch;
			audioSource.outputAudioMixerGroup = group;
			audioSource.loop = soundLooped;
			audioSource.clip = sample.clip;
			audioSource.rolloffMode = settings3DPreset.rolloffMode;
			audioSource.minDistance = settings3DPreset.minDistance;
			audioSource.maxDistance = settings3DPreset.maxDistance;
			audioSource.dopplerLevel = settings3DPreset.dopplerLevel;
			audioSource.spread = settings3DPreset.spread;
			audioSource.spatialBlend = settings3DPreset.spatialBlend;
			audioSource.reverbZoneMix = settings3DPreset.reverbZoneMix;
			audioSource.SetCustomCurve(AudioSourceCurveType.Spread, settings3DPreset.GetCustomCurve(AudioSourceCurveType.Spread));
			audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, settings3DPreset.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
			audioSource.SetCustomCurve(AudioSourceCurveType.SpatialBlend, settings3DPreset.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
			audioSource.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, settings3DPreset.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000FF04 File Offset: 0x0000E104
		private SoundController GetFreeSoundController()
		{
			SoundController soundController;
			if (this.inactiveSources.Count != 0)
			{
				soundController = this.inactiveSources.Pop();
			}
			else
			{
				if (!this.dynamic)
				{
					Debug.LogError("Audio System Error: No free AudioSource available");
					return null;
				}
				soundController = this.CreateSoundController();
			}
			soundController.gameObject.SetActive(true);
			return soundController;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000FF54 File Offset: 0x0000E154
		private bool IsValidTimePlaybackSound(string id)
		{
			foreach (SoundController soundController in this.activeSources)
			{
				if (soundController.id == id && soundController.startTime + this.minDelayTimeBetweenSounds > Time.time)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040000D5 RID: 213
		[SerializeField]
		private Transform microphone;

		// Token: 0x040000D6 RID: 214
		[Header("Settings")]
		[SerializeField]
		private int initialPoolSize = 15;

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		private bool dynamic = true;

		// Token: 0x040000D8 RID: 216
		[SerializeField]
		private float minDelayTimeBetweenSounds = 15f;

		// Token: 0x040000D9 RID: 217
		[Space]
		[SerializeField]
		private List<SoundController> activeSources = new List<SoundController>();

		// Token: 0x040000DA RID: 218
		[SerializeField]
		private Stack<SoundController> inactiveSources = new Stack<SoundController>();

		// Token: 0x040000DB RID: 219
		private List<PlaylistController> playlistControllers = new List<PlaylistController>();

		// Token: 0x040000DC RID: 220
		private Dictionary<AudioSettings3DType, AudioSource> audio3DSettingsPresets = new Dictionary<AudioSettings3DType, AudioSource>();

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private AudioConfig audioConfig;

		// Token: 0x040000DE RID: 222
		private bool isDataCached;

		// Token: 0x040000DF RID: 223
		private List<PlaylistWeight> cachedPlaylistWeights = new List<PlaylistWeight>();

		// Token: 0x040000E0 RID: 224
		private static LazyAudio instance;

		// Token: 0x040000E1 RID: 225
		private static bool isInitialized;

		// Token: 0x040000E2 RID: 226
		private VoiceOverPlayer voiceOverPlayer;

		// Token: 0x040000E3 RID: 227
		public static Func<VoiceOverPlayer> VoiceOverPlayerFactory;
	}
}
