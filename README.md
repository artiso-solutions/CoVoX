# CoVoX

**Cloud enabled library providing a customizable voice-interface for your application or your device**

![CoVoXSimpleGraph](assets/CoVoXSimpleGraph.png)

Covox allows the interaction with an application or device through voice.<br/>
You provide a list of _Commands_, i.e. operations that can be invoked via the voice interface,
Covox then listens to the audio and when a command matches with the spoken words, it's executed.
It also has multi-language support!

![CoVoXMultiLanguageGraph](assets/CoVoXMultiLanguageGraph.png)

With some imagination you could speak to a calculator, a virtual assistant, or a CRM application!

## How it works

![CoVoXHowItWorks](assets/CoVoXHowItWorks.png)

1) define commands and provide them to a `CovoxEngine` instance
2) start the audio capture by calling the method `covox.StartAsync`
3) covox will translate and recognize the input, and then it will emit the event `Recognized`
4) execute the logic connected to the detected command

## Getting started

Covox is offered as .NET library and acts on behalf of the [Azure Cognitive Services](https://azure.microsoft.com/services/cognitive-services/), therefore to use it you will need:

- an Azure Cognitive Services subscription key (follow this [guideline](https://azure.microsoft.com/try/cognitive-services/))
- a .NET project or application
- a device connected to internet
- a device with a working microphone

In order to get started, take a look at the [samples](/samples).

## How to use

Consider a simple use case: a voice-controlled light-switching application.

- define the available commands, with unique IDs and one or many voice triggers (in English):

```csharp
var turnOnLightCmd = new Command
{
    Id = "TurnOnLight",
    VoiceTriggers = new[] { "turn on the light", "light on", "on" }
};

var turnOffLightCmd = new Command
{
    Id = "TurnOffLight",
    VoiceTriggers = new[] { "turn off the light", "light off", "off" }
};
```

- create an instance of `CovoxEngine`:

```csharp
var covox = new CovoxEngine(new Configuration
{
    AzureConfiguration = AzureConfiguration.FromSubscription(
        subscriptionKey: YOUR_SUBSCRIPTION_KEY,
        region: YOUR_REGION),

    // Define all the languages that can be regognized
    InputLanguages = new[] { "en-US", "de-DE", "it-IT", "es-ES" },
});

covox.RegisterCommands(turnOnLightCmd, turnOffLightCmd);
```

- define a delegate for when a command is recognized:

```csharp
covox.Recognized += (cmd, ctx) =>
{
    if (cmd == turnOnLightCmd) { // ... }
    else if (cmd == turnOffLightCmd) { // ... }
};

await covox.StartAsync();
```

## Use case scenarios

<details>
  <summary><b>Basic</b></summary>

### LightSwitch

Basic showcase of the engine and commands invocation.

### Commands

- turn on the lights<br/>
  output: "I turned on the lights"
- turn off the lights<br/>
  output: "I turned off the lights"

### Technologies

- CoVoX engine

<hr/>

</details>

<details>
  <summary><b>Web application</b></summary>

### Pac-Scream

Pac-Scream is a variant on the popular game Pac-Man, in which movements are defined via voice commands instead of keys press.

![image](https://user-images.githubusercontent.com/8939890/106443307-9e549e00-647c-11eb-921f-dd25ed5d0bfb.png)

### Commands

- left / move left
- right / move right
- up / move up
- down / move down
- stop / cancel / no<br/>
  to cancel the previous command

### Technologies

- CoVoX engine
- ASP.NET Core 5
- SignalR
- WebGL

<hr/>

</details>

<details>
  <summary><b>Mobile application</b></summary>

### Find-it

Find-it it's a Mobile App that is able to recognize objects in an image, or in a video, from user voice request.
Given an image or a video, if the user requests to see a particular object, the application will create a box around the object that match the description.

### Technologies

- CoVoX engine
- [Flutter](https://flutter.dev/?gclid=CjwKCAiAgc-ABhA7EiwAjev-j209M2n1IrpNH86tVHhSkPU5ED2KyUM6Rj8IkBVu2N8kD-fgoxIC_RoCuI4QAvD_BwE&gclsrc=aw.ds)
- [Azure computer vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/)

<hr/>

</details>

<details>
  <summary><b>AI/Machine Learning</b></summary>

### Guess-Who

Guess Who is a game for 2 players.  Each player has a "playing field" with different people and a fixed person, which must be guessed by the opponent, by exclusion questions.
Via Voice commands you should be able to ask a question, such as, "Does the woman have red hair?"
Image recognition should then return the answer yes / no.

<img alt="drawing" src="https://user-images.githubusercontent.com/8939890/106584417-716cbd80-6546-11eb-8fe4-40b047dee3c4.png" height="250" width="400">

### Procedure

1. Asking a Question via Voice Command
2. Recognize and process question
3. Looking at e.g. Image and detect the answer
4. Returning Answer (Yes / No)

### Technologies

- CoVoX engine
- Python / Tensorflow
- [Face](https://azure.microsoft.com/en-us/services/cognitive-services/face/)

<hr/>

</details>

<details>
  <summary><b>Security</b></summary>

### Voice-Unlock

Voice-Unlock showcases the voice recognition service from azure. An application will display a locked lock. If the authorized user says "Unlock", the lock should unlock. Instead, if an unauthorized users says "Unlock" the background flashes a few seconds in red.

### Technologies

- CoVoX engine
- [Speaker Recognition](https://azure.microsoft.com/en-us/services/cognitive-services/speaker-recognition/)
- VueJS application

<hr/>

</details>

<details>
  <summary><b>External device</b></summary>

### Robobutler

Robobutler is a robot capable of executing voice triggered actions based on its perception of the current environment. The idea is that an operator can tell the robot to "Bring me the yellow box" and the robot will in this case do the following:

1. Confirm/Repeat the task the robot was told to do
2. Go to the yellow box
3. Pick it up
4. Bring it to the operator

### Other possible scenarios

- Placing a box on top of another
- Basic movements (Stop, rotate, etc)
- Spatial awarness (e.g. go to the nearest corner)

### Benefit to the real world

In the real world you could have a warehouse with a lot of heavy weight packages. Working in a human-robot collaboration environment the human would be able to control the robot either with a controller or by voice. Adding intelligence to the robot does simplify the interaction with the robot increasing the overall productivity and performance of the human and the facility. Furthermore it enables the human do multitask.

### Robo to use

https://www.dji.com/de/robomaster-s1

The desired configuration would be an industrial arm on top of a body with wheels to represent a valid scenario for the industry.

### Technologies

- CoVoX engine
- [Azure computer vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/)
- Python (to control the robot)

<hr/>

</details>

## Technologies

The library is developed in .NET 5 and uses the Azure's Cognitive Services.
