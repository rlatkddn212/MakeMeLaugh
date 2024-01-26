using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public void Initialize(Vector3 _position)
	{
		transform.position = _position;
	}
}
