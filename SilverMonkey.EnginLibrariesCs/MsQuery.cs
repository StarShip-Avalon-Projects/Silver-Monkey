using Furcadia.Net.Utils.ChannelObjects;
using Monkeyspeak;
using System;
using System.Linq;

namespace Libraries
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
        /// <param name="args">Parametized argument of vars to use to pass runtime vars to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            Add(TriggerCategory.Cause,
                SummonRequestTriggeringFurre,
                "When anyone requests to summon the bot,");

            Add(TriggerCategory.Cause,
                SummonRequestFurreNamed,
                "When a furre named {..} requests to summon the bot,");

            Add(TriggerCategory.Cause,
              JoinRequestTriggeringFurre,
               "When anyone requests to join the bot,");

            Add(TriggerCategory.Cause,
                JoinRequestFurreNamed,
                "When a furre named {..} requests to join the bot,");

            Add(TriggerCategory.Cause,
              FollowRequestAnyFurre,
               "When anyone requests to follow the bot,");

            Add(TriggerCategory.Cause,
               FollowRequestFurreNamed,
                "When a furre named {..} requests to follow the bot,");

            Add(TriggerCategory.Cause,
              LeadRequestAnyFurre,
                "When anyone requests to lead the bot,");

            Add(TriggerCategory.Cause,
              LeadRequestFurreNamed,
                "When a furre named {..} requests to lead the bot,");

            Add(TriggerCategory.Cause,
             CuddleRequestAnyFurre,
                "When anyone requests to cuddle with the bot,");

            Add(TriggerCategory.Cause,
               CuddleRequestFurreNamed,
                "When a furre named {..} requests to cuddle with the bot,");

            Add(TriggerCategory.Cause,
               AnyQueryRequest,
                "When the bot see a query (lead, follow summon, join, cuddle),");

            Add(TriggerCategory.Effect,
                 r =>
                 SendServer($"`summon {Player.ShortName}"),
                 "summon the triggering furre");

            Add(TriggerCategory.Effect,
                r =>
                SendServer($"`summon {r.ReadString().ToFurcadiaShortName()}"),
                "summon the the furre named {...}.");

            Add(TriggerCategory.Effect,
                r =>
                SendServer($"`join {Player.ShortName}"),
                "join the triggering furre");

            Add(TriggerCategory.Effect,
                r =>
                SendServer($"`join {r.ReadString().ToFurcadiaShortName()}"),
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
                "cuddle with the triggering furre");

            Add(TriggerCategory.Effect,
                r => SendServer($"`cuddle {r.ReadString().ToFurcadiaShortName()}"),
                "cuddle with the furre named {...}.");

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

        #region Private Methods

        private bool AnyQueryRequest(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            return true;
        }

        private bool CuddleRequestAnyFurre(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            return QueryType.cuddle == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool CuddleRequestFurreNamed(TriggerReader reader)
        {
            return NameIs(reader)
                && QueryType.cuddle == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool FollowRequestAnyFurre(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            return QueryType.follow == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool FollowRequestFurreNamed(TriggerReader reader)
        {
            return NameIs(reader)
                 && QueryType.follow == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool JoinRequestFurreNamed(TriggerReader reader)
        {
            return NameIs(reader) &&
                QueryType.join == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool JoinRequestTriggeringFurre(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            return QueryType.join == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool LeadRequestAnyFurre(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            return QueryType.lead == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool LeadRequestFurreNamed(TriggerReader reader)
        {
            return NameIs(reader)
               && QueryType.lead == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool SummonRequestFurreNamed(TriggerReader reader)
        {
            return NameIs(reader) &&
                 QueryType.summon == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        private bool SummonRequestTriggeringFurre(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            return QueryType.summon == reader.GetParametersOfType<QueryChannelObject>().First().Query;
        }

        #endregion Private Methods
    }
}