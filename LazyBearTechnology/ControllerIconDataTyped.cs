using System;

// Token: 0x0200000A RID: 10
[Serializable]
public class ControllerIconDataTyped
{
	// Token: 0x06000009 RID: 9 RVA: 0x00002112 File Offset: 0x00000312
	public ControllerIconDataTyped(GameKeyIconType iconType, string iconId)
	{
		this.iconType = iconType;
		this.iconId = iconId;
	}

	// Token: 0x04000028 RID: 40
	public GameKeyIconType iconType;

	// Token: 0x04000029 RID: 41
	public string iconId;
}
