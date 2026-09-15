using System;
using UnityEngine;

// Token: 0x02000361 RID: 865
public interface IMovable
{
	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06001702 RID: 5890
	// (set) Token: 0x06001703 RID: 5891
	Vector3 MovablePosition { get; set; }

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06001704 RID: 5892
	// (set) Token: 0x06001705 RID: 5893
	Vector3 MovablePositionWithoutDirectionChange { get; set; }

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06001706 RID: 5894
	// (set) Token: 0x06001707 RID: 5895
	Vector2 MovableDirection { get; set; }

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06001708 RID: 5896
	string MovableObjectId { get; }

	// Token: 0x06001709 RID: 5897 RVA: 0x00002318 File Offset: 0x00000518
	void OnTransitionReached(string currentWorldId, string destinationWorldId)
	{
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x00002318 File Offset: 0x00000518
	void OnPathStart()
	{
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x00002318 File Offset: 0x00000518
	void OnPathComplete(MovementComponent component)
	{
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x00002318 File Offset: 0x00000518
	void OnTeleportToTransitPoint(Vector3 from, Vector3 to)
	{
	}
}
