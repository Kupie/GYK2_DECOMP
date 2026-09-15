using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020003B5 RID: 949
[CreateAssetMenu(fileName = "StartReses", menuName = "GK2/Start Reses")]
public class StartReses : ScriptableObject
{
	// Token: 0x06001997 RID: 6551 RVA: 0x00078F9C File Offset: 0x0007719C
	public static StartReses Load()
	{
		return Addressables.LoadAssetAsync<StartReses>("StartReses/PlayerStartState.asset").WaitForCompletion();
	}

	// Token: 0x040018D6 RID: 6358
	private const string RESOURCE_FOLDER = "StartReses";

	// Token: 0x040018D7 RID: 6359
	public List<StartReses.StartItemData> startItems = new List<StartReses.StartItemData>();

	// Token: 0x040018D8 RID: 6360
	public GameRes startGameRes = new GameRes();

	// Token: 0x040018D9 RID: 6361
	public GameResStr startGameResStr = new GameResStr();

	// Token: 0x020003B6 RID: 950
	[Serializable]
	public class StartItemData
	{
		// Token: 0x040018DA RID: 6362
		public string id;

		// Token: 0x040018DB RID: 6363
		public int count;

		// Token: 0x040018DC RID: 6364
		public bool shouldBeEquipped;
	}
}
