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

// TODO EventManager should resolve events at end of frame
// TODO EventManager should allow to listen to all events with an argument (IEntity)

public class zTest : PMonoBehaviour
{
	public SomeEnum EnumFlag;
	public Events Enum;
	public PEntity Entity2;
	[Inject]
	ISystemManager systemManager = null;
	const int iterations = 1000;

	[Button]
	public bool test;
	void Test()
	{

	}

	void Awake()
	{
		systemManager.AddSystem<MotionSystem>();
		systemManager.AddSystem<InputMotionSystem>();
	}

	void Update()
	{
	}
}

public partial class SomeEnum
{
	public static readonly SomeEnum Option_4 = new SomeEnum(7);
}

[Serializable]
public partial class SomeEnum : PEnumFlag<SomeEnum>
{
	public static readonly SomeEnum Option_1 = new SomeEnum(1, 2, 3);
	public static readonly SomeEnum Option_2 = new SomeEnum(4, 5);
	public static readonly SomeEnum Option_3 = new SomeEnum(6);

	protected SomeEnum(params byte[] values) : base(values) { }
}

[Serializable]
public class Events : PEnum<Events, int>
{
	public static readonly Events On_Move = new Events(1);
	public static readonly Events On_Damage = new Events(2);

	protected Events(int value) : base(value) { }
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

		EventManager.Subscribe(Events.On_Move, () => { });
		EventManager.Subscribe((Events identifier) => { });
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
		{
			var entity = entities[i];
			var time = entity.GetComponent<TimeComponent>();
			var motion = entity.GetComponent<MotionComponent>();

			motion.CachedTransform.position += motion.Motion.ToVector3() * time.DeltaTime;

			//if (time.Time % 5 > 4.5f)
			EventManager.Trigger(Events.On_Move);
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