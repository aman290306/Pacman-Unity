using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;// Array of ghost GameObjects
    public Pacman pacman;// Pacman GameObject
    public Transform pellets;// Pellets Transform
    public int GhostMultiplier { get; private set; } = 1; // Multiplier for ghost points
    public int score { get; private set; } // Read-only property for score
    public int lives { get; private set; } // Read-only property for lives 
    public GameObject[] hearts;
    public TMP_Text scoreText; // Text component to display score
    public TMP_Text gameOverText; // Text component to display "Game Over" message
    public GameObject fruit;
    public GameObject deadPacman; // Dead Pacman GameObject
    // Only GameManager can modify score and lives.
    public AudioClip eatPelletSound; // Audio clips for various game events
    public AudioClip eatGhostSound; 
    public AudioClip deathSound;
    public AudioClip loadingSound; 
    public AudioClip fruitSound;
    public AudioClip powerPelletSound;
    public AudioClip[] ghostSirens;
    public AudioSource Power;
    public int Level {get; private set;} = 0;
    public AudioSource audiosource; // Audio source to play clips
    private void Start() // Unity method called on the frame when a script is enabled
    {
        NewGame();
    }

    private void Update() // Unity method called once per frame
    {
        if(this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }
    public void PlayAudioClip(AudioClip clip)
    {
       audiosource.PlayOneShot(clip);
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        for (int i = 0; i < ghosts.Length; i++)
        {
            this.ghosts[i].movement.speed = 7f;// Reset speed 
        }
        gameOverText.text="";
        PlayAudioClip(loadingSound);
        NewRound();
        Power.clip=ghostSirens[Level];
    }

    private void NewRound()
    {
        
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState(); 
    }

    private void ResetState() // pellets remain active
    {  
        ResetGhostMultiplier();
        deadPacman.gameObject.SetActive(false);
        for(int i = 0; i<ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();        
        }
        this.pacman.ResetState();
    }

    private void GameOver()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }
        Power.Stop();
        this.pacman.gameObject.SetActive(false);
        this.deadPacman.gameObject.SetActive(false);
        gameOverText.text = "Game Over";
        int finalScore = score;  // your GameManager Score variable

        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScore > savedHighScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save();
        }
    }

    public void SetScore(int score)
    {
        this.score = score;
        this.scoreText.text = "Score: " + this.score.ToString();
    }

    private void SetLives(int lives)
    {   
        this.lives = lives;
        if(lives==3){
            hearts[2].gameObject.SetActive(true);
            hearts[1].gameObject.SetActive(true);
            hearts[0].gameObject.SetActive(true);
        }
        else if(lives==2){
            hearts[2].gameObject.SetActive(true);
            hearts[1].gameObject.SetActive(true);
            hearts[0].gameObject.SetActive(false);
        }
        else if(lives==1){
            hearts[2].gameObject.SetActive(true);
            hearts[1].gameObject.SetActive(false);
            hearts[0].gameObject.SetActive(false);
        }
         if(lives==0){
            hearts[2].gameObject.SetActive(false);
            hearts[1].gameObject.SetActive(false);
            hearts[0].gameObject.SetActive(false);
        }
        
    }

    public void GhostEaten(Ghost ghost)
    {
        SetScore(this.score + ghost.points * this.GhostMultiplier);// Increase score by ghost's points
        this.GhostMultiplier++;// Increase multiplier for next ghost
        PlayAudioClip(eatGhostSound);
    }

    public void PacmanEaten()
    {
        
        deadPacman.gameObject.SetActive(true);
        deadPacman.transform.position = this.pacman.transform.position;
        deadPacman.GetComponent<AnimatedSprite>().ResetAnimation();
        PlayAudioClip(deathSound);
        this.pacman.gameObject.SetActive(false);
        SetLives(this.lives - 1);

        if(this.lives > 0){
            Invoke(nameof(ResetState), 2.0f);// Reset state after 3 seconds
            deadPacman.GetComponent<AnimatedSprite>().loop = true;
        }
        else{
            GameOver();
        }
    }
    
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);// Deactivate the pellet
        PlayAudioClip(eatPelletSound);
        SetScore(this.score + pellet.points);// Increase score by pellet's points

        if(!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            IncreaseDifficulty();
            Invoke(nameof(NewRound), 3.0f);// Start new round after 3 seconds
        }
    }
    private void IncreaseDifficulty()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            this.ghosts[i].movement.speed += 0.5f;// Increase speed of each ghost
        }
    }
    public void PowerPelletEaten(PowerPellet powerPellet)
    {   
        for (int i = 0; i < ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(powerPellet.duration);// Enable frightened mode for all ghosts
        }
      
        Power.clip=powerPelletSound;
        Power.Play();
        PelletEaten(powerPellet);
        CancelInvoke();// Cancel any existing invokes to retain ghost multiplier and increase duration.
        Invoke(nameof(ResetGhostMultiplier), powerPellet.duration);// Reset ghost multiplier after power pellet duration
        
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        Power.clip=ghostSirens[Level];
        Power.Play();

        this.GhostMultiplier = 1;
    }
    public void FruitEaten()
    {
        PlayAudioClip(fruitSound);
        SetScore(this.score + 100);
    }

}
