namespace RSApp.Core.Services.Core;

public interface IBaseService<EntityVm> where EntityVm : class {
  Task<IEnumerable<EntityVm>> GetAll();
  Task<EntityVm> GetById(int id);
}
