using System;
using System.Collections;
using System.Collections.Generic;

namespace Expressive
{
	// Token: 0x02000033 RID: 51
	internal class VariableProviderDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00006F9F File Offset: 0x0000519F
		public VariableProviderDictionary(IVariableProvider variableProvider)
		{
			this.variableProvider = variableProvider;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006FAE File Offset: 0x000051AE
		public bool TryGetValue(string key, out object value)
		{
			return this.variableProvider.TryGetValue(key, out value);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006FBD File Offset: 0x000051BD
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return VariableProviderDictionary.ThrowNotSupported<IEnumerator<KeyValuePair<string, object>>>();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006FC4 File Offset: 0x000051C4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006FCC File Offset: 0x000051CC
		public void Add(KeyValuePair<string, object> item)
		{
			VariableProviderDictionary.ThrowNotSupported<bool>();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006FD4 File Offset: 0x000051D4
		public void Clear()
		{
			VariableProviderDictionary.ThrowNotSupported<bool>();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006FDC File Offset: 0x000051DC
		public bool Contains(KeyValuePair<string, object> item)
		{
			return VariableProviderDictionary.ThrowNotSupported<bool>();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006FE3 File Offset: 0x000051E3
		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			VariableProviderDictionary.ThrowNotSupported<bool>();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006FEB File Offset: 0x000051EB
		public bool Remove(KeyValuePair<string, object> item)
		{
			return VariableProviderDictionary.ThrowNotSupported<bool>();
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00006FF2 File Offset: 0x000051F2
		public int Count
		{
			get
			{
				return VariableProviderDictionary.ThrowNotSupported<int>();
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006FF9 File Offset: 0x000051F9
		public bool IsReadOnly
		{
			get
			{
				return VariableProviderDictionary.ThrowNotSupported<bool>();
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007000 File Offset: 0x00005200
		public void Add(string key, object value)
		{
			VariableProviderDictionary.ThrowNotSupported<bool>();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00007008 File Offset: 0x00005208
		public bool ContainsKey(string key)
		{
			return VariableProviderDictionary.ThrowNotSupported<bool>();
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000700F File Offset: 0x0000520F
		public bool Remove(string key)
		{
			return VariableProviderDictionary.ThrowNotSupported<bool>();
		}

		// Token: 0x17000022 RID: 34
		public object this[string key]
		{
			get
			{
				return VariableProviderDictionary.ThrowNotSupported<object>();
			}
			set
			{
				VariableProviderDictionary.ThrowNotSupported<object>();
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00007025 File Offset: 0x00005225
		public ICollection<string> Keys
		{
			get
			{
				return VariableProviderDictionary.ThrowNotSupported<ICollection<string>>();
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000702C File Offset: 0x0000522C
		public ICollection<object> Values
		{
			get
			{
				return VariableProviderDictionary.ThrowNotSupported<ICollection<object>>();
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007034 File Offset: 0x00005234
		private static TReturn ThrowNotSupported<TReturn>()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000091 RID: 145
		private readonly IVariableProvider variableProvider;
	}
}
