using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000113 RID: 275
	public class LazyPlatformDependentElement : MonoBehaviour
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0001CD72 File Offset: 0x0001AF72
		public bool UseAwakeForInit
		{
			get
			{
				return this.useAwakeForInit;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001CD7A File Offset: 0x0001AF7A
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x0001CD82 File Offset: 0x0001AF82
		public bool IsActive { get; protected set; }

		// Token: 0x06000594 RID: 1428 RVA: 0x0001CD8C File Offset: 0x0001AF8C
		public virtual bool Init()
		{
			if (!base.enabled)
			{
				return false;
			}
			bool flag = true;
			switch (LazyAPI.Platform.GetPlatformId())
			{
			case LazyPlatform.PC:
				flag &= this.platformPC;
				break;
			case LazyPlatform.Xbox:
				flag &= this.platformXbox || this.platformAnyConsole;
				break;
			case LazyPlatform.NintendoSwitch:
				flag &= this.platformSwitch || this.platformAnyConsole;
				break;
			case LazyPlatform.PlayStation:
				flag &= this.platformPS || this.platformAnyConsole;
				break;
			default:
				throw new NotImplementedException();
			}
			if (LazyAPI.Platform.GetPlatformId() == LazyPlatform.PC)
			{
				if (LazyInput.IsGamepadActive)
				{
					flag &= this.controllerGamepad;
				}
				else
				{
					flag &= this.controllerMouse;
				}
			}
			else
			{
				flag &= this.controllerGamepad;
			}
			base.gameObject.SetActive(flag);
			this.IsActive = flag;
			return flag;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001CE61 File Offset: 0x0001B061
		protected virtual void Awake()
		{
			if (this.useAwakeForInit)
			{
				this.Init();
			}
		}

		// Token: 0x04000293 RID: 659
		[Header("Show when:")]
		public bool controllerGamepad = true;

		// Token: 0x04000294 RID: 660
		public bool controllerMouse = true;

		// Token: 0x04000295 RID: 661
		public bool platformPC = true;

		// Token: 0x04000296 RID: 662
		public bool platformAnyConsole = true;

		// Token: 0x04000297 RID: 663
		public bool platformXbox = true;

		// Token: 0x04000298 RID: 664
		public bool platformSwitch = true;

		// Token: 0x04000299 RID: 665
		public bool platformPS = true;

		// Token: 0x0400029A RID: 666
		[Space]
		[SerializeField]
		protected bool useAwakeForInit;
	}
}
