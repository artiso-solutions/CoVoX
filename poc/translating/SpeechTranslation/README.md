# Speech translation using TranslationRecognizer

The `TranslationRecognizer` type provided by the SDK merges together the speech-to-text and translation functionalities, otherwise provided by the `SpeechRecognizer` type and the translation APIs. Doing both these operations in one go may improve the performance of the engine.

The `TranslationRecognizer` requires a `SpeechRecognitionLanguage` (the language spoken) and _1..n_ target languages to translate to.

Subscription keys for the Speech service also work with the translation recognizer.

Following are a set of rough perf tests, measuring the time between the readyness of the SDK and the `Translated` event.<br/>
Each translation targeted 3 languages, each test ran 3 iterations of the same sentence:

**Speaking in English**

| Lang       | Event                | Result             |        Delay |
| ---------- | -------------------- | ------------------ | -----------: |
| en-US      | SpeechStartDetected  |                    | 7621.3682 ms |
| en-US      | TranslatingSpeech... | Turn               | 7628.0109 ms |
| en-US      | TranslatingSpeech... | Turn off the       |  8031.113 ms |
| en-US      | Translated           | Turn on the light. | 8232.9611 ms |
| ---------- | ----------           | ----------         |   ---------- |
| en-US      | TranslatingSpeech... | Turn               |  1204.162 ms |
| en-US      | TranslatingSpeech... | Turn on the        | 1606.9234 ms |
| en-US      | Translated           | Turn on the light. |  1807.991 ms |
| ---------- | ----------           | ----------         |   ---------- |
| en-US      | TranslatingSpeech... | Turn               | 2006.6232 ms |
| en-US      | TranslatingSpeech... | Turn on the light  | 2408.9913 ms |
| en-US      | Translated           | Turn on the light. | 2611.1086 ms |

**Speaking in Italian**

| Lang       | Event                | Result            |        Delay |
| ---------- | -------------------- | ----------------- | -----------: |
| it-IT      | TranslatingSpeech... | Apri              | 3412.7599 ms |
| it-IT      | TranslatingSpeech... | Apri la finestra  | 4216.9582 ms |
| it-IT      | Translated           | Apri la finestra. | 4217.6709 ms |
| ---------- | ----------           | ----------        |   ---------- |
| it-IT      | TranslatingSpeech... | Apri              | 1806.0731 ms |
| it-IT      | TranslatingSpeech... | Apri la finestra  | 2209.8643 ms |
| it-IT      | Translated           | Apri la finestra. | 2411.7047 ms |
| ---------- | ----------           | ----------        |   ---------- |
| it-IT      | TranslatingSpeech... | Apri              | 1805.4636 ms |
| it-IT      | TranslatingSpeech... | Apri la finestra  | 2207.9355 ms |
| it-IT      | Translated           | Apri la finestra. | 2409.2259 ms |

The results show approssimate ~2s for the operation to complete.
