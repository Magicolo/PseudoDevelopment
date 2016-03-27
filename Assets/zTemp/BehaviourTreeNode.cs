using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public abstract class BehaviourTreeNodez : ComponentBehaviour
{
	protected void SendMessage(BehaviourTreeMessages message)
	{
		var parent = Entity.Parent;

		if (parent != null)
			parent.SendMessage(message, this);
	}

	[Message(BehaviourTreeMessages.Success)]
	protected abstract void NodeSuccess(BehaviourTreeNodez node);

	[Message(BehaviourTreeMessages.Success)]
	protected virtual void NodeSuccess()
	{
		SendMessage(BehaviourTreeMessages.Success);
	}

	[Message(BehaviourTreeMessages.Failure)]
	protected abstract void NodeFailure(BehaviourTreeNodez node);

	[Message(BehaviourTreeMessages.Failure)]
	protected virtual void NodeFailure()
	{
		SendMessage(BehaviourTreeMessages.Failure);
	}
}