using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Whisper.Utils;

public class InGameMgr : SingletonMB<InGameMgr>
{
	[SerializeField]
	private int m_Count = 0;

	[SerializeField]
	private ExitLocation m_ExitLocation;

	[ShowInInspector]
	private bool IsRecording => m_MicrophoneRecord != null && m_MicrophoneRecord.IsRecording;

	[SerializeField]
	private Player m_Player = null;

	[SerializeField]
	private Dropdown m_Dropdown = null;

	private ConfigData m_ConfigData = null;

	[SerializeField]
	private MicrophoneRecord m_MicrophoneRecord = null;

	private CancellationTokenSource m_CancelTokenSource = null;

	public readonly List<AudioClip> m_AudioClipList = new();

	public float PlayerSpeed => m_ConfigData == null ? 10.0f : m_ConfigData.PlayerSpeed;
	public float PlayerSensitivity => m_ConfigData == null ? 1.0f : m_ConfigData.PlayerSensitivity;

	public float EnemySoundInterval => m_ConfigData == null ? 3.0f : m_ConfigData.EnemySoundInterval;

	protected override void Initialize()
	{
		base.Initialize();

		m_AudioClipList.Clear();

		// 출구 제거
        m_ExitLocation.gameObject.SetActive(false);
		EnemyMgr.In.SpawnEnemy();
		m_Count = 0;

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
		m_MicrophoneRecord.OnGetAudioClip += OnGetAudioClip;

		m_MicrophoneRecord.SelectedMicDevice = m_ConfigData.MicDevice;
		m_MicrophoneRecord.maxLengthSec = m_ConfigData.RecordDuration;
	}

	protected override void Release()
	{
		base.Release();

		m_CancelTokenSource?.Cancel();
		m_CancelTokenSource?.Dispose();
	}

	private void Update()
	{
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Y) && Input.GetKeyDown(KeyCode.M))
		{
			UIMgr.In.ToggleOptionPanel();
		}
	}

	public void SetInput()
	{
		if(m_MicrophoneRecord.IsRecording)
		{
			return;
		}

		UIMgr.In.SetGameText("날 좀 웃겨봐");

		m_CancelTokenSource?.Dispose();
		m_CancelTokenSource = new();

		RecordAsync().Forget();
	}

	private async UniTaskVoid RecordAsync()
	{
		Log.InGame.I("Start");

		m_MicrophoneRecord.StartRecord();

		await UniTask.WaitForSeconds(m_ConfigData.RecordDuration,cancellationToken : m_CancelTokenSource.Token);

		m_MicrophoneRecord.StopRecord();

		Log.InGame.I("End");
	}

	private void OnGetAudioClip(AudioClip _clip)
	{
		// var folderPath = FileTools.PathCombine(FileTools.GetProjectPath(),"Sound");

		// FileTools.CreateFolder(folderPath);

		// var filepathArray = Directory.GetFiles(folderPath);
		// var musicPath = FileTools.PathCombine(FileTools.GetProjectPath(),string.Format("Sound/laugh_{0}.wav",filepathArray.Length));

		m_AudioClipList.Add(ObjectTools.CopyObject(_clip));

		// musicPath;

		// var data

		// FileTools.WriteAudioClipToWAV(_clip);
	}

	private async void OnRecordStop(AudioChunk _recorded)
	{
		var clip = m_MicrophoneRecord.ClipSamples;

		var result = await VoiceMgr.In.GetTextAsync(_recorded.Data,_recorded.Frequency,_recorded.Channels);
		var text = result.Result.ToLower();

		// 결과 판단하기
		if(text.Contains("laugh") || text.Contains("lol"))
		{
			UIMgr.In.SetGameText("");

			// 적군 죽음
			await EnemyMgr.In.KillEnemyAsync();

			m_Count++;

			if(m_Count == m_ConfigData.EndingCount)
			{
				// 탈출 포탈 생성
				Log.InGame.I("게임 승리");
                m_ExitLocation.gameObject.SetActive(true);
            }
			else
			{
				await UniTask.WaitForSeconds(2.0f);

				EnemyMgr.In.SpawnEnemy();
			}
		}
		else
		{
			UIMgr.In.SetGameText(result.Result);
			// 내가 죽음

			await m_Player.DiePlayerAsync();

			await UIMgr.In.FadeOutAsync();

			await EndGameAsync();

        }
	}

	public void PlayAllSound()
	{
		foreach(var clip in new List<AudioClip>(m_AudioClipList))
		{
			PlayAudioAndDestroy.Play(clip,transform.position);
		}
	}

	public async UniTask EndGameAsync()
	{
        await UniTask.WaitForSeconds(2.0f);
		SceneManager.LoadScene("TitleScene");
	}
}