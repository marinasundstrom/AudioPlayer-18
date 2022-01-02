# Audio Player

Cross-platform app for Axis Audio Player. 

[Demo video](https://youtu.be/WcQDYZiyFVw)

## Background

This was a side-project when I worked as a consultant at Axis Communications in Lund, back in 2017-2018.

The motivation was that I wanted to learn software design and mobile app development in .NET and Xamarin.Forms.

## Disclaimer

Although it uses the word Axis, it is in no way affiliated with the company.

I originally asked if I could publish the source code. The only thing that they requested of me in return was to removed the API calls just to be sure. At first, I thought I could just remove them and polish stuff up, but it proved not to be a fun thing to do. The API client layer is not the prettiest that I have made. So I just left the project there.

Now, in 2022, I figure that the APIs are dated, and probably replaced by Axis new cloud initiative. So here is the code!

There is not guarantee that this app will work with current speakers and firmware.

## The project

The project consists of a Xamarin.Forms app that is structured according to the Model-View-ViewModel (MVVM) pattern. The MVVM library is custom-made. The app also uses Reactive Extensions (RX).

The app invokes some undocumented Web APIs that was being used by the original Web App that is on-device.

## Features

These are the features of the app:

* Music player - Controling speaker
* Library - for listing on-device tracks
* Device-discovery with ZeroConf - Discovers speakers
* Zones - grouping and controling devices (a feature of the device)
