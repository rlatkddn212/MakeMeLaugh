#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using System;
using NPOI.SS.UserModel;
using System.Reflection;
using System.ComponentModel;
using System.Linq;

namespace KZLib.KZFiles
{
	public partial class ExcelFile
	{
		public List<string> GetTitleArray(string _sheetName)
		{
			var sheet = GetSheet(_sheetName);
			var titleList = new List<string>();
			var rowData = sheet.GetRow(0);

			if(rowData == null)
			{
				throw new NullReferenceException("첫번째 셀의 값이 비어있습니다.");
			}

			for(var i=0;i<rowData.LastCellNum;i++)
			{
				var cell = rowData.GetCell(i);

				if(cell == null || cell.CellType == CellType.Blank || cell.StringCellValue.IsEmpty())
				{
					continue;
				}

				titleList.Add(cell.StringCellValue);
			}

			foreach(var column in titleList)
			{
				if(KEY_WORD_ARRAY.Any(x=>x.IsEqual(column.ToLowerInvariant())))
				{
					throw new NullReferenceException(string.Format("{0}는 헤더로 사용할 수 없습니다.",column));
				}
			}

			return titleList;
		}

		public List<TData> Deserialize<TData>(string _sheetName,int _startRow = 1)
		{
			var sheet = GetSheet(_sheetName);
			var dataList = new List<TData>();
			var propertyArray = typeof(TData).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			var rowCount = sheet.GetRow(0).PhysicalNumberOfCells; // 헤드는 중간에 빈칸이 없으므로.
			var startRow = Mathf.Clamp(_startRow,0,sheet.PhysicalNumberOfRows);

			for(var i=startRow;i<=sheet.PhysicalNumberOfRows;i++)
			{
				var row = sheet.GetRow(i);

				if(row == null)
				{
					continue;
				}

				var data = Activator.CreateInstance<TData>();
				var flag = false;

				for(var j=0;j<=rowCount;j++)
				{
					var cell = row.GetCell(j);

					if(cell == null || cell.CellType == CellType.Blank)
					{
						//? 빈 셀 스킵
						continue;
					}

					var header = sheet.GetRow(0).GetCell(j).StringCellValue;
					var property = Array.Find(propertyArray,x=>x.Name.IsEqual(header));

					if(property == null)
					{
						continue;
					}

					if(property.CanWrite)
					{
						try
						{
							flag = true;
							property.SetValue(data,ConvertData(cell,property.PropertyType),null);
						}
						catch(Exception _ex)
						{
							Log.Data.F(string.Format("엑셀 파일에 문제가 있습니다. 시트:{0} 오류:{1} 위치: {2}",_sheetName,_ex.Message,string.Format("행[{0}]/열[{1}]",i+1,header)));
						}
					}
				}

				if(flag)
				{
					dataList.Add(data);
				}
			}

			return dataList;
		}

		public List<string> GetColumnList(string _sheetName,string _columnName,int _startRow = 1)
		{
			var sheet = GetSheet(_sheetName);
			var cellList = new List<string>();
			var index = GetTitleArray(_sheetName).FindIndex(x=>x.IsEqual(_columnName));

			if(index == -1)
			{
				throw new NullReferenceException(string.Format("{0}은 {1}시트에 없는 컬럼입니다.",_columnName,_sheetName));
			}

			var startRow = Mathf.Clamp(_startRow,0,sheet.PhysicalNumberOfRows);

			for(var i=startRow;i<=sheet.PhysicalNumberOfRows;i++)
			{
				var row = sheet.GetRow(i);

				if(row == null)
				{
					continue;
				}

				var cell = row.GetCell(index);

				cellList.Add(cell.StringCellValue);
			}

			return cellList;
		}

		/// <summary>
		/// key는 column의 인덱스 값
		/// </summary>
		public Dictionary<TKey,TValue> ConvertToDictionary<TKey,TValue>(string _sheetName,int _keyIndex,int _valueIndex,int _startRow = 1)
		{
			var sheet = GetSheet(_sheetName);
			var dataDict = new Dictionary<TKey,TValue>();
			var startRow = Mathf.Clamp(_startRow,0,sheet.PhysicalNumberOfRows);

			for(var i=startRow;i<=sheet.PhysicalNumberOfRows;i++)
			{
				var row = sheet.GetRow(i);

				if(row == null)
				{
					continue;
				}

				var keyCell = row.GetCell(_keyIndex);
				var valueCell = row.GetCell(_valueIndex);

				dataDict.Add((TKey) ConvertData(keyCell,typeof(TKey)),(TValue) ConvertData(valueCell,typeof(TValue)));
			}

			return dataDict;
		}

		/// <summary>
		/// key는 column의 인덱스 값
		/// </summary>
		public Dictionary<int,List<string>> ConvertToListDict(string _sheetName,int _startRow,int[] _columnArray)
		{
			var sheet = GetSheet(_sheetName);
			var lastRow = sheet.PhysicalNumberOfRows;
			var startRow = Mathf.Clamp(_startRow,0,lastRow);
			var rowSize = lastRow-startRow;

			//? 0번째 컬럼은 무조껀 추가한다 (Key값이므로)
			var columnList = new List<int>(_columnArray) { 0, };
			var resultListDict = new Dictionary<int,List<string>>(columnList.Count);

			for(var i=startRow;i<lastRow;i++)
			{
				var row = sheet.GetRow(i);

				if(row == null)
				{
					continue;
				}

				for(var j=0;j<columnList.Count;j++)
				{
					//? 해당 엑셀 값
					var cell = ParseCell(row.GetCell(columnList[j]));

					//? 엑셀의 키 값이 빈칸인 경우는 생략
					if(string.IsNullOrWhiteSpace(cell))
					{
						continue;
					}

					if(!resultListDict.ContainsKey(j))
					{
						resultListDict.Add(j,new List<string>(rowSize));
					}

					resultListDict[j].Add(cell);
				}
			}

			return resultListDict;
		}

		public string[,] ConvertToArray(string _sheetName,int _startRow,int[] _indexArray)
		{
			var sheet = GetSheet(_sheetName);
			var indexList = new List<int>(_indexArray);
			var startRow = Mathf.Clamp(_startRow,0,sheet.PhysicalNumberOfRows);

			indexList.AddNotOverlap(0);
			indexList = indexList.OrderBy(x=>x).ToList();
			var resultArray = new string[indexList.Count,sheet.PhysicalNumberOfRows-startRow];
			var resultList = new List<string>();

			for(var i=startRow;i<sheet.PhysicalNumberOfRows;i++)
			{
				var row = sheet.GetRow(i);

				if(row == null)
				{
					continue;
				}

				for(var j=0;j<indexList.Count;j++)
				{
					var data = ParseCell(row.GetCell(indexList[j]));

					if(indexList[j] == 0 && data.IsEqual("0"))
					{
						break;
					}

					resultArray[j,i-startRow] = data;
				}
			}

			return resultArray;
		}

		private object ConvertData(ICell _cell,Type _type)
		{
			object data = ConvertCell(_cell,_type);

			if(_type.IsGenericType && _type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				return new NullableConverter(_type).ConvertFrom(data);
			}
			else if(_type.IsEnum)
			{
				if(Enum.TryParse(_type,_cell.StringCellValue,out var value))
				{
					return value;
				}

				throw new InvalidCastException(string.Format("{0}을 캐스팅 하는데 문제가 있습니다. [{1}]",_type,_cell.StringCellValue));
			}
			else if(_type.Equals(typeof(Vector3)))
			{
				return ConvertVector3(data);
			}
			else if(_type.IsArray)
			{
				return ConvertArrayData(data,_type.GetElementType());
			}

			return Convert.ChangeType(data,_type);
		}

		private object ConvertCell(ICell _cell,Type _type)
		{
			object data = null;

			if(_type.Equals(typeof(float)) || _type.Equals(typeof(double)) || _type.Equals(typeof(short)) || _type.Equals(typeof(int)) || _type.Equals(typeof(long)))
			{
				if(_cell.CellType == CellType.Numeric)
				{
					data = _cell.NumericCellValue;
				}
				else if(_cell.CellType == CellType.String)
				{
					data = ConvertObject(_cell.StringCellValue,_type);
				}
				else if(_cell.CellType == CellType.Formula)
				{
					data = ConvertObject(_cell.NumericCellValue,_type);
				}
			}
			else if(_type.Equals(typeof(Vector3)))
			{
				data = _cell.StringCellValue;
			}
			else if(_type.Equals(typeof(string)) || _type.IsArray)
			{
				data = (_cell.CellType == CellType.Numeric) ? (object) _cell.NumericCellValue : (object) _cell.StringCellValue;
			}
			else if(_type.Equals(typeof(bool)))
			{
				data = _cell.BooleanCellValue;
			}

			return data;
		}

		private object ConvertObject(object _object,Type _type)
		{
			if(_object == null)
			{
				throw new FormatException(string.Format("바꿀 수 있는 포맷이 없습니다. [오브젝트 : {0}/ 타입 : {1}]",_object,_type));
			}

			if(_type.Equals(typeof(float)))
			{
				return Convert.ToSingle(_object);
			}

			if(_type.Equals(typeof(double)))
			{
				return Convert.ToDouble(_object);
			}

			if(_type.Equals(typeof(short)))
			{
				return Convert.ToInt16(_object);
			}

			if(_type.Equals(typeof(int)))
			{
				return Convert.ToInt32(_object);
			}

			if(_type.Equals(typeof(long)))
			{
				return Convert.ToInt64(_object);
			}

			throw new FormatException(string.Format("바꿀 수 있는 포맷이 없습니다. [오브젝트 : {0}/ 타입 : {1}]",_object,_type));
		}

		private object ConvertArrayData(object _object,Type _type)
		{
			if(_object == null)
			{
				throw new FormatException(string.Format("바꿀 수 있는 포맷이 없습니다. [오브젝트 : {0}/ 타입 : {1}]",_object,_type));
			}

			var data = _object.ToString().Replace(" ","").TrimEnd('&',' ');
			var dataArray = data.Split('&');

			if(_type.Equals(typeof(float)))
			{
				return dataArray.Select(x=>Convert.ToSingle(x)).ToArray();
			}

			if(_type.Equals(typeof(double)))
			{
				return dataArray.Select(x=>Convert.ToDouble(x)).ToArray();
			}

			if(_type.Equals(typeof(short)))
			{
				return dataArray.Select(x=>Convert.ToInt16(x)).ToArray();
			}

			if(_type.Equals(typeof(int)))
			{
				return dataArray.Select(x=>Convert.ToInt32(x)).ToArray();
			}

			if(_type.Equals(typeof(long)))
			{
				return dataArray.Select(x=>Convert.ToInt64(x)).ToArray();
			}

			if(_type.Equals(typeof(string)))
			{
				return dataArray;
			}

			throw new FormatException(string.Format("바꿀 수 있는 포맷이 없습니다. [오브젝트 : {0}/ 타입 : {1}]",_object,_type));
		}

		private object ConvertVector3(object _object)
		{
			return _object.ToString().ToVector3();
		}

		private string ParseCell(ICell _cell)
		{
			if(_cell == null || _cell.CellType == CellType.Blank)
			{
				return string.Empty;
			}

			return _cell.CellType == CellType.Numeric ? _cell.NumericCellValue.ToString() : _cell.StringCellValue;
		}
	}
}
#endif