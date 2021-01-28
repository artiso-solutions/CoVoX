# CoVoX

A plug-in enabled framework for voice-activated commands.

## Overview

With the use of cloud cognitive services, this engine enables to convert voice instructions into actions or intents defined by a use case.

The engine follow these stages:

1) **listening:** translates voice into text<br/>
  using [Speech to Text](https://azure.microsoft.com/en-us/services/cognitive-services/speech-to-text/)

2) **translating:** allows the user to speak in any language, by transforming the sentence into a base language (english)<br/>
  using [Text Translation](https://azure.microsoft.com/en-us/services/cognitive-services/translator)<br/>
  using [Speech Translation](https://azure.microsoft.com/en-us/services/cognitive-services/speech-translation/)

3) **understanding:** matches the intent to an available command given the current context<br/>
  using [Language Understanding
](https://azure.microsoft.com/en-us/services/cognitive-services/language-understanding-intelligent-service/)

4) **executing:** invokes the command using the use case's logic

## Use case proposals:

### Base use case

Say: "turn on the lights"
Output: "I turned on the lights"

Say: "turn off the lights"
Output: "I turned off the lights"

### Robobutler

TODO: control a robot, do stuff like: "bring me the yellow box"

### Pac-Scream

TODO: browser game

### Guess-Who

TODO: browser game<br/>
[Face](https://azure.microsoft.com/en-us/services/cognitive-services/face/)

### Find-It

TODO: Flutter app, using video stream<br/>
[Computer Vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/)

### Voice-Unlock

TODO: only the owner can open its lock<br/>
[Speaker Recognition](https://azure.microsoft.com/en-us/services/cognitive-services/speaker-recognition/)

