using System;
using System.Globalization;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class SaveSlotData : ISerializableData
{
	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06001EEB RID: 7915 RVA: 0x000922EB File Offset: 0x000904EB
	public bool ShouldConvertDemoProgressOnLoad
	{
		get
		{
			return this.isDemoSave || this.importedFromDemo;
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06001EEC RID: 7916 RVA: 0x000922FD File Offset: 0x000904FD
	// (set) Token: 0x06001EED RID: 7917 RVA: 0x0009230A File Offset: 0x0009050A
	public bool IsDemoSlotDeleted
	{
		get
		{
			return RemovedDemoSavesList.IsDeleted(this.slotName);
		}
		set
		{
			if (value)
			{
				RemovedDemoSavesList.TryAddSave(this.slotName);
				return;
			}
			RemovedDemoSavesList.TryRemoveSave(this.slotName);
		}
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x00092328 File Offset: 0x00090528
	public SaveSlotData Copy()
	{
		return new SaveSlotData
		{
			slotName = this.slotName,
			day = this.day,
			platform = this.platform,
			gameSaveVersion = this.gameSaveVersion,
			isAutoSave = this.isAutoSave,
			saveDateTime = this.saveDateTime,
			serializedCulture = this.serializedCulture,
			isAutoSave = this.isAutoSave,
			isDemoSave = this.isDemoSave,
			importedFromDemo = this.importedFromDemo,
			graveyardQuality = this.graveyardQuality,
			churchQuality = this.churchQuality,
			villageRep = this.villageRep,
			repValue = this.repValue
		};
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000923E4 File Offset: 0x000905E4
	public DateTime GetSaveDateTime()
	{
		DateTime dateTime;
		try
		{
			if (!string.IsNullOrEmpty(this.serializedCulture))
			{
				dateTime = DateTime.Parse(this.saveDateTime, new CultureInfo(this.serializedCulture));
			}
			else
			{
				dateTime = DateTime.Parse(this.saveDateTime);
			}
		}
		catch (Exception)
		{
			Debug.LogError("Cant parse save slot:[" + this.slotName + "] DateTime!!!");
			dateTime = default(DateTime);
		}
		return dateTime;
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x00002318 File Offset: 0x00000518
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06001EF1 RID: 7921 RVA: 0x00002318 File Offset: 0x00000518
	public void OnAfterSerialize()
	{
	}

	// Token: 0x04001BDC RID: 7132
	[NonSerialized]
	public string slotName;

	// Token: 0x04001BDD RID: 7133
	public int day;

	// Token: 0x04001BDE RID: 7134
	public bool isAutoSave;

	// Token: 0x04001BDF RID: 7135
	public string saveDateTime;

	// Token: 0x04001BE0 RID: 7136
	public string serializedCulture;

	// Token: 0x04001BE1 RID: 7137
	public string platform;

	// Token: 0x04001BE2 RID: 7138
	public string gameSaveVersion;

	// Token: 0x04001BE3 RID: 7139
	public int graveyardQuality;

	// Token: 0x04001BE4 RID: 7140
	public int churchQuality;

	// Token: 0x04001BE5 RID: 7141
	public int villageRep;

	// Token: 0x04001BE6 RID: 7142
	public bool repValue;

	// Token: 0x04001BE7 RID: 7143
	public bool isDemoSave;

	// Token: 0x04001BE8 RID: 7144
	public bool importedFromDemo;
}
