using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x0200032A RID: 810
[CreateAssetMenu(fileName = "GenericFightersPresets", menuName = "GK2/GenericFightersPresets")]
public class GenericFightersPresets : ScriptableObject
{
	// Token: 0x060015A6 RID: 5542 RVA: 0x00069770 File Offset: 0x00067970
	public static AsyncOperationHandle TryLoad()
	{
		return Addressables.LoadAssetAsync<GenericFightersPresets>("Fighting/GenericFightersPresets.asset");
	}

	// Token: 0x0400161E RID: 5662
	private const string PRESETS_FOLDER = "Fighting";

	// Token: 0x0400161F RID: 5663
	public List<GenericFightersPresets.GenericFightersPreset> genericFightersPresets = new List<GenericFightersPresets.GenericFightersPreset>();

	// Token: 0x0200032B RID: 811
	[Serializable]
	public class GenericFightersPreset
	{
		// Token: 0x04001620 RID: 5664
		public int tier;

		// Token: 0x04001621 RID: 5665
		public List<string> weaponsIds = new List<string>();

		// Token: 0x04001622 RID: 5666
		public List<string> armorsIds = new List<string>();
	}
}
