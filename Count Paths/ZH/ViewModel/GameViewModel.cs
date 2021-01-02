using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using ZH.Model;

namespace ZH.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        private GameModel _model;
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand NewGameCommand4 { get; private set; }
        public DelegateCommand NewGameCommand6 { get; private set; }
        public DelegateCommand NewGameCommand8 { get; private set; }
        public DelegateCommand CountPaths { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public ObservableCollection<ModelField> Fields { get; set; }
       
        public Int32 GridSize { get; private set; }

        public event EventHandler<int> NewGame;

        public GameViewModel(GameModel model)
        {
            GridSize = 6;
            // játék csatlakoztatása
            _model = model;
            
            _model.GameOver += new EventHandler<ModelEventArgs>(Model_GameOver);
            _model.GameCreated += new EventHandler<ModelEventArgs>(Model_GameCreated);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame(GridSize));
            NewGameCommand4 = new DelegateCommand(param => OnNewGame(4));
            NewGameCommand6 = new DelegateCommand(param => OnNewGame(6));
            NewGameCommand8 = new DelegateCommand(param => OnNewGame(8));
            CountPaths = new DelegateCommand(param => OnCountPaths());


            // játéktábla létrehozása
            Fields = new ObservableCollection<ModelField>();
            for (Int32 i = 0; i < _model.Table.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    Fields.Add(new ModelField
                    {
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Number = i * _model.Table.Size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        
                    });
                }
            }
            RefreshTable();
        }

        private void OnCountPaths()
        {
            _model.count_paths(_model.Table.Size, _model.Table.Size);
            RefreshTable();
        }

        private void RefreshTable()
        {
            foreach (ModelField field in Fields) // inicializálni kell a mezőket is
            {
                field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
                field.Type = _model.Table[field.X, field.Y];
            }

            OnPropertyChanged("GameTime");
        }

        private void StepGame(int index)
        {
            ModelField field = Fields[index];

            _model.Step(field.X, field.Y);
            if(_model.isCounted)
            {
                _model.count_paths(_model.Table.Size, _model.Table.Size);
                Fields.Clear();
                for (Int32 i = 0; i < _model.Table.Size; i++) // inicializáljuk a mezőket
                {
                    for (Int32 j = 0; j < _model.Table.Size; j++)
                    {
                        Fields.Add(new ModelField
                        {
                            Text = String.Empty,
                            X = i,
                            Y = j,
                            Number = i * _model.Table.Size + j,
                            StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        });
                    }
                }
                RefreshTable();
                
            }

            field.Text = _model.Table[field.X, field.Y] > 0 ? _model.Table[field.X, field.Y].ToString() : String.Empty; // visszaírjuk a szöveget
            OnPropertyChanged("GameStepCount"); // jelezzük a lépésszám változást
            field.Type = _model.Table[field.X, field.Y];
            field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
        }


       
        private void OnNewGame(int size)
        {
            Debug.Write("ramkattolt" + size +"\n");
            Fields.Clear();
            GridSize = size;
            OnPropertyChanged("GridSize");
            if (NewGame != null)
                NewGame(this, size);
            for (Int32 i = 0; i < size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < size; j++)
                {
                    Fields.Add(new ModelField
                    {
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Number = i * size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            RefreshTable();

        }

        private void Model_GameCreated(object sender, ModelEventArgs e)
        {
            RefreshTable();
        }

        private void Model_GameOver(object sender, ModelEventArgs e)
        {
            Debug.Write("vege");
        }

        private void Model_GameAdvanced(object sender, ModelEventArgs e)
        {
            OnPropertyChanged("GameTime");
        }
      
    }
}
