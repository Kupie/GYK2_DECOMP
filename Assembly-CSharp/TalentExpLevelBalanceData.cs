using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200020B RID: 523
[Serializable]
public class TalentExpLevelBalanceData
{
	// Token: 0x06000CAE RID: 3246 RVA: 0x0003FDCB File Offset: 0x0003DFCB
	public TalentExpLevelBalanceData()
	{
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0003FDDE File Offset: 0x0003DFDE
	public TalentExpLevelBalanceData(string id)
	{
		this.id = id;
		this.expLevels = new List<int>();
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0003FE03 File Offset: 0x0003E003
	public int GetExpForLevel(int level)
	{
		if (!this.HasLevelCorrectValue(level))
		{
			return 0;
		}
		return this.expLevels[level - 1];
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0003FE1E File Offset: 0x0003E01E
	public int GetExpForNextLevel(int curLevel)
	{
		if (!this.HasLevelCorrectValue(curLevel))
		{
			return 0;
		}
		return this.expLevels[curLevel];
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x0003FE37 File Offset: 0x0003E037
	private bool HasLevelCorrectValue(int level)
	{
		if (level < 0 || level >= this.expLevels.Count)
		{
			Debug.LogError(string.Format("Talent [{0}]: level [{1}] is out of range", this.id, level));
			return false;
		}
		return true;
	}

	// Token: 0x04000EF1 RID: 3825
	public string id;

	// Token: 0x04000EF2 RID: 3826
	public List<int> expLevels = new List<int>();
}
