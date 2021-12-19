using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class CameraFollow : GameObjectController
{
	public float smoothing = 5f;

	private Transform _target;
	private GameObjectData _cashedGameObjectData;
	
	Vector3 offset = new Vector3(0f, 6f, -7.5f);
	
	public void SetTarget(Transform target)
	{
		_target = target;
	}

	void FixedUpdate(){

		if (_target is null)
		{
			return;
		}
		
		Vector3 targetCamPos = _target.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
