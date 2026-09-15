using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000135 RID: 309
	[CreateAssetMenu(fileName = "GamepadTypeData", menuName = "Lazy/GamepadTypeData", order = 1)]
	public class GamepadTypeData : LazySingletonSO<GamepadTypeData>
	{
		// Token: 0x06000603 RID: 1539 RVA: 0x0001ED7C File Offset: 0x0001CF7C
		public GamepadType GetTypeByGuid(Guid guid)
		{
			for (int i = 0; i < this.configurations.Length; i++)
			{
				for (int j = 0; j < this.configurations[i].maps.Length; j++)
				{
					if (this.configurations[i].maps[j].Guid == guid)
					{
						return this.configurations[i].gamepadType;
					}
				}
			}
			return GamepadTypeData.GetDefaultTypeForPlatform();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001EDE5 File Offset: 0x0001CFE5
		private static GamepadType GetDefaultTypeForPlatform()
		{
			return GamepadType.Xbox_XboxController;
		}

		// Token: 0x0400037E RID: 894
		public GamepadTypeConfiguration[] configurations = new GamepadTypeConfiguration[]
		{
			new GamepadTypeConfiguration
			{
				gamepadType = GamepadType.Xbox_XboxController
			},
			new GamepadTypeConfiguration
			{
				gamepadType = GamepadType.Sony_DualShock
			}
		};
	}
}
