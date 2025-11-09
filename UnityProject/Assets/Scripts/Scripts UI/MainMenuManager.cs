using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Groups (assign in Inspector)")]
    public CanvasGroup fondo;           // Fondo (imagen)
    public CanvasGroup uiGroup;         // Grupo que contiene título + botones

    [Header("Timings")]
    public float fadeDuration = 1.2f;
    public float idleTimeToReset = 20f; // 20 segundos -> volver a IntroScene

    float idleTimer = 0f;
    Vector3 lastMousePos;

    void Start()
    {
        // seguridad: si no asignaste, intenta buscarlos
        if (fondo == null || uiGroup == null) Debug.LogWarning("Asigna fondo y uiGroup en MainMenuManager.");

        // inicio oculto (asegúrate en Inspector que alpha = 0 también)
        if (fondo != null) fondo.alpha = 0f;
        if (uiGroup != null) uiGroup.alpha = 0f;

        StartCoroutine(ShowMenuSequence());
        lastMousePos = Input.mousePosition;
    }

    IEnumerator ShowMenuSequence()
    {
        // Fade in del fondo
        if (fondo != null) yield return StartCoroutine(FadeCanvasGroup(fondo, 0f, 1f, fadeDuration));
        else yield return null;

        // Pequeña pausa opcional
        yield return new WaitForSeconds(0.15f);

        // Fade in del título y botones
        if (uiGroup != null) yield return StartCoroutine(FadeCanvasGroup(uiGroup, 0f, 1f, fadeDuration));
    }

    void Update()
    {
        // Detectar interacción: teclado, click/touch o movimiento de ratón
        bool interacted = false;

        if (Input.anyKeyDown) interacted = true;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) interacted = true;
        if (Input.touchCount > 0) interacted = true;

        // detectar movimiento de mouse (para contar como interacción)
        if (Input.mousePosition != lastMousePos)
        {
            interacted = true;
            lastMousePos = Input.mousePosition;
        }

        if (interacted) idleTimer = 0f;
        else idleTimer += Time.deltaTime;

        if (idleTimer >= idleTimeToReset)
        {
            // volver a la IntroScene
            SceneManager.LoadScene("IntroScene");
        }
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

    // ---------- Métodos que enlazarás a los botones ----------
    public void OnJugarPressed()
    {
        SceneManager.LoadScene("Jugar"); // crea esta escena más adelante
    }

    public void OnOpcionesPressed()
    {
        SceneManager.LoadScene("Options");
    }

    public void OnCreditosPressed()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnSalirPressed()
    {
        Application.Quit();
        Debug.Log("Salir pressed - Application.Quit()");
    }
}
