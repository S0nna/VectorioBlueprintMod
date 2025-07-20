using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200006D RID: 109
public class ModelConstructor : Singleton<ModelConstructor>
{
	// Token: 0x0600050B RID: 1291 RVA: 0x0001A8C7 File Offset: 0x00018AC7
	public void Start()
	{
		this._defaultMaterial = Library.GetDefaultMaterial();
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0001A8D4 File Offset: 0x00018AD4
	public ModelObject GetModel(Model model, Entity parent, string layer, bool excludeBlueprintLayers = false)
	{
		ModelObject modelObject = this.BuildModel(model, layer, excludeBlueprintLayers);
		if (parent != null)
		{
			modelObject.transform.SetParent(parent.transform);
			modelObject.transform.localPosition = Vector2.zero;
			parent.SetModel(modelObject);
		}
		return modelObject;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0001A924 File Offset: 0x00018B24
	private ModelObject BuildModel(Model model, string layer, bool excludeBlueprintLayers)
	{
		ModelObject modelObject = new GameObject(model.id).AddComponent<ModelObject>();
		modelObject.AssignID(model.id);
		foreach (SpriteLayer spriteLayer in model.layers)
		{
			if (!(spriteLayer.sprite == null) && (!excludeBlueprintLayers || !spriteLayer.hideOnBlueprint))
			{
				GameObject gameObject = new GameObject(spriteLayer.sprite.name);
				gameObject.transform.SetParent(modelObject.transform);
				gameObject.transform.localScale = new Vector2(spriteLayer.size, spriteLayer.size);
				gameObject.transform.localPosition = new Vector2(spriteLayer.posX, spriteLayer.posY);
				if (spriteLayer.useRotation)
				{
					gameObject.transform.localRotation = Quaternion.Euler(gameObject.transform.localRotation.x, gameObject.transform.localRotation.y, spriteLayer.rotation);
				}
				SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = spriteLayer.sprite;
				spriteRenderer.color = spriteLayer.color;
				spriteRenderer.material = spriteLayer.material;
				spriteRenderer.sortingLayerName = layer;
				spriteRenderer.sortingOrder = spriteLayer.index;
				modelObject.RegisterLayer(spriteLayer, spriteRenderer);
			}
		}
		return modelObject;
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0001AA84 File Offset: 0x00018C84
	public GameObject BuildInterfaceModel(Model model, Vector2 size, Transform parent, Accent accent = null)
	{
		GameObject gameObject = new GameObject(parent.name + "_" + model.id);
		gameObject.transform.SetParent(parent);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		if (accent != null)
		{
			foreach (SpriteLayer spriteLayer in model.layers)
			{
				if (!(spriteLayer.sprite == null) && !spriteLayer.hideOnUI)
				{
					Image image = this.CreateInterfaceSprite(spriteLayer, gameObject, size);
					switch (spriteLayer.accent)
					{
					case AccentType.PrimaryColor:
						image.color = accent.primaryColor;
						break;
					case AccentType.SecondaryColor:
						image.color = accent.secondaryColor;
						break;
					case AccentType.PrimaryMaterial:
						image.color = accent.primaryMaterial.color;
						break;
					case AccentType.SecondaryMaterial:
						image.color = accent.secondaryMaterial.color;
						break;
					}
				}
			}
		}
		else
		{
			foreach (SpriteLayer spriteLayer2 in model.layers)
			{
				if (!(spriteLayer2.sprite == null) && !spriteLayer2.hideOnUI)
				{
					this.CreateInterfaceSprite(spriteLayer2, gameObject, size);
				}
			}
		}
		return gameObject;
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x0001ABCC File Offset: 0x00018DCC
	private Image CreateInterfaceSprite(SpriteLayer layer, GameObject parent, Vector2 size)
	{
		Image image = new GameObject(layer.sprite.name).AddComponent<Image>();
		image.transform.SetParent(parent.transform);
		image.transform.localPosition = new Vector2(layer.posX, layer.posY);
		image.transform.localScale = Vector3.one;
		if (layer.useRotation)
		{
			image.transform.localRotation = Quaternion.Euler(image.transform.localRotation.x, image.transform.localRotation.y, layer.rotation);
		}
		image.sprite = (layer.overrideUI ? layer.overrideSprite : layer.sprite);
		if (layer.material != null)
		{
			if (layer.material == this._defaultMaterial)
			{
				image.color = layer.color;
			}
			else
			{
				image.color = layer.material.color;
			}
		}
		else
		{
			Debug.Log("[MODEL] Missing material reference for " + parent.name + " model");
		}
		image.material = this._defaultMaterial;
		image.GetComponent<RectTransform>().sizeDelta = size;
		image.preserveAspect = true;
		return image;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0001AD08 File Offset: 0x00018F08
	public ModelObject BuildBlueprintModel(Model model, Transform parent)
	{
		ModelObject modelObject = new GameObject("Hologram").AddComponent<ModelObject>();
		if (parent != null)
		{
			modelObject.transform.SetParent(parent);
			modelObject.transform.localPosition = Vector3.zero;
		}
		modelObject.AssignID(model.id);
		foreach (SpriteLayer spriteLayer in model.layers)
		{
			if (!(spriteLayer.sprite == null) && !spriteLayer.hideOnBlueprint)
			{
				SpriteRenderer spriteRenderer = new GameObject(spriteLayer.sprite.name).AddComponent<SpriteRenderer>();
				spriteRenderer.transform.SetParent(modelObject.transform);
				spriteRenderer.transform.localScale = new Vector2(spriteLayer.size, spriteLayer.size);
				spriteRenderer.transform.localPosition = new Vector2(spriteLayer.posX, spriteLayer.posY);
				if (spriteLayer.useRotation)
				{
					spriteRenderer.transform.localRotation = Quaternion.Euler(spriteRenderer.transform.localRotation.x, spriteRenderer.transform.localRotation.y, spriteLayer.rotation);
				}
				spriteRenderer.sprite = spriteLayer.sprite;
				spriteRenderer.color = spriteLayer.color;
				spriteRenderer.sortingLayerName = Layers.HOLOGRAM_LAYER;
				spriteRenderer.sortingOrder = spriteLayer.index;
				spriteRenderer.material = this._hologramMaterial;
			}
		}
		return modelObject;
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0001AE80 File Offset: 0x00019080
	public Indicator ConstructIndicator(Transform parent, Vector2 localPosition, string layer)
	{
		Indicator indicator = Object.Instantiate<Indicator>(this._indicatorPrefab);
		indicator.transform.SetParent(parent);
		indicator.Setup(localPosition, layer);
		return indicator;
	}

	// Token: 0x040002EE RID: 750
	[SerializeField]
	private Material _hologramMaterial;

	// Token: 0x040002EF RID: 751
	[SerializeField]
	private Indicator _indicatorPrefab;

	// Token: 0x040002F0 RID: 752
	private Material _defaultMaterial;
}
