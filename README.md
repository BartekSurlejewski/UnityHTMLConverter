# Unity to HTML Converter
A converter that extracts basic scene data from unity and generates HTML file for displaying the scene in web browser, using three.js.

## Features
- camera and game objects hierarchy export
- position, rotation and scale
- HTML viewer using three.js

## How to run
### To export demo scene
1. Open project in Unity (developed using Unity 6000.3.10f1)
2. Open ExporterDemoScene
3. Tools -> Export To HTML5
4. Select a destination folder
5. Open generated index.html file in web browser (confirmed to work with Google Chrome, Microsoft Edge and Opera)

### To create a custom scene
1. Open project in Unity
2. Create a new scene and open it
3. Make sure that the camera has "Main Camera" tag
4. Place objects in world. For now only the objects with cube primitive mesh will be rendered in HTML export
5. Tools -> Export To HTML5
6. Select a destination folder
7. Open generated index.html file in web browser (confirmed to work with Google Chrome, Microsoft Edge and Opera)

## Known limitations/other info
- Currently it only displays cube primitives on html side
- html and js template files are found by hardcoded path. You can change the paths by modifying SceneExporter.cs
- The tool is prone to bugs and crashes if template files are moved or changed
- It's not designed to be performant, as it’s an editor tool, not meant to be used at runtime
- Requires internet connection to access three.js source code
- Scene data json is inserted into the html file’s body section
- Camera needs to be tagged as main camera

  ## Demo scene in engine
  <img width="1100" height="650" alt="image" src="https://github.com/user-attachments/assets/28882fed-7c8a-4b9b-9550-8ada551e3fee" />

  ## Demo scene in web browser
<img width="1736" height="905" alt="image" src="https://github.com/user-attachments/assets/b98933c3-2c9e-4de7-aa74-468096533d6c" />
