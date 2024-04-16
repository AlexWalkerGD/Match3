using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    private GameController gameController;
    public GameObject panel;
    public Text numberText;
    public GameObject upAnim;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        animator = upAnim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {       
        
    }

    public void ShowCanva(int numberMatch)
    {
        panel.SetActive(true);
        numberText.text = numberMatch.ToString();
        animator.Play("upAnim");
        StartCoroutine(HiddenCanva());
    }

    IEnumerator HiddenCanva()
    {
        yield return new WaitForSeconds(3);
        panel.SetActive(false);
        StopCoroutine(HiddenCanva());
    }
}
