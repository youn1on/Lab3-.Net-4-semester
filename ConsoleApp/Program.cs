using ConsoleApp;
using Infrastructure.DbContext;

public static class Program
{
    public static void Main()
    {
        DbContext database = DbContext.InitializeDb();
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