namespace NL.HNOGames.Domoticz.Controls
{
    /// <summary>
    /// Defines the <see cref="IShare" />
    /// </summary>
    public interface IShare
    {
        /// <summary>
        /// The Share
        /// </summary>
        /// <param name="subject">The subject<see cref="string"/></param>
        /// <param name="message">The message<see cref="string"/></param>
        /// <param name="image">The image<see cref="byte[]"/></param>
        void Share(string subject, string message, byte[] image);
    }
}
