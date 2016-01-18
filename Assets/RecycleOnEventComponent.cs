using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

public class RecycleOnEventComponent : PMonoBehaviour, IComponent
{
	public EntityBehaviour ToRecycle;
	public Events RecycleEvents;
}
