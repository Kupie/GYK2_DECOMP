using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000B03 RID: 2819
[CreateAssetMenu(fileName = "DebugShaders")]
public class ShadersDebugSettings : LazySingletonSO<ShadersDebugSettings>
{
	// Token: 0x04003C96 RID: 15510
	[Tooltip("Global shaders will be applied to materials alongside preset shaders")]
	public List<Shader> globalShaders;

	// Token: 0x04003C97 RID: 15511
	[Tooltip("Note: you don't need to add original shader that is currently applied to a material")]
	public List<ShadersDebugSettings.ShaderPreset> overridableMaterials;

	// Token: 0x04003C98 RID: 15512
	public List<ShadersDebugSettings.ShaderPreset> seaMaterials = new List<ShadersDebugSettings.ShaderPreset>();

	// Token: 0x02000B04 RID: 2820
	[Serializable]
	public class ShaderPreset
	{
		// Token: 0x04003C99 RID: 15513
		public Material material;

		// Token: 0x04003C9A RID: 15514
		public List<Shader> shaderVariants;
	}
}
