using Practice_Quiz_Generator.Shared.DTOs.Request;

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


        public static string BuildTheoryGradingPrompt(TheoryGradingRequestDto request)
        {
            var keywordsString = string.Join(", ", request.Keywords);

            return $@"You are an expert academic grader. Grade the following student's answer to a theory question.

Question:
{request.Question}

Student's Answer:
{request.Answer}

Required Keywords: {keywordsString}

 Respond ONLY in strict JSON with this schema:
{{
    ""score"": <number between 0-100>,
    ""feedback"": ""<detailed constructive feedback>""
}}

Grading Criteria:
 1. Accuracy and correctness of the answwer (40%)
 2. Presence of required keywords (30%)
 3. Clarity and structure of explanation (15%)
 4. Depth of understanding demonstrated (15%)

Be fair, constructive, and specific in your feedback.";
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
