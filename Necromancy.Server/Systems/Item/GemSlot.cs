namespace Necromancy.Server.Systems.Item
{
    public class GemSlot
    {
        //TODO make gems their own type maybe?
        private ItemInstance _gem;
        public GemType type { get; set; }

        public ItemInstance gem
        {
            get => _gem;
            set
            {
                _gem = value;
                isFilled = true;
            }
        }

        /// <summary>
        ///     Helper Property to determine if the slot is filled or not.
        /// </summary>
        public bool isFilled { get; private set; }
    }
}
