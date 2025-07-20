using System;
using System.Collections.Generic;

namespace Vectorio.Entities
{
	// Token: 0x020002AB RID: 683
	[Serializable]
	public class EntityCreationData : NetworkEventBase
	{
		// Token: 0x06001307 RID: 4871 RVA: 0x00057558 File Offset: 0x00055758
		public EntityCreationData(EntityCreationData data)
		{
			this.SyncType = data.SyncType;
			this.FromSave = data.FromSave;
			this.EntityID = data.EntityID;
			this.EntityFlags = data.EntityFlags;
			this.ApplyFlagsPostCreation = data.ApplyFlagsPostCreation;
			this._hasRuntimeID = data._hasRuntimeID;
			this._runtimeID = data._runtimeID;
			this.Callback = data.Callback;
			this.FactionID = data.FactionID;
			this.PosX = data.PosX;
			this.PosY = data.PosY;
			this.Flags = data.Flags;
			this.Checks = data.Checks;
			this.Accent = data.Accent;
			this.ModelID = data.ModelID;
			this.IsBlueprint = false;
			if (this.Has_EFlag(EntityFlags.IsBlueprint))
			{
				this.Set_EFlag(EntityFlags.IsBlueprint, false);
			}
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00034093 File Offset: 0x00032293
		public EntityCreationData()
		{
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x0005763F File Offset: 0x0005583F
		// (set) Token: 0x0600130A RID: 4874 RVA: 0x00057647 File Offset: 0x00055847
		public bool FromSave { get; set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x00057650 File Offset: 0x00055850
		// (set) Token: 0x0600130C RID: 4876 RVA: 0x00057658 File Offset: 0x00055858
		public bool ApplyFlagsPostCreation { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600130D RID: 4877 RVA: 0x00057661 File Offset: 0x00055861
		// (set) Token: 0x0600130E RID: 4878 RVA: 0x00057669 File Offset: 0x00055869
		public EntityMetadata PipetteData
		{
			get
			{
				return this._pipetteData;
			}
			set
			{
				this._pipetteData = value;
				this.HasPipette = (this._pipetteData != null);
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x00057681 File Offset: 0x00055881
		// (set) Token: 0x06001310 RID: 4880 RVA: 0x00057689 File Offset: 0x00055889
		public uint RuntimeID
		{
			get
			{
				return this._runtimeID;
			}
			set
			{
				this._hasRuntimeID = true;
				this._runtimeID = value;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001311 RID: 4881 RVA: 0x00057699 File Offset: 0x00055899
		public bool HasRuntimeID
		{
			get
			{
				return this._hasRuntimeID;
			}
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x000576A4 File Offset: 0x000558A4
		public override string ToString()
		{
			return string.Format("SyncType: {0}\nIsBlueprint: {1}\nEntityID: {2}\n", this.SyncType, this.IsBlueprint, this.EntityID) + string.Format("HasRuntimeID: {0}\nRuntimeID: {1}\nRuntimeID: {2}\n", this._hasRuntimeID, this._runtimeID, this._runtimeID) + string.Format("FactionID: {0}\nPosX: {1}\nPosY: {2}\nFlags: {3}\n", new object[]
			{
				this.FactionID,
				this.PosX,
				this.PosY,
				this.Flags
			}) + string.Format("Checks: {0}\nModelID: {1}", this.Checks, this.ModelID);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00057765 File Offset: 0x00055965
		public bool IsCreationFlagSet(CreationFlags flagToCheck)
		{
			return (this.Flags & flagToCheck) == flagToCheck;
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00057772 File Offset: 0x00055972
		public bool IsCheckFlagSet(CheckFlags flagToCheck)
		{
			return (this.Checks & flagToCheck) == flagToCheck;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0005777F File Offset: 0x0005597F
		private bool Has_EFlag(EntityFlags flag)
		{
			return (this.EntityFlags & flag) == flag;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0005778C File Offset: 0x0005598C
		private void Set_EFlag(EntityFlags flag, bool value)
		{
			if (value)
			{
				this.EntityFlags |= flag;
				return;
			}
			this.EntityFlags &= ~flag;
		}

		// Token: 0x040010A8 RID: 4264
		public SyncType SyncType;

		// Token: 0x040010A9 RID: 4265
		public bool IsBlueprint;

		// Token: 0x040010AB RID: 4267
		public string EntityID;

		// Token: 0x040010AC RID: 4268
		public EntityFlags EntityFlags;

		// Token: 0x040010AE RID: 4270
		public bool _hasRuntimeID;

		// Token: 0x040010AF RID: 4271
		public uint _runtimeID;

		// Token: 0x040010B0 RID: 4272
		public OnCreationCallback Callback;

		// Token: 0x040010B1 RID: 4273
		public string FactionID;

		// Token: 0x040010B2 RID: 4274
		public float PosX;

		// Token: 0x040010B3 RID: 4275
		public float PosY;

		// Token: 0x040010B4 RID: 4276
		public CreationFlags Flags;

		// Token: 0x040010B5 RID: 4277
		public CheckFlags Checks;

		// Token: 0x040010B6 RID: 4278
		public AccentData Accent;

		// Token: 0x040010B7 RID: 4279
		public string ModelID;

		// Token: 0x040010B8 RID: 4280
		public List<VariableContainer> Variables;

		// Token: 0x040010B9 RID: 4281
		public bool HasPipette;

		// Token: 0x040010BA RID: 4282
		public EntityMetadata _pipetteData;
	}
}
