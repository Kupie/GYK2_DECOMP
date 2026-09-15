using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000377 RID: 887
public static class MouseAimHelper
{
	// Token: 0x0600178C RID: 6028 RVA: 0x0006FC28 File Offset: 0x0006DE28
	public static bool TryGetAimDirection(Vector3 playerWorldPos, out Vector2 direction)
	{
		direction = Vector2.zero;
		CameraSystem instance = CameraSystem.Instance;
		if (instance == null || instance.MainCamera == null)
		{
			return false;
		}
		MainCamera mainCamera = instance.MainCamera;
		if (mainCamera.GetRenderType() == MainCamera.RenderMode.Lightweight && mainCamera.RenderTexture == null)
		{
			return false;
		}
		Ray ray = CameraSystem.ScreenPointToRay(Input.mousePosition);
		Plane plane = new Plane(Vector3.up, playerWorldPos);
		float num;
		if (!plane.Raycast(ray, out num) || num <= 0f)
		{
			return false;
		}
		Vector3 vector = ray.GetPoint(num) - playerWorldPos;
		vector.y = 0f;
		if (vector.sqrMagnitude < 0.04f)
		{
			return false;
		}
		direction = new Vector2(vector.x, vector.z * 1.25f);
		if (direction.sqrMagnitude < 0.0001f)
		{
			return false;
		}
		direction.Normalize();
		return true;
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x0006FD10 File Offset: 0x0006DF10
	public static bool IsPointerOverUI()
	{
		EventSystem current = EventSystem.current;
		return current != null && current.IsPointerOverGameObject();
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x0006FD34 File Offset: 0x0006DF34
	public static bool TryApplyAim(PlayerPhysicalBody physicalBody)
	{
		if (physicalBody == null)
		{
			return false;
		}
		Vector2 vector;
		if (!MouseAimHelper.TryGetAimDirection(physicalBody.transform.position, out vector))
		{
			return false;
		}
		physicalBody.SetAimDirection(vector);
		return true;
	}

	// Token: 0x04001768 RID: 5992
	private const float WorldDeadZoneSqr = 0.04f;
}
