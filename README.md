# Virtual Performance Simulator
A Unity-based VR application for practicing public speaking and performance in customizable environments.

## Project Setup
Unity version: Unity 6.4 (6000.4.0f1)
Make sure you are using this exact version to avoid compatibility issues.

## Project Structure
This project uses a strict separation between template assets and our own work.
### Our Work (USE THIS)
All team-created content goes inside:

`Assets/_Project/`

Structure:

```
_Project/
  Audio/
  Materials/
  Prefabs/
    Environment/
    Interaction/
    UI/
    XR/
  Scenes/
  Scripts/
    Core/
    Environment/
    Presentation/
    UI/
    XR/
  Settings/
  UI/
  ```
### Template/Unity Files (DO NOT MODIFY)
All other folders in Assets/ (e.g. XR, XRI, VRTemplateAssets, Samples, etc.) are:
- Unity systems
- XR template dependencies

**Do not move, rename, or reorganize these folders or anything in them**

If you need something from them:
- Copy it into _Project
- Modify your copy

## Scenes
All working scenes are located in:
`Assets/_Project/Scenes/`
Current Scenes:
- MainMenu
- Lobby
- Classroom_Small
- LectureHall_Large
- PitchRoom_Small
- TEST (sandbox/testing scene)

Use TEST for experimenting - don't break main scenes.

## XR Rig
A reusable XR rig prefab has been created:

`Assets/_Project/Prefabs/XR/VPP_XROrigin_Base`

Features:
- Controller-based interaction
- Near/Far interaction (UI + objects)
- Teleportation only (no continuous movement)

Standards:
- Use this prefab in all scenes
- Do not modify the template XR rig directly
- If changes are needed update this prefab
