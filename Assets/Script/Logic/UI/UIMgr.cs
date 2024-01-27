using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMgr : SingletonMB<UIMgr>
{
    [SerializeField]
    private GameObject m_InGameUI = null;
    [SerializeField]
    private Text m_InGameText = null;
    [SerializeField]
    private GameObject m_OptionUI = null;

    [SerializeField]
    private RawImage m_FadeScreenUI = null;

    private WebCamTexture m_WebcamTexture = null;

    protected override void Initialize()
    {
        m_FadeScreenUI.color = Color.clear;

        m_InGameUI.SetActiveSelf(true);
        m_OptionUI.SetActiveSelf(false);

        m_InGameText.text = null;
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

    public async UniTask PlayWebCamAsync()
    {
        m_WebcamTexture = new WebCamTexture();

        m_FadeScreenUI.color = Color.white;

        m_FadeScreenUI.texture = m_WebcamTexture;
        m_WebcamTexture.Play();

        await UniTask.WaitForSeconds(3.0f);

        await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,1.0f,(progress)=>
        {
            m_FadeScreenUI.color = Color.Lerp(Color.clear,Color.black,progress);
        });

        SceneManager.LoadScene("TitleScene");
    }
}