using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace LazyBearTechnology
{
	// Token: 0x020000C8 RID: 200
	[CreateAssetMenu(fileName = "AudioConfig", menuName = "Lazy/AudioConfig", order = 1)]
	public class AudioConfig : LazySingletonSO<AudioConfig>
	{
		// Token: 0x0600035F RID: 863 RVA: 0x00011CA4 File Offset: 0x0000FEA4
		public Sound Get(string id)
		{
			return this.sounds.Find((Sound x) => x.id == id);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00011CD8 File Offset: 0x0000FED8
		public AudioConfigData CreateConfigData()
		{
			AudioConfigData audioConfigData = new AudioConfigData();
			foreach (Sound sound in this.sounds)
			{
				SoundData soundData = new SoundData
				{
					id = sound.id,
					volume = sound.volume,
					panning = sound.panning
				};
				foreach (Sample sample in sound.samples)
				{
					soundData.samples.Add(new SampleData
					{
						clipName = ((sample.clip != null) ? sample.clip.name : ""),
						volume = sample.volume,
						pitch = sample.pitch,
						panning = sample.panning,
						pitchVariation = sample.pitchVariation
					});
				}
				audioConfigData.sounds.Add(soundData);
			}
			foreach (Playlist playlist in this.playlists)
			{
				PlaylistData playlistData = new PlaylistData
				{
					id = playlist.id,
					volume = playlist.volume
				};
				foreach (Track track in playlist.tracks)
				{
					playlistData.tracks.Add(new TrackData
					{
						clipName = ((track.clip != null) ? track.clip.name : ""),
						volume = track.volume,
						pitch = track.pitch,
						panning = track.panning
					});
				}
				audioConfigData.playlists.Add(playlistData);
			}
			return audioConfigData;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00011F20 File Offset: 0x00010120
		public void ApplyConfigData(AudioConfigData config)
		{
			if (config == null)
			{
				return;
			}
			using (List<SoundData>.Enumerator enumerator = config.sounds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SoundData soundData = enumerator.Current;
					Sound sound = this.sounds.Find((Sound s) => s.id == soundData.id);
					if (sound != null)
					{
						sound.volume = soundData.volume;
						sound.panning = soundData.panning;
						using (List<SampleData>.Enumerator enumerator2 = soundData.samples.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								SampleData sampleData = enumerator2.Current;
								Sample sample = sound.samples.Find((Sample s) => s.clip != null && s.clip.name == sampleData.clipName);
								if (sample != null)
								{
									sample.volume = sampleData.volume;
									sample.pitch = sampleData.pitch;
									sample.panning = sampleData.panning;
									sample.pitchVariation = sampleData.pitchVariation;
								}
							}
						}
					}
				}
			}
			using (List<PlaylistData>.Enumerator enumerator3 = config.playlists.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					PlaylistData playlistData = enumerator3.Current;
					Playlist playlist = this.playlists.Find((Playlist p) => p.id == playlistData.id);
					if (playlist != null)
					{
						playlist.volume = playlistData.volume;
						using (List<TrackData>.Enumerator enumerator4 = playlistData.tracks.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								TrackData trackData = enumerator4.Current;
								Track track = playlist.tracks.Find((Track t) => t.clip != null && t.clip.name == trackData.clipName);
								if (track != null)
								{
									track.volume = trackData.volume;
									track.pitch = trackData.pitch;
									track.panning = trackData.panning;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000118 RID: 280
		public AudioClip defaultClip;

		// Token: 0x04000119 RID: 281
		public AudioMixerGroup voiceOverGroup;

		// Token: 0x0400011A RID: 282
		public AudioMixerGroup mumbleGroup;

		// Token: 0x0400011B RID: 283
		public AudioMixer audioMixer;

		// Token: 0x0400011C RID: 284
		public List<Sound> sounds;

		// Token: 0x0400011D RID: 285
		[Space(20f)]
		public List<Playlist> playlists;

		// Token: 0x0400011E RID: 286
		public float dBQuietestValue = -36f;

		// Token: 0x0400011F RID: 287
		public const string JSON_CONFIG_PATH = "/AudioConfigData.json";

		// Token: 0x04000120 RID: 288
		private const string CONFIGS_FOLDER = "AudioConfigs";
	}
}
