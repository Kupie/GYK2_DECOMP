using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000AA5 RID: 2725
[CreateAssetMenu(menuName = "GK2/ConstructorPartPoolInitialSizesConfig", fileName = "ConstructorPartPoolInitialSizesConfig")]
public class ConstructorPartPoolInitialSizesConfig : LazySingletonSO<ConstructorPartPoolInitialSizesConfig>
{
	// Token: 0x060049C2 RID: 18882 RVA: 0x0015C6E8 File Offset: 0x0015A8E8
	public int GetSizeForPath(string path)
	{
		int @int = this.initialSizeConfigs.GetInt(path);
		if (@int > 0)
		{
			return @int;
		}
		return 0;
	}

	// Token: 0x060049C3 RID: 18883 RVA: 0x0015C709 File Offset: 0x0015A909
	public void SetSizeForPath(string path, int size)
	{
		this.initialSizeConfigs.Set(path, (float)size);
	}

	// Token: 0x060049C4 RID: 18884 RVA: 0x0015C719 File Offset: 0x0015A919
	public void LogSizeForPath(string path)
	{
		Debug.Log(string.Format("Size for path:[{0}] is:[{1}]", path, this.GetSizeForPath(path)));
	}

	// Token: 0x060049C5 RID: 18885 RVA: 0x0015C737 File Offset: 0x0015A937
	public void Clear()
	{
		this.initialSizeConfigs.Clear();
	}

	// Token: 0x0400398C RID: 14732
	public GameRes initialSizeConfigs = new GameRes();
}
