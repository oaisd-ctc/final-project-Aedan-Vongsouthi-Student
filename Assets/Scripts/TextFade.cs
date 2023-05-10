using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFade : MonoBehaviour
{
    [SerializeField] TMP_Text textmeshPro;
    byte alpha;
    public void Start()
    {
        textmeshPro = GetComponent<TMP_Text>();
        alpha = 0;
        textmeshPro.color = new Color32(255,255,255, alpha);
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
            textmeshPro.color = new Color32(255,255,255, alpha);
            yield return null;
        }

    }

    IEnumerator FadeIn()
    {
        for (alpha = 0; alpha <= 255; alpha++)
        {
            textmeshPro.color = new Color32(255,255,255, alpha);
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
