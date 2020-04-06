using AppDbEntity = Api.Database.Models.App;
using AppModel = Api.Models.App;
using AppFileModel = Api.Models.AppFile;
using AppFileDbEntity = Api.Database.Models.AppFile;

namespace Api.Models.Factories {
    public static class AppFactory {
        public static AppDbEntity Create(AppModel model) {
            if (model == default) return default;
            AppFileDbEntity appFileDbEntity = null;
            if (model.File != null) {
                appFileDbEntity =
                    AppFileFactory.Create(new AppFileModel(model.File.Name, model.File.MimeType, model.File.Data));
            }

            AppDbEntity appDbEntity = new AppDbEntity(model.Id, model.Version, model.ReleaseDate, model.ReleaseNotes);
            appDbEntity.File = appFileDbEntity;
            return appDbEntity;
        }

        public static AppModel Create(AppDbEntity entity) {
            if (entity == default) return default;
            AppFileModel appFileModel = AppFileFactory.Create(entity.File);
            return new AppModel(entity.Id, entity.Version, appFileModel, entity.ReleaseNotes, entity.ReleaseDate);
        }
    }
}