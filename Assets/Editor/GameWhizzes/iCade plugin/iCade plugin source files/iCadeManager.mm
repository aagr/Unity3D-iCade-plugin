//
//  iCadeManager.m
//  Unity-iPhone
//
//  Created by Alexandre Ribeiro on 7/16/13.
//
//

#import <objc/runtime.h>
#import <Foundation/Foundation.h>
#import "iCadeManager.h"
#import "iPhone_View.h"
#import "iCade/iCadeReaderView.h"

void UnitySendMessage( const char * className, const char * methodName, const char * param );
void UnityPause( bool pause );
UIViewController *UnityGetGLViewController();

@interface iCadeManager() <iCadeEventDelegate>
{
    UnityCallback stateCallback;
    UnityCallback buttonUpCallback;
    UnityCallback buttonDownCallback;
    iCadeReaderView *iCadeView;
}
@property (nonatomic,assign) BOOL activated;
@property (nonatomic,assign) NSTimer *timer;

@end



@implementation iCadeManager



//http://www.galloway.me.uk/tutorials/singleton-classes/
+ (id)sharedManager {
    static iCadeManager *sharedMyManager = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedMyManager = [[self alloc] init];
    });
    return sharedMyManager;
}

-(void) activate:(BOOL)state
{
    NSLog(@"Unity GLView=%@",UnityGetGLView());
    
    //If activate is called in Monobehaviour.Start(), we're not able to set the text box that answers to iCade input as the firstResponder until we wait at least one frame, so we wait a small ammount of time here if we're activating. To deactivate we can do it immediately.
    NSNumber *num=[NSNumber numberWithBool:state];
    [NSObject cancelPreviousPerformRequestsWithTarget:self];
    if (state)
    {
        [self performSelector:@selector(delayedActivate:) withObject:num afterDelay:0.1];
    }
    else
    {
        [self delayedActivate:num];
    }
    
}


-(void) delayedActivate:(NSNumber*)num
{
    
    BOOL state=[num boolValue];
    if (state)
    {
        if (!self.activated)
        {
            self.activated=TRUE;
            iCadeView=[[iCadeReaderView alloc] initWithFrame:CGRectZero];
            [UnityGetGLView() addSubview:iCadeView];
            iCadeView.active=YES;
            iCadeView.delegate=self;
            iCadeView.hidden=NO;
            [iCadeView release];
        }
        else
        {
            NSLog(@"iCade plugin already activated.");
        }
    }
    else
    {
        if (self.activated)
        {
            self.activated=FALSE;
            [iCadeView removeFromSuperview];
            iCadeView.hidden=YES;
        }
        else
        {
            NSLog(@"iCadePlugin already deactivated.");
        }
        
    }
}

-(void) setUnityStateCallback:(UnityCallback)callback
{
    self->stateCallback=callback;
}

- (void)stateChanged:(iCadeState)state
{
    if (stateCallback!=nil)
    {
        stateCallback(state);
    }
}

-(void)setUnityButtonDownCallback:(UnityCallback)callback
{
    self->buttonDownCallback=callback;
}

-(void)setUnityButtonUpCallback:(UnityCallback)callback
{
    self->buttonUpCallback=callback;
}


- (void)buttonDown:(iCadeState)state
{
    buttonDownCallback(state);
}

- (void)buttonUp:(iCadeState)state
{
    buttonUpCallback(state);
}

-(int)getState
{
    return iCadeView.iCadeState;
}


@end
