using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public event Action<Vector2> OnMove;

	public event Action OnFire;

	public event Action<Vector3> OnRotate;

	public event Action OnGamePause;

	public event Action OnGameRevival;

	private Camera _camera;

	private bool _gameIsPaused = false;

	private void Awake()
	{
		_camera = Camera.main;
	}

	private void Update()
	{
		CheckMovement();
		CheckIfFiring();
		CheckRotation();
		CheckPause();
	}

    private void CheckPause()
    {
        if (Input.GetButtonDown("Cancel"))
        {
			_gameIsPaused = !_gameIsPaused;
            if (_gameIsPaused)
            {
				OnGamePause?.Invoke();
			}
            else
            {
				OnGameRevival?.Invoke();
            }
		}
    }

    private Vector3 GetMouseInWorldPosition()
	{
		return _camera.ScreenToWorldPoint(Input.mousePosition
		                                      + Vector3.back * _camera.transform.position.z);
	}

	private void CheckIfFiring()
	{
		if (Input.GetButton("Fire1") && OnFire != null) OnFire();
	}

	private void CheckRotation()
	{
		if (OnRotate != null) OnRotate(GetMouseInWorldPosition());
	}

	private void CheckMovement()
	{
		var movement = Vector2.zero;
		movement.x = Input.GetAxis("Horizontal");
		movement.y = Input.GetAxis("Vertical");
		if (OnMove != null) OnMove(movement);
	}
}
