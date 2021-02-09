## Listening
### Intent Recognition

#### Requirements
In contrast to other services supported by the Cognitive Services Speech SDK, the Language Understanding service
requires a specific subscription key from https://www.luis.ai/.

#### How to Proceed
To Use IntentRecognition is it necessary to create a Language Understanding service (LUIS), 
application offered by Azure as Service, built on top of the CognitiveServices platform.
It is required to configure intents in the Luis platform.

#### Multi-language IntentRecognizer: Multi-Language support for LUIS apps
It supports only one language per time. If you need a multi-language LUIS client application
such as a chatbot, you have a few options.
If LUIS supports all the languages, you develop a LUIS app for each language. 
Each LUIS app has a unique app ID, and endpoint log. 
If you need to provide language understanding for a language LUIS does not support, 
you can use the Translator service to translate the utterance into a supported language, 
submit the utterance to the LUIS endpoint, and receive the resulting scores.
[ReadMore...](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-language-support)
