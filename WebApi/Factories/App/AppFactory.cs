using AppDbEntity = WebApi.Database.Models.App;
using AppModel = WebApi.Models.App;
using AppFileModel = WebApi.Models.AppFile;

namespace WebApi.Models.Factories {
    public static class AppFactory {
        public static AppDbEntity Create(AppModel model) {
            if (model == default) return default;

            return null;//new AppDbEntity(model.Id, model.Version, model.File.Id, model.ReleaseDate, model.ReleaseNotes,
                //model.File); // TODO get all bug entities related to this app model and pass to constructor
        }

        public static AppModel Create(AppDbEntity entity) {
            if (entity == default) return default;
            AppFileModel appFileModel = AppFileFactory.Create(entity.File);
            return new AppModel(entity.Id, entity.Version, appFileModel, entity.Version, entity.ReleaseDate);
        }
    }
}