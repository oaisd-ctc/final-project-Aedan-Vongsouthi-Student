using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeIn : MonoBehaviour
{
    [SerializeField] TMP_Text textmeshPro;
    byte alpha;
    public void Start()
    {
        textmeshPro = GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            StartCoroutine("Fade");
        } 

        if (alpha <= 0)
        {
            StopCoroutine("Fade");
        }
    }

    IEnumerator Fade()
    {
        for (alpha = 255; alpha >= 0; alpha--)
        {
            textmeshPro.color = new Color32(255,255,255, alpha);
            Debug.Log(alpha);
            yield return null;
        }

    }
}
