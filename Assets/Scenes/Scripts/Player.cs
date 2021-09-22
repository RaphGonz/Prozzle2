using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject controllerGameObject;
    private Controller2D controller;

    private Vector3 destination;
    private int jump = 5;

    public float speed;

    private bool moving;
    public bool canMove;
    public bool autoDestruct = false; //va servir dans le crossing over

    public bool gameOver = false; //va empêcher le joueur de bouger en cas de gameOver;

    public LayerMask groundMask;
    public LayerMask blocMask;
    
    
    void Awake()
    {
        destination = transform.position;
        controller = controllerGameObject.GetComponent<Controller2D>();
        moving = false;
        canMove = false;
    }

    
    void Update()
    {
        
        if (moving)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            canMove = false;
        }

        if (transform.position == destination)
        {
            moving = false;

            canMove = !gameOver; //si il y a pas de gameover, alors il peut bouger

            if (autoDestruct)
            {
                Destroy(transform.gameObject);
            }
            
        }
    }


    public void MoveRight()
    {
        
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position +( Vector3.right * 0.5f ), Vector2.right, 1.0f, groundMask);
        if (hitGround.collider != null)
        {
            destination = hitGround.transform.position;
            moving = true;
        }
        else
        {
            
            GameObject newplyrGameObject = Instantiate(transform.gameObject, transform.position - (jump * Vector3.right), Quaternion.identity);
            //On créer un nouveau joueur de l'autre coté du plateau
            newplyrGameObject.name = transform.gameObject.name;


            Player newplyr = newplyrGameObject.GetComponent<Player>();
            newplyr.MoveRight();
            //On bouge ce nouveau joueur dans la direction voulue

            controller.ResetPlayer(newplyrGameObject);

            //On change le joueur dans le controlleur pour mtn diriger le nouveau qu'on a créé

            destination = transform.position + Vector3.right;
            moving = true;
            autoDestruct = true;
            //On destine cet object à aller à droite et s'autodetruire une fois la destination atteinte

        }
        RaycastHit2D hitBloc = Physics2D.Raycast(transform.position + (Vector3.right * 0.5f), Vector2.right, 1.0f, blocMask);
        if (hitBloc.collider != null)
        {
            Bloc bloc = hitBloc.transform.gameObject.GetComponent<Bloc>();
            bloc.MoveRight();
        }


    }

    public void MoveLeft()
    {

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position + (Vector3.left * 0.5f), Vector2.left, 1.0f, groundMask);
        if (hitGround.collider != null)
        {
            destination = hitGround.transform.position;
            moving = true;
        }
        else
        {

            GameObject newplyrGameObject = Instantiate(transform.gameObject, transform.position - (jump * Vector3.left), Quaternion.identity);
            
            newplyrGameObject.name = transform.gameObject.name;


            Player newplyr = newplyrGameObject.GetComponent<Player>();
            newplyr.MoveLeft();
            

            controller.ResetPlayer(newplyrGameObject);

            

            destination = transform.position + Vector3.left;
            moving = true;
            autoDestruct = true;
           

        }
        RaycastHit2D hitBloc = Physics2D.Raycast(transform.position + (Vector3.left * 0.5f), Vector2.left, 1.0f, blocMask);
        if(hitBloc.collider != null)
        {
            Bloc bloc = hitBloc.transform.gameObject.GetComponent<Bloc>();
            bloc.MoveLeft();
        }


    }

    public void MoveUp()
    {

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), Vector2.up, 1.0f, groundMask);
        if (hitGround.collider != null)
        {
            destination = hitGround.transform.position;
            moving = true;
        }
        else
        {

            GameObject newplyrGameObject = Instantiate(transform.gameObject, transform.position - (jump * Vector3.up), Quaternion.identity);

            newplyrGameObject.name = transform.gameObject.name;


            Player newplyr = newplyrGameObject.GetComponent<Player>();
            newplyr.MoveUp();


            controller.ResetPlayer(newplyrGameObject);



            destination = transform.position + Vector3.up;
            moving = true;
            autoDestruct = true;


        }
        RaycastHit2D hitBloc = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), Vector2.up, 1.0f, blocMask);
        if (hitBloc.collider != null)
        {
            Bloc bloc = hitBloc.transform.gameObject.GetComponent<Bloc>();
            bloc.MoveUp();
        }

    }

    public void MoveDown()
    {

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position + (Vector3.down * 0.5f), Vector2.down, 1.0f, groundMask);
        if (hitGround.collider != null)
        {
            destination = hitGround.transform.position;
            moving = true;
        }
        else
        {

            GameObject newplyrGameObject = Instantiate(transform.gameObject, transform.position - (jump * Vector3.down), Quaternion.identity);

            newplyrGameObject.name = transform.gameObject.name;


            Player newplyr = newplyrGameObject.GetComponent<Player>();
            newplyr.MoveDown();


            controller.ResetPlayer(newplyrGameObject);



            destination = transform.position + Vector3.down;
            moving = true;
            autoDestruct = true;


        }
        RaycastHit2D hitBloc = Physics2D.Raycast(transform.position + (Vector3.down * 0.5f), Vector2.down, 1.0f, blocMask);
        if (hitBloc.collider != null)
        {
            Bloc bloc = hitBloc.transform.gameObject.GetComponent<Bloc>();
            bloc.MoveDown();
        }

    }



}
