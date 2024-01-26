using UnityEngine;

public static partial class Tools
{
	public static Vector2 Rotate(Vector2 _vector,float _radians)
	{
		float sin = Mathf.Sin(_radians);
		float cos = Mathf.Cos(_radians);

		return new Vector2(Mathf.Abs(_vector.x*cos)+Mathf.Abs(_vector.y*sin),Mathf.Abs(_vector.x*sin)+Mathf.Abs(_vector.y*cos));
	}

	public static float RotateTowards(float _from,float _to,float _maxAngle)
	{
		var angle = (_to-_from).ToWrapAngle();

		if(Mathf.Abs(angle) > _maxAngle)
		{
			angle = _maxAngle*Mathf.Sign(angle);
		}

		return _from+angle;
	}
}