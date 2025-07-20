using System;
using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000314 RID: 788
	public class ListViewItem : MonoBehaviour
	{
		// Token: 0x0600156E RID: 5486 RVA: 0x000621F0 File Offset: 0x000603F0
		public void PassReferences()
		{
			if (this.rowCount == ListView.RowCount.One)
			{
				this.row0.gameObject.SetActive(true);
				this.row1.gameObject.SetActive(false);
				this.row2.gameObject.SetActive(false);
			}
			else if (this.rowCount == ListView.RowCount.Two)
			{
				this.row0.gameObject.SetActive(true);
				this.row1.gameObject.SetActive(true);
				this.row2.gameObject.SetActive(false);
			}
			else if (this.rowCount == ListView.RowCount.Three)
			{
				this.row0.gameObject.SetActive(true);
				this.row1.gameObject.SetActive(true);
				this.row2.gameObject.SetActive(true);
			}
			if (this.row0Ref.rowType == ListView.RowType.Icon)
			{
				this.row0.iconImage.sprite = this.row0Ref.rowIcon;
				this.row0.iconImage.gameObject.SetActive(true);
				this.row0.textObject.gameObject.SetActive(false);
				this.row0.iconImage.transform.localScale = new Vector3(this.row0Ref.iconScale, this.row0Ref.iconScale, this.row0Ref.iconScale);
			}
			else if (this.row0Ref.rowType == ListView.RowType.Text)
			{
				this.row0.textObject.text = this.row0Ref.rowText;
				this.row0.iconImage.gameObject.SetActive(false);
				this.row0.textObject.gameObject.SetActive(true);
			}
			if (this.row0Ref.usePreferredWidth)
			{
				this.row0.layoutElement.preferredWidth = (float)this.row0Ref.preferredWidth;
			}
			else
			{
				this.row0.layoutElement.preferredWidth = -1f;
			}
			if (this.row1Ref == null)
			{
				return;
			}
			if (this.row1Ref.rowType == ListView.RowType.Icon)
			{
				this.row1.iconImage.sprite = this.row1Ref.rowIcon;
				this.row1.iconImage.gameObject.SetActive(true);
				this.row1.textObject.gameObject.SetActive(false);
				this.row1.iconImage.transform.localScale = new Vector3(this.row1Ref.iconScale, this.row1Ref.iconScale, this.row1Ref.iconScale);
			}
			else if (this.row1Ref.rowType == ListView.RowType.Text)
			{
				this.row1.textObject.text = this.row1Ref.rowText;
				this.row1.iconImage.gameObject.SetActive(false);
				this.row1.textObject.gameObject.SetActive(true);
			}
			if (this.row1Ref.usePreferredWidth)
			{
				this.row1.layoutElement.preferredWidth = (float)this.row1Ref.preferredWidth;
			}
			else
			{
				this.row1.layoutElement.preferredWidth = -1f;
			}
			if (this.row2Ref == null)
			{
				return;
			}
			if (this.row2Ref.rowType == ListView.RowType.Icon)
			{
				this.row2.iconImage.sprite = this.row2Ref.rowIcon;
				this.row2.iconImage.gameObject.SetActive(true);
				this.row2.textObject.gameObject.SetActive(false);
				this.row2.iconImage.transform.localScale = new Vector3(this.row2Ref.iconScale, this.row2Ref.iconScale, this.row2Ref.iconScale);
			}
			else if (this.row2Ref.rowType == ListView.RowType.Text)
			{
				this.row2.textObject.text = this.row2Ref.rowText;
				this.row2.iconImage.gameObject.SetActive(false);
				this.row2.textObject.gameObject.SetActive(true);
			}
			if (this.row2Ref.usePreferredWidth)
			{
				this.row2.layoutElement.preferredWidth = (float)this.row2Ref.preferredWidth;
				return;
			}
			this.row2.layoutElement.preferredWidth = -1f;
		}

		// Token: 0x0400137B RID: 4987
		[Header("Settings")]
		public ListViewRow row0;

		// Token: 0x0400137C RID: 4988
		public ListViewRow row1;

		// Token: 0x0400137D RID: 4989
		public ListViewRow row2;

		// Token: 0x0400137E RID: 4990
		[Header("References")]
		public ListView.RowCount rowCount;

		// Token: 0x0400137F RID: 4991
		public ListView.ListRow row0Ref;

		// Token: 0x04001380 RID: 4992
		public ListView.ListRow row1Ref;

		// Token: 0x04001381 RID: 4993
		public ListView.ListRow row2Ref;
	}
}
