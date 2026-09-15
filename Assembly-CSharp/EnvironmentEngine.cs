using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02000AB9 RID: 2745
[ExecuteInEditMode]
public class EnvironmentEngine : MonoBehaviour, ICustomUpdatable
{
	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x06004A34 RID: 18996 RVA: 0x0015DDA8 File Offset: 0x0015BFA8
	// (set) Token: 0x06004A35 RID: 18997 RVA: 0x0015DDF7 File Offset: 0x0015BFF7
	public static EnvironmentEngine Instance
	{
		get
		{
			if (EnvironmentEngine.cachedInstance == null)
			{
				EnvironmentEngine.cachedInstance = global::UnityEngine.Object.FindObjectOfType<EnvironmentEngine>();
				if (EnvironmentEngine.cachedInstance == null)
				{
					Debug.LogError(string.Format("Cannot find instance of {0} on current scene.", typeof(EnvironmentEngine)));
				}
			}
			return EnvironmentEngine.cachedInstance;
		}
		set
		{
			EnvironmentEngine.cachedInstance = value;
		}
	}

	// Token: 0x140000CD RID: 205
	// (add) Token: 0x06004A36 RID: 18998 RVA: 0x0015DE00 File Offset: 0x0015C000
	// (remove) Token: 0x06004A37 RID: 18999 RVA: 0x0015DE34 File Offset: 0x0015C034
	public static event Action<float, bool> OnTimeOfDayChangedEvent;

	// Token: 0x140000CE RID: 206
	// (add) Token: 0x06004A38 RID: 19000 RVA: 0x0015DE68 File Offset: 0x0015C068
	// (remove) Token: 0x06004A39 RID: 19001 RVA: 0x0015DE9C File Offset: 0x0015C09C
	public static event Action<int> OnNewDayStarted;

	// Token: 0x140000CF RID: 207
	// (add) Token: 0x06004A3A RID: 19002 RVA: 0x0015DED0 File Offset: 0x0015C0D0
	// (remove) Token: 0x06004A3B RID: 19003 RVA: 0x0015DF04 File Offset: 0x0015C104
	public static event Action<int> OnNewDayStartedWithDayNumber;

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x06004A3C RID: 19004 RVA: 0x0015DF37 File Offset: 0x0015C137
	public string CurrentOverrodeAmbientSoundId
	{
		get
		{
			return this.currentOverrodeAmbientSoundId;
		}
	}

	// Token: 0x06004A3D RID: 19005 RVA: 0x0015DF40 File Offset: 0x0015C140
	public void PushOverrodeAmbientSound(object requester, string soundId, bool crossfade = true)
	{
		EnvironmentEngine.OverrideRequest overrideRequest = this.FindOverrideRequest(requester);
		if (overrideRequest != null)
		{
			overrideRequest.soundId = soundId ?? string.Empty;
			overrideRequest.crossfade = crossfade;
		}
		else
		{
			this.overrideRequests.Add(new EnvironmentEngine.OverrideRequest
			{
				requester = requester,
				soundId = (soundId ?? string.Empty),
				crossfade = crossfade
			});
		}
		this.ApplyTopOverrodeAmbientSound(crossfade);
	}

	// Token: 0x06004A3E RID: 19006 RVA: 0x0015DFA8 File Offset: 0x0015C1A8
	public void ClearOverrodeAmbientSound(object requester, bool crossfade = true)
	{
		for (int i = this.overrideRequests.Count - 1; i >= 0; i--)
		{
			if (this.overrideRequests[i].requester == requester)
			{
				this.overrideRequests.RemoveAt(i);
				break;
			}
		}
		this.ApplyTopOverrodeAmbientSound(crossfade);
	}

	// Token: 0x06004A3F RID: 19007 RVA: 0x0015DFF8 File Offset: 0x0015C1F8
	private EnvironmentEngine.OverrideRequest FindOverrideRequest(object requester)
	{
		for (int i = 0; i < this.overrideRequests.Count; i++)
		{
			if (this.overrideRequests[i].requester == requester)
			{
				return this.overrideRequests[i];
			}
		}
		return null;
	}

	// Token: 0x06004A40 RID: 19008 RVA: 0x0015E040 File Offset: 0x0015C240
	private void ApplyTopOverrodeAmbientSound(bool fallbackCrossfade)
	{
		string text = string.Empty;
		bool flag = fallbackCrossfade;
		for (int i = this.overrideRequests.Count - 1; i >= 0; i--)
		{
			if (!string.IsNullOrEmpty(this.overrideRequests[i].soundId))
			{
				text = this.overrideRequests[i].soundId;
				flag = this.overrideRequests[i].crossfade;
				break;
			}
		}
		if (text == this.currentOverrodeAmbientSoundId)
		{
			return;
		}
		this.currentOverrodeAmbientSoundId = text;
		this.ambientMixer.SwitchDuration = this.overrodeAmbientSoundSwitchTime;
		this.ambientMixer.SetOverride(string.IsNullOrEmpty(this.currentOverrodeAmbientSoundId) ? null : this.currentOverrodeAmbientSoundId, flag);
	}

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x06004A41 RID: 19009 RVA: 0x0015E0F4 File Offset: 0x0015C2F4
	public EnvironmentData Data
	{
		get
		{
			this.data = ((MainGame.Instance != null) ? MainGame.Instance.GameSave.environmentData : this.data);
			return this.data;
		}
	}

	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x06004A42 RID: 19010 RVA: 0x0015E126 File Offset: 0x0015C326
	public TimeOfDayPresets TimesOfDay
	{
		get
		{
			return this.timesOfDay;
		}
	}

	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x06004A43 RID: 19011 RVA: 0x0015E12E File Offset: 0x0015C32E
	// (set) Token: 0x06004A44 RID: 19012 RVA: 0x0015E136 File Offset: 0x0015C336
	public bool IsPaused
	{
		get
		{
			return this.isPaused;
		}
		set
		{
			if (this.isPaused == value)
			{
				return;
			}
			Debug.Log(string.Format("Set EnvEng IsPaused to [{0}] ", value));
			this.isPaused = value;
		}
	}

	// Token: 0x06004A45 RID: 19013 RVA: 0x0015E15E File Offset: 0x0015C35E
	public void ApplyOverridePreset(LightEnvironmentPreset preset, float intensity = 1f)
	{
		this.presetOverride = preset;
		this.presetOverrideIntensity = intensity;
		this.RecalcLightPresetLerp();
	}

	// Token: 0x06004A46 RID: 19014 RVA: 0x0015E174 File Offset: 0x0015C374
	public void ApplyOverridePreset(string presetName, float intensity = 1f)
	{
		if (string.IsNullOrEmpty(presetName))
		{
			this.ApplyOverridePreset(null, 0f);
			return;
		}
		LightEnvironmentPreset lightEnvironmentPreset = this.FindOverridePreset(presetName);
		if (lightEnvironmentPreset == null)
		{
			Debug.LogError("Light environment preset [" + presetName + "] wasn't found");
			return;
		}
		this.ApplyOverridePreset(lightEnvironmentPreset, intensity);
	}

	// Token: 0x06004A47 RID: 19015 RVA: 0x0015E1C5 File Offset: 0x0015C3C5
	public void RefreshAppliedLightPreset()
	{
		this.RecalcLightPresetLerp();
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x0015E1D0 File Offset: 0x0015C3D0
	private LightEnvironmentPreset FindOverridePreset(string presetName)
	{
		if (this.presets == null)
		{
			return null;
		}
		for (int i = 0; i < this.presets.Count; i++)
		{
			LightEnvironmentPreset lightEnvironmentPreset = this.presets[i];
			if (lightEnvironmentPreset != null && lightEnvironmentPreset.name == presetName)
			{
				return lightEnvironmentPreset;
			}
		}
		return null;
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x0015E224 File Offset: 0x0015C424
	public void SetTimeOfDay(float timeOfDay)
	{
		this.timeOfDay = timeOfDay;
		this.data.SetTimeOfDay(timeOfDay);
		this.OnTimeOfDayChanged(timeOfDay, false);
		Action<float, bool> onTimeOfDayChangedEvent = EnvironmentEngine.OnTimeOfDayChangedEvent;
		if (onTimeOfDayChangedEvent == null)
		{
			return;
		}
		onTimeOfDayChangedEvent(timeOfDay, false);
	}

	// Token: 0x06004A4A RID: 19018 RVA: 0x0015E252 File Offset: 0x0015C452
	public void SetTimeOfDayFake(float timeOfDay)
	{
		this.IsPaused = true;
		this.timeOfDay = timeOfDay;
		this.OnTimeOfDayChanged(timeOfDay, false);
		Action<float, bool> onTimeOfDayChangedEvent = EnvironmentEngine.OnTimeOfDayChangedEvent;
		if (onTimeOfDayChangedEvent == null)
		{
			return;
		}
		onTimeOfDayChangedEvent(timeOfDay, true);
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x0015E27B File Offset: 0x0015C47B
	public void ResumeTimeOfDayFromData()
	{
		this.IsPaused = false;
		this.timeOfDay = this.data.TimeOfDay;
		this.OnTimeOfDayChanged(this.timeOfDay, false);
		Action<float, bool> onTimeOfDayChangedEvent = EnvironmentEngine.OnTimeOfDayChangedEvent;
		if (onTimeOfDayChangedEvent == null)
		{
			return;
		}
		onTimeOfDayChangedEvent(this.timeOfDay, false);
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x0015E2B8 File Offset: 0x0015C4B8
	public TimeOfDayType GetTimeOfDayType()
	{
		if (this.timeOfDay >= 0.25f && this.timeOfDay < 0.75f)
		{
			return TimeOfDayType.Day;
		}
		return TimeOfDayType.Night;
	}

	// Token: 0x06004A4D RID: 19021 RVA: 0x0015E2D7 File Offset: 0x0015C4D7
	public void SetTimeOfDay_DEV(float timeOfDay)
	{
		this.SetTimeOfDay(timeOfDay);
		if (!Application.isPlaying)
		{
			this.OnTimeOfDayChanged(timeOfDay, false);
		}
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x0015E2F0 File Offset: 0x0015C4F0
	private void RecalcLightPresetLerp()
	{
		LightEnvironmentPreset.Lerp(this.presetTimeLerped, this.presetTime1, this.presetTime2, this.presetTimeLerp);
		if (this.presetOverride == null)
		{
			EnvironmentEngine.ApplyPreset(this.presetTimeLerped);
			return;
		}
		LightEnvironmentPreset.Lerp(this.presetOverrideLerped, this.presetTimeLerped, this.presetOverride, this.presetOverrideIntensity);
		EnvironmentEngine.ApplyPreset(this.presetOverrideLerped);
	}

	// Token: 0x06004A4F RID: 19023 RVA: 0x0015E35C File Offset: 0x0015C55C
	private void RecalcSoundPresetLerp(bool hardCut = false)
	{
		this.ambientMixer.SetAmbientPair(this.soundId1, this.soundId2, this.soundTimeLerp, !hardCut);
	}

	// Token: 0x06004A50 RID: 19024 RVA: 0x0015E380 File Offset: 0x0015C580
	private void OnTimeOfDayChanged(float timeOfDay, bool hardCutSound = false)
	{
		if (this.timesOfDay == null)
		{
			return;
		}
		this.presetTime1 = (this.presetTime2 = null);
		for (int i = 0; i < this.timesOfDay.presets.Count; i++)
		{
			TimeOfDayPresets.TimeAndPreset timeAndPreset = this.timesOfDay.presets[i];
			if (timeAndPreset.time == timeOfDay)
			{
				this.presetTime1 = (this.presetTime2 = timeAndPreset.preset);
				this.presetTimeLerp = 0f;
				break;
			}
			if (timeOfDay > timeAndPreset.time)
			{
				this.presetTime1 = timeAndPreset.preset;
				if (i + 1 >= this.timesOfDay.presets.Count)
				{
					Debug.LogError("Error picking a time of day preset. Probably, last preset time is less then 1.0");
					this.presetTime2 = this.presetTime1;
					this.presetTimeLerp = 0f;
					break;
				}
				TimeOfDayPresets.TimeAndPreset timeAndPreset2 = this.timesOfDay.presets[i + 1];
				if (timeOfDay <= timeAndPreset2.time)
				{
					this.presetTime2 = timeAndPreset2.preset;
					this.presetTimeLerp = (timeOfDay - timeAndPreset.time) / (timeAndPreset2.time - timeAndPreset.time);
					break;
				}
			}
		}
		this.RecalcLightPresetLerp();
		if (this.timesOfDay.soundEnvironmentConfig != null)
		{
			float crossfadeDuration = this.timesOfDay.soundEnvironmentConfig.GetCrossfadeDuration01(this.gameplayDayInMinutes * 60f);
			SoundEnvironmentConfig.FindPair(this.timesOfDay.soundEnvironmentConfig.sounds, timeOfDay, crossfadeDuration, out this.soundId1, out this.soundId2, out this.soundTimeLerp);
		}
		else
		{
			this.soundId1 = (this.soundId2 = null);
			this.soundTimeLerp = 0f;
		}
		this.RecalcSoundPresetLerp(hardCutSound);
		this.UpdateAdditionalSound(timeOfDay);
	}

	// Token: 0x06004A51 RID: 19025 RVA: 0x0015E534 File Offset: 0x0015C734
	private void UpdateAdditionalSound(float timeOfDay)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.additionalSoundConfig == null || this.additionalSoundConfig.sounds == null || this.additionalSoundConfig.sounds.Count == 0)
		{
			this.StopAdditionalSound();
			return;
		}
		string text = SoundEnvironmentConfig.FindActiveSegment(this.additionalSoundConfig.sounds, timeOfDay);
		if (text == this.currentAdditionalSoundId)
		{
			return;
		}
		this.StopAdditionalSound();
		this.currentAdditionalSoundId = text;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		this.additionalSoundHandler = LazyAudio.Play(text, false);
		if (this.additionalSoundHandler == null)
		{
			Debug.LogWarning("EnvironmentEngine: failed to play additional sound [" + text + "]");
		}
	}

	// Token: 0x06004A52 RID: 19026 RVA: 0x0015E5DE File Offset: 0x0015C7DE
	private void StopAdditionalSound()
	{
		SoundHandler soundHandler = this.additionalSoundHandler;
		if (soundHandler != null)
		{
			soundHandler.Stop();
		}
		this.additionalSoundHandler = null;
		this.currentAdditionalSoundId = null;
	}

	// Token: 0x06004A53 RID: 19027 RVA: 0x0015E600 File Offset: 0x0015C800
	public UniTask PreloadTimeOfDayPresets()
	{
		this.presetsPreloadTask = UniTask.Lazy(new Func<UniTask>(this.LoadTimeOfDayPresetsAsync));
		return this.presetsPreloadTask.Task;
	}

	// Token: 0x06004A54 RID: 19028 RVA: 0x0015E624 File Offset: 0x0015C824
	private UniTask LoadTimeOfDayPresetsAsync()
	{
		EnvironmentEngine.<LoadTimeOfDayPresetsAsync>d__75 <LoadTimeOfDayPresetsAsync>d__;
		<LoadTimeOfDayPresetsAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadTimeOfDayPresetsAsync>d__.<>4__this = this;
		<LoadTimeOfDayPresetsAsync>d__.<>1__state = -1;
		<LoadTimeOfDayPresetsAsync>d__.<>t__builder.Start<EnvironmentEngine.<LoadTimeOfDayPresetsAsync>d__75>(ref <LoadTimeOfDayPresetsAsync>d__);
		return <LoadTimeOfDayPresetsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06004A55 RID: 19029 RVA: 0x0015E668 File Offset: 0x0015C868
	public void SetTimeOfDayPreset(string presetName)
	{
		this.SetTimeOfDayPresetAsync(presetName).Forget();
	}

	// Token: 0x06004A56 RID: 19030 RVA: 0x0015E684 File Offset: 0x0015C884
	private async UniTaskVoid SetTimeOfDayPresetAsync(string presetName)
	{
		if (this.presetsPreloadTask != null)
		{
			await this.presetsPreloadTask.Task;
		}
		if (!(this == null))
		{
			TimeOfDayPresets timeOfDayPresets;
			if (this.timesOfDay != null && this.timesOfDay.name == presetName)
			{
				Debug.Log("Preset [" + presetName + "] was already loaded");
				this.SyncIndoorAudioSnapshot();
			}
			else if (!this.availablePresets.TryGetValue(presetName, out timeOfDayPresets))
			{
				Debug.LogError("Preset [" + presetName + "] wasn't found");
			}
			else
			{
				Debug.Log("SetTimeOfDayPreset: [" + presetName + "]");
				this.timesOfDay = timeOfDayPresets;
				this.data.timeOfDayPresetName = presetName;
				this.RecalcLightPresetLerp();
				this.OnTimeOfDayChanged(this.data.TimeOfDay, true);
				if (WeatherSystem.Instance != null)
				{
					WeatherSystem.Instance.SetPauseState(WeatherSystemPauseFlag.TimeOfDay, this.timesOfDay.indoorPreset);
					WeatherSystem.Instance.ApplySoundParameters(this.timesOfDay.indoorPreset, this.timesOfDay.applySfxFromOutdoor);
				}
				else
				{
					Debug.LogWarning("SetTimeOfDayPresetAsync: WeatherSystem.Instance is missing, skipping weather sync.");
				}
				this.SyncIndoorAudioSnapshot();
			}
		}
	}

	// Token: 0x06004A57 RID: 19031 RVA: 0x0015E6CF File Offset: 0x0015C8CF
	public void SyncIndoorAudioSnapshot()
	{
		if (WeatherSystem.Instance == null)
		{
			return;
		}
		WeatherSystem.Instance.AudioMixerStateController.SetIndoorVariant(this.ResolveIndoorAudioLayer());
	}

	// Token: 0x06004A58 RID: 19032 RVA: 0x0015E6F4 File Offset: 0x0015C8F4
	private AudioMixerSnapshotLayer ResolveIndoorAudioLayer()
	{
		if (this.timesOfDay == null || !this.timesOfDay.indoorPreset)
		{
			return AudioMixerSnapshotLayer.Default;
		}
		if (this.timesOfDay.name == "dungeons")
		{
			return AudioMixerSnapshotLayer.IndoorDungeon;
		}
		if (this.timesOfDay.name == "church")
		{
			return AudioMixerSnapshotLayer.IndoorChurch;
		}
		return AudioMixerSnapshotLayer.Indoor;
	}

	// Token: 0x06004A59 RID: 19033 RVA: 0x0015E754 File Offset: 0x0015C954
	public void CustomUpdate(float deltaTime)
	{
		if (EnvironmentEngine.Instance.isPaused)
		{
			return;
		}
		float num = this.ConvertDeltaTimeToGameplayTime01(deltaTime);
		float num2 = this.timeOfDay + num;
		this.SetTimeOfDay(MathUtilities.ClampCycleWithinRange(num2, 0f, 1f));
		this.Data.HandleTimeOfDayChanged(num);
		if (num2 >= 1f)
		{
			this.Data.AddToDay();
			Action<int> onNewDayStarted = EnvironmentEngine.OnNewDayStarted;
			if (onNewDayStarted != null)
			{
				onNewDayStarted(this.Data.Day);
			}
			Action<int> onNewDayStartedWithDayNumber = EnvironmentEngine.OnNewDayStartedWithDayNumber;
			if (onNewDayStartedWithDayNumber == null)
			{
				return;
			}
			onNewDayStartedWithDayNumber(this.Data.CurrentDayNumber);
		}
	}

	// Token: 0x06004A5A RID: 19034 RVA: 0x0015E7E9 File Offset: 0x0015C9E9
	public float ConvertDeltaTimeToGameplayTime01(float deltaTime)
	{
		return deltaTime / (this.gameplayDayInMinutes * 60f);
	}

	// Token: 0x06004A5B RID: 19035 RVA: 0x0015E7FC File Offset: 0x0015C9FC
	private void Awake()
	{
		Debug.Log("EnvironmentEngine.Awake()");
		this.presetTimeLerped = ScriptableObject.CreateInstance<LightEnvironmentPreset>();
		this.presetOverrideLerped = ScriptableObject.CreateInstance<LightEnvironmentPreset>();
		CameraSystem.Instance.MainCamera.AddAdditionalBloomThresholdGetter(() => this.additiveBloomThreshold);
		this.ambientMixer.SwitchDuration = this.overrodeAmbientSoundSwitchTime;
		this.TryInitDependencies();
		EnvironmentEngine.ApplyPreset(this.presetTimeLerped);
		this.OnTimeOfDayChanged(this.data.TimeOfDay, false);
	}

	// Token: 0x06004A5C RID: 19036 RVA: 0x0015E878 File Offset: 0x0015CA78
	private void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.ambientMixer.SwitchDuration = this.overrodeAmbientSoundSwitchTime;
		this.ambientMixer.Update(Time.deltaTime);
	}

	// Token: 0x06004A5D RID: 19037 RVA: 0x0015E8A3 File Offset: 0x0015CAA3
	private void OnDestroy()
	{
		this.ambientMixer.StopAll();
		this.StopAdditionalSound();
	}

	// Token: 0x06004A5E RID: 19038 RVA: 0x0015E8B8 File Offset: 0x0015CAB8
	private static void ApplyPreset(LightEnvironmentPreset preset)
	{
		if (preset == null)
		{
			return;
		}
		Light sunLight = EnvironmentEngine.Instance.lightsSystem.SunLight;
		sunLight.color = preset.sunLightColor;
		sunLight.intensity = preset.sunLightIntensity;
		sunLight.shadowStrength = preset.sunLightShadowStrength;
		sunLight.transform.rotation = Quaternion.Euler(preset.sunLightRotation);
		Light backLight = EnvironmentEngine.Instance.lightsSystem.BackLight;
		backLight.color = preset.backLightColor;
		backLight.intensity = preset.backLightIntensity;
		EnvironmentEngine.Instance.globalShaderParameters.backlightColor = preset.gspBacklightColor;
		EnvironmentEngine.Instance.globalShaderParameters.lightMaxBurn = preset.gspMaxLightBurn;
		EnvironmentEngine.Instance.globalShaderParameters.sunLight = preset.sunLightAmount;
		LazySingleton<GlobalShaderParameters>.Instance.ApplyShaderParameters();
		RenderSettings.ambientLight = preset.ambientLightColor;
		ColorGrading setting = CameraSystem.Instance.MainCamera.PostProcessVolume.profile.GetSetting<ColorGrading>();
		if (setting != null)
		{
			setting.ldrLut.value = preset.LutTexture;
		}
		EnvironmentEngine.Instance.additiveBloomThreshold = preset.additiveBloomThreshold;
		CameraSystem.Instance.MainCamera.UpdateBloomThreshold();
	}

	// Token: 0x06004A5F RID: 19039 RVA: 0x0015E9E4 File Offset: 0x0015CBE4
	private void TryInitDependencies()
	{
		GameObject gameObject = base.transform.parent.gameObject;
		if (this.lightsSystem == null)
		{
			this.lightsSystem = gameObject.GetComponentInChildren<LightsSystem>();
		}
		if (this.globalShaderParameters == null)
		{
			this.globalShaderParameters = gameObject.GetComponentInChildren<GlobalShaderParameters>();
		}
	}

	// Token: 0x06004A60 RID: 19040 RVA: 0x0015EA36 File Offset: 0x0015CC36
	private void SetTimeOfDayTest(float timeOfDay)
	{
		this.SetTimeOfDay(timeOfDay);
	}

	// Token: 0x06004A61 RID: 19041 RVA: 0x0015EA3F File Offset: 0x0015CC3F
	private void SetTimeOfDayFakeTest(float timeOfDay)
	{
		this.SetTimeOfDayFake(timeOfDay);
	}

	// Token: 0x06004A62 RID: 19042 RVA: 0x0015EA48 File Offset: 0x0015CC48
	private void ResumeTimeOfDatFromDataTest(float timeOfDay)
	{
		this.ResumeTimeOfDayFromData();
	}

	// Token: 0x040039F6 RID: 14838
	private const int MINUTES_PER_DAY = 1440;

	// Token: 0x040039F7 RID: 14839
	private const int SECONDS_PER_MINUTE = 60;

	// Token: 0x040039F8 RID: 14840
	private const string TIME_OF_DAY_PRESETS_LABEL = "_TimeOfDayPresets";

	// Token: 0x040039F9 RID: 14841
	private const string DungeonsTimeOfDayPresetName = "dungeons";

	// Token: 0x040039FA RID: 14842
	private const string ChurchTimeOfDayPresetName = "church";

	// Token: 0x040039FB RID: 14843
	private static EnvironmentEngine cachedInstance;

	// Token: 0x040039FF RID: 14847
	[SerializeField]
	private TimeOfDayPresets timesOfDay;

	// Token: 0x04003A00 RID: 14848
	[SerializeField]
	[Range(1f, 10f)]
	public float gameplayDayInMinutes = 5f;

	// Token: 0x04003A01 RID: 14849
	[Range(0f, 1f)]
	public float timeOfDay;

	// Token: 0x04003A02 RID: 14850
	[SerializeField]
	private EnvironmentData data = new EnvironmentData();

	// Token: 0x04003A03 RID: 14851
	[SerializeField]
	private List<LightEnvironmentPreset> presets = new List<LightEnvironmentPreset>();

	// Token: 0x04003A04 RID: 14852
	[SerializeField]
	private LightEnvironmentPreset presetOverride;

	// Token: 0x04003A05 RID: 14853
	[Range(0f, 1f)]
	public float presetOverrideIntensity = 1f;

	// Token: 0x04003A06 RID: 14854
	[SerializeField]
	private bool isPaused;

	// Token: 0x04003A07 RID: 14855
	[SerializeField]
	[HideInInspector]
	private LightsSystem lightsSystem;

	// Token: 0x04003A08 RID: 14856
	[SerializeField]
	[HideInInspector]
	private GlobalShaderParameters globalShaderParameters;

	// Token: 0x04003A09 RID: 14857
	private LightEnvironmentPreset presetTime1;

	// Token: 0x04003A0A RID: 14858
	private LightEnvironmentPreset presetTime2;

	// Token: 0x04003A0B RID: 14859
	[Range(0f, 1f)]
	private float presetTimeLerp;

	// Token: 0x04003A0C RID: 14860
	private float additiveBloomThreshold;

	// Token: 0x04003A0D RID: 14861
	private LightEnvironmentPreset presetTimeLerped;

	// Token: 0x04003A0E RID: 14862
	private LightEnvironmentPreset presetOverrideLerped;

	// Token: 0x04003A0F RID: 14863
	private Dictionary<string, TimeOfDayPresets> availablePresets = new Dictionary<string, TimeOfDayPresets>();

	// Token: 0x04003A10 RID: 14864
	private AsyncLazy presetsPreloadTask;

	// Token: 0x04003A11 RID: 14865
	[SerializeField]
	private SoundEnvironmentConfig additionalSoundConfig;

	// Token: 0x04003A12 RID: 14866
	private readonly AmbientSoundMixer ambientMixer = new AmbientSoundMixer();

	// Token: 0x04003A13 RID: 14867
	private string soundId1;

	// Token: 0x04003A14 RID: 14868
	private string soundId2;

	// Token: 0x04003A15 RID: 14869
	[Range(0f, 1f)]
	private float soundTimeLerp;

	// Token: 0x04003A16 RID: 14870
	[Range(0f, 10f)]
	private float overrodeAmbientSoundSwitchTime = 5f;

	// Token: 0x04003A17 RID: 14871
	private string currentOverrodeAmbientSoundId = string.Empty;

	// Token: 0x04003A18 RID: 14872
	private readonly List<EnvironmentEngine.OverrideRequest> overrideRequests = new List<EnvironmentEngine.OverrideRequest>();

	// Token: 0x04003A19 RID: 14873
	private string currentAdditionalSoundId;

	// Token: 0x04003A1A RID: 14874
	private SoundHandler additionalSoundHandler;

	// Token: 0x04003A1B RID: 14875
	public float timeOfDayTest;

	// Token: 0x02000ABA RID: 2746
	private class OverrideRequest
	{
		// Token: 0x04003A1C RID: 14876
		public object requester;

		// Token: 0x04003A1D RID: 14877
		public string soundId;

		// Token: 0x04003A1E RID: 14878
		public bool crossfade;
	}
}
