using System;

// Token: 0x0200001E RID: 30
public static class BoolExtensions
{
	// Token: 0x0600008D RID: 141 RVA: 0x00004454 File Offset: 0x00002654
	public static int ToInt(this bool value, int bit = 0)
	{
		if (!value)
		{
			return 0;
		}
		return 1 << bit;
	}
}
