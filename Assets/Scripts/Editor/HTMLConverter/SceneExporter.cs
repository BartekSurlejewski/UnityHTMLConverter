using System;
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
			try
			{
				Debug.Log("[HTML Scene Exporter] Export started");
				string exportFolderPath = EditorUtility.OpenFolderPanel("Select path for HTML5 export", "", "");

				if (string.IsNullOrEmpty(exportFolderPath))
				{
					Debug.Log($"[HTML Scene Exporter] No export path selected, export cancelled.");

					return;
				}

				Debug.Log($"[HTML Scene Exporter] Selected folder: {exportFolderPath}");

				SceneData sceneData = GetSceneData();

				string sceneDataJson = JsonUtility.ToJson(sceneData, prettyPrint: true);
				WriteExportFiles(exportFolderPath, sceneDataJson);

				Debug.Log($"[HTML Scene Exporter] Export finished. \nPath: {exportFolderPath}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);

				throw;
			}
		}

		private static SceneData GetSceneData()
		{
			SceneData sceneData = new SceneData();
			Camera mainCamera = Camera.main;

			if (!mainCamera)
			{
				throw new Exception("No main camera found in the scene");
			}

			sceneData.Camera = new CameraData()
			{
				Position = ToWebPosition(mainCamera.transform.position),
				Rotation = ToWebRotation(mainCamera.transform.rotation)
			};

			Transform[] sceneTransforms = GameObject.FindObjectsByType<Transform>(FindObjectsSortMode.None);

			sceneData.SceneObjects = new List<SceneObjectData>();

			for (int i = 0; i < sceneTransforms.Length; i++)
			{
				Transform sceneTransform = sceneTransforms[i];
				Transform parentTransform = sceneTransforms[i].transform.parent;
				int parentIndex = -1;

				if (parentTransform)
				{
					parentIndex = System.Array.IndexOf(sceneTransforms, parentTransform);
				}

				sceneData.SceneObjects.Add(new SceneObjectData()
				{
					Name = sceneTransform.name,
					PrimitiveType = GetPrimitiveType(sceneTransform),
					ParentIndex = parentIndex,
					Position = ToWebPosition(sceneTransform.localPosition),
					Rotation = ToWebRotation(sceneTransform.localEulerAngles),
					Scale = sceneTransform.localScale
				});
			}

			return sceneData;

			static HtmlPrimitiveType GetPrimitiveType(Transform sceneTransform)
			{
				MeshFilter meshFilter = sceneTransform.GetComponent<MeshFilter>();

				if (!meshFilter || !meshFilter.sharedMesh)
				{
					return HtmlPrimitiveType.None;
				}

				Mesh cubePrimitiveMesh = HtmlConverterSettings.GetOrCreate().CubeMesh;

				if (meshFilter.sharedMesh == cubePrimitiveMesh)
				{
					return HtmlPrimitiveType.Cube;
				}

				return HtmlPrimitiveType.None;
			}
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
				throw new Exception($"Failed to read template files at path {htmlTemplatePath} and {jsTemplatePath}");
			}

			if (!htmlText.Contains("const SCENE_DATA = {}"))
			{
				throw new Exception(
					$"Invalid html template at path {htmlTemplatePath}. \nMissing 'const SCENE_DATA = {{}}' in body section.");
			}

			htmlText = htmlText.Replace("const SCENE_DATA = {}", $"const SCENE_DATA = {sceneDataJsonText}");

			File.WriteAllText(htmlOutputPath, htmlText);
			File.WriteAllText(jsOutputPath, jsText);
			EditorUtility.RevealInFinder(exportFolderPath);
		}

		#region Web rendering API dependent transformations

		public static Vector3 ToWebPosition(Vector3 unityPosition)
		{
			WebRenderingApi currentRenderingApi = HtmlConverterSettings.GetOrCreate().WebRenderingApi;

			if (currentRenderingApi == WebRenderingApi.ThreeJs)
			{
				return ToThreeJsPosition(unityPosition);
			}

			return unityPosition;

			static Vector3 ToThreeJsPosition(Vector3 unityPosition) => new Vector3(unityPosition.x, unityPosition.y, -unityPosition.z);
		}

		public static Vector3 ToWebRotation(Vector3 unityEulerRotation)
		{
			WebRenderingApi currentRenderingApi = HtmlConverterSettings.GetOrCreate().WebRenderingApi;

			if (currentRenderingApi == WebRenderingApi.ThreeJs)
			{
				return ToThreeJsRotation(unityEulerRotation);
			}

			return unityEulerRotation;

			static Vector3 ToThreeJsRotation(Vector3 unityEulerRotation) =>
				new Vector3(-unityEulerRotation.x, -unityEulerRotation.y, unityEulerRotation.z);
		}

		public static Quaternion ToWebRotation(Quaternion unityQuaternionRotation)
		{
			WebRenderingApi currentRenderingApi = HtmlConverterSettings.GetOrCreate().WebRenderingApi;

			if (currentRenderingApi == WebRenderingApi.ThreeJs)
			{
				return ToThreeJsRotation(unityQuaternionRotation);
			}

			return unityQuaternionRotation;

			static Quaternion ToThreeJsRotation(Quaternion unityQuaternionRotation) =>
				new Quaternion(unityQuaternionRotation.x, unityQuaternionRotation.y, -unityQuaternionRotation.z,
					-unityQuaternionRotation.w);
		}

		#endregion
	}
}
