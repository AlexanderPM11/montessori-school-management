using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Helpers
{
    public interface IInitDataHelper
    {
        Task<(string action, string controller)> ValidatedInitData();
    }
}
