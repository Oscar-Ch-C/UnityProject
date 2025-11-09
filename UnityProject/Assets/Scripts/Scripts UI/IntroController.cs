using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [Header("UI")]
    public Image imageDisplay;
    public CanvasGroup imageCanvasGroup;
    public TextMeshProUGUI narrativeText;
    public CanvasGroup narrativeCanvasGroup;
    public TextMeshProUGUI titleText;
    public CanvasGroup titleCanvasGroup;

    [Header("Story")]
    public Sprite[] storySprites;
    public string[] storyLines;
    public int linesPerImage = 2;

    [Header("Timings")]
    public float secondsPerImage = 3f;   // tiempo mínimo que cada imagen debe estar visible
    public float fadeDuration = 0.6f;
    public float titleDisplayDuration = 2f;
    public string menuSceneName = "MainMenu";

    [Header("Audio")]
    public GameObject musicPlayer; // arrastra el GameObject MusicPlayer

    // referencia al typewriter (se toma automáticamente si está en NarrativeText)
    private TypewriterEffect typewriter;

    void Start()
    {
        if (narrativeText == null) Debug.LogError("NarrativeText no asignado en IntroController.");
        if (imageDisplay == null) Debug.LogError("ImageDisplay no asignado en IntroController.");

        typewriter = narrativeText.GetComponent<TypewriterEffect>();

        // estado inicial
        if (imageCanvasGroup != null) imageCanvasGroup.alpha = 0f;
        if (narrativeCanvasGroup != null) narrativeCanvasGroup.alpha = 0f;
        if (titleCanvasGroup != null) titleCanvasGroup.alpha = 0f;

        narrativeText.text = "";
        if (titleText != null) titleText.gameObject.SetActive(true); // lo controlamos por alpha

        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        int imagesCount = storySprites != null ? storySprites.Length : 0;
        if (imagesCount == 0)
        {
            Debug.LogWarning("No hay sprites en storySprites.");
        }

        for (int i = 0; i < imagesCount; i++)
        {
            // asigna sprite
            imageDisplay.sprite = storySprites[i];

            // fade in imagen + texto
            yield return StartCoroutine(FadeCanvasGroup(imageCanvasGroup, 0f, 1f, fadeDuration));
            yield return StartCoroutine(FadeCanvasGroup(narrativeCanvasGroup, 0f, 1f, fadeDuration));

            // construir bloque de texto para esta imagen
            int startLine = i * linesPerImage;
            string block = "";
            for (int j = 0; j < linesPerImage; j++)
            {
                int idx = startLine + j;
                if (idx >= storyLines.Length) break;
                block += storyLines[idx];
                if (j < linesPerImage - 1 && (startLine + j + 1) < storyLines.Length) block += "\n";
            }

            // inicia typewriter (si existe)
            if (typewriter != null)
                StartCoroutine(typewriter.TypeText(block));
            else
                narrativeText.text = block;

            // espera hasta que se cumpla el tiempo mínimo Y el typewriter haya terminado
            float t0 = Time.time;
            while (Time.time - t0 < secondsPerImage || (typewriter != null && typewriter.IsTyping))
            {
                yield return null;
            }

            // fade out texto e imagen
            yield return StartCoroutine(FadeCanvasGroup(narrativeCanvasGroup, 1f, 0f, fadeDuration));
            yield return StartCoroutine(FadeCanvasGroup(imageCanvasGroup, 1f, 0f, fadeDuration));

            narrativeText.text = "";
        }

        // mostrar título (fade)
        yield return StartCoroutine(FadeCanvasGroup(titleCanvasGroup, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(titleDisplayDuration);
        yield return StartCoroutine(FadeCanvasGroup(titleCanvasGroup, 1f, 0f, fadeDuration));

        // cargar menú
        SceneManager.LoadScene(menuSceneName);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        if (cg == null) yield break;
        float t = 0f;
        cg.alpha = from;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
    }
}
