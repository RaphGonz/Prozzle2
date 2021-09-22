using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{


    
    

    public GameObject bloc;

    public int nombreDeCouleurs;

    public int nbrSpawn;


    [HideInInspector]
    public int bloclayer;
    [HideInInspector]
    public int playerlayer;
    [HideInInspector]
    public int spawnlayer;

    public System.Random rnd = new System.Random();

    Color[] couleursTotal;
    Color[] couleurs;

    private GameManager gm;


    private void Start() //on assigne les bonnes valeurs
    {
        nombreDeCouleurs = Mathf.Clamp(nombreDeCouleurs, 4, 9); //Permettra de régler le nombre de couleur : pas en dessous de 4 pour cause de voisin

        couleursTotal = new Color[] { Color.red, Color.blue, Color.yellow, Color.green, Color.magenta, Color.cyan, Color.white, Color.grey, Color.black};

        couleurs = new Color[nombreDeCouleurs];
        for(int i = 0; i < nombreDeCouleurs; i++)
        {
            couleurs[i] = couleursTotal[i];
        }
        //on initialise le tableau des couleurs utilisables
        bloclayer = 1 << 9;
        playerlayer = 1 << 8;
        spawnlayer = bloclayer | playerlayer;

        gm = FindObjectOfType<GameManager>();

    }


    private void Update() //juste pour le test
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
            gm.score += 5;
        }
    }




    public List<Vector3> ListeSpawnPossible() //On regarde où on peut faire spawner
    {
        List<Vector3> res = new List<Vector3>();

        for(int i = -2; i < 3; i++) //remettre lim à 3 pour du 5*5 plateau
        {
            for(int j = -2; j < 3; j++) //remettre init à -2
            {
                
                Vector3 src = new Vector3(i, j,-0.1f);
                RaycastHit2D hit = Physics2D.Raycast(src + (Vector3.left * 0.5f), Vector3.right, 1.0f, spawnlayer);
                

                if (hit.collider == null)
                {
                   
                    res.Add(src);
                    
                }
                
            }
        }

        return res;
    }

    

    List<int> GenerateRandomIntList(int intMax, int nbr) //On créer une liste de int random tous différent
    {
        List<int> res = new List<int>();

        for (int k = 0; k < nbr; k++)
        {
            int i = rnd.Next(0, intMax);

            if (! res.Contains(i))
            {
                res.Add(i);
            }

        }
        return res;
    }

    List<Color> Voisins(Vector3 v)
    {
        List<Color> res = new List<Color>();

        for(int i = 0; i < nombreDeCouleurs; i++)
        {
            res.Add(couleurs[i]);
        }

        

        RaycastHit2D hit1 = Physics2D.Raycast(v + (Vector3.up * 0.5f), Vector3.up, 1.0f, bloclayer);

        RaycastHit2D hit2 = Physics2D.Raycast(v + (Vector3.right * 0.5f), Vector3.right, 1.0f, bloclayer);

        RaycastHit2D hit3 = Physics2D.Raycast(v + (Vector3.down * 0.5f), Vector3.down, 1.0f, bloclayer);

        RaycastHit2D hit4 = Physics2D.Raycast(v + (Vector3.left * 0.5f), Vector3.left, 1.0f, bloclayer);


        if (hit1.collider != null)
        {
            res.Remove(hit1.transform.gameObject.GetComponent<SpriteRenderer>().color);
            
        }
        if (hit2.collider != null)
        {
            res.Remove(hit2.transform.gameObject.GetComponent<SpriteRenderer>().color);
            
        }
        if (hit3.collider != null)
        {
            res.Remove(hit3.transform.gameObject.GetComponent<SpriteRenderer>().color);

        }
        if (hit4.collider != null)
        {
            res.Remove(hit4.transform.gameObject.GetComponent<SpriteRenderer>().color);
        }

        return res;
    }



    public void Spawn() //On fait apparaitre des bloc de couleur random aux endroit select au hasard et on détruit autour de nous ay cas ou
    {
        List<Vector3> listeSpawnPossible = ListeSpawnPossible();

        int n = listeSpawnPossible.Count;
        if (n != 0)
        {
            int N;
            if (n != 0)
            {
                N = Mathf.Clamp(n, 1, nbrSpawn); //le nombre de bloc qu'on va spawn
            }
            else
            {
                N = 0;
            }


            List<int> listIndex = GenerateRandomIntList(n, N);
            List<Vector3> listSpawn = new List<Vector3>();

            foreach (int i in listIndex)
            {
                listSpawn.Add(listeSpawnPossible[i]); //on choisi N parmis les n dispo
            }

            foreach (Vector3 v in listSpawn)
            {
                List<Color> listCouleursAutor = Voisins(v);
                int r = rnd.Next(0, listCouleursAutor.Count);

                GameObject newbloc = Instantiate(bloc, v, Quaternion.identity);
                newbloc.GetComponent<SpriteRenderer>().color = listCouleursAutor[r];


            }
        }
        else
        {
            transform.gameObject.GetComponent<GameManager>().GameOver();

        }


    }


}
