using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class ObjectTools
{
	private const long MAX_INDEX_COUNT = 65535L;

	public static void CombineMeshFilter(MeshFilter _combinedFilter,MeshFilter[] _filterArray)
	{
		if(_combinedFilter.sharedMesh == null)
		{
			_combinedFilter.sharedMesh = new Mesh { name = _combinedFilter.gameObject.name };
		}

		_combinedFilter.sharedMesh.Clear();

		var count = GetMeshIndexCount(_filterArray);

		if(!IsValidMeshIndexCount(count))
		{
			Log.System.W("배칭된 메쉬의 인덱스 갯수가 많습니다. 오브젝트 : {0} : 인덱스 갯수 : {1",_combinedFilter.gameObject.name,count);
		}

		var instanceList = new List<CombineInstance>();

		foreach(var filter in _filterArray)
		{
			if(filter == null || filter.sharedMesh == null)
			{
				continue;
			}

			var instance = new CombineInstance
			{
				mesh = filter.sharedMesh,
				transform = filter.transform.localToWorldMatrix,
			};

			filter.gameObject.SetActive(false);

			instanceList.Add(instance);
		}

		_combinedFilter.sharedMesh.CombineMeshes(instanceList.ToArray(),true);
	}

	public static bool AppendMesh(MeshFilter _filter,Mesh _mesh)
	{
		if(_mesh == null)
		{
			throw new ArgumentException("메쉬가 null 입니다.");
		}

		if(_filter.sharedMesh == null)
		{
			_filter.sharedMesh = _mesh;
			_filter.sharedMesh.name = "Combined Mesh";

			return true;
		}

		if(_filter.sharedMesh.Equals(_mesh))
		{
			return true;
		}

		var count = GetMeshIndexCount(_filter)+GetMeshIndexCount(_mesh);

		if(!IsValidMeshIndexCount(count))
		{
			return false;
		}

		var instanceArray = new CombineInstance[2];

		instanceArray[0] = new CombineInstance
		{
			mesh = _filter.sharedMesh,
			transform = Matrix4x4.identity,
			// transform = _filter.transform.localToWorldMatrix,
		};
		instanceArray[1] = new CombineInstance
		{
			mesh = _mesh,
			transform = Matrix4x4.identity,
		};

		var combined = new Mesh();
		combined.CombineMeshes(instanceArray,true);

		combined.RecalculateBounds();
		combined.RecalculateNormals();

		_filter.sharedMesh = combined;

		return true;
	}

	private static long GetMeshIndexCount(params MeshFilter[] _filterArray)
	{
		var count = 0L;

		foreach(var filter in _filterArray)
		{
			if(filter == null || filter.sharedMesh == null)
			{
				continue;
			}

			for(var i=0;i<filter.sharedMesh.subMeshCount;i++)
			{
				count += filter.sharedMesh.GetIndexCount(i);
			}
		}

		return count;
	}

	private static long GetMeshIndexCount(params Mesh[] _meshArray)
	{
		var count = 0L;

		foreach(var mesh in _meshArray)
		{
			if(mesh == null)
			{
				continue;
			}

			for(var i=0;i<mesh.subMeshCount;i++)
			{
				count += mesh.GetIndexCount(i);
			}
		}

		return count;
	}

	private static bool IsValidMeshIndexCount(long _count)
	{
		return _count < MAX_INDEX_COUNT;
	}
}