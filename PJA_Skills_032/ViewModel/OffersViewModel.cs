using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Parse;
using PJA_Skills_032.Model;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.ViewModel
{
    public class OffersViewModel : BindableBase
    {
        #region props

        private ObservableCollection<Offer> _offersObservableCollection = new ObservableCollection<Offer>();

        public ObservableCollection<Offer> OffersObservableCollection
        {
            get { return this._offersObservableCollection; }
            set { this.SetProperty(ref this._offersObservableCollection, value); }
        }

        #endregion

        public OffersViewModel()
        {

        }


        #region methods

        public async Task<ObservableCollection<Offer>> DownloadAllOffers()
        {
            ParseQuery<ParseObject> query =
                from item in ParseObject.GetQuery(ParseHelper.OBJECT_OFFER)
                orderby item.CreatedAt
                select item;
            IEnumerable<Offer> allOffersEnumerable = from item in await query.FindAsync()
                                                     select new Offer(item);
            ObservableCollection<Offer> offersObservableCollection = new ObservableCollection<Offer>(allOffersEnumerable);
            return offersObservableCollection;
        }

        public async Task AddDownloadedOffers()
        {
            ObservableCollection<Offer> downloadedOffers = await DownloadAllOffers();
            foreach (Offer offer in downloadedOffers)
            {
                await offer.GetUser();
                OffersObservableCollection.Add(offer);
            }
        }
        #endregion

    }
}
