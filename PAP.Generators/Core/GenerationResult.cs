namespace PAP.Generators.Core
{
    /// <summary>
    /// Outcome of a generation request, with the message shown to the user.
    /// </summary>
    public class GenerationResult
    {
        private GenerationResult(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public bool Succeeded { get; }

        public string Message { get; }

        public static GenerationResult Success(string message)
        {
            return new GenerationResult(true, message);
        }

        public static GenerationResult Failure(string message)
        {
            return new GenerationResult(false, message);
        }
    }
}
