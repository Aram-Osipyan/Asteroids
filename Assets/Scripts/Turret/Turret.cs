using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

	[SerializeField] private Bullet bulletPrefab;
	[SerializeField] private float secondsBetweenShots;
	
	private Transform _transform;
	private float _lastShoot;
	private Transform _bulletAnchor;
		
	private void Awake()
	{
		_transform = transform;
		_bulletAnchor = new GameObject("Bullet Anchor").transform;
	}

	public void Fire()
	{
		var now = Time.time;
		if (now - _lastShoot < secondsBetweenShots) return;
		_lastShoot = Time.time;
		Instantiate(bulletPrefab, _transform.position, _transform.rotation, _bulletAnchor);
	}

	public void Rotate(Vector3 rotationPosition)
	{
		transform.LookAt(rotationPosition, Vector3.back);
	}
}
