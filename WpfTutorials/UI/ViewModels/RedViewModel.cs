using WpfTutorials.Domain.Abstractions;

namespace WpfTutorials.UI.ViewModels
{
    public class RedViewModel : BaseViewModel, IColorViewModel
    {
        private string message;

        public RedViewModel()
        {
            Message = "Red View Model";
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChnaged();
            }
        }
    }
}