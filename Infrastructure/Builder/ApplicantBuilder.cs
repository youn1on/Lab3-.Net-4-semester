using System.Reflection.Metadata.Ecma335;
using Infrastructure.Model;

namespace Infrastructure.Builder;

public class ApplicantBuilder
{
    private Applicant _applicant;

    public ApplicantBuilder()
    {
        _applicant = new Applicant();
    }

    public ApplicantBuilder SetName(string name)
    {
        _applicant.Name = name;
        return this;
    }
    
    public ApplicantBuilder SetSurname(string name)
    {
        _applicant.Surname = name;
        return this;
    }
    
    public ApplicantBuilder SetPatronymic(string patronymic)
    {
        _applicant.Patronymic = patronymic;
        return this;
    }
    
    public ApplicantBuilder SetBirthDate(DateOnly birthdate)
    {
        _applicant.BirthDate = birthdate;
        return this;
    }
    
    public ApplicantBuilder SetEducationLevel(EducationLevel level)
    {
        _applicant.EducationLevel = level;
        return this;
    }
    
    public ApplicantBuilder AddRate(Subject subject, float rate)
    {
        if (rate is >= 0 and <= 200)
            _applicant.Rates.Add((subject, rate));
        return this;
    }
    
    public ApplicantBuilder AddApplication(ushort speciality, int institutionId, EducationForm form)
    {
        if (speciality is >= 11 and <= 275)
            _applicant.Applications.Add((speciality, institutionId, form));
        return this;
    }

    public Applicant Build() => _applicant;
}