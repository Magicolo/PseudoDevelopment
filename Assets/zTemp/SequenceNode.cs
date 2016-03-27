using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class SequenceNodez : BehaviourTreeNodez
{
	public bool Repeat;
	public BehaviourTreeNodez[] Nodes;

	int currentNodeIndex;
	BehaviourTreeNodez currentNode;

	public override void OnAdded()
	{
		base.OnAdded();

		for (int i = 0; i < Nodes.Length; i++)
		{
			var node = Nodes[i];

			if (node != null)
				node.CachedGameObject.SetActive(false);
		}

		SetCurrentNode(Nodes.First());
	}

	void SetCurrentNode(BehaviourTreeNodez node)
	{
		if (currentNode != null)
			currentNode.CachedGameObject.SetActive(false);

		currentNode = node;

		if (currentNode != null)
			currentNode.CachedGameObject.SetActive(true);
	}

	protected override void NodeSuccess(BehaviourTreeNodez node)
	{
		if (currentNode != node)
			return;

		currentNodeIndex++;

		if (Repeat)
			currentNodeIndex %= Nodes.Length;

		if (currentNodeIndex >= Nodes.Length)
		{
			currentNodeIndex = -1;
			SetCurrentNode(null);
			SendMessage(BehaviourTreeMessages.Success);
		}
		else
			SetCurrentNode(Nodes[currentNodeIndex]);
	}

	protected override void NodeFailure(BehaviourTreeNodez node)
	{
		if (currentNode != node)
			return;

		currentNodeIndex = -1;
		SetCurrentNode(null);
		SendMessage(BehaviourTreeMessages.Failure);
	}
}