using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float charsPerSecond = 40f;
    public AudioSource tickSource; // opcional

    [HideInInspector]
    public bool IsTyping = false;

    public IEnumerator TypeText(string fullText)
    {
        if (textComponent == null) yield break;
        IsTyping = true;
        textComponent.text = "";

        float delay = 1f / Mathf.Max(1f, charsPerSecond);

        for (int i = 0; i < fullText.Length; i++)
        {
            char c = fullText[i];
            textComponent.text += c;
            if (tickSource != null && c != ' ')
            {
                tickSource.PlayOneShot(tickSource.clip);
            }

            // permitir completar el bloque con Space o Return
            float elapsed = 0f;
            while (elapsed < delay)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    textComponent.text = fullText;
                    IsTyping = false;
                    yield break;
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        IsTyping = false;
    }
}
