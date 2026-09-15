using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000ABE RID: 2750
public class AmbientSoundMixer
{
	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x06004A73 RID: 19059 RVA: 0x0015F26A File Offset: 0x0015D46A
	// (set) Token: 0x06004A74 RID: 19060 RVA: 0x0015F272 File Offset: 0x0015D472
	public float SwitchDuration
	{
		get
		{
			return this.switchDuration;
		}
		set
		{
			this.switchDuration = Mathf.Max(0.01f, value);
		}
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x0015F288 File Offset: 0x0015D488
	public void SetAmbientPair(string id1, string id2, float lerp, bool crossfade = true)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.pairId1 != id1 || this.pairId2 != id2)
		{
			if (crossfade)
			{
				this.FadeOutUnusedPairSounds(id1, id2);
			}
			else
			{
				this.HardStopUnusedPairSounds(id1, id2, this.pairId1, this.pairId2);
			}
		}
		this.pairId1 = id1;
		this.pairId2 = id2;
		this.pairLerp = Mathf.Clamp01(lerp);
		this.EnsurePlaying(this.pairId1);
		if (!string.IsNullOrEmpty(this.pairId2) && this.pairId2 != this.pairId1)
		{
			this.EnsurePlaying(this.pairId2);
		}
		this.ApplyVolumes();
	}

	// Token: 0x06004A76 RID: 19062 RVA: 0x0015F334 File Offset: 0x0015D534
	public void SetOverride(string id, bool crossfade = true)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		string text = (string.IsNullOrEmpty(id) ? null : id);
		if (text == this.overrideId)
		{
			this.overrideTarget = ((text != null) ? 1f : 0f);
			if (!crossfade)
			{
				this.overrideBlend = this.overrideTarget;
				this.ApplyVolumes();
			}
			return;
		}
		SoundHandler soundHandler;
		if (!string.IsNullOrEmpty(this.overrideId) && this.active.TryGetValue(this.overrideId, out soundHandler) && this.overrideId != this.pairId1 && this.overrideId != this.pairId2)
		{
			if (crossfade)
			{
				this.fadingOut.Add(new AmbientSoundMixer.FadingOutEntry
				{
					id = this.overrideId,
					handler = soundHandler,
					volume = this.overrideBlend
				});
			}
			else
			{
				this.CacheAndStop(this.overrideId, soundHandler);
			}
			this.active.Remove(this.overrideId);
		}
		this.overrideId = text;
		this.overrideTarget = ((text != null) ? 1f : 0f);
		if (text != null)
		{
			this.EnsurePlaying(text);
		}
		if (!crossfade)
		{
			this.overrideBlend = this.overrideTarget;
		}
		this.ApplyVolumes();
	}

	// Token: 0x06004A77 RID: 19063 RVA: 0x0015F474 File Offset: 0x0015D674
	public void Update(float deltaTime)
	{
		bool flag = false;
		if (!Mathf.Approximately(this.overrideBlend, this.overrideTarget))
		{
			float num = deltaTime / this.switchDuration;
			if (this.overrideBlend < this.overrideTarget)
			{
				this.overrideBlend = Mathf.Min(this.overrideBlend + num, this.overrideTarget);
			}
			else
			{
				this.overrideBlend = Mathf.Max(this.overrideBlend - num, this.overrideTarget);
			}
			flag = true;
		}
		for (int i = this.fadingOut.Count - 1; i >= 0; i--)
		{
			AmbientSoundMixer.FadingOutEntry fadingOutEntry = this.fadingOut[i];
			fadingOutEntry.volume = Mathf.Max(0f, fadingOutEntry.volume - deltaTime / this.switchDuration);
			if (fadingOutEntry.handler != null && fadingOutEntry.handler.IsActive)
			{
				fadingOutEntry.handler.SetVolume(fadingOutEntry.volume);
			}
			if (fadingOutEntry.volume <= 0f)
			{
				this.CacheAndStop(fadingOutEntry.id, fadingOutEntry.handler);
				this.fadingOut.RemoveAt(i);
			}
			else
			{
				this.fadingOut[i] = fadingOutEntry;
			}
		}
		if (flag)
		{
			this.ApplyVolumes();
		}
	}

	// Token: 0x06004A78 RID: 19064 RVA: 0x0015F598 File Offset: 0x0015D798
	public void StopAll()
	{
		foreach (KeyValuePair<string, SoundHandler> keyValuePair in this.active)
		{
			this.CacheAndStop(keyValuePair.Key, keyValuePair.Value);
		}
		this.active.Clear();
		for (int i = 0; i < this.fadingOut.Count; i++)
		{
			this.CacheAndStop(this.fadingOut[i].id, this.fadingOut[i].handler);
		}
		this.fadingOut.Clear();
		this.pairId1 = null;
		this.pairId2 = null;
		this.overrideId = null;
		this.overrideBlend = 0f;
		this.overrideTarget = 0f;
	}

	// Token: 0x06004A79 RID: 19065 RVA: 0x0015F678 File Offset: 0x0015D878
	private void EnsurePlaying(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return;
		}
		SoundHandler soundHandler;
		if (this.active.TryGetValue(id, out soundHandler) && soundHandler != null && soundHandler.IsActive)
		{
			return;
		}
		int i = this.fadingOut.Count - 1;
		while (i >= 0)
		{
			if (this.fadingOut[i].id == id)
			{
				SoundHandler handler = this.fadingOut[i].handler;
				this.fadingOut.RemoveAt(i);
				if (handler != null && handler.IsActive)
				{
					this.active[id] = handler;
					return;
				}
				break;
			}
			else
			{
				i--;
			}
		}
		SoundHandler soundHandler2 = LazyAudio.Play(id, false);
		if (soundHandler2 == null)
		{
			Debug.LogWarning("AmbientSoundMixer: failed to play sound [" + id + "]");
			return;
		}
		float num;
		if (this.playTimeCache.TryGetValue(id, out num))
		{
			soundHandler2.SetTime(num);
		}
		this.active[id] = soundHandler2;
	}

	// Token: 0x06004A7A RID: 19066 RVA: 0x0015F75C File Offset: 0x0015D95C
	private void ApplyVolumes()
	{
		float num = 1f - this.overrideBlend;
		SoundHandler soundHandler;
		if (!string.IsNullOrEmpty(this.pairId1) && this.active.TryGetValue(this.pairId1, out soundHandler) && soundHandler != null && soundHandler.IsActive)
		{
			float num2 = ((this.pairId1 == this.pairId2 || string.IsNullOrEmpty(this.pairId2)) ? num : ((1f - this.pairLerp) * num));
			soundHandler.SetVolume(num2);
		}
		SoundHandler soundHandler2;
		if (!string.IsNullOrEmpty(this.pairId2) && this.pairId2 != this.pairId1 && this.active.TryGetValue(this.pairId2, out soundHandler2) && soundHandler2 != null && soundHandler2.IsActive)
		{
			soundHandler2.SetVolume(this.pairLerp * num);
		}
		SoundHandler soundHandler3;
		if (!string.IsNullOrEmpty(this.overrideId) && this.active.TryGetValue(this.overrideId, out soundHandler3) && soundHandler3 != null && soundHandler3.IsActive)
		{
			if (this.overrideId != this.pairId1 && this.overrideId != this.pairId2)
			{
				soundHandler3.SetVolume(this.overrideBlend);
				return;
			}
			soundHandler3.SetVolume(Mathf.Max(this.GetPairVolume(this.overrideId) * num, this.overrideBlend));
		}
	}

	// Token: 0x06004A7B RID: 19067 RVA: 0x0015F8B0 File Offset: 0x0015DAB0
	private float GetPairVolume(string id)
	{
		if (id == this.pairId1 && (this.pairId1 == this.pairId2 || string.IsNullOrEmpty(this.pairId2)))
		{
			return 1f;
		}
		if (id == this.pairId1)
		{
			return 1f - this.pairLerp;
		}
		if (id == this.pairId2)
		{
			return this.pairLerp;
		}
		return 0f;
	}

	// Token: 0x06004A7C RID: 19068 RVA: 0x0015F928 File Offset: 0x0015DB28
	private void FadeOutUnusedPairSounds(string newId1, string newId2)
	{
		float num = 1f - this.overrideBlend;
		List<string> list = null;
		foreach (KeyValuePair<string, SoundHandler> keyValuePair in this.active)
		{
			string key = keyValuePair.Key;
			if (!(key == newId1) && !(key == newId2) && !(key == this.overrideId))
			{
				bool flag = false;
				for (int i = 0; i < this.fadingOut.Count; i++)
				{
					if (this.fadingOut[i].id == key)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					float num2 = this.GetPairVolume(key) * num;
					if (num2 < 0.001f)
					{
						this.CacheAndStop(key, keyValuePair.Value);
					}
					else
					{
						this.fadingOut.Add(new AmbientSoundMixer.FadingOutEntry
						{
							id = key,
							handler = keyValuePair.Value,
							volume = num2
						});
					}
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(key);
				}
			}
		}
		if (list == null)
		{
			return;
		}
		for (int j = 0; j < list.Count; j++)
		{
			this.active.Remove(list[j]);
		}
	}

	// Token: 0x06004A7D RID: 19069 RVA: 0x0015FA98 File Offset: 0x0015DC98
	private void HardStopUnusedPairSounds(string newId1, string newId2, string oldId1, string oldId2)
	{
		List<string> list = null;
		foreach (KeyValuePair<string, SoundHandler> keyValuePair in this.active)
		{
			string key = keyValuePair.Key;
			if (!(key == newId1) && !(key == newId2) && !(key == this.overrideId))
			{
				this.CacheAndStop(key, keyValuePair.Value);
				if (list == null)
				{
					list = new List<string>();
				}
				list.Add(key);
			}
		}
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.active.Remove(list[i]);
			}
		}
		for (int j = this.fadingOut.Count - 1; j >= 0; j--)
		{
			string id = this.fadingOut[j].id;
			if ((!(id != oldId1) || !(id != oldId2)) && !(id == newId1) && !(id == newId2) && !(id == this.overrideId))
			{
				this.CacheAndStop(id, this.fadingOut[j].handler);
				this.fadingOut.RemoveAt(j);
			}
		}
	}

	// Token: 0x06004A7E RID: 19070 RVA: 0x0015FBE8 File Offset: 0x0015DDE8
	private void CacheAndStop(string id, SoundHandler handler)
	{
		if (handler != null && handler.IsActive)
		{
			this.playTimeCache[id] = handler.GetTime();
			handler.Stop();
		}
	}

	// Token: 0x04003A4D RID: 14925
	private readonly Dictionary<string, SoundHandler> active = new Dictionary<string, SoundHandler>();

	// Token: 0x04003A4E RID: 14926
	private readonly Dictionary<string, float> playTimeCache = new Dictionary<string, float>();

	// Token: 0x04003A4F RID: 14927
	private readonly List<AmbientSoundMixer.FadingOutEntry> fadingOut = new List<AmbientSoundMixer.FadingOutEntry>();

	// Token: 0x04003A50 RID: 14928
	private string pairId1;

	// Token: 0x04003A51 RID: 14929
	private string pairId2;

	// Token: 0x04003A52 RID: 14930
	private float pairLerp;

	// Token: 0x04003A53 RID: 14931
	private string overrideId;

	// Token: 0x04003A54 RID: 14932
	private float overrideBlend;

	// Token: 0x04003A55 RID: 14933
	private float switchDuration = 5f;

	// Token: 0x04003A56 RID: 14934
	private float overrideTarget;

	// Token: 0x02000ABF RID: 2751
	private struct FadingOutEntry
	{
		// Token: 0x04003A57 RID: 14935
		public string id;

		// Token: 0x04003A58 RID: 14936
		public SoundHandler handler;

		// Token: 0x04003A59 RID: 14937
		public float volume;
	}
}
