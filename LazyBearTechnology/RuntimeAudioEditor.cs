using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000D8 RID: 216
	public class RuntimeAudioEditor : MonoBehaviour
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0001257B File Offset: 0x0001077B
		private string ConfigsPath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "AudioConfigs");
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001258C File Offset: 0x0001078C
		public static void ToggleWindow()
		{
			if (RuntimeAudioEditor.instance == null)
			{
				RuntimeAudioEditor.CreateInstance();
				return;
			}
			RuntimeAudioEditor.instance.Toggle();
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000125AB File Offset: 0x000107AB
		private static void CreateInstance()
		{
			GameObject gameObject = new GameObject("RuntimeAudioEditor");
			global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
			RuntimeAudioEditor.instance = gameObject.AddComponent<RuntimeAudioEditor>();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000125C8 File Offset: 0x000107C8
		private void Awake()
		{
			this.config = LazySingletonSO<AudioConfig>.Instance;
			this.originalConfig = this.config.CreateConfigData();
			this.EnsureConfigsDirectoryExists();
			this.previewAudioSource = base.gameObject.AddComponent<AudioSource>();
			this.previewAudioSource.playOnAwake = false;
			this.windowRect = new Rect(((float)Screen.width - 900f) / 2f, ((float)Screen.height - 650f) / 2f, 900f, 650f);
			this.RefreshConfigList();
			this.isVisible = true;
			string text = "[RuntimeAudioEditor] Initialized with {0} sounds and {1} playlists";
			List<Sound> sounds = this.config.sounds;
			object obj = ((sounds != null) ? sounds.Count : 0);
			List<Playlist> playlists = this.config.playlists;
			Debug.Log(string.Format(text, obj, (playlists != null) ? playlists.Count : 0));
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000126A1 File Offset: 0x000108A1
		private void EnsureConfigsDirectoryExists()
		{
			if (!Directory.Exists(this.ConfigsPath))
			{
				Directory.CreateDirectory(this.ConfigsPath);
				Debug.Log("[RuntimeAudioEditor] Created configs directory: " + this.ConfigsPath);
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000126D1 File Offset: 0x000108D1
		private void OnDestroy()
		{
			this.StopPreview();
			if (RuntimeAudioEditor.instance == this)
			{
				RuntimeAudioEditor.instance = null;
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000126EC File Offset: 0x000108EC
		private void Update()
		{
			if (!string.IsNullOrEmpty(this.statusMessage) && Time.time - this.statusTime > 3f)
			{
				this.statusMessage = "";
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00012719 File Offset: 0x00010919
		public void Toggle()
		{
			this.isVisible = !this.isVisible;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001272C File Offset: 0x0001092C
		private void OnGUI()
		{
			if (!this.isVisible)
			{
				return;
			}
			this.InitStyles();
			this.windowRect.x = Mathf.Clamp(this.windowRect.x, 0f, (float)Screen.width - this.windowRect.width);
			this.windowRect.y = Mathf.Clamp(this.windowRect.y, 0f, (float)Screen.height - this.windowRect.height);
			GUI.Box(this.windowRect, "", this.windowStyle);
			GUILayout.BeginArea(this.windowRect);
			this.DrawHeader();
			this.DrawTabs();
			this.DrawSearch();
			this.DrawContent();
			this.DrawFooter();
			GUILayout.EndArea();
			this.HandleDragging();
			this.UpdateLiveAudio();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000127FC File Offset: 0x000109FC
		private void InitStyles()
		{
			if (this.stylesInitialized)
			{
				return;
			}
			this.stylesInitialized = true;
			this.windowStyle = new GUIStyle(GUI.skin.box);
			this.windowStyle.normal.background = this.MakeTexture(2, 2, new Color(0.15f, 0.15f, 0.18f, 0.98f));
			this.headerStyle = new GUIStyle(GUI.skin.label);
			this.headerStyle.fontSize = 18;
			this.headerStyle.fontStyle = FontStyle.Bold;
			this.headerStyle.normal.textColor = Color.white;
			this.headerStyle.alignment = TextAnchor.MiddleLeft;
			this.tabActiveStyle = new GUIStyle(GUI.skin.button);
			this.tabActiveStyle.normal.background = this.MakeTexture(2, 2, new Color(0.3f, 0.5f, 0.7f));
			this.tabActiveStyle.normal.textColor = Color.white;
			this.tabActiveStyle.fontStyle = FontStyle.Bold;
			this.tabActiveStyle.fontSize = 13;
			this.tabInactiveStyle = new GUIStyle(GUI.skin.button);
			this.tabInactiveStyle.normal.background = this.MakeTexture(2, 2, new Color(0.25f, 0.25f, 0.28f));
			this.tabInactiveStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
			this.tabInactiveStyle.fontSize = 13;
			this.itemStyle = new GUIStyle(GUI.skin.box);
			this.itemStyle.normal.background = this.MakeTexture(2, 2, new Color(0.22f, 0.22f, 0.25f));
			this.itemStyle.margin = new RectOffset(0, 0, 2, 2);
			this.itemStyle.padding = new RectOffset(8, 8, 6, 6);
			this.sampleStyle = new GUIStyle(GUI.skin.box);
			this.sampleStyle.normal.background = this.MakeTexture(2, 2, new Color(0.18f, 0.18f, 0.21f));
			this.sampleStyle.margin = new RectOffset(20, 0, 1, 1);
			this.sampleStyle.padding = new RectOffset(8, 8, 4, 4);
			this.itemStylePlaying = new GUIStyle(GUI.skin.box);
			this.itemStylePlaying.normal.background = this.MakeTexture(2, 2, new Color(0.15f, 0.35f, 0.2f));
			this.itemStylePlaying.margin = new RectOffset(0, 0, 2, 2);
			this.itemStylePlaying.padding = new RectOffset(8, 8, 6, 6);
			this.sampleStylePlaying = new GUIStyle(GUI.skin.box);
			this.sampleStylePlaying.normal.background = this.MakeTexture(2, 2, new Color(0.12f, 0.3f, 0.18f));
			this.sampleStylePlaying.margin = new RectOffset(20, 0, 1, 1);
			this.sampleStylePlaying.padding = new RectOffset(8, 8, 4, 4);
			this.labelStyle = new GUIStyle(GUI.skin.label);
			this.labelStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
			this.labelStyle.fontSize = 12;
			this.smallButtonStyle = new GUIStyle(GUI.skin.button);
			this.smallButtonStyle.fontSize = 14;
			this.smallButtonStyle.fixedWidth = 28f;
			this.smallButtonStyle.fixedHeight = 22f;
			this.foldoutStyle = new GUIStyle(GUI.skin.label);
			this.foldoutStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f);
			this.foldoutStyle.fontStyle = FontStyle.Bold;
			this.foldoutStyle.fontSize = 13;
			this.statusStyle = new GUIStyle(GUI.skin.label);
			this.statusStyle.fontSize = 11;
			this.statusStyle.alignment = TextAnchor.MiddleLeft;
			this.searchStyle = new GUIStyle(GUI.skin.textField);
			this.searchStyle.fontSize = 12;
			this.valueFieldStyle = new GUIStyle(GUI.skin.textField);
			this.valueFieldStyle.fontSize = 11;
			this.valueFieldStyle.alignment = TextAnchor.MiddleCenter;
			this.valueFieldStyle.fixedHeight = 18f;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00012CA0 File Offset: 0x00010EA0
		private float FloatSliderWithInput(float value, float min, float max, float sliderWidth, float fieldWidth)
		{
			float num = GUILayout.HorizontalSlider(value, min, max, new GUILayoutOption[] { GUILayout.Width(sliderWidth) });
			string text = value.ToString("F3");
			string text2 = GUILayout.TextField(text, this.valueFieldStyle, new GUILayoutOption[] { GUILayout.Width(fieldWidth) });
			if (text2 != text)
			{
				text2 = text2.Replace(',', '.');
				float num2;
				if (float.TryParse(text2, NumberStyles.Float, CultureInfo.InvariantCulture, out num2))
				{
					return Mathf.Clamp(num2, min, max);
				}
			}
			if (Mathf.Abs(num - value) > 0.0001f)
			{
				return num;
			}
			return value;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00012D34 File Offset: 0x00010F34
		private Texture2D MakeTexture(int width, int height, Color color)
		{
			Color[] array = new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = color;
			}
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00012D74 File Offset: 0x00010F74
		private void HandleDragging()
		{
			Rect rect = new Rect(this.windowRect.x, this.windowRect.y, this.windowRect.width - 40f, 35f);
			Event current = Event.current;
			if (current.type == EventType.MouseDown && rect.Contains(current.mousePosition))
			{
				this.isDragging = true;
				this.dragOffset = current.mousePosition - new Vector2(this.windowRect.x, this.windowRect.y);
				current.Use();
				return;
			}
			if (current.type == EventType.MouseUp)
			{
				this.isDragging = false;
				return;
			}
			if (current.type == EventType.MouseDrag && this.isDragging)
			{
				this.windowRect.x = current.mousePosition.x - this.dragOffset.x;
				this.windowRect.y = current.mousePosition.y - this.dragOffset.y;
				current.Use();
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00012E78 File Offset: 0x00011078
		private void DrawHeader()
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(15f);
			GUILayout.Label("Audio Editor", this.headerStyle, new GUILayoutOption[] { GUILayout.Height(35f) });
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("✕", new GUILayoutOption[]
			{
				GUILayout.Width(30f),
				GUILayout.Height(25f)
			}))
			{
				this.isVisible = false;
			}
			GUILayout.Space(10f);
			GUILayout.EndHorizontal();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00012F04 File Offset: 0x00011104
		private void DrawTabs()
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(15f);
			if (GUILayout.Button("Sounds", (this.currentTab == 0) ? this.tabActiveStyle : this.tabInactiveStyle, new GUILayoutOption[]
			{
				GUILayout.Height(28f),
				GUILayout.Width(150f)
			}))
			{
				this.currentTab = 0;
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Playlists", (this.currentTab == 1) ? this.tabActiveStyle : this.tabInactiveStyle, new GUILayoutOption[]
			{
				GUILayout.Height(28f),
				GUILayout.Width(150f)
			}))
			{
				this.currentTab = 1;
			}
			GUILayout.FlexibleSpace();
			GUILayout.Space(15f);
			GUILayout.EndHorizontal();
			GUILayout.Space(8f);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00012FE0 File Offset: 0x000111E0
		private void DrawSearch()
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(15f);
			GUILayout.Label("Search:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(50f) });
			this.searchText = GUILayout.TextField(this.searchText, this.searchStyle, new GUILayoutOption[] { GUILayout.Height(22f) });
			GUILayout.Space(15f);
			GUILayout.EndHorizontal();
			GUILayout.Space(8f);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00013068 File Offset: 0x00011268
		private void DrawContent()
		{
			GUILayout.BeginArea(new Rect(15f, 110f, this.windowRect.width - 30f, this.windowRect.height - 230f));
			if (this.currentTab == 0)
			{
				this.DrawSoundsTab();
			}
			else
			{
				this.DrawPlaylistsTab();
			}
			GUILayout.EndArea();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000130C8 File Offset: 0x000112C8
		private void DrawSoundsTab()
		{
			this.soundsScroll = GUILayout.BeginScrollView(this.soundsScroll, Array.Empty<GUILayoutOption>());
			string text = this.searchText.ToLower();
			foreach (Sound sound in this.config.sounds)
			{
				if (string.IsNullOrEmpty(text) || (!string.IsNullOrEmpty(sound.id) && sound.id.ToLower().Contains(text)))
				{
					this.DrawSoundItem(sound);
				}
			}
			GUILayout.EndScrollView();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00013170 File Offset: 0x00011370
		private void DrawSoundItem(Sound sound)
		{
			GUILayout.BeginVertical((this.previewAudioSource != null && this.previewAudioSource.isPlaying && this.playingSound == sound) ? this.itemStylePlaying : this.itemStyle, Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag = this.expandedSounds.Contains(sound.id);
			if (GUILayout.Button(flag ? "▼" : "►", this.smallButtonStyle, Array.Empty<GUILayoutOption>()))
			{
				if (flag)
				{
					this.expandedSounds.Remove(sound.id);
				}
				else
				{
					this.expandedSounds.Add(sound.id);
				}
			}
			if (GUILayout.Button("▶", this.smallButtonStyle, Array.Empty<GUILayoutOption>()))
			{
				this.PlaySound(sound);
			}
			if (GUILayout.Button("■", this.smallButtonStyle, Array.Empty<GUILayoutOption>()))
			{
				this.StopPreview();
			}
			GUILayout.Label(sound.id, this.foldoutStyle, new GUILayoutOption[] { GUILayout.Width(180f) });
			GUILayout.Label("Vol:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(30f) });
			sound.volume = this.FloatSliderWithInput(sound.volume, 0f, 1f, 100f, 40f);
			GUILayout.Label("Pan:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(30f) });
			sound.panning = this.FloatSliderWithInput(sound.panning, -1f, 1f, 80f, 45f);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			if (this.expandedSounds.Contains(sound.id) && sound.samples != null)
			{
				GUILayout.Space(4f);
				foreach (Sample sample in sound.samples)
				{
					this.DrawSampleItem(sample, sound);
				}
			}
			GUILayout.EndVertical();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00013390 File Offset: 0x00011590
		private void DrawSampleItem(Sample sample, Sound parentSound)
		{
			GUILayout.BeginHorizontal((this.previewAudioSource != null && this.previewAudioSource.isPlaying && this.playingSample == sample) ? this.sampleStylePlaying : this.sampleStyle, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("▶", this.smallButtonStyle, Array.Empty<GUILayoutOption>()))
			{
				this.PlaySample(sample, parentSound);
			}
			GUILayout.Label((sample.clip != null) ? sample.clip.name : "(no clip)", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(150f) });
			GUILayout.Label("Vol:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(28f) });
			sample.volume = this.FloatSliderWithInput(sample.volume, 0f, 1f, 60f, 38f);
			GUILayout.Label("Pitch:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(35f) });
			sample.pitch = this.FloatSliderWithInput(sample.pitch, -3f, 3f, 60f, 38f);
			GUILayout.Label("Pan:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(28f) });
			sample.panning = this.FloatSliderWithInput(sample.panning, -1f, 1f, 50f, 38f);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00013520 File Offset: 0x00011720
		private void DrawPlaylistsTab()
		{
			this.playlistsScroll = GUILayout.BeginScrollView(this.playlistsScroll, Array.Empty<GUILayoutOption>());
			string text = this.searchText.ToLower();
			foreach (Playlist playlist in this.config.playlists)
			{
				if (string.IsNullOrEmpty(text) || (!string.IsNullOrEmpty(playlist.id) && playlist.id.ToLower().Contains(text)))
				{
					this.DrawPlaylistItem(playlist);
				}
			}
			GUILayout.EndScrollView();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000135C8 File Offset: 0x000117C8
		private void DrawPlaylistItem(Playlist playlist)
		{
			GUILayout.BeginVertical((this.previewAudioSource != null && this.previewAudioSource.isPlaying && this.playingPlaylist == playlist) ? this.itemStylePlaying : this.itemStyle, Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag = this.expandedPlaylists.Contains(playlist.id);
			if (GUILayout.Button(flag ? "▼" : "►", this.smallButtonStyle, Array.Empty<GUILayoutOption>()))
			{
				if (flag)
				{
					this.expandedPlaylists.Remove(playlist.id);
				}
				else
				{
					this.expandedPlaylists.Add(playlist.id);
				}
			}
			if (GUILayout.Button("▶", this.smallButtonStyle, Array.Empty<GUILayoutOption>()))
			{
				this.PlayPlaylist(playlist);
			}
			if (GUILayout.Button("■", this.smallButtonStyle, Array.Empty<GUILayoutOption>()))
			{
				this.StopPreview();
			}
			GUILayout.Label(playlist.id, this.foldoutStyle, new GUILayoutOption[] { GUILayout.Width(200f) });
			GUILayout.Label("Vol:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(30f) });
			playlist.volume = this.FloatSliderWithInput(playlist.volume, 0f, 1f, 120f, 45f);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			if (this.expandedPlaylists.Contains(playlist.id) && playlist.tracks != null)
			{
				GUILayout.Space(4f);
				foreach (Track track in playlist.tracks)
				{
					this.DrawTrackItem(track, playlist);
				}
			}
			GUILayout.EndVertical();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000137A0 File Offset: 0x000119A0
		private void DrawTrackItem(Track track, Playlist parentPlaylist)
		{
			GUILayout.BeginHorizontal((this.previewAudioSource != null && this.previewAudioSource.isPlaying && this.playingTrack == track) ? this.sampleStylePlaying : this.sampleStyle, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("▶", this.smallButtonStyle, Array.Empty<GUILayoutOption>()))
			{
				this.PlayTrack(track, parentPlaylist);
			}
			GUILayout.Label((!string.IsNullOrEmpty(track.id)) ? track.id : ((track.clip != null) ? track.clip.name : "(no clip)"), this.labelStyle, new GUILayoutOption[] { GUILayout.Width(150f) });
			GUILayout.Label("Vol:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(28f) });
			track.volume = this.FloatSliderWithInput(track.volume, 0f, 1f, 60f, 38f);
			GUILayout.Label("Pitch:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(35f) });
			track.pitch = this.FloatSliderWithInput(track.pitch, -3f, 3f, 60f, 38f);
			GUILayout.Label("Pan:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(28f) });
			track.panning = this.FloatSliderWithInput(track.panning, -1f, 1f, 50f, 38f);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00013948 File Offset: 0x00011B48
		private void DrawFooter()
		{
			GUILayout.BeginArea(new Rect(15f, this.windowRect.height - 120f, this.windowRect.width - 30f, 115f));
			if (!string.IsNullOrEmpty(this.statusMessage))
			{
				this.statusStyle.normal.textColor = (this.statusIsError ? new Color(1f, 0.5f, 0.5f) : new Color(0.5f, 1f, 0.5f));
				GUILayout.Label(this.statusMessage, this.statusStyle, Array.Empty<GUILayoutOption>());
			}
			else
			{
				GUILayout.Label("", new GUILayoutOption[] { GUILayout.Height(16f) });
			}
			GUILayout.Space(3f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Save as:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(55f) });
			this.configName = GUILayout.TextField(this.configName, new GUILayoutOption[]
			{
				GUILayout.Width(200f),
				GUILayout.Height(22f)
			});
			GUILayout.Space(5f);
			if (GUILayout.Button("Save", new GUILayoutOption[]
			{
				GUILayout.Width(70f),
				GUILayout.Height(24f)
			}))
			{
				this.SaveConfig();
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Apply Loaded", new GUILayoutOption[]
			{
				GUILayout.Width(100f),
				GUILayout.Height(24f)
			}))
			{
				this.ApplyLoadedConfig();
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Reset to Original", new GUILayoutOption[]
			{
				GUILayout.Width(115f),
				GUILayout.Height(24f)
			}))
			{
				this.ResetToOriginal();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(3f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Load:", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(55f) });
			if (this.configFiles.Count > 0)
			{
				this.selectedConfigIndex = Mathf.Clamp(this.selectedConfigIndex, 0, this.configFiles.Count - 1);
				this.selectedConfigIndex = this.EditorPopup(this.selectedConfigIndex, this.configFiles.ToArray(), new GUILayoutOption[]
				{
					GUILayout.Width(200f),
					GUILayout.Height(22f)
				});
			}
			else
			{
				GUILayout.Label("(no configs)", this.labelStyle, new GUILayoutOption[] { GUILayout.Width(200f) });
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Load", new GUILayoutOption[]
			{
				GUILayout.Width(70f),
				GUILayout.Height(24f)
			}))
			{
				this.LoadConfig();
			}
			if (GUILayout.Button("↻", new GUILayoutOption[]
			{
				GUILayout.Width(28f),
				GUILayout.Height(24f)
			}))
			{
				this.RefreshConfigList();
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Open Folder", new GUILayoutOption[]
			{
				GUILayout.Width(85f),
				GUILayout.Height(24f)
			}))
			{
				this.OpenConfigsFolder();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00013CA0 File Offset: 0x00011EA0
		private void OpenConfigsFolder()
		{
			this.EnsureConfigsDirectoryExists();
			Application.OpenURL("file://" + this.ConfigsPath);
			this.SetStatus("Opened: " + this.ConfigsPath, false);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00013CD4 File Offset: 0x00011ED4
		private int EditorPopup(int selectedIndex, string[] options, params GUILayoutOption[] layoutOptions)
		{
			if (GUILayout.Button(((options.Length != 0 && selectedIndex >= 0 && selectedIndex < options.Length) ? options[selectedIndex] : "(none)") + " ▾", layoutOptions))
			{
				selectedIndex = (selectedIndex + 1) % options.Length;
			}
			return selectedIndex;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00013D0C File Offset: 0x00011F0C
		private void PlaySound(Sound sound)
		{
			this.ClearPlayingReferences();
			if (sound.samples == null || sound.samples.Count == 0)
			{
				this.playingSound = sound;
				this.PlayClip(this.config.defaultClip, sound.volume, sound.panning, 1f, sound.loop);
				return;
			}
			Sample randomSample = sound.RandomSample;
			if (randomSample == null || randomSample.clip == null)
			{
				this.playingSound = sound;
				this.PlayClip(this.config.defaultClip, sound.volume, sound.panning, 1f, sound.loop);
				return;
			}
			this.playingSound = sound;
			this.playingSample = randomSample;
			float num = Mathf.Clamp(randomSample.panning + sound.panning, -1f, 1f);
			this.PlayClip(randomSample.clip, randomSample.volume * sound.volume, num, randomSample.Pitch, sound.loop);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00013E00 File Offset: 0x00012000
		private void PlaySample(Sample sample, Sound parentSound)
		{
			this.ClearPlayingReferences();
			this.playingSound = parentSound;
			this.playingSample = sample;
			if (sample.clip == null)
			{
				this.PlayClip(this.config.defaultClip, sample.volume * parentSound.volume, sample.panning, 1f, false);
				return;
			}
			float num = Mathf.Clamp(sample.panning + parentSound.panning, -1f, 1f);
			this.PlayClip(sample.clip, sample.volume * parentSound.volume, num, sample.Pitch, false);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00013E98 File Offset: 0x00012098
		private void PlayPlaylist(Playlist playlist)
		{
			this.ClearPlayingReferences();
			if (playlist.tracks == null || playlist.tracks.Count == 0)
			{
				return;
			}
			Track track = playlist.tracks[0];
			if (track.clip == null)
			{
				return;
			}
			this.playingPlaylist = playlist;
			this.playingTrack = track;
			this.PlayClip(track.clip, track.volume * playlist.volume, track.panning, track.pitch, false);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00013F14 File Offset: 0x00012114
		private void PlayTrack(Track track, Playlist parentPlaylist)
		{
			this.ClearPlayingReferences();
			if (track.clip == null)
			{
				return;
			}
			this.playingPlaylist = parentPlaylist;
			this.playingTrack = track;
			this.PlayClip(track.clip, track.volume * parentPlaylist.volume, track.panning, track.pitch, false);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00013F6C File Offset: 0x0001216C
		private void PlayClip(AudioClip clip, float volume, float panning, float pitch, bool loop)
		{
			if (clip == null)
			{
				Debug.LogWarning("[RuntimeAudioEditor] Cannot play null clip");
				return;
			}
			this.previewAudioSource.clip = clip;
			this.previewAudioSource.volume = volume;
			this.previewAudioSource.panStereo = panning;
			this.previewAudioSource.pitch = pitch;
			this.previewAudioSource.loop = loop;
			this.previewAudioSource.Play();
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00013FD6 File Offset: 0x000121D6
		private void StopPreview()
		{
			this.ClearPlayingReferences();
			if (this.previewAudioSource != null && this.previewAudioSource.isPlaying)
			{
				this.previewAudioSource.Stop();
				this.previewAudioSource.clip = null;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00014010 File Offset: 0x00012210
		private void ClearPlayingReferences()
		{
			this.playingSound = null;
			this.playingSample = null;
			this.playingPlaylist = null;
			this.playingTrack = null;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00014030 File Offset: 0x00012230
		private void UpdateLiveAudio()
		{
			if (this.previewAudioSource == null || !this.previewAudioSource.isPlaying)
			{
				return;
			}
			if (this.playingSample != null && this.playingSound != null)
			{
				float num = Mathf.Clamp(this.playingSample.panning + this.playingSound.panning, -1f, 1f);
				this.previewAudioSource.volume = this.playingSample.volume * this.playingSound.volume;
				this.previewAudioSource.panStereo = num;
				this.previewAudioSource.pitch = this.playingSample.Pitch;
				return;
			}
			if (this.playingSound != null)
			{
				this.previewAudioSource.volume = this.playingSound.volume;
				this.previewAudioSource.panStereo = this.playingSound.panning;
				return;
			}
			if (this.playingTrack != null && this.playingPlaylist != null)
			{
				this.previewAudioSource.volume = this.playingTrack.volume * this.playingPlaylist.volume;
				this.previewAudioSource.panStereo = this.playingTrack.panning;
				this.previewAudioSource.pitch = this.playingTrack.pitch;
				return;
			}
			if (this.playingPlaylist != null)
			{
				this.previewAudioSource.volume = this.playingPlaylist.volume;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00014188 File Offset: 0x00012388
		private void RefreshConfigList()
		{
			this.EnsureConfigsDirectoryExists();
			this.configFiles = (from f in Directory.GetFiles(this.ConfigsPath, "*.json")
				select Path.GetFileNameWithoutExtension(f) into f
				orderby f
				select f).ToList<string>();
			if (this.configFiles.Count > 0 && this.selectedConfigIndex >= this.configFiles.Count)
			{
				this.selectedConfigIndex = 0;
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00014228 File Offset: 0x00012428
		private void SaveConfig()
		{
			string text = this.configName;
			string text2 = ((text != null) ? text.Trim() : null);
			if (string.IsNullOrEmpty(text2))
			{
				this.SetStatus("Enter a config name", true);
				return;
			}
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				text2 = text2.Replace(c, '_');
			}
			string text3 = Path.Combine(this.ConfigsPath, text2 + ".json");
			try
			{
				string text4 = JsonUtility.ToJson(this.config.CreateConfigData(), true);
				File.WriteAllText(text3, text4);
				this.SetStatus("Saved: " + text2, false);
				Debug.Log("[RuntimeAudioEditor] Saved config to: " + text3);
				this.RefreshConfigList();
				int num = this.configFiles.IndexOf(text2);
				if (num >= 0)
				{
					this.selectedConfigIndex = num;
				}
			}
			catch (Exception ex)
			{
				this.SetStatus("Save failed: " + ex.Message, true);
				Debug.LogError("[RuntimeAudioEditor] Save failed: " + ex.Message);
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001433C File Offset: 0x0001253C
		private void LoadConfig()
		{
			if (this.configFiles.Count == 0 || this.selectedConfigIndex < 0 || this.selectedConfigIndex >= this.configFiles.Count)
			{
				this.SetStatus("No config selected", true);
				return;
			}
			string text = this.configFiles[this.selectedConfigIndex];
			string text2 = Path.Combine(this.ConfigsPath, text + ".json");
			if (!File.Exists(text2))
			{
				this.SetStatus("Config not found: " + text, true);
				this.RefreshConfigList();
				return;
			}
			try
			{
				string text3 = File.ReadAllText(text2);
				this.loadedConfig = JsonUtility.FromJson<AudioConfigData>(text3);
				this.loadedConfigName = text;
				this.SetStatus("Loaded: " + text + " (click Apply to use)", false);
				Debug.Log("[RuntimeAudioEditor] Loaded config from: " + text2);
			}
			catch (Exception ex)
			{
				this.SetStatus("Load failed: " + ex.Message, true);
				Debug.LogError("[RuntimeAudioEditor] Load failed: " + ex.Message);
				this.loadedConfig = null;
				this.loadedConfigName = null;
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001445C File Offset: 0x0001265C
		private void ApplyLoadedConfig()
		{
			if (this.loadedConfig == null)
			{
				this.SetStatus("Load a config first", true);
				return;
			}
			this.config.ApplyConfigData(this.loadedConfig);
			this.SetStatus("Applied: " + this.loadedConfigName, false);
			Debug.Log("[RuntimeAudioEditor] Applied config: " + this.loadedConfigName);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000144BB File Offset: 0x000126BB
		private void ResetToOriginal()
		{
			if (this.originalConfig == null)
			{
				this.SetStatus("No original config cached", true);
				return;
			}
			this.config.ApplyConfigData(this.originalConfig);
			this.SetStatus("Reset to original values", false);
			Debug.Log("[RuntimeAudioEditor] Reset to original values.");
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000144F9 File Offset: 0x000126F9
		private void SetStatus(string message, bool isError = false)
		{
			this.statusMessage = message;
			this.statusIsError = isError;
			this.statusTime = Time.time;
		}

		// Token: 0x0400016A RID: 362
		private static RuntimeAudioEditor instance;

		// Token: 0x0400016B RID: 363
		private AudioSource previewAudioSource;

		// Token: 0x0400016C RID: 364
		private AudioConfig config;

		// Token: 0x0400016D RID: 365
		private AudioConfigData originalConfig;

		// Token: 0x0400016E RID: 366
		private AudioConfigData loadedConfig;

		// Token: 0x0400016F RID: 367
		private string loadedConfigName;

		// Token: 0x04000170 RID: 368
		private bool isVisible;

		// Token: 0x04000171 RID: 369
		private int currentTab;

		// Token: 0x04000172 RID: 370
		private string searchText = "";

		// Token: 0x04000173 RID: 371
		private string configName = "AudioConfigData";

		// Token: 0x04000174 RID: 372
		private int selectedConfigIndex;

		// Token: 0x04000175 RID: 373
		private string statusMessage = "";

		// Token: 0x04000176 RID: 374
		private bool statusIsError;

		// Token: 0x04000177 RID: 375
		private float statusTime;

		// Token: 0x04000178 RID: 376
		private Vector2 soundsScroll;

		// Token: 0x04000179 RID: 377
		private Vector2 playlistsScroll;

		// Token: 0x0400017A RID: 378
		private HashSet<string> expandedSounds = new HashSet<string>();

		// Token: 0x0400017B RID: 379
		private HashSet<string> expandedPlaylists = new HashSet<string>();

		// Token: 0x0400017C RID: 380
		private List<string> configFiles = new List<string>();

		// Token: 0x0400017D RID: 381
		private Rect windowRect;

		// Token: 0x0400017E RID: 382
		private bool isDragging;

		// Token: 0x0400017F RID: 383
		private Vector2 dragOffset;

		// Token: 0x04000180 RID: 384
		private Sound playingSound;

		// Token: 0x04000181 RID: 385
		private Sample playingSample;

		// Token: 0x04000182 RID: 386
		private Playlist playingPlaylist;

		// Token: 0x04000183 RID: 387
		private Track playingTrack;

		// Token: 0x04000184 RID: 388
		private const string WINDOW_TITLE = "Audio Editor";

		// Token: 0x04000185 RID: 389
		private const float WINDOW_WIDTH = 900f;

		// Token: 0x04000186 RID: 390
		private const float WINDOW_HEIGHT = 650f;

		// Token: 0x04000187 RID: 391
		private const string CONFIGS_FOLDER = "AudioConfigs";

		// Token: 0x04000188 RID: 392
		private const string CONFIG_EXTENSION = ".json";

		// Token: 0x04000189 RID: 393
		private GUIStyle windowStyle;

		// Token: 0x0400018A RID: 394
		private GUIStyle headerStyle;

		// Token: 0x0400018B RID: 395
		private GUIStyle tabActiveStyle;

		// Token: 0x0400018C RID: 396
		private GUIStyle tabInactiveStyle;

		// Token: 0x0400018D RID: 397
		private GUIStyle itemStyle;

		// Token: 0x0400018E RID: 398
		private GUIStyle itemStylePlaying;

		// Token: 0x0400018F RID: 399
		private GUIStyle sampleStyle;

		// Token: 0x04000190 RID: 400
		private GUIStyle sampleStylePlaying;

		// Token: 0x04000191 RID: 401
		private GUIStyle labelStyle;

		// Token: 0x04000192 RID: 402
		private GUIStyle smallButtonStyle;

		// Token: 0x04000193 RID: 403
		private GUIStyle foldoutStyle;

		// Token: 0x04000194 RID: 404
		private GUIStyle statusStyle;

		// Token: 0x04000195 RID: 405
		private GUIStyle searchStyle;

		// Token: 0x04000196 RID: 406
		private GUIStyle valueFieldStyle;

		// Token: 0x04000197 RID: 407
		private bool stylesInitialized;
	}
}
