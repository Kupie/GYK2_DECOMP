using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200049D RID: 1181
[Serializable]
public class EnergySystem
{
	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06001F67 RID: 8039 RVA: 0x00085510 File Offset: 0x00083710
	private PerkSystemData PerkSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.perkSystemData;
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06001F68 RID: 8040 RVA: 0x00094B9F File Offset: 0x00092D9F
	// (set) Token: 0x06001F69 RID: 8041 RVA: 0x00094BA7 File Offset: 0x00092DA7
	public bool IsSleeping { get; private set; }

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06001F6A RID: 8042 RVA: 0x00094BB0 File Offset: 0x00092DB0
	// (set) Token: 0x06001F6B RID: 8043 RVA: 0x00094BB8 File Offset: 0x00092DB8
	public bool IsInTransitionBetweenSleep { get; private set; }

	// Token: 0x06001F6C RID: 8044 RVA: 0x00094BC4 File Offset: 0x00092DC4
	public void StartSleeping(Action onSleepEndedByEnergyCallback = null, Action onSleepDidNotStartedCallback = null, bool sleepWithoutSavingGame = false, bool sleepWithMaxEnergy = false, SleepAnimType sleepAnimType = SleepAnimType.None, float sleepDuration = 2f)
	{
		this.onSleepEndedByEnergyCallback = onSleepEndedByEnergyCallback;
		this.sleepWithoutSavingGame = sleepWithoutSavingGame;
		this.remainingSleepTime = sleepDuration;
		if (PlayerEnergyGameResSystem.GetSystem().HasMax() && !sleepWithMaxEnergy && !this.PerkSystemData.HasPerk("lack_of_sleep_debuff"))
		{
			Debug.Log("[EnergySystem]: player tried started sleeping, but it is not required");
			if (onSleepDidNotStartedCallback != null)
			{
				onSleepDidNotStartedCallback();
			}
			return;
		}
		if (EnvironmentEngine.Instance.IsPaused)
		{
			Debug.LogWarning("Try sleep with EnvironmentEngine paused. Unpausing");
			EnvironmentEngine.Instance.IsPaused = false;
		}
		MainGame.PlayerController.SetControlTakenType(TakenControlType.BySleep, false);
		MainGame.Instance.dropSystem.CollectAllGameResDropsToPlayer(1f);
		this.IsInTransitionBetweenSleep = true;
		LazyUI.Get<UISleepFade>().FadeIn(delegate
		{
			this.IsInTransitionBetweenSleep = false;
			this.IsSleeping = true;
			this.timeWithoutSleep = 0f;
			this.prevFrameTime = Time.time;
			MainGame.UpdateManager.SetTimeSpeedMultiplier(50f);
		}, FadeFlag.Common, true, sleepAnimType, false);
		Debug.Log("[EnergySystem]: player started sleeping");
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x00094C8C File Offset: 0x00092E8C
	public void StopSleeping()
	{
		this.IsInTransitionBetweenSleep = true;
		this.IsSleeping = false;
		MainGame.UpdateManager.SetTimeSpeedMultiplier(1f);
		LazyUI.Get<UISleepFade>().FadeOut(delegate
		{
			this.IsInTransitionBetweenSleep = false;
			MainGame.PlayerController.SetControlTakenType(TakenControlType.BySleep, true);
		}, false);
		Debug.Log("[EnergySystem]: player stopped sleeping");
		if (!this.sleepWithoutSavingGame)
		{
			SaveSystem.Save(MainGame.Instance.SaveSlotData, MainGame.Instance.GameSave, null, null, true, null);
			return;
		}
		this.sleepWithoutSavingGame = false;
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x00094D04 File Offset: 0x00092F04
	public void UpdateSleepLogic(float dayDeltaTime)
	{
		if (this.IsInTransitionBetweenSleep)
		{
			return;
		}
		if (this.IsSleeping)
		{
			this.RestoreEnergyWhileSleeping(dayDeltaTime);
			return;
		}
		this.TrackTimeWithoutSleep(dayDeltaTime);
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x00094D26 File Offset: 0x00092F26
	public void DeactivateLackOfSleep()
	{
		this.PerkSystemData.RemovePerk("lack_of_sleep_debuff", false);
		this.timeWithoutSleep = 0f;
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x00094D44 File Offset: 0x00092F44
	private void ActivateLackOfSleep()
	{
		this.PerkSystemData.AddPerk("lack_of_sleep_debuff");
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x00094D58 File Offset: 0x00092F58
	private void RestoreEnergyWhileSleeping(float dayDeltaTime)
	{
		if (this.remainingSleepTime >= 0f)
		{
			this.remainingSleepTime -= Time.time - this.prevFrameTime;
			this.prevFrameTime = Time.time;
		}
		if (!PlayerEnergyGameResSystem.GetSystem().HasMax())
		{
			float num = dayDeltaTime * 400f;
			PlayerEnergyGameResSystem.GetSystem().Add(num, false);
			return;
		}
		if (this.PerkSystemData.HasPerk("lack_of_sleep_debuff"))
		{
			this.DeactivateLackOfSleep();
			PlayerInsanityGameResSystem.GetSystem().Add(-20f, false);
			return;
		}
		if (this.remainingSleepTime <= 0f)
		{
			this.StopSleeping();
			Action action = this.onSleepEndedByEnergyCallback;
			if (action != null)
			{
				action();
			}
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.AfterSleep, "");
		}
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x00094E10 File Offset: 0x00093010
	private void TrackTimeWithoutSleep(float dayDeltaTime)
	{
		this.timeWithoutSleep += dayDeltaTime;
		if (this.timeWithoutSleep >= 2f && !this.PerkSystemData.HasPerk("lack_of_sleep_debuff"))
		{
			this.ActivateLackOfSleep();
		}
	}

	// Token: 0x04001C26 RID: 7206
	private const float TIME_MULTIPLIER_WHILE_SLEEP = 50f;

	// Token: 0x04001C27 RID: 7207
	private const int REMOVE_INSANITY_AFTER_SLEEP = -20;

	// Token: 0x04001C28 RID: 7208
	private const float ENERGY_PER_DELTA_VALUE = 400f;

	// Token: 0x04001C29 RID: 7209
	public float timeWithoutSleep;

	// Token: 0x04001C2A RID: 7210
	private Action onSleepEndedByEnergyCallback;

	// Token: 0x04001C2D RID: 7213
	private bool sleepWithoutSavingGame;

	// Token: 0x04001C2E RID: 7214
	private bool sleepWithMaxEnergy;

	// Token: 0x04001C2F RID: 7215
	private float remainingSleepTime;

	// Token: 0x04001C30 RID: 7216
	private float prevFrameTime;
}
