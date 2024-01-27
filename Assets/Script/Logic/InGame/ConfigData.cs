
public record ConfigData
{
	public int RecordDuration { get; set; } = 3;
	public float PlayerSpeed { get; set; } = 10.0f;
	public float PlayerSensitivity { get; set; } = 5.0f;
	public float EnemySoundInterval { get; set; } = 3.0f;

	public int EndingCount { get; set; } = 1;

	public string MicDevice { get; set; } = "Default microphone";
}