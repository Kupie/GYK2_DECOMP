using System;
using UnityEngine;

// Token: 0x02000B16 RID: 2838
[ExecuteInEditMode]
[DefaultExecutionOrder(10)]
public class FollowMainCamera : MonoBehaviour
{
	// Token: 0x06004B9C RID: 19356 RVA: 0x00165731 File Offset: 0x00163931
	private void Awake()
	{
		this.mainCamera = CameraSystem.Instance.MainCamera;
	}

	// Token: 0x06004B9D RID: 19357 RVA: 0x00165744 File Offset: 0x00163944
	private void Update()
	{
		Vector3 position = this.mainCamera.transform.position;
		if (!this.followY)
		{
			position.y = base.transform.position.y - this.positionDelta.y;
		}
		base.transform.position = position + this.positionDelta;
	}

	// Token: 0x06004B9E RID: 19358 RVA: 0x001657A4 File Offset: 0x001639A4
	private void OnDrawGizmosSelected()
	{
		if (this.mainCamera == null)
		{
			return;
		}
		Camera camera = this.mainCamera.Camera;
		Gizmos.color = Color.gray;
		float orthographicSize = camera.orthographicSize;
		float aspect = camera.aspect;
		float nearClipPlane = camera.nearClipPlane;
		float farClipPlane = camera.farClipPlane;
		float num = orthographicSize;
		float num2 = orthographicSize * aspect;
		Vector3[] array = new Vector3[]
		{
			camera.transform.position + camera.transform.rotation * new Vector3(-num2, -num, nearClipPlane),
			camera.transform.position + camera.transform.rotation * new Vector3(num2, -num, nearClipPlane),
			camera.transform.position + camera.transform.rotation * new Vector3(num2, num, nearClipPlane),
			camera.transform.position + camera.transform.rotation * new Vector3(-num2, num, nearClipPlane),
			camera.transform.position + camera.transform.rotation * new Vector3(-num2, -num, farClipPlane),
			camera.transform.position + camera.transform.rotation * new Vector3(num2, -num, farClipPlane),
			camera.transform.position + camera.transform.rotation * new Vector3(num2, num, farClipPlane),
			camera.transform.position + camera.transform.rotation * new Vector3(-num2, num, farClipPlane)
		};
		for (int i = 0; i < 4; i++)
		{
			Gizmos.DrawLine(array[i], array[(i + 1) % 4]);
			Gizmos.DrawLine(array[i + 4], array[(i + 1) % 4 + 4]);
			Gizmos.DrawLine(array[i], array[i + 4]);
		}
	}

	// Token: 0x04003CF3 RID: 15603
	private MainCamera mainCamera;

	// Token: 0x04003CF4 RID: 15604
	public Vector3 positionDelta = Vector3.zero;

	// Token: 0x04003CF5 RID: 15605
	public bool followY = true;
}
