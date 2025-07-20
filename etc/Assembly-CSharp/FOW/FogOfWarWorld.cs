using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace FOW
{
	// Token: 0x0200036A RID: 874
	[DefaultExecutionOrder(-100)]
	public class FogOfWarWorld : MonoBehaviour
	{
		// Token: 0x060016F5 RID: 5877 RVA: 0x0006D309 File Offset: 0x0006B509
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x0006D309 File Offset: 0x0006B509
		private void OnEnable()
		{
			this.Initialize();
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x0006D311 File Offset: 0x0006B511
		private void OnDisable()
		{
			this.Cleanup();
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x0006D311 File Offset: 0x0006B511
		private void OnDestroy()
		{
			this.Cleanup();
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x0006D31C File Offset: 0x0006B51C
		private void Update()
		{
			if (this._numRevealers > 0)
			{
				switch (this.revealerMode)
				{
				case FogOfWarWorld.RevealerUpdateMode.Every_Frame:
					for (int i = 0; i < this._numRevealers; i++)
					{
						this.Revealers[i].RevealHiders();
						if (!this.Revealers[i].StaticRevealer)
						{
							this.Revealers[i].LineOfSightPhase1();
						}
					}
					for (int j = 0; j < this._numRevealers; j++)
					{
						if (!this.Revealers[j].StaticRevealer)
						{
							this.Revealers[j].LineOfSightPhase2();
						}
					}
					break;
				case FogOfWarWorld.RevealerUpdateMode.N_Per_Frame:
				{
					int num = this.currentIndex;
					for (int k = 0; k < Mathf.Clamp(this.MaxNumRevealersPerFrame, 0, this.numDynamicRevealers); k++)
					{
						num = (num + 1) % this._numRevealers;
						this.Revealers[num].RevealHiders();
						if (!this.Revealers[num].StaticRevealer)
						{
							this.Revealers[num].LineOfSightPhase1();
						}
						else
						{
							k--;
						}
					}
					for (int l = 0; l < Mathf.Clamp(this.MaxNumRevealersPerFrame, 0, this.numDynamicRevealers); l++)
					{
						this.currentIndex = (this.currentIndex + 1) % this._numRevealers;
						if (!this.Revealers[this.currentIndex].StaticRevealer)
						{
							this.Revealers[this.currentIndex].LineOfSightPhase2();
						}
						else
						{
							l--;
						}
					}
					break;
				}
				}
			}
			if (this.UseMiniMap || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Texture || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Both)
			{
				if (this.UseRegrow)
				{
					Graphics.Blit(this.FOW_RT, this.FOW_REGROW_RT, this.FowTextureMaterial, 1);
					Graphics.Blit(this.FOW_REGROW_RT, this.FOW_RT, this.FowTextureMaterial, 0);
					return;
				}
				Graphics.Blit(null, this.FOW_RT, this.FowTextureMaterial, 0);
			}
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x0006D4F0 File Offset: 0x0006B6F0
		private void Cleanup()
		{
			int numRevealers = this._numRevealers;
			for (int i = 0; i < numRevealers; i++)
			{
				FogOfWarRevealer fogOfWarRevealer = this.Revealers[0];
				fogOfWarRevealer.DeregisterRevealer();
				FogOfWarWorld.RevealersToRegister.Add(fogOfWarRevealer);
			}
			if (this.CircleBuffer != null)
			{
				this.IndicesBuffer.Dispose();
				this.CircleBuffer.Dispose();
				this.AnglesBuffer.Dispose();
			}
			FogOfWarWorld.instance = null;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x0006D55C File Offset: 0x0006B75C
		public void Initialize()
		{
			if (FogOfWarWorld.instance)
			{
				return;
			}
			FogOfWarWorld.instance = this;
			this.maxCones = this.MaxPossibleRevealers * this.MaxPossibleSegmentsPerRevealer;
			this.Revealers = new FogOfWarRevealer[this.MaxPossibleRevealers];
			this.IndicesBuffer = new ComputeBuffer(this.MaxPossibleRevealers, Marshal.SizeOf(typeof(int)), ComputeBufferType.Default);
			this.CircleBuffer = new ComputeBuffer(this.MaxPossibleRevealers, Marshal.SizeOf(typeof(FogOfWarWorld.RevealerStruct)), ComputeBufferType.Default);
			this.anglesArray = new FogOfWarWorld.ConeEdgeStruct[this.MaxPossibleSegmentsPerRevealer];
			this.AnglesBuffer = new ComputeBuffer(this.maxCones, Marshal.SizeOf(typeof(FogOfWarWorld.ConeEdgeStruct)), ComputeBufferType.Default);
			this.FogOfWarMaterial = new Material(Shader.Find("Hidden/FullScreen/FOW/SolidColor"));
			if (this.UseMiniMap || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Texture || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Both)
			{
				this.FowTextureMaterial = new Material(Shader.Find("Hidden/FullScreen/FOW/FOW_RT"));
				this.InitFOWRT();
				this.UpdateMaterialProperties(this.FowTextureMaterial);
				this.FowTextureMaterial.SetBuffer(Shader.PropertyToID("_ActiveCircleIndices"), this.IndicesBuffer);
				this.FowTextureMaterial.SetBuffer(Shader.PropertyToID("_CircleBuffer"), this.CircleBuffer);
				this.FowTextureMaterial.SetBuffer(Shader.PropertyToID("_ConeBuffer"), this.AnglesBuffer);
				this.FowTextureMaterial.EnableKeyword("IGNORE_HEIGHT");
			}
			this.SetFogShader();
			this.UpdateAllMaterialProperties();
			this.SetAllMaterialBounds();
			foreach (FogOfWarRevealer fogOfWarRevealer in FogOfWarWorld.RevealersToRegister)
			{
				if (fogOfWarRevealer != null)
				{
					fogOfWarRevealer.RegisterRevealer();
				}
			}
			FogOfWarWorld.RevealersToRegister.Clear();
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x0006D738 File Offset: 0x0006B938
		public void InitFOWRT()
		{
			this.FOW_RT = new RenderTexture(this.FowResX, this.FowResY, 0);
			this.FOW_RT.format = RenderTextureFormat.ARGBHalf;
			this.FOW_RT.antiAliasing = 8;
			this.FOW_RT.filterMode = FilterMode.Trilinear;
			this.FOW_RT.anisoLevel = 9;
			this.FOW_RT.Create();
			RenderTexture.active = this.FOW_RT;
			Material material = new Material(Shader.Find("Hidden/Internal-Colored"));
			material.SetInt("_SrcBlend", 4);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_Cull", 0);
			material.SetInt("_ZWrite", 0);
			material.SetInt("_ZTest", 8);
			material.SetPass(0);
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 1f - this.InitialFogExplorationValue));
			GL.End();
			if (this.UseMiniMap && this.UIImage != null)
			{
				this.UIImage.texture = this.FOW_RT;
			}
			if (this.UseRegrow)
			{
				this.FOW_REGROW_RT = new RenderTexture(this.FOW_RT);
				this.FOW_REGROW_RT.Create();
			}
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x0006D875 File Offset: 0x0006BA75
		public RenderTexture GetFOWRT()
		{
			return this.FOW_RT;
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x0006D880 File Offset: 0x0006BA80
		public void SetFogShader()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.UsingSoftening = false;
			string text = "Hidden/FullScreen/FOW";
			switch (this.FogAppearance)
			{
			case FogOfWarWorld.FogOfWarAppearance.Solid_Color:
				text += "/SolidColor";
				break;
			case FogOfWarWorld.FogOfWarAppearance.GrayScale:
				text += "/GrayScale";
				break;
			case FogOfWarWorld.FogOfWarAppearance.Blur:
				text += "/Blur";
				break;
			case FogOfWarWorld.FogOfWarAppearance.Texture_Sample:
				text += "/TextureSample";
				break;
			case FogOfWarWorld.FogOfWarAppearance.Outline:
				text += "/Outline";
				break;
			case FogOfWarWorld.FogOfWarAppearance.None:
				text = "Hidden/BlitCopy";
				break;
			}
			this.FogOfWarMaterial.shader = Shader.Find(text);
			this.InitializeFogProperties(this.FogOfWarMaterial);
			this.UpdateMaterialProperties(this.FogOfWarMaterial);
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x0006D940 File Offset: 0x0006BB40
		public void InitializeFogProperties(Material material)
		{
			material.DisableKeyword("IS_2D");
			material.DisableKeyword("IS_3D");
			if (!this.is2D)
			{
				material.EnableKeyword("IS_3D");
				material.DisableKeyword("PLANE_XZ");
				material.DisableKeyword("PLANE_XY");
				material.DisableKeyword("PLANE_ZY");
				switch (this.gamePlane)
				{
				case FogOfWarWorld.GamePlane.XZ:
					material.EnableKeyword("PLANE_XZ");
					FogOfWarWorld.UpVector = Vector3.up;
					break;
				case FogOfWarWorld.GamePlane.XY:
					material.EnableKeyword("PLANE_XY");
					FogOfWarWorld.UpVector = -Vector3.forward;
					break;
				case FogOfWarWorld.GamePlane.ZY:
					material.EnableKeyword("PLANE_ZY");
					FogOfWarWorld.UpVector = Vector3.right;
					break;
				}
			}
			else
			{
				FogOfWarWorld.UpVector = -Vector3.forward;
				material.EnableKeyword("IS_2D");
			}
			material.SetBuffer(Shader.PropertyToID("_ActiveCircleIndices"), this.IndicesBuffer);
			material.SetBuffer(Shader.PropertyToID("_CircleBuffer"), this.CircleBuffer);
			material.SetBuffer(Shader.PropertyToID("_ConeBuffer"), this.AnglesBuffer);
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x0006DA5C File Offset: 0x0006BC5C
		public void UpdateAllMaterialProperties()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.UpdateMaterialProperties(this.FogOfWarMaterial);
			if (this.FowTextureMaterial != null)
			{
				this.UpdateMaterialProperties(this.FowTextureMaterial);
			}
			foreach (PartialHider partialHider in FogOfWarWorld.PartialHiders)
			{
				this.UpdateMaterialProperties(partialHider.HiderMaterial);
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x0006DAE4 File Offset: 0x0006BCE4
		public void UpdateMaterialProperties(Material material)
		{
			material.DisableKeyword("HARD");
			material.DisableKeyword("SOFT");
			this.UsingSoftening = false;
			FogOfWarWorld.FogOfWarType fogType = this.FogType;
			if (fogType != FogOfWarWorld.FogOfWarType.Hard)
			{
				if (fogType == FogOfWarWorld.FogOfWarType.Soft)
				{
					material.EnableKeyword("SOFT");
					this.UsingSoftening = true;
				}
			}
			else
			{
				material.EnableKeyword("HARD");
			}
			material.SetInt("BLEED", 0);
			if (this.AllowBleeding)
			{
				material.SetInt("BLEED", 1);
			}
			material.SetColor(this.materialColorID, (material == this.FowTextureMaterial) ? this.MiniMapColor : this.UnknownColor);
			material.SetFloat(this.unobscuredBlurRadiusID, this.UnobscuredSoftenDistance);
			material.DisableKeyword("INNER_SOFTEN");
			if (this.FogType == FogOfWarWorld.FogOfWarType.Soft && this.UseInnerSoften)
			{
				material.EnableKeyword("INNER_SOFTEN");
				material.SetFloat(Shader.PropertyToID("_fadeOutDegrees"), this.InnerSoftenAngle);
			}
			else
			{
				material.SetFloat(Shader.PropertyToID("_fadeOutDegrees"), 0f);
			}
			material.SetFloat(this.extraRadiusID, this.SightExtraAmount);
			material.SetFloat(Shader.PropertyToID("_edgeSoftenDistance"), this.EdgeSoftenDistance);
			material.SetFloat(this.maxDistanceID, this.MaxFogDistance);
			material.SetInt("_pixelate", 0);
			if (this.PixelateFog && !this.WorldSpacePixelate)
			{
				material.SetInt("_pixelate", 1);
			}
			material.SetInt("_pixelateWS", 0);
			if (this.PixelateFog && this.WorldSpacePixelate)
			{
				material.SetInt("_pixelateWS", 1);
			}
			if (this.PixelateFog)
			{
				material.SetFloat(this.extraRadiusID, this.SightExtraAmount + 1f / this.PixelDensity);
			}
			material.SetFloat("_pixelDensity", this.PixelDensity);
			material.SetVector("_pixelOffset", this.PixelGridOffset);
			material.SetInt("_ditherFog", 0);
			if (this.UseDithering)
			{
				material.SetInt("_ditherFog", 1);
			}
			material.SetFloat("_ditherSize", this.DitherSize);
			material.SetInt("_invertEffect", 0);
			if (this.InvertFowEffect)
			{
				material.SetInt("_invertEffect", 1);
			}
			switch (this.FogFade)
			{
			case FogOfWarWorld.FogOfWarFadeType.Linear:
				material.SetInt("_fadeType", 0);
				break;
			case FogOfWarWorld.FogOfWarFadeType.Exponential:
				material.SetInt("_fadeType", 4);
				material.SetFloat(this.fadePowerID, this.FogFadePower);
				break;
			case FogOfWarWorld.FogOfWarFadeType.Smooth:
				material.SetInt("_fadeType", 1);
				break;
			case FogOfWarWorld.FogOfWarFadeType.Smoother:
				material.SetInt("_fadeType", 2);
				break;
			case FogOfWarWorld.FogOfWarFadeType.Smoothstep:
				material.SetInt("_fadeType", 3);
				break;
			}
			material.SetInt("BLEND_MAX", 1);
			FogOfWarWorld.FogOfWarBlendMode blendType = this.BlendType;
			if (blendType != FogOfWarWorld.FogOfWarBlendMode.Max)
			{
				if (blendType == FogOfWarWorld.FogOfWarBlendMode.Addative)
				{
					material.SetInt("BLEND_MAX", 0);
				}
			}
			else
			{
				material.SetInt("BLEND_MAX", 1);
			}
			switch (this.FogAppearance)
			{
			case FogOfWarWorld.FogOfWarAppearance.GrayScale:
				material.SetFloat(this.saturationStrengthID, this.SaturationStrength);
				break;
			case FogOfWarWorld.FogOfWarAppearance.Blur:
				material.SetFloat(this.blurStrengthID, this.BlurStrength);
				material.SetFloat(this.blurPixelOffsetMinID, (float)Screen.height * (this.BlurDistanceScreenPercentMin / 100f));
				material.SetFloat(this.blurPixelOffsetMaxID, (float)Screen.height * (this.BlurDistanceScreenPercentMax / 100f));
				material.SetInt(this.blurSamplesID, this.BlurSamples);
				material.SetFloat(this.blurPeriodID, 6.2831855f / (float)this.BlurSamples);
				break;
			case FogOfWarWorld.FogOfWarAppearance.Texture_Sample:
				material.SetTexture(this.fowTetureID, this.FogTexture);
				material.SetVector(this.fowTilingID, this.FogTextureTiling);
				material.SetFloat(this.fowSpeedID, this.FogScrollSpeed);
				break;
			case FogOfWarWorld.FogOfWarAppearance.Outline:
				material.SetFloat("lineThickness", this.OutlineThickness);
				break;
			}
			material.DisableKeyword("SAMPLE_REALTIME");
			if (this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Pixel_Perfect || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Both)
			{
				material.EnableKeyword("SAMPLE_REALTIME");
			}
			material.DisableKeyword("SAMPLE_TEXTURE");
			material.DisableKeyword("USE_TEXTURE_BLUR");
			if (this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Texture || this.FOWSamplingMode == FogOfWarWorld.FogSampleMode.Both)
			{
				material.SetTexture("_FowRT", this.FOW_RT);
				material.EnableKeyword("SAMPLE_TEXTURE");
				if (this.UseConstantBlur)
				{
					material.EnableKeyword("USE_TEXTURE_BLUR");
					material.SetFloat("_Sample_Blur_Quality", (float)this.ConstantTextureBlurQuality);
					material.SetFloat("_Sample_Blur_Amount", this.ConstantTextureBlurAmount);
				}
			}
			if (material == this.FowTextureMaterial)
			{
				material.SetFloat("_regrowSpeed", this.FogRegrowSpeed);
				material.SetFloat("_maxRegrowAmount", this.MaxFogRegrowAmount);
				material.EnableKeyword("SAMPLE_REALTIME");
				material.DisableKeyword("SAMPLE_TEXTURE");
				material.DisableKeyword("USE_REGROW");
				if (this.UseRegrow)
				{
					material.EnableKeyword("USE_REGROW");
				}
			}
			material.DisableKeyword("USE_WORLD_BOUNDS");
			if (this.UseRegrow)
			{
				material.EnableKeyword("USE_WORLD_BOUNDS");
			}
			material.SetFloat("_worldBoundsInfluence", 0f);
			if (this.UseWorldBounds)
			{
				material.SetFloat("_worldBoundsSoftenDistance", this.WorldBoundsSoftenDistance);
				material.SetFloat("_worldBoundsInfluence", this.WorldBoundsInfluence);
			}
			this.SetMaterialBounds(material);
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x0006E02F File Offset: 0x0006C22F
		public void UpdateWorldBounds(Vector3 center, Vector3 extent)
		{
			this.WorldBounds.center = center;
			this.WorldBounds.extents = extent;
			this.SetAllMaterialBounds();
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x0006E04F File Offset: 0x0006C24F
		public void UpdateWorldBounds(Bounds newBounds)
		{
			this.WorldBounds = newBounds;
			this.SetAllMaterialBounds();
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x0006E05E File Offset: 0x0006C25E
		private void SetAllMaterialBounds()
		{
			if (this.FogOfWarMaterial != null)
			{
				this.SetMaterialBounds(this.FogOfWarMaterial);
			}
			if (this.FowTextureMaterial != null)
			{
				this.SetMaterialBounds(this.FowTextureMaterial);
			}
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x0006E094 File Offset: 0x0006C294
		private void SetMaterialBounds(Material mat)
		{
			Vector4 boundsVectorForShader = this.GetBoundsVectorForShader();
			if (this.FogOfWarMaterial != null)
			{
				this.FogOfWarMaterial.SetVector("_worldBounds", boundsVectorForShader);
			}
			if (this.FowTextureMaterial != null)
			{
				this.FowTextureMaterial.SetVector("_worldBounds", boundsVectorForShader);
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x0006E0E8 File Offset: 0x0006C2E8
		public Vector4 GetBoundsVectorForShader()
		{
			if (this.is2D)
			{
				return new Vector4(this.WorldBounds.size.x, this.WorldBounds.center.x, this.WorldBounds.size.y, this.WorldBounds.center.y);
			}
			switch (this.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return new Vector4(this.WorldBounds.size.x, this.WorldBounds.center.x, this.WorldBounds.size.z, this.WorldBounds.center.z);
			case FogOfWarWorld.GamePlane.XY:
				return new Vector4(this.WorldBounds.size.x, this.WorldBounds.center.x, this.WorldBounds.size.y, this.WorldBounds.center.y);
			case FogOfWarWorld.GamePlane.ZY:
				return new Vector4(this.WorldBounds.size.z, this.WorldBounds.center.z, this.WorldBounds.size.z, this.WorldBounds.center.z);
			default:
				return new Vector4(this.WorldBounds.size.x, this.WorldBounds.center.x, this.WorldBounds.size.z, this.WorldBounds.center.z);
			}
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x0006E278 File Offset: 0x0006C478
		public Vector2 GetFowPositionFromWorldPosition(Vector3 WorldPosition)
		{
			if (this.is2D)
			{
				return new Vector2(WorldPosition.x, WorldPosition.y);
			}
			switch (this.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return new Vector2(WorldPosition.x, WorldPosition.z);
			case FogOfWarWorld.GamePlane.XY:
				return new Vector2(WorldPosition.x, WorldPosition.y);
			case FogOfWarWorld.GamePlane.ZY:
				return new Vector2(WorldPosition.z, WorldPosition.y);
			default:
				return new Vector2(WorldPosition.x, WorldPosition.z);
			}
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x0006E304 File Offset: 0x0006C504
		private void SetNumRevealers()
		{
			if (this.FogOfWarMaterial != null)
			{
				this.SetNumRevealers(this.FogOfWarMaterial);
			}
			if (this.FowTextureMaterial != null)
			{
				this.SetNumRevealers(this.FowTextureMaterial);
			}
			foreach (PartialHider partialHider in FogOfWarWorld.PartialHiders)
			{
				this.SetNumRevealers(partialHider.HiderMaterial);
			}
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0006E390 File Offset: 0x0006C590
		public void SetNumRevealers(Material material)
		{
			material.SetInt(this.numRevealersID, this._numRevealers);
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x0006E3A4 File Offset: 0x0006C5A4
		public int RegisterRevealer(FogOfWarRevealer newRevealer)
		{
			this._numRevealers++;
			if (!newRevealer.StaticRevealer)
			{
				this.numDynamicRevealers++;
			}
			this.SetNumRevealers();
			int num = this._numRevealers - 1;
			this.Revealers[num] = newRevealer;
			if (this.numDeregistered > 0)
			{
				this.numDeregistered--;
				num = this.DeregisteredIDs[0];
				this.DeregisteredIDs.RemoveAt(0);
			}
			newRevealer.IndexID = this._numRevealers - 1;
			this.indiciesDataToSet[0] = num;
			this.IndicesBuffer.SetData(this.indiciesDataToSet, 0, this._numRevealers - 1, 1);
			return num;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0006E450 File Offset: 0x0006C650
		public void DeRegisterRevealer(FogOfWarRevealer toRemove)
		{
			int indexID = toRemove.IndexID;
			this.DeregisteredIDs.Add(toRemove.FogOfWarID);
			this.numDeregistered++;
			this._numRevealers--;
			if (!toRemove.StaticRevealer)
			{
				this.numDynamicRevealers--;
			}
			FogOfWarRevealer fogOfWarRevealer = this.Revealers[this._numRevealers];
			if (toRemove != fogOfWarRevealer)
			{
				this.Revealers[indexID] = fogOfWarRevealer;
				this.indiciesDataToSet[0] = fogOfWarRevealer.FogOfWarID;
				this.IndicesBuffer.SetData(this.indiciesDataToSet, 0, indexID, 1);
				fogOfWarRevealer.IndexID = indexID;
			}
			this.SetNumRevealers();
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x0006E4F8 File Offset: 0x0006C6F8
		public void UpdateRevealerData(int id, FogOfWarWorld.RevealerStruct data, int numHits, float[] radii, float[] distances, bool[] hits)
		{
			data.StartIndex = id * this.MaxPossibleSegmentsPerRevealer;
			this._revealerDataToSet[0] = data;
			this.CircleBuffer.SetData(this._revealerDataToSet, 0, id, 1);
			if (numHits > this.MaxPossibleSegmentsPerRevealer)
			{
				Debug.LogError(string.Format("the revealer is trying to register {0} segments. this is more than was set by maxPossibleSegmentsPerRevealer", numHits));
				return;
			}
			for (int i = 0; i < numHits; i++)
			{
				this.anglesArray[i].angle = radii[i];
				this.anglesArray[i].length = distances[i];
				this.anglesArray[i].cutShort = (hits[i] ? 1 : 0);
			}
			this.AnglesBuffer.SetData(this.anglesArray, 0, id * this.MaxPossibleSegmentsPerRevealer, numHits);
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x0006E5C0 File Offset: 0x0006C7C0
		public static bool TestPointVisibility(Vector3 point)
		{
			for (int i = 0; i < FogOfWarWorld.instance._numRevealers; i++)
			{
				if (FogOfWarWorld.instance.Revealers[i].TestPoint(point))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0006E5FC File Offset: 0x0006C7FC
		public void ClearRegrowTexture()
		{
			RenderTexture.active = this.FOW_RT;
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 1f - this.InitialFogExplorationValue));
			GL.End();
			RenderTexture.active = this.FOW_REGROW_RT;
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 1f - this.InitialFogExplorationValue));
			GL.End();
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x0006E683 File Offset: 0x0006C883
		public void SetFowAppearance(FogOfWarWorld.FogOfWarAppearance AppearanceMode)
		{
			this.FogAppearance = AppearanceMode;
			if (!Application.isPlaying)
			{
				return;
			}
			base.enabled = false;
			base.enabled = true;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x0006E6A2 File Offset: 0x0006C8A2
		public FogOfWarWorld.FogOfWarAppearance GetFowAppearance()
		{
			return this.FogAppearance;
		}

		// Token: 0x040015FC RID: 5628
		public static FogOfWarWorld instance;

		// Token: 0x040015FD RID: 5629
		public bool UsingSoftening;

		// Token: 0x040015FE RID: 5630
		public FogOfWarWorld.FogOfWarType FogType = FogOfWarWorld.FogOfWarType.Soft;

		// Token: 0x040015FF RID: 5631
		public FogOfWarWorld.FogOfWarFadeType FogFade = FogOfWarWorld.FogOfWarFadeType.Smoothstep;

		// Token: 0x04001600 RID: 5632
		public FogOfWarWorld.FogOfWarBlendMode BlendType;

		// Token: 0x04001601 RID: 5633
		public float EdgeSoftenDistance = 0.1f;

		// Token: 0x04001602 RID: 5634
		public float UnobscuredSoftenDistance = 0.25f;

		// Token: 0x04001603 RID: 5635
		public bool UseInnerSoften = true;

		// Token: 0x04001604 RID: 5636
		public float InnerSoftenAngle = 5f;

		// Token: 0x04001605 RID: 5637
		public bool AllowBleeding;

		// Token: 0x04001606 RID: 5638
		public float SightExtraAmount = 0.01f;

		// Token: 0x04001607 RID: 5639
		public float MaxFogDistance = 10000f;

		// Token: 0x04001608 RID: 5640
		public bool PixelateFog;

		// Token: 0x04001609 RID: 5641
		public bool WorldSpacePixelate;

		// Token: 0x0400160A RID: 5642
		public float PixelDensity = 2f;

		// Token: 0x0400160B RID: 5643
		public bool RoundRevealerPosition;

		// Token: 0x0400160C RID: 5644
		public Vector2 PixelGridOffset;

		// Token: 0x0400160D RID: 5645
		public bool UseDithering;

		// Token: 0x0400160E RID: 5646
		public float DitherSize = 20f;

		// Token: 0x0400160F RID: 5647
		public bool InvertFowEffect;

		// Token: 0x04001610 RID: 5648
		public float FogFadePower = 1f;

		// Token: 0x04001611 RID: 5649
		[SerializeField]
		private FogOfWarWorld.FogOfWarAppearance FogAppearance;

		// Token: 0x04001612 RID: 5650
		[Tooltip("The color of the fog")]
		public Color UnknownColor = new Color(0.35f, 0.35f, 0.35f);

		// Token: 0x04001613 RID: 5651
		public float SaturationStrength;

		// Token: 0x04001614 RID: 5652
		public float BlurStrength = 1f;

		// Token: 0x04001615 RID: 5653
		[Range(0f, 2f)]
		public float BlurDistanceScreenPercentMin = 0.1f;

		// Token: 0x04001616 RID: 5654
		[Range(0f, 2f)]
		public float BlurDistanceScreenPercentMax = 1f;

		// Token: 0x04001617 RID: 5655
		public int BlurSamples = 6;

		// Token: 0x04001618 RID: 5656
		public Texture2D FogTexture;

		// Token: 0x04001619 RID: 5657
		public Vector2 FogTextureTiling = Vector2.one;

		// Token: 0x0400161A RID: 5658
		public float FogScrollSpeed;

		// Token: 0x0400161B RID: 5659
		public float OutlineThickness = 0.1f;

		// Token: 0x0400161C RID: 5660
		public FogOfWarWorld.FogSampleMode FOWSamplingMode;

		// Token: 0x0400161D RID: 5661
		public bool UseRegrow;

		// Token: 0x0400161E RID: 5662
		public float FogRegrowSpeed = 0.5f;

		// Token: 0x0400161F RID: 5663
		public float InitialFogExplorationValue;

		// Token: 0x04001620 RID: 5664
		public float MaxFogRegrowAmount = 0.3f;

		// Token: 0x04001621 RID: 5665
		private RenderTexture FOW_RT;

		// Token: 0x04001622 RID: 5666
		private RenderTexture FOW_REGROW_RT;

		// Token: 0x04001623 RID: 5667
		public Material FowTextureMaterial;

		// Token: 0x04001624 RID: 5668
		public int FowResX = 256;

		// Token: 0x04001625 RID: 5669
		public int FowResY = 256;

		// Token: 0x04001626 RID: 5670
		public bool UseConstantBlur = true;

		// Token: 0x04001627 RID: 5671
		public int ConstantTextureBlurQuality = 2;

		// Token: 0x04001628 RID: 5672
		public float ConstantTextureBlurAmount = 0.75f;

		// Token: 0x04001629 RID: 5673
		public bool UseWorldBounds;

		// Token: 0x0400162A RID: 5674
		public float WorldBoundsSoftenDistance = 1f;

		// Token: 0x0400162B RID: 5675
		public float WorldBoundsInfluence = 1f;

		// Token: 0x0400162C RID: 5676
		public Bounds WorldBounds = new Bounds(Vector3.zero, Vector3.one);

		// Token: 0x0400162D RID: 5677
		public bool UseMiniMap;

		// Token: 0x0400162E RID: 5678
		public Color MiniMapColor = new Color(0.4f, 0.4f, 0.4f, 0.95f);

		// Token: 0x0400162F RID: 5679
		public RawImage UIImage;

		// Token: 0x04001630 RID: 5680
		public FogOfWarWorld.RevealerUpdateMode revealerMode;

		// Token: 0x04001631 RID: 5681
		[Tooltip("The number of revealers to update each frame. Only used when Revealer Mode is set to N_Per_Frame")]
		public int MaxNumRevealersPerFrame = 3;

		// Token: 0x04001632 RID: 5682
		[Tooltip("The Max possible number of revealers. Keep this as low as possible to use less GPU memory")]
		public int MaxPossibleRevealers = 256;

		// Token: 0x04001633 RID: 5683
		[Tooltip("The Max possible number of segments per revealer. Keep this as low as possible to use less GPU memory")]
		public int MaxPossibleSegmentsPerRevealer = 128;

		// Token: 0x04001634 RID: 5684
		public bool is2D;

		// Token: 0x04001635 RID: 5685
		public FogOfWarWorld.GamePlane gamePlane;

		// Token: 0x04001636 RID: 5686
		public Material FogOfWarMaterial;

		// Token: 0x04001637 RID: 5687
		private int maxCones;

		// Token: 0x04001638 RID: 5688
		public ComputeBuffer IndicesBuffer;

		// Token: 0x04001639 RID: 5689
		public ComputeBuffer CircleBuffer;

		// Token: 0x0400163A RID: 5690
		public ComputeBuffer AnglesBuffer;

		// Token: 0x0400163B RID: 5691
		public FogOfWarRevealer[] Revealers;

		// Token: 0x0400163C RID: 5692
		private int _numRevealers;

		// Token: 0x0400163D RID: 5693
		public int numDynamicRevealers;

		// Token: 0x0400163E RID: 5694
		public static List<FogOfWarHider> HidersList = new List<FogOfWarHider>();

		// Token: 0x0400163F RID: 5695
		public static List<PartialHider> PartialHiders = new List<PartialHider>();

		// Token: 0x04001640 RID: 5696
		public static int NumHiders;

		// Token: 0x04001641 RID: 5697
		private int numRevealersID = Shader.PropertyToID("_NumRevealers");

		// Token: 0x04001642 RID: 5698
		private int materialColorID = Shader.PropertyToID("_unKnownColor");

		// Token: 0x04001643 RID: 5699
		private int unobscuredBlurRadiusID = Shader.PropertyToID("_unboscuredFadeOutDistance");

		// Token: 0x04001644 RID: 5700
		private int extraRadiusID = Shader.PropertyToID("_extraRadius");

		// Token: 0x04001645 RID: 5701
		private int maxDistanceID = Shader.PropertyToID("_maxDistance");

		// Token: 0x04001646 RID: 5702
		private int fadePowerID = Shader.PropertyToID("_fadePower");

		// Token: 0x04001647 RID: 5703
		private int saturationStrengthID = Shader.PropertyToID("_saturationStrength");

		// Token: 0x04001648 RID: 5704
		private int blurStrengthID = Shader.PropertyToID("_blurStrength");

		// Token: 0x04001649 RID: 5705
		private int blurPixelOffsetMinID = Shader.PropertyToID("_blurPixelOffsetMin");

		// Token: 0x0400164A RID: 5706
		private int blurPixelOffsetMaxID = Shader.PropertyToID("_blurPixelOffsetMax");

		// Token: 0x0400164B RID: 5707
		private int blurSamplesID = Shader.PropertyToID("_blurSamples");

		// Token: 0x0400164C RID: 5708
		private int blurPeriodID = Shader.PropertyToID("_samplePeriod");

		// Token: 0x0400164D RID: 5709
		private int fowTetureID = Shader.PropertyToID("_fowTexture");

		// Token: 0x0400164E RID: 5710
		private int fowTilingID = Shader.PropertyToID("_fowTiling");

		// Token: 0x0400164F RID: 5711
		private int fowSpeedID = Shader.PropertyToID("_fowScrollSpeed");

		// Token: 0x04001650 RID: 5712
		private int currentIndex;

		// Token: 0x04001651 RID: 5713
		private FogOfWarWorld.ConeEdgeStruct[] anglesArray;

		// Token: 0x04001652 RID: 5714
		public static Vector3 UpVector;

		// Token: 0x04001653 RID: 5715
		public static Vector3 ForwardVector;

		// Token: 0x04001654 RID: 5716
		public List<int> DeregisteredIDs = new List<int>();

		// Token: 0x04001655 RID: 5717
		private int numDeregistered;

		// Token: 0x04001656 RID: 5718
		public static List<FogOfWarRevealer> RevealersToRegister = new List<FogOfWarRevealer>();

		// Token: 0x04001657 RID: 5719
		private int[] indiciesDataToSet = new int[1];

		// Token: 0x04001658 RID: 5720
		private FogOfWarWorld.RevealerStruct[] _revealerDataToSet = new FogOfWarWorld.RevealerStruct[1];

		// Token: 0x0200036B RID: 875
		public struct RevealerStruct
		{
			// Token: 0x04001659 RID: 5721
			public Vector2 CircleOrigin;

			// Token: 0x0400165A RID: 5722
			public int StartIndex;

			// Token: 0x0400165B RID: 5723
			public int NumSegments;

			// Token: 0x0400165C RID: 5724
			public float CircleHeight;

			// Token: 0x0400165D RID: 5725
			public float UnobscuredRadius;

			// Token: 0x0400165E RID: 5726
			public float CircleRadius;

			// Token: 0x0400165F RID: 5727
			public float CircleFade;

			// Token: 0x04001660 RID: 5728
			public float VisionHeight;

			// Token: 0x04001661 RID: 5729
			public float HeightFade;

			// Token: 0x04001662 RID: 5730
			public float Opacity;
		}

		// Token: 0x0200036C RID: 876
		public struct ConeEdgeStruct
		{
			// Token: 0x04001663 RID: 5731
			public float angle;

			// Token: 0x04001664 RID: 5732
			public float length;

			// Token: 0x04001665 RID: 5733
			public int cutShort;
		}

		// Token: 0x0200036D RID: 877
		public enum RevealerUpdateMode
		{
			// Token: 0x04001667 RID: 5735
			Every_Frame,
			// Token: 0x04001668 RID: 5736
			N_Per_Frame,
			// Token: 0x04001669 RID: 5737
			Controlled_ElseWhere
		}

		// Token: 0x0200036E RID: 878
		public enum FogSampleMode
		{
			// Token: 0x0400166B RID: 5739
			Pixel_Perfect,
			// Token: 0x0400166C RID: 5740
			Texture,
			// Token: 0x0400166D RID: 5741
			Both
		}

		// Token: 0x0200036F RID: 879
		public enum FogOfWarType
		{
			// Token: 0x0400166F RID: 5743
			Hard,
			// Token: 0x04001670 RID: 5744
			Soft
		}

		// Token: 0x02000370 RID: 880
		public enum FogOfWarFadeType
		{
			// Token: 0x04001672 RID: 5746
			Linear,
			// Token: 0x04001673 RID: 5747
			Exponential,
			// Token: 0x04001674 RID: 5748
			Smooth,
			// Token: 0x04001675 RID: 5749
			Smoother,
			// Token: 0x04001676 RID: 5750
			Smoothstep
		}

		// Token: 0x02000371 RID: 881
		public enum FogOfWarBlendMode
		{
			// Token: 0x04001678 RID: 5752
			Max,
			// Token: 0x04001679 RID: 5753
			Addative
		}

		// Token: 0x02000372 RID: 882
		public enum FogOfWarAppearance
		{
			// Token: 0x0400167B RID: 5755
			Solid_Color,
			// Token: 0x0400167C RID: 5756
			GrayScale,
			// Token: 0x0400167D RID: 5757
			Blur,
			// Token: 0x0400167E RID: 5758
			Texture_Sample,
			// Token: 0x0400167F RID: 5759
			Outline,
			// Token: 0x04001680 RID: 5760
			None
		}

		// Token: 0x02000373 RID: 883
		public enum GamePlane
		{
			// Token: 0x04001682 RID: 5762
			XZ,
			// Token: 0x04001683 RID: 5763
			XY,
			// Token: 0x04001684 RID: 5764
			ZY
		}

		// Token: 0x02000374 RID: 884
		[BurstCompile(CompileSynchronously = true)]
		private struct SetAnglesBuffersJob : IJobParallelFor
		{
			// Token: 0x06001713 RID: 5907 RVA: 0x0006E963 File Offset: 0x0006CB63
			public void Execute(int index)
			{
				this.AnglesArray[index] = this.Angles[index];
			}

			// Token: 0x04001685 RID: 5765
			[ReadOnly]
			public NativeArray<FogOfWarWorld.ConeEdgeStruct> Angles;

			// Token: 0x04001686 RID: 5766
			[WriteOnly]
			public NativeArray<FogOfWarWorld.ConeEdgeStruct> AnglesArray;
		}
	}
}
