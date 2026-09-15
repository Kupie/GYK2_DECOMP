using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000DC RID: 220
	[CreateAssetMenu(menuName = "Lazy/LazyApplicationSettings", fileName = "LazyApplicationSettings")]
	public class LazyApplicationSettings : LazySingletonSO<LazyApplicationSettings>
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x00014964 File Offset: 0x00012B64
		public static ApplicationSettings GetCurrentSetting()
		{
			if (LazySingletonSO<LazyApplicationSettings>.Instance == null)
			{
				return null;
			}
			if (LazySingletonSO<LazyApplicationSettings>.Instance.settingsList.Count == 0)
			{
				return null;
			}
			if (LazySingletonSO<LazyApplicationSettings>.Instance.settingsList.Count == 1)
			{
				LazySingletonSO<LazyApplicationSettings>.Instance.current = LazySingletonSO<LazyApplicationSettings>.Instance.settingsList[0].buildType;
			}
			return LazySingletonSO<LazyApplicationSettings>.Instance.settingsList.Find((ApplicationSettings s) => s.buildType == LazySingletonSO<LazyApplicationSettings>.Instance.current);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000149F4 File Offset: 0x00012BF4
		public static ApplicationSettings GetSettingsByBuildType(int buildType)
		{
			if (LazySingletonSO<LazyApplicationSettings>.Instance == null)
			{
				return null;
			}
			if (LazySingletonSO<LazyApplicationSettings>.Instance.settingsList.Count == 0)
			{
				return null;
			}
			return LazySingletonSO<LazyApplicationSettings>.Instance.settingsList.Find((ApplicationSettings s) => s.buildType.value == buildType);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00014A4C File Offset: 0x00012C4C
		public static List<string> GetExclusiveDefinesExceptCurrent()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < LazySingletonSO<LazyApplicationSettings>.Instance.settingsList.Count; i++)
			{
				if (!(LazySingletonSO<LazyApplicationSettings>.Instance.settingsList[i].buildType == LazySingletonSO<LazyApplicationSettings>.Instance.current))
				{
					list.AddRange(LazySingletonSO<LazyApplicationSettings>.Instance.settingsList[i].exclusiveDefines);
				}
			}
			return list;
		}

		// Token: 0x040001C9 RID: 457
		[SerializeField]
		private LazyBuildType current;

		// Token: 0x040001CA RID: 458
		[SerializeField]
		[Tooltip("If this list is empty, all settings will be taken from PlayerSettings")]
		private List<ApplicationSettings> settingsList = new List<ApplicationSettings>();
	}
}
