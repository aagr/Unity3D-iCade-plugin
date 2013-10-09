using System;
using UnityEngine;
using System.Runtime.InteropServices;
using AOT;
using Object = UnityEngine.Object;

// ReSharper disable once CheckNamespace
// ReSharper disable once InconsistentNaming
public class iCadeInput:MonoBehaviour
{
    //iCade key down values
	private const string OnStates  = "wdxayhujikol";

    //iCade key up values. These aren't used in this plugin and are just here for reference. Always use key down values when checking state (either poll or callback) or using GetButtonXXX
	private const string OffStates = "eczqtrfnmpgv";

	//User defined event callbacks will be stored here
    private static Action<int> _userEventCallbacks;

    //User defined button up callbacks will be stored here
    private static Action<char> _userButtonUpCallbacks;

    //User defined button down callbacks will be stored here
    private static Action<char> _userButtonDownCallbacks;

#pragma warning disable 414
    //Old and current state
    static int _oldState, _currentState;
#pragma warning restore 414
    

    [DllImport("__Internal")]
	private static extern void _activate(bool state);
			
	[DllImport("__Internal")]
    private static extern void _registerStateCallback(Action<int> cntCallback);

	[DllImport("__Internal")]
    private static extern void _registerButtonUpCallback(Action<int> cntCallback);

	[DllImport("__Internal")]
    private static extern void _registerButtonDownCallback(Action<int> cntCallback);

	[DllImport("__Internal")]
	private static extern int _getState();


	/// <summary>
	/// Called by native code when iCade state changes. Will then proceed to call user defined event callbacks (if there are any) 
	/// </summary>
	/// <param name="state">State contains the current iCade state</param>
	[MonoPInvokeCallback(typeof(Action<int>))]
	public static void UnityStateCallBack(int state)
	{
		if (_userEventCallbacks!=null)
		{
			_userEventCallbacks(state);
		}
	}

	/// <summary>
	/// Called by native code when there's a button up event in iCade
	/// </summary>
	/// <param name="button">Button contains an int that contains the state corresponding to a button press.</param>
    [MonoPInvokeCallback(typeof(Action<int>))]
	public static void UnityButtonUpCallBack(int button)
	{
	    _currentState = GetState();
	    if (_userButtonUpCallbacks != null)
	    {
	        _userButtonUpCallbacks(GetButton(button));
	    }
	}

	/// <summary>
	/// Called by native code when there's a button down event in iCade
	/// </summary>
    /// <param name="button">Button contains an int that contains the state corresponding to a button press.</param>
    [MonoPInvokeCallback(typeof(Action<int>))]
	public static void UnityButtonDownCallBack(int button)
	{
	    _currentState = GetState();
        if (_userButtonDownCallbacks != null)
        {
            _userButtonDownCallbacks(GetButton(button));
        }
	}

    /// <summary>
    /// Helper function to get the key char out of a state. ONLY WORKS IF THE STATE ONLY CONTAINS ONE KEY (as sent to UnityButtonXXXCallback).
    /// To get multiple key presses out of a state, check the code example in GetState().
    /// </summary>
    /// <param name="state"></param>
    /// <returns>Single key in state.</returns>
    private static char GetButton(int state)
    {
        int length = OnStates.Length;

        for (int i = length - 1; i >= 0; i--)
        {
            if ((state & 1 << i) != 0)
            {
                return(OnStates[i]);
            }
        }

        return (char) 0;
    }



	/// <summary>
	/// Allows the user to add a callback method, that's called back when iCade state changes
	/// </summary>
	/// <param name="callback">Callback.</param>
    public static void AddICadeEventCallback(Action<int> callback)
	{
		_userEventCallbacks+=callback;
	}

	/// <summary>
	/// Allows the user to remove a previously added callback method
	/// </summary>
	/// <param name="callback">Callback.</param>
    public static void RemoveICadeEventCallback(Action<int> callback)
	{
	    if (_userEventCallbacks != null)
	    {
	        _userEventCallbacks-=callback;
	    }
	}

    /// <summary>
    /// Allows the user to clear up all iCade event callbacks
    /// </summary>
    public static void ClearICadeEventCallbacks()
    {
        _userEventCallbacks = null;
    }

    /// <summary>
    /// Allows the user to add a callback method that's called when a button up event occurs in iCade
    /// </summary>
    /// <param name="callback"></param>
    public static void AddICadeButtonUpCallback(Action<char> callback)
    {
        _userButtonUpCallbacks += callback;
    }


    /// <summary>
    /// Allows the user to remove a previously added callback method
    /// </summary>
    /// <param name="callback"></param>
    public static void RemoveICadeButtonUpCallback(Action<char> callback)
    {
        if (_userButtonUpCallbacks != null)
        {
            _userButtonUpCallbacks -= callback;
        }
    }


    /// <summary>
    /// Allows the user to clear up all iCade button up callbacks
    /// </summary>
    /// <param name="callback"></param>
    public static void ClearICadeButtonUpCallbacks()
    {
        _userButtonUpCallbacks = null;
    }


    /// <summary>
    /// Allows the user to add a callback method that's called when a button down event occurs in iCade
    /// </summary>
    /// <param name="callback"></param>
    public static void AddICadeButtonDownCallback(Action<char> callback)
    {
        _userButtonDownCallbacks += callback;
    }


    /// <summary>
    /// Allows the user to remove a previously added callback method
    /// </summary>
    /// <param name="callback"></param>
    public static void RemoveICadeButtonDownCallback(Action<char> callback)
    {
        if (_userButtonDownCallbacks != null)
        {
            _userButtonDownCallbacks -= callback;
        }
    }


    /// <summary>
    /// Allows the user to clear up all iCade button down callbacks
    /// </summary>
    /// <param name="callback"></param>
    public static void ClearICadeButtonDownCallbacks()
    {
        _userButtonDownCallbacks = null;
    }
    


    /// <summary>
	/// Activates or deactivates iCade support
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public static void Activate(bool state)
	{
#if !UNITY_EDITOR
		_activate(state);
        _registerStateCallback(UnityStateCallBack);
		_registerButtonUpCallback(UnityButtonUpCallBack);
		_registerButtonDownCallback(UnityButtonDownCallBack);
#endif
	}


	public static float GetAxis(string axis)
	{
		switch(axis)
		{
			case "HORIZONTAL":
#if !UNITY_EDITOR
				if ((GetState() & 1<<OnStates.IndexOf('d'))!=0)
				{
					return 1;
				}
				else if ((GetState() & 1<<OnStates.IndexOf('a'))!=0)
			    {
					return -1;
				}
				else
				{
					return 0;
				}
#else
		        if (Input.GetKey(KeyCode.RightArrow))
		        {
		            return 1;
		        }
		        else if (Input.GetKey(KeyCode.LeftArrow))
		        {
		            return -1;
		        }
		        else
		        {
		            return 0;
		        }
#endif

			case "VERTICAL":
#if !UNITY_EDITOR
				if ((GetState() & 1<<OnStates.IndexOf('w'))!=0)
				{
					return 1;
				}
				else if ((GetState() & 1<<OnStates.IndexOf('x'))!=0)
				{
					return -1;
				}
				else
				{
					return 0;
				}
#else
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    return 1;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
#endif
		}

		return 0;
	}


    /// <summary>
    /// Behaves similarly to Unity's Input.GetButtonDown.
    /// Returns true in the frame where the button was initially pressed down (aftwerwards it returns false, even if the button is still down)
    /// Refer to http://www.ionaudio.com/downloads/ION%20Arcade%20Dev%20Resource%20v1.5.pdf to get the list of button values.
    /// You should only check for iCade's Key Down letters (i.e. Y,H,U,J,I,K,O,L).
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
	public static bool GetButtonDown(char button)
	{
#if !UNITY_EDITOR
	    if (_currentState == 0)
	    {
	        return false;
	    }
        
	    int index = OnStates.IndexOf(button);
        int maskedCurrentState = _currentState & (1 << index);
        
        //If button is currently being pressed down
        if (maskedCurrentState != 0)
        {
            //Get the old state of this button
            int maskedOldState = _oldState & (1 << index);
            //if the states are different, it means that this is the first key down event, after a key up.
            return (maskedCurrentState != maskedOldState);
        }

	    return false;
#else
        return Input.GetKeyDown(button.ToString());
#endif
	}

    /// <summary>
    /// Behaves similarly to Unity's Input.GetButtonUp.
    /// Returns true in the frame where the button was initially up (aftwerwards it returns false, even if the button is still up)
    /// Refer to http://www.ionaudio.com/downloads/ION%20Arcade%20Dev%20Resource%20v1.5.pdf to get the list of button values.
    /// You should only check for iCade's Key Down letters (i.e. Y,H,U,J,I,K,O,L). DON'T CHECK for iCade's Key Up letters.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
	public static bool GetButtonUp(char button)
    {
#if !UNITY_EDITOR
        int index = OnStates.IndexOf(button);
        int maskedCurrentState = _currentState & (1 << index);

        //If button is currently not being pressed down
        if (maskedCurrentState == 0)
        {
            //Get the old state of this button
            int maskedOldState = _oldState & (1 << index);
            //if the states are different, it means that this is the first key up event, after a key down.
            return (maskedCurrentState != maskedOldState);
        }

        return false;
#else
        return Input.GetKeyUp(button.ToString());
#endif
	}


    /// <summary>
    /// Behaves similarly to Unity's Input.GetButtonUp.
    /// Returns true if the button is down.
    /// Refer to http://www.ionaudio.com/downloads/ION%20Arcade%20Dev%20Resource%20v1.5.pdf to get the list of button values.
    /// You should only check for iCade's Key Down letters (i.e. Y,H,U,J,I,K,O,L). DON'T CHECK for iCade's Key Up letters.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
	public static bool GetButton(char button)
	{
#if !UNITY_EDITOR
        int index = OnStates.IndexOf(button);
        int maskedCurrentState = _currentState & (1 << index);

		return (maskedCurrentState!=0);
#else
        return Input.GetKey(button.ToString());
#endif
	}

    
    /// <summary>
    /// Gets the state of the iCade. Return is an int that contains all buttons state.
    /// You can then easily find which buttons are pressed by going through ON_STATES or OFF_STATES and anding with 1 leftshifted with the index value of a given char.
    /// </summary>
    /// <returns></returns>
    /*
     * Example of how to retrieve the presed buttons from the state:
     * 
     int length=ON_STATES.Length;
		
		for (int i=length-1;i>=0;i--)
		{
			if ((state & 1<<i)!=0)
			{
				print (iCadeInput.ON_STATES[i]);
			}
		}
     */
	public static int GetState()
	{
		return _getState();
	}


    /// <summary>
    /// Creates an iCade Manager gameobject in the scene, if one doesn't exist. Sets it as DontDestroyOnLoad.
    /// </summary>
    static iCadeInput()
    {
        GameObject iCadeManagerGO = GameObject.Find("iCade Manager");
        if (iCadeManagerGO == null)
        {
            iCadeManagerGO = new GameObject("iCade Manager");
            Object.DontDestroyOnLoad(iCadeManagerGO);
            iCadeManagerGO.AddComponent(typeof (iCadeInput));
        }
    }


    void LateUpdate()
    {
        _oldState = _currentState;
    }

}
