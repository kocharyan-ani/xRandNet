using FileDbEntity = Api.Database.Models.File;
using AppFileDbEntity = Api.Database.Models.AppFile;
using PublicationFileDbEntity = Api.Database.Models.PublicationFile;
using FileModel = Api.Models.File;
using AppFileModel = Api.Models.AppFile;
using PublicationFileModel = Api.Models.PublicationFile;

namespace Api.Models.Factories {
    public static class FileFactory {
        public static FileDbEntity Create(FileModel model) {
            if (model == default) return default;
            if (model is AppFileModel) {
                return new AppFileDbEntity(model.Id, model.Name, model.MimeType, model.Data);
            }
            if (model is PublicationFileModel) {
                return new PublicationFileDbEntity(model.Id, model.Name, model.MimeType, model.Data);
            }

            return new FileDbEntity(model.Id, model.Name, model.MimeType, model.Data);
        }

        public static FileModel Create(FileDbEntity entity) {
            if (entity == default) return default;
            if (entity is AppFileDbEntity) {
                return new AppFileModel(entity.Id, entity.Name, entity.MimeType, entity.Data);
            }
            if (entity is PublicationFileDbEntity) {
                return new PublicationFileModel(entity.Id, entity.Name, entity.MimeType, entity.Data);
            }

            return new FileModel(entity.Id, entity.Name, entity.MimeType, entity.Data);
        }
    }
}