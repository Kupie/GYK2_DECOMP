using System;
using System.Collections.Generic;

// Token: 0x02000AAB RID: 2731
[Serializable]
public class ModelsBySubgroup
{
	// Token: 0x060049DE RID: 18910 RVA: 0x0015CDDE File Offset: 0x0015AFDE
	public ModelsBySubgroup(string subgroupName, List<string> modelsNames)
	{
		this.subgroupName = subgroupName;
		this.modelsNames = modelsNames;
	}

	// Token: 0x060049DF RID: 18911 RVA: 0x0015CE0C File Offset: 0x0015B00C
	public void InitAvailableGroups(ref List<string> allAvailableGroups)
	{
		for (int i = 0; i < this.modelsNames.Count; i++)
		{
			string[] array = this.modelsNames[i].Split('-', StringSplitOptions.None);
			if (!this.availableGroups.Contains(array[1]))
			{
				this.availableGroups.Add(array[1]);
			}
			allAvailableGroups.Remove(array[1]);
		}
	}

	// Token: 0x040039A2 RID: 14754
	public string subgroupName;

	// Token: 0x040039A3 RID: 14755
	public List<string> modelsNames = new List<string>();

	// Token: 0x040039A4 RID: 14756
	public List<string> availableGroups = new List<string>();
}
