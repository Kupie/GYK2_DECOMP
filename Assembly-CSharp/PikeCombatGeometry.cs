using System;
using UnityEngine;

// Token: 0x020002ED RID: 749
public static class PikeCombatGeometry
{
	// Token: 0x060013C1 RID: 5057 RVA: 0x000607E7 File Offset: 0x0005E9E7
	public static float EffectiveReach(FightingAgent agent)
	{
		if (((agent != null) ? agent.FighterDef : null) == null || agent.Wgo == null)
		{
			return 0f;
		}
		return agent.FighterDef.atkRange.EvaluateFloat(agent.Wgo) * 0.78f;
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x00060827 File Offset: 0x0005EA27
	public static float ApproachDistance(FightingAgent agent)
	{
		return Mathf.Max(0.4f, PikeCombatGeometry.EffectiveReach(agent) - 0.35f);
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x00060840 File Offset: 0x0005EA40
	public static bool IsAlignedForStrike(FightingAgent agent, Vector3 targetPos, float lateralTolerance)
	{
		bool flag;
		if (agent == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = agent.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (!flag)
		{
			return false;
		}
		Vector2 vector = (targetPos - agent.Wgo.Data.Position).XZ2();
		if (vector.sqrMagnitude < 0.0001f)
		{
			return false;
		}
		float num = BasicNpcSteppedRotationPreset.Instance.ComputeAngle(vector);
		Vector2 vector2 = new Vector2(Mathf.Cos(num * 0.017453292f), Mathf.Sin(num * 0.017453292f));
		float num2 = Vector2.Dot(vector, vector2);
		float num3 = Mathf.Abs(vector.x * vector2.y - vector.y * vector2.x);
		return num2 > 0f && num2 <= PikeCombatGeometry.EffectiveReach(agent) + 0.2f && num3 <= lateralTolerance;
	}

	// Token: 0x040014F7 RID: 5367
	public const float REACH_FACTOR = 0.78f;

	// Token: 0x040014F8 RID: 5368
	public const float MAX_LATERAL_OFFSET = 0.4f;

	// Token: 0x040014F9 RID: 5369
	public const float LATERAL_HYSTERESIS = 0.15f;

	// Token: 0x040014FA RID: 5370
	public const float ALONG_TOLERANCE = 0.2f;

	// Token: 0x040014FB RID: 5371
	public const float APPROACH_MARGIN = 0.35f;
}
