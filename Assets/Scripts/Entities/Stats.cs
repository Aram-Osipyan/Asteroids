using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts;
using UnityEngine;

public class Stats : MonoBehaviour
{
	[SerializeField] private StatsScriptableObject stats;
	
	/// <summary>
	/// <para>
	/// Event executed when the health reaches zero
	/// </para>
	/// </summary>
	public event Action OnDie;

	private float _currentHealth;
	
	public float Health
	{
		get { return _currentHealth; }
		set
		{
			if (value < 0.01f && OnDie != null) OnDie();
			_currentHealth = value;
		}
	}

	public float MovementSpeed
	{
		get { return stats.movementSpeed; }
	}

	public float RotationSpeed
	{
		get { return stats.rotationSpeed; }
	}

	public float MaxSpeed
	{
		get { return stats.maxSpeed; }
	}

	private void Awake()
	{
		_currentHealth = stats.health;
	}

	public void ResetHealth()
	{
		_currentHealth = stats.health;
	}
}
