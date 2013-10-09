//
//  iCadeManager.h
//  Unity-iPhone
//
//  Created by Alexandre Ribeiro on 7/16/13.
//
//

#import <Foundation/Foundation.h>

typedef void (*UnityCallback)(int state);

@interface iCadeManager : NSObject

+(id) sharedManager;
-(void) activate:(BOOL)state;
-(void) delayedActivate:(NSNumber*)num;
-(void) setUnityStateCallback:(UnityCallback)callback;
-(void) setUnityButtonUpCallback:(UnityCallback)callback;
-(void) setUnityButtonDownCallback:(UnityCallback)callback;
-(int) getState;

@end
