using System;
using UnityEngine;

// Token: 0x02000261 RID: 609
[Serializable]
public class WorldFxColliderTriggerData : ColliderTriggerDataBase
{
	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0004EE20 File Offset: 0x0004D020
	// (set) Token: 0x06000F62 RID: 3938 RVA: 0x0004EE28 File Offset: 0x0004D028
	public Transform PlayTarget { get; set; }

	// Token: 0x06000F63 RID: 3939 RVA: 0x0004EE31 File Offset: 0x0004D031
	protected override bool IsSetupCompleted()
	{
		return !string.IsNullOrEmpty(this.fxName);
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x0004EE41 File Offset: 0x0004D041
	protected override void TriggerSetAction()
	{
		Debug.Log("WorldFxColliderTriggerData play FX:[" + this.fxName + "]");
		WorldFX.Spawn(this.PlayTarget.position, this.fxName, null, this.size);
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TriggerResetAction()
	{
	}

	// Token: 0x04001243 RID: 4675
	[SerializeField]
	private string fxName;

	// Token: 0x04001244 RID: 4676
	[SerializeField]
	private Vector3 size = Vector3.one;
}
