using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class DialogDataContainer : ScriptableObject
{
	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000974 RID: 2420 RVA: 0x00030166 File Offset: 0x0002E366
	public static DialogDataContainer Instance
	{
		get
		{
			if (DialogDataContainer.cachedInstance == null)
			{
				DialogDataContainer.cachedInstance = Resources.Load<DialogDataContainer>("Locales/DialogData");
				DialogDataContainer.cachedInstance.Initialize();
			}
			return DialogDataContainer.cachedInstance;
		}
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00030194 File Offset: 0x0002E394
	private void Initialize()
	{
		this.hash.Clear();
		foreach (DialogData dialogData in this.dialogDataList)
		{
			this.hash.Add(dialogData.id, dialogData);
		}
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00030200 File Offset: 0x0002E400
	public static DialogData Get(string localeKey)
	{
		DialogData dialogData;
		if (DialogDataContainer.Instance.hash.TryGetValue(localeKey, out dialogData))
		{
			return dialogData;
		}
		Debug.LogError(string.Format("Cannot find {0} for localeKey [{1}].", typeof(DialogData), localeKey));
		return null;
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0003023E File Offset: 0x0002E43E
	public static bool TryGet(string localeKey, out DialogData result)
	{
		return DialogDataContainer.Instance.hash.TryGetValue(localeKey, out result);
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00030254 File Offset: 0x0002E454
	public static bool IsVoiceOverMuted(string localeKey)
	{
		if (string.IsNullOrEmpty(localeKey))
		{
			return false;
		}
		string text = LLBase.ResolveAlias(localeKey);
		DialogData dialogData;
		return (DialogDataContainer.TryGet(text, out dialogData) && dialogData.modificator == LL.LocModificator.VoiceOverMuted) || (text != localeKey && DialogDataContainer.TryGet(localeKey, out dialogData) && dialogData.modificator == LL.LocModificator.VoiceOverMuted);
	}

	// Token: 0x04000B11 RID: 2833
	public List<DialogData> dialogDataList = new List<DialogData>();

	// Token: 0x04000B12 RID: 2834
	private Dictionary<string, DialogData> hash = new Dictionary<string, DialogData>();

	// Token: 0x04000B13 RID: 2835
	private static DialogDataContainer cachedInstance;
}
