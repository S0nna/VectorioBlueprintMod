using System;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x0200030F RID: 783
	public class ListView : MonoBehaviour
	{
		// Token: 0x06001569 RID: 5481 RVA: 0x00061FCB File Offset: 0x000601CB
		private void Awake()
		{
			if (this.itemParent == null)
			{
				Debug.LogError("<b>[List View]</b> 'Item Parent' is missing.");
				return;
			}
			if (this.initializeOnAwake)
			{
				this.InitializeItems();
			}
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00061FF4 File Offset: 0x000601F4
		public void InitializeItems()
		{
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.listItems.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemPreset, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.itemParent, false);
				gameObject.name = this.listItems[i].itemTitle;
				ListViewItem component = gameObject.GetComponent<ListViewItem>();
				component.rowCount = this.rowCount;
				component.row0Ref = this.listItems[i].row0;
				component.row1Ref = this.listItems[i].row1;
				component.row2Ref = this.listItems[i].row2;
				component.PassReferences();
			}
			if (!this.showScrollbar && this.scrollbar != null)
			{
				this.scrollbar.transform.localScale = new Vector3(0f, 0f, 0f);
				return;
			}
			if (this.showScrollbar && this.scrollbar != null)
			{
				this.scrollbar.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}

		// Token: 0x04001363 RID: 4963
		public Transform itemParent;

		// Token: 0x04001364 RID: 4964
		public GameObject itemPreset;

		// Token: 0x04001365 RID: 4965
		public GameObject scrollbar;

		// Token: 0x04001366 RID: 4966
		public bool initializeOnAwake = true;

		// Token: 0x04001367 RID: 4967
		public bool showScrollbar = true;

		// Token: 0x04001368 RID: 4968
		public ListView.RowCount rowCount = ListView.RowCount.Two;

		// Token: 0x04001369 RID: 4969
		[SerializeField]
		public List<ListView.ListItem> listItems = new List<ListView.ListItem>();

		// Token: 0x02000310 RID: 784
		[Serializable]
		public class ListItem
		{
			// Token: 0x0400136A RID: 4970
			public string itemTitle = "List Item";

			// Token: 0x0400136B RID: 4971
			[HideInInspector]
			public ListView.ListRow row0;

			// Token: 0x0400136C RID: 4972
			[HideInInspector]
			public ListView.ListRow row1;

			// Token: 0x0400136D RID: 4973
			[HideInInspector]
			public ListView.ListRow row2;
		}

		// Token: 0x02000311 RID: 785
		[Serializable]
		public class ListRow
		{
			// Token: 0x0400136E RID: 4974
			public ListView.RowType rowType = ListView.RowType.Text;

			// Token: 0x0400136F RID: 4975
			public Sprite rowIcon;

			// Token: 0x04001370 RID: 4976
			public string rowText = "Row text";

			// Token: 0x04001371 RID: 4977
			public bool usePreferredWidth;

			// Token: 0x04001372 RID: 4978
			public int preferredWidth = 50;

			// Token: 0x04001373 RID: 4979
			[Range(0.1f, 1f)]
			public float iconScale = 1f;
		}

		// Token: 0x02000312 RID: 786
		public enum RowType
		{
			// Token: 0x04001375 RID: 4981
			Icon,
			// Token: 0x04001376 RID: 4982
			Text
		}

		// Token: 0x02000313 RID: 787
		public enum RowCount
		{
			// Token: 0x04001378 RID: 4984
			One,
			// Token: 0x04001379 RID: 4985
			Two,
			// Token: 0x0400137A RID: 4986
			Three
		}
	}
}
