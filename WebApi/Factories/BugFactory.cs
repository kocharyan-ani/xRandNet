using BugDbEntity = WebApi.Database.Models.Bug;
using BugModel = WebApi.Models.Bug;
using AppDbEntity = WebApi.Database.Models.App;
using AppModel = WebApi.Models.App;

namespace WebApi.Models.Factories {
    public static class BugFactory {
        public static BugDbEntity Create(BugModel model) {
            if (model == default) return default;
            AppDbEntity appDbEntity = AppFactory.Create(model.App);
            return new BugDbEntity(model.Id, model.Summary, model.Description, model.Status, model.ReportDate,
                model.Reporter, appDbEntity.Id, appDbEntity);
        }

        public static BugModel Create(BugDbEntity entity) {
            if (entity == default) return default;
            AppModel appModel = AppFactory.Create(entity.App);
            return new BugModel(entity.Id, entity.Summary, entity.Description, entity.Reporter, entity.Status,
                entity.ReportDate);
        }
    }
}