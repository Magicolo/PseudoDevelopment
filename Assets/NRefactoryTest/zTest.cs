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
using Pseudo.Internal.Entity3;

public class zTest : PMonoBehaviour
{
	public PEntity Entity2;
	SystemManager systemManager = new SystemManager();
	IEntityManager entityManager = Pseudo.Internal.Entity3.EntityManager.Instance;

	[Button]
	public bool test;
	void Test()
	{
	}

	void Awake()
	{
		var motionSystem = new MotionSystem(systemManager, entityManager);
		systemManager.AddSystem(motionSystem);
	}

	void Update()
	{
		systemManager.Update();
	}

	void FixedUpdate()
	{
		systemManager.FixedUpdate();
	}

	void LateUpdate()
	{
		systemManager.LateUpdate();
	}
}

public class MotionSystem : SystemBase, IUpdateable
{
	public bool Active
	{
		get { return active; }
		set { active = value; }
	}
	public float UpdateRate { get { return 0f; } }

	bool active = true;
	Pseudo.Internal.Entity3.IEntityGroup moveableGroup;

	public MotionSystem(ISystemManager systemManager, IEntityManager entityManager) : base(systemManager, entityManager)
	{
		moveableGroup = entityManager.GetEntityGroup(new[] {
			typeof(ITransformComponent),
			typeof(IMotionComponent),
			typeof(ITimeComponent)});
	}

	public void Update()
	{
		for (int i = 0; i < moveableGroup.Entities.Count; i++)
		{
			var entity = moveableGroup.Entities[i];
			var time = entity.GetComponent<ITimeComponent>();
			var transform = entity.GetComponent<ITransformComponent>().Transform;
			var motion = entity.GetComponent<IMotionComponent>().Motion;

			transform.position += motion.ToVector3() * time.DeltaTime;
		}
	}
}