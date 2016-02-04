using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class RecycleOnMessage : ComponentBehaviour, IMessageable
{
	public MessageEnum Message;

	bool recycle;

	public void LateUpdate()
	{
		if (recycle)
			EntityHolder.Recycle();
	}

	public void OnMessage<TId>(TId message)
	{
		recycle |= Message.Equals(message);
	}
}
