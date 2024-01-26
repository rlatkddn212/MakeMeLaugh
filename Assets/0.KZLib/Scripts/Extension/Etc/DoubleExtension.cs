using System;

public static class DoubleExtension
{
	public static double Sign(this double _double)
	{
		return _double < 0.0d ? -1.0d : _double > 0.0d ? 1.0d : 0.0d;
	}

	public static string ToStringComma(this double _double)
	{
		return string.Format("{0:n0}",_double);
	}

	public static string ToStringPercent(this double _double,int _dot)
	{
		return string.Format("{0:f1}%",_double.ToCeil(_dot));
	}
	
	public static string ToStringSign(this double _double)
	{
		return string.Format("{0}{1}",_double > 0.0d ? "+" : "",_double);
	}
	
	public static double ToCeil(this double _double,int _dot)
	{
		var pivot = Math.Pow(10.0d,_dot);

		return Math.Ceiling(_double*pivot)/pivot;
	}

	public static double ToRound(this double _double,int _dot)
	{
		var pivot = Math.Pow(10.0d,_dot);

		return Math.Round(_double*pivot)/pivot;
	}

	public static double ToFloor(this double _double,int _dot)
	{
		var pivot = Math.Pow(10.0d,_dot);

		return Math.Floor(_double*pivot)/pivot;
	}

	public static double ToWrapAngle(this double _angle)
	{
		while(_angle > 180.0d)
		{
			_angle -= 360.0d;
		}

		while(_angle < -180.0d)
		{
			_angle += 360.0d;
		}

		return _angle;
	}

	public static long ToMilliseconds(this double _seconds)
	{
		return (long) _seconds*1000L;
	}

	public static double DegreeToRadian(this double _angle)
	{
		return Math.PI*_angle/180.0d;
	}

	public static double RadianToDegree(this double _angle)
	{
		return _angle*(180.0d/Math.PI);
	}

	public static bool Approximately(this double _double,double _number,double _delta = 1e-15d)
	{
		return Math.Abs(_double-_number) <= _delta;
	}

	public static bool ApproximatelyZero(this double _double,double _delta = 1e-15d)
	{
		return Approximately(_double,0.0d,_delta);
	}

	public static void SeparateDecimal(this double _double,out int _integer,out double _decimal)
	{
		_integer = (int) Math.Floor(_double);
		_decimal = _double.Approximately(_integer) ? 0.0d : Math.Abs(_double-_integer);
	}
}