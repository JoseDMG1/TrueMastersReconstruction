public class WAV
{
	public float[] LeftChannel { get; internal set; }

	public float[] RightChannel { get; internal set; }

	public float[] StereoChannel { get; internal set; }

	public int ChannelCount { get; internal set; }

	public int SampleCount { get; internal set; }

	public int Frequency { get; internal set; }

	private static float bytesToFloat(byte firstByte, byte secondByte)
	{
		return 0f;
	}

	private static int bytesToInt(byte[] bytes, int offset = 0)
	{
		return 0;
	}

	public WAV(byte[] wav)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
