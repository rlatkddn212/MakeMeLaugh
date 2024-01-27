using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitLocation : MonoBehaviour
{
    [SerializeField]
    private AudioSource loopAudio;
    [SerializeField]
    private AudioSource _audio;

    private bool m_enter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_enter)
        {
            m_enter = true;

            Debug.Log("Exit");

            loopAudio.Stop();

            _audio.Play();

            UIMgr.In.PlayWebCamAsync().Forget();
        }
    }
}
