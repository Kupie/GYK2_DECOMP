using System;
using LazyBearTechnology;

// Token: 0x02000A36 RID: 2614
public class UISaveSlotsWindowLimitedPopUpData : LazyWidgetDataBase
{
	// Token: 0x17000ABE RID: 2750
	// (get) Token: 0x06004676 RID: 18038 RVA: 0x0014D9D4 File Offset: 0x0014BBD4
	public int TargetSlotIndex { get; }

	// Token: 0x17000ABF RID: 2751
	// (get) Token: 0x06004677 RID: 18039 RVA: 0x0014D9DC File Offset: 0x0014BBDC
	public Action OnImported { get; }

	// Token: 0x06004678 RID: 18040 RVA: 0x0014D9E4 File Offset: 0x0014BBE4
	public UISaveSlotsWindowLimitedPopUpData(int targetSlotIndex, Action onImported)
	{
		this.TargetSlotIndex = targetSlotIndex;
		this.OnImported = onImported;
	}
}
