using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMonsterArea : MonoBehaviour
{
    private Unit monster;

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();
        
    }
}
