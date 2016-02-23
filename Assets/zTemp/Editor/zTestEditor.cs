using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEditor;
using Pseudo.Internal.Editor;

[CustomEditor(typeof(zTest))]
public class zTestEditor : CustomEditorBase
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var test = (zTest)target;

		test.Data = ObjectField(test.Data, "Value".ToGUIContent());

		if (GUILayout.Button("ChangeData"))
			test.Data = new object[] { 1, 1f, new MinMax(), new Vector2(), new Color(), new AnimationCurve() }.GetRandom();
	}
}
