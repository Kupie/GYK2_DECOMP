using System;
using UnityEngine;

namespace DefaultNamespace
{
	// Token: 0x02000B4B RID: 2891
	public class TestObject : MonoBehaviour
	{
		// Token: 0x06004CD2 RID: 19666 RVA: 0x0016A11C File Offset: 0x0016831C
		private void OutputObjToWorldMatrix()
		{
			this.OutputMatrix(base.transform.localToWorldMatrix, 1f);
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x0016A134 File Offset: 0x00168334
		private void OutputWorldToObjMatrix()
		{
			this.OutputMatrix(base.transform.worldToLocalMatrix, 1f);
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0016A14C File Offset: 0x0016834C
		private void OutputWorldToCameraMatrix()
		{
			this.OutputMatrix(Camera.main.worldToCameraMatrix * base.transform.localToWorldMatrix, 1f);
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x0016A174 File Offset: 0x00168374
		private void OutputMatrix(Matrix4x4 m, float k = 1f)
		{
			Debug.Log(m);
			string text = "";
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (text.Length > 0)
					{
						text += "; ";
					}
					text += string.Format("{0:0.000000}", m[i, j] * k);
				}
			}
			text = text.Replace(",", ".");
			text = text.Replace(";", ",");
			text = "float3x3(" + text + ")";
			Debug.Log(text);
		}
	}
}
