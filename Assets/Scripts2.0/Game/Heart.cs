using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    [SerializeField]
    private float speed = 1f;

    private void Update()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 2f)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0f, 0.5f, 0f), speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            unit.GetHeart();
            Destroy(gameObject);
        }
    }
}
