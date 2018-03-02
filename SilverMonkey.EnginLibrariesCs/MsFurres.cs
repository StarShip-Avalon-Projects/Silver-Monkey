using Furcadia.Net.DreamInfo;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System.Collections.Generic;

namespace Libraries
{
    /// <summary>
    /// Furres AFK and in the current dream
    /// </summary>
    /// <seealso cref="Libraries.MonkeySpeakLibrary" />
    public class MsFurres : MonkeySpeakLibrary
    {
        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId
        {
            get
            {
                return 700;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of vars to use to pass runtime vars to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            Add(TriggerCategory.Condition,
                TriggeringInDream,
                "and the triggering furre in the dream,");

            Add(TriggerCategory.Condition,
                TriggeringNotInDream,
                "and the triggering furre is not in the dream,");

            Add(TriggerCategory.Condition,
                FurreNamedInDream,
                "and the furre named {...} is in the dream,");

            Add(TriggerCategory.Condition,
                FurreNamedNotInDream,
                "and the furre named {...} is not in the dream,");

            Add(TriggerCategory.Condition,
                TriggeringCanSe,
                "and the triggering furre is visible,");

            Add(TriggerCategory.Condition,
                TriggeringNotCanSe,
                "and the triggering furre is not visible,");

            Add(TriggerCategory.Condition,
                FurreNamedCanSe,
                "and the furre named {...} is visible,");

            Add(TriggerCategory.Condition,
                FurreNamedNotCanSe,
                "and the furre named {...} is not visible,");

            Add(TriggerCategory.Condition,
                FurreNamedAFK,
             "and the furre named {...}, status is a.f.k.,");

            Add(TriggerCategory.Condition,
                FurreNamedActive,
                "and the furre named {...} is active in the dream,");

            Add(TriggerCategory.Effect, FurreListToVariable,
                "copy the dreams's furre-list to variable %Variable.");

            Add(TriggerCategory.Effect,
                FurresCount,
                "save the dream furre list count to variable %Variable.");

            Add(TriggerCategory.Effect,
                FurreActiveListCount,
                "count the number of active furres in the dream and put it in the variable %Variable.");

            Add(TriggerCategory.Effect,
                FurreAFKListCount,
                "count the number of A.F.K furres in the dream and put it in the variable %Variable.");
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

        [TriggerVariableParameter]
        private bool FurreActiveListCount(TriggerReader reader)
        {
            var var = reader.ReadVariable(true);
            double count = 0;
            foreach (Furre fur in DreamInfo.Furres)
            {
                if (fur.AfkTime == 0)
                {
                    count++;
                }
            }

            var.Value = count;
            return true;
        }

        [TriggerVariableParameter]
        private bool FurreAFKListCount(TriggerReader reader)
        {
            var var = reader.ReadVariable(true);
            double count = 0;
            foreach (Furre fs in DreamInfo.Furres)
            {
                if (fs.AfkTime > 0)
                {
                    count++;
                }
            }

            var.Value = count;
            return true;
        }

        [TriggerStringParameter]
        private bool FurreNamedActive(TriggerReader reader)
        {
            var name = reader.ReadString();
            var Target = (Furre)DreamInfo.Furres.GetFurreByName(name);
            return Target.AfkTime > 0;
        }

        [TriggerStringParameter]
        private bool FurreNamedAFK(TriggerReader reader)
        {
            var name = reader.ReadString();
            var Target = (Furre)DreamInfo.Furres.GetFurreByName(name);
            return Target.AfkTime > 0;
        }

        [TriggerStringParameter]
        private bool FurreNamedCanSe(TriggerReader reader)
        {
            var name = reader.ReadString();
            var Target = (Furre)DreamInfo.Furres.GetFurreByName(name);
            return Target.Visible;
        }

        [TriggerStringParameter]
        private bool FurreNamedInDream(TriggerReader reader)
        {
            var name = reader.ReadString();
            var Target = DreamInfo.Furres.GetFurreByName(name);
            return InDream(Target);
        }

        [TriggerStringParameter]
        private bool FurreNamedNotCanSe(TriggerReader reader)
        {
            var name = reader.ReadString();
            var Target = (Furre)DreamInfo.Furres.GetFurreByName(name);
            return !Target.Visible;
        }

        [TriggerStringParameter]
        private bool FurreNamedNotInDream(TriggerReader reader)
        {
            var name = reader.ReadString();
            var Target = DreamInfo.Furres.GetFurreByName(name);
            return !InDream(Target);
        }

        [TriggerVariableParameter]
        private bool FurresCount(TriggerReader reader)
        {
            var var = reader.ReadVariable(true);
            var.Value = DreamInfo.Furres.Count;
            return true;
        }

        [TriggerVariableParameter]
        private bool FurreListToVariable(TriggerReader reader)
        {
            var var = reader.ReadVariable(true);
            List<string> FurList = new List<string>();
            foreach (Furre fur in DreamInfo.Furres)
            {
                FurList.Add(fur.Name);
            }

            var.Value = string.Join(", ", FurList.ToArray());
            return true;
        }

        private bool TriggeringCanSe(TriggerReader reader)
        {
            return ((Furre)Player).Visible;
        }

        private bool TriggeringInDream(TriggerReader reader)
        {
            return InDream(Player);
        }

        private bool TriggeringNotCanSe(TriggerReader reader)
        {
            return !((Furre)Player).Visible;
        }

        private bool TriggeringNotInDream(TriggerReader reader)
        {
            return !InDream(Player);
        }

        #endregion Private Methods
    }
}