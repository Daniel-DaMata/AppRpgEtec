using AppRpgEtec.Helpers.Message;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Views.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;
        public ICommand RegistrarCommand { get; set; }
        public ICommand AutenticarCommand { get; set; }
        public ICommand DirecionarCadastroCommand { get; set; }

        //ctor + TAB + TAB: atalho para criar o construtor
        public UsuarioViewModel()
        {
            uService = new UsuarioService();
            InicializarCommands();
        }


        public async Task DirecionarParaCadastro()//metodo para exibição da view do Cadastro
        {
            try
            {
                await Application.Current.MainPage
                    .Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public void InicializarCommands()
        {
            RegistrarCommand = new Command(async () => await RegistrarUsuario());
            AutenticarCommand = new Command(async () => await AutenticarUsuario());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
        }

        #region AttributosPropiedades

        private string login = string.Empty;
        public string Login
        { 
            get { return login; } 
            set 
            {
                login = value;
                OnPropertyChanged();
            } 
        }
        private string senha = string.Empty;
        public string Senha
        { 
            get { return senha; }
        set
            {
                senha = value;
                OnPropertyChanged();
            }
        }
        
        public async Task RegistrarUsuario()//Método para registrar um usuário
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = senha;

                Usuario uRegistrado = await uService.PostRegistrarUsuarioAsync(u);

                if (uRegistrado.Id != 0) 
                {
                    string mensagem = $"Usuário Id {uRegistrado.Id} registrado com sucesso.";
                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                    await Application.Current.MainPage
                        .Navigation.PopAsync(); //Remove a pagina da pilha de visualização
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task AutenticarUsuario()//Metodo para registrar um usuario
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = senha;

                Usuario uAutenticado = await uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token)) 
                {
                    string mensagem = $"Bem-vindo(a) {uAutenticado.Username}.";

                    //Guardando dados do usuario para uso futuro
                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    Models.Email email = new Models.Email();
                    email.Remetente = "daniel.soares.damata@gmail.com";
                    email.RemetentePassword = "ndabmpjkwujdzqfn";
                    email.Destinatario = "luizfernando987@gmail.com";
                    email.DominioPrimario = "smtp.gmail.com";
                    email.PortaPrimaria = 587;
                    email.Assunto = "Notificação de acesso";
                    email.Mensagem = $"Usuário {u.Username} acessou o aplicativo" + 
                        $"em {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                    EmailHelpers emailHelpers= new EmailHelpers();
                    await emailHelpers.EviarEmail(email);

                    await Application.Current.MainPage
                        .DisplayAlert("Informação", mensagem, "Ok");

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Dados incorrretos :(", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }




        #endregion
    }
}
