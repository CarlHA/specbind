using System.Collections.ObjectModel;

namespace SpecBind.Configuration
{
    public class ApplicationConfiguration
    {
        public Collection<ExcludedAssembly> ExcludedAssemblies { get; set; } = new Collection<ExcludedAssembly>();
        public bool WaitForStillElementBeforeClicking { get; set; }
        public string StartUrl { get; set; }
        public bool RetryValidationUntilTimeout { get; set; }
    }
}
