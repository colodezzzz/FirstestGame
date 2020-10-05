using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject parent;
    public GameObject Parent { set { parent = value; } get { return parent; } }

    private bool characterBullet = false; //variable checking parent, if it is Character then it destroy other objects with class Unit else does not destroy other Units, only Character
    public bool CharacterBullet { set { characterBullet = value; } get { return characterBullet; } }

    private float speed = 10f;

    private Vector3 direction;
    public Vector3 Direction
    {
        set { direction = value; }
    }

    private void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Hello");

        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit.gameObject != parent)
        {
            if (unit is Character)
            {
                unit.RecieveDamage(unit.transform.position.x - transform.position.x);
                Destroy(gameObject);
            }
            else if (characterBullet)
            {
                unit.RecieveDamage();
                Destroy(gameObject);
            }
            
        }
        else if (!unit)
        {
            Destroy(gameObject);
        }
    }
}
