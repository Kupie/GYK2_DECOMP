using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace LazyBearTechnology
{
	// Token: 0x020000DF RID: 223
	[CreateAssetMenu(fileName = "SpriteCollection", menuName = "Lazy/SpriteCollection")]
	public class EasySpritesCollection : LazySingletonSO<EasySpritesCollection>
	{
		// Token: 0x060003CF RID: 975 RVA: 0x00014E2C File Offset: 0x0001302C
		public void Initialize()
		{
			if (EasySpritesCollection.isInitialized)
			{
				return;
			}
			EasySpritesCollection.isInitialized = true;
			foreach (SpriteAtlasInfo spriteAtlasInfo in this.atlasInfoList)
			{
				foreach (string text in spriteAtlasInfo.spriteNames)
				{
					if (this.hash.ContainsKey(text))
					{
						string text2;
						this.hash.TryGetValue(text, out text2);
						this.DoLog(string.Concat(new string[] { "#sprites# Sprite ", text, " from atlas ", spriteAtlasInfo.atlasName, " has duplicate (", text, " ", text2 }), LazyLogType.Error);
					}
					else
					{
						this.hash.Add(text, spriteAtlasInfo.atlasName);
					}
				}
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00014F48 File Offset: 0x00013148
		private void ScanAllAtlases()
		{
			this.atlasInfoList.Clear();
			string[] array = new string[] { ".spriteatlas", ".spriteatlasv2" };
			string text = this.addressablesSubDirectory;
			string text2 = "Assets/" + text;
			this.DoLog("#sprites# Start scanning \"" + text2 + "\" directory for SpriteAtlases", LazyLogType.Default);
			DirectoryInfo directoryInfo = new DirectoryInfo(text2);
			foreach (string text3 in array)
			{
				foreach (FileInfo fileInfo in directoryInfo.GetFiles("*" + text3, SearchOption.AllDirectories))
				{
					string text4 = fileInfo.Name.Split('.', StringSplitOptions.None)[0];
					int num = fileInfo.Directory.FullName.LastIndexOf(text, StringComparison.Ordinal);
					if (num != -1)
					{
						num += text.Length;
					}
					string text5 = fileInfo.Directory.FullName.Substring(num) + "/" + text4;
					text5 = text5.Replace('\\', '/');
					if (text5.StartsWith("/"))
					{
						text5 = text5.Substring(1);
					}
					text5 += text3;
					text5 = text2 + "/" + text5;
					SpriteAtlas spriteAtlas = this.LoadSpriteAtlasForScan(text5);
					if (spriteAtlas == null)
					{
						this.DoLog("#sprites# Cannot load SpriteAtlas at path: " + text5, LazyLogType.Error);
					}
					else
					{
						Sprite[] array3 = new Sprite[spriteAtlas.spriteCount];
						spriteAtlas.GetSprites(array3);
						this.DoLog(string.Format("#sprites# atlas found: {0}, sprites: {1}", text4, spriteAtlas.spriteCount), LazyLogType.Default);
						SpriteAtlasInfo spriteAtlasInfo = new SpriteAtlasInfo
						{
							atlasName = spriteAtlas.name,
							path = text5
						};
						foreach (Sprite sprite in array3)
						{
							if (!(sprite == null))
							{
								string text6 = sprite.name.Replace("(Clone)", "");
								if (spriteAtlasInfo.spriteNames.Contains(text6))
								{
									this.DoLog(string.Concat(new string[] { "#sprites# Cannot add sprite [", text6, "] to atlasData [", spriteAtlas.name, "]: name duplicate." }), LazyLogType.Error);
								}
								else
								{
									spriteAtlasInfo.spriteNames.Add(text6);
								}
							}
						}
						Addressables.Release<SpriteAtlas>(spriteAtlas);
						this.atlasInfoList.Add(spriteAtlasInfo);
					}
				}
			}
			this.DoLog(string.Format("#sprites# Scanning for SpriteAtlases is done. Total atlases: {0}", this.atlasInfoList.Count), LazyLogType.Default);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000151E0 File Offset: 0x000133E0
		private SpriteAtlas LoadSpriteAtlasForScan(string path)
		{
			return Addressables.LoadAssetAsync<SpriteAtlas>(path).WaitForCompletion();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000151FB File Offset: 0x000133FB
		public bool HasSprite(string spriteName)
		{
			if (string.IsNullOrEmpty(spriteName))
			{
				return false;
			}
			if (!EasySpritesCollection.isInitialized)
			{
				this.DoLog("#sprites# EasySpritesCollection not initialized yet. Initializing...", LazyLogType.Default);
				this.Initialize();
			}
			return this.hash.ContainsKey(spriteName);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001522C File Offset: 0x0001342C
		public Sprite GetSprite(string spriteName, string fallbackSpriteName = null)
		{
			if (!EasySpritesCollection.isInitialized)
			{
				this.DoLog("#sprites# EasySpritesCollection not initialized yet. Initializing...", LazyLogType.Default);
				this.Initialize();
			}
			string text;
			if (this.hash.TryGetValue(spriteName, out text))
			{
				return this.EnsureSpriteAtlasWithNameLoaded(text).GetSprite(spriteName);
			}
			if (!string.IsNullOrEmpty(fallbackSpriteName) && this.hash.TryGetValue(fallbackSpriteName, out text))
			{
				SpriteAtlas spriteAtlas = this.EnsureSpriteAtlasWithNameLoaded(text);
				if (EasySpritesCollection.fallbackSpriteLogType != LazyLogType.None)
				{
					this.DoLog(string.Concat(new string[] { "#sprites# Cannot find spriteName [", spriteName, "] in hash, using fallbackSpriteName [", fallbackSpriteName, "]." }), EasySpritesCollection.fallbackSpriteLogType);
				}
				return spriteAtlas.GetSprite(fallbackSpriteName);
			}
			if (EasySpritesCollection.spriteLogType != LazyLogType.None)
			{
				this.DoLog("#sprites# Cannot find spriteName [" + spriteName + "] in hash.", EasySpritesCollection.spriteLogType);
			}
			return null;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000152F8 File Offset: 0x000134F8
		public SpriteAtlas EnsureSpriteAtlasWithNameLoaded(string atlasName)
		{
			LoadedSpriteAtlasData loadedSpriteAtlasData;
			if (this.loadedAtlasesDataDictionary.TryGetValue(atlasName, out loadedSpriteAtlasData))
			{
				return loadedSpriteAtlasData.atlas;
			}
			foreach (SpriteAtlasInfo spriteAtlasInfo in this.atlasInfoList)
			{
				if (spriteAtlasInfo.atlasName == atlasName)
				{
					SpriteAtlas spriteAtlas = Addressables.LoadAssetAsync<SpriteAtlas>(spriteAtlasInfo.path).WaitForCompletion();
					if (this.loadedAtlasesDataDictionary.TryGetValue(spriteAtlas.name, out loadedSpriteAtlasData))
					{
						Addressables.Release<SpriteAtlas>(spriteAtlas);
						return loadedSpriteAtlasData.atlas;
					}
					loadedSpriteAtlasData = new LoadedSpriteAtlasData
					{
						atlas = spriteAtlas
					};
					this.loadedAtlasesDataDictionary.Add(loadedSpriteAtlasData.atlas.name, loadedSpriteAtlasData);
					return loadedSpriteAtlasData.atlas;
				}
			}
			this.DoLog("#sprites# Cannot find meta info about atlas with name: " + atlasName, LazyLogType.Error);
			return null;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000153EC File Offset: 0x000135EC
		public SpriteAtlas EnsureSpriteAtlasWithSpriteLoaded(string spriteName)
		{
			if (!EasySpritesCollection.isInitialized)
			{
				this.DoLog("#sprites# EasySpritesCollection not initialized yet. Initializing...", LazyLogType.Default);
				this.Initialize();
			}
			string text;
			if (this.hash.TryGetValue(spriteName, out text))
			{
				return this.EnsureSpriteAtlasWithNameLoaded(text);
			}
			this.DoLog("#sprites# Cannot find atlas for sprite name: " + spriteName, LazyLogType.Error);
			return null;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00015440 File Offset: 0x00013640
		public void UnloadAtlas(string atlasName)
		{
			LoadedSpriteAtlasData loadedSpriteAtlasData;
			if (this.loadedAtlasesDataDictionary.TryGetValue(atlasName, out loadedSpriteAtlasData))
			{
				Addressables.Release<SpriteAtlas>(loadedSpriteAtlasData.atlas);
				this.loadedAtlasesDataDictionary.Remove(atlasName);
				return;
			}
			this.DoLog("#sprites# Cannot unload atlas " + atlasName + ": atlas not loaded.", LazyLogType.Error);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00015490 File Offset: 0x00013690
		public void UnloadAtlases(bool includeProtected = false)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, LoadedSpriteAtlasData> keyValuePair in this.loadedAtlasesDataDictionary)
			{
				if (!keyValuePair.Value.isProtected || includeProtected)
				{
					Addressables.Release<SpriteAtlas>(keyValuePair.Value.atlas);
					list.Add(keyValuePair.Key);
				}
			}
			foreach (string text in list)
			{
				this.loadedAtlasesDataDictionary.Remove(text);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00015558 File Offset: 0x00013758
		public void ProtectAtlases()
		{
			foreach (KeyValuePair<string, LoadedSpriteAtlasData> keyValuePair in this.loadedAtlasesDataDictionary)
			{
				keyValuePair.Value.isProtected = true;
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000155B4 File Offset: 0x000137B4
		public void UnprotectAtlases()
		{
			foreach (KeyValuePair<string, LoadedSpriteAtlasData> keyValuePair in this.loadedAtlasesDataDictionary)
			{
				keyValuePair.Value.isProtected = false;
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00015610 File Offset: 0x00013810
		private void DoLog(string message, LazyLogType type = LazyLogType.Default)
		{
			switch (type)
			{
			case LazyLogType.Default:
				Debug.Log(message);
				return;
			case LazyLogType.Warning:
				Debug.LogWarning(message);
				return;
			case LazyLogType.Error:
				Debug.LogError(message);
				return;
			default:
				return;
			}
		}

		// Token: 0x040001D9 RID: 473
		public const string COLLECTION_SO_NAME = "SpriteCollection";

		// Token: 0x040001DA RID: 474
		public string addressablesSubDirectory = "Addressables";

		// Token: 0x040001DB RID: 475
		[SerializeField]
		private List<SpriteAtlasInfo> atlasInfoList = new List<SpriteAtlasInfo>();

		// Token: 0x040001DC RID: 476
		private Dictionary<string, LoadedSpriteAtlasData> loadedAtlasesDataDictionary = new Dictionary<string, LoadedSpriteAtlasData>();

		// Token: 0x040001DD RID: 477
		private Dictionary<string, string> hash = new Dictionary<string, string>();

		// Token: 0x040001DE RID: 478
		private static bool isInitialized = false;

		// Token: 0x040001DF RID: 479
		public static LazyLogType spriteLogType = LazyLogType.Warning;

		// Token: 0x040001E0 RID: 480
		public static LazyLogType fallbackSpriteLogType = LazyLogType.Warning;
	}
}
