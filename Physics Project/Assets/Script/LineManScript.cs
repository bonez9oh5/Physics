using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LineManScript : MonoBehaviour {

    [SerializeField]
    Text winnerText;
    [SerializeField]
    Animator anim;
    [SerializeField]
    Collider finishLine;
    [SerializeField]
    GameObject startText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startText.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        winnerText.text = "Winner is: " + other.gameObject.name;
        Destroy(finishLine);
        anim.SetTrigger("Finished");
        Invoke("ReturnToTitle", 1);

    }
    
    void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
