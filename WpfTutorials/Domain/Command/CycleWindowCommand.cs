using System;
using System.Windows.Input;
using WpfTutorials.Domain.Abstractions;
using WpfTutorials.UI.ViewModels;

namespace WpfTutorials.Domain.Command
{
    public class CycleWindowCommand : ICommand
    {
        private readonly NavigationWindowVIewModel colorViewModel;


        public CycleWindowCommand(NavigationWindowVIewModel colorViewModel)
        {
            this.colorViewModel = colorViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (colorViewModel.ColorViewModel is BlueViewModel)
                colorViewModel.ColorViewModel = new GreenViewModel();
            else if (colorViewModel.ColorViewModel is GreenViewModel)
                colorViewModel.ColorViewModel = new RedViewModel();
            else
                colorViewModel.ColorViewModel = new BlueViewModel();
        }
    }
}