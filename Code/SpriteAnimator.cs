using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {

    public Sprite[] frames;
    public int framesPerSecond = 30;
    public bool loop = true;
    public delegate void OnLoopDel();
    public OnLoopDel onLoop;
    public bool useUnscaledDeltaTime;
    private bool isActive = true;
    private float timer;
    private float timerMax;
    private int currentFrame;
    private SpriteRenderer spriteRenderer;
    
    /**
    * Method to construct the spriteAnimator.
    **/
    private void Awake() {
        timerMax = 1f/framesPerSecond;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        if (frames != null) {
            spriteRenderer.sprite = frames[0];
        } else {
            isActive = false;
        }
    }

    /**
    * Method to update the spriteAnimator.
    **/
    private void Update() {
        if (!isActive) return;
        timer += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
        bool newFrame = false;
        while (timer >= timerMax) {
            timer -= timerMax;
            //Next frame
            currentFrame = (currentFrame+1) % frames.Length;
            newFrame = true;
            if (currentFrame == 0) {
                //Looped
                if (!loop) {
                    isActive = false;
                    newFrame = false;
                }
                if (onLoop != null) {
                    onLoop();
                }
            }
        }
        if (newFrame) {
            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    /**
    * Method to setup the spriteAnimator.
    **/
    public void Setup(Sprite[] frames, int framesPerSecond) {
        this.frames = frames;
        this.framesPerSecond = framesPerSecond;
        timerMax = 1f/framesPerSecond;
        spriteRenderer.sprite = frames[0];
        timer = 0f;
        PlayStart();
    }

    /**
    * Method to set the max for the timer.
    **/
    public void SetTimerMax(float timerMax) {
        this.timerMax = timerMax;
    }

    /**
    * Method to start playing the spriteAnimator.
    **/
    public void PlayStart() {
        timer = 0;
        currentFrame = 0;
        spriteRenderer.sprite = frames[currentFrame];
        isActive = true;
    }

}
