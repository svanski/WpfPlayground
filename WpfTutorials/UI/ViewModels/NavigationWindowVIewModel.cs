using System.Windows.Input;
using WpfTutorials.Domain.Abstractions;
using WpfTutorials.Domain.Command;

namespace WpfTutorials.UI.ViewModels
{
    public class NavigationWindowVIewModel : BaseViewModel
    {
        private IColorViewModel colorViewModel;

        public NavigationWindowVIewModel()
        {
            ColorViewModel = new GreenViewModel();
            RotateCommand = new CycleWindowCommand(this);
        }

        public IColorViewModel ColorViewModel
        {
            get { return colorViewModel; }
            set
            {
                colorViewModel = value;
                OnPropertyChnaged();
            }
        }

        public ICommand RotateCommand { get; private set; }
    }
}