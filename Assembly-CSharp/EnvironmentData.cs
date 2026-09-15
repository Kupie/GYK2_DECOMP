using System;
using UnityEngine;

// Token: 0x02000460 RID: 1120
[Serializable]
public class EnvironmentData
{
	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06001D5D RID: 7517 RVA: 0x0008A441 File Offset: 0x00088641
	public int CurrentDayNumber
	{
		get
		{
			return this.GetDayNumberFromDay(this.day);
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06001D5E RID: 7518 RVA: 0x0008A44F File Offset: 0x0008864F
	public int NextDayNumber
	{
		get
		{
			if (!string.IsNullOrEmpty(this.overrodeNextDay))
			{
				return ConstDef.Get(this.overrodeNextDay).IntValue;
			}
			return this.GetDayNumberFromDay(this.day + 1);
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06001D5F RID: 7519 RVA: 0x0008A47D File Offset: 0x0008867D
	public float TimeOfDay
	{
		get
		{
			return this.timeOfDay;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06001D60 RID: 7520 RVA: 0x0008A485 File Offset: 0x00088685
	public int Day
	{
		get
		{
			return this.day;
		}
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x06001D61 RID: 7521 RVA: 0x0008A48D File Offset: 0x0008868D
	public EnvironmentEngine EnvironmentEngine
	{
		get
		{
			if (EnvironmentData.envEngineCache != null)
			{
				return EnvironmentData.envEngineCache;
			}
			EnvironmentData.envEngineCache = EnvironmentEngine.Instance;
			return EnvironmentData.envEngineCache;
		}
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x0008A4B4 File Offset: 0x000886B4
	public void PrepareForGame(float gameplayDayInMinutes)
	{
		this.gameplayDayInMinutes = gameplayDayInMinutes;
		EnvironmentEngine.Instance.SetTimeOfDay(this.timeOfDay);
		EnvironmentData.weatherSystemCache = WeatherSystem.Instance;
		EnvironmentData.vendorSystemCache = MainGame.Instance.GameSave.vendorSystem;
		EnvironmentData.townSystemCache = MainGame.Instance.GameSave.townSystem;
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x0008A50A File Offset: 0x0008870A
	public void SetTimeOfDay(float newTimeOfDay)
	{
		this.timeOfDay = newTimeOfDay;
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x0008A513 File Offset: 0x00088713
	public void HandleTimeOfDayChanged(float gameplayDeltaTime)
	{
		MainGame.PlayerData.energySystem.UpdateSleepLogic(gameplayDeltaTime);
		EnvironmentData.weatherSystemCache.OnGameTimeChanged(gameplayDeltaTime);
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x0008A530 File Offset: 0x00088730
	public int GetDayNumberFromDay(int day)
	{
		int num = day % 6;
		if (num != 0)
		{
			return num;
		}
		return 6;
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x0008A548 File Offset: 0x00088748
	public int GetDiffInDaysBetweenCurrentAndNext()
	{
		int currentDayNumber = this.CurrentDayNumber;
		int nextDayNumber = this.NextDayNumber;
		if (currentDayNumber >= nextDayNumber)
		{
			return 6 - currentDayNumber + nextDayNumber;
		}
		return nextDayNumber - currentDayNumber;
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x0008A570 File Offset: 0x00088770
	public void AddToDay()
	{
		if (!string.IsNullOrEmpty(this.overrodeNextDay))
		{
			int nextDayNumber = this.NextDayNumber;
			do
			{
				this.day++;
			}
			while (this.CurrentDayNumber != nextDayNumber);
			this.overrodeNextDay = string.Empty;
		}
		else
		{
			this.day++;
		}
		EnvironmentData.vendorSystemCache.UpdateSystemAtTheEndOfDay(this.day);
		EnvironmentData.townSystemCache.UpdateSystemAtTheEndOfDay(this.day);
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x0008A5E3 File Offset: 0x000887E3
	public void OverrideNextDayNumber(string dayName)
	{
		this.overrodeNextDay = dayName;
	}

	// Token: 0x04001B18 RID: 6936
	private const int MINUTES_PER_DAY = 1440;

	// Token: 0x04001B19 RID: 6937
	private const int SECONDS_PER_MINUTE = 60;

	// Token: 0x04001B1A RID: 6938
	public const int DAYS_IN_WEEK = 6;

	// Token: 0x04001B1B RID: 6939
	[Range(1f, 10f)]
	private float gameplayDayInMinutes = 5f;

	// Token: 0x04001B1C RID: 6940
	[SerializeField]
	[Range(0f, 1f)]
	private float timeOfDay = 0.5f;

	// Token: 0x04001B1D RID: 6941
	[SerializeField]
	private int day = 1;

	// Token: 0x04001B1E RID: 6942
	[SerializeField]
	private string overrodeNextDay;

	// Token: 0x04001B1F RID: 6943
	public string timeOfDayPresetName = "outdoor";

	// Token: 0x04001B20 RID: 6944
	private static EnvironmentEngine envEngineCache;

	// Token: 0x04001B21 RID: 6945
	private static WeatherSystem weatherSystemCache;

	// Token: 0x04001B22 RID: 6946
	private static VendorSystem vendorSystemCache;

	// Token: 0x04001B23 RID: 6947
	private static TownSystem townSystemCache;
}
