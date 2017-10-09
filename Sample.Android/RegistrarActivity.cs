using Android.App;
using Android.OS;
using Android.Widget;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.IO;

namespace Sample.Android
{
    [Activity(Label = "RegistrarActivity")]
    public class RegistrarActivity : Activity
    {
        EditText txtNovoUsuario;
        EditText txtSenhaNovoUsuario;
        Button btnCriarNovoUsuario;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NovoUsuario);

            btnCriarNovoUsuario = FindViewById<Button>(Resource.Id.btnRegistrar);
            txtNovoUsuario = FindViewById<EditText>(Resource.Id.txtNovoUsuario);
            txtSenhaNovoUsuario = FindViewById<EditText>(Resource.Id.txtSenhaNovoUsuario);

            btnCriarNovoUsuario.Click += BtnCriarNovoUsuario_Click;
        }
        
        private void BtnCriarNovoUsuario_Click(object sender, System.EventArgs e)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Usuario2.db3"));
                //connection.CreateTableAsync<Login>();

                Login tblogin = new Login();
                tblogin.usuario = txtNovoUsuario.Text;
                tblogin.senha = txtSenhaNovoUsuario.Text;
                
                connection.InsertAsync(tblogin);

                Toast.MakeText(this, "Registro incluído com sucesso...,", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }
        
    }
}