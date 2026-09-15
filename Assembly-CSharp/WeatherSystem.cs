using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using NodeCanvas.StateMachines;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000B33 RID: 2867
[ExecuteAlways]
[DefaultExecutionOrder(-5)]
public class WeatherSystem : MonoBehaviour
{
	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x06004C42 RID: 19522 RVA: 0x00167C4A File Offset: 0x00165E4A
	public static WeatherSystem Instance
	{
		get
		{
			if (WeatherSystem.instance == null)
			{
				WeatherSystem.instance = global::UnityEngine.Object.FindObjectOfType<WeatherSystem>(true);
			}
			return WeatherSystem.instance;
		}
	}

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x06004C43 RID: 19523 RVA: 0x00167C69 File Offset: 0x00165E69
	private WeatherData WeatherData
	{
		get
		{
			return MainGame.Instance.GameSave.weatherData;
		}
	}

	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x06004C44 RID: 19524 RVA: 0x00167C7A File Offset: 0x00165E7A
	// (set) Token: 0x06004C45 RID: 19525 RVA: 0x00167C82 File Offset: 0x00165E82
	public float WindValue
	{
		get
		{
			return this.windValue;
		}
		set
		{
			this.windValue = value;
			this.OnWindValueChanged();
		}
	}

	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x06004C46 RID: 19526 RVA: 0x00167C91 File Offset: 0x00165E91
	public AudioMixerStateController AudioMixerStateController
	{
		get
		{
			this.EnsureAudioMixerStateController();
			return this.audioMixerStateController;
		}
	}

	// Token: 0x06004C47 RID: 19527 RVA: 0x00167CA0 File Offset: 0x00165EA0
	protected void Awake()
	{
		if (WeatherSystem.instance != null && WeatherSystem.instance != this)
		{
			return;
		}
		WeatherSystem.instance = this;
		this.fsmOwner = base.GetComponent<FSMOwner>();
		foreach (WeatherComponent weatherComponent in base.GetComponentsInChildren<WeatherComponent>(true))
		{
			this.components.TryAdd(weatherComponent.name, weatherComponent);
		}
		LazyPlatformDependentElement[] componentsInChildren2 = base.GetComponentsInChildren<LazyPlatformDependentElement>(true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].Init();
		}
		this.pauseMultiFlagDisabledState.Init(new Action<bool>(this.SetPauseState), false);
		this.OnWindValueChanged();
	}

	// Token: 0x06004C48 RID: 19528 RVA: 0x00167D44 File Offset: 0x00165F44
	private void EnsureAudioMixerStateController()
	{
		if (this.audioMixerStateController != AudioMixerStateController.Unavailable)
		{
			return;
		}
		AudioMixer audioMixer;
		if (!LazyAudio.TryGetAudioMixer(out audioMixer))
		{
			return;
		}
		this.audioMixerStateController = new AudioMixerStateController(audioMixer);
		this.SyncAudioMixerLayersFromData();
	}

	// Token: 0x06004C49 RID: 19529 RVA: 0x00167D7B File Offset: 0x00165F7B
	private void SyncAudioMixerLayersFromData()
	{
		if (MainGame.Instance == null || MainGame.Instance.GameSave == null)
		{
			return;
		}
		this.SyncIndoorAudioLayerFromEnvironment();
		this.audioMixerStateController.SetLayerActive(AudioMixerSnapshotLayer.Cinematics, this.WeatherData.isWeatherPausedByCinematics);
	}

	// Token: 0x06004C4A RID: 19530 RVA: 0x00167DB4 File Offset: 0x00165FB4
	private void SyncIndoorAudioLayerFromEnvironment()
	{
		if (EnvironmentEngine.Instance != null)
		{
			EnvironmentEngine.Instance.SyncIndoorAudioSnapshot();
			return;
		}
		this.audioMixerStateController.SetLayerActive(AudioMixerSnapshotLayer.Indoor, this.WeatherData.isSoundEnabled);
	}

	// Token: 0x06004C4B RID: 19531 RVA: 0x00167DE8 File Offset: 0x00165FE8
	public void RestoreFSMStateFromData()
	{
		if (string.IsNullOrEmpty(this.WeatherData.stateName))
		{
			this.WeatherData.stateName = "CleanWeather";
		}
		foreach (string text in this.WeatherData.enabledWeatherComponents)
		{
			this.SetWeatherComponent(text, true);
		}
		this.SetWeatherState(this.WeatherData.stateName, false);
		this.pauseMultiFlagDisabledState.UpdateFlag(WeatherSystemPauseFlag.TimeOfDay, this.WeatherData.isWeatherPausedByTimeOfDay);
		this.pauseMultiFlagDisabledState.UpdateFlag(WeatherSystemPauseFlag.Cinematics, this.WeatherData.isWeatherPausedByCinematics);
		this.ApplySoundParameters(this.WeatherData.isSoundEnabled, this.WeatherData.isIndoorSfxEnabled);
		this.SyncIndoorAudioLayerFromEnvironment();
		this.AudioMixerStateController.SetLayerActive(AudioMixerSnapshotLayer.Cinematics, this.WeatherData.isWeatherPausedByCinematics);
	}

	// Token: 0x06004C4C RID: 19532 RVA: 0x00167EDC File Offset: 0x001660DC
	private void SetPauseState(bool isPaused)
	{
		this.isWeatherPaused = isPaused;
		if (isPaused)
		{
			this.ForceStopGodRays();
		}
		VerticalFog componentInChildren = base.GetComponentInChildren<VerticalFog>();
		if (componentInChildren != null)
		{
			componentInChildren.fogEnabled = !isPaused;
			componentInChildren.ApplyFogParameters();
		}
		base.gameObject.SetActive(!isPaused);
	}

	// Token: 0x06004C4D RID: 19533 RVA: 0x00167F28 File Offset: 0x00166128
	public void SetPauseState(WeatherSystemPauseFlag flag, bool isPaused)
	{
		this.pauseMultiFlagDisabledState.UpdateFlag(flag, isPaused);
		if (flag == WeatherSystemPauseFlag.TimeOfDay)
		{
			this.WeatherData.isWeatherPausedByTimeOfDay = isPaused;
			return;
		}
		if (flag != WeatherSystemPauseFlag.Cinematics)
		{
			return;
		}
		this.WeatherData.isWeatherPausedByCinematics = isPaused;
	}

	// Token: 0x06004C4E RID: 19534 RVA: 0x00167F5C File Offset: 0x0016615C
	public void ClearWeather()
	{
		this.ForceStopGodRays();
		this.lastWeatherStateName = "";
		this.WeatherData.isWeatherPausedByTimeOfDay = false;
		this.WeatherData.isWeatherPausedByCinematics = false;
		this.pauseMultiFlagDisabledState.Reset(false);
		this.SetPauseState(false);
		foreach (WeatherComponent weatherComponent in this.components.Values)
		{
			weatherComponent.ClearState();
		}
		this.AudioMixerStateController.ResetToDefault();
	}

	// Token: 0x06004C4F RID: 19535 RVA: 0x00167FF8 File Offset: 0x001661F8
	public void ApplySoundParameters(bool playSound, bool applyIndoorMod = false)
	{
		this.WeatherData.isSoundEnabled = playSound;
		this.WeatherData.isIndoorSfxEnabled = applyIndoorMod;
	}

	// Token: 0x06004C50 RID: 19536 RVA: 0x00168014 File Offset: 0x00166214
	public void OnGameTimeChanged(float deltaTime)
	{
		if (deltaTime > 0.5f || deltaTime < 0f)
		{
			return;
		}
		this.WeatherData.currentPhaseLen += deltaTime;
		if (this.WeatherData.currentPhaseLen >= this.phaseLength)
		{
			this.WeatherData.currentPhaseLen -= this.phaseLength;
			this.RollNextWeatherState();
		}
	}

	// Token: 0x06004C51 RID: 19537 RVA: 0x00168078 File Offset: 0x00166278
	public void SetWeatherState(string stateName, bool force = false)
	{
		FSMWeatherState fsmweatherState = this.fsmOwner.GetCurrentState(true) as FSMWeatherState;
		FSMState stateWithName = fsmweatherState.FSM.GetStateWithName(stateName);
		if (stateWithName == null)
		{
			Debug.LogWarning("Warning: instance name:[" + base.gameObject.name + "] Can't find a weather state with name: " + stateName);
			return;
		}
		string name = fsmweatherState.name;
		fsmweatherState.FSM.EnterState(stateWithName, FSM.TransitionCallMode.Normal);
		Debug.Log(string.Format("Set weather state to: {0}, force: {1}", stateName, force));
		this.WeatherData.stateName = stateName;
		if (force)
		{
			this.WeatherData.hasForceState = true;
		}
		this.TryTriggerGodRaysOnTransition(name, stateName);
		this.lastWeatherStateName = stateName;
	}

	// Token: 0x06004C52 RID: 19538 RVA: 0x0016811D File Offset: 0x0016631D
	public void ResetWeatherState()
	{
		this.SetWeatherState("CleanWeather", false);
		this.WeatherData.hasForceState = false;
	}

	// Token: 0x06004C53 RID: 19539 RVA: 0x00168138 File Offset: 0x00166338
	public void SetWeatherComponent(string componentName, bool isActive)
	{
		WeatherComponent weatherComponent;
		if (!this.components.TryGetValue(componentName, out weatherComponent))
		{
			Debug.LogWarning("Can't find weather component with name: " + componentName);
			return;
		}
		if (isActive)
		{
			weatherComponent.FadeIn();
			if (!this.WeatherData.enabledWeatherComponents.Contains(componentName))
			{
				this.WeatherData.enabledWeatherComponents.Add(componentName);
			}
		}
		else
		{
			weatherComponent.FadeOutIfActive();
			this.WeatherData.enabledWeatherComponents.Remove(componentName);
		}
		Debug.Log(string.Format("Set weather component: {0}, active: {1}", componentName, isActive));
	}

	// Token: 0x06004C54 RID: 19540 RVA: 0x001681C4 File Offset: 0x001663C4
	private void RollNextWeatherState()
	{
		if (this.WeatherData.hasForceState)
		{
			return;
		}
		FSMWeatherState fsmweatherState = this.fsmOwner.GetCurrentState(true) as FSMWeatherState;
		if (fsmweatherState == null)
		{
			Debug.Log("Current weather state is null.");
			return;
		}
		fsmweatherState.FinishWeatherState();
		this.stateWasChanged = true;
		this.needUpdateState = false;
	}

	// Token: 0x06004C55 RID: 19541 RVA: 0x00168214 File Offset: 0x00166414
	private void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (MainGame.IsGamePaused)
		{
			return;
		}
		foreach (WeatherComponent weatherComponent in this.components.Values)
		{
			weatherComponent.CustomUpdate();
		}
		CPWindValue.ApplyParameters();
		VerticalFog.GlobalInstance.GlobalFogUpdate();
		CPDirectShadowsBlur.ApplyParameters();
		CPCloudsDensity.ApplyParameters();
		if (this.needUpdateState)
		{
			this.needUpdateState = false;
			string name = this.fsmOwner.GetCurrentState(true).name;
			this.WeatherData.stateName = name;
			Debug.Log("New weather state: " + this.WeatherData.stateName);
			this.TryTriggerGodRaysOnTransition(this.lastWeatherStateName, name);
			this.lastWeatherStateName = name;
		}
		else if (this.stateWasChanged)
		{
			this.needUpdateState = true;
			this.stateWasChanged = false;
		}
		this.UpdateGodRaysPositions();
		this.UpdateGodRaysLifeTimes();
	}

	// Token: 0x06004C56 RID: 19542 RVA: 0x00168314 File Offset: 0x00166514
	private void OnDisable()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.ForceStopGodRays();
	}

	// Token: 0x06004C57 RID: 19543 RVA: 0x00168324 File Offset: 0x00166524
	private void TryTriggerGodRaysOnTransition(string prevStateName, string newStateName)
	{
		if (this.isWeatherPaused)
		{
			return;
		}
		if (string.IsNullOrEmpty(prevStateName))
		{
			return;
		}
		if (prevStateName.IndexOf("rain", StringComparison.OrdinalIgnoreCase) < 0)
		{
			return;
		}
		bool flag = newStateName.IndexOf("rain", StringComparison.OrdinalIgnoreCase) >= 0;
		bool flag2 = newStateName.IndexOf("mist", StringComparison.OrdinalIgnoreCase) >= 0;
		if (flag || flag2)
		{
			return;
		}
		float num = ((EnvironmentEngine.Instance != null) ? EnvironmentEngine.Instance.timeOfDay : (-1f));
		if (num < this.godRaysTimeMin || num >= this.godRaysTimeMax)
		{
			return;
		}
		this.StartGodRaysSequence();
	}

	// Token: 0x06004C58 RID: 19544 RVA: 0x001683BC File Offset: 0x001665BC
	private void StartGodRaysSequence()
	{
		this.StopGodRaysSequence();
		int num = Mathf.Max(0, this.godRaysCount - this.activeGodRays.Count);
		if (num <= 0)
		{
			return;
		}
		this.godRaysSpawnCoroutine = base.StartCoroutine(this.GodRaysSpawnRoutine(num));
	}

	// Token: 0x06004C59 RID: 19545 RVA: 0x00168400 File Offset: 0x00166600
	private IEnumerator GodRaysSpawnRoutine(int count)
	{
		int num;
		for (int i = 0; i < count; i = num + 1)
		{
			while (MainGame.IsGamePaused)
			{
				yield return null;
			}
			this.SpawnGodRay();
			if (i < count - 1)
			{
				float wait = global::UnityEngine.Random.Range(0.2f, 0.8f);
				float elapsed = 0f;
				while (elapsed < wait)
				{
					while (MainGame.IsGamePaused)
					{
						yield return null;
					}
					elapsed += Time.deltaTime;
					yield return null;
				}
			}
			num = i;
		}
		this.godRaysSpawnCoroutine = null;
		yield break;
	}

	// Token: 0x06004C5A RID: 19546 RVA: 0x00168416 File Offset: 0x00166616
	private void StopGodRaysSequence()
	{
		if (this.godRaysSpawnCoroutine != null)
		{
			base.StopCoroutine(this.godRaysSpawnCoroutine);
			this.godRaysSpawnCoroutine = null;
		}
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x00168434 File Offset: 0x00166634
	private void SpawnGodRay()
	{
		if (this.godRaysPrefab == null)
		{
			return;
		}
		Vector3 godRayActivationCenter = this.GetGodRayActivationCenter();
		Vector2 vector = global::UnityEngine.Random.insideUnitCircle * this.godRaysRange;
		Vector3 vector2 = new Vector3(vector.x, 0f, vector.y);
		Vector3 vector3 = godRayActivationCenter + vector2;
		GameObject pooledGodRay = this.GetPooledGodRay();
		pooledGodRay.transform.SetParent(base.transform, false);
		pooledGodRay.transform.position = vector3;
		pooledGodRay.SetActive(true);
		this.activeGodRays.Add(pooledGodRay);
		this.activeGodRaysLifeTimes.Add(this.godRaysLifeTime);
		this.activeGodRaysOffsets.Add(vector2);
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x001684DC File Offset: 0x001666DC
	private Vector3 GetGodRayActivationCenter()
	{
		CameraController cameraController = ((CameraSystem.Instance != null) ? CameraSystem.Instance.ActiveCameraController : null);
		if (cameraController != null && cameraController.Target != null)
		{
			return cameraController.Target.position;
		}
		if (CameraSystem.Instance != null && CameraSystem.Instance.WorldCamera != null)
		{
			return CameraSystem.Instance.WorldCamera.transform.position;
		}
		return base.transform.position;
	}

	// Token: 0x06004C5D RID: 19549 RVA: 0x00168568 File Offset: 0x00166768
	private GameObject GetPooledGodRay()
	{
		if (this.cachedGodRays.Count > 0)
		{
			int num = this.cachedGodRays.Count - 1;
			GameObject gameObject = this.cachedGodRays[num];
			this.cachedGodRays.RemoveAt(num);
			return gameObject;
		}
		return global::UnityEngine.Object.Instantiate<GameObject>(this.godRaysPrefab);
	}

	// Token: 0x06004C5E RID: 19550 RVA: 0x001685B8 File Offset: 0x001667B8
	private void ReleaseGodRay(int index)
	{
		GameObject gameObject = this.activeGodRays[index];
		gameObject.SetActive(false);
		this.activeGodRays.RemoveAt(index);
		this.activeGodRaysLifeTimes.RemoveAt(index);
		this.activeGodRaysOffsets.RemoveAt(index);
		this.cachedGodRays.Add(gameObject);
	}

	// Token: 0x06004C5F RID: 19551 RVA: 0x0016860C File Offset: 0x0016680C
	private void UpdateGodRaysPositions()
	{
		if (this.activeGodRays.Count == 0)
		{
			return;
		}
		Vector3 godRayActivationCenter = this.GetGodRayActivationCenter();
		for (int i = 0; i < this.activeGodRays.Count; i++)
		{
			this.activeGodRays[i].transform.position = godRayActivationCenter + this.activeGodRaysOffsets[i];
		}
	}

	// Token: 0x06004C60 RID: 19552 RVA: 0x0016866C File Offset: 0x0016686C
	private void UpdateGodRaysLifeTimes()
	{
		if (this.activeGodRaysLifeTimes.Count == 0)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		for (int i = this.activeGodRaysLifeTimes.Count - 1; i >= 0; i--)
		{
			float num = this.activeGodRaysLifeTimes[i] - deltaTime;
			if (num <= 0f)
			{
				this.ReleaseGodRay(i);
			}
			else
			{
				this.activeGodRaysLifeTimes[i] = num;
			}
		}
	}

	// Token: 0x06004C61 RID: 19553 RVA: 0x001686D4 File Offset: 0x001668D4
	private void ForceStopGodRays()
	{
		this.StopGodRaysSequence();
		for (int i = this.activeGodRays.Count - 1; i >= 0; i--)
		{
			this.ReleaseGodRay(i);
		}
	}

	// Token: 0x06004C62 RID: 19554 RVA: 0x00168706 File Offset: 0x00166906
	private void OnWindValueChanged()
	{
		Shader.SetGlobalFloat(GlobalShaderParameters.idWindValue, this.WindValue);
		this.UpdateDeformMaterialParameters(LazySingletonSO<GlobalResources>.Instance.matObject3DDeforming);
		this.UpdateDeformMaterialParameters(LazySingletonSO<GlobalResources>.Instance.matDeformingGrass);
		WorldParticleController.UpdateParameters();
		WindDependentSound.UpdateSounds();
	}

	// Token: 0x06004C63 RID: 19555 RVA: 0x00168744 File Offset: 0x00166944
	private void UpdateDeformMaterialParameters(Material mat)
	{
		mat.SetFloat(WeatherSystem.idWindAmp, this.windGrassMultiplier * this.WindValue * 0.15f + 0.025f);
		mat.SetFloat(WeatherSystem.idWindConstant, this.windGrassMultiplier * this.WindValue * 0.2f);
		mat.SetFloat(WeatherSystem.idWindGustAmp, this.windGrassMultiplier * this.WindValue * 0.5f);
	}

	// Token: 0x04003D5F RID: 15711
	public static readonly int idWindAmp = Shader.PropertyToID("_WindAmplitude");

	// Token: 0x04003D60 RID: 15712
	public static readonly int idWindConstant = Shader.PropertyToID("_WindConstant");

	// Token: 0x04003D61 RID: 15713
	public static readonly int idWindGustAmp = Shader.PropertyToID("_WindGustAmp");

	// Token: 0x04003D62 RID: 15714
	public const string CLEAN_WEATHER_NAME = "CleanWeather";

	// Token: 0x04003D63 RID: 15715
	public LightsSystem lightsSystem;

	// Token: 0x04003D64 RID: 15716
	private static WeatherSystem instance;

	// Token: 0x04003D65 RID: 15717
	[NonSerialized]
	public Dictionary<string, WeatherComponent> components = new Dictionary<string, WeatherComponent>();

	// Token: 0x04003D66 RID: 15718
	public float fadeTime = 4f;

	// Token: 0x04003D67 RID: 15719
	public float phaseLength = 0.25f;

	// Token: 0x04003D68 RID: 15720
	[Space]
	[SerializeField]
	[Range(0f, 1f)]
	private float windValue;

	// Token: 0x04003D69 RID: 15721
	[Space]
	[Range(-1f, 1f)]
	public float windGrassMultiplier = 1f;

	// Token: 0x04003D6A RID: 15722
	private FSMOwner fsmOwner;

	// Token: 0x04003D6B RID: 15723
	private bool stateWasChanged;

	// Token: 0x04003D6C RID: 15724
	private bool needUpdateState;

	// Token: 0x04003D6D RID: 15725
	private AudioMixerStateController audioMixerStateController = AudioMixerStateController.Unavailable;

	// Token: 0x04003D6E RID: 15726
	private MultiFlagOR<WeatherSystemPauseFlag> pauseMultiFlagDisabledState = new MultiFlagOR<WeatherSystemPauseFlag>();

	// Token: 0x04003D6F RID: 15727
	private const int MAX_GOD_RAYS = 5;

	// Token: 0x04003D70 RID: 15728
	private const float GOD_RAYS_LIFE_TIME = 10f;

	// Token: 0x04003D71 RID: 15729
	private const float MAX_RANGE_FROM_ACTIVATION_POINT = 10f;

	// Token: 0x04003D72 RID: 15730
	private const float GOD_RAY_SPAWN_INTERVAL_MIN = 0.2f;

	// Token: 0x04003D73 RID: 15731
	private const float GOD_RAY_SPAWN_INTERVAL_MAX = 0.8f;

	// Token: 0x04003D74 RID: 15732
	[SerializeField]
	private GameObject godRaysPrefab;

	// Token: 0x04003D75 RID: 15733
	[Header("God Rays Time Window (time of day 0..1)")]
	[SerializeField]
	[Range(0f, 1f)]
	private float godRaysTimeMin = 0.25f;

	// Token: 0x04003D76 RID: 15734
	[SerializeField]
	[Range(0f, 1f)]
	private float godRaysTimeMax = 0.65f;

	// Token: 0x04003D77 RID: 15735
	[SerializeField]
	private float godRaysLifeTime = 10f;

	// Token: 0x04003D78 RID: 15736
	[SerializeField]
	private float godRaysRange = 10f;

	// Token: 0x04003D79 RID: 15737
	[SerializeField]
	private int godRaysCount = 5;

	// Token: 0x04003D7A RID: 15738
	private List<GameObject> activeGodRays = new List<GameObject>();

	// Token: 0x04003D7B RID: 15739
	private List<float> activeGodRaysLifeTimes = new List<float>();

	// Token: 0x04003D7C RID: 15740
	private List<Vector3> activeGodRaysOffsets = new List<Vector3>();

	// Token: 0x04003D7D RID: 15741
	private List<GameObject> cachedGodRays = new List<GameObject>();

	// Token: 0x04003D7E RID: 15742
	private bool isWeatherPaused;

	// Token: 0x04003D7F RID: 15743
	private string lastWeatherStateName = "";

	// Token: 0x04003D80 RID: 15744
	private Coroutine godRaysSpawnCoroutine;
}
