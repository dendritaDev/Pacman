using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public bool gameStarted = false;
    public bool gamePaused = false;

    public AudioClip pauseAudio;
    private AudioSource _audioSource;

    public float invincibleTime = 0.0f;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        StartCoroutine("StartGame");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            gamePaused = !gamePaused;
            if(gamePaused)
            {
                PlayPauseMusic();
            }
            else
            {
                StopPauseMusic();
            }
        }

        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();

        if(invincibleTime > 0 )
        {
            invincibleTime -= Time.deltaTime;
        }
    }
    
    void PlayPauseMusic()
    {
        _audioSource.clip = pauseAudio;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    void StopPauseMusic()
    {
        _audioSource.Stop();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(4.0f); //el tiempo que dura la musica de inicio
        gameStarted = true;
    }

    public  void MakeInvincibleFor(float numerOfSeconds)
    {
        this.invincibleTime += numerOfSeconds;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMapScene");
    }
}
