using System;
using Unity.Multiplayer.Tools.NetStats;

// Token: 0x02000774 RID: 1908
[MetricTypeEnum(DisplayName = "CustomMetric")]
internal enum CustomMetric
{
	// Token: 0x04002799 RID: 10137
	[MetricMetadata(Units = Units.None, MetricKind = MetricKind.Counter)]
	LogicPackagesSent,
	// Token: 0x0400279A RID: 10138
	[MetricMetadata(Units = Units.None, MetricKind = MetricKind.Counter)]
	LogicPackagesReceived,
	// Token: 0x0400279B RID: 10139
	[MetricMetadata(Units = Units.None, MetricKind = MetricKind.Counter)]
	AveragePackagesSent,
	// Token: 0x0400279C RID: 10140
	[MetricMetadata(Units = Units.None, MetricKind = MetricKind.Counter)]
	AveragePackagesReceived
}
