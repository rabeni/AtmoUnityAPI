# AtmoUnityAPI

AtmoUnityAPI interfaces Atmo with Unity. [AtmoTracking](/README.md#atmotracking) is responsible for providing information about marker detections, [AtmoLight](/README.md#atmolight) is used to control the light strip in Atmo.

## Getting started

1. Download the repository. 
2. Download the [Atmo](https://drive.google.com/open?id=1hCropZ12itlpUQ8lhq1flumJUgpD3bh8) application.
3. Create a new Unity project.
4. Add a new resolution on the Game view with fixed resolution of 1280x800.

![Add new resolution](/readme-imgs/atmo-resolution.png)

5. Go to Edit | Project Settings | Player | PC, Mac & Linux Standalone settings | Other Settings | Optimization | API Compatibility Level and select ".Net 2.0".
6. Copy the content of the AtmoUnityAPI-master/Assets folder in your Unity project's Assets folder.
7. (a) Open the [Example](/README.md#exemple) scene in the Example folder. 
7. (b) Start development with the Template scene in the AtmoUnityAPI folder.
8. Start the Atmo application anc click Tracking to get marker detection information in Unity. If more cameras are attached to your computer, you'll have to choose the right camera id in the application.

## AtmoTracking

The AtmoTracking gameobject is responsible for marker tracking. The TrackingHandler script provides marker detection information with the help of three events with a Marker object argument: onDetected, onRedetected and onLost. 

#### onDetected event

This event is invoked when a new marker is detected on the table.

#### onLost event

This event is invoked when a marker disappears from the table. This can also happen if a marker is hidden by another object or by someone's hand.

#### onRedetected event

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

The AtmoLight gameobject is responsible for controlling the 144 pixel LED strip in Atmo. 

#### Usage

The public functions in the Strip script can be used for controlling the LED pixels. Best way to use it is to add a new script to AtmoLight, get the Strip script from there and call the needed functions.

```
Strip strip;

void Start() {
  strip = GetComponent<Strip>();
  
  // This sets all pixel colors to red
  strip.SetAll(new Color32(200,0,0,255));
}
```

Functions usally take a Vector3 or Color32 color argument. In case of Vector3, (x,y,z) attributes stand for (r,g,b). In case of Color32, the a (alpha) argument is only used for the virtual representation of the LED pixels but not for the real ones.

LED pixels are also visualised on the Scene as spheres. This can help to sync orientation of light animations with projections.

![LED pixels on Scene](/readme-imgs/leds.png)

## Example

Open the Example scene in the Example folder. This basic application provides a basic summary of the AtmoUnityAPI. It highlight markers on the table with the corresponding colors denoted on the interface. Furthermore, lights can be controled with buttons and sliders on the screen.

![Example scene](/readme-imgs/example.png)



