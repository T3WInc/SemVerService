using System;
using System.Collections.Generic;
using System.Text;

namespace t3winc.version.common.Interfaces
{
    public interface IVersionRepo
    {
        string NewRegistration(string organization);
        bool IsKeyValid(string key);
        int GetVersionId(string key);
        string GetNextVersionNumber(int version, string product, string branch);
    }
}
