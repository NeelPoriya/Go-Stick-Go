using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float increaseStickHeightSpeed = 2f;
    [SerializeField] CheckWin checkWin;
    bool takeInput = true;
    public bool TakeInput { get { return takeInput; } }
    [SerializeField] Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player.stopInputs) return;
        if (Input.GetMouseButton(0) && takeInput)
        {
            transform.localScale = transform.localScale + new Vector3(0f, increaseStickHeightSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetMouseButtonUp(0) && takeInput)
        {
            takeInput = false;
            StartCoroutine(TurnStick());
        }
    }

    IEnumerator TurnStick()
    {
        float percent = 0f;
        while(percent < 1f)
        {
            transform.eulerAngles = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -90f), percent);
            percent += Time.deltaTime * rotationSpeed;
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = new Vector3(0f, 0f, -90f);
        checkWin.Activated = true;
        Invoke("MovePlayerWithDelay", 0.1f);
    }

    void MovePlayerWithDelay()
    {
        StartCoroutine(player.MovePlayer());
    }

    void ResetStick()
    {
        transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        takeInput = true;
    }
}
