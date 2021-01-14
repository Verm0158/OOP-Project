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
using CodeMonkey;
using CodeMonkey.Utils;

public class Level : MonoBehaviour {

    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = +100f;
    private const float GROUND_DESTROY_X_POSITION = -200f;
    private const float CLOUD_DESTROY_X_POSITION = -160f;
    private const float CLOUD_SPAWN_X_POSITION = +160f;
    private const float CLOUD_SPAWN_Y_POSITION = +30f;
    private const float BIRD_X_POSITION = 0f;

    private static Level instance;

    public static Level GetInstance() {
        return instance;
    }

    private List<Transform> groundList;
    private List<Transform> cloudList;
    private float cloudSpawnTimer;
    private List<Pipe> pipeList;
    private List<MiddlePipe> middlePipeList;
    private FeedbackWindow feedbackWindow;
    private int pipesPassedCount;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;
    private State state;
    private bool isWaitingQuestion = false;
    private bool isWaitingFeedback = false;
    private bool isWaitingAnswer = false;
    private string answer;
    private List<string> questions; 
    private List<string> feedback;
    private List<string> rightAnswers; 
    private List<string> wrongAnswers;
    private int randomIndex;
    private bool isWaitingIndex = false;
    private bool isWaitingReady = false;
    private string currentQuestion;
    private string currentFeedback;
    private string currentRightAnswer;
    private string currentWrongAnswer;
    private int randomNumber;
    private bool answerIsRight;


    public enum Difficulty {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private enum State {
        WaitingToStart,
        Playing,
        BirdDead,
    }


    private void Awake() {
        instance = this;
        SpawnInitialGround();
        SpawnInitialClouds();
        pipeList = new List<Pipe>();
        middlePipeList = new List<MiddlePipe>();
        pipeSpawnTimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
        

            questions = new List<string>{"Is het handig om voor elk account hetzelfde wachtwoord te gebruiken?", "Welke mensen houden zich bezig met het beveiligen van je gegevens?", "Wat wordt er niet privé gehouden op je social media-account?", "Is het handig om je email te verifiëren na het aanmaken van een social media-account?", "Welke van de twee antwoorden is handiger om te doen met betrekking tot het kiezen van een goed wachtwoord?", "Wat is het handigste om te doen als je een hele nare reactie krijgt?", "Je ziet dat er een vervelende reactie wordt geplaatst onder een bericht van een goede vriend/vriendin. Wat kan je het beste doen?", "Hoe kan je online pesten in grote maten voorkomen?", "Stel dat jouw social media-account op privé staat, is het dan handig om iedereen toe te voegen die jou wilt volgen?"};
            feedback = new List<string>{"Als je dit doet en je wachtwoord wordt gehackt, kan de hacker in elk account waar je dit wachtwoord hebt gebruikt.", "Cyber beveiligers zorgen ervoor dat je belangrijkste gegevens worden beschermd tegen hackers. Deze hackers proberen je gegevens juist te stelen.", "Je gebruikersnaam is nooit privé. Hierdoor kunnen andere mensen zoals je vrienden je vinden op social media. Je foto’s kan je laten afschermen voor onbekenden.", "Dit is een extra beveiliging. Als je op een nieuw apparaat op je account wilt inloggen moet je dit doen met je wachtwoord en een code die via je mail wordt verstuurd. Als je wachtwoord is gekraakt kan er in dat geval niemand in je account zolang ze niet bij je mail kunnen.", "Een wachtwoord wordt gekraakt door een computerprogramma. Hoe langer het wachtwoord is, hoe langer dit programma doet over het kraken van je wachtwoord.", "Als je iemand blokkeert kan hij of zij je op geen enkele manier via dit social media-platform bereiken. Deze persoon kan je dus nooit meer lastig vallen.", "Mensen die vervelende reacties plaatsen willen je uit de tent lokken en hopen dat je een nare reactie plaatst. Help deze mensen daar nooit bij. Rapporteer het bericht en het wordt verwijderd door het social media-platform.", "Zodra je gevoelige informatie niet deelt heeft niemand een reden om je te pesten. Zelf gaan pesten is nooit de oplossing. Hierdoor kan je mensen verdrietig en onzeker maken.", "Als je je account op privé zet wil je dat niet iedereen zomaar je foto’s en andere gegevens kan bekijken. Als je daarna onbekenden gaat toevoegen kunnen zij toch deze gegevens bekijken. Dan heeft het dus geen zin om je account op privé te zetten."};
            rightAnswers = new List<string>{"Nee", "Cyber beveiligers", "Gebruikers-naam", "Ja", "Een lang wachtwoord", "Persoon blokkeren", "Bericht rapporteren", "Niet delen van gevoelige informatie", "Nee"};
            wrongAnswers = new List<string>{"Ja", "Hackers", "Foto's", "Nee", "Een wachtwoord tekens", "Persoon negeren", "Nare reactie plaatsen", "Zelf gaan pesten", "Ja"};
     
    }
    private void Start() {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs e) {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e) {
        //CMDebug.TextPopupMouse("Dead!");
        state = State.BirdDead;
    }

    private void Update() {
        if (state == State.Playing) {
            HandlePipeMovement();
            HandlePipeSpawning();
            HandleGround();
            HandleClouds();
            drawQuestion();
            if(isWaitingIndex == false){
                getRandomIndex();
                getQuestion();
                getFeedback();
                getRightAnswer();
                getWrongAnswer();
                // upperAnswer() = upperAnswer;
                randomNumber = Random.Range(0,2);
                upperAnswer();
                Debug.Log(upperAnswerIsRight());
                Debug.Log(randomNumber);
                
            }
            for (int i = 0; i < middlePipeList.Count; i++) {
            MiddlePipe middlePipe = middlePipeList[i];
                if (middlePipe.GetXPosition() > 0 && middlePipe.GetXPosition() < 0.2 && isWaitingAnswer == false) {
                    if(isUpperGap() == true && upperAnswerIsRight() == true) {
                        answer = "Goed";
                    }
                    if(isUpperGap() == true && upperAnswerIsRight() == false){
                        answer = "Fout";
                    }
                    if(isUpperGap() == false && upperAnswerIsRight() == true){
                        answer = "Fout";
                    }
                    if(isUpperGap() == false && upperAnswerIsRight() == false){
                        answer = "Goed";
                    }
                    isWaitingAnswer = true;
                }
                if(middlePipe.GetXPosition() < -25 && middlePipe.GetXPosition() > -26 && isWaitingIndex == true && isWaitingReady == false){
                    feedback.RemoveAt(randomIndex);
                    questions.RemoveAt(randomIndex);
                    rightAnswers.RemoveAt(randomIndex);
                    wrongAnswers.RemoveAt(randomIndex);
                    isWaitingIndex = false;
                    isWaitingReady = true;
                }
            }
        }
    }

    private void SpawnInitialClouds() {
        cloudList = new List<Transform>();
        Transform cloudTransform;
        cloudTransform = Instantiate(GetCloudPrefabTransform(), new Vector3(0, CLOUD_SPAWN_Y_POSITION, 0), Quaternion.identity);
        cloudList.Add(cloudTransform);
    }

    private Transform GetCloudPrefabTransform() {
        switch (Random.Range(0, 3)) {
        default:
        case 0: return GameAssets.GetInstance().pfCloud_1;
        case 1: return GameAssets.GetInstance().pfCloud_2;
        case 2: return GameAssets.GetInstance().pfCloud_3;
        }
    }

    private void HandleClouds() {
        // Handle Cloud Spawning
        cloudSpawnTimer -= Time.deltaTime;
        if (cloudSpawnTimer < 0) {
            // Time to spawn another cloud
            float cloudSpawnTimerMax = 6f;
            cloudSpawnTimer = cloudSpawnTimerMax;
            Transform cloudTransform = Instantiate(GetCloudPrefabTransform(), new Vector3(CLOUD_SPAWN_X_POSITION, CLOUD_SPAWN_Y_POSITION, 0), Quaternion.identity);
            cloudList.Add(cloudTransform);
        }

        // Handle Cloud Moving
        for (int i=0; i<cloudList.Count; i++) {
            Transform cloudTransform = cloudList[i];
            // Move cloud by less speed than pipes for Parallax
            cloudTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime * .7f;

            if (cloudTransform.position.x < CLOUD_DESTROY_X_POSITION) {
                // Cloud past destroy point, destroy self
                Destroy(cloudTransform.gameObject);
                cloudList.RemoveAt(i);
                i--;
            }
        }
    }

    private void SpawnInitialGround() {
        groundList = new List<Transform>();
        Transform groundTransform;
        float groundY = -47.5f;
        float groundWidth = 192f;
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(0, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth * 2f, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
    }

    private void HandleGround() {
        foreach (Transform groundTransform in groundList) {
            groundTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;

            if (groundTransform.position.x < GROUND_DESTROY_X_POSITION) {
                // Ground passed the left side, relocate on right side
                // Find right most X position
                float rightMostXPosition = -100f;
                for (int i = 0; i < groundList.Count; i++) {
                    if (groundList[i].position.x > rightMostXPosition) {
                        rightMostXPosition = groundList[i].position.x;
                    }
                }

                // Place Ground on the right most position
                float groundWidth = 192f;
                groundTransform.position = new Vector3(rightMostXPosition + groundWidth, groundTransform.position.y, groundTransform.position.z);
            }
        }
    }

    private void HandlePipeSpawning() {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0) {
            // Time to spawn another Pipe
            pipeSpawnTimer += pipeSpawnTimerMax;
            
            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);
            
            if(pipesSpawned % 4 == 0 && pipesSpawned != 0){
                CreateDubblePipes(height, gapSize, PIPE_SPAWN_X_POSITION);
            } else {
                CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
            }

            
        }
    }

    private bool isMiddlePipe(){
        
        for (int i = 0; i < middlePipeList.Count; i++){
        
        MiddlePipe middlePipe = middlePipeList[i];
        if(middlePipe.GetXPosition() < 5f && middlePipe.GetXPosition() > -5f) {
            return true;
        } 
        } 
            return false;
    }

    private void drawQuestion(){
        Bird bird = Bird.GetInstance();
        for (int i = 0; i < middlePipeList.Count; i++)
        {
            MiddlePipe middlePipe = middlePipeList[i];
            
            if(middlePipe.GetXPosition() > 40f && middlePipe.GetXPosition() < 41f && isWaitingQuestion == false){
                isWaitingQuestion = true;
                state = State.WaitingToStart;
                bird.stopQuestion();
                isWaitingFeedback = false;
                isWaitingReady = false;
            }

            if(middlePipe.GetXPosition() < -20f && middlePipe.GetXPosition() > -21f && isWaitingFeedback == false){
                isWaitingFeedback = true;
                state = State.WaitingToStart;
                bird.stopFeedback();
                isWaitingQuestion = false;
                isWaitingAnswer = false;
            }
        }
        

    }

    public void getRandomIndex(){
            randomIndex = Random.Range(0, questions.Count);
            isWaitingIndex = true;
    }

    public void getQuestion(){
        for (int i = 0; i < questions.Count; i++)
        {
            currentQuestion = questions[randomIndex];
        }
    }

    public string GetQuestion() {
        return currentQuestion;
    }

    
    public void getFeedback(){
        for (int i = 0; i < feedback.Count; i++)
        {
            currentFeedback = feedback[randomIndex];
        }
    }

    public string GetFeedback() {
        return currentFeedback;
    }

    
    public void getRightAnswer(){
        for (int i = 0; i < rightAnswers.Count; i++)
        {
            currentRightAnswer = rightAnswers[randomIndex];
        }
    }

    public string GetRightAnswer() {
        return currentRightAnswer;
    }
    
    public void getWrongAnswer(){
        for (int i = 0; i < wrongAnswers.Count; i++)
        {
            currentWrongAnswer = wrongAnswers[randomIndex];
        }
    }

    public string GetWrongAnswer() {
        return currentWrongAnswer;
    }

    public string upperAnswer(){
        
        if(randomNumber == 0){
            return currentRightAnswer;
        } if(randomNumber == 1) {
            return currentWrongAnswer;
        } return "ERROR";
            
    }

    public string lowerAnswer(){
        if(randomNumber == 0){
            return currentWrongAnswer;
        }
            return currentRightAnswer;
    }
    
    private bool upperAnswerIsRight(){
        if(randomNumber == 0){
            return true;
        } return false;
    }

    private bool answerDraw() {
        if (isUpperGap() == true) {
            return false;
        } 
        return true;
    }

    private bool isUpperGap(){
        Bird bird = Bird.GetInstance();
        if(isMiddlePipe() == true){
            if(bird.GetYPosition() > 0){
                return true;
            } 
        }
        return false;
    }

    public string GetAnswer() {
        return answer;
    }

    private void HandlePipeMovement() {
        for (int i=0; i<pipeList.Count; i++) {
            Pipe pipe = pipeList[i];

            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom()) {
                // Pipe passed Bird
                pipesPassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.Score);
            }

            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION) {
                // Destroy Pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }

        for (int i = 0; i < middlePipeList.Count; i++)
        {
            MiddlePipe middlePipe = middlePipeList[i];
            middlePipe.Move();

            if (middlePipe.GetXPosition() < PIPE_DESTROY_X_POSITION) {
                // Destroy Pipe
                middlePipe.DestroySelf();
                middlePipeList.Remove(middlePipe);
                i--;
            }

        }
    }

    private void SetDifficulty(Difficulty difficulty) {
        switch (difficulty) {
        case Difficulty.Easy:
            gapSize = 50f;
            pipeSpawnTimerMax = 1.4f;
            break;
        case Difficulty.Medium:
            gapSize = 40f;
            pipeSpawnTimerMax = 1.3f;
            break;
        case Difficulty.Hard:
            gapSize = 33f;
            pipeSpawnTimerMax = 1.1f;
            break;
        case Difficulty.Impossible:
            gapSize = 24f;
            pipeSpawnTimerMax = 1.0f;
            break;
        }
    }

    private Difficulty GetDifficulty() {
        if (pipesSpawned >= 24) return Difficulty.Impossible;
        if (pipesSpawned >= 12) return Difficulty.Hard;
        if (pipesSpawned >= 5) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPosition) {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreateDubblePipes(float gapY, float gapSize, float xPosition) {
        CreatePipe(100 * .15f, xPosition, true);
        CreateMiddlePipes(10f, xPosition);
        CreatePipe(100 * .1f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreateMiddlePipes(float height, float xPosition){
        // Set up Pipe Head
        Transform pipeHeadTop = Instantiate(GameAssets.GetInstance().pfPipeHead);
        Transform pipeHeadBottom = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPositionTop;
        float pipeHeadYPositionBottom;
        
            pipeHeadYPositionTop = 5 - PIPE_HEAD_HEIGHT * .5f;
            pipeHeadYPositionBottom = -5 + PIPE_HEAD_HEIGHT * .5f;
        
        pipeHeadTop.position = new Vector3(xPosition, pipeHeadYPositionTop);
        pipeHeadBottom.position = new Vector3(xPosition, pipeHeadYPositionBottom);

        // Set up Pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        pipeBody.position = new Vector3(xPosition, -5f);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

        MiddlePipe middlePipe = new MiddlePipe(pipeHeadTop, pipeHeadBottom, pipeBody);
        middlePipeList.Add(middlePipe);
    }

    private void CreatePipe(float height, float xPosition, bool createBottom) {
        // Set up Pipe Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom) {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        } else {
            pipeHeadYPosition = +CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);

        // Set up Pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom) {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        } else {
            pipeBodyYPosition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    public int GetPipesSpawned() {
        return pipesSpawned;
    }

    public int GetPipesPassedCount() {
        return pipesPassedCount;
    }

    /*
     * Represents a single Pipe
     * */
    private class Pipe {

        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom) {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move() {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition() {
            return pipeHeadTransform.position.x;
        }

        public bool IsBottom() {
            return isBottom;
        }

        public void DestroySelf() {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }

    }

    private class MiddlePipe{
        
        private Transform pipeHeadTopTransform;
        private Transform pipeHeadBottomTransform;
        private Transform pipeBodyTransform;

        public MiddlePipe(Transform pipeHeadTopTransform, Transform pipeHeadBottomTransform, Transform pipeBodyTransform) {
            this.pipeHeadTopTransform = pipeHeadTopTransform;
            this.pipeHeadBottomTransform = pipeHeadBottomTransform;
            this.pipeBodyTransform = pipeBodyTransform;
        }

        public void Move() {
            pipeHeadTopTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeHeadBottomTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition(){
            return pipeBodyTransform.position.x;
        }

        public void DestroySelf() {
            Destroy(pipeHeadTopTransform.gameObject);
            Destroy(pipeHeadBottomTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }

    }
}


