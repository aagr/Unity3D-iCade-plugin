//
//  iCadeManagerBindings.c
//  Unity-iPhone
//
//  Created by Alexandre Ribeiro on 7/16/13.
//
//

#include <stdio.h>
#include "iCadeManager.h"


void _activate(BOOL state)
{
    [[iCadeManager sharedManager] activate:state];
}

void _registerStateCallback(UnityCallback cntCallback)
{
    [[iCadeManager sharedManager] setUnityStateCallback:cntCallback];
}

void _registerButtonUpCallback(UnityCallback cntCallback)
{
    [[iCadeManager sharedManager] setUnityButtonUpCallback:cntCallback];
}

void _registerButtonDownCallback(UnityCallback cntCallback)
{
    [[iCadeManager sharedManager] setUnityButtonDownCallback:cntCallback];
}

int _getState()
{
    return ([[iCadeManager sharedManager] getState]);
}