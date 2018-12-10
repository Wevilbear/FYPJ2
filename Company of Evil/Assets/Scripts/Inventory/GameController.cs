using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject objectToDisable;
    public  bool disabled = false;

    private void Update()
    {
        if (disabled)
            objectToDisable.SetActive(false);
        else
            objectToDisable.SetActive(true);
    }
}
