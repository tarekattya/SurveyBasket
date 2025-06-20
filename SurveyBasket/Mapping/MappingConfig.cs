﻿

using SurveyBasket.Contracts.Account;

namespace SurveyBasket.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<QuestionRequest, Question>()
                   .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));

            config.NewConfig<RegisterRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email);

            


        }
    }
}
