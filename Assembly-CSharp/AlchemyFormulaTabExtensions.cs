using System;

// Token: 0x020001D8 RID: 472
public static class AlchemyFormulaTabExtensions
{
	// Token: 0x06000C00 RID: 3072 RVA: 0x0003C94A File Offset: 0x0003AB4A
	public static bool IsRuneTab(this AlchemyFormulaTab tab)
	{
		return tab == AlchemyFormulaTab.RedRune || tab == AlchemyFormulaTab.GreenRune || tab == AlchemyFormulaTab.BlueRune;
	}
}
