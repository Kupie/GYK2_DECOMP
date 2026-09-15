using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Sirenix.Serialization;
using UnityEngine;

// Token: 0x020005D7 RID: 1495
[Serializable]
public class WsoData : ObjectLinkedToDefinition<WSODef>
{
	// Token: 0x1400008B RID: 139
	// (add) Token: 0x06002769 RID: 10089 RVA: 0x000B9230 File Offset: 0x000B7430
	// (remove) Token: 0x0600276A RID: 10090 RVA: 0x000B9268 File Offset: 0x000B7468
	public event Action<bool> OnHiddenStateChanged;

	// Token: 0x1400008C RID: 140
	// (add) Token: 0x0600276B RID: 10091 RVA: 0x000B92A0 File Offset: 0x000B74A0
	// (remove) Token: 0x0600276C RID: 10092 RVA: 0x000B92D8 File Offset: 0x000B74D8
	public event Action OnRepairStateChanged;

	// Token: 0x0600276D RID: 10093 RVA: 0x000B930D File Offset: 0x000B750D
	public WsoData CreateDataFromMe(Vector3 globalOffset, string gameSceneId, bool copySGuid)
	{
		WsoData wsoData = new WsoData(this, copySGuid ? this.UniqueId : null);
		wsoData.Position += globalOffset;
		wsoData.WorldId = gameSceneId;
		return wsoData;
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x0600276E RID: 10094 RVA: 0x000B933A File Offset: 0x000B753A
	public SGuid UniqueId
	{
		get
		{
			return this.uniqueId;
		}
	}

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x0600276F RID: 10095 RVA: 0x000B9342 File Offset: 0x000B7542
	// (set) Token: 0x06002770 RID: 10096 RVA: 0x000B934A File Offset: 0x000B754A
	public Vector3 Position
	{
		get
		{
			return this.position;
		}
		set
		{
			this.position = value;
		}
	}

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06002771 RID: 10097 RVA: 0x000B9353 File Offset: 0x000B7553
	// (set) Token: 0x06002772 RID: 10098 RVA: 0x000B935B File Offset: 0x000B755B
	public Vector3 Scale
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06002773 RID: 10099 RVA: 0x000B9364 File Offset: 0x000B7564
	// (set) Token: 0x06002774 RID: 10100 RVA: 0x000B936C File Offset: 0x000B756C
	public string WorldId
	{
		get
		{
			return this.worldId;
		}
		set
		{
			this.worldId = value;
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06002775 RID: 10101 RVA: 0x000B9375 File Offset: 0x000B7575
	// (set) Token: 0x06002776 RID: 10102 RVA: 0x000B937D File Offset: 0x000B757D
	public bool IsHidden
	{
		get
		{
			return this.isHidden;
		}
		set
		{
			if (this.isHidden == value)
			{
				return;
			}
			this.isHidden = value;
			Action<bool> onHiddenStateChanged = this.OnHiddenStateChanged;
			if (onHiddenStateChanged == null)
			{
				return;
			}
			onHiddenStateChanged(this.isHidden);
		}
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06002777 RID: 10103 RVA: 0x000B93A6 File Offset: 0x000B75A6
	// (set) Token: 0x06002778 RID: 10104 RVA: 0x000B93AE File Offset: 0x000B75AE
	public string CustomTag
	{
		get
		{
			return this.customTag;
		}
		set
		{
			this.customTag = value;
		}
	}

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06002779 RID: 10105 RVA: 0x000B93B7 File Offset: 0x000B75B7
	public IReadOnlyList<WsoComponentDataBase> ComponentData
	{
		get
		{
			return this.componentData;
		}
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x000B93BF File Offset: 0x000B75BF
	public WsoData()
	{
		this.uniqueId = new SGuid();
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x000B93F3 File Offset: 0x000B75F3
	public WsoData(WSODef wsoDef)
		: this(wsoDef, null)
	{
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x000B9400 File Offset: 0x000B7600
	public WsoData(WSODef wsoDef, SGuid newUniqueId)
	{
		if (!SGuid.IsNullOrEmpty(newUniqueId))
		{
			this.uniqueId.SetGuid(newUniqueId);
		}
		if (wsoDef == null)
		{
			Debug.LogError("wsoDef is null, that mustn't be happened");
			return;
		}
		this.id = wsoDef.id;
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x000B9462 File Offset: 0x000B7662
	public WsoData(WSODef wsoDef, SGuid newUniqueId, Vector3 position, string worldId)
		: this(wsoDef, newUniqueId)
	{
		this.position = position;
		this.worldId = worldId;
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x000B947C File Offset: 0x000B767C
	public WsoData(WsoData source, SGuid newUniqueId = null)
		: base(source.id)
	{
		if (!SGuid.IsNullOrEmpty(newUniqueId))
		{
			this.uniqueId.SetGuid(newUniqueId);
		}
		else
		{
			this.uniqueId = new SGuid();
		}
		this.position = source.position;
		this.scale = source.scale;
		this.worldId = source.worldId;
		this.isHidden = source.isHidden;
		this.customTag = source.customTag;
		foreach (WsoComponentDataBase wsoComponentDataBase in source.componentData)
		{
			WsoComponentDataBase wsoComponentDataBase2 = SerializationUtility.CreateCopy(wsoComponentDataBase) as WsoComponentDataBase;
			if (wsoComponentDataBase2 != null)
			{
				this.componentData.Add(wsoComponentDataBase2);
			}
		}
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x000B956C File Offset: 0x000B776C
	public T GetComponentData<T>() where T : WsoComponentDataBase
	{
		foreach (WsoComponentDataBase wsoComponentDataBase in this.componentData)
		{
			T t = wsoComponentDataBase as T;
			if (t != null)
			{
				return t;
			}
		}
		return default(T);
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x000B95DC File Offset: 0x000B77DC
	public List<T> GetAllComponentData<T>() where T : WsoComponentDataBase
	{
		List<T> list = new List<T>();
		foreach (WsoComponentDataBase wsoComponentDataBase in this.componentData)
		{
			T t = wsoComponentDataBase as T;
			if (t != null)
			{
				list.Add(t);
			}
		}
		return list;
	}

	// Token: 0x06002781 RID: 10113 RVA: 0x000B9648 File Offset: 0x000B7848
	public void AddComponentData(WsoComponentDataBase data)
	{
		if (data == null)
		{
			return;
		}
		this.componentData.Add(data);
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x000B965A File Offset: 0x000B785A
	public bool RemoveComponentData(WsoComponentDataBase data)
	{
		return this.componentData.Remove(data);
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x000B9668 File Offset: 0x000B7868
	public WsoRepairablePartData GetOrCreateRepairablePartData()
	{
		WsoRepairablePartData wsoRepairablePartData = this.GetComponentData<WsoRepairablePartData>();
		if (wsoRepairablePartData != null)
		{
			return wsoRepairablePartData;
		}
		WsoRepairablePartData wsoRepairablePartData2 = new WsoRepairablePartData();
		this.componentData.Add(wsoRepairablePartData2);
		return wsoRepairablePartData2;
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000B9694 File Offset: 0x000B7894
	public bool HasRepairablePartData()
	{
		return this.GetComponentData<WsoRepairablePartData>() != null;
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000B96A0 File Offset: 0x000B78A0
	public void PrepareForGame()
	{
		foreach (WsoComponentDataBase wsoComponentDataBase in this.componentData)
		{
			wsoComponentDataBase.PrepareForGame();
		}
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x000B96F0 File Offset: 0x000B78F0
	public void Cleanup()
	{
		foreach (WsoComponentDataBase wsoComponentDataBase in this.componentData)
		{
			wsoComponentDataBase.Cleanup();
		}
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x000B9740 File Offset: 0x000B7940
	public void NotifyRepairStateChanged()
	{
		Action onRepairStateChanged = this.OnRepairStateChanged;
		if (onRepairStateChanged == null)
		{
			return;
		}
		onRepairStateChanged();
	}

	// Token: 0x0400219D RID: 8605
	[SerializeField]
	private SGuid uniqueId = new SGuid();

	// Token: 0x0400219E RID: 8606
	[SerializeField]
	private Vector3 position;

	// Token: 0x0400219F RID: 8607
	[SerializeField]
	private Vector3 scale = Vector3.one;

	// Token: 0x040021A0 RID: 8608
	[SerializeField]
	private string worldId;

	// Token: 0x040021A1 RID: 8609
	[SerializeField]
	private bool isHidden;

	// Token: 0x040021A2 RID: 8610
	[SerializeField]
	private string customTag;

	// Token: 0x040021A3 RID: 8611
	[SerializeReference]
	private List<WsoComponentDataBase> componentData = new List<WsoComponentDataBase>();
}
