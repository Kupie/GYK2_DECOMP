using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class WorldZoneQualitySystemGameResSystem : GK2GameResSystem
{
	// Token: 0x06001C98 RID: 7320 RVA: 0x0008596D File Offset: 0x00083B6D
	public WorldZoneQualitySystemGameResSystem(string gameResAtomName, GameRes gameRes)
		: base(gameResAtomName, gameRes)
	{
		this.worldZoneId = gameResAtomName.Replace("wz_", "");
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x00085990 File Offset: 0x00083B90
	public override float Get()
	{
		string text = this.worldZoneId;
		if (!(text == "town"))
		{
			if (!(text == "conveyor"))
			{
				WorldZoneData worldZoneDataById = MainGame.Instance.GameSave.worldData.GetWorldZoneDataById(this.worldZoneId);
				float withoutSystemsCheck = this.resForChanges.GetWithoutSystemsCheck(this.gameResAtomName, 0f);
				float totalQuality = worldZoneDataById.GetTotalQuality();
				GK2GameResSystem.PlayerData.SetResWithoutSystemsCheck(this.gameResAtomName, totalQuality);
				if (!Mathf.Approximately(withoutSystemsCheck, totalQuality))
				{
					this.TryUnlockGraveyardQualityAchievement(totalQuality);
				}
			}
			else
			{
				float num = Mathf.Min(MainGame.Instance.conveyorSystem.ConveyorWorldZone.GetTotalQuality(WorldZoneWgoQualityType.ConveyorPowerSource), MainGame.Instance.conveyorSystem.ConveyorWorldZone.GetTotalQuality(WorldZoneWgoQualityType.ConveyorCells));
				GK2GameResSystem.PlayerData.SetResWithoutSystemsCheck(this.gameResAtomName, num);
			}
		}
		else
		{
			GK2GameResSystem.PlayerData.SetResWithoutSystemsCheck(this.gameResAtomName, (float)MainGame.Instance.GameSave.townSystem.Quality);
		}
		return base.Get();
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x00085A8B File Offset: 0x00083C8B
	public override void Add(float value, bool silent = false)
	{
		if (this.worldZoneId == "town")
		{
			MainGame.Instance.GameSave.townSystem.Quality += (int)value;
			return;
		}
		base.Add(value, silent);
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x00085AC5 File Offset: 0x00083CC5
	private void TryUnlockGraveyardQualityAchievement(float totalQuality)
	{
		if (this.worldZoneId == "graveyard" && totalQuality >= 200f)
		{
			AchievementsSystem.Instance.Unlock("ach_graveyard_quality_200");
		}
	}

	// Token: 0x04001ABB RID: 6843
	private string worldZoneId;
}
