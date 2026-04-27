using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SlideScreenController : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] private Renderer screenRenderer;

    [Header("Slides")]
    [SerializeField] private string slidesFolder = "Slides";

    public event Action<int, int> OnSlideChanged;

    private readonly List<Texture2D> loadedSlides = new();
    private int currentSlideIndex = -1;

    private void Start()
    {
        LoadAllSlides();

        if (loadedSlides.Count == 0)
        {
            Debug.LogError("SlideScreenController: No slides were loaded.");
            return;
        }

        ShowSlideByIndex(0);
    }

    private void LoadAllSlides()
    {
        loadedSlides.Clear();

        string folderPath = Path.Combine(Application.streamingAssetsPath, slidesFolder);

        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Slides folder not found: " + folderPath);
            return;
        }

        string[] pngFiles = Directory.GetFiles(folderPath, "*.png");

        pngFiles = pngFiles
            .OrderBy(path => ExtractLeadingNumber(Path.GetFileNameWithoutExtension(path)))
            .ThenBy(path => Path.GetFileNameWithoutExtension(path))
            .ToArray();

        foreach (string filePath in pngFiles)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);

            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            bool loaded = ImageConversion.LoadImage(texture, imageBytes);

            if (!loaded)
            {
                Debug.LogError("Failed to decode image: " + filePath);
                Destroy(texture);
                continue;
            }

            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            loadedSlides.Add(texture);
        }

        Debug.Log("Loaded slides: " + loadedSlides.Count);
    }

    private int ExtractLeadingNumber(string fileName)
    {
        if (int.TryParse(fileName, out int number))
            return number;

        return int.MaxValue;
    }

    public void NextSlide()
    {
        if (currentSlideIndex < loadedSlides.Count - 1)
            ShowSlideByIndex(currentSlideIndex + 1);
    }

    public void PreviousSlide()
    {
        if (currentSlideIndex > 0)
            ShowSlideByIndex(currentSlideIndex - 1);
    }

    public void JumpToSlide(int slideNumber)
    {
        int index = slideNumber - 1;

        if (index < 0 || index >= loadedSlides.Count)
            return;

        ShowSlideByIndex(index);
    }

    public int GetCurrentSlideNumber()
    {
        return currentSlideIndex + 1;
    }

    public int GetTotalSlides()
    {
        return loadedSlides.Count;
    }

    private void ShowSlideByIndex(int index)
    {
        if (index < 0 || index >= loadedSlides.Count)
            return;

        currentSlideIndex = index;
        screenRenderer.material.mainTexture = loadedSlides[currentSlideIndex];

        OnSlideChanged?.Invoke(GetCurrentSlideNumber(), GetTotalSlides());
    }

    private void OnDestroy()
    {
        foreach (Texture2D tex in loadedSlides)
        {
            if (tex != null)
                Destroy(tex);
        }

        loadedSlides.Clear();
    }
}