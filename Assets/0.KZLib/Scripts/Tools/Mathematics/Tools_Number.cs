using System;
using UnityEngine;

public static partial class Tools
{
	public static void GetGCD_LCM(int _number1,int _number2,out int _gcd,out int _lcm)
	{
		_gcd = GetGCD(_number1,_number2);
		
		_lcm = _number1*_number2/_gcd;
	}
	
	private static int GetGCD(int _number1,int _number2)
	{
		return _number1 % _number2 == 0 ? _number2 : GetGCD(_number2,_number1%_number2);
	}

	public static byte ConvertBoolArrayToByte(bool[] _sourceArray)
	{
		byte result = 0x00;
		int index  = 8-_sourceArray.Length;
		
		for(var i=0;i<_sourceArray.Length;i++)
		{
			if(_sourceArray[i])
			{
				result |= (byte) (1<<(7-index));
			}

			index++;
		}

		return result;
	}
	
	public static bool[] ConvertByteToBoolArray(byte _byte)
	{
		var resultArray = new bool[8];
		
		for(var i=0;i<8;i++)
		{
			resultArray[i] = (_byte&(1<<i)) != 0;
		}		

		for(var i=0;i<resultArray.Length/2;i++)
		{
            (resultArray[resultArray.Length-i-1],resultArray[i]) = (resultArray[i],resultArray[resultArray.Length-i-1]);
        }

        return resultArray;
	}
}