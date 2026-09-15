using System;

// Token: 0x0200068B RID: 1675
public interface IWgoCustomComponent<T>
{
	// Token: 0x06002CE0 RID: 11488
	T OnSave();

	// Token: 0x06002CE1 RID: 11489
	void OnLoad(T data);

	// Token: 0x06002CE2 RID: 11490
	void OnUnload();
}
