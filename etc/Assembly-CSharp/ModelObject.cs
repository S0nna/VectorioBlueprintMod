using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class ModelObject : MonoBehaviour
{
	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0004AE23 File Offset: 0x00049023
	public string ID
	{
		get
		{
			return this._modelID;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0004AE2B File Offset: 0x0004902B
	// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x0004AE33 File Offset: 0x00049033
	public bool HasCustomModel
	{
		get
		{
			return this._hasCustomModel;
		}
		set
		{
			this._hasCustomModel = value;
		}
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0004AE3C File Offset: 0x0004903C
	public void AssignID(string id)
	{
		this._modelID = id;
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0004AE45 File Offset: 0x00049045
	public Accent GetAccent()
	{
		return this._accent;
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x0004AE4D File Offset: 0x0004904D
	public bool HasAccent
	{
		get
		{
			return this._hasAccent;
		}
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x0004AE55 File Offset: 0x00049055
	public AccentData GetAccentData()
	{
		return new AccentData(this._accent);
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x0004AE64 File Offset: 0x00049064
	public List<ModelObject.Layer> Layers
	{
		get
		{
			List<ModelObject.Layer> list = new List<ModelObject.Layer>();
			foreach (List<ModelObject.Layer> collection in this._layers.Values)
			{
				list.AddRange(collection);
			}
			return list;
		}
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0004AEC4 File Offset: 0x000490C4
	public Color GetColor(AccentType accent)
	{
		if (!this._layers.ContainsKey(accent))
		{
			return Color.white;
		}
		if (this._layers[accent].Count <= 0)
		{
			Debug.Log("[MODEL OBJECT] Could not return color. (1)");
			return Color.white;
		}
		if (this._layers[accent][0] == null)
		{
			Debug.Log("[MODEL OBJECT] Could not return color. (2)");
			return Color.white;
		}
		if (this._layers[accent][0].spriteRenderer != null)
		{
			return this._layers[accent][0].spriteRenderer.color;
		}
		Debug.Log("[MODEL OBJECT] Could not return color. (3)");
		return Color.white;
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x0004AF7C File Offset: 0x0004917C
	public Material GetMaterial(AccentType accent)
	{
		if (!this._layers.ContainsKey(accent))
		{
			return Library.GetDefaultMaterial();
		}
		if (this._layers[accent].Count <= 0)
		{
			Debug.Log("[MODEL OBJECT] Could not return material. (1)");
			return Library.GetDefaultMaterial();
		}
		if (this._layers[accent][0] == null)
		{
			Debug.Log("[MODEL OBJECT] Could not return material. (2)");
			return Library.GetDefaultMaterial();
		}
		if (this._layers[accent][0].spriteRenderer != null)
		{
			return this._layers[accent][0].spriteRenderer.material;
		}
		Debug.Log("[MODEL OBJECT] Could not return material. (3)");
		return Library.GetDefaultMaterial();
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0004B033 File Offset: 0x00049233
	public List<Transform> GetAnimationGroup(AnimationGroup group)
	{
		if (!this._animationGroups.ContainsKey(group))
		{
			return null;
		}
		return this._animationGroups[group];
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0004B054 File Offset: 0x00049254
	public void RegisterLayer(SpriteLayer layer, SpriteRenderer sprite)
	{
		if (!this._isRendererTargetSetup)
		{
			this._rendererTarget = sprite;
			this._isRendererTargetSetup = true;
		}
		if (!this._layers.ContainsKey(layer.accent))
		{
			this._layers.Add(layer.accent, new List<ModelObject.Layer>());
		}
		this._layers[layer.accent].Add(new ModelObject.Layer(sprite));
		if (layer.animationGroup != AnimationGroup.None)
		{
			if (!this._animationGroups.ContainsKey(layer.animationGroup))
			{
				this._animationGroups.Add(layer.animationGroup, new List<Transform>());
			}
			this._animationGroups[layer.animationGroup].Add(sprite.transform);
		}
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x00003212 File Offset: 0x00001412
	public void ApplySuperAccent(Accent accent)
	{
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0004B10C File Offset: 0x0004930C
	public void ApplyAccent(Accent accent)
	{
		this._accent = accent;
		this._hasAccent = true;
		if (this._layers.ContainsKey(AccentType.PrimaryColor))
		{
			List<ModelObject.Layer> list = this._layers[AccentType.PrimaryColor];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == null || list[i].spriteRenderer == null)
				{
					list.RemoveAt(i);
					i--;
				}
				else
				{
					list[i].spriteRenderer.color = accent.primaryColor;
				}
			}
		}
		if (this._layers.ContainsKey(AccentType.SecondaryColor))
		{
			List<ModelObject.Layer> list2 = this._layers[AccentType.SecondaryColor];
			for (int j = 0; j < list2.Count; j++)
			{
				if (list2[j] == null || list2[j].spriteRenderer == null)
				{
					list2.RemoveAt(j);
					j--;
				}
				else
				{
					list2[j].spriteRenderer.color = accent.secondaryColor;
				}
			}
		}
		if (this._layers.ContainsKey(AccentType.PrimaryMaterial))
		{
			List<ModelObject.Layer> list3 = this._layers[AccentType.PrimaryMaterial];
			for (int k = 0; k < list3.Count; k++)
			{
				if (list3[k] == null || list3[k].spriteRenderer == null)
				{
					list3.RemoveAt(k);
					k--;
				}
				else
				{
					list3[k].spriteRenderer.material = accent.primaryMaterial;
				}
			}
		}
		if (this._layers.ContainsKey(AccentType.SecondaryMaterial))
		{
			List<ModelObject.Layer> list4 = this._layers[AccentType.SecondaryMaterial];
			for (int l = 0; l < list4.Count; l++)
			{
				if (list4[l] == null || list4[l].spriteRenderer == null)
				{
					list4.RemoveAt(l);
					l--;
				}
				else
				{
					list4[l].spriteRenderer.material = accent.secondaryMaterial;
				}
			}
		}
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0004B2FC File Offset: 0x000494FC
	public void ResetAccent()
	{
		this._accent = null;
		this._hasAccent = false;
		if (this._layers.ContainsKey(AccentType.PrimaryColor))
		{
			foreach (ModelObject.Layer layer in this._layers[AccentType.PrimaryColor])
			{
				layer.spriteRenderer.color = layer.GetOriginalColor();
			}
		}
		if (this._layers.ContainsKey(AccentType.SecondaryColor))
		{
			foreach (ModelObject.Layer layer2 in this._layers[AccentType.SecondaryColor])
			{
				layer2.spriteRenderer.color = layer2.GetOriginalColor();
			}
		}
		if (this._layers.ContainsKey(AccentType.PrimaryMaterial))
		{
			foreach (ModelObject.Layer layer3 in this._layers[AccentType.PrimaryMaterial])
			{
				layer3.spriteRenderer.material = layer3.GetOriginalMaterial();
			}
		}
		if (this._layers.ContainsKey(AccentType.SecondaryMaterial))
		{
			foreach (ModelObject.Layer layer4 in this._layers[AccentType.SecondaryMaterial])
			{
				layer4.spriteRenderer.material = layer4.GetOriginalMaterial();
			}
		}
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x0004B4A0 File Offset: 0x000496A0
	public void ConnectRenderer(Entity entity)
	{
		if (this._isRendererTargetSetup && !this._isRendererSetup)
		{
			if (this._rendererTarget == null)
			{
				return;
			}
			this._renderer = this._rendererTarget.AddComponent<EntityRenderer>().Setup(entity);
			this._isRendererSetup = true;
		}
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0004B4E0 File Offset: 0x000496E0
	public void DisconnectRenderer()
	{
		if (this._isRendererSetup)
		{
			if (this._renderer != null)
			{
				Entity parent = this._renderer.Parent;
				Object.Destroy(this._renderer);
				if (parent != null)
				{
					parent.IsOnScreen = false;
				}
			}
			this._isRendererSetup = false;
		}
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x0004B531 File Offset: 0x00049731
	public void Reset()
	{
		this.DisconnectRenderer();
		this.ResetAccent();
	}

	// Token: 0x04000E13 RID: 3603
	private string _modelID = "default";

	// Token: 0x04000E14 RID: 3604
	private bool _hasCustomModel;

	// Token: 0x04000E15 RID: 3605
	public EntityRenderer _renderer;

	// Token: 0x04000E16 RID: 3606
	public bool _isRendererSetup;

	// Token: 0x04000E17 RID: 3607
	public SpriteRenderer _rendererTarget;

	// Token: 0x04000E18 RID: 3608
	public bool _isRendererTargetSetup;

	// Token: 0x04000E19 RID: 3609
	private Accent _accent;

	// Token: 0x04000E1A RID: 3610
	private bool _hasAccent;

	// Token: 0x04000E1B RID: 3611
	[SerializeField]
	private Dictionary<AccentType, List<ModelObject.Layer>> _layers = new Dictionary<AccentType, List<ModelObject.Layer>>();

	// Token: 0x04000E1C RID: 3612
	[SerializeField]
	private Dictionary<AnimationGroup, List<Transform>> _animationGroups = new Dictionary<AnimationGroup, List<Transform>>();

	// Token: 0x02000220 RID: 544
	public class Layer
	{
		// Token: 0x06001003 RID: 4099 RVA: 0x0004B568 File Offset: 0x00049768
		public Layer(SpriteRenderer spriteRenderer)
		{
			this.spriteRenderer = spriteRenderer;
			this._originalColor = spriteRenderer.color;
			this._originalMaterial = spriteRenderer.material;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0004B58F File Offset: 0x0004978F
		public Material GetOriginalMaterial()
		{
			return this._originalMaterial;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0004B597 File Offset: 0x00049797
		public Color GetOriginalColor()
		{
			return this._originalColor;
		}

		// Token: 0x04000E1D RID: 3613
		public SpriteRenderer spriteRenderer;

		// Token: 0x04000E1E RID: 3614
		private Material _originalMaterial;

		// Token: 0x04000E1F RID: 3615
		private Color _originalColor;
	}
}
