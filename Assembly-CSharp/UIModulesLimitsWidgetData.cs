using System;
using LazyBearTechnology;

// Token: 0x020007F3 RID: 2035
public class UIModulesLimitsWidgetData : LazyWidgetDataBase
{
	// Token: 0x06003443 RID: 13379 RVA: 0x000F93E5 File Offset: 0x000F75E5
	public UIModulesLimitsWidgetData()
	{
	}

	// Token: 0x06003444 RID: 13380 RVA: 0x000FBC7A File Offset: 0x000F9E7A
	public UIModulesLimitsWidgetData(int modulesCount, int modulesLimit)
	{
		this.ModulesCount = modulesCount;
		this.ModulesLimit = modulesLimit;
	}

	// Token: 0x06003445 RID: 13381 RVA: 0x000FBC90 File Offset: 0x000F9E90
	public UIModulesLimitsWidgetData(BuildingDef buildingDef)
	{
		this.ModulesCount = buildingDef.currentLimitExpression.EvaluateInt();
		this.ModulesLimit = buildingDef.limitMax;
	}

	// Token: 0x040029BE RID: 10686
	public int ModulesLimit;

	// Token: 0x040029BF RID: 10687
	public int ModulesCount;
}
