using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour {

    private Text highscoreText;
    private Text scoreText;

    /**
    * Method to construct the scorewindow.
    **/
    private void Awake() {
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        highscoreText = transform.Find("highscoreText").GetComponent<Text>();
    }

    /**
    * Method to start the scorewindow.
    **/
    private void Start() {
        highscoreText.text = "HIGHSCORE: " + Score.GetHighscore().ToString();
        Bird.GetInstance().OnDied += ScoreWindow_OnDied;
        Bird.GetInstance().OnStartedPlaying += ScoreWindow_OnStartedPlaying;
        Hide();
    }

    /**
    * Method to show the scorewindow when the game started playing.
    **/
    private void ScoreWindow_OnStartedPlaying(object sender, System.EventArgs e) {
        Show();
    }

    /** 
    * Method to hide the scorewindow when the bird died
    **/
    private void ScoreWindow_OnDied(object sender, System.EventArgs e) {
        Hide();
    }

    /**
    * Method to update the scorewindow.
    **/
    private void Update() {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
    }

    /**
    * Method to hide the scorewindow.
    **/
    private void Hide() {
        gameObject.SetActive(false);
    }

    /**
    * Method to show the scorewindow.
    **/
    private void Show() {
        gameObject.SetActive(true);
    }

}
