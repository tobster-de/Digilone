using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public interface ICoordinates
    {
        int Row
        {
            get;
        }

        int Diagonal
        {
            get;
        }

        bool Valid
        {
            get;
        }

        void Modify(Direction direction);
    }
}
