using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000B13 RID: 2835
[CreateAssetMenu(menuName = "GK2/WgoPartPoolInitialSizesConfig", fileName = "WgoPartPoolInitialSizesConfig")]
public class WgoPartPoolInitialSizesConfig : LazySingletonSO<WgoPartPoolInitialSizesConfig>
{
	// Token: 0x06004B76 RID: 19318 RVA: 0x00164C80 File Offset: 0x00162E80
	public int GetSizeForPath(string path)
	{
		int @int = this.initialSizeConfigs.GetInt(path);
		if (@int > 0)
		{
			return @int;
		}
		return 0;
	}

	// Token: 0x06004B77 RID: 19319 RVA: 0x00164CA1 File Offset: 0x00162EA1
	public void SetSizeForPath(string path, int size)
	{
		this.initialSizeConfigs.Set(path, (float)size);
	}

	// Token: 0x06004B78 RID: 19320 RVA: 0x00164CB1 File Offset: 0x00162EB1
	public void LogSizeForPath(string path)
	{
		Debug.Log(string.Format("Size for path:[{0}] is:[{1}]", path, this.GetSizeForPath(path)));
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x00164CCF File Offset: 0x00162ECF
	public void Clear()
	{
		this.initialSizeConfigs.Clear();
	}

	// Token: 0x04003CD5 RID: 15573
	public GameRes initialSizeConfigs = new GameRes();
}
