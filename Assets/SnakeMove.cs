using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

enum currentDir
{
    right, left, up, down
}
public class SnakeMove : MonoBehaviour
{
    // Start is called before the first frame update
    currentDir currentDir = currentDir.right;

    private Vector2 dir = Vector2.left;
    private float time = 0;

    public List<Transform> ListTail = new();

    public float timeToRun = 0.25f;

    public Transform snakeTailPrefab;

    // Update is called once per frame
    void Update()
    {
        KillTail();

        if (Time.time >= time)
        {
            Move();
            time += timeToRun;
        }

        if (Input.GetKey(KeyCode.D) && currentDir != currentDir.right)
        {
            dir = Vector2.right;
            currentDir = currentDir.left;
        }
        else if (Input.GetKey(KeyCode.S) && currentDir != currentDir.down)
        {
            dir = Vector2.down;
            currentDir = currentDir.up;
        }// '-up' means 'down'
        else if (Input.GetKey(KeyCode.A) && currentDir != currentDir.left)
        {
            dir = Vector2.left;
            currentDir = currentDir.right;
        }
        // '-right' means 'left'
        else if (Input.GetKey(KeyCode.W) && currentDir != currentDir.up)
        {
            dir = Vector2.up;
            currentDir = currentDir.down;
        }
    }


    void KillTail()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Transform trans = ListTail[^1];
            ListTail.Remove(trans.transform);
            Destroy(trans.gameObject);


        }
    }
    void Move()
    {

        // Move head into new direction (now there is a gap)

        Vector2 posBeforeMove = transform.position;

        transform.Translate(dir);

        // Do we have a Tail?
        if (ListTail.Count > 0)
        {
            // Move last Tail Element to where the Head was
            ListTail.Last().position = posBeforeMove;

            Debug.Log("move tail");


            // Add to front of list, remove from the back
            ListTail.Insert(0, ListTail.Last());

            ListTail.RemoveAt(ListTail.Count -1);
        }
    }

    private void AddTails()
    {
        Transform trans = Instantiate(snakeTailPrefab, ListTail.Last().position, Quaternion.identity).transform;
        ListTail.Add(trans);
        Debug.Log("add tail");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            AddTails();
        }
    }

    
}
