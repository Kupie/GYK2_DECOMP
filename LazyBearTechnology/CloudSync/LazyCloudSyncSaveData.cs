using System;
using UnityEngine;

namespace LazyBearTechnology.CloudSync
{
	// Token: 0x0200019E RID: 414
	public class LazyCloudSyncSaveData
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0002D21E File Offset: 0x0002B41E
		public string CombinedBody
		{
			get
			{
				return this.header + "|" + this.data;
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0002D236 File Offset: 0x0002B436
		public LazyCloudSyncSaveData()
		{
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002D240 File Offset: 0x0002B440
		public LazyCloudSyncSaveData(string combinedData)
		{
			int num = combinedData.IndexOf('|');
			if (num == -1)
			{
				Debug.LogError("LBCloudSyncSaveData constructor error -- separator character not found!");
				return;
			}
			this.header = combinedData.Substring(0, num);
			this.data = combinedData.Substring(num + 1);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0002D288 File Offset: 0x0002B488
		public LazyCloudSyncSaveData(string header, string data)
		{
			this.header = header;
			this.data = data;
		}

		// Token: 0x040005AE RID: 1454
		private const char HEADER_BODY_SEPARATOR = '|';

		// Token: 0x040005AF RID: 1455
		public string header;

		// Token: 0x040005B0 RID: 1456
		public string data;
	}
}
