namespace Neuralm.Services.TrainingRoomService.Messages
{
    /// <summary>
    /// Represents the <see cref="DictionaryEntry{TKey, TValue}"/> class.
    /// </summary>
    public class DictionaryEntry<TKey, TValue>
    {
        /// <summary>
        /// Gets and sets the key.
        /// </summary>
        public TKey Key { get; set; }
        
        /// <summary>
        /// Gets and sets the value.
        /// </summary>
        public TValue Value { get; set; }
    }
}