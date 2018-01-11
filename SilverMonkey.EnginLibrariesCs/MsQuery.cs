using Monkeyspeak;
using System.Linq;

namespace Engine.Libraries
{
    /// <summary>
    /// Join Summon Lead Follow cuddle commands
    /// </summary>
    /// <seealso cref="MonkeySpeakLibrary" />
    public class MsQuery : MonkeySpeakLibrary
    {
        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 40;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of objects to use to pass runtime objects to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            // Summon
            // (0:40) When anyone requests to summon the bot,
            Add(TriggerCategory.Cause, 40, r =>
                ReadTriggeringFurreParams(r)
                && "summon" == r.GetParametersOfType<string>().FirstOrDefault(),
                "When anyone requests to summon the bot,");

            // (0:41) When a furre named {..} requests to summon the bot,
            Add(TriggerCategory.Cause, 41,
                r => NameIs(r)
                && "summon" == r.GetParametersOfType<string>().FirstOrDefault(),
                "When a furre named {..} requests to summon the bot,");

            // Join
            // (0:42) When anyone requests to join the bot,
            Add(TriggerCategory.Cause, 42,
               r => ReadTriggeringFurreParams(r)
               && "join" == r.GetParametersOfType<string>().FirstOrDefault(),
               "When anyone requests to join the bot,");

            // (0:43) When a furre named {..} requests to join the bot,
            Add(TriggerCategory.Cause, 43,
                r => NameIs(r)
                && "join" == r.GetParametersOfType<string>().FirstOrDefault(),
                "When a furre named {..} requests to join the bot,");

            // Follow
            // (0:44) When anyone requests to follow the bot,
            Add(TriggerCategory.Cause, 44,
               r => ReadTriggeringFurreParams(r)
               && "follow" == r.GetParametersOfType<string>().FirstOrDefault(),
               "When anyone requests to follow the bot,");

            // (0:35) When a furre named {..} requests to follow the bot,
            Add(TriggerCategory.Cause, 45,
                r => NameIs(r)
                && "follow" == r.GetParametersOfType<string>().FirstOrDefault(),
                "When a furre named {..} requests to follow the bot,");
            // Lead

            // (0:46) When anyone requests to lead the bot,
            Add(TriggerCategory.Cause, 46,
                r => ReadTriggeringFurreParams(r)
                && "lead" == r.GetParametersOfType<string>().FirstOrDefault(),
                "When anyone requests to lead the bot,");

            // (0:47) When a furre named {..} requests to lead the bot,
            Add(TriggerCategory.Cause, 47,
                r => NameIs(r)
                && "lead" == r.GetParametersOfType<string>().FirstOrDefault(),
                "When a furre named {..} requests to lead the bot,");

            // Cuddle
            // (0:48) When anyone requests to cuddle with the bot.
            Add(TriggerCategory.Cause, 48,
                r => ReadTriggeringFurreParams(r)
                && "cuddle" == r.GetParametersOfType<string>().FirstOrDefault(),
                "When anyone requests to cuddle with the bot,");

            // (0:49) When a furre named {..} requests to cuddle with the bot,
            Add(TriggerCategory.Cause, 49,
                r => NameIs(r)
                && "cuddle" == r.GetParametersOfType<string>().FirstOrDefault(),
                "When a furre named {..} requests to cuddle with the bot,");

            // (0:50) When the bot see a query (lead, follow summon, join, cuddle),
            Add(TriggerCategory.Cause, 50,
                r => ReadTriggeringFurreParams(r),
                "When the bot see a query (lead, follow summon, join, cuddle),");

            Add(TriggerCategory.Effect,
                 r => SendServer($"`summon {Player.ShortName}"),

                 "summon the triggering furre");
            Add(TriggerCategory.Effect,
                r => SendServer($"`summon {r.ReadString().ToFurcadiaShortName()}"),
                "summon the the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => SendServer($"`join {Player.ShortName}"),
                "join the triggering furre");

            Add(TriggerCategory.Effect,
                r => SendServer($"`join {r.ReadString().ToFurcadiaShortName()}"),
                "join the the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => SendServer($"`follow {Player.ShortName}"),
                "follow the triggering furre");

            Add(TriggerCategory.Effect,
                r => SendServer($"`follow {r.ReadString().ToFurcadiaShortName()}"),
                "follow the the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => SendServer($"`lean {Player.ShortName}"),
                "lead the triggering furre");

            Add(TriggerCategory.Effect,
                r => SendServer($"`lead {r.ReadString().ToFurcadiaShortName()}"),
                "lead the the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => SendServer($"`cuddle {Player.ShortName}"),
                "cuddle the triggering furre");

            Add(TriggerCategory.Effect,
                r => SendServer($"`cuddle {r.ReadString().ToFurcadiaShortName()}"),
                "lead the the cuddle named {...}.");

            Add(TriggerCategory.Effect,
                r => SendServer($"`stop"),
                "stop leading ,following or cuddling");
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
        }

        #endregion Public Methods
    }
}