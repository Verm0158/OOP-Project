/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Bird : MonoBehaviour {

    private const float JUMP_AMOUNT = 90f;

    private static Bird instance;

    public QuestionWindow questionWindow;
    public FeedbackWindow feedbackWindow;
    public TextWindow answersWindow;

    public static Bird GetInstance() {
        return instance;
    }

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;
    public event EventHandler Question;
    public event EventHandler Feedback;
    public event EventHandler Answers;
    private bool waiting = true;

    public Rigidbody2D birdRigidbody2D;
    private State state;

    private enum State {
        WaitingToStart,
        Playing,
        Dead,
        Question,
        Feedback,
        Answers
    }

    private void Awake() {
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }

    private void Update() {
        switch (state) {
        default:
        case State.WaitingToStart:
            if (TestInput()) {
                // Start playing
                state = State.Playing;
                birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                Jump();
                if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
            }
            break;
        case State.Playing:
            if (TestInput()) {
                Jump();
            }

            // Rotate bird as it jumps and falls
            transform.eulerAngles = new Vector3(0, 0, birdRigidbody2D.velocity.y * .15f);
            break;
        case State.Dead:
            break;
        case State.Question:
            if (Question != null) Question(this, EventArgs.Empty);
            if (Answers != null) Answers (this, EventArgs.Empty);
            if (waiting == false) {
                state = State.Playing;
                birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                Jump();
                questionWindow.Hide();
                answersWindow.Hide();
                waiting = true;
                if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
            }
            break;
        case State.Feedback:
            if (Feedback != null) Feedback(this, EventArgs.Empty);
            if (waiting == false) {
                state = State.Playing;
                birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                Jump();
                feedbackWindow.Hide();
                waiting = true;
                if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
            }
            break;
        }
        aboveMap();
    }


    private bool TestInput() {
        return 
            Input.GetKeyDown(KeyCode.Space) || 
            Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0;
    }

    private void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
        SoundManager.PlaySound(SoundManager.Sound.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }

    private void aboveMap(){
        if(birdRigidbody2D.position.y > 55) {
            birdRigidbody2D.bodyType = RigidbodyType2D.Static;
            SoundManager.PlaySound(SoundManager.Sound.Lose);
            if (OnDied != null) OnDied(this, EventArgs.Empty);
        }
    }

    public float GetYPosition(){
        return birdRigidbody2D.position.y;
    }

    public void stopQuestion(){
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.Question;
    }

    public void stopFeedback(){
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.Feedback;
    }

    public bool play() {
        waiting = false;
        return waiting;
    }
}
