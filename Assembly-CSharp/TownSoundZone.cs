using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000AC2 RID: 2754
[RequireComponent(typeof(Collider))]
public class TownSoundZone : MonoBehaviour
{
	// Token: 0x06004A84 RID: 19076 RVA: 0x0015FDD8 File Offset: 0x0015DFD8
	private void Awake()
	{
		Collider component = base.GetComponent<Collider>();
		if (component != null)
		{
			component.isTrigger = true;
		}
	}

	// Token: 0x06004A85 RID: 19077 RVA: 0x0015FDFC File Offset: 0x0015DFFC
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<ISoundZoneRecognizable>() == null)
		{
			return;
		}
		this.insideCount++;
		if (this.exitTimer >= 0f)
		{
			this.exitTimer = -1f;
			return;
		}
		if (this.insideCount == 1)
		{
			this.EnterZone();
		}
	}

	// Token: 0x06004A86 RID: 19078 RVA: 0x0015FE48 File Offset: 0x0015E048
	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<ISoundZoneRecognizable>() == null)
		{
			return;
		}
		this.insideCount = Mathf.Max(0, this.insideCount - 1);
		if (this.insideCount == 0 && this.isActive)
		{
			this.exitTimer = this.exitGraceTime;
		}
	}

	// Token: 0x06004A87 RID: 19079 RVA: 0x0015FE84 File Offset: 0x0015E084
	private void Update()
	{
		if (!this.isActive)
		{
			return;
		}
		if (this.exitTimer >= 0f)
		{
			this.exitTimer -= Time.deltaTime;
			if (this.exitTimer < 0f)
			{
				this.exitTimer = -1f;
				this.ExitZone();
				return;
			}
		}
		this.mixer.Update(Time.deltaTime);
	}

	// Token: 0x06004A88 RID: 19080 RVA: 0x0015FEE8 File Offset: 0x0015E0E8
	private void OnDestroy()
	{
		if (this.isActive)
		{
			this.ExitZone();
		}
	}

	// Token: 0x06004A89 RID: 19081 RVA: 0x0015FEF8 File Offset: 0x0015E0F8
	private void EnterZone()
	{
		this.config = AddressableUtils.LoadAssetReferenceSync<SoundEnvironmentConfig>(this.configRef, ref this.configHandle);
		if (this.config == null)
		{
			Debug.LogWarning("TownSoundZone [" + base.name + "]: failed to load SoundEnvironmentConfig");
			return;
		}
		this.isActive = true;
		this.exitTimer = -1f;
		this.mixer.SwitchDuration = this.switchTime;
		EnvironmentEngine.OnTimeOfDayChangedEvent += this.OnTimeOfDayChanged;
		if (MainGame.PlayerData != null)
		{
			MainGame.PlayerData.OnGameResChanged += this.OnGameResChanged;
		}
		this.currentGameResValue = this.ComputeGameResValue();
		float num = ((EnvironmentEngine.Instance != null) ? EnvironmentEngine.Instance.timeOfDay : 0f);
		this.ApplyPair(num);
	}

	// Token: 0x06004A8A RID: 19082 RVA: 0x0015FFC8 File Offset: 0x0015E1C8
	private void ExitZone()
	{
		if (!this.isActive)
		{
			return;
		}
		this.isActive = false;
		EnvironmentEngine.OnTimeOfDayChangedEvent -= this.OnTimeOfDayChanged;
		if (MainGame.PlayerData != null)
		{
			MainGame.PlayerData.OnGameResChanged -= this.OnGameResChanged;
		}
		this.mixer.StopAll();
		this.currentGameResValue = 0;
		this.cachedBaseId1 = (this.cachedBaseId2 = null);
		this.cachedVariantId1 = (this.cachedVariantId2 = null);
		this.config = null;
		AddressableUtils.ReleaseAssetReference<SoundEnvironmentConfig>(ref this.configHandle);
	}

	// Token: 0x06004A8B RID: 19083 RVA: 0x00160057 File Offset: 0x0015E257
	private void OnTimeOfDayChanged(float timeOfDay, bool isFake)
	{
		this.ApplyPair(timeOfDay);
	}

	// Token: 0x06004A8C RID: 19084 RVA: 0x00160060 File Offset: 0x0015E260
	private void OnGameResChanged()
	{
		int num = this.ComputeGameResValue();
		if (num == this.currentGameResValue)
		{
			return;
		}
		this.currentGameResValue = num;
		this.cachedBaseId1 = (this.cachedBaseId2 = null);
		this.ApplyPair(this.currentTimeOfDay);
	}

	// Token: 0x06004A8D RID: 19085 RVA: 0x001600A1 File Offset: 0x0015E2A1
	private int ComputeGameResValue()
	{
		if (MainGame.PlayerData == null || string.IsNullOrEmpty(this.soundPostfixPlayerGameRes))
		{
			return 0;
		}
		return (int)MainGame.PlayerData.GetRes(this.soundPostfixPlayerGameRes, 0f);
	}

	// Token: 0x06004A8E RID: 19086 RVA: 0x001600D0 File Offset: 0x0015E2D0
	private void ApplyPair(float timeOfDay)
	{
		this.currentTimeOfDay = timeOfDay;
		if (this.config == null)
		{
			return;
		}
		float num = ((EnvironmentEngine.Instance != null) ? this.config.GetCrossfadeDuration01(EnvironmentEngine.Instance.gameplayDayInMinutes * 60f) : 0f);
		string text;
		string text2;
		float num2;
		SoundEnvironmentConfig.FindPair(this.config.sounds, timeOfDay, num, out text, out text2, out num2);
		string text3 = this.WithVariant(text, ref this.cachedBaseId1, ref this.cachedVariantId1);
		string text4 = this.WithVariant(text2, ref this.cachedBaseId2, ref this.cachedVariantId2);
		this.mixer.SetAmbientPair(text3, text4, num2, true);
	}

	// Token: 0x06004A8F RID: 19087 RVA: 0x00160178 File Offset: 0x0015E378
	private string WithVariant(string baseId, ref string cachedBase, ref string cachedVariant)
	{
		if (string.IsNullOrEmpty(baseId) || this.currentGameResValue <= 0)
		{
			return baseId;
		}
		if (baseId == cachedBase && cachedVariant != null)
		{
			return cachedVariant;
		}
		cachedBase = baseId;
		cachedVariant = baseId + "_" + this.currentGameResValue.ToString(CultureInfo.InvariantCulture);
		return cachedVariant;
	}

	// Token: 0x04003A5E RID: 14942
	[SerializeField]
	private AssetReferenceT<SoundEnvironmentConfig> configRef;

	// Token: 0x04003A5F RID: 14943
	[SerializeField]
	private string soundPostfixPlayerGameRes;

	// Token: 0x04003A60 RID: 14944
	[SerializeField]
	[Range(0.01f, 10f)]
	private float switchTime = 5f;

	// Token: 0x04003A61 RID: 14945
	[SerializeField]
	[Range(0f, 2f)]
	private float exitGraceTime = 0.4f;

	// Token: 0x04003A62 RID: 14946
	private readonly AmbientSoundMixer mixer = new AmbientSoundMixer();

	// Token: 0x04003A63 RID: 14947
	private AsyncOperationHandle<SoundEnvironmentConfig> configHandle;

	// Token: 0x04003A64 RID: 14948
	private SoundEnvironmentConfig config;

	// Token: 0x04003A65 RID: 14949
	private int insideCount;

	// Token: 0x04003A66 RID: 14950
	private bool isActive;

	// Token: 0x04003A67 RID: 14951
	private float exitTimer = -1f;

	// Token: 0x04003A68 RID: 14952
	private float currentTimeOfDay;

	// Token: 0x04003A69 RID: 14953
	private int currentGameResValue;

	// Token: 0x04003A6A RID: 14954
	private string cachedBaseId1;

	// Token: 0x04003A6B RID: 14955
	private string cachedBaseId2;

	// Token: 0x04003A6C RID: 14956
	private string cachedVariantId1;

	// Token: 0x04003A6D RID: 14957
	private string cachedVariantId2;
}
