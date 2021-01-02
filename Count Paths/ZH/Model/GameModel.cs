using System;
using System.Collections.Generic;
using System.Text;

namespace ZH.Model
{
    public struct KeyValue
    {  
        public int i;
        public int j;
        public KeyValue(int _i, int _j)
        {
            i = _i;
            j = _j;
        }
    };

    public class GameModel
    {
        private ModelTable _table;
        public Boolean isCounted = false;
        public ModelTable Table { get { return _table; } }

        private void clearNotBlack()
        {
            for(int i = 0; i < _table.Size; i++) 
                for(int j = 0; j < _table.Size; j++)
                {
                    if (_table.GetValue(i, j) != -1) _table.SetValue(i, j, 0);
                }
            
        }
        public void count_paths(int rows, int cols)
        {

            if (isCounted) clearNotBlack();
            Queue<KeyValue> values = new Queue<KeyValue>();
            values.Enqueue(new KeyValue(0,0));

            while (values.Count != 0)
            {
                KeyValue tmp = values.Dequeue();
                if(_table.GetValue(tmp.i, tmp.j) != -1)
                    _table.SetValue(tmp.i, tmp.j, _table.GetValue(tmp.i, tmp.j) + 1);
                if (tmp.i + 1 < rows && _table.GetValue(tmp.i,tmp.j) != -1)
                {
                    values.Enqueue(new KeyValue( tmp.i + 1, tmp.j));
                }

                if (tmp.j + 1 < cols && _table.GetValue(tmp.i, tmp.j) != -1)
                {
                    values.Enqueue(new KeyValue(tmp.i, tmp.j + 1));
                }
                
            }

            isCounted = true;
            if (IsGameOver) OnGameOver(false);
        }

        public Boolean IsGameOver { get { return (_table.GetValue(_table.Size - 1, _table.Size - 1) == 0); } }
        public event EventHandler<ModelEventArgs> GameOver;
        public event EventHandler<ModelEventArgs> GameCreated;

        public GameModel()
        {
            _table = new ModelTable();
            
        }

        public void NewGame(int size)
        {
            _table = new ModelTable(size);
            isCounted = false;
            GenerateFields();
            OnGameCreated();
        }

        

        public void Step(Int32 x, Int32 y) // ha tikre mozog a pálya előre akkor hasznos és akkor paraméter se kell
        {   
            if (x == 0 && y == 0) return;
            _table.SetValue(x, y, -1);
            
        }

        private void GenerateFields()
        {
            for(Int32 i = 0; i < _table.Size; i++)
            {
                for (Int32 j = 0; j < _table.Size; j++)
                {
                    _table.SetValue(i, j, 0);
                }
            }
        }

       
        private void OnGameOver(Boolean isWon)
        {
            if (GameOver != null)
                GameOver(this, new ModelEventArgs(isWon));
        }
        private void OnGameCreated()
        {
            if (GameCreated != null)
                GameCreated(this, new ModelEventArgs(false));
        }
    }
}
