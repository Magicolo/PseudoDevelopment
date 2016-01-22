using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

[RequireComponent(typeof(TimeComponent))]
public class InputMotionComponent : ComponentBehaviour
{
	public float Speed = 5f;
}
