using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public Transform[] waypoints;
    int currentWaypoint = 0;

    public float speed = 0.3f;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;

    public bool shouldWaitHome = false;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(GameManager.sharedInstance.invincibleTime>0)
        {
            _animator.SetBool("PacmanInvincible", true);
        }
        else
        {
            _animator.SetBool("PacmanInvincible", false);
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.gameStarted && !GameManager.sharedInstance.gamePaused)
        {
            _audioSource.volume = 0.05f;

            if(!shouldWaitHome)
            {
                float distanceToWaypoint = Vector2.Distance((Vector2)this.transform.position,
                                                    (Vector2)waypoints[currentWaypoint].position);

                if (distanceToWaypoint < 0.1f)
                {
                    currentWaypoint = (currentWaypoint + 1) % waypoints.Length; //si ha llegado al final del array, vuelve al 0.
                    Vector2 newDirection = waypoints[currentWaypoint].position - this.transform.position;
                    _animator.SetFloat("DirX", newDirection.x);
                    _animator.SetFloat("DirY", newDirection.y);

                }
                else
                {
                    Vector2 newPos = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);
                    _rigidbody.MovePosition(newPos);
                }
            }

            
        }
        else
        {
            GetComponent<AudioSource>().volume = 0.0f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(GameManager.sharedInstance.invincibleTime<=0)
            {
                GameManager.sharedInstance.gameStarted = false;
                Destroy(collision.gameObject);

                StartCoroutine("RestartGame");
            }
            else
            {
                UIManager.sharedInstance.ScorePoints(50);
                GameObject home = GameObject.Find("GhostHome");
                this.transform.position = home.transform.position;
                this.currentWaypoint = 0;
                this.shouldWaitHome = true;
                StartCoroutine("AwakeFromHome");
            }
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        GameManager.sharedInstance.RestartGame();
    }

    IEnumerator AwakeFromHome()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        this.shouldWaitHome = false;
        this.speed *= 1.2f; //cada vez que el fantasma despierta, es un 20% más rápido.
    }
}
