using System;
using UnityEngine;

// Token: 0x02000AAE RID: 2734
public class PreviewFormatIntegerAttribute : PropertyAttribute
{
	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x060049E6 RID: 18918 RVA: 0x0015D12D File Offset: 0x0015B32D
	// (set) Token: 0x060049E7 RID: 18919 RVA: 0x0015D135 File Offset: 0x0015B335
	public string PreviewFormatInteger { get; private set; }

	// Token: 0x060049E8 RID: 18920 RVA: 0x0015D13E File Offset: 0x0015B33E
	public PreviewFormatIntegerAttribute(string format)
	{
		this.PreviewFormatInteger = format;
	}
}
