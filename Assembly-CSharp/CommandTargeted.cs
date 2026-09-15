using System;
using UnityEngine;

// Token: 0x02000740 RID: 1856
public abstract class CommandTargeted<T> : Command
{
	// Token: 0x06003055 RID: 12373 RVA: 0x000E791F File Offset: 0x000E5B1F
	public CommandTargeted()
	{
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x000E7D1F File Offset: 0x000E5F1F
	public CommandTargeted(T target)
	{
		this.target = target;
		Debug.Log(string.Format("CommandTargeted Constructor: {0}", target));
	}

	// Token: 0x04002725 RID: 10021
	protected T target;
}
