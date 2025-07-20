using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class StorageSettings : EntitySettings
{
	// Token: 0x06000799 RID: 1945 RVA: 0x00021F4C File Offset: 0x0002014C
	public override void Set(EntityComponent component)
	{
		ResourceModule resourceModule = component as ResourceModule;
		if (resourceModule != null)
		{
			this._hasInputContainer = (resourceModule.HasInputContainer() && !resourceModule.RouteInputToOutput);
			this._hasOutputContainer = resourceModule.HasOutputContainer();
			if (this._hasInputContainer)
			{
				this._inputContainer = resourceModule.GetInputContainer();
				this._inputResources = new Dictionary<ResourceData, StoredResourceInfo>();
				this.hasInputContainerObj.SetActive(true);
				this.noInputContainerObj.SetActive(false);
				this.inputInvalidObj.SetActive(false);
				if (this._inputContainer.StorageMode == StorageMode.LocalizedStorage)
				{
					this.inputLocalizedObj.SetActive(true);
				}
			}
			else
			{
				this.hasInputContainerObj.SetActive(false);
				this.noInputContainerObj.SetActive(true);
				this.inputCapacity.text = "NO STORAGE AVAILABLE";
				this.inputInvalidObj.SetActive(true);
			}
			if (!this._hasOutputContainer)
			{
				this.hasOutputContainerObj.SetActive(false);
				this.noOutputContainerObj.SetActive(true);
				this.outputCapacity.text = "NO STORAGE AVAILABLE";
				this.outputInvalidObj.SetActive(true);
				return;
			}
			this._outputContainer = resourceModule.GetOutputContainer();
			this._outputResources = new Dictionary<ResourceData, StoredResourceInfo>();
			this.hasOutputContainerObj.SetActive(true);
			this.noOutputContainerObj.SetActive(false);
			this.outputInvalidObj.SetActive(false);
			if (this._outputContainer.StorageMode == StorageMode.LocalizedStorage)
			{
				this.outputLocalizedObj.SetActive(true);
				return;
			}
		}
		else
		{
			Debug.LogWarning("The provided component does not match the setting type.");
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x000220BC File Offset: 0x000202BC
	public unsafe override void CustomUpdate()
	{
		if (this._hasInputContainer)
		{
			foreach (StoredResource storedResource in this._inputContainer.GetStoredResources())
			{
				if (!this._inputResources.ContainsKey(storedResource.ResourceData))
				{
					StoredResourceInfo storedResourceInfo = Object.Instantiate<StoredResourceInfo>(this.infoPrefab);
					storedResourceInfo.transform.SetParent(this.inputList);
					storedResourceInfo.transform.localScale = Vector2.one;
					storedResourceInfo.Set(storedResource, this._inputContainer.Storage);
					this._inputResources.Add(storedResource.ResourceData, storedResourceInfo);
				}
				this._inputResources[storedResource.ResourceData].CustomUpdate();
			}
			if (this._inputContainer.StorageMode == StorageMode.SharedStorage)
			{
				this.inputCapacity.text = "<b>CAPACITY:</b> <size=9>" + this._inputContainer.AmountStored.ToString() + "/" + this._inputContainer.Storage->Value.ToString();
				if (this._inputContainer.IsFull(null))
				{
					if (!this.inputCapacityObj.gameObject.activeSelf)
					{
						this.inputCapacityObj.SetActive(true);
					}
				}
				else if (this.inputCapacityObj.activeSelf)
				{
					this.inputCapacityObj.SetActive(false);
				}
			}
			else
			{
				this.inputCapacity.text = "<b>CAPACITY:</b> <size=9>" + this._inputContainer.Storage->Value.ToString() + " per resource";
			}
		}
		if (this._hasOutputContainer)
		{
			foreach (StoredResource storedResource2 in this._outputContainer.GetStoredResources())
			{
				if (!this._outputResources.ContainsKey(storedResource2.ResourceData))
				{
					StoredResourceInfo storedResourceInfo2 = Object.Instantiate<StoredResourceInfo>(this.infoPrefab);
					storedResourceInfo2.transform.SetParent(this.outputList);
					storedResourceInfo2.Set(storedResource2, this._outputContainer.Storage);
					storedResourceInfo2.transform.localScale = Vector2.one;
					this._outputResources.Add(storedResource2.ResourceData, storedResourceInfo2);
				}
				this._outputResources[storedResource2.ResourceData].CustomUpdate();
			}
			if (this._outputContainer.StorageMode == StorageMode.SharedStorage)
			{
				this.outputCapacity.text = "<b>CAPACITY:</b> <size=9>" + this._outputContainer.AmountStored.ToString() + "/" + this._outputContainer.Storage->Value.ToString();
				if (this._outputContainer.IsFull(null))
				{
					if (!this.outputCapacityObj.activeSelf)
					{
						this.outputCapacityObj.SetActive(true);
						return;
					}
				}
				else if (this.outputCapacityObj.activeSelf)
				{
					this.outputCapacityObj.SetActive(false);
					return;
				}
			}
			else
			{
				this.outputCapacity.text = "<b>CAPACITY:</b> <size=9>" + this._outputContainer.Storage->Value.ToString() + " per resource";
			}
		}
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00003212 File Offset: 0x00001412
	public override void Clear()
	{
	}

	// Token: 0x04000508 RID: 1288
	private ResourceContainer _inputContainer;

	// Token: 0x04000509 RID: 1289
	private ResourceContainer _outputContainer;

	// Token: 0x0400050A RID: 1290
	public StoredResourceInfo infoPrefab;

	// Token: 0x0400050B RID: 1291
	public Transform inputList;

	// Token: 0x0400050C RID: 1292
	public Transform outputList;

	// Token: 0x0400050D RID: 1293
	public TextMeshProUGUI inputCapacity;

	// Token: 0x0400050E RID: 1294
	public TextMeshProUGUI outputCapacity;

	// Token: 0x0400050F RID: 1295
	public TextMeshProUGUI inputPending;

	// Token: 0x04000510 RID: 1296
	public TextMeshProUGUI outputPending;

	// Token: 0x04000511 RID: 1297
	public GameObject hasInputContainerObj;

	// Token: 0x04000512 RID: 1298
	public GameObject noInputContainerObj;

	// Token: 0x04000513 RID: 1299
	public GameObject inputCapacityObj;

	// Token: 0x04000514 RID: 1300
	public GameObject inputLocalizedObj;

	// Token: 0x04000515 RID: 1301
	public GameObject inputInvalidObj;

	// Token: 0x04000516 RID: 1302
	public GameObject hasOutputContainerObj;

	// Token: 0x04000517 RID: 1303
	public GameObject noOutputContainerObj;

	// Token: 0x04000518 RID: 1304
	public GameObject outputCapacityObj;

	// Token: 0x04000519 RID: 1305
	public GameObject outputLocalizedObj;

	// Token: 0x0400051A RID: 1306
	public GameObject outputInvalidObj;

	// Token: 0x0400051B RID: 1307
	private bool _hasInputContainer;

	// Token: 0x0400051C RID: 1308
	private bool _hasOutputContainer;

	// Token: 0x0400051D RID: 1309
	private Dictionary<ResourceData, StoredResourceInfo> _inputResources;

	// Token: 0x0400051E RID: 1310
	private Dictionary<ResourceData, StoredResourceInfo> _outputResources;
}
