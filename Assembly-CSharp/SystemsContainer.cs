using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020007BB RID: 1979
[DefaultExecutionOrder(-20)]
[ExecuteAlways]
public class SystemsContainer : MonoBehaviour
{
	// Token: 0x060032EF RID: 13039 RVA: 0x000F5A28 File Offset: 0x000F3C28
	private void Awake()
	{
		SystemsContainer.TryDestroyManagerIfNotOnMainScene(this);
	}

	// Token: 0x060032F0 RID: 13040 RVA: 0x000F5A34 File Offset: 0x000F3C34
	private static bool TryDestroyManagerIfNotOnMainScene(MonoBehaviour component)
	{
		if (Application.isPlaying)
		{
			int num = 0;
			while (num < SceneManager.sceneCount && !(SceneManager.GetSceneAt(num).name == "MainScene"))
			{
				num++;
			}
			if (component.gameObject.scene.name != "MainScene")
			{
				global::UnityEngine.Object.Destroy(component.gameObject);
				return true;
			}
		}
		return false;
	}
}
