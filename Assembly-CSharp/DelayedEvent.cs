using System;

// Token: 0x02000584 RID: 1412
[Serializable]
public class DelayedEvent
{
	// Token: 0x0600242B RID: 9259 RVA: 0x000AA4D0 File Offset: 0x000A86D0
	public DelayedEvent(string eventName, float delayTime)
	{
		this.eventName = eventName;
		this.delayTime = delayTime;
	}

	// Token: 0x0400201F RID: 8223
	public string eventName;

	// Token: 0x04002020 RID: 8224
	public float delayTime;
}
