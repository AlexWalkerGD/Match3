using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameInvisible()
    {
        gameController.isMatching = false;
        gameController.isSwap = false;
        gameController.firstSquareTouched = null;
        gameController.secondSquareTouched = null;
        Destroy(this.gameObject);
    }
}
