using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;

public class InGameMgr : SingletonMB<InGameMgr>
{
	[ShowInInspector]
	private bool IsRecording => m_MicrophoneRecord != null && m_MicrophoneRecord.IsRecording;

	[SerializeField]
	private Dropdown m_Dropdown = null;

	[SerializeField]
	private Text m_ResultText = null;

	private ConfigData m_ConfigData = null;

	[SerializeField]
	private MicrophoneRecord m_MicrophoneRecord = null;

	private CancellationTokenSource m_CancelTokenSource = null;

	[SerializeField]
	private GameObject m_OptionObject = null;

	public float PlayerSpeed => m_ConfigData == null ? 10.0f : m_ConfigData.PlayerSpeed;
	public float PlayerSensitivity => m_ConfigData == null ? 5.0f : m_ConfigData.PlayerSensitivity;
	public float PlayerSmoothing => m_ConfigData == null ? 2.0f : m_ConfigData.PlayerSmoothing;

	protected override void Initialize()
	{
		base.Initialize();

		var configPath = FileTools.PathCombine(FileTools.GetProjectPath(),"ConfigData.json");

		if(FileTools.IsExistFile(configPath))
		{
			var textFile = FileTools.ReadDataFromFile(configPath);

			m_ConfigData = JsonConvert.DeserializeObject<ConfigData>(textFile);
		}
		else
		{
			m_ConfigData = new();

			FileTools.WriteJsonFile(configPath,m_ConfigData);
		}

		if(!Microphone.devices.Contains(m_ConfigData.MicDevice))
		{
			m_ConfigData.MicDevice = Microphone.devices.First();

			FileTools.WriteJsonFile(configPath,m_ConfigData);
		}

		m_Dropdown.onValueChanged.AddListener((index)=>
		{
            var option = m_Dropdown.options[index];

			m_MicrophoneRecord.SelectedMicDevice = m_ConfigData.MicDevice = option.text;
			FileTools.WriteJsonFile(configPath,m_ConfigData);
		});

		m_MicrophoneRecord.OnRecordStop += OnRecordStop;

		m_MicrophoneRecord.SelectedMicDevice = m_ConfigData.MicDevice;
		m_MicrophoneRecord.maxLengthSec = m_ConfigData.RecordDuration;

		m_ResultText.text = null;

		m_OptionObject.SetActive(false);
	}

	protected override void Release()
	{
		base.Release();

		m_CancelTokenSource?.Cancel();
		m_CancelTokenSource?.Dispose();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(m_MicrophoneRecord.IsRecording)
			{
				return;
			}

			m_CancelTokenSource?.Dispose();
			m_CancelTokenSource = new();

			RecordAsync().Forget();
		}

		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Return))
		{
			m_OptionObject.SetActiveToggle();
		}
	}

	private async UniTaskVoid RecordAsync()
	{
		Log.InGame.I("Start");

		m_ResultText.text = null;
		m_MicrophoneRecord.StartRecord();

		await UniTask.WaitForSeconds(m_ConfigData.RecordDuration,cancellationToken : m_CancelTokenSource.Token);

		m_MicrophoneRecord.StopRecord();

		Log.InGame.I("End");
	}

	private async void OnRecordStop(AudioChunk _recorded)
	{
		var result = await VoiceMgr.In.GetTextAsync(_recorded.Data,_recorded.Frequency,_recorded.Channels);

		m_ResultText.text = result.Result;
	}
}