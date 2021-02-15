# CustomCommands

### About
CustomCommands is a feature provided by Azure Cognitive Service team as a [SpeechStudio](https://speech.microsoft.com/) feature.
CustomCommands allows you to define commands, and interact with them using your voice.
This work show how you can benefit from this feature, in you own application, and How it can (easily) implemented. 
to a conversational application that is able to run command that you define.

### Getting started
First step for using Custom Commands to create an [Azure Speech resource](https://ms.portal.azure.com/#create/Microsoft.CognitiveServicesSpeechServices) . 
You can author your Custom Commands app on the Speech Studio and publish it, 
after which an on-device application can communicate with it using the Speech SDK.
[Follow this 10 minutes tutorial](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/quickstart-custom-commands-application)
Import our [SmartRoomLite.json](Configuration/CustomCommandsApp/SmartRoomLite.json) as predefined configuration.

### How to use this demo proceed
1. First creates a new CustomCommands app in SpeechStudio, and import the example `customCommandsAppConfig.json`.

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
4. Then service will respond `COMMAND TURN ON DETECTED`
5. Have fun!

