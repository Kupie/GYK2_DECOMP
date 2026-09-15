using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200017F RID: 383
[CreateAssetMenu(fileName = "SpeakerIdAliases", menuName = "ScriptableObjects/SpeakerIdAliases")]
public class SpeakerIdAliases : ScriptableObject
{
	// Token: 0x17000177 RID: 375
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x000302C4 File Offset: 0x0002E4C4
	public static SpeakerIdAliases Instance
	{
		get
		{
			if (SpeakerIdAliases.cachedInstance == null)
			{
				SpeakerIdAliases.cachedInstance = Resources.Load<SpeakerIdAliases>("Locales/SpeakerIdAliases");
			}
			return SpeakerIdAliases.cachedInstance;
		}
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x000302E8 File Offset: 0x0002E4E8
	public static bool TryGetWgoId(string key, out string value)
	{
		SpeakerIdAliases.AliasElement aliasElement = SpeakerIdAliases.Instance.aliases.Find((SpeakerIdAliases.AliasElement x) => x.key == key);
		value = ((aliasElement != null) ? aliasElement.value : null);
		if (string.IsNullOrEmpty(value))
		{
			Debug.LogError("[SpeakerIdAliases]: there's no value for key '" + key + "'");
			return false;
		}
		return true;
	}

	// Token: 0x04000B14 RID: 2836
	[SerializeField]
	private List<SpeakerIdAliases.AliasElement> aliases = new List<SpeakerIdAliases.AliasElement>();

	// Token: 0x04000B15 RID: 2837
	private static SpeakerIdAliases cachedInstance;

	// Token: 0x02000180 RID: 384
	[Serializable]
	private class AliasElement
	{
		// Token: 0x04000B16 RID: 2838
		public string key;

		// Token: 0x04000B17 RID: 2839
		public string value;
	}
}
