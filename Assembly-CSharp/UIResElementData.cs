using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000880 RID: 2176
public class UIResElementData : LazyWidgetDataBase
{
	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x060037A2 RID: 14242 RVA: 0x0010C67D File Offset: 0x0010A87D
	// (set) Token: 0x060037A3 RID: 14243 RVA: 0x0010C685 File Offset: 0x0010A885
	public Action<UIResElementData> OnShowTimeComplete { get; private set; }

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x060037A4 RID: 14244 RVA: 0x0010C68E File Offset: 0x0010A88E
	// (set) Token: 0x060037A5 RID: 14245 RVA: 0x0010C696 File Offset: 0x0010A896
	public string Id { get; private set; }

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x060037A6 RID: 14246 RVA: 0x0010C69F File Offset: 0x0010A89F
	// (set) Token: 0x060037A7 RID: 14247 RVA: 0x0010C6A7 File Offset: 0x0010A8A7
	public float Value { get; private set; }

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x060037A8 RID: 14248 RVA: 0x0010C6B0 File Offset: 0x0010A8B0
	// (set) Token: 0x060037A9 RID: 14249 RVA: 0x0010C6B8 File Offset: 0x0010A8B8
	public string IconId { get; private set; }

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x060037AA RID: 14250 RVA: 0x0010C6C1 File Offset: 0x0010A8C1
	// (set) Token: 0x060037AB RID: 14251 RVA: 0x0010C6C9 File Offset: 0x0010A8C9
	public bool HasTarget { get; private set; }

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x060037AC RID: 14252 RVA: 0x0010C6D2 File Offset: 0x0010A8D2
	// (set) Token: 0x060037AD RID: 14253 RVA: 0x0010C6DA File Offset: 0x0010A8DA
	public bool IsUITarget { get; private set; }

	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x060037AE RID: 14254 RVA: 0x0010C6E3 File Offset: 0x0010A8E3
	// (set) Token: 0x060037AF RID: 14255 RVA: 0x0010C6EB File Offset: 0x0010A8EB
	public UIGameResDisplayingType DisplayingType { get; private set; }

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x060037B0 RID: 14256 RVA: 0x0010C6F4 File Offset: 0x0010A8F4
	// (set) Token: 0x060037B1 RID: 14257 RVA: 0x0010C6FC File Offset: 0x0010A8FC
	public Transform Target { get; private set; }

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x060037B2 RID: 14258 RVA: 0x0010C705 File Offset: 0x0010A905
	// (set) Token: 0x060037B3 RID: 14259 RVA: 0x0010C70D File Offset: 0x0010A90D
	public Vector3 StartPosition { get; private set; }

	// Token: 0x060037B4 RID: 14260 RVA: 0x0010C718 File Offset: 0x0010A918
	public UIResElementData(string id, UIGameResDisplayingType displayType, string iconId, float value, Transform target, bool isUITarget, Action<UIResElementData> onShowTimeComplete = null)
	{
		this.Id = id;
		this.DisplayingType = displayType;
		this.IconId = iconId;
		this.Value = value;
		this.OnShowTimeComplete = onShowTimeComplete;
		this.IsUITarget = isUITarget;
		this.Target = target;
		this.StartPosition = target.position;
		this.HasTarget = target != null;
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x0010C77B File Offset: 0x0010A97B
	public void AddValue(float value)
	{
		this.Value += value;
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x0010C78C File Offset: 0x0010A98C
	public int GetShowValue()
	{
		int num = Mathf.RoundToInt(this.Value);
		int num2 = (this.Value.EqualsTo((float)num, 1E-05f) ? num : ((int)Mathf.Sign(this.Value) * Mathf.FloorToInt(Mathf.Abs(this.Value))));
		this.Value -= (float)num2;
		return num2;
	}
}
