using System;
using Unity.Multiplayer.Tools.NetStats;
using Unity.Multiplayer.Tools.NetStatsMonitor;

// Token: 0x02000775 RID: 1909
public class CustomNetStats
{
	// Token: 0x0600318E RID: 12686 RVA: 0x00021B94 File Offset: 0x0001FD94
	public CustomNetStats()
	{
	}

	// Token: 0x0600318F RID: 12687 RVA: 0x000EABBF File Offset: 0x000E8DBF
	public CustomNetStats(RuntimeNetStatsMonitor statsMonitor)
	{
		this.statsMonitor = statsMonitor;
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x000EABD0 File Offset: 0x000E8DD0
	public void UpdatePackageStats(int sentPackages, int receivedPackages)
	{
		if (sentPackages != 0)
		{
			this.statsMonitor.AddCustomValue(MetricId.Create<CustomMetric>(CustomMetric.LogicPackagesSent), (float)sentPackages);
		}
		if (receivedPackages != 0)
		{
			this.statsMonitor.AddCustomValue(MetricId.Create<CustomMetric>(CustomMetric.LogicPackagesReceived), (float)receivedPackages);
		}
		this.statsMonitor.AddCustomValue(MetricId.Create<CustomMetric>(CustomMetric.AveragePackagesSent), (float)sentPackages);
		this.statsMonitor.AddCustomValue(MetricId.Create<CustomMetric>(CustomMetric.AveragePackagesReceived), (float)receivedPackages);
	}

	// Token: 0x0400279D RID: 10141
	private RuntimeNetStatsMonitor statsMonitor;
}
