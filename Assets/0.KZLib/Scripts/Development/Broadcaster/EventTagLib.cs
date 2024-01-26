namespace KZLib.KZDevelop
{
	public partial class EventTag : Enumeration
	{
		// Language
		public static readonly EventTag ChangeLanguage = new(nameof(ChangeLanguage));

		// Graphic Option
		public static readonly EventTag ChangeGraphic = new(nameof(ChangeGraphic));
		// Sound Option
		public static readonly EventTag ChangeSound = new(nameof(ChangeSound));

		// Vibration Option
		public static readonly EventTag ChangeVibration = new(nameof(ChangeVibration));

		// Touch
		public static readonly EventTag TouchPoint = new(nameof(TouchPoint));

		public EventTag(string _name) : base(string.Format("[Event] {0}",_name)) { }
	}
}