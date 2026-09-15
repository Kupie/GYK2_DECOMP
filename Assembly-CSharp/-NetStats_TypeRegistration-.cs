using System;
using Unity.Multiplayer.Tools.NetStats;
using UnityEngine.Scripting;

// Token: 0x02000C69 RID: 3177
public class <NetStats_TypeRegistration>
{
	// Token: 0x060050C0 RID: 20672 RVA: 0x00180C07 File Offset: 0x0017EE07
	[Preserve]
	static void Run()
	{
		MetricIdTypeLibrary.RegisterType<CustomMetric>();
	}
}
