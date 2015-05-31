using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrings.Logic
{
    class Corporation
    {
        private List<Shareholder> shareholders;

        public List<Shareholder> Shareholders
        {
            get { return shareholders; }
            set { shareholders = value; }
        }

        private Graph<Shareholder> graph;

        public Graph<Shareholder> Graph
        {
            get { return graph; }
            set { graph = value; }
        }

        public Corporation()
        {
            Shareholders = getShareholdersList();
        }

        public Corporation(List<Shareholder> _shareholders)
        {
            Shareholders = _shareholders;
        }

        public Corporation(Dictionary<int, Shareholder> _shareholders)
        {
            Shareholders = new List<Shareholder>();
            foreach (int shareholderId in _shareholders.Keys)
            {
                Shareholders.Add(_shareholders[shareholderId]);
            }
        }

        public float getTotalShares()
        {
            float totalShares = 0;
            foreach(Shareholder shareholder in shareholders)
            {
                totalShares += shareholder.Share;
            }
            return totalShares;
        }

        public KeyValuePair<float,List<Shareholder>> getIndependentShareholdersWithHighestShare()
        {
            Graph = new Graph<Shareholder>(Shareholders);
            foreach(Shareholder shareholder in shareholders)
            {
                Graph.addSourceTargetEdge(shareholder, shareholder.SpiedShareholder);
                Graph.addTargetSourceEdge(shareholder.SpiedShareholder, shareholder);
            }
            return Graph.getMaxIndependentSet(true);
        }

        public List<Shareholder> getShareholdersList()
        {
            List<Shareholder> _shareholders = new List<Shareholder>();

            Shareholder tom = new Shareholder("Tom", 3, 1, null);
            Shareholder john = new Shareholder("John", 2, 2, tom);
            Shareholder jack = new Shareholder("Jack", 10, 3, null);
            Shareholder tyrion = new Shareholder("Tyrion", 6, 4, jack);
            Shareholder mike = new Shareholder("Mike", 8, 5, tyrion);
            Shareholder chris = new Shareholder("Chris", 7, 6, mike);
            Shareholder robert = new Shareholder("Robert", 12, 7, jack);
            jack.SpiedShareholder = chris;

            _shareholders.Add(tom);
            _shareholders.Add(john);
            _shareholders.Add(jack);
            _shareholders.Add(tyrion);
            _shareholders.Add(mike);
            _shareholders.Add(chris);
            _shareholders.Add(robert);

            return _shareholders;
        }
    }
}
