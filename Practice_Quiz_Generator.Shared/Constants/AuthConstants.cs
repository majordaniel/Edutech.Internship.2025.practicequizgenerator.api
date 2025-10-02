namespace Practice_Quiz_Generator.Shared.Constants
{
    public class AuthConstants
    {
    }

    public class UploadConstant
    {
        public const int Limit = 50;
    }

    public static class EmailTemplate
    {
        public static string BuildWelcomeEmailTemplate(string firstName, string confirmationLink)
        {
            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <style>
    body {{
      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
      background-color: #f4f6f9;
      margin: 0;
      padding: 0;
      color: #333333;
    }}
    .container {{
      max-width: 600px;
      margin: 30px auto;
      background: #ffffff;
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 4px 12px rgba(0,0,0,0.08);
    }}
    .header {{
      background: #2C6BED; /* Academic Blue */
      color: white;
      text-align: center;
      padding: 25px;
    }}
    .header h2 {{
      margin: 0;
      font-size: 24px;
      font-weight: 600;
    }}
    .content {{
      padding: 30px;
      line-height: 1.7;
      font-size: 15px;
    }}
    .content p {{
      margin: 12px 0;
    }}
    .btn {{
      display: inline-block;
      padding: 12px 30px;
      margin: 25px 0;
      background-color: #2C6BED; /* Academic Blue */
      color: #ffffff !important;
      text-decoration: none;
      border-radius: 5px;
      font-weight: 600;
      font-size: 15px;
      transition: background 0.3s ease;
    }}
    .btn:hover {{
      background-color: #1f54b6;
    }}
    .footer {{
      background: #f9f9f9;
      text-align: center;
      padding: 18px;
      font-size: 12px;
      color: #777777;
      border-top: 1px solid #eeeeee;
    }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>
      <h2>Confirm Your Email</h2>
    </div>
    <div class='content'>
      <p>Hi {firstName},</p>
      <p>Welcome to <strong>Practice Quiz Generator</strong> 🎓.</p>
      <p>Your account has been created successfully, and you are now officially part of our academic community. 
      To activate your account and get started, please confirm your email address.</p>

       <p><strong>Note:</strong> Your default login password is <code>PracticeQuiz@2025</code>. For security, please change it after your first login.</p>

      
      <p style='text-align:center;'>
        <a href='{confirmationLink}' class='btn'>Confirm My Email</a>
      </p>
      
      <p>If the button above does not work, please copy and paste the following link into your browser:</p>
      <p style='word-break:break-all;'><a href='{confirmationLink}'>{confirmationLink}</a></p>
      
      <p>Thank you for choosing PQG as your academic companion. We look forward to supporting your learning journey.</p>
      
      <p>Best regards,<br/><strong>The PQG Team</strong></p>
    </div>
    <div class='footer'>
      &copy; {DateTime.UtcNow.Year} Practice Quiz Generator | Empowering Academic Excellence
    </div>
  </div>
</body>
</html>";
        }
    }
}
