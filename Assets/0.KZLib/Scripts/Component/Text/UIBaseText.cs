using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

[RequireComponent(typeof(TMP_Text))]
public abstract class UIBaseText : UIBaseComponent
{
	[SerializeField,LabelText("텍스트 메쉬")]
	protected TMP_Text m_Text = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_Text)
		{
			m_Text = GetComponent<TMP_Text>();
		}
	}
}