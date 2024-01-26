using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

#if !UNITY_EDITOR

using Cysharp.Threading.Tasks;
using KZLib.KZDevelop;

#endif

namespace KZLib
{
	public class LogMgr : Singleton<LogMgr>
	{
		private const int MAX_LOG_COUNT = 100;

		private readonly CircularQueue<LogData> m_LogDataQueue = new(MAX_LOG_COUNT);

		public IReadOnlyCollection<LogData> LogDataCollection => m_LogDataQueue;

#if !UNITY_EDITOR
		private const int COOL_TIME_TIMER = 30*1000; // 30초

		private bool m_SendLock = false;
#endif
		private Action<LogData> m_OnAddLog = null;

		public event Action<LogData> OnAddLog
		{
			add { m_OnAddLog -= value; m_OnAddLog += value; }
			remove { m_OnAddLog -= value; }
		}

		protected override void Initialize()
		{
			Application.logMessageReceived += OnGetLog;
		}

		protected override void Release(bool _disposing)
		{
			if(m_Disposed)
			{
				return;
			}

			Application.logMessageReceived -= OnGetLog;

			if(_disposing)
			{
				m_LogDataQueue.Clear();
			}

			base.Release(_disposing);
		}

		private void OnGetLog(string _condition,string _stack,LogType _type)
		{
			if(_type == LogType.Exception)
			{
				GetLog(LogType.Exception,string.Format("[Exception] {0} [{1}]",_condition,_stack));
			}
		}

		private string SetPrefix(Log _kind)
		{
			return _kind == Log.Normal ? string.Empty : string.Format("[{0}] ",_kind);
		}

		public string ShowLog(Log _kind,LogType _type,object _message,params object[] _argumentArray)
		{
			return GetLog(_type,string.Concat(SetPrefix(_kind),ConvertText(_message)),_argumentArray);
		}

		private string GetLog(LogType _type,string _text,params object[] _argumentArray)
		{
			var result = _argumentArray.IsNullOrEmpty() ? _text : string.Format(_text,_argumentArray);

			AddLogData(_type,result);

#if !UNITY_EDITOR
			if(!m_SendLock && (_type == LogType.Exception))
			{
				SendBugReportAsync().Forget();
			}
#endif

			return result;
		}

		private string ConvertText(object _object)
		{
			if(_object == null)
			{
				return "NULL";
			}

			if(_object is string text)
			{
				return text;
			}
			
			if(_object is IEnumerable enumerable)
			{
				var textList = new List<string>();
				var count = 0;

				foreach(var item in enumerable)
				{
					textList.Add(item.ToString());
					count++;
				}

				if(count == 0)
				{
					return string.Format("Empty - [{0}]",_object.ToString());
				}

				return string.Format("{0} - [{1}]",count,string.Join(" & ",textList));
			}

			return _object.ToString();
		}

		private void AddLogData(LogType _type,string _log)
		{
			var data = new LogData(_type,_log);

			m_LogDataQueue.Enqueue(data);

			m_OnAddLog?.Invoke(data);
		}

#if !UNITY_EDITOR
		private async UniTaskVoid SendBugReportAsync()
		{
			m_SendLock = true;

			await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

			var texture = ObjectTools.GetScreenShot();

			await NetworkReport.SendBugReportAsync(texture.EncodeToPNG());

			await UniTask.WaitForSeconds(COOL_TIME_TIMER);

			m_SendLock = false;
		}
#endif

		public void ClearLogData()
		{
			m_LogDataQueue.Clear();
		}
	}
}