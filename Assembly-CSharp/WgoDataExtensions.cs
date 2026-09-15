using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000ACE RID: 2766
public static class WgoDataExtensions
{
	// Token: 0x06004AA9 RID: 19113 RVA: 0x001605B0 File Offset: 0x0015E7B0
	public static bool TryGetNearestBuilderWorldZone(this WgoData builderWgoData, out WorldZoneData worldZoneData)
	{
		worldZoneData = null;
		if (builderWgoData == null || string.IsNullOrEmpty(builderWgoData.WorldId))
		{
			return false;
		}
		GameSceneData gameSceneDataById = MainGame.Instance.GameSave.WorldData.GetGameSceneDataById(builderWgoData.WorldId);
		if (gameSceneDataById == null)
		{
			return false;
		}
		Vector2 vector = builderWgoData.Position.XZ2();
		Rect rect = new Rect(vector.x - 10f, vector.y - 10f, 20f, 20f);
		float num = float.PositiveInfinity;
		WorldZoneData worldZoneData2 = null;
		for (int i = 0; i < gameSceneDataById.worldZones.Count; i++)
		{
			WorldZoneData worldZoneData3 = gameSceneDataById.worldZones[i];
			if (WgoDataExtensions.IsBuilderForWorldZone(builderWgoData, worldZoneData3))
			{
				Rect wholeZoneRect = worldZoneData3.wholeZoneRect;
				if (wholeZoneRect.Overlaps(rect, true))
				{
					if (wholeZoneRect.Contains(vector))
					{
						worldZoneData = worldZoneData3;
						return true;
					}
					float rectSqrDistance = WgoDataExtensions.GetRectSqrDistance(wholeZoneRect, vector);
					if (num > rectSqrDistance)
					{
						num = rectSqrDistance;
						worldZoneData2 = worldZoneData3;
					}
				}
			}
		}
		if (worldZoneData2 == null)
		{
			return false;
		}
		worldZoneData = worldZoneData2;
		return true;
	}

	// Token: 0x06004AAA RID: 19114 RVA: 0x001606A8 File Offset: 0x0015E8A8
	private static bool IsBuilderForWorldZone(WgoData builderWgoData, WorldZoneData worldZoneData)
	{
		if (builderWgoData == null || worldZoneData == null)
		{
			return false;
		}
		WorldZoneDef worldZoneDef = worldZoneData.Definition ?? GameBalance.Me.GetDataOrNull<WorldZoneDef>(worldZoneData.id);
		return worldZoneDef != null && !string.IsNullOrEmpty(worldZoneDef.builderId) && worldZoneDef.builderId == builderWgoData.id;
	}

	// Token: 0x06004AAB RID: 19115 RVA: 0x001606FC File Offset: 0x0015E8FC
	private static float GetRectSqrDistance(Rect rect, Vector2 point)
	{
		float num = Mathf.Clamp(point.x, rect.xMin, rect.xMax);
		float num2 = Mathf.Clamp(point.y, rect.yMin, rect.yMax);
		float num3 = point.x - num;
		float num4 = point.y - num2;
		return num3 * num3 + num4 * num4;
	}

	// Token: 0x06004AAC RID: 19116 RVA: 0x00160754 File Offset: 0x0015E954
	public static void StoreSermonResult(this WgoData data, SermonResultData sermonResultData)
	{
		data.AddGameRes("money", sermonResultData.Money);
		bool flag = false;
		using (List<InteractionEvent>.Enumerator enumerator = data.Events.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.str == "sermon_reward")
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			data.AddInteractionEvent("sermon_reward", false);
		}
	}

	// Token: 0x06004AAD RID: 19117 RVA: 0x001607D8 File Offset: 0x0015E9D8
	public static void AddMoneyToPlayerAndClearSermonResult(this WgoData data)
	{
		MainGame.PlayerData.AddRes("money", (float)data.GetGameResInt("money"));
		LazyAudio.PlayAndForget("coins_sound");
		data.SetGameRes("money", 0f);
	}

	// Token: 0x06004AAE RID: 19118 RVA: 0x00160810 File Offset: 0x0015EA10
	public static void StorePaletteTradingResult(this WgoData data, int totalMoney)
	{
		data.AddGameRes("money", totalMoney);
		bool flag = false;
		using (List<InteractionEvent>.Enumerator enumerator = data.Events.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.str == "palette_trading_reward")
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			data.AddInteractionEvent("palette_trading_reward", false);
		}
	}

	// Token: 0x06004AAF RID: 19119 RVA: 0x001607D8 File Offset: 0x0015E9D8
	public static void AddMoneyToPlayerAndPaletteTradingResult(this WgoData data)
	{
		MainGame.PlayerData.AddRes("money", (float)data.GetGameResInt("money"));
		LazyAudio.PlayAndForget("coins_sound");
		data.SetGameRes("money", 0f);
	}

	// Token: 0x04003A78 RID: 14968
	private const float COLLIDER_BOX_SEARCH_HALF_SIZE = 10f;
}
