using Riok.Mapperly.Abstractions;

namespace SkillUpHub.Profile.Infrastructure.Mappers;

[Mapper]
public partial class ProfileMapper
{
    public partial Contract.Models.Profile MappingToContractModel(Entities.Profile model);
    public partial Entities.Profile MappingToInfrastructureModel(Contract.Models.Profile model);
}