using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SlideRemoteController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private SlideScreenController targetScreen;

    [Header("Actions While Held")]
    [SerializeField] private InputActionReference nextSlideAction;
    [SerializeField] private InputActionReference previousSlideAction;

    [Header("Optional")]
    [SerializeField] private bool allowOnlyRightHand = false;
    [SerializeField] private bool allowOnlyLeftHand = false;

    private XRGrabInteractable grabInteractable;
    private bool isHeld;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor currentInteractor;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }

        if (nextSlideAction != null)
        {
            nextSlideAction.action.performed += OnNextPressed;
            nextSlideAction.action.Enable();
        }

        if (previousSlideAction != null)
        {
            previousSlideAction.action.performed += OnPreviousPressed;
            previousSlideAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }

        if (nextSlideAction != null)
        {
            nextSlideAction.action.performed -= OnNextPressed;
            nextSlideAction.action.Disable();
        }

        if (previousSlideAction != null)
        {
            previousSlideAction.action.performed -= OnPreviousPressed;
            previousSlideAction.action.Disable();
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        currentInteractor = args.interactorObject;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        currentInteractor = null;
    }

    private void OnNextPressed(InputAction.CallbackContext context)
    {
        if (!CanUseRemote())
            return;

        targetScreen.NextSlide();
    }

    private void OnPreviousPressed(InputAction.CallbackContext context)
    {
        if (!CanUseRemote())
            return;

        targetScreen.PreviousSlide();
    }

    private bool CanUseRemote()
    {
        if (!isHeld || targetScreen == null)
            return false;

        if (!allowOnlyLeftHand && !allowOnlyRightHand)
            return true;

        if (currentInteractor is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor inputInteractor)
        {
            var handedness = inputInteractor.handedness;

            if (allowOnlyLeftHand && handedness == UnityEngine.XR.Interaction.Toolkit.Interactors.InteractorHandedness.Left)
                return true;

            if (allowOnlyRightHand && handedness == UnityEngine.XR.Interaction.Toolkit.Interactors.InteractorHandedness.Right)
                return true;
        }

        return false;
    }
}