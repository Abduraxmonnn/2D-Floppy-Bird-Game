using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;

    [Header("Audio")]
    [SerializeField] private AudioClip gameMusic;  // Game music clip
    [SerializeField] private AudioClip deathSound; // Death sound clip
    private AudioSource audioSource;               // AudioSource component to play music

    public int score { get; private set; } = 0;

    private enum GameState
    {
        Menu,
        Playing,
        GameOver
    }

    private GameState currentState;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        // Initially, set the game to the menu state
        currentState = GameState.Menu;
        PlayGameMusic(); // Game music starts when the game is ready
        Pause();
    }

    private void Update()
    {
        // If the game is in the "GameOver" state and music is not playing, restart game music.
        if (currentState == GameState.GameOver && !audioSource.isPlaying)
        {
            PlayGameMusic(); // Optionally you can restart the game music if needed.
        }
    }

    // Function to play the game music
    public void PlayGameMusic()
    {
        if (audioSource != null && gameMusic != null && currentState == GameState.Playing)
        {
            audioSource.clip = gameMusic;  // Set the game music clip
            audioSource.loop = true;       // Loop the game music
            audioSource.Play();            // Play the game music
        }
    }

    // Function to play the death sound
    public void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            Debug.Log("AudioSource: " + audioSource);  // Log AudioSource component
            Debug.Log("Death Sound Clip: " + deathSound);  // Log death sound clip
            audioSource.PlayOneShot(deathSound);  // Play the death sound
        }
        else
        {
            Debug.LogError("Death sound or AudioSource is not assigned!");
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        audioSource.Stop();
    }

    public void Play()
    {
        // Transition from Menu to Playing
        currentState = GameState.Playing;

        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        // Play the game music
        PlayGameMusic();

        // Clean up any existing pipes from previous game sessions
        Pipes[] pipes = Object.FindObjectsByType<Pipes>(FindObjectsSortMode.None);

        for (int i = 0; i < pipes.Length; i++) {
            Destroy(pipes[i].gameObject);
        }
    }

    public void GameOver()
    {
        // Transition from Playing to GameOver
        currentState = GameState.GameOver;

        // Play death sound and then show game over screen
        PlayDeathSound();

        playButton.SetActive(true);
        gameOver.SetActive(true);

        // Stop the game music and play the game over music (or menu music if you have one)
        audioSource.Stop();
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
