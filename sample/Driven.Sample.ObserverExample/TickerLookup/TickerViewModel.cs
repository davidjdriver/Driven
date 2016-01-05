using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverExample.TickerLookup
{
    public class TickerViewModel : BaseViewModel
    {
        private string _Results;
        public string Results
        {
            get { return _Results; }
            set
            {
                if (_Results != value)
                {
                    _Results = value;
                    OnPropertyChanged("Results");
                }
            }
        }

        public TickerViewModel()
        {
            SystemContext.Instance.RegisterTickerChanged(Search);
        }

        protected async void Search(string ticker)
        {
            if (!string.IsNullOrEmpty(ticker))
            {
                StockQuoteService.StockQuoteSoapClient c = new StockQuoteService.StockQuoteSoapClient("StockQuoteSoap12");
                Results = await c.GetQuoteAsync(ticker);
            }
        }
    }
}
