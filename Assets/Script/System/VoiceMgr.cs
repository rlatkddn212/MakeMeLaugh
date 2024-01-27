using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whisper;
using Whisper.Native;
using Whisper.Utils;

public class VoiceMgr : SingletonMB<VoiceMgr>
{
	private WhisperWrapper m_WhisperWrapper = null;
	private WhisperParams m_WhisperParams = null;

	[SerializeField]
	private WhisperSamplingStrategy strategy = WhisperSamplingStrategy.WHISPER_SAMPLING_GREEDY;

	private readonly MainThreadDispatcher m_Dispatcher = new();

	public event OnNewSegmentDelegate OnNewSegment;

	public event OnProgressDelegate OnProgress;

	protected override void Initialize()
	{
		base.Initialize();

		InitModelAsync().Forget();
	}

	private async UniTask InitModelAsync()
	{
        var modelPath = FileTools.PathCombine(Application.dataPath,"Whisper/ggml-tiny.bin");
		Log.System.I(modelPath);

		m_WhisperWrapper = await WhisperWrapper.InitFromFileAsync(modelPath);

		m_WhisperParams = WhisperParams.GetDefaultParams(strategy);
		UpdateParams();

		m_WhisperWrapper.OnNewSegment += OnNewSegmentHandler;
		m_WhisperWrapper.OnProgress += OnProgressHandler;
	}

	private void UpdateParams()
	{
		m_WhisperParams.Language = "en";
		m_WhisperParams.Translate = false;
		m_WhisperParams.NoContext = true;
		m_WhisperParams.SingleSegment = true;
		m_WhisperParams.SpeedUp = false;
		m_WhisperParams.AudioCtx = 0;
		m_WhisperParams.EnableTokens = false;
		m_WhisperParams.TokenTimestamps = false;
		m_WhisperParams.InitialPrompt = "";
	}

	private void OnNewSegmentHandler(WhisperSegment segment)
	{
		m_Dispatcher.Execute(() =>
		{
			OnNewSegment?.Invoke(segment);
		});
	}
	
	private void OnProgressHandler(int progress)
	{
		m_Dispatcher.Execute(() =>
		{
			OnProgress?.Invoke(progress);
		});
	}

	private void Update()
	{
		m_Dispatcher.Update();
	}

	public async UniTask<WhisperResult> GetTextAsync(float[] _samples,int _frequency,int _channels)
	{
		return await m_WhisperWrapper.GetTextAsync(_samples,_frequency,_channels,m_WhisperParams);
	}
}