using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200011C RID: 284
public class AudioMixerStateController
{
	// Token: 0x17000115 RID: 277
	// (get) Token: 0x060006DA RID: 1754 RVA: 0x00020950 File Offset: 0x0001EB50
	public static AudioMixerStateController Unavailable { get; } = new AudioMixerStateController();

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060006DB RID: 1755 RVA: 0x00020957 File Offset: 0x0001EB57
	public AudioMixerSnapshotLayer ActiveLayer
	{
		get
		{
			return this.ResolveActiveLayer();
		}
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002095F File Offset: 0x0001EB5F
	private AudioMixerStateController()
	{
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00020988 File Offset: 0x0001EB88
	public AudioMixerStateController(AudioMixer audioMixer)
	{
		if (audioMixer == null)
		{
			Debug.LogError("AudioMixerStateController: audioMixer is null");
			return;
		}
		this.isAvailable = true;
		this.Register(audioMixer, AudioMixerSnapshotLayer.Default, "Default");
		this.Register(audioMixer, AudioMixerSnapshotLayer.Indoor, "Indoor");
		this.Register(audioMixer, AudioMixerSnapshotLayer.IndoorDungeon, "Indoor_Dungeon");
		this.Register(audioMixer, AudioMixerSnapshotLayer.IndoorChurch, "Indoor_Church");
		this.Register(audioMixer, AudioMixerSnapshotLayer.Cinematics, "Cinematics");
		this.Register(audioMixer, AudioMixerSnapshotLayer.MainMenu, "MainMenu");
		this.Register(audioMixer, AudioMixerSnapshotLayer.NotDirectlyInGame, "NotDirectlyInGame");
		this.ApplyActive();
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00020A35 File Offset: 0x0001EC35
	public void Push(AudioMixerSnapshotLayer layer)
	{
		if (!this.isAvailable || layer == AudioMixerSnapshotLayer.Default)
		{
			return;
		}
		if (!this.activeLayers.Add(layer))
		{
			return;
		}
		this.ApplyActive();
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00020A58 File Offset: 0x0001EC58
	public void Pop(AudioMixerSnapshotLayer layer)
	{
		if (!this.isAvailable || layer == AudioMixerSnapshotLayer.Default)
		{
			return;
		}
		if (!this.activeLayers.Remove(layer))
		{
			return;
		}
		this.ApplyActive();
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00020A7B File Offset: 0x0001EC7B
	public void SetIndoorVariant(AudioMixerSnapshotLayer indoorLayer)
	{
		if (!this.isAvailable)
		{
			return;
		}
		if (false | this.SetLayerMembership(AudioMixerSnapshotLayer.Indoor, indoorLayer == AudioMixerSnapshotLayer.Indoor) | this.SetLayerMembership(AudioMixerSnapshotLayer.IndoorDungeon, indoorLayer == AudioMixerSnapshotLayer.IndoorDungeon) | this.SetLayerMembership(AudioMixerSnapshotLayer.IndoorChurch, indoorLayer == AudioMixerSnapshotLayer.IndoorChurch))
		{
			this.ApplyActive();
		}
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00020AB3 File Offset: 0x0001ECB3
	public void SetLayerActive(AudioMixerSnapshotLayer layer, bool active)
	{
		if (!this.isAvailable)
		{
			return;
		}
		if (active)
		{
			this.Push(layer);
			return;
		}
		this.Pop(layer);
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00020AD0 File Offset: 0x0001ECD0
	public void ResetToDefault()
	{
		if (!this.isAvailable)
		{
			return;
		}
		this.activeLayers.Clear();
		this.activeLayers.Add(AudioMixerSnapshotLayer.Default);
		this.ApplyActive();
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00020AF9 File Offset: 0x0001ECF9
	private bool SetLayerMembership(AudioMixerSnapshotLayer layer, bool active)
	{
		if (layer == AudioMixerSnapshotLayer.Default)
		{
			return false;
		}
		if (!active)
		{
			return this.activeLayers.Remove(layer);
		}
		return this.activeLayers.Add(layer);
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00020B1C File Offset: 0x0001ED1C
	private void ApplyActive()
	{
		AudioMixerSnapshotLayer audioMixerSnapshotLayer = this.ResolveActiveLayer();
		AudioMixerSnapshot audioMixerSnapshot;
		if (!this.snapshots.TryGetValue(audioMixerSnapshotLayer, out audioMixerSnapshot) || audioMixerSnapshot == null)
		{
			Debug.LogError(string.Format("AudioMixerStateController: snapshot for [{0}] is missing", audioMixerSnapshotLayer));
			return;
		}
		audioMixerSnapshot.TransitionTo(0.01f);
		InWorldSfxFilterController.Reapply();
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00020B70 File Offset: 0x0001ED70
	private AudioMixerSnapshotLayer ResolveActiveLayer()
	{
		AudioMixerSnapshotLayer audioMixerSnapshotLayer = AudioMixerSnapshotLayer.Default;
		foreach (AudioMixerSnapshotLayer audioMixerSnapshotLayer2 in this.activeLayers)
		{
			if (audioMixerSnapshotLayer2 > audioMixerSnapshotLayer)
			{
				audioMixerSnapshotLayer = audioMixerSnapshotLayer2;
			}
		}
		return audioMixerSnapshotLayer;
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00020BC8 File Offset: 0x0001EDC8
	private void Register(AudioMixer audioMixer, AudioMixerSnapshotLayer layer, string snapshotName)
	{
		if (audioMixer == null)
		{
			Debug.LogError("AudioMixerStateController: cannot register snapshot [" + snapshotName + "], audioMixer is null");
			this.snapshots[layer] = null;
			return;
		}
		AudioMixerSnapshot audioMixerSnapshot = audioMixer.FindSnapshot(snapshotName);
		if (audioMixerSnapshot == null)
		{
			Debug.LogError(string.Concat(new string[] { "AudioMixerStateController: snapshot [", snapshotName, "] not found on [", audioMixer.name, "]" }));
		}
		this.snapshots[layer] = audioMixerSnapshot;
	}

	// Token: 0x040008D0 RID: 2256
	private const float SnapshotTransitionSeconds = 0.01f;

	// Token: 0x040008D2 RID: 2258
	private readonly Dictionary<AudioMixerSnapshotLayer, AudioMixerSnapshot> snapshots = new Dictionary<AudioMixerSnapshotLayer, AudioMixerSnapshot>();

	// Token: 0x040008D3 RID: 2259
	private readonly HashSet<AudioMixerSnapshotLayer> activeLayers = new HashSet<AudioMixerSnapshotLayer> { AudioMixerSnapshotLayer.Default };

	// Token: 0x040008D4 RID: 2260
	private readonly bool isAvailable;
}
