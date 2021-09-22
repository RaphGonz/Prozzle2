using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{


    private Vector3 destination;
    private int jump = 5; //pour la distance de respawn de l'objet quand cross over = juste pour tester

    public float speed;

    public int seuil = 3;

    private bool moving;
    public bool autoDestruct = false; //va servir dans le crossing over

    public LayerMask groundMask;
    public LayerMask blocMask;

    public Vector3 lastDir;//On va mémoriser la direction qu'a pris le bloc quand il a été poussé pour aider sa destruction ou non
                           // C'est la direction dans laquelle le bloc a bougé
    

    public int blocScore; //Va permettre de compter le nombre de descendants de la même couleur
    public bool tryDestruct = false; //permet de savoir quand tenter la destruction (ici quand le bloc est immobile
    private IEnumerator coroutine;


    public GameManager gm;

    void Awake()
    {
        destination = transform.position;
        moving = false;
        gm = FindObjectOfType<GameManager>();
    }


    void Update()
    {

        if (moving)
        {

            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }

        if (transform.position == destination)
        {
            moving = false;

            if (autoDestruct)
            {
                Destroy(transform.gameObject);
            }
            if (tryDestruct)
            {
                coroutine = TryDestroyBloc(lastDir);
                StartCoroutine(coroutine);
                tryDestruct = false;
            }

        }
    }


    public void MoveRight()
    {

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position + (Vector3.right * 0.5f), Vector2.right, 1.0f, groundMask);
        if (hitGround.collider != null)
        {
            destination = hitGround.transform.position;
            moving = true;
            lastDir = Vector3.right;
            tryDestruct = true;
        }
        else
        {

            GameObject newBlocGameObject = Instantiate(transform.gameObject, transform.position - (jump * Vector3.right), Quaternion.identity);

            newBlocGameObject.name = transform.gameObject.name;


            Bloc newBloc = newBlocGameObject.GetComponent<Bloc>();
            newBloc.MoveRight();

            destination = transform.position + Vector3.right;
            moving = true;
            autoDestruct = true;


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
            lastDir = Vector3.left;
            tryDestruct = true;
            moving = true;
        }
        else
        {

            GameObject newBlocGameObject = Instantiate(transform.gameObject, transform.position - (jump * Vector3.left), Quaternion.identity);

            newBlocGameObject.name = transform.gameObject.name;


            Bloc newBloc = newBlocGameObject.GetComponent<Bloc>();
            newBloc.MoveLeft();



            destination = transform.position + Vector3.left;
            moving = true;
            autoDestruct = true;


        }
        RaycastHit2D hitBloc = Physics2D.Raycast(transform.position + (Vector3.left * 0.5f), Vector2.left, 1.0f, blocMask);
        if (hitBloc.collider != null)
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
            lastDir = Vector3.up;
            tryDestruct = true;
        }
        else
        {

            GameObject newBlocGameObject = Instantiate(transform.gameObject, transform.position - (jump * Vector3.up), Quaternion.identity);

            newBlocGameObject.name = transform.gameObject.name;


            Bloc newBloc = newBlocGameObject.GetComponent<Bloc>();
            newBloc.MoveUp();


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
            lastDir = Vector3.down;
            tryDestruct = true;
        }
        else
        {

            GameObject newBlocGameObject = Instantiate(transform.gameObject, transform.position - (jump * Vector3.down), Quaternion.identity);

            newBlocGameObject.name = transform.gameObject.name;


            Bloc newBloc = newBlocGameObject.GetComponent<Bloc>();
            newBloc.MoveDown();


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

    IEnumerator TryDestroyBloc(Vector3 dir) //On va parcourir les blocs adjacents en profondeur et essayer de les détruire
    {
        blocScore = CountScore(dir);
        yield return new WaitForSeconds(0.1f); //faire comme ça permet de laisser tous les blocs calculer leur score avant de les détruire -> plus de bug de superposition de score
        KillOrReset(blocScore, dir);

    }

    public int CountScore(Vector3 dir)
    {


        Vector3 dir1 = dir;
        Vector3 dir2 = Vector3.Cross(dir, Vector3.forward);
        Vector3 dir3 = Vector3.Cross(dir, Vector3.back); //Permet d'avoir la direction vers l'avant et les 2 sur le coté droit et gauche

        int c1 = 0;
        int c2 = 0;
        int c3 = 0;

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + (dir1 * 0.5f), dir1, 1.0f, blocMask);

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + (dir2 * 0.5f), dir2, 1.0f, blocMask);

        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + (dir3 * 0.5f), dir3, 1.0f, blocMask);


        if (hit1.collider != null && hit1.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
        {
            c1 = hit1.transform.gameObject.GetComponent<Bloc>().CountScore(dir1);
        }
        if (hit2.collider != null && hit2.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
        {
            c2 = hit2.transform.gameObject.GetComponent<Bloc>().CountScore(dir2);
        }
        if (hit3.collider != null && hit3.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
        {
            c3 = hit3.transform.gameObject.GetComponent<Bloc>().CountScore(dir3);
        }



        return 1 + c1 + c2 + c3;

        //On lance un rayon pour si on touche qquch dans chaque direction et si oui si c'est de la même couleur que nous
        //et si c'est le cas on continue de parcourir à partir de ce bloc

    }

    public void KillOrReset(int score, Vector3 dir)
    {


        Vector3 dir1 = dir;
        Vector3 dir2 = Vector3.Cross(dir, Vector3.forward);
        Vector3 dir3 = Vector3.Cross(dir, Vector3.back); //Permet d'avoir la direction vers l'avant et les 2 sur le coté droit et gauche


        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + (dir1 * 0.5f), dir1, 1.0f, blocMask);
        Debug.DrawRay(transform.position + (dir1 * 0.5f), dir1, Color.green, 1f);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + (dir2 * 0.5f), dir2, 1.0f, blocMask);
        Debug.DrawRay(transform.position + (dir2 * 0.5f), dir2, Color.green, 1f);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + (dir3 * 0.5f), dir3, 1.0f, blocMask);
        Debug.DrawRay(transform.position + (dir3 * 0.5f), dir3, Color.green, 1f);


        if (score >= seuil)
        {
            if (hit1.collider != null && hit1.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
            {
                hit1.transform.gameObject.GetComponent<Bloc>().KillOrReset(score, dir1);
            }
            if (hit2.collider != null && hit2.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
            {
                hit2.transform.gameObject.GetComponent<Bloc>().KillOrReset(score, dir2);
            }
            if (hit3.collider != null && hit3.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
            {
                hit3.transform.gameObject.GetComponent<Bloc>().KillOrReset(score, dir3);
            }

            gm.score += score;

            Destroy(transform.gameObject);
        }
        else
        {
            if (hit1.collider != null && hit1.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
            {
                hit1.transform.gameObject.GetComponent<Bloc>().KillOrReset(0, dir1);
            }
            if (hit2.collider != null && hit2.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
            {
                hit2.transform.gameObject.GetComponent<Bloc>().KillOrReset(0, dir2);
            }
            if (hit3.collider != null && hit3.transform.gameObject.GetComponent<SpriteRenderer>().color == transform.gameObject.GetComponent<SpriteRenderer>().color)
            {
                hit3.transform.gameObject.GetComponent<Bloc>().KillOrReset(0, dir3);
            }

            transform.gameObject.GetComponent<Bloc>().blocScore = 0;
        }
        //On va parcourir notre arbre de bloc avec le score de la racine en tête
        //Si le score est >=3 alors on va lancer la fonction dans chaque voisins de même couleur avant de s'autodétruire
        //sinon on remet le score de ce bloc à 0
        //peut être que le lancement simultané de chaque bloc est pas bonne ? pcq yaura ptet des modif en surcouche
        //qui vont tout faire merde : vérifier lorsqu'un bloc bouge si le bouge qu'il déplace est de la même couleur ou
        //non et lancer TryDestroy en fonction ( non si meme couleur et oui si couleur !=)


    }
}

