using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

[RequireComponent(typeof(TimeComponent))]
public class LifeTimeComponent : PMonoBehaviour, IComponent
{
	public float LifeTime = 2f;
	public float LifeCounter
	{
		get { return lifeCounter; }
		set { lifeCounter = value; }
	}

	float lifeCounter;
}
