using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000C2 RID: 194
	[Serializable]
	public class AnimationTriggerData
	{
		// Token: 0x040000C8 RID: 200
		public string triggerName;

		// Token: 0x040000C9 RID: 201
		[Range(1f, 100f)]
		public int weight;
	}
}
