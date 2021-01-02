using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ZH.Model;
using ZH.ViewModel;

namespace ZH
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameModel _model;
        private GameViewModel _viewModel;
        private MainWindow _view;


        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new GameModel();
            _model.GameOver += new EventHandler<ModelEventArgs>(Model_GameOver);
            _model.NewGame(6);

            // nézemodell létrehozása
            _viewModel = new GameViewModel(_model);
            _viewModel.NewGame += ViewModel_NewGame;
            


            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

           
        }

        private void ViewModel_ExitGame(object sender, EventArgs e)
        {
            _view.Close();
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Game", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást
            }
        }
        private void ViewModel_NewGame(object sender, int e)
        {
            _model.NewGame(e);

        }

        private void Model_GameOver(object sender, ModelEventArgs e)
        {
            MessageBox.Show("There are no, available path");
            _model.NewGame(_viewModel.GridSize);
        }

       
    }
}
