using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float startPos, length;
    public World world;
    public float parralaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = world.transform.position.x * (1-parralaxEffect);
        float dist = world.transform.position.x * parralaxEffect;

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log($"temp : {temp}, dist : {dist}");
        }

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        while(transform.position.x >= length)
        {
            transform.position -= Vector3.right * length;
        }
        while(transform.position.x <= -length)
        {
            transform.position += Vector3.right * length;
        }
    }
}
