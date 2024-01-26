
namespace KZLib
{
	public partial class ResMgr : Singleton<ResMgr>
	{
		private const string RESOURCES = "Resources";
		private const float UPDATE_PERIOD = 0.1f;

		protected override void Initialize()
		{
			base.Initialize();

			LoadingAsync().Forget();
		}

		protected override void Release(bool _disposing)
		{
			if(m_Disposed)
			{
				return;
			}

			if(_disposing)
			{
				foreach(var data in m_CachedDataDict.Values)
				{
					data.Release();
				}

				m_CachedDataDict.Clear();
				m_LoadingSet.Clear();
			}

			base.Release(_disposing);
		}
	}
}