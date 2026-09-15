using System;

// Token: 0x02000426 RID: 1062
[Serializable]
public abstract class SerializedItemProperty
{
	// Token: 0x06001C15 RID: 7189 RVA: 0x00082E1D File Offset: 0x0008101D
	public virtual SerializedItemProperty Clone()
	{
		return (SerializedItemProperty)Activator.CreateInstance(base.GetType());
	}
}
