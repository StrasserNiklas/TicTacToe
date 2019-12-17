// Niklas Strasser, Felix Brandstetter, Yannick Gruber

namespace GameLibrary
{
    /// <summary>
    /// This class is used to simplify information about a game.
    /// Only contains the name of both players.
    /// </summary>
    public class SimpleGameInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleGameInformation"/> class.
        /// </summary>
        /// <param name="playerOne">The player one.</param>
        /// <param name="playerTwo">The player two.</param>
        public SimpleGameInformation(string playerOne, string playerTwo)
        {
            this.PlayerOne = playerOne;
            this.PlayerTwo = playerTwo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleGameInformation"/> class.
        /// </summary>
        public SimpleGameInformation()
        {
        }

        /// <summary>
        /// Gets or sets the player one.
        /// </summary>
        /// <value>
        /// The player one.
        /// </value>
        public string PlayerOne { get; set; }

        /// <summary>
        /// Gets or sets the player two.
        /// </summary>
        /// <value>
        /// The player two.
        /// </value>
        public string PlayerTwo { get; set; }
    }
}
