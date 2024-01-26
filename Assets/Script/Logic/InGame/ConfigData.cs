
public record ConfigData
{
	public int RecordDuration { get; set; } = 3;
	public float PlayerSpeed { get; set; } = 10.0f;
	public float PlayerSensitivity { get; set; } = 5.0f;
	public float PlayerSmoothing { get; set; } = 2.0f;

	public string MicDevice { get; set; } = "Default microphone";
}