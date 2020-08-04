using System.Windows.Controls;

namespace AuthenticationModule.Views
{
    /// <summary>
    /// Interaction logic for SignUp
    /// </summary>
    public partial class SignUp : UserControl
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void ConfirmPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            //if(Password.Password != ConfirmPassword.Password)
            //{
            //    Validation.MarkInvalid(ConfirmPassword,new ValidationError(new ExceptionValidationRule))
            //}
        }
    }
}
