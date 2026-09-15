using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class SoundZone : MonoBehaviour
{
	// Token: 0x1700011A RID: 282
	// (get) Token: 0x0600072D RID: 1837 RVA: 0x00022072 File Offset: 0x00020272
	private bool IsCustomAndCurve
	{
		get
		{
			return this.customValue && this.soundType == SoundZoneValuesType.Curve;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x0600072E RID: 1838 RVA: 0x00022087 File Offset: 0x00020287
	public bool PlaySoundAs3DSound
	{
		get
		{
			return this.playSoundAs3DSound;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x0600072F RID: 1839 RVA: 0x0002208F File Offset: 0x0002028F
	public float MinDistance
	{
		get
		{
			if (!this.customValue)
			{
				return this.preset.minDistance;
			}
			return this.minDistance;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000730 RID: 1840 RVA: 0x000220AB File Offset: 0x000202AB
	public float MaxDistance
	{
		get
		{
			if (!this.customValue)
			{
				return this.preset.maxDistance;
			}
			return this.maxDistance;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x000220C7 File Offset: 0x000202C7
	public bool OverrideAmbientSound
	{
		get
		{
			return this.overrideAmbientSound;
		}
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x000220CF File Offset: 0x000202CF
	private float CalcVolumeCoef(float coef)
	{
		if (!this.customValue)
		{
			return this.preset.Evaluate(coef);
		}
		if (this.soundType == SoundZoneValuesType.Curve)
		{
			return this.curve.Evaluate(coef);
		}
		return coef;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x000220FD File Offset: 0x000202FD
	public void OnPlayerEnter(int colliderIndex)
	{
		this.isMainCameraInsideList[colliderIndex] = true;
		this.EnsurePlayerInside();
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00022112 File Offset: 0x00020312
	public void OnPlayerExit(int colliderIndex)
	{
		this.isMainCameraInsideList[colliderIndex] = false;
		this.EnsurePlayerInside();
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00022128 File Offset: 0x00020328
	private void EnsurePlayerInside()
	{
		bool flag = this.isMainCameraInsideZone;
		this.isMainCameraInsideZone = this.isMainCameraInsideList.Contains(true);
		if (flag != this.isMainCameraInsideZone)
		{
			if (this.isMainCameraInsideZone)
			{
				if (this.overrideAmbientSound && EnvironmentEngine.Instance != null)
				{
					EnvironmentEngine.Instance.PushOverrodeAmbientSound(this, this.soundId, !this.hardCutOverride);
				}
				switch (this.trackType)
				{
				case SoundZoneTrackType.Sound:
				{
					SoundHandler soundHandler = this.soundHandler;
					if (soundHandler != null)
					{
						soundHandler.Stop();
					}
					if (this.playSoundAs3DSound)
					{
						if (this.sound3DTransform == null)
						{
							this.sound3DTransform = new GameObject("SoundTransform").transform;
							this.sound3DTransform.SetParent(base.transform);
						}
						this.soundHandler = LazyAudio.PlayAtGameObject(this.soundId, this.sound3DTransform, SpatialType.sound3D, true);
						return;
					}
					this.soundHandler = LazyAudio.Play(this.soundId);
					return;
				}
				case SoundZoneTrackType.Playlist:
				{
					PlaylistController activePlaylistController = this.GetActivePlaylistController();
					if (!(activePlaylistController != null) || !(activePlaylistController.Id == this.soundId))
					{
						this.previousPlaylistController = activePlaylistController;
						PlaylistController playlistController = this.previousPlaylistController;
						if (playlistController != null)
						{
							playlistController.Pause(this.playlistTransitionDuration, this.playlistTransitionEase);
						}
						this.playlistController = LazyAudio.PlayPlaylist(this.soundId);
						return;
					}
					break;
				}
				case SoundZoneTrackType.None:
					break;
				default:
					return;
				}
			}
			else
			{
				this.ReleaseAmbientOverride();
				switch (this.trackType)
				{
				case SoundZoneTrackType.Sound:
				{
					SoundHandler soundHandler2 = this.soundHandler;
					if (soundHandler2 != null)
					{
						soundHandler2.Stop();
					}
					if (this.playSoundAs3DSound && this.sound3DTransform != null)
					{
						global::UnityEngine.Object.Destroy(this.sound3DTransform.gameObject);
						return;
					}
					break;
				}
				case SoundZoneTrackType.Playlist:
				{
					PlaylistController playlistController2 = this.playlistController;
					if (playlistController2 != null)
					{
						playlistController2.Stop();
					}
					this.playlistController = null;
					if (this.previousPlaylistController == null)
					{
						this.playlistController = LazyAudio.PlayPlaylist("gameplay");
						return;
					}
					PlaylistController playlistController3 = this.previousPlaylistController;
					if (playlistController3 != null)
					{
						playlistController3.UnPause(this.playlistTransitionDuration, this.playlistTransitionEase);
					}
					this.previousPlaylistController = null;
					break;
				}
				case SoundZoneTrackType.None:
					break;
				default:
					return;
				}
			}
		}
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00022336 File Offset: 0x00020536
	private void ReleaseAmbientOverride()
	{
		if (!this.overrideAmbientSound)
		{
			return;
		}
		if (EnvironmentEngine.Instance != null)
		{
			EnvironmentEngine.Instance.ClearOverrodeAmbientSound(this, !this.hardCutOverride);
		}
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00022364 File Offset: 0x00020564
	private void OnDisable()
	{
		this.ReleaseAmbientOverride();
		for (int i = 0; i < this.isMainCameraInsideList.Count; i++)
		{
			this.isMainCameraInsideList[i] = false;
		}
		this.isMainCameraInsideZone = false;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x000223A4 File Offset: 0x000205A4
	private PlaylistController GetActivePlaylistController()
	{
		foreach (PlaylistController playlistController in LazyAudio.GetPlaylistControllers())
		{
			if (!(playlistController.Id == this.soundId) && playlistController.IsActive && !playlistController.IsPaused)
			{
				return playlistController;
			}
		}
		return null;
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x0002241C File Offset: 0x0002061C
	private void Start()
	{
		this.CalculatePathData();
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00022424 File Offset: 0x00020624
	private void Update()
	{
		if (this.isMainCameraInsideZone)
		{
			switch (this.type)
			{
			case SoundZoneType.Point:
				this.DoPointCalculations();
				return;
			case SoundZoneType.Line:
				if (this.playSoundAs3DSound && this.sound3DTransform != null)
				{
					this.sound3DTransform.position = this.CalculateSourcePosition();
				}
				this.DoLineCalculations();
				return;
			case SoundZoneType.Polygon:
				if (this.points.Count >= 3)
				{
					this.DoPolygonCalculations();
				}
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002249C File Offset: 0x0002069C
	private void OnDestroy()
	{
		this.ReleaseAmbientOverride();
		SoundHandler soundHandler = this.soundHandler;
		if (soundHandler != null)
		{
			soundHandler.Stop();
		}
		if (this.isMainCameraInsideZone && this.trackType == SoundZoneTrackType.Playlist)
		{
			PlaylistController playlistController = this.playlistController;
			if (playlistController != null)
			{
				playlistController.Stop();
			}
			PlaylistController playlistController2 = this.previousPlaylistController;
			if (playlistController2 == null)
			{
				return;
			}
			playlistController2.UnPause();
		}
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x000224F4 File Offset: 0x000206F4
	private void CalculatePathData()
	{
		if (this.points.Count < 2)
		{
			return;
		}
		if (this.type == SoundZoneType.Polygon && this.points.Count >= 3)
		{
			Vector3 vector = Vector3.zero;
			foreach (SoundPoint soundPoint in this.points)
			{
				vector += soundPoint.transform.position;
			}
			this.polygonCenter = vector / (float)this.points.Count;
			return;
		}
		this.segmentLengths = new float[this.points.Count - 1];
		this.segmentDirections = new Vector3[this.points.Count - 1];
		this.totalLineLength = 0f;
		for (int i = 0; i < this.points.Count - 1; i++)
		{
			Vector3 vector2 = this.points[i + 1].transform.position - this.points[i].transform.position;
			this.segmentLengths[i] = vector2.magnitude;
			this.segmentDirections[i] = vector2.normalized;
			this.totalLineLength += this.segmentLengths[i];
		}
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00022658 File Offset: 0x00020858
	private Vector3 CalculateSourcePosition()
	{
		Vector3 position = LazyAudio.Microphone.position;
		float num = this.FindWeightedTargetPosOnLine(position);
		float num2 = this.maxMoveDistance * Time.deltaTime;
		float num3 = num - this.curPosOnLine;
		if (Mathf.Abs(num3) > num2)
		{
			this.curPosOnLine += Mathf.Sign(num3) * num2;
		}
		else
		{
			this.curPosOnLine = num;
		}
		this.curPosOnLine = Mathf.Clamp(this.curPosOnLine, 0f, this.totalLineLength);
		return this.GetPositionAtDistance(this.curPosOnLine);
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x000226E0 File Offset: 0x000208E0
	private float FindWeightedTargetPosOnLine(Vector3 playerPos)
	{
		List<SoundZone.SegmentPoint> list = new List<SoundZone.SegmentPoint>();
		float num = 0f;
		for (int i = 0; i < this.points.Count - 1; i++)
		{
			Vector3 position = this.points[i].transform.position;
			Vector3 vector = this.segmentDirections[i];
			float num2 = Vector3.Dot(playerPos - position, vector);
			num2 = Mathf.Clamp(num2, 0f, this.segmentLengths[i]);
			Vector3 vector2 = position + vector * num2;
			float num3 = this.CalcDistanceBetween(playerPos, vector2);
			float num4 = num + num2;
			list.Add(new SoundZone.SegmentPoint(i, num3, num4));
			num += this.segmentLengths[i];
		}
		list.Sort((SoundZone.SegmentPoint a, SoundZone.SegmentPoint b) => a.distance.CompareTo(b.distance));
		this.p1 = list[0];
		this.p2 = list[1];
		if (this.p2.positionOnLine.EqualsTo(this.GetSegmentStartLength(this.p2.segmentIndex), 1E-05f) || this.p2.positionOnLine.EqualsTo(this.GetSegmentEndLength(this.p2.segmentIndex), 1E-05f))
		{
			this.p2 = null;
			return this.p1.positionOnLine;
		}
		float num5 = 0.001f;
		float num6 = 1f / (this.p1.distance - this.minDistance + num5);
		float num7 = 1f / (this.p2.distance - this.minDistance + num5);
		float num8 = num6 + num7;
		num6 /= num8;
		num7 /= num8;
		return this.p1.positionOnLine * num6 + this.p2.positionOnLine * num7;
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x000228B8 File Offset: 0x00020AB8
	private float GetSegmentStartLength(int index)
	{
		if (index == 0)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < index; i++)
		{
			num += this.segmentLengths[i];
		}
		return num;
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x000228EC File Offset: 0x00020AEC
	private float GetSegmentEndLength(int index)
	{
		float num = 0f;
		for (int i = 0; i <= index; i++)
		{
			num += this.segmentLengths[i];
		}
		return num;
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00022918 File Offset: 0x00020B18
	public Vector3 GetPositionAtDistance(float distance)
	{
		float num = 0f;
		for (int i = 0; i < this.segmentLengths.Length; i++)
		{
			if (num + this.segmentLengths[i] >= distance)
			{
				float num2 = (distance - num) / this.segmentLengths[i];
				return Vector3.Lerp(this.points[i].transform.position, this.points[i + 1].transform.position, num2);
			}
			num += this.segmentLengths[i];
		}
		List<SoundPoint> list = this.points;
		return list[list.Count - 1].transform.position;
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x000229B8 File Offset: 0x00020BB8
	private void DoLineCalculations()
	{
		Vector3 position = LazyAudio.Microphone.position;
		float num2;
		if (!this.playSoundAs3DSound)
		{
			this.curClosestSpIndex = this.FindClosestToPlayerPointIndex();
			Vector3 vector;
			if (this.curClosestSpIndex == 0 || this.curClosestSpIndex == this.points.Count - 1)
			{
				int num = ((this.curClosestSpIndex == 0) ? (this.curClosestSpIndex + 1) : (this.curClosestSpIndex - 1));
				vector = this.GetNearestPointToPlayerOnFiniteLine(this.points[num].transform.position, this.points[this.curClosestSpIndex].transform.position);
			}
			else
			{
				Vector3 nearestPointToPlayerOnFiniteLine = this.GetNearestPointToPlayerOnFiniteLine(this.points[this.curClosestSpIndex - 1].transform.position, this.points[this.curClosestSpIndex].transform.position);
				Vector3 nearestPointToPlayerOnFiniteLine2 = this.GetNearestPointToPlayerOnFiniteLine(this.points[this.curClosestSpIndex + 1].transform.position, this.points[this.curClosestSpIndex].transform.position);
				float magnitude = (position - nearestPointToPlayerOnFiniteLine).magnitude;
				float magnitude2 = (position - nearestPointToPlayerOnFiniteLine2).magnitude;
				vector = ((magnitude < magnitude2) ? nearestPointToPlayerOnFiniteLine : nearestPointToPlayerOnFiniteLine2);
			}
			this.closestPosOnLine = vector;
			num2 = (position - this.closestPosOnLine).magnitude;
		}
		else
		{
			this.closestPosOnLine = this.GetPositionAtDistance(this.p1.positionOnLine);
			num2 = (position - this.closestPosOnLine).magnitude;
		}
		this.ApplyCoefByDistance(num2);
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00022B62 File Offset: 0x00020D62
	private void DoPointCalculations()
	{
		this.CalcDistanceAndApplyCoef(base.transform.position);
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00022B78 File Offset: 0x00020D78
	private void DoPolygonCalculations()
	{
		Vector3 position = LazyAudio.Microphone.position;
		Vector3 closestPointOnPolygonBoundary = this.GetClosestPointOnPolygonBoundary(position);
		bool flag = this.IsPointInsidePolygon(position);
		if (this.playSoundAs3DSound && this.sound3DTransform != null)
		{
			this.sound3DTransform.position = (flag ? position : closestPointOnPolygonBoundary);
		}
		this.closestPosOnLine = closestPointOnPolygonBoundary;
		if (flag)
		{
			this.ApplyCoefByDistance(0f);
			return;
		}
		this.CalcDistanceAndApplyCoef(closestPointOnPolygonBoundary);
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00022BE8 File Offset: 0x00020DE8
	private bool IsPointInsidePolygon(Vector3 point)
	{
		if (this.points == null || this.points.Count < 3)
		{
			return false;
		}
		bool flag = false;
		int i = 0;
		int num = this.points.Count - 1;
		while (i < this.points.Count)
		{
			Vector3 position = this.points[i].transform.position;
			Vector3 position2 = this.points[num].transform.position;
			if (position.z > point.z != position2.z > point.z && point.x < (position2.x - position.x) * (point.z - position.z) / (position2.z - position.z) + position.x)
			{
				flag = !flag;
			}
			num = i++;
		}
		return flag;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00022CCC File Offset: 0x00020ECC
	private Vector3 GetClosestPointOnPolygonBoundary(Vector3 position)
	{
		if (this.points == null || this.points.Count < 3)
		{
			return this.polygonCenter;
		}
		Vector3 vector = this.points[0].transform.position;
		float num = float.MaxValue;
		for (int i = 0; i < this.points.Count; i++)
		{
			int num2 = (i + 1) % this.points.Count;
			Vector3 nearestPointToPlayerOnFiniteLine = this.GetNearestPointToPlayerOnFiniteLine(this.points[i].transform.position, this.points[num2].transform.position);
			float sqrMagnitude = (position - nearestPointToPlayerOnFiniteLine).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				vector = nearestPointToPlayerOnFiniteLine;
			}
		}
		return vector;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x00022D8C File Offset: 0x00020F8C
	private void CalcDistanceAndApplyCoef(Vector3 closestPoint)
	{
		this.ApplyCoefByDistance((LazyAudio.Microphone.position - closestPoint).magnitude);
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x00022DB8 File Offset: 0x00020FB8
	private void ApplyCoefByDistance(float distance)
	{
		if (distance < this.MinDistance)
		{
			this.currentVolumeCoef = 1f;
		}
		else
		{
			this.currentVolumeCoef = Mathf.Clamp01(1f - (distance - this.MinDistance) / (this.MaxDistance - this.MinDistance));
			this.currentVolumeCoef = this.CalcVolumeCoef(this.currentVolumeCoef);
		}
		SoundHandler soundHandler = this.soundHandler;
		if (soundHandler != null)
		{
			soundHandler.SetVolume(this.currentVolumeCoef);
		}
		this.currentDistance = distance;
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00022E34 File Offset: 0x00021034
	private int FindClosestToPlayerPointIndex()
	{
		Vector3 position = LazyAudio.Microphone.position;
		int num = 0;
		float num2 = this.CalcDistanceBetween(position, this.points[0].transform.position);
		for (int i = 1; i < this.points.Count; i++)
		{
			float num3 = this.CalcDistanceBetween(position, this.points[i].transform.position);
			if (num3 <= num2)
			{
				num2 = num3;
				num = i;
			}
		}
		return num;
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00022EAC File Offset: 0x000210AC
	private float CalcDistanceBetween(Vector3 pt1, Vector3 pt2)
	{
		return Vector3.Distance(pt1, pt2);
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00022EB8 File Offset: 0x000210B8
	private Vector3 GetNearestPointOnLine(Vector3 startPt, Vector3 lineDir)
	{
		lineDir.Normalize();
		float num = Vector3.Dot(LazyAudio.Microphone.position - startPt, lineDir);
		return startPt + lineDir * num;
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x00022EF0 File Offset: 0x000210F0
	private Vector3 GetNearestPointToPlayerOnFiniteLine(Vector3 startPt, Vector3 endPt)
	{
		Vector3 vector = endPt - startPt;
		float magnitude = vector.magnitude;
		vector.Normalize();
		float num = Vector3.Dot(LazyAudio.Microphone.position - startPt, vector);
		num = Mathf.Clamp(num, 0f, magnitude);
		return startPt + vector * num;
	}

	// Token: 0x0400090D RID: 2317
	[SerializeField]
	private SoundZoneTrackType trackType;

	// Token: 0x0400090E RID: 2318
	[SerializeField]
	private string soundId;

	// Token: 0x0400090F RID: 2319
	[Tooltip("Sound would be attached to object")]
	[SerializeField]
	private bool playSoundAs3DSound;

	// Token: 0x04000910 RID: 2320
	[SerializeField]
	private float playlistTransitionDuration = 2f;

	// Token: 0x04000911 RID: 2321
	[SerializeField]
	private Ease playlistTransitionEase = Ease.InOutSine;

	// Token: 0x04000912 RID: 2322
	[SerializeField]
	private bool customValue = true;

	// Token: 0x04000913 RID: 2323
	[SerializeField]
	private SoundZonePreset preset;

	// Token: 0x04000914 RID: 2324
	[SerializeField]
	private SoundZoneValuesType soundType;

	// Token: 0x04000915 RID: 2325
	[SerializeField]
	private AnimationCurve curve;

	// Token: 0x04000916 RID: 2326
	[SerializeField]
	private float minDistance;

	// Token: 0x04000917 RID: 2327
	[SerializeField]
	private float maxDistance;

	// Token: 0x04000918 RID: 2328
	[Space]
	[SerializeField]
	private float currentDistance;

	// Token: 0x04000919 RID: 2329
	[SerializeField]
	private float currentVolumeCoef;

	// Token: 0x0400091A RID: 2330
	[SerializeField]
	private SoundZoneType type = SoundZoneType.Line;

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	private bool overrideAmbientSound;

	// Token: 0x0400091C RID: 2332
	[Tooltip("If true, the ambient override switches instantly instead of crossfading over the environment's switch duration.")]
	[SerializeField]
	private bool hardCutOverride;

	// Token: 0x0400091D RID: 2333
	[SerializeField]
	private List<SoundPoint> points = new List<SoundPoint>();

	// Token: 0x0400091E RID: 2334
	[SerializeField]
	private List<SoundZoneCollider> colliders = new List<SoundZoneCollider>();

	// Token: 0x0400091F RID: 2335
	[SerializeField]
	private List<bool> isMainCameraInsideList = new List<bool>();

	// Token: 0x04000920 RID: 2336
	[SerializeField]
	private float[] segmentLengths;

	// Token: 0x04000921 RID: 2337
	[SerializeField]
	private Vector3[] segmentDirections;

	// Token: 0x04000922 RID: 2338
	[SerializeField]
	private int curClosestSpIndex;

	// Token: 0x04000923 RID: 2339
	[SerializeField]
	private Vector3 closestPosOnLine;

	// Token: 0x04000924 RID: 2340
	[SerializeField]
	private float totalLineLength;

	// Token: 0x04000925 RID: 2341
	[SerializeField]
	private float curPosOnLine;

	// Token: 0x04000926 RID: 2342
	[SerializeField]
	private SoundZone.SegmentPoint p1;

	// Token: 0x04000927 RID: 2343
	[SerializeField]
	private SoundZone.SegmentPoint p2;

	// Token: 0x04000928 RID: 2344
	[SerializeField]
	private Vector3 polygonCenter;

	// Token: 0x04000929 RID: 2345
	private SoundHandler soundHandler;

	// Token: 0x0400092A RID: 2346
	private PlaylistController playlistController;

	// Token: 0x0400092B RID: 2347
	private PlaylistController previousPlaylistController;

	// Token: 0x0400092C RID: 2348
	private Transform sound3DTransform;

	// Token: 0x0400092D RID: 2349
	private bool isMainCameraInsideZone;

	// Token: 0x0400092E RID: 2350
	private float maxMoveDistance = 10f;

	// Token: 0x0200012C RID: 300
	[Serializable]
	private class SegmentPoint
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x00022FA7 File Offset: 0x000211A7
		public SegmentPoint(int segmentIndex, float distance, float positionOnLine)
		{
			this.segmentIndex = segmentIndex;
			this.distance = distance;
			this.positionOnLine = positionOnLine;
		}

		// Token: 0x0400092F RID: 2351
		public int segmentIndex;

		// Token: 0x04000930 RID: 2352
		public float distance;

		// Token: 0x04000931 RID: 2353
		public float positionOnLine;
	}
}
