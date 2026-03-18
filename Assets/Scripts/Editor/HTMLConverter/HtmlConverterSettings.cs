using UnityEditor;
using UnityEngine;

namespace HTMLConverter
{
	[CreateAssetMenu(fileName = "HTML Converter Settings", menuName = "HTML Converter/Settings", order = 0)]
	public class HtmlConverterSettings : ScriptableObject
	{
		private const string SETTINGS_PATH = "Assets/Editor/Settings/HTML Converter Settings.asset";

		public WebRenderingApi WebRenderingApi = WebRenderingApi.ThreeJs;
		public Mesh CubeMesh;

		internal static HtmlConverterSettings GetOrCreate()
		{
			HtmlConverterSettings settings = AssetDatabase.LoadAssetAtPath<HtmlConverterSettings>(SETTINGS_PATH);

			if (settings == null)
			{
				settings = CreateInstance<HtmlConverterSettings>();
				AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
				AssetDatabase.SaveAssets();
			}

			return settings;
		}

		internal static SerializedObject GetSO() => new SerializedObject(GetOrCreate());
	}

	public static class HtmlConverterSettingsProvider
	{
		[SettingsProvider]
		public static SettingsProvider CreateProvider()
		{
			return new SettingsProvider("Project/HTML Converter Settings", SettingsScope.Project)
			{
				guiHandler = _ =>
				{
					SerializedObject serializedObject = HtmlConverterSettings.GetSO();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("WebRenderingApi"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("CubeMesh"));
					serializedObject.ApplyModifiedProperties();
				},
				keywords = new System.Collections.Generic.HashSet<string>(new[] { "HTML", "Converter" })
			};
		}
	}

	public enum WebRenderingApi
	{
		ThreeJs
	}
}
