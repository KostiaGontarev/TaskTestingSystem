using System;
using System.Collections.Generic;
using System.Linq;

using TTS.Core.Interfaces.Model;
using TTS.Core.Declarations;


namespace TTS.Core.Model
{
    [Serializable]
    internal class Task : ITask
    {
        #region Data Members
        private Guid id;
        private string name;
        private string description;

        private readonly List<Characteristic> requirements;
        private readonly List<Guid> tests;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return this.id; }
            set
            {
                if (value != Guid.Empty)
                    this.id = value;
            }
        }
        public string Name
        {
            get { return this.name; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this.name = value;
            }
        }
        public string Description
        {
            get { return this.description; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this.description = value;
            }
        }
        public IList<Characteristic> Requirements
        {
            get { return this.requirements; }
            set
            {
                if (value == null || !value.Any())
                    return;

                this.requirements.Clear();
                this.requirements.AddRange(value);
            }
        }
        public IList<Guid> Tests
        {
            get { return this.tests; }
            set
            {
                if (value == null || !value.Any())
                    return;

                this.tests.Clear();
                this.tests.AddRange(value);
            }
        }
        #endregion

        #region Constructors
        public Task()
        {
            this.id = Guid.NewGuid();
            this.name = String.Empty;
            this.description = String.Empty;
            this.requirements = new List<Characteristic>();
            this.tests = new List<Guid>();
        }
        #endregion
    }
}
