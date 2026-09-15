using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B0D RID: 2829
[Serializable]
public class VariableNotificator<T> : IEquatable<T>
{
	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x06004B52 RID: 19282 RVA: 0x00163E0C File Offset: 0x0016200C
	// (remove) Token: 0x06004B53 RID: 19283 RVA: 0x00163E44 File Offset: 0x00162044
	public event Action<T> ValueChanged;

	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x06004B54 RID: 19284 RVA: 0x00163E79 File Offset: 0x00162079
	// (set) Token: 0x06004B55 RID: 19285 RVA: 0x00163E81 File Offset: 0x00162081
	public T Value
	{
		get
		{
			return this.value;
		}
		set
		{
			bool flag = this.value.Equals(value);
			this.value = value;
			if (flag)
			{
				return;
			}
			Action<T> valueChanged = this.ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged(this.value);
		}
	}

	// Token: 0x06004B56 RID: 19286 RVA: 0x00021B94 File Offset: 0x0001FD94
	public VariableNotificator()
	{
	}

	// Token: 0x06004B57 RID: 19287 RVA: 0x00163EBA File Offset: 0x001620BA
	public VariableNotificator(T value)
	{
		this.value = value;
	}

	// Token: 0x06004B58 RID: 19288 RVA: 0x00163EC9 File Offset: 0x001620C9
	protected bool Equals(VariableNotificator<T> other)
	{
		return EqualityComparer<T>.Default.Equals(this.value, other.value);
	}

	// Token: 0x06004B59 RID: 19289 RVA: 0x00163EE4 File Offset: 0x001620E4
	public bool Equals(T obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals(obj as VariableNotificator<T>)));
	}

	// Token: 0x04003CB2 RID: 15538
	[SerializeField]
	protected T value;
}
