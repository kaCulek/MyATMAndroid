using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Java.Interop;

namespace MyATMAndroid
{
    [Activity(Label = "LastTransactionsActivity")]
    public class LastTransactionsActivity : Activity
    {
        private string cardNumber;
        private LastTransactionsAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            cardNumber = Intent.GetStringExtra("UserCard") ?? string.Empty;
            SetContentView(Resource.Layout.last_transactions);

            var view = FindViewById<ListView>(Resource.Id.lastTrasactionsView);
            adapter = new LastTransactionsAdapter(GetTransactions(), this);
            view.Adapter = adapter;
        }

        private JavaList<AtmUserTransaction> GetTransactions()
        {
            var users = new AtmUsers();
            var user = users.GetUser(cardNumber);
            var transactions = user.Transactions.OrderByDescending(i => i.Date).Take(5);
            return new JavaList<AtmUserTransaction>(transactions);
        }

        [Export("BackClick")]
        public void BackClick(Android.Views.View e)
        {
            var activity = new Intent(this, typeof(DashboardActivity));
            activity.PutExtra("UserCard", cardNumber);
            StartActivity(activity);
        }
    }
}