using AppFileDbEntity = WebApi.Database.Models.AppFile;
using AppFileModel = WebApi.Models.AppFile;
using AppModel = WebApi.Models.App;

namespace WebApi.Models.Factories {
    public static class AppFileFactory {
        public static AppFileDbEntity Create(AppFileModel model) {
            if (model == default) return default;
            return new AppFileDbEntity(model.Id, model.Name, model.MimeType, model.Data);
        }

        public static AppFileModel Create(AppFileDbEntity entity) {
            if (entity == default) return default;
            return new AppFileModel(entity.Id, entity.Name, entity.MimeType, entity.Data);
        }
    }
}