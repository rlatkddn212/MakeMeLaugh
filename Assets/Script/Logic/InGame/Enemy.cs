using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private AudioSource m_AudioSource = null;
	private float m_SoundInterval = 3.0f;

	private CancellationTokenSource m_CancelTokenSource = null;

	private bool m_Meet = false;

	private void Awake()
	{
		m_SoundInterval = InGameMgr.In.EnemySoundInterval;
	}

	public void Initialize(Vector3 _position)
	{
		transform.DOKill();
		transform.position = _position;

		m_Meet = false;

		m_CancelTokenSource?.Dispose();
		m_CancelTokenSource = new();

		PlayLoopSound().Forget();
	}

	private async UniTaskVoid PlayLoopSound()
	{
		while(!m_CancelTokenSource.IsCancellationRequested)
		{
			m_AudioSource.Play();

			await UniTask.WaitForSeconds(m_SoundInterval);
		}

		m_AudioSource.Stop();
	}

	private void OnTriggerEnter(Collider _collider)
	{
		if(_collider.gameObject.CompareTag("Player") && !m_Meet)
		{
			Log.InGame.I("플레이어 만남");

			m_Meet = true;

			m_CancelTokenSource?.Cancel();

			var direction = _collider.transform.position-transform.position;

			transform.DORotateQuaternion(Quaternion.LookRotation(direction),1.0f).OnComplete(()=>
			{
				InGameMgr.In.SetInput();
			});
		}
	}

	public async UniTask DestroyEffectAsync()
	{
		// 이펙트 시작
		await UniTask.WaitForSeconds(1.0f);
	}
}