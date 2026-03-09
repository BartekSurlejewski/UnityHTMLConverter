using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HTMLConverter
{
	public class SceneExporter
	{
		[MenuItem("Tools/Export To HTML5")]
		public static void Export()
		{
			Debug.Log("Exporting");
			string exportFolderPath = EditorUtility.OpenFolderPanel("Select path for HTML5 export", "", "");

			if (string.IsNullOrEmpty(exportFolderPath))
			{
				return;
			}

			Debug.Log($"Selected folder: {exportFolderPath}");

			SceneData sceneData = new SceneData();
			Camera mainCamera = Camera.main;
			sceneData.Camera = new CameraData()
			{
				Position = mainCamera.transform.position,
				Rotation = mainCamera.transform.rotation
			};

			HTMLCube[] htmlCubes = GameObject.FindObjectsByType<HTMLCube>(FindObjectsSortMode.None);

			sceneData.Cubes = new List<CubeData>();

			foreach (HTMLCube cube in htmlCubes)
			{
				sceneData.Cubes.Add(new CubeData()
				{
					ParentIndex = -1,
					Position = cube.transform.position,
					Rotation = cube.transform.eulerAngles,
					Scale = cube.transform.lossyScale
				});
			}

			string sceneDataJson = JsonUtility.ToJson(sceneData, prettyPrint: true);

			string filePath = Path.Combine(exportFolderPath, "scene.json");
			File.WriteAllText(filePath, sceneDataJson);
		}
	}
}
