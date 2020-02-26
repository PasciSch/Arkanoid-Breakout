using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool IsGameStarted { get; set; }

    private void Start() {
        Screen.SetResolution(540, 960, false);
    }
}
