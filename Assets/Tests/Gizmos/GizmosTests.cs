using UnityEngine;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Editor.Internal;

namespace Pseudo.Tests
{
	public class GizmosTests : MonoBehaviour
	{
		void OnDrawGizmos()
		{
			DrawUtility.DrawText(transform.position, "KWAME est content");
		}
	}
}