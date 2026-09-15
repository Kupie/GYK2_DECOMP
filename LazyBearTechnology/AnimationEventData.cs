using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000C1 RID: 193
	[Serializable]
	public class AnimationEventData
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000EAEC File Offset: 0x0000CCEC
		public string PickTrigger()
		{
			if (this.triggers.Count == 0)
			{
				Debug.LogError("Triggers for event type " + this.id + " not defined.");
				return string.Empty;
			}
			int num = this.CalcWeightTotal();
			int num2 = global::UnityEngine.Random.Range(0, num + 1);
			int num3 = 0;
			foreach (AnimationTriggerData animationTriggerData in this.triggers)
			{
				if (num3 + animationTriggerData.weight >= num2)
				{
					return animationTriggerData.triggerName;
				}
				num3 += animationTriggerData.weight;
			}
			throw new Exception();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000EBA4 File Offset: 0x0000CDA4
		public int CalcWeightTotal()
		{
			int num = 0;
			foreach (AnimationTriggerData animationTriggerData in this.triggers)
			{
				num += animationTriggerData.weight;
			}
			return num;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000EBFC File Offset: 0x0000CDFC
		public void PlanStartPlayingTime()
		{
			this.planningTime = Time.time;
			this.nextTimeToPlay = this.time + global::UnityEngine.Random.Range(-this.deltaTime, this.deltaTime);
		}

		// Token: 0x040000C2 RID: 194
		public string id;

		// Token: 0x040000C3 RID: 195
		[Range(0f, 100f)]
		public float time;

		// Token: 0x040000C4 RID: 196
		[Range(0f, 100f)]
		public float deltaTime;

		// Token: 0x040000C5 RID: 197
		[HideInInspector]
		public float planningTime;

		// Token: 0x040000C6 RID: 198
		[HideInInspector]
		public float nextTimeToPlay;

		// Token: 0x040000C7 RID: 199
		public List<AnimationTriggerData> triggers;
	}
}
