namespace FgssrApi.CustomMethodsServices
{
    public class AdmissionNumberGenerator
    {
        public static string GenerateAdmissionNumber(int minimum, int maximum)
        {
            Random random = new Random();
            int code = random.Next(minimum, maximum);
            return code.ToString();
        }
    }
}
