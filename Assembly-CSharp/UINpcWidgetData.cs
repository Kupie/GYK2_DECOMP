using System;
using LazyBearTechnology;

// Token: 0x02000897 RID: 2199
public class UINpcWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x0600388D RID: 14477 RVA: 0x0010FA54 File Offset: 0x0010DC54
	// (set) Token: 0x0600388E RID: 14478 RVA: 0x0010FA5C File Offset: 0x0010DC5C
	public string NpcId { get; set; }

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x0600388F RID: 14479 RVA: 0x0010FA65 File Offset: 0x0010DC65
	public WGODef WgoDef
	{
		get
		{
			return GameBalance.Me.GetData<WGODef>(this.NpcId);
		}
	}
}
