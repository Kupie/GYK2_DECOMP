using System;
using LazyBearTechnology;

// Token: 0x020007E8 RID: 2024
public class ProgressBarWidget_SimplifiedData : LazyWidgetDataBase
{
	// Token: 0x06003415 RID: 13333 RVA: 0x000FB3A7 File Offset: 0x000F95A7
	public ProgressBarWidget_SimplifiedData(int cellCount, int greenValue, int redValue = 0, CraftDef craftDef = null)
	{
		this.CellCount = cellCount;
		this.GreenValue = greenValue;
		this.RedValue = redValue;
		this.CraftDef = craftDef;
	}

	// Token: 0x04002993 RID: 10643
	public int CellCount;

	// Token: 0x04002994 RID: 10644
	public int GreenValue;

	// Token: 0x04002995 RID: 10645
	public int RedValue;

	// Token: 0x04002996 RID: 10646
	public CraftDef CraftDef;
}
