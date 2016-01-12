using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class MotionComponent : PMonoBehaviour, IMotionComponent
{
	[SerializeField, Polar]
	Vector2 motion;

	public Vector2 Motion
	{
		get { return motion; }
		set { motion = value; }
	}
}

public interface IMotionComponent : Pseudo.Internal.Entity3.IComponent
{
	Vector2 Motion { get; set; }
}