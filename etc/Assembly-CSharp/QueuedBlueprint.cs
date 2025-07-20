using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E0 RID: 480
public class QueuedBlueprint : MonoBehaviour
{
	// Token: 0x06000EE3 RID: 3811 RVA: 0x00044BA0 File Offset: 0x00042DA0
	public void Setup(Blueprint blueprint)
	{
		this._entityData = Library.RequestData<EntityData>(blueprint.EntityID);
		this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(this._entityData.model, this.modelSize, this.modelParent, null);
		this.title.text = this._entityData.Name.ToUpper() + "<size=7>(x" + this._amount.ToString() + ")</size>";
		this.resourceOne.text = this._entityData.NormalCost.amount.ToString();
		this.resourceTwo.text = this._entityData.SpecialCost.amount.ToString();
		this.resourceIconOne.sprite = this._entityData.SpecialCost.resource.IconSprite;
		this.resourceIconTwo.sprite = this._entityData.SpecialCost.resource.IconSprite;
	}

	// Token: 0x04000BF5 RID: 3061
	public Transform modelParent;

	// Token: 0x04000BF6 RID: 3062
	public TextMeshProUGUI title;

	// Token: 0x04000BF7 RID: 3063
	public TextMeshProUGUI resourceOne;

	// Token: 0x04000BF8 RID: 3064
	public TextMeshProUGUI resourceTwo;

	// Token: 0x04000BF9 RID: 3065
	public Image resourceIconOne;

	// Token: 0x04000BFA RID: 3066
	public Image resourceIconTwo;

	// Token: 0x04000BFB RID: 3067
	public Vector2 modelSize;

	// Token: 0x04000BFC RID: 3068
	protected EntityData _entityData;

	// Token: 0x04000BFD RID: 3069
	protected GameObject _model;

	// Token: 0x04000BFE RID: 3070
	protected int _amount = 1;
}
