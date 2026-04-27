using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlideControlPanelController : MonoBehaviour
{
    [Header("Target Screen")]
    [SerializeField] private SlideScreenController targetScreen;

    [Header("UI")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TMP_InputField pageInput;
    [SerializeField] private Button jumpButton;

    private void Start()
    {
        if (prevButton != null)
            prevButton.onClick.AddListener(OnPrevClicked);

        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextClicked);

        if (jumpButton != null)
            jumpButton.onClick.AddListener(OnJumpClicked);

        if (targetScreen != null)
        {
            targetScreen.OnSlideChanged += UpdateUI;
            UpdateUI(targetScreen.GetCurrentSlideNumber(), targetScreen.GetTotalSlides());
        }
    }

    private void OnDestroy()
    {
        if (prevButton != null)
            prevButton.onClick.RemoveListener(OnPrevClicked);

        if (nextButton != null)
            nextButton.onClick.RemoveListener(OnNextClicked);

        if (jumpButton != null)
            jumpButton.onClick.RemoveListener(OnJumpClicked);

        if (targetScreen != null)
            targetScreen.OnSlideChanged -= UpdateUI;
    }

    private void OnPrevClicked()
    {
        if (targetScreen != null)
            targetScreen.PreviousSlide();
    }

    private void OnNextClicked()
    {
        if (targetScreen != null)
            targetScreen.NextSlide();
    }

    private void OnJumpClicked()
    {
        if (targetScreen == null || pageInput == null)
            return;

        if (int.TryParse(pageInput.text, out int slideNumber))
        {
            targetScreen.JumpToSlide(slideNumber);
        }
    }

    private void UpdateUI(int currentSlide, int totalSlides)
    {
        if (counterText != null)
            counterText.text = $"{currentSlide} / {totalSlides}";

        if (prevButton != null)
            prevButton.interactable = currentSlide > 1;

        if (nextButton != null)
            nextButton.interactable = currentSlide < totalSlides;
    }
}