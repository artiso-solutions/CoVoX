# CustomCommands

### About
CustomCommands is a feature provided by Azure Cognitive Service in Azure [SpeechStudio](https://speech.microsoft.com/).
CustomCommands allows you to define commands, and interact with them using your voice.
This work show how you can benefit from this feature, in you own application, and How it can (easily) implemented. 
to a conversational application that is able to run command that you define.

### Getting started
First step for using Custom Commands to create an [Azure Speech resource](https://ms.portal.azure.com/#create/Microsoft.CognitiveServicesSpeechServices) . 
You can author your Custom Commands app on the Speech Studio and publish it, 
After this step an on-device application can communicate with it using the Speech SDK.
[Follow this 10 minutes tutorial](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/quickstart-custom-commands-application)
To start quickly our [SmartRoomLite.json](Configuration/CustomCommandsApp/SmartRoomLite.json) as predefined configuration.

### How to use this demo proceed
1. First creates a new CustomCommands a `New Project` in SpeechStudio, and import the example [SmartRoomLite.json](Configuration/CustomCommandsApp/SmartRoomLite.json) .

2. Provide to the app the required configuration in to the `appsettings.json` file:

```json
{
  "CustomCommandClientConfiguration": {
    "AppId": "your-app-id",
    "SubscriptionKey": "your-subscription-key",
    "Region": "luis-app-prediction-resource-region"
  }
}
```
2. Run the application and say `OK SPEAKER`
3. After the service voice responds, say `TURN ON THE LIGHTS`. 
4. Then the service will return the event defined in the `Completion Rules` of the given `TurnOnOff` command.
   (You can also setup speech responses, or endpoint interaction.. As you prefer). 
5. Have fun!

