using System;
using UnityEngine;

// Token: 0x02000589 RID: 1417
[Serializable]
public class FightingLevelData
{
	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06002444 RID: 9284 RVA: 0x000AA7D4 File Offset: 0x000A89D4
	// (remove) Token: 0x06002445 RID: 9285 RVA: 0x000AA808 File Offset: 0x000A8A08
	public static event Action<FightingLevelData> OnFightingLevelDataChanged;

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06002446 RID: 9286 RVA: 0x000AA83B File Offset: 0x000A8A3B
	// (set) Token: 0x06002447 RID: 9287 RVA: 0x000AA843 File Offset: 0x000A8A43
	public int CurStageId
	{
		get
		{
			return this.curStageId;
		}
		set
		{
			this.curStageId = value;
			Action<FightingLevelData> onFightingLevelDataChanged = FightingLevelData.OnFightingLevelDataChanged;
			if (onFightingLevelDataChanged == null)
			{
				return;
			}
			onFightingLevelDataChanged(this);
		}
	}

	// Token: 0x06002448 RID: 9288 RVA: 0x00021B94 File Offset: 0x0001FD94
	public FightingLevelData()
	{
	}

	// Token: 0x06002449 RID: 9289 RVA: 0x000AA85C File Offset: 0x000A8A5C
	public FightingLevelData(string id)
	{
		this.id = id;
	}

	// Token: 0x04002034 RID: 8244
	public string id;

	// Token: 0x04002035 RID: 8245
	[SerializeField]
	private int curStageId;
}
