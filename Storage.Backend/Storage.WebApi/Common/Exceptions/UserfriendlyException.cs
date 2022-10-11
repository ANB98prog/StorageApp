using Newtonsoft.Json;

namespace Storage.WebApi.Common.Exceptions
{
    public class UserfriendlyException
    {
        public List<string> errors { get; set; } = new List<string>();

        public UserfriendlyException(string userfriendlyMessage)
        {
            errors.Add(userfriendlyMessage);
        }

        public UserfriendlyException(IEnumerable<string> userfriendlyMessages)
        {
            errors.AddRange(userfriendlyMessages);
        }
    }
}
