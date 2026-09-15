using System;
using System.Collections.Generic;
using Rewired.Data.Mapping;
using Rewired.Platforms.Custom;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x0200011A RID: 282
	[Serializable]
	public class CustomPlatformHardwareJoystickMapProvider : IHardwareJoystickMapCustomPlatformMapProvider
	{
		// Token: 0x06000D3B RID: 3387 RVA: 0x000272D8 File Offset: 0x000254D8
		public HardwareJoystickMap.Platform GetPlatformMap(int customPlatformId, Guid hardwareTypeGuid)
		{
			CustomPlatformHardwareJoystickMapPlatformDataSet platformDataSet = this.GetPlatformDataSet(customPlatformId);
			if (platformDataSet == null)
			{
				return null;
			}
			return CustomPlatformHardwareJoystickMapProvider.GetPlatformMap(platformDataSet, hardwareTypeGuid);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00027300 File Offset: 0x00025500
		private CustomPlatformHardwareJoystickMapPlatformDataSet GetPlatformDataSet(int customPlatformId)
		{
			int count = this.platformJoystickDataSets.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.platformJoystickDataSets[i] != null && this.platformJoystickDataSets[i].platformType == (CustomPlatformType)customPlatformId)
				{
					return this.platformJoystickDataSets[i].dataSet;
				}
			}
			return null;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0002735C File Offset: 0x0002555C
		private static HardwareJoystickMap.Platform GetPlatformMap(CustomPlatformHardwareJoystickMapPlatformDataSet platformDataSet, Guid hardwareTypeGuid)
		{
			if (platformDataSet == null || platformDataSet.platformMaps == null)
			{
				return null;
			}
			int count = platformDataSet.platformMaps.Count;
			for (int i = 0; i < count; i++)
			{
				if (platformDataSet.platformMaps[i] != null && platformDataSet.platformMaps[i].Matches(hardwareTypeGuid))
				{
					return platformDataSet.platformMaps[i].GetPlatformMap();
				}
			}
			return null;
		}

		// Token: 0x0400070E RID: 1806
		public List<CustomPlatformHardwareJoystickMapProvider.PlatformDataSet> platformJoystickDataSets;

		// Token: 0x0200011B RID: 283
		[Serializable]
		public class PlatformDataSet
		{
			// Token: 0x0400070F RID: 1807
			public CustomPlatformType platformType;

			// Token: 0x04000710 RID: 1808
			public CustomPlatformHardwareJoystickMapPlatformDataSet dataSet;
		}
	}
}
