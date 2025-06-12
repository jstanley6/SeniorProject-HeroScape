# Heroscape Digital Scenario Editor \- User Manual

## Installation

Check the releases section on the right side of the github page for our most recent build, which will be a .zip file. On Windows, simply extract the zip file, and within you will find the executable for the HeroScape Digital Scenario Editor. 

We plan on releasing builds for other platforms soon, as well as eventually migrating to a web-based version accessible in your browser.

## Camera

When you open HeroScape DSE, you should see a blank field of hexagons and a bar of pieces on the right side.

To **rotate** the camera, simply right click and drag to orbit around the center of the space.

To **pan** the camera, middle click and drag in any direction to move.

You can also use your number keys to move the camera to **preset positions**:  
*1: Front View*  
*2: Back View*  
*3: Left Side View*  
*4: Right Side View*  
*5: Top-Down View*  
*6: Reset View*

## The Terrain Editor

To begin editing terrain, look to the right side of the screen where you will find a list of terrain piece images. You can use the search bar above to quickly find a specific piece. To place a piece onto the scene, simply left click and drag and the image of the piece onto the scene. The selected piece will appear in the scene wherever you drop it, but if you change your mind before then, you can drag the piece back into the sidebar.  

Current **searchable piece names:**  
*Types: Grass, Sand, Stone, Water*  
*Sizes: 1, 2, 3, 7, 24*

Once a piece has been placed, a widget will appear on the selected piece. You can left click and drag on the part with the red, blue, and green arrows to move the piece around the scene.

You can also left click and drag on the pink ring to rotate the piece, automatically snapping to one of six positions to align with the hexagonal grid. If you find yourself unable to rotate a piece in one direction or another, that means one or more of the locations the piece would take up is already filled, and you will need to either remove the blockage or move your piece elsewhere before rotating.

Lastly, while a piece is selected, you can press “delete” on your keyboard to delete the selected piece. 

## Scenario Details

In the top center of your screen, there is a text box that says “Untitled Scenario.” You can click here and enter the name of your scenario.

You can click the “Details” button to open a new box on the left side of the screen. Inside, you can enter the scenario description, goal, setup, victory conditions, and any other special rules, just like any official HeroScape scenario. These fields will be saved and loaded just like any other part of your scenario.

## Save/Load

HeroScape DSE features a json-based save and load system using files saved to your device. Every part of your scenario, from the terrain to the name and other scenario details, are saved in these .json text files, which you can feel free to share with other users however you’d like\!

To save a scenario, click “File” in the top left corner, then “Save Scenario.” From there, your system will open a file explorer window to let you choose a file name and save location.

To load a scenario, click “File” in the top left corner, then “Load Scenario.” From there, your system will open a file explorer window to let you choose a .json file to load into HeroScape DSE. Note: Please **only load .json files created by HeroScape DSE** and **do not manually edit them yourself**\!

Lastly, you can clear the current contents of the editor by going to “File,” then clicking “New Scenario.”
