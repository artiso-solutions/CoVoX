## Listening
### Intent Recognition

#### Requirements
In contrast to other services supported by the Cognitive Services Speech SDK, the Language Understanding service
requires a specific subscription key from https://www.luis.ai/.

#### How to Proceed
To Use IntentRecognition is it necessary to create a Language Understanding service (LUIS), 
application offered by Azure as Service, built on top of the CognitiveServices platform.
It is required to configure intents in the Luis platform.
After your LUIS app is created and published (as Production or Staging env.) you can use the simple test created here by providing the Luis configuration as follow:

```c#

public class LanguageUnderstandingClientConfiguration
    {
        public string EndPoint { get; init; }

        public string AppId { get; init; }

        public string PredictionKey { get; init; }

        public  LanguageUnderstandingClientEnv  Env { get; init; }

        public string Region { get; init; }
    }

```

#### How to Create a LUIS app
Follow this instructions to create a LUIS app.
<br>[ReadMore...](LUIS/README.md)

#### Multi-language IntentRecognizer: Multi-Language support for LUIS apps
It supports only one language per time. If you need a multi-language LUIS client application
such as a chatbot, you have a few options.
****If LUIS supports all the languages, you develop a LUIS app for each language. 
Each LUIS app has a unique app ID, and endpoint log****. 
If you need to provide language understanding for a language LUIS does not support, 
you can use the Translator service to translate the utterance into a supported language, 
submit the utterance to the LUIS endpoint, and receive the resulting scores.
<br>[ReadMore...](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-language-support)
