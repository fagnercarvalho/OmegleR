namespace OmegleR.Models
{
    /// <summary>
    ///     Class that holds two users chatting.
    /// </summary>
    public class OneOnOne
    {
        /// <summary>
        ///     Gets or sets the first user connected to the chat.
        /// </summary>
        public string User1 { get; set; }

        /// <summary>
        ///     Gets or sets the second user connected to the chat.
        /// </summary>
        public string User2 { get; set; }
    }
}