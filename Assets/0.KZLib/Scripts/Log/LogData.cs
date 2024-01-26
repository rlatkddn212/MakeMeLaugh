using System;
using UnityEngine;

namespace KZLib
{
	public record LogData : MessageData
	{
		private readonly LogType m_Type = LogType.Log;

		public string Time { get; }

		public LogData(LogType _type,string _text) : base(string.Format("<{0}> [{1}]",_type,DateTime.Now.ToString("MM/dd HH:mm:ss:ff")),_text)
		{
			m_Type = _type;
			Time = string.Format("[{0}]",DateTime.Now.ToString("MM/dd HH:mm:ss:ff"));
		}
		
		public bool IsInfo => m_Type == LogType.Log || m_Type == LogType.Assert;
		public bool IsWarning => m_Type == LogType.Warning;
		public bool IsError => m_Type == LogType.Error || m_Type == LogType.Exception;

		public void CheckCount(ref int _info,ref int _warning,ref int _error)
		{
			if(IsInfo)
			{
				_info++;
			}
			else if(IsWarning)
			{
				_warning++;
			}
			else if(IsError)
			{
				_error++;
			}
		}
	}
}