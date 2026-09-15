using System;
using LazyBearTechnology;

// Token: 0x0200000C RID: 12
[Serializable]
public class GameKeyIconType : Enumeration
{
	// Token: 0x06000025 RID: 37 RVA: 0x00002B63 File Offset: 0x00000D63
	public GameKeyIconType(int value)
		: base(value)
	{
	}

	// Token: 0x0400003A RID: 58
	public static GameKeyIconType Default = new GameKeyIconType(0);

	// Token: 0x0400003B RID: 59
	public static GameKeyIconType Inactive = new GameKeyIconType(1);
}
