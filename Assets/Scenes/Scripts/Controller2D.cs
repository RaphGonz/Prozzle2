using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour
{

    public GameObject playerGameObject;
    private Player player;
    
    void Start()
    {
        player = playerGameObject.GetComponent<Player>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && player.canMove)
        {
            player.MoveRight();
            
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && player.canMove)
        {
            player.MoveLeft();

        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && player.canMove)
        {
            player.MoveUp();

        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && player.canMove)
        {
            player.MoveDown();

        }
    }

    public void ResetPlayer(GameObject newplayer) //sert à bien controller qui est le perso qu'on va utiliser lors du crossing over
    {
        playerGameObject = newplayer;
        player = newplayer.GetComponent<Player>();
    }
}
