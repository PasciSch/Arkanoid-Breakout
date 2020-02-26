using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    #region Signleton
    private static GameManger _instance;

    public static GameManger Instance => _instance;

    private void Awake() {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public GameObject gameOverScreen;
    public GameObject victoryScreen;

    public int AvailableLives = 3;
    public bool IsGameStarted { get; set; }
    public int Lives { get; set; }

    public static event Action<int> OnLiveLost;

    private void Start() {
        this.Lives = this.AvailableLives;
        Screen.SetResolution(540, 960, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
        if(BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManger.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Ball ball)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;

            if (this.Lives < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLiveLost.Invoke(this.Lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);   
    }

    private void OnDisable() {
        Ball.OnBallDeath -= OnBallDeath;    
    }
}
