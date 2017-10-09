using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Sample.Android
{
    
    [Activity(Label = "VeiculosSituacaoActivity")]
    public class VeiculosSituacaoOprActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.HistoricoAlertas, countries);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += Btn_ClickLista;
        }

        private void Btn_ClickLista(object sender, AdapterView.ItemClickEventArgs args)
        {
            Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();
            //StartActivity(typeof(AlterarSituacaoActivity));
        }

        static readonly string[] countries = new String[] {
            "Afghanistan","Albania","Algeria","American Samoa","Andorra",
            "Angola","Anguilla","Antarctica","Antigua and Barbuda","Argentina",
            "Armenia","Aruba","Australia","Austria","Azerbaijan",
            "Bahrain","Bangladesh","Barbados","Belarus","Belgium",
            "Belize","Benin","Bermuda","Bhutan","Bolivia",
            "Bosnia and Herzegovina","Botswana","Bouvet Island","Brazil","British Indian Ocean Territory",
            "British Virgin Islands","Brunei","Bulgaria","Burkina Faso","Burundi",
            "Cote d'Ivoire","Cambodia","Cameroon","Canada","Cape Verde",
            "Cayman Islands","Central African Republic","Chad","Chile","China",
            "Christmas Island","Cocos (Keeling) Islands","Colombia","Comoros","Congo",
            "Cook Islands","Costa Rica","Croatia","Cuba","Cyprus","Czech Republic",
            "Democratic Republic of the Congo","Denmark","Djibouti","Dominica","Dominican Republic",
            "East Timor","Ecuador","Egypt","El Salvador","Equatorial Guinea","Eritrea",
            "Estonia","Ethiopia","Faeroe Islands","Falkland Islands","Fiji","Finland",
            "Former Yugoslav Republic of Macedonia","France","French Guiana","French Polynesia",
            "French Southern Territories","Gabon","Georgia","Germany","Ghana","Gibraltar",
            "Greece","Greenland","Grenada","Guadeloupe","Guam","Guatemala","Guinea","Guinea-Bissau",
            "Guyana","Haiti","Heard Island and McDonald Islands","Honduras","Hong Kong","Hungary",
            "Iceland","India","Indonesia","Iran","Iraq","Ireland","Israel","Italy","Jamaica",
            "Japan","Jordan","Kazakhstan","Kenya","Kiribati","Kuwait","Kyrgyzstan","Laos",
            "Latvia","Lebanon","Lesotho","Liberia","Libya","Liechtenstein","Lithuania","Luxembourg",
            "Macau","Madagascar","Malawi","Malaysia","Maldives","Mali","Malta","Marshall Islands",
          };

    }
}

/*
var layout = new LinearLayout(this);
layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
aLabel.Text = "Hello, World!!!";

            var aButton = new Button(this);
aButton.Text = "Say Hello!";

aButton.Click += (sender, e) =>
{ aLabel.Text = "Hello Android!"; };

layout.AddView(aLabel);
layout.AddView(aButton);
SetContentView(layout);
*/