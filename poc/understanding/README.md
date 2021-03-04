## Understanding

### F23.String Similarity Algorithms

#### Metric vs. Normalized 
Normalized Similarities define a similarity between strings between 0 and 1.0 while
Metric Similarities define distances in the metric system, since they verify the triangle inequality (d(x, y) <= d(x,z) + d(z,y).

Examples for Normalized Similarity Algorithms are:
- Normalized Levenshtein
- Jaro Winkler
- N-Gram
- Cosine Similarity

Examples for Metric Similarity Algorithms are:
- Levenshtein
- Damerau-Levenshtein
- Metric Longest Common Subsequence

Metric Similarity Algorithms are not really applicable to our usecase since they don't return a percentage value.

Weighted Algorithms like weighted Levenshtein allow the definition of different weights for different character substitutions.
This is e.g. used in OCR where P and R are quite similar while P and M are not.
The definition of these weights is not really needed in our usecase.

### Fuzzy Sharp

Fuzzy Sharp is a NuGet Package for matching string Similarities. 
The Algorithm uses Levenshtein Distance to calculate similarity between strings.

The WeightedRatio() Method is the Method we are using most, because it already has an internal weighting.

### Test Results

| Algorithm                        | Library              | Normalized | Target            | Input             | Score |
| -------------------------------- | -------------------- | ---------- | ----------------- | ----------------- | ----- |
| FuzzyWeightedRatio               | FuzzySharp           | yes        | Turn on the Light | Turn on the Light | 1     |
| CosineSimilarity                 | F23.StringSimilarity | yes        | Turn on the Light | Turn on the Light | 1     |
| JaroWinklerSimilarity            | F23.StringSimilarity | yes        | Turn on the Light | Turn on the Light | 1     |
| NGramSimilarity                  | F23.StringSimilarity | yes        | Turn on the Light | Turn on the Light | 1     |
| NormalizedLevenshteinSimilarity  | F23.StringSimilarity | yes        | Turn on the Light | Turn on the Light | 1     |
| MetricLCSSimilarity              | F23.StringSimilarity | yes        | Turn on the Light | Turn on the Light | 1     |
| QGramSimilarity                  | F23.StringSimilarity | no         | Turn on the Light | Turn on the Light | 1     |
| DamerauSimilarity                | F23.StringSimilarity | no         | Turn on the Light | Turn on the Light | 1     |
| LevenshteinSimilarity            | F23.StringSimilarity | no         | Turn on the Light | Turn on the Light | 1     |
| LongestCommonSubsequenceSimilarity | F23.StringSimilarity | no         | Turn on the Light | Turn on the Light | 1     |


| Algorithm           | Library             | Normalized | Target            | Input              | Score              |
| ------------------- | ------------------- | ---------- | ----------------- | ------------------ | ------------------ |
| FuzzyWeightedRatio  | FuzzySharp          | yes        | Turn on the Light | Turn the Light on. | 0,92               |
| CosineSimilarity    | F23.StringSimilarity | yes        | Turn on the Light | Turn the Light on. | 0,9146591207600472 |
| JaroWinklerSimilarity  | F23.StringSimilarity | yes        | Turn on the Light | Turn the Light on. | 0,8513072199291654 |
| NGramSimilarity     | F23.StringSimilarity | yes        | Turn on the Light | Turn the Light on. | 0,5972222089767456 
| NormalizedLevenshteinSimilarity | F23.StringSimilarity | yes        | Turn on the Light | Turn the Light on. | 0,6111111111111112 |
| MetricLCSSimilarity | F23.StringSimilarity | yes        | Turn on the Light | Turn the Light on. | 0,7777777777777778 |
| QGramSimilarity     | F23.StringSimilarity | no         | Turn on the Light | Turn the Light on. | -2                 |
| DamerauSimilarity   | F23.StringSimilarity | no         | Turn on the Light | Turn the Light on. | -6                 |
| LevenshteinSimilarity | F23.StringSimilarity | no         | Turn on the Light | Turn the Light on. | -6                 |
| LongestCommonSubsequenceSimilarity | F23.StringSimilarity | no         | Turn on the Light | Turn the Light on. | -6      


| Algorithm                   | Library              | Normalized | Target            | Input    | Score               |
| --------------------------- | -------------------- | ---------- | ----------------- | -------- | ------------------- |
| FuzzyWeightedRatio          | FuzzySharp           | yes        | Turn on the Light | Light on | 0,86                |
| CosineSimilarity            | F23.StringSimilarity | yes        | Turn on the Light | Light on | 0,5345224838248488  |
| JaroWinklerSimilarity       | F23.StringSimilarity | yes        | Turn on the Light | Light on | 0,5063725709915161  |
| NGramSimilarity             | F23.StringSimilarity | yes        | Turn on the Light | Light on | 0,10294115543365479 |
| NormalizedLevenshteinSimilarity | F23.StringSimilarity | yes        | Turn on the Light | Light on | 0,11764705882352944 |
| MetricLCSSimilarity         | F23.StringSimilarity | yes        | Turn on the Light | Light on | 0,2941176470588236  |
| QGramSimilarity             | F23.StringSimilarity | no         | Turn on the Light | Light on | -10                 |
| DamerauSimilarity           | F23.StringSimilarity | no         | Turn on the Light | Light on | -14                 |
| LevenshteinSimilarity       | F23.StringSimilarity | no         | Turn on the Light | Light on | -14                 |
| LongestCommonSubsequenceSimilarity | F23.StringSimilarity | no         | Turn on the Light | Light on | -14           |


| Algorithm           | Library             | Normalized | Target            | Input              | Score              |
| ------------------- | ------------------- | ---------- | ----------------- | ------------------ | ------------------ |
| FuzzyWeightedRatio  | FuzzySharp          | yes        | Turn on the Light | Turn off the Light | 0,91               |
| CosineSimilarity    | F23.StringSimilarity | yes        | Turn on the Light | Turn off the Light | 0,8574929257125442 |
| JaroWinklerSimilarity | F23.StringSimilarity | yes        | Turn on the Light | Turn off the Light | 0,9622367223103842 |
| NGramSimilarity     | F23.StringSimilarity | yes        | Turn on the Light | Turn off the Light | 0,8611111044883728 |
| NormalizedLevenshteinSimilarity  | F23.StringSimilarity | yes        | Turn on the Light | Turn off the Light | 0,8888888888888888 |
| MetricLCSSimilarity | F23.StringSimilarity | yes        | Turn on the Light | Turn off the Light | 0,8888888888888888 |
| QGramSimilarity     | F23.StringSimilarity | no         | Turn on the Light | Turn off the Light | -4                 |
| DamerauSimilarity   | F23.StringSimilarity | no         | Turn on the Light | Turn off the Light | -1                 |
| LevenshteinSimilarity | F23.StringSimilarity | no         | Turn on the Light | Turn off the Light | -1                 |
| LongestCommonSubsequenceSimilarity | F23.StringSimilarity | no         | Turn on the Light | Turn off the Light | -2   |



| Algorithm           | Library             | Normalized | Target            | Input              | Score              |
| ------------------- | ------------------- | ---------- | ----------------- | ------------------ | ------------------ |
| FuzzyWeightedRatio  | FuzzySharp          | yes        | Turn on the Light | Take a picture of me   | 0,38               |
| CosineSimilarity    | F23.StringSimilarity | yes        | Turn on the Light | Take a picture of me   | 0,2057377999494558|
| JaroWinklerSimilarity | F23.StringSimilarity | yes        | Turn on the Light | Take a picture of me   |0,4920167922973633|
| NGramSimilarity     | F23.StringSimilarity | yes        | Turn on the Light | Take a picture of me   | 0,2791666388511657|
| NormalizedLevenshteinSimilarity | F23.StringSimilarity | yes        | Turn on the Light | Take a picture of me   | 0,25|
| MetricLCSSimilarity | F23.StringSimilarity | yes        | Turn on the Light | Take a picture of me   | 0,35           |
| QGramSimilarity     | F23.StringSimilarity | no         | Turn on the Light | Take a picture of me   | -28            |
| DamerauSimilarity   | F23.StringSimilarity | no         | Turn on the Light | Take a picture of me   | -14            |
| LevenshteinSimilarity | F23.StringSimilarity | no         | Turn on the Light | Take a picture of me   | -14            |
| LongestCommonSubsequenceSimilarity | F23.StringSimilarity | no         | Turn on the Light | Take a picture of me   | -22|


### Result:
A normalized algorithm should be used for our usecase. In terms of accuracy, they are all quite similar.
However, since the CosineSimilarity algorithm is using the "bag of words" aproach it could be the most accurate one for our use case.
Levenshteins algorithm could also be a good choice. It is commonly used and its normalized version is also used in FuzzySharp. However, the FuzzzySharp implmentation
uses a weighing system of which it is not clear on how it was built.
Levenshteins is based on number of insertions, deletions and substitutiions in strings and is the most commonly used way to calculate the distance between two strings.
Please note that it is case sensitive since it takes one substitution to get from 'T' to 't'. (All tested algorithms are case sensitive)

### Conclusion:
As of this moment it would be recommended to use the Cosine Alogorithm found in the F23.StringSimilarity Nuget Package.
