using AutoMapper;
using SecretaryBot.Dal.Models;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Dal
{
    public class DalMapperConfigurator
    {
        private readonly IMapperConfigurationExpression _expression;

        public DalMapperConfigurator(IMapperConfigurationExpression expression)
        {
            MappingUser(expression);
            MappingCategory(expression);
            MappingPurchase(expression);
            _expression = expression;
        }

        public IMapperConfigurationExpression AddConfiguration() => _expression;

        private static void MappingCategory(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<DalCategory, Category>()
                .ReverseMap();
        }

        private static void MappingUser(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<DalUser, User>()
                .ReverseMap();
        }

        private static void MappingPurchase(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<DalPurchase, Purchase>()
                .ReverseMap();
        }
    }
}
