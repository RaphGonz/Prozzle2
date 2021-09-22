using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int level;
    public int score;
    public int timeAmount;

    private Spawner sp;

    public GameObject scoreUI;
    public GameObject levelUI;
    public GameObject chronoUI;
    public GameObject gameoverUI;



    private void Start()
    {
        sp = transform.gameObject.GetComponent<Spawner>(); //pour moins écrire et rendre pluslisible

        level = 10 - timeAmount + 1;
        levelUI.GetComponent<Text>().text = "level : " + level;
        score = 0;
        scoreUI.GetComponent<Text>().text = "score : " + score;
        gameoverUI.GetComponent<Text>().enabled = false;
        StartCoroutine("Chrono");
        StartCoroutine("TextUpdate");

        int tmp = sp.nbrSpawn;
        sp.nbrSpawn = 25; //On va remplir presque complètement le plateau
        sp.Spawn();
        sp.nbrSpawn = tmp;
        
        

    }

    IEnumerator TextUpdate()
    {
        for (; ; )
        {
            scoreUI.GetComponent<Text>().text = "score : " + score;
            levelUI.GetComponent<Text>().text = "level : " + level;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GameOver()
    {
        print("GAMEOVER");
        StopCoroutine("Chrono");
        StopCoroutine("TextUpdate");
        FindObjectOfType<Player>().gameOver = true;
        gameoverUI.GetComponent<Text>().enabled = true;

    }

    IEnumerator Chrono()
    {
       
        for (; ; )
        {
            int t = timeAmount;

            

            for (int i = t; i > 0; i--)
            {
                chronoUI.GetComponent<Text>().text = "Spawn dans " + i + " secondes";
                yield return new WaitForSeconds(1f); //On descend le chrono
            }


            while (!FindObjectOfType<Player>().canMove) //On attend que le joueur soir sur une case avant de lancer un spawn pour eviter des spawns de bloc sur le joeur
            {
                yield return new WaitForSeconds(0.01f);
            }
            sp.Spawn();

            if (score >= 20 * level && timeAmount > 1) //on dessus d'un certain seuil on passe au niveau suivant
            {
                level += 1;
                timeAmount -= 1;
            }
        }

    }
}
