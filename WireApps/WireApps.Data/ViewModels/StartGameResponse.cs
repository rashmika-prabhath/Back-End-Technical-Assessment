using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireApps.Data.ViewModels;

public class StartGameResponse
{
    public string Message { get; set; }
    public string[][] Board { get; set; }
}
