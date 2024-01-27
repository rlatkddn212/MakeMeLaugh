using System;
using System.IO;
using UnityEditor;

public static partial class FileTools
{
	/// <summary>
	/// 실제 경로를 생성함 (폴더면 폴더 생성 / 파일이면 부모 폴더 생성)
	/// </summary>
	public static void CreateFolder(string _path)
	{
		if(_path.IsEmpty())
		{
			throw new NullReferenceException("폴더의 경로가 null 입니다.");
		}

		var fullPath = GetFullPath(IsFilePath(_path) ? Path.GetDirectoryName(_path) : _path);

		if(!Directory.Exists(fullPath))
		{
			Directory.CreateDirectory(fullPath);
		}
	}
}