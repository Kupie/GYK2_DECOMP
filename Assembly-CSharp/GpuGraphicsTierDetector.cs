using System;
using UnityEngine;

// Token: 0x0200077B RID: 1915
public static class GpuGraphicsTierDetector
{
	// Token: 0x060031A5 RID: 12709 RVA: 0x000EDF1C File Offset: 0x000EC11C
	public static GraphicsTier DetectDefaultGraphicsTier()
	{
		string graphicsDeviceName = SystemInfo.graphicsDeviceName;
		int graphicsDeviceVendorID = SystemInfo.graphicsDeviceVendorID;
		int graphicsDeviceID = SystemInfo.graphicsDeviceID;
		Debug.Log(string.Concat(new string[]
		{
			"[GpuGraphicsTierDetector] SystemInfo: name='",
			graphicsDeviceName,
			"', vendor='",
			SystemInfo.graphicsDeviceVendor,
			"', ",
			string.Format("vendorId=0x{0:X4}, deviceId=0x{1:X4}, type={2}, ", graphicsDeviceVendorID, graphicsDeviceID, SystemInfo.graphicsDeviceType),
			string.Format("vram={0}MB, version='{1}'", SystemInfo.graphicsMemorySize, SystemInfo.graphicsDeviceVersion)
		}));
		int benchmarkPoints = GpuBenchmarkDatabase.GetBenchmarkPoints("GeForce RTX 3060", 0, 0);
		if (benchmarkPoints <= 0)
		{
			Debug.LogError(string.Format("{0} Reference GPU '{1}' not found in the benchmark database. Falling back to {2}.", "[GpuGraphicsTierDetector]", "GeForce RTX 3060", GraphicsTier.High));
			return GraphicsTier.High;
		}
		int benchmarkPoints2 = GpuBenchmarkDatabase.GetBenchmarkPoints(graphicsDeviceName, graphicsDeviceVendorID, graphicsDeviceID);
		if (benchmarkPoints2 < 0)
		{
			Debug.Log(string.Format("{0} GPU '{1}' (0x{2:X4}:0x{3:X4}) not found in the benchmark database. Falling back to {4}.", new object[]
			{
				"[GpuGraphicsTierDetector]",
				graphicsDeviceName,
				graphicsDeviceVendorID,
				graphicsDeviceID,
				GraphicsTier.High
			}));
			return GraphicsTier.High;
		}
		GraphicsTier graphicsTier = GpuGraphicsTierDetector.ResolveTier(benchmarkPoints2, benchmarkPoints);
		Debug.Log(string.Format("{0} GPU '{1}' found: {2} points. Reference '{3}': {4} points ", new object[] { "[GpuGraphicsTierDetector]", graphicsDeviceName, benchmarkPoints2, "GeForce RTX 3060", benchmarkPoints }) + string.Format("(ratio {0:0.00}). Thresholds: High>={1}, ", (float)benchmarkPoints2 / (float)benchmarkPoints, benchmarkPoints) + string.Format("Medium>={0}, Low>={1}. ", Mathf.RoundToInt((float)benchmarkPoints * 0.5f), Mathf.RoundToInt((float)benchmarkPoints * 0.3f)) + string.Format("Selected tier: {0}", graphicsTier));
		return graphicsTier;
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x000EE0D4 File Offset: 0x000EC2D4
	public static GraphicsTier ResolveTier(int points, int referencePoints)
	{
		if (points >= referencePoints)
		{
			return GraphicsTier.High;
		}
		if ((float)points >= (float)referencePoints * 0.5f)
		{
			return GraphicsTier.Medium;
		}
		if ((float)points >= (float)referencePoints * 0.3f)
		{
			return GraphicsTier.Low;
		}
		return GraphicsTier.Lowest;
	}

	// Token: 0x040027AC RID: 10156
	private const string LOG_PREFIX = "[GpuGraphicsTierDetector]";

	// Token: 0x040027AD RID: 10157
	public const string HIGH_REFERENCE_GPU_NAME = "GeForce RTX 3060";

	// Token: 0x040027AE RID: 10158
	private const float MEDIUM_TIER_FRACTION = 0.5f;

	// Token: 0x040027AF RID: 10159
	private const float LOW_TIER_FRACTION = 0.3f;
}
