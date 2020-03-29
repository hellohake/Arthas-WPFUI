using Arthas.Binder;

namespace Arthas.Demo
{
    public class MainWindow_Model : ViewModel
    {
        public string Title
        {
            get => GetValue(() => "Arthas.Demo");
            set => SetValue(value);
        }
    }
}