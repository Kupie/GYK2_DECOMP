using System;
using System.IO;

namespace LazyBearTechnology
{
	// Token: 0x0200010E RID: 270
	public class LazyFile
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001C9F2 File Offset: 0x0001ABF2
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x0001C9FA File Offset: 0x0001ABFA
		public bool IsSupportingBackupSaves
		{
			get
			{
				return this.isSupportingBackupSaves;
			}
			private set
			{
				this.isSupportingBackupSaves = value;
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001CA03 File Offset: 0x0001AC03
		public LazyFile()
		{
			this.IsSupportingBackupSaves = true;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001CA12 File Offset: 0x0001AC12
		public void DisableBackupSaving()
		{
			this.isSupportingBackupSaves = false;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001CA1B File Offset: 0x0001AC1B
		public bool WriteAllBytes(string path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);
			return true;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001CA25 File Offset: 0x0001AC25
		public bool WriteAllText(string path, string contents)
		{
			File.WriteAllText(path, contents);
			return true;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001CA2F File Offset: 0x0001AC2F
		public bool Exists(string path)
		{
			return File.Exists(path);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001CA37 File Offset: 0x0001AC37
		public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.GetFiles(path, "*" + searchPattern, searchOption);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001CA4B File Offset: 0x0001AC4B
		public byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001CA53 File Offset: 0x0001AC53
		public string ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001CA5B File Offset: 0x0001AC5B
		public bool Delete(string path)
		{
			File.Delete(path);
			return true;
		}

		// Token: 0x04000280 RID: 640
		private bool isSupportingBackupSaves;
	}
}
