using System;

// Token: 0x0200085A RID: 2138
public static class CursorUtils
{
	// Token: 0x060036C8 RID: 14024 RVA: 0x0010972E File Offset: 0x0010792E
	public static void AddCursorState(this ICursorChanger changer, CursorType type)
	{
		CursorController.AddCursorState(type, changer);
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x00109737 File Offset: 0x00107937
	public static void RemoveCursorState(this ICursorChanger changer)
	{
		CursorController.RemoveCursorState(changer);
	}
}
