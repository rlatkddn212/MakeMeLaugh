using UnityEngine;

public static class FloatExtension
{
	public static float Sign(this float _single)
	{
		return _single < 0.0f ? -1.0f : _single > 0.0f ? 1.0f : 0.0f;
	}

	public static string ToStringComma(this float _single)
	{
		return string.Format("{0:n0}",_single);
	}

	public static string ToStringPercent(this float _single,int _dot)
	{
		return string.Format("{0:f1}%",_single.ToCeil(_dot));
	}

	public static string ToStringSign(this float _single)
	{
		return string.Format("{0}{1}",_single > 0.0 ? "+" : "",_single);
	}

	public static float ToCeil(this float _single,int _dot)
	{
		var pivot = Mathf.Pow(10.0f,_dot);

		return Mathf.Ceil(_single*pivot)/pivot;
	}

	public static float ToRound(this float _single,int _dot)
	{
		var pivot = Mathf.Pow(10.0f,_dot);

		return Mathf.Round(_single*pivot)/pivot;
	}

	public static float ToFloor(this float _single,int _dot)
	{
		var pivot = Mathf.Pow(10.0f,_dot);

		return Mathf.Floor(_single*pivot)/pivot;
	}

	public static float ToWrapAngle(this float _angle)
	{
		while(_angle > 180.0f)
		{
			_angle -= 360.0f;
		}

		while(_angle < -180.0f)
		{
			_angle += 360.0f;
		}

		return _angle;
	}
	
	public static Vector3 ToVector(this float _radius,float _degree)
	{
		return new Vector3(_radius*Mathf.Cos(_degree*Mathf.Deg2Rad),0,_radius*Mathf.Sin(_degree*Mathf.Deg2Rad));
	}
	
	public static float DegreeToRadian(this float _angle)
	{
		return Mathf.PI*_angle/180.0f;
	}

	public static float RadianToDegree(this float _angle)
	{
		return _angle*(180.0f/Mathf.PI);
	}

	public static bool Approximately(this float _single,float _number)
	{
		return Mathf.Approximately(_single,_number);
	}

	public static bool ApproximatelyZero(this float _single)
	{
		return Approximately(_single,0.0f);
	}

	public static void SeparateDecimal(this float _single,out int _integer,out float _decimal)
	{
		_integer = Mathf.FloorToInt(_single);
		_decimal = _single.Approximately(_integer) ? 0.0f : Mathf.Abs(_single-_integer);
	}
}