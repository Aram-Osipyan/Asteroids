using System;
using System.Collections;
using __Scripts.Asteroids;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private Jumper player;
    [Header("UI")]
    [SerializeField] private UiTextModifier scoreText;
    [SerializeField] private UiTextModifier jumpsText;
    [SerializeField] private GameOverCanvas _gameOverCanvas;
    [Header("Game Settings")]
    [SerializeField,Range(0,5)] private float timeBetweenReload = 2f;
    private float _score;
    private int _bigAsteroidCount = 2;

    private void Awake()
    {
        asteroidSpawner.OnAsteroidDestroyed += AsteroidDestroyed;
        asteroidSpawner.OnAllAsteroidDestroyed += GameReload;
        player.OnJumpUsed += JumpUsed;
    }

    private void Start()
    {
        jumpsText.UpdateText(player.JumpsRemaining);
    }

    private void AsteroidDestroyed(Asteroid asteroid)
    {
        if (!asteroid.DestroyedByBullet) 
            return;
        _score += asteroid.Points;
        scoreText.UpdateText(_score);
    }

    private void JumpUsed(int jumpsRemaining)
    {
        if(jumpsRemaining == -1) 
            GameOver();
        else 
            jumpsText.UpdateText(jumpsRemaining);
    }

    private void GameReload()
    {
        _bigAsteroidCount++;
        StartCoroutine(InitAsteroids());
        Debug.Log("GameReload");
    }

    private IEnumerator InitAsteroids()
    {
        yield return new WaitForSeconds(timeBetweenReload);
        asteroidSpawner.InitAsteroidByType(AsteroidType.Big, _bigAsteroidCount++);
    } 

    private void GameOver()
    {
        _gameOverCanvas.ShowGameOverCanvas(_score);
        Debug.Log("GameOver");
    }

}
