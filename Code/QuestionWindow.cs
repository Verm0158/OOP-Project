using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class QuestionWindow : MonoBehaviour {

    private Text questionText;

    /**
    * Method to construct the questionwindow.
    **/
    private void Awake() {
        questionText = transform.Find("QuestionText").GetComponent<Text>();

        transform.Find("skipBtn").GetComponent<Button_UI>().ClickFunc = () => { Bird.GetInstance().play(); };
        transform.Find("skipBtn").GetComponent<Button_UI>().AddButtonSounds();

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    /**
    * Method to hide the questionwindow at the start of the game.
    **/
    private void Start() {
        Bird.GetInstance().Question += Bird_Question;
        Hide();
    }

    /** 
    * Method to show the questionwindow with the right text.
    **/
    private void Bird_Question(object sender, System.EventArgs e) {
        questionText.text = Level.GetInstance().GetQuestion();

        Show();
    }

    /** 
    * Method to hide the questionwindow.
    **/
    public void Hide() {
        gameObject.SetActive(false);
    }

    /**
    * Method to show the questionwindow.
    **/
    private void Show() {
        gameObject.SetActive(true);
    }

}
