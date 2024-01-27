using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KZLib;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	// 적 얼굴 랜더러
	[SerializeField]
    private Renderer _renderer;

    [SerializeField]
	private AudioSource m_AudioSource = null;
	private float m_SoundInterval = 3.0f;

	private Rigidbody rb;

    // 이펙트 시작
	private CancellationTokenSource m_CancelTokenSource = null;

	private bool m_Meet = false;

	private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        FaceMgr.In.OnFaceDetected += OnFaceRender;
        m_SoundInterval = InGameMgr.In.EnemySoundInterval;
    }

	public void Initialize(Vector3 _position)
	{
		rb.velocity = Vector3.zero;
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
			direction = new Vector3(direction.x,0.0f,direction.z);
			transform.DORotateQuaternion(Quaternion.LookRotation(direction),1.0f).OnComplete(()=>
			{
				InGameMgr.In.SetInput();
			});
		}
	}

	public async UniTask DestroyEffectAsync()
	{
        // 특정 방향으로 힘을 가합니다.
        Vector3 forceDirection = new Vector3(0f, 1f, 0f); // y축 방향으로 힘을 가하는 예제
        rb.AddForce(forceDirection * 10f, ForceMode.Impulse);

		EffectObject obj = EffectPoolMgr.In.GetPooledObject(0, 5.0f);
        obj.transform.position = transform.position;
        await UniTask.WaitForSeconds(1.0f);
	}

	public void OnFaceRender(Texture2D _texture)
	{
        _renderer.material.mainTexture = _texture;
    }
}