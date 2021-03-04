## Translating
Using the Translator component of the Azure Cognitive Services it is possible to translate a text from one language to one or multiple other languages.

## Usage
A HTTP request is made to the translator endpoint passing the API version, the language from which it should be translated (optional) and the language to be translated to.

https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&from=de&to=en

if the "from=" part of the url isn't specified, the from language will be detected automatically and the response will contain the detected language with a confidence score.

### Response example
From "de" to "en":
```json
{
   "translations": [
      {
         "text": "Turn on the light",
         "to": "en"
      }
   ]
}
```
To "en" with auto-detection of source language:
```json
{
   "detectedLanguage": {
      "language": "de",
      "score": 1.0
   },
   "translations": [
      {
         "text": "Turn on the light",
         "to": "en"
      }
   ]
}
```

## Perf
The `ApiPerfScenario` tests how long the API takes to translate a sentence (from any language, default use cases in Italian) to English.<br/>
The results show that the API returns in ~115ms, excluding the initial call that takes longer (defined in the test as the wake-up call):

|       | Sentence                                     | Time             |
| ----- | -------------------------------------------- | ---------------- |
| Input | Svegliati                                    |                  |
|       | Wake up                                      | 00:00:00.9473747 |
| Input | Ciao mondo                                   |                  |
|       | Hello world                                  | 00:00:00.1126020 |
|       | Hello world                                  | 00:00:00.1120887 |
|       | Hello world                                  | 00:00:00.1108917 |
| Input | Giù                                          |                  |
|       | Down                                         | 00:00:00.1112989 |
|       | Down                                         | 00:00:00.1115169 |
|       | Down                                         | 00:00:00.1113381 |
| Input | Accendi la luce                              |                  |
|       | Turn on the light                            | 00:00:00.1111561 |
|       | Turn on the light                            | 00:00:00.1123014 |
|       | Turn on the light                            | 00:00:00.1109371 |
| Input | 42 è la risposta alla domanda fondamentale   |                  |
|       | 42 is the answer to the fundamental question | 00:00:00.1124080 |
|       | 42 is the answer to the fundamental question | 00:00:00.1120873 |
|       | 42 is the answer to the fundamental question | 00:00:00.1143339 |

The test was run with a free-tier, a S4-tier and a multiple-service API key: the invocation fell within the same time range, with no signs of changes in performance.
