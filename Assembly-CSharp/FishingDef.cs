using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001E7 RID: 487
[Serializable]
public class FishingDef : BalanceBaseObject
{
	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0003E19B File Offset: 0x0003C39B
	public float IntervalTimeLeft
	{
		get
		{
			return this.intervalTimeRange[0];
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000C3B RID: 3131 RVA: 0x0003E1A5 File Offset: 0x0003C3A5
	public float IntervalTimeRight
	{
		get
		{
			return this.intervalTimeRange[1];
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0003E1AF File Offset: 0x0003C3AF
	public float ResistTimeLeft
	{
		get
		{
			return this.resistTimeRange[0];
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000C3D RID: 3133 RVA: 0x0003E1B9 File Offset: 0x0003C3B9
	public float ResistTimeRight
	{
		get
		{
			return this.resistTimeRange[1];
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0003E1C3 File Offset: 0x0003C3C3
	public float WaitTimeLeft
	{
		get
		{
			return this.waitTimeRange[0];
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000C3F RID: 3135 RVA: 0x0003E1CD File Offset: 0x0003C3CD
	public float WaitTimeRight
	{
		get
		{
			return this.waitTimeRange[1];
		}
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x0003E1D8 File Offset: 0x0003C3D8
	public static List<FishingDef> GetAllForReservoir(string reservoirId)
	{
		return GameBalance.Me.fishingDefs.FindAll((FishingDef x) => x.reservoirId == reservoirId);
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x0003E210 File Offset: 0x0003C410
	public static List<Item> GetAvailableBaits(List<Item> baits, List<FishingDef> fishingDefs)
	{
		List<Item> list = new List<Item>(baits);
		using (List<Item>.Enumerator enumerator = baits.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Item bait = enumerator.Current;
				bool flag = false;
				Predicate<GameResAtom> <>9__0;
				foreach (FishingDef fishingDef in fishingDefs)
				{
					if (fishingDef.baitMod != null)
					{
						List<GameResAtom> list2 = fishingDef.baitMod.List;
						Predicate<GameResAtom> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = (GameResAtom x) => x.type == bait.id);
						}
						if (list2.Exists(predicate))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					list.Remove(bait);
				}
			}
		}
		return list;
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x0003E304 File Offset: 0x0003C504
	public float GetDayTimeMod()
	{
		TimeOfDayType timeOfDayType = EnvironmentEngine.Instance.GetTimeOfDayType();
		if (timeOfDayType == TimeOfDayType.Day)
		{
			return this.daytimeMod[0];
		}
		if (timeOfDayType != TimeOfDayType.Night)
		{
			Debug.LogError(string.Format("[{0}]: Unknown time of day type: {1}", "FishingDef", timeOfDayType));
			return 0f;
		}
		return this.daytimeMod[1];
	}

	// Token: 0x04000DC9 RID: 3529
	[AutoParse("fish_item")]
	public string fishId;

	// Token: 0x04000DCA RID: 3530
	[AutoParse("reservoir_id")]
	public string reservoirId;

	// Token: 0x04000DCB RID: 3531
	[AutoParse("curve_preset_id")]
	public string curvePresetId;

	// Token: 0x04000DCC RID: 3532
	public float[] intervalTimeRange = new float[2];

	// Token: 0x04000DCD RID: 3533
	public float[] resistTimeRange = new float[2];

	// Token: 0x04000DCE RID: 3534
	public float[] waitTimeRange = new float[2];

	// Token: 0x04000DCF RID: 3535
	[AutoParse("bait_time")]
	public float baitTime;

	// Token: 0x04000DD0 RID: 3536
	[AutoParse("speed_change_time")]
	public float speedChangeTime;

	// Token: 0x04000DD1 RID: 3537
	[AutoParse("anger")]
	public LazyExpression anger = new LazyExpression();

	// Token: 0x04000DD2 RID: 3538
	[AutoParse("base_weight")]
	public int baseWeight;

	// Token: 0x04000DD3 RID: 3539
	[AutoParse("bait_mod")]
	public GameRes baitMod;

	// Token: 0x04000DD4 RID: 3540
	public float[] daytimeMod = new float[2];

	// Token: 0x04000DD5 RID: 3541
	[AutoParse("base_count")]
	public int baseCount;

	// Token: 0x04000DD6 RID: 3542
	[AutoParse("regen_time")]
	public float regenTime;

	// Token: 0x04000DD7 RID: 3543
	[AutoParse("expression_on_end")]
	public List<LazyExpression> expressionsOnEnd;

	// Token: 0x04000DD8 RID: 3544
	[AutoParse("underwater_gfx_type")]
	public FishGfxUnderwaterType underwaterGfxType = FishGfxUnderwaterType.Small;

	// Token: 0x04000DD9 RID: 3545
	[AutoParse("splash_coef")]
	public float splashCoef = 1f;

	// Token: 0x04000DDA RID: 3546
	[AutoParse("wriggle_coef")]
	public float wriggleCoef = 1f;
}
