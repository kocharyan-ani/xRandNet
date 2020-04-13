using AppDbEntity = Api.Database.Models.App;
using AppModel = Api.Models.App;
using FileModel = Api.Models.File;

namespace Api.Models.Factories {
    public static class AppFactory {
        public static AppDbEntity Create(AppModel model) {
            if (model == default) return default;
            AppDbEntity appDbEntity = new AppDbEntity(model.Id, model.Version, model.ReleaseDate, model.ReleaseNotes);
            if (model.File != null) {
                appDbEntity.File = (Database.Models.AppFile) FileFactory.Create(new AppFile(model.File.Name,
                    model.File.MimeType,
                    model.File.Data));
            }

            return appDbEntity;
        }

        public static AppModel Create(AppDbEntity entity) {
            if (entity == default) return default;
            File fileModel = FileFactory.Create(entity.File);
            return new AppModel(entity.Id, entity.Version, fileModel, entity.ReleaseNotes, entity.ReleaseDate);
        }
    }
}