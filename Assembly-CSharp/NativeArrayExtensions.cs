using System;
using Unity.Collections;

// Token: 0x02000ACB RID: 2763
public static class NativeArrayExtensions
{
	// Token: 0x06004AA2 RID: 19106 RVA: 0x001604A0 File Offset: 0x0015E6A0
	public static NativeArray<T> CombineNativeArrays<T>(NativeArray<T>[] arrays, Allocator allocator) where T : struct
	{
		int num = 0;
		foreach (NativeArray<T> nativeArray in arrays)
		{
			num += nativeArray.Length;
		}
		NativeArray<T> nativeArray2 = new NativeArray<T>(num, allocator, NativeArrayOptions.ClearMemory);
		int num2 = 0;
		foreach (NativeArray<T> nativeArray3 in arrays)
		{
			NativeArray<T>.Copy(nativeArray3, 0, nativeArray2, num2, nativeArray3.Length);
			num2 += nativeArray3.Length;
		}
		return nativeArray2;
	}
}
