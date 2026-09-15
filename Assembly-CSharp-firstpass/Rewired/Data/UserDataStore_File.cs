using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Rewired.Utils.Libraries.CLZF2;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	// Token: 0x0200002B RID: 43
	public class UserDataStore_File : UserDataStore_KeyValue
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00004D88 File Offset: 0x00002F88
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00004DB7 File Offset: 0x00002FB7
		public string directory
		{
			get
			{
				if (string.IsNullOrEmpty(this.__directory))
				{
					return this.__directory = Application.persistentDataPath;
				}
				return this.__directory;
			}
			set
			{
				this.__directory = value;
				if (this._initialized)
				{
					this.OnDataSourceChanged();
				}
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00004DCE File Offset: 0x00002FCE
		// (set) Token: 0x06000363 RID: 867 RVA: 0x00004DD6 File Offset: 0x00002FD6
		public string fileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				this._fileName = value;
				if (this._initialized)
				{
					this.OnDataSourceChanged();
				}
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00004DED File Offset: 0x00002FED
		// (set) Token: 0x06000365 RID: 869 RVA: 0x00004DF5 File Offset: 0x00002FF5
		public UserDataStore_File.DataFormat dataFormat
		{
			get
			{
				return this._dataFormat;
			}
			set
			{
				this._dataFormat = value;
				if (this._initialized)
				{
					this.OnDataSourceChanged();
				}
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00004E0C File Offset: 0x0000300C
		// (set) Token: 0x06000367 RID: 871 RVA: 0x00004E49 File Offset: 0x00003049
		protected UserDataStore_File.IDataHandler dataHandler
		{
			get
			{
				if (this.__dataHandler == null)
				{
					return this.__dataHandler = new UserDataStore_File.LocalFileDataHandler(() => this._dataFormat, new UserDataStore_File.CLZF2());
				}
				return this.__dataHandler;
			}
			set
			{
				this.__dataHandler = value;
				if (this._initialized)
				{
					this.OnDataSourceChanged();
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00004E60 File Offset: 0x00003060
		protected override UserDataStore_KeyValue.IDataStore dataStore
		{
			get
			{
				return this._dataStore;
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void SetInitialValues()
		{
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00004E68 File Offset: 0x00003068
		protected override void OnInitialize()
		{
			this.SetInitialValues();
			this._initialized = true;
			this.OnDataSourceChanged();
			base.OnInitialize();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00004E83 File Offset: 0x00003083
		private void OnDataSourceChanged()
		{
			this._dataStore = new UserDataStore_File.DataStore((!string.IsNullOrEmpty(this._fileName)) ? this._fileName : "RewiredSaveData.json", this.directory, this.dataHandler);
		}

		// Token: 0x0400022E RID: 558
		private static readonly string thisScriptName = typeof(UserDataStore_File).Name;

		// Token: 0x0400022F RID: 559
		private const string logPrefix = "Rewired: ";

		// Token: 0x04000230 RID: 560
		private const string defaultExtensionText = ".json";

		// Token: 0x04000231 RID: 561
		private const string defaultExtensionBinary = ".bin";

		// Token: 0x04000232 RID: 562
		private const string defaultFileName = "RewiredSaveData.json";

		// Token: 0x04000233 RID: 563
		[Tooltip("The data file name. Changing this will make saved data already stored with the old file name no longer accessible.")]
		[SerializeField]
		private string _fileName = "RewiredSaveData.json";

		// Token: 0x04000234 RID: 564
		[Tooltip("Determines if the file should be stored as binary or text. Changing this will make saved data already stored no longer accessible.")]
		[SerializeField]
		private UserDataStore_File.DataFormat _dataFormat;

		// Token: 0x04000235 RID: 565
		[NonSerialized]
		private string __directory;

		// Token: 0x04000236 RID: 566
		[NonSerialized]
		private UserDataStore_File.DataStore _dataStore;

		// Token: 0x04000237 RID: 567
		[NonSerialized]
		private UserDataStore_File.IDataHandler __dataHandler;

		// Token: 0x04000238 RID: 568
		[NonSerialized]
		private bool _initialized;

		// Token: 0x0200002C RID: 44
		private sealed class DataStore : UserDataStore_KeyValue.IDataStore
		{
			// Token: 0x0600036F RID: 879 RVA: 0x00004EDF File Offset: 0x000030DF
			public DataStore(string fileName, string absDirectory, UserDataStore_File.IDataHandler dataHandler)
			{
				this._absFilePath = Path.Combine(absDirectory, fileName);
				if (dataHandler == null)
				{
					throw new ArgumentNullException("dataHandler");
				}
				this._dataHandler = dataHandler;
				this._data = new Dictionary<string, object>();
				this.Load();
			}

			// Token: 0x06000370 RID: 880 RVA: 0x00004F1B File Offset: 0x0000311B
			public bool TryGetValue(string key, out object value)
			{
				if (string.IsNullOrEmpty(key))
				{
					value = null;
					return false;
				}
				return this._data.TryGetValue(key, out value);
			}

			// Token: 0x06000371 RID: 881 RVA: 0x00004F37 File Offset: 0x00003137
			public bool SetValue(string key, object value)
			{
				if (string.IsNullOrEmpty(key))
				{
					return false;
				}
				this._data[key] = value;
				return true;
			}

			// Token: 0x06000372 RID: 882 RVA: 0x00004F54 File Offset: 0x00003154
			public bool Save()
			{
				bool flag;
				try
				{
					flag = this._dataHandler.Save(this._absFilePath, JsonWriter.ToJson(this._data));
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					flag = false;
				}
				return flag;
			}

			// Token: 0x06000373 RID: 883 RVA: 0x00004F9C File Offset: 0x0000319C
			public bool Load()
			{
				bool flag2;
				try
				{
					string text;
					bool flag = this._dataHandler.Load(this._absFilePath, out text);
					if (flag)
					{
						Dictionary<string, object> dictionary = JsonParser.FromJson<Dictionary<string, object>>(text);
						if (dictionary == null)
						{
							dictionary = new Dictionary<string, object>();
						}
						this._data = dictionary;
					}
					flag2 = flag;
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					flag2 = false;
				}
				return flag2;
			}

			// Token: 0x06000374 RID: 884 RVA: 0x00004FF4 File Offset: 0x000031F4
			public bool Clear()
			{
				bool flag;
				try
				{
					flag = this._dataHandler.Clear(this._absFilePath);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					flag = false;
				}
				this._data.Clear();
				return flag;
			}

			// Token: 0x04000239 RID: 569
			private Dictionary<string, object> _data;

			// Token: 0x0400023A RID: 570
			private readonly string _absFilePath;

			// Token: 0x0400023B RID: 571
			private UserDataStore_File.IDataHandler _dataHandler;
		}

		// Token: 0x0200002D RID: 45
		private sealed class LocalFileDataHandler : UserDataStore_File.IDataHandler
		{
			// Token: 0x06000375 RID: 885 RVA: 0x0000503C File Offset: 0x0000323C
			public LocalFileDataHandler(Func<UserDataStore_File.DataFormat> dataFormatDelegate, UserDataStore_File.Codec codec)
			{
				if (dataFormatDelegate == null)
				{
					throw new ArgumentNullException("dataFormatDelegate");
				}
				this._dataFormatDelegate = dataFormatDelegate;
				if (codec == null)
				{
					codec = new UserDataStore_File.UTF8Text();
				}
				this._codec = codec;
			}

			// Token: 0x06000376 RID: 886 RVA: 0x0000506C File Offset: 0x0000326C
			public bool Load(string absoluteFilePath, out string data)
			{
				data = null;
				if (string.IsNullOrEmpty(absoluteFilePath))
				{
					return false;
				}
				if (!File.Exists(absoluteFilePath))
				{
					return false;
				}
				bool flag;
				try
				{
					UserDataStore_File.DataFormat dataFormat = this._dataFormatDelegate();
					if (dataFormat != UserDataStore_File.DataFormat.Text)
					{
						if (dataFormat != UserDataStore_File.DataFormat.Binary)
						{
							throw new NotImplementedException();
						}
						byte[] array = File.ReadAllBytes(absoluteFilePath);
						data = this._codec.Decode(array);
						flag = array != null && array.Length != 0;
					}
					else
					{
						data = File.ReadAllText(absoluteFilePath);
						flag = !string.IsNullOrEmpty(data);
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					flag = false;
				}
				return flag;
			}

			// Token: 0x06000377 RID: 887 RVA: 0x000050FC File Offset: 0x000032FC
			public bool Save(string absoluteFilePath, string data)
			{
				if (string.IsNullOrEmpty(absoluteFilePath))
				{
					return false;
				}
				bool flag;
				try
				{
					string directoryName = Path.GetDirectoryName(absoluteFilePath);
					if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
					{
						Directory.CreateDirectory(directoryName);
					}
					UserDataStore_File.DataFormat dataFormat = this._dataFormatDelegate();
					if (dataFormat != UserDataStore_File.DataFormat.Text)
					{
						if (dataFormat != UserDataStore_File.DataFormat.Binary)
						{
							throw new NotImplementedException();
						}
						File.WriteAllBytes(absoluteFilePath, this._codec.Encode(data));
					}
					else
					{
						File.WriteAllText(absoluteFilePath, data);
					}
					flag = true;
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					flag = false;
				}
				return flag;
			}

			// Token: 0x06000378 RID: 888 RVA: 0x00005188 File Offset: 0x00003388
			public bool Clear(string absoluteFilePath)
			{
				if (string.IsNullOrEmpty(absoluteFilePath))
				{
					return false;
				}
				try
				{
					if (File.Exists(absoluteFilePath))
					{
						File.Delete(absoluteFilePath);
						return true;
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}
				return false;
			}

			// Token: 0x0400023C RID: 572
			private readonly Func<UserDataStore_File.DataFormat> _dataFormatDelegate;

			// Token: 0x0400023D RID: 573
			private readonly UserDataStore_File.Codec _codec;
		}

		// Token: 0x0200002E RID: 46
		private abstract class Codec
		{
			// Token: 0x06000379 RID: 889
			public abstract byte[] Encode(string @string);

			// Token: 0x0600037A RID: 890
			public abstract string Decode(byte[] data);
		}

		// Token: 0x0200002F RID: 47
		private sealed class UTF8Text : UserDataStore_File.Codec
		{
			// Token: 0x0600037C RID: 892 RVA: 0x000051D0 File Offset: 0x000033D0
			public override byte[] Encode(string @string)
			{
				return Encoding.UTF8.GetBytes(@string);
			}

			// Token: 0x0600037D RID: 893 RVA: 0x000051DD File Offset: 0x000033DD
			public override string Decode(byte[] data)
			{
				return Encoding.UTF8.GetString(data);
			}
		}

		// Token: 0x02000030 RID: 48
		private sealed class CLZF2 : UserDataStore_File.Codec
		{
			// Token: 0x0600037F RID: 895 RVA: 0x000051F2 File Offset: 0x000033F2
			public CLZF2()
			{
				this._cLZF2 = new Rewired.Utils.Libraries.CLZF2.CLZF2();
			}

			// Token: 0x06000380 RID: 896 RVA: 0x00005205 File Offset: 0x00003405
			public override byte[] Encode(string @string)
			{
				return this._cLZF2.Compress(Encoding.UTF8.GetBytes(@string));
			}

			// Token: 0x06000381 RID: 897 RVA: 0x0000521D File Offset: 0x0000341D
			public override string Decode(byte[] data)
			{
				return Encoding.UTF8.GetString(this._cLZF2.Decompress(data));
			}

			// Token: 0x0400023E RID: 574
			private readonly Rewired.Utils.Libraries.CLZF2.CLZF2 _cLZF2;
		}

		// Token: 0x02000031 RID: 49
		public interface IDataHandler
		{
			// Token: 0x06000382 RID: 898
			bool Load(string absoluteFilePath, out string data);

			// Token: 0x06000383 RID: 899
			bool Save(string absoluteFilePath, string data);

			// Token: 0x06000384 RID: 900
			bool Clear(string absoluteFilePath);
		}

		// Token: 0x02000032 RID: 50
		public enum DataFormat
		{
			// Token: 0x04000240 RID: 576
			Text,
			// Token: 0x04000241 RID: 577
			Binary
		}
	}
}
