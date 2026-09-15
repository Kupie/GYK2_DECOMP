using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000706 RID: 1798
public class CinematicsSceneDisplayManager : LazySingleton<CinematicsSceneDisplayManager>
{
	// Token: 0x06002F6A RID: 12138 RVA: 0x000E3738 File Offset: 0x000E1938
	public void DisplayCinematicsScene(string id, Action onCompleted = null)
	{
		if (string.IsNullOrEmpty(id))
		{
			Debug.LogError("Failed to display cinematics scene: id is null or empty");
			return;
		}
		string text = id.Split('_', StringSplitOptions.None)[1];
		string prefabPath = string.Concat(new string[] { "Assets/_Scenes/Cinematics/", text, "/", id, ".prefab" });
		this.onCompleted = onCompleted;
		UIFade uiFade = LazyUI.Get<UIFade>();
		Action <>9__2;
		Action <>9__3;
		Action<AsyncOperationHandle<GameObject>> <>9__1;
		uiFade.FadeIn(this.fadeDuration, delegate
		{
			this.prefabHandle = Addressables.InstantiateAsync(prefabPath, this.gameObject.transform, false, false);
			CinematicsSceneDisplayManager <>4__this = this;
			Action<AsyncOperationHandle<GameObject>> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(AsyncOperationHandle<GameObject> handle)
				{
					if (handle.Status != AsyncOperationStatus.Succeeded)
					{
						Debug.LogError("Failed to load prefab: " + prefabPath);
						UIBasicFade uiFade3 = uiFade;
						float num = this.fadeDuration;
						Action action2;
						if ((action2 = <>9__2) == null)
						{
							action2 = (<>9__2 = delegate
							{
								Action action4 = onCompleted;
								if (action4 != null)
								{
									action4();
								}
								onCompleted = null;
							});
						}
						uiFade3.FadeOut(num, action2, FadeFlag.Common);
						return;
					}
					GameObject result = handle.Result;
					if (!result.TryGetComponent<CinematicsScene>(out this.cinematicsScene))
					{
						Debug.LogError("Failed to get CinematicsScene component from prefab: " + prefabPath);
						UIBasicFade uiFade2 = uiFade;
						float num2 = this.fadeDuration;
						Action action3;
						if ((action3 = <>9__3) == null)
						{
							action3 = (<>9__3 = delegate
							{
								Action action5 = onCompleted;
								if (action5 != null)
								{
									action5();
								}
								onCompleted = null;
							});
						}
						uiFade2.FadeOut(num2, action3, FadeFlag.Common);
						return;
					}
					result.transform.localPosition = Vector3.zero;
					MainGame.PlayerController.SetControlTakenType(TakenControlType.ByCinematics, false);
					LazyUI.Get<HUD>().SetDisableState(HudStateType.CinematicsScene, false, null);
					CameraSystem.Instance.SetActiveCamera(global::CameraType.Cinematics);
					CameraSystem.Instance.ActiveCameraController.SetTarget(this.cinematicsScene.CameraTarget, 0f, null);
					WeatherSystem.Instance.AudioMixerStateController.Push(AudioMixerSnapshotLayer.Cinematics);
					WeatherSystem.Instance.SetPauseState(WeatherSystemPauseFlag.Cinematics, true);
					this.SetBloomActive(false);
					this.cinematicsScene.Director.stopped += this.OnDirectorStopped;
					uiFade.FadeOut(this.fadeDuration, new Action(this.cinematicsScene.Play), FadeFlag.Common);
				});
			}
			<>4__this.prefabHandle.Completed += action;
		}, FadeFlag.Common, false);
	}

	// Token: 0x06002F6B RID: 12139 RVA: 0x000E37DF File Offset: 0x000E19DF
	private void OnDirectorStopped(PlayableDirector director)
	{
		director.stopped -= this.OnDirectorStopped;
		LazyUI.Get<UIFade>().Fade(this.fadeDuration, null, delegate
		{
			MainGame.PlayerController.SetControlTakenType(TakenControlType.ByCinematics, true);
			LazyUI.Get<HUD>().SetDisableState(HudStateType.CinematicsScene, true, null);
			CameraSystem.Instance.ActiveCameraController.SetTarget(null, 0f, null);
			CameraSystem.Instance.SetActiveCamera(global::CameraType.Main);
			this.SetBloomActive(this.bloomWasActive);
			Action action = this.onCompleted;
			if (action != null)
			{
				action();
			}
			this.onCompleted = null;
			Addressables.Release<GameObject>(this.prefabHandle);
			WeatherSystem.Instance.AudioMixerStateController.Pop(AudioMixerSnapshotLayer.Cinematics);
			WeatherSystem.Instance.SetPauseState(WeatherSystemPauseFlag.Cinematics, false);
		});
	}

	// Token: 0x06002F6C RID: 12140 RVA: 0x000E3810 File Offset: 0x000E1A10
	private void SetBloomActive(bool active)
	{
		Bloom bloom;
		if (CameraSystem.Instance.MainCamera.PostProcessVolume.profile.TryGetSettings<Bloom>(out bloom))
		{
			if (!active)
			{
				this.bloomWasActive = bloom.active;
			}
			bloom.active = active;
		}
	}

	// Token: 0x0400264F RID: 9807
	private const string CINEMATICS_SCENE_PREFAB_PATH = "Assets/_Scenes/Cinematics";

	// Token: 0x04002650 RID: 9808
	[SerializeField]
	private float fadeDuration = 0.5f;

	// Token: 0x04002651 RID: 9809
	private AsyncOperationHandle<GameObject> prefabHandle;

	// Token: 0x04002652 RID: 9810
	private Action onCompleted;

	// Token: 0x04002653 RID: 9811
	private CinematicsScene cinematicsScene;

	// Token: 0x04002654 RID: 9812
	private bool bloomWasActive;
}
