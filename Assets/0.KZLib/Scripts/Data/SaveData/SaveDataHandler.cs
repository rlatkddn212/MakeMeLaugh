using System;
using Newtonsoft.Json;

namespace KZLib
{
	public abstract class SaveDataHandler
	{
		protected abstract string TABLE_NAME { get; }
		protected virtual bool EncryptKey => false;
		protected virtual bool EncryptData => false;

		public SaveDataHandler()
		{
			SaveDataMgr.In.LoadSQLTable(TABLE_NAME);
		}

		public bool HasKey(string _key)
		{
			return SaveDataMgr.In.HasKey(TABLE_NAME,GetKey(_key));
		}

		public void SetString(string _key,string _data)
		{
			SetDataInner(_key,_data);
		}

		public void SetInt(string _key,int _data)
		{
			SetDataInner(_key,_data.ToString());
		}

		public void SetFloat(string _key,float _data)
		{
			SetDataInner(_key,_data.ToString());
		}

		public void SetBool(string _key,bool _data)
		{
			SetDataInner(_key,_data.ToString());
		}

		public void SetEnum<TEnum>(string _key,TEnum _data) where TEnum : struct
		{
			SetDataInner(_key,_data.ToString());
		}

		public void SetData<TData>(string _key,TData _data)
		{
			SetDataInner(_key,JsonConvert.SerializeObject(_data));
		}

		private void SetDataInner(string _key,string _data)
		{
			var data = EncryptData ? SecurityTools.AESEncryptData(TABLE_NAME,_data) : _data;

			SaveDataMgr.In.SetData(TABLE_NAME,GetKey(_key),data);
		}

		public string GetString(string _key,string _default = null)
		{
			return GetDataInner(_key,_default);
		}

		public int GetInt(string _key,int _default = 0)
		{
			return int.TryParse(GetDataInner(_key,_default.ToString()),out var result) ? result : _default;
		}

		public float GetFloat(string _key,float _default = 0.0f)
		{
			return float.TryParse(GetDataInner(_key,_default.ToString()),out var result) ? result : _default;
		}

		public bool GetBool(string _key,bool _default = true)
		{
			return bool.TryParse(GetDataInner(_key,_default.ToString()),out var result) ? result : _default;
		}

		public TEnum GetEnum<TEnum>(string _key,TEnum _default = default) where TEnum : struct
		{
			return Enum.TryParse(GetDataInner(_key,_default.ToString()),true,out TEnum result) ? result : _default;
		}

		public TData GetData<TData>(string _key,TData _default = default)
		{
			return JsonConvert.DeserializeObject<TData>(GetDataInner(_key,JsonConvert.SerializeObject(_default)));
		}

		private string GetDataInner(string _key,string _default)
		{
			var result = SaveDataMgr.In.GetData(TABLE_NAME,GetKey(_key),_default);

			if(result.IsEqual(_default) || !EncryptData)
			{
				return result;
			}
			else
			{
				return SecurityTools.AESDecryptData(TABLE_NAME,result);
			}
		}

		public void RemoveKey(string _key)
		{
			SaveDataMgr.In.RemoveKey(TABLE_NAME,GetKey(_key));
		}

		private string GetKey(string _key)
		{
			return EncryptKey ? SecurityTools.AESEncryptData(TABLE_NAME,_key) : _key;
		}
	}
}