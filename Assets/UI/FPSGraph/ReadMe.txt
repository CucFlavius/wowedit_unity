FPS Graph - Performance Analyzer Tool

To use FPS Graph simply add this script to your main camera. 
   detailed explanation: Select the camera used in your scene then go to the inspector window click on the component menu and down to scripts you should find FPSGraphC.

 Options:
    Show Performance on Click: The performance breakdown shows up when you click on the graph
    Audio Feedback: This allows you to audibly "visualize" the performance of the scene
    Audio Feedback Volume: This specifies how loud the audio feedback is, from 0.0-1.0
    Graph Multiply: This specifies how zoomed in the graph is, the default is the graph is multiplyed by 2x, meaning every pixel is doubled.
    Graph Position: This specifies where the graph sits on the screen examples: x:0.0, y:0.0 (top-left) x:1.0, y:0.0 (top-right) x:0.0, y:1.0 (bottom-left) x:1.0 y:1.0 (bottom-right)
    Frame History Length: This is the length of FPS that is displayed (Set this to a low amount if you are targeting older mobile devices)
    Use Min FPS: Check this option if you don't want the graph to be constantly re-alligning with what the new minimum fps is on the screen.
