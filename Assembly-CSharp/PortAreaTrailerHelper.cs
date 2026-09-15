using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007D1 RID: 2001
public class PortAreaTrailerHelper : MonoBehaviour
{
	// Token: 0x0600336E RID: 13166 RVA: 0x000F8A82 File Offset: 0x000F6C82
	public void Play()
	{
		this.director.Play();
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x000F8A8F File Offset: 0x000F6C8F
	public void SetCameraFollowGo()
	{
		CameraSystem.Instance.ActiveCameraController.SetTarget(this.cameraFollowGo.transform, 0f, null);
		GUIElements.Instance.SetVisibilityState(false);
	}

	// Token: 0x04002920 RID: 10528
	public PlayableDirector director;

	// Token: 0x04002921 RID: 10529
	public Transform cameraFollowGo;
}
