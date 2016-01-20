using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;
using Zenject;

public class MotionComponent : PMonoBehaviour, IComponent
{
	[Polar]
	public Vector2 Motion = Vector2.right;
}