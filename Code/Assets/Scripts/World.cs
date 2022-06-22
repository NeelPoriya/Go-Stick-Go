using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    [SerializeField] GameObject cherryPrefab;
    [SerializeField] Transform platformSpawnPoint;
    [SerializeField] int cherries = 0;
    public int totalCherries;
    int highscore;
    public int Cherries { get { return cherries; } set { cherries = value; } }
    GameObject nextPlatform, nextCherry;
    Transform platformsGameObject;
    Transform previousPlatform;
    ScoreBoard board;

    public int GetScore()
    {
        return Mathf.Max(board.score, highscore);
    }

    private void Start()
    {
        board = GetComponent<ScoreBoard>();
        platformsGameObject = transform.Find("Platforms");
        PlayerData data = SaveSystem.LoadPlayer();
        if(data != null)
        {
            cherries = 0;
            totalCherries = data.cherries;
            highscore = data.highscore;
        }
        else
        {
            cherries = 0;
            totalCherries = 0;
            SaveSystem.SavePlayer(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void CreateNextPlatform()
    {
        if(nextPlatform)
            previousPlatform = nextPlatform.transform;
        nextPlatform = Instantiate(platformPrefab, platformSpawnPoint.transform.position, Quaternion.identity);
        nextPlatform.transform.parent = platformsGameObject.transform;
        Transform platformBody = nextPlatform.transform.Find("Body");
        platformBody.localScale = new Vector3(Random.Range(1f, 3f), platformBody.localScale.y, platformBody.localScale.z);

        if (Random.value <= 0.75f && previousPlatform != null)
        {
            //Spawn a Cherry between the two platforms
            Vector2 spawnPoint = new Vector2(platformSpawnPoint.transform.position.x, cherryPrefab.transform.position.y + transform.position.y);
            nextCherry = Instantiate(cherryPrefab, spawnPoint, cherryPrefab.transform.rotation, transform);
        }
        else
        {
            nextCherry = null;
        }
    }
    public IEnumerator MoveWorld(float x)
    {
        CreateNextPlatform();
        Vector3 nextPlatformInitialPos = nextPlatform.transform.position;
        Vector3 nextPlatformNewPlace = new Vector3(Random.Range(-4f, 7f), nextPlatform.transform.position.y, nextPlatform.transform.position.z);


        Vector3 initialPos = transform.position;
        Vector3 finalPos = transform.position - new Vector3(x, 0f, 0f);

        float xMin, xMax;
        Vector2 cherryInitialPos = Vector2.zero, cherryFinalPos = Vector2.zero;
        if (nextCherry)
        {
            xMin = previousPlatform.localPosition.x + previousPlatform.Find("Body").GetComponent<BoxCollider2D>().bounds.extents.x + nextCherry.GetComponent<CircleCollider2D>().bounds.extents.x;
            xMax = nextPlatformNewPlace.x - finalPos.x - nextPlatform.transform.Find("Body").GetComponent<BoxCollider2D>().bounds.extents.x - nextCherry.GetComponent<CircleCollider2D>().bounds.extents.x;
            
            cherryInitialPos = nextCherry.transform.localPosition;
            cherryFinalPos = new Vector2(Random.Range(xMin, xMax), cherryPrefab.transform.localPosition.y);
        }
        

        float travelPercent = 0f;
        while(travelPercent < 1f)
        {
            transform.position = Vector3.Lerp(initialPos, finalPos, travelPercent);
            nextPlatform.transform.position = Vector3.Lerp(nextPlatformInitialPos, nextPlatformNewPlace, travelPercent);
            if(nextCherry)
                nextCherry.transform.localPosition = Vector2.Lerp(cherryInitialPos, cherryFinalPos, travelPercent);
            travelPercent += Time.deltaTime * 1.5f;
            yield return new WaitForEndOfFrame();
        }

        transform.position = finalPos;
    }

    public void AddCherries(int amount)
    {
        cherries += amount;
    }

    public void removeCherries(int amount)
    {
        if(amount > cherries)
        {
            cherries -= amount;
        }
    }
}
