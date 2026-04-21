using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class SlideScreenController : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] private Renderer screenRenderer;

    [Header("Buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [Header("Counter")]
    [SerializeField] private TMP_Text counterText;

    [Header("Slides")]
    [SerializeField] private string slidesFolder = "Slides";
    [SerializeField] private int firstSlideNumber = 1;
    [SerializeField] private int lastSlideNumber = 49;

    [Header("Optional Testing")]
    [SerializeField] private bool enableKeyboardTesting = true;

    private readonly List<Texture2D> loadedSlides = new();
    private int currentSlideIndex = -1;

    private IEnumerator Start()
    {
        if (screenRenderer == null)
        {
            Debug.LogError("SlideScreenController: screenRenderer is not assigned.");
            yield break;
        }

        if (prevButton != null)
            prevButton.onClick.AddListener(PreviousSlide);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextSlide);

        yield return StartCoroutine(LoadAllSlides());

        if (loadedSlides.Count == 0)
        {
            Debug.LogError("SlideScreenController: No slides were loaded.");
            yield break;
        }

        ShowSlide(0);
    }

    private void Update()
    {
        if (!enableKeyboardTesting)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            NextSlide();

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            PreviousSlide();
    }

    private IEnumerator LoadAllSlides()
    {
        loadedSlides.Clear();

        for (int i = firstSlideNumber; i <= lastSlideNumber; i++)
        {
            string fileName = i + ".png";
            string fullPath = Path.Combine(Application.streamingAssetsPath, slidesFolder, fileName);

            Texture2D texture = null;

            if (fullPath.StartsWith("jar:") || fullPath.StartsWith("http"))
            {
                using UnityWebRequest request = UnityWebRequestTexture.GetTexture(fullPath);
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to load slide: " + fileName + " | " + request.error);
                    continue;
                }

                texture = DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                if (!File.Exists(fullPath))
                {
                    Debug.LogError("Slide not found: " + fullPath);
                    continue;
                }

                byte[] imageBytes = File.ReadAllBytes(fullPath);
                texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

                bool loaded = ImageConversion.LoadImage(texture, imageBytes);
                if (!loaded)
                {
                    Debug.LogError("Failed to decode image: " + fullPath);
                    Destroy(texture);
                    continue;
                }
            }

            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            loadedSlides.Add(texture);
        }

        Debug.Log("Loaded slides: " + loadedSlides.Count);
    }

    public void NextSlide()
    {
        if (currentSlideIndex < loadedSlides.Count - 1)
            ShowSlide(currentSlideIndex + 1);
    }

    public void PreviousSlide()
    {
        if (currentSlideIndex > 0)
            ShowSlide(currentSlideIndex - 1);
    }

    private void ShowSlide(int index)
    {
        if (index < 0 || index >= loadedSlides.Count)
            return;

        currentSlideIndex = index;

        screenRenderer.material.mainTexture = loadedSlides[currentSlideIndex];

        if (counterText != null)
            counterText.text = $"{currentSlideIndex + 1} / {loadedSlides.Count}";

        if (prevButton != null)
            prevButton.interactable = currentSlideIndex > 0;

        if (nextButton != null)
            nextButton.interactable = currentSlideIndex < loadedSlides.Count - 1;
    }

    private void OnDestroy()
    {
        if (prevButton != null)
            prevButton.onClick.RemoveListener(PreviousSlide);

        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextSlide);

        foreach (Texture2D tex in loadedSlides)
        {
            if (tex != null)
                Destroy(tex);
        }

        loadedSlides.Clear();
    }
}