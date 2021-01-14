/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class FeedbackWindow : MonoBehaviour {

    private Text feedbackText;
    private Text answer;

    private void Awake() {
        feedbackText = transform.Find("FeedbackText").GetComponent<Text>();
        answer = transform.Find("Answer").GetComponent<Text>();

        transform.Find("skipBtn").GetComponent<Button_UI>().ClickFunc = () => { Bird.GetInstance().play(); };
        transform.Find("skipBtn").GetComponent<Button_UI>().AddButtonSounds();

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }


    private void Start() {
        Bird.GetInstance().Feedback += Bird_Feedback;
        Hide();
    }

    private void Bird_Feedback(object sender, System.EventArgs e) {
        feedbackText.text = Level.GetInstance().GetFeedback();

        answer.text = "Antwoord is: " + Level.GetInstance().GetAnswer();

        Show();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

}
