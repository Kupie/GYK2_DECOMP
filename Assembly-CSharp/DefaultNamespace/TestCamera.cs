using System;
using UnityEngine;

namespace DefaultNamespace
{
	// Token: 0x02000B4A RID: 2890
	public class TestCamera : MonoBehaviour
	{
		// Token: 0x06004CD0 RID: 19664 RVA: 0x00169F78 File Offset: 0x00168178
		private void OutputRotationMatrix()
		{
			Camera component = base.GetComponent<Camera>();
			Matrix4x4 worldToCameraMatrix = component.worldToCameraMatrix;
			Debug.Log(worldToCameraMatrix);
			string text = string.Format("#define CUSTOM_WORLD_TO_CAMERA float3x3({0:0.000000}; {1:0.000000}; {2:0.000000}; {3:0.000000}; {4:0.000000}; {5:0.000000}; {6:0.000000}; {7:0.000000}; {8:0.000000})", new object[]
			{
				worldToCameraMatrix[0, 0],
				worldToCameraMatrix[0, 1],
				worldToCameraMatrix[0, 2],
				worldToCameraMatrix[1, 0],
				worldToCameraMatrix[1, 1],
				worldToCameraMatrix[1, 2],
				worldToCameraMatrix[2, 0],
				worldToCameraMatrix[2, 1],
				worldToCameraMatrix[2, 2]
			});
			text = text.Replace(",", ".");
			text = text.Replace(";", ",");
			Vector3 eulerAngles = component.transform.rotation.eulerAngles;
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			if (eulerAngles.y > 180f)
			{
				eulerAngles.y -= 360f;
			}
			if (eulerAngles.z > 180f)
			{
				eulerAngles.z -= 360f;
			}
			text = string.Format("// rotation matrix for camera ({0}, {1}, {2}) calculated with camera.worldToCameraMatrix():\n{3}", new object[] { eulerAngles.x, eulerAngles.y, eulerAngles.z, text });
			Debug.Log(text);
		}
	}
}
