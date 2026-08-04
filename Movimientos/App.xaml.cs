using Microsoft.Extensions.DependencyInjection;

namespace Movimientos
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Current!.UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}