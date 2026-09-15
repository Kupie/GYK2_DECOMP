using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class ConveyorSoundSystem : LazySingleton<ConveyorSoundSystem>
{
	// Token: 0x060006ED RID: 1773 RVA: 0x00020C98 File Offset: 0x0001EE98
	public void Register(ConveyorPlaySoundOnEnable source)
	{
		if (source == null)
		{
			return;
		}
		string soundId = source.SoundId;
		if (string.IsNullOrEmpty(soundId))
		{
			return;
		}
		List<ConveyorPlaySoundOnEnable> list;
		if (!this.registeredSources.TryGetValue(soundId, out list))
		{
			list = new List<ConveyorPlaySoundOnEnable>();
			this.registeredSources[soundId] = list;
		}
		if (!list.Contains(source))
		{
			list.Add(source);
		}
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00020CF4 File Offset: 0x0001EEF4
	public void Unregister(ConveyorPlaySoundOnEnable source)
	{
		if (source == null)
		{
			return;
		}
		foreach (KeyValuePair<string, List<ConveyorPlaySoundOnEnable>> keyValuePair in this.registeredSources)
		{
			keyValuePair.Value.Remove(source);
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00020D58 File Offset: 0x0001EF58
	protected override void Awake()
	{
		base.Awake();
		this.InitializeProxyPool();
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00020D68 File Offset: 0x0001EF68
	private void InitializeProxyPool()
	{
		for (int i = 0; i < this.maxActiveClusters; i++)
		{
			GameObject gameObject = new GameObject(string.Format("ConveyorSoundProxy_{0}", i));
			gameObject.transform.SetParent(base.transform);
			this.freeProxies.Push(gameObject.transform);
		}
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00020DC0 File Offset: 0x0001EFC0
	private void Update()
	{
		float num = this.centroidFollowSpeed * Time.deltaTime;
		foreach (ConveyorSoundSystem.ClusterVoice clusterVoice in this.activeVoices)
		{
			if (!(clusterVoice.proxy == null))
			{
				clusterVoice.proxy.position = Vector3.MoveTowards(clusterVoice.proxy.position, clusterVoice.targetPosition, num);
			}
		}
		this.recomputeTimer += Time.deltaTime;
		if (this.recomputeTimer >= this.recomputeInterval)
		{
			this.recomputeTimer = 0f;
			this.RecomputeClusters();
		}
		this.UpdateDebugInfo();
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00020E80 File Offset: 0x0001F080
	public void PlaySounds()
	{
		this.RecomputeClusters();
		this.phaseSoundsUnpaused = true;
		foreach (ConveyorSoundSystem.ClusterVoice clusterVoice in this.activeVoices)
		{
			SoundHandler handler = clusterVoice.handler;
			if (handler != null)
			{
				handler.UnPauseIfActive();
			}
		}
		this.recomputeTimer = 0f;
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00020EF4 File Offset: 0x0001F0F4
	public void PauseSounds()
	{
		this.phaseSoundsUnpaused = false;
		this.RecomputeClusters();
		foreach (ConveyorSoundSystem.ClusterVoice clusterVoice in this.activeVoices)
		{
			SoundHandler handler = clusterVoice.handler;
			if (handler != null)
			{
				handler.PauseIfActive();
			}
		}
		this.recomputeTimer = 0f;
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00020F68 File Offset: 0x0001F168
	private void RecomputeClusters()
	{
		Vector3 position = LazyAudio.Microphone.position;
		List<ConveyorSoundSystem.ComputedCluster> list = new List<ConveyorSoundSystem.ComputedCluster>();
		foreach (KeyValuePair<string, List<ConveyorPlaySoundOnEnable>> keyValuePair in this.registeredSources)
		{
			string key = keyValuePair.Key;
			List<ConveyorPlaySoundOnEnable> value = keyValuePair.Value;
			value.RemoveAll((ConveyorPlaySoundOnEnable source) => source == null || !source.isActiveAndEnabled);
			if (value.Count != 0)
			{
				Sound sound = LazySingletonSO<AudioConfig>.Instance.Get(key);
				float num = ((sound != null) ? sound.volume : 1f);
				foreach (List<ConveyorPlaySoundOnEnable> list2 in ConveyorSoundSystem.ClusterByProximity(value, this.clusterRadius))
				{
					list.Add(this.ComputeCluster(key, list2, position, num));
				}
			}
		}
		list.Sort((ConveyorSoundSystem.ComputedCluster a, ConveyorSoundSystem.ComputedCluster b) => b.mass.CompareTo(a.mass));
		HashSet<ConveyorSoundSystem.ClusterVoice> hashSet = new HashSet<ConveyorSoundSystem.ClusterVoice>();
		foreach (ConveyorSoundSystem.ComputedCluster computedCluster in list)
		{
			ConveyorSoundSystem.ClusterVoice clusterVoice = null;
			float num2 = float.MaxValue;
			foreach (ConveyorSoundSystem.ClusterVoice clusterVoice2 in this.activeVoices)
			{
				if (!hashSet.Contains(clusterVoice2) && !(clusterVoice2.soundId != computedCluster.soundId))
				{
					float sqrMagnitude = (clusterVoice2.targetPosition - computedCluster.centroid).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
						clusterVoice = clusterVoice2;
					}
				}
			}
			if (clusterVoice != null)
			{
				hashSet.Add(clusterVoice);
				this.UpdateVoice(clusterVoice, computedCluster);
			}
			else
			{
				ConveyorSoundSystem.ClusterVoice clusterVoice3 = this.TryAssignVoice(computedCluster);
				if (clusterVoice3 != null)
				{
					hashSet.Add(clusterVoice3);
				}
			}
		}
		for (int i = this.activeVoices.Count - 1; i >= 0; i--)
		{
			if (!hashSet.Contains(this.activeVoices[i]))
			{
				this.ReleaseVoice(this.activeVoices[i]);
				this.activeVoices.RemoveAt(i);
			}
		}
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00021200 File Offset: 0x0001F400
	private static List<List<ConveyorPlaySoundOnEnable>> ClusterByProximity(List<ConveyorPlaySoundOnEnable> sources, float radius)
	{
		HashSet<ConveyorPlaySoundOnEnable> hashSet = new HashSet<ConveyorPlaySoundOnEnable>(sources);
		List<List<ConveyorPlaySoundOnEnable>> list = new List<List<ConveyorPlaySoundOnEnable>>();
		float num = radius * radius;
		while (hashSet.Count > 0)
		{
			List<ConveyorPlaySoundOnEnable> list2 = new List<ConveyorPlaySoundOnEnable>();
			Queue<ConveyorPlaySoundOnEnable> queue = new Queue<ConveyorPlaySoundOnEnable>();
			ConveyorPlaySoundOnEnable conveyorPlaySoundOnEnable = null;
			using (HashSet<ConveyorPlaySoundOnEnable>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					conveyorPlaySoundOnEnable = enumerator.Current;
				}
			}
			hashSet.Remove(conveyorPlaySoundOnEnable);
			queue.Enqueue(conveyorPlaySoundOnEnable);
			while (queue.Count > 0)
			{
				ConveyorPlaySoundOnEnable conveyorPlaySoundOnEnable2 = queue.Dequeue();
				list2.Add(conveyorPlaySoundOnEnable2);
				List<ConveyorPlaySoundOnEnable> list3 = null;
				foreach (ConveyorPlaySoundOnEnable conveyorPlaySoundOnEnable3 in hashSet)
				{
					if ((conveyorPlaySoundOnEnable2.transform.position - conveyorPlaySoundOnEnable3.transform.position).sqrMagnitude <= num)
					{
						if (list3 == null)
						{
							list3 = new List<ConveyorPlaySoundOnEnable>();
						}
						list3.Add(conveyorPlaySoundOnEnable3);
					}
				}
				if (list3 != null)
				{
					foreach (ConveyorPlaySoundOnEnable conveyorPlaySoundOnEnable4 in list3)
					{
						hashSet.Remove(conveyorPlaySoundOnEnable4);
						queue.Enqueue(conveyorPlaySoundOnEnable4);
					}
				}
			}
			list.Add(list2);
		}
		return list;
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00021384 File Offset: 0x0001F584
	private ConveyorSoundSystem.ComputedCluster ComputeCluster(string soundId, List<ConveyorPlaySoundOnEnable> members, Vector3 listenerPosition, float baseVolume)
	{
		Vector3 vector = Vector3.zero;
		float num = 0f;
		foreach (ConveyorPlaySoundOnEnable conveyorPlaySoundOnEnable in members)
		{
			float num2 = Vector3.Distance(listenerPosition, conveyorPlaySoundOnEnable.transform.position);
			float num3 = baseVolume * conveyorPlaySoundOnEnable.MassMultiplier / Mathf.Max(num2, this.massDistanceFloor);
			vector += conveyorPlaySoundOnEnable.transform.position * num3;
			num += num3;
		}
		Vector3 vector2 = ((num > 0f) ? (vector / num) : members[0].transform.position);
		return new ConveyorSoundSystem.ComputedCluster
		{
			soundId = soundId,
			centroid = vector2,
			mass = num,
			members = members
		};
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0002146C File Offset: 0x0001F66C
	private ConveyorSoundSystem.ClusterVoice TryAssignVoice(ConveyorSoundSystem.ComputedCluster cluster)
	{
		ConveyorSoundSystem.ClusterVoice clusterVoice = null;
		if (this.freeProxies.Count > 0)
		{
			clusterVoice = this.CreateVoiceFromPool(cluster.soundId);
		}
		else if (this.activeVoices.Count >= this.maxActiveClusters)
		{
			ConveyorSoundSystem.ClusterVoice clusterVoice2 = null;
			float num = float.MaxValue;
			foreach (ConveyorSoundSystem.ClusterVoice clusterVoice3 in this.activeVoices)
			{
				if (clusterVoice3.mass < num)
				{
					num = clusterVoice3.mass;
					clusterVoice2 = clusterVoice3;
				}
			}
			if (clusterVoice2 != null && cluster.mass > num * this.evictionMassMargin)
			{
				this.ReleaseVoice(clusterVoice2);
				this.activeVoices.Remove(clusterVoice2);
				clusterVoice = this.CreateVoiceFromPool(cluster.soundId);
			}
		}
		if (clusterVoice == null)
		{
			return null;
		}
		this.activeVoices.Add(clusterVoice);
		this.StartVoice(clusterVoice, cluster);
		return clusterVoice;
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0002155C File Offset: 0x0001F75C
	private ConveyorSoundSystem.ClusterVoice CreateVoiceFromPool(string soundId)
	{
		return new ConveyorSoundSystem.ClusterVoice
		{
			soundId = soundId,
			proxy = this.freeProxies.Pop(),
			members = new List<ConveyorPlaySoundOnEnable>()
		};
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00021588 File Offset: 0x0001F788
	private void StartVoice(ConveyorSoundSystem.ClusterVoice voice, ConveyorSoundSystem.ComputedCluster cluster)
	{
		voice.targetPosition = cluster.centroid;
		voice.mass = cluster.mass;
		voice.members = cluster.members;
		voice.proxy.position = cluster.centroid;
		float num;
		float num2;
		this.GetPhaseSyncReference(cluster.soundId, voice, out num, out num2);
		voice.handler = LazyAudio.PlayAtGameObject(cluster.soundId, voice.proxy, SpatialType.sound3D, false);
		if (voice.handler != null && num > 0f)
		{
			try
			{
				float clipLength = voice.handler.GetClipLength();
				float num3 = ((num2 > 0f && clipLength > 0f) ? (num / num2 * clipLength) : num);
				voice.handler.SetTime(num3);
			}
			catch (Exception)
			{
			}
		}
		SoundHandler handler = voice.handler;
		if (handler != null)
		{
			handler.SetVolume(Mathf.Clamp01(cluster.mass));
		}
		if (!this.phaseSoundsUnpaused)
		{
			SoundHandler handler2 = voice.handler;
			if (handler2 == null)
			{
				return;
			}
			handler2.PauseIfActive();
			return;
		}
		else
		{
			SoundHandler handler3 = voice.handler;
			if (handler3 == null)
			{
				return;
			}
			handler3.UnPauseIfActive();
			return;
		}
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00021690 File Offset: 0x0001F890
	private void GetPhaseSyncReference(string soundId, ConveyorSoundSystem.ClusterVoice exclude, out float refTime, out float refLength)
	{
		refTime = 0f;
		refLength = 0f;
		foreach (ConveyorSoundSystem.ClusterVoice clusterVoice in this.activeVoices)
		{
			if (clusterVoice != exclude && !(clusterVoice.soundId != soundId))
			{
				SoundHandler handler = clusterVoice.handler;
				if (handler != null && handler.IsActive)
				{
					try
					{
						refTime = handler.GetTime();
						refLength = handler.GetClipLength();
						break;
					}
					catch (Exception)
					{
						refTime = 0f;
						refLength = 0f;
						break;
					}
				}
			}
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00021744 File Offset: 0x0001F944
	private void UpdateVoice(ConveyorSoundSystem.ClusterVoice voice, ConveyorSoundSystem.ComputedCluster cluster)
	{
		voice.targetPosition = cluster.centroid;
		voice.mass = cluster.mass;
		voice.members = cluster.members;
		bool flag = !this.phaseSoundsUnpaused;
		if (flag)
		{
			if (voice.handler == null || !voice.handler.IsActive || !voice.handler.IsPaused)
			{
				SoundHandler handler = voice.handler;
				if (handler != null)
				{
					handler.Stop();
				}
				voice.handler = LazyAudio.PlayAtGameObject(cluster.soundId, voice.proxy, SpatialType.sound3D, false);
			}
		}
		else if (ConveyorSoundSystem.IsLoopingSound(cluster.soundId) && (voice.handler == null || !voice.handler.IsActive))
		{
			voice.handler = LazyAudio.PlayAtGameObject(cluster.soundId, voice.proxy, SpatialType.sound3D, false);
		}
		SoundHandler handler2 = voice.handler;
		if (handler2 != null)
		{
			handler2.SetVolume(Mathf.Clamp01(cluster.mass));
		}
		if (flag)
		{
			SoundHandler handler3 = voice.handler;
			if (handler3 == null)
			{
				return;
			}
			handler3.PauseIfActive();
			return;
		}
		else
		{
			SoundHandler handler4 = voice.handler;
			if (handler4 == null)
			{
				return;
			}
			handler4.UnPauseIfActive();
			return;
		}
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00021851 File Offset: 0x0001FA51
	private static bool IsLoopingSound(string soundId)
	{
		Sound sound = LazySingletonSO<AudioConfig>.Instance.Get(soundId);
		return sound != null && sound.loop;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002186C File Offset: 0x0001FA6C
	private void ReleaseVoice(ConveyorSoundSystem.ClusterVoice voice)
	{
		SoundHandler handler = voice.handler;
		if (handler != null)
		{
			handler.Stop();
		}
		voice.handler = null;
		List<ConveyorPlaySoundOnEnable> members = voice.members;
		if (members != null)
		{
			members.Clear();
		}
		voice.mass = 0f;
		if (voice.proxy != null)
		{
			this.freeProxies.Push(voice.proxy);
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x000218D0 File Offset: 0x0001FAD0
	private void UpdateDebugInfo()
	{
		this.registeredSourceCount = 0;
		foreach (KeyValuePair<string, List<ConveyorPlaySoundOnEnable>> keyValuePair in this.registeredSources)
		{
			keyValuePair.Value.RemoveAll((ConveyorPlaySoundOnEnable source) => source == null);
			this.registeredSourceCount += keyValuePair.Value.Count;
		}
		this.activeVoiceCount = this.activeVoices.Count;
		this.freeProxyCount = this.freeProxies.Count;
		this.activeClusterDebug.Clear();
		foreach (ConveyorSoundSystem.ClusterVoice clusterVoice in this.activeVoices)
		{
			List<ConveyorSoundSystem.ClusterDebugInfo> list = this.activeClusterDebug;
			ConveyorSoundSystem.ClusterDebugInfo clusterDebugInfo = new ConveyorSoundSystem.ClusterDebugInfo();
			clusterDebugInfo.soundId = clusterVoice.soundId;
			List<ConveyorPlaySoundOnEnable> members = clusterVoice.members;
			clusterDebugInfo.memberCount = ((members != null) ? members.Count : 0);
			clusterDebugInfo.mass = clusterVoice.mass;
			clusterDebugInfo.volume = Mathf.Clamp01(clusterVoice.mass);
			clusterDebugInfo.isPlaying = clusterVoice.handler != null && clusterVoice.handler.IsActive;
			clusterDebugInfo.proxyPosition = ((clusterVoice.proxy != null) ? clusterVoice.proxy.position : clusterVoice.targetPosition);
			clusterDebugInfo.targetPosition = clusterVoice.targetPosition;
			list.Add(clusterDebugInfo);
		}
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00021A78 File Offset: 0x0001FC78
	private void OnDestroy()
	{
		foreach (ConveyorSoundSystem.ClusterVoice clusterVoice in this.activeVoices)
		{
			SoundHandler handler = clusterVoice.handler;
			if (handler != null)
			{
				handler.Stop();
			}
		}
		this.activeVoices.Clear();
		while (this.freeProxies.Count > 0)
		{
			Transform transform = this.freeProxies.Pop();
			if (transform != null)
			{
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x040008D6 RID: 2262
	[SerializeField]
	private float clusterRadius = 3f;

	// Token: 0x040008D7 RID: 2263
	[SerializeField]
	private float recomputeInterval = 0.15f;

	// Token: 0x040008D8 RID: 2264
	[SerializeField]
	private int maxActiveClusters = 8;

	// Token: 0x040008D9 RID: 2265
	[SerializeField]
	private float centroidFollowSpeed = 10f;

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	private float massDistanceFloor = 1f;

	// Token: 0x040008DB RID: 2267
	[SerializeField]
	private float evictionMassMargin = 1.5f;

	// Token: 0x040008DC RID: 2268
	private readonly Dictionary<string, List<ConveyorPlaySoundOnEnable>> registeredSources = new Dictionary<string, List<ConveyorPlaySoundOnEnable>>();

	// Token: 0x040008DD RID: 2269
	private readonly List<ConveyorSoundSystem.ClusterVoice> activeVoices = new List<ConveyorSoundSystem.ClusterVoice>();

	// Token: 0x040008DE RID: 2270
	private readonly Stack<Transform> freeProxies = new Stack<Transform>();

	// Token: 0x040008DF RID: 2271
	private float recomputeTimer;

	// Token: 0x040008E0 RID: 2272
	private bool phaseSoundsUnpaused;

	// Token: 0x040008E1 RID: 2273
	[SerializeField]
	private bool showDebugGizmos = true;

	// Token: 0x040008E2 RID: 2274
	[SerializeField]
	private int registeredSourceCount;

	// Token: 0x040008E3 RID: 2275
	[SerializeField]
	private int activeVoiceCount;

	// Token: 0x040008E4 RID: 2276
	[SerializeField]
	private int freeProxyCount;

	// Token: 0x040008E5 RID: 2277
	[SerializeField]
	private List<ConveyorSoundSystem.ClusterDebugInfo> activeClusterDebug = new List<ConveyorSoundSystem.ClusterDebugInfo>();

	// Token: 0x0200011F RID: 287
	[Serializable]
	private class ClusterDebugInfo
	{
		// Token: 0x040008E6 RID: 2278
		public string soundId;

		// Token: 0x040008E7 RID: 2279
		public int memberCount;

		// Token: 0x040008E8 RID: 2280
		public float mass;

		// Token: 0x040008E9 RID: 2281
		public float volume;

		// Token: 0x040008EA RID: 2282
		public bool isPlaying;

		// Token: 0x040008EB RID: 2283
		public Vector3 proxyPosition;

		// Token: 0x040008EC RID: 2284
		public Vector3 targetPosition;
	}

	// Token: 0x02000120 RID: 288
	private class ClusterVoice
	{
		// Token: 0x040008ED RID: 2285
		public string soundId;

		// Token: 0x040008EE RID: 2286
		public Transform proxy;

		// Token: 0x040008EF RID: 2287
		public SoundHandler handler;

		// Token: 0x040008F0 RID: 2288
		public Vector3 targetPosition;

		// Token: 0x040008F1 RID: 2289
		public float mass;

		// Token: 0x040008F2 RID: 2290
		public List<ConveyorPlaySoundOnEnable> members;
	}

	// Token: 0x02000121 RID: 289
	private class ComputedCluster
	{
		// Token: 0x040008F3 RID: 2291
		public string soundId;

		// Token: 0x040008F4 RID: 2292
		public Vector3 centroid;

		// Token: 0x040008F5 RID: 2293
		public float mass;

		// Token: 0x040008F6 RID: 2294
		public List<ConveyorPlaySoundOnEnable> members;
	}
}
