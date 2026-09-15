using System;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000C68 RID: 3176
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__1221673671587648887
{
	// Token: 0x060050BE RID: 20670 RVA: 0x00180BC4 File Offset: 0x0017EDC4
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForExtensions.EarlyJobInit<ChunkDataIntersectsJob>();
			IJobParallelForExtensions.EarlyJobInit<ChunkVisibilityJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x060050BF RID: 20671 RVA: 0x00180C00 File Offset: 0x0017EE00
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__1221673671587648887.CreateJobReflectionData();
	}
}
