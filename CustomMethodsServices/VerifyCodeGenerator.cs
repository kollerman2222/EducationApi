namespace FgssrApi.CustomMethodsServices
{
    public static class VerifyCodeGenerator
    {
         public static string GenerateRandomCode(int minimum, int maximum)
        {
            Random random = new Random();
            int code = random.Next(minimum, maximum);
            return code.ToString();
        }
    }
}
