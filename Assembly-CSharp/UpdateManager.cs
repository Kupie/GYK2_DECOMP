using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200070A RID: 1802
public class UpdateManager : MonoBehaviour
{
	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x06002F76 RID: 12150 RVA: 0x000E3B15 File Offset: 0x000E1D15
	// (set) Token: 0x06002F77 RID: 12151 RVA: 0x000E3B1D File Offset: 0x000E1D1D
	public bool IsActive { get; set; }

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x06002F78 RID: 12152 RVA: 0x000E3B26 File Offset: 0x000E1D26
	public float TimeMultiplier
	{
		get
		{
			return this.timeMultiplier;
		}
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x000E3B2E File Offset: 0x000E1D2E
	public void AddScheduledUpdate(ScheduledUpdate scheduledUpdate)
	{
		if (this.scheduledUpdates.Contains(scheduledUpdate))
		{
			Debug.LogWarning(string.Format("Already added scheduled update: {0}", scheduledUpdate));
			return;
		}
		this.scheduledUpdates.Add(scheduledUpdate);
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x000E3B5C File Offset: 0x000E1D5C
	private void Update()
	{
		if (!this.IsActive)
		{
			return;
		}
		float num = this.timeMultiplier;
		foreach (ScheduledUpdate scheduledUpdate in this.scheduledUpdates)
		{
			if (scheduledUpdate.updateInterval > 0f)
			{
				scheduledUpdate.accumulatedTime += Time.deltaTime * num;
				while (scheduledUpdate.accumulatedTime >= scheduledUpdate.updateInterval)
				{
					scheduledUpdate.CallUpdate(scheduledUpdate.updateInterval, false);
					scheduledUpdate.accumulatedTime -= scheduledUpdate.updateInterval;
				}
			}
			else
			{
				scheduledUpdate.CallUpdate(Time.deltaTime, true);
			}
		}
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x000E3C18 File Offset: 0x000E1E18
	public void SetTimeSpeedMultiplier(float value)
	{
		this.timeMultiplier = value;
	}

	// Token: 0x0400265C RID: 9820
	[SerializeField]
	private List<ScheduledUpdate> scheduledUpdates = new List<ScheduledUpdate>();

	// Token: 0x0400265E RID: 9822
	private float timeMultiplier = 1f;
}
