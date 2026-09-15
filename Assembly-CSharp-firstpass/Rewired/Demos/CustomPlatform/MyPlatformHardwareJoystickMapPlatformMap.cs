using System;
using System.Collections.Generic;
using Rewired.Data.Mapping;
using Rewired.Platforms.Custom;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x02000121 RID: 289
	public sealed class MyPlatformHardwareJoystickMapPlatformMap : HardwareJoystickMapCustomPlatformMapSO
	{
		// Token: 0x06000D4D RID: 3405 RVA: 0x000274F0 File Offset: 0x000256F0
		public override HardwareJoystickMap.Platform GetPlatformMap()
		{
			return this.platformMap;
		}

		// Token: 0x04000717 RID: 1815
		public MyPlatformHardwareJoystickMapPlatformMap.PlatformMap platformMap;

		// Token: 0x02000122 RID: 290
		[Serializable]
		public class PlatformMapBase : HardwareJoystickMapCustomPlatformMap<MyPlatformHardwareJoystickMapPlatformMap.MatchingCriteria>
		{
			// Token: 0x06000D4F RID: 3407 RVA: 0x00027500 File Offset: 0x00025700
			protected override object CreateInstance()
			{
				return new MyPlatformHardwareJoystickMapPlatformMap.PlatformMapBase();
			}
		}

		// Token: 0x02000123 RID: 291
		[Serializable]
		public sealed class PlatformMap : MyPlatformHardwareJoystickMapPlatformMap.PlatformMapBase
		{
			// Token: 0x06000D51 RID: 3409 RVA: 0x0002750F File Offset: 0x0002570F
			public override IList<HardwareJoystickMap.Platform> GetVariants()
			{
				return this.variants;
			}

			// Token: 0x06000D52 RID: 3410 RVA: 0x00027517 File Offset: 0x00025717
			protected override object CreateInstance()
			{
				return new MyPlatformHardwareJoystickMapPlatformMap.PlatformMap();
			}

			// Token: 0x04000718 RID: 1816
			public MyPlatformHardwareJoystickMapPlatformMap.PlatformMapBase[] variants;
		}

		// Token: 0x02000124 RID: 292
		[Serializable]
		public sealed class MatchingCriteria : HardwareJoystickMapCustomPlatformMap.MatchingCriteria
		{
			// Token: 0x06000D54 RID: 3412 RVA: 0x00027528 File Offset: 0x00025728
			public override bool Matches(object customIdentifier)
			{
				if (!(customIdentifier is MyPlatformControllerIdentifier))
				{
					return false;
				}
				MyPlatformControllerIdentifier myPlatformControllerIdentifier = (MyPlatformControllerIdentifier)customIdentifier;
				return (uint)myPlatformControllerIdentifier.productId == this.productId && (uint)myPlatformControllerIdentifier.vendorId == this.vendorId;
			}

			// Token: 0x06000D55 RID: 3413 RVA: 0x00027564 File Offset: 0x00025764
			protected override object CreateInstance()
			{
				return new MyPlatformHardwareJoystickMapPlatformMap.MatchingCriteria();
			}

			// Token: 0x06000D56 RID: 3414 RVA: 0x0002756B File Offset: 0x0002576B
			protected override void DeepClone(object destination)
			{
				base.DeepClone(destination);
				MyPlatformHardwareJoystickMapPlatformMap.MatchingCriteria matchingCriteria = (MyPlatformHardwareJoystickMapPlatformMap.MatchingCriteria)destination;
				matchingCriteria.vendorId = this.vendorId;
				matchingCriteria.productId = this.productId;
			}

			// Token: 0x04000719 RID: 1817
			public uint vendorId;

			// Token: 0x0400071A RID: 1818
			public uint productId;
		}
	}
}
