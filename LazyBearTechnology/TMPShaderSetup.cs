using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200015B RID: 347
	[CreateAssetMenu(menuName = "LazyFont/TMPShaderSetup", fileName = "TMPShaderSetup")]
	public class TMPShaderSetup : LazySingletonSO<TMPShaderSetup>
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x00026706 File Offset: 0x00024906
		private Shader Default
		{
			get
			{
				return LazySingletonSO<TMPShaderSetup>.Instance.shaderDataList[0].shader;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00026720 File Offset: 0x00024920
		public static Shader FindShader(bool outline, bool secondOutline, bool shadow, bool overlayTexture, bool eightSide)
		{
			TMPShaderSetup.ShaderConditionData shaderConditionData = LazySingletonSO<TMPShaderSetup>.Instance.shaderDataList.Find((TMPShaderSetup.ShaderConditionData x) => x.outline == outline && x.secondOutline == secondOutline && x.shadow == shadow && x.eightSide == eightSide);
			Shader shader = ((shaderConditionData != null) ? shaderConditionData.shader : null);
			if (shader == null)
			{
				Debug.LogWarning(string.Format("Cant find shader for setup:(outline:[{0}] shadow:[{1}] overlayTexture:[{2}] eightSide:[{3}]) return default", new object[] { outline, shadow, overlayTexture, eightSide }));
				return LazySingletonSO<TMPShaderSetup>.Instance.Default;
			}
			return shader;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000267D8 File Offset: 0x000249D8
		public void AddDefault()
		{
			if (this.shaderDataList.Count == 0)
			{
				TMPShaderSetup.ShaderConditionData shaderConditionData = new TMPShaderSetup.ShaderConditionData();
				shaderConditionData.shader = Shader.Find("TextMeshPro/Pixel Font Simple");
				this.shaderDataList.Add(shaderConditionData);
			}
		}

		// Token: 0x04000495 RID: 1173
		[SerializeField]
		private List<TMPShaderSetup.ShaderConditionData> shaderDataList = new List<TMPShaderSetup.ShaderConditionData>();

		// Token: 0x02000200 RID: 512
		[Serializable]
		public class ShaderConditionData
		{
			// Token: 0x040006CB RID: 1739
			public bool outline;

			// Token: 0x040006CC RID: 1740
			public bool secondOutline;

			// Token: 0x040006CD RID: 1741
			public bool shadow;

			// Token: 0x040006CE RID: 1742
			public bool eightSide;

			// Token: 0x040006CF RID: 1743
			public Shader shader;
		}
	}
}
