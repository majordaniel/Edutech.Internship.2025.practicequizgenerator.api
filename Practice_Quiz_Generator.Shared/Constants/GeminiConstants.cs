namespace Practice_Quiz_Generator.Shared.Constants
{
    public class GeminiConstants
    {
        public const string GeminiFlashModel = "gemini-2.0-flash";
        public const int DefaultQuestionLimit = 10;
    }

    public static class PromptTemplates
    {
        public static string BuildQuizPrompt(string uploadedText, int numberOfQuestions)
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
//        public static string BuildQuizPrompt(string studyMaterial, int numberOfQuestions)
//        {
//            return $@"
//You are a quiz generator. 
//Generate exactly {numberOfQuestions} multiple-choice questions in JSON format.

//The JSON schema must follow this structure:
//{{
//  ""questions"": [
//    {{
//      ""question"": ""string"",
//      ""options"": [""string"", ""string"", ""string"", ""string""],
//      ""correctOptionIndex"": int
//    }}
//  ]
//}}

//Study Material:
//{studyMaterial}
//";
//        }

    }
}
