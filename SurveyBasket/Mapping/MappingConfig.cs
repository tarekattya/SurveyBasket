

namespace SurveyBasket.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<Student, StudenResponse>().Map(dest => dest.FullName, src => $"{src.FirstName} {src.MiddleName} {src.LastName}")
                .Map(dest => dest.Age, src => DateTime.Now.Year - src.DateOfBirth!.Value.Year, SrcCond => SrcCond.DateOfBirth.HasValue)
                .Ignore(dest => dest.DepartmentName);


        }
    }
}
