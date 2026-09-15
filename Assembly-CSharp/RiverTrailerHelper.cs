using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007D2 RID: 2002
public class RiverTrailerHelper : MonoBehaviour
{
	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x06003371 RID: 13169 RVA: 0x000F8ABC File Offset: 0x000F6CBC
	// (set) Token: 0x06003372 RID: 13170 RVA: 0x000F8AC4 File Offset: 0x000F6CC4
	private HashSet<IChunkableObject> AllStaticObjectsInZone { get; set; }

	// Token: 0x06003373 RID: 13171 RVA: 0x000F8ACD File Offset: 0x000F6CCD
	public void SetCustomCamaraFollowTarget()
	{
		CameraSystem.Instance.ActiveCameraController.SetTarget(this.cameraFollowGo.transform, 0f, null);
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x000F8AEF File Offset: 0x000F6CEF
	public void ResetCustomCamaraFollowTarget()
	{
		CameraSystem.Instance.ActiveCameraController.SetTarget(CameraSystem.Instance.GroundPointTransform, 0f, null);
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x000F8B10 File Offset: 0x000F6D10
	public void MakePreparationsOnAnimStart()
	{
		EnvironmentEngine environmentEngine = MainGame.Instance.GameSave.environmentData.EnvironmentEngine;
		environmentEngine.SetTimeOfDay(Mathf.Clamp(this.timeOfDayForAnim, 0f, 1f));
		environmentEngine.IsPaused = true;
		GUIElements.Instance.SetVisibilityState(false);
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x000F8B5D File Offset: 0x000F6D5D
	public void SpawnBodies()
	{
		if (!this.areChunksIgnored)
		{
			this.areChunksIgnored = true;
			this.SetNotIgnoredStateForChunkableObjects();
		}
		this.bodiesSpawner.SpawnBodies();
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x000F8B7F File Offset: 0x000F6D7F
	public void ClearBodies()
	{
		this.bodiesSpawner.ClearBodies();
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x000F8B8C File Offset: 0x000F6D8C
	public void Play()
	{
		this.director.Play();
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x000F8B99 File Offset: 0x000F6D99
	public void ResetBodiesPos()
	{
		if (this.bodiesSpawner)
		{
			this.bodiesSpawner.ResetBodiesPos(new bool?(true));
		}
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x000F8BB9 File Offset: 0x000F6DB9
	public void StartRiverFlowTransform()
	{
		if (this.bodiesSpawner)
		{
			this.bodiesSpawner.StartFlowTransform();
		}
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x000F8BD3 File Offset: 0x000F6DD3
	public void StartRiverFlowPhysics()
	{
		if (this.bodiesSpawner)
		{
			this.bodiesSpawner.StartFlow();
		}
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x000F8BED File Offset: 0x000F6DED
	public void AddSingleBodyToOthersForFlow()
	{
		this.bodiesSpawner.AddBody(this.singleFlowingBody);
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x000F8C00 File Offset: 0x000F6E00
	private void SetNotIgnoredStateForChunkableObjects()
	{
		this.AllStaticObjectsInZone = new HashSet<IChunkableObject>();
		foreach (IChunkableObject chunkableObject in LazySingleton<ChunkManager>.Instance.GetAllChunkableObjectsInBoundsForSelectedLayers(new List<ChunkManagerLayerType> { ChunkManagerLayerType.StaticObjects }, this.chunkIgnoreCollider.bounds))
		{
			this.AllStaticObjectsInZone.Add(chunkableObject);
			chunkableObject.UpdateFlag(ChunkingIgnoreType.Fighting, true);
		}
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x000F8C88 File Offset: 0x000F6E88
	private void Start()
	{
		EnvironmentEngine.Instance.SetTimeOfDayPreset(this.timeOfDayPresetName);
	}

	// Token: 0x04002922 RID: 10530
	public PlayableDirector director;

	// Token: 0x04002923 RID: 10531
	public GameObject cameraFollowGo;

	// Token: 0x04002924 RID: 10532
	public AreaBodiesSpawner bodiesSpawner;

	// Token: 0x04002925 RID: 10533
	public BoxCollider chunkIgnoreCollider;

	// Token: 0x04002926 RID: 10534
	public GameObject singleFlowingBody;

	// Token: 0x04002927 RID: 10535
	[Range(0f, 1f)]
	public float timeOfDayForAnim = 0.3f;

	// Token: 0x04002928 RID: 10536
	public string timeOfDayPresetName = "outdoor";

	// Token: 0x04002929 RID: 10537
	private bool areChunksIgnored;
}
