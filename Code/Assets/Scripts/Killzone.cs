using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Body" || collision.name == "Perfection" || collision.name == "StickEnd")
            Destroy(collision.transform.parent.gameObject);
        Destroy(collision.gameObject);
    }
}
