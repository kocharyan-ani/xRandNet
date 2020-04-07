using AppFileDbEntity = Api.Database.Models.AppFile;
using AppFileModel = Api.Models.AppFile;
using AppModel = Api.Models.App;

namespace Api.Models.Factories {
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