using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class UIBaseEvent : UIBaseComponent
{
	[SerializeField] private Image m_Image = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_Image)
		{
			m_Image = GetComponent<Image>();
		}
	}
}