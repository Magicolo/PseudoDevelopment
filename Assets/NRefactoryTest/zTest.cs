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

public class zTest : PMonoBehaviour
{
	[Inject]
	ISystemManager systemManager = null;

	[Button]
	public bool test;
	void Test()
	{
		//SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 2);
	}

	void Awake()
	{
		systemManager.AddSystem<MotionSystem>();
		systemManager.AddSystem<InputMotionSystem>();
		systemManager.AddSystem<SoundSystem>();
	}
}

[Serializable]
public partial class Events : PEnumFlag<Events>
{
	public static readonly Events OnAll = new Events(1, 2, 3);
	public static readonly Events OnEquip = new Events(1);
	public static readonly Events OnUnequip = new Events(2);
	public static readonly Events OnBuy = new Events(3);

	protected Events(params byte[] values) : base(values) { }

	public override bool Equals(Events other)
	{
		return HasAny(other);
	}
}

public class MotionSystem : SystemBase, IUpdateable
{
	public bool Active
	{
		get { return active; }
		set { active = value; }
	}
	public float UpdateDelay { get { return 0f; } }

	bool active = true;
	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

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
	public bool Active
	{
		get { return active; }
		set { active = value; }
	}
	public float UpdateDelay { get { return 0f; } }

	bool active = true;
	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

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
	protected override void Initialize()
	{
		base.Initialize();

		EventManager.Subscribe(Events.OnAll, (Action<IEntity>)OnEvent);
	}

	void OnEvent(IEntity entity)
	{
		if (entity == null)
			return;

		SoundComponent sound;

		if (entity.TryGetComponent(out sound))
			AudioManager.CreateItem(sound.Sound, sound.CachedTransform.position).Play();
	}
}

public class EventRelaySystem : SystemBase
{
	IEntityGroup entities;

	protected override void Initialize()
	{
		base.Initialize();

		EventManager.SubscribeAll((Action<Events>)OnEntityEvent);

		entities = EntityManager.Entities.Filter(new[]
		{
			typeof(EventRelayerComponent),
		});
	}

	void OnEntityEvent(Events entityEvent)
	{
		for (int i = 0; i < entities.Count; i++)
		{
			var entity = entities[i];
			var eventRelayer = entity.GetComponent<EventRelayerComponent>();

			if (eventRelayer.EventsToRelay.HasAll(entityEvent))
			{
				foreach (var relayTo in eventRelayer.RelayTo)
					EventManager.Trigger(entityEvent, relayTo.Entity);
			}
		}
	}
}