using System;

// Token: 0x02000756 RID: 1878
public interface IProxyClassTargeted<T> where T : class
{
	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x060030C3 RID: 12483
	// (set) Token: 0x060030C4 RID: 12484
	T Target { get; set; }
}
