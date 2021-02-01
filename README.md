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

## Use case proposals

<details>
  <summary><b>Base use case</b></summary>

Basic showcase of the engine and commands invocation.

**Commands:**
- turn on the lights<br/>
  output: "I turned on the lights"
- turn off the lights<br/>
  output: "I turned off the lights"

**Technologies**
- CoVoX engine

<hr/>

</details>

**[Robobutler](Robobutler.md)**

**[Pac-Scream](docs/use-cases/pac-scream.md)**

<details>
  <summary><b>Pac-Scream</b></summary>

Pac-Scream is a variant on the popular game Pac-Man in which movements are defined via voice commands instead of keys press.

**Commands:**
- left / move left
- right / move right
- up / move up
- down / move down
- *(proposal)* stop / cancel / no<br/>
  *to cancel the previous command*

**Technologies**
- CoVoX engine
- ASP.NET Core 5
- SignalR
- WebGL

<hr/>

</details>

**Guess-Who**

TODO: browser game<br/>
[Face](https://azure.microsoft.com/en-us/services/cognitive-services/face/)

**[Find-It](Find-It.md)**

**[Voice-Unlock](Voice-Unlock.md)**

