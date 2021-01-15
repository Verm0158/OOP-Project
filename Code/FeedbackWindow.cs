using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class FeedbackWindow : MonoBehaviour {

    private Text feedbackText;
    private Text answer;

    /**
    * Method to construct the feedbackwindow.
    **/
    private void Awake() {
        feedbackText = transform.Find("FeedbackText").GetComponent<Text>();
        answer = transform.Find("Answer").GetComponent<Text>();

        transform.Find("skipBtn").GetComponent<Button_UI>().ClickFunc = () => { Bird.GetInstance().play(); };
        transform.Find("skipBtn").GetComponent<Button_UI>().AddButtonSounds();

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    /** Method to hide the feedbackwindow at the start of the game.
    *
    **/
    private void Start() {
        Bird.GetInstance().Feedback += Bird_Feedback;
        Hide();
    }

    /**
    * Method to show the feedbackwindow with the right text.
    **/
    private void Bird_Feedback(object sender, System.EventArgs e) {
        feedbackText.text = Level.GetInstance().GetFeedback();

        answer.text = "Antwoord is: " + Level.GetInstance().GetAnswer();

        Show();
    }

    /**
    * Method to hide the feedbackwindow.
    **/
    public void Hide() {
        gameObject.SetActive(false);
    }

    /**
    * Method to show the feedbackwindow.
    **/
    private void Show() {
        gameObject.SetActive(true);
    }

}
