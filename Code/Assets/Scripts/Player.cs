using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Transform stickEnd;
    bool isGrounded = true;
    [SerializeField] float backOffset = 0.5f;
    [SerializeField] GameObject stickPrefab;
    [SerializeField] Transform StickSpawnPoint;
    CheckWin checkWin;
    ScoreBoard scoreBoard;
    Vector3 nextGround;
    Vector3 finalPos;
    GameObject stick;
    bool isTravelling = false, isInverted = false;
    [SerializeField] float downY = 2f;
    [SerializeField] bool dead = false;
    private bool stopLerping = false;
    World world;
    bool foundCherry = false;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public LayerMask layerMask;
    public bool stopInputs = false;
    public GameObject EndScreen;
    public Text EndScreenScoreText;

    // Start is called before the first frame update
    void Start()
    {
        stick = Instantiate(stickPrefab, StickSpawnPoint.position, Quaternion.identity);
        stick.transform.parent = transform.parent;
        stickEnd = stick.transform.Find("StickEnd");
        checkWin = stickEnd.GetComponent<CheckWin>();

        scoreBoard = FindObjectOfType<ScoreBoard>();
        world = FindObjectOfType<World>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTravelling && !isGrounded && !stick.GetComponent<Stick>().TakeInput && Input.GetMouseButtonDown(0) && !stopInputs)
        {
            if (!isInverted)
            {
                spriteRenderer.flipY = true;
                transform.position = new Vector3(transform.position.x, transform.position.y - downY, transform.position.z);
                GetComponent<Rigidbody2D>().gravityScale = 0f;
                isInverted = true;
            }
            else
            {
                spriteRenderer.flipY = false;
                transform.position = new Vector3(transform.position.x, transform.position.y + downY, transform.position.z);
                GetComponent<Rigidbody2D>().gravityScale = 50f;
                isInverted = false;
            }
        }

        if (!isTravelling && !checkGround())
        {
            if (!isGrounded && isInverted)
            {
                stopLerping = true;
                //collider.GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Rigidbody2D>().gravityScale = 50f;

            }
        }
        if (!dead)
            Die();
    }

    private void Die()
    {
        if (transform.position.y < -16f)
        {
            dead = true;
            CameraShaker.Instance.ShakeOnce(4f, 5f, 0.1f, 1f);
            EndScreenScoreText.text = scoreBoard.score.ToString();
            EndScreen.SetActive(true);
        }
        if (!dead) return;
        world.totalCherries += world.Cherries;
        SaveSystem.SavePlayer(world);
    }

    public IEnumerator MovePlayer()
    {
        float travelPercent = 0f;
        Vector3 initialPos = transform.position;
        Vector3 initialPosInverted = new Vector3(transform.position.x, transform.position.y - downY, transform.position.z);
        finalPos = new Vector3(stickEnd.position.x, transform.position.y, transform.position.z);
        Vector3 finalPosInverted = new Vector3(stickEnd.position.x, transform.position.y - downY, transform.position.z);
        float distance = Vector3.Distance(finalPos, initialPos);

        if (checkWin.HitIndeed)
        {
            float finalX = checkWin.NextGround.position.x + checkWin.NextGround.localScale.x / 2 - backOffset;
            finalPos = new Vector3(finalX, transform.position.y, transform.position.z);
            finalPosInverted = new Vector3(finalX, transform.position.y - downY, transform.position.z);
        }

        isTravelling = true;
        while (travelPercent < 1f)
        {
            animator.SetBool("walk", true);
            if (!isInverted)
                transform.position = Vector3.Lerp(initialPos, finalPos, travelPercent);
            else
                transform.position = Vector3.Lerp(initialPosInverted, finalPosInverted, travelPercent);

            if (stopLerping)
            {
                animator.SetBool("walk", false);
                yield break;
            }

            travelPercent += Time.deltaTime * speed * 1 / distance;
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("walk", false);


        if (stopLerping)
        {
            yield break;
        }

        if (isInverted)
        {
            GetComponent<Rigidbody2D>().gravityScale = 50f;
            stick.GetComponent<BoxCollider2D>().enabled = false;
            yield break;
        }
        else
        {
            stick.GetComponent<BoxCollider2D>().enabled = false;
            transform.position = finalPos;
        }

        isTravelling = false;
        if (!isGrounded)
        {
            stick.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            if (foundCherry)
            {
                foundCherry = false;
                world.AddCherries(1);
            }
            scoreBoard.IncrementScore();

            float deltaX = transform.position.x - initialPos.x;
            StartCoroutine(FindObjectOfType<World>().MoveWorld(deltaX));

            stick = Instantiate(stickPrefab, StickSpawnPoint.position, Quaternion.identity);
            stick.transform.parent = transform.parent;
            stickEnd = stick.transform.Find("StickEnd");
            checkWin = stickEnd.GetComponent<CheckWin>();
        }
    }

    bool checkGround()
    {
        RaycastHit2D[] results;
        return Physics2D.Raycast(transform.position, Vector2.down, 2f, layerMask);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            if (!isInverted)
                isGrounded = true;
        }
    }
    public void StopMoving(Collider2D collider)
    {
        //if (isInverted)
        {
            stopLerping = true;
            collider.GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 50f;
            stopInputs = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = false;
            stopLerping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cherry") && !dead && isInverted && !isGrounded)
        {
            foundCherry = true;
            Destroy(collision.gameObject);
        }
    }

}
