using System;
using LazyBearTechnology;

// Token: 0x02000009 RID: 9
[Serializable]
public class ControllerIconData
{
	// Token: 0x06000008 RID: 8 RVA: 0x000020FC File Offset: 0x000002FC
	public ControllerIconData(GameKey gameKey, ControllerIconDataTyped[] typedIcons)
	{
		this.gameKey = gameKey;
		this.typedIcons = typedIcons;
	}

	// Token: 0x04000026 RID: 38
	public GameKey gameKey;

	// Token: 0x04000027 RID: 39
	public ControllerIconDataTyped[] typedIcons;
}
