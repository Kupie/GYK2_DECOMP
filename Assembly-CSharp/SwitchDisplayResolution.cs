using System;

// Token: 0x02000793 RID: 1939
public static class SwitchDisplayResolution
{
	// Token: 0x060031E3 RID: 12771 RVA: 0x000EF358 File Offset: 0x000ED558
	public static bool TryGet(out int width, out int height)
	{
		width = 0;
		height = 0;
		return false;
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x00002318 File Offset: 0x00000518
	public static void InitChangeEvent()
	{
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x00028294 File Offset: 0x00026494
	public static bool TryConsumeChange()
	{
		return false;
	}
}
