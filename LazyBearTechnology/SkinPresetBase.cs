using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000170 RID: 368
	public abstract class SkinPresetBase : ScriptableObject
	{
		// Token: 0x06000814 RID: 2068 RVA: 0x000287F2 File Offset: 0x000269F2
		public static LazySkinPreset Load(string id)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000287F9 File Offset: 0x000269F9
		public virtual int DefineSkinIdFor(char char4, char char5, char char6)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00028800 File Offset: 0x00026A00
		public virtual void ApplyShaderParametersTo(List<SpriteRenderer> sprites)
		{
			throw new NotImplementedException();
		}
	}
}
