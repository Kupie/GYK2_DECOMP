using System;
using System.Collections.Generic;

// Token: 0x0200067C RID: 1660
public class AsyncLoadRequestTracker<TTarget> where TTarget : class
{
	// Token: 0x06002BF9 RID: 11257 RVA: 0x000D01F4 File Offset: 0x000CE3F4
	public int Begin(TTarget target)
	{
		if (target == null)
		{
			return -1;
		}
		int num = this.NextRequestId(target);
		this.activeRequestIds[target] = num;
		return num;
	}

	// Token: 0x06002BFA RID: 11258 RVA: 0x000D0221 File Offset: 0x000CE421
	public void Cancel(TTarget target)
	{
		if (target == null)
		{
			return;
		}
		this.activeRequestIds.Remove(target);
	}

	// Token: 0x06002BFB RID: 11259 RVA: 0x000D023C File Offset: 0x000CE43C
	public bool IsActual(TTarget target, int requestId)
	{
		int num;
		return target != null && this.activeRequestIds.TryGetValue(target, out num) && num == requestId;
	}

	// Token: 0x06002BFC RID: 11260 RVA: 0x000D0267 File Offset: 0x000CE467
	public bool IsTracking(TTarget target)
	{
		return target != null && this.activeRequestIds.ContainsKey(target);
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x000D027F File Offset: 0x000CE47F
	public void Complete(TTarget target, int requestId)
	{
		if (this.IsActual(target, requestId))
		{
			this.activeRequestIds.Remove(target);
		}
	}

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06002BFE RID: 11262 RVA: 0x000D0298 File Offset: 0x000CE498
	public bool HasActiveRequests
	{
		get
		{
			return this.activeRequestIds.Count > 0;
		}
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x000D02A8 File Offset: 0x000CE4A8
	private int NextRequestId(TTarget target)
	{
		int num;
		if (this.activeRequestIds.TryGetValue(target, out num))
		{
			return num + 1;
		}
		return 1;
	}

	// Token: 0x04002395 RID: 9109
	private readonly Dictionary<TTarget, int> activeRequestIds = new Dictionary<TTarget, int>();
}
