# AtmoUnityAPI

AtmoUnityAPI interfaces Atmo with Unity. AtmoTracking provides marker events, AtmoLight is used to control the light strip in Atmo.

## Getting started

1. Download the repository. 
2. Create a new Unity project.
3. Add a new resolution on the Game view with fixed resolution of 1280x800.

![Add new resolution](/readme-imgs/atmo-resolution.png)

4. Go to Edit | Project Settings | Player | PC, Mac & Linux Standalone settings | Other Settings | Optimization | API Compatibility Level and select ".Net 2.0".
5. Copy the content of the AtmoUnityAPI-master/Assets folder in your Unity project's Assets folder.
6. (a) Open the Example scene in the Example folder. 
6. (b) Start development with the Template scene in the AtmoUnityAPI folder.

## AtmoTracking

The AtmoTracking gameobject is responsible for marker tracking. The TrackingHandler script provides marker detection information with the help of three events with a Marker object argument: onDetected, onRedetected and onLost. 

#### onDetected

This event is invoked when a new marker is detected on the table.

#### onLost

This event is invoked when a marker disappears from the table. This can also happen if a marker is hidden by another object or by someone's hand.

#### onRedetected

Lost markers are saved in a history for 5 seconds. In case the same marker appears on the same location within this 5 seconds, this event is invokeded instead of the onDetected.

#### Marker object

The marker object contains the following information about a marker:

```
int markerID;       // id of the marker, same for identical markers
int uniqueID;       // unique id of detection, remains the same for all three events invoked by the same detection
Vector2 Position;   // postion of the marker in Unity world space
```

#### Usage

Listeners to the events can be either added on the editor or in script.

![Add listeners](/readme-imgs/tracking-events.png)

```
GetComponent<TrackingHandler>().onDetected.AddListener(HandleOnDetected);
GetComponent<TrackingHandler>().onReDetected.AddListener(HandleOnRedetected);
GetComponent<TrackingHandler>().onLost.AddListener(HandleOnLost);

void HandleOnDetected(Marker marker) {
  //Your code to handle new markers
}

void HandleOnRedetected(Marker marker) {
  //Your code to handle redetected markers
}

void HandleOnLost(Marker marker) {
  //Your code to handle lost markers
}
```

## AtmoLight

