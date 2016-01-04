using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.EventSystems;
using System.Threading;
using System.Reflection;
using Pseudo.Internal.Audio;
using System.Runtime.Serialization;
using Pseudo.Internal;
using Pseudo.Internal.Tests;

public class zTest : PMonoBehaviour
{
	public AnimationCurve Original;
	public AnimationCurve Copy;

	[Button]
	public bool test;
	void Test()
	{
	}

	void Update()
	{
		if (Original.length != Copy.length)
			Copy.keys = Original.keys;
		else
		{
			for (int i = 0; i < Original.length; i++)
				Copy.MoveKey(i, Original[i]);
		}
	}
}