using BugDbEntity = Api.Database.Models.Bug;
using BugModel = Api.Models.Bug;
using AppDbEntity = Api.Database.Models.App;
using AppModel = Api.Models.App;

namespace Api.Models.Factories {
    public static class BugFactory {
        public static Database.Models.Bug Create(Bug model) {
            if (model == default) return default;
            Database.Models.App appDbEntity = AppFactory.Create(model.App);
            return new Database.Models.Bug(model.Id, model.Summary, model.Description, model.Status, model.ReportDate,
                model.Reporter, appDbEntity.Id, appDbEntity);
        }

        public static Bug Create(Database.Models.Bug entity) {
            if (entity == default) return default;
            App appModel = AppFactory.Create(entity.App);
            return new Bug(entity.Id, entity.Summary, entity.Description, entity.Reporter, entity.Status,
                entity.ReportDate);
        }
    }
}