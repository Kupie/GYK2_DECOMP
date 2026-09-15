using System;

namespace Expressive
{
	// Token: 0x02000031 RID: 49
	public interface IVariableProvider
	{
		// Token: 0x060000FF RID: 255
		bool TryGetValue(string variableName, out object value);
	}
}
