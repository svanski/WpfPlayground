using WpfTutorials.Domain.Abstractions;

namespace WpfTutorials.UI.ViewModels
{
    public class BlueViewModel : BaseViewModel, IColorViewModel
    {
        private string message;

        public BlueViewModel()
        {
            Message = "Blue View Model";
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