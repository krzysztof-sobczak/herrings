using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrings.Logic
{
    class Shareholder : AbstractVertice
    {
        private float share;

        public float Share
        {
            get { return share; }
            set { share = value; }
        }

        private Shareholder spiedShareholder;

        public Shareholder SpiedShareholder
        {
            get { return spiedShareholder; }
            set { spiedShareholder = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int spiedId;

        public int SpiedId
        {
            get { return spiedId; }
            set { spiedId = value; }
        }

        public Shareholder()
        {

        }

        public Shareholder(string _name, float _share, int _id, Shareholder _spiedShareholder = null)
        {
            Id = _id;
            Name = _name;
            Share = _share;
            SpiedShareholder = _spiedShareholder;
        }

        override public int toInt()
        {
            return Id;
        }

        override public float getValue()
        {
            return Share;
        }

        public override string ToString()
        {
            if (SpiedShareholder != null)
            {
                return String.Format("[{0}] {1} with share: {2} -> spies [{3}] {4}", this.Id, this.Name, this.Share, this.SpiedShareholder.Id, this.SpiedShareholder.Name);
            }
            else
            {
                return String.Format("[{0}] {1} with share: {2} -> spies nobody", this.Id, this.Name, this.Share);
            }
        }

    }
}
