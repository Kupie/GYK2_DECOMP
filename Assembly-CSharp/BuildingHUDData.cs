using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000879 RID: 2169
public class BuildingHUDData : LazyWidgetDataBase
{
	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x06003776 RID: 14198 RVA: 0x0010BA51 File Offset: 0x00109C51
	public IReadOnlyList<DockPoint> DockPoints
	{
		get
		{
			return this.dockPoints;
		}
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x0010BA59 File Offset: 0x00109C59
	public BuildingHUDData(IReadOnlyList<DockPoint> targetDockPoints = null, bool hasRotation = false)
	{
		this.isTargetCanBeRotated = hasRotation;
		if (targetDockPoints != null && targetDockPoints.Count > 0)
		{
			this.dockPoints = targetDockPoints;
		}
	}

	// Token: 0x04002C20 RID: 11296
	private string buildHintText;

	// Token: 0x04002C21 RID: 11297
	private string rotationHintText;

	// Token: 0x04002C22 RID: 11298
	private string exitHintText;

	// Token: 0x04002C23 RID: 11299
	private IReadOnlyList<DockPoint> dockPoints = new List<DockPoint>();

	// Token: 0x04002C24 RID: 11300
	public bool isTargetCanBeRotated;
}
