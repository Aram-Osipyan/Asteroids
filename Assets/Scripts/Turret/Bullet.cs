using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts.Asteroids;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

	[SerializeField] private float speed;
	[SerializeField] private float secondsToLive = 2;
	[SerializeField] private float damage;
	
	private Rigidbody _rigidbody;
	private Transform _transform;
	private Coroutine _secondsToLiveCoroutine;
	private WaitForSeconds _waitingTime;
	
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_transform = transform;
		_rigidbody.drag = 0;
		_waitingTime = new WaitForSeconds(secondsToLive);
	}

	private void Start()
	{
		_rigidbody.velocity = _transform.forward * speed;
		StartCoroutine(SecondsToLive());
	}

	private IEnumerator SecondsToLive()
	{
		yield return _waitingTime;
		Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision other)
	{
		var otherGameObject = other.gameObject;
		var asteroid = otherGameObject.GetComponentInParent<Asteroid>();
		if (asteroid == null) return;
		asteroid.ReceiveDamage(damage, true);
		Destroy(gameObject);
	}
}
