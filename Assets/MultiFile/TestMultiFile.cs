using UnityEditor;
using UnityEngine;

/*
 *   Desc: Test select file panel
 *   Date: 2021/12/4
 *   Author: dont-laugh
 */

namespace DontLaugh
{
	internal class TestMultiFile
	{
		[MenuItem("Test/Test MultiFilePanel.Open")]
		private static void TestMultiFilePanel()
		{
			string[] files = MultiFilePanel.Open("Select Files", Application.dataPath, "cs");
			foreach (string file in files)
			{
				Debug.Log(file);
			}
		}

		[MenuItem("Test/Test MultiFilePanel.OpenWithFilter")]
		private static void TestMultiFilePanelWithFilter()
		{
			string[] files = MultiFilePanel.OpenWithFilter
			(
				"Select Files",
				Application.dataPath,
				new[] { "CSharp Code", "*.cs", "Python Code", "*.py" }
			);
			foreach (string file in files)
			{
				Debug.Log(file);
			}
		}
	}
}