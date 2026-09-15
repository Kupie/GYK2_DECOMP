using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000703 RID: 1795
public class ScenePoolPathRegistry : LazySingleton<ScenePoolPathRegistry>
{
	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x06002F54 RID: 12116 RVA: 0x000E31BC File Offset: 0x000E13BC
	// (set) Token: 0x06002F55 RID: 12117 RVA: 0x000E31C4 File Offset: 0x000E13C4
	public ScenePoolTrimPolicy TrimPolicy
	{
		get
		{
			return this.trimPolicy;
		}
		set
		{
			this.trimPolicy = value;
		}
	}

	// Token: 0x06002F56 RID: 12118 RVA: 0x000E31CD File Offset: 0x000E13CD
	protected override void Awake()
	{
		base.Awake();
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06002F57 RID: 12119 RVA: 0x000E31E0 File Offset: 0x000E13E0
	public void RegisterScenePaths(string sceneId, IEnumerable<string> paths)
	{
		if (string.IsNullOrEmpty(sceneId) || paths == null)
		{
			return;
		}
		HashSet<string> hashSet;
		if (!this.sceneIdToPaths.TryGetValue(sceneId, out hashSet))
		{
			hashSet = new HashSet<string>();
			this.sceneIdToPaths[sceneId] = hashSet;
		}
		foreach (string text in paths)
		{
			if (!string.IsNullOrEmpty(text) && hashSet.Add(text))
			{
				int num;
				this.pathRefCount.TryGetValue(text, out num);
				this.pathRefCount[text] = num + 1;
			}
		}
	}

	// Token: 0x06002F58 RID: 12120 RVA: 0x000E3280 File Offset: 0x000E1480
	public void UnregisterScenePaths(string sceneId)
	{
		if (string.IsNullOrEmpty(sceneId))
		{
			return;
		}
		HashSet<string> hashSet;
		if (!this.sceneIdToPaths.TryGetValue(sceneId, out hashSet))
		{
			return;
		}
		this.sceneIdToPaths.Remove(sceneId);
		List<string> list = new List<string>();
		foreach (string text in hashSet)
		{
			int num;
			if (this.pathRefCount.TryGetValue(text, out num))
			{
				num--;
				if (num <= 0)
				{
					this.pathRefCount.Remove(text);
					list.Add(text);
				}
				else
				{
					this.pathRefCount[text] = num;
				}
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		this.TrimOrphanedPaths(sceneId, list);
	}

	// Token: 0x06002F59 RID: 12121 RVA: 0x000E3344 File Offset: 0x000E1544
	private void TrimOrphanedPaths(string sceneId, List<string> paths)
	{
		int num = BakedChunkableObjectPool.TrimPaths(paths, this.trimPolicy);
		int num2 = ConstructorPartPool.TrimPaths(paths, this.trimPolicy);
		int num3 = WgoPartPool.TrimPaths(paths, this.trimPolicy);
		int num4 = num + num2 + num3;
		if (num4 > 0)
		{
			Debug.Log(string.Format("[ScenePoolPathRegistry] scene=[{0}] policy={1} trimmed baked={2} constructor={3} wgoPart={4} total={5}", new object[] { sceneId, this.trimPolicy, num, num2, num3, num4 }));
		}
	}

	// Token: 0x06002F5A RID: 12122 RVA: 0x000E33D0 File Offset: 0x000E15D0
	private void LogRegistryState()
	{
		Debug.Log(string.Format("[{0}] policy={1}, scenes={2}, trackedPaths={3}", new object[]
		{
			"ScenePoolPathRegistry",
			this.trimPolicy,
			this.sceneIdToPaths.Count,
			this.pathRefCount.Count
		}));
	}

	// Token: 0x04002641 RID: 9793
	[SerializeField]
	private ScenePoolTrimPolicy trimPolicy;

	// Token: 0x04002642 RID: 9794
	private readonly Dictionary<string, HashSet<string>> sceneIdToPaths = new Dictionary<string, HashSet<string>>();

	// Token: 0x04002643 RID: 9795
	private readonly Dictionary<string, int> pathRefCount = new Dictionary<string, int>();
}
