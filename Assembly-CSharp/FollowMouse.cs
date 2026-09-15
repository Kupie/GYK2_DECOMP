using System;
using UnityEngine;

// Token: 0x02000B42 RID: 2882
public class FollowMouse : MonoBehaviour
{
	// Token: 0x06004C8F RID: 19599 RVA: 0x001696F6 File Offset: 0x001678F6
	private void OnEnable()
	{
		Cursor.visible = false;
		base.transform.position = Input.mousePosition;
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x0016970E File Offset: 0x0016790E
	private void OnDisable()
	{
		Cursor.visible = true;
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x00169716 File Offset: 0x00167916
	private void Update()
	{
		base.transform.position = Input.mousePosition;
	}
}
