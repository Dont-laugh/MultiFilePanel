using System;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

/*
 *   Desc: Open multi select file panel
 *   Date: 2021/12/4
 *   Author: dont-laugh
 */

namespace DontLaugh
{
	public class MultiFilePanel
	{
		private const string PYTHON_PATH = "Assets/MultiFile/MultiFilePanel.py";

		public static string[] Open(string title, string directory, string extension)
		{
			var filters = new string[2];
			filters[0] = extension;
			filters[1] = "*." + extension;
			return Internal_Open(title, directory, filters);
		}

		public static string[] OpenWithFilter(string title, string directory, string[] filters)
		{
			return Internal_Open(title, directory, filters);
		}

		private static string[] Internal_Open(string title, string directory, string[] filters)
		{
#if UNITY_EDITOR_OSX
			const string TERMINAL = "/bin/bash";
#elif UNITY_EDITOR_WIN
			const string TERMINAL = "cmd.exe";
#endif

			var fileTypesArg = string.Empty;

			for (int i = 0; i < filters.Length; i += 2)
			{
				fileTypesArg += $"{filters[i]},{filters[i + 1]}";
				if (i < filters.Length - 2)
					fileTypesArg += ";";
			}

			var cmd = $"python {PYTHON_PATH} -title \"{title}\" -directory \"{directory}\" -filetypes \"{fileTypesArg}\"";
			Debug.Log(cmd);

			using var p = new Process();
			p.StartInfo.FileName = TERMINAL;           // 运行控制台
			p.StartInfo.CreateNoWindow = true;         // 不显示窗口
			p.StartInfo.UseShellExecute = false;       // 不使用操作系统外壳程序启动进程
			p.StartInfo.RedirectStandardInput = true;  // 重定向输入流，否则不能写入命令
			p.StartInfo.RedirectStandardOutput = true; // 重定向输出流，否则不能读取输出

			p.Start();
			p.StandardInput.Flush();
			p.StandardInput.WriteLine(cmd);
			p.StandardInput.Close();

			string output = p.StandardOutput.ReadToEnd();
			p.StandardOutput.Close();
			p.WaitForExit();

			Debug.Log(output);

			string[] lines = output.Replace("\r", string.Empty).Split('\n');
			var retStr = string.Empty;

#if UNITY_EDITOR_OSX
			if (lines.Length > 0)
			{
				retStr = lines[0].Trim('(', ')').Replace("'", string.Empty);
			}
#elif UNITY_EDITOR_WIN
			for (var i = 0; i < lines.Length; i++)
			{
				if (lines[i].Contains(cmd))
				{
					retStr = lines[i + 1].Trim('(', ')').Replace("'", string.Empty);
					break;
				}
			}
#endif

			if (string.IsNullOrEmpty(retStr))
			{
				return Array.Empty<string>();
			}

			string[] files = retStr.Split(',', ' ');
			ArrayUtility.Remove(ref files, string.Empty);
			return files;
		}
	}
}