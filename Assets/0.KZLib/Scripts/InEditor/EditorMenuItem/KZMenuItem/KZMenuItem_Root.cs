#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace KZLib.KZMenu
{
	public partial class KZMenuItem
	{
		private const int MENU_LINE = 20;
		private const int PIVOT_BIT = 10;

		private enum Category
		{
			//? Option
			Option				= 0 << PIVOT_BIT,

			Option_Delete		= Option+0*MENU_LINE,
			Option_Assets		= Option+1*MENU_LINE,
			Option_Add			= Option+2*MENU_LINE,
			Option_Check		= Option+3*MENU_LINE,

			//? Build
			Build				= 1 << PIVOT_BIT,

			Build_Auto			= Build+0*MENU_LINE,
			Build_Quick			= Build+1*MENU_LINE,

			//? Window
			Window				= 2 << PIVOT_BIT,

			Window_Settings		= Window+0,
			Window_LocalData	= Window+1,
			Window_Manual		= Window+2,
			Window_Utility		= Window+3,

			//? Scene
			Scene				= 3 << PIVOT_BIT,

			Scene_Core			= Scene+0,
			Scene_Sample		= Scene+1,
			Scene_Tool			= Scene+2,
			Scene_Test			= Scene+3,

			//? Custom
			Custom				= 4 << PIVOT_BIT,
		}
	}
}
#endif
