using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : SingletonMB<UIMgr>
{
    [SerializeField]
    private GameObject m_TitleUI = null;
    [SerializeField]
    private GameObject m_InGameUI = null;
    [SerializeField]
    private Text m_InGameText = null;
    [SerializeField]
    private GameObject m_OptionUI = null;

    [SerializeField]
    private Image m_FadeScreenUI = null;

    protected override void Initialize()
    {
        m_FadeScreenUI.color = Color.clear;

        m_TitleUI.SetActiveSelf(true);
        m_InGameUI.SetActiveSelf(false);
        m_OptionUI.SetActiveSelf(false);

        m_InGameText.text = null;
    }

    public void GameStart()
    {
        m_TitleUI.SetActiveSelf(false);
        m_InGameUI.SetActiveSelf(true);
    }

    public void SetGameText(string _text)
    {
        m_InGameText.text = _text;
    }

    public void ToggleOptionPanel()
    {
        m_OptionUI.SetActiveToggle();
    }

    public async UniTask FadeOutAsync()
    {
        m_FadeScreenUI.color = Color.clear;

        await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,1.0f,(progress)=>
        {
            m_FadeScreenUI.color = Color.Lerp(Color.clear,Color.black,progress);
        });
    }
}