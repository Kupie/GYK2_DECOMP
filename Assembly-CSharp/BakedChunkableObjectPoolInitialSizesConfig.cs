using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006E9 RID: 1769
[CreateAssetMenu(menuName = "GK2/BakedChunkableObjectPoolInitialSizesConfig", fileName = "BakedChunkableObjectPoolInitialSizesConfig")]
public class BakedChunkableObjectPoolInitialSizesConfig : LazySingletonSO<BakedChunkableObjectPoolInitialSizesConfig>
{
	// Token: 0x06002ECA RID: 11978 RVA: 0x000E000C File Offset: 0x000DE20C
	public int GetSizeForPath(string path)
	{
		int @int = this.initialSizeConfigs.GetInt(path);
		if (@int > 0)
		{
			return @int;
		}
		return 0;
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x000E002D File Offset: 0x000DE22D
	public void SetSizeForPath(string path, int size)
	{
		this.initialSizeConfigs.Set(path, (float)size);
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x000E003D File Offset: 0x000DE23D
	public void LogSizeForPath(string path)
	{
		Debug.Log(string.Format("Size for path:[{0}] is:[{1}]", path, this.GetSizeForPath(path)));
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x000E005B File Offset: 0x000DE25B
	public void Clear()
	{
		this.initialSizeConfigs.Clear();
	}

	// Token: 0x040025D1 RID: 9681
	public GameRes initialSizeConfigs = new GameRes();
}
