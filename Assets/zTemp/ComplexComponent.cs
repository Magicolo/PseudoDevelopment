using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class ComplexComponent : ComponentBehaviour
{
	[Serializable]
	public class DataHolder
	{
		public int Data1;
		public float Data2;
		public string Data3;
		public UnityEngine.Object Data4;
		public Transform Data5;
	}

	[InitializeContent]
	public DataHolder[] Data;
}