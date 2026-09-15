using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000535 RID: 1333
[CreateAssetMenu(menuName = "GK2/WorldFXPoolInitialSizeConfig", fileName = "WorldFXPoolInitialSizeConfig")]
public class WorldFXPoolInitialSizeConfig : LazySingletonSO<WorldFXPoolInitialSizeConfig>
{
	// Token: 0x06002253 RID: 8787 RVA: 0x000A13D0 File Offset: 0x0009F5D0
	public int GetSizeForName(string fxName)
	{
		int @int = this.initialSizeConfigs.GetInt(fxName);
		if (@int > 0)
		{
			return @int;
		}
		return 0;
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000A13F1 File Offset: 0x0009F5F1
	public void SetSizeForName(string fxName, int size)
	{
		this.initialSizeConfigs.Set(fxName, (float)size);
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000A1401 File Offset: 0x0009F601
	public void LogSizeForName(string fxName)
	{
		Debug.Log(string.Format("Size for fxName:[{0}] is:[{1}]", fxName, this.GetSizeForName(fxName)));
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000A141F File Offset: 0x0009F61F
	public void Clear()
	{
		this.initialSizeConfigs.Clear();
	}

	// Token: 0x04001EDE RID: 7902
	public GameRes initialSizeConfigs = new GameRes();
}
