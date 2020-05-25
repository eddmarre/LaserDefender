using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    void Start()
    {

    }


    void Update()
    {
        Move();
    }

    void Move()
    {

        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = transform.position.x + deltaX;

        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var nexYPos = transform.position.y + deltaY;
        transform.position = new Vector2(newXPos, nexYPos);
    }
}
