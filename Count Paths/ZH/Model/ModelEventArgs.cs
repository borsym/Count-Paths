using System;
using System.Collections.Generic;
using System.Text;

namespace ZH.Model
{
    public class ModelEventArgs : EventArgs
    {
     
        private Boolean _isWon;
        
        public Boolean IsWon { get { return _isWon; } }
        public ModelEventArgs(Boolean isWon)
        {
            _isWon = isWon;
        }
    }
}
