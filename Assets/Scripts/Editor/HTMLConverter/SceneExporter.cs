using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HTMLConverter
{
	public class SceneExporter
	{
		private static readonly string HTML_TEMPLATE_RELATIVE_PATH = "Templates/index.html";
		private static readonly string JS_TEMPLATE_RELATIVE_PATH = "Templates/viewer.js";

		[MenuItem("Tools/Export To HTML5")]
		public static void Export()
		{
			Debug.Log("[HTML Scene Exporter] Export started");
			string exportFolderPath = EditorUtility.OpenFolderPanel("Select path for HTML5 export", "", "");

			if (string.IsNullOrEmpty(exportFolderPath))
			{
				Debug.LogError($"[HTML Scene Exporter] Invalid folder selected at path {exportFolderPath}");

				return;
			}

			Debug.Log($"[HTML Scene Exporter] Selected folder: {exportFolderPath}");

			SceneData sceneData = GetSceneData();

			string sceneDataJson = JsonUtility.ToJson(sceneData, prettyPrint: true);
			WriteExportFiles(exportFolderPath, sceneDataJson);
			
			Debug.Log($"[HTML Scene Exporter] Export finished. \nPath: {exportFolderPath}");
		}

		private static SceneData GetSceneData()
		{
			SceneData sceneData = new SceneData();
			Camera mainCamera = Camera.main;
			sceneData.Camera = new CameraData()
			{
				Position = mainCamera.transform.position,
				Rotation = mainCamera.transform.rotation
			};

			HTMLCube[] htmlCubes = GameObject.FindObjectsByType<HTMLCube>(FindObjectsSortMode.None);

			sceneData.Cubes = new List<CubeData>();

			for (int i = 0; i < htmlCubes.Length; i++)
			{
				HTMLCube cube = htmlCubes[i];
				HTMLCube parentCube = htmlCubes[i].transform.parent?.GetComponent<HTMLCube>();
				int parentIdex = -1;

				if (parentCube)
				{
					parentIdex = System.Array.IndexOf(htmlCubes, parentCube);
				}

				sceneData.Cubes.Add(new CubeData()
				{
					ParentIndex = parentIdex,
					Position = cube.transform.localPosition,
					Rotation = cube.transform.localEulerAngles,
					Scale = cube.transform.localScale
				});
			}

			return sceneData;
		}

		private static void WriteExportFiles(string exportFolderPath, string sceneDataJsonText)
		{
			string htmlTemplatePath = Path.Combine(Application.dataPath, HTML_TEMPLATE_RELATIVE_PATH);
			string jsTemplatePath = Path.Combine(Application.dataPath, JS_TEMPLATE_RELATIVE_PATH);
			string htmlOutputPath = Path.Combine(exportFolderPath, "index.html");
			string jsOutputPath = Path.Combine(exportFolderPath, "viewer.js");
			string htmlText = File.ReadAllText(htmlTemplatePath);
			string jsText = File.ReadAllText(jsTemplatePath);

			if (string.IsNullOrEmpty(htmlText) || string.IsNullOrEmpty(jsText))
			{
				Debug.LogError("[HTML Scene Exporter] Failed to read template files");

				return;
			}

			if (!htmlText.Contains("const SCENE_DATA = {}"))
			{
				Debug.LogWarning(
					$"[HTML Scene Exporter] Invalid html template at path {htmlTemplatePath}. \nMissing 'const SCENE_DATA = {{}}' in body section.");
			}

			htmlText = htmlText.Replace("const SCENE_DATA = {}", $"const SCENE_DATA = {sceneDataJsonText}");

			File.WriteAllText(htmlOutputPath, htmlText);
			File.WriteAllText(jsOutputPath, jsText);
			EditorUtility.RevealInFinder(exportFolderPath);
			AssetDatabase.Refresh();
		}
	}
}
