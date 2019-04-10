﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OldMoatGames;
using Fungus;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public GameObject BananaIdle;
    public GameObject BananaTurn;
    public GameObject youDiePic;
    public GameObject youWinPic;
    public GameObject winButton;
    public GameObject loseButton;
    public GameObject timerS;

    private Vector3 startPos;
    private float Timer;
    private float randomTimer;
    private bool stopTimer = false;
    private bool checkPlayer = false;
    private bool isStart = false;
    private AnimatedGifPlayer AnimatedGifPlayer;
    private DragFunction dragFuc;
    private AudioSource dieSFX;

    public List<float> RandomNub;
    public float RestartTime;
    public float dieTime;
    public float startTimer;

    public Flowchart fcC1;


    // Start is called before the first frame update
    void Awake()
    {
        dieSFX = GetComponent<AudioSource>();
        dragFuc = FindObjectOfType<DragFunction>();
    }

    private void Start()
    {
        randomTimer = RandomNub[Random.Range(0, RandomNub.Count)];
        startPos = dragFuc.gameObject.transform.position;
        Timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            TimerFunc();
            if (checkPlayer)
                lostGameCheck();
        }
        else
            StartTimer();
    }

    void StartTimer()
    {
        startTimer -= Time.deltaTime;
        timerS.GetComponent<TextMeshProUGUI>().text = ((int)startTimer).ToString();
        if ((int)startTimer == 0)
            timerS.GetComponent<TextMeshProUGUI>().text = "Go!";
        if (startTimer < 0)
        {
            isStart = true;
            timerS.SetActive(false);
            BananaIdle.SetActive(true);
            dragFuc.canMove = true;
        }
    }
    void TimerFunc()
    {
        if (!stopTimer)
            Timer += Time.deltaTime;
        if (Timer >= randomTimer)
        {
            stopTimer = true;
            Timer = 0;
            BananaIdle.SetActive(false);
            BananaTurn.SetActive(true);
            AnimatedGifPlayer = BananaTurn.GetComponent<AnimatedGifPlayer>();
            AnimatedGifPlayer.Play();
            Invoke("CheckPlayer", dieTime);
            Invoke("Restart", RestartTime);
        }
    }
    void lostGameCheck()
    {
        if (dragFuc.Playerfollowing)
        {
            youDiePic.SetActive(true);
            loseButton.SetActive(true);
            dragFuc.canMove = false;
            Time.timeScale = 0;
        }
    }
    public void ResetGameButton()
    {
        dragFuc.gameObject.transform.position = startPos;
        Restart();
        dragFuc.canMove = true;
        youDiePic.SetActive(false);
        loseButton.SetActive(false);
        Time.timeScale = 1;
    }

    public void wonGameFuc()
    {
        winButton.SetActive(true);
        youWinPic.SetActive(true);
        Time.timeScale = 0;
    }
    public void wonButton()
    {
        Time.timeScale = 1;
        fcC1.SetBooleanVariable("StealthGameWon", true);
    }

    void CheckPlayer()
    {
        checkPlayer = true;
        dieSFX.Play();
    }
    void Restart()
    {
        stopTimer = false;
        checkPlayer = false;
        randomTimer = RandomNub[Random.Range(0, RandomNub.Count)];
        BananaIdle.SetActive(true);
        BananaTurn.SetActive(false);
    }
}
