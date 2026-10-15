# CDRunning - Call of Duty Utility

CDRunning is a VB.NET Windows Forms desktop utility a buddy and I made a couple years back for fun with CoD.  We made it for managing automation presets, simulated input, and screen-based preset switching.  It can easily be configured to work with other shooters, for us we wanted to see what it was like to program in VB so we went with it.

The app lets you create named presets with staged movement/timing values, bind hotkeys, and automatically switch presets by checking screen pixels and color matches. It also includes a visual "swap mapper" tool that helps capture screen regions and reference pixels used for detection.

## What it does

- Create and manage named presets
- Store stage/loadout values for each preset
- Start and stop automation with a hotkey
- Simulate mouse movement, mouse clicks, and key presses
- Toggle features such as:
  - auto focus
  - auto ping
  - click position capture
  - auto recoil swap
- Detect screen state by checking saved pixel/color mappings
- Use a draggable on-screen mapper window to create and test swap mappings

## Main components

### `CDRunningForm`
Main application window.

Responsible for:
- loading and saving settings
- handling hotkeys
- running the main timer loop
- simulating mouse and keyboard input
- selecting presets
- checking swap mappings against the screen

### `AutoSwapForm`
Used to create, load, test, and remove swap mappings.

Responsible for:
- managing saved mapping entries
- locking/unlocking mapping capture
- testing whether a saved mapping matches the current screen state

### `DragMe`
A small draggable overlay window used during mapping.

Responsible for:
- selecting the screen region to map
- capturing pixel colors when locked
- adding captured points/colors to the current mapping

### `Globals`
Contains shared state and helper methods.

Includes:
- preset/mapping collections
- serialization helpers for rectangles, points, and colors
- key conversion helpers

## How it works

1. The app loads presets, default keys, and saved screen mappings from local files.
2. You choose or create a preset.
3. Each preset contains one or more stage values in this format:

   `Y:X:Milliseconds`

   Example:

   `2:0:1000;1:0:5000`

4. When the tool is started, it monitors the configured hotkeys and mouse state.
5. If enabled, it can also check saved screen mappings and automatically switch to the matching preset.
6. Input is sent using Windows `SendInput`.

## Files used

The app saves data in the `Resources` folder:

- `settings.txt` - preset names and loadout values
- `defaultkeys.txt` - default key bindings
- `positions.txt` - saved swap-mapper data

## How to use

### 1. Build and run
Open the solution/project in Visual Studio and run the WinForms app.

### 2. Configure hotkeys
Set the main keys in the dropdowns:
- Start key
- BR key
- Shoot key
- Focus key
- Ping key

### 3. Create or select a preset
Use the preset dropdown to select an existing preset or create a new one.

Preset names control behaviour:
- `Key:NUMPAD1` at the END of the name binds that key (A-Z, 0-9, F1-F12, NUMPAD0-9); it takes effect after the next Start.
- `__` in the name marks the fallback preset used when no screen match is found.
- `_BR_` in the name turns BattleRifle rapid-fire on when the preset is selected.

### 4. Enter stage values
Add stage values in the textbox using this format:

`Y:X:Milliseconds;`

Example:

`2:0:1000;1:0:5000`

### 5. Start the process
Press the configured start key or click the start button.

### 6. Optional: create a swap mapping
To create a screen-based mapping:

1. Open the swap mapper
2. Select the preset you want to map
3. Click `Create`
4. Drag the `DragMe` overlay over the target screen area
5. Click `Lock`
6. Click inside the overlay to capture reference pixels/colors
7. Save the mapping
8. Test it to confirm detection works

When Auto Recoil Swap is enabled, the app can compare saved mappings to the current screen and switch presets automatically.

## Notes

- This project is Windows-only because it depends on WinForms and `user32.dll`
- Mappings depend on screen position and color values, so resolution/UI changes can break detection
- The project uses shared/global state heavily, so future refactoring would improve maintainability
- We had to give it admin rights to take over the mouse when playing a game and triggering an automated event

## Tech stack

- VB.NET
- Windows Forms
- Windows `user32.dll` input APIs

## Disclaimer

This project is a personal desktop automation utility intended for local use and experimentation.