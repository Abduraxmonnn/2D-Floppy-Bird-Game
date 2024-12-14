using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;  // Reference to the AudioSource component
    [SerializeField] private AudioClip menuMusic;      // Background music for the menu
    [SerializeField] private AudioClip gameModeMusic;  // Background music (game mode song)
    [SerializeField] private AudioClip deathSound;     // Death sound (played when the player dies)

    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;

    public int score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        PlayMenuMusic();  // Play the menu music when the game starts
        Pause();          // Pause the game at the beginning
    }

    // Play the menu music (before the game starts)
    public void PlayMenuMusic()
    {
        if (audioSource != null && menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;  // Loop the music during the menu screen
            audioSource.Play();       // Play the menu music
        }
        else
        {
            Debug.LogError("AudioSource or menuMusic is not assigned!");
        }
    }

    // Start playing the background music (game music)
    public void PlayGameMusic()
    {
        if (audioSource != null && gameModeMusic != null)
        {
            audioSource.clip = gameModeMusic;
            audioSource.loop = true;  // Loop the music during gameplay
            audioSource.Play();       // Play the background music
        }
        else
        {
            Debug.LogError("AudioSource or gameModeMusic is not assigned!");
        }
    }

    // Play the death sound when the player dies
    public void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.Stop();               // Stop the current music (menu or game music)
            audioSource.PlayOneShot(deathSound); // Play the death sound (one-shot)
        }
        else
        {
            Debug.LogError("AudioSource or deathSound is not assigned!");
        }
    }

    // Pause the game (stop time and disable player control)
    public void Pause()
    {
        Time.timeScale = 0f;  // Stop game time (freeze the game)
        player.enabled = false;  // Disable player control
    }

    // Start the game
    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);  // Hide the play button
        gameOver.SetActive(false);    // Hide the game over screen

        Time.timeScale = 1f;  // Resume game time
        player.enabled = true;  // Enable player control

        // Destroy any remaining pipes from previous games
        Pipes[] pipes = Object.FindObjectsByType<Pipes>(FindObjectsSortMode.None);
        foreach (var pipe in pipes) {
            Destroy(pipe.gameObject);
        }

        PlayGameMusic();  // Start the background music (game music)
    }

    // Game Over (end the game)
    public void GameOver()
    {
        playButton.SetActive(true);  // Show the play button
        gameOver.SetActive(true);    // Show the game over screen

        Pause();  // Pause the game
        PlayDeathSound();  // Play the death sound when the player dies
    }

    // Increase the score and update the score display
    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
