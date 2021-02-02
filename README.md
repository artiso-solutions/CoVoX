# CoVoX 
### Cloud enabled library providing a customizable voice-interface for your application or your device

Covox allows the interaction with an application or device through voice.

![DraftDraw](https://raw.githubusercontent.com/artiso-solutions/CoVoX/doc/assets/CoVoXGraph.png)

You provide a list of _Commands_, i.e. operations that can be invoked via the voice interface,
Covox then listens to the audio and when a command matches with the spoken words, it's executed.
It also has multi-language support!

With some imagination, you could control a calculator, or a virtual assistant, or even CRM application!

## Sample

Taking a simple usecase (light switch application): the client application must use Covox and configure it with a set of commands (i.e. switch on the light, switch off the light). Each of the commands will have a unique identifier and a set of voice triggers.  

| Identifier   |      Voice Triggers
|----------|:-------------:
| TURN_ON_COMMAND |  switch on the light, turn on light ...
| TURN_OFF_COMMAND |    switch off the light, turn off light ...

<br>
The application starts the Covox engine audio recognition that listens for voice triggers. 
When Covox detects that a voice trigger matches one of the given commands voice trigger [i.e. switch on the light], it returns the corresponding command (i.e. TURN_ON_COMMAND) to the client application.

Then the application will perform the action matching with the recognized command.

![DraftDraw](https://raw.githubusercontent.com/artiso-solutions/CoVoX/doc/assets/CovoxDrawDraw.png)

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

<details>
  <summary><b>Pac-Scream</b></summary>

Pac-Scream is a variant on the popular game Pac-Man, in which movements are defined via voice commands instead of keys press.

![image](https://user-images.githubusercontent.com/8939890/106443307-9e549e00-647c-11eb-921f-dd25ed5d0bfb.png)

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

<details>
  <summary><b>Guess-Who</b></summary>

Guess Who is a game for 2 players.  Each player has a "playing field" with different people and a fixed person, which must be guessed by the opponent, by exclusion questions.  
Via Voice commands you should be able to ask a question, such as, "Does the woman have red hair?" 
Image recognition should then return the answer yes / no. 

![image](https://cdn-gamesworldau.pressidium.com/wp-content/uploads/2020/05/guess-who-2.jpg)

**Procedure:**
1. Asking a Question via Voice Command
2. Recognize and process question
3. Looking at e.g. Image and detect the answer 
4. Returning Answer (Yes / No)

**Technologies**
- CoVoX engine
- Python / Tensorflow 

<hr/>

</details>

[Face](https://azure.microsoft.com/en-us/services/cognitive-services/face/)

**[Find-It](Find-It.md)**

**[Voice-Unlock](Voice-Unlock.md)**

