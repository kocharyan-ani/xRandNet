using AppDbEntity = WebApi.Database.Models.App;
using AppModel = WebApi.Models.App;
using AppFileModel = WebApi.Models.AppFile;
using AppFileDbEntity = WebApi.Database.Models.AppFile;

namespace WebApi.Models.Factories {
    public static class AppFactory {
        public static AppDbEntity Create(AppModel model) {
            if (model == default) return default;
            return new AppDbEntity(model.Id, model.Version, model.ReleaseDate, model.ReleaseNotes);
        }

        public static AppModel Create(AppDbEntity entity) {
            if (entity == default) return default;
            AppFileModel appFileModel = AppFileFactory.Create(entity.File);
            return new AppModel(entity.Id, entity.Version, appFileModel, entity.Version, entity.ReleaseDate);
        }
    }
}