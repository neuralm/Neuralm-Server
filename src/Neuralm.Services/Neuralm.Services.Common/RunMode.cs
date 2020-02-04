namespace Neuralm.Services.Common
{
    /// <summary>
    /// Represents the <see cref="RunMode"/> class.
    /// A static class to determine if the application is running in test mode.
    /// </summary>
    public static class RunMode
    {
        /// <summary>
        /// Gets and sets whether the application is in test mode.
        /// </summary>
        public static bool IsTest { get; set; }
    }
}
