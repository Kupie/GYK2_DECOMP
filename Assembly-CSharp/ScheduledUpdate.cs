using System;
using System.Collections.Generic;

// Token: 0x0200070B RID: 1803
[Serializable]
public class ScheduledUpdate
{
	// Token: 0x06002F7D RID: 12157 RVA: 0x000E3C3F File Offset: 0x000E1E3F
	public ScheduledUpdate(ICustomUpdatable updatable, float updateInterval)
	{
		this.updateInterval = updateInterval;
		this.customUpdatables.Add(updatable);
	}

	// Token: 0x06002F7E RID: 12158 RVA: 0x000E3C65 File Offset: 0x000E1E65
	public ScheduledUpdate(List<ICustomUpdatable> updatables, float updateInterval)
	{
		this.updateInterval = updateInterval;
		this.customUpdatables.AddRange(updatables);
	}

	// Token: 0x06002F7F RID: 12159 RVA: 0x000E3C8C File Offset: 0x000E1E8C
	public void CallUpdate(float deltaTime, bool applyTimeMultiplier = true)
	{
		if (applyTimeMultiplier)
		{
			deltaTime *= MainGame.UpdateManager.TimeMultiplier;
		}
		foreach (ICustomUpdatable customUpdatable in this.customUpdatables)
		{
			customUpdatable.CustomUpdate(deltaTime);
		}
	}

	// Token: 0x0400265F RID: 9823
	public float updateInterval;

	// Token: 0x04002660 RID: 9824
	public List<ICustomUpdatable> customUpdatables = new List<ICustomUpdatable>();

	// Token: 0x04002661 RID: 9825
	public float accumulatedTime;
}
