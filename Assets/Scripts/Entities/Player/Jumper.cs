using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(APlayerShip))]
public class Jumper : MonoBehaviour
{
	[SerializeField] private float secondsBetweenJumps = 2f;
	[SerializeField] private float maximumDistanceFromAsteroids = 2f;
	[SerializeField] private int quantityOfJumps = 3;

	public delegate void JumpUsed(int remainingJumps);

	public event JumpUsed OnJumpUsed;
	public float JumpsRemaining
	{
		get { return quantityOfJumps; }
	}
	
	private WaitForSeconds _waitingTime;
	private Func<IEnumerator> _jumpCoroutine;
	private Collider[] _hits;
	private Transform _transform;
	private Bounds _bounds;
	private int _asteroidsLayer;
	private APlayerShip _playerShip;

	private void Awake()
	{
		_playerShip = GetComponent<APlayerShip>();
		_waitingTime = new WaitForSeconds(secondsBetweenJumps);
		_hits = new Collider[10];
		_jumpCoroutine = JumpCoroutine;
		_bounds = MapLimit.GetCameraBounds(Camera.main);
		_asteroidsLayer = LayerMask.GetMask("Asteroid");
		_playerShip.Stats.OnDie += StartCoroutineForJumping;
		_transform = transform;
	}

	private void StartCoroutineForJumping()
	{
		if (quantityOfJumps > 0)
		{
			StartCoroutine(_jumpCoroutine());
		}
		quantityOfJumps--;
		_playerShip.Disable();
		if (OnJumpUsed != null) OnJumpUsed(quantityOfJumps);
	}

	private IEnumerator JumpCoroutine()
	{
		yield return _waitingTime;
		Jump();
	}
	
	private void Jump()
	{
		var size = 1;
		var position = Vector3.zero;
		while (size > 0)
		{
			position = GetRandomPosition();
			size = Physics.OverlapSphereNonAlloc(position, maximumDistanceFromAsteroids, _hits, _asteroidsLayer);
		}

		_transform.position = position;
		_playerShip.Enable();
	}

	private Vector3 GetRandomPosition()
	{
		var x = Random.Range(_bounds.min.x, _bounds.max.x);
		var y = Random.Range(_bounds.min.y, _bounds.max.y);
		return new Vector3(x, y, _transform.position.z);
	}

	
}
