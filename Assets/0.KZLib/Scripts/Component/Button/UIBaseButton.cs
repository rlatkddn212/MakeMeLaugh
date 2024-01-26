using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class UIBaseButton : UIBaseComponent
{
	[SerializeField]
	protected Button m_Button = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_Button)
		{
			m_Button = GetComponent<Button>();
		}
	}
}