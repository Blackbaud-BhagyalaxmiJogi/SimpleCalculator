using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Logging
{
    public interface IHistoryReader
    {
        bool HasHistory();
        string[] ReadAll();
    }
}
