using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWin : MonoBehaviour
{
    [SerializeField] bool hitIndeed = false;
    bool activated = false;
    public bool Activated { get { return activated; } set { activated = value; } }
    public bool HitIndeed {  get { return hitIndeed; } }
    Transform nextGround;
    public Transform NextGround { get { return nextGround; } }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            hitIndeed = true;
            nextGround = collision.transform;
        }

        if(collision.name == "Perfection" && activated)
        {
            FindObjectOfType<ScoreBoard>().IncrementScore();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Platform"))
            hitIndeed = false;
    }
}
