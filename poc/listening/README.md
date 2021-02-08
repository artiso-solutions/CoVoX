## Listening

### Requirements
An Azure Cognitive Services Speech subscription key is required

### Recognition modes
There are different possibilities for speech-to-text recognition, depending on:
+ Audio input source:
    - [From microphone](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=script%2Cbrowser%2Cwindowsinstall&pivots=programming-language-csharp#recognize-from-microphone:~:text=configuration.-,Recognize%20from%20microphone): issues may arise when the microphone is in use by another application
    - [From file](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=script%2Cbrowser%2Cwindowsinstall&pivots=programming-language-csharp#recognize-from-file:~:text=Recognize%20from%20file,-If)
    - [From in-memory stream](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=script%2Cbrowser%2Cwindowsinstall&pivots=programming-language-csharp#recognize-from-in-memory-stream:~:text=Recognize%20from%20in%2Dmemory%20stream,-For): accepts an AudioStream in form of a byte array
+ Recognition mode
    - [Single-shot detection](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=script%2Cbrowser%2Cwindowsinstall&pivots=programming-language-csharp#recognize-from-microphone:~:text=configuration.-,Recognize%20from%20microphone) stops after a listening silence at the end or a maximum of 15 seconds
    - [Continuous recognition](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=script%2Cbrowser%2Cwindowsinstall&pivots=programming-language-csharp#continuous-recognition:~:text=Continuous%20recognition,-The) has to be started and stop and has recognition events

### Source language
The source language has to be specified in the speech configuration [Read more](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-speech-to-text?tabs=script%2Cbrowser%2Cwindowsinstall&pivots=programming-language-csharp#change-source-language:~:text=Change%20source%20language,-A)

It is also possible to enable automatic source language detection, for this a list of languages can be specified. At the time of writing there is a service side limitation for auto source language detection of 4 languages per recognition [Read more](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-automatic-language-detection?pivots=programming-language-csharp#automatic-language-detection-with-the-speech-sdk:~:text=Automatic%20language%20detection%20currently%20has%20a%20services%2Dside%20limit%20of%20four%20languages%20per%20detection.)



### Links
- Speech [HowToUseAudioInputAsStream](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-use-audio-input-streams)
- [Stream to Server](https://docs.microsoft.com/en-us/aspnet/core/signalr/streaming?view=aspnetcore-5.0)
