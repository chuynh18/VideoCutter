# VideoCutter

** This is a work in progress and currently has zero functionality.  Stay tuned!**

## What is it?

VideoCutter is a (probably) badly-named barebones front-end for [FFMpeg](https://ffmpeg.org/).  FFMpeg is a powerful tool for processing media files.  However, as a commandline tool, it is not very accessible for the general population.  As a result, many popular front-ends have been written for FFMpeg, the most famous of which is [HandBrake](http://handbrake.fr/).

However, no front-end can hope to expose all the functionality of FFMpeg and most front-ends attempt to expose the various encoding functionalities.  VideoCutter hopes to expose some of the lesser-known but equally useful and powerful features:
* Cutting video without requiring re-encoding
* Merging multiple video files of the same format without requiring re-encoding
* Extracting the audio and video tracks from a muxed media file

## Why should I care?

While it is possible to cut video with current tools or FFMpeg front-ends, most tools implicitly require the user to re-encode the file.  This can take significantly longer and can impose some amount of quality loss.  FFMpeg has the capability to cut the audio and video tracks, then repackage them into the same container format as the original file.  This results in zero quality loss, and the speed of this operation is limited purely by the I/O capability of the user's machine.  In other words and as an example, it can be possible to extract 15 minutes of video from a 4k or 8k movie with no loss in quality in just a few seconds.  On the other hand, with re-encoding, this same operation can take several minutes to over an hour.

## Misc info and implementation details

This application is a simple [WPF](https://docs.microsoft.com/en-us/dotnet/framework/wpf/) front-end for FFMpeg.  I just wanted to play around with C# outside the context of Unity and throw together a simple Windows application.  Take it or leave it!

The FFMpeg command for cutting video has the following syntax:
`ffmpeg -ss [amount of time to seek in seconds] -i [path to input video] -c copy -t [amount of time to cut in seconds] [path to output]`

This is unwieldy to remember.  All VideoCutter does is exposes a drag-and-drop UI and some TextBoxes so that the user can input their desired parameters.  It will then construct the arguments list and pass that on to FFMpeg.

## Future development

Hopefully, merging identically formatted videos and extracting individual media tracks will come next.

Potentially, I'd also like to add a much more intuitive interface for cutting video.  The current design requires the user to open the video file up in a separate media player to find the timestamps.  They then have to convert the timestamps to seconds, and type that into VideoCutter.  This is unfriendly; it would be far easier to have a video player built right into the application.