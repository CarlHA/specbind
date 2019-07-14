using System.Collections.ObjectModel;

namespace SpecBind.Configuration
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            ExcludedAssemblies = new Collection<ExcludedAssembly>();
        }

        public Collection<ExcludedAssembly> ExcludedAssemblies { get; set; }
        public bool WaitForStillElementBeforeClicking { get; set; }
        public string StartUrl { get; set; }
        public bool RetryValidationUntilTimeout { get; set; }
    }
}
