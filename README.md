# Unity to HTML Converter
A converter that extracts basic scene data from unity and generates HTML file for displaying the scene in web browser, using three.js.

## Features
- camera export
- cube hierarchy
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
4. Place objects in world and attach HTMLCube component to them, or simply use HTMLCube prefab, located in Prefabs folder
5. Tools -> Export To HTML5
6. Select a destination folder
7. Open generated index.html file in web browser (confirmed to work with Google Chrome, Microsoft Edge and Opera)