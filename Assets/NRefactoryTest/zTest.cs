using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.EventSystems;
using System.Threading;
using System.Reflection;
using Pseudo.Internal.Audio;
using System.Runtime.Serialization;
using Pseudo.Internal;
using UnityEngine.SceneManagement;
using Pseudo.Internal.Entity;
using Zenject;
using Pseudo.Internal.Pool;

// TimeComponent.Time is not reset when pooled.
public class zTest : PMonoBehaviour
{
	public EntityBehaviour Entity;
	[Inject]
	IEntityManager entityManager = null;
	[Inject]
	ISystemManager systemManager = null;
	public const int iterations = 1000;

	[Button]
	public bool test;
	void Test()
	{
		entityManager.CreateEntity(Entity);
		//SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 2);
	}

	void Awake()
	{
		systemManager.AddSystem<MotionSystem>();
		systemManager.AddSystem<InputMotionSystem>();
		systemManager.AddSystem<SoundSystem>();
		systemManager.AddSystem<EventRelaySystem>();
		systemManager.AddSystem<LifeTimeSystem>();
		systemManager.AddSystem<RecycleSystem>();
	}

	void Update()
	{
		//for (int i = 0; i < 100; i++)
		entityManager.CreateEntity(Entity);
	}
}

[Serializable]
public class Events : PEnumFlag<Events>
{
	public static readonly Events OnAll = new Events(1, 2, 3);
	public static readonly Events OnEquip = new Events(1);
	public static readonly Events OnUnequip = new Events(2);
	public static readonly Events OnBuy = new Events(3);
	public static readonly Events OnDie = new Events(4);

	protected Events(params byte[] values) : base(values) { }

	public override bool Equals(Events other)
	{
		return HasAny(other);
	}
}

public class MotionSystem : SystemBase, IUpdateable
{
	public bool Active { get; set; }
	public float UpdateDelay { get { return 0f; } }

	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

		Active = true;
		entities = EntityManager.Entities.Filter(new[]
		{
			typeof(MotionComponent),
			typeof(TimeComponent)
		});
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
		{
			var entity = entities[i];
			var time = entity.GetComponent<TimeComponent>();
			var motion = entity.GetComponent<MotionComponent>();

			motion.CachedTransform.position += motion.Motion.ToVector3() * time.DeltaTime;

			if (time.Time % 3 > 2.9f)
				EventManager.Trigger(Events.OnEquip, entity);
		}
	}
}

public class InputMotionSystem : SystemBase, IUpdateable
{
	public bool Active { get; set; }
	public float UpdateDelay { get { return 0f; } }

	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

		Active = true;
		entities = EntityManager.Entities.Filter(new[]
		{
			typeof(TimeComponent),
			typeof(InputMotionComponent)
		});
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
		{
			var entity = entities[i];
			var time = entity.GetComponent<TimeComponent>();
			var motion = entity.GetComponent<InputMotionComponent>();

			motion.CachedTransform.Translate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * time.DeltaTime * motion.Speed, Axes.XY);
		}
	}
}

public class SoundSystem : SystemBase
{
	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

		entities = EntityManager.Entities.Filter(typeof(SoundComponent));
		EventManager.SubscribeAll((Action<Events, IEntity>)OnEvent);
	}

	void OnEvent(Events identifier, IEntity entity)
	{
		if (!entities.Contains(entity))
			return;

		var sound = entity.GetComponent<SoundComponent>();

		if (sound.PlayEvent.HasAll(identifier))
			AudioManager.CreateItem(sound.Sound, sound.CachedTransform.position).Play();
	}
}

public class EventRelaySystem : SystemBase
{
	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

		entities = EntityManager.Entities.Filter(typeof(EventRelayerComponent));
		EventManager.SubscribeAll((Action<Events, IEntity>)OnEvent);
	}

	void OnEvent(Events identifier, IEntity entity)
	{
		if (!entities.Contains(entity))
			return;

		var eventRelayer = entity.GetComponent<EventRelayerComponent>();

		if (eventRelayer.EventsToRelay.HasAll(identifier))
		{
			foreach (var relayTo in eventRelayer.RelayTo)
				EventManager.Trigger(identifier, relayTo.Entity);
		}
	}
}

public class LifeTimeSystem : SystemBase, IUpdateable
{
	public bool Active { get; set; }

	public float UpdateDelay
	{
		get { return 0f; }
	}

	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

		Active = true;
		entities = EntityManager.Entities.Filter(new[]
		{
			typeof(LifeTimeComponent),
			typeof(TimeComponent)
		});
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
		{
			var entity = entities[i];
			var lifeTime = entity.GetComponent<LifeTimeComponent>();
			var time = entity.GetComponent<TimeComponent>();

			lifeTime.LifeCounter += time.DeltaTime;

			if (lifeTime.LifeCounter >= lifeTime.LifeTime)
				EventManager.Trigger(Events.OnDie, entity);
		}
	}
}

public class RecycleSystem : SystemBase
{
	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

		entities = EntityManager.Entities.Filter(typeof(RecycleOnEventComponent));
		EventManager.SubscribeAll((Action<Events, IEntity>)OnEvent);
	}

	void OnEvent(Events identifier, IEntity entity)
	{
		if (!entities.Contains(entity))
			return;

		var recycle = entity.GetComponent<RecycleOnEventComponent>();

		if (recycle.RecycleEvents.HasAll(identifier))
			recycle.ToRecycle.Recycle();
	}
}