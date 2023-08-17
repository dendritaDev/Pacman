using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMovement : MonoBehaviour
{
    public float speed = 0.4f;

    public Vector2 destination = Vector2.zero;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private AudioSource _audioSource;

    void Start()
    {
        destination = this.transform.position;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if(GameManager.sharedInstance.gameStarted && !GameManager.sharedInstance.gamePaused)
        {
            _audioSource.volume = 0.2f;

            //Calculamos el nuevo punto donde hay que ir en base a la variable destino
            Vector2 newPos = Vector2.MoveTowards(this.transform.position, destination, speed * Time.deltaTime);
            //Usamos el rigidbody para transportar a Pacman hasta dicha posición
            _rigidbody.MovePosition(newPos);


            float distanceToDestination = Vector2.Distance((Vector2)this.transform.position, destination);

            Debug.DrawLine(this.transform.position, destination);

            if (distanceToDestination < 2.0f)
            {

                if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && CanMoveTo(Vector2.up))
                {
                    destination = (Vector2)this.transform.position + Vector2.up;
                }

                if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && CanMoveTo(Vector2.right))
                {
                    destination = (Vector2)this.transform.position + Vector2.right;
                }

                if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && CanMoveTo(Vector2.down))
                {
                    destination = (Vector2)this.transform.position + Vector2.down;
                }

                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && CanMoveTo(Vector2.left))
                {
                    destination = (Vector2)this.transform.position + Vector2.left;
                }
            }

            Vector2 dir = destination - (Vector2)this.transform.position;

            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
        }
        else
        {
            _audioSource.volume = 0.0f;
        }


    }

    //Método que dada una posible dirección de movimiento
    //devuelve true si podemos ir en dicha dirección
    //y false si algo nos impide avanzar
    bool CanMoveTo(Vector2 dir)
    {
        Vector2 pacmanPos = this.transform.position;
        //Trazamos una línea del objetivo donde quiero ir hacia Pacman
        RaycastHit2D hit = Physics2D.Linecast(pacmanPos + dir, pacmanPos);

        Debug.DrawLine(pacmanPos, pacmanPos + dir);

        Collider2D pacmanCollider = _collider;
        Collider2D hitCollider = hit.collider;

        if (hitCollider == pacmanCollider)
        {
            //no tengo nada más enmedio -> puedo moverme allí
            return true;
        }
        else
        {
            //tengo un collider delante que NO es el de pacman -> no puedo moverme allí
            return false;
        }
    }
}
