using System.Collections.Generic;
using KZLib;

namespace GameData
{
	/// <summary>
	/// 디폴트 옵션은 게임세팅에서 가져온다.
	/// 여기 저장되는건 유저 옵션들
	/// </summary>
	public partial class Option : IGameData
	{
		private class SaveData : SaveDataHandler
		{
			protected override string TABLE_NAME => "Option_Table";
		}

		private SaveData m_SaveData = null;

		public void Initialize()
		{
			m_SaveData = new SaveData();

			InitializeSound();

			InitializePartial();
		}

		public void Release()
		{
			ReleaseSound();
		}

		partial void InitializePartial();
	}
}