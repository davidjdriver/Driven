using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverExample.SearchLog
{
    public class SearchLogViewModel : BaseViewModel
    {
        public ObservableCollection<SearchAudit> Audits { get; set; }

        public SearchLogViewModel()
        {
            Audits = new ObservableCollection<SearchAudit>();
            SystemContext.Instance.RegisterTickerChanged(SearchRequested);
        }

        public void SearchRequested(string ticker)
        {
            if (!string.IsNullOrEmpty(ticker))
            {
                Audits.Add(new SearchAudit()
                {
                    Ticker = ticker,
                    SearchTime = DateTime.Now
                });
            }
        }
    }

    public class SearchAudit
    {
        public string Ticker { get; set; }
        public DateTime SearchTime { get; set; }
    }
}
