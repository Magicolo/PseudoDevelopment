using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.BehaviourTree;

public class zTest : PMonoBehaviour
{
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	[Inject]
	IEntityManager entityManager = null;
	public int Iterations = 1000;

	[Button]
	public bool test;
	void Test()
	{
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

	//T Add<T>(T a, T b)
	//{
	//	var type = typeof(T);

	//	if (type == typeof(int))
	//		return Cast<T>.T
	//}

	void Update()
	{
		if (SpawnMany)
			entityManager.CreateEntity(Entity);
	}

	float property1 = 2;
	float property2 = 1.1f;

	public float Property1
	{
		get { return property1; }
		set { property1 = value; }
	}

	public float Property2
	{
		get { return property2; }
		set { property2 = value; }
	}

	public float Method1(float a)
	{
		return a + 1f;
	}

	public void Method2(zTest test, float d, float b) { }
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