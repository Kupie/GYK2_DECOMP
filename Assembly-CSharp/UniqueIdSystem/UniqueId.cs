using System;
using UnityEngine;

namespace UniqueIdSystem
{
	// Token: 0x02000B49 RID: 2889
	[DisallowMultipleComponent]
	[ExecuteAlways]
	public class UniqueId : MonoBehaviour
	{
		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06004CCD RID: 19661 RVA: 0x00169F55 File Offset: 0x00168155
		public string Uid
		{
			get
			{
				return this.uid;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06004CCE RID: 19662 RVA: 0x00169F5D File Offset: 0x0016815D
		public long AssignedTimestampUtc
		{
			get
			{
				return this.assignedTimestampUtc;
			}
		}

		// Token: 0x04003DC6 RID: 15814
		[SerializeField]
		private string uid = "";

		// Token: 0x04003DC7 RID: 15815
		[SerializeField]
		private long assignedTimestampUtc;

		// Token: 0x04003DC8 RID: 15816
		private IUniqueIdUser uniqueIdUser;
	}
}
