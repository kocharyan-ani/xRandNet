using AppFileDbEntity = Api.Database.Models.AppFile;
using AppFileModel = Api.Models.AppFile;
using AppModel = Api.Models.App;

namespace Api.Models.Factories {
    public static class AppFileFactory {
        public static Database.Models.AppFile Create(AppFile model) {
            if (model == default) return default;
            return new Database.Models.AppFile(model.Id, model.Name, model.MimeType, model.Data);
        }

        public static AppFile Create(Database.Models.AppFile entity) {
            if (entity == default) return default;
            return new AppFile(entity.Id, entity.Name, entity.MimeType, entity.Data);
        }
    }
}