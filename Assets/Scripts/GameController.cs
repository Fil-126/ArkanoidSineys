using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public TMP_Text scoreLabel;
    public TMP_Text levelLabel;
    public TMP_Text hpLabel;

    public GameObject levelCompleteLabel;

    public GameObject[] levels;
    public BallMovement ballMovement;
    
    private int _playerHp;
    private int _score;
    private int _level;
    private int _bricks;

    public int playerHp
    {
        get => _playerHp;
        set
        {
            _playerHp = value;
            hpLabel.SetText(_playerHp.ToString());
        }
    }
    public int score
    {
        get => _score;
        set
        {
            _score = value;
            PlayerScore.score = _score;
            scoreLabel.SetText(_score.ToString());
        }
    }

    public int level
    {
        get => _level;
        set
        {
            _level = value;
            if (_level > levels.Length)
            {
                SceneManager.LoadScene("WinMenu", LoadSceneMode.Single);
                return;
            }
            
            if (_level > 1)
            {
                ballMovement.ResetBall();
                levels[_level - 2].SetActive(false);
                levels[_level - 1].SetActive(true);
            }
            
            levelLabel.SetText(_level.ToString());
        }
    }
    public int bricks
    {
        get => _bricks;
        set
        {
            _bricks = value;
            if (_bricks <= 0)
            {
                levelCompleteLabel.SetActive(true);
                ballMovement.gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerHp = 3;
        score = 0;
        level = 1;
        CountBricks();
    }

    void Update()
    {
        if (levelCompleteLabel.activeSelf && Input.GetButtonDown("Jump"))
        {
            level += 1;
            if (_level <= levels.Length)
            {
                levelCompleteLabel.SetActive(false);
                ballMovement.gameObject.SetActive(true);
                CountBricks();    
            }
        }
    }

    private void CountBricks()
    {
        _bricks = levels[level - 1].transform.childCount;
    }
}
