using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000116 RID: 278
	[DisallowMultipleComponent]
	public class GogGalaxyManager : LazySingleton<GogGalaxyManager>
	{
		// Token: 0x0400029D RID: 669
		[SerializeField]
		private string clientID;

		// Token: 0x0400029E RID: 670
		[SerializeField]
		private string clientSecret;
	}
}
