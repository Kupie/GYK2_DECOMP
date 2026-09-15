using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000485 RID: 1157
[CreateAssetMenu(menuName = "GK2/SaveFixer/Save Fix", fileName = "1.0")]
public class SaveFixData : ScriptableObject
{
	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x06001EB0 RID: 7856 RVA: 0x00090F8A File Offset: 0x0008F18A
	public string Version
	{
		get
		{
			return this.version;
		}
	}

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x00090F92 File Offset: 0x0008F192
	public IReadOnlyList<SaveFixOperation> Operations
	{
		get
		{
			return this.operations;
		}
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x00090F9A File Offset: 0x0008F19A
	public bool TryGetVersion(out GameSaveVersion parsed)
	{
		return GameSaveVersion.TryParse(this.version, out parsed) || GameSaveVersion.TryParse(base.name, out parsed);
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x00090FB8 File Offset: 0x0008F1B8
	public void AddOperation(SaveFixOperation operation)
	{
		if (this.operations == null)
		{
			this.operations = new List<SaveFixOperation>();
		}
		this.operations.Add(operation);
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x00090FDC File Offset: 0x0008F1DC
	public bool ContainsWgoOperationFor(SGuid uniqueId)
	{
		if (SGuid.IsNullOrEmpty(uniqueId) || this.operations == null)
		{
			return false;
		}
		foreach (SaveFixOperation saveFixOperation in this.operations)
		{
			SaveFixWgoOperation saveFixWgoOperation = saveFixOperation as SaveFixWgoOperation;
			if (saveFixWgoOperation != null && saveFixWgoOperation.OccupiesUniqueId && saveFixWgoOperation.ContainsUniqueId(uniqueId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x0009105C File Offset: 0x0008F25C
	public T FindOperation<T>() where T : SaveFixOperation
	{
		if (this.operations == null)
		{
			return default(T);
		}
		foreach (SaveFixOperation saveFixOperation in this.operations)
		{
			T t = saveFixOperation as T;
			if (t != null)
			{
				return t;
			}
		}
		return default(T);
	}

	// Token: 0x04001BC6 RID: 7110
	public const string AddressablesFolder = "Assets/AddressableAssets/SaveFixerAssets";

	// Token: 0x04001BC7 RID: 7111
	public const string AddressablesLabel = "SaveFixerAssets";

	// Token: 0x04001BC8 RID: 7112
	public const string AddressablesGroup = "SaveFixerAssets";

	// Token: 0x04001BC9 RID: 7113
	[SerializeField]
	private string version;

	// Token: 0x04001BCA RID: 7114
	[SerializeReference]
	private List<SaveFixOperation> operations = new List<SaveFixOperation>();
}
