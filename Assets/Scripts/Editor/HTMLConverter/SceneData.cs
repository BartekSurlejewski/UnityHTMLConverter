using System;
using System.Collections.Generic;
using UnityEngine;

namespace HTMLConverter
{
	[Serializable]
	public struct SceneData
	{
		public CameraData Camera;
		public List<SceneObjectData> SceneObjects;
	}

	[Serializable]
	public struct CameraData
	{
		public Vector3 Position;
		public Quaternion Rotation;
	}

	[Serializable]
	public struct SceneObjectData
	{
		public string Name;
		public HtmlPrimitiveType PrimitiveType;
		public int ParentIndex;
		public Vector3 Position;
		public Vector3 Rotation;
		public Vector3 Scale;
	}

	public enum HtmlPrimitiveType
	{
		None = 0,	// Will be assumed to ba an empty parent
		Cube = 1
	}
}
