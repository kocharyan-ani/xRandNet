using FileDbEntity = Api.Database.Models.File;
using AppFileDbEntity = Api.Database.Models.AppFile;
using FileModel = Api.Models.File;
using AppFileModel = Api.Models.AppFile;

namespace Api.Models.Factories {
    public static class FileFactory {
        public static FileDbEntity Create(FileModel model) {
            if (model == default) return default;
            if (model is AppFileModel) {
                return new AppFileDbEntity(model.Id, model.Name, model.MimeType, model.Data);
            }

            return new FileDbEntity(model.Id, model.Name, model.MimeType, model.Data);
        }

        public static FileModel Create(FileDbEntity entity) {
            if (entity == default) return default;
            if (entity is AppFileDbEntity) {
                return new AppFileModel(entity.Id, entity.Name, entity.MimeType, entity.Data);
            }

            return new FileModel(entity.Id, entity.Name, entity.MimeType, entity.Data);
        }
    }
}