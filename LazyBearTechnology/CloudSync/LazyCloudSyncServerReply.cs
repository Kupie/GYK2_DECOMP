using System;
using UnityEngine;

namespace LazyBearTechnology.CloudSync
{
	// Token: 0x0200019F RID: 415
	internal class LazyCloudSyncServerReply
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0002D29E File Offset: 0x0002B49E
		public bool IsError
		{
			get
			{
				return !string.IsNullOrEmpty(this.error);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0002D2AE File Offset: 0x0002B4AE
		public bool IsAuthenicated
		{
			get
			{
				return (this.query == "syncinit" && this.IsResultOK) || this.auth == 1;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0002D2D5 File Offset: 0x0002B4D5
		public bool IsResultOK
		{
			get
			{
				return this.result == "ok";
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0002D2E7 File Offset: 0x0002B4E7
		public bool DoOverwrite
		{
			get
			{
				return this.overwrite == 1;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0002D2F4 File Offset: 0x0002B4F4
		public LazyCloudSync.CloudResult CloudResult
		{
			get
			{
				string text = this.result;
				if (text == "ok")
				{
					return LazyCloudSync.CloudResult.OK;
				}
				if (text == "not_found")
				{
					return LazyCloudSync.CloudResult.WrongSyncCode;
				}
				if (text == "same_id")
				{
					return LazyCloudSync.CloudResult.CantLinkToSameDevice;
				}
				if (!(text == "auth_error"))
				{
					return LazyCloudSync.CloudResult.Unknown;
				}
				return LazyCloudSync.CloudResult.AuthenticationError;
			}
		}

		// Token: 0x040005B1 RID: 1457
		[SerializeField]
		private string debugRequest;

		// Token: 0x040005B2 RID: 1458
		public string result;

		// Token: 0x040005B3 RID: 1459
		[SerializeField]
		private int auth;

		// Token: 0x040005B4 RID: 1460
		public int overwrite;

		// Token: 0x040005B5 RID: 1461
		public string query;

		// Token: 0x040005B6 RID: 1462
		public string error;

		// Token: 0x040005B7 RID: 1463
		public string serverId;

		// Token: 0x040005B8 RID: 1464
		public string password;

		// Token: 0x040005B9 RID: 1465
		public string code;

		// Token: 0x040005BA RID: 1466
		public string file;

		// Token: 0x040005BB RID: 1467
		public int lifetime;
	}
}
