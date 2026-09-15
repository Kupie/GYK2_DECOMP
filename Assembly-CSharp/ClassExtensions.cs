using System;

// Token: 0x02000AC6 RID: 2758
public static class ClassExtensions
{
	// Token: 0x06004A92 RID: 19090 RVA: 0x00160212 File Offset: 0x0015E412
	public static T InvokeMethod<T>(this T obj, Action<T> action)
	{
		action(obj);
		return obj;
	}
}
