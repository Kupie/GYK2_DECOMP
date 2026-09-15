using System;
using LazyBearTechnology;

// Token: 0x0200086E RID: 2158
public class UIBuffElementData : LazyWidgetDataBase
{
	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x0600373A RID: 14138 RVA: 0x0010AC52 File Offset: 0x00108E52
	// (set) Token: 0x0600373B RID: 14139 RVA: 0x0010AC5A File Offset: 0x00108E5A
	public PerkData PerkData { get; private set; }

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x0600373C RID: 14140 RVA: 0x0010AC63 File Offset: 0x00108E63
	// (set) Token: 0x0600373D RID: 14141 RVA: 0x0010AC6B File Offset: 0x00108E6B
	public float Duration { get; private set; }

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x0600373E RID: 14142 RVA: 0x0010AC74 File Offset: 0x00108E74
	// (set) Token: 0x0600373F RID: 14143 RVA: 0x0010AC7C File Offset: 0x00108E7C
	public bool HasHiddenTimer { get; private set; }

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x06003740 RID: 14144 RVA: 0x0010AC85 File Offset: 0x00108E85
	// (set) Token: 0x06003741 RID: 14145 RVA: 0x0010AC8D File Offset: 0x00108E8D
	public bool IsInfinite { get; private set; }

	// Token: 0x06003742 RID: 14146 RVA: 0x0010AC96 File Offset: 0x00108E96
	public UIBuffElementData(PerkData perkData, float duration, bool hasHiddenTimer, bool isInfinite)
	{
		this.PerkData = perkData;
		this.Duration = duration;
		this.HasHiddenTimer = hasHiddenTimer;
		this.IsInfinite = isInfinite;
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x0010ACBB File Offset: 0x00108EBB
	public void UpdateData(float duration)
	{
		this.Duration = duration;
	}
}
