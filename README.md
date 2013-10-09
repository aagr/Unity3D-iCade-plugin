Unity3D-iCade-plugin
====================

iCade plugin for Unity3D. Supports poll and callback usages. Contains postprocessing code that changes the generated Xcode project (uses mod_pbxproj).


You can use this plugin in two ways:


a) Poll

This mimics the way Unity’s Input class works, with a couple of subtle differences.

You need to:

Activate the plugin with iCadeInput.Activate(true)

Use bool iCadeInput.GetButtonDown(char button) or iCadeInput.GetButtonUp(char button) to get the state of a given button. Like Unity’s Input.GetButtonDown, this will only return true in the first frame where the state changed.

Use bool iCadeInput.GetButton(char button) to get the state of a button. This will continuously report the state of a button, like Unity’s Input.GetButton


Event driven (callback)

This allows you to be notified when a state occurs in iCade. 

You need to:

Activate the plugin with iCadeInput.Activate(true)

Use iCadeInput.AddICadeEventCallback to be notified of all state changes
Use iCadeInput.AddICadeButtonUpCallBack to be notified of button up events (includes joystick)
Use iCadeInput.AddICadeButtonDownCallBack to be notified of button down events (includes joystick)


Check PlugInTest.cs for an example on all usages.
