namespace FgssrApi.CustomMethodsServices
{
    public class StudentNumberGenerator
    {

        public static string GenerateStudentNumber(long minimum, long maximum)
        {
            Random random = new Random();
            long code = random.NextInt64(minimum, maximum);
            return code.ToString();
        }
    }
}
