using WpfTutorials.Domain.Abstractions;

namespace WpfTutorials.UI.ViewModels
{
    public class GreenViewModel : BaseViewModel, IColorViewModel
    {
        private string message;

        public GreenViewModel()
        {
            Message = "Green View Model";
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