using System;
using UnityEngine;

// Token: 0x02000394 RID: 916
public class PlayerInvisibleWalls : MonoBehaviour
{
	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06001886 RID: 6278 RVA: 0x00073FAE File Offset: 0x000721AE
	// (set) Token: 0x06001887 RID: 6279 RVA: 0x00073FB6 File Offset: 0x000721B6
	public bool IsWallsCheckEnabled
	{
		get
		{
			return this.isWallsCheckEnabled;
		}
		set
		{
			this.isWallsCheckEnabled = value;
		}
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x00073FC0 File Offset: 0x000721C0
	public void UpdateEdgeWallsState(Vector3 playerPosition)
	{
		if (!this.isWallsCheckEnabled)
		{
			return;
		}
		Vector3 vector = new Vector3(0f, -this.edgeRaycastLength);
		Vector3 vector2 = playerPosition + Vector3.up * this.raycastYOffset;
		Vector3 roundedPosY = VisualConsts.GetRoundedPosY(vector2, 1, 2);
		float num = 0.75f;
		float num2 = 0.7f;
		float num3 = 0.6f;
		float num4 = Mathf.Sqrt(Mathf.Pow(0.25f, 2f) / 2f) + 0.5f;
		float num5 = Mathf.Sqrt(Mathf.Pow(0.2f, 2f) / 2f) + 0.5f;
		float num6 = Mathf.Sqrt(Mathf.Pow(0.1f, 2f) / 2f) + 0.5f;
		this.xPlusWall.position = roundedPosY + new Vector3(num, 0f);
		this.xMinusWall.position = roundedPosY + new Vector3(-num, 0f);
		this.zPlusWall.position = roundedPosY + new Vector3(0f, 0f, num2);
		this.zMinusWall.position = roundedPosY + new Vector3(0f, 0f, -num3);
		this.xPlusZMinusWall.position = roundedPosY + new Vector3(num4, 0f, -num6);
		this.xPlusZPlusWall.position = roundedPosY + new Vector3(num4, 0f, num5);
		this.xMinusZPlusWall.position = roundedPosY + new Vector3(-num4, 0f, num5);
		this.xMinusZMinusWall.position = roundedPosY + new Vector3(-num4, 0f, -num6);
		this.xPlusWall.gameObject.SetActive(!this.IsRaycastHitWalkableGround(vector2 + new Vector3(this.edgeCheckDistanceX, 0f), vector));
		this.xMinusWall.gameObject.SetActive(!this.IsRaycastHitWalkableGround(vector2 + new Vector3(-this.edgeCheckDistanceX, 0f), vector));
		this.zPlusWall.gameObject.SetActive(!this.IsRaycastHitWalkableGround(vector2 + new Vector3(0f, 0f, this.edgeCheckDistanceZPlus), vector));
		this.zMinusWall.gameObject.SetActive(!this.IsRaycastHitWalkableGround(vector2 + new Vector3(0f, 0f, -this.edgeCheckDistanceZMinus), vector));
		this.xPlusZMinusWall.gameObject.SetActive(!this.IsRaycastHitWalkableGround(vector2 + new Vector3(this.edgeCheckDistanceDiagonalX, 0f, -this.edgeCheckDistanceDiagonalZMinus), vector));
		this.xPlusZPlusWall.gameObject.SetActive(!this.IsRaycastHitWalkableGround(vector2 + new Vector3(this.edgeCheckDistanceDiagonalX, 0f, this.edgeCheckDistanceDiagonalZPlus), vector));
		this.xMinusZPlusWall.gameObject.SetActive(!this.IsRaycastHitWalkableGround(vector2 + new Vector3(-this.edgeCheckDistanceDiagonalX, 0f, this.edgeCheckDistanceDiagonalZPlus), vector));
		this.xMinusZMinusWall.gameObject.SetActive(!this.IsRaycastHitWalkableGround(vector2 + new Vector3(-this.edgeCheckDistanceDiagonalX, 0f, -this.edgeCheckDistanceDiagonalZMinus), vector));
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x0007432C File Offset: 0x0007252C
	public void ChangeWallsVisibility(bool visible)
	{
		this.xPlusWall.gameObject.SetActive(visible);
		this.xMinusWall.gameObject.SetActive(visible);
		this.zPlusWall.gameObject.SetActive(visible);
		this.zMinusWall.gameObject.SetActive(visible);
		this.xPlusZMinusWall.gameObject.SetActive(visible);
		this.xPlusZPlusWall.gameObject.SetActive(visible);
		this.xMinusZPlusWall.gameObject.SetActive(visible);
		this.xMinusZMinusWall.gameObject.SetActive(visible);
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x000743C4 File Offset: 0x000725C4
	private bool IsRaycastHitWalkableGround(Vector3 startRayPoint, Vector3 rayDirection)
	{
		Array.Clear(PlayerInvisibleWalls.raycastResults, 0, 10);
		bool flag = Physics.SphereCastNonAlloc(startRayPoint, 0.01f, rayDirection, PlayerInvisibleWalls.raycastResults, this.edgeRaycastLength, 6144) > 0;
		Color color = (flag ? Color.white : Color.red);
		Debug.DrawRay(startRayPoint, rayDirection, color);
		return flag;
	}

	// Token: 0x040017FD RID: 6141
	public const float INVISIBLE_WALL_COLLIDER_HALF_WIDTH = 0.5f;

	// Token: 0x040017FE RID: 6142
	private const int COLLIDERS_LIMIT_COUNT = 10;

	// Token: 0x040017FF RID: 6143
	private const float CAST_SPHERE_RADIUS = 0.01f;

	// Token: 0x04001800 RID: 6144
	private const float PLAYER_COLLIDER_RADIUS_X = 0.25f;

	// Token: 0x04001801 RID: 6145
	private const float PLAYER_COLLIDER_RADIUS_Z_PLUS = 0.2f;

	// Token: 0x04001802 RID: 6146
	private const float PLAYER_COLLIDER_RADIUS_Z_MINUS = 0.1f;

	// Token: 0x04001803 RID: 6147
	[SerializeField]
	private float edgeCheckDistanceZPlus = 0.4f;

	// Token: 0x04001804 RID: 6148
	[SerializeField]
	private float edgeCheckDistanceZMinus = 0.1f;

	// Token: 0x04001805 RID: 6149
	[SerializeField]
	private float edgeCheckDistanceX = 0.25f;

	// Token: 0x04001806 RID: 6150
	[SerializeField]
	private float edgeCheckDistanceDiagonalZPlus = 0.4f;

	// Token: 0x04001807 RID: 6151
	[SerializeField]
	private float edgeCheckDistanceDiagonalZMinus = 0.1f;

	// Token: 0x04001808 RID: 6152
	[SerializeField]
	private float edgeCheckDistanceDiagonalX = 0.25f;

	// Token: 0x04001809 RID: 6153
	[SerializeField]
	private float edgeRaycastLength = 0.6f;

	// Token: 0x0400180A RID: 6154
	[SerializeField]
	private float raycastYOffset = 0.05f;

	// Token: 0x0400180B RID: 6155
	public Transform xPlusWall;

	// Token: 0x0400180C RID: 6156
	public Transform xMinusWall;

	// Token: 0x0400180D RID: 6157
	public Transform zPlusWall;

	// Token: 0x0400180E RID: 6158
	public Transform zMinusWall;

	// Token: 0x0400180F RID: 6159
	public Transform xPlusZMinusWall;

	// Token: 0x04001810 RID: 6160
	public Transform xPlusZPlusWall;

	// Token: 0x04001811 RID: 6161
	public Transform xMinusZMinusWall;

	// Token: 0x04001812 RID: 6162
	public Transform xMinusZPlusWall;

	// Token: 0x04001813 RID: 6163
	private static RaycastHit[] raycastResults = new RaycastHit[10];

	// Token: 0x04001814 RID: 6164
	private bool isWallsCheckEnabled;
}
