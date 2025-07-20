using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class NetworkPackageValidator
{
	// Token: 0x06000BD7 RID: 3031 RVA: 0x00033D10 File Offset: 0x00031F10
	public void Reset()
	{
		this._creationEventPositions.Clear();
		this._damageEventEntities.Clear();
		this._destroyEventEntities.Clear();
		this._metadataEventEntities.Clear();
		this._startedCallbackEvents.Clear();
		this._finishedCallbackEvents.Clear();
		this._syncEventEntities.Clear();
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x00033D6A File Offset: 0x00031F6A
	public void AddCreationEventPosition(string id, Vector2 position)
	{
		if (!this._creationEventPositions.ContainsKey(id))
		{
			this._creationEventPositions.Add(id, new HashSet<Vector2>());
		}
		this._creationEventPositions[id].Add(position);
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00033D9E File Offset: 0x00031F9E
	public void AddDamageEventEntity(uint id)
	{
		this._damageEventEntities.Add(id);
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00033DAD File Offset: 0x00031FAD
	public void AddDestroyEventEntity(uint id)
	{
		this._destroyEventEntities.Add(id);
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x00033DBC File Offset: 0x00031FBC
	public void AddMetadataEventEntity(uint id)
	{
		this._metadataEventEntities.Add(id);
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x00033DCB File Offset: 0x00031FCB
	public void AddStartedCallbackEvent(uint id)
	{
		this._startedCallbackEvents.Add(id);
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x00033DDA File Offset: 0x00031FDA
	public void AddFinishedCallbackEvent(uint id)
	{
		this._finishedCallbackEvents.Add(id);
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x00033DE9 File Offset: 0x00031FE9
	public void AddSyncEvent(uint ID)
	{
		this._syncEventEntities.Add(ID);
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x00033DF8 File Offset: 0x00031FF8
	public bool HasCreateEvent(string id, Vector2 position)
	{
		return this._creationEventPositions.ContainsKey(id) && this._creationEventPositions[id].Contains(position);
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x00033E1C File Offset: 0x0003201C
	public bool HasDamageEvent(uint entity_ID)
	{
		return this._damageEventEntities.Contains(entity_ID);
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x00033E2A File Offset: 0x0003202A
	public bool HasDestroyEvent(uint entity_ID)
	{
		return this._destroyEventEntities.Contains(entity_ID);
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x00033E38 File Offset: 0x00032038
	public bool HasMetadataEvent(uint entity_ID)
	{
		return this._metadataEventEntities.Contains(entity_ID);
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x00033E46 File Offset: 0x00032046
	public bool HasStartedCallbackEvent(uint entity_ID)
	{
		return this._startedCallbackEvents.Contains(entity_ID);
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x00033E54 File Offset: 0x00032054
	public bool HasFinishedCallbackEvent(uint entity_ID)
	{
		return this._finishedCallbackEvents.Contains(entity_ID);
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00033E62 File Offset: 0x00032062
	public bool HasSyncEvent(uint entity_ID)
	{
		return this._syncEventEntities.Contains(entity_ID);
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00033E70 File Offset: 0x00032070
	public void ResetCreationEvent(string id, Vector2 position)
	{
		if (this.HasCreateEvent(id, position))
		{
			this._creationEventPositions.Remove(id);
		}
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x00033E89 File Offset: 0x00032089
	public void ResetDamageEventEntity(uint id)
	{
		if (this.HasDamageEvent(id))
		{
			this._damageEventEntities.Remove(id);
		}
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x00033EA1 File Offset: 0x000320A1
	public void ResetDestroyEventEntity(uint id)
	{
		if (this.HasDestroyEvent(id))
		{
			this._destroyEventEntities.Remove(id);
		}
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x00033EB9 File Offset: 0x000320B9
	public void ResetMetadataEventEntity(uint id)
	{
		if (this.HasMetadataEvent(id))
		{
			this._metadataEventEntities.Remove(id);
		}
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x00033ED1 File Offset: 0x000320D1
	public void ResetStartedCallbackEvent(uint id)
	{
		if (this.HasStartedCallbackEvent(id))
		{
			this._startedCallbackEvents.Remove(id);
		}
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00033EE9 File Offset: 0x000320E9
	public void ResetFinishedCallbackEvent(uint id)
	{
		if (this.HasFinishedCallbackEvent(id))
		{
			this._finishedCallbackEvents.Remove(id);
		}
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00033F01 File Offset: 0x00032101
	public void ResetSyncEvent(uint id)
	{
		if (this.HasSyncEvent(id))
		{
			this._syncEventEntities.Remove(id);
		}
	}

	// Token: 0x04000809 RID: 2057
	private Dictionary<string, HashSet<Vector2>> _creationEventPositions = new Dictionary<string, HashSet<Vector2>>();

	// Token: 0x0400080A RID: 2058
	private HashSet<uint> _damageEventEntities = new HashSet<uint>();

	// Token: 0x0400080B RID: 2059
	private HashSet<uint> _destroyEventEntities = new HashSet<uint>();

	// Token: 0x0400080C RID: 2060
	private HashSet<uint> _metadataEventEntities = new HashSet<uint>();

	// Token: 0x0400080D RID: 2061
	private HashSet<uint> _startedCallbackEvents = new HashSet<uint>();

	// Token: 0x0400080E RID: 2062
	private HashSet<uint> _finishedCallbackEvents = new HashSet<uint>();

	// Token: 0x0400080F RID: 2063
	private HashSet<uint> _syncEventEntities = new HashSet<uint>();
}
