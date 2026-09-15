using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007C1 RID: 1985
[Serializable]
public class EnvironmentControlBehaviour : PlayableBehaviour
{
	// Token: 0x0600330A RID: 13066 RVA: 0x000F5F50 File Offset: 0x000F4150
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (EnvironmentEngine.Instance == null)
		{
			return;
		}
		if (!this.isInitialStateSaved)
		{
			this.isInitialStateSaved = true;
			this.initialTimeOfDay = EnvironmentEngine.Instance.timeOfDay;
		}
		if (this.setTime)
		{
			EnvironmentEngine.Instance.SetTimeOfDay(this.timeOfDay);
		}
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x000F5FA2 File Offset: 0x000F41A2
	public override void OnPlayableDestroy(Playable playable)
	{
		if (Application.isPlaying || EnvironmentEngine.Instance == null || !this.isInitialStateSaved)
		{
			return;
		}
		EnvironmentEngine.Instance.SetTimeOfDay(this.initialTimeOfDay);
		this.isInitialStateSaved = false;
	}

	// Token: 0x040028DC RID: 10460
	public bool setTime;

	// Token: 0x040028DD RID: 10461
	[Range(0f, 1f)]
	public float timeOfDay;

	// Token: 0x040028DE RID: 10462
	private bool isInitialStateSaved;

	// Token: 0x040028DF RID: 10463
	private float initialTimeOfDay;
}
