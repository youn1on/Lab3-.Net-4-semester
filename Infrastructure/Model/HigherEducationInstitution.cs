using Infrastructure.Interfaces;

namespace Infrastructure.Model;

public class HigherEducationInstitution : IDbModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public HigherEducationInstitution(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public HigherEducationInstitution(string csvLine)
    {
        string[] data = csvLine.Split(';');
        if (data.Length != 2 || !int.TryParse(data[0], out int id))
            throw new ArgumentException("Incorrect institution`s csv");
        Id = id;
        Name = data[1];
    }
    
    public string ToCsvString()
    {
        return Id.ToString() + ';' + Name + '\n';
    }
}