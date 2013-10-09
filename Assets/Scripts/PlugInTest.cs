/*
 * Simple example of how to use this iCade plugin.
 * Poll and callback methods are used below, for documentation purposes.
 * Check http://www.ionaudio.com/downloads/ION%20Arcade%20Dev%20Resource%20v1.5.pdf for button commands.
 */

using UnityEngine;

// ReSharper disable once CheckNamespace
public class PlugInTest : MonoBehaviour {
 
    /// <summary>
    /// This will be called whenever the iCade state changes i.e. it will get called for ALL events
    /// </summary>
    /// <param name="state"></param>
	void iCadeStateCallback(int state)
	{
        print("iCade state change. Current state="+state);
	}

    /// <summary>
    /// This will be called whenever there's a button up event in iCade. It will get called for buttons and axis, since axis movement also translates into key presses
    /// </summary>
    /// <param name="button"></param>
    void iCadeButtonUpCallback(char button)
    {
        print("Button up event. Button " + button + " up");
    }

    /// <summary>
    /// This will be called whenever there's a button down event in iCade. It will get called for buttons and axis, since axis movement also translates into key presses
    /// </summary>
    /// <param name="button"></param>
    void iCadeButtonDownCallback(char button)
    {
        print("Button down event. Button " + button + " down");
    }



    void Start()
    {
        //This is needed to activate the iCade plugin. Deactivate it by using iCadeInput.Activate(false)
		iCadeInput.Activate(true);

        //Register some callbacks
        iCadeInput.AddICadeEventCallback(iCadeStateCallback);
        iCadeInput.AddICadeButtonUpCallback(iCadeButtonUpCallback);
        iCadeInput.AddICadeButtonDownCallback(iCadeButtonDownCallback);
    }


	void Update()
	{
        //Polls for axis state
		float horz=iCadeInput.GetAxis("HORIZONTAL");
		float vert=iCadeInput.GetAxis("VERTICAL");

		if (horz<0f)
		{
			print ("LEFT");
		}
		else if (horz>0)
		{
			print("RIGHT");
		}

		if (vert>0)
		{
			print("UP");
		}
		else if (vert<0)
		{
			print("DOWN");
		}
        

        //Polls for button 5 (key press = 'y') down event
	    bool y = iCadeInput.GetButtonDown('y');
	    if (y)
	    {
	        print("UNITY DOWN: Y");
	    }

        //Polls for button 5 (key press = 'y') up event
	    y = iCadeInput.GetButtonUp('y');
	    if (y)
	    {
	        print("UNITY UP: Y");
	    }

        //Polls for button 7 (key press = 'u') event
        bool u = iCadeInput.GetButton('u');
        if (u)
        {
            print("UNITY BUTTON: U");
        }

	}

}
