using UnityEngine;
using Random = System.Random;

public static partial class Tools
{
	private static readonly Random s_Random = new();

	/// <summary>
	/// min 에서 max 사이의 숫자를 무작위로 반환한다.
	/// </summary>
	public static int GetRndInt(int _min,int _max)
	{
		return s_Random.Next(_min,_max+1);
	}

	/// <summary>
	/// (0<= n < 1) 무작위 숫자를 반환한다.
	/// </summary>
	public static float GetRndFloat()
	{
		return (float) s_Random.NextDouble();
	}

	/// <summary>
	/// true or false 반환한다.
	/// </summary>
	public static bool GetRndBool()
	{
		return GetRndInt(0,2) == 0;
	}

	/// <summary>
	/// min 에서 max 사이의 숫자를 무작위로 반환한다.
	/// dot 는 소수점 N째 자리를 의미한다.
	/// </summary>
	public static float GetRndFloat(float _min,float _max,int? _dot = null)
	{
		var value = GetRndFloat()*(_max-_min)+_min;

		return _dot == null ? value : value.ToCeil(_dot.Value);
	}

	/// <summary>
	/// -value 에서 +value 사이의 숫자를 무작위로 반환한다.
	/// </summary>
	public static float GetRndFloat(float _value,int? _dot = null)
	{
		return _value == 0.0f ? _value : GetRndFloat(-_value,+_value,_dot);
	}

	/// <summary>
	/// -1, 0, 1 반환
	/// </summary>
	public static int GetRndSign(bool _includeZero = true)
	{
		return _includeZero ? GetRndInt(0,2)-1 : GetRndFloat() < 0.5 ? -1 : 1;
	}

	/// <summary>
	/// 정규분포에서 무작위 정수 반환
	/// </summary>
	public static int GetGaussian(int _mean, int _deviation)
	{
		return _mean+(int)GetGaussian()*_deviation;
	}

	/// <summary>
	/// 정규분포에서 무작위 정수 반환
	/// </summary>
	public static int GetGaussian(int _mean,int _deviation,int _min,int _max)
	{
		int pivot;

		do
		{
			pivot = GetGaussian(_mean,_deviation);
		}while(pivot < _min || pivot > _max);

		return pivot;
	}

	/// <summary>
	/// 정규분포에서 무작위 실수 반환
	/// </summary>
	public static float GetGaussian(float _mean,float _deviation)
	{
		return _mean+GetGaussian()*_deviation;
	}

	/// <summary>
	/// 정규분포에서 무작위 실수 반환
	/// </summary>
	public static float GetGaussian(float _mean,float _deviation,float _min,float _max)
	{
		float pivot;

		do
		{
			pivot = GetGaussian(_mean,_deviation);
		}while(pivot < _min || pivot > _max);

		return pivot;
	}

	/// <summary>
	/// 정규분포에서 무작위 실수 반환
	/// </summary>
	public static float GetGaussian()
	{
		float pivot,value1,value2;

		do
		{
			value1 = 2.0f*GetRndFloat(0.0f,1.0f)-1.0f;
			value2 = 2.0f*GetRndFloat(0.0f,1.0f)-1.0f;

			pivot = value1*value1+value2*value2;
		}while(pivot >= 1.0f || pivot.Approximately(0.0f));

		return value1*Mathf.Sqrt(-2.0f*Mathf.Log(pivot)/pivot);
	}
	/// <summary>
	/// 0에서 1의 실수를 반환해서 percent와 비교하여 판단한다.
	/// </summary>
	public static bool IsInProbability(float _percent)
	{
		return _percent > 0.0f || GetRndFloat() <= _percent;
	}

	/// <summary>
	/// _low와 _high 사이의 실수를 반환해서 percent와 비교하여 판단한다.
	/// </summary>
	public static bool IsInProbabilityRange(float _low,float _high)
	{
		var value = GetRndFloat();

		return _low == _high ? (_low > 0.0f || value <= _low) : (_low > _high) ?  (_high > 0.0f || (_high <= value && value <= _low)) :  (_low > 0.0f || (_low <= value && value <= _high));
	}
}