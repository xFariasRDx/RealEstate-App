using AutoMapper;
using RSApp.Core.Application.Features.Upgrades.Commands.Update;
using RSApp.Core.Domain.Entities;
using RSApp.Core.Services.Dtos.Upgrade;
using RSApp.Core.Services.ViewModels;
using RSApp.Core.Services.ViewModels.SaveVm;

namespace RSApp.Core.Application.Mapping;

public class UpgradeProfile : Profile {
  public UpgradeProfile() {
    CreateMap<Upgrade, UpgradeVm>()
    .ForMember(vm => vm.HasError, opt => opt.Ignore())
    .ForMember(vm => vm.ErrorMessage, opt => opt.Ignore())
    .ReverseMap()
    .ForMember(ent => ent.PropertyUpgrades, opt => opt.Ignore());

    CreateMap<Upgrade, SaveUpgradeVm>()
    .ForMember(vm => vm.HasError, opt => opt.Ignore())
    .ForMember(vm => vm.ErrorMessage, opt => opt.Ignore())
    .ReverseMap()
    .ForMember(ent => ent.PropertyUpgrades, opt => opt.Ignore());

    CreateMap<Upgrade, UpgradeDto>()
    .ReverseMap()
    .ForMember(ent => ent.PropertyUpgrades, opt => opt.Ignore());

    CreateMap<Upgrade, SaveUpgradeDto>()
    .ReverseMap()
    .ForMember(ent => ent.PropertyUpgrades, opt => opt.Ignore());

    CreateMap<Upgrade, UpdateUpgradeResponse>()
    .ReverseMap()
    .ForMember(ent => ent.CreatedAt, opt => opt.Ignore())
    .ForMember(ent => ent.LastModifiedAt, opt => opt.Ignore())
    .ForMember(ent => ent.PropertyUpgrades, opt => opt.Ignore());

  }
}
