namespace FgssrApi.CustomMethodsServices
{
    public class AddJwtToCookie
    {
        private readonly RequestDelegate _next;

        public AddJwtToCookie(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //var token = context.Request.Cookies["AuthToken"];


            //if (!string.IsNullOrEmpty(token))
            //{
            //    // Set the Authorization header with the Bearer token
            //    context.Request.Headers["Authorization"] = "Bearer " + token;
            //}



            //await _next(context); // Call the next middleware in the pipeline

            if (context.Request.Cookies.TryGetValue("AuthToken", out var token) && !string.IsNullOrEmpty(token))
            {
                // Log for debugging
                Console.WriteLine($"Token retrieved from cookie: {token}");

                // Set the Authorization header with the Bearer token
                context.Request.Headers["Authorization"] = "Bearer " + token;
            }
            else
            {
                Console.WriteLine("No AuthToken cookie found or it's empty.");
            }

            await _next(context); // Call the next middleware in the pipeline

        }
    }
}
