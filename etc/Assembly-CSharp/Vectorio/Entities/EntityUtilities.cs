using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.Entities
{
	// Token: 0x02000293 RID: 659
	public class EntityUtilities : Singleton<EntityUtilities>
	{
		// Token: 0x060012A6 RID: 4774 RVA: 0x00056286 File Offset: 0x00054486
		public override void Awake()
		{
			if (!this._isSetup)
			{
				this._isSetup = true;
				base.Awake();
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00003212 File Offset: 0x00001412
		public void Start()
		{
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x000562A0 File Offset: 0x000544A0
		public void Update()
		{
			if (Singleton<Gamemode>.Instance.IsPaused)
			{
				return;
			}
			if (this._delayedEntityDestruction.Count > 0)
			{
				if (this._delayedEntityDestruction[0].delay > 0f)
				{
					this._delayedEntityDestruction[0].delay -= Time.deltaTime;
					return;
				}
				Entity entity = this._delayedEntityDestruction[0].entity;
				this._delayedEntityDestruction.RemoveAt(0);
				Singleton<EntityManager>.Instance.QueueDestroyEvent(EventBuilder.BuildDestroyEvent(entity, null), SyncType.ServerInitiated);
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0005632E File Offset: 0x0005452E
		public void AddDelayedEntity(Entity entity, float delay = 0.01f)
		{
			this._delayedEntityDestruction.Add(new EntityUtilities.DelayedEntity(entity, delay));
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00056344 File Offset: 0x00054544
		public EntityUtilities.ConnectionLine CreateConnectionLine(EntityUtilities.ConnectionType type, Sprite sprite, Material material, Color color, int sortingOrder, Vector2 startPos, Vector2 endPos)
		{
			SpriteRenderer spriteRenderer = new GameObject("Connection " + startPos.ToString() + " -> " + endPos.ToString())
			{
				transform = 
				{
					localScale = Vector2.one
				}
			}.AddComponent<SpriteRenderer>();
			spriteRenderer.drawMode = SpriteDrawMode.Tiled;
			spriteRenderer.sprite = sprite;
			spriteRenderer.material = material;
			spriteRenderer.color = color;
			spriteRenderer.sortingLayerName = Layers.SORTING_BUILDING_LAYER;
			spriteRenderer.sortingOrder = sortingOrder;
			EntityUtilities.ConnectionLine connectionLine = new EntityUtilities.ConnectionLine(type, spriteRenderer, startPos, endPos);
			connectionLine.UpdateLine(startPos, endPos);
			if (!this._connectionLines.ContainsKey(type))
			{
				this._connectionLines.Add(type, new List<EntityUtilities.ConnectionLine>());
			}
			this._connectionLines[type].Add(connectionLine);
			return connectionLine;
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00056414 File Offset: 0x00054614
		public void DestroyConnectionLine(EntityUtilities.ConnectionLine line)
		{
			if (this._connectionLines.ContainsKey(line.type) && this._connectionLines[line.type].Contains(line))
			{
				line.DestroyLine();
				this._connectionLines[line.type].Remove(line);
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0005646C File Offset: 0x0005466C
		public void CreateExplosion(Entity caller, Vector2 position, float range, float damage, float knockback, string faction)
		{
			LayerMask mask = LayerMask.GetMask(new string[]
			{
				Layers.UNIT_LAYER(faction, true)
			}) | LayerMask.GetMask(new string[]
			{
				Layers.BUILDING_LAYER(faction, true)
			});
			Collider2D[] array = Physics2D.OverlapCircleAll(position, range, mask);
			for (int i = 0; i < array.Length; i++)
			{
				Entity component = array[i].GetComponent<Entity>();
				if (component != null && component.Has_EComponent<HealthComponent>())
				{
					Singleton<EntityManager>.Instance.QueueDamageEvent(EventBuilder.BuildDamageEvent(component, damage, caller), SyncType.ServerInitiated);
				}
			}
			if (caller.IsOnScreen)
			{
				Singleton<AudioPlayer>.Instance.Play(this.explosionSound, "sound_explosion", 1f, true, 0.9f, 1.1f, false);
			}
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00056524 File Offset: 0x00054724
		public void CreateBuildingExplosion(ParticleSystem particle, Vector2 position)
		{
			Object.Instantiate<ParticleSystem>(particle, position, Quaternion.identity);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00056538 File Offset: 0x00054738
		public void CreateBulletParticle(ParticleSystem particle, Vector2 position, Quaternion rotation)
		{
			ParticleSystemRenderer component = Object.Instantiate<ParticleSystem>(particle, position, rotation).GetComponent<ParticleSystemRenderer>();
			component.sortingLayerName = Layers.PARTICLE_LAYER;
			component.transform.rotation *= Quaternion.Euler(0f, 0f, 90f);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0005658C File Offset: 0x0005478C
		public void CreateBulletParticle(ParticleSystem particle, Material material, Vector2 position, Quaternion rotation)
		{
			ParticleSystemRenderer component = Object.Instantiate<ParticleSystem>(particle, position, rotation).GetComponent<ParticleSystemRenderer>();
			if (material != null)
			{
				component.material = material;
				component.trailMaterial = material;
			}
			component.sortingLayerName = Layers.PARTICLE_LAYER;
			component.transform.rotation *= Quaternion.Euler(0f, 0f, 90f);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000565FC File Offset: 0x000547FC
		public void CreateBulletParticle(ParticleSystem particle, Color color, Vector2 position, Quaternion rotation)
		{
			ParticleSystem particleSystem = Object.Instantiate<ParticleSystem>(particle, position, rotation);
			ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
			component.sortingLayerName = Layers.PARTICLE_LAYER;
			component.transform.rotation *= Quaternion.Euler(0f, 0f, 90f);
			particleSystem.main.startColor = color;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00056664 File Offset: 0x00054864
		public void ShowRange(Vector2 position, float range)
		{
			this.rangeViewer.gameObject.SetActive(true);
			this.rangeViewer.transform.position = position;
			this.rangeViewer.transform.localScale = new Vector2(range, range);
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000566B4 File Offset: 0x000548B4
		public void HideRange()
		{
			this.rangeViewer.gameObject.SetActive(false);
		}

		// Token: 0x04001059 RID: 4185
		protected List<EntityUtilities.DelayedEntity> _delayedEntityDestruction = new List<EntityUtilities.DelayedEntity>();

		// Token: 0x0400105A RID: 4186
		protected Dictionary<EntityUtilities.ConnectionType, List<EntityUtilities.ConnectionLine>> _connectionLines = new Dictionary<EntityUtilities.ConnectionType, List<EntityUtilities.ConnectionLine>>();

		// Token: 0x0400105B RID: 4187
		protected EntityUtilities.DelayedEntity _delayedEntity;

		// Token: 0x0400105C RID: 4188
		protected float _entityDelay;

		// Token: 0x0400105D RID: 4189
		public ParticleSystem explosionParticle;

		// Token: 0x0400105E RID: 4190
		public AudioClip explosionSound;

		// Token: 0x0400105F RID: 4191
		public Material generatorLine;

		// Token: 0x04001060 RID: 4192
		public Sprite generatorSprite;

		// Token: 0x04001061 RID: 4193
		public SpriteRenderer rangeViewer;

		// Token: 0x04001062 RID: 4194
		protected bool _isSetup;

		// Token: 0x02000294 RID: 660
		public class DelayedEntity
		{
			// Token: 0x060012B4 RID: 4788 RVA: 0x000566E5 File Offset: 0x000548E5
			public DelayedEntity(Entity entity, float delay)
			{
				this.entity = entity;
				this.delay = delay;
			}

			// Token: 0x04001063 RID: 4195
			public Entity entity;

			// Token: 0x04001064 RID: 4196
			public float delay;
		}

		// Token: 0x02000295 RID: 661
		public class ConnectionLine
		{
			// Token: 0x060012B5 RID: 4789 RVA: 0x000566FB File Offset: 0x000548FB
			public ConnectionLine(EntityUtilities.ConnectionType type, SpriteRenderer line, Vector2 startPos, Vector2 endPos)
			{
				this.type = type;
				this.line = line;
				this.startPos = startPos;
				this.endPos = endPos;
			}

			// Token: 0x060012B6 RID: 4790 RVA: 0x00056720 File Offset: 0x00054920
			public void DestroyLine()
			{
				if (this.line.gameObject != null)
				{
					Object.Destroy(this.line.gameObject);
				}
			}

			// Token: 0x060012B7 RID: 4791 RVA: 0x00056748 File Offset: 0x00054948
			public void UpdateLine(Vector2 startPos, Vector2 endPos)
			{
				float num = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * 57.29578f;
				Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, num + 180f));
				Vector2 v = (startPos + endPos) / 2f;
				this.line.transform.position = v;
				this.line.transform.rotation = rotation;
				this.line.size = new Vector2(Vector2.Distance(startPos, endPos), 1f);
			}

			// Token: 0x04001065 RID: 4197
			public EntityUtilities.ConnectionType type;

			// Token: 0x04001066 RID: 4198
			private SpriteRenderer line;

			// Token: 0x04001067 RID: 4199
			public Vector2 startPos;

			// Token: 0x04001068 RID: 4200
			public Vector2 endPos;
		}

		// Token: 0x02000296 RID: 662
		public enum ConnectionType
		{
			// Token: 0x0400106A RID: 4202
			Undefined,
			// Token: 0x0400106B RID: 4203
			DroneRoute,
			// Token: 0x0400106C RID: 4204
			Generator
		}
	}
}
