#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Text.RegularExpressions;

namespace KZLib.KZFiles
{
	[Serializable]
	public partial class ExcelFile
	{
		[SerializeField,HideInInspector]
		private readonly string m_LocalFilePath = null;

		private IWorkbook m_Workbook = null;

		private IWorkbook Workbook
		{
			get
			{
				if(m_Workbook == null)
				{
					GetWorkbook();
				}

				return m_Workbook;
			}
		}

		public ExcelFile(string _localFilePath)
		{
			m_LocalFilePath = _localFilePath;

			GetWorkbook();
		}

		private void GetWorkbook()
		{
			var filePath = FileTools.GetAbsoluteFullPath(m_LocalFilePath);

			FileTools.IsExistFile(filePath,true);

			using var stream = new FileStream(filePath,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
			var extension = FileTools.GetExtension(filePath);

			if(extension.IsEqual(".xls"))
			{
				m_Workbook = new HSSFWorkbook(stream);

				return;
			}
			else if(extension.IsEqual(".xlsx"))
			{
				m_Workbook = new XSSFWorkbook(stream);

				return;
			}

			throw new NullReferenceException("파일이 잘못 되었습니다.");
		}

		public List<string> SheetNameList
		{
			get
			{
				var nameList = new List<string>();

				for(var i=0;i<Workbook.NumberOfSheets;i++)
				{
					nameList.Add(Workbook.GetSheetName(i));
				}

				return nameList;
			}
		}

		public Dictionary<string,string[]> GetEnumDict(string _sheetName,List<int> _orderList)
		{
			var sheet = GetSheet(_sheetName);
			var explicitDict = new Dictionary<string,string[]>();

			foreach(var data in sheet.GetDataValidations())
			{
				// 데이터 유효성의 목록 가져오기
				var explicitArray = data.ValidationConstraint.ExplicitListValues;

				if(explicitArray.IsNullOrEmpty())
				{
					continue;
				}

				for(var i=0;i<explicitArray.Length;i++)
				{
					explicitArray[i] = Regex.Replace(explicitArray[i],@"[^0-9a-zA-Z_]+",string.Empty);
				}

				var index = GetHeader(sheet,_orderList,explicitArray);

				if(!_orderList.ContainsIndex(index))
				{
					continue;
				}

				var header = sheet.GetRow(0).GetCell(_orderList[index]).StringCellValue;

				if(explicitDict.ContainsKey(header))
				{
					continue;
				}

				explicitDict.Add(header,explicitArray);

				_orderList.RemoveAt(index);
			}

			return explicitDict;
		}

		private int GetHeader(ISheet _sheet,List<int> _orderList,string[] _dataArray)
		{
			for(var i=1;i<=_sheet.LastRowNum;i++)
			{
				for(var j=0;j<_orderList.Count;j++)
				{
					var row = _sheet.GetRow(i);

					if(row.LastCellNum < _orderList[j])
					{
						Log.Data.E("{0}번째 줄의 {1}의 값이 존재하지 않습니다.",i,_orderList[j]);

						continue;
					}

					var cell = _sheet.GetRow(i).GetCell(_orderList[j]);

					if(cell == null || cell.StringCellValue.IsEmpty())
					{
						continue;
					}

					if(_dataArray.Any(x=>x.Contains(cell.StringCellValue)))
					{
						return j;
					}
				}
			}

			return -1;
		}

		private ISheet GetSheet(string _sheetName)
		{
			var sheet = Workbook.GetSheet(_sheetName);

			if(sheet == null)
			{
				throw new NullReferenceException("시트가 없습니다.");
			}

			return sheet;
		}

		private static string[] KEY_WORD_ARRAY => new string[]
		{
			"abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", 
			"class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum",
			"event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto",
			"if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
			"namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected",
			"public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", 
			"string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
			"ushort", "using", "virtual", "void", "volatile", "while",
		};
	}
}
#endif