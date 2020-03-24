using AppDbEntity = Api.Database.Models.App;
using AppModel = Api.Models.App;
using AppFileModel = Api.Models.AppFile;
using AppFileDbEntity = Api.Database.Models.AppFile;

namespace Api.Models.Factories {
    public static class AppFactory {
        public static Database.Models.App Create(App model) {
            if (model == default) return default;
            return new Database.Models.App(model.Id, model.Version, model.ReleaseDate, model.ReleaseNotes);
        }

        public static App Create(Database.Models.App entity) {
            if (entity == default) return default;
            AppFile appFileModel = AppFileFactory.Create(entity.File);
            return new App(entity.Id, entity.Version, appFileModel, entity.Version, entity.ReleaseDate);
        }
    }
}