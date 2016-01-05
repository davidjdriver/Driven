using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverExample.HardCodedChange
{
    public class HardCodedChangeViewModel : BaseViewModel
    {
        public void Change()
        {
            SystemContext.Instance.RaiseTickerChanged("AAPL");
        }
    }
}
