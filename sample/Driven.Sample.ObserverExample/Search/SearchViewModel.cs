using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverExample.Search
{
    public class SearchViewModel : BaseViewModel
    {
        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                if (_SearchText != value)
                {
                    _SearchText = value;
                    OnPropertyChanged("SearchText");
                }
            }
        }

        public void Search()
        {
            SystemContext.Instance.RaiseTickerChanged(SearchText);

            /* 
             * In a tightly coupled version, this View Model would have to know
             * about every other that needed to receive search text updates
             * e.g.
             * TickerViewModel
             * SearchLogViewModel
             * 
             * In addition to this view model knowing about the above view models,
             * The hard coded change view model would have to know about them also.
             * So we can see this approach would not scale very well and not be as maintainable
             */
        }
    }
}
