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
	SystemManager systemManager;

	[Button]
	public bool test;
	void Test()
	{
	}

	void Awake()
	{
		systemManager = new SystemManager();
		//systemManager.AddSystem(new MotionSystem(systemManager));
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

//public class MotionSystem : SystemBase, IFixedUpdateable
//{
//	public bool Active { get; set; }

//Pseudo.Internal.Entity3.IEntityGroup movables = EntityManager.GetEntityGroup(new Type[] { typeof(IMotionComponent), typeof(IPositionComponent) });

//public MotionSystem(ISystemManager systemManager) : base(systemManager) { }

//public void FixedUpdate()
//{
//	for (int i = 0; i < movables.Entities.Count; i++)
//	{
//		var movable = movables.Entities[i];
//		var position = movable.GetComponent<IPositionComponent>();
//		var motion = movable.GetComponent<IMotionComponent>();

//		position.Position += Vector3.right * motion.Speed;
//	}
//}
//}

public class MotionComponent : IMotionComponent
{
	public float Speed { get; set; }
}

public interface IMotionComponent : Pseudo.Internal.Entity3.IComponent
{
	float Speed { get; set; }
}

public class PositionComponent : IPositionComponent
{
	public Vector3 Position { get; set; }
}

public interface IPositionComponent : Pseudo.Internal.Entity3.IComponent
{
	Vector3 Position { get; set; }
}