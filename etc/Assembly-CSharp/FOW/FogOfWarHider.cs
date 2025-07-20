using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace FOW
{
	// Token: 0x02000376 RID: 886
	public class FogOfWarHider : MonoBehaviour
	{
		// Token: 0x06001716 RID: 5910 RVA: 0x0006E9C1 File Offset: 0x0006CBC1
		public void OnEnable()
		{
			this.CalculateSamplePointData();
			this.RegisterHider();
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x0006E9CF File Offset: 0x0006CBCF
		public void OnDisable()
		{
			this.SetActive(true);
			this.DeregisterHider();
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x0006E9E0 File Offset: 0x0006CBE0
		private void CalculateSamplePointData()
		{
			if (this.SamplePoints == null || this.SamplePoints.Length == 0)
			{
				this.SamplePoints = new Transform[1];
				this.SamplePoints[0] = base.transform;
			}
			this.MaxDistBetweenSamplePoints = 0f;
			for (int i = 0; i < this.SamplePoints.Length; i++)
			{
				for (int j = i; j < this.SamplePoints.Length; j++)
				{
					this.MaxDistBetweenSamplePoints = Mathf.Max(this.MaxDistBetweenSamplePoints, Vector3.Distance(this.SamplePoints[i].position, this.SamplePoints[j].position));
				}
			}
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x0006EA79 File Offset: 0x0006CC79
		public void RegisterHider()
		{
			this.CachedTransform = base.transform;
			if (!FogOfWarWorld.HidersList.Contains(this))
			{
				FogOfWarWorld.HidersList.Add(this);
				FogOfWarWorld.NumHiders++;
				this.SetActive(false);
			}
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0006EAB4 File Offset: 0x0006CCB4
		public void DeregisterHider()
		{
			if (FogOfWarWorld.HidersList.Contains(this))
			{
				FogOfWarWorld.HidersList.Remove(this);
				FogOfWarWorld.NumHiders--;
				foreach (FogOfWarRevealer fogOfWarRevealer in this.Observers)
				{
					fogOfWarRevealer.HidersSeen.Remove(this);
				}
				this.NumObservers = 0;
				this.Observers.Clear();
			}
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x0006EB44 File Offset: 0x0006CD44
		public void AddObserver(FogOfWarRevealer Observer)
		{
			if (this.PermanentlyReveal)
			{
				base.enabled = false;
				return;
			}
			this.Observers.Add(Observer);
			if (this.NumObservers == 0)
			{
				this.SetActive(true);
			}
			this.NumObservers++;
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x0006EB7F File Offset: 0x0006CD7F
		public void RemoveObserver(FogOfWarRevealer Observer)
		{
			this.Observers.Remove(Observer);
			this.NumObservers--;
			if (this.NumObservers == 0)
			{
				this.SetActive(false);
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600171D RID: 5917 RVA: 0x0006EBAC File Offset: 0x0006CDAC
		// (remove) Token: 0x0600171E RID: 5918 RVA: 0x0006EBE4 File Offset: 0x0006CDE4
		public event FogOfWarHider.OnChangeActive OnActiveChanged;

		// Token: 0x0600171F RID: 5919 RVA: 0x0006EC19 File Offset: 0x0006CE19
		private void SetActive(bool isActive)
		{
			FogOfWarHider.OnChangeActive onActiveChanged = this.OnActiveChanged;
			if (onActiveChanged == null)
			{
				return;
			}
			onActiveChanged(isActive);
		}

		// Token: 0x0400168A RID: 5770
		[Tooltip("Leaving this empty will make the hider use its own transform as a sample point.")]
		[FormerlySerializedAs("samplePoints")]
		public Transform[] SamplePoints;

		// Token: 0x0400168B RID: 5771
		public bool PermanentlyReveal;

		// Token: 0x0400168C RID: 5772
		[HideInInspector]
		public float MaxDistBetweenSamplePoints;

		// Token: 0x0400168D RID: 5773
		[HideInInspector]
		public int NumObservers;

		// Token: 0x0400168E RID: 5774
		[HideInInspector]
		public List<FogOfWarRevealer> Observers = new List<FogOfWarRevealer>();

		// Token: 0x0400168F RID: 5775
		[HideInInspector]
		public Transform CachedTransform;

		// Token: 0x02000377 RID: 887
		// (Invoke) Token: 0x06001722 RID: 5922
		public delegate void OnChangeActive(bool isActive);
	}
}
