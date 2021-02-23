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

#### Result:
A normalized algorithm should be used for our usecase. In terms of accuracy, they are all quite similar.
Levenshteins algortithm could be a good choice. It is commonly used and its normalized version is also used in FuzzySharp.
It is based on the number of insertions, deletions and substitutiions in strings and is the most commonly used way to calculate the distance between two strings.
Please note that it is case sensitive since it takes one substitution to get from 'T' to 't'. (All tested algorithms are case sensitive)