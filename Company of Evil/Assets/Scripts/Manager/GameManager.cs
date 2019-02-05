using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private Player player;

    // Update is called once per frame
    void Update()
    {
        ClickTarget();

    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1024);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("you hit skele");
                    player.MyTarget = hit.transform;
                }

            }
            else
            {
                player.MyTarget = null;
            }
        }

    }
}
