using System;
using LazyBearTechnology;

// Token: 0x02000233 RID: 563
[Serializable]
public class GameResIconType : Enumeration
{
	// Token: 0x06000D3B RID: 3387 RVA: 0x0004426F File Offset: 0x0004246F
	public GameResIconType(int value)
		: base(value)
	{
	}

	// Token: 0x04001062 RID: 4194
	public static GameResIconType Common = new GameResIconType(0);

	// Token: 0x04001063 RID: 4195
	public static GameResIconType MoneyBig = new GameResIconType(1);

	// Token: 0x04001064 RID: 4196
	public static GameResIconType TechPointSmall = new GameResIconType(2);
}
