using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KZLib;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UILoopImage : UIBaseImage
{
	[SerializeField,LabelText("이미지 경로들")]
	private List<string> m_ImagePathList = new();

	[SerializeField,LabelText("무작위 모드")]
	private bool m_RandomMode = false;

	[SerializeField,LabelText("보여지는 시간"),MinValue(0.01f)]
	private float m_ShowDuration = 0.01f;

	private readonly List<Sprite> m_SpriteList = new();

	private CancellationTokenSource m_Source = null;

	protected override void Awake()
	{
		base.Awake();

		for(var i=0;i<m_ImagePathList.Count;i++)
		{
			var path = m_ImagePathList[i];

			if(path.IsEmpty())
			{
				continue;
			}

			var sprite = ResMgr.In.GetSprite(path);

			if(sprite)
			{
				m_SpriteList.Add(sprite);
			}
		}
	}

	private void OnEnable()
	{
		m_Source?.Dispose();
		m_Source = new();

		PlayLoopImage();
	}

	private void OnDisable()
	{
		m_Source?.Cancel();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		m_Source?.Cancel();
		m_Source?.Dispose();
	}

	public void PlayLoopImage()
	{
		PlayLoopImageAsync().Forget();
	}

	private async UniTask PlayLoopImageAsync()
	{
		var spriteList = new List<Sprite>(m_SpriteList);

		if(m_RandomMode)
		{
			spriteList.Randomize();
		}

		while(true)
		{
			for(var i=0;i<spriteList.Count;i++)
			{
				m_Image.gameObject.SetActiveSelf(true);

				m_Image.SetSafeImage(spriteList[i]);

				await UniTask.WaitForSeconds(m_ShowDuration,false,cancellationToken : m_Source.Token);

				if(m_Source.IsCancellationRequested)
				{
					return;
				}

				m_Image.gameObject.SetActiveSelf(false);
			}
		}
	}
}