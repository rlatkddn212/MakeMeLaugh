#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace KZLib.KZWindow
{
	public class LocalDataWindow : OdinEditorWindow
	{
		#region LocalData
		[Serializable]
		private class LocalData
		{
			[SerializeField,HideInInspector]
			private string m_Key = null;

			[SerializeField,HideInInspector]
			private string m_Data = null;

			[TitleGroup("$m_Key"),HideLabel,ShowInInspector,MultiLineProperty(3)]
			public string Data
			{
				get => m_Data;
				private set
				{
					if(m_Data.IsEqual(value))
					{
						return;
					}

					var result = new Dictionary<string,object>();

					try
					{
						result.AddRange(JsonConvert.DeserializeObject<Dictionary<string,object>>(value));
					}
					catch(Exception)
					{
						result = null;
					}

					if(result == null)
					{
						return;
					}

					m_Data = value;

					var key = RealKey;
					var data = m_IsEncrypted ? SecurityTools.AESEncryptData(m_TableName,value) : value;

					SaveDataMgr.In.SetData(m_TableName,key,data);

					Log.Data.I(string.Format("{0}의 {1}의 값이 {2}로 변경 되었습니다.",m_TableName,key,data));
				}
			}

			public string RealKey => m_IsEncrypted ? SecurityTools.AESEncryptData(m_TableName,m_Key) : m_Key;

			private readonly bool m_IsEncrypted = false;
			
			private readonly string m_TableName = null;

			public LocalData(string _tableName,string _key,string _data,bool _isEncrypted)
			{
				m_TableName = _tableName;
				m_IsEncrypted = _isEncrypted;

				m_Key = _key;
				m_Data = _data;
			}
		}
		#endregion LocalData

		private string m_TableName = null;

		[HorizontalGroup("옵션",Order = 0),LabelText("테이블 이름"),ShowInInspector,ValueDropdown("m_TableNameList"),ShowIf("@this.m_TableNameList.Count > 0")]
		private string TableName
		{
			get => m_TableName;
			set
			{
				if(m_TableName == value)
				{
					return;
				}

				m_TableName = value;

				m_LocalDataList.Clear();

				if(m_TableName.IsEmpty())
				{
					return;
				}

				var dataDict = SaveDataMgr.In.GetDataInTable(value);

				if(dataDict.IsNullOrEmpty())
				{
					return;
				}

				var isEncrypted = true;

				var data = dataDict.First();

				try
				{
					var key = SecurityTools.AESDecryptData(value,data.Key);
				}
				catch(CryptographicException)
				{
					isEncrypted = false;
				}

				foreach(var pair in SaveDataMgr.In.GetDataInTable(TableName))
				{
					var key = isEncrypted ? SecurityTools.AESDecryptData(TableName,pair.Key) : pair.Key;
					var result = isEncrypted ? SecurityTools.AESDecryptData(TableName,pair.Value) : pair.Value;

					m_LocalDataList.Add(new LocalData(TableName,key,result,isEncrypted));
				}
			}
		}

		[BoxGroup("테이블",Order = 1,ShowLabel = false)]
		[VerticalGroup("테이블/2",Order = 2),Button("테이블 삭제",ButtonSizes.Medium),ShowIf("@this.m_LocalDataList.Count > 0")]
		private void OnDeleteTable()
		{
			SaveDataMgr.In.DeleteTable(TableName);

			Log.Data.I(string.Format("{0}이 삭제 되었습니다.",TableName));

			TableName = null;
		}

		[VerticalGroup("테이블/0",Order = 0),LabelText("테이블"),SerializeField,ListDrawerSettings(ShowFoldout = false,DraggableItems = false,HideAddButton = true,CustomRemoveIndexFunction = nameof(OnRemoveData)),ShowIf("@this.m_LocalDataList.Count > 0")]
		private List<LocalData> m_LocalDataList = new();

		[HideLabel,ShowInInspector,HideIf("@this.m_TableNameList.Count > 0"),DisplayAsString]
		private string InfoText => "테이블이 비어 있습니다.";

		private readonly List<string> m_TableNameList = new();

		protected override void Initialize()
		{
			base.Initialize();

			if(SaveDataMgr.HasInstance)
			{
				SaveDataMgr.In.Dispose();
			}

			var tableNameList = SaveDataMgr.In.GetAllTableName();

			if(tableNameList.IsNullOrEmpty())
			{
				return;
			}

			m_TableNameList.AddRange(tableNameList);
		}

		private void OnRemoveData(int _index)
		{
			var key = m_LocalDataList[_index].RealKey;

			SaveDataMgr.In.RemoveKey(TableName,key);

			m_LocalDataList.RemoveAt(_index);

			Log.Data.I(string.Format("{0}의 {1}가 삭제 되었습니다.",TableName,key));
		}
	}
}
#endif