using Infrastructure.DbContext;
using Infrastructure.Model;

namespace ConsoleApp;

public static class Program
{
    public static void Main()
    {
        try
        {
            UserInterface.MainLoop();
        }
        finally
        {
            DbContext.Save();
        }
    }
}