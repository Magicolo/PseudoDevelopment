using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

public class EventRelayerComponent : PMonoBehaviour, IComponent
{
	public Events EventsToRelay;
	public EntityBehaviour[] RelayTo;
}
