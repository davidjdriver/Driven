using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverExample
{
    //Singleton class representing the context of the stock ticker application
    public class SystemContext
    {
        //Static instance is shared globally
        public static readonly SystemContext Instance = new SystemContext();

        private List<Action<string>> actions = new List<Action<string>>();

        //Private constructor prevents new instances from being created
        private SystemContext()
        {

        }

        public void RegisterTickerChanged(Action<string> a)
        {
            actions.Add(a);
        }

        public void RaiseTickerChanged(string s)
        {
            foreach (var action in actions)
            {
                action(s);
            }
        }


    }
}