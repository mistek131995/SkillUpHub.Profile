using Riok.Mapperly.Abstractions;

namespace SkillUpHub.Command.Infrastructure.Mappers;

[Mapper]
public partial class ProfileMapper
{
    public partial Command.Contract.Models.Profile MappingToContractModel(Entities.Profile model);
    public partial Entities.Profile MappingToInfrastructureModel(Command.Contract.Models.Profile model);
}