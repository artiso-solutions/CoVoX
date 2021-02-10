## Translating
Using the Translator component of the Azure Cognitive Services it is possible to translate a text from one language to one or multiple other languages.

## Usage
A HTTP request is made to the translator endpoint passing the API version, the language from which it should be translated (optional) and the language to be translated to.

https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&from=de&to=en

if the "from=" part of the url isn't specified, the from language will be detected automatically and the response will contain the detected language with a confidence score.

### Response example
- From "de" to "en":
```
{
    "translations":
    [
        {
            "text" : "Turn on the light",
            "to" : "en"
        }
    ]
}
```
- To "en" with auto-detection of source language:
```
{
    "detectedLanguage":
    {
        "language" : "de",
        "score" : 1.0
    },
    "translations":
    [
        {
            "text" : "Turn on the light",
            "to" : "en"
        }
    ]
}
```

## Requirements
A subscription key for Azure Cognitive Services Translator