using System;
using System.Collections.Generic;
using UnityEngine;

namespace HTMLConverter
{
	[Serializable]
	public struct SceneData
	{
		public CameraData Camera;
		public List<CubeData> Cubes;
	}

	[Serializable]
	public struct CameraData
	{
		public Vector3 Position;
		public Quaternion Rotation;
	}

	[Serializable]
	public struct CubeData
	{
		public int ParentIndex;
		public Vector3 Position;
		public Vector3 Rotation;
		public Vector3 Scale;
	}
}
