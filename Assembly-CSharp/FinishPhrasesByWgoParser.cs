using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

// Token: 0x02000AF3 RID: 2803
public static class FinishPhrasesByWgoParser
{
	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x06004ADC RID: 19164 RVA: 0x00161610 File Offset: 0x0015F810
	public static FinishPhrasesByWgoData PhrasesByWgoData
	{
		get
		{
			if (FinishPhrasesByWgoParser.phrasesByWgoData == null)
			{
				FinishPhrasesByWgoParser.phrasesByWgoData = Addressables.LoadAssetAsync<FinishPhrasesByWgoData>("Assets/AddressableAssets/Helpers/FinishPhrasesByWgo.asset").WaitForCompletion();
			}
			return FinishPhrasesByWgoParser.phrasesByWgoData;
		}
	}

	// Token: 0x06004ADD RID: 19165 RVA: 0x00161646 File Offset: 0x0015F846
	public static bool HasFinishPhrases(string wgoId)
	{
		return !string.IsNullOrEmpty(wgoId) && FinishPhrasesByWgoParser.PhrasesByWgoId.ContainsKey(wgoId);
	}

	// Token: 0x06004ADE RID: 19166 RVA: 0x0016165D File Offset: 0x0015F85D
	public static bool TryGetFinishPhrases(string wgoId, out PhrasesByWgo phrasesByWgo)
	{
		phrasesByWgo = null;
		return !string.IsNullOrEmpty(wgoId) && FinishPhrasesByWgoParser.PhrasesByWgoId.TryGetValue(wgoId, out phrasesByWgo);
	}

	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x06004ADF RID: 19167 RVA: 0x00161678 File Offset: 0x0015F878
	private static Dictionary<string, PhrasesByWgo> PhrasesByWgoId
	{
		get
		{
			if (FinishPhrasesByWgoParser.phrasesByWgoId == null)
			{
				FinishPhrasesByWgoParser.phrasesByWgoId = new Dictionary<string, PhrasesByWgo>();
				FinishPhrasesByWgoData finishPhrasesByWgoData = FinishPhrasesByWgoParser.PhrasesByWgoData;
				List<PhrasesByWgo> list = ((finishPhrasesByWgoData != null) ? finishPhrasesByWgoData.finishPhrasesByWgo : null);
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						PhrasesByWgo phrasesByWgo = list[i];
						if (phrasesByWgo != null && !string.IsNullOrEmpty(phrasesByWgo.wgoId))
						{
							FinishPhrasesByWgoParser.phrasesByWgoId[phrasesByWgo.wgoId] = phrasesByWgo;
						}
					}
				}
			}
			return FinishPhrasesByWgoParser.phrasesByWgoId;
		}
	}

	// Token: 0x06004AE0 RID: 19168 RVA: 0x001616EA File Offset: 0x0015F8EA
	private static void InvalidatePhrasesIndex()
	{
		FinishPhrasesByWgoParser.phrasesByWgoId = null;
	}

	// Token: 0x04003C74 RID: 15476
	private const string PHRASES_BY_WGO_ASSET_PATH = "Assets/AddressableAssets/Helpers/FinishPhrasesByWgo.asset";

	// Token: 0x04003C75 RID: 15477
	private const string PHRASES_BY_WGO_ASSET_DIRECTORY = "/AddressableAssets/Helpers";

	// Token: 0x04003C76 RID: 15478
	private static FinishPhrasesByWgoData phrasesByWgoData;

	// Token: 0x04003C77 RID: 15479
	private static Dictionary<string, PhrasesByWgo> phrasesByWgoId;
}
