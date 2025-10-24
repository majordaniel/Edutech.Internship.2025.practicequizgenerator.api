namespace Practice_Quiz_Generator.Shared.Constants
{
    public class GeminiConstants
    {
        public const string GeminiFlashModel = "gemini-2.0-flash";
        public const int DefaultQuestionLimit = 10;
    }

    public static class PromptTemplates
    {
        public static string GenerateFromUploadPrompt(string uploadedText, int numberOfQuestions)
        {
            return $@"
Generate {numberOfQuestions} multiple-choice questions 
from the following content:

{uploadedText}

Respond ONLY in strict JSON with this schema:
{{
  ""questions"": [
    {{
      ""question"": ""string"",
      ""options"": [""string"", ""string"", ""string"", ""string""],
      ""correctOptionIndex"": int
    }}
  ]
}}";
        }
        public static string GenerateFromQuestionBankPrompt(string questions, int numberOfQuestions)
        {
            return $@"
Generate {numberOfQuestions} multiple-choice questions 
from the following content:

{questions}

Before returning, **rephrase each question and its options** in your own words while preserving their original meaning.

Respond ONLY in strict JSON with this schema:
{{
  ""questions"": [
    {{
      ""question"": ""string"",
      ""options"": [""string"", ""string"", ""string"", ""string""],
      ""correctOptionIndex"": int
    }}
  ]
}}";

        }

    }
}
