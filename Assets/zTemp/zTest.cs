using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.BehaviourTree;
using System.Reflection;
using Pseudo.Internal;
using Pseudo.Internal.Injection;
using UnityEngine.SceneManagement;

public class zTest : PMonoBehaviour
{
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	public int Iterations = 1000;

	[Inject]
	readonly IEntityManager entityManager = null;
	[Inject]
	readonly IBinder binder = null;

	[Button]
	public bool test;
	void Test()
	{
		PDebug.Log(binder);
		//	type.GetMethods(flags)
		//	.Where(m => !m.IsSpecialName)
		//	.Concat(types
		//	.SelectMany(t => t.GetMethods(flags))
		//	.Where(m => m.IsPrivate && !m.IsSpecialName))
		//	.Select(m => string.Format("{0}.{1} : {2} ({3})\n", m.DeclaringType.Name, m.Name, m.ReturnType.Name, m.ReflectedType))
		//	.ToArray());

		//var treeNode = new BehaviourTreeAsset
		//{
		//	Root = new SequenceNode
		//	{
		//		Children = new List<NodeBase>
		//		{
		//			new LogNode(),
		//			new LogNode()
		//		}
		//	}
		//};

		//var tree = treeNode.CreateAction();

		//PDebug.Log(tree.State);

		//while (tree.Update(null) == ActionStates.Running)
		//	PDebug.Log(tree.State);

		//PDebug.Log(tree.State);
	}

	void Update()
	{
		if (SpawnMany)
			entityManager.CreateEntity(Entity);
	}
}

[MessageEnum]
public enum Messages : byte
{
	Zero,
	One,
	Two,
	Three,
}

public class LogAction : ActionBase
{
	readonly int maxCount;
	int count;

	public LogAction(int maxCount)
	{
		this.maxCount = maxCount;
	}

	public override ActionStates OnExecute(BehaviourTree tree)
	{
		count++;
		PDebug.Log(this, GetHashCode(), count);

		if (count < maxCount)
			return ActionStates.Running;
		else
			return ActionStates.Success;
	}
}

public class LogNode : NodeBase
{
	public override IAction CreateAction()
	{
		return new LogAction(3);
	}
}