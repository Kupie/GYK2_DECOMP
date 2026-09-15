using System;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class BuildModeCameraController : MonoBehaviour
{
	// Token: 0x060007E4 RID: 2020 RVA: 0x00026DF0 File Offset: 0x00024FF0
	public void Enable(Transform followTarget, Collider boundingVolume)
	{
		CameraSystem instance = CameraSystem.Instance;
		instance.SetActiveCamera(global::CameraType.BuildMode);
		if (!instance.ActiveCameraController.TrySet3DConfinerBounds(boundingVolume))
		{
			Debug.LogError("The build mode camera must have bounding volume collider");
		}
		this.followTarget = followTarget;
		instance.ActiveCameraController.SetTarget(followTarget, 0f, null);
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00026E2E File Offset: 0x0002502E
	public void Disable()
	{
		CameraSystem.Instance.SetActiveCamera(global::CameraType.Main);
		this.followTarget = null;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00026E42 File Offset: 0x00025042
	public void SetPauseState(bool isPaused)
	{
		if (isPaused)
		{
			CameraSystem.Instance.ActiveCameraController.SetTargetInstant(null);
			return;
		}
		CameraSystem.Instance.ActiveCameraController.SetTargetInstant(this.followTarget);
	}

	// Token: 0x040009DB RID: 2523
	private Transform followTarget;
}
