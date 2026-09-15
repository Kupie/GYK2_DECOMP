using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200040C RID: 1036
[CreateAssetMenu(fileName = "FishingSettings", menuName = "ScriptableObjects/FishingSettings")]
public class FishingSettings : LazySingletonSerializedSO<FishingSettings>
{
	// Token: 0x06001B12 RID: 6930 RVA: 0x0007E2EC File Offset: 0x0007C4EC
	private void ResetDriftSettingsToDefaults()
	{
		this.bothPullingDrift = FishingSettings.FishDriftStateSettings.BothPullingPreset;
		this.fishPullingOnlyDrift = FishingSettings.FishDriftStateSettings.FishPullingOnlyPreset;
		this.idleDrift = FishingSettings.FishDriftStateSettings.IdlePreset;
		this.driftFadeOutDuration = 0.35f;
		this.frequencyTransitionSpeed = 10f;
		this.axisTransitionSpeed = 3f;
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06001B13 RID: 6931 RVA: 0x0007E33B File Offset: 0x0007C53B
	public float PullingProgressConst
	{
		get
		{
			return this.pullingProgressConst;
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06001B14 RID: 6932 RVA: 0x0007E343 File Offset: 0x0007C543
	public float IdleProgressConst
	{
		get
		{
			return this.idleProgressConst;
		}
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06001B15 RID: 6933 RVA: 0x0007E34B File Offset: 0x0007C54B
	public float CurveMultiplier
	{
		get
		{
			return this.curveMultiplier;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06001B16 RID: 6934 RVA: 0x0007E353 File Offset: 0x0007C553
	public ReservoirConfig DefaultReservoirConfig
	{
		get
		{
			return this.defaultReservoirConfig;
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0007E35B File Offset: 0x0007C55B
	public FishingSettings.FishDriftStateSettings BothPullingDrift
	{
		get
		{
			return this.bothPullingDrift;
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06001B18 RID: 6936 RVA: 0x0007E363 File Offset: 0x0007C563
	public FishingSettings.FishDriftStateSettings FishPullingOnlyDrift
	{
		get
		{
			return this.fishPullingOnlyDrift;
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0007E36B File Offset: 0x0007C56B
	public FishingSettings.FishDriftStateSettings IdleDrift
	{
		get
		{
			return this.idleDrift;
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06001B1A RID: 6938 RVA: 0x0007E373 File Offset: 0x0007C573
	public float DriftFadeOutDuration
	{
		get
		{
			return this.driftFadeOutDuration;
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06001B1B RID: 6939 RVA: 0x0007E37B File Offset: 0x0007C57B
	public float FrequencyTransitionSpeed
	{
		get
		{
			return this.frequencyTransitionSpeed;
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06001B1C RID: 6940 RVA: 0x0007E383 File Offset: 0x0007C583
	public float AxisTransitionSpeed
	{
		get
		{
			return this.axisTransitionSpeed;
		}
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x0007E38C File Offset: 0x0007C58C
	public AnimationCurve GetCurveById(string fishingId)
	{
		if (this.curvePresets.Exists((FishingSettings.FishingCurve x) => x.id == fishingId))
		{
			return this.curvePresets.Find((FishingSettings.FishingCurve x) => x.id == fishingId).curve;
		}
		Debug.LogError("[FishingSettings]: curve with id '" + fishingId + "' not found");
		return null;
	}

	// Token: 0x04001A39 RID: 6713
	[SerializeField]
	private List<FishingSettings.FishingCurve> curvePresets;

	// Token: 0x04001A3A RID: 6714
	[SerializeField]
	private ReservoirConfig defaultReservoirConfig;

	// Token: 0x04001A3B RID: 6715
	[SerializeField]
	private float pullingProgressConst;

	// Token: 0x04001A3C RID: 6716
	[SerializeField]
	private float idleProgressConst;

	// Token: 0x04001A3D RID: 6717
	[SerializeField]
	private float curveMultiplier;

	// Token: 0x04001A3E RID: 6718
	[SerializeField]
	public Gradient ropeGradient;

	// Token: 0x04001A3F RID: 6719
	[SerializeField]
	public Gradient tensionBarGradient;

	// Token: 0x04001A40 RID: 6720
	[Tooltip("Sharp oscillations when both player and fish are pulling (high tension)")]
	[SerializeField]
	private FishingSettings.FishDriftStateSettings bothPullingDrift = FishingSettings.FishDriftStateSettings.BothPullingPreset;

	// Token: 0x04001A41 RID: 6721
	[Tooltip("Medium oscillations when only fish is pulling (fish resisting, player idle)")]
	[SerializeField]
	private FishingSettings.FishDriftStateSettings fishPullingOnlyDrift = FishingSettings.FishDriftStateSettings.FishPullingOnlyPreset;

	// Token: 0x04001A42 RID: 6722
	[Tooltip("Slow idle drift when nobody is pulling (calm state)")]
	[SerializeField]
	private FishingSettings.FishDriftStateSettings idleDrift = FishingSettings.FishDriftStateSettings.IdlePreset;

	// Token: 0x04001A43 RID: 6723
	[Tooltip("Duration for fade-out when fishing ends or oscillation stops (seconds)")]
	[SerializeField]
	private float driftFadeOutDuration = 0.35f;

	// Token: 0x04001A44 RID: 6724
	[Tooltip("Speed of frequency transition when switching presets (Hz per second)")]
	[SerializeField]
	private float frequencyTransitionSpeed = 10f;

	// Token: 0x04001A45 RID: 6725
	[Tooltip("Speed of axis transition when switching presets (units per second)")]
	[SerializeField]
	private float axisTransitionSpeed = 3f;

	// Token: 0x0200040D RID: 1037
	[Serializable]
	public class FishingCurve
	{
		// Token: 0x04001A46 RID: 6726
		[SerializeField]
		public string id;

		// Token: 0x04001A47 RID: 6727
		[SerializeField]
		public AnimationCurve curve;
	}

	// Token: 0x0200040E RID: 1038
	[Serializable]
	public class FishDriftStateSettings
	{
		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x0007E44D File Offset: 0x0007C64D
		public float MinAmplitude
		{
			get
			{
				return this.minAmplitude;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x0007E455 File Offset: 0x0007C655
		public float MaxAmplitude
		{
			get
			{
				return this.maxAmplitude;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x0007E45D File Offset: 0x0007C65D
		public float MinDuration
		{
			get
			{
				return this.minDuration;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x0007E465 File Offset: 0x0007C665
		public float MaxDuration
		{
			get
			{
				return this.maxDuration;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x0007E46D File Offset: 0x0007C66D
		public float Frequency
		{
			get
			{
				return this.frequency;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x0007E475 File Offset: 0x0007C675
		public float MinSplashPeriodTime
		{
			get
			{
				return this.minSplashPeriodTime;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x0007E47D File Offset: 0x0007C67D
		public Vector3 OscillationAxis
		{
			get
			{
				if (this.oscillationAxis.sqrMagnitude <= 0.001f)
				{
					return Vector3.zero;
				}
				return this.oscillationAxis.normalized;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x0007E4A2 File Offset: 0x0007C6A2
		public bool EnableRandomization
		{
			get
			{
				return this.enableRandomization;
			}
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0007E4AC File Offset: 0x0007C6AC
		public FishDriftStateSettings()
		{
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0007E514 File Offset: 0x0007C714
		public FishDriftStateSettings(float minAmp, float maxAmp, float minDur, float maxDur, float freq, Vector3 axis, bool randomize = true)
		{
			this.minAmplitude = minAmp;
			this.maxAmplitude = maxAmp;
			this.minDuration = minDur;
			this.maxDuration = maxDur;
			this.frequency = freq;
			this.oscillationAxis = axis;
			this.enableRandomization = randomize;
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0007E5B0 File Offset: 0x0007C7B0
		public void CopyFrom(FishingSettings.FishDriftStateSettings other)
		{
			this.minAmplitude = other.minAmplitude;
			this.maxAmplitude = other.maxAmplitude;
			this.minDuration = other.minDuration;
			this.maxDuration = other.maxDuration;
			this.frequency = other.frequency;
			this.minSplashPeriodTime = other.minSplashPeriodTime;
			this.oscillationAxis = other.oscillationAxis;
			this.enableRandomization = other.enableRandomization;
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x0007E61D File Offset: 0x0007C81D
		public static FishingSettings.FishDriftStateSettings BothPullingPreset
		{
			get
			{
				return new FishingSettings.FishDriftStateSettings(0.4f, 0.4f, 0.35f, 0.8f, 7f, Vector3.forward, true);
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x0007E643 File Offset: 0x0007C843
		public static FishingSettings.FishDriftStateSettings FishPullingOnlyPreset
		{
			get
			{
				return new FishingSettings.FishDriftStateSettings(0.25f, 0.25f, 0.6f, 1.2f, 4.25f, Vector3.forward, true);
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0007E669 File Offset: 0x0007C869
		public static FishingSettings.FishDriftStateSettings IdlePreset
		{
			get
			{
				return new FishingSettings.FishDriftStateSettings(0.1f, 0.1f, 1f, 2f, 2f, Vector3.forward, true);
			}
		}

		// Token: 0x04001A48 RID: 6728
		[Tooltip("Minimum oscillation amplitude")]
		[SerializeField]
		private float minAmplitude = 0.1f;

		// Token: 0x04001A49 RID: 6729
		[Tooltip("Maximum oscillation amplitude")]
		[SerializeField]
		private float maxAmplitude = 0.1f;

		// Token: 0x04001A4A RID: 6730
		[Tooltip("Minimum duration of this drift phase before re-randomizing")]
		[SerializeField]
		private float minDuration = 1f;

		// Token: 0x04001A4B RID: 6731
		[Tooltip("Maximum duration of this drift phase before re-randomizing")]
		[SerializeField]
		private float maxDuration = 2f;

		// Token: 0x04001A4C RID: 6732
		[Tooltip("Oscillation frequency (cycles per second)")]
		[SerializeField]
		private float frequency = 2f;

		// Token: 0x04001A4D RID: 6733
		[Tooltip("Minimum period of time between fish splashes")]
		[SerializeField]
		private float minSplashPeriodTime = 0.8f;

		// Token: 0x04001A4E RID: 6734
		[Tooltip("Axis direction for oscillation. Use (0,0,1) for Z only, (1,0,0) for X only, (1,0,1) for XZ plane")]
		[SerializeField]
		private Vector3 oscillationAxis = Vector3.forward;

		// Token: 0x04001A4F RID: 6735
		[Tooltip("If enabled, parameters (amplitude) will be re-randomized periodically based on duration. If disabled, oscillation continues unchanged while this preset is active.")]
		[SerializeField]
		private bool enableRandomization = true;
	}
}
