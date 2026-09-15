using System;
using LazyBearTechnology;

// Token: 0x020009F1 RID: 2545
public class UIItemCountWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x06004492 RID: 17554 RVA: 0x00145801 File Offset: 0x00143A01
	// (set) Token: 0x06004493 RID: 17555 RVA: 0x00145809 File Offset: 0x00143A09
	public Item Item { get; set; }

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x06004494 RID: 17556 RVA: 0x00145812 File Offset: 0x00143A12
	// (set) Token: 0x06004495 RID: 17557 RVA: 0x0014581A File Offset: 0x00143A1A
	public int Min { get; set; }

	// Token: 0x17000A72 RID: 2674
	// (get) Token: 0x06004496 RID: 17558 RVA: 0x00145823 File Offset: 0x00143A23
	// (set) Token: 0x06004497 RID: 17559 RVA: 0x0014582B File Offset: 0x00143A2B
	public int Max { get; set; }

	// Token: 0x17000A73 RID: 2675
	// (get) Token: 0x06004498 RID: 17560 RVA: 0x00145834 File Offset: 0x00143A34
	// (set) Token: 0x06004499 RID: 17561 RVA: 0x0014583C File Offset: 0x00143A3C
	public int SnapStep { get; set; } = 1;

	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x0600449A RID: 17562 RVA: 0x00145845 File Offset: 0x00143A45
	// (set) Token: 0x0600449B RID: 17563 RVA: 0x0014584D File Offset: 0x00143A4D
	public Action<int> OnConfirm { get; set; }

	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x0600449C RID: 17564 RVA: 0x00145856 File Offset: 0x00143A56
	// (set) Token: 0x0600449D RID: 17565 RVA: 0x0014585E File Offset: 0x00143A5E
	public UIItemCountWindowData.PriceCalculateDelegate PriceCalculateDel { get; set; }

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x0600449E RID: 17566 RVA: 0x00145867 File Offset: 0x00143A67
	// (set) Token: 0x0600449F RID: 17567 RVA: 0x0014586F File Offset: 0x00143A6F
	public bool IsForVendor { get; set; }

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x060044A0 RID: 17568 RVA: 0x00145878 File Offset: 0x00143A78
	// (set) Token: 0x060044A1 RID: 17569 RVA: 0x00145880 File Offset: 0x00143A80
	public UIDialogWindowData.ButtonData OkBtnData { get; set; }

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x060044A2 RID: 17570 RVA: 0x00145889 File Offset: 0x00143A89
	// (set) Token: 0x060044A3 RID: 17571 RVA: 0x00145891 File Offset: 0x00143A91
	public UIDialogWindowData.ButtonData BackBtnData { get; set; }

	// Token: 0x020009F2 RID: 2546
	// (Invoke) Token: 0x060044A6 RID: 17574
	public delegate int PriceCalculateDelegate(int amount);
}
