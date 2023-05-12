using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueFade : MonoBehaviour
{
    TMP_Text dialogue;
    [SerializeField] Image dialogueBox;
    Button continueButton;
    byte alpha;
    public void Start()
    {
        dialogue = GetComponentInChildren<TMP_Text>();
        dialogueBox = GetComponent<Image>();
        continueButton = GetComponentInChildren<Button>();
        alpha = 0;
        dialogue.color = new Color32(255,255,255, alpha);
        dialogueBox.color = new Color32(255,255,255, alpha);
    }

    
    void Update()
    {

        if (alpha <= 1)
        {
            StopCoroutine("FadeOut");
        } 
        if (alpha >= 250)
        {
            StopCoroutine("FadeIn");
        }
    }

    IEnumerator FadeOut()
    {
        for (alpha = 255; alpha >= 0; alpha--)
        {
            dialogue.color = new Color32(255,255,255, alpha);
            dialogueBox.color = new Color32(255,255,255, alpha);
            yield return null;
        }

    }

    IEnumerator FadeIn()
    {
        for (alpha = 0; alpha <= 255; alpha++)
        {
            dialogue.color = new Color32(255,255,255, alpha);
            dialogueBox.color = new Color32(255,255,255, alpha);
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            StartCoroutine("FadeIn");
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            StartCoroutine("FadeOut");
        }
    }
}
