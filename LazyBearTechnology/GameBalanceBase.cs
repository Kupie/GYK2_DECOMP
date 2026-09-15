using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000E6 RID: 230
	public class GameBalanceBase : ScriptableObject, IBalance
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001655E File Offset: 0x0001475E
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0001657D File Offset: 0x0001477D
		public static GameBalanceBase Instance
		{
			get
			{
				if (GameBalanceBase.instance == null)
				{
					throw new Exception("GameBalanceBase.Instance is null. You should set it after the balance is loaded to use this functionality.");
				}
				return GameBalanceBase.instance;
			}
			set
			{
				GameBalanceBase.instance = value;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00016588 File Offset: 0x00014788
		public GameBalanceBase()
		{
			this.InitBalance();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000165D8 File Offset: 0x000147D8
		public void InitBalance()
		{
			this.datas.Clear();
			this.types.Clear();
			this.cache.Clear();
			foreach (KeyValuePair<string, IList> keyValuePair in this.GetAllTabs())
			{
				this.datas.Add(keyValuePair.Value);
				Type type = keyValuePair.Value.GetType().GetGenericArguments()[0];
				this.types.Add(type);
				this.cache.Add(new Dictionary<string, int>());
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00016688 File Offset: 0x00014888
		public virtual void InitCache()
		{
			this.CreateIDsCache();
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00016690 File Offset: 0x00014890
		private void CreateIDsCache()
		{
			for (int i = 0; i < this.types.Count; i++)
			{
				this.cache[i].Clear();
				for (int j = 0; j < this.datas[i].Count; j++)
				{
					BalanceBaseObject balanceBaseObject = this.datas[i][j] as BalanceBaseObject;
					this.cache[i].Add(balanceBaseObject.id, j);
				}
			}
			this.cacheCreated = true;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00016718 File Offset: 0x00014918
		public void ClearBalance()
		{
			this.InitBalance();
			foreach (IList list in this.datas)
			{
				list.Clear();
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00016770 File Offset: 0x00014970
		protected static T DataNotFound<T>(string identifier) where T : BalanceBaseObject
		{
			string[] array = new string[5];
			array[0] = "No data for object [";
			int num = 1;
			Type typeFromHandle = typeof(T);
			array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
			array[2] = "] with id = \"";
			array[3] = identifier;
			array[4] = "\"";
			Debug.LogWarning(string.Concat(array));
			return default(T);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000167CC File Offset: 0x000149CC
		private static bool HaveCollectionSameID<T>(List<T> list, T data) where T : BalanceBaseObject
		{
			return list.FindIndex((T p) => p.id == data.id) != -1;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00016800 File Offset: 0x00014A00
		private static T GetElementByID<T>(List<T> list, string id) where T : BalanceBaseObject
		{
			return list.Find((T p) => p.id == id);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001682C File Offset: 0x00014A2C
		private static T GetElementByID<T>(List<T> list, Dictionary<string, int> cache, string id) where T : BalanceBaseObject
		{
			T t;
			if (id == null)
			{
				t = default(T);
				return t;
			}
			try
			{
				if (cache == null)
				{
					string text = "ERROR: Trying to get a ";
					Type typeFromHandle = typeof(T);
					Debug.LogError(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " item with a null cache");
					t = default(T);
					t = t;
				}
				else
				{
					t = list[cache[id]];
				}
			}
			catch (Exception)
			{
				t = default(T);
			}
			return t;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000168B0 File Offset: 0x00014AB0
		private string AddToDataCollections<T>(List<T> list, T dataToAdd) where T : BalanceBaseObject
		{
			if (GameBalanceBase.HaveCollectionSameID<T>(list, dataToAdd))
			{
				return "Can't add: same id already exist: " + dataToAdd.id;
			}
			list.Add(dataToAdd);
			return "";
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000168E0 File Offset: 0x00014AE0
		public string AddData<T>(T dataToAdd) where T : BalanceBaseObject
		{
			int num = this.types.IndexOf(typeof(T));
			if (num == -1)
			{
				string text = "Unknown type at AddData: ";
				Type typeFromHandle = typeof(T);
				Debug.LogError(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
				return null;
			}
			return this.AddToDataCollections<T>(this.datas[num] as List<T>, dataToAdd);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00016948 File Offset: 0x00014B48
		public string AddDataUniversal(object dataToAdd)
		{
			BalanceBaseObject balanceBaseObject = dataToAdd as BalanceBaseObject;
			Type type = dataToAdd.GetType();
			if (balanceBaseObject == null)
			{
				Debug.LogError("Type " + type.Name + " couldn't be converted to BalanceBaseObject");
				return null;
			}
			int num = this.types.IndexOf(type);
			if (num == -1)
			{
				Debug.LogError("Unknown type at AddData: " + type.Name);
				return null;
			}
			using (IEnumerator enumerator = this.datas[num].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((enumerator.Current as BalanceBaseObject).id == balanceBaseObject.id)
					{
						return "Can't add. Same id already exist: " + balanceBaseObject.id;
					}
				}
			}
			this.datas[num].Add(dataToAdd);
			return "";
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00016A38 File Offset: 0x00014C38
		public T GetData<T>(string id) where T : BalanceBaseObject
		{
			T t;
			if ((t = this.GetDataOrNull<T>(id)) == null)
			{
				t = GameBalanceBase.DataNotFound<T>(id);
			}
			return t;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00016A50 File Offset: 0x00014C50
		public T GetDataOrNull<T>(string id) where T : BalanceBaseObject
		{
			if (id == null)
			{
				return default(T);
			}
			int num = this.types.IndexOf(typeof(T));
			if (num == -1)
			{
				Debug.LogError(string.Format("No data for object [{0}] with id = [{1}]", typeof(T), id));
				return default(T);
			}
			if (!this.cacheCreated)
			{
				return GameBalanceBase.GetElementByID<T>(this.datas[num] as List<T>, id);
			}
			return GameBalanceBase.GetElementByID<T>(this.datas[num] as List<T>, this.cache[num], id);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00016AEC File Offset: 0x00014CEC
		public List<T> GetDataCollection<T>() where T : BalanceBaseObject
		{
			int num = this.types.IndexOf(typeof(T));
			if (num == -1)
			{
				string text = "Unknown type at GetDataCollection: ";
				Type typeFromHandle = typeof(T);
				Debug.LogError(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
				return null;
			}
			return this.datas[num] as List<T>;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00016B4C File Offset: 0x00014D4C
		public IList GetDataCollection(Type type)
		{
			int num = this.types.IndexOf(type);
			if (num == -1)
			{
				Debug.LogError("Unknown type at GetDataCollection: " + ((type != null) ? type.ToString() : null));
				return null;
			}
			return this.datas[num];
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00016B94 File Offset: 0x00014D94
		[Obsolete("There's no need overriding this method anymore. Use a [BalanceTab] attribute instead.")]
		public virtual Dictionary<string, IList> GetAllDataListsAndGoogleTabs()
		{
			return new Dictionary<string, IList>();
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00016B9C File Offset: 0x00014D9C
		public Dictionary<string, IList> GetAllTabs()
		{
			Dictionary<string, IList> allDataListsAndGoogleTabs = this.GetAllDataListsAndGoogleTabs();
			foreach (FieldInfo fieldInfo in base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				BalanceTabAttribute customAttribute = fieldInfo.GetCustomAttribute<BalanceTabAttribute>();
				if (customAttribute != null)
				{
					allDataListsAndGoogleTabs.Add(customAttribute.TabName, (IList)fieldInfo.GetValue(this));
					if (customAttribute.StartRow != -1)
					{
						this.tabStartRow[customAttribute.TabName] = customAttribute.StartRow;
					}
				}
			}
			return allDataListsAndGoogleTabs;
		}

		// Token: 0x040001FA RID: 506
		[NonSerialized]
		private List<IList> datas = new List<IList>();

		// Token: 0x040001FB RID: 507
		[NonSerialized]
		private List<Type> types = new List<Type>();

		// Token: 0x040001FC RID: 508
		[NonSerialized]
		private List<Dictionary<string, int>> cache = new List<Dictionary<string, int>>();

		// Token: 0x040001FD RID: 509
		private bool cacheCreated;

		// Token: 0x040001FE RID: 510
		public static string currentTabName = string.Empty;

		// Token: 0x040001FF RID: 511
		private Dictionary<string, int> tabStartRow = new Dictionary<string, int>();

		// Token: 0x04000200 RID: 512
		public Dictionary<Type, List<AutoValidatorObjectData>> validationData = new Dictionary<Type, List<AutoValidatorObjectData>>();

		// Token: 0x04000201 RID: 513
		private static GameBalanceBase instance = null;
	}
}
